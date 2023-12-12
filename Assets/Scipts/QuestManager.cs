using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public int questId;
    public int questActionIndex;
    public GameObject[] questObject;

    Dictionary<int, QuestData> questList,MyquestList;

    private void Awake()
    {
        MyquestList = new Dictionary<int, QuestData>();
        GenerateData();
    }

    void GenerateData()
    {
        MyquestList.Add(10, new QuestData("마을 사람들과 대화하기."
                                        ,new int[] {1000,2000 }));
        MyquestList.Add(20, new QuestData("루도의 동전 찾아주기."
                                       , new int[] { 5000, 2000 }));
        MyquestList.Add(30, new QuestData("퀘스트 올 클리어!"
                                       , new int[] { 0 }));

        questList = new Dictionary<int, QuestData>();
        questList = MyquestList;
    }

    public int GetQuestTalkIndex(int id)
    {
        return questId + questActionIndex;
    }

    public string CheckQuest(int id)
    {
        //Next Talk Target
        if (id == questList[questId].npcId[questActionIndex])
            questActionIndex++;

        //Control Quest Object
        ControlObject();

        //Talk Complete & Next Quest
        if (questActionIndex == questList[questId].npcId.Length)
            NextQuest();

        //Quest Name
        return questList[questId].questName;
    }

    public string CheckQuest()
    {
        //Quest Name
        return questList[questId].questName;
    }
    void NextQuest()
    {
        questId += 10;
        questActionIndex = 0;
    }

    public void ControlObject()
    {
        switch(questId)
        {
            case 10:
                if(questActionIndex == 2)
                    questObject[0].SetActive(true);
                break;
            case 20:
                if(questActionIndex == 0)
                    questObject[0].SetActive(true);
                if(questActionIndex == 1)
                    questObject[0].SetActive(false);
                break;
        }
    }
}
