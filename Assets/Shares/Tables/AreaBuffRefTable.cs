//=========================
//作者：李邵南
//日期：2017/03/07
//用途：读区域Buff静态配置
//=========================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AreaBuffRefTable : AssetTable
{
    public List<AreaBuffRef> infoList = new List<AreaBuffRef>();
}

[System.Serializable]
public class AreaBuffRef
{
    public int id;
    public int sceneID;
    public Vector3 coordinate;
    public int radius;
    public int camp;
    /// <summary>
    /// 进buff提示
    /// </summary>
    public int inTip;
    /// <summary>
    /// 出buff提示
    /// </summary>
    public int outTip;
}