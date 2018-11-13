//====================================================
//作者：邓成
//日期：2016/3/2
//用途：物品分解界面
//====================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DecompositionWnd : SubWnd {
	public int guideDecompositionItemID = 5500325;

	public UIPanel bagItemsPanel;
	public UIButton btnOneKey;
	public UIButton btnDecomposition;
	public UIButton btnSee;
	public UILabel labWillResult;
	public UILabel labPrefabResult;
	public UIFxAutoActive uiFx;
	/// <summary>
	/// 选择分解的物品
	/// </summary>
	public ItemUI[] chooseItems;
	/// <summary>
	/// 移除按钮
	/// </summary>
	public GameObject[] btnRemoves;

	public Vector4 positionInfo = new Vector4(128,-26,0,90);
	/// <summary>
	/// key表示对应的位置,从0开始
	/// </summary>
	protected Dictionary<int,EquipmentInfo> chooseBagItems = new Dictionary<int, EquipmentInfo>();
	/// <summary>
	/// key表示物品的唯一ID
	/// </summary>
	protected Dictionary<int,EquipmentInfo> tempBagItems = new Dictionary<int, EquipmentInfo>();

	protected Dictionary<int,GameObject> allGoItems = new Dictionary<int, GameObject>();

	void Awake()
	{
		if(btnDecomposition != null)UIEventListener.Get(btnDecomposition.gameObject).onClick = DecompositionAll;
		if(btnOneKey != null)UIEventListener.Get(btnOneKey.gameObject).onClick = OneKeyAdd;
		if(btnSee != null)UIEventListener.Get(btnSee.gameObject).onClick = PreviewResult;
		CleanData();
	}
	protected override void OnOpen ()
	{
		base.OnOpen ();
		GetEquipFromBag();
		ShowBagItems();
	}
	protected override void OnClose ()
	{
		base.OnClose ();
		CleanData();
	}
	protected override void HandEvent (bool _bind)
	{
		base.HandEvent (_bind);
		if(_bind)
		{
			GameCenter.inventoryMng.OnGetDecomposeResult += DecomposeResult;
		}else
		{
			GameCenter.inventoryMng.OnGetDecomposeResult -= DecomposeResult;
		}
	}
	void GetEquipFromBag()
	{
		tempBagItems.Clear();
		EquipmentInfo guideItem = GameCenter.inventoryMng.GetEquipByType(guideDecompositionItemID);
		if(guideItem != null)
		{
			tempBagItems.Add(guideItem.InstanceID,guideItem);
		}
		Dictionary<int,EquipmentInfo> backDic = GameCenter.inventoryMng.GetBackpackEquipAndMountEquipDic();
		using(var e = backDic.GetEnumerator())
		{
			while(e.MoveNext())
			{
				EquipmentInfo info = e.Current.Value;
				if(info.Slot != EquipSlot.None && info.StackCurCount != 0 && info.EID != guideDecompositionItemID)
					tempBagItems[info.InstanceID] = info;
			}
		}
	}

	void RefreshWnd()
	{
		ShowBagItems();
		ShowChooseData();
	}
	#region 右侧逻辑
	void ShowBagItems()
	{
		HideAllGoItems();
		int index = 0;
		using(var e = tempBagItems.GetEnumerator())
		{
			while(e.MoveNext())
			{
				EquipmentInfo info = e.Current.Value;
				if(info == null)continue;
				GameObject go = null;
				if(!allGoItems.ContainsKey(info.InstanceID))
				{
					go = Instantiate(exResources.GetResource(ResourceType.GUI,"Backpack/DecomposeItem")) as GameObject;
					allGoItems[info.InstanceID] = go;
				}
				go = allGoItems[info.InstanceID];
				go.SetActive(true);
				go.transform.parent = bagItemsPanel.transform;
				go.transform.localPosition = new Vector3(positionInfo.x,positionInfo.y - positionInfo.w*index,-1);
				go.transform.localScale = Vector3.one;
				DecomposeItemUI bagItemUI = go.GetComponent<DecomposeItemUI>();
				go = null;
				bagItemUI.SetData(info,ChooseBagItem);
				index++;
			}
		}
	}
	void HideAllGoItems()
	{
		foreach(GameObject go in allGoItems.Values)
		{
			if(go != null)go.SetActive(false);
		}
		using(var e = allGoItems.GetEnumerator())
		{
			while(e.MoveNext())
			{
				GameObject go = e.Current.Value;
				if(go != null)go.SetActive(false);
			}
		}
	}
	/// <summary>
	/// 从背包中选中物品操作
	/// </summary>
	void ChooseBagItem(GameObject go)
	{
		EquipmentInfo info = (EquipmentInfo)UIEventListener.Get(go).parameter;
		ChooseOne(info);
	}
	/// <summary>
	/// 从背包中选中物品逻辑
	/// </summary>
	void ChooseOne(EquipmentInfo info)
	{
		if(ChooseCount >= 10)
		{
			GameCenter.messageMng.AddClientMsg(449);
			return;
		}
		if(info != null)
		{
			tempBagItems[info.InstanceID] = null;
		}
		for(int index=0;index < 10;index++)
		{
			if(!chooseBagItems.ContainsKey(index) || chooseBagItems[index] == null)
			{
				chooseBagItems[index] = info;
				break;
			}
		}
		RefreshWnd();
	}
	#endregion

	#region 左侧逻辑
	int ChooseCount
	{
		get
		{
			int count = 0;
			using(var e = chooseBagItems.GetEnumerator())
			{
				while(e.MoveNext())
				{
					var item = e.Current.Value;
					if(item != null)
						count++;
				}
			}
			return count;
		}
	}
	void CleanData()
	{
		if(chooseItems == null || btnRemoves == null)return;
		for(int i=0;i<10;i++)
		{
			if(chooseItems.Length > i && btnRemoves.Length > i)
			{
				if(chooseItems[i] != null)
					chooseItems[i].FillInfo(null);
				if(btnRemoves[i] != null)
				{	
					UIEventListener.Get(btnRemoves[i].gameObject).onClick = RemoveItem;
					UIEventListener.Get(btnRemoves[i].gameObject).parameter = i;//按钮位置
					btnRemoves[i].SetActive(false);
				}
			}
		}
		chooseBagItems.Clear();
	}
	void ShowChooseData()
	{
		if(chooseItems == null || btnRemoves == null)return;
		for(int i=0;i<10;i++)
		{
			if(chooseItems.Length > i && btnRemoves.Length > i)
			{
				EquipmentInfo info = chooseBagItems.ContainsKey(i)?chooseBagItems[i]:null;
				if(chooseItems[i] != null)
				{
					chooseItems[i].FillInfo(info);
				}
				if(btnRemoves[i] != null)
				{	
					if(btnRemoves[i] != null)btnRemoves[i].SetActive(info != null);
				}
			}
		}
	}
	/// <summary>
	/// 移除不要分解的物品逻辑
	/// </summary>
	void RemoveOne(int index)
	{
		if(chooseBagItems.ContainsKey(index) && chooseBagItems[index] != null)
		{
			EquipmentInfo removeOne = chooseBagItems[index];
			chooseBagItems[index] = null;
			tempBagItems[removeOne.InstanceID] = removeOne;
		}else
		{
			Debug.Log("检查分解逻辑");
		}
		RefreshWnd();
	}
	/// <summary>
	/// 移除不要分解的物品操作
	/// </summary>
	protected void RemoveItem(GameObject go)
	{
		int index = (int)UIEventListener.Get(go).parameter;
		if(chooseItems.Length > index && chooseItems[index] != null)
		{	
			chooseItems[index].FillInfo(null);
			RemoveOne(index);
		}
	}
	protected void DecompositionAll(GameObject go)
	{
		List<uint> equipList = new List<uint>();
        bool haveHighQuality = false;
		using(var e = chooseBagItems.GetEnumerator())
		{
			while(e.MoveNext())
			{
				EquipmentInfo item = e.Current.Value;
                if (item != null)
                {
                    equipList.Add((uint)item.InstanceID);
                    if (item.Quality > 3)//有紫色以上的装备
                        haveHighQuality = true;
                }
			}
		}
        if (haveHighQuality && !GameCenter.inventoryMng.DontTipHighQualityForDecompose)
        {
            MessageST mst = new MessageST();
            object[] pa = { 1 };
            mst.pars = pa;
            mst.delPars = delegate(object[] ob)
            {
                if (ob.Length > 0)
                {
                    bool b = (bool)ob[0];
                    if (b)
                        GameCenter.inventoryMng.DontTipHighQualityForDecompose = true;
                }
            };
            mst.messID = 491;
            mst.delYes = delegate
            {
                GameCenter.inventoryMng.C2S_DecompositionEquip(equipList);
            };
            GameCenter.messageMng.AddClientMsg(mst);
            return;
        }
        if (equipList.Count <= 0)
            GameCenter.messageMng.AddClientMsg(448);
        else
            GameCenter.inventoryMng.C2S_DecompositionEquip(equipList);
	}
	protected void OneKeyAdd(GameObject go)
	{
		int index = 0;
		GetEquipFromBag();
		chooseBagItems.Clear();
		Dictionary<int,EquipmentInfo> restEqDic = new Dictionary<int, EquipmentInfo>();
		using(var e = tempBagItems.GetEnumerator())
		{
			while(e.MoveNext())
			{
				EquipmentInfo info = e.Current.Value;
				if(info != null)
				{
					if(index < 10)
					{
						chooseBagItems[index] = info;
						index++;
					}else
					{
						restEqDic[info.InstanceID] = info;
					}
				}
			}
		}
		tempBagItems = restEqDic;
		RefreshWnd();
	}
	protected void PreviewResult(GameObject go)
	{
		if(labWillResult != null)
		{
			labWillResult.text = PreviewOne(chooseBagItems);
		}
	}
	string PreviewOne(Dictionary<int,EquipmentInfo> equipDic)
	{
		System.Text.StringBuilder builder = new System.Text.StringBuilder();
		Dictionary<int,ItemValue> itemList = new Dictionary<int,ItemValue>();
		int coinNum = 0;
		bool haveHighQualityEquip = false;
		using(var e = equipDic.GetEnumerator())
		{
			while(e.MoveNext())
			{
				EquipmentInfo temp = e.Current.Value;
				if(temp == null)continue;
				coinNum += temp.Price;
				//if(temp.Quality <= 3)continue;//蓝色及以下只有铜币
                if (temp.IsEquip)
                {
                    haveHighQualityEquip = true;//有稀有物品
                    StrengthenRef strengthenRef = ConfigMng.Instance.GetStrengthenRefByLv(temp.UpgradeLv);
                    if (strengthenRef != null)
                    {
                        for (int j = 0, maxJ = strengthenRef.decoItems.Count; j < maxJ; j++)
                        {
                            ItemValue item = strengthenRef.decoItems[j];
                            if (item.eid == 5)// 分解强化的装备也给钱
                                coinNum += item.count;
                            else
                                AddOne(itemList, item);
                        }
                    }
                    List<ItemValue> luckyItems = ConfigMng.Instance.GetEquipmentListByLuckyLv(temp.LuckyLv);
                    for (int i = 0, max = luckyItems.Count; i < max; i++)
                    {
                        ItemValue item = luckyItems[i];
                        AddOne(itemList, item);
                    }
                    List<ItemValue> qualityList = ConfigMng.Instance.GetResolveItemListByLv(temp.UseReqLevel, temp.Quality, (int)temp.Slot);
                    for (int h = 0, maxh = qualityList.Count; h < maxh; h++)
                    {
                        ItemValue item = qualityList[h];
                        AddOne(itemList, item);
                    }
                }
                else if (temp.Family == EquipmentFamily.MOUNTEQUIP)
                {
                    MountStrenConsumeRef mouuntStren = ConfigMng.Instance.GetMountStrenConsumeRef(temp.UpgradeLv);
                    if (mouuntStren != null)
                    {
                        if (mouuntStren.deco_cons != null && mouuntStren.deco_cons.eid != 0) AddOne(itemList, mouuntStren.deco_cons);
                    }
                    MountEquQuailtRef quality = ConfigMng.Instance.GetMountEquipQualityRef(temp.Quality, temp.Slot);
                    if (quality != null)
                    {
                        if (quality.deco_cons != null && quality.deco_cons.eid != 0) AddOne(itemList, quality.deco_cons);
                    }
                }
			}
		}
		if(labPrefabResult != null)//预制描述
            labPrefabResult.enabled = (equipDic.Count == 0);
		if(labWillResult != null)//描述
            labWillResult.enabled = (equipDic.Count != 0);
		builder.Append(ConfigMng.Instance.GetUItext(270));
        if (coinNum > 0) builder.Append(ConfigMng.Instance.GetUItext(271)).Append(coinNum);
		using(var e = itemList.GetEnumerator())
		{
			while(e.MoveNext())
			{
				ItemValue itemVal = e.Current.Value;
				builder.Append("  ");
				EquipmentInfo info = new EquipmentInfo(itemVal,EquipmentBelongTo.NONE);
				if(info != null)
				{
					builder.Append(info.ItemName).Append("*").Append(itemVal.count);
				}
			}
		}
		if(haveHighQualityEquip)builder.Append(ConfigMng.Instance.GetUItext(272));
		return builder.ToString();
	}
	void AddOne(Dictionary<int,ItemValue> itemList,ItemValue item)
	{
		if(itemList.ContainsKey(item.eid))
		{
			itemList[item.eid] = new ItemValue(item.eid,itemList[item.eid].count + item.count);
		}else
		{
			itemList[item.eid] = item;
		}
	}
	#endregion

	void DecomposeResult()
	{
		CleanData();
		if(uiFx != null)
			uiFx.ShowFx();
	}
}
