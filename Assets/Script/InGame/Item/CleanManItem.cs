using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CleanManItem : BaseItem, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private CleanMan cleanManObj;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SoundManager.Instance.PlaySe(SeEnum.Touch);
        if (!CheckCost(false))
            return;

        cleanManObj = Instantiate(Resources.Load<CleanMan>("InGame/Item/CleanMan"), transform);
        cleanManObj.tileController = tileController;
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        cleanManObj.transform.position = new Vector3(worldPos.x, worldPos.y + 70);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!CheckCost(false) || cleanManObj == null)
            return;

        Vector2 worldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        cleanManObj.transform.position = new Vector3(worldPos.x, worldPos.y + 100);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!CheckCost() || cleanManObj == null)
            return;

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
        

        if (tile != null)
        {
            cleanManObj.transform.position = tile.transform.position;
            cleanManObj.Init(tile);
            AddCost();
        }
        else
        {
            Destroy(cleanManObj.gameObject);
        }

        cleanManObj = null;
    }
}
