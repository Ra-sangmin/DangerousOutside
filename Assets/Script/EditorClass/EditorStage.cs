using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorStage : MonoBehaviour
{
    public List<EditorTile> tiles = new List<EditorTile>();
    public List<EditorCitizen> citizenList = new List<EditorCitizen>();
    public List<EditorBuilding> buildingList = new List<EditorBuilding>();
    public int stage_ID;
    public Tile_Type select_Tile_Type;
    public EditorBuilding select_Build_Obj;
    public EditorCitizen select_Citizen_Obj;

    public int minClearTime = 120;
    public int minRedCitizen = 10;
    public int maxRedTile = 10;

    private void Start()
    {
        /*
        for (int x = 0; x < TileController.x_max_value; x++)
        {
            for (int y = 0; y < TileController.y_max_value; y++)
            {
                EditorTile tileobj = Instantiate(Resources.Load<EditorTile>("EditorObj/EditorTile"), transform);
                tileobj.pos = new Vector2(x, y);
                //tileobj.transform.localPosition = new Vector2(40f * x, 33.3f * y);
                tileobj.transform.localPosition = new Vector2(55f * x, 40f * y);
                tileobj.TileChange(Tile_Type.White);
                tileobj.Tile_mng = this;
                if (y < 4 && (x <= 2 || x >= 12))
                {
                    tileobj.TileChange(Tile_Type.None, true);
                }
                //클릭시 호출 이벤트 등록
                tileobj.clickEvent += ChangeTileType;

                tiles.Add(tileobj);
            }
        }
        */

        transform.parent.GetComponent<Editor_Mnr>().ChangeTileOn();
    }

    public EditorCitizen Citizens_Crt(Citizen_Type citizen)
    {
        EditorCitizen citizen_obj = Instantiate(Resources.Load<EditorCitizen>("EditorObj/EditorCitizen"), transform);
        citizen_obj.Citizen_set(citizen);
        citizen_obj.editorStage = this;
        citizenList.Add(citizen_obj);
        CitizenIDReset();
        return citizen_obj;
    }

    public void CitizenIDReset()
    {
        for (int i = 0; i < citizenList.Count; i++)
        {
            citizenList[i].IDreset(i);
        }
    }

    public EditorBuilding Build_Crt(Building_Type Build)
    {
        EditorBuilding build_obj = Instantiate(Resources.Load<EditorBuilding>("EditorObj/EditorBuild"), transform);
        build_obj.Build_set(Build);
        build_obj.editorStage = this;
        buildingList.Add(build_obj);
        BuildIDReset();

        return build_obj;
    }

    public void BuildIDReset()
    {
        for (int i = 0; i < buildingList.Count; i++)
        {
            buildingList[i].IDreset(i);
        }
    }

    public EditorTile GetResultPos(int x, int y, Vector2 size, int id)
    {
        //현재 좌표
        Vector2 currentPos = new Vector2(x, y);
        Vector2 resultPos = FindResultPos(currentPos, size, id);
        return tiles.Find(tileData => tileData.pos == resultPos);
    }

    private Vector2 FindResultPos(Vector2 currentPos, Vector2 size, int id)
    {
        /*
        //int width = TileController.x_max_value;
        //int height = TileController.y_max_value;
        //bool[,] map = new bool[width, height];

        Vector2 resultPos = currentPos;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (y < 4)
                {
                    if (x <= 4 || x >= 13)
                    {
                        map[x, y] = true;
                    }
                }
            }
        }
        
        foreach (var citizen in citizenList)
        {
            int posX = (int)citizen.pos.x;
            int posY = (int)citizen.pos.y;

            if (posX < 0 || posY < 0 || posX >= width || posY >= height)
                continue;

            map[posX, posY] = true;
        }

        foreach (var building in buildingList)
        {
            if (building.pos == Vector2.one * -1 || (size != Vector2.one && building.id == id))
                continue;

            for (int x = 0; x < (int)building.buildSize.x; x++)
            {
                for (int y = 0; y < (int)building.buildSize.y; y++)
                {
                    int posX = (int)building.pos.x + x;
                    int posY = (int)building.pos.y + y;

                    if (posX < 0 || posY < 0 || posX >= width || posY >= height)
                        continue;

                    map[posX, posY] = true;
                }
            }
        }

        while (resultPos.x >= 0)
        {
            bool readyOn = true;

            for (int x = 0; x < (int)size.x; x++)
            {
                for (int y = 0; y < (int)size.y; y++)
                {
                    int posX = (int)resultPos.x + x;
                    int posY = (int)resultPos.y + y;

                    if (posX < 0 || posY < 0 || posX >= width || posY >= height)
                    {
                        readyOn = false;
                        continue;
                    }

                    if (map[posX, posY] == true)
                    {
                        readyOn = false;
                    }
                }
            }

            if (readyOn)
            {
                return resultPos;
            }
            else
            {
                resultPos.x--;
            }


        }

        resultPos.x = 0;

        while (resultPos.y >= 0)
        {
            bool readyOn = true;

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    int posX = (int)resultPos.x + x;
                    int posY = (int)resultPos.y + y;

                    if (posX < 0 || posY < 0 || posX >= width || posY >= height)
                    {
                        readyOn = false;
                        continue;
                    }

                    if (map[posX, posY] == true)
                    {
                        readyOn = false;
                    }
                }
            }

            if (readyOn)
            {
                return resultPos;
            }
            else
            {
                resultPos.y--;
            }
        }
        */
        return Vector2.zero;
    }

    public void SetSelectTileType(Tile_Type tile_Type)
    {
        this.select_Tile_Type = tile_Type;
    }

    public void CitizenDestroy(EditorCitizen editorCitizen)
    {
        citizenList.Remove(editorCitizen);
        Destroy(editorCitizen.gameObject);
        transform.parent.GetComponent<Editor_Mnr>().ResetCitizensCnt();
        CitizenIDReset();
    }
    public void BuildDestroy(EditorBuilding editorBuilding)
    {
        buildingList.Remove(editorBuilding);
        Destroy(editorBuilding.gameObject);
        BuildIDReset();
    }

    /// <summary>
    /// 타일의 타입변경
    /// </summary>
    /// <param name="tile_Type"></param>
    public void ChangeTileType(EditorTile tile)
    {
        tile.TileChange(select_Tile_Type);
        transform.parent.GetComponent<Editor_Mnr>().ChangeTileOn();
    }

    public void SetSelectBuilding(EditorBuilding building)
    {
        this.select_Build_Obj = building;
        transform.parent.GetComponent<Editor_Mnr>().CurrentBuildChange();
    }
    public void SetSelectCitizen(EditorCitizen citizen)
    {
        this.select_Citizen_Obj = citizen;
        transform.parent.GetComponent<Editor_Mnr>().CurrentCitizenChange();
    }

}
