using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tile : MonoBehaviour
{
    public Vector2 pos;
    public Tile_Type tile_Type = Tile_Type.White;
    public Animator anim;
    public UnityAction tileChangeOn;

    public void TileChange(Tile_Type type, bool setOn = false)
    {
        if (type == Tile_Type.None && setOn == false)
            return;

        Tile_Type beforeType = tile_Type;
        tile_Type = type;

        if (beforeType == Tile_Type.None && tile_Type != Tile_Type.White)
        {
            beforeType = Tile_Type.White;
        }

        string animName = string.Format("Tiles200x200_{0}2{1}", GetTypeName(beforeType), GetTypeName(type));

        if (anim != null)
        {
            anim.Play(animName);

            if (type == Tile_Type.None)
            {
                anim.gameObject.SetActive(false);
            }
        }

        if (tileChangeOn != null)
        {
            tileChangeOn();
        }
    }

    public string GetTypeName(Tile_Type type)
    {
        switch (type)
        {
            case Tile_Type.None:
                return "n";
            case Tile_Type.White:
                return "w";
            case Tile_Type.Red:
                return "r";
            case Tile_Type.Blue:
                return "b";
        }

        return "n";

    }

}
