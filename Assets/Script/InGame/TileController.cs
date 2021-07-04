using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;

public class TileController : MonoBehaviour
{
    [SerializeField] StageAllSaveData stageAllSaveData;

    //public const int x_max_value = 18;
    //public const int y_max_value = 30;

    public int x_max_value = 15;
    public int y_max_value = 23;

    public Node[,] Map;
    public bool[,] wall;

    public Tile[,] tiles;

    public int selectStageId = -1;

    [System.NonSerialized] public StageSaveData stageSaveData;
    [System.NonSerialized] public List<Citizen> citizenList = new List<Citizen>();
    [System.NonSerialized] public List<Building> buildingList = new List<Building>();
    [System.NonSerialized] public List<Node> banTileList = new List<Node>();

    [SerializeField] CityhallController cityhallController;
    [SerializeField] ForceGoHomeItem forceGoHomeItem;
    [SerializeField] Slider redTilSlider;
    [SerializeField] Text redTilText;

    public UnityAction gameWinOn;
    public UnityAction gameFailOn;

    public RectTransform tileParant;
    public RectTransform builingParant;
    public RectTransform citizenParant;

    bool isEnd;

    [SerializeField] List<Image> bgList = new List<Image>();
    [SerializeField] List<RectTransform> patternList = new List<RectTransform>();

    // Start is called before the first frame update
    void Start()
    {
        isEnd = false;
        GameManager.Ins.tileController = this;
    }

    public void Init()
    {
        if (stageAllSaveData == null || GameManager.Ins.tutorialOn)
        {
            return;
        }

        if (selectStageId == -1)
        {
            selectStageId = GameManager.Ins.selectStageId;
        }

        stageSaveData = stageAllSaveData.stageList.Find(stageData => stageData.stageId == selectStageId);

        StartCoroutine(TileCreate());

        SetBG();
    }

    void SetBG()
    {
        int chapterId = (selectStageId / 5);

        bgList[chapterId].gameObject.SetActive(true);
        patternList[chapterId].gameObject.SetActive(true);
    }

    Vector2 GetTileSize()
    {
        Vector2 tileSize = Vector2.one;

        if (selectStageId < 5)
        {
            tileSize = new Vector2(5, 5);
        }
        else if (selectStageId < 10)
        {
            tileSize = new Vector2(6, 6);
        }

        return tileSize;
    }


    IEnumerator TileCreate()
    {
        Vector2 parantSize = tileParant.sizeDelta;
        GridLayoutGroup gridLayoutGroup = tileParant.GetComponent<GridLayoutGroup>();

        float xGridValue = parantSize.x / stageSaveData.tileSize.x;
        float yGridValue = parantSize.y / stageSaveData.tileSize.y;

        gridLayoutGroup.cellSize = new Vector2(xGridValue, yGridValue);

        x_max_value = (int)stageSaveData.tileSize.x;
        y_max_value = (int)stageSaveData.tileSize.y;

        tiles = new Tile[x_max_value, y_max_value];
        Map = new Node[x_max_value, y_max_value];
        //wall = new bool[x_max_value, y_max_value];

        foreach (var tile in stageSaveData.tileList)
        {
           // TileCreate(tile);
        }

        //int tileCnt = (int)(tileSize.x * tileSize.y);

//        stageSaveData.tileList

        
        for (int y = (int)stageSaveData.tileSize.y-1; y >= 0; y--)
        {
            for (int x = 0; x < stageSaveData.tileSize.x; x++)
            {
                TileSaveData tile = stageSaveData.tileList.FirstOrDefault(data => data.pos.x == x && data.pos.y == y);

                if (tile != null)
                {
                    TileCreate(tile);
                }
            }
        }

        yield return new WaitForEndOfFrame();

        foreach (var citizen in stageSaveData.citizenList)
        {
            CitizenCreate(citizen);
        }

        foreach (var building in stageSaveData.buildingList)
        {
            BuildingCreate(building);
        }

        if (cityhallController != null)
        {
            cityhallController.SetBuilding(buildingList);
        }

        redTilSlider.gameObject.SetActive(true);

        if (GameManager.Ins.selectStageId != 4)
        {
            RedTileCheck();
        }

        /*
        yield return new WaitForEndOfFrame();

        BuildingSaveData buildingSaveData = new BuildingSaveData();
        buildingSaveData.buildingType = Building_Type.Small;
        buildingSaveData.pos = new Vector2(1, 1);
        buildingSaveData.isRead = true;
        buildingSaveData.chitizen_normal_num = 5;

        Building building = BuildingCreate(buildingSaveData);
        ((Apartment)building).currentFoodCnt = 8;

        BuildingSaveData buildingSaveData2 = new BuildingSaveData();
        buildingSaveData2.buildingType = Building_Type.Small;
        buildingSaveData2.pos = new Vector2(3, 3);
        buildingSaveData2.isRead = true;
        buildingSaveData2.chitizen_normal_num = 5;

        BuildingCreate(buildingSaveData2);

        if (cityhallController != null)
        {
            cityhallController.SetBuilding(buildingList);
        }

        //gridLayoutGroup
        */
    }

