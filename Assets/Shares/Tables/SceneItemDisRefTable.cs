//=============================
//作者：易睿
//日期：2015/09/28
//用途：场景物品静态配置数据(SceneItemDisRef)
//=============================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneItemDisRefTable : AssetTable
{

    public List<SceneItemDisRef> infoList = new List<SceneItemDisRef>();
}

[System.Serializable]
public class SceneItemDisRef
{
    /// <summary>
    /// ID
    /// </summary>
    public int id;
//    /// <summary>
//    /// 场景ID
//    /// </summary>
//    public int sceneId;
    /// <summary>
    /// 场景物品ID
    /// </summary>
    public int sceneItemId;
//    /// <summary>
//    /// 刷新周期时间
//    /// </summary>
//    public int refreshCycle;
    /// <summary>
    /// 方向
    /// </summary>
    public int direction;
//    /// <summary>
//    /// 脚本执行的延迟时间（采集时间）
//    /// </summary>
//    public int scriptTime;
    /// <summary>
    /// 生命值 用于攻击
    /// </summary>
    public int HP;
    /// <summary>
    /// 阻挡物每次挨打掉血值，用于攻击
    /// </summary>
    public int dmgBehit;
    /// <summary>
    /// 跳跃传送的时间
    /// </summary>
    public float jumpTime;
//    /// <summary>
//    /// 通知文本
//    /// </summary>
//    public string alertText;
//    /// <summary>
//    /// 对应后台脚本
//    /// </summary>
//    public string script;
//    /// <summary>
//    /// 刷新类型
//    /// </summary>
//    public RefreshType refreshType;
//    /// <summary>
//    /// 是全线刷新，还是随机单线刷新
//    /// </summary>
//    public RefreshRange refreshRange;
//    /// <summary>
//    /// 刷新的通告类型
//    /// </summary>
//    public RefreshAlertType refreshAlertType;
//    /// <summary>
//    /// 通告方式
//    /// </summary>
//    public AlertModel alertModel;
    /// <summary>
    /// 触发后结果
    /// </summary>
    public ActionResult actionResult;
    /// <summary>
    /// 关联方式
    /// </summary>
    public ConnectType connectType;
    /// <summary>
    /// 关联操作时，触发者的镜头操作
    /// </summary>
    public ConnectCameraType connectCameraType;
    /// <summary>
    /// 自动战斗时是否对其进行操作（或攻击）
    /// </summary>
    public AI ai;
    /// <summary>
    /// 功能类型
    /// </summary>
    public SceneFunctionType sceneFunctionType;
//    /// <summary>
//    /// 刷新坐标
//    /// </summary>
//    public List<int> disCoordinateList = new List<int>();
//    /// <summary>
//    /// 刷新时间点
//    /// </summary>
//    public List<int> refreshTimeList = new List<int>();
    /// <summary>
    /// 关联实例ID
    /// </summary>
    public List<int> connectItemIdList = new List<int>();
    /// <summary>
    /// 模型大小
    /// </summary>
    public float modelSize;
    /// <summary>
    /// 泡泡说话ID
    /// </summary>
    public int bubbleID;
    //public Vector3 modelSize;
    /// <summary>
    /// 启用的任务线
    /// </summary>
    public int task;
    /// <summary>
    /// 开始启用的任务步骤
    /// </summary>
    public int startStep;
    /// <summary>
    /// 开始   任务状态
    /// </summary>
    public TaskStateType startStepTime;
    /// <summary>
    /// 结束启用的任务步骤
    /// </summary>
    public int overStep;
    /// <summary>
    /// 结束   任务状态
    /// </summary>
    public TaskStateType overStepTime;

}


///// <summary>
///// 刷新类型
///// </summary>
//public enum RefreshType
//{ 
//    /// <summary>
//    /// 周期刷新
//    /// </summary>
//    CYCLE,
//    /// <summary>
//    /// 时间点刷新
//    /// </summary>
//    TIME,
//    /// <summary>
//    /// 脚本刷新
//    /// </summary>
//    SCRIPT,
//}
///// <summary>
///// 是全线刷新，还是随机单线刷新
///// </summary>
//public enum RefreshRange
//{ 
//    /// <summary>
//    /// 全线刷新
//    /// </summary>
//    ALLLINE,
//    /// <summary>
//    /// 随机单线刷新
//    /// </summary>
//    RANDOMLINE,
//}
/// <summary>
/// 刷新的通告类型
/// </summary>
//public enum RefreshAlertType
//{ 
//    /// <summary>
//    /// 无通知
//    /// </summary>
//    NO,
//    /// <summary>
//    /// 世界通知
//    /// </summary>
//    WORLD,
//    /// <summary>
//    /// 区域通知
//    /// </summary>
//    AREA,
//    /// <summary>
//    /// 场景通知
//    /// </summary>
//    SCENE,
//}
/// <summary>
/// 通告方式
/// </summary>
public enum AlertModel
{ 
    /// <summary>
    /// 无通知
    /// </summary>
    NO,
    /// <summary>
    /// 弹窗通知
    /// </summary>
    DIALOG,
    /// <summary>
    /// 上浮文字通知
    /// </summary>
    PUSHWORD,
}
/// <summary>
/// 触发后结果
/// </summary>
public enum ActionResult
{
    /// <summary>
    /// 保留
    /// </summary>
    REMAIN,
    /// <summary>
    /// 死亡
    /// </summary>
    DEAD,
}
/// <summary>
/// 关联方式
/// </summary>
public enum ConnectType
{ 
    /// <summary>
    /// 无联动
    /// </summary>
    NONE,
    /// <summary>
    /// 瞬间传送
    /// </summary>
    TRANSFORM,
    /// <summary>
    /// 跳跃传送
    /// </summary>
    JUMP,
    /// <summary>
    /// 联动
    /// </summary>
    OPERATE,
    /// <summary>
    /// 杀死
    /// </summary>
    KILL,
    /// <summary>
    /// 阻挡
    /// </summary>
    BLOCK,
    /// <summary>
    /// 纯脚本触发
    /// </summary>
    TRIGGER,
}
/// <summary>
/// 关联操作时，触发者的镜头操作
/// </summary>
public enum ConnectCameraType
{ 
    /// <summary>
    /// 关注
    /// </summary>
    FOCUS,
    /// <summary>
    /// 不关注
    /// </summary>
    IGNORE,
}
/// <summary>
/// 自动战斗时是否对其进行操作（或攻击）
/// </summary>
public enum AI
{
    /// <summary>
    /// 关注
    /// </summary>
    FOCUS,
    /// <summary>
    /// 不关注
    /// </summary>
    IGNORE,
}
/// <summary>
/// 功能类型
/// </summary>
public enum SceneFunctionType
{ 
    NONE = 0,
    /// <summary>
    /// 阻挡
    /// </summary>
    BLOCK = 1,
    /// <summary>
    /// 纯脚本触发
    /// </summary>
    TRIGGER = 2,
    /// <summary>
    /// 其他
    /// </summary>
    OTHER = 3,
    /// <summary>
    /// 传送
    /// </summary>
    TELE=4,
	/// <summary>
	/// 神圣水晶石
	/// </summary>
	HOLYSTONE = 5,
    /// <summary>
    /// 后台脚本
    /// </summary>
    SCRIPT,
    /// <summary>
    /// 安全区
    /// </summary>
    SAFEROOM,

}
