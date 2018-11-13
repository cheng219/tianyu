//==============================================
//作者：邓成
//日期：2016/3/15
//用途：试炼任务界面类
//==============================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrialTaskWnd : GUIBase {
	public TrialTaskItemUI primaryTrialTaskItemUI;
	public TrialTaskItemUI middleTrialTaskItemUI;
	public TrialTaskItemUI seniorTrialTaskItemUI;
	
	public UIButton btnOneKeyFinish; 
	public UIButton btnClose;

    public UIButton btnRefreshRestNum;//刷新剩余次数,当三种试练任务都完成后才能刷新 
    public UILabel restNum;
    public UILabel costLab;
    protected int costNum = 20;//刷新消耗的元宝

	void Awake()
	{ 
		layer = GUIZLayer.NORMALWINDOW;
		mutualExclusion = true;
		if(btnOneKeyFinish != null)UIEventListener.Get(btnOneKeyFinish.gameObject).onClick = OneKeyFinishTrialTask;
        if (btnRefreshRestNum != null) UIEventListener.Get(btnRefreshRestNum.gameObject).onClick = RefreshRestNum;
		if(btnClose != null)UIEventListener.Get(btnClose.gameObject).onClick = CloseWnd;
	}
	/// <summary>
	/// 一键完成试炼任务
	/// </summary>
	void OneKeyFinishTrialTask(GameObject go)
	{
        if (GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount < 20)
        {
            GameCenter.messageMng.AddClientMsg(545);//暂定20元宝
            return;
        }
        MessageST mst = new MessageST();
        //mst.messID = 269;
        mst.messID = 549;
        //mst.words = new string[1]{(GameCenter.taskMng.TrialTaskRestRewardTimes*10).ToString()};
        mst.words = new string[1] {"20"}; 
        mst.delYes = (x) =>
        {
            GameCenter.taskMng.C2S_FinishAllTrialTask();
        };
        GameCenter.messageMng.AddClientMsg(mst);
	}
    /// <summary>
    /// 刷新试练任务次数
    /// </summary> 
    void RefreshRestNum(GameObject go)
    {
        if ((int)GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount < costNum)
        {
            GameCenter.messageMng.AddClientMsg(545);//暂定20元宝
            return;
        }
        if (GameCenter.taskMng.TrialTaskRestRewardTimes <= 0)
        {
            GameCenter.messageMng.AddClientMsg(544);
            return;
        }
         
        MessageST mst = new MessageST();
        mst.messID = 550;
        mst.words = new string[1] { costNum.ToString() }; 
        mst.delYes = (x) =>
        {
            GameCenter.taskMng.C2S_RefreshTrialTaskNum();
        };
        GameCenter.messageMng.AddClientMsg(mst);
    }
	void RefreshTask()
	{ 
        if (btnOneKeyFinish != null) btnOneKeyFinish.gameObject.SetActive(false);
        if (btnRefreshRestNum != null) btnRefreshRestNum.gameObject.SetActive(false); 
        TaskInfo primaryInfo = null;
        TaskInfo middleInfo = null;
        TaskInfo seniorInfo = null; 
        primaryInfo = GameCenter.taskMng.GetTrialTaskByLevel(TaskMng.PRIMARYTRIALTASK);
        middleInfo = GameCenter.taskMng.GetTrialTaskByLevel(TaskMng.MIDDLETRIALTASK); 
        seniorInfo = GameCenter.taskMng.GetTrialTaskByLevel(TaskMng.SENIORTRIALTASK);  

        if (primaryTrialTaskItemUI != null && primaryInfo != null) primaryTrialTaskItemUI.SetData(primaryInfo);
        if (middleTrialTaskItemUI != null && middleInfo != null) middleTrialTaskItemUI.SetData(middleInfo);
        if (seniorTrialTaskItemUI != null && seniorInfo != null) seniorTrialTaskItemUI.SetData(seniorInfo);

        if (primaryInfo != null &&primaryInfo.TaskState == TaskStateType.ENDED &&
            middleInfo != null && middleInfo.TaskState == TaskStateType.ENDED &&
            seniorInfo != null && seniorInfo.TaskState == TaskStateType.ENDED)
        {
            if (btnRefreshRestNum != null) btnRefreshRestNum.gameObject.SetActive(true);
            if (restNum != null) restNum.text = GameCenter.taskMng.TrialTaskRestRewardTimes.ToString();
            if (costLab != null)
            {
                costLab.text = GetRefreshCost().ToString();
            }
        }
        else
        {
            if (btnOneKeyFinish != null) btnOneKeyFinish.gameObject.SetActive(true);
        } 
	}
	void RefreshTask(TaskType _type)
	{
		if(_type == TaskType.Trial)
			RefreshTask();
	}
	protected override void OnOpen ()
	{
		base.OnOpen ();
		RefreshTask();
		ShowRestRewardTimes();
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
			GameCenter.taskMng.OnTaskGroupUpdate += RefreshTask;
			GameCenter.taskMng.OnUpdateTrialTaskRestRewardTimes += ShowRestRewardTimes;
		}else
		{
			GameCenter.taskMng.OnTaskGroupUpdate -= RefreshTask;
			GameCenter.taskMng.OnUpdateTrialTaskRestRewardTimes -= ShowRestRewardTimes;
		}
	}

	void ShowRestRewardTimes()
	{ 
        if (restNum != null) restNum.text = GameCenter.taskMng.TrialTaskRestRewardTimes.ToString();
        if (costLab != null) costLab.text = GetRefreshCost().ToString();
	}

    protected int GetRefreshCost()
    {
        int restTime = GameCenter.taskMng.TrialTaskRestRewardTimes;
        int allRefreshTime = 0;
        VIPRef vip = ConfigMng.Instance.GetVIPRef(GameCenter.vipMng.VipData != null ? GameCenter.vipMng.VipData.vLev : 0);
        if (vip != null) allRefreshTime = vip.trailRefreshNum;
        StepConsumptionRef consume = ConfigMng.Instance.GetStepConsumptionRef(allRefreshTime - restTime + 1);
        if (consume != null && consume.trailTaskCost.Count > 0)
        {
            costNum = consume.trailTaskCost[0].count;
            return consume.trailTaskCost[0].count;
        }
        else
        {
            Debug.Log("阶梯消费表找不到id :  " + (allRefreshTime - restTime + 1) + " 的数据，试练任务的剩余次数 ：" + restTime + " , 当前vip等级获得的总次数 ：" + allRefreshTime);
        }
        return 20;
    }

	void CloseWnd(GameObject go)
	{
		GameCenter.uIMng.SwitchToUI(GUIType.NONE);
	}
}
