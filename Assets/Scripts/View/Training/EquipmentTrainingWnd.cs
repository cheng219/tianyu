//==============================================
//作者：邓成
//日期：2016/3/23
//用途：装备培养界面类
//==============================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EquipmentTrainingWnd : GUIBase {
	public UIToggle[] toggles;

	public DecomposeItemUI equipGo;
	/// <summary>
	/// 父节点
	/// </summary>
	public UIGrid itemParent;
	public UIScrollView scrollView;
	public UIToggle toggleEquip;
	public UIToggle toggleBag;
	protected Dictionary<int,DecomposeItemUI> allGoItems = new Dictionary<int, DecomposeItemUI>();
	/// <summary>
	/// 右侧装备排列的位置及间隔(起始X 起始Y X增量 Y增量)
	/// </summary>
	public Vector4 positionInfo = new Vector4(128,-26,0,90);

	public UIButton btnClose;

	public UIFxAutoActive normalFx;
	public UIFxAutoActive washFx;

	protected bool firstOpenWnd = false;

	enum EquipmentTrainingType
	{
		/// <summary>
		/// 强化
		/// </summary>
		Strengthening,
		/// <summary>
		/// 升阶
		/// </summary>
		Upgrade,
		/// <summary>
		/// 橙炼
		/// </summary>
		OrangeRefine,
		/// <summary>
		/// 洗练
		/// </summary>
		Wash,
		/// <summary>
		/// 镶嵌
		/// </summary>
		Inlay,
		/// <summary>
		/// 继承
		/// </summary>
		Extend,
	}
	void Awake()
	{
		layer = GUIZLayer.NORMALWINDOW;
		mutualExclusion = true; 
		allSubWndNeedInstantiate = true;
	}
	protected UIToggle lastChangeToggle = null;
	protected void ClickToggleEvent(GameObject go)
	{
		UIToggle toggle = go.GetComponent<UIToggle>();
		if(toggle != lastChangeToggle)
		{
			OnChange();
		}
		if(toggle != null && toggle.value)lastChangeToggle = toggle;
	}

	protected UIToggle lastEquipToggle = null;
	protected void ClickEquipToggle(GameObject go)
	{
		UIToggle toggle = go.GetComponent<UIToggle>();
		if(toggle != lastEquipToggle)
		{
			OnChangeEquipList();
		}
		if(toggle != null && toggle.value)lastEquipToggle = toggle;
	}

	protected override void OnOpen ()
	{
		if(initSubGUIType == SubGUIType.NONE)initSubGUIType = SubGUIType.STRENGTHING;
		base.OnOpen ();
		firstOpenWnd = true;
	}
	protected override void OnClose ()
	{
		base.OnClose ();
		firstOpenWnd = false;
		GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo = null;
	}
	protected override void HandEvent (bool _bind)
	{
		base.HandEvent (_bind);
		if(_bind)
		{
			if(btnClose != null)UIEventListener.Get(btnClose.gameObject).onClick = CloseWnd;
			if (toggles != null)
			{
				for (int i = 0, max = toggles.Length; i < max; i++)
				{
					if(toggles[i] != null)UIEventListener.Get(toggles[i].gameObject).onClick = ClickToggleEvent;
				}
			} 
			if (toggleEquip != null)UIEventListener.Get(toggleEquip.gameObject).onClick = ClickEquipToggle;
			if (toggleBag != null)UIEventListener.Get(toggleBag.gameObject).onClick = ClickEquipToggle;

			GameCenter.equipmentTrainingMng.OnShowEquipmentBySlot += ShowEquipmentBySlot;
			GameCenter.equipmentTrainingMng.OnUpgradeEquipmentUpdateEvent += OnChangeEquipList;
			GameCenter.inventoryMng.OnBackpackUpdate += RefreshRedTip;
            GameCenter.inventoryMng.OnBackpackUpdate += OnChangeEquipListForExtend;
            GameCenter.inventoryMng.OnEquipUpdate += OnChangeEquipListForExtend;
			GameCenter.equipmentTrainingMng.OnEquipmentTrainingSucessEvent += ShowEffect;
		}else
		{
			if(btnClose != null)UIEventListener.Get(btnClose.gameObject).onClick = null;
			if (toggles != null)
			{
				for (int i = 0, max = toggles.Length; i < max; i++)
				{
					if(toggles[i] != null)UIEventListener.Get(toggles[i].gameObject).onClick = null;
				}
			} 
			if (toggleEquip != null)UIEventListener.Get(toggleEquip.gameObject).onClick = null;
			if (toggleBag != null)UIEventListener.Get(toggleBag.gameObject).onClick = null;

			GameCenter.equipmentTrainingMng.OnShowEquipmentBySlot -= ShowEquipmentBySlot;
			GameCenter.equipmentTrainingMng.OnUpgradeEquipmentUpdateEvent -= OnChangeEquipList;
			GameCenter.inventoryMng.OnBackpackUpdate -= RefreshRedTip;
            GameCenter.inventoryMng.OnBackpackUpdate -= OnChangeEquipListForExtend;
            GameCenter.inventoryMng.OnEquipUpdate -= OnChangeEquipListForExtend;
			GameCenter.equipmentTrainingMng.OnEquipmentTrainingSucessEvent -= ShowEffect;
		}
	}
	/// <summary>
	/// 从外面打开子界面后,选中对应的标签(toggle)
	/// </summary>
	protected override void InitSubWndState ()
	{
		base.InitSubWndState ();
		UIToggle toggle = null;
		switch(initSubGUIType)
		{
		case SubGUIType.STRENGTHING:
			if(toggles != null && toggles.Length > (int)EquipmentTrainingType.Strengthening)toggle = toggles[(int)EquipmentTrainingType.Strengthening];
			break;
		case SubGUIType.EQUIPMENTWASH:
			if(toggles != null && toggles.Length > (int)EquipmentTrainingType.Wash)toggle = toggles[(int)EquipmentTrainingType.Wash];
			break;
		case SubGUIType.EQUIPMENTINLAY:
			if(toggles != null && toggles.Length > (int)EquipmentTrainingType.Inlay)toggle = toggles[(int)EquipmentTrainingType.Inlay];
			break;
		case SubGUIType.EQUIPMENTUPGRADE:
			if(toggles != null && toggles.Length > (int)EquipmentTrainingType.Upgrade)toggle = toggles[(int)EquipmentTrainingType.Upgrade];
			break;
		case SubGUIType.ORANGEREFINE:
			if(toggles != null && toggles.Length > (int)EquipmentTrainingType.OrangeRefine)toggle = toggles[(int)EquipmentTrainingType.OrangeRefine];
			break;
		case SubGUIType.EQUIPMENTEXTEND:
			if(toggles != null && toggles.Length > (int)EquipmentTrainingType.Extend)toggle = toggles[(int)EquipmentTrainingType.Extend];
			break;
		}
		if(toggle != null)
		{
			toggle.value = true;
			ClickToggleEvent(toggle.gameObject);
		}
	}

	protected EquipSlot curSlot = EquipSlot.None;
	/// <summary>
	/// 筛选继承副装备,只需根据槽位筛选
	/// </summary>
	void ShowEquipmentBySlot(EquipSlot slot)
	{
		curSlot = slot;
		if(toggleEquip.value)
		{
			if(slot != EquipSlot.None)//筛选副装备
				ShowEquipItems(GameCenter.inventoryMng.GetPlayerEquipDic(),false);
			else//筛选主装备(必须能继承)
				ShowEquipItems(FilterItemsByType(GameCenter.inventoryMng.GetPlayerEquipDic(),CurEquipmentTrainingType),false);
		}else
		{
			if(slot != EquipSlot.None)
				ShowBagItems(GameCenter.inventoryMng.GetBackpackEquipDic(),false);
			else
				ShowBagItems(FilterItemsByType(GameCenter.inventoryMng.GetBackpackEquipDic(),CurEquipmentTrainingType),false);
		}
	}

	EquipmentTrainingType CurEquipmentTrainingType = EquipmentTrainingType.Strengthening;
	void OnChange()
	{
		if(toggleBag != null)toggleBag.transform.localScale = Vector3.one;
		for (int i = 0,max=toggles.Length; i < max; i++) {
			if(toggles[i].value)
			{
				CurEquipmentTrainingType = (EquipmentTrainingType)i;
				//subWndArray[i].OpenUI();
				SwitchToSubWnd(subWndArray[i].type);
				switch(CurEquipmentTrainingType)
				{
				case EquipmentTrainingType.Strengthening:
					ShowEquipItems(FilterItemsByType(GameCenter.inventoryMng.GetPlayerEquipDic(),EquipmentTrainingType.Strengthening),(GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo == null));
					toggleEquip.value = true;
					lastEquipToggle = toggleEquip;
					toggleBag.transform.localScale = Vector3.zero;
					break;
				case EquipmentTrainingType.Inlay:
					ShowEquipItems(FilterItemsByType(GameCenter.inventoryMng.GetPlayerEquipDic(),EquipmentTrainingType.Inlay));
					toggleEquip.value = true;
					lastEquipToggle = toggleEquip;
					toggleBag.transform.localScale = Vector3.zero;
					break;
				case EquipmentTrainingType.Wash:
					ShowEquipItems(FilterItemsByType(GameCenter.inventoryMng.GetPlayerEquipDic(),EquipmentTrainingType.Wash));
					toggleEquip.value = true;
					lastEquipToggle = toggleEquip;
					toggleBag.transform.localScale = Vector3.zero;
					break;
				case EquipmentTrainingType.Extend:
					ShowBagItems(FilterItemsByType(GameCenter.inventoryMng.GetBackpackEquipDic(),EquipmentTrainingType.Extend),false);
					toggleBag.value = true;
					lastEquipToggle = toggleBag;
					break;
				case EquipmentTrainingType.Upgrade:
					ShowEquipItems(FilterItemsByType(GameCenter.inventoryMng.GetPlayerEquipDic(),EquipmentTrainingType.Upgrade));
					toggleEquip.value = true;
					lastEquipToggle = toggleEquip;
					break;
				case EquipmentTrainingType.OrangeRefine:
					ShowEquipItems(FilterItemsByType(GameCenter.inventoryMng.GetPlayerEquipDic(),EquipmentTrainingType.OrangeRefine));
					toggleEquip.value = true;
					lastEquipToggle = toggleEquip;
					break;
				}
			}else
			{
				subWndArray[i].CloseUI();
			}
		}
	}
    void OnChangeEquipListForExtend()
    {
        if (initSubGUIType != SubGUIType.EQUIPMENTEXTEND)
            return;
        OnChangeEquipList();
    }
	void OnChangeEquipList()
	{
		if(toggleBag != null && toggleBag.value)
		{
			ShowBagItems(FilterItemsByType(GameCenter.inventoryMng.GetBackpackEquipDic(),CurEquipmentTrainingType),!(initSubGUIType == SubGUIType.EQUIPMENTEXTEND));
		}
		if(toggleEquip != null && toggleEquip.value)
		{
			bool checkFirst = true;
			if(initSubGUIType == SubGUIType.EQUIPMENTEXTEND)
				checkFirst = false;
			if(firstOpenWnd && initSubGUIType == SubGUIType.STRENGTHING && GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo != null)
				checkFirst = false;//物品描述界面点锻造跳转进来,选中点击的装备,不选中第一个
			ShowEquipItems(FilterItemsByType(GameCenter.inventoryMng.GetPlayerEquipDic(),CurEquipmentTrainingType),checkFirst);
		}
	}
	/// <summary>
	/// 显示背包中的装备
	/// </summary>
	/// <param name="itemDic">装备列表</param>
	/// <param name="needCheckFirst">If set to <c>true</c> 是否需要选中第一个装备</param>
	void ShowBagItems(Dictionary<int,EquipmentInfo> itemDic,bool needCheckFirst = true)
	{
		HideAllGoItems();
		int index = 0;
		DecomposeItemUI firstOne = null;
		List<EquipmentInfo> itemList = new List<EquipmentInfo>(itemDic.Values);
		itemList.Sort(CompareEquip);
		for(int i=0,count=itemList.Count;i<count;i++)
		{
			EquipmentInfo info = itemList[i];
			if(info == null)continue;
			EquipmentInfo selectOne = GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo;
			if(GameCenter.equipmentTrainingMng.CurSlot != EquipSlot.None && (GameCenter.equipmentTrainingMng.CurSlot != info.Slot || (selectOne != null && (selectOne.NeedProf != info.NeedProf || info.InstanceID == selectOne.InstanceID))))continue;
			//选中了副装备就隐藏副装备
			if(GameCenter.equipmentTrainingMng.CurViceEquipmentInfo != null && GameCenter.equipmentTrainingMng.CurViceEquipmentInfo.InstanceID == info.InstanceID)continue;
			DecomposeItemUI itemUI = null;
			if(!allGoItems.ContainsKey(index))
			{
				if(equipGo != null)itemUI = equipGo.CreateNew(itemParent.transform);
				allGoItems[index] = itemUI;
			}
			itemUI = allGoItems[index];
			itemUI.gameObject.SetActive(true);
			itemUI.SetData(info,ChooseBagItem,initSubGUIType);
			if(index == 0 && needCheckFirst)firstOne = itemUI;
			index++;
		}
		if(firstOne != null && needCheckFirst)firstOne.SetChecked();
		if(scrollView != null)scrollView.SetDragAmount(0,0,false);
		if(itemParent != null)itemParent.repositionNow = true;
		if(itemDic.Count == 0)
			GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo = null;
	}
	/// <summary>
	/// 显示身上的装备
	/// </summary>
	/// <param name="itemDic">装备列表</param>
	/// <param name="needCheckFirst">If set to <c>true</c> 是否需要选中第一个装备</param>	
	void ShowEquipItems(Dictionary<EquipSlot,EquipmentInfo> itemDic,bool needCheckFirst = true)
	{
		HideAllGoItems();
		int index = 0;
		DecomposeItemUI firstOne = null;
		DecomposeItemUI checkedOne = null;
		List<EquipmentInfo> itemList = new List<EquipmentInfo>(itemDic.Values);
		itemList.Sort(CompareEquip);
		for(int i=0,count=itemList.Count;i<count;i++)
		{
			EquipmentInfo info = itemList[i];
			if(info == null)continue;
			EquipmentInfo selectOne = GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo;
			//继承的副装备必须部位相同、职业相同、且排除主装备本身
			if(GameCenter.equipmentTrainingMng.CurSlot != EquipSlot.None && (GameCenter.equipmentTrainingMng.CurSlot != info.Slot || (selectOne != null && (selectOne.NeedProf != info.NeedProf || info.InstanceID == selectOne.InstanceID))))continue;
			//选中了副装备就隐藏副装备
			if(GameCenter.equipmentTrainingMng.CurViceEquipmentInfo != null && GameCenter.equipmentTrainingMng.CurViceEquipmentInfo.InstanceID == info.InstanceID)continue;
			DecomposeItemUI itemUI = null;
			if(!allGoItems.ContainsKey(index))
			{
				if(equipGo != null)itemUI = equipGo.CreateNew(itemParent.transform);
				allGoItems[index] = itemUI;
			}
			itemUI = allGoItems[index];
			itemUI.gameObject.SetActive(true);
			itemUI.SetData(info,ChooseBagItem,initSubGUIType);
			if(index == 0 && needCheckFirst)firstOne = itemUI;
			if(!needCheckFirst && selectOne != null && info.InstanceID == selectOne.InstanceID)checkedOne = itemUI;//不是第一个的默认选中
			index++;
		}
		if(firstOne != null && needCheckFirst)firstOne.SetChecked();
		if(!needCheckFirst && checkedOne != null)checkedOne.SetChecked();//不是第一个的默认选中
		if(scrollView != null)scrollView.SetDragAmount(0,0,false);
		if(itemParent != null)itemParent.repositionNow = true;
		if(itemDic.Count == 0)
			GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo = null;
	}
	void HideAllGoItems()
	{
		foreach(var go in allGoItems.Values)
		{
			//if(go != null)go.transform.localScale = Vector3.zero;
			if(go != null)go.gameObject.SetActive(false);
		}
	}
	/// <summary>
	/// 刷新展示出来的装备的红点(背包中材料变化即刷新)
	/// </summary>
	void RefreshRedTip()
	{
		foreach(var go in allGoItems.Values)
		{
			if(go != null && go.gameObject.activeSelf)
			{
				DecomposeItemUI itemUI = go.GetComponent<DecomposeItemUI>();
				if(itemUI != null)itemUI.ShowRedTip();
			}
		}
	}
	/// <summary>
	/// 根据不同界面筛选装备(不同的分页筛选的条件不同)
	/// </summary>
	Dictionary<EquipSlot,EquipmentInfo> FilterItemsByType(Dictionary<EquipSlot,EquipmentInfo> itemDic,EquipmentTrainingType type)
	{
		Dictionary<EquipSlot,EquipmentInfo> dic = new Dictionary<EquipSlot, EquipmentInfo>();
		foreach(EquipmentInfo info in itemDic.Values)
		{
			switch(type)
			{
			case EquipmentTrainingType.Upgrade:
				if(info.CanUpgrade)dic[info.Slot] = info;
				break;
			case EquipmentTrainingType.OrangeRefine:
				if(info.CanOrangeRefine)dic[info.Slot] = info;
				break;
			case EquipmentTrainingType.Wash:
				if(info.CanWash)dic[info.Slot] = info;
				break;
			case EquipmentTrainingType.Strengthening:
				dic[info.Slot] = info;
				break;
			case EquipmentTrainingType.Inlay:
				if(info.CanInlay)dic[info.Slot] = info;
				break;
			case EquipmentTrainingType.Extend:
				if(curSlot == EquipSlot.None && info.CanExtend)//继承功能只有选择主装备时(curSlot == EquipSlot.None)才筛选
					dic[info.Slot] = info;
				if(curSlot != EquipSlot.None)//根据SLOT筛选副装备
					dic[info.Slot] = info;
				break;
			}
		}
		return dic;
	}
	/// <summary>
	/// 根据不同界面筛选装备(不同的分页筛选的条件不同)
	/// </summary>
	Dictionary<int,EquipmentInfo> FilterItemsByType(Dictionary<int,EquipmentInfo> itemDic,EquipmentTrainingType type)
	{
		Dictionary<int,EquipmentInfo> dic = new Dictionary<int,EquipmentInfo>();
		foreach(EquipmentInfo info in itemDic.Values)
		{
			switch(type)
			{
			case EquipmentTrainingType.Upgrade:
				if(info.CanUpgrade)dic[info.InstanceID] = info;
				break;
			case EquipmentTrainingType.OrangeRefine:
				if(info.CanOrangeRefine)dic[info.InstanceID] = info;
				break;
			case EquipmentTrainingType.Wash:
				if(info.CanWash)dic[info.InstanceID] = info;
				break;
			case EquipmentTrainingType.Strengthening:
				dic[info.InstanceID] = info;
				break;
			case EquipmentTrainingType.Inlay:
				if(info.CanInlay)dic[info.InstanceID] = info;
				break;
			case EquipmentTrainingType.Extend:
				if(curSlot == EquipSlot.None && info.CanExtend)//继承功能只有已选择主装备是才筛选
					dic[info.InstanceID] = info;
				if(curSlot != EquipSlot.None)//根据SLOT筛选副装备
					dic[info.InstanceID] = info;
				break;
			}
		}
		return dic;
	}

	/// <summary>
	/// 选中右边的物品
	/// </summary>
	void ChooseBagItem(GameObject go)
	{
		EquipmentInfo info = (EquipmentInfo)UIEventListener.Get(go).parameter;
		if(initSubGUIType == SubGUIType.EQUIPMENTEXTEND && GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo != null)
		{
			GameCenter.equipmentTrainingMng.CurViceEquipmentInfo = info;//此时选择的是继承的副装备
		}else
		{
			GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo = info;
		}
	}
	void CloseWnd(GameObject go)
	{
		GameCenter.uIMng.SwitchToUI(GUIType.NONE);
	}
	/// <summary>
	/// 根据sort排序
	/// </summary>
	int CompareEquip(EquipmentInfo eq1,EquipmentInfo eq2)
	{
        if (eq1.Quality > eq2.Quality)
            return -1;
        else if (eq1.Quality < eq2.Quality)
            return 1;
        if (eq1.LV > eq2.LV)
            return -1;
        else if (eq1.LV < eq2.LV)
            return 1;
        int oldSort1 = eq1.OldSort;
        int oldSort2 = eq2.OldSort;
        if (oldSort1 == 2) oldSort1 = 12;
        else if (oldSort1 == 12) oldSort1 = 2;
        if (oldSort2 == 2) oldSort2 = 12;
        else if (oldSort2 == 12) oldSort2 = 2;
        if (oldSort1 > oldSort2)
			return -1;
        else if (oldSort1 < oldSort2)
			return 1;
		return 0;
	}

	protected void ShowEffect()
	{
		if(initSubGUIType == SubGUIType.EQUIPMENTWASH)
		{
			if(washFx != null)washFx.ShowFx();
		}else
		{
			if(normalFx != null)normalFx.ShowFx();
		}
	}
}
