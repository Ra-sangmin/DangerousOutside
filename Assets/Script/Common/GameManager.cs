﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager
{
    private static GameManager ins;
    public static GameManager Ins
    {
        get
        {
            if (ins == null)
                ins = new GameManager();
            return ins;
        }
    }

    public bool resultOn = false;
    public bool pause = false;

    string vibrationOnkey = "vibrationOn";
    public bool vibrationOn = true;
    public int beforeSelectStageId = -1;
    public int selectStageId = -1;
    public bool clearOn = false;

    string challengeCurrentCntkey = "challengeCurrentCnt";
    public int challengeCurrentCnt;
    public int challengeMaxCount = 5;

    string nextChageTimekey = "nextChageTime";
    public DateTime nextChageDT = DateTime.Now;

    string tutorialStatekey = "tutorialState";
    public int tutorialState;
    public bool tutorialOn = false;

    public TileController tileController;

    public string targetScene;

    public int difficultyStatus = 0;

    private GameManager()
    {
        Init();
    }

    void Init()
    {
        vibrationOn = PlayerPrefs.GetInt(vibrationOnkey, 1) == 0 ? false : true;
        tutorialState = PlayerPrefs.GetInt(tutorialStatekey, 0);
        tutorialState = 5;
        //challengeCurrentCnt = 5;
    }

    public void VibrationSet(bool value)
    {
        vibrationOn = value;
        PlayerPrefs.SetInt(vibrationOnkey, vibrationOn == true ? 1 : 0);

        if (vibrationOn)
        {
#if UNITY_ANDROID
            Handheld.Vibrate();
#endif
        }
    }

    public void NextChageDTAdd()
    {
        //도전횟수1당 필요 시간
        int addSec = 60 * 3;
        nextChageDT = nextChageDT.AddSeconds(addSec);
    }

    public void DateTimeSet()
    {
        //PlayerPrefs.DeleteAll();

        challengeCurrentCnt = PlayerPrefs.GetInt(challengeCurrentCntkey, 5);

        string strValue = PlayerPrefs.GetString(nextChageTimekey, "-1");
        long nextChagetick = long.Parse(strValue);

        DateTime now = DateTime.Now;

        //도전 횟수가 풀이라면
        if (challengeCurrentCnt == 5)
        {
            return;
        }
        ////도전횟수 정보가 없다며
        //else if (nextChagetick == -1)
        //{
        //    challengeCurrentCnt = 5;
        //}
        else
        {
            nextChageDT = new DateTime(nextChagetick);

            for (int i = challengeCurrentCnt; i < 5; i++)
            {
                //도전횟수 충전시간이 지금 보다 미래라면
                if (DateTime.Compare(nextChageDT, now) >= 1)
                {
                    break;
                }
                //이미 도전시간 충전시간이 과거라면
                else
                {
                    challengeCurrentCnt++;

                    if (challengeCurrentCnt >= 5)
                    {
                        break;
                    }

                    NextChageDTAdd();
                }
            }
        }

        PlayerPrefs.SetString(nextChageTimekey, nextChageDT.Ticks.ToString());
        PlayerPrefs.SetInt(challengeCurrentCntkey, challengeCurrentCnt);
    }

    public void CountAddOn(int addValue)
    {
        if (challengeCurrentCnt == 5 && addValue < 0)
        {
            nextChageDT = DateTime.Now;
            NextChageDTAdd();
        }

        challengeCurrentCnt += addValue;

        PlayerPrefs.SetString(nextChageTimekey, nextChageDT.Ticks.ToString());
        PlayerPrefs.SetInt(challengeCurrentCntkey, challengeCurrentCnt);
    }

    public void StageStartOn(int selectStageId, bool tutorialSkip = false)
    {
        if (selectStageId > 9)
        {
            WarningManager.Instance.WarningSet("준비중 입니다.");
            return;
        }

        beforeSelectStageId = this.selectStageId;

        this.selectStageId = selectStageId;
        CountAddOn(-1);
        
        StageIconDataManager.Ins.PlayOnSet(selectStageId);
        resultOn = false;

        string targetSceneName = "InGame";

        //1스테이지만 튜토리얼 반복
        GameManager.ins.tutorialOn = this.selectStageId == 0;

        if (tutorialSkip == false)
        {
            if (tutorialState == 0 && this.selectStageId >= 0)
            {
                targetSceneName = "Tutorial_0";
                tutorialOn = true;
            }
            else if (tutorialState == 1 && this.selectStageId >= 1)
            {
                targetSceneName = "Tutorial_1";
                tutorialOn = true;
            }
            else if (tutorialState == 2 && this.selectStageId >= 2)
            {
                targetSceneName = "Tutorial_2";
                tutorialOn = true;
            }
        }

        targetScene = targetSceneName;

        SceneManager.LoadScene("Loading");
    }
    public void SetPause(bool pause)
    {
        GameManager.Ins.pause = pause;
        Time.timeScale = pause ? 0 : 1;
    }

    public void SetTutorialState(int clearTutorialState)
    {
        int nextState = ++clearTutorialState;
        this.tutorialState = nextState;
        PlayerPrefs.SetInt(tutorialStatekey, tutorialState);
    }
}
