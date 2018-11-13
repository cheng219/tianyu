//=========================
//作者：邓成
//日期：2016/05/19
//用途：城内商店静态配置
//=========================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CityShopRefTable : AssetTable
{

    public List<CityShopRef> infoList = new List<CityShopRef>();
}

[System.Serializable]
public class CityShopRef
{
    public int id;
    public int page;
    public int itemID;
    public int price;
    public int priceType;
    public int limitCount;
    public int permission;
}
