//=============================================
//作者：邓成
//日期：2016/3/14
//用途：任务管理类
//===============================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;
using System;
using System.Text;

/// <summary>
/// 任务管理类 by吴江
/// </summary>
public class TaskMng
{

    #region 构造
    /// <summary>
    /// 构造
    /// </summary>
    public static TaskMng CreateNew(MainPlayerMng _father)
    {
        if (_father == null) return null;
        if (_father.taskMng == null)
        {
            TaskMng taskMng = new TaskMng();
            taskMng.Init(_father);
            return taskMng;
        }
        else
        {
            _father.taskMng.UnRegist(_father);
            _father.taskMng.Init(_father);
            return _father.taskMng;
        }
    }

    protected void Init(MainPlayerMng _father)
    {
        MsgHander.Regist(0xD013, S2C_OnGotTaskList);
        MsgHander.Regist(0xD018, S2C_OnAddTask);
        MsgHander.Regist(0xD014, S2C_OnTaskUpdate);
        MsgHander.Regist(0xD017, S2C_OnGiveUpTask);
        MsgHander.Regist(0xD132, S2C_GetStarAndCount);
		MsgHander.Regist(0xD698,S2C_GetTrialTaskRestRewardTimes);
        MsgHander.Regist(0xD016, S2C_OnCommitTask);
        MsgHander.Regist(0xC120, S2C_OnRingTaskFlyOver);
        MsgHander.Regist(0xC143, S2C_GetRingTaskPrograss);
        foreach (TaskType item in Enum.GetValues(typeof(TaskType)))
        {
            taskInfoDic[item] = new Dictionary<int, TaskInfo>();
            endedTaskInfoDic[item] = new Dictionary<int, TaskInfo>();
        }
        if (_father != null)
        {
            _father.MainPlayerInfo.OnBaseUpdate += OnMainPlayerLevelChange;
        }
        //autoFinishRingTask = false;
        //haveFiveFold = false; 
        SetRingAutoFinishType(1,false);
        SetRingAutoFinishType(2, false);
        SetRingAutoFinishType(3, false);
        SetRingAutoFinishType(4, false); 
    }

    protected void UnRegist(MainPlayerMng _father)
    {
        MsgHander.UnRegist(0xD013, S2C_OnGotTaskList);
        MsgHander.UnRegist(0xD018, S2C_OnAddTask);
        MsgHander.UnRegist(0xD014, S2C_OnTaskUpdate);
        MsgHander.UnRegist(0xD017, S2C_OnGiveUpTask);
        MsgHander.UnRegist(0xD132, S2C_GetStarAndCount);
		MsgHander.UnRegist(0xD698,S2C_GetTrialTaskRestRewardTimes);
        MsgHander.UnRegist(0xD016, S2C_OnCommitTask);
        MsgHander.UnRegist(0xC120, S2C_OnRingTaskFlyOver);
        MsgHander.UnRegist(0xC143, S2C_GetRingTaskPrograss);
        curFocusTask = null;
        taskInfoDic.Clear();
        endedTaskInfoDic.Clear();
        ringTypeIsFinish.Clear();
        if (_father != null)
        {
            _father.MainPlayerInfo.OnBaseUpdate -= OnMainPlayerLevelChange;
        }
		TrialTaskRestRewardTimes = 0;
		FinishAllRingTask = false;
        autoFinishRingTask = false;
        //haveFiveFold = false;
        ringTaskProgress.Clear();
        curRingTaskType = 1;
        //ringTask = null;
    }
    #endregion

    #region 数据

	/// <summary>
	/// 当前寻路的目标对象ID
	/// </summary>
	public int CurTargetID = 0;
    /// <summary>
    /// 当前寻路的目标场景
    /// </summary>
    public int CurTargetSceneID=0;
    /// <summary>
    /// 当前寻路的目标点
    /// </summary>
    public Vector3 CurTargetPoint=Vector3.zero;

    /// <summary>
    /// 以任务类型作为一级索引，以任务配置ID为二级索引的当前任务列表数据集
    /// </summary>
    protected Dictionary<TaskType, Dictionary<int, TaskInfo>> taskInfoDic = new Dictionary<TaskType, Dictionary<int, TaskInfo>>();
    /// <summary>
    /// 任务列表发生变化的事件
    /// </summary>
    public System.Action<TaskDataType, TaskType> OnTaskListUpdate;
	/// <summary>
	/// 一类任务发生变化
	/// </summary>
	public System.Action<TaskType> OnTaskGroupUpdate;

    /// <summary>
    /// 以NPC的id为索引的“起始”任务列表字典，这部分数据由客户端根据服务端传过来的任务信息自行构造
    /// </summary>
    protected Dictionary<int, Dictionary<int, TaskInfo>> npcStartTaskInfoDic = new Dictionary<int, Dictionary<int, TaskInfo>>();

    /// <summary>
    /// 已结束任务列表数据集(根据step是否为0判断)
    /// </summary>
    protected Dictionary<TaskType, Dictionary<int, TaskInfo>> endedTaskInfoDic = new Dictionary<TaskType, Dictionary<int, TaskInfo>>();

    /// <summary>
    /// 当前追踪/关注的任务发生变化的事件
    /// </summary>
    public System.Action CurFocusTaskChange;
	/// <summary>
	/// 任务完成事件,用于做提示
	/// </summary>
	public static System.Action OnTaskFinishEvent;
    /// <summary>
    /// 当前追踪/关注的任务
    /// </summary>
    protected TaskInfo curFocusTask = null;

	public System.Action<string> OnTaskProgressUpdateEvent;

    public Action<int, Vector3> TryTrace; 
    /// <summary>
    /// 当前需要传送的坐标
    /// </summary>
    public Vector3 flyVec = Vector3.zero;
    /// <summary>
    /// 当前需要传送的场景id
    /// </summary>
    public int seceneId = 0;
    /// <summary>
    /// 传送提示界面 1 任务传送 2 地图传送
    /// </summary>
    public int flyType = 1;

    /// <summary>
    /// 当前追踪/关注的任务
    /// </summary>
    public TaskInfo CurfocusTask
    {
        get
        {
            return curFocusTask;//1.	继续完成最近手动追踪的任务，
        }
        set
        {
            if (curFocusTask != value)
            {
                curFocusTask = value;
                if (CurFocusTaskChange != null)
                {
                    CurFocusTaskChange();
                }
            }
        }
    }
    /// <summary>
    /// 当前传送用到的任务
    /// </summary>
    public bool CurTaskNeedFly = false;
	protected TaskTeamWnd.ToggleType curSelectToggle = TaskTeamWnd.ToggleType.TASK;
    /// <summary>
    /// 当前选中的Toggle
    /// </summary>
	public TaskTeamWnd.ToggleType CurSelectToggle 
    {
        get 
        {
            return curSelectToggle;
        }
    }

    /// <summary>
    /// 设置当前选中的Toggle
    /// </summary>
	public void SetCurSelectToggle(TaskTeamWnd.ToggleType _select) 
    {
        curSelectToggle = _select;
    }
    ///// <summary>
    ///// 当前需要传送的换任务
    ///// </summary>
    //private TaskInfo ringTask = null;
    //public TaskInfo RingTask
    //{
    //    get
    //    {
    //        return ringTask;
    //    }
    //    set
    //    {
    //        ringTask = value;
    //    }
    //}
    /// <summary>
    /// 环式任务是否已经五倍领取
    /// </summary>
    //private bool haveFiveFold;
    //public bool HaveFiveFold
    //{
    //    get
    //    {
    //        return haveFiveFold;
    //    }
    //    set
    //    {
    //        haveFiveFold = value;
    //        if(updateRefreshStar!=null)
    //        updateRefreshStar();
    //    }
    //}
    public Dictionary<int, bool> autoFinishType = new Dictionary<int, bool>();
    public void SetRingAutoFinishType(int _type, bool _isauto)
    {
        autoFinishType[_type] = _isauto;
    }
    public bool GetRingAutoFinish(int _type)
    {
        if (autoFinishType.ContainsKey(_type))
        {
            return autoFinishType[_type];
        }
        return false;
    }
    /// <summary>
    /// 环式任务是否选择自动挂机完成
    /// </summary>
    private bool autoFinishRingTask;
    //public bool AutoFinishRingTask
    //{
    //    get
    //    {
    //        return autoFinishRingTask;
    //    }
    //    set
    //    {
    //        autoFinishRingTask = value;
    //    }
    //}
    /// <summary>
    /// 任务引导事件
    /// </summary>
    public System.Action<TaskInfo> OnTaskGuideUpdateEvent;
	/// <summary>
	/// 任务引导事件
	/// </summary>
	public System.Action<TaskInfo> OnTaskFinishedGuideUpdateEvent;
    /// <summary>
    /// 新任务的事件
    /// </summary>
    public System.Action<TaskInfo> AddNewTask;
    /// <summary>
    /// 交任务的事件
    /// </summary>
    public System.Action<TaskInfo> CommitForNewTask;
    /// 单个任务完成度变化的事件
    /// </summary>
    public System.Action<TaskInfo> updateSingleTask;
    /// 已经刷新五倍奖励的事件
    /// </summary>
    //public System.Action  updateRefreshStar;
    /// <summary>
    /// 当前寻路列表
    /// </summary>
    public List<TaskSinglePath> taskSinglePath = new List<TaskSinglePath>();


    #endregion