    public void TileCreate(TileSaveData tile)
    {
        if (tile.pos.x >= x_max_value || tile.pos.y >= y_max_value)
            return;
        
        Tile tileobj = Instantiate(Resources.Load<Tile>("InGame/Tile3"), tileParant.transform);
        tileobj.pos = tile.pos;
        //tileobj.transform.localPosition = new Vector2(tileobj.pos.x * 55, tileobj.pos.y * 43);
        //tileobj.transform.localPosition = new Vector2(tileobj.pos.x * 67, tileobj.pos.y * 55);
        tileobj.TileChange(tile.tile_Type, true);
        tileobj.tileChangeOn = RedTileCheck;
        tiles[(int)tile.pos.x, (int)tile.pos.y] = tileobj;

        SetNodeData(tileobj);
    }

    public Citizen CitizenCreate(CitizenSaveData citizen)
    {
        Citizen_Type type = citizen.citizen_Type;

        string citizenPath = "InGame/Citizen/";
        switch (type)
        {
            case Citizen_Type.Normal:
                citizenPath += "Normal_Citizen"; break;
            case Citizen_Type.Young:
                citizenPath += "Young_Citizen"; break;
            case Citizen_Type.Old:
                citizenPath += "Old_Citizen"; break;
            case Citizen_Type.The:
                citizenPath += "The_Citizen"; break;
        }

        Citizen citizenOBJ = Instantiate(Resources.Load<Citizen>(citizenPath), citizenParant.transform);
        Tile editorTile = tiles[(int)citizen.pos.x, (int)citizen.pos.y];

        citizenOBJ.Init(this, editorTile);
        citizenOBJ.ChangeColor(citizen.citizenColor);

        citizenList.Add(citizenOBJ);

        return citizenOBJ;
    }

    public void OutCitizenCreate(Citizen citizen , Building building)
    {
        Citizen_Type type = citizen.citizen_Type;

        string citizenPath = "InGame/Citizen/";
        switch (type)
        {
            case Citizen_Type.Normal:
                citizenPath += "Normal_Citizen"; break;
            case Citizen_Type.Young:
                citizenPath += "Young_Citizen"; break;
            case Citizen_Type.Old:
                citizenPath += "Old_Citizen"; break;
            case Citizen_Type.The:
                citizenPath += "The_Citizen"; break;
        }

        Citizen citizenOBJ = Instantiate(Resources.Load<Citizen>(citizenPath), citizenParant.transform);
        citizenOBJ.home = citizen.home;

        if (building.isRead)
        {
            citizen.citizen_color = CitizenColor.Red;
        }

        Tile tile = tiles[(int)building.tile.pos.x, (int)building.tile.pos.y-1];

        citizenOBJ.Init(this, tile);
        citizenOBJ.ChangeColor(citizen.citizen_color);

        citizenList.Add(citizenOBJ);
    }

