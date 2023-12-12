using Newtonsoft.Json; // json�� ���� �Լ� ���
using System.Collections;
using System.Collections.Generic;
using System.IO; //������ �ؽ�Ʈ �������� �����ϰ� ���ش�.
using TMPro;
using UnityEngine;
//using UnityEngine.UIElements;
using UnityEngine.UI;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

[System.Serializable] //����ȭ, inspector â���� ����
public class Item
{
    //�������� ������ �Ѱ��ֱ� ���� ������
    public Item(string _Type, string _Name, string _Explain, string _Number, bool _isUsing, string _Index)
    { Type = _Type; Name = _Name; Explain = _Explain; Number = _Number; isUsing = _isUsing; Index = _Index; }
 
    public string Type, Name, Explain, Number, Index;
    public bool isUsing;
}
public class newGameManager : MonoBehaviour
{
    public TextAsset ItemDatabase;
    public List<Item> AllItemList,MyItemList,CurItemList;
    public string curType = "Character"; //Ŭ���� ��ư�� �̸�, ó�� ���õ� ���� character�� ������.
    public GameObject[] Slot, UsingImage;
    public Image[] TabImage, ItemImage;
    public Sprite TabIdleSprite, TabSelectSprite; //Ŭ���� �̹����� �⺻ �̹���
    public Sprite[] ItemSprite;
    public GameObject ExplainPanel; //����â ��������
    public RectTransform CanvasRect;
    public TMP_InputField ItemNameInput, ItemNumberInput;
    IEnumerator PointerCoroutine;
    RectTransform ExplainRect;
    void Start()
    {
        //��ü ������ ����Ʈ �ҷ�����
        //�����۸��� ������(�����̽� ����)
        string[] line = ItemDatabase.text.Substring(0, ItemDatabase.text.Length - 1).Split('\n');
        for(int i = 0; i < line.Length; i++)
        {
            //������ ���� �Ӽ����� ������(�� ����)
            string[] row = line[i].Split('\t');
            //�������� �����ڷκ��� ���� ���� ������ ������ �Ѱܹް�, �����ͷκ��� ���� ���� ���ĺ��� ����
            //bool���� ���, String���� �ޱ⶧���� ���ϴ� ���� �������� �������ش�.
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
        //inputfield�� �ؽ�Ʈ�� ���� ������ �ִ� ������ ����Ʈ�� ������ x�߿� �̸��� ��ġ�ϴ� ���� �ִٸ�
        // �� ������ x�� curItem���� �������ش�.
        Item curItem = MyItemList.Find(x => x.Name == ItemNameInput.text);
       if(curItem != null) 
        {
           //�������� ������� �ʴٸ� ��Ʈ�� �������� ����Ǿ� �ִ� �������� ���ڸ� int���·� �ľ����ְ�
           //inputfield�� �Է��� ������ ������ŭ �����ش�.
          curItem.Number = (int.Parse(curItem.Number) + int.Parse(ItemNumberInput.text)).ToString();
        }
        else
        {
            //���� �������� ���� ���
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
        //������� �������� �־��ش�.
        Item UsingItem = CurItemList.Find(x => x.isUsing == true);

        if(curType == "Character")
        {
            //���� ������� �������� ���ٸ� isusing�� �������� ���������.
            if(UsingItem != null) UsingItem.isUsing = false;
            CurItem.isUsing = true;
        }
        else //curType == "Balloon"
        {
            //���� �������� isusing ���� �ٲ��ش�. ��, ������ ���ϴ� �ɼ����� ������ش�.
            CurItem.isUsing =!CurItem.isUsing;
            //������� �������� false�� ������ش�.
            if(UsingItem != null) UsingItem.isUsing = false;
        }

        Save();
    }

    public void TabClick(string tabName)
    {
        //���� ������ ����Ʈ�� Ŭ���� Ÿ�Ը� �߰�
        curType = tabName;
        //Findall: ������ �����ϴ� ���� ��� ����Ʈ ���·� �����´�.
        //MyitemList���� x�� type(ĳ���ͳ� ǳ��)�� ���� Ŭ���� ��ư�� �̸�(ĳ���ͳ� ǳ��)�� ���ٸ� �� �ҷ��ͼ� ����Ʈ�� ����� ����.
        CurItemList = MyItemList.FindAll(x => x.Type == tabName);

        
        for(int i = 0; i< Slot.Length;i++)
        {
            //���԰� �ؽ�Ʈ ���̱�
            //���� �������� ������ ���� ������ ����Ʈ�� �����ϴ°�
            bool isExist = i < CurItemList.Count;
            //���� ���� ���� ������ �����Ϳ��� �޾ƿ� ������ŭ ������ Ȱ��ȭ������. 
            Slot[i].SetActive(isExist);
            //���������� ���� ���� ������ ������ŭ ������ �ڽ��� �ؽ�Ʈ�� ������ �̸��� �־��ش�.
            Slot[i].GetComponentInChildren<TextMeshProUGUI>().text = isExist ? CurItemList[i].Name: "";

            if(isExist)
            {
                //�����ϴ� ���Կ� ���� ��ü �������� ���� ���� �����ۿ� �ش��ϴ� �ε��� ��ȣ�� ��ġ�ϴ� ��������Ʈ��
                //��ũ�� �信 �ִ� ������ �̹����� �־��ش�.
                ItemImage[i].sprite = ItemSprite[AllItemList.FindIndex(x => x.Name == CurItemList[i].Name)];
                UsingImage[i].SetActive(CurItemList[i].isUsing);
            }
        }

        //Ŭ���� ��ư�� ���� �̹����� �ٸ��� �������� ����.
        int tabNum = 0;
        switch (tabName)
        {
            case "Character": tabNum = 0 ; break;
            case "Balloon": tabNum = 1; break;
        }
        //������ �̹����� ��ȣ tabNum�� ��ġ�� �̹������� ���ؼ� ������ ���õ� �̹����� �������� ������.
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
        ExplainPanel.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = CurItemList[slotNum].Number + "��";
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
        //allitemlist�� ������ JsonConvert�� ���� ����ȭ(�� �ٷ� ǥ����).
        string jdata = JsonConvert.SerializeObject(MyItemList);
        //using Unity IO�� �־�� �Ѵ�.
        //Application.dataPath�� ������Ʈ ������ asset���� ��θ� ��Ÿ����.
        File.WriteAllText(Application.dataPath + "/Resources/MyItemText.txt", jdata);

        TabClick(curType);//������Ʈ�� ���� ���ش�.
    }

    void Load()
    {
        //���� ����� ��� �����͸� �ҷ��´�.
        //������ȭ�� ���ؼ� ��Ʈ���� ����Ʈ�� ������ش�.
        //DeserializeObject<List<������ �� Ŭ����>>(jdata);
        string jdata = File.ReadAllText(Application.dataPath + "/Resources/MyItemText.txt");
        MyItemList = JsonConvert.DeserializeObject<List<Item>>(jdata);

        TabClick(curType);
    }


}
