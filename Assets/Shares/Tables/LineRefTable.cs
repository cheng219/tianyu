//=========================
//作者：黄洪兴
//日期：2016/03/28
//用途：无尽试炼线段静态配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LineRefTable : AssetTable
{
	public List<LineRef> infoList = new List<LineRef>();
}


[System.Serializable]
public class LineRef
{
	/// <summary>
	/// 所属章节
	/// </summary>
	public	int chapter;
	/// <summary>
	/// 线段信息集合
	/// </summary>
	public List<Lines> Line=new List<Lines>(); 



}
[System.Serializable]
public class Lines
{
	/// <summary>
	/// 图片名字
	/// </summary>
	public 	string icon;
	/// <summary>
	/// 坐标
	/// </summary>
	public Vector3 coordinate;
	/// <summary>
	/// 旋转角度
	/// </summary>
	public Vector3 rotate;
	public Lines(string _str,Vector3 _coordinate,Vector3 _rotate )
	{
		this.icon = _str;
		this.coordinate = _coordinate;
		this.rotate = _rotate;
	}
	
}