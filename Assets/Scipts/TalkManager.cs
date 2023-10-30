using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData;
    // Start is called before the first frame update
    void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        GenerateData();
    }

    // Update is called once per frame
    void GenerateData()
    {
        talkData.Add(1000, new string[] { "�ȳ�?", "�� ���� ó�� �Ա���?" });

        talkData.Add(2000, new string[] { "����","�� ȣ���� ���� �Ƹ�����?" });

        talkData.Add(100, new string[] { "����� �������ڴ�."});
        talkData.Add(200, new string[] { "������ ����� ������ �ִ� å���̴�."});
    }

    public string GetTalk(int id,int talkIndex)
    {
        if (talkIndex == talkData[id].Length)
            return null;
        else
            return talkData[id][talkIndex];
    }
}
