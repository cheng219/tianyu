//===========================
//作者：唐源
//日期：2017/2/8
//用途：强化通用静态配置
//===========================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class StrengthenDesRefTable : AssetTable
{
	public List<StrengthenDesRef> infoList = new List<StrengthenDesRef>();
}


[System.Serializable]
public class StrengthenDesRef
{
    public int id;
    public string text;
}
