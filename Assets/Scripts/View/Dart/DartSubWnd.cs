//===============================
//作者：邓成
//日期：2016/5/11
//用途：运镖子界面(每日运镖、仙盟运镖)类
//===============================

using UnityEngine;
using System.Collections;

public class DartSubWnd : SubWnd {
	public UIToggle[] toggleChoose;
	public UILabel[] labDurable;//耐久
	public Load3DObject[] loadModels;

	public UIButton btnReward;
	public UIButton btnStart;
	public UILabel remainTimes;//剩余次数

	void Awake()
	{
		if(btnStart != null)UIEventListener.Get(btnStart.gameObject).onClick = StartDart;
		if(btnReward != null)UIEventListener.Get(btnReward.gameObject).onClick = SeeReward;
	}
	protected override void OnOpen ()
	{
		base.OnOpen ();
		InitWnd();
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

		}else
		{

		}
	}
	void InitWnd()
	{
		if(toggleChoose != null)
		{
			for (int i = 0,max=toggleChoose.Length; i < max; i++) {
				if(toggleChoose[i] != null)
				{
					toggleChoose[i].value = (i == 0);//选中第一个
				}
			}
		}
//		if(labDurable != null)
//		{
//			for (int i = 0,max=labDurable.Length; i < max; i++) {
//				if(labDurable[i] != null)
//				{
//					labDurable[i].text = ((i+1)*100).ToString();//选中第一个
//				}
//			}
//		}
		int [] dartIDs = null;
		if(type == SubGUIType.DAILYDART)
		{
			ActivityDataInfo data = GameCenter.activityMng.GetActivityDataInfo((int)ActivityType.DAILYTRANSPORTDART);
			if(remainTimes != null)remainTimes.text = (data==null?string.Empty:data.Num.ToString());
			dartIDs = new int[]{400001,400002,400003};
		}else if(type == SubGUIType.GUILDDART)
		{
			ActivityDataInfo data = GameCenter.activityMng.GetActivityDataInfo((int)ActivityType.FAIRYAUSHIPMENTDART);
			if(remainTimes != null)remainTimes.text = (data==null?string.Empty:data.Num.ToString());
			dartIDs = new int[]{400004,400005,400006};
		}
		if(loadModels != null)
		{
			for (int i = 0,max=loadModels.Length; i < max; i++) {
				if(loadModels[i] != null && dartIDs.Length > i)
				{
					loadModels[i].StartLoad(dartIDs[i],NGUI3DType.Monster);
				}
			}
		}
	}
	void StartDart(GameObject go)
	{
		int dartType = 0;
		if(toggleChoose != null)
		{
			for (int i = 0,max=toggleChoose.Length; i < max; i++) {
				if(toggleChoose[i] != null && toggleChoose[i].value)
				{
					dartType = (i+1);
					break;
				}
			}
		}
		if(type == SubGUIType.DAILYDART)
		{
			bool doubleTime = (GameCenter.activityMng.GetActivityState(ActivityType.DAILYTRANSPORTDART) == ActivityState.ONGOING);
			if(!doubleTime)//不在双倍时间
			{
				MessageST mst = new MessageST();
				mst.messID = 418;
				mst.delYes = (x)=>
				{
					GameCenter.activityMng.C2S_StartDart(dartType,((type == SubGUIType.DAILYDART)?DartType.DailyDart:DartType.GuildDart));
				};
				GameCenter.messageMng.AddClientMsg(mst);
			}else//在双倍时间
			{
				GameCenter.activityMng.C2S_StartDart(dartType,((type == SubGUIType.DAILYDART)?DartType.DailyDart:DartType.GuildDart));
			}
		}else
		{
			GameCenter.activityMng.C2S_StartDart(dartType,((type == SubGUIType.DAILYDART)?DartType.DailyDart:DartType.GuildDart));
		}
	}
	void SeeReward(GameObject go)
	{
	//	GameCenter.activityMng.C2S_ReqDartPos(((type == SubGUIType.DAILYDART)?DartType.DailyDart:DartType.GuildDart));
	}
}
