//==============================================
//作者：邓成
//日期：2016/7/12
//用途：NPC处打开的仓库
//==============================================

using UnityEngine;
using System.Collections;

public class StorageBaseWnd : GUIBase {
	public GameObject btnClose;
	void Awake()
	{
		mutualExclusion = true;
		layer = GUIZLayer.NORMALWINDOW;
		if(btnClose != null)UIEventListener.Get(btnClose).onClick = CloseWnd;
	}
	protected override void OnOpen ()
	{
		base.OnOpen ();
		GameCenter.uIMng.GenGUI(GUIType.STORAGE,true);
		GameCenter.uIMng.GenGUI(GUIType.BACKPACKWND,true);
	}
	protected override void OnClose ()
	{
		base.OnClose ();
        GameCenter.uIMng.ReleaseGUI(GUIType.STORAGE);
        GameCenter.uIMng.ReleaseGUI(GUIType.BACKPACKWND);
	}
	void CloseWnd(GameObject go)
	{
		GameCenter.uIMng.SwitchToUI(GUIType.NONE);
	}
}
