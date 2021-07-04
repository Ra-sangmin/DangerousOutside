using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EditorCitizen : MonoBehaviour, IDragHandler, IPointerUpHandler , IPointerClickHandler
{
    public int id;
    public Vector2 pos = Vector2.one * -1;
    public CitizenColor citizen_color = CitizenColor.White;
    public Citizen_Type citizen_Type;
    public EditorStage editorStage;
    [SerializeField] Image image;
    public bool cleanerOn = false;

    public void Citizen_set(Citizen_Type citizen)
    {
        citizen_Type = citizen;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(eventData.position);
        transform.position = new Vector2(pos.x, pos.y);
    }

    public void IDreset(int id)
    {
        this.id = id;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        editorStage.SetSelectCitizen(this);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
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
                EditorTile tempTile = editorStage.tiles.Find(tileData => tileData.pos == new Vector2(x, y));

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
            tile = editorStage.GetResultPos((int)tile.pos.x, (int)tile.pos.y, Vector2.one, id);
            if (tile != null)
            {
                ResetPos(tile);
            }
        }
    }

    public void DeleteOn()
    {
        editorStage.CitizenDestroy(this);
    }

    public void ResetPos(EditorTile tile)
    {
        transform.position = tile.transform.position;
        pos = tile.pos;
    }

    public void SetColor(CitizenColor color)
    {
        this.citizen_color = color;

        Color imageColor = Color.white;

        switch (citizen_color)
        {
            case  CitizenColor.White :
                imageColor = Color.white;
                break;
            case CitizenColor.Red:
                imageColor = Color.red;
                break;
            case CitizenColor.Blue:
                imageColor = Color.blue;
                break;
        }

        image.color = imageColor;
    }

    public string GetName()
    {
        string name = string.Empty;

        switch (citizen_Type)
        {
            case Citizen_Type.Normal:
                name = "일반 주민";
                break;
            case Citizen_Type.Young:
                name = "영 주민";
                break;
            case Citizen_Type.Old:
                name = "노 주민";
                break;
            case Citizen_Type.The:
                name = "그 주민";
                break;
        }

        return string.Format("{0} - {1}", name,id);
    }

}