//=====================================
//作者：易睿
//日期：2015/7/09
//用途：一次性技能表现静态配置
//==========================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillPerformanceRefTable : AssetTable
{
    public List<SkillPerformanceRef> infoList = new List<SkillPerformanceRef>();
}


[System.Serializable]
public class SkillPerformanceRef
{
    /// <summary>
    /// 技能ID 
    /// </summary>
    public int skillId;
    /// <summary>
    /// 目标数量  只对范围随机的数量模式产生影响
    /// </summary>
    public int targetNum;
    /// <summary>
    /// 范围中心距离  范围中心距玩家自身的距离（在目标与自身的直线上）
    /// </summary>
    public float areaCenterRange;
    /// <summary>
    /// 主体位移距离 决定瞬移以及方向两种移动方式的距离
    /// </summary>
    public float selfShiftRange;
    /// <summary>
    /// 范围特效 填写范围类型的技能，在释放点所播放的特效
    /// </summary>
    public int aoeEffect;
    /// <summary>
    ///  弹道特效 填写弹道类型的技能，所释放的飞行道具的特效
    /// </summary>
    public int arrowEffect;
    /// <summary>
    ///施放距离 技能的最远释放距离，目标需要在这个距离之内，技能才能释放
    /// </summary>
    public float castRange;
    /// <summary>
    /// 位移相对方位
    /// </summary>
    public float shiftDirection;
    /// <summary>
    /// 目标位移距离  目标位移只有方向一种方式 目标位移的方向为目标坐在位置与技能释放点的连线 目标的位移会在第一次伤害的时间点上执行，速度为设定死的值，无需配置
    /// </summary>
    public float targetShiftRange;
    /// <summary>
    /// 击退距离
    /// </summary>
    public float kickDistance;
    /// <summary>
    /// 硬直开始
    /// </summary>
    public float timeYzStart;
    /// <summary>
    /// 前摇时间 前摇动作以及特效的播放时间
    /// </summary>
    public float frontTime;
    /// <summary>
    /// 后摇时间 后摇动作和以及特效的播放时间
    /// </summary>
    public float afterTime;
    /// <summary>
    /// 硬直时间
    /// </summary>
    public float timeYz;
    /// <summary>
    /// 霸体开始
    /// </summary>
    public float timeBtStart;
    /// <summary>
    /// 霸体时间
    /// </summary>
    public float timeBt;
    /// <summary>
    /// 无敌开始
    /// </summary>
    public float timeWdStart;
    /// <summary>
    /// 无敌时间
    /// </summary>
    public float timeWd;
    /// <summary>
    /// 僵直持续时间
    /// </summary>
    public float kickTimes;
    /// <summary>
    /// 非主动操作状态下的技能的最短动作时间，从0开始
    /// </summary>
    public float protectTime;
    /// <summary>
    /// 击退速度 发生击退位移时的位移速度
    /// </summary>
    public float kickShowTimes;
    /// <summary>
    /// 技能名称
    /// </summary>
    public string skillName;
    /// <summary>
    /// 前摇动作 一个单独的动作
    /// </summary>
    public string frontAction;
    /// <summary>
    /// 后摇动作 一个单独的动作
    /// </summary>
    public string afterAction;
    /// <summary>
    /// 前摇动作  一个单独的特效，播放前摇动作时播放
    /// </summary>
    public string frontEffect;
    /// <summary>
    /// 后摇动作 一个单独的特效，播放后摇动作时播放
    /// </summary>
    public string afterEffect;
    /// <summary>
    /// 整体时间特效
    /// </summary>
    public string wholetimeEffect;
    /// <summary>
    /// 震动方向向量
    /// </summary>
    public Vector3 shakeDirection;

    ///// <summary>
    ///// 附加BUFF 填写技能对目标所附加的BUFFID，可以以数组的方式填写多个
    ///// </summary>
    //public List<int> buffIdList = new List<int>();

