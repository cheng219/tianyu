//==================================
//作者：邓成
//日期：2016/4/21
//用途：仙盟仓库取物品审核Item界面类
//=================================

using UnityEngine;
using System.Collections;
using st.net.NetBase;

public class ApplyListItemUI : MonoBehaviour {
	public UILabel memName;
	public UILabel memLev;
	public UILabel memContribution;
	public UILabel itemName;
	public UIButton btnAgree;
	public UIButton btnRefruse;

	public void SetData(guild_check_out_item_ask_list item)
	{
		if(memName != null)memName.text = item.name;
		if(memLev != null)memLev.text = item.lev.ToString();
		if(memContribution != null)memContribution.text = item.all_contribute.ToString();
		EquipmentRef equip = ConfigMng.Instance.GetEquipmentRef(item.item_type);
		if(itemName != null && equip != null)itemName.text = equip.name.ToString();

		if(btnAgree != null)
		{
			UIEventListener.Get(btnAgree.gameObject).onClick = (x)=>
			{
				GameCenter.guildMng.C2S_ApplyCheckOutItem(item.uid,item.item_id,1);
			};
		}
		if(btnRefruse != null)
		{
			UIEventListener.Get(btnRefruse.gameObject).onClick = (x)=>
			{
				GameCenter.guildMng.C2S_ApplyCheckOutItem(item.uid,item.item_id,0);
			};
		}
	}

	public static ApplyListItemUI CreateNew(Transform _parent)
	{
		GameObject go = null;
		UnityEngine.Object prefab = exResources.GetResource(ResourceType.GUI, "Guild/ApplyListItem");
		go = Instantiate(prefab) as GameObject;
		go.transform.parent = _parent;
		go.transform.localPosition = Vector3.zero;
		go.transform.localScale = Vector3.one;
		go.SetActive(true);
        prefab = null;
		ApplyListItemUI applyItem = go.GetComponent<ApplyListItemUI>();
		if (applyItem == null) applyItem = go.AddComponent<ApplyListItemUI>();
		return applyItem;
	}
}
