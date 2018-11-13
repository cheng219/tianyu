//=========================
//作者：黄洪兴
//日期：2016/04/26
//用途：Boss挑战静态配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossRefTable : AssetTable
{
	public List<BossRef> infoList = new List<BossRef>();
}


[System.Serializable]
public class BossRef
{
    public int monsterId;
	public int type;
	public string res;
	public string wayres;
	public string tip;

	public List<int> item=new List<int>();

	public int needLevel;

    public int sceneID;
    public int sceneX;
    public int sceneY;
    /// <summary>
    /// Boss刷新提示
    /// </summary>
    public string bossTip;
}


