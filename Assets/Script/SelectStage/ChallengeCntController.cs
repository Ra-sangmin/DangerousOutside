using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeCntController : MonoBehaviour
{
    [SerializeField] private RectTransform cntNotFullPanel;
    [SerializeField] private RectTransform cntFullPanel;

    [SerializeField] private Text countText;
    [SerializeField] private Text countAddTimerText;

    bool fullCountOn = false;

    // Start is called before the first frame update
    void Start()
    {
        EventSet();
        Init();    
    }


    void EventSet()
    {
        TimeManager.Ins.secOn += SecOn;
    }
    private void OnDestroy()
    {
        TimeManager.Ins.secOn -= SecOn;
    }

    void Init()
    {
        GameManager.Ins.DateTimeSet();

        fullCountOn = GameManager.Ins.challengeCurrentCnt >= GameManager.Ins.challengeMaxCount;

        TextReSet();
    }

    void SecOn()
    {
        if (fullCountOn)
            return;

        DateTime now = DateTime.Now;
        if (DateTime.Compare(GameManager.Ins.nextChageDT, now) <= 0)
        {
            GameManager.Ins.CountAddOn(1);
            GameManager.Ins.NextChageDTAdd();
            fullCountOn = GameManager.Ins.challengeCurrentCnt >= GameManager.Ins.challengeMaxCount;
        }

        TextReSet();
    }

    void TextReSet()
    {
        cntNotFullPanel.gameObject.SetActive(!fullCountOn);
        cntFullPanel.gameObject.SetActive(fullCountOn);

        if (fullCountOn == false)
        {
            CountTextReSet();
            AddTimeReset();
        }
    }

    void CountTextReSet()
    {
        countText.text = string.Format("{0}/{1}", GameManager.Ins.challengeCurrentCnt, GameManager.Ins.challengeMaxCount);
    }

    void AddTimeReset()
    {
        DateTime now = DateTime.Now;

        int min = (GameManager.Ins.nextChageDT - now).Minutes;
        int sec = (GameManager.Ins.nextChageDT - now).Seconds;

        countAddTimerText.text = string.Format("{0:D1}:{1:D2}", min, sec);
    }

    void FullCountCheck()
    {
        fullCountOn = GameManager.Ins.challengeCurrentCnt == GameManager.Ins.challengeMaxCount;
        TextReSet();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CountAddOn(int addValue)
    {
        FullCountCheck();
    }
}
