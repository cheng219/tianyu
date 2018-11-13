//==============================================
//作者：邓成 配合后台李南景
//日期：2016/3/23
//用途：装备培养管理类
//==============================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;
using System;

public class EquipmentTrainingMng {
	/// <summary>
	/// 锻造(强化、洗练、橙炼、升阶)成功
	/// </summary>
	public Action OnEquipmentTrainingSucessEvent;
	/// <summary>
	/// 洗练结果(第一个为单次洗练结果,后面八个为批量洗练结果)
	/// </summary>
	public Dictionary<int,List<spare_list>> washAttrList = new Dictionary<int, List<spare_list>>();
	/// <summary>
	/// 洗练结果返回事件
	/// </summary>
	public Action<EquipmentInfo> OnEquipmentWashResultUpdate;
	/// <summary>
	/// 升阶or橙炼结果返回
	/// </summary>
	public Action OnUpgradeEquipmentUpdateEvent;
    /// <summary>
    /// 继承结果返回
    /// </summary>
    public Action OnExtendEquipmentResultEvent;

	/// <summary>
	/// 选中的装备变化事件
	/// </summary>
	public Action OnSelectEquipmentUpdate;
	/// <summary>
	/// 选中的装备变化事件(继承的副装备)
	/// </summary>
	public Action OnViceEquipmentUpdate;
	/// <summary>
	/// 继承界面,选中一件装备刷新备选物品
	/// </summary>
	public Action<EquipSlot> OnShowEquipmentBySlot;

	protected EquipmentInfo curSelectEquipmentInfo = null;
	/// <summary>
	/// 当前选中装备
	/// </summary>
	public EquipmentInfo CurSelectEquipmentInfo
	{
		set
		{
			curSelectEquipmentInfo = value;
			if(OnSelectEquipmentUpdate != null)
				OnSelectEquipmentUpdate();
	}
		get
		{
			return curSelectEquipmentInfo;
		}
	}

	protected EquipmentInfo curViceEquipmentInfo = null;
	/// <summary>
	/// 继承的副装备
	/// </summary>
	public EquipmentInfo CurViceEquipmentInfo
	{
		set
		{
			curViceEquipmentInfo = value;
			if(OnViceEquipmentUpdate != null)
				OnViceEquipmentUpdate();
			if(OnShowEquipmentBySlot != null)
				OnShowEquipmentBySlot(CurSlot);//为了再次筛选副装备(排除本身)
		}
		get
		{
			return curViceEquipmentInfo;
		}
	}

	#region 快捷购买部分
	/// <summary>
	/// 快捷购买价格变化事件
	/// </summary>
	public Action OnQuickBuyCostUpdateEvent;
	/// <summary>
	/// 选择用快捷购买
	/// </summary>
	public bool ChooseQuickBuy = false;
	/// <summary>
	/// 快捷购买的物品列表
	/// </summary>
	public List<EquipmentInfo> quickBuyList
	{
		set
		{
			quickBuyCost = GameCenter.inventoryMng.QuickBuyConsume(value);
			if(OnQuickBuyCostUpdateEvent != null)
				OnQuickBuyCostUpdateEvent();
		}
	}
	public int quickBuyCost = 0;
	/// <summary>
	/// 快捷购买价格
	/// </summary>
	public int QuickBuyCost
	{
		get
		{
			
			return quickBuyCost;
		}
	}
	#endregion

	#region 红点

