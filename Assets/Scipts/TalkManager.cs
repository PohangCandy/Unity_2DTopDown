using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData;
    Dictionary<int, Sprite> portraitData;

    public Sprite[] portraitArr;
    void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        portraitData = new Dictionary<int, Sprite>();
        GenerateData();
    }

    // Update is called once per frame
    void GenerateData()
    {
        //TalkDAta
        //NPC A:1000, NPC B:2000
        //Box: 100, Desk: 200
        talkData.Add(1000, new string[] { "안녕?:0",
                                          "이 곳에 처음 왔구나?:1" ,
                                           "한번 둘러보도록 해.:0"});

        talkData.Add(2000, new string[] { "여어:1",
                                          "이곳의 호수를 본 적 있니?:0",
                                          "사실 그곳엔 숨겨진 비밀이 있어.:1"});

        talkData.Add(100, new string[] { "내가 가지고 다니는 나무상자다."});
        talkData.Add(200, new string[] { "누군가 사용한 흔적이 있는 책상이다."});

        //Quest Talk
        talkData.Add(10 + 1000, new string[] { "그거 알아?:1",
                                          "이 마을에 놀라운 전설이 있다는데:0" ,
                                          "오른쪽에 루도가 알려줄거야.:0"});

        talkData.Add(11 + 2000, new string[] { "안녕, 또 보네:1",
                                          "이 호수의 전설을 들으러 온거야?:0" ,
                                          "그럼 일 좀 하나 해주면 좋을텐데...:1",
                                          "내 집 근처에 떨어진 동전 좀 주워줘.:0"});

        talkData.Add(20 + 1000, new string[] { "루도의 동전?:0",
                                          "돈을 흘리고 다니면 못쓰지!:3" ,
                                          "나중에 루도에게 한마디 해야겠어..:3"});
        talkData.Add(20 + 2000, new string[] { "찾으면 꼭 좀 가져다 줘:1"});
        talkData.Add(20 + 5000, new string[] { "떨어져 있는 동전을 찾았다" });

        talkData.Add(21 + 2000, new string[] { "앗, 찾아줘서 고마워!:2" });


        //Portrait Data
        //0:Normal, 1:Speak, 2:Happy, 3:Angry
        portraitData.Add(1000 + 0, portraitArr[0]);
        portraitData.Add(1000 + 1, portraitArr[1]);
        portraitData.Add(1000 + 2, portraitArr[2]);
        portraitData.Add(1000 + 3, portraitArr[3]);
        portraitData.Add(2000 + 0, portraitArr[4]);
        portraitData.Add(2000 + 1, portraitArr[5]);
        portraitData.Add(2000 + 2, portraitArr[6]);
        portraitData.Add(2000 + 3, portraitArr[7]);
    }

    public string GetTalk(int id,int talkIndex)
    {
        if (!talkData.ContainsKey(id))
        {
            //퀘스트 맨 처음 대사마저 없을 때
            //기본 대사를 가지고 온다.
            if (!talkData.ContainsKey(id - id % 10))
                return GetTalk(id - id % 100, talkIndex); //Get First
            else
                return GetTalk(id - id % 10, talkIndex); //Get First Quest Talk
        }

        if (talkIndex == talkData[id].Length)
            return null;
        else
            return talkData[id][talkIndex];
    }

    public Sprite GetPortrait(int id, int portraitindex)
    {
        return portraitData[id + portraitindex];
    }
}
