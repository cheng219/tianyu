//=========================
//作者：黄洪兴
//日期：2016/06/07
//用途：套装属性静态配置
//=========================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EquipmentSetRefTable : AssetTable
{

    public List<EquipmentSetRef> infoList = new List<EquipmentSetRef>();
}

[System.Serializable]
public class EquipmentSetRef
{
    public int id;
    public int type;
    public int lev;
    public int quality;
    public AttributePair three_attr;
    public AttributePair six_attr;
    public string des;
}
