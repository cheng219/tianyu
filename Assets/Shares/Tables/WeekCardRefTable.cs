//=========================
//作者：黄洪兴
//日期：2016/7/14
//用途：周卡静态配置表
//=========================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeekCardRefTable : AssetTable
{
    public List<WeekCardRef> infoList = new List<WeekCardRef>();
}

[System.Serializable]
public class WeekCardRef
{


    public int id;

    public string name;

    public int price;

    public List<ItemValue> every_day_reward=new List<ItemValue>();

}
