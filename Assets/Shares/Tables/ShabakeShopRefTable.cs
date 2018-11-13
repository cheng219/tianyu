//=========================
//作者：黄洪兴
//日期：2016/05/25
//用途：攻城战商店静态配置
//=========================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShabakeShopRefTable : AssetTable
{

    public List<ShabakeShopRef> infoList = new List<ShabakeShopRef>();
}

[System.Serializable]
public class ShabakeShopRef
{
    public int id;
    public int page;
    public int item;
    public List<int> price=new List<int>();
    public int num;
    public int Permission;
}
