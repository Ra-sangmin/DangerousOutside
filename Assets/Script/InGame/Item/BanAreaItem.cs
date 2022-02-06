using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class BanAreaItem : BaseItem, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private RectTransform banAreaObj;
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

        tileController.citizenAddEventOn += BanAreaCitizenCheck;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SoundManager.Instance.PlaySe(SeEnum.Touch);

        if (!CheckCost(false))
            return;
      
        banAreaObj = Instantiate(Resources.Load<RectTransform>("InGame/Item/BanAreaObj"),transform);

        //Vector2 worldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        //worldPos.z = 0;
        banAreaObj.anchoredPosition3D = Vector3.zero;
        //cleanManObjRect.anchoredPosition3D = new Vector2(0, 100);

        //Debug.LogWarning(banAreaObj.transform.position);

        //Vector2 worldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        //banAreaObj.transform.position = new Vector3(worldPos.x, worldPos.y);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!CheckCost(false) || banAreaObj == null)
            return;

        Vector2 pos = banAreaObj.anchoredPosition3D;
        pos += eventData.delta;
        banAreaObj.anchoredPosition3D = pos;

        //Vector2 worldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        //banAreaObj.transform.position = worldPos;
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
        //tilePos += new Vector2(26, -21);
        banAreaObj.transform.position = new Vector3(tilePos.x, tilePos.y, banAreaObj.transform.position.z);

        int xMinVaue = (int)tile.pos.x - 1;
        int xMaxVaue = (int)tile.pos.x + 1;
        int yMinVaue = (int)tile.pos.y - 1;
        int yMaxVaue = (int)tile.pos.y + 1;

        
        //금지 구역 지정
        List<Node> banTileList = new List<Node>();
        
        for (int x = xMinVaue; x <= xMaxVaue; x++)
        {
            for (int y = yMinVaue; y <= yMaxVaue; y++)
            {
                if (x < 0 || y < 0 || x >= tileController.x_max_value || y >= tileController.y_max_value)
                    continue;

                banTileList.Add(new Node(x,y));
            }
        }

        if (banTileList.Count == null)
            return;

        tileController.banTileList.AddRange(banTileList);

        //금지구역 정보 클래스 생성
        BanAreaData banAreaData = new BanAreaData();
        banAreaData.deleteTime = 10;
        banAreaData.banTileList = banTileList;
        banAreaData.banAreaObj = banAreaObj.gameObject;
        banAreaData.centerTile = tile;
        banAreaDataList.Add(banAreaData);

        BanAreaCitizenCheck();
    }

    void BanAreaCitizenCheck()
    {
        //금지구역에 있는 주민 체크
        foreach (var banAreaData in banAreaDataList)
        {
            foreach (var node in banAreaData.banTileList)
            {
                List<Citizen> citizenList = tileController.citizenList.Where(data => data.currentTile.pos.x == node.X && data.currentTile.pos.y == node.Y).ToList();

                if (citizenList != null && citizenList.Count != 0)
                {
                    foreach (var citizen in citizenList)
                    {
                        if (citizen != null)
                        {
                            citizen.BanAreaOn(banAreaData.centerTile);
                        }
                        
                    }
                }
            }
            
        }
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

                foreach (var node in banAreaData.banTileList)
                {
                    List<Citizen> citizenList = tileController.citizenList.Where(data => data.currentTile.pos.x == node.X && data.currentTile.pos.y == node.Y).ToList();

                    if (citizenList != null && citizenList.Count != 0)
                    {
                        foreach (var citizen in citizenList)
                        {
                            citizen.BanAreaOff();
                        }
                    }
                }
                BanAreaCitizenCheck();

            }
                
            break;
        }
    }
}

public class BanAreaData
{
    public float deleteTime;
    public List<Node> banTileList = new List<Node>();
    public Tile centerTile;
    public GameObject banAreaObj;
}
