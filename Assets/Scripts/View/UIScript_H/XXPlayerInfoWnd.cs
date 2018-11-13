/// <summary>
/// add 何明军
/// 2016/4/19
/// XX 人物属性总界面预制Player_information
/// </summary>
using UnityEngine;
using System.Collections;

public class XXPlayerInfoWnd : GUIBase {
	
	public GameObject btnClose;
	
	public UIGrid uiTogsParent;
	
	void Awake(){
		mutualExclusion = true;
		Layer = GUIZLayer.TOPWINDOW;
		
		if(btnClose != null)UIEventListener.Get(btnClose).onClick = delegate {
			BtnClose();
		};
	}
	
	protected override void OnOpen ()
	{
		base.OnOpen ();
	}
	
	protected override void OnClose ()
	{
		base.OnClose ();
	}
	
	void BtnClose(){
		GameCenter.uIMng.SwitchToUI(GUIType.NONE);
	}
}
