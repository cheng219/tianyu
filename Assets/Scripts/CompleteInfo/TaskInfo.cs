//====================================
//作者：吴江
//日期：2015/10/21
//用途：任务数据层对象（Info结尾的类名都为数据层对象，包含 服务端数据  客户端静态数据   访问器 三部分）
//=====================================



using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;



/// <summary>
/// 任务数据层对象 by吴江
/// </summary>
public class TaskInfo  {

    /// <summary>
    /// 任务星级
    /// </summary>
    public int taskStar = 1;

	public int taskLoop = 1;

    /// <summary>
    /// 服务端数据
    /// </summary>
    protected st.net.NetBase.task_list_info data = null;

    /// <summary>
    /// 客户端配置数据
    /// </summary>
    protected TaskStepRef refData = null;
    protected TaskStepRef RefData
    {
        get
        {
          //  Debug.Log(refData + ":" + refData.id + ":" + (int)data.taskid + ":" + refData.step + ":" + (int)data.taskstep);
            if (refData == null || refData.id != (int)data.taskid || refData.step != (int)data.taskstep)
            {
                refData = ConfigMng.Instance.GetTaskStepRef((int)data.taskid, (int)data.taskstep);
            }
            return refData;
        }
    }


    /// <summary>
    /// 构造
    /// </summary>
    /// <param name="_data">服务端任务数据</param>
    public TaskInfo(st.net.NetBase.task_list_info _data)
    {
        data = _data;
    }


    /// <summary>
    /// 构造
    /// </summary>
    /// <param name="_data">服务端任务数据</param>
    public TaskInfo(int _taskID, int _taskStep, TaskStateType _state)
    {
        data = new st.net.NetBase.task_list_info();
        data.taskid = (uint)_taskID;
        data.taskstep = (uint)_taskStep;
        data.state = (uint)_state;
        isServerDirty = true;
    }
    /// <summary>
    /// 是否由客户端构造
    /// </summary>
    protected bool isServerDirty;
    /// <summary>
    /// 是否由客户端构造
    /// </summary>
    public bool IsServerDirty
    {
        get { return isServerDirty; }
    }


    /// <summary>
    /// 数据变化
    /// </summary>
    /// <param name="_data"></param>
    public void Update(st.net.NetBase.task_list_info _data)
    {
        if (_data.taskid != data.taskid) return;
        data.taskstep = _data.taskstep;
        data.state = _data.state;
        data.c1_num = _data.c1_num;
        data.c2_num = _data.c2_num;
        data.c3_num = _data.c3_num;

        if (OnPropertyUpdate != null)
        {
            OnPropertyUpdate();
        }
        isServerDirty = false;
    }

    /// <summary>
    /// 数据变化
    /// </summary>
    /// <param name="_data"></param>
    public void Update(pt_del_task_d017 _data)
    {
        if (_data.taskid != data.taskid) return;
        data.taskstep = _data.taskstep;
        if (OnPropertyUpdate != null)
        {
            OnPropertyUpdate();
        }
        isServerDirty = false;
    }

    /// <summary>
    /// 数据变化
    /// </summary>
    /// <param name="_data"></param>
    public void UpdateSingle(st.net.NetBase.task_list_info _data)
    {
        
        if (_data.taskid != data.taskid) return;
        int oldTaskNum = 0;
        data.taskstep = _data.taskstep;
        data.state = _data.state;

        oldTaskNum = (int)data.c1_num;
        data.c1_num = _data.c1_num;
        TaskShowNumText(oldTaskNum, (int)data.c1_num, 0);

        oldTaskNum = (int)data.c2_num;
        data.c2_num = _data.c2_num;
        TaskShowNumText(oldTaskNum, (int)data.c2_num, 1);

        oldTaskNum = (int)data.c3_num;
        data.c3_num = _data.c3_num;
        TaskShowNumText(oldTaskNum, (int)data.c2_num, 2);

        if (OnPropertyUpdate != null)
        {
            OnPropertyUpdate();
        }
    }

    /// <summary>
    /// 更新任务的星级和环数
    /// </summary>
	public void UpdateStar(int _star,int _taskLoop)
    {
        taskStar = _star;
		taskLoop = _taskLoop;
    }


