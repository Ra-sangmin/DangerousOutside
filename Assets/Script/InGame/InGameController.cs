using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

/// <summary>
/// 인게임을 관리하는 클래스
/// </summary>
public class InGameController : MonoBehaviour
{
    [SerializeField] TimerController timerController;
    [SerializeField] TileController tileController;
    [SerializeField] TaxController taxController;
    [SerializeField] CityhallController cityhallController;

    public SettingPopup settingPopup;
    public RectTransform resultFailPopup;
    public ResultWinPopup resultWinPopup;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Ins.resultOn = false;

        EventSet();

        if (GameManager.Ins.tutorialOn == false)
        {
            Init();
        }
    }

    /// <summary>
    /// 이벤트 설정
    /// </summary>
    void EventSet()
    {
        timerController.timeOverEvent = GameFailOn;
        if (GameManager.Ins.selectStageId == 0)
            timerController.timeOverEvent = GameWinOn;
        
        tileController.gameFailOn = GameFailOn;
        tileController.gameWinOn = GameWinOn;
    }

    /// <summary>
    /// 초기화
    /// </summary>
    public void Init()
    {
        SoundManager.Instance.PlayBGM(BGMEnum.InGame);

        int timeValue = GameManager.Ins.selectStageId == 0 ? 60 : 180;
        timerController.TimeStart(timeValue);

        tileController.Init();
        taxController.Init();
        cityhallController.Init();

        settingPopup.DataSet();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            tileController.gameWinOn();
        }
    }

    void GameFailOn()
    {
        SoundManager.Instance.StopBGM();
        SoundManager.Instance.PlaySe(SeEnum.Result_Fail);
        GameManager.Ins.resultOn = true;
        timerController.timeOn = false;

        resultFailPopup.gameObject.SetActive(true);
    }


    public void PauseOn(bool pause)
    {
        SoundManager.Instance.PlaySe(SeEnum.Touch);
        GameManager.Ins.SetPause(pause);
        settingPopup.gameObject.SetActive(pause);
    }

    public void GoStageOn()
    {
        SoundManager.Instance.PlaySe(SeEnum.Touch);
        PauseOn(false);
        SceneManager.LoadScene("SelectStage");
    }

    public void RestartOn()
    {
        SoundManager.Instance.PlaySe(SeEnum.Touch);
        //if (GameManager.Ins.challengeCurrentCnt <= 0)
        //{
        //    WarningManager.Instance.WarningSet("도전 횟수가 부족합니다.");
        //    return;
        //}
            

        PauseOn(false);
        GameManager.Ins.StageStartOn(GameManager.Ins.selectStageId,true);
    }

    public void NextStageStartOn()
    {
        SoundManager.Instance.PlaySe(SeEnum.Touch);
        //if (GameManager.Ins.challengeCurrentCnt <= 0)
        //{
        //    WarningManager.Instance.WarningSet("도전 횟수가 부족합니다.");
        //    return;
        //}

        PauseOn(false);
        GameManager.Ins.StageStartOn(GameManager.Ins.selectStageId+1);
    }

    public void ContinueOn()
    {
        SoundManager.Instance.PlaySe(SeEnum.Touch);
        PauseOn(false);
    }

    void GameWinOn()
    {
        SoundManager.Instance.StopBGM();
        SoundManager.Instance.PlaySe(SeEnum.Result_Success);
        GameManager.Ins.resultOn = true;
        GameManager.Ins.clearOn = true;
        timerController.timeOn = false;

        int redCnt = tileController.citizenList.Count(data => data.citizen_color == CitizenColor.Red);
        resultWinPopup.gameObject.SetActive(true);
        resultWinPopup.Init(timerController.secTime, redCnt, tileController.stageSaveData);

        ItemSaveDataSet();
    }

    void ItemSaveDataSet()
    {
        ItemState saveItem = ItemState.None; 

        switch (GameManager.Ins.selectStageId)
        {
            case 0 :
                saveItem = ItemState.CleanMan; 
                break;
            case 1:
                saveItem = ItemState.GiftCntAdd;
                break;
            case 2:
                saveItem = ItemState.Delivery;
                //saveItem = ItemState.Cleaner;
                break;
            case 3:
                saveItem = ItemState.ForceGoHome;
                break;
            case 4:
                saveItem = ItemState.BanArea; 
                break;
            case 5:
                break;
        }

        if (saveItem != ItemState.None)
        {
            ItemManager.Ins.SaveItemOnData(saveItem);
        }
    }
}