    #region 协议传输
    #region S2C
    /// <summary>
    /// pt_task_list_d013 得到任务列表
    /// </summary>
    /// <param name="_cmd"></param>
    protected void S2C_OnGotTaskList(Pt _pt)
    {
        //Debug.Log(" pt_task_list_d013");
        pt_task_list_d013 msg = _pt as pt_task_list_d013;
        if (msg != null)
        {
            npcStartTaskInfoDic.Clear();
            for (int i = 0; i < msg.tasks.Count; i++)
            {
                TaskInfo info = new TaskInfo(msg.tasks[i]);
                //Debug.Log("S2C_OnGotTaskList:" + info.TaskType + ",step:" + info.Step + ",state:" + msg.tasks[i].state + ",id:" + info.ID + "  , satate : " + info.TaskState + "  , diffcuilty : " + info.StarLevel);
                int taskLineIsEnd = (int)msg.tasks[i].state;
                if (!taskInfoDic.ContainsKey(info.TaskType))
                {
                    taskInfoDic[info.TaskType] = new Dictionary<int, TaskInfo>();

                }
                if (taskLineIsEnd != 3 || info.TaskType == TaskType.Trial)//不把已完成的试练任务删除
                {
                    if (taskInfoDic[info.TaskType].ContainsKey(info.ID) && taskInfoDic[info.TaskType][info.ID] != null)
                    {
                        taskInfoDic[info.TaskType][info.ID].Update(msg.tasks[i]);
                    }
                    else
                    {
                        taskInfoDic[info.TaskType][info.ID] = info;
                    }
                }
                else
                {

                    if (taskInfoDic[info.TaskType].ContainsKey(info.ID))
                    {
                        endedTaskInfoDic[info.TaskType][info.ID] = info; 
                        taskInfoDic[info.TaskType].Remove(info.ID); 
                    }
                    else
                    {
                        endedTaskInfoDic[info.TaskType][info.ID] = info;
                    }
                }
            }  
            //计算整个游戏的尚可接的"起始"任务，存入npc任务字典
            CalculateNPCStartTask();
            if (taskInfoDic.Count > 0 && OnTaskListUpdate != null) OnTaskListUpdate(TaskDataType.Started, TaskType.UnKnow);
            if (endedTaskInfoDic.Count > 0 && OnTaskListUpdate != null) OnTaskListUpdate(TaskDataType.Ended, TaskType.UnKnow);

			if(GameCenter.sceneMng != null)//这里为空
            	GameCenter.sceneMng.InitSceneNPCInstances(GameCenter.mainPlayerMng.MainPlayerInfo.SceneID);
			AddSpecialTask();
			GameCenter.mainPlayerMng.InitFunctionData();
        }
    }
    /// <summary>
    /// pt_accept_task_d018 新增任务
    /// </summary>
    /// <param name="_cmd"></param>
    protected void S2C_OnAddTask(Pt _pt)
    {
        //Debug.Log("收到新增任务协议pt_accept_task_response_d018");
        pt_accept_task_response_d018 msg = _pt as pt_accept_task_response_d018;
        if (msg != null && msg.tasks.Count > 0)
        {
            st.net.NetBase.task_list_info data = msg.tasks[0];
			
            TaskInfo curAddTaskInfo = null;

            TaskStepRef refData = ConfigMng.Instance.GetTaskStepRef((int)data.taskid, (int)data.taskstep > 0 ? (int)data.taskstep : 1);
            if (refData == null)
            {
                Debug.Log("找不到任务线" + (int)data.taskid + "的任何步骤配置数据!");
                return;
            }
            //Debug.Log("收到新增任务协议 S2C_OnAddTask:" + data.taskid + ",step:" + data.taskstep + ",state:" + data.state + "  , sort : " + refData.sort);
            //检测已开始任务列表，有则修改，无则加上
            TaskInfo hasTakeInfo = null;
            Dictionary<int, TaskInfo> dic = null;

            int id = (int)data.taskid; 
            if (taskInfoDic.TryGetValue(refData.sort, out dic))
            { 
				if (dic.TryGetValue(id, out hasTakeInfo))
                {
                    GameCenter.messageMng.AddClientMsg(33);
                    //========================================================
                    hasTakeInfo.Update(data);//注意，在这个update以后，hasCommitInfo实际上已经变成了下一步的任务，状态标记为未接。  已经不是交的那个原任务了  by吴江
                    curAddTaskInfo = hasTakeInfo;
                }
                else
                {
					dic[id] = new TaskInfo(data);
					curAddTaskInfo = dic[id];
                }
            }
            else
            {
                taskInfoDic[refData.sort] = new Dictionary<int, TaskInfo>();
				taskInfoDic[refData.sort][id] = new TaskInfo(data);
				curAddTaskInfo = taskInfoDic[refData.sort][id];
            }
            //如果本次通知是已接
            if (curAddTaskInfo != null && curAddTaskInfo.TaskState != TaskStateType.UnTake)
            {
                //如果是起始任务，将数据修改后从npc起始任务列表移动到已开始任务列表中
                int npcID = refData.takeFromNpc.npcID;
                if (data.taskstep == 1 && npcStartTaskInfoDic.ContainsKey(npcID))
                {
					if (npcStartTaskInfoDic[npcID].ContainsKey(id))
                    {
						TaskInfo info = npcStartTaskInfoDic[npcID][id];
						npcStartTaskInfoDic[npcID].Remove(id);
                        info.Update(data);
						taskInfoDic[info.TaskType][info.ID] = info;
                        curAddTaskInfo = info;
                    }
                    if (npcStartTaskInfoDic[npcID].Count == 0)
                    {
                        npcStartTaskInfoDic.Remove(npcID);
                    }
                }
                if (curAddTaskInfo.StartSceneAnimID > 0)
                {
                    //GameCenter.sceneAnimMng.PushSceneAnima(curAddTaskInfo.StartSceneAnimID);
                }
                if (OnTaskListUpdate != null && curAddTaskInfo != null) OnTaskListUpdate(TaskDataType.Started, curAddTaskInfo.TaskType);
            }
			//FindPathAfterTaskProcess(curAddTaskInfo);//放到AddNewTask事件前面，防止新手引导的事件还在寻路
            if (AddNewTask != null)
            {
                AddNewTask(curAddTaskInfo);   
            }
            if (OnTaskGroupUpdate != null)
            {
                OnTaskGroupUpdate(refData.sort);
            }
        }
        
    }
    /// <summary>
    /// 0xD014 单个任务变化
    /// </summary>
    /// <param name="_cmd"></param>
    protected void S2C_OnTaskUpdate(Pt _pt)
    {
        //Debug.Log(" 单个任务变化：pt_update_task_d014");
        interruptAutoTask = false;
        //ringTask = null;
        pt_update_task_d014 msg = _pt as pt_update_task_d014;
        if (msg != null&&msg.tasks.Count>0)
        {
            TaskInfo info = null;
            Dictionary<int, TaskInfo> dic = null;
            st.net.NetBase.task_list_info data = msg.tasks[0];
            //Debug.Log("单个任务变化 S2C_OnTaskUpdate:" + data.taskid + ",step:" + data.taskstep + ",state:" + data.state);
			TaskStepRef stepRef = ConfigMng.Instance.GetTaskStepRef((int)data.taskid,(int)data.taskstep);
			if (stepRef == null)
            {
                GameSys.LogError("任务更新失败，找不到任务" + (int)data.taskid + "的静态配置！");
                return;
            }
			//Debug.Log(stepRef.sort + ":" + data.taskid +":"+data.taskstep);
			if (taskInfoDic.TryGetValue(stepRef.sort, out dic))
            { 
                if (dic.TryGetValue((int)data.taskid, out info))
                {
                    //ringTask = info;
                    if (info.TaskState == TaskStateType.Finished)//任务完成时给出完成任务的提示
                    {
                    //    GameCenter.messageMng.AddClientMsg("任务完成");
						if(OnTaskFinishEvent != null)
							OnTaskFinishEvent();
						//TaskRewardTip(info);  有通用提示了,后台提示
						GuideAfterFinishTask(info);//交完任务,引导添加
                    }
					bool getNewTask = false;
					//任务状态从未接到进行中为获得新任务
					if(info.TaskType == TaskType.Main && info.TaskState == TaskStateType.UnTake)
					{
						getNewTask = true;
					}
                    info.UpdateSingle(data);

					if(getNewTask)
						GuideAfterGetNewTask(info);//获得新任务,引导添加

					if (data.state == 3 && info.TaskType != TaskType.Trial)//不删除试练任务
                    {
						dic.Remove(info.ID);
						endedTaskInfoDic[info.TaskType][info.ID] = info;
                        if (OnTaskListUpdate != null) OnTaskListUpdate(TaskDataType.Ended, info.TaskType);
						if(info.TaskType == TaskType.Main)
						{
							GameCenter.messageMng.AddClientMsg(470);//主线任务结束提示
						}
                    }
                    if (updateSingleTask != null)
                    {
                        updateSingleTask(info);
                    }
					if((info.TaskGuideType == GuideType.FINISHSTAY || info.TaskGuideType == GuideType.STAY) && info.TaskState == TaskStateType.Finished)
					{
						//引导过程中完成任务,不自动任务
						interruptAutoTask = true;
					}
					if(!interruptAutoTask && !outsideInterruptAutoTask)
					{
						if(info.TaskType == TaskType.Main)
						{
                            switch (info.TaskState)
                            { 
                                case TaskStateType.UnTake:
                                    FindPathAfterTaskUnTake(info);
                                    break;
                                case TaskStateType.Process:
                                    FindPathAfterTaskProcess(info);
                                    break;
                                case TaskStateType.Finished:
                                    FindPathAfterTaskFinish(info);
                                    break;
                            }
							
						}
                        if (info.TaskType == TaskType.Ring)
                        {
                            DoSomethingAfterUpdateRingTask(info);
                        }
					}
                }
            }
			if(OnTaskGroupUpdate != null)
				OnTaskGroupUpdate(stepRef.sort);
        }
        
    }

	protected bool interruptAutoTask = false;

	protected bool outsideInterruptAutoTask = false;
	/// <summary>
	/// 外部条件是否打断自动任务
	/// </summary>
	public bool OutsideInterruptAutoTask
	{
		set
		{
			outsideInterruptAutoTask = value;
		}
	}
	/// <summary>
	/// 完成任务之后引导GuideType.FINISHSTAY=3
	/// </summary>
	void GuideAfterFinishTask(TaskInfo _info)
	{
		if(_info.TaskType == TaskType.Main && _info.TaskState == TaskStateType.Finished)
		{
			if( _info.TaskGuideType == GuideType.FINISHSTAY)
			{
				interruptAutoTask = true;
				if(OnTaskGuideUpdateEvent != null)
					OnTaskGuideUpdateEvent(_info);
				//Debug.Log("OnTaskGuideUpdateEvent:"+_info.TaskGuideType);
			}
		}
        //if (GameCenter.instance.isPlatform && GameCenter.instance.isDataEyePattern)
        //{
        //    DCTask.complete(_info.EID.ToString());
        //}
	}
	/// <summary>
	/// 接到新任务之后(可能为完成or进行中)引导GuideType.STAY=1 or 4
	/// </summary>
	void GuideAfterGetNewTask(TaskInfo _info)
	{
		//引导GuideType.STAY=1
		if(_info.TaskType == TaskType.Main && _info.TaskState != TaskStateType.UnTake && _info.TaskGuideType == GuideType.STAY)
		{
			interruptAutoTask = true;
			if(OnTaskGuideUpdateEvent != null)
				OnTaskGuideUpdateEvent(_info);
			//Debug.Log("OnTaskGuideUpdateEvent:"+_info.TaskGuideType);
		}
		//引导GuideType.STAY=4
		if(_info.TaskType == TaskType.Main && _info.TaskState != TaskStateType.UnTake && _info.TaskGuideType == GuideType.MOVINGGUIDE)
		{
			if(OnTaskGuideUpdateEvent != null)
				OnTaskGuideUpdateEvent(_info);
			//Debug.Log("OnTaskGuideUpdateEvent:"+_info.TaskGuideType);
		}
        //if (GameCenter.instance.isPlatform && GameCenter.instance.isDataEyePattern)
        //{
        //    switch (_info.TaskType)
        //    {
        //        case TaskType.Main:
        //            DCTask.begin(_info.EID.ToString(), DCTaskType.DC_MainLine);
        //            break;
        //        case TaskType.Ring:
        //            DCTask.begin(_info.EID.ToString(), DCTaskType.DC_Daily);
        //            break;
        //        case TaskType.Trial:
        //            DCTask.begin(_info.EID.ToString(), DCTaskType.DC_BranchLine);
        //            break;
        //        default:
        //            DCTask.begin(_info.EID.ToString(), DCTaskType.DC_Other);
        //            break;
        //    }
        //}
	}
    /// <summary>
    /// 接到新任务自动做任务
    /// </summary>
	void FindPathAfterTaskProcess(TaskInfo _info)
    { 
        if (GameCenter.curMainPlayer.CurFSMState != MainPlayer.EventType.TASK_PATH_FIND)
        {
            //计数任务进度为0的时候才寻路任务。计数变化时不寻路
            if (_info.IsStartProgressCondition)
			{
				//接取环任务后，应该关闭UI并且立即自动寻路执行任务
				if(_info.TaskType == TaskType.Ring && GameCenter.uIMng.CurOpenType == GUIType.RINGTASK)
				{
					GameCenter.uIMng.SwitchToUI(GUIType.NONE);
                }
				GameCenter.curMainPlayer.GoTraceTask(_info);
			}
        }
        if(GameCenter.taskMng.GetRingAutoFinish(_info.StarLevel))
        {
            //Debug.Log("GameCenter.curMainPlayer.GoTraceTask(_info);");D:\xiyou\xiyou\Assets\
            if (_info.TaskType == TaskType.Ring && GameCenter.uIMng.CurOpenType == GUIType.RINGTASK)
            {
                GameCenter.uIMng.SwitchToUI(GUIType.NONE);
            }
            GameCenter.curMainPlayer.CancelCommands();
            GameCenter.curMainPlayer.GoTraceTask(_info); 
        }
    }

    /// <summary>
    /// 任务完成自动交取任务
    /// </summary>
    void FindPathAfterTaskFinish(TaskInfo _info) 
    {
        if (_info.TaskState == TaskStateType.Finished && GameCenter.curMainPlayer.CurFSMState!= MainPlayer.EventType.TASK_PATH_FIND) 
        {
            GameCenter.curMainPlayer.GoTraceTask(_info);
        }
    }

    /// <summary>
    /// 环式任务自动交接任务的特殊需求
    /// </summary>
    void DoSomethingAfterUpdateRingTask(TaskInfo _info)
    { 
        if (_info.TaskState == TaskStateType.Finished)
        {
            curRingTaskType = _info.StarLevel;
            GameCenter.curMainPlayer.GoNormal();
            GameCenter.uIMng.SwitchToUI(GUIType.RINGTASK);//环任务完成直接打开交任务界面
        }
        if (_info.TaskState == TaskStateType.Process)
        { 
            FindPathAfterTaskProcess(_info);
        }
        if (_info.TaskState == TaskStateType.UnTake)
        {

        }
    }
    /// <summary>
    /// 获得未接取的任务后,自动接任务
    /// </summary>
    void FindPathAfterTaskUnTake(TaskInfo _info)
	{
		if (GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel < _info.TaskNeedLv)
		{
			GameCenter.uIMng.SwitchToSubUI(SubGUIType.TASKTIP);
			return;
		}
		GameCenter.curMainPlayer.GoTraceTask(_info);
	}

    /// <summary>
    /// 0xD017 放弃任务
    /// </summary>
    /// <param name="_cmd"></param>
    protected void S2C_OnGiveUpTask(Pt _pt)
    {
        pt_del_task_d017 msg = _pt as pt_del_task_d017;
        if (msg != null)
        {
            if (curFocusTask != null && curFocusTask.ID == (int)msg.taskid)
            {
                curFocusTask = null;
            }
            TaskInfo info = null;
            Dictionary<int, TaskInfo> dic = null;
			TaskStepRef stepRef = ConfigMng.Instance.GetTaskStepRef((int)msg.taskid,(int)msg.taskstep);
			if (stepRef == null)
            {
                GameSys.LogError("放弃任务失败，找不到任务" + (int)msg.taskid + "的静态配置！");
                if (OnTaskListUpdate != null) OnTaskListUpdate(TaskDataType.UnStart, TaskType.UnKnow);
                return;
            }
            //Debug.Log("0xD017 放弃任务    " + stepRef.sort);
			if (taskInfoDic.TryGetValue(stepRef.sort, out dic))
            {
                int id = (int)msg.taskid; 
				if (dic.TryGetValue(id, out info))
                {
                    //如果是非起始任务，则不移动数据存放位置
                    info.Update(msg);
					dic.Remove(id);

                    //Debug.Log("TaskType :  " + info.TaskType);
                    //如果起始任务，存入到起始任务表   日常任务放弃就没了
                    if (info.TaskType != TaskType.Daily && info.Step == 1)
                    {
                        if (!npcStartTaskInfoDic.ContainsKey(info.TakeFromNPCID))
                        {
                            npcStartTaskInfoDic[info.TakeFromNPCID] = new Dictionary<int, TaskInfo>();
                        }
						if (!npcStartTaskInfoDic[info.TakeFromNPCID].ContainsKey(info.ID))
                        {
							npcStartTaskInfoDic[info.TakeFromNPCID][info.ID] = info;
                        }
                    }
                    if (OnTaskListUpdate != null) OnTaskListUpdate(TaskDataType.UnStart, info.TaskType);
                }
            }
        }
    }

    /// <summary>
    /// 提交任务返回,用于:背包满时关NPC界面
    /// </summary>
    /// <param name="_cmd"></param>
    protected void S2C_OnCommitTask(Pt _pt)
    {
        //Debug.Log("S2C_OnCommitTask");
        pt_finish_task_d016 finish_task = _pt as pt_finish_task_d016;
        if (finish_task != null)
        {
            GameCenter.uIMng.SwitchToUI(GUIType.NONE);
        }
    }
    ///
    protected void S2C_OnRingTaskFlyOver(Pt _pt)
    {
        //Debug.Log("飞了飞了上天了");
        pt_surround_task_fly_state_c120 flyOver = _pt as pt_surround_task_fly_state_c120;
        if(flyOver != null)
        {
            
            //Debug.Log("飞完 之后寻路");
            if (GetCurRingTask != null)
            GameCenter.curMainPlayer.GoTraceTask(GetCurRingTask);
            else
            {
                Debug.LogError("当前环任务为空不能飞");
            }
        }
    }
    #endregion

