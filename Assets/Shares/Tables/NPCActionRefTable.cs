//=====================================
//作者：易睿
//日期：2015/7/07
//用途：NPCAction静态配置
//==========================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCActionRefTable : AssetTable
{
    public List<NPCActionRef> infoList = new List<NPCActionRef>();
}


[System.Serializable]
public class NPCActionRef
{
    public int actionId;
    public TypeAction sort;
    public TargetType targetType;
    public int targetConfigId;
    public int targetScene;
    public Vector3 targetPos;
    public int targetPoint;
}
public enum TypeAction
{ 
    MOVE=1,
    LEAD=2,
    FOLLOW=3,
    AVOID =4,
}
public enum TargetType
{ 
    NONE = 0,
    MAINPLAYER = 1,
    MONSTER = 2,
    NPC = 3,
    SCENEITEM = 4,
}
