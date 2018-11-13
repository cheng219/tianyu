//=========================
//作者：黄洪兴
//日期：2016/05/20
//用途：读条静态配置
//=========================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SustainRefTable : AssetTable
{

    public List<SustainRef> infoList = new List<SustainRef>();
}

[System.Serializable]
public class SustainRef
{
    public int id;
    public string text;
    public int time;
    public string action;
    public string des;
    public string eff;
}
