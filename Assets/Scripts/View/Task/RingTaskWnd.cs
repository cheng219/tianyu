//==============================================
//作者：邓成
//日期：2016/3/15
//用途：环式任务界面类
//==============================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;

public class RingTaskWnd : GUIBase {

    public UILabel ringTaskDes;
    public UILabel ringTaskNameLab;
    public UISprite headIcon;
    public ItemUIContainer extraRewardItem;
    public UILabel noReward;

	public UILabel ringNum;
	public UILabel taskTarget;
    public UILabel alreadyDidTask;
	public UILabel expNum;
	//public UILabel copperNum;
	public UIProgressBar rewardStar;
	//public UILabel refreshStarCost;
	//public UIButton btnRefreshStar;
	public UIButton btnAddTask;
	public UIButton btnFinishTask;
	public UIButton btnDiamondFinish;
    public UIButton btnToPatrolled;//前往巡山
    public UIButton btnRefreshNum;//刷新巡山次数
    public UILabel refreshCost;//刷新花费
    public UILabel restRefreshNum;
	public UIButton btnClose;

	public GameObject effect;

	
	public UIButton btnOneKeyFinish;
	public UISpriteEx oneKeyFinishIcon;
	//public UILabel oneKeyFinishCost;
	public UILabel labFinishRingNum;

	//public int OneKeyFinishCost = 180;
	public int MaxRingTaskLoop = 10;
	public int OneKeyFinishVipLv = 4;
	public ulong RefreshStarCost = 50;
	public int DiamondFinishCost = 10;
    public UIFxAutoActive refreshStarEffect;
    public UIToggle  autoToggle;
    public UITimer time;
    public GameObject autoFilish;
    public UIButton closeAutoFilishBtn;
    //public UILabel fiveDes1;
    //public UILabel fiveDes2;
    public GameObject cost;
    public UILabel expDes;
    public UILabel num;
    protected int refreshCostNum = 20;
    protected int restTime = 0;

    protected TaskInfo info;
    void Awake()
	{
		layer = GUIZLayer.NORMALWINDOW;
		mutualExclusion = true;
        //HideRefreshStar();
        //if (!GameCenter.taskMng.HaveFiveFold)
        //{
        //    if (btnRefreshStar != null) UIEventListener.Get(btnRefreshStar.gameObject).onClick = RefreshTaskStar;
        //    if (fiveDes1 != null)
        //        fiveDes1.gameObject.SetActive(true);
        //    if (fiveDes2 != null) fiveDes2.gameObject.SetActive(false);
        //    if (cost != null) cost.SetActive(true);
        //}
        //else
        //{
        //    btnRefreshStar.gameObject.SetActive(false);
        //    if (fiveDes1 != null)
        //        fiveDes1.gameObject.SetActive(false);
        //    if (fiveDes2 != null) fiveDes2.gameObject.SetActive(true);
        //    if (cost != null) cost.SetActive(false);
        //}
        if (btnRefreshNum != null) UIEventListener.Get(btnRefreshNum.gameObject).onClick = RefreshRestNum;
        if (btnToPatrolled != null) UIEventListener.Get(btnToPatrolled.gameObject).onClick = GotoTheTask;
        if (btnAddTask != null)UIEventListener.Get(btnAddTask.gameObject).onClick = AddTask;
        if (btnFinishTask != null) UIEventListener.Get(btnFinishTask.gameObject).onClick = FinishTask;
		if(btnDiamondFinish != null)UIEventListener.Get(btnDiamondFinish.gameObject).onClick = DiamondFinish;
		if(btnOneKeyFinish != null)UIEventListener.Get(btnOneKeyFinish.gameObject).onClick = OneKeyFinish;
		if(btnClose != null)UIEventListener.Get(btnClose.gameObject).onClick = CloseWnd;
        if (closeAutoFilishBtn != null) UIEventListener.Get(closeAutoFilishBtn.gameObject).onClick = CloseAutoFilish;
        //勾选挂机跑环的逻辑处理
        if (autoToggle != null)
        {
            EventDelegate.Remove(autoToggle.onChange, AutoOnHook);
            EventDelegate.Add(autoToggle.onChange, AutoOnHook);
            //autoToggle.value = GameCenter.taskMng.AutoFinishRingTask;
            autoToggle.value = GameCenter.taskMng.GetRingAutoFinish(GameCenter.taskMng.curRingTaskType);
            if (autoToggle.value&& autoFilish!=null&& time!=null)
            {
                autoFilish.SetActive(true);
            }
        }
    }
	protected override void OnOpen ()
	{
		base.OnOpen (); 
		ShowTaskInfo();
    }
	protected override void OnClose ()
	{
		base.OnClose (); 
	}
	protected override void HandEvent (bool _bind)
	{
		base.HandEvent (_bind);
		if(_bind)
		{
			GameCenter.taskMng.OnTaskGroupUpdate += RefreshTaskInfo;
            //GameCenter.taskMng.updateRefreshStar += HideRefreshStar;

		}else
		{
			GameCenter.taskMng.OnTaskGroupUpdate -= RefreshTaskInfo;
            //GameCenter.taskMng.updateRefreshStar -= HideRefreshStar;
        }
	}

