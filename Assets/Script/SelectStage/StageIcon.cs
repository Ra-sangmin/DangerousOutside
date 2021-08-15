using System.Collections;
using System.Collections.Generic;
using Mobcast.Coffee.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StageIcon : MonoBehaviour
{
    public StageIconData stageData;

    [SerializeField] private AtlasImage bgImage;
    [SerializeField] private AtlasImage findImage;
    [SerializeField] private Text stageNumText;

    [SerializeField] private RectTransform starIconPanel;
    [SerializeField] private List<Animator> starIconList = new List<Animator>();

    public UnityAction<StageIconData> clickEvent;

    public Animator amin;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void DataSet(StageIconData stageData , UnityAction<StageIconData> clickEvent)
    {
        this.stageData = stageData;

        this.clickEvent = clickEvent;

        stageNumText.text = (stageData.id+1).ToString();

        //findImage.gameObject.SetActive(false);
        stageNumText.fontSize = 72;

        var userData = stageData.stageIconDataUserData;

        bool playerOn = userData.playOn && userData.starCount > 0;

        /*
        starIconPanel.gameObject.SetActive(playerOn);
        stageNumText.transform.localPosition = playerOn ? new Vector3(0, -27, 0) : Vector3.zero;

        for (int i = 0; i < starIconList.Count; i++)
        {
            starIconList[i].gameObject.SetActive(i < userData.starCount);
        }

        if (playerOn)
        {
            //기존 포인트
            bgImage.spriteName = userData.starCount == 3 ? "Img_Stage_3star" : "Img_Stage_poiont";
        }
        else
        {
            if (stageData.id == StageIconDataManager.Ins.playMaxStageID + 1)
            {
                //현재 포인트
                findImage.gameObject.SetActive(true);
                stageNumText.fontSize = 90;
            }
            else
            {
                //미발견 포인트
                bgImage.spriteName = "Img_Stage_poiont_unfind";
            }
        }*/

        AnimSet();
    }

    void AnimSet()
    {
        var userData = stageData.stageIconDataUserData;

        string animName = string.Empty;

        switch (stageData.pageIndex) 
        {
            case 0:
                animName = userData.starCount != 0 ? "Chapter1_stage_stars_yellow" : "Chapter_stage_stars_gray";
                break;
            case 1:
                animName = userData.starCount != 0 ? "Congal_duck" : "Congal_help";
                break;
            case 2:
                animName = userData.starCount != 0 ? "Congal_white" : GetChapter3IconAnim(stageData.id);
                break;
        }

        amin.Play(animName);

        StartCoroutine(StarReset());
    }


    string GetChapter3IconAnim(int stageId)
    {
        string animName = string.Empty;

        switch (stageId)
        {
            case 10:
                animName = "Congal_bat";
                break;
            case 11:
                animName = "Congal_devil";
                break;
            case 12:
                animName = "Congal_ghost";
                break;
            case 13:
                animName = "Congal_skeleton";
                break;
            case 14:
                animName = "Congal_wolf";
                break;
        }

        return animName;
    }


    public void ClickOn()
    {
        if (clickEvent != null)
        {
            clickEvent(stageData);
        }
    }

    public void ClearEventOn()
    {
        StartCoroutine(StarReset(true , 0.2f));
    }

    public void StarAllInactive()
    {
        for (int i = 0; i < starIconList.Count; i++)
        {
            starIconList[i].Play("ready");
        }
    }

    IEnumerator StarReset(bool eventOn = false, float delay = 0)
    {
        var userData = stageData.stageIconDataUserData;

        for (int i = 0; i < starIconList.Count; i++)
        {
            string animName = "ready";

            if (i < userData.starCount)
            {
                animName = eventOn ? "clear" : "clear_loop";
            }

            starIconList[i].Play(animName);

            yield return new WaitForSeconds(delay);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
