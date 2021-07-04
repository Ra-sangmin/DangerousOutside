using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StageAllSaveData : ScriptableObject
{
    public List<StageSaveData> stageList = new List<StageSaveData>();
}

[Serializable]
public class StageSaveData
{
    public int stageId;
    public int minClearTime;
    public int minRedCitizen;
    public int maxRedTile;
    public Vector2 tileSize;
    public List<TileSaveData> tileList = new List<TileSaveData>();
    public List<CitizenSaveData> citizenList = new List<CitizenSaveData>();
    public List<BuildingSaveData> buildingList = new List<BuildingSaveData>();
}

[Serializable]
public class TileSaveData
{
    public Vector2 pos;
    public Tile_Type tile_Type;
}

[Serializable]
public class CitizenSaveData
{
    public int id;
    public CitizenColor citizenColor;
    public Vector2 pos;
    public Citizen_Type citizen_Type;
}

[Serializable]
public class BuildingSaveData
{
    public int id;
    public Vector2 pos;
    public Building_Type buildingType;
    public Vector2 buildSize = Vector2.one * 2;
    public bool isRead = false;
    public int chitizen_normal_num;
    public int chitizen_young_num;
    public int chitizen_old_num;
    public int chitizen_The_num;
}
