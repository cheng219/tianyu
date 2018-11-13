//=========================
//作者：黄洪兴
//日期：2016/3/10
//用途：错误报告静态配置表
//=========================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ErrorCodeRefTable : AssetTable
{
	public List<ErrorCodeRef> infoList = new List<ErrorCodeRef>();
}

[System.Serializable]
public class ErrorCodeRef
{


	public int id;
	public string svr_name;
	public string des;
	public int sort;
	public string txt;
	public int size;
	public int showBg;
	public int stayTime;
	public int moveTime;
	public int font;
	public Vector3 flowStartV3;
	public string showType;
	public Vector3 flowEndV3;
	public float holdTime;
	public int acceleration;
	public Vector3 flowLocaV3;

	//	public 	List<AttributePair> attrs = new List<AttributePair> ();


}
