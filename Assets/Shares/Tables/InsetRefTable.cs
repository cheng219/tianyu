//=========================
//作者：唐源
//日期：2017/2/8
//用途：镶嵌开启静态配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InsetRefTable : AssetTable
{
    public List<InsetRef> infoList = new List<InsetRef>();
}


[System.Serializable]
public class InsetRef
{
    public int id;
    public int gemType;
    public int openType;
    public int num;
    public string des;
}
