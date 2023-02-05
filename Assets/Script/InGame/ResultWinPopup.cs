using System.Collections;
using System.Collections.Generic;
using Mobcast.Coffee.UI;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ResultWinPopup : MonoBehaviour
{
    [SerializeField] Text currentTimeText;
    [SerializeField] Text redCitizenCntText;

    [SerializeField] Text targetStageClearText;
    [SerializeField] Text targetTimeText;
    [SerializeField] Text targetredCitizenText;

    [SerializeField] List<Animator> animList = new List<Animator>();

    [SerializeField] AtlasImage minTimeOnLine;
    [SerializeField] AtlasImage minRedCntLine;

    [SerializeField] RectTransform nextBtn;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Init(int timeValue , int redCnt, StageSaveData stageSaveData)
    {
        int min = (int)(timeValue / 60);
        int sec = (int)(timeValue % 60);
        currentTimeText.text = string.Format("{0:D2}:{1:D2}", min, sec);
        redCitizenCntText.text = redCnt.ToString();

        int missionMin = (int)(stageSaveData.minClearTime / 60);
        int missionSec = (int)(stageSaveData.minClearTime % 60);

        TextReset(targetStageClearText, "클리어", 0.5f);
        
        string targetTimeStr = string.Format("{0}분 {1:D2}초 이내", missionMin, missionSec);
        TextReset(targetTimeText, targetTimeStr,0.8f);

        string targetredCitizenStr = string.Format("누적 {0}명", stageSaveData.minRedCitizen);
        TextReset(targetredCitizenText, targetredCitizenStr,1.1f);

        int starCnt = 1;

        bool minTimeOn = timeValue >= stageSaveData.minClearTime;
        bool minRedCntOn = redCnt <= stageSaveData.minRedCitizen;

        minTimeOnLine.gameObject.SetActive(minTimeOn);
        minRedCntLine.gameObject.SetActive(minRedCntOn);

        if (minTimeOn && minRedCntOn)
        {
            starCnt = 3;
        }
        else if (minTimeOn || minRedCntOn)
        {
            starCnt = 2;
        }

        animList[0].SetBool("success", minTimeOn);
        animList[0].SetInteger("index", 2);

        animList[1].SetInteger("index", 1);

        animList[2].SetBool("success", minRedCntOn);
        animList[2].SetInteger("index", 2);

        StageIconDataManager.Ins.StarCntSet(GameManager.Ins.selectStageId, starCnt);

        if (GameManager.Ins.selectStageId == 3 || GameManager.Ins.selectStageId == 4)
        {
            nextBtn.gameObject.SetActive(false);
        }
    }

    void TextReset(Text textObj, string str,float delay)
    {
        textObj.text = str;

        AtlasImage image = textObj.transform.GetComponentInChildren<AtlasImage>();
        image.DOFade(0, 0).OnComplete(()=>
        {
            image.DOFade(1, 0.5f).SetDelay(delay);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
