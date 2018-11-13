//=========================
//作者：黄洪兴
//日期：2016/06/4
//用途：小助手静态配置
//=========================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LittleHelperRefTable : AssetTable
{

    public List<LittleHelperRef> infoList = new List<LittleHelperRef>();
}

[System.Serializable]
public class LittleHelperRef
{
    public int id;
    public int type;
    public string name;
    public int star;
    public int buttontype;
    public int npcId;
    public string uiType;
    public List<int> TaskId;
}
