//=========================
//作者：黄洪兴
//日期：2016/04/19
//用途：飞升静态配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlyUpRefTable : AssetTable
{
	public List<FlyUpRef> infoList = new List<FlyUpRef>();
}


[System.Serializable]
public class FlyUpRef
{
	/// <summary>
	///境界ID
	/// </summary>
	public	int jingJieId;
	/// <summary>
	/// 境界的名称
	/// </summary>
	public string name;
	/// <summary>
	/// 升到该境界所需要的仙气
	/// </summary>
	public int xianQi;
    /// <summary>
    /// 升到该境界所需要的等级
    /// </summary>
    public int needLev;
	/// <summary>
	/// 到达该境界后所增加的最大生命值
	/// </summary>
	public int hp;
	/// <summary>
	/// 到达该境界后所增加的防御值
	/// </summary>
	public int def;
	/// <summary>
	/// 到达该境界后所增加的暴击值
	/// </summary>
	public int cri;
	/// <summary>
	/// 到达该境界后所增加的韧性值
	/// </summary>
	public int tough;
	/// <summary>
	/// 到达该境界后所增加的生命值
	/// </summary>
	public int hit;
	/// <summary>
	/// 到达该境界后所增加的闪避值
	/// </summary>
	public int dod;
	/// <summary>
	/// 到达该境界后所增加的攻击值
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