//=========================
//作者：黄洪兴
//日期：2016/3/10
//用途：转生静态配置表
//=========================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SuperLifeRefTable : AssetTable
{
	public List<SuperLifeRef> infoList = new List<SuperLifeRef>();
}

[System.Serializable]
public class SuperLifeRef
{

	/// <summary>
	/// 转生次数
	/// </summary>
	public int id;
	/// <summary>
	/// 转生需要的修为
	/// </summary>
	public long superExp;
	/// <summary>
	/// 增加的攻击下限
	/// </summary>
	public int atk1;
	/// <summary>
	/// 增加的攻击上限
	/// </summary>
	public int atk2;
	/// <summary>
	/// 防御下限
	/// </summary>
	public int def1;
	/// <summary>
	/// 防御上限
	/// </summary>
	public int def2;
	/// <summary>
	/// 增加的暴击
	/// </summary>
	public int critical;
	/// <summary>
	/// 增加的韧性
	/// </summary>
	public int tough;
	/// <summary>
	/// 增加的命中
	/// </summary>
	public int hit;
	/// <summary>
	/// 增加的闪避
	/// </summary>
	public int dodge;
	/// <summary>
	/// 增加的法力
	/// </summary>
	public int mana;
	/// <summary>
	/// 增加的生命值
	/// </summary>
	public int health;
	/// <summary>
	/// 转生解锁的技能
	/// </summary>
    public List<profSkill> unlock = new List<profSkill>();

	/// <summary>
	/// 购买的修为数量
	/// </summary>
	public int buySuperExp;
	/// <summary>
	/// 购买修为消耗的经验
	/// </summary>
	public int needExp;
	/// <summary>
	/// 购买修为消耗的金币
	/// </summary>
	public int needGold;
	/// <summary>
	/// 购买物品增加的修为1
	/// </summary>
	public int buyThing1;
	/// <summary>
	/// 购买物品增加的修为2.
	/// </summary>
	public int buyThing2;
	/// <summary>
	/// 转生需求等级
	/// </summary>
	public int need_lev;
	/// <summary>
	/// 购买次数
	/// </summary>
	public int buy_num;


	public List<int> attr = new List<int>(); 
	public List<int> items = new List<int>(); 
	public void SetAttr(){
		attr.Clear();
		attr.Add(atk1);
		attr.Add(def1);
		attr.Add(critical);
		attr.Add(tough);
		attr.Add(hit);
		attr.Add(dodge);
		attr.Add(mana);
		attr.Add(health);
		
		items.Clear();
		items.Add(buyThing1);
		items.Add(buyThing2);
	}
	
	
}
[System.Serializable]
public class profSkill
{
   public  int prof;
   public int skillID;
   public profSkill(int _prof,int _skillID)
   {
       prof = _prof;
       skillID = _skillID;
   }

   public profSkill()
   {

   }

}