    /// <summary>
    /// 伤害分布 与伤害时间点对应，将总伤害按百分比进行分布，以数组的方式配置多个，总值100%（100）
    /// </summary>
    public List<int> damageRateList = new List<int>();//伤害分布
    /// <summary>
    /// 被击特效时间
    /// </summary>
    public List<float> hitEffectTimeList = new List<float>();
    /// <summary>
    /// 过程时间 过程动作以及特效的总播放时间
    /// </summary>
    public List<float> processTimeList = new List<float>();
    /// <summary>
    /// 伤害时间点 在整个过程动作中，在哪些时间点显示被击特效和伤害,可以以数组的方式配置多个
    /// </summary>
    public List<float> damageTimeList = new List<float>();//伤害时间点
    /// <summary>
    /// 范围参数 外径|内径|角度|长|宽|上高|下高
    /// </summary>
    public List<float> areaParaList = new List<float>();
    /// <summary>
    /// 震动力度组
    /// </summary>
    public List<float> shakePowerGroupList = new List<float>();
    /// <summary>
    /// 震动时间点组
    /// </summary>
    public List<int> shakeTimeList = new List<int>();
    /// <summary>
    /// 击退分段 在每次伤害时位移距离，总值应该为总位移距离
    /// </summary>
    public List<float> kickDisSectionList = new List<float>();
    /// <summary>
    /// 被击特效 与伤害时间点对应，也可以以数组的方式配置多个
    /// </summary>
    public List<string> hitEffectList = new List<string>();
    /// <summary>
    ///过程动作 可以以数组的方式配置多个动作，播放时会依次播放
    /// </summary>
    public List<string> processActionList = new List<string>();
    /// <summary>
    ///过程特效 可以以数组的方式配置多个特效，播放时将配合过程动作同步播放
    /// </summary>
    public List<string> processEffectList = new List<string>();
    public List<AbilityDelayEffectRefData> abilityDelayEffectRefDataList = new List<AbilityDelayEffectRefData>();
    /// <summary>
    /// 攻击音效
    /// </summary>
    public List<SkillSoundPair> soundAtkRes = new List<SkillSoundPair>();
    /// <summary>
    /// 被击音效
    /// </summary>
    public List<SkillSoundPair> soundDefRes = new List<SkillSoundPair>();
    /// <summary>
    ///  目标类型
    /// </summary>
    public SkillTargetType skillTargetType;
    /// <summary>
    /// 技能正负方向
    /// </summary>
    public SkillGroup skillGroup;
    /// <summary>
    /// 技能类型
    /// </summary>
    public SkillType skillType;
    /// <summary>
    /// 释放方式
    /// </summary>
    public CastType castType;
    /// <summary>
    /// 释放点
    /// </summary>
    public CastPoint castPoint;
    /// <summary>
    /// 目标数量模式
    /// </summary>
    public TargetNumType targetNumType;
    /// <summary>
    /// 影响范围
    /// </summary>
    public ImpactArea impactArea;
    /// <summary>
    /// 主体位移方式
    /// </summary>
    public SelfShiftType selfShiftType;
    /// <summary>
    /// 击退模式
    /// </summary>
    public KickType kickType;
    /// <summary>
    /// 转向类型 （技能释放时主角的转向）
    /// </summary>
    public TurnType turnType;
    /// <summary>
    /// 预警显示时间
    /// </summary>
    public float alertTimesShow;
    /// <summary>
    /// 决定扇形的半径，矩形的长度（玩家面朝方向）
    /// </summary>
    public float alertAreaLength;
    /// <summary>
    /// 决定矩形的宽度（玩家面朝垂直方向）
    /// </summary>
    public float alertAreaWidth;
    /// <summary>
    /// 预警范围
    /// </summary>
    public AlertAreaType alertAreaType;
    /// <summary>
    /// 纯前台特殊表现类型
    /// </summary>
    public ClientShowType clientShowType;

    public int reportId;
    /// <summary>
    /// 冒泡ID
    /// </summary>
    public int popId;
}

[System.Serializable]
public class AbilityDelayEffectRefData
{
    public Vector3 diffPos;
    public string effectName;
    public float duration;
    public float startTIme;
}

