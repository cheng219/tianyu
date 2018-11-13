//=========================
//作者：黄洪兴
//日期：2016/03/05
//用途：宠物技能配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewPetSkillRefTable : AssetTable
{
	public List<NewPetSkillRef> infoList = new List<NewPetSkillRef>();
}


[System.Serializable]
public class NewPetSkillRef
{
	/// <summary>
	/// 编号
	/// </summary>
	public int id;
	/// <summary>
	/// 类型
	/// </summary>
	public int type;
	/// <summary>
	/// 类别
	/// </summary>
	public int kind;
	/// <summary>
	/// 技能名
	/// </summary>
	public string name;
	/// <summary>
	/// 图标特效
	/// </summary>
	public string icon;
	/// <summary>
	/// 技能书
	/// </summary>
	public List<ItemValue> book=new List<ItemValue>();
	/// <summary>
	/// 封印物品
	/// </summary>
	public List<ItemValue> fengYinItem=new List<ItemValue>();
	/// <summary>
	/// 是否为被动技能
	/// </summary>
	public int beidong;
	/// <summary>
	/// 技能提示图标（主动）
	/// </summary>
	public string skillIcon;
	/// <summary>
	/// 释放技能（主动）
	/// </summary>
	public int skillId;
	/// <summary>
	/// 释放几率（主动）
	/// </summary>
	public int chance;
	/// <summary>
	/// 技能说明
	/// </summary>
	public string res;

    /// <summary>
    /// 增加的属性
    /// </summary>
    public List<AttributePair> add_attr = new List<AttributePair>();


    /// <summary>
    /// 品质
    /// </summary>
    public int quality;



}