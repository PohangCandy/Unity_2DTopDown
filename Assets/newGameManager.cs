using Newtonsoft.Json; // json을 위한 함수 사용
using System.Collections;
using System.Collections.Generic;
using System.IO; //파일을 텍스트 형식으로 저장하게 해준다.
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable] //직렬화, inspector 창에서 보임
public class Item
{
    //아이템의 형식을 넘겨주기 위한 생성자
    public Item(string _Type, string _Name, string _Explain, string _Number, bool _isUsing)
    { Type = _Type; Name = _Name; Explain = _Explain; Number = _Number; isUsing = _isUsing; }
 
    public string Type, Name, Explain, Number;
    public bool isUsing;
}
public class newGameManager : MonoBehaviour
{
    public TextAsset ItemDatabase;
    public List<Item> AllItemList,MyItemList,CurItemList;
    public string curType = "Character"; //클릭한 버튼의 이름, 처음 선택된 값을 character로 해주자.
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
            AllItemList.Add(new Item(row[0], row[1], row[2], row[3], row[4] == "TRUE"));
        }

        Load();
    }

    public void TabClick(string tabName)
    {
        curType = tabName;
        //Findall: 조건을 만족하는 값을 모두 리스트 형태로 가져온다.
        //MyitemList안의 x의 type(캐릭터나 풍선)이 내가 클릭한 버튼의 이름(캐릭터나 풍선)과 같다면 다 불러와서 리스트로 만들어 담자.
        CurItemList = MyItemList.FindAll(x => x.Type == tabName);
    }
    void Save()
    {
        //allitemlist의 내용을 JsonConvert를 통해 직렬화(한 줄로 표현됨).
        string jdata = JsonConvert.SerializeObject(AllItemList);
        //using Unity IO가 있어야 한다.
        //Application.dataPath는 프로젝트 파일의 asset까지 경로를 나타낸다.
        File.WriteAllText(Application.dataPath + "/Resources/MyItemText.txt", jdata);
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
