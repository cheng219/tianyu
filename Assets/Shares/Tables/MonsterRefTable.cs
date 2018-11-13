//=====================================
//作者：龙英杰
//日期：2015/7/16
//用途：怪物静态数据配置
//=====================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterRefTable : AssetTable {

	public List<MonsterRef> infoList = new List<MonsterRef>();
}

[System.Serializable]
public class MonsterRef{
    /// <summary>
    /// 怪物ID
    /// </summary>
	public int id;
    /// <summary>
    /// 怪物等级
    /// </summary>
    public int lv;
    /// <summary>
    /// 怪物档次
    /// </summary>
    public int rank_level;
    /// <summary>
    /// 普攻技能
    /// </summary>
    public int skill_public;
    /// <summary>
    /// 体积大小
    /// </summary>
    public int volume;
    /// <summary>
    /// 模型缩放千分比
    /// </summary>
    public int model_scale;
    /// <summary>
    /// 巡逻停留间隔
    /// </summary>
    public int rest_time;
    /// <summary>
    /// 尸体停留时间
    /// </summary>
    public int corpse_remain_time;
    /// <summary>
    /// 基础移动速度
    /// </summary>
    public float baseMoveSpd;
    /// <summary>
    /// 怪物名字
    /// </summary>
    public string name;
    /// <summary>
    /// 资源路径
    /// </summary>
    public string resName;
    /// <summary>
    /// 骨骼资源
    /// </summary>
    public string boneName;
    /// <summary>
    /// 死亡动作
    /// </summary>
    public string dead_action;
    /// <summary>
    /// 描述文字
    /// </summary>
	public string des;
    /// <summary>
    /// 图标名称
    /// </summary>
	public string res;
    /// <summary>
    /// 受伤音效
    /// </summary>
    public string sound_hurt_res;
    /// <summary>
    /// 特效
    /// </summary>
    public string effect;
    /// <summary>
    /// 出生动作
    /// </summary>
    public string bornAnim;
    /// <summary>
    /// 出生特效
    /// </summary>
    public string bornEffect;
    /// <summary>
    /// 名字高度
    /// </summary>
    public float name_height;
	/// <summary>
    /// 客户端半径
	/// </summary>
	public float cmodel_r;
    /// <summary>
    /// 客户端高度
    /// </summary>
	public float model_y;
    /// <summary>
    /// 选中光环大小
    /// </summary>
    public float displaySize;
    /// <summary>
    /// 预览的时候，模型缩放  by吴江
    /// </summary>
    public float preview_scale;
    /// <summary>
    /// 怪物步伐动画速度
    /// </summary>
    public float paceSpeed;
    /// <summary>
    /// 预览设置
    /// </summary>
    public float taskPreviewScale;
	/// <summary>
    /// 预览的时候，模型位置  by 
    /// </summary>
	public Vector3 previewPscale = Vector3.zero;
	/// <summary>
    /// 预览的时候，模型旋转角度  by 
    /// </summary>
	public Vector3 previewRscale = Vector3.zero;
    /// <summary>
    /// 预览设置
    /// </summary>
    public Vector3 taskpreviewPscale = Vector3.zero;
    /// <summary>
    /// 预览设置
    /// </summary>
    public Vector3 taskpreviewRscale = Vector3.zero;
    /// <summary>
    /// CD技能，当这个列表的技能都进入cd时才开始进行普通攻击
    /// </summary>
    public List<int> skill;
    /// <summary>
    /// 模型资源列表（装备列表）
    /// </summary>
    public List<int> equipList;
    /// <summary>
    /// 怪物换色颜色
    /// </summary>
    public Color color;

    public int blood_volume;
}
public enum MobRankLevel
{
	/// <summary>
	/// 普通小怪
	/// </summary>
	NORMAL,
	/// <summary>
	/// 精英怪
	/// </summary>
	ELITE,
	/// <summary>
	/// BOSS
	/// </summary>
	BOSS,
	/// <summary>
	/// 个人镖车
	/// </summary>
	DAILYDART,
	/// <summary>
	/// 公会镖车
	/// </summary>
	GUILDDART,
	/// <summary>
	/// 花车
	/// </summary>
	SEDAN,
}

