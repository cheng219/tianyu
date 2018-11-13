//==================================
//作者：邓成
//日期：2016/7/13
//用途：选择服务器item类
//=================================

using UnityEngine;
using System.Collections;

public class ServerChoiceItem : MonoBehaviour {

    public UILabel nameLab;
	/// <summary>
    /// 新服\维护\正常
	/// </summary>
    public GameObject[] flagObj;

	public UIToggle toggle;

    public void SetData(ServerChoiceData _data,System.Action<ServerChoiceData> _callback)
    {
        ClearData();
		if(nameLab != null)nameLab.text = "[b]" + _data.serverName;
        if (flagObj != null)
        {
            for (int i = 0, length = flagObj.Length; i < length; i++)
            {
                if (flagObj[i] != null)
                    flagObj[i].SetActive(i == (_data.serverStatus -1));
            }
        }
		if(toggle != null)
		{
			toggle.optionCanBeNone = false;
			EventDelegate.Add(toggle.onChange,()=>
			{
				if (toggle.value && _callback != null)
					_callback(_data);
			});
		}
    }
	void ClearData()
    {
        if (flagObj != null)
        {
            for (int i = 0, length = flagObj.Length; i < length; i++)
            {
                if (flagObj[i] != null)
                    flagObj[i].SetActive(false);
            }
        }
    }
	public void SetChecked()
	{
		if(toggle != null)
		{
			toggle.value = true;
			EventDelegate.Execute(toggle.onChange);
		}
	}

	public void SetUnChecked()
	{
		if(toggle != null)
		{
			toggle.optionCanBeNone = true;
			toggle.value = false;
		}
	}
    public ServerChoiceItem CreateNew(Transform _parent)
    {
		ServerChoiceItem serverChoiceItem = null;
		GameObject go = Instantiate(this.gameObject) as GameObject;
		if(go != null)
		{
	        go.transform.parent = _parent;
	        go.SetActive(true);
	        go.transform.localPosition = Vector3.zero;
	        go.transform.localScale = Vector3.one;
			serverChoiceItem = go.GetComponent<ServerChoiceItem>();
	        if (serverChoiceItem == null)
				serverChoiceItem = go.AddComponent<ServerChoiceItem>();
		}
        return serverChoiceItem;
    }
}
