//=========================
//作者：黄洪兴
//日期：2016/7/23
//用途：特殊飞行静态配置表
//=========================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlyExRefTable : AssetTable
{
    public List<FlyExRef> infoList = new List<FlyExRef>();
}

[System.Serializable]
public class FlyExRef
{

    /// <summary>
    /// 请求传送的场景
    /// </summary>
    public int flyScence;
    /// <summary>
    /// 实际传送的场景
    /// </summary>
    public int goScence;
    /// <summary>
    /// 实际传入的坐标
    /// </summary>
    public Vector2 goScenceXZ=Vector2.zero;

}
