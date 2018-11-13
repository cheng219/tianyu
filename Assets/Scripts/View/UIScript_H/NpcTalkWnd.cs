/// <summary>
/// 2016/4/19
/// 何明军
/// NPC对话界面
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NpcTalkWnd : GUIBase {
	
	public GameObject npcShow;
	public UITexture npcTexture;
	public UILabel npcName;
	public UILabel npcTalk;
	
	public GameObject npcTaskShow;
	public List<ItemUI> taskReward;
//	public UITexture playerTexture;
	
	public GameObject npcFuncShow;
	public List<UIButton> npcFuncBtn;
	public List<UILabel> npcFuncBtnLab;
	public GameObject npcNextStep;
	
	NPCInfo stateNpcFuction = null;
	TaskInfo curTask = null;
	List<int> funcList = null;
    //跳过NPC任务对话
    public UIButton overDialog;
    //倒计时(任务倒计时)
    public UITimer countDown;
    private int time;
	string curTalkText;
	string CurTalkText{
		get{
			return curTalkText;
		}
		set{
			curTalkText = value;
			curTalkTextStep = 0;
		}
	}
	int curTalkTextStep = 0;
	
	void Awake(){
		mutualExclusion = true;
		Layer = GUIZLayer.TOPWINDOW;
		
		NPC npc = GameCenter.curMainPlayer.CurTarget as NPC;
		if(npc != null)
		{
			stateNpcFuction = GameCenter.sceneMng.GetNPCInfo(npc.id);
		}
		
		curTask = null;
		funcList = new List<int>();
		InitStage();
		if(countDown!=null)
        {
            //if (countDown.GetComponent<UILabel>() != null)
            //{
            //   if(!int.TryParse(countDown.GetComponent<UILabel>().text,out time))
            //        time = 8;
            //}  
            //else
                time = 8;
        }
		if(npcNextStep != null)UIEventListener.Get(npcNextStep).onClick = OnNextStep;
		GameCenter.previewManager.ClearModel();
        //跳过NPC对话
        if (overDialog != null) UIEventListener.Get(overDialog.gameObject).onClick = OverDialog;
	}
	
	void InitStage(){
		if(stateNpcFuction == null)return ;
		List<TaskInfo> taskList = GameCenter.taskMng.GetNPCTaskList(stateNpcFuction.Type);
		for(int i =0;i<taskList.Count;i++){
			if(taskList[i].NeedChatWithNpc(stateNpcFuction.Type)){
				curTask = taskList[i];
				return ;
			}
			if(taskList[i].TaskType == TaskType.Main){
				curTask = taskList[i];
//				return ;
			}
		}
		if(taskList.Count  > 0){
			curTask = taskList[0];
			if(curTask != null && curTask.TaskType == TaskType.Ring && !GameCenter.mainPlayerMng.FunctionIsOpen(FunctionType.RINGTASK))
			{
				curTask = null;//环任务功能未开启的时候,不取环任务显示
			}
			if(curTask != null && curTask.TaskType == TaskType.Trial && !GameCenter.mainPlayerMng.FunctionIsOpen(FunctionType.TESTTASK))
			{
				curTask = null;//试炼任务功能未开启的时候,不取环任务显示
			}
			return ;
		}
		if(curTask == null)
        {
            overDialog.gameObject.SetActive(false);
        }

//		if(stateNpcFuction.Function > 0){
//			funcList.Add(stateNpcFuction.Function);
//		}
//		if(stateNpcFuction.FunctionNext > 0){
//			funcList.Add(stateNpcFuction.FunctionNext);
//		}
		
		if(funcList.Count > 0){
			return;
		}
	}
	
	void ShowStage(){
		if(stateNpcFuction == null)return ;
		npcTaskShow.SetActive(false);
		npcFuncShow.SetActive(false);
		npcNextStep.SetActive(true);
		curTalkText = string.Empty;
		
		npcShow.SetActive(true);
		npcName.text = stateNpcFuction.Name;
		
//		createPreview = true;
		if(stateNpcFuction != null && npcTexture != null)GameCenter.previewManager.TryPreviewSingelNPC(stateNpcFuction,npcTexture,PreviewConfigType.Dialog);
		if(curTask != null){
			for(int i=0,len=taskReward.Count;i<len;i++){
				if(curTask.RewardList.Count > i){
					taskReward[i].FillInfo(new EquipmentInfo(curTask.RewardList[i].eid,curTask.RewardList[i].count,EquipmentBelongTo.PREVIEW));
					taskReward[i].gameObject.SetActive(true);
				}else{
					taskReward[i].gameObject.SetActive(false);
				}
			}
//			GameCenter.previewManager.TryPreviewSinglePlayer(GameCenter.mainPlayerMng.MainPlayerInfo, playerTexture);
			CurTalkText = curTask.TaskState == TaskStateType.Finished ? curTask.CommitToNPCDialogText : curTask.TakeFromNPCDialogText;
			if(curTask.NeedChatWithNpc(stateNpcFuction.Type) && curTask.TaskState == TaskStateType.Process)
				CurTalkText = curTask.CommitToNPCDialogText;//对话任务计数特殊做法(程序制作#18380)  by邓成
			OnNextStep(null);
			return ;
		}
		
		if(funcList.Count > 0){
			for(int i=0,len=npcFuncBtn.Count;i<len;i++){
				if(funcList.Count > i){
					npcFuncBtn[i].gameObject.SetActive(true);
					npcFuncBtnLab[i].text = FuncName(funcList[i]);
				}else{
					npcFuncBtn[i].gameObject.SetActive(false);
				}
			}
			npcShow.SetActive(true);
			npcName.text = stateNpcFuction.Name;
			CurTalkText = stateNpcFuction.Talk;
			OnNextStep(null);
			return ;
		}
		
		CurTalkText = stateNpcFuction.Talk;
		OnNextStep(null);
	}
//	bool createPreview = false;
//	bool close = false;
//	void Update(){
//		if(close){
//			close = false;
//			GameCenter.uIMng.SwitchToUI(GUIType.NONE);
//		}
//		if(createPreview){
//			createPreview = false;
//			if(stateNpcFuction != null && npcTexture != null)GameCenter.previewManager.TryPreviewSingelNPC(stateNpcFuction,npcTexture,PreviewConfigType.Dialog);
//		}
//	}
	
	void OnNextStep(GameObject game){
        CountDown();
        if (stateNpcFuction == null){
            //			close = true;
            //Debug.Log("GUIType.NONE");
			GameCenter.uIMng.SwitchToUI(GUIType.NONE);
			return ;
		}
        //与NPC交谈的任务,只需要显示第一段话。by邓成
        if (curTask != null && curTask.NeedChatWithNpc(stateNpcFuction.Type) && curTask.TaskState == TaskStateType.Process && game != null)
		{
			GameCenter.taskMng.C2S_ChatWithNpc(stateNpcFuction.Type);
            //			close = true;
            GameCenter.uIMng.SwitchToUI(GUIType.NONE);
			return;
		}
		string[] strStep = CurTalkText.Split('|');

		if(curTalkTextStep == strStep.Length){
			curTalkTextStep ++ ;
			if(curTask != null){
				npcTaskShow.SetActive(true);
				npcShow.SetActive(false);
				return ;
			}
			if(funcList.Count > 0){
				npcFuncShow.SetActive(true);
				npcNextStep.SetActive(false);
				return ;
			}
			GameCenter.uIMng.SwitchToUI(GUIType.NONE);
		}else if(curTalkTextStep > strStep.Length){
			curTalkTextStep = 0 ;
			bool dontCloseWnd = false;
			if(curTask != null)
			{
				if(curTask.TaskState == TaskStateType.Finished){
					GameCenter.taskMng.C2S_ReqCommitTask(curTask.ID,curTask.Step);
					if(curTask.TaskType == TaskType.Main)
						dontCloseWnd = (curTask.PreviewBossID == stateNpcFuction.RefID);
				}else if(curTask.TaskState == TaskStateType.UnTake && curTask.TaskNeedLv <= GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel){
					GameCenter.taskMng.C2S_ReqAcceptTask(curTask.ID,curTask.Step);
				}
			}
			if(!dontCloseWnd)GameCenter.uIMng.SwitchToUI(GUIType.NONE);
			return ;
		}else{
			npcTalk.text = strStep[curTalkTextStep];
			curTalkTextStep ++ ;
		}
	}
	
	string FuncName(int func){
		
		return "";
	}
	
	protected override void OnOpen ()
	{
		base.OnOpen ();
		ToolTipMng.ShowEquipmentModel = false;//不显示物品热感上的模型
		ShowStage();
        CountDown();
    }
	
	protected override void OnClose ()
	{
		base.OnClose ();
		ToolTipMng.ShowEquipmentModel = true;
    }
	
	void OnDestroy(){
		GameCenter.previewManager.ClearModel();
	}
    //跳过对话直接完成任务
    void OverDialog(GameObject _obj)
    {
        if (curTask != null && curTask.NeedChatWithNpc(stateNpcFuction.Type) && curTask.TaskState == TaskStateType.Process && _obj != null)
        {
            GameCenter.taskMng.C2S_ChatWithNpc(stateNpcFuction.Type);
            GameCenter.uIMng.SwitchToUI(GUIType.NONE);
            return;
        }
        bool dontCloseWnd = false;
        if (curTask.TaskState == TaskStateType.Finished)
        {
            if (curTask.TaskType == TaskType.Main)
                dontCloseWnd = (curTask.PreviewBossID == stateNpcFuction.RefID);
            GameCenter.taskMng.C2S_ReqCommitTask(curTask.ID, curTask.Step);
        }
        if (curTask.TaskState == TaskStateType.UnTake && curTask.TaskNeedLv <= GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel)
        {
            GameCenter.taskMng.C2S_ReqAcceptTask(curTask.ID, curTask.Step);
        }
        if (curTask.TaskState == TaskStateType.Process && curTask.TaskNeedLv <= GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel)
        {
            GameCenter.taskMng.C2S_ReqAcceptTask(curTask.ID, curTask.Step);
        }
        if (!dontCloseWnd) GameCenter.uIMng.SwitchToUI(GUIType.NONE);
        GameCenter.previewManager.ClearModel();
    }
    //倒计时完成任务
    void CountDown()
    {
        //Debug.Log("time"+time);
        if (countDown != null)
        {
            //Debug.Log("time" + time);
            countDown.StartIntervalTimer(time);
            countDown.onTimeOut = (x) =>
            {
                if (overDialog != null&& npcNextStep!=null)
                {
                    //Debug.Log("进入手动调用点击一次特效");
                    countDown.StopTimer();
                    Invoke("ResetCountDown",0.1f);
                    npcNextStep.SendMessage("OnClick", npcNextStep, SendMessageOptions.DontRequireReceiver);//这一句放到最后面,不然会报错mising...
                }

                else
                    Debug.LogError("预制上名为overDialog的组件为空");
            };
        }
        else
            Debug.LogError("名为countDown的Timer组件丢失");

    }
    void ResetCountDown()
    {
        countDown.StartTimer();
        countDown.StartIntervalTimer(time);
    }
}