    #region C2S
    /// <summary>
    /// 获取当前主玩家的任务列表
    /// </summary>
    public void C2S_ReqTaskList()
    {
        pt_req_task_list_d01c msg = new pt_req_task_list_d01c();
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 请求接受任务
    /// </summary>
    /// <param name="_taskID"></param>
    /// <param name="_step"></param>
    public void C2S_ReqAcceptTask(int _taskID, int _step)
    {
        //Debug.Log("请求接受任务："+ _taskID+","+"任务步骤：+"+_step);
        pt_accept_task_d015 msg = new pt_accept_task_d015();
		msg.seq = NetMsgMng.CreateNewSerializeID();
        msg.taskid = (uint)_taskID;
        msg.taskstep = (uint)_step;
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 请求完成任务
    /// </summary>
    /// <param name="taskety"></param>
    public void C2S_ReqCommitTask(int _taskID, int _step)
    {
        //Debug.Log("C2S_ReqCommitTask(int _taskID, int _step)");
        pt_finish_task_d016 msg = new pt_finish_task_d016();
		msg.seq = NetMsgMng.CreateNewSerializeID();
        msg.taskid = (uint)_taskID;
        msg.taskstep = (uint)_step;
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 请求放弃任务去掉改成刷新试练次数
    /// </summary>
    /// <param name="taskety"></param>
    //public void C2S_ReqRefreshTaskNum(int _taskID, int _step)
    //{
    //    pt_del_task_d017 msg = new pt_del_task_d017();
    //    msg.taskid = (uint)_taskID;
    //    msg.taskstep = (uint)_step;
    //    NetMsgMng.SendMsg(msg);

    //}

    /// <summary>
    /// 请求刷新试练次数
    /// </summary>
    public void C2S_ReqRefreshTaskNum()
    {
        pt_req_refresh_shilian_task_c141 msg = new pt_req_refresh_shilian_task_c141(); 
        NetMsgMng.SendMsg(msg);

    }

    /// <summary>
    /// 请求传进副本
    /// </summary>
    /// <param name="taskety"></param>
    public void C2S_AskFlyToScene(int _sceneID)
    {
      //  Debug.Log(_sceneID);
        pt_scene_fly_scene_d134 msg = new pt_scene_fly_scene_d134();
        msg.fly_scene_id = _sceneID;
        NetMsgMng.SendMsg(msg);

    }

	public void C2S_ChatWithNpc(int npc)
	{ 
		pt_task_count_talk_to_npc_d699 msg = new pt_task_count_talk_to_npc_d699();
		msg.npc_id = npc;
		NetMsgMng.SendMsg(msg);
	}

    #endregion
    #endregion

    #region 辅助逻辑

    #region 任务数据获得
    /// <summary>
    /// 获取已接的指定的任务类型列表
    /// </summary>
    /// <param name="_type"></param>
    /// <returns></returns>
    public Dictionary<int, TaskInfo> GetTaskDic(TaskType _type)
    {
       // Debug.Log(taskInfoDic[_type].Count + ":" + _type);
        if (taskInfoDic.ContainsKey(_type))
        {
            return taskInfoDic[_type];
        }
        else
        {
            return new Dictionary<int, TaskInfo>();
        }
    }
	/// <summary>
	/// 获取所有任务,包括已经结束的
	/// </summary>
	public List<TaskInfo> GetAllTaskDic(TaskType _type)
	{
		List<TaskInfo> taskList = new List<TaskInfo>(GetTaskDic(_type).Values);
		using(var e = endedTaskInfoDic[_type].GetEnumerator())
		{
			while(e.MoveNext())
			{
				taskList.Add(e.Current.Value);
			}
		}
		return taskList;
	}
	/// <summary>
	/// 添加前台特殊任务 by邓成
	/// </summary>
	public void AddSpecialTask()
	{
		if(taskInfoDic.ContainsKey(TaskType.Special))
		{
			taskInfoDic[TaskType.Special].Clear();
		}else
		{
			taskInfoDic[TaskType.Special] = new Dictionary<int, TaskInfo>();
		}
		TaskInfo info = new TaskInfo(10001,1,TaskStateType.Process);
		if(info != null)taskInfoDic[TaskType.Special][info.ID] = info;
		info = new TaskInfo(10002,1,TaskStateType.Process);
		if(info != null)taskInfoDic[TaskType.Special][info.ID] = info;
		info = new TaskInfo(10003,1,TaskStateType.Process);
		if(info != null)taskInfoDic[TaskType.Special][info.ID] = info;
		info = new TaskInfo(10004,1,TaskStateType.Process);
		if(info != null)taskInfoDic[TaskType.Special][info.ID] = info;
	}

    /// <summary>
    /// 获取已接的指定的任务
    /// </summary>
    /// <returns></returns>
    public TaskInfo GetTaskInfo(int _taskID)
    {
		using(var e = taskInfoDic.GetEnumerator())
		{
			while(e.MoveNext())
			{
				var item = e.Current.Value;
				if (item.ContainsKey(_taskID))
				{
					return item[_taskID];
				}
			}
		}
        return null;
    }

    /// <summary>
    /// 获取已接的主线任务
    /// </summary>
    /// <returns></returns>
    public TaskInfo GetMainTaskInfo()
    {
		using(var e = taskInfoDic.GetEnumerator())
		{
			while(e.MoveNext())
			{
				var item = e.Current.Value;
				if (item.ContainsKey((int)TaskType.Main))
				{
					return item[(int)TaskType.Main];
				}
			}
		}
        return null;
    }
    /// <summary>
    /// 判断任务是否在身上
    /// </summary>
    public bool HaveMainTask(int _task, int _step)
    {
        if (taskInfoDic.ContainsKey(TaskType.Main))
        {
            Dictionary<int, TaskInfo> mainTaskDic = taskInfoDic[TaskType.Main];
            if (mainTaskDic.ContainsKey(_task))
            {
                return (mainTaskDic[_task].Step <= _step);
            }
        }
        return false;
    }
	/// <summary>
	/// 预告领奖 是否到了领奖的任务
	/// </summary>
	public bool CurTaskCanGetReward(int _task,int _step)
	{
		if(taskInfoDic.ContainsKey(TaskType.Main))
		{
            //Debug.Log("功能预告到了领奖");
			Dictionary<int,TaskInfo> mainTaskDic = taskInfoDic[TaskType.Main];
			if(mainTaskDic.ContainsKey(_task))
			{
				return (mainTaskDic[_task].Step >= _step);
			}
		}
		return false;
	}

	/// <summary>
	/// 获取当前主线任务的步骤
	/// </summary>
	public int GetMainTaskStep()
	{
		int taskStep = 0;
		bool hasMain = false;//有未完成的任务，或者完成了未交的任务
		Dictionary<int, TaskInfo> mainTask = taskInfoDic[TaskType.Main];
		using(var e = mainTask.GetEnumerator())
		{
			while(e.MoveNext())
			{
				var info = e.Current.Value;
				if(info.TaskState != TaskStateType.UnTake)
				{
					hasMain = true;
					taskStep = info.Step;
					break;
				}
			}
		}
		if(hasMain == false)//任务列表中没有主线任务，还在NPC上
		{
			using(var e = mainTask.GetEnumerator())
			{
				while(e.MoveNext())
				{
					var info = e.Current.Value;
					if(info.Step > taskStep)
						taskStep = info.Step;	
				}
			}
		}	
		return taskStep;
	}
    /// <summary>
    /// 检测任务寻路结束后是否需要进入自动战斗(寻路打怪or采集)
    /// </summary>
    public bool IsTaskNeedAutoFight(TaskInfo _taskInfo)
    {
        List<TaskConditionRef> taskConList = _taskInfo.ConditionRefList;
        for (int i = 0, length = taskConList.Count; i < length; i++)
        {
            TaskConditionRef task = taskConList[i];
            switch (task.sort)
            {
                case TaskConditionType.KillAnyKindMonster:
                case TaskConditionType.KillAnyKindMonsterAnyScene:
                case TaskConditionType.KillLevel35Monster:
                case TaskConditionType.KillLevel40Monster:
                case TaskConditionType.KillLevel45Monster:
                case TaskConditionType.KillLevel50Monster:
                case TaskConditionType.KillOneKindMonster:
                case TaskConditionType.KillOneSceneMonster:
                case TaskConditionType.CollectAnyItem:
                case TaskConditionType.CollectSceneItem:
                    return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 任务线是否已经全部完成
    /// </summary>
    /// <returns></returns>
    public bool IsTaskEnded(int _taskID,TaskType _type = TaskType.UnKnow)
    {
        if (_type == TaskType.UnKnow)
        {
			using(var e = endedTaskInfoDic.GetEnumerator())
			{
				while(e.MoveNext())
				{
					var item = e.Current.Value;
					if (item.ContainsKey(_taskID))
					{
						return true;
					}
				}
			}
            return false;
        }
        else
        {
            return endedTaskInfoDic.ContainsKey(_type) && endedTaskInfoDic[_type].ContainsKey(_taskID);
        }
    }
	/// <summary>
	/// 是否有已接取or已完成的试炼任务  by邓成
	/// </summary>
	public bool HaveTrialTask()
	{
		Dictionary<int, TaskInfo> taskDic = GameCenter.taskMng.GetTaskDic(TaskType.Trial);
		using(var e = taskDic.GetEnumerator())
		{
			while(e.MoveNext())
			{
				var item = e.Current.Value;
                if (item.TaskState != TaskStateType.UnTake && item.TaskState != TaskStateType.ENDED) return true;
			}
		}
		return false;
	}
	/// <summary>
	/// 是否有已接取or已完成的环任务任务  by邓成  
	/// </summary>
	public bool HaveRingTask()
	{
		Dictionary<int, TaskInfo> taskDic = GameCenter.taskMng.GetTaskDic(TaskType.Ring);
		using(var e = taskDic.GetEnumerator())
		{
			while(e.MoveNext())
			{
				var item = e.Current.Value;
				if(item.TaskState != TaskStateType.UnTake)return true;
			}
		}
		return false;
	}
    /// <summary>
    /// 是否显示该环任务
    /// </summary> 
    public bool IsShowThisRingTask(int _lev)
    {
        for (int i = 0, max = ringTaskProgress.Count; i < max; i++)
        {
            if (ringTaskProgress[i].task_sort == _lev && ringTaskProgress[i].finish_num > 0&&ringTaskProgress[i].finish_num <10)
            {
                return true;
            }
        } 
        Dictionary<int, TaskInfo> taskDic = GameCenter.taskMng.GetTaskDic(TaskType.Ring);
        using (var e = taskDic.GetEnumerator())
        {
            while (e.MoveNext())
            {
                var item = e.Current.Value;
                if (item.StarLevel == _lev && item.TaskState != TaskStateType.UnTake) return true;
            }
        }
        return false;
    }
    /// <summary>
    /// （显示环任务的条件）环任务开始做了，环任务接了
    /// </summary>
    /// <returns></returns>
    public bool IsShowSpecislRingTask()
    { 
        for (int i = 0, max = ringTaskProgress.Count; i < max; i++)
        {
            if (ringTaskProgress[i].finish_num > 0 && ringTaskProgress[i].finish_num < 10)
            {
                return false;
            }
        }
        Dictionary<int, TaskInfo> taskDic = GameCenter.taskMng.GetTaskDic(TaskType.Ring);
        using (var e = taskDic.GetEnumerator())
        {
            while (e.MoveNext())
            {
                var item = e.Current.Value;
                if (item.TaskState != TaskStateType.UnTake) return false;
            }
        } 
        return true;
    }

    /// <summary>
    /// 任务线是否正在进行中
    /// </summary>
    /// <returns></returns>
    public bool IsTaskStepping(int _taskID, TaskType _type = TaskType.UnKnow)
    {
        if (_type == TaskType.UnKnow)
        {
			using(var e = taskInfoDic.GetEnumerator())
			{
				while(e.MoveNext())
				{
					var item = e.Current.Value;
					if (item.ContainsKey(_taskID))
					{
						return true;
					}
				}
			}
            return false;
        }
        else
        {
            return taskInfoDic.ContainsKey(_type) && taskInfoDic[_type].ContainsKey(_taskID);
        }
    }

    /// <summary>
    /// 单个任务的完成状态
    /// </summary>
    /// <returns></returns>
    public bool IsTaskState(int _taskID, int _taskStep, TaskType _type = TaskType.UnKnow) 
    {
        int mainRow = 0;
        Dictionary<int, TaskInfo> mainTask = GetTaskDic(TaskType.Main);
        if (mainTask != null)
        {
			using(var e = mainTask.GetEnumerator())
			{
				while(e.MoveNext())
				{
					mainRow = e.Current.Value.TaskRow;
					break;
				}
			}
            TaskStepRef taskRef = ConfigMng.Instance.GetTaskStepRef(1, _taskStep);//只查询主线任务
            if (taskRef != null)
            {
                if (taskRef.taskRow < mainRow) return true;
                if (taskRef.taskRow >= mainRow) return false;
            }
        }
        return false;
    }


    /// <summary>
    /// 该起始任务是否可接
    /// </summary>
    /// <param name="_taskID"></param>
    /// <param name="_type"></param>
    /// <returns></returns>
    public bool IsStartTaskCanTake(TaskStepRef _startTask)
    {
        if (_startTask == null) return false; 
        if (_startTask.need_task == 0) return true;
		using(var e = taskInfoDic.GetEnumerator())
		{
			while(e.MoveNext())
			{
				var dictionary = e.Current.Value;
				TaskInfo checkTask = null;
				if (dictionary.TryGetValue(_startTask.need_task, out checkTask))
				{ 
					if (checkTask.Step == 0 || checkTask.Step > _startTask.need_task_step)
					{ 
						return true;
					}
				}
			}
		}
        if (taskInfoDic.ContainsKey(TaskType.Main))//主线任务做完
        { 
            if (taskInfoDic[TaskType.Main].Count <= 0) return true;
        }
        return false;
    }
    /// <summary>
    /// 获取已开始但尚未接的任务线任务
    /// </summary>
    /// <param name="_type"></param>
    /// <returns></returns>
    public List<TaskInfo> GetCanTakeSteppingTaskList(TaskType _type)
    {
        List<TaskInfo> canTakeSteppingTask = new List<TaskInfo>();//已开始的任务线可接任务列表
        Dictionary<int, TaskInfo> dic = GetTaskDic(_type);
        if (dic != null && dic.Count > 0)
        {
			using(var e = dic.GetEnumerator())
			{
				while(e.MoveNext())
				{
					var item = e.Current.Value;
					if (item.TaskState == TaskStateType.UnTake && item.ReqLevel <= GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel)
					{
						canTakeSteppingTask.Add(item);
					}
				}
			}
        }
        return canTakeSteppingTask;
    }



    /// <summary>
    /// 对任务排序
    /// 的需求：按任务的完成状态(已可交，未接，已接)，任务的类型(主线，支线，日常)，任务的等级降序。
    /// </summary>
    /// <param name="_a"></param>
    /// <param name="_b"></param>
    /// <returns></returns>
    protected int SortForNPC(TaskInfo _a, TaskInfo _b)
    {
        if (_b.TaskState - _a.TaskState != 0) return _b.TaskState - _a.TaskState;
        return _a.TaskType - _b.TaskType;
    }

 
    /// <summary>
    /// 任务奖励提示
    /// </summary>
    protected void TaskRewardTip(TaskInfo _task)
    {
        if (_task == null) return;
        if (_task.TaskType == TaskType.Guild) return;
        GameCenter.messageMng.AddClientMsg(32);
        if (_task.RewardList.Count > 0)
        {
            List<EquipmentInfo> eqList = new List<EquipmentInfo>();
			for(int i = 0,max=_task.RewardList.Count; i < max; i++) {
				var item = _task.RewardList[i];
				eqList.Add(new EquipmentInfo(item.eid, item.count, EquipmentBelongTo.PREVIEW));
			}
            GameCenter.messageMng.AddClientMsg(eqList, 34);
        }
    }

	public void TaskProgressTip(string _progressTip)
	{
		if(OnTaskProgressUpdateEvent != null)
			OnTaskProgressUpdateEvent(_progressTip);
	}

    #endregion


    #region 环式任务

    #region 环任务类型
    /// <summary>
    /// 当前环任务难度
    /// </summary>
    public int curRingTaskType = 1; 
    #endregion
    /// <summary>
	/// 获取当前环任务
	/// </summary>
	public TaskInfo GetCurRingTask
	{
        get
        {
            Dictionary<int, TaskInfo> infoDic = GetTaskDic(TaskType.Ring);
            if (infoDic.Count == 0) return null;
            using (var e = infoDic.GetEnumerator())
            {
                while (e.MoveNext())
                {
                    TaskInfo taskInfo = e.Current.Value;
                    if (taskInfo.StarLevel == curRingTaskType)//add 新增区分类型
                    {
                        //Debug.Log(" GetCurRingTask :  " + taskInfo.StarLevel + "  , id : " + taskInfo.ID + "  ,step : " + taskInfo.Step);
                        return taskInfo;
                    }
                }
            }
            return null;
        }
	}
    public void C2S_FinishAllRingTask(int _type)
	{
        //Debug.Log("C2S_FinishAllRingTask");
        pt_req_finish_all_surround_task_by_vip_d332 msg = new pt_req_finish_all_surround_task_by_vip_d332();
        msg.surround_task_id = (uint)_type;
		NetMsgMng.SendMsg(msg);
	}

	/// <summary>
	/// 刷新环式任务
	/// </summary>
	public void C2S_ResetRingTask(int _difficuilty)
	{
        //Debug.Log("C2S_ResetRingTask");
        //HaveFiveFold = true;
        pt_req_fresh_surround_task_star_d330 msg = new pt_req_fresh_surround_task_star_d330();
        msg.refresh_difficulty = (uint)_difficuilty;
		NetMsgMng.SendMsg(msg);
	}

    /// <summary>
    /// 请求获取各环任务的进度和次数
    /// </summary>
    public void C2S_GetRingTaskPrograss()
    { 
        pt_req_task_surround_ui_info_c142 msg = new pt_req_task_surround_ui_info_c142();
        NetMsgMng.SendMsg(msg);
    }

	public void C2S_AskAcceptRingTask()
	{ 
        if (GetCurRingTask != null)
			C2S_ReqAcceptTask(GetCurRingTask.ID,GetCurRingTask.Step);
	}
	public void C2S_AskFinishRingTask()
	{
        //Debug.Log("请求完成任务");
		if(GetCurRingTask != null)
			C2S_ReqCommitTask(GetCurRingTask.ID,GetCurRingTask.Step);
	}
	public void C2S_AskDiamondFinishRingTask()
	{
		//Debug.Log("C2S_AskDiamondFinishRingTask");
		if(GetCurRingTask != null)
		{
			pt_req_finish_surround_task_by_diamo_d331 msg = new pt_req_finish_surround_task_by_diamo_d331();
			msg.taskid = (uint)GetCurRingTask.ID;
			msg.taskstep = (uint)GetCurRingTask.Step;
			NetMsgMng.SendMsg(msg);
		}
	}
    /// <summary>
    /// 各环任务是否完成
    /// </summary>
    public Dictionary<int, bool> ringTypeIsFinish = new Dictionary<int, bool>();
	/// <summary>
	/// 所有的环任务都完成了
	/// </summary>
	public bool FinishAllRingTask = false;
    /// <summary>
    /// 环任务进度
    /// </summary>
    public List<task_surround_info> ringTaskProgress = new List<task_surround_info>();
    /// <summary>
    ///得到星级和环数
    /// </summary>
    protected void S2C_GetStarAndCount(Pt _pt)
    { 
        pt_wanted_task_d132 data = _pt as pt_wanted_task_d132;
        if (data != null)
        { 
            TaskStepRef taskRef = ConfigMng.Instance.GetTaskStepRef((int)data.wanted_task_id, (int)data.wanted_task_step); 
            curRingTaskType = taskRef.startLv;
            ringTypeIsFinish[curRingTaskType] = data.wanted_task_loop == 10;//该环任务类型完成了 
            //HaveFiveFold = (data.five_fold_state == 0 ? false : true);
            if (GetCurRingTask != null) GetCurRingTask.UpdateStar((int)data.wanted_task_star, (int)data.wanted_task_loop);
            if (OnTaskGroupUpdate != null)
                OnTaskGroupUpdate(TaskType.Ring);
        }
    }
    /// <summary>
    /// 获得环任务各类型进度和次数
    /// </summary> 
    protected void S2C_GetRingTaskPrograss(Pt _pt)
    { 
        pt_update_task_surround_ui_info_c143 pt = _pt as pt_update_task_surround_ui_info_c143;
        if (pt != null)
        { 
            ringTaskProgress.Clear();
            ringTaskProgress = pt.task_surround;
            ringTaskProgress.Sort(SortRingTaskType); 
            for (int i = 0, max = pt.task_surround.Count; i < max; i++)
            {
                //Debug.Log("  difficulty : " + pt.task_surround[i].task_sort + "   , finish_num : " + pt.task_surround[i].finish_num + "    , refreshnum : " + pt.task_surround[i].surplus_refresh_num);
                ringTypeIsFinish[(int)pt.task_surround[i].task_sort] = pt.task_surround[i].finish_num == 10;
            } 
            bool isFinish = true;
            for (int i = 0, max = pt.task_surround.Count; i < max; i++)
            {
                if (pt.task_surround[i].finish_num < 10 || pt.task_surround[i].surplus_refresh_num > 0)
                {
                    isFinish = false;
                    break;
                } 
            }
            FinishAllRingTask = isFinish; 
            if (OnTaskGroupUpdate != null)
                OnTaskGroupUpdate(TaskType.Ring);
        }
    }
    protected int SortRingTaskType(task_surround_info _one, task_surround_info _two)
    {
        if (_one.task_sort > _two.task_sort)
            return 1;
        if (_one.task_sort < _two.task_sort)
            return -1;
        return 0;
    }
	/// <summary>
	/// 完成所有试炼任务
	/// </summary>
	public bool FinishAllTrialTask()
	{ 
		Dictionary<int, TaskInfo> taskDic = GameCenter.taskMng.GetTaskDic(TaskType.Trial);
		using(var e = taskDic.GetEnumerator())
		{
			while(e.MoveNext())
			{
				var item = e.Current.Value;
				if(item.TaskState != TaskStateType.ENDED)return false;
			}
		}
		return true;  
	}
	/// <summary>
	/// 试炼任务的剩余刷新次数  
	/// </summary>
	public int TrialTaskRestRewardTimes = 0;
	/// <summary>
	/// 试炼任务的剩余刷新次数更新事件
	/// </summary>
	public Action OnUpdateTrialTaskRestRewardTimes;
	/// <summary>
	/// 获取试炼任务的剩余刷新次数  
	/// </summary>
	protected void S2C_GetTrialTaskRestRewardTimes(Pt _pt)
	{
		pt_shilian_task_info_d698 data = _pt as pt_shilian_task_info_d698;
		if(data != null)
		{ 
			TrialTaskRestRewardTimes = data.num;
			if(OnUpdateTrialTaskRestRewardTimes != null)
				OnUpdateTrialTaskRestRewardTimes();
			if(OnTaskGroupUpdate != null)
				OnTaskGroupUpdate(TaskType.Trial);	
		}
	}
    #endregion

    #region 试炼任务
    public const int PRIMARYTRIALTASK = 1;
	public const int MIDDLETRIALTASK = 2;
	public const int SENIORTRIALTASK = 3;
	public TaskInfo GetTrialTaskByLevel(int _level)
	{
        if (taskInfoDic.ContainsKey(TaskType.Trial))
        {
            Dictionary<int, TaskInfo> taskDic = taskInfoDic[TaskType.Trial];
            using (var e = taskDic.GetEnumerator())
            {
                while (e.MoveNext())
                {
                    TaskInfo taskInfo = e.Current.Value; 
                    if (taskInfo.StarLevel == _level)
                        return taskInfo;
                }
            }
        } 
		return null;
	}

	public void C2S_FinishAllTrialTask()
	{
		pt_req_finish_all_shilian_task_by_vip_d333 msg = new pt_req_finish_all_shilian_task_by_vip_d333();
		NetMsgMng.SendMsg(msg);
	}

	public void C2S_AskAcceptTrialTask(TaskInfo taskInfo)
	{
		C2S_ReqAcceptTask(taskInfo.ID,taskInfo.Step);
	}

	public void C2S_GetTrialTaskReward(TaskInfo taskInfo)
	{
		C2S_ReqCommitTask(taskInfo.ID,taskInfo.Step);
	}
     
    public void C2S_RefreshTrialTaskNum()
    {
        C2S_ReqRefreshTaskNum();
    }

	public void C2S_QuickFinishTrialTask(TaskInfo taskInfo)
	{
		pt_req_finish_shilian_task_by_diamo_d334 msg = new pt_req_finish_shilian_task_by_diamo_d334();
		msg.taskid = (uint)taskInfo.ID;
		msg.taskstep = (uint)taskInfo.Step;
		NetMsgMng.SendMsg(msg);
	}
	#endregion

    #region 任务关联NPC

    /// <summary>
    /// 获取指定NPC身上的所有任务并排序（已完全结束的任务被剔除） by吴江
    /// 编写初衷是供 NPC对话界面使用
    /// </summary>
    /// <param name="_npcID"></param>
    /// <returns></returns>
    public List<TaskInfo> GetNPCTaskList(int _npcID)
    {
        
        List<TaskInfo> list = new List<TaskInfo>();
        if (npcStartTaskInfoDic.ContainsKey(_npcID))
        {
            PlayerConfig curRef = ConfigMng.Instance.GetPlayerConfig(GameCenter.mainPlayerMng.MainPlayerInfo.Prof);
			using(var e = npcStartTaskInfoDic[_npcID].GetEnumerator())
			{
				while(e.MoveNext())
				{
					var item = e.Current.Value;
					PlayerConfig needRef = ConfigMng.Instance.GetPlayerConfig(item.ReqProf);
					bool profOk = false;
					if (curRef != null && needRef != null)
					{
						int curFlag = curRef.proFlag;
						int needFlag = needRef.proFlag;
						profOk = (curFlag & needFlag) == needFlag;
					}
					if (item.ReqLevel <= GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel && profOk && item.TaskType != TaskType.Offer)//排除每日任务LZR
					{
						if (item.TaskCamp == CampType.None)
						{
							list.Add(item);
						}
						else 
						{

						}

					}
				}
			}
        }
		using(var e = taskInfoDic.GetEnumerator())
		{
			while(e.MoveNext())
			{
				var taskDic = e.Current.Value;
				using(var t = taskDic.GetEnumerator())
				{
					while(t.MoveNext())
					{
                        var task = t.Current.Value; 
						if (task.TakeFromNPCID == _npcID && task.TaskState == TaskStateType.UnTake && task.TaskType != TaskType.Offer)//排除每日任务LZR
						{
							list.Add(task);
						}
						else if (task.CommitNPCID == _npcID && task.TaskState != TaskStateType.UnTake && task.TaskType != TaskType.Offer)//排除每日任务LZR
						{
							list.Add(task);
						}else if(task.NeedChatWithNpc(_npcID) && task.TaskState == TaskStateType.Process)
						{
							list.Add(task);
						}
					}
				}
			}	
		}
        list.Sort(SortForNPC);
        return list;
    }


	protected void OnMainPlayerLevelChange(ActorBaseTag _tag, ulong _value, bool _fromAbility)
    {
        if (_tag == ActorBaseTag.Level)
        {
            CalculateNPCStartTask();
        }
    }

    /// <summary>
    /// 计算整个游戏的尚可接的"起始"任务，存入npc任务字典
    /// </summary>
    /// <param name="_npcID"></param>
    /// <returns></returns>
    protected void CalculateNPCStartTask()
    {
        npcStartTaskInfoDic.Clear();
        FDictionary npcIDList = ConfigMng.Instance.GetNpcRefTable();
        foreach (int id in npcIDList.Keys)
        {
            Dictionary<int, TaskInfo> startList = InitNPCStartTaskDic(id);
            if (startList != null)
            {
                npcStartTaskInfoDic[id] = startList;
            }
        }
    }

    /// <summary>
    /// 获取指定npc的可接的“起始”任务列表（日常除外）
    /// 必须已收到服务端发过来的已接/已完成列表以及主玩家信息才可使用这个方法。
    /// </summary>
    /// <param name="_npcID"></param>
    /// <returns></returns>
    protected Dictionary<int, TaskInfo> InitNPCStartTaskDic(int _npcID)
    {
        MainPlayerInfo playerInfo = GameCenter.mainPlayerMng.MainPlayerInfo;
        if (playerInfo == null) return null;
		int level = playerInfo.CurLevel;
        List<TaskStepRef> list = ConfigMng.Instance.GetStartTaskListFromNPC(_npcID);
        if (list != null)
        {
            Dictionary<int, TaskInfo> npcTaskList = new Dictionary<int, TaskInfo>();
			for (int i = 0,max=list.Count; i < max; i++) {
				var item = list[i];
				if (item.sort != TaskType.Daily && item.need_lev <= level && !IsTaskEnded(item.task, item.sort) && !IsTaskStepping(item.task, item.sort) && IsStartTaskCanTake(item))
				{
					npcTaskList[item.task] = new TaskInfo(item.task, 1, TaskStateType.UnTake);
				}
			}
            return npcTaskList;
        }
        return null;
    }

    /// <summary>
    /// 获取指定npc的可接的“起始”任务列表（日常除外）
    /// 必须已收到服务端发过来的已接/已完成列表以及主玩家信息才可使用这个方法。
    /// </summary>
    /// <param name="_npcID"></param>
    /// <returns></returns>
    public Dictionary<int, TaskInfo> GetNPCStartTaskDic(int _npcID)
    {
        if (npcStartTaskInfoDic.ContainsKey(_npcID))
        {
            return npcStartTaskInfoDic[_npcID];
        }
        return null;
    }

    #endregion

    #region 任务寻路

    /// <summary>
    /// “提取”当前任务寻路队列中的第一个。 by吴江
    /// </summary>
    /// <returns></returns>
    public TaskSinglePath PopTaskPath()
    {
        if (taskSinglePath == null || taskSinglePath.Count == 0) return null;
        TaskSinglePath temp = taskSinglePath[0];
        taskSinglePath.RemoveAt(0);
        return temp;
    }
    /// <summary>
    /// 当前寻路的剩余路径数量
    /// </summary>
    public int RestPathCount
    {
        get { return taskSinglePath.Count; }
    }
	/// <summary>
	/// 清空寻路,点击地面需要  by邓成
	/// </summary>
	public void ClearTaskPath()
	{
		taskSinglePath.Clear();
	}

    /// <summary>
    /// 为当前关注的任务创建任务路径列表 by吴江
    /// </summary>
    public void BuildCurFocusTaskPath()
    {
        if (CurfocusTask == null) return; 
        switch (CurfocusTask.TaskState)
        {
            case TaskStateType.Process:
                switch (CurfocusTask.ContentType)
                {
                    case TaskContentType.NPC_FUNCTION:

                        TraceToNpc(CurfocusTask.ContentValue);
                        break;
                    case TaskContentType.GOTO_SCENE:

                        TraceToScene(CurfocusTask.ContentValue);
                        break;
                    case TaskContentType.DO_SOMETHING:
                        bool finished = true;
                        for (int i = 0; i < CurfocusTask.ConditionCount; i++)
                        {
                            if (CurfocusTask.ProgressConditionList.Count > i && CurfocusTask.ProgressConditionList[i] < CurfocusTask.ConditionRefList[i].number)
                            {
                                TraceToAction(CurfocusTask, CurfocusTask.ConditionRefList[i]);
                                finished = false;
                                break;
                            }
                        }
                        if (finished)
                        {
                            TraceToNpc(CurfocusTask.CommitNPCID);
                        }
                        break;
				case TaskContentType.NONE:
					for (int i = 0; i < CurfocusTask.ConditionCount; i++)
					{
						if (CurfocusTask.ProgressConditionList.Count > i && CurfocusTask.ProgressConditionList[i] < CurfocusTask.ConditionRefList[i].number)
						{
							TraceToAction(CurfocusTask.ConditionRefList[i]);
							break;
						}
					}
					break;
                }
                break;
            case TaskStateType.UnTake:
                TraceToNpc(CurfocusTask.TakeFromNPCID);
                break;
            case TaskStateType.Finished:
                TraceToNpc(CurfocusTask.CommitNPCID);
                break;
            default:
                break;
        }
    }


    /// <summary>
    /// 为任务行为寻路(寻路到NPC或副本传送点，需要寻路的行为) by吴江
    /// </summary>
    /// <param name="_ref"></param>
    protected void TraceToAction(TaskInfo _info, TaskConditionRef _ref)
    {
      //  Debug.Log(_ref.sort);
        //这个主线任务需要走新手引导，特殊处理，（不科学）
//        if (_info.ID == 1 && _info.Step == 26)
//        {
//            return;
//        }
        taskSinglePath.Clear();
        if (_ref == null) return;
        //Debug.Log(_ref.sort);
        switch (_ref.sort)
        {

            case TaskConditionType.KillAnyKindMonster:
            case TaskConditionType.KillAnyKindMonsterAnyScene:
            case TaskConditionType.KillOneKindMonster:
			case TaskConditionType.KillOneSceneMonster:
			case TaskConditionType.KillLevel35Monster:
			case TaskConditionType.KillLevel40Monster:
			case TaskConditionType.KillLevel45Monster:
			case TaskConditionType.KillLevel50Monster:
                TraceToScene(_info.ContentValue, _info.TargetPos);
                break;
            case TaskConditionType.FinishAnyKindMap:
            case TaskConditionType.FinishOneKindMap:
                //GameCenter.uIMng.SwitchToUI(GUIType.DUNGEON);
                break;
            case TaskConditionType.CollectSceneItem:
			case TaskConditionType.CollectAnyItem:
               // TraceToFlyPoint(_ref.data);
              //  TraceToScene(_ref.data);
                TraceToScene(_info.ContentValue, _info.TargetPos);
                break;
            default:
             //   TraceToScene(_info.ContentValue, _info.TargetPos);
                break;
        }
    }

    /// <summary>
    /// 任务行为(不需要寻路的行为) by吴江
    /// </summary>
    /// <param name="_ref"></param>
    public void TraceToAction(TaskInfo _info)
    {
        if (_info == null ) return;
        //GameCenter.uIMng.SwitchToUI(GUIType.MAINFIGHT);
        //GameCenter.uIMng.SwitchToUI(GUIType.TASK);
        //GameCenter.uIMng.SwitchToUI(GUIType.LITTLEMAP);
		if (GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel < _info.TaskNeedLv)
        {
            //string des = " 需要玩家达到" + curTargetTaskInfo.TaskNeedLv + "级";
            //  GameCenter.messageMng.AddClientMsg(des);
			GameCenter.uIMng.SwitchToSubUI(SubGUIType.TASKTIP);
        }
        else
        {
        //    Debug.Log(GameCenter.curMainPlayer.GoTraceTask(_info) + ":" + _info.ContentType);
			if(_info.JumpToGUIType != GUIType.NONE && _info.TaskState == TaskStateType.Process)//打开界面的任务
			{
				GameCenter.uIMng.SwitchToUI(_info.JumpToGUIType);
				return;
			}
			GoTraceTask(_info);
        }
    }
	/// <summary>
	/// 任务行为(需要寻路的行为) by邓成
	/// </summary>
	public void GoTraceTask(TaskInfo _info)
	{
		if (_info == null )
		{
			Debug.LogError("当前任务为null!");
			return;
		}
		if (!GameCenter.curMainPlayer.GoTraceTask(_info))
		{
			if (_info.ContentType == TaskContentType.DO_SOMETHING)
			{
				for (int i = 0; i < _info.ConditionCount; i++)
				{
					if (_info.ProgressConditionList.Count > i && _info.ProgressConditionList[i] < _info.ConditionRefList[i].number)
					{
						//   Debug.Log("@@@@@@@@@@@@");
						if (TraceToAction(_info.ConditionRefList[i]))
						{
							break;
						}
					}
				}
			}
		}
	}
    /// <summary>
    /// 任务行为(不需要寻路的行为) by吴江
    /// </summary>
    /// <param name="_ref"></param>
    public static bool TraceToAction(TaskConditionRef _ref)
    {
       // Debug.Log(_ref.sort);
        int data = _ref.data;
        switch (_ref.sort)
        {
            case TaskConditionType.FinishStoryMap:
                {
                    if (GameCenter.mainPlayerMng.MainPlayerInfo.SceneID == data)
                    {
                        GameCenter.curMainPlayer.GoAutoFight();
                    }
                    else
                    {
                        GameCenter.taskMng.C2S_AskFlyToScene(_ref.data);
                    }
                }
                break;
            case TaskConditionType.StrenthenEquipment:
                {
					GameCenter.uIMng.SwitchToSubUI(SubGUIType.STRENGTHING);
                } 
                break;
            case TaskConditionType.SubmitOneKindItem:
            case TaskConditionType.ArmyEquipment:
                {
                //    GameCenter.uIMng.SwitchToUI(GUIType.BACKPACK);
					GameCenter.inventoryMng.GuideEquipmentTask((EquipSlot)_ref.data);
                }
                break;
        }
        return false;
    }


    /// <summary>
    /// 寻路到指定npc by吴江
    /// </summary>
    /// <param name="_npcID"></param>
    protected void TraceToNpc(int _npcID)
    {
        CurTargetID = 0;
        CurTargetSceneID = 0;
        CurTargetPoint = Vector3.zero;
        NPCAIRef Ref = ConfigMng.Instance.GetNPCAIRefByType(_npcID);
        if (Ref != null)
        {
			CurTargetID = _npcID;
            CurTargetSceneID = Ref.scene;
            CurTargetPoint = new Vector3(Ref.pointX, 0, Ref.pointY);
        }

        taskSinglePath.Clear();
        if (_npcID < 0) return;
        NPC npc = null;
        FDictionary npcDic = GameCenter.sceneMng.NPCInfoDictionary;
        if (npcDic != null)
        {
            foreach (NPCInfo item in npcDic.Values)
            {
                if (item.Type == _npcID)
                {
                    npc = GameCenter.curGameStage.GetNPC(item.ServerInstanceID);
                    if (npc != null) break;
                }
            }
        }
        if (npc != null)//如果npc在本场景内，则直接寻路过去
        {
            taskSinglePath.Add(new TaskSinglePath(npc));
        }
        else//如果npc不在场景内，那么先寻路到npc所在场景，再寻路到npc
        {
            NPCAIRef refData = ConfigMng.Instance.GetNPCAIRefByType(_npcID);
            if (refData != null)
            {
                TraceToScene(refData.scene);
            }
            taskSinglePath.Add(new TaskSinglePath(ObjectType.NPC, _npcID));
        }
    }

    /// <summary>
    /// 寻路到指定场景
    /// 战斗失败界面用到
    /// </summary>
    /// <param name='_sceneID'>
    /// _scene I.
    /// </param>
    public void PublicTraceToScene(int _sceneID)
    {
        TraceToScene(_sceneID);
    }


    public void PublicTraceToNpc(int _npcType)
    {
        TraceToNpc(_npcType);
		GameCenter.curMainPlayer.GoTraceTask();
    }

    /// <summary>
    /// 寻路到指定场景的指定坐标 by吴江
    /// </summary>
    /// <param name="_sceneID"></param>
    public void TraceToScene(int _sceneID, Vector3 _pos)
    {
        CurTargetID = 0;
        CurTargetSceneID = _sceneID;
        CurTargetPoint = _pos;
        taskSinglePath.Clear();
        if (_sceneID < 0) return;

        PlayGameStage stage = GameCenter.curGameStage as PlayGameStage;
        if (stage == null) return;

        if (_sceneID == stage.SceneID)
        {
            taskSinglePath.Add(new TaskSinglePath(_pos));
            if (TryTrace != null)
            {
                GameCenter.curMainPlayer.CurTargetPoint = new TracePoint(_sceneID, _pos);
                TryTrace(_sceneID, _pos);
            }
            return;
        }

        List<int> flyList = new List<int>();
        List<int> forbidList = new List<int>();
        if (GetPathFlyPointList(stage.SceneID, _sceneID, ref flyList, ref forbidList))
        {
			for (int i = 0,max=flyList.Count; i < max; i++) {
				taskSinglePath.Add(new TaskSinglePath(ObjectType.FlyPoint, flyList[i]));
			}
			taskSinglePath.Add(new TaskSinglePath(_pos));
            if (TryTrace != null)
            {
                GameCenter.curMainPlayer.CurTargetPoint = new TracePoint(_sceneID, _pos);
                TryTrace(_sceneID, _pos);
            }
        }
    }

    /// <summary>
    /// 寻路到指定场景 by吴江
    /// </summary>
    /// <param name="_sceneID"></param>
    protected void TraceToScene(int _sceneID)
    {

        taskSinglePath.Clear();
        if (_sceneID < 0) return;
        PlayGameStage stage = GameCenter.curGameStage as PlayGameStage;
        if (stage == null) return;
        if (_sceneID == stage.SceneID) return;
        List<int> flyList = new List<int>();
        List<int> forbidList = new List<int>();
        // Debug.Log(GetPathFlyPointList(stage.SceneID, _sceneID, ref flyList, ref forbidList));
        if (GetPathFlyPointList(stage.SceneID, _sceneID, ref flyList, ref forbidList))
        {

            for (int i = 0; i < flyList.Count; i++)
            {
                taskSinglePath.Add(new TaskSinglePath(ObjectType.FlyPoint, flyList[i]));
            }
        }
    }

    /// <summary>
    /// 寻路到指定传送点 by吴江
    /// </summary>
    /// <param name="_sceneID"></param>
    protected void TraceToFlyPoint(int _id)
    {

        taskSinglePath.Clear();
        if (_id < 0) return;
        InteractiveObject fly = GameCenter.curGameStage.GetObject(ObjectType.FlyPoint, _id);
        if (fly == null)//如果传送点在本场景内，则直接寻路过去
        {
            taskSinglePath.Add(new TaskSinglePath(fly));
        }
        else//如果传送点不在场景内，那么先寻路到传送点所在场景，再寻路到传送点
        {
            PlayGameStage stage = GameCenter.curGameStage as PlayGameStage;
            if (stage == null) return;
            FlyPointRef flyRef = ConfigMng.Instance.GetFlyPointRef(_id);
            if (flyRef != null)
            {
                List<int> flyList = new List<int>();
                List<int> forbidList = new List<int>();
                if (GetPathFlyPointList(stage.SceneID, flyRef.scene, ref flyList, ref forbidList))
                {
					for (int i = 0,max=flyList.Count; i < max; i++) {
						taskSinglePath.Add(new TaskSinglePath(ObjectType.FlyPoint, flyList[i]));
					}
                    taskSinglePath.Add(new TaskSinglePath(ObjectType.FlyPoint, _id));
                }
            }
        }
    }

    /// <summary>
    /// 获得从指定场景到另外一个指定场景所需要经过的传送门 by吴江
    /// </summary>
    /// <param name="_curScene">出发场景id</param>
    /// <param name="_targetScene">目标场景id</param>
    /// <param name="_flyPoints">传送门id列表</param>
    /// <returns></returns>
    public bool GetPathFlyPointList(int _curScene, int _targetScene, ref List<int> _flyPoints, ref List<int> _forbidPoints)
    {
        if (_curScene == _targetScene)
        {
            return true;
        }
        _forbidPoints.Add(_curScene);
        List<FlyPointRef> list = ConfigMng.Instance.GetFlyPointRefByScene(_curScene);
        if (list.Count == 0) return false;
		for (int i = 0,max=list.Count; i < max; i++) {
			FlyPointRef item = list[i];
			if (item.sort == FlyPointSort.targetScene || item.sort == FlyPointSort.recall) //直接进入（主城）的传送门之间递归寻找 by吴江
			{
				if (item.targetScene == _targetScene)
				{
					_flyPoints.Add(item.id);
					return true;
				}
				else
				{
					if (_forbidPoints.Contains(item.targetScene)) continue;
					List<int> laterPoints = new List<int>();
					if (GetPathFlyPointList(item.targetScene, _targetScene, ref laterPoints, ref _forbidPoints))
					{
						_flyPoints.Add(item.id);
						for (int j = 0,maxJ=laterPoints.Count; j < maxJ; j++) {
							_flyPoints.Add(laterPoints[j]);
						}
						return true;
					}
				}
			}
			else if (item.sort == FlyPointSort.openUI) //选择性传送门（地下城）之间，根据目前的具体需求，不做递归深入寻找。（以后如果地下城里也有传送门，则需要加上） by吴江
			{
			}
		}
        return false;
    }

    #endregion



    #endregion
}


/// <summary>
/// 任务的单个路径   一整个任务步骤的寻径可能有多个这样的数据组成  by吴江
/// 因为目标可能跨场景，所以提供了只需要设置类型和id的构造。
/// 在到达正确场景后，再根据类型和id去获取寻路目标对象实例，然后再开始寻路
/// </summary>
public class TaskSinglePath
{
    public ObjectType targetType;
    public int targetID;
    public bool inited = false;
    public InteractiveObject target;
    public Vector3 targetPos;


    public TaskSinglePath(Vector3 _targetPos)
    {
        targetPos = _targetPos;
        inited = true;
    }

    public TaskSinglePath(ObjectType _type, int _id)
    {
        targetType = _type;
        targetID = _id;
        inited = false;
    }

    public TaskSinglePath(InteractiveObject _taget)
    {
        inited = true;
        target = _taget;
        if (_taget == null)
        {
            GameSys.LogError("传入的目标对象为空！无法寻路！");
            inited = false;
            return;
        }
        targetType = _taget.typeID;
        targetID = _taget.id;
    }
    /// <summary>
    /// 尝试根据路径信息，获取路径目标对象。获取到了则返回真。
    /// </summary>
    /// <returns></returns>
    public bool TryGenTarget()
    {
        if (targetPos != Vector3.zero)
        {
            return true;
        }
        if (!inited || target == null)
        {
            if (targetType == ObjectType.NPC)//如果未完成初始化，并且类型是npc，那么说明当时寻路的时候查找的是下一个场景的npc，这个时候只有类型id，而无实例id，只能查找。
            {
                if (targetID < 0) return false;
                NPC npc = null;
                FDictionary npcDic = GameCenter.sceneMng.NPCInfoDictionary;
                if (npcDic != null)
                {
                    foreach (NPCInfo item in npcDic.Values)
                    {
                        if (item.Type == targetID)
                        {
                            npc = GameCenter.curGameStage.GetNPC(item.ServerInstanceID);
                            if (npc != null) break;
                        }
                    }
                }
                target = npc;
                inited = target != null;
            }
            else
            {
                target = GameCenter.curGameStage.GetObject(targetType, targetID);
                inited = target != null;
            }
        }
        return inited;
    }
}

public class TracePoint
{
   public  int scenID = 0;
   public  Vector3 targetPoint = Vector3.zero;
    public TracePoint(int _scenID,Vector3 _targetPoint)
    {
        this.scenID = _scenID;
        this.targetPoint = _targetPoint;

    }

}