    void ShowFinishTask()
    {
        if (taskTarget != null) taskTarget.gameObject.SetActive(false);  
        if (btnRefreshNum != null) btnRefreshNum.gameObject.SetActive(true);
        if (alreadyDidTask != null) alreadyDidTask.gameObject.SetActive(true);
        if (ringNum != null) ringNum.text = "10";
        if (labFinishRingNum != null) labFinishRingNum.text = MaxRingTaskLoop + "/" + MaxRingTaskLoop; 
    }

	void RefreshTaskInfo(TaskType _type)
	{
		if(_type == TaskType.Ring)
			ShowTaskInfo();
	}
	void ShowTaskInfo()
	{  
        if (btnRefreshNum != null) btnRefreshNum.gameObject.SetActive(false);
        if (btnAddTask != null) btnAddTask.gameObject.SetActive(false);
        if (autoToggle != null) autoToggle.gameObject.SetActive(false);
        if (alreadyDidTask != null) alreadyDidTask.gameObject.SetActive(false);
        if (btnFinishTask != null) btnFinishTask.gameObject.SetActive(false);
        if (btnDiamondFinish != null) btnDiamondFinish.gameObject.SetActive(false); 
        if (btnToPatrolled != null) btnToPatrolled.gameObject.SetActive(false);

        int curTaskType = GameCenter.taskMng.curRingTaskType;

        TaskRingRewardRef taskRingRewardRef = ConfigMng.Instance.GetTaskRingRewardRef(curTaskType);
        if (taskRingRewardRef != null)
        {
            if (ringTaskDes != null) ringTaskDes.text = taskRingRewardRef.des.Replace("\\n", "\n");
            if (ringTaskNameLab != null) ringTaskNameLab.text = taskRingRewardRef.typeName;
            if (headIcon != null) headIcon.spriteName = taskRingRewardRef.icon;
            if (extraRewardItem != null) extraRewardItem.RefreshItems(taskRingRewardRef.reward, 3, taskRingRewardRef.reward.Count);
        }
        bool isFinish = false;
        if(GameCenter.taskMng.ringTypeIsFinish.ContainsKey(curTaskType))
            isFinish = GameCenter.taskMng.ringTypeIsFinish[curTaskType];

        if (expNum != null) expNum.gameObject.SetActive(!isFinish);
        if (noReward != null) noReward.gameObject.SetActive(isFinish);
        if (btnOneKeyFinish != null)
        {
            //bool enable = (GameCenter.vipMng.VipData != null && GameCenter.vipMng.VipData.vLev >= 4 && !isFinish);
            btnOneKeyFinish.isEnabled = !isFinish;
            if (oneKeyFinishIcon != null) oneKeyFinishIcon.IsGray = isFinish ? UISpriteEx.ColorGray.Gray : UISpriteEx.ColorGray.normal;
        } 
        if (isFinish)//该类环任务已经完成
        { 
            restTime = 0;
            int allRefreshTime = 0;
            for (int i = 0, max = GameCenter.taskMng.ringTaskProgress.Count; i < max; i++)
            {
                if (GameCenter.taskMng.ringTaskProgress[i].task_sort == curTaskType)
                {
                    restTime = (int)GameCenter.taskMng.ringTaskProgress[i].surplus_refresh_num; 
                    break;
                }
            } 
            VIPRef vip = ConfigMng.Instance.GetVIPRef(GameCenter.vipMng.VipData != null ? GameCenter.vipMng.VipData.vLev : 0);
            if (vip != null) allRefreshTime = vip.ringRefreshNum; 
            StepConsumptionRef consume = ConfigMng.Instance.GetStepConsumptionRef(allRefreshTime - restTime + 1);
            if (consume != null && consume.ringTaskCost.Count > 0)
            {
                refreshCostNum = consume.ringTaskCost[0].count;
            }
            if (refreshCost != null) refreshCost.text = refreshCostNum.ToString();
            if (restRefreshNum != null) restRefreshNum.text = restTime.ToString();

            ShowFinishTask();

            if (GameCenter.taskMng.GetRingAutoFinish(curTaskType))
            {
                if (autoFilish != null)
                {
                    autoFilish.SetActive(false);
                    autoToggle.value = false;
                }
            }
            return;
        }

        info = GameCenter.taskMng.GetCurRingTask;
        if (info == null)
        {
            Debug.Log("当前没有难度为 " + curTaskType + "  的任务找后台");
            return;
        }
		if(ringNum != null)ringNum.text =(info.taskLoop+1).ToString();
        if (taskTarget != null)
        {
            taskTarget.gameObject.SetActive(true);
            taskTarget.text = info.TaskDes;
        }
		TaskSurroundRewardRef taskReward = ConfigMng.Instance.GetTaskSurroundRewardRefLv(GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel);
		int coin = 0;
		int exp = 0;
		if(taskReward != null)
		{
			switch(info.taskStar)
			{
			case 1:
				coin = (int)(taskReward.gold*0.2f);
				exp = (int)(taskReward.exp*0.2f);
				break;
			case 2:
				coin = (int)(taskReward.gold*0.4f);
				exp = (int)(taskReward.exp*0.4f);
				break;
			case 3:
				coin = (int)(taskReward.gold*0.6f);
				exp = (int)(taskReward.exp*0.6f);
				break;
			case 4:
				coin = (int)(taskReward.gold*0.8f);
				exp = (int)(taskReward.exp*0.8f);
				break;
			case 5:
				coin = (int)(taskReward.gold);
				exp = (int)(taskReward.exp);
				break;
			}
		}
        if (expNum != null) expNum.text = exp.ToString(); 
		//if(copperNum != null)copperNum.text = coin.ToString();
		//if(refreshStarCost != null)refreshStarCost.text = RefreshStarCost.ToString();
		//if(oneKeyFinishCost != null)oneKeyFinishCost.text = ((MaxRingTaskLoop-info.taskLoop)*10).ToString();//一键完成剩余环任务(10元宝一个)
		if(labFinishRingNum != null)labFinishRingNum.text = info.taskLoop+"/"+MaxRingTaskLoop;
		if(rewardStar != null)rewardStar.value = (float)info.taskStar/5f;
        //当前几倍经验
        if (expDes != null)
            expDes.text = info.taskStar.ToString();
        //当前奖励倍数
        if(num!=null)
            num.text = info.taskStar.ToString(); 
        switch (info.TaskState)
        {
            case TaskStateType.UnTake:
                if (btnAddTask != null) btnAddTask.gameObject.SetActive(true);
                if (autoToggle != null) autoToggle.gameObject.SetActive(true);  
                break;
            case TaskStateType.Finished:
                if (btnFinishTask != null) btnFinishTask.gameObject.SetActive(true);
                if (btnDiamondFinish != null) btnDiamondFinish.gameObject.SetActive(true); 
                //if (alreadyDidTask != null) alreadyDidTask.gameObject.SetActive(true);
                break;
            case TaskStateType.Process:
                if (btnToPatrolled != null) btnToPatrolled.gameObject.SetActive(true);
                if (autoToggle != null) autoToggle.gameObject.SetActive(true);  
                break; 
        }
  
        //if (btnRefreshStar != null)
        //{
        //    if (!GameCenter.taskMng.HaveFiveFold)
        //        btnRefreshStar.gameObject.SetActive(!btnDiamondFinish.gameObject.activeSelf);
        //}
        //if(fiveDes2!=null)
        //{
        //    if(GameCenter.taskMng.HaveFiveFold)
        //    fiveDes2.gameObject.SetActive(!btnDiamondFinish.gameObject.activeSelf);
        //}
        if (effect != null)
		{
			effect.SetActive(info.TaskState == TaskStateType.UnTake && info.taskStar<5);
		} 
        if (refreshStarEffect != null)
        {
            refreshStarEffect.ReShowFx();
        } 
        if (GameCenter.taskMng.GetRingAutoFinish(curTaskType))
        {
            //CountDown();
            switch (info.TaskState)
            {
                case TaskStateType.UnTake://倒计时3秒接任务
                case TaskStateType.Finished://倒计时3秒完成任务
                CountDown();
                break;
                case TaskStateType.Process://接了任务关闭界面 
                     GameCenter.taskMng.GoTraceTask(info);
                    if (GameCenter.uIMng.CurOpenType == GUIType.RINGTASK)
                    {
                        GameCenter.uIMng.SwitchToUI(GUIType.NONE);
                    }
                    break;
            }
        }
	}


