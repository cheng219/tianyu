//======================================================
//作者:唐源
//日期:2017/3/1
//用途:UI跳转
//======================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class UISkipRefTable : AssetTable
{
    public List<UISkipRef> infoList = new List<UISkipRef>();
}
[System.Serializable]
public class UISkipRef
{
    /// <summary>
    /// id
    /// </summary>
    public int id;
    /// <summary>
    /// 名字
    /// </summary>
    public string name;
    /// <summary>
    /// 类型
    /// </summary>
    public string type;
    /// <summary>
    /// UI层级
    /// </summary>
    public int Level;
    /// <summary>
    /// 分页数
    /// </summary>
    public int num;
}
