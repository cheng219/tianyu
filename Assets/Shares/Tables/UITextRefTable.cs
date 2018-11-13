//==========================================
//作者：李邵南
//日期：2017/05/23
//用途：繁体文本信息客户端静态配置
//=========================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UItextRefTable:AssetTable
{
    public List<UItextRef> infoList = new List<UItextRef>();
}

[System.Serializable]
public class UItextRef
{
    public string yuanwen;
    public string fanyiwen;
}
