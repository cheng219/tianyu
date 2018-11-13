//==============================================
//作者：邓成
//日期：2016/7/8
//用途：PK提示窗口
//==============================================

using UnityEngine;
using System.Collections;

public class PKTipWnd : SubWnd {
	public GameObject btnClose;

	void Awake()
	{
		if(btnClose != null)UIEventListener.Get(btnClose).onClick = CloseWnd;
	}
	protected override void OnOpen ()
	{
		base.OnOpen ();
	}
	protected void CloseWnd(GameObject go)
	{
		//GameCenter.uIMng.CloseGUI(GUIType.NOVICETIP);
        GameCenter.uIMng.SwitchToUI(GUIType.NONE);//打开的时候,主界面收进去了,关闭后没出来
	}
}