    public void CheckComplete()
    {
        bool complete = true;
        if (RefData != null)
        {
            switch (RefData.condtionRefList.Count)
            {
                case 1:
                    complete = (int)data.c1_num >= RefData.condtionRefList[0].number;
                    break;
                case 2:
                    complete = (int)data.c1_num >= RefData.condtionRefList[0].number && (int)data.c2_num >= RefData.condtionRefList[1].number;
                    break;
                case 3:
                    complete = (int)data.c1_num >= RefData.condtionRefList[0].number && (int)data.c2_num >= RefData.condtionRefList[1].number && (int)data.c3_num >= RefData.condtionRefList[2].number;
                    break;
                default:
                    break;
            }
			if ((TaskStateType)data.state != TaskStateType.UnTake && (TaskStateType)data.state != TaskStateType.ENDED)
            {
                if (complete)
                {
                    data.state = (uint)TaskStateType.Finished;
                }
                else
                {
                    data.state = (uint)TaskStateType.Process;
                }
            }
        }
    }

    #region 访问器
    /// <summary>
    /// 任务唯一ID by吴江
    /// </summary>
    public int ID
    {
        get { return (int)data.taskid; }
    }
	/// <summary>
	/// 配置ID
	/// </summary>
	public int EID
	{
		get{ return RefData == null ? 0 : RefData.id; }
	}
	
	/// <summary>
	/// 任务静态ID
	/// </summary>
	public int Task
	{
		get{ return RefData == null ? 0 : RefData.task; }
	}

    /// <summary>
    /// 任务步骤（为0则意味着到了任务链终点，已经结束）
    /// </summary>
    public int Step
    {
        get { return (int)data.taskstep; }
    }

    /// <summary>
    /// 任务类型 by吴江
    /// </summary>
    public TaskType TaskType
    {
        get { return RefData == null ? TaskType.UnKnow : RefData.sort; }
    }

    /// <summary>
    /// 需要完成的前置任务
    /// </summary>
    public int NeedTaskID
    {
        get { return RefData == null ? 0 : RefData.need_task; }
    }
    /// <summary>
    /// 需要完成的前置任务步骤
    /// </summary>
    public int NeedTaskStep
    {
        get { return RefData == null ? 0 : RefData.need_task_step; }
    }

    /// <summary>
    /// 目标点
    /// </summary>
    public Vector3 TargetPos
    {
        get
        {
            return RefData == null ? Vector3.zero : RefData.targetCoordiante;
        }
    }

    /// <summary>
    /// 当前任务的完成状况
    /// </summary>
    public TaskStateType TaskState
    {
        get {
			if(TaskType != TaskType.Special)//特殊任务,不检查了
            	CheckComplete();
            return (TaskStateType)data.state; 
        }
    }

    /// <summary>
    /// 任务序列号
    /// </summary>
    public int SerializeID
    {
        get { return RefData == null ? -1 : RefData.step; }
    }
    /// <summary>
    /// 任务需求等级
    /// </summary>
    public int ReqLevel
    {
        get { return RefData == null ? 0 : RefData.need_lev; }
    }

    public bool SpecialTaskCanShow
    {
        get
        {
            if (RefData == null) return false;
            if (ReqLevel <= GameCenter.mainPlayerMng.MainPlayerInfo.Level)
            {
                return GameCenter.taskMng.IsStartTaskCanTake(RefData);
            }
            return false;
        }
    }

    /// <summary>
    /// 任务需求职业
    /// </summary>
    public int ReqProf
    {
        get { return RefData == null ? -1 : RefData.prof; }
    }

    /// <summary>
    /// 任务名称
    /// </summary>
    public string TaskName
    {
        get 
		{ 
			string tip = string.Empty;
			string levTip = string.Empty;
            string name = "[00ff00]{0}[-]";
			if(TaskNeedLv > GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel)
                levTip = "[FF2929](" + GameHelper.GetLevelStr(TaskNeedLv) + ConfigMng.Instance.GetUItext(182);
			switch(TaskType)
			{
			case TaskType.Main:
                    tip = ConfigMng.Instance.GetUItext(183);
				break;
			case TaskType.Ring:
                tip = "[00ff00]" + ConfigMng.Instance.GetUItext(181) + (taskLoop + 1).ToString() + ConfigMng.Instance.GetUItext(184);
				break;
			case TaskType.Trial:
                tip = ConfigMng.Instance.GetUItext(185);
				break;
			}
            return RefData == null ? string.Empty : (string.Format(name, RefData.name) + levTip + tip); 
		}
    }

    /// <summary>
    /// 任务条件数量
    /// </summary>
    public int ConditionCount
    {
        get { return RefData == null ? 0 : RefData.condtionRefList.Count; }
    }

