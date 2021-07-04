using System.Collections;
using System.Collections.Generic;
using Mobcast.Coffee.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StageIcon : MonoBehaviour
{
    private StageIconData stageData;

    [SerializeField] private AtlasImage bgImage;
    [SerializeField] private AtlasImage findImage;
    [SerializeField] private Text stageNumText;

    [SerializeField] private RectTransform starIconPanel;
    [SerializeField] private List<AtlasImage> starIconList = new List<AtlasImage>();

    public UnityAction<StageIconData> clickEvent;

    // Start is called before the first frame update
    void Start()
    {
        //DataSet();
    }

    public void DataSet(StageIconData stageData)
    {
        this.stageData = stageData;
        stageNumText.text = (stageData.id+1).ToString();

        findImage.gameObject.SetActive(false);
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
    }

    public void ClickOn()
    {
        if (clickEvent != null)
        {
            clickEvent(stageData);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
