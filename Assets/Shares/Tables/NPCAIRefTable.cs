//=====================================
//作者：易睿
//日期：2015/7/07
//用途：NPCAI静态配置
//==========================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCAIRefTable : AssetTable
{
    public List<NPCAIRef> infoList = new List<NPCAIRef>();
}


[System.Serializable]
public class NPCAIRef
{
    public int id;
    public int npcId;
    public int scene;
    public int sceneX;
    public int sceneY;
    public int scenePoint;
    public bool isSmart;
    public ActionType sort;
    public List<int> npcAction = new List<int>();
    public int actionData;
    public int camp;
    public int task;
    public int startStep;
    public int startStepTime;
    public int overStep;
    public int overStepTime;
    public int moveSpeed;
    public float paceSpeed;
    /// <summary>
    /// 类型名字
    /// </summary>
    public string desName;
    public int pointX;
    public int pointY;
}
public enum ActionType
{
    /// <summary>
    /// 无动作
    /// </summary>
    NONE = 0,
    /// <summary>
    /// 单次
    /// </summary>
    ONCE = 1,
    /// <summary>
    /// 随机
    /// </summary>
    RANDOM = 2,
    /// <summary>
    /// 循环
    /// </summary>
    LOOP = 3,
    /// <summary>
    /// 随机间隔
    /// </summary>
    RANDOMINTERVAL = 4,
}

