//=========================
//作者：黄洪兴
//日期：2016/06/14
//用途：自动使用药品静态配置
//=========================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AutoItemRefTable : AssetTable
{

    public List<AutoItemRef> infoList = new List<AutoItemRef>();
}

[System.Serializable]
public class AutoItemRef
{
    public int level;
    public int hpPrice;
    public int mpPrice;
    public int hpItem;
    public int mpItem;

}