/// <summary>
/// 目标类型
/// </summary>
public enum SkillTargetType
{
    /// <summary>
    ///  无目标
    /// </summary>
    NONE = 0,
    /// <summary>
    /// 敌人
    /// </summary>
    ENEMY = 1,
    /// <summary>
    /// 自己
    /// </summary>
    SELF = 2,
    /// <summary>
    /// 友方
    /// </summary>
    TEAMMATE = 3,
}
/// <summary>
/// 技能正负方向
/// </summary>
public enum SkillGroup
{ 
    /// <summary>
    /// 伤害：对目标起负面效果的技能
    /// </summary>
    DAMAGE,
    /// <summary>
    /// 友善：对目标起正面效果的技能
    /// </summary>
    BENEFIT,  
}
/// <summary>
/// 技能类型
/// </summary>
public enum SkillType
{ 
    /// <summary>
    /// 范围型：对一个形状范围内的目标造成影响
    /// </summary>
    AOE,
    /// <summary>
    /// 跟踪弹道型：从释放点发射一个飞行道具飞向目标，对目标造成影响，跟踪目标
    /// </summary>
    FOLLOWARROW,
    /// <summary>
    /// 方向弹道型：从释放点发射一个飞行道具飞向目标，对碰撞的目标造成伤害，不跟踪
    /// </summary>
    DIRECTARROW,
    /// <summary>
    /// 陷阱型：在目标所在地点释放一个陷阱
    /// </summary>
    TRAP,
    /// <summary>
    /// 召唤型
    /// </summary>
    CALL,
    /// <summary>
    /// 其他
    /// </summary>
    OTHER,
}
/// <summary>
/// 释放方式
/// </summary>
public enum CastType
{ 
    /// <summary>
    /// 指向性：需要选择正确的目标才能释放
    /// </summary>
    TARGET,
    /// <summary>
    /// 非指向性：无需选择目标，即可原地释放
    /// </summary>
    NOTARGET,
}
/// <summary>
/// 释放点
/// </summary>
public enum CastPoint
{
    /// <summary>
    /// 自身地点：角色当前所在坐标为释放点
    /// </summary>
    SELFCOORDINATE,
    /// <summary>
    /// 目标地点：所选中的目标所在坐标为释放点
    /// </summary>
    TARGETCOORDINATE,
    /// <summary>
    /// 自身角色：对自身角色释放
    /// </summary>
    SELFROLE,
    /// <summary>
    /// 目标角色：对目标角色释放
    /// </summary>
    TARGETROLE,
    /// <summary>
    /// 表示对指定位置释放（仅对陷阱有效）
    /// </summary>
    COORDINATE,
}
/// <summary>
/// 目标数量模式
/// </summary>
public enum TargetNumType
{ 
    /// <summary>
    /// 目标单体：只影响所选目标一个单位
    /// </summary>
    TARGETSINGLE,
    /// <summary>
    /// 范围所有：影响范围内的所有单位
    /// </summary>
    AOEALL,
    /// <summary>
    /// 范围随机：影响范围内一定数量的单位
    /// </summary>
    AOERANDOM,
    /// <summary>
    /// 无伤害：没有额外影响
    /// </summary>
    NOEFFECT,
}
/// <summary>
/// 影响范围
/// </summary>
public enum ImpactArea
{ 
    /// <summary>
    /// 圆形
    /// </summary>
    CYCLE,
    /// <summary>
    /// 矩形
    /// </summary>
    RECT,
    /// <summary>
    /// 扇形
    /// </summary>
    SECTOR,
    /// <summary>
    /// 环形
    /// </summary>
    RING,
    /// <summary>
    /// 点
    /// </summary>
    POINT,
}
/// <summary>
/// 主体位移方式
/// </summary>
public enum SelfShiftType
{
    /// <summary>
    /// 瞬移：该技能释放后，会瞬间移动技能的释放本体
    /// </summary>
    BLINK,
    /// <summary>
    /// 路径：该技能释放后，会在过程时间内，以过程动作，从当前位置，移动到目标所在位置身边
    /// </summary>
    PATH,
    /// <summary>
    /// 方向：该技能释放后，会朝目标所在方向，在过程时间内，以过程动作，移动一定距离
    /// </summary>
    DIRECTION,
    /// <summary>
    /// 无位移：技能释放者不会发生位移
    /// </summary>
    NO,
}

/// <summary>
/// 击退模式
/// </summary>
public enum KickType
{
    /// <summary>
    /// 击退
    /// </summary>
    KICKBACK,
    /// <summary>
    /// 击倒
    /// </SUMMARY>
    KICKDOWN,
    /// <summary>
    /// 击飞
    /// </summary>
    KICKFLY,
    /// <summary>
    /// 无效果
    /// </summary>
    NO,
}
/// <summary>
/// 转向类型 （技能释放时主角的转向）
/// </summary>
public enum TurnType
{ 
    NONE = 0,
    /// <summary>
    /// 转向敌人所在方向释放
    /// </summary>
    TURN = 1,
    /// <summary>
    /// 原方向释放
    /// </summary>
    NOTTURN = 2,
}
/// <summary>
/// 预警范围
/// </summary>
public enum AlertAreaType
{
    NONE = 0,
    /// <summary>
    /// 30度扇形预警范围
    /// </summary>
    SECTOR30 = 1,
    /// <summary>
    /// 45度扇形预警范围
    /// </summary>
    SECTOR45 = 2,
    /// <summary>
    /// 60度扇形预警范围
    /// </summary>
    SECTOR60 = 3,
    /// <summary>
    /// 90度扇形预警范围
    /// </summary>
    SECTOR90 = 4,
    /// <summary>
    /// 135度扇形预警范围
    /// </summary>
    SECTOR135 = 5,
    /// <summary>
    /// 180度扇形预警范围
    /// </summary>
    SECTOR180 = 6,
    /// <summary>
    /// 270度扇形预警范围
    /// </summary>
    SECTOR270 = 7,
    /// <summary>
    /// 360度扇形预警范围
    /// </summary>
    SECTOR360 = 8,
    /// <summary>
    /// 矩形
    /// </summary>
    RECT = 9,
}
/// <summary>
/// 纯前台特殊表现类型
/// </summary>
public enum ClientShowType
{
    NONE = 0,
    /// <summary>
    /// 无敌斩
    /// </summary>
    Invinciblechop = 1,
    /// <summary>
    /// 闪电链
    /// </summary>
    Lightingskill = 2,
}

[System.Serializable]
public class SkillSoundPair
{
    public string res;
    public float time;
}

