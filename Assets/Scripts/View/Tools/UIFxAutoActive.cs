//==================================
//作者：邓成
//日期：2016/7/22
//用途：自动播放特效组件(仅用于美术制作的自销毁特效)
//原理：复制一份预制上的特效,显示出来，预制上的特效保持不变
//=================================

using UnityEngine;
using System.Collections;

public class UIFxAutoActive : MonoBehaviour {

	public NcAutoDestruct oldFx;

	protected System.Action callBack;

    protected GameObject go = null;

	public void ShowFx(System.Action _callBack = null)
	{
		if(oldFx != null)
		{
			GameObject oldGo = oldFx.gameObject;
			if(go == null)go = Instantiate(oldGo) as GameObject;
			if(go != null)
			{
				go.transform.parent = oldGo.transform.parent;
				go.transform.localScale = oldGo.transform.localScale;
				go.transform.localPosition = oldGo.transform.localPosition;
				go.transform.localRotation = oldGo.transform.localRotation;
				go.SetActive(true);
				callBack = _callBack;
				Invoke("CallBack",oldFx.m_fLifeTime - 0.1f);
			}
		}
	}



    /// <summary>
    /// 每次都重新创建新特效播放 by黄洪兴
    /// </summary>
    /// <param name="_callBack"></param>
    public void ReShowFx(System.Action _callBack = null)
    {
        if (oldFx != null)
        {
            GameObject oldGo = oldFx.gameObject;
            if (go != null)
            {
                Destroy(go);
            }
            go = Instantiate(oldGo) as GameObject;
            if (go != null)
            {
                go.transform.parent = oldGo.transform.parent;
                go.transform.localScale = oldGo.transform.localScale;
                go.transform.localPosition = oldGo.transform.localPosition;
                go.transform.localRotation = oldGo.transform.localRotation;
                go.SetActive(true);
                callBack = _callBack;
                CancelInvoke("CallBack");
                Invoke("CallBack", oldFx.m_fLifeTime - 0.1f);
            }
        }
    }

	void CallBack()
	{
		if(callBack != null)
			callBack();
	}
}
