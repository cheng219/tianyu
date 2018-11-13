//==============================
//作者：黄洪兴
//日期：2016/3/22
//用途：坐骑幻化表静态配置
//==============================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkinPropertyRefTable : AssetTable
{
	public List<SkinPropertyRef> infoList = new List<SkinPropertyRef>();
}


[System.Serializable]
public class SkinPropertyRef
{
	/// <summary>
	/// 表编号
	/// </summary>
	public int id;
	/// <summary>
	/// 等级
	/// </summary>
	public int level;
	/// <summary>
	/// 等级的名字
	/// </summary>
	public string name;
	/// <summary>
	/// 经验
	/// </summary>
	public int exp;
	/// <summary>
	/// 生命
	/// </summary>
	public int hp;
	/// <summary>
	/// 攻击
	/// </summary>
	public int att;
	/// <summary>
	/// 防御
	/// </summary>
	public int def;
	/// <summary>
	/// 暴击
	/// </summary>
	public int cri;
	/// <summary>
	/// 韧性
	/// </summary>
	public int duc;
	/// <summary>
	/// 命中
	/// </summary>
	public int hit;
	/// <summary>
	/// 闪避
	/// </summary>
	public int dge;
	/// <summary>
	/// 战力
	/// </summary>
	public int gs;


    public List<int> attr = new List<int>();
    public void SetAttr()
    {
        attr.Clear();
        attr.Add(hp);
        attr.Add(att);
        attr.Add(def);
        attr.Add(cri);
        attr.Add(hit);
        attr.Add(dge);
        attr.Add(duc);
    }

}

