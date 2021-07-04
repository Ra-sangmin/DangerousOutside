using System.Collections;
using System.Collections.Generic;
using Mobcast.Coffee.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class TutorialController_0 : BaseTutorialController
{
    private CleanMan cleanManObj;
    [SerializeField] private AtlasImage handImage;
    bool handMoveStop = false;

    // Start is called before the first frame update
    void Start()
    {
        Init();
        SetEvent();
    }

    void SetEvent()
    {
        chatController.chatEndEvent = ChatEndEvent;
    }

    protected override void Init()
    {
        base.Init();
        StartCoroutine(TutorialStart());
    }

    void TileSet()
    {
        tileController.GetTile(new Vector2(7, 11)).TileChange(Tile_Type.Red);
    }

    IEnumerator TutorialStart()
    {
        TileSet();

        chatController.ChatPopupActive(false);
        tutorialObjPanel.gameObject.SetActive(false);
        yield return new WaitForSeconds(3);

        chatController.DataSet(TutorialChatDataManager.Ins.GetData("0_0"));
        yield return new WaitUntil(()=> nextOn);
        nextOn = false;

        tutorialObjPanel.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        ShowTutorialObj(0);
        yield return new WaitForSeconds(2);

        tutorialObjPanel.gameObject.SetActive(false);
        chatController.DataSet(TutorialChatDataManager.Ins.GetData("0_1"));
        yield return new WaitUntil(() => nextOn);

        nextOn = false;
        tutorialObjPanel.gameObject.SetActive(true);
        ShowTutorialObj(1);

        HandMove();

        yield return new WaitUntil(() => nextOn);

        handMoveStop = true;
        nextOn = false;
        tutorialObjPanel.gameObject.SetActive(false);

        yield return new WaitForSeconds(1);
        cleanManObj.stopOn = true;

        yield return new WaitForSeconds(1);

        chatController.DataSet(TutorialChatDataManager.Ins.GetData("0_2"));
        yield return new WaitUntil(() => nextOn);

        Destroy(cleanManObj.gameObject);

        yield return new WaitForSeconds(0.5f);

        GameManager.Ins.SetTutorialState(0);
        GameManager.Ins.tutorialOn = false;
        GameManager.Ins.StageStartOn(GameManager.Ins.selectStageId, true);
    }

    void HandMove()
    {
        if (handMoveStop == true)
            return;

        handImage.color = Color.white;
        handImage.transform.localPosition = Vector3.zero;

        handImage.transform.DOLocalMoveY(830, 1.5f)
            .SetEase(Ease.Linear)
            .SetDelay(1);

        handImage.DOFade(0, 1)
            .SetDelay(3)
            .OnComplete(() => HandMove());
    }

    void ShowTutorialObj(int index)
    {
        for (int i = 0; i < tutorialObjList.Count; i++)
        {
            tutorialObjList[i].gameObject.SetActive(i == index);
        }
    }

    public void ChatEndEvent()
    {
        chatController.ChatPopupActive(false);
        nextOn = true;
    }

    public void CleanManItemDrag(BaseEventData baseEventData)
    {
        PointerEventData eventData = (PointerEventData)baseEventData;

        if (cleanManObj == null)
        {
            cleanManObj = Instantiate(Resources.Load<CleanMan>("InGame/Item/CleanMan"), transform);
        }

        Vector2 worldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        cleanManObj.transform.position = new Vector3(worldPos.x, worldPos.y + 100);
    }
    public void CleanManItemOnPointerUp(BaseEventData baseEventData)
    {
        if (cleanManObj == null)
        {
            return;
        }

        PointerEventData eventData = (PointerEventData)baseEventData;

        Vector2 worldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        worldPos.y += 100;

        Tile tile = null;
        float distance = 100;
        
        for (int x = 0; x < tileController.x_max_value; x++)
        {
            for (int y = 0; y < tileController.y_max_value; y++)
            {
                Vector2 tilePos = tileController.tiles[x, y].transform.position;

                float tempDistance = Vector2.Distance(worldPos, tilePos);

                if (distance > tempDistance)
                {
                    distance = tempDistance;
                    tile = tileController.tiles[x, y];
                }
            }
        }
        

        if (tile == null)
        {
            Destroy(cleanManObj.gameObject);
            cleanManObj = null;
        }
        else
        {
            if (tile.pos == new Vector2(7,11))
            {
                cleanManObj.transform.position = tile.transform.position;
                cleanManObj.Init(tile);
                nextOn = true;
            }
            else
            {
                Destroy(cleanManObj.gameObject);
                cleanManObj = null;
                WarningManager.Instance.WarningSet("빨강 타일에 소환해 주십시오.");
            }
        }
    }
}