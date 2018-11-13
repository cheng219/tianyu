//==========================================
//作者：易睿
//日期：2015/10/21
//用途：任务系统的客户端静态配置
//=========================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class TaskConfigRefTable : AssetTable
{
    public List<TaskConfigRef> infoList = new List<TaskConfigRef>();
}

[System.Serializable]
public class TaskConfigRef
{
    public int id;
    public int task;
    public List<TaskStepRef> stepList = new List<TaskStepRef>();


    protected Dictionary<int, TaskStepRef> stepDic = new Dictionary<int, TaskStepRef>();

    public void InitData()
    {
        foreach (var item in stepList)
        {
            stepDic[item.step] = item;
        }
    }

    public TaskStepRef GetTaskStepRef(int _step)
    {
        if (stepDic.ContainsKey(_step))
        {
            return stepDic[_step];
        }
        return null;
    }

}

[System.Serializable]
public class TaskStepRef{
	public int id;
	public int task;
	public int step;
	public int is_start;
	public string name;
    public TaskType sort;
    public TaskContentType taskContentType;
	public int prof;
	public int need_lev;
	public int need_task;
	public int need_task_step;
    public TaskNpcQuestRef takeFromNpc;
    public TaskNpcQuestRef commitToNpc;
    public int bossID;
	public int start_movie;
	public int complete_movie;
    public string accept_icon;
    public string finish_icon;
	public string task_details;
	public int coin;
	public int exp;
    public int sp;
    public int startLv;
    public List<ItemValue> rewardList = new List<ItemValue>();
	public string detail_talk;
    public List<TaskConditionRef> condtionRefList = new List<TaskConditionRef>();
	public int xy;
    public GUIType jumpToGUIType;
	public List<int> funcSequence=new List<int>();
    public List<string> tasknumDetails = new List<string>();
    public int taskRow;
    public Vector3 targetCoordiante;
    public List<string> bubbleList = new List<string>();
    /// <summary>
    /// 任务结束状态
    /// </summary>
    public int taskEnding;
    /// <summary>
    /// 任务所属阵营
    /// </summary>
    public int taskCamp;
    /// <summary>
    /// 1，走自动任务流程   2，走引导流程
    /// </summary>
    public int guide;
}

/// <summary>
/// 开启于任务步骤的状态
/// </summary>
public enum TaskStateType
{
    /// <summary>
    /// 未接
    /// </summary>
    UnTake = 0,
    /// <summary>
    /// 已接受未完成
    /// </summary>
    Process = 1,
    /// <summary>
    /// 已完成未交
    /// </summary>
    Finished = 2,
	/// <summary>
	/// 任务结束(一般情况结束就从列表中删除,结义任务除外)
	/// </summary>
	ENDED = 3,
}

public enum GuideType
{
	NONE,
	/// <summary>
	/// 呆在原地
	/// </summary>
	STAY = 1,
	/// <summary>
	/// 移动之后呆在原地
	/// </summary>
	MOVEANDSTAY = 2,
	/// <summary>
	/// 完成任务后呆在原地
	/// </summary>
	FINISHSTAY = 3,
	/// <summary>
	/// 移动中引导
	/// </summary>
	MOVINGGUIDE = 4,
}



public enum TaskType
{
    /// <summary>
    /// 未知
    /// </summary>
    UnKnow = 0,
    /// <summary>
    /// 主线任务
    /// </summary>
    Main = 1,
    /// <summary>
    /// 支线/深渊任务
    /// </summary>
    Branch = 2,
    /// <summary>
    /// 环任务
    /// </summary>
    Ring = 3,
    /// <summary>
    /// 试炼任务
    /// </summary>
    Trial = 4,
    /// <summary>
    /// 日常任务(后台用于结义任务)
    /// </summary>
    Daily = 5,
    /// <summary>
    /// 每日任务(暂时没用)
    /// </summary>
    Offer = 6,
    /// <summary>
    /// 工会任务(暂时没用)
    /// </summary>
    Guild = 7,
    /// <summary>
    /// 成长任务(暂时没用)
    /// </summary>
    Grow = 8,
	/// <summary>
	/// 特殊任务(未接取环任务or试炼任务or挂机or副本)
	/// </summary>
	Special = 9,
}
/// <summary>
/// 任务内容类型
/// </summary>
public enum TaskContentType
{
    /// <summary>
    /// 未知
    /// </summary>
    NONE = 0,
    /// <summary>
    /// 使用NPC功能
    /// </summary>
    NPC_FUNCTION = 1,
    /// <summary>
    /// 达成某些条件
    /// </summary>
    DO_SOMETHING = 2,
    /// <summary>
    /// 去某个场景
    /// </summary>
    GOTO_SCENE = 3,

}



[System.Serializable]
public class TaskNpcQuestRef
{
    public int npcID;
    public string npcHeadIconName;
    public string text;
}

[System.Serializable]
public class TaskConditionRef
{
    public TaskConditionType sort;
    public int data;
    public int number;
}


/// <summary>
/// 任务行为（包括要去哪里，以及到达地点后要干什么）
/// </summary>
public enum TaskConditionType
{
    /// <summary>
    /// 击杀任意怪物1001
    /// </summary>
    KillAnyKindMonster = 1001,
    /// <summary>
    /// 击杀指定怪物1002
    /// </summary>
    KillOneKindMonster= 1002,
    /// <summary>
    /// 击杀指定场景指定怪物1003
    /// </summary>
    KillAnyKindMonsterAnyScene = 1003,
	/// <summary>
	/// 击杀指定场景的怪
	/// </summary>
	KillOneSceneMonster = 1004,
	/// <summary>
	/// 击杀指定等级的怪
	/// </summary>
	KillLevel35Monster = 1005,
	KillLevel40Monster = 1006,
	KillLevel45Monster = 1007,
	KillLevel50Monster = 1008,
    /// <summary>
    /// 过任意副本2001
    /// </summary>
    FinishAnyKindMap = 2001,
    /// <summary>
    /// 过指定副本2002
    /// </summary>
    FinishOneKindMap = 2002,
    /// <summary>
    /// 过剧情副本2003
    /// </summary>
    FinishStoryMap = 2003,
    /// <summary>
    /// 提交物品3001
    /// </summary>
    SubmitOneKindItem = 3001,
    /// <summary>
    /// 拥有物品3002
    /// </summary>
    HaveOneKindItem = 3002,
    /// <summary>
    /// 穿装备6001
    /// </summary>
    ArmyEquipment = 20001,
    /// <summary>
    /// 强化装备6002
    /// </summary>
    StrenthenEquipment = 6002,
    /// <summary>
    /// 收集场景物品8001
    /// </summary>
    CollectSceneItem = 8001,
	/// <summary>
	/// 收集场景任意物品8001
	/// </summary>
	CollectAnyItem = 8002,
	/// <summary>
	/// 与指定的NPC对话
	/// </summary>
	ChatWithNpc = 19001,
}