	/// <summary>
	/// 标识该任务目标为:与特定NPC对话
	/// </summary>
	public bool NeedChatWithNpc(int npcId)
	{
		for (int i = 0; i < ConditionCount; i++) {
			if(RefData.condtionRefList[i].sort == TaskConditionType.ChatWithNpc && RefData.condtionRefList[i].data == npcId)
				return true;
		}
		return false;
	}

    /// <summary>
    /// 任务星级
    /// </summary>
    public int StarLevel
    {
        get { return RefData == null ? 0 : RefData.startLv; }
    }

	/// <summary>
	/// 任务目标怪物ID
	/// </summary>
	public int TargetMosterID 
	{
		get 
		{
			return 0;//RefData == null || RefData.targetMonsterID <= 0 ? -1 : RefData.targetMonsterID;  需要配置任务怪物ID
		}
	}

    /// <summary>
    /// 任务进度文字
    /// </summary>
    /// <param name="_index"></param>
    /// <returns></returns>
    public string GetConditionProgress(int _index)
    {
        if (RefData == null || RefData.condtionRefList.Count <= _index)
        {
            return string.Empty;
        }
        int count = 0;
        switch (_index)
        {
            case 0:
                count = data.c1_num >= RefData.condtionRefList[_index].number ? RefData.condtionRefList[_index].number : (int)data.c1_num;
                break;
            case 1:
                count = data.c2_num >= RefData.condtionRefList[_index].number ? RefData.condtionRefList[_index].number : (int)data.c2_num;
                break;
            case 2:
                count = data.c3_num >= RefData.condtionRefList[_index].number ? RefData.condtionRefList[_index].number : (int)data.c3_num;
                break;
            default:
                break;
        }
       
        return count + "/" + RefData.condtionRefList[_index].number;//任务计数超过用静态数据LZR
    }

    /// <summary>
    /// 接受任务对话框任务详情介绍文字
    /// </summary>
    public string TakeFromNPCDialogText
    {
        get
        {
            if (RefData == null) return string.Empty;
            if (RefData.takeFromNpc.text == "0") return string.Empty;
            StringBuilder strb = new StringBuilder(RefData.takeFromNpc.text);

            for (int i = 0; i < RefData.condtionRefList.Count; i++)
            {
                uint num = 0;
                switch (i)
                {
                    case 0:
                        num = data.c1_num;
                        break;
                    case 1:
                        num = data.c2_num;
                        break;
                    case 2:
                        num = data.c3_num;
                        break;
                    default:
                        break;
                }

                strb.Replace("#" + (i + 1), "[00EE00]" + num + "/" + RefData.condtionRefList[i].number + "[-]");
            }

            return strb.ToString();
        }
    }

    /// <summary>
    /// 任务窗口展示的文字
    /// </summary>
    public string TaskShowText
    {
        get
        {
            string text = string.Empty;
            if (RefData == null) return text;
            text = RefData.detail_talk;
            int count = Mathf.Min(ProgressConditionList.Count,RefData.condtionRefList.Count);
            for (int i = 0; i < count; i++)
            {
                string completeStr = ProgressConditionList[i] >= RefData.condtionRefList[i].number ? "[-][00ff00]" + RefData.condtionRefList[i].number.ToString() + "[-]" ://任务计数超过用静态数据LZR
                     "[-][ff0000]" + ProgressConditionList[i].ToString() + "[-]";
                string needStr = "[-][00ff00]/" + RefData.condtionRefList[i].number.ToString() + "[-]";
                text = text.Replace("#" + (i + 1).ToString(), completeStr + needStr);
            }
            return text;
        }
    }

