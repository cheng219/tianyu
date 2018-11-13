//==================================
//作者：邓成
//日期：2016/4/19
//用途：玩法按钮界面辅助类
//=================================

using UnityEngine;
using System.Collections;

public class UIFocusItem : MonoBehaviour {

	void OnEnable()
	{
		UICamera.selectedObject = this.gameObject;
	}

	void OnSelect (bool isSelected)
	{

		if (!isSelected)	//添加是否处在锁屏引导中的判断。
		{
            if (!GameCenter.noviceGuideMng.StartGuide) Invoke("Hide", 0.15f);//延迟一点才能点子按钮
		}
		else
		{
			this.gameObject.SetActive(true);
		}
	}

	void Hide()
	{
		this.gameObject.SetActive(false);
	}
}
