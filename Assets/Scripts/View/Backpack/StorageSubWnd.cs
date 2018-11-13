//==============================================
//作者：邓成
//日期：2016/6/22
//用途：仓库背景界面
//=================================================

using UnityEngine;
using System.Collections;

public class StorageSubWnd : SubWnd {

	protected override void OnOpen ()
	{
		base.OnOpen ();
		GameCenter.uIMng.GenGUI(GUIType.BACKPACKWND,true);
		GameCenter.uIMng.GenGUI(GUIType.STORAGE,true);
	}
	protected override void OnClose ()
	{
		base.OnClose ();
        GameCenter.uIMng.ReleaseGUI(GUIType.BACKPACKWND);
        GameCenter.uIMng.ReleaseGUI(GUIType.STORAGE);
	}
	protected override void HandEvent (bool _bind)
	{
		base.HandEvent (_bind);
	}
}
