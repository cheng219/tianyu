//=========================
//作者：黄洪兴
//日期：2016/06/1
//用途：星星类型静态配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StarTypeRefTable : AssetTable
{
    public List<StarTypeRef> infoList = new List<StarTypeRef>();
}


[System.Serializable]
public class StarTypeRef
{
    public int level;
    public string icon;
    public string color;
   

}