    public string TaskGoalText
    {
        get
        {
            if (RefData != null)
            {
				if(TaskState == TaskStateType.UnTake)
				{
					if(TaskType == TaskType.Special)return Description;
					if(TaskType == TaskType.Main)
					{
						int npcId = RefData.takeFromNpc.npcID;
						NPCTypeRef npc = ConfigMng.Instance.GetNPCTypeRef(npcId);
						if(npc != null)
                            return ConfigMng.Instance.GetUItext(186) + npc.name + ConfigMng.Instance.GetUItext(187);
                        return ConfigMng.Instance.GetUItext(188);
					}
				}
				if(TaskState == TaskStateType.Process)
				{
					if(TaskType == TaskType.Special)return Description;
	                string taskText = RefData.tasknumDetails.Count > 0 ? RefData.tasknumDetails[0] : string.Empty;
	                string taskTextContainTaskNum = string.Empty;
	                if (RefData == null) return string.Empty;
	                int count = Mathf.Min(ProgressConditionList.Count, RefData.condtionRefList.Count);
	                for (int i = 0; i < count; i++)
	                {
	                    string text = RefData.tasknumDetails[i];
	                    string completeStr = ProgressConditionList[i] >= RefData.condtionRefList[i].number ? "[00ff00]" + RefData.condtionRefList[i].number.ToString() + "[-]" ://任务计数超过用静态数据LZR
	                         "[ff0000]" + ProgressConditionList[i].ToString() + "[-]";
	                    string needStr = "[00ff00]/" + RefData.condtionRefList[i].number.ToString() + "[-]";
	                    text = text.Replace("#" + (i + 1).ToString(), completeStr + needStr);
	                    taskTextContainTaskNum += text;

	                }
	                return string.Equals(taskTextContainTaskNum, string.Empty) ? taskText : taskTextContainTaskNum;
				}
				if(TaskState == TaskStateType.Finished)
				{
					int npcId = RefData.commitToNpc.npcID;
					NPCTypeRef npc = ConfigMng.Instance.GetNPCTypeRef(npcId);
					if(npc != null)
                        return ConfigMng.Instance.GetUItext(191) + npc.name + ConfigMng.Instance.GetUItext(189);
                    return ConfigMng.Instance.GetUItext(190);
				}
            }
            return String.Empty;
        }
    }



    /// <summary>
    /// 数据变化客户端提示
    /// </summary>
    public void TaskShowNumText(int _oldTaskNum,int _curTaskNum,int _num)
    {
         string text = string.Empty;
         if (RefData == null ) return ;
         if (_oldTaskNum == _curTaskNum || _curTaskNum == 0) return ;
         int tempNum = Mathf.Min(_num, RefData.condtionRefList.Count);
         if (tempNum == RefData.condtionRefList.Count)
             tempNum = RefData.condtionRefList.Count - 1;
         if (RefData.condtionRefList[tempNum].number == 0 || RefData.tasknumDetails == null) return; 
         text = RefData.tasknumDetails.Count > _num ? RefData.tasknumDetails[_num] : string.Empty;
         string completeStr = _curTaskNum >= RefData.condtionRefList[tempNum].number ? "[00ff00]" + RefData.condtionRefList[tempNum].number.ToString() + "[-]" ://任务计数超过用静态数据LZR
              "[ff0000]" + _curTaskNum.ToString() + "[-]";
         string needStr = "[00ff00]/" + RefData.condtionRefList[tempNum].number.ToString() + "[-]";
         text = text.Replace("#" + (_num + 1).ToString(), completeStr + needStr);
         if (!string.Equals(text, "0"))
         {
         //    if (refData.sort != TaskType.Guild) GameCenter.messageMng.AddClientMsg(35, new string[] { text }); 
			GameCenter.taskMng.TaskProgressTip(text);
         }
         
    }

    public string DailyTaskNumText //每日任务数量条件文本LZR
    {
        get
        {
            string text = string.Empty;
            if (RefData == null) return text;
            int count = Mathf.Min(ProgressConditionList.Count, RefData.condtionRefList.Count);
            for (int i = 0; i < count; i++)
            {
                string completeStr = ProgressConditionList[i] >= RefData.condtionRefList[i].number ? "[-][00ff00]" + ProgressConditionList[i].ToString() + "[-]" :
                     "[-][ff0000]" + ProgressConditionList[i].ToString() + "[-]";
                string needStr = "[-][00ff00]/" + RefData.condtionRefList[i].number.ToString() + "[-]";
                text =  completeStr + needStr;
            }
            return text;
        }
    } 

    /// <summary>
    /// 开始的过场动画
    /// </summary>
    public int StartSceneAnimID
    {
        get
        {
            return RefData == null ? -1 : RefData.start_movie;
        }
    }
    /// <summary>
    /// 结束的过场动画
    /// </summary>
    public int EndSceneAnimID
    {
        get
        {
            return RefData == null ? -1 : RefData.complete_movie;
        }
    }

    /// <summary>
    /// 接受任务时的图片
    /// </summary>
    public string AcceptIcon
    {
        get
        {
            return RefData == null ? string.Empty : RefData.accept_icon;
        }
    }
    /// <summary>
    /// 完成任务时的图片
    /// </summary>
    public string FinishIcon
    {
        get
        {
            return RefData == null ? string.Empty : RefData.finish_icon;
        }
    }