	void OneKeyFinish(GameObject go)
	{
		if(GameCenter.vipMng.VipData != null && GameCenter.vipMng.VipData.vLev < 4)
		{
			GameCenter.messageMng.AddClientMsg(548);
			return;
		}
		TaskInfo info = GameCenter.taskMng.GetCurRingTask;
		int cost = 0;
		if(info != null)
			cost = ((MaxRingTaskLoop-info.taskLoop)*10);
		MessageST mst = new MessageST();
		mst.messID = 270;
		mst.words = new string[]{cost.ToString()};
		mst.delYes = (x)=>
		{
            if (GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount < (ulong)cost)
			{
				//GameCenter.messageMng.AddClientMsg(137);
				MessageST mst1 = new MessageST();
				mst1.messID = 137;
				mst1.delYes = (y)=>
				{
					GameCenter.uIMng.SwitchToUI(GUIType.RECHARGE);
				};
				GameCenter.messageMng.AddClientMsg(mst1);
				return;
			}
			//GameCenter.uIMng.SwitchToUI(GUIType.NONE);
			GameCenter.taskMng.C2S_FinishAllRingTask(GameCenter.taskMng.curRingTaskType);
		};
		GameCenter.messageMng.AddClientMsg(mst);
	}
    //一键五倍
    //void RefreshTaskStar(GameObject go)
    //{
    //    if(GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount < RefreshStarCost)
    //    {
    //        MessageST mst = new MessageST();
    //        mst.messID = 137;
    //        mst.delYes = delegate
    //        {
    //            // 跳转到充值界面
    //            GameCenter.uIMng.SwitchToUI(GUIType.RECHARGE);
    //        };
    //        GameCenter.messageMng.AddClientMsg(mst);
    //        return;
    //    }
    //    GameCenter.taskMng.C2S_ResetRingTask(GameCenter.taskMng.curRingTaskType);
    //}
	void AddTask(GameObject go)
	{
		GameCenter.taskMng.C2S_AskAcceptRingTask();
	}
	void FinishTask(GameObject go)
	{
		GameCenter.taskMng.C2S_AskFinishRingTask();
		TaskInfo info = GameCenter.taskMng.GetCurRingTask;
        if (info != null && info.taskLoop == 9)//完成第十五环任务关闭界面
        {
           // GameCenter.uIMng.SwitchToUI(GUIType.NONE);
            if (autoFilish != null)//退出自动挂机
            {
                autoFilish.SetActive(false);
                autoToggle.value = false;
            }
        }
	}
	void DiamondFinish(GameObject go)
	{
		MessageST mst = new MessageST();
		mst.messID = 271;
		mst.words = new string[1]{DiamondFinishCost.ToString()};
		mst.delYes = (x)=>
		{
            if (GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount < (ulong)DiamondFinishCost)
			{
				//GameCenter.messageMng.AddClientMsg(137);
				MessageST mst1 = new MessageST();
				mst1.messID = 137;
				mst1.delYes = (y)=>
				{
					GameCenter.uIMng.SwitchToUI(GUIType.RECHARGE);
				};
				GameCenter.messageMng.AddClientMsg(mst1);
				return;
			}
			GameCenter.taskMng.C2S_AskDiamondFinishRingTask();
			TaskInfo info = GameCenter.taskMng.GetCurRingTask;
            //if(info != null && info.taskLoop == 9)
            //{
            //    GameCenter.uIMng.SwitchToUI(GUIType.NONE);
            //}
		};
		GameCenter.messageMng.AddClientMsg(mst);
	}
	void CloseWnd(GameObject go)
	{
		//GameCenter.uIMng.SwitchToUI(GUIType.NONE);
        GameCenter.uIMng.SwitchToUI(GUIType.RINGTASKTYPE);
	}
    /// <summary>
    /// 倒计时自动接取环式任务
    /// </summary>
    void CountDown()
    {
        if (time != null)
        {
            time.GetComponent<UILabel>().text = "3";
        }
        if (time!=null)
        {  
            time.StartIntervalTimer(3);
            if (autoFilish != null && !autoFilish.activeSelf) autoFilish.SetActive(true); 
            time.onTimeOut = (x) =>
            { 
                autoFilish.SetActive(false);
                TaskInfo info = GameCenter.taskMng.GetCurRingTask;
                if (info.TaskState == TaskStateType.UnTake)
                {
                    GameCenter.taskMng.C2S_AskAcceptRingTask();
                } 
                else if (info.TaskState == TaskStateType.Finished)
                {
                    FinishTask(btnFinishTask.gameObject);
                    //Invoke("FinishTask", 0.5f);
                }
                else if (info.TaskState == TaskStateType.Process)
                {
                    GameCenter.taskMng.GoTraceTask(info);
                    if (GameCenter.uIMng.CurOpenType == GUIType.RINGTASK)
                    {
                        GameCenter.uIMng.SwitchToUI(GUIType.NONE);
                    }
                }
            };
        }
    }
    /// <summary>
    /// 自动挂机完成
    /// </summary>
    void FinishTask()
    {
        if (!autoFilish.activeSelf)
            autoFilish.SetActive(true);
        if (time != null)
        {
            time.StartIntervalTimer(3);
            time.onTimeOut = (x) =>
            { 
                GameCenter.taskMng.C2S_AskAcceptRingTask();
                //GameCenter.uIMng.SwitchToUI(GUIType.NONE);
            };
        }
    }
    /// <summary>
    /// 勾选自动挂机
    /// </summary>
    void AutoOnHook()
    {
       if(autoToggle.value)
        {
            CountDown();
        }
       if(!autoToggle.value)
        {
            if (time != null)
            {
                time.StopTimer();
            }
            if (autoFilish != null && autoFilish.activeSelf)
            {
                autoFilish.SetActive(false); 
            }
        }
       GameCenter.taskMng.SetRingAutoFinishType(GameCenter.taskMng.curRingTaskType, autoToggle.value);
        //GameCenter.taskMng.AutoFinishRingTask = autoToggle.value;
    }
    /// <summary>
    /// 退出自动挂机
    /// </summary>
    void CloseAutoFilish(GameObject _obj)
    {
        //Debug.Log("退出自动挂机");
        if (autoFilish != null)
        {
            autoFilish.SetActive(false);
            autoToggle.value = false;
        }
    }
    /// <summary>
    /// 隐藏刷新五倍
    /// </summary>
    //void HideRefreshStar()
    //{
    //    if (!GameCenter.taskMng.HaveFiveFold)
    //    {
    //        //if (btnRefreshStar != null) UIEventListener.Get(btnRefreshStar.gameObject).onClick = RefreshTaskStar;
    //        //if (fiveDes1 != null)
    //        //    fiveDes1.gameObject.SetActive(true);
    //        if (fiveDes2 != null) fiveDes2.gameObject.SetActive(false);
    //        //if (cost != null) cost.SetActive(true);
    //    }
    //    else
    //    {
    //        btnRefreshStar.gameObject.SetActive(false);
    //        //if (fiveDes1 != null)
    //        //    fiveDes1.gameObject.SetActive(false);
    //        if (fiveDes2 != null) fiveDes2.gameObject.SetActive(true);
    //        //if (cost != null) cost.SetActive(false);
    //    }
    //}
    /// <summary>
    /// 刷新剩余次数
    /// </summary> 
    void RefreshRestNum(GameObject go)
    {
        if (restTime <= 0)
        {
            MessageST mst = new MessageST();
            mst.messID = 546;
            mst.delYes = (x) =>
            {
                GameCenter.uIMng.SwitchToUI(GUIType.RECHARGE);
            };
            GameCenter.messageMng.AddClientMsg(mst);
        }
        else
        {
            MessageST mst = new MessageST();
            mst.messID = 552;
            mst.words = new string[1] { refreshCostNum.ToString() };
            mst.delYes = (x) =>
            {
                GameCenter.taskMng.C2S_ResetRingTask(GameCenter.taskMng.curRingTaskType);
            };
            GameCenter.messageMng.AddClientMsg(mst);
        } 
    }

    void GotoTheTask(GameObject go)
    {
        if (info != null)
        {
            GameCenter.taskMng.GoTraceTask(info);
            GameCenter.uIMng.SwitchToUI(GUIType.NONE);
        }
    }
}
