using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Linq;
using Mobcast.Coffee.UI;
using DG.Tweening;

public class StagePageController : MonoBehaviour
{
    public StageAllSaveData stageAllSaveData;
    public UnityAction startEvent = null;
    public int currenPage = 0;
    public int selectStage;

    public StageIconData stageData { get; set; }
    
    public bool selectStageOn = false;

    [SerializeField] List<BaseChapter> chapterList = new List<BaseChapter>();
    [SerializeField] StageInfoPopup stageInfoPopup;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    void Init()
    {
        EventSet();
    }

    void EventSet() 
    {
        foreach (var chapter in chapterList)
        {
            chapter.clickEvent = StageStartOn;
            chapter.Init();
        }
    }

    public void ChapterLoadReadyOn(int chapderIndex)
    {
        currenPage = chapderIndex;
        chapterList[currenPage].ChapterLoadReadyOn();
    }

    public void ChapterSelectOn(int chapderIndex)
    {
        selectStageOn = true;
        currenPage = chapderIndex;
        chapterList[currenPage].ChapterDataSet();
    }
   
    // Update is called once per frame
    void Update()
    {
    }
   
    public void StageStartOn(StageIconData selectStageData)
    {
        if (selectStageOn == false)
            return;

        SoundManager.Instance.PlaySe(SeEnum.Touch);

        if (selectStageData.id > 0 )
        {
            int selectId = selectStageData.id - 1;
            StageIconDataUserData userData = StageIconDataManager.Ins.GetStageData(selectId).stageIconDataUserData;

            //if (userData.playOn == false)
            //{
            //    WarningManager.Instance.WarningSet("이전 스테이지를 클리어 하십시오.");
            //    return;
            //}
        }

        this.stageData = selectStageData;

        StageSaveData stageSaveData = stageAllSaveData.stageList.Find(data => data.stageId == selectStageData.id);
        stageInfoPopup.Init(this.stageData, stageSaveData);
        stageInfoPopup.gameObject.SetActive(true);
    }

    public void StartOn()
    {
        if (startEvent != null)
        {
            SoundManager.Instance.PlaySe(SeEnum.Touch);
            startEvent();
        }
    }
}
