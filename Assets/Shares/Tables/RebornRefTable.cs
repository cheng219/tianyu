//=========================
//作者：黄洪兴
//日期：2016/8/2
//用途：复活态配置表
//=========================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RebornRefTable : AssetTable
{
    public List<RebornRef> infoList = new List<RebornRef>();
}

[System.Serializable]
public class RebornRef
{


    public int id;

    /// <summary>
    /// 是否能使用物品复活
    /// </summary>
    public bool item_use;

    public bool Special;


}
