using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class StageIconDataManager
{
    private static StageIconDataManager ins;
    public static StageIconDataManager Ins
    {
        get
        {
            if (ins == null)
                ins = new StageIconDataManager();
            return ins;
        }
    }

    public StageIconSaveData stageIconSaveData = new StageIconSaveData();
    string stageIconSaveDatakey = "stageIconSaveData";

    public List<StageIconData> stageDataList = new List<StageIconData>();

    //public int playMaxStageID;
    public int maxPage;

    private StageIconDataManager()
    {
        DataSet();
    }

    void DataSet()
    {
        stageDataList = new List<StageIconData>()
        {
            new StageIconData(0, new Vector2(-268.5f,-163.5f) ,0),
            new StageIconData(1, new Vector2(-417f,319f)   ,0),
            new StageIconData(2, new Vector2(0,588)  ,0),
            new StageIconData(3, new Vector2(418 ,320)  ,0),
            new StageIconData(4, new Vector2(269,-161)   ,0),

            new StageIconData(5, new Vector2(-268.5f,-163.5f) ,1),
            new StageIconData(6, new Vector2(-417f,319f)   ,1),
            new StageIconData(7, new Vector2(0,588)  ,1),
            new StageIconData(8, new Vector2(418 ,320)  ,1),
            new StageIconData(9, new Vector2(269,-161)   ,1),
        };
        UserDataSet();

        maxPage = stageDataList.Max(data => data.pageIndex);
        //MaxStageReset();
    }

    void UserDataSet()
    {
        string jsonStr = PlayerPrefs.GetString(stageIconSaveDatakey, string.Empty);
        if (jsonStr != string.Empty)
        {
            stageIconSaveData = JsonUtility.FromJson<StageIconSaveData>(jsonStr);
        }

        foreach (var stageData in stageDataList)
        {
            StageIconDataUserData userData = GetUserData(stageData.id);
            //if (userData.id <= 3)
            //{
            //    userData.playOn = true;
            //    userData.starCount = 1;
            //}
            if (userData == null)
            {
                userData = new StageIconDataUserData();
                userData.id = stageData.id;
            }

            stageIconSaveData.stageUserDataList.Add(userData);
            stageData.stageIconDataUserData = userData;
        }
    }

    void UserDataSave()
    {
        string jsonStr = JsonUtility.ToJson(stageIconSaveData);
        PlayerPrefs.SetString(stageIconSaveDatakey, jsonStr);
    }

    public StageIconDataUserData GetUserData(int stageid)
    {
        return stageIconSaveData.stageUserDataList.Find(data => data.id == stageid);
    }

    /*

    public int MaxStageReset()
    {
        var stagezeroData = stageDataList[0].stageIconDataUserData;

        if (stagezeroData.playOn == false ||
            (stagezeroData.playOn && stagezeroData.starCount ==0))
        {
            //playMaxStageID = -1;
            playMaxStageID = 5;
            return playMaxStageID;
        }

        playMaxStageID = stageDataList
                            .Where(data => data.stageIconDataUserData != null &&
                                           data.stageIconDataUserData.playOn &&
                                           data.stageIconDataUserData.starCount > 0)
                            .Max(data => data.id);

        return playMaxStageID;
    }
    */

    public void PlayOnSet(int stageId)
    {
        StageIconDataUserData data = GetUserData(stageId);

        if (data != null)
        {
            data.playOn = true;
        }
        UserDataSave();
    }

    public void StarCntSet(int stageId, int starCnt)
    {
        StageIconDataUserData data = GetUserData(stageId);

        if (data != null)
        {
            data.starCount = Mathf.Max(starCnt, data.starCount);
        }
        UserDataSave();
    }

    public List<StageIconData> GetPageData(int page)
    {
        return stageDataList
                    .Where(data => data.pageIndex == page)
                    .ToList();
    }
}


[Serializable]
public class StageIconSaveData
{
    public List<StageIconDataUserData> stageUserDataList = new List<StageIconDataUserData>();
}



[Serializable]
public class StageIconDataUserData
{
    public int id = 0;
    public bool playOn = false;
    public int starCount = 0;
    public bool starRewardReceiveOn;

    public StageIconDataUserData() { }
    public StageIconDataUserData(int id, bool playOn, int starCount, bool starRewardReceiveOn)
    {
        this.id = id;
        this.playOn = playOn;
        this.starCount = starCount;
        this.starRewardReceiveOn = starRewardReceiveOn;
    }
}

[Serializable]
public class StageIconData
{
    public int id = 0;
    public StageIconDataUserData stageIconDataUserData;
    public Vector2 posData;
    public int pageIndex;

    public StageIconData() { }
    public StageIconData(int id, Vector2 posData, int pageIndex)
    {
        this.id = id;
        this.posData = posData;
        this.pageIndex = pageIndex;
    }
}


