//=========================
//作者：黄洪兴
//日期：2016/04/19
//用途：修行静态配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StyliteRefTable : AssetTable
{
	public List<StyliteRef> infoList = new List<StyliteRef>();
}


[System.Serializable]
public class StyliteRef
{
	/// <summary>
	///ID
	/// </summary>
	public	int id;
	/// <summary>
	/// 境界ID
	/// </summary>
	public int jingJieId;
	/// <summary>
	/// 阶段序号
	/// </summary>
	public int jieDuan;
	/// <summary>
	/// 点的序号
	/// </summary>
	//public int point;
	/// <summary>
	/// 升级该点所需的灵气
	/// </summary>
	public int lingqi;

	/// <summary>
	/// 该飞升的点所增加的最大生命值
	/// </summary>
	public int hp;
	/// <summary>
	/// 该飞升的点所增加的防御值
	/// </summary>
	public int def;
	/// <summary>
	/// 该飞升的点所增加的暴击值
	/// </summary>
	public int cri;
	/// <summary>
	/// 该飞升的点所增加的韧性值
	/// </summary>
	public int tough;
	/// <summary>
	/// 该飞升的点所增加的生命值
	/// </summary>
	public int hit;
	/// <summary>
	/// 该飞升的点所增加的闪避值
	/// </summary>
	public int dod;
	/// <summary>
	/// 该飞升的点所增加的攻击值
	/// </summary>
	public int atk;


    public List<int> attr = new List<int>();
    public void SetAttr()
    {
        attr.Clear();
        attr.Add(hp);
        attr.Add(def);
        attr.Add(cri);
        attr.Add(tough);
        attr.Add(hit);
        attr.Add(dod);
        attr.Add(atk);
    }

}