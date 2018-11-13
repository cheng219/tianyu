//==============================
//作者：黄洪兴
//日期：2016/3/22
//用途：坐骑属性表静态配置
//==============================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RidePropertyRefTable : AssetTable
{
	public List<RidePropertyRef> infoList = new List<RidePropertyRef>();
}


[System.Serializable]
public class RidePropertyRef
{

	/// <summary>
	/// 等级
	/// </summary>
	public int level;
	/// <summary>
	/// 等级的名字
	/// </summary>
	public string name;
	/// <summary>
	/// 升级所需要
	/// </summary>
	public List<ItemValue> item=new List<ItemValue>();
	/// <summary>
	/// 升级成功率 万分比
	/// </summary>
	public int chance;
	/// <summary>
	/// 速度
	/// </summary>
	public int speed;
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
    public int MountId;

    public List<int> attr = new List<int>();
    public void SetAttr()
    {
        attr.Clear();
        attr.Add(speed);
        attr.Add(hp);
        attr.Add(att);
        attr.Add(def);
        attr.Add(cri);
        attr.Add(duc);
        attr.Add(hit);
        attr.Add(dge);
    }


}