    /// <summary>
    /// 提交任务对话框任务详情介绍文字
    /// </summary>
    public string CommitToNPCDialogText
    {
        get
        {
            if (RefData == null) return string.Empty;
            if (RefData.commitToNpc.text == "0") return string.Empty;
            StringBuilder strb = new StringBuilder(RefData.commitToNpc.text);
            for (int i = 0; i < RefData.condtionRefList.Count; i++)
            {
                uint num = 0;
                switch (i)
                {
                    case 0:
                        num = data.c1_num;
                        break;
                    case 1:
                        num = data.c2_num;
                        break;
                    case 2:
                        num = data.c3_num;
                        break;
                    default:
                        break;
                }

                strb.Replace("#" + (i + 1), "[00EE00]" + num + "/" + RefData.condtionRefList[i].number + "[-]");
            }
            return strb.ToString();
        }
    }

    /// <summary>
    /// 任务简略描述
    /// </summary>
    public string SimpleDes
    {
        get
        {

            string text = string.Empty;
            if (RefData == null) return text;
            text = RefData.task_details;
            int count = Mathf.Min(ProgressConditionList.Count, RefData.condtionRefList.Count);
            for (int i = 0; i < count; i++)
            {
                string completeStr = ProgressConditionList[i] >= RefData.condtionRefList[i].number ? "[-][00ff00]" + ProgressConditionList[i].ToString() + "[-]" :
                     "[-][ff0000]" + ProgressConditionList[i].ToString() + "[-]";
                string needStr = "[-][00ff00]/" + RefData.condtionRefList[i].number.ToString() + "[-]";
                text = text.Replace("#" + (i + 1).ToString(), completeStr + needStr);
            }
            return text;
        }
    }
    /// <summary>
    /// 任务描述  eg ： 击败30个大头蛇    add by zsy 用于环任务
    /// </summary>
    public string TaskDes
    {
        get
        { 
            string text = string.Empty;
            if (RefData == null) return text;
            text = RefData.task_details;
            int count = Mathf.Min(ProgressConditionList.Count, RefData.condtionRefList.Count);
            for (int i = 0; i < count; i++)
            { 
                string needStr = "[-][00ff00]" + RefData.condtionRefList[i].number.ToString() + "[-]";
                text = text.Replace("#" + (i + 1).ToString(), needStr);
            }
            return text;
        }
    }
    /// <summary>
    /// 任务简略描述(只显示达到了的数超过任务数读任务数)add zsy
    /// </summary>
    public string SimpleDesUntilComplete
    {
        get
        {

            string text = string.Empty;
            if (RefData == null) return text;
            text = RefData.task_details;
            int count = Mathf.Min(ProgressConditionList.Count, RefData.condtionRefList.Count);
            for (int i = 0; i < count; i++)
            {
                string completeStr = ProgressConditionList[i] >= RefData.condtionRefList[i].number ? "[-][00ff00]" + RefData.condtionRefList[i].number + "[-]" :
                     "[-][ff0000]" + ProgressConditionList[i].ToString() + "[-]";
                string needStr = "[-][00ff00]/" + RefData.condtionRefList[i].number.ToString() + "[-]";
                text = text.Replace("#" + (i + 1).ToString(), completeStr + needStr);
            }
            return text;
        }
    }

    public string TaskDescription 
    {
        get 
        {
            if (TaskState == TaskStateType.UnTake)
            {
                return Description;
            }
            else 
            {
                return TaskShowText;
            }
        }
    }


    /// <summary>
    /// 任务描述
    /// </summary>
    public string Description
    {
        get
        {
            return RefData == null ? string.Empty : RefData.task_details;
        }
    }

    /// <summary>
    /// 任务奖励列表
    /// </summary>
    public List<ItemValue> RewardList
    {
        get 
		{ 
			List<ItemValue> itemList = new List<ItemValue>();
			if(RefData == null)
				return itemList;
			itemList.AddRange(RefData.rewardList);
			if(RewardCoin != 0)itemList.Add(new ItemValue(5,RewardCoin));
			if(RewardExp != 0)itemList.Add(new ItemValue(3,RewardExp));
			return itemList;
		}
    }

    /// <summary>
    /// 任务奖励的金币
    /// </summary>
    public int RewardCoin
    {
        get
        {
            if (RefData != null)
            {
                return RefData.coin;
            }
            return 0;
        }
    }

