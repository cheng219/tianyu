//===============================
//作者：邓成
//日期：2016/5/11
//用途：运镖界面类
//===============================

using UnityEngine;
using System.Collections;

public class DartWnd : GUIBase {

	public UIButton btnClose;

	void Awake()
	{
		layer = GUIZLayer.NORMALWINDOW;
		mutualExclusion = true;
		if(btnClose != null)UIEventListener.Get(btnClose.gameObject).onClick = CloseWnd;
	}
	protected override void OnOpen ()
	{
		base.OnOpen ();
		//initSubGUIType = SubGUIType.DAILYDART;
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
	void CloseWnd(GameObject go)
	{
		GameCenter.uIMng.SwitchToUI(GUIType.NONE);
	}
}
