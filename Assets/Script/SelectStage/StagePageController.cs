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

    public List<Animator> stageAnimList = new List<Animator>();

    [SerializeField] List<StageIcon> stageIconObjList = new List<StageIcon>();
    public StageIconData stageData;
    [SerializeField] RectTransform iconPanel;

    [SerializeField] AtlasImage bg;
    [SerializeField] RectTransform rtBG;

    [SerializeField] Image leftBtn;
    [SerializeField] Image rightBtn;

    [SerializeField] StageInfoPopup stageInfoPopup;

    [SerializeField] Animator lightAnim;
    [SerializeField] RectTransform lightObj;
    [SerializeField] Animator bossAnim;
    
    [SerializeField] List<Animator> activeTileAnimList = new List<Animator>();
    float activeTileDelay = 0;

    [SerializeField] Animator bossAnim2;
    [SerializeField] List<Animator> congalAnimList = new List<Animator>();
    [SerializeField] List<Animator> congalAnimList2 = new List<Animator>();

    [SerializeField] CanvasGroup congalAnimPanel;

    [SerializeField] Animator stage3_anim;

    public bool selectStageOn = false;

    bool stage_3_IntroOn = false;


    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    void Init()
    {
        PageDataSet();
        //BGSet();

        string animName = string.Empty;

        for (int i = 0; i < congalAnimList2.Count; i++)
        {
            switch (i)
            {
                case 0:
                    animName = "Congal_bat";
                    break;
                case 1:
                    animName = "Congal_devil";
                    break;
                case 2:
                    animName = "Congal_ghost";
                    break;
                case 3:
                    animName = "Congal_skeleton";
                    break;
                case 4:
                    animName = "Congal_wolf";
                    break;
            }

            congalAnimList2[i].Play(animName);
        }

        congalAnimPanel.alpha = 0;

    }

    public void PageDataSet()
    {
        List<StageIconData> currentPageDataList = StageIconDataManager.Ins.GetPageData(currenPage);

        foreach (var stageIconObj in stageIconObjList)
        {
            stageIconObj.gameObject.SetActive(false);
        }

        for (int i = stageIconObjList.Count; i < currentPageDataList.Count; i++)
        {
            StageIcon stageIcon = Instantiate(Resources.Load<StageIcon>("SelectStage/StageIcon"), iconPanel);
            stageIcon.transform.localPosition = currentPageDataList[i].posData;
            stageIconObjList.Add(stageIcon);
            stageIcon.clickEvent += StageStartOn;
        }

        for (int i = 0; i < currentPageDataList.Count; i++)
        {
            stageIconObjList[i].gameObject.SetActive(true);
            stageIconObjList[i].DataSet(currentPageDataList[i]);
            stageIconObjList[i].transform.localPosition = currentPageDataList[i].posData;
        }

        bossAnim.Play("Chapter1_Silhouette_default");

        if (GameManager.Ins.clearOn)
        {
            GameManager.Ins.clearOn = false;
            int clearIndex = GameManager.Ins.selectStageId;

            index = GameManager.Ins.selectStageId;

            lightObj.transform.DOLocalRotate(new Vector3(0, 0, GetLightRotValue()), 0).SetEase(Ease.Linear);

            index = clearIndex + 1;

            StartCoroutine(SetLightRot());

        }
    }

    public void ChapterSelectOn(int chapderIndex)
    {
        if (chapderIndex == 2 && stage_3_IntroOn == false)
        {
            StartCoroutine(Stage3IntroPlay());       
        }
    }

    IEnumerator Stage3IntroPlay()
    {
        congalAnimPanel.alpha = 0;

        stage_3_IntroOn = true;
        stage3_anim.Play("C3_start_walk");

        yield return new WaitForSeconds(11.25f);

        congalAnimPanel.alpha = 1;
    }


    private int index = 0;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            index++;

            if (index == 5)
            {
                index = 0;
            }

            StartCoroutine( SetLightRot());
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.GetComponent<RectTransform>().DOAnchorPosX(-1080, 1);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            transform.GetComponent<RectTransform>().DOAnchorPosX(0, 1);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            stage3_anim.Play("C3_r2b");

            for (int i = 0; i < congalAnimList2.Count; i++)
            {
                if (i < 5)
                {
                    congalAnimList2[i].Play("Congal_white");
                }   
            }
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            bossAnim2.Play("duck_r2y");
            congalAnimList[4].Play("Congal_duck");
        }


        ActiveTileDelayCheck();
    }

    void ActiveTileDelayCheck()
    {
        activeTileDelay -= Time.deltaTime;

        if (activeTileDelay < 0)
        {
            activeTileDelay = 3;

            int ran = Random.Range(0, activeTileAnimList.Count);
            activeTileAnimList[ran].Play("Stage1_bg");
        }
    }

    float GetLightRotValue()
    {
        float value = -230f;

        //빛이 회전해야될 각도 
        switch (index)
        {
            case 0:
                value = 50f;
                break;
            case 1:
                value = -20f;
                break;
            case 2:
                value = -90f;
                break;
            case 3:
                value = -160f;
                break;
            case 4:
                value = -230f;
                break;
        }
        return value;
    }

    IEnumerator SetLightRot()
    {
        if (index < 5 )
        {
            float rotValue = GetLightRotValue();

            //빛 움직임 준비
            lightAnim.Play("Chapter1_Light_begin_anim");
            yield return new WaitForSeconds(1);

            //빛 움직임 시작
            lightAnim.Play("Chapter1_Light_moving_anim");
            lightObj.DOLocalRotate(new Vector3(0, 0, rotValue), 1).SetEase(Ease.Linear);

            yield return new WaitForSeconds(1f);

            //빛 움직임 끝
            lightAnim.Play("Chapter1_Light_end_aim");
        }

        yield return new WaitForSeconds(1f);

        if (index == 4)
        {
            bossAnim.Play("Stage1_Silhouette");
        }
        if (index == 5)
        {
            bossAnim.Play("Chapter1_Silhouette_clear");
        }
        else
        {
            bossAnim.Play("Chapter1_Silhouette_default");
        }
    }


    public void StageStartOn(StageIconData stageData)
    {
        SoundManager.Instance.PlaySe(SeEnum.Touch);
        this.stageData = stageData;

        //if (this.stageData.id >= 5)
        //{
        //    WarningManager.Instance.WarningSet("준비중 입니다. 다음 업데이트를 기다려주세요.");
        //}
        //else
        //if (this.stageData.id <= StageIconDataManager.Ins.playMaxStageID + 1)
        //if (this.stageData.id <= StageIconDataManager.Ins.playMaxStageID + 1)
        {
            StageSaveData stageSaveData = stageAllSaveData.stageList.Find(data => data.stageId == this.stageData.id);
            stageInfoPopup.Init(this.stageData, stageSaveData);
            stageInfoPopup.gameObject.SetActive(true);
        }
        //else
        //{
          //  WarningManager.Instance.WarningSet("아직은 입장할수 없습니다.");
        //}
    }

    public void StageStartOn(int stageNum)
    {
        if (selectStageOn == false)
            return;
        
        StageSaveData stageSaveData = stageAllSaveData.stageList.Find(data => data.stageId == stageNum);

        //this.stageData = stageSaveData; 
        //Debug.LogWarning(stageSaveData.stageId);

        stageInfoPopup.Init(this.stageData, stageSaveData);
        stageInfoPopup.gameObject.SetActive(true);

        GameManager.Ins.StageStartOn(stageNum);

    }

    public void StartOn()
    {
        //Debug.LogWarning("start On = " + startEvent);
        if (startEvent != null)
        {
            SoundManager.Instance.PlaySe(SeEnum.Touch);
            startEvent();
        }
    }

    public void PageMove(bool leftOn)
    {
        SoundManager.Instance.PlaySe(SeEnum.Touch);
        if (leftOn)
        {
            currenPage = Mathf.Max(--currenPage,0);
        }
        else
        {
            currenPage = Mathf.Min(++currenPage, StageIconDataManager.Ins.maxPage);
            
        }

        StartCoroutine(BGSetDoing());

        

    }

    IEnumerator BGSetDoing() 
    {
        iconPanel.gameObject.SetActive(false);


        rtBG.transform.DOLocalMoveX(BGSet(), 1);

        yield return new WaitForSeconds(1);

        PageDataSet();
        iconPanel.gameObject.SetActive(true);

    }

    public float BGSet()
    {
        float targetPos = 0;

        if (currenPage == 0)
        {
            //bg.spriteName = "Img_Map_3";

            targetPos = 0;

            rightBtn.gameObject.SetActive(true);
            leftBtn.gameObject.SetActive(false);
        }
        else if (currenPage == 1)
        {
            //bg.spriteName = "Img_Map_4";

            targetPos = -1698;

            leftBtn.gameObject.SetActive(true);
            rightBtn.gameObject.SetActive(false);
        }


        return targetPos;


    }

}
