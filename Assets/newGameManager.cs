using Newtonsoft.Json; // json�� ���� �Լ� ���
using System.Collections;
using System.Collections.Generic;
using System.IO; //������ �ؽ�Ʈ �������� �����ϰ� ���ش�.
using TMPro;
using UnityEngine;
//using UnityEngine.UIElements;
using UnityEngine.UI;

[System.Serializable] //����ȭ, inspector â���� ����
public class Item
{
    //�������� ������ �Ѱ��ֱ� ���� ������
    public Item(string _Type, string _Name, string _Explain, string _Number, bool _isUsing)
    { Type = _Type; Name = _Name; Explain = _Explain; Number = _Number; isUsing = _isUsing; }
 
    public string Type, Name, Explain, Number;
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
            AllItemList.Add(new Item(row[0], row[1], row[2], row[3], row[4] == "TRUE"));
        }

        Load();
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
        curType = tabName;
        //Findall: ������ �����ϴ� ���� ��� ����Ʈ ���·� �����´�.
        //MyitemList���� x�� type(ĳ���ͳ� ǳ��)�� ���� Ŭ���� ��ư�� �̸�(ĳ���ͳ� ǳ��)�� ���ٸ� �� �ҷ��ͼ� ����Ʈ�� ����� ����.
        CurItemList = MyItemList.FindAll(x => x.Type == tabName);

        //���԰� �ؽ�Ʈ ���̱�
        for(int i = 0; i< Slot.Length;i++)
        {
            //���� ���� ���� ������ �����Ϳ��� �޾ƿ� ������ŭ ������ Ȱ��ȭ������. 
            Slot[i].SetActive(i < CurItemList.Count);
            //���������� ���� ���� ������ ������ŭ ������ �ڽ��� �ؽ�Ʈ�� ������ �̸��� �־��ش�.
            Slot[i].GetComponentInChildren<TextMeshProUGUI>().text = i < CurItemList.Count ? CurItemList[i].Name + "/" + CurItemList[i].isUsing : "";
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