    public Building BuildingCreate(BuildingSaveData building)
    {
        string building_Type = null;

        BuildingManager.Instance.arrayList.Clear();

        switch (building.buildingType)
        {
            case Building_Type.Small:
                building_Type = "Apartment_Small";
                break;

            case Building_Type.Middle:
                building_Type = "Apartment_Middle";
                break;

            case Building_Type.Big:
                building_Type = "Apartment_Big";
                break;

            case Building_Type.BathHouse:
                building_Type = "BathHouse";
                break;

            case Building_Type.Church:
                building_Type = "Church";
                break;

            case Building_Type.Karaoke:
                building_Type = "Karaoke";
                break;

            case Building_Type.School:
                building_Type = "School";
                break;

            case Building_Type.Company:
                building_Type = "Company";
                break;
        }

        if (building_Type == null)
            return null ;

        GameObject editorBuilding = Instantiate(Resources.Load<GameObject>("InGame/Building/" + building_Type), builingParant.transform);
        Tile tile = tiles[(int)building.pos.x, (int)building.pos.y];

        Building buildingObj = editorBuilding.GetComponent<Building>();
        buildingObj.Init(building);
        buildingObj.ResetPos(tile);
        buildingObj.isRead = building.isRead;
        buildingList.Add(buildingObj);
        
        SetNodeData(building);

        return buildingObj;
    }

    void SetNodeData(Tile tileobj)
    {
        int x = (int)tileobj.pos.x;
        int y = (int)tileobj.pos.y;

        // 맵이 없는 부분
        bool wallOn = (tileobj.tile_Type == Tile_Type.None);

        if (wall != null)
        {
            wall[x, y] = wallOn;
        }
        
    }

    void SetNodeData(BuildingSaveData building)
    {
        //건물 사이즈 별로 좌표값 가져옴
        for (int x = 0; x < (int)building.buildSize.x; x++)
        {
            for (int y = 0; y < (int)building.buildSize.y; y++)
            {
                if (x == 0 && y == 0 || wall == null)
                    continue;

                int posX = (int)building.pos.x + x;
                int posY = (int)building.pos.y + y;

                wall[posX, posY] = true;
            }
        }
    }

    public void RedTileCheck()
    {
        if (GameManager.Ins.tutorialOn)
        {
            return;
        }

        int redTileCnt = 0;

        for (int x = 0; x < x_max_value; x++)
        {
            for (int y = 0; y < y_max_value; y++)
            {
                if (tiles[x, y] != null && tiles[x, y].tile_Type == Tile_Type.Red)
                {
                    redTileCnt++;
                }
            }
        }

        float maxValue = stageSaveData.maxRedTile;
        float sliderValue = redTileCnt / maxValue;
        redTilSlider.DOValue(sliderValue, 0.5f);

        redTilText.text = string.Format("남은 감염지역 : {0}", redTileCnt) .ToString();

        if (redTileCnt == 0)
        {
            if (gameWinOn != null && isEnd == false)
            {
                isEnd = true;
                gameWinOn();
            }
        }
        else if (redTileCnt >= maxValue)
        {
            if (gameFailOn != null && isEnd == false)
            {
                isEnd = true;
                gameFailOn();
            }
        }
    }

    public bool IsWall(Vector2 pos)
    {
        if (wall == null)
        {
            return false;
        }
        return wall[(int)pos.x, (int)pos.y];
    }

    public Tile GetTile(Vector2 pos)
    {
        if (pos.x < 0 || pos.x >= x_max_value || pos.y < 0 || pos.y >= y_max_value)
            return null;
        
        return tiles[(int)pos.x, (int)pos.y];
    }
}