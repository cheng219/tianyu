//=========================
//作者：黄洪兴
//日期：2016/6/24
//用途：CDKEY静态配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CDKeyRefTable : AssetTable
{
    public List<CDKeyRef> infoList = new List<CDKeyRef>();
}


[System.Serializable]
public class CDKeyRef
{

    public int id;
    public string name;
    public string icon;
    public List<ItemValue> item = new List<ItemValue>();


  


}