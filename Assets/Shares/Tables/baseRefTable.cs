//=========================
//作者：李邵南
//日期：2017/04/06
//用途：登陆红利后台静态配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class baceRefTable : AssetTable
{

    public List<baceRef> infoList = new List<baceRef>();
}


[System.Serializable]
public class baceRef
{
    /// <summary>
    /// ID
    /// </summary>
    public int ID;
    /// <summary>
    /// 激活所需元宝数
    /// </summary>
    public int active_num;
}