//==================================
//作者：邓成
//日期：2016/4/12
//用途：公会仓库界面类
//=================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;

public class GuildStorageWnd : GUIBase {
	#region UI控件对象
    public BackpackPageUI backpackPageUI;
	/// <summary>
	/// 整理按钮
	/// </summary>
	public UIButton ArrangeButton;

	public GameObject applyGo;
	public UILabel labApplyLog;
	public UIButton btnSeeApply;
	public UIGrid uigrid;
	public Vector4 positionInfo;

	protected Dictionary<int,ApplyListItemUI> memberList = new Dictionary<int, ApplyListItemUI>();

	public UIButton btnClose;
	#endregion
	void Awake()
	{
		if(ArrangeButton != null) UIEventListener.Get(ArrangeButton.gameObject).onClick = OnClickArrangeBtn;
		if(btnSeeApply != null) UIEventListener.Get(btnSeeApply.gameObject).onClick = SeeApply;
		if(btnClose != null) UIEventListener.Get(btnClose.gameObject).onClick = CloseWnd;

		layer = GUIZLayer.TOPWINDOW;
		mutualExclusion = true;
	}
	protected override void OnOpen()
	{
		base.OnOpen();
		GameCenter.guildMng.C2S_ReqStorageItemList();
        InitProgress();
	}
	protected override void OnClose ()
	{
		base.OnClose ();
        GameCenter.uIMng.ReleaseGUI(GUIType.BACKPACKWND);
	}
	protected override void HandEvent(bool _bind)
	{
		base.HandEvent(_bind);
		if (_bind)
		{
            GameCenter.guildMng.OnGotStorageData += UpdateAllItems;
			//GameCenter.guildMng.OnStorageItemUpdate += UpdateAllItems;
			GameCenter.guildMng.OnGetApplyItemListEvent += ShowApply;
			GameCenter.guildMng.OnGetStorageLogListEvent += ShowLog;
		}
		else
		{
            GameCenter.guildMng.OnGotStorageData -= UpdateAllItems;
			//GameCenter.guildMng.OnStorageItemUpdate -= UpdateAllItems;
			GameCenter.guildMng.OnGetApplyItemListEvent -= ShowApply;
			GameCenter.guildMng.OnGetStorageLogListEvent -= ShowLog;
		}
	}
    void InitProgress()
    {
        List<EquipmentInfo> backpackItems = new List<EquipmentInfo>(GameCenter.guildMng.RealStorageDictionary.Values);
        backpackItems.Sort(SortEquipment);
        if (backpackPageUI != null) backpackPageUI.Init(1, backpackItems);
    }
	/// <summary>
	/// 创建所有物品
	/// </summary>
	protected void RefreshItems()
	{
		List<EquipmentInfo> backpackItems = new List<EquipmentInfo>(GameCenter.guildMng.RealStorageDictionary.Values);
		backpackItems.Sort(SortEquipment);
        if (backpackPageUI != null) backpackPageUI.UpdateItems(backpackItems);
	}
	/// <summary>
	/// 刷新所有物品
	/// </summary>
	protected void UpdateAllItems()
	{
        RefreshItems();
	}

	protected void ShowApply()
	{
		HideApplyList();
		List<guild_check_out_item_ask_list> guildMembers = GameCenter.guildMng.ApplyList;
		int index = 0;
		foreach(guild_check_out_item_ask_list data in guildMembers)
		{
			ApplyListItemUI item = null;
			if(!memberList.TryGetValue(index,out item))
			{
				if(uigrid != null)item = ApplyListItemUI.CreateNew(uigrid.transform);
				memberList[index] = item;
			}
			if(item != null)
			{
				item.SetData(data);
				item.gameObject.SetActive(true);
			}
			index++;
		}
		if(uigrid != null)uigrid.repositionNow = true;
}
	void HideApplyList()
	{
		foreach(ApplyListItemUI go in memberList.Values)
		{
			if(go != null)go.gameObject.SetActive(false);
		}
	}

	void ShowLog()
	{
		List<guild_items_log_list> guildLogs = GameCenter.guildMng.storageLogList;
		System.Text.StringBuilder builder = new System.Text.StringBuilder();
		for (int i = 0,max=guildLogs.Count; i < max; i++) 
		{
			ConfigMng.Instance.GetEquipmentRef(guildLogs[i].item_type);
			builder.Append(guildLogs[i].name).Append(guildLogs[i].action==1?ConfigMng.Instance.GetUItext(286):ConfigMng.Instance.GetUItext(287)).Append(GameHelper.GetEquipNameByType(guildLogs[i].item_type));
			if(i < max-1)
				builder.Append("\n");
		}
		if(labApplyLog != null)
			labApplyLog.text = builder.ToString();
	}
	#region 控件事件
	/// <summary>
	/// 点击整理按钮
	/// </summary>
	/// <param name="obj"></param>
	void OnClickArrangeBtn(GameObject obj)
	{
	//	GameCenter.inventoryMng.C2S_ArrangeStorage();
		GameCenter.guildMng.C2S_ArrangeStorageItem();
	}
	/// <summary>
	/// 查看申请
	/// </summary>
	/// <param name="go">Go.</param>
	void SeeApply(GameObject go)
	{
		if(applyGo != null)
			applyGo.SetActive(true);
		GameCenter.guildMng.C2S_ReqItemApplyList();
		GameCenter.guildMng.C2S_ReqStorageLogList(1);
	}
	/// <summary>
	/// 点击关闭按钮
	/// </summary>
	void CloseWnd(GameObject obj)
	{
		GameCenter.uIMng.ReleaseGUI(GUIType.GUILDSTORAGE);
		GameCenter.uIMng.SwitchToUI(GUIType.GUILDMAIN);
		//	GameCenter.inventoryMng.C2S_ArrangeStorage();
	}
	#endregion
	public int SortEquipment(EquipmentInfo eq1,EquipmentInfo eq2)
	{
		if(eq1.Postion > eq2.Postion)
			return 1;
		if(eq1.Postion < eq2.Postion) 
			return -1;
		return 0;
	}

}
