//==============================================
//作者：邓成
//日期：2016/6/22
//用途：背包背景界面
//=================================================

using UnityEngine;
using System.Collections;

public class BackpackBackWnd : SubWnd {

	protected override void OnOpen ()
	{
		base.OnOpen ();
		//GameCenter.uIMng.GenGUI(GUIType.BACKPACKWND,true);
        GameCenter.inventoryMng.OpenBackpack(ItemShowUIType.NORMALBAG);
		GameCenter.uIMng.GenGUI(GUIType.PREVIEW_MAIN,true);
	}
	protected override void OnClose ()
	{
		base.OnClose ();
		GameCenter.uIMng.ReleaseGUI(GUIType.BACKPACKWND);
        GameCenter.uIMng.ReleaseGUI(GUIType.PREVIEW_MAIN);
	}
	protected override void HandEvent (bool _bind)
	{
		base.HandEvent (_bind);
	}
}
