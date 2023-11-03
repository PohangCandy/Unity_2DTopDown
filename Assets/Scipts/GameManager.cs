using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TalkManager talkManager;
    public QuestManager questManager;
    public GameObject talkPanel;
    public Image portraiting;
    public Text talkText;
    public GameObject scanObject;
    public bool isAction;
    public int talkIndex;

    public void Start()
    {
        Debug.Log(questManager.CheckQuest());
    }
    public void Action(GameObject scanObj)
    {
        //Get Current Object
        scanObject = scanObj;
        ObjData objData = scanObj.GetComponent<ObjData>();
        Talk(objData.id, objData.isNpc);

        //Visible Talk for Action
        talkPanel.SetActive(isAction);
    }

    void Talk(int id,bool isNpc)
    {
        //Set Talk Index
        int questTalkIndex = questManager.GetQuestTalkIndex(id);
        string talkData = talkManager.GetTalk(id + questTalkIndex, talkIndex);

        //End Talk
        if (talkData == null)
        {
            isAction = false;
            talkIndex = 0;
            Debug.Log(questManager.CheckQuest(id));
            return;
        }

        //Continue Talk
        if (isNpc)
        {
            talkText.text = talkData.Split(":")[0];

            portraiting.sprite =talkManager.GetPortrait(id, int.Parse(talkData.Split(":")[1]));
            portraiting.color = new Color(1, 1, 1, 1);
        }
        else
        {
            talkText.text = talkData;

            portraiting.color = new Color(1, 1, 1, 0);
        }

        isAction = true;
        talkIndex++;
    }
}
