//==============================================
//作者：邓成
//日期：2016/3/31
//用途：装备洗练界面类
//==============================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class EquipmentWashWnd : SubWnd {
	public UIButton btnWash;
	public UIButton btnSave;
	/// <summary>
	/// 批量洗练	
	/// </summary>
	public UIButton btnOpenBatchWash;
	public UIButton btnCloseBatchWash;
	public UIButton btnXBatchWash;
	public UIButton btnBatchWash;
	public UIButton btnSaveWash;

	public UIButton btnSuperWash;
	public UIButton btnBatchSuperWash;
	public GameObject normalWashBtns;
	public GameObject normalBatchWashBtns;

	public UIProgressBar progressBar;
	public UILabel progressNum;
	public UIProgressBar batchProgressBar;
	public UILabel batchProgressNum;

	public UILabel washStone;

	public UILabel batchWashStone;

	public ItemUI chooseEquip;
	public UILabel[] curAttr;
	public UIToggle[] attrLock;
	public UIButton[] attrLockBtn;

	public UILabel[] batchCurAttr;
	public UIToggle[] batchAttrLock;
	public UIButton[] batchAttrLockBtn;
	public BatchWashItemUI[] batchAttr;

	public UIToggle toggleQuickBuy;
	public UILabel quickCost;

	public UIToggle batchToggleQuickBuy;
	public UILabel batchQuickCost;
    public UIFxAutoActive washProFx;//洗练进度条特效
	protected List<int> WashLockList
	{
		get
		{
			List<int> lockList = new List<int>();
			if(attrLock != null)
			{
				for (int i = 0,max=attrLock.Length; i < max; i++) 
				{
					if(attrLock[i].gameObject.activeSelf && attrLock[i].value)lockList.Add(i+1);
				}
			}
			return lockList;
		}
	}
	protected int LockCount
	{
		get
		{		
			int count = 0;
			if(attrLock != null)
			{
				for (int i = 0,max=attrLock.Length; i < max; i++) 
				{
					if(attrLock[i].gameObject.activeSelf)count++;
				}
			}
			return count;
		}
	}
	protected List<int> BatchWashLockList
	{
		get
		{
			List<int> lockList = new List<int>();
			if(batchAttrLock != null)
			{
				for (int i = 0,max=batchAttrLock.Length; i < max; i++) 
				{
					if(batchAttrLock[i].gameObject.activeSelf && batchAttrLock[i].value)lockList.Add(i+1);
				}
			}
			return lockList;
		}
	}
	protected int BatchLockCount
	{
		get
		{		
			int count = 0;
			if(batchAttrLock != null)
			{
				for (int i = 0,max=batchAttrLock.Length; i < max; i++) 
				{
					if(batchAttrLock[i].gameObject.activeSelf)count++;
				}
			}
			return count;
		}
	}

	void AddDelegate()
	{
		if(btnWash != null)UIEventListener.Get(btnWash.gameObject).onClick = WashEquip;
		if(btnSave != null)UIEventListener.Get(btnSave.gameObject).onClick = SaveWashAttr;
		if(btnOpenBatchWash != null)UIEventListener.Get(btnOpenBatchWash.gameObject).onClick = OpenBatchWashPanel;
		if(btnCloseBatchWash != null)UIEventListener.Get(btnCloseBatchWash.gameObject).onClick = CloseBatchWashPanel;
		if(btnXBatchWash != null)UIEventListener.Get(btnXBatchWash.gameObject).onClick = CloseBatchWashPanel;
		if(btnBatchWash != null)UIEventListener.Get(btnBatchWash.gameObject).onClick = BatchWashEquip;
		if(btnSaveWash != null)UIEventListener.Get(btnSaveWash.gameObject).onClick = SaveBatchAttr;
		if(btnSuperWash != null)UIEventListener.Get(btnSuperWash.gameObject).onClick = SuperWashEquip;
		if(btnBatchSuperWash != null)UIEventListener.Get(btnBatchSuperWash.gameObject).onClick = BatchSuperWashEquip;

		if(attrLockBtn != null)
		{
			for (int i = 0,max=attrLockBtn.Length; i < max; i++) {
				if(attrLockBtn[i] != null)UIEventListener.Get(attrLockBtn[i].gameObject).onClick = OnClickLock;
			}
		}
		if(batchAttrLockBtn != null)
		{
			for (int i = 0,max=batchAttrLockBtn.Length; i < max; i++) {
				if(batchAttrLockBtn[i] != null)UIEventListener.Get(batchAttrLockBtn[i].gameObject).onClick = OnClickBatchLock;
			}
		}
	}
	protected override void OnOpen ()
	{
		base.OnOpen ();
		AddDelegate();
		if(toggleQuickBuy != null)toggleQuickBuy.value = false;
		if(batchToggleQuickBuy != null)batchToggleQuickBuy.value = false;
	}
	protected override void OnClose ()
	{
		base.OnClose ();
	}
	protected override void HandEvent (bool _bind)
	{
		base.HandEvent (_bind);
		if (_bind) 
		{
            if (toggleQuickBuy != null) EventDelegate.Add(toggleQuickBuy.onChange, OnChooseQuickBuy);
            if (batchToggleQuickBuy != null) EventDelegate.Add(batchToggleQuickBuy.onChange, OnBatchChooseQuickBuy);
            if (attrLock != null)
            {
                for (int i = 0, max = attrLock.Length; i < max; i++)
                {
                    if (attrLock[i] != null) EventDelegate.Add(attrLock[i].onChange, OnChangeLock);
                }
            }
            if (batchAttrLock != null)
            {
                for (int i = 0, max = batchAttrLock.Length; i < max; i++)
                {
                    if (batchAttrLock[i] != null) EventDelegate.Add(batchAttrLock[i].onChange, OnChangeLock);
                }
            }
            GameCenter.equipmentTrainingMng.OnSelectEquipmentUpdate += ShowChooseEquip;
            GameCenter.equipmentTrainingMng.OnEquipmentWashResultUpdate += showEffect;
			GameCenter.equipmentTrainingMng.OnEquipmentWashResultUpdate += ShowWashEquip;
			GameCenter.equipmentTrainingMng.OnQuickBuyCostUpdateEvent += OnChooseQuickBuy;
			GameCenter.inventoryMng.OnBackpackUpdate += ShowEquipConsume;
			GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate += OnBaseUpdate;
		}else
		{
            if (toggleQuickBuy != null) EventDelegate.Remove(toggleQuickBuy.onChange, OnChooseQuickBuy);
            if (batchToggleQuickBuy != null) EventDelegate.Remove(batchToggleQuickBuy.onChange, OnBatchChooseQuickBuy);
            if (attrLock != null)
            {
                for (int i = 0, max = attrLock.Length; i < max; i++)
                {
                    if (attrLock[i] != null) EventDelegate.Remove(attrLock[i].onChange, OnChangeLock);
                }
            }
            if (batchAttrLock != null)
            {
                for (int i = 0, max = batchAttrLock.Length; i < max; i++)
                {
                    if (batchAttrLock[i] != null) EventDelegate.Remove(batchAttrLock[i].onChange, OnChangeLock);
                }
            }
			GameCenter.equipmentTrainingMng.OnSelectEquipmentUpdate -= ShowChooseEquip;
            GameCenter.equipmentTrainingMng.OnEquipmentWashResultUpdate -= showEffect;
			GameCenter.equipmentTrainingMng.OnEquipmentWashResultUpdate -= ShowWashEquip;
			GameCenter.equipmentTrainingMng.OnQuickBuyCostUpdateEvent -= OnChooseQuickBuy;
			GameCenter.inventoryMng.OnBackpackUpdate -= ShowEquipConsume;
			GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate -= OnBaseUpdate;
		}
	}
	void OnChangeLock()
	{
		ShowEquipConsume();//消耗显示
	}
	void OnClickLock(GameObject go)
	{
		if(go != null)
		{
			UIToggle tog = go.GetComponent<UIToggle>();
			if(tog != null && WashLockList.Count >= LockCount)
			{
				tog.value = false;
				GameCenter.messageMng.AddClientMsg(20);
			}
		}
	}
	void OnClickBatchLock(GameObject go)
	{
		if(go != null)
		{
			UIToggle tog = go.GetComponent<UIToggle>();
			if(tog != null && BatchWashLockList.Count >= BatchLockCount)
			{
				tog.value = false;
				GameCenter.messageMng.AddClientMsg(20);
			}
		}
	}
	void OnBaseUpdate(ActorBaseTag tag,ulong val,bool state)
	{
		if(tag == ActorBaseTag.UnBindCoin || tag == ActorBaseTag.BindCoin)
			ShowEquipConsume();
		if(tag == ActorBaseTag.Diamond)
		{
			ShowQuickCost();
			ShowBatchQuickCost();
		}
	}
	protected int lastEquipInstanceID = 0;
	/// <summary>
	/// 显示选择的装备
	/// </summary>
	void ShowChooseEquip()
	{
		EquipmentInfo equip = null;
		if(GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo != null)
			equip = new EquipmentInfo(GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo,EquipmentBelongTo.PREVIEW);
		if(lastEquipInstanceID == 0 || equip == null || lastEquipInstanceID != equip.InstanceID)
		{
			ResetLock();//保存属性后不重置锁
		}
		if(equip != null)
		{
			lastEquipInstanceID = equip.InstanceID;
			GameCenter.equipmentTrainingMng.C2S_ReqWashAttr(equip.InstanceID);
			if(chooseEquip != null)chooseEquip.FillInfo(equip);
			for (int i = 0,max=curAttr.Length; i < max; i++) 
			{
				string valueString = null;
				switch(i)
				{
				case 0:
					valueString = ConfigMng.Instance.GetValueStringByID(equip.EquOne);
					break;
				case 1:
					valueString = ConfigMng.Instance.GetValueStringByID(equip.EquTwo);
					break;
				case 2:
					valueString = ConfigMng.Instance.GetValueStringByID(equip.EquThree);
					break;
				case 3:
					valueString = ConfigMng.Instance.GetValueStringByID(equip.EquFour);
					break;
				}
				if(curAttr[i] != null)
				{
					curAttr[i].text = valueString;
					if(batchCurAttr != null && batchCurAttr.Length > i && batchCurAttr[i] != null)//批量洗练界面的显示
						batchCurAttr[i].text = valueString;
				}
			}
			if(progressNum != null)progressNum.text = equip.WashExp+"/10000";
			if(progressBar != null)progressBar.value = (float)equip.WashExp/10000f;
			if(batchProgressNum != null)batchProgressNum.text = equip.WashExp+"/10000";
			if(batchProgressBar != null)batchProgressBar.value = (float)equip.WashExp/10000f;
			ShowEquipConsume();
		}else
		{
			if(progressNum != null)progressNum.text = "0";
			if(progressBar != null)progressBar.value = 0;
			if(batchProgressNum != null)batchProgressNum.text = "0";
			if(batchProgressBar != null)batchProgressBar.value = 0;
			for (int i = 0,max=curAttr.Length; i < max; i++) 
			{
				if(curAttr[i] != null)
				{
					curAttr[i].text = string.Empty;
					if(batchCurAttr != null && batchCurAttr.Length > i && batchCurAttr[i] != null)//批量洗练界面的显示
						batchCurAttr[i].text = string.Empty;
				}
			}
			for (int i = 0,max=batchAttr.Length; i < max; i++) {
				if(batchAttr[i] != null)batchAttr[i].ClearData();
			}
			if(washStone != null)washStone.text = string.Empty;
			if(batchWashStone != null)batchWashStone.text = string.Empty;
			if(chooseEquip != null)chooseEquip.FillInfo(null);
		}
	}
    void showEffect(EquipmentInfo _info)
    {
        if (washProFx != null)
        {
            washProFx.ShowFx();
        }
    }

	bool haveAttrCanSave = false;
	bool haveHighAttrNotChoose = false;
	/// <summary>
	/// 显示洗练了的备选详情
	/// </summary>
	void ShowWashEquip(EquipmentInfo info)
	{
		Dictionary<int, List<st.net.NetBase.spare_list>> washAttrList = GameCenter.equipmentTrainingMng.washAttrList;
		for (int i = 0,max=batchAttr.Length; i < max; i++) 
		{
			if(batchAttr[i] != null)
			{
				if(i == 0)
					batchAttr[i].SetData(washAttrList.ContainsKey(i)?washAttrList[i]:new List<st.net.NetBase.spare_list>(),OnBatchWashChooseUpdate,out haveAttrCanSave,out haveHighAttrNotChoose);
				else
					batchAttr[i].SetData(washAttrList.ContainsKey(i)?washAttrList[i]:new List<st.net.NetBase.spare_list>(),OnBatchWashChooseUpdate);
			}
		}
		if(progressNum != null)progressNum.text = (info.WashExp)+"/10000";
		if(progressBar != null)progressBar.value = (float)info.WashExp/10000f;
		if(batchProgressNum != null)batchProgressNum.text = info.WashExp+"/10000";
		if(batchProgressBar != null)batchProgressBar.value = (float)info.WashExp/10000f;
		int washAttrCount = 0;//洗练属性的条数
		if(GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo.EquThree != 0)
			washAttrCount = 3;
		if(GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo.EquFour != 0)
			washAttrCount = 4;
		if(attrLock != null)
		{
			for (int i = 0,max=attrLock.Length; i < max; i++) {
				if(attrLock[i] != null)attrLock[i].gameObject.SetActive(washAttrCount>i);
			}
		}
		if(batchAttrLock != null)
		{
			for (int i = 0,max=batchAttrLock.Length; i < max; i++) {
				if(batchAttrLock[i] != null)batchAttrLock[i].gameObject.SetActive(washAttrCount>i);
			}
		}
		if(btnSave != null)btnSave.isEnabled = haveAttrCanSave;
		if(btnSuperWash != null)btnSuperWash.gameObject.SetActive(info.WashExp >= 10000);
		if(btnBatchSuperWash != null)btnBatchSuperWash.gameObject.SetActive(info.WashExp >= 10000);
		if(normalWashBtns != null)normalWashBtns.SetActive(info.WashExp < 10000);
		if(normalBatchWashBtns != null)normalBatchWashBtns.SetActive(info.WashExp < 10000);

		//至尊洗练只显示一次材料消耗(此处:在批量洗练界面达到至尊洗练条件,修改材料显示;在至尊洗练完成后又显示成原样)
		ShowEquipConsume();//显示材料消耗
		ShowBatchQuickCost();//显示快捷购买消耗
	}

	void OnBatchWashChooseUpdate()
	{
		bool haveChoose = false;
		if(batchAttr != null)
		{
			for (int i = 0,max=batchAttr.Length; i < max; i++) 
			{
				if(batchAttr[i].IsChoose)
				{
					haveChoose = true;
					break;
				}
			}
		}
		if(btnSaveWash != null)btnSaveWash.isEnabled = haveChoose;
	}

	bool enoughMaterial = true;
	bool enoughCoin = true;
	bool batchEnoughMaterial = true;
	bool superBatchEnoughMaterial = true;
	List<EquipmentInfo> lackEquipList = new List<EquipmentInfo>();
	List<EquipmentInfo> batchLackEquipList = new List<EquipmentInfo>();
	List<EquipmentInfo> superBatchLackEquipList = new List<EquipmentInfo>();
	/// <summary>
	/// 洗练消耗
	/// </summary>
	void ShowEquipConsume()
	{
		enoughMaterial = true;
		enoughCoin = true;
		batchEnoughMaterial = true;
		lackEquipList.Clear();
		batchLackEquipList.Clear();
		superBatchLackEquipList.Clear();
		EquipmentInfo equip = GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo;
		if(equip == null)
		{
			return;
		}
		EquipmentWashConsumeRef consumeRef = ConfigMng.Instance.GetEquipmentWashConsumeRefByQuality(equip.Quality);
		if(consumeRef != null)
		{
			StringBuilder builder = new StringBuilder();
			StringBuilder batchBuilder = new StringBuilder();
			StringBuilder superBatchBuilder = new StringBuilder();
			bool enough;
			bool batchEnough;
			bool superBatchEnough;
			EquipmentInfo lackEquip = null;
			EquipmentInfo batchLackEquip = null;
			EquipmentInfo superBatchLackEquip = null;
			for (int i = 0,max=consumeRef.consumeItem.Count; i < max; i++) 
			{
				builder.Append(GameHelper.GetStringWithBagNumber(consumeRef.consumeItem[i],out enough,out lackEquip));
				batchBuilder.Append(GameHelper.GetStringWithBagNumber(new ItemValue(consumeRef.consumeItem[i].eid,consumeRef.consumeItem[i].count*8),out batchEnough,out batchLackEquip));
				superBatchBuilder.Append(GameHelper.GetStringWithBagNumber(consumeRef.consumeItem[i],out superBatchEnough,out superBatchLackEquip));
				if(enough == false)
				{
					if(consumeRef.consumeItem[i].eid == 5)
						enoughCoin = enough;
					else
						enoughMaterial = enough;
				}
				if(batchEnough == false)
				{
					batchEnoughMaterial = batchEnough;
				}
				if(superBatchEnough == false)
				{
					superBatchEnoughMaterial = superBatchEnough;
				}
				if(i < max-1)builder.Append(" ");
				if(i < max-1)batchBuilder.Append(" ");
				if(i < max-1)superBatchBuilder.Append(" ");
				if(lackEquip != null)
					lackEquipList.Add(lackEquip);
				if(batchLackEquip != null)
					batchLackEquipList.Add(batchLackEquip);
				if(superBatchLackEquip != null)
					superBatchLackEquipList.Add(superBatchLackEquip);
			}
			List<int> lockList = WashLockList;
			if(lockList.Count > 0)
			{
				for (int i = 0,max=consumeRef.lockingConsumeItem.Count; i < max; i++) 
				{
					ItemValue itemValue = new ItemValue(consumeRef.lockingConsumeItem[i].eid,consumeRef.lockingConsumeItem[i].count*lockList.Count);
					builder.Append(GameHelper.GetStringWithBagNumber(itemValue,out enough,out lackEquip));
					if(enough == false)
					{
						if(itemValue.eid == 5)
							enoughCoin = enough;
						else
							enoughMaterial = enough;
					}
					if(i < max-1)builder.Append(" ");
					if(lackEquip != null)
						lackEquipList.Add(lackEquip);
				}
			}
			List<int> batchLockList = BatchWashLockList;
			if(batchLockList.Count > 0)
			{
				for (int i = 0,max=consumeRef.lockingConsumeItem.Count; i < max; i++) 
				{
					ItemValue itemValue = new ItemValue(consumeRef.lockingConsumeItem[i].eid,consumeRef.lockingConsumeItem[i].count*batchLockList.Count*8);
					ItemValue superItemValue = new ItemValue(consumeRef.lockingConsumeItem[i].eid,consumeRef.lockingConsumeItem[i].count*batchLockList.Count);
					batchBuilder.Append(GameHelper.GetStringWithBagNumber(itemValue,out batchEnough,out batchLackEquip));
					superBatchBuilder.Append(GameHelper.GetStringWithBagNumber(superItemValue,out superBatchEnough,out superBatchLackEquip));
					if(batchEnough == false)
					{
						batchEnoughMaterial = batchEnough;
					}
					if(superBatchEnough == false)
					{
						superBatchEnoughMaterial = superBatchEnough;
					}
					if(i < max-1)batchBuilder.Append(" ");
					if(i < max-1)superBatchBuilder.Append(" ");
					if(batchLackEquip != null)
						batchLackEquipList.Add(batchLackEquip);
					if(superBatchLackEquip != null)
						superBatchLackEquipList.Add(superBatchLackEquip);
				}
			}
			if(washStone != null)washStone.text = builder.ToString();
			if(batchWashStone != null)batchWashStone.text = (equip.WashExp < 10000)?batchBuilder.ToString():superBatchBuilder.ToString();//至尊洗练消耗一次材料(特殊显示!)
			if(OpenBatchWash)
			{
				GameCenter.equipmentTrainingMng.quickBuyList = (equip.WashExp < 10000)?batchLackEquipList:superBatchLackEquipList;//至尊洗练快捷购买一次材料(特殊显示!)
			}else
			{
				GameCenter.equipmentTrainingMng.quickBuyList = lackEquipList;
			}
		}
	}

	void WashEquip(GameObject go)
	{
		if(haveHighAttrNotChoose && WashLockList.Count == 0)
		{
			MessageST mst = new MessageST();
			mst.messID = 407;
			mst.delYes = (x)=>
			{
				WashEquip();
			};
			GameCenter.messageMng.AddClientMsg(mst);
		}else
		{
			WashEquip();
		}
	}
	void WashEquip()
	{
		GameCenter.equipmentTrainingMng.ChooseQuickBuy = toggleQuickBuy.value;
		if(!enoughCoin)
		{
			GameCenter.messageMng.AddClientMsg(155);
			return;
		}
		if(GameCenter.equipmentTrainingMng.ChooseQuickBuy && !enoughMaterial)
		{
			//快捷购买
			GameCenter.equipmentTrainingMng.C2S_WashEquip(GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo.InstanceID,WashLockList,true);
			return;
		}
		if(!GameCenter.equipmentTrainingMng.ChooseQuickBuy && !enoughMaterial)
		{
			MessageST mst = new MessageST();
			mst.messID = 211;
			mst.words = new string[1]{GameCenter.equipmentTrainingMng.QuickBuyCost.ToString()};
			mst.delYes = (x)=>
			{
				//快捷购买
				GameCenter.equipmentTrainingMng.C2S_WashEquip(GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo.InstanceID,WashLockList,true);
			};
			GameCenter.messageMng.AddClientMsg(mst);
			return;
		}
		GameCenter.equipmentTrainingMng.C2S_WashEquip(GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo.InstanceID,WashLockList,false);
	}
	/// <summary>
	/// 单次的至尊洗练
	/// </summary>
	void SuperWashEquip(GameObject go)
	{
		if(haveHighAttrNotChoose && WashLockList.Count == 0)
		{
			MessageST mst = new MessageST();
			mst.messID = 407;
			mst.delYes = (x)=>
			{
				SuperWashEquip();
			};
			GameCenter.messageMng.AddClientMsg(mst);
		}else
		{
			SuperWashEquip();
		}
	}
	void SuperWashEquip()
	{
		GameCenter.equipmentTrainingMng.ChooseQuickBuy = toggleQuickBuy.value;
		if(!enoughCoin)
		{
			GameCenter.messageMng.AddClientMsg(155);
			return;
		}
		if(GameCenter.equipmentTrainingMng.ChooseQuickBuy && !enoughMaterial)
		{
			//快捷购买
			GameCenter.equipmentTrainingMng.C2S_SuperWashEquip(GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo.InstanceID,WashLockList,true);
			return;
		}
		if(!GameCenter.equipmentTrainingMng.ChooseQuickBuy && !enoughMaterial)
		{
			MessageST mst = new MessageST();
			mst.messID = 211;
			mst.words = new string[1]{GameCenter.equipmentTrainingMng.QuickBuyCost.ToString()};
			mst.delYes = (x)=>
			{
				//快捷购买
				GameCenter.equipmentTrainingMng.C2S_SuperWashEquip(GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo.InstanceID,WashLockList,true);
			};
			GameCenter.messageMng.AddClientMsg(mst);
			return;
		}
		GameCenter.equipmentTrainingMng.C2S_SuperWashEquip(GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo.InstanceID,WashLockList,false);
	}
	/// <summary>
	/// 批量洗练界面的至尊洗练
	/// </summary>
	void BatchSuperWashEquip(GameObject go)
	{
		GameCenter.equipmentTrainingMng.ChooseQuickBuy = batchToggleQuickBuy.value;
		if(!enoughCoin)
		{
			GameCenter.messageMng.AddClientMsg(155);
			return;
		}
		if(GameCenter.equipmentTrainingMng.ChooseQuickBuy && !superBatchEnoughMaterial)
		{
			//快捷购买
			GameCenter.equipmentTrainingMng.C2S_BatchSuperWashEquip(GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo.InstanceID,BatchWashLockList,true);
			return;
		}
		if(!GameCenter.equipmentTrainingMng.ChooseQuickBuy && !superBatchEnoughMaterial)
		{
			MessageST mst = new MessageST();
			mst.messID = 211;
			mst.words = new string[1]{GameCenter.equipmentTrainingMng.QuickBuyCost.ToString()};
			mst.delYes = (x)=>
			{
				//快捷购买
				GameCenter.equipmentTrainingMng.C2S_BatchSuperWashEquip(GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo.InstanceID,BatchWashLockList,true);
			};
			GameCenter.messageMng.AddClientMsg(mst);
			return;
		}
		GameCenter.equipmentTrainingMng.C2S_BatchSuperWashEquip(GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo.InstanceID,BatchWashLockList,false);
	}

	void BatchWashEquip(GameObject go)
	{
		GameCenter.equipmentTrainingMng.ChooseQuickBuy = batchToggleQuickBuy.value;
		if(!enoughCoin)
		{
			GameCenter.messageMng.AddClientMsg(155);
			return;
		}
        if (GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo == null)
        {
            return;
        }
		if(GameCenter.equipmentTrainingMng.ChooseQuickBuy && !batchEnoughMaterial)
		{
			//快捷购买
			GameCenter.equipmentTrainingMng.C2S_BatchWashEquip(GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo.InstanceID,BatchWashLockList,true);
			return;
		}
		if(!GameCenter.equipmentTrainingMng.ChooseQuickBuy && !batchEnoughMaterial)
		{
			MessageST mst = new MessageST();
			mst.messID = 211;
			mst.words = new string[1]{GameCenter.equipmentTrainingMng.QuickBuyCost.ToString()};
			mst.delYes = (x)=>
			{
				//快捷购买
				GameCenter.equipmentTrainingMng.C2S_BatchWashEquip(GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo.InstanceID,BatchWashLockList,true);
			};
			GameCenter.messageMng.AddClientMsg(mst);
			return;
		}
		GameCenter.equipmentTrainingMng.C2S_BatchWashEquip(GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo.InstanceID,BatchWashLockList,false);
	}
	void SaveWashAttr(GameObject go)
	{
		if(GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo.haveHighQualityWashAttr)
		{
			MessageST msg = new MessageST();
			msg.messID = 213;
			msg.delYes = (x)=>
			{
				GameCenter.equipmentTrainingMng.C2S_SaveWashAttr(GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo.InstanceID,0);
			};
			GameCenter.messageMng.AddClientMsg(msg);
		}else
			GameCenter.equipmentTrainingMng.C2S_SaveWashAttr(GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo.InstanceID,0);
	}
	void SaveBatchAttr(GameObject go)
	{
		int choosePos = 1;
		bool haveChoose = false;
		if(batchAttr != null)
		{
			for (int i = 0,max=batchAttr.Length; i < max; i++) 
			{
				if(batchAttr[i].IsChoose)
				{
					choosePos = i;//不需要加1,因为数组第0个是单次洗练的属性
					haveChoose = true;
					break;
				}
			}
		}
		if(haveChoose)
		{
			if(GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo.haveHighQualityWashAttr)
			{
				MessageST msg = new MessageST();
				msg.messID = 213;
				msg.delYes = (x)=>
				{
					GameCenter.equipmentTrainingMng.C2S_SaveWashAttr(GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo.InstanceID,choosePos);
				};
				GameCenter.messageMng.AddClientMsg(msg);
			}else
			{
				GameCenter.equipmentTrainingMng.C2S_SaveWashAttr(GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo.InstanceID,choosePos);
			}
		}else
		{
			GameCenter.messageMng.AddClientMsg(214);
		}
	}
	bool OpenBatchWash = false;
	void OpenBatchWashPanel(GameObject go)
	{
		OpenBatchWash = true;
		if(batchToggleQuickBuy != null)batchToggleQuickBuy.value = false;
		if(GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo != null &&  GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo.WashExp >= 10000)
			GameCenter.equipmentTrainingMng.quickBuyList = lackEquipList;//至尊洗练只显示一次材料消耗;
		else
			GameCenter.equipmentTrainingMng.quickBuyList = batchLackEquipList;
		OnBatchChooseQuickBuy();
		for (int i = 0,max=batchAttr.Length; i < max; i++) 
		{
			if(batchAttr[i] != null)
			{
				batchAttr[i].ClearChoose();
			}
		}
	}
	void CloseBatchWashPanel(GameObject go)
	{
		OpenBatchWash = false;
		GameCenter.equipmentTrainingMng.quickBuyList = lackEquipList;
		OnChooseQuickBuy();
	}

	void OnChooseQuickBuy()
	{
		ShowQuickCost();
	}
	void ShowQuickCost()
	{
        bool redColor = (ulong)GameCenter.equipmentTrainingMng.QuickBuyCost > GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount;
		System.Text.StringBuilder builder = new System.Text.StringBuilder();
		if(quickCost != null)
            quickCost.text = builder.Append(GameCenter.equipmentTrainingMng.QuickBuyCost).Append((redColor ? "/[ff0000]" : "/[00ff00]")).Append(GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount).Append("[-]").ToString();
	}
	void OnBatchChooseQuickBuy()
	{
		ShowBatchQuickCost();
	}
	void ShowBatchQuickCost()
	{
        bool redColor = (ulong)GameCenter.equipmentTrainingMng.QuickBuyCost > GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount;
		System.Text.StringBuilder builder = new System.Text.StringBuilder();
		if(batchQuickCost != null)
            batchQuickCost.text = builder.Append(GameCenter.equipmentTrainingMng.QuickBuyCost).Append((redColor ? "/[ff0000]" : "/[00ff00]")).Append(GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount).Append("[-]").ToString();
	}

	void ResetLock()
	{
		if(attrLock != null)
		{
			for (int i = 0,max=attrLock.Length; i < max; i++) {
				if(attrLock[i] != null)attrLock[i].value = false;
			}
		}
		if(batchAttrLock != null)
		{
			for (int i = 0,max=batchAttrLock.Length; i < max; i++) {
				if(batchAttrLock[i] != null)batchAttrLock[i].value = false;
			}
		}
	}
}
