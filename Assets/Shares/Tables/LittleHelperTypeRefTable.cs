//=========================
//作者：黄洪兴
//日期：2016/06/4
//用途：小助手类型静态配置
//=========================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LittleHelperTypeRefTable : AssetTable
{

    public List<LittleHelperTypeRef> infoList = new List<LittleHelperTypeRef>();
}

[System.Serializable]
public class LittleHelperTypeRef
{
    public int type;
    public string icon;
    public List<int>id=new List<int>();

}
