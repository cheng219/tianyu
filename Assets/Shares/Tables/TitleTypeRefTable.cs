//=========================
//作者：黄洪兴
//日期：2016/7/7
//用途：称号类型静态配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TitleTypeRefTable : AssetTable
{
    public List<TitleTypeRef> infoList = new List<TitleTypeRef>();
}


[System.Serializable]
public class TitleTypeRef
{
    public int type;
    public List<int> id = new List<int>();


}