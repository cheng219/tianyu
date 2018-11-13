//==================================
//作者：邓成
//日期：2016/7/14
//用途：服务器页数类
//=================================
using UnityEngine;
using System.Collections;

public class ServerPageItem : MonoBehaviour {
    public UILabel pageLab;
	public UIToggle toggle;

	public void SetData(ServerPageData pageData,System.Action<ServerPageData> _callback)
    {
		if(pageLab != null)pageLab.text = "[b]" + pageData.CurPageName;
		if(toggle != null)EventDelegate.Add(toggle.onChange,()=>
			{
				if (toggle.value && _callback != null)
					_callback(pageData);
			});
    }
	public void SetChected()
	{
		if(toggle != null)toggle.value = true;
	}
    public ServerPageItem CreateNew(Transform _parent)
    {
		ServerPageItem serverPageItem = null;
		GameObject go = Instantiate(gameObject) as GameObject;
		if(go != null)
		{
			go.transform.parent = _parent;
			go.SetActive(true);
			go.transform.localPosition = Vector3.zero;
			go.transform.localScale = Vector3.one;
			serverPageItem = go.GetComponent<ServerPageItem>();
			if (serverPageItem == null) serverPageItem = go.AddComponent<ServerPageItem>();
		}
        return serverPageItem;
    }
}
