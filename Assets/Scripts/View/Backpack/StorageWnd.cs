//====================================================
//作者：邓成
//日期：2016/3/2
//用途：仓库界面
//====================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StorageWnd : GUIBase {
	#region UI控件对象
    public BackpackPageUI backpackPageUI;
	/// <summary>
	/// 整理按钮
	/// </summary>
	public UIButton ArrangeButton;
	#endregion
	void Awake()
	{
		if (ArrangeButton != null) UIEventListener.Get(ArrangeButton.gameObject).onClick = OnClickArrangeBtn;
		layer = GUIZLayer.TOPWINDOW;
		mutualExclusion = false;
	}
	protected override void OnOpen()
	{
		base.OnOpen();
        GameCenter.inventoryMng.C2S_AskForStorageData();
        InitProgress();
	}
	protected override void OnClose ()
	{
		base.OnClose ();
	}
	protected override void HandEvent(bool _bind)
	{
		base.HandEvent(_bind);
		if (_bind)
		{
            GameCenter.inventoryMng.OnGotStorageData += UpdateAllItems;
			//GameCenter.inventoryMng.OnStorageItemUpdate += UpdateItems;
		}
		else
		{
            GameCenter.inventoryMng.OnGotStorageData -= UpdateAllItems;
			//GameCenter.inventoryMng.OnStorageItemUpdate -= UpdateItems;
		}
	}
    void InitProgress()
    {
        List<EquipmentInfo> backpackItems = new List<EquipmentInfo>(GameCenter.inventoryMng.RealStorageDictionary.Values);
		backpackItems.Sort(SortEquipment);
        if (backpackPageUI != null) backpackPageUI.Init(1, backpackItems);
    }
	/// <summary>
	/// 创建所有物品
	/// </summary>
	protected void RefreshItems()
	{
		List<EquipmentInfo> backpackItems = new List<EquipmentInfo>(GameCenter.inventoryMng.RealStorageDictionary.Values);
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
	/// <summary>
	/// 背包更新单个数据
	/// </summary>
	protected void UpdateItems(int pos,EquipmentInfo eq)
	{
        RefreshItems();
	}

	#region 控件事件
	/// <summary>
	/// 点击整理按钮
	/// </summary>
	/// <param name="obj"></param>
	void OnClickArrangeBtn(GameObject obj)
	{
		GameCenter.inventoryMng.C2S_ArrangeStorage();
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
