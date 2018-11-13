//==============================================
//作者：邓成
//日期：2016/7/8
//用途：任务和竞技提示窗口
//==============================================

using UnityEngine;
using System.Collections;

public class NoviceTipWnd : GUIBase {

    void Awake()
    {
        layer = GUIZLayer.NORMALWINDOW;
        mutualExclusion = true;
    }
	protected override void OnOpen ()
	{
		base.OnOpen ();
	}

}
