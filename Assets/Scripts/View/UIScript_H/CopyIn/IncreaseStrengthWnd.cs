/// <summary>
/// 战斗失败提示
/// 何明军
/// 2016/4/19
/// </summary>
using UnityEngine;
using System.Collections;

public class IncreaseStrengthWnd : GUIBase {

	public GameObject btnP;
	public GameObject btnS;
	public GameObject btnClose;

	void Awake(){
		mutualExclusion = true;
		Layer = GUIZLayer.TOPWINDOW;
		if(btnClose != null)UIEventListener.Get(btnClose).onClick = delegate {
			GameCenter.uIMng.SwitchToUI(GUIType.NONE);		
		};
        if (btnP != null) UIEventListener.Get(btnP).onClick = delegate
        {
            GameCenter.littleHelperMng.OpenWndByType(LittleHelpType.STRONG);
		};
        if (btnS != null) UIEventListener.Get(btnS).onClick = delegate
        {
            GameCenter.littleHelperMng.OpenWndByType(LittleHelpType.PET);
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
}
