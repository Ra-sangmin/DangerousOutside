using System.Collections;
using System.Collections.Generic;
using Mobcast.Coffee.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EditorBuilding : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerClickHandler
{
    public int id;
    public Vector2 pos = Vector2.one * -1;
    public Vector2 buildSize = Vector2.one;
    public Building_Type object_Build;
    public AtlasImage image;
    public AtlasImage moveImage;
    public Text idText;
    [System.NonSerialized] public EditorStage editorStage;
    [System.NonSerialized] public int chitizen_normal_num;
    [System.NonSerialized] public int chitizen_young_num;
    [System.NonSerialized] public int chitizen_old_num;
    [System.NonSerialized] public int chitizen_The_num;

    public void Build_set(Building_Type Build)
    {
        object_Build = Build;
        string imageName = "";

        switch (Build)
        {
            //집(소)
            case Building_Type.Small:
                buildSize = Vector2.one * 2;
                imageName = "Object_House_s";
                break;
            //집(중)
            case Building_Type.Middle:
                buildSize = Vector2.one * 2;
                imageName = "Object_House_m";
                break;
            //집(대)
            case Building_Type.Big:
                buildSize = Vector2.one * 2;
                imageName = "Object_House_l";
                break;
            //약국
            case Building_Type.BathHouse:
                buildSize = new Vector2(2,3);
                imageName = "Object_Drugstore";
                break;
            //교회
            case Building_Type.Church:
                buildSize = Vector2.one * 2;
                imageName = "Object_Church";
                break;
            //노래방
            case Building_Type.Karaoke:
                buildSize = new Vector2(2, 3);
                imageName = "Object_Sing";
                break;
            //학교
            case Building_Type.School:
                buildSize = new Vector2(2, 3);
                imageName = "Object_School";
                break;
            //회사
            case Building_Type.Company:
                buildSize = new Vector2(2, 3);
                imageName = "Object_Company";
                break;
            default:
                break;
        }

        image.rectTransform.sizeDelta = new Vector2(30 * buildSize.x, 30 * buildSize.y);
        image.spriteName = imageName;
        moveImage.spriteName = imageName;
    }

    public void IDreset(int id)
    {
        this.id = id;
        idText.text = id.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        editorStage.SetSelectBuilding(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        image.gameObject.SetActive(false);
        idText.gameObject.SetActive(false);
        moveImage.gameObject.SetActive(true);
        
        transform.position = Camera.main.ScreenToWorldPoint(eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        image.gameObject.SetActive(true);
        idText.gameObject.SetActive(true);
        moveImage.gameObject.SetActive(false);

        Vector2 world_pos = Camera.main.ScreenToWorldPoint(eventData.position);

        GameObject recycleBinOBJ = GameObject.Find("Delete");
        Vector2 recycleBinOBJPos = recycleBinOBJ.transform.position;
        float recycleBinDistance = Vector2.Distance(recycleBinOBJPos, world_pos);

        if (recycleBinDistance < 100)
        {
            DeleteOn();
            return;
        }

        EditorTile tile = null;
        float distance = 100;

        /*
        for (int x = 0; x < TileController.x_max_value; x++)
        {
            for (int y = 0; y < TileController.y_max_value; y++)
            {
                EditorTile tempTile = editorStage.tiles.Find(tileData => tileData.pos == new Vector2(x,y));

                float tempDistance = Vector2.Distance(world_pos, tempTile.transform.position);

                if (distance > tempDistance)
                {
                    distance = tempDistance;
                    tile = tempTile;
                }
            }
        }
        */

        if (tile != null)
        {
            tile = editorStage.GetResultPos((int)tile.pos.x, (int)tile.pos.y, buildSize , id);
            if (tile != null)
            {
                ResetPos(tile);
            }            
        }
    }

    public void DeleteOn()
    {
        editorStage.BuildDestroy(this);
    }

    public void ResetPos(EditorTile tile)
    {
        transform.position = tile.transform.position;
        pos = tile.pos;
    }

    public string GetBuildName()
    {
        string name = "";

        switch (object_Build)
        {
            case Building_Type.Small:
                name = "집(소)";
                break;
            case Building_Type.Middle:
                name = "집(중)";
                break;
            case Building_Type.Big:
                name = "집(대)";
                break;
            case Building_Type.BathHouse:
                name = "약국";
                break;
            case Building_Type.Church:
                name = "교회";
                break;
            case Building_Type.Karaoke:
                name = "노래방";
                break;
            case Building_Type.School:
                name = "학교";
                break;
            case Building_Type.Company:
                name = "회사";
                break;
        }

        return string.Format("{0} - {1}", name, id);
    }
}
