//==============================================
//作者：邓成
//日期：2016/4/5
//用途：装备批量洗练单次属性界面类
//==============================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;

public class BatchWashItemUI : MonoBehaviour {
	public UIToggle toggleChoose;
	public UILabel[] labAttr;

	public bool IsChoose
	{
		get
		{
			return (toggleChoose != null)?toggleChoose.value:false;
		}
	}

	System.Action OnChooseUpdate;

	 
    void OnEnable()
    {
        if (toggleChoose != null) EventDelegate.Add(toggleChoose.onChange, OnChangeChoose);
    }
    void OnDisable()
    {
        if (toggleChoose != null) EventDelegate.Remove(toggleChoose.onChange, OnChangeChoose);
    }
	public void SetData(List<spare_list> attrList,System.Action onChooseUpdate)
	{
		OnChooseUpdate = onChooseUpdate;
		for (int i = 0,max=labAttr.Length; i < max; i++) 
		{
			if(attrList.Count > i)
			{
				if(labAttr[i] != null)
					labAttr[i].text = ConfigMng.Instance.GetValueStringByID(attrList[i].att_id);
			}else
			{
				if(labAttr[i] != null)labAttr[i].text = string.Empty;
			}
		}
		ClearChoose();
	}
	public void SetData(List<spare_list> attrList,System.Action onChooseUpdate,out bool haveAttrCanSave,out bool haveHighAttrNotChoose)
	{
		bool tempCanSave = false;
		bool tempNotChoose = false;
		OnChooseUpdate = onChooseUpdate;
		for (int i = 0,max=labAttr.Length; i < max; i++) 
		{
			if(attrList.Count > i)
			{
				EquipmentWashValueRef valueRef = ConfigMng.Instance.GetEquipmentWashValueRefByID(attrList[i].att_id);
				if(valueRef != null && valueRef.att_quality >= 4)
					tempNotChoose = true;
				if(labAttr[i] != null)
					labAttr[i].text = ConfigMng.Instance.GetValueStringByID(attrList[i].att_id);
				if(i == 0)tempCanSave = (attrList[i].att_id != 0);//判断第一条属性是否等于0即可(大多数第四条属性为0)
			}else
			{
				if(labAttr[i] != null)labAttr[i].text = string.Empty;
				//tempCanSave = false;
			}
		}
		ClearChoose();
		haveAttrCanSave = tempCanSave;
		haveHighAttrNotChoose = tempNotChoose;
	}
	/// <summary>
	/// 清空选中
	/// </summary>
	public void ClearChoose()
	{
		if(toggleChoose != null)toggleChoose.value = false;
	}

	public void ClearData()
	{
		if(toggleChoose != null)toggleChoose.value = false;
		for (int i = 0,max=labAttr.Length; i < max; i++) 
		{
			if(labAttr[i] != null)labAttr[i].text = string.Empty;
		}
	}

	void OnChangeChoose()
	{
		if(OnChooseUpdate != null)OnChooseUpdate();
	}
}
