//=========================
//作者：黄洪兴
//日期：2016/03/05
//用途：宠物静态配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewPetRefTable : AssetTable
{
	public List<NewPetRef> infoList = new List<NewPetRef>();
}


[System.Serializable]
public class NewPetRef
{
	/// <summary>
	/// 表编号
	/// </summary>
	public	int id;
	/// <summary>
	/// 宠物名字
	/// </summary>
	public string petname;
	/// <summary>
	/// 宠物说明
	/// </summary>
	public string name;
	/// <summary>
	/// 宠物图片
	/// </summary>
	public string icon;
	/// <summary>
	/// 宠物骨骼名称
	/// </summary>
	public string boneName;
	/// <summary>
	/// 骨骼资源
	/// </summary>
	public string res_name;
	/// <summary>
	/// 模型资源
	/// </summary>
	public int equipList;
	/// <summary>
	/// AI类型
	/// </summary>
	public int type;
	/// <summary>
	/// 步伐幅度
	/// </summary>
	public int pace_speed;
	/// <summary>
	/// 移动速度
	/// </summary>
	public int move_spd;
	/// <summary>
	/// 跟随范围
	/// </summary>
	public int followRange;

    /// <summary>
    /// 多少范围内停止跟随
    /// </summary>
    public int stopFollowRange;

    /// <summary>
    /// 锁定怪物的距离
    /// </summary>
    public int pveAtkRange;

    /// <summary>
    /// 主人目标距离自身超过多少范围后会优先攻击主人目标
    /// </summary>
    public int pveMasterAtkRange;

    /// <summary>
    /// PVE的返回距离
    /// </summary>
    public int pveReturnRange;
    /// <summary>
    /// 锁定其他玩家的距离
    /// </summary>
    public int pvpAttackRange;

    /// <summary>
    /// 反击攻击主人的敌人的距离
    /// </summary>
    public int pvpDefenseRange;

    /// <summary>
    /// PVP主人目标距离自身超过多少范围后会优先攻击主人目标
    /// </summary>
    public int pvpMasterAtkRange;
    /// <summary>
    /// 宠物跟随时让宠物返回主人身边的距离
    /// </summary>
    public int teleportRange;

    /// <summary>
    /// 普通攻击
    /// </summary>
    public int normolSkill;
	/// <summary>
	/// 模型大小
	/// </summary>
	public float modelScale;
	/// <summary>
	/// 初始攻击
	/// </summary>
	public int att;
	/// <summary>
	/// 初始命中
	/// </summary>
	public int hit;
	/// <summary>
	/// 初始暴击
	/// </summary>
	public int cri;
	/// <summary>
	/// 初始宠物的等级
	/// </summary>
	public int grow_up_lev;
	/// <summary>
	/// 孵化时宠物的资质
	/// </summary>
	public int zizhi;
	/// <summary>
	/// 孵化时宠物的灵修
	/// </summary>
	public List<int> lingxiu=new List<int>();
	/// <summary>
	/// 孵化时宠物附带的技能
	/// </summary>
	public List<int> skill=new List<int>();
	/// <summary>
	/// 初始战力
	/// </summary>
	public int gs;

    /// <summary>
    /// 预览坐标
    /// </summary>
    public Vector3 previewPosition;

    /// <summary>
    /// 预览朝向
    /// </summary>
    public Vector3 previewRotation;


}