using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class DescriptionManager : MonoBehaviour
{
    [Header("Contents")]
    public GameObject objDescriptionPanel;
    public TileController objTileManager;
    public InGameController objInGameController;
    public TaxController objTaxController;

    [Header("Values")]
    //[SerializeField]
    private int intCurrentStage;
    [SerializeField]
    private int intCurrentCardIndex;

    public TutorialImageAllData tutorialImageAllData;
    List<Sprite> imageList = new List<Sprite>();
    List<Button> imageBtnList = new List<Button>();

    private void Awake()
    {
        if (GameManager.Ins.tutorialOn)
        {
            Init();
        }
    }

    void Init()
    {
        int stageId = GameManager.Ins.selectStageId;

        TutorialImageData tutorialImageData = tutorialImageAllData.tutorialImageDataList.Find(data => data.stageID == stageId);

        if (tutorialImageData != null)
        {
            imageList = tutorialImageData.imageList;
            ImageSet();
        }
        else
        {
            GameStart();
        }
    }

    void ImageSet()
    {
        for (int i = 0; i < imageList.Count; i++)
        {
            int index = i;

            GameObject obj = Instantiate(Resources.Load("InGame/Other/ImgDescription"), objDescriptionPanel.transform) as GameObject;
            Button btn = obj.GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => ButtonClickOn(index));

            obj.GetComponent<Image>().sprite = imageList[i];

            imageBtnList.Add(btn);
        }

        for (int i = 0; i < imageBtnList.Count; i++)
        {
            imageBtnList[i].gameObject.transform.SetSiblingIndex((imageBtnList.Count -1)- i);
        }

        objDescriptionPanel.gameObject.SetActive(true);
    }

    public void ButtonClickOn (int index)
    {
        CardClickAnim(imageBtnList[index].gameObject);

        if (index >= imageBtnList.Count -1)
        {
            GameStart();
        }
    }

    void CardClickAnim(GameObject obj)
    {
        RectTransform rtObj = obj.transform.GetComponent<RectTransform>();

        rtObj.transform.DOLocalMoveX(-2000, 1);

        intCurrentCardIndex++;
    }

    //게임시작
    void GameStart()
    {
        GameManager.Ins.tutorialOn = false;
        objInGameController.Init();
        objDescriptionPanel.SetActive(false);
    }
}
