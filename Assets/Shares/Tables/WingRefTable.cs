//==============================
//作者：黄洪兴
//日期：2016/3/24
//用途：翅膀表静态配置
//==============================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WingRefTable : AssetTable
{
	public List<WingRefEty> infoList = new List<WingRefEty>();
}

[System.Serializable]
public class WingRefEty
{
	public int id;
	public int lev;
	public WingRef getWingRef(int lev)
	{

		for (int i = 0; i < wingRefList.Count; i++) {
			if (wingRefList [i].lev == lev) {
				return wingRefList [i];
			}
		}
		return null;
	}
	public WingRefEty(WingRef info)
	{
		id = info.id;
		lev = info.lev;
		wingRefList.Add (info);
	}
	public List<WingRef> wingRefList = new List<WingRef>();

}

[System.Serializable]
public class WingRef
{
	/// <summary>
	/// 表编号
	/// </summary>
	public int id;
	/// <summary>
	/// 翅膀等级
	/// </summary>
	public int lev;
	/// <summary>
	/// 翅膀类型
	/// </summary>
	public int type;
	/// <summary>
	/// 翅膀名字
	/// </summary>
	public string name;
	/// <summary>
	/// 翅膀附加属性
	/// </summary>
	public List<ItemValue> property_list = new List<ItemValue>();
	/// <summary>
	/// 翅膀附加技能
	/// </summary>
    public SkillLKey passivity_skill;
	/// <summary>
	/// 每级升级需要的经验
	/// </summary>
	public int exp;
	/// <summary>
	/// 淬炼消耗	
	/// </summary>
	public List<ItemValue> up_need_item=new List<ItemValue>();
	/// <summary>
	/// 淬炼一次增加的经验
	/// </summary>
	public List<int> add_exp;
	/// <summary>
	/// 开启要求1
	/// </summary>
	public List<int> condition_1;
	/// <summary>
	/// 开启要求2
	/// </summary>
	public List<int> condition_2;
	/// <summary>
	/// 使用等级上限 0表示没有使用上限
	/// </summary>
	public int use_lev;
	/// <summary>
	/// 模型名称
	/// </summary>
	public string model;

	/// <summary>
	/// 翅膀未激活时UI上给的文本提示
	/// </summary>
	public string des;
	/// <summary>
	/// 翅膀的技能文字提示
	/// </summary>
	public string skill_des;
	/// <summary>
	/// 翅膀未激活时的技能文字提示
	/// </summary>
	public string not_active_skill;
	/// <summary>
	/// 每个等级对应的物品图标
	/// </summary>
	public int itemui;

    public float progress_exp;



    /// <summary>
    /// 翅膀特效
    /// </summary>
    public List<WingEffect> wingEffectList = new List<WingEffect>();

}
[System.Serializable]
public class SkillLKey
{
  public   int skillid;
   public  int skilllev;
   public SkillLKey(int _id,int _lev)
   {
       skillid = _id;
       skilllev = _lev;

   }

}

[System.Serializable]
public class WingEffect
{
    public string boneName;
    public string effectName;


    public WingEffect(string _boneName, string _effectName)
    {
        this.boneName = _boneName;
        this.effectName = _effectName;

    }

    public WingEffect()
    {
    }

}

