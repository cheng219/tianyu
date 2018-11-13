//=============================
//作者：龙英杰
//日期：2015/10/08
//用途：常量配置表
//=============================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParaConfigRefTable : AssetTable
{

    public List<ParaConfigRef> infoList = new List<ParaConfigRef>();
}

[System.Serializable]
public class ParaConfigRef
{
    /// <summary>
    /// id
    /// </summary>
    public int id;
    /// <summary>
    /// 常量值
    /// </summary>
    public float val;
    /// <summary>
    /// 常量名
    /// </summary>
    public string name;
    /// <summary>
    /// 
    /// </summary>
    public string des;
}
