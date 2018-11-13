//=========================
//作者：黄洪兴
//日期：2016/03/29
//用途：强化属性静态配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StrengthenAttrRefTable : AssetTable
{
	public List<StrengthenAttrRef> infoList = new List<StrengthenAttrRef>();
}


[System.Serializable]
public class StrengthenAttrRef
{
	public	int id;
	/// <summary>
	/// 装备的当前强化等级
	/// </summary>
	public int lev;
	/// <summary>
	/// 装备所属部位
	/// </summary>
	public int position;
	/// <summary>
	/// 强化到该等级后的属性
	/// </summary>
	public List<AttributePair> attrs;

    /// <summary>
    /// 增加战力
    /// </summary>
    public int GS;

}