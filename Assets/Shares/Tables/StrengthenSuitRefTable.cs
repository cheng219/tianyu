//=========================
//作者：黄洪兴
//日期：2016/03/29
//用途：强化套装静态配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StrengthenSuitRefTable : AssetTable
{
	public List<StrengthenSuitRef> infoList = new List<StrengthenSuitRef>();
}


[System.Serializable]
public class StrengthenSuitRef
{
	public	int id;
    ///// <summary>
    ///// 套装类型
    ///// </summary>
    //public int type;

	//public int lev;
	//public int quality;
	public int str_Lev;
	//public int str_num;
	/// <summary>
	/// 强化到该等级后的属性
	/// </summary>
	//public List<AttributePair> attrs=new List<AttributePair>();
	public List<string> des=new List<string>();
    /// <summary>
    /// 类型 1为套装属性 2 为强化光环
    /// </summary>
    public int type;
    /// <summary>
    /// 特效名称
    /// </summary>
    public string effects;

}