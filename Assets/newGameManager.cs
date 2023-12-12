using Newtonsoft.Json; // json을 위한 함수 사용
using System.Collections;
using System.Collections.Generic;
using System.IO; //파일을 텍스트 형식으로 저장하게 해준다.
using TMPro;
using UnityEngine;
//using UnityEngine.UIElements;
using UnityEngine.UI;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

[System.Serializable] //직렬화, inspector 창에서 보임
public class Item
{
    //아이템의 형식을 넘겨주기 위한 생성자
    public Item(string _Type, string _Name, string _Explain, string _Number, bool _isUsing, string _Index)
    { Type = _Type; Name = _Name; Explain = _Explain; Number = _Number; isUsing = _isUsing; Index = _Index; }
 
    public string Type, Name, Explain, Number, Index;
    public bool isUsing;
}
public class newGameManager : MonoBehaviour
{
    public TextAsset ItemDatabase;
    public List<Item> AllItemList,MyItemList,CurItemList;
    public string curType = "Character"; //클릭한 버튼의 이름, 처음 선택된 값을 character로 해주자.
    public GameObject[] Slot, UsingImage;
    public Image[] TabImage, ItemImage;
    public Sprite TabIdleSprite, TabSelectSprite; //클릭한 이미지와 기본 이미지
    public Sprite[] ItemSprite;
    public GameObject ExplainPanel; //설명창 가져오기
    public RectTransform CanvasRect;
    public TMP_InputField ItemNameInput, ItemNumberInput;
    IEnumerator PointerCoroutine;
    RectTransform ExplainRect;
    void Start()
    {
        //전체 아이템 리스트 불러오기
        //아이템마다 나누기(스페이스 기준)
        string[] line = ItemDatabase.text.Substring(0, ItemDatabase.text.Length - 1).Split('\n');
        for(int i = 0; i < line.Length; i++)
        {
            //아이템 안의 속성마다 나누기(탭 기준)
            string[] row = line[i].Split('\t');
            //아이템의 생성자로부터 직접 만든 데이터 형식을 넘겨받고, 데이터로부터 받은 값을 형식별로 저장
            //bool값의 경우, String으로 받기때문에 원하는 값이 나오도록 유도해준다.
            AllItemList.Add(new Item(row[0], row[1], row[2], row[3], row[4] == "TRUE", row[5]));
        }

        Load();

        ExplainRect = ExplainPanel.GetComponent<RectTransform>();
    }

    private void Update()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(CanvasRect, Input.mousePosition, Camera.main, out Vector2 anchoredPos);
        ExplainRect.anchoredPosition = anchoredPos + new Vector2(0,300);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            SceneManager.LoadScene("Main");
        }
    }

    public void GetItemClick()
    {
        //inputfield의 텍스트와 내가 가지고 있는 아이템 리스트의 아이템 x중에 이름이 일치하는 것이 있다면
        // 그 아이템 x를 curItem으로 저장해준다.
        Item curItem = MyItemList.Find(x => x.Name == ItemNameInput.text);
       if(curItem != null) 
        {
           //아이템이 비어있지 않다면 스트링 형식으로 저장되어 있는 아이템의 숫자를 int형태로 파씽해주고
           //inputfield에 입력한 아이템 개수만큼 더해준다.
          curItem.Number = (int.Parse(curItem.Number) + int.Parse(ItemNumberInput.text)).ToString();
        }
        else
        {
            //얻을 아이템이 없는 경우
            Item curAllItem = AllItemList.Find(x => x.Name == ItemNameInput.text);
            if(curAllItem != null)
            {
                curAllItem.Number = ItemNumberInput.text;
                MyItemList.Add(curAllItem);
            }
                
        }
        MyItemList.Sort((p1, p2) => p1.Index.CompareTo(p2.Index));
        Save();
    }

    public void RemoveItemClick()
    {
        Item curItem = MyItemList.Find(x=>x.Name == ItemNameInput.text);
        if(curItem != null)
        {
           int curNumber = int.Parse(curItem.Number) - int.Parse(ItemNumberInput.text);

            if(curNumber <= 0) MyItemList.Remove(curItem);
            else curItem.Number = curNumber.ToString();
        }
        MyItemList.Sort((p1, p2) => p1.Index.CompareTo(p2.Index));
        Save();
    }
    public void SlotClick(int slotNum)
    {
        Item CurItem =  CurItemList[slotNum];
        //사용중인 아이템을 넣어준다.
        Item UsingItem = CurItemList.Find(x => x.isUsing == true);

        if(curType == "Character")
        {
            //만약 사용중인 아이템이 없다면 isusing을 거짓으로 만들어주자.
            if(UsingItem != null) UsingItem.isUsing = false;
            CurItem.isUsing = true;
        }
        else //curType == "Balloon"
        {
            //현재 아이템의 isusing 값을 바꿔준다. 즉, 선택을 안하는 옵션으로 만들어준다.
            CurItem.isUsing =!CurItem.isUsing;
            //사용중인 아이템을 false로 만들어준다.
            if(UsingItem != null) UsingItem.isUsing = false;
        }

        Save();
    }

    public void TabClick(string tabName)
    {
        //현재 아이템 리스트에 클릭한 타입만 추가
        curType = tabName;
        //Findall: 조건을 만족하는 값을 모두 리스트 형태로 가져온다.
        //MyitemList안의 x의 type(캐릭터나 풍선)이 내가 클릭한 버튼의 이름(캐릭터나 풍선)과 같다면 다 불러와서 리스트로 만들어 담자.
        CurItemList = MyItemList.FindAll(x => x.Type == tabName);

        
        for(int i = 0; i< Slot.Length;i++)
        {
            //슬롯과 텍스트 보이기
            //지금 보여지는 슬롯이 현재 아이템 리스트에 존재하는가
            bool isExist = i < CurItemList.Count;
            //현재 내가 가진 아이템 데이터에서 받아온 개수만큼 슬롯을 활성화해주자. 
            Slot[i].SetActive(isExist);
            //마찬가지로 내가 가진 데이터 개수만큼 슬롯의 자식인 텍스트에 아이템 이름을 넣어준다.
            Slot[i].GetComponentInChildren<TextMeshProUGUI>().text = isExist ? CurItemList[i].Name: "";

            if(isExist)
            {
                //존재하는 슬롯에 대해 전체 아이템중 내가 가진 아이템에 해당하는 인덱스 번호와 일치하는 스프라이트를
                //스크롤 뷰에 있는 아이템 이미지에 넣어준다.
                ItemImage[i].sprite = ItemSprite[AllItemList.FindIndex(x => x.Name == CurItemList[i].Name)];
                UsingImage[i].SetActive(CurItemList[i].isUsing);
            }
        }

        //클릭한 버튼에 따라 이미지가 다르게 나오도록 하자.
        int tabNum = 0;
        switch (tabName)
        {
            case "Character": tabNum = 0 ; break;
            case "Balloon": tabNum = 1; break;
        }
        //선택한 이미지의 번호 tabNum과 배치된 이미지들을 비교해서 같으면 선택된 이미지가 나오도록 해주자.
        for(int i = 0; i < TabImage.Length;i++)
        {
            TabImage[i].sprite = i == tabNum ? TabSelectSprite : TabIdleSprite;
        }
    }

    public void PointerEnter(int slotNum)
    {
        PointerCoroutine = PointerEnterDelay(slotNum);
        StartCoroutine(PointerCoroutine);

        ExplainPanel.GetComponentInChildren<TextMeshProUGUI>().text = CurItemList[slotNum].Name;
        ExplainPanel.transform.GetChild(2).GetComponent<Image>().sprite = Slot[slotNum].transform.GetChild(1).GetComponent<Image>().sprite;
        ExplainPanel.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = CurItemList[slotNum].Number + "개";
        ExplainPanel.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = CurItemList[slotNum].Explain;
    }

    IEnumerator PointerEnterDelay(int slotNum)
    {
        yield return new WaitForSeconds(0.5f);
        ExplainPanel.SetActive(true);
    }
    public void PointerExit(int slotNum) 
    {
        StopCoroutine(PointerCoroutine);
        ExplainPanel.SetActive(false);
    }
    void Save()
    {
        //allitemlist의 내용을 JsonConvert를 통해 직렬화(한 줄로 표현됨).
        string jdata = JsonConvert.SerializeObject(MyItemList);
        //using Unity IO가 있어야 한다.
        //Application.dataPath는 프로젝트 파일의 asset까지 경로를 나타낸다.
        File.WriteAllText(Application.dataPath + "/Resources/MyItemText.txt", jdata);

        TabClick(curType);//업데이트를 위해 해준다.
    }

    void Load()
    {
        //파일 경로의 모든 데이터를 불러온다.
        //역직렬화를 통해서 스트링을 리스트로 만들어준다.
        //DeserializeObject<List<형식이 될 클래스>>(jdata);
        string jdata = File.ReadAllText(Application.dataPath + "/Resources/MyItemText.txt");
        MyItemList = JsonConvert.DeserializeObject<List<Item>>(jdata);

        TabClick(curType);
    }


}
