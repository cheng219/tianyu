//=====================================
//作者：易睿
//日期：2015/7/07
//用途：SceneNPC静态配置
//==========================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneNPCRefTable : AssetTable
{
    public List<SceneNPCRef> infoList = new List<SceneNPCRef>();
}


[System.Serializable]
public class SceneNPCRef
{
    public int id;
    /// <summary>
    /// 泡泡ID
    /// </summary>
    public int bubbleID;
    public bool isHide;
    /// <summary>
    /// 智能NPC
    /// </summary>
    public bool issmartNPC;
    /// <summary>
    /// 对话特写的x值
    /// </summary>
    public float talkx;
    /// <summary>
    /// 对话特写的y值
    /// </summary>
    public float talky;
    /// <summary>
    /// 对话特写的distance值
    /// </summary>
    public float talkDistance;
    public List<int> actionConfig = new List<int>();
}

