//==============================================
//作者：邓成
//日期：2016/3/29
//用途：装备镶嵌界面类
//==============================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EquipmentInlayWnd : SubWnd {
	public GemInlaySlotUI[] gemSlotUIs;
	public ItemUIContainer gemContainer;
	public UIButton btnNextPage;
	public UIButton btnPreviousPage;
	public UILabel labPage;

	public  int PerPageShowCount = 5;
	protected List<EquipmentInfo> gemShowList = new List<EquipmentInfo>();
	protected int totalPage = 0;
	protected int curPage = 1;
	protected int CurPage
	{
		get{return curPage;}
		set
		{
			curPage = value;
			List<EquipmentInfo> gemList = GameCenter.inventoryMng.GetGemList();
			totalPage = (int)Mathf.Ceil( (float)gemList.Count/(float)PerPageShowCount);
			gemShowList.Clear();
			for (int i = 0,max=gemList.Count; i < max; i++) 
			{
				if(i >= (curPage-1)*PerPageShowCount)
				{
					gemShowList.Add(gemList[i]);
					if(gemShowList.Count >= PerPageShowCount)break;
				}
			}
			if(btnNextPage != null)btnNextPage.isEnabled = (curPage < totalPage);
			if(btnPreviousPage != null)btnPreviousPage.isEnabled = (curPage > 1);
			if(labPage != null)labPage.text = curPage + "/" + totalPage;
			ShowGems();
		}
	}

	void Start()
	{
		if(btnNextPage != null)UIEventListener.Get(btnNextPage.gameObject).onClick = OnNextPage;
		if(btnPreviousPage != null)UIEventListener.Get(btnPreviousPage.gameObject).onClick = OnPreviousPage;
	}
	protected override void OnOpen ()
	{
		base.OnOpen ();
		CurPage = 1;
		ClearGemSlot();
	}
	protected override void OnClose ()
	{
		base.OnClose ();
	}
	protected override void HandEvent (bool _bind)
	{
		base.HandEvent (_bind);
		if(_bind)
		{
			GameCenter.equipmentTrainingMng.OnSelectEquipmentUpdate += ShowGemSlot;
			GameCenter.inventoryMng.OnBackpackUpdate += UpdateShowGems;
		}else
		{
			GameCenter.equipmentTrainingMng.OnSelectEquipmentUpdate -= ShowGemSlot;
			GameCenter.inventoryMng.OnBackpackUpdate -= UpdateShowGems;
		}
	}
	void OnNextPage(GameObject go)
	{
		CurPage += 1;
	}
	void OnPreviousPage(GameObject go)
	{
		CurPage -= 1;
	}
	void UpdateShowGems()
	{
		CurPage = CurPage;
	}
	/// <summary>
	/// 预览背包中的宝石
	/// </summary>
	void ShowGems()
	{
		gemContainer.CellWidth = 88;
		gemContainer.CellHeight = 88;
		gemContainer.RefreshItems(gemShowList,5,gemShowList.Count);
	}
	/// <summary>
	/// 显示选中装备的镶嵌详情
	/// </summary>
	void ShowGemSlot()
	{
		EquipmentInfo selectOne = GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo;
		if(selectOne != null && gemSlotUIs != null)
		{
			for (int i = 0,max=gemSlotUIs.Length; i < max; i++) 
			{
				if(gemSlotUIs[i] != null)
				{
					if(selectOne.InlayGemDic.ContainsKey(i+1))
						gemSlotUIs[i].SetData(selectOne.InlayGemDic[i+1]);	
					else
						gemSlotUIs[i].SetEmpty();
				}
			}
		}
	}
	/// <summary>
	/// 清空宝石槽位上的显示
	/// </summary>
	void ClearGemSlot()
	{
		if(gemSlotUIs != null)
		{
			for (int i = 0,max=gemSlotUIs.Length; i < max; i++) 
			{
				if(gemSlotUIs[i] != null)
					gemSlotUIs[i].SetEmpty();
			}
		}
	}
}
