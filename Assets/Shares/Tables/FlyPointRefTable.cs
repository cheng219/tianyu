//=====================================
//作者：易睿
//日期：2015/7/10
//用途：传送点静态配置
//==========================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlyPointRefTable : AssetTable
{
    public List<FlyPointRef> infoList = new List<FlyPointRef>();
}


[System.Serializable]
public class FlyPointRef
{
    /// <summary>
    /// 索引ID
    /// </summary>
    public int id;
    /// <summary>
    /// 所在场景的ID
    /// </summary>
    public int scene;
    /// <summary>
    /// 所在场景的坐标X
    /// </summary>
    protected int x;
    /// <summary>
    /// 所在场景的坐标Y
    /// </summary>
    protected int y;
    /// <summary>
    /// 所在场景的坐标Z
    /// </summary>
    protected int z;
    /// <summary>
    /// 所在场景的坐标点坐标
    /// </summary>
    public Vector3 sceneVector;
    /// <summary>
    /// 传送点类型
    /// </summary>
    public FlyPointSort sort;
    /// <summary>
    /// 前往场景的ID
    /// </summary>
    public int targetScene;
    /// <summary>
    /// 前往场景的坐标X
    /// </summary>
    protected int targetX;
    /// <summary>
    /// 前往场景的坐标Y
    /// </summary>
    protected int targetY;
    /// <summary>
    /// 前往场景的坐标Z
    /// </summary>
    protected int targetZ;
    /// <summary>
    /// 前往场景的坐标点坐标
    /// </summary>
    public Vector3 targetVector;
    /// <summary>
    /// 需求等级
    /// </summary>
    public int needLv;
    /// <summary>
    /// 朝向
    /// </summary>
    public int direction;
    /// <summary>
    /// 名字
    /// </summary>
    public string name;
    /// <summary>
    /// 类型名字
    /// </summary>
    public string desName;
    /// <summary>
    /// 传送点使用的资源模型
    /// </summary>
    public string resourceModel;
}
/// <summary>
/// 传送点sort
/// </summary>
public enum FlyPointSort
{ 
    /// <summary>
    /// 打开界面
    /// </summary>
    openUI,
    /// <summary>
    /// 传送至指定场景
    /// </summary>
    targetScene,
    /// <summary>
    /// 回当前绑定的主城
    /// </summary>
    recall,
}