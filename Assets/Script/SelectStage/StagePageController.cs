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
