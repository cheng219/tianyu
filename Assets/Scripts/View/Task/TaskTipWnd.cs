//==============================================
//作者：邓成
//日期：2016/7/8
//用途：任务提示窗口
//==============================================

using UnityEngine;
using System.Collections;

public class TaskTipWnd : SubWnd {
	public UILabel labName;
	public GameObject btnGo;
	public GameObject btnClose;

	void Awake()
	{
		if(btnClose != null)UIEventListener.Get(btnClose).onClick = CloseWnd;
		if(btnGo != null)UIEventListener.Get(btnGo).onClick = GoRingTask;
	}
	protected override void OnOpen ()
	{
		base.OnOpen ();
		if(labName != null)labName.text = GameCenter.mainPlayerMng.MainPlayerInfo.Name;
	}

	protected void CloseWnd(GameObject go)
	{
		Invoke("InvokeClose",0.1f);
	}

	protected void GoRingTask(GameObject go)
	{
		TaskInfo ringTask = GameCenter.taskMng.GetCurRingTask;
		GameCenter.taskMng.GoTraceTask(ringTask);
		Invoke("InvokeClose",0.1f);
	}
	void InvokeClose()
	{
		//GameCenter.uIMng.ReleaseGUI(GUIType.NOVICETIP);
        GameCenter.uIMng.SwitchToUI(GUIType.NONE);
	}
}
