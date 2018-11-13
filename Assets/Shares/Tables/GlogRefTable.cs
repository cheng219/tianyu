//==========================
//作者：黄洪兴
//日期：2016/4/15
//用途：日志文本静态配置
//==========================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GlogRefTable : AssetTable
{
	public List<GlogRef> infoList = new List<GlogRef>();
}


[System.Serializable]
public class GlogRef
{
	/// <summary>
	/// id
	/// </summary>
	public int id;
	/// <summary>
	/// 文本
	/// </summary>
	public string txt;

}

