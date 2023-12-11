using Newtonsoft.Json; // json�� ���� �Լ� ���
using System.Collections;
using System.Collections.Generic;
using System.IO; //������ �ؽ�Ʈ �������� �����ϰ� ���ش�.
using UnityEngine;
using UnityEngine.UIElements;

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

    public void TabClick(string tabName)
    {
        curType = tabName;
        //Findall: ������ �����ϴ� ���� ��� ����Ʈ ���·� �����´�.
        //MyitemList���� x�� type(ĳ���ͳ� ǳ��)�� ���� Ŭ���� ��ư�� �̸�(ĳ���ͳ� ǳ��)�� ���ٸ� �� �ҷ��ͼ� ����Ʈ�� ����� ����.
        CurItemList = MyItemList.FindAll(x => x.Type == tabName);
    }
    void Save()
    {
        //allitemlist�� ������ JsonConvert�� ���� ����ȭ(�� �ٷ� ǥ����).
        string jdata = JsonConvert.SerializeObject(AllItemList);
        //using Unity IO�� �־�� �Ѵ�.
        //Application.dataPath�� ������Ʈ ������ asset���� ��θ� ��Ÿ����.
        File.WriteAllText(Application.dataPath + "/Resources/MyItemText.txt", jdata);
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
