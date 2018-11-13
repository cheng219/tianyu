//=========================
//作者：黄洪兴
//日期：2016/05/31
//用途：分解静态配置
//=========================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResolveRefTable : AssetTable
{

    public List<ResolveRef> infoList = new List<ResolveRef>();
}

[System.Serializable]
public class ResolveRef
{
    public int id;
    public int quality;
    public int Lev;
    public int position;
    public List<ItemValue> Will_item= new List<ItemValue>();
    public int probability;
    public List<ItemValue> pro_item = new List<ItemValue>();

}
