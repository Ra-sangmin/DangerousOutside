using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BanAreaItem : BaseItem, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private GameObject banAreaObj;
    List<BanAreaData> banAreaDataList = new List<BanAreaData>();

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        needCost = 2;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!CheckCost(false))
            return;
      
        banAreaObj = Instantiate(Resources.Load("InGame/Item/BanAreaObj"),transform) as GameObject;
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        banAreaObj.transform.position = new Vector3(worldPos.x, worldPos.y);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!CheckCost(false) || banAreaObj == null)
            return;

        Vector2 worldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        banAreaObj.transform.position = worldPos;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!CheckCost() || banAreaObj == null)
            return;

        Vector2 worldPos = Camera.main.ScreenToWorldPoint(eventData.position);
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
        
        //금지구역 생성
        if (tile != null)
        {
            CreateBanAreaData(tile);
        }
        else
        {
            Destroy(banAreaObj);
        }

        banAreaObj = null;
    }

    void CreateBanAreaData(Tile tile)
    {
        AddCost();

        Vector2 tilePos = tile.transform.position;
        tilePos += new Vector2(26, -21);
        banAreaObj.transform.position = tilePos;

        int xMinVaue = (int)tile.pos.x - 2;
        int xMaxVaue = (int)tile.pos.x + 2;
        int yMinVaue = (int)tile.pos.y - 2;
        int yMaxVaue = (int)tile.pos.y + 2;

        
        //금지 구역 지정
        List<Node> banTileList = new List<Node>();
        
        for (int x = xMinVaue; x <= xMaxVaue; x++)
        {
            for (int y = yMinVaue; y <= yMaxVaue; y++)
            {
                if (x < 0 || y < 0 || x >= tileController.x_max_value || y >= tileController.y_max_value)
                    continue;

                banTileList.Add(tileController.Map[x, y]);
            }
        }
        

        if (banTileList.Count == null)
            return;

        tileController.banTileList.AddRange(banTileList);

        //금지구역 정보 클래스 생성
        BanAreaData banAreaData = new BanAreaData();
        banAreaData.deleteTime = 10;
        banAreaData.banTileList = banTileList;
        banAreaData.banAreaObj = banAreaObj;
        banAreaDataList.Add(banAreaData);

        //금지구역에 있는 주민 체크
        foreach (var node in banTileList)
        {
            Citizen citizen = tileController.citizenList.Find(data => data.currentTile.pos.x == node.X && data.currentTile.pos.y == node.Y);

            if (citizen != null)
            {
                //citizen.ChangePattern(Ballone.Move);
            }
        }

        SoundManager.Instance.PlaySe(SeEnum.UseBanner);
    }

    void Update()
    {
        BanAreaDeleteCheck();
    }

    void BanAreaDeleteCheck()
    {
        if (banAreaDataList.Count == 0)
            return;

        foreach (var banAreaData in banAreaDataList)
        {
            banAreaData.deleteTime -= Time.deltaTime;

            if (banAreaData.deleteTime <= 0)
            {
                Destroy(banAreaData.banAreaObj);
                tileController.banTileList.RemoveAll(i => banAreaData.banTileList.Contains(i));
                banAreaDataList.Remove(banAreaData);
            }
                
            break;
        }
    }
}

public class BanAreaData
{
    public float deleteTime;
    public List<Node> banTileList = new List<Node>();
    public GameObject banAreaObj;
}