	public void SetRedTipState()
	{
		bool strengthRed = false;
		bool washRed = false;
		bool upgradeRed = false;
		bool orangeRefineRed = false;
		List<EquipmentInfo> equipList= new List<EquipmentInfo>(GameCenter.inventoryMng.GetPlayerEquipList().Values);
		for (int i = 0,max=equipList.Count; i < max; i++) 
		{
			if(!strengthRed && equipList[i] != null && equipList[i].RealCanStrength)
			{
				strengthRed = true;//只设一次红点提示
			}
			if(!washRed && equipList[i] != null && equipList[i].RealCanWash)
			{
				washRed = true;
			}
			if(!upgradeRed && equipList[i] != null && equipList[i].RealCanUpgrade)
			{
				upgradeRed = true;
			}
			if(!orangeRefineRed && equipList[i] != null && equipList[i].RealCanOrangeRefine)
			{
				orangeRefineRed = true;
			}
			if(upgradeRed && orangeRefineRed && washRed && strengthRed)
			{
				break;
			}
		}	
		if(upgradeRed && orangeRefineRed)//已有红点不扫描背包
		{
			List<EquipmentInfo> bagEquipList= new List<EquipmentInfo>(GameCenter.inventoryMng.GetBackpackEquipDic().Values);
			for (int i = 0,max=bagEquipList.Count; i < max; i++) {
				if(!upgradeRed && bagEquipList[i] != null && bagEquipList[i].RealCanUpgrade)
				{
					upgradeRed = true;
				}
				if(!orangeRefineRed && bagEquipList[i] != null && bagEquipList[i].RealCanOrangeRefine)
				{
					orangeRefineRed = true;
				}
				if(upgradeRed && orangeRefineRed)
				{
					break;
				}
			}
		}
		if(GameCenter.mainPlayerMng != null)
		{
			GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.STRENGTHENING,strengthRed);
			GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.WASHSPRACTICE,washRed);
			GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.DEGREE,upgradeRed);
			GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.ORANGEREFINING,orangeRefineRed);
		}
	}
	#endregion

	public EquipSlot CurSlot = EquipSlot.None;
	/// <summary>
	/// 显示指定槽位的装备
	/// </summary>
	public void ShowEquipmentBySlot(EquipSlot slot)
	{
		if(CurSlot == slot)return;//不重复筛选,容易造成死循环
		CurSlot = slot;
		if(OnShowEquipmentBySlot != null)
			OnShowEquipmentBySlot(slot);
	}

	public static EquipmentTrainingMng CreateNew()
	{
		if(GameCenter.equipmentTrainingMng == null)
		{
			GameCenter.equipmentTrainingMng = new EquipmentTrainingMng();
			GameCenter.equipmentTrainingMng.Init();
			return GameCenter.equipmentTrainingMng;
		}else
		{
			GameCenter.equipmentTrainingMng.UnRegist();
			GameCenter.equipmentTrainingMng.Init();
			return GameCenter.equipmentTrainingMng;
		}
	}
	void Init()
	{
		MsgHander.Regist(0xD361,S2C_StrengthResult);
		MsgHander.Regist(0xD364,S2C_InlayResult);
		MsgHander.Regist(0xD365,S2C_WashAttrResult);
		MsgHander.Regist(0xD368,S2C_SaveWashResult);
		MsgHander.Regist(0xD369,S2C_ExtendEquipResult);
		MsgHander.Regist(0xD370,S2C_UpgradeEquipResult);
		SetRedTipState();
	}
	void UnRegist()
	{
		MsgHander.UnRegist(0xD361,S2C_StrengthResult);
		MsgHander.UnRegist(0xD364,S2C_InlayResult);
		MsgHander.UnRegist(0xD365,S2C_WashAttrResult);
		MsgHander.UnRegist(0xD368,S2C_SaveWashResult);
		MsgHander.UnRegist(0xD369,S2C_ExtendEquipResult);
		MsgHander.UnRegist(0xD370,S2C_UpgradeEquipResult);
	}

	#region S2C
	/// <summary>
	/// 强化结果返回
	/// </summary>
	protected void S2C_StrengthResult(Pt pt)
	{
		pt_strengthen_info_d361 pt_strengthen = pt as pt_strengthen_info_d361;
		if(pt_strengthen != null)
		{
			//Debug.Log("pt_strengthen.lev:"+pt_strengthen.lev+",pt_strengthen.exp"+pt_strengthen.exp);
			EquipmentInfo equipInfo = GameCenter.inventoryMng.GetEquipFromEquipDicByID((int)pt_strengthen.id);
			if(equipInfo == null)
			{
				equipInfo = GameCenter.inventoryMng.GetEquipInfoByID((int)pt_strengthen.id);
			}
			if(equipInfo != null)
			{
				CurSelectEquipmentInfo = equipInfo;
			}
			if(OnEquipmentTrainingSucessEvent != null && pt_strengthen.success == 1)
				OnEquipmentTrainingSucessEvent();
		}
	}
	/// <summary>
	/// 1是镶嵌 2是取下;1是成功 0是失败
	/// </summary>
	/// <param name="pt">Point.</param>
	protected void S2C_InlayResult(Pt pt)
	{
		pt_gem_action_result_d364 pt_inlay = pt as pt_gem_action_result_d364;
		if(pt_inlay != null)
		{
			//Debug.Log("pt_inlay.action:"+pt_inlay.action+",pt_inlay.result"+pt_inlay.result+",id:"+pt_inlay.id);
			EquipmentInfo equipInfo = GameCenter.inventoryMng.GetEquipFromEquipDicByID((int)pt_inlay.id);
			if(equipInfo == null)
			{
				equipInfo = GameCenter.inventoryMng.GetEquipInfoByID((int)pt_inlay.id);
			}
			if(equipInfo != null)
			{
				CurSelectEquipmentInfo = equipInfo;
			}
		}
	}
	/// <summary>
	/// 继承结果返回
	/// </summary>
	protected void S2C_ExtendEquipResult(Pt pt)
	{
		pt_inhert_d369 pt_inhert = pt as pt_inhert_d369;
		if (pt_inhert != null) 
		{
			//Debug.Log("S2C_ExtendEquipResult pt_inhert.action:"+pt_inhert.target_id+",id:"+pt_inhert.id);
            CurSlot = EquipSlot.None;
            if (OnExtendEquipmentResultEvent != null)
                OnExtendEquipmentResultEvent();
		}
	}
	/// <summary>
	/// 橙炼、升阶结果返回
	/// </summary>
	protected void S2C_UpgradeEquipResult(Pt pt)
	{
		pt_req_change_equip_d370 pt_change = pt as pt_req_change_equip_d370;
		if (pt_change != null) 
		{
			//Debug.Log("S2C_UpgradeEquipResult pt_change.action:"+pt_change.action+",id:"+pt_change.id);
			if(pt_change.action == 1)//升阶成功继续选中该装备
			{
				EquipmentInfo equipInfo = GameCenter.inventoryMng.GetEquipFromEquipDicByID((int)pt_change.id);
				if(equipInfo == null)
				{
					equipInfo = GameCenter.inventoryMng.GetEquipInfoByID((int)pt_change.id);
				}
				if(equipInfo != null)
				{
					CurSelectEquipmentInfo = equipInfo;
				}
			}else//橙炼成功排除橙色装备
			{
				if(OnUpgradeEquipmentUpdateEvent != null)
					OnUpgradeEquipmentUpdateEvent();
			}
			if(OnEquipmentTrainingSucessEvent != null)
				OnEquipmentTrainingSucessEvent();
		}
	}
	/// <summary>
	/// 洗练属性结果返回
	/// </summary>
	protected void S2C_WashAttrResult(Pt pt)
	{
		//Debug.Log("S2C_WashAttrResult");
		pt_spare_propertys_d365 pt_propertys = pt as pt_spare_propertys_d365;
		if (pt_propertys != null) 
		{
			washAttrList.Clear();
			washAttrList[0] = pt_propertys.single_property;
			washAttrList[1] = pt_propertys.one_property;
			washAttrList[2] = pt_propertys.two_property;
			washAttrList[3] = pt_propertys.three_property;
			washAttrList[4] = pt_propertys.four_property;
			washAttrList[5] = pt_propertys.five_property;
			washAttrList[6] = pt_propertys.six_property;
			washAttrList[7] = pt_propertys.seven_property;
			washAttrList[8] = pt_propertys.eight_property;
			EquipmentInfo equipInfo = GameCenter.inventoryMng.GetEquipFromEquipDicByID((int)pt_propertys.recast_item_id);
			if(equipInfo == null)
			{
				equipInfo = GameCenter.inventoryMng.GetEquipInfoByID((int)pt_propertys.recast_item_id);
			}
			if(equipInfo != null)
			{
				if(OnEquipmentWashResultUpdate != null)
					OnEquipmentWashResultUpdate(equipInfo);
			}else
			{
				Debug.LogError("后台数据错误:洗练目标装备ID:"+pt_propertys.recast_item_id+"不存在!");
			}
		}
	}
	/// <summary>
	/// 替换洗练结果返回
	/// </summary>
	/// <param name="pt">Point.</param>
	protected void S2C_SaveWashResult(Pt pt)
	{
		pt_req_store_pro_to_equip_d368 pt_save = pt as pt_req_store_pro_to_equip_d368;
		if(pt_save != null)
		{
			//Debug.Log("pt_save.id:"+pt_save.equip_id);
			EquipmentInfo equipInfo = GameCenter.inventoryMng.GetEquipFromEquipDicByID((int)pt_save.equip_id);
			if(equipInfo == null)
			{
				equipInfo = GameCenter.inventoryMng.GetEquipInfoByID((int)pt_save.equip_id);
			}
			if(equipInfo != null)
			{
				CurSelectEquipmentInfo = equipInfo;
			}
			if(OnEquipmentTrainingSucessEvent != null)
				OnEquipmentTrainingSucessEvent();
		}
	}
	#endregion

	#region C2S
	/// <summary>
	/// 一键强化
	/// </summary>
	/// <param name="_id"></param>
	public void C2S_OneKeyStrengthen(int _id)
	{
		pt_action_int_d003 msg = new pt_action_int_d003();
		msg.action = 1023;
		msg.data = _id;
		NetMsgMng.SendMsg(msg);
	}
	/// <summary>
	/// 强化装备,type=0,一次强化,type=1,一键强化,tyoe=2完美强化
	/// </summary>
	/// <param name="_id"></param>
	public void C2S_Strengthen(int _id,int type,bool isQuickBuy)
	{
		//Debug.Log("C2S_Strengthen,ID:"+_id+",type:"+type+",isQuickBuy:"+isQuickBuy);
		pt_req_strengthen_equip_d360 msg = new pt_req_strengthen_equip_d360();
		msg.id = (uint)_id;
		msg.type = (uint)type;
		msg.quik_buy = (isQuickBuy?1:0);
		NetMsgMng.SendMsg(msg);
	}
	/// <summary>
	/// 镶嵌宝石
	/// </summary>
	public void C2S_InlayGem(int _gemId,int _equipId,int _pos)
	{
		//Debug.Log("C2S_InlayGem:gemId:"+_gemId+",equipId:"+_equipId+",_pos:"+_pos);
		if(_pos == 0)
		{
			GameCenter.messageMng.AddClientMsg(442);
			return;
		}
		pt_req_inlay_gem_d362 msg = new pt_req_inlay_gem_d362();
		msg.equip_id = (uint)_equipId;
		msg.gem_id = (uint)_gemId;
		msg.pos = (uint)_pos;
		NetMsgMng.SendMsg(msg);
	}
	/// <summary>
	/// 取下宝石
	/// </summary>
	public void C2S_UnstallGem(int _pos,int _equipId)
	{
		//Debug.Log("C2S_UnstallGem _pos:"+_pos+",equipId:"+_equipId);
		pt_req_unstall_gem_d363 msg = new pt_req_unstall_gem_d363();
		msg.equip_id = (uint)_equipId;
		msg.pos = (uint)_pos;
		NetMsgMng.SendMsg(msg);
	}
	/// <summary>
	/// 洗练装备,action=1表示单次洗练,action=2表示批量洗练
	/// </summary>
	public void C2S_WashEquip(int _instanceId,List<int> lockList,bool isQuickBuy)
	{
		//Debug.Log("C2S_WashEquip _instanceId:"+_instanceId+",lockList:"+lockList.Count);
		pt_req_recast_d366 msg = new pt_req_recast_d366();
		msg.equip_id = _instanceId;
		msg.action = 1;
		msg.locking_list = lockList;
		msg.quik_buy = isQuickBuy?1:0;
		NetMsgMng.SendMsg(msg);
	}
	/// <summary>
	/// 批量洗练装备,action=1表示单次洗练,action=2表示批量洗练
	/// </summary>
	public void C2S_BatchWashEquip(int _instanceId,List<int> lockList,bool isQuickBuy)
	{
		//Debug.Log("C2S_BatchWashEquip _instanceId:"+_instanceId+",lockList:"+lockList.Count);
		pt_req_recast_d366 msg = new pt_req_recast_d366();
		msg.equip_id = _instanceId;
		msg.action = 2;
		msg.locking_list = lockList;
		msg.quik_buy = isQuickBuy?1:0;
		NetMsgMng.SendMsg(msg);
	}
	/// <summary>
	/// 至尊洗练装备,
	/// </summary>
	public void C2S_SuperWashEquip(int _instanceId,List<int> lockList,bool isQuickBuy)
	{		
		//Debug.Log("C2S_SuperWashEquip _instanceId:"+_instanceId+",lockList:"+lockList.Count);
		pt_req_recast_d366 msg = new pt_req_recast_d366();
		msg.equip_id = _instanceId;
		msg.action = 3;
		msg.locking_list = lockList;
		msg.quik_buy = isQuickBuy?1:0;
		NetMsgMng.SendMsg(msg);
	}
	/// <summary>
	/// 批量界面至尊洗练装备,
	/// </summary>
	public void C2S_BatchSuperWashEquip(int _instanceId,List<int> lockList,bool isQuickBuy)
	{		
		//Debug.Log("C2S_SuperWashEquip _instanceId:"+_instanceId+",lockList:"+lockList.Count);
		pt_req_recast_d366 msg = new pt_req_recast_d366();
		msg.equip_id = _instanceId;
		msg.action = 4;
		msg.locking_list = lockList;
		msg.quik_buy = isQuickBuy?1:0;
		NetMsgMng.SendMsg(msg);
	}
	/// <summary>
	/// 请求保存了的洗练属性
	/// </summary>
	public void C2S_ReqWashAttr(int _instanceId)
	{
		//Debug.Log("C2S_ReqWashAttr _instanceId:"+_instanceId);
		pt_req_spare_property_d367 msg = new pt_req_spare_property_d367();
		msg.equip_id = _instanceId;
		NetMsgMng.SendMsg(msg);
	}
	/// <summary>
	/// 请求保存选中属性,0表示保存单次洗练属性,1~8为保存选中的批量洗练属性
	/// </summary>
	public void C2S_SaveWashAttr(int _instanceId,int _pos)
	{
		//Debug.Log("C2S_SaveWashAttr _pos:"+_pos+",_instanceId:"+_instanceId);
		pt_req_store_pro_to_equip_d368 msg = new pt_req_store_pro_to_equip_d368();
		msg.equip_id = _instanceId;
		msg.pos = _pos;
		NetMsgMng.SendMsg(msg);
	}
	/// <summary>
	/// 继承装备属性
	/// </summary>
	/// <param name="curEquipID">Current equip I.</param>
	/// <param name="viceEquipID">副装备.</param>
	/// <param name="extendList">Extend list.</param>
	public void C2S_ExtendEquipAttr(int curEquipID,int viceEquipID,List<int> extendList,bool isQuickBuy)
	{
		//Debug.Log("C2S_ExtendEquipAttr curEquipID:"+curEquipID+",viceEquipID:"+viceEquipID+",extendList:"+extendList.Count+",isQuickBuy:"+isQuickBuy);
		pt_inhert_d369 msg = new pt_inhert_d369();
		msg.target_id = viceEquipID;
		msg.id = curEquipID;
		msg.inhert_list = extendList;
		msg.quik_buy = isQuickBuy?1:0;
		NetMsgMng.SendMsg(msg);
	}
	/// <summary>
	/// 橙炼，升阶,actionType=1升阶,actionType=2橙练
	/// </summary>
	/// <param name="_id"></param>
	public void C2S_UpgradeEquip(int _instanceID,int actionType,bool isQuickBuy)
	{
		//Debug.Log("C2S_UpgradeEquip _instanceID:"+_instanceID+",actionType:"+actionType);
		pt_req_change_equip_d370 msg = new pt_req_change_equip_d370();
		msg.action = actionType;
		msg.id = _instanceID;
		msg.quik_buy = isQuickBuy?1:0;
		NetMsgMng.SendMsg(msg);
	}
	#endregion


}
