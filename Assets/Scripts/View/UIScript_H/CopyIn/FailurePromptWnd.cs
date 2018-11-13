/// <summary>
/// 战斗失败
/// 何明军
/// 2016/4/19
/// </summary>
using UnityEngine;
using System.Collections;

public class FailurePromptWnd : GUIBase {

	public UITimer time;
	public GameObject btnClose;
	
	void Awake(){
		mutualExclusion = true;
		Layer = GUIZLayer.TOPWINDOW;
		if(btnClose != null)UIEventListener.Get(btnClose).onClick = delegate {
			GameCenter.duplicateMng.C2S_OutCopy();
		};
	}
	
	protected override void OnOpen ()
	{
		base.OnOpen ();
		time.StartIntervalTimer(5);
		time.onTimeOut = delegate {
			GameCenter.duplicateMng.C2S_OutCopy();
		};
	}
	
	protected override void OnClose ()
	{
		base.OnClose ();
	}
}
