using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager
{
    private static ItemManager ins;
    public static ItemManager Ins
    {
        get
        {
            if (ins == null)
                ins = new ItemManager();
            return ins;
        }
    }

    public List<bool> itemOnDataList = new List<bool>();

    string saveKey = "itemOn";

    private ItemManager()
    {
        PlayerPrefs.DeleteAll();

        DataSet();
    }

    void DataSet()
    {
        itemOnDataList = new List<bool>();

        foreach (ItemState itemState in Enum.GetValues(typeof(ItemState)))
        {
            string key = string.Format("{0}_{1}", saveKey, itemState);
            bool itemOnValue = PlayerPrefs.GetInt(key,0) == 1;

            if (itemState == ItemState.Cityhall)
            {
                itemOnValue = true;
            }

            itemOnDataList.Add(itemOnValue);
        }
    }

    public bool GetItemOnData(ItemState itemState)
    {
        return itemOnDataList[(int)itemState];
    }

    public void SaveItemOnData(ItemState itemState)
    {
        itemOnDataList[(int)itemState] = true;
        string key = string.Format("{0}_{1}", saveKey, itemState);
        int saveValue = itemOnDataList[(int)itemState] ? 1 : 0;
        PlayerPrefs.SetInt(key, saveValue);
    }
}
