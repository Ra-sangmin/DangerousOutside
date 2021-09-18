using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[Serializable]
public class ShopAllData : ScriptableObject
{
    public List<ShopBuyIconData> shopDataList = new List<ShopBuyIconData>();

    public List<ShopBuyIconData> GetList(ShopBuyIconType type)
    {
        return shopDataList.Where(data => data.type == type).ToList();
    }

}

[Serializable]
public class ShopBuyIconData
{
    public ShopBuyIconType type = ShopBuyIconType.None;
    public string name;
    public int price;
    public Sprite iconImage;
}
public enum ShopBuyIconType
{
    None,
    Cash,
    Gold,
}
