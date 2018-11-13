//==============================================
//作者：邓成
//日期：2016/3/15
//用途：试炼任务子界面类
//==============================================

using UnityEngine;
using System.Collections;

public class TrialTaskItemUI : MonoBehaviour {
	public UILabel taskName;
	public UILabel taskDes;
	public UISprite taskIcon;
	public UILabel rewardExp;
	public UILabel rewardCopper;
	public Load3DObject load3DObject;

    public UIButton btnGoToTheTask;//前往
	public UIButton btnAccept;
	public UIButton btnGetReward;
	//public UIButton btnQuickFinish;
    public UISprite alreadDid;//已经完成的状态
	//public UIButton btnGiveUpTask;

	protected TaskInfo curTaskInfo = null;
	void Start()
	{
		if(btnAccept != null)UIEventListener.Get(btnAccept.gameObject).onClick = AcceptTask;
		if(btnGetReward != null)UIEventListener.Get(btnGetReward.gameObject).onClick = GetReward;
		//if(btnGiveUpTask != null)UIEventListener.Get(btnGiveUpTask.gameObject).onClick = GiveUpTask;
		//if(btnQuickFinish != null)UIEventListener.Get(btnQuickFinish.gameObject).onClick = QuickFinishTask;
        if (btnGoToTheTask != null) UIEventListener.Get(btnGoToTheTask.gameObject).onClick = GotoTheTask;
	}
	public void SetData(TaskInfo taskInfo)
	{
		curTaskInfo = taskInfo;
		if(taskInfo == null)
		{
			Debug.Log("试炼任务数据为空!");
			return;
		}
		if(taskName != null)taskName.text = taskInfo.TaskName;
		if(taskDes != null)taskDes.text = taskInfo.Description;
		if(rewardExp != null)rewardExp.text = taskInfo.RewardExp.ToString();
		if(rewardCopper != null)rewardCopper.text = taskInfo.RewardCoin.ToString();
		if(taskIcon != null)taskIcon.spriteName = taskInfo.AcceptIcon;

		if(btnAccept != null)btnAccept.gameObject.SetActive(false);
		if(btnGetReward != null)btnGetReward.gameObject.SetActive(false);
        if (btnGoToTheTask != null) btnGoToTheTask.gameObject.SetActive(false); 
        if (alreadDid != null) alreadDid.gameObject.SetActive(false);
		if(load3DObject != null)
		{
			load3DObject.configID = taskInfo.PreviewBossID;
			load3DObject.type = NGUI3DType.Monster;
			load3DObject.StartLoad();
		}
		switch(taskInfo.TaskState)
		{
		case TaskStateType.UnTake:
			if(btnAccept != null)btnAccept.gameObject.SetActive(true);
			break;
		case TaskStateType.Finished:
			if(btnGetReward != null)btnGetReward.gameObject.SetActive(true);
			break;
		case TaskStateType.Process:
            if (btnGoToTheTask != null) btnGoToTheTask.gameObject.SetActive(true); 
			break;
         case TaskStateType.ENDED:
            if (alreadDid != null) alreadDid.gameObject.SetActive(true);
            break;
		}
	}

	void GetReward(GameObject go)
	{
		if(curTaskInfo != null)
			GameCenter.taskMng.C2S_GetTrialTaskReward(curTaskInfo);
	}
	void AcceptTask(GameObject go)
	{
		if(curTaskInfo != null)
			GameCenter.taskMng.C2S_AskAcceptTrialTask(curTaskInfo);
	}
    void GotoTheTask(GameObject go)
    {
        if (curTaskInfo != null)
        {
            GameCenter.taskMng.GoTraceTask(curTaskInfo);
            GameCenter.uIMng.SwitchToUI(GUIType.NONE);
        }
    }
    //void GiveUpTask(GameObject go)
    //{
    //    if(curTaskInfo != null)
    //        GameCenter.taskMng.C2S_GiveUpTrialTask(curTaskInfo);
    //}
    //void QuickFinishTask(GameObject go)
    //{
    //    int  diamondCost = 0;
    //    if(curTaskInfo == null)return;
    //    switch(curTaskInfo.StarLevel)
    //    {
    //    case TaskMng.PRIMARYTRIALTASK:
    //        diamondCost = 5;
    //        break;
    //    case TaskMng.MIDDLETRIALTASK:
    //        diamondCost = 7;
    //        break;
    //    case TaskMng.SENIORTRIALTASK:
    //        diamondCost = 10;
    //        break;
    //    }
    //    MessageST mst = new MessageST();
    //    mst.messID = 268;
    //    mst.words = new string[1]{diamondCost.ToString()};
    //    mst.delYes = (x)=>
    //    {
    //        GameCenter.taskMng.C2S_QuickFinishTrialTask(curTaskInfo);
    //    };
    //    GameCenter.messageMng.AddClientMsg(mst);
    //}
}