    /// <summary>
    /// 任务奖励的经验
    /// </summary>
    public int RewardExp
    {
        get 
        {
            if (RefData != null)
            {
                return RefData.exp;
            }
            return 0;
        }
    }

    /// <summary>
    /// 是否为战斗力达标任务
    /// </summary>
    public bool IsPowerValueTask
    {
        get {
            if (RefData == null) return false;
            if (RefData.condtionRefList.Count == 0) return false;
            return RefData.condtionRefList[0].sort == TaskConditionType.HaveOneKindItem && RefData.condtionRefList[0].data == 34;//TO DO：这种配置判断的方式需要修改得更严谨 by吴江
        }
    }

    /// <summary>
    /// 接任务的NPCID
    /// </summary>
    public int TakeFromNPCID
    {
        get { return RefData != null ? RefData.takeFromNpc.npcID:-1 ; }
    }


    /// <summary>
    /// 交任务的NPCID
    /// </summary>
    public int CommitNPCID
    {
        get { return RefData != null?  RefData.commitToNpc.npcID:-1 ; }
    }

    /// <summary>
    /// 预览的怪物ID
    /// </summary>
    public int PreviewBossID
    {
        get
        {
            return RefData == null ? -1 : RefData.bossID;
        }
    }
    /// <summary>
    /// 需要跳转的UI
    /// </summary>
    public GUIType JumpToGUIType
    {
        get
        {
            return RefData == null ? GUIType.NONE : RefData.jumpToGUIType;
        }
    }
	/// <summary>
	/// 任务引导类型
	/// </summary>
	public GuideType TaskGuideType
	{
		get
		{
			return RefData == null ? GuideType.NONE : (GuideType)RefData.guide;
		}
	}

    /// <summary>
    /// 接任务的NPC ICON
    /// </summary>
    public string TakeFromNPCHeadIcon
    {
        get { return RefData != null ? RefData.takeFromNpc.npcHeadIconName : string.Empty; }
    }


    /// <summary>
    /// 交任务的NPCID  ICON
    /// </summary>
    public string CommitNPCHeadIcon
    {
        get { return RefData != null? RefData.commitToNpc.npcHeadIconName : string.Empty; }
    }

    /// <summary>
    /// 任务内容的类型
    /// </summary>
    public TaskContentType ContentType
    {
        get
        {
            if (RefData == null) return TaskContentType.NONE;
            return RefData.taskContentType;
        }
    }

    /// <summary>
    /// 任务内容的值
    /// </summary>
    public int ContentValue
    {
        get { return RefData == null ? -1 : RefData.xy; }
    }

    public List<int> ProgressConditionList
    {
        get
        {
            List<int> list = new List<int>();
            list.Add((int)data.c1_num);
            list.Add((int)data.c2_num);
            list.Add((int)data.c3_num);
            return list;
        }
    }
	/// <summary>
	/// 是否是刚开始计数任务
	/// </summary>
	public bool IsStartProgressCondition
	{
		get
		{
			return (ProgressConditionList.Count != 0 && ProgressConditionList[0] == 0);
		}
	}

    public List<TaskConditionRef> ConditionRefList
    {
        get
        {
            return RefData == null ? new List<TaskConditionRef>() : RefData.condtionRefList; 
        }
    }


	public List<int> FuncSequence
	{
		get
		{
			return RefData == null? new List<int>() :RefData.funcSequence;
		}
	}

    /// <summary>
    /// 任务进度数目
    /// </summary>
    public int TaskRow 
    {
        get 
        {
            return RefData == null ? -1 : RefData.taskRow;
        }
    }

    public int TaskNeedLv 
    {
        get 
        {
			return RefData == null ? 0 : RefData.need_lev;
        }
    }



    public int SortFlag
    {
        get 
        {
            if (RefData == null) return 0;
            if (RefData.sort == TaskType.Main) return 5;
            if (RefData.sort == TaskType.Daily) return 4;
            if (RefData.sort == TaskType.Grow) return 3;
            if (RefData.sort == TaskType.Branch) return 2;
            return 0;
        }
    }

    public bool TaskLineIsEnd 
    {
        get 
        {
            return data.state == 3;
        }
    }

    public CampType TaskCamp 
    {
        get 
        {
          return  RefData == null ? CampType.None : (CampType)RefData.taskCamp;
        }
    }


    #endregion

    /// <summary>
    /// 任务数据发生变化的事件 by吴江
    /// </summary>
    public Action OnPropertyUpdate;
}


