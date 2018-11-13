//=========================
//作者：唐源
//日期：2017/2/8
//用途：坐骑装备等级上限静态配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MountEquQuailtMaxRefTable : AssetTable
{
    public List<MountEquQuailtMaxRef> infoList = new List<MountEquQuailtMaxRef>();

}

[System.Serializable]
public class MountEquQuailtMaxRef
{
    public int quality;
    public int maxLev;
}