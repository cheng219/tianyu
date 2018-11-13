//=============================
//作者：龙英杰
//日期：2015/10/08
//用途：人物属性type静态配置数据
//=============================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttributeTypeRefTable : AssetTable
{

    public List<AttributeTypeRef> infoList = new List<AttributeTypeRef>();
}

[System.Serializable]
public class AttributeTypeRef
{
    public int id;
    public string stats;
    public string explain;
}
