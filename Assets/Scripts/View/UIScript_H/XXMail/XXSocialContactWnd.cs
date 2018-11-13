/// <summary>
/// 何明军
/// 2016/4/19
/// Social_Contact界面
/// </summary>

using UnityEngine;
using System.Collections;

public class XXSocialContactWnd : GUIBase {

	public GameObject btnClose;

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
        GameCenter.uIMng.ReleaseGUI(GUIType.MAIL);
	}
}
