//===================================
//作者：吴江
//日期：2015/7/7
//用途：物品管理的辅助类
//======================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// 物品管理的辅助类 by吴江
/// </summary>
public static class InventoryUtility  {

    /// <summary>
    /// 根据物品行为类型获取物品行为名称
    /// </summary>
    /// <param name="_eq"></param>
    /// <param name="_type"></param>
    /// <returns></returns>
    public static string GetItemActionName(this EquipmentInfo _eq, ItemActionType _type)
    {
        switch (_type)
        {
			case ItemActionType.NormalLeft:
				if (_eq.Slot != EquipSlot.None)
				{
					if (_eq.BelongTo == EquipmentBelongTo.EQUIP)
					{
						return ConfigMng.Instance.GetUItext(231);
					}
					else
					{
                        return ConfigMng.Instance.GetUItext(145);
					}
				}
				return string.Empty;
			case ItemActionType.NormalMiddle:
				if (_eq.Slot != EquipSlot.None && _eq.Family != EquipmentFamily.GEM)
				{
					if(_eq.BelongTo == EquipmentBelongTo.BACKPACK)
					{
						if(_eq.Family != EquipmentFamily.COSMETIC)
                            return ConfigMng.Instance.GetUItext(232);
					}else if(_eq.BelongTo == EquipmentBelongTo.EQUIP)
					{
                        if (_eq.IsEquip)
                            return ConfigMng.Instance.GetUItext(233);
					}
					return string.Empty;
				}else if (_eq.CanDiscard)
				{
                    return ConfigMng.Instance.GetUItext(234);
				}
				return string.Empty;
            case ItemActionType.NormalRight:
                return ConfigMng.Instance.GetUItext(235);
			case ItemActionType.TryToUse:
				if(_eq.Family == EquipmentFamily.GEM)
                    return ConfigMng.Instance.GetUItext(236);
                if (_eq.OldSort == 502)
                    return ConfigMng.Instance.GetUItext(237);
                return ConfigMng.Instance.GetUItext(238);
			case ItemActionType.TryTakeOff:
                return ConfigMng.Instance.GetUItext(231);
			case ItemActionType.TryPreviewEquip:
				if (_eq.Slot != EquipSlot.None)
                    return ConfigMng.Instance.GetUItext(239);
				else
					return string.Empty;
            case ItemActionType.TreasureDecompose:
                return ConfigMng.Instance.GetUItext(232);
                //  结束添加
              //  return _eq.CanDiscard ? "销 毁" : string.Empty;
            case ItemActionType.TryToDestory:
                return _eq.CanDiscard ? ConfigMng.Instance.GetUItext(240) : string.Empty;
            case ItemActionType.SureToDestory:
                return ConfigMng.Instance.GetUItext(241);
            case ItemActionType.MallBuy:
                return ConfigMng.Instance.GetUItext(242);
            case ItemActionType.HonorMallBuy:
                return ConfigMng.Instance.GetUItext(242);
            case ItemActionType.SpecialBuy:
                return ConfigMng.Instance.GetUItext(242);
            case ItemActionType.StoreSell:
                return ConfigMng.Instance.GetUItext(243);
            case ItemActionType.StoreBuy:
			case ItemActionType.CityShopBuy:
                return ConfigMng.Instance.GetUItext(242);
            case ItemActionType.SelectAdd:
                return ConfigMng.Instance.GetUItext(244);
            case ItemActionType.ReplaceThis:
                return ConfigMng.Instance.GetUItext(245);
            case ItemActionType.AuctionBuy:
                return ConfigMng.Instance.GetUItext(246);
            case ItemActionType.AuctionRetrieve:
                return ConfigMng.Instance.GetUItext(247);
            case ItemActionType.AuctionHore:
                return ConfigMng.Instance.GetUItext(248);
            case ItemActionType.UseBetter:
                if (_eq.Slot != EquipSlot.None)
                    return ConfigMng.Instance.GetUItext(145);
                else
                    return ConfigMng.Instance.GetUItext(238);
            case ItemActionType.AuctionQuickSell:
                return ConfigMng.Instance.GetUItext(249);
            case ItemActionType.QuitEquip:
                return ConfigMng.Instance.GetUItext(250);
            case ItemActionType.Inlay:
                return ConfigMng.Instance.GetUItext(236);
			case ItemActionType.Synthetic:
                return ConfigMng.Instance.GetUItext(251);
			case ItemActionType.UnInlay:
                return ConfigMng.Instance.GetUItext(231);
            case ItemActionType.UpGrade:
                return ConfigMng.Instance.GetUItext(252);
			case ItemActionType.Harvest:
                return ConfigMng.Instance.GetUItext(253);
            case ItemActionType.StrengEquipment:
                return ConfigMng.Instance.GetUItext(233);
			case ItemActionType.PutInStorage:
                return ConfigMng.Instance.GetUItext(254);
		    case ItemActionType.Putaway:
                return ConfigMng.Instance.GetUItext(254);
            case ItemActionType.Redeem:
                return ConfigMng.Instance.GetUItext(255);
            case ItemActionType.Flaunt:
                return ConfigMng.Instance.GetUItext(235);
            case ItemActionType.Trade:
                return ConfigMng.Instance.GetUItext(256);
            case ItemActionType.ToForever:
                return ConfigMng.Instance.GetUItext(257);
			case ItemActionType.TakeOutStorage:
				//不是会长，则是申请取出
				GuildStorageWnd 	guildStorageWnd = GameCenter.uIMng.GetGui<GuildStorageWnd>();
				if(GameCenter.guildMng.MyGuildInfo != null && GameCenter.guildMng.MyGuildInfo.MyPosition == GuildMemPosition.MEMBER && guildStorageWnd != null)
                    return ConfigMng.Instance.GetUItext(258);
				else
                    return ConfigMng.Instance.GetUItext(259);
            case ItemActionType.BLESSING:
                return ConfigMng.Instance.GetUItext(260);
            case ItemActionType.TAKEOUT:
                return ConfigMng.Instance.GetUItext(259);
            case ItemActionType.MIX:
                return ConfigMng.Instance.GetUItext(261); 
			default:
	                return string.Empty;
        }
    }


    public static void DoItemAction(this EquipmentInfo _eq, ItemActionType _type)
    {
//		if(_type == ItemActionType.SelectAdd){
//			NGUIDebug.Log("DoItemAction    " + _eq.InstanceID);
//		}
//		Debug.Log("Check Click Times---------DoItemAction--------------");
//		System.Action action = _eq.GetItemAction(_type);
//        if (action != null) action();
		_eq.GetItemAction(_type);
    }

    public static void GetItemAction(this EquipmentInfo _eq, ItemActionType _type)
    {
        switch (_type)
        {
		case ItemActionType.NormalLeft:
			if (_eq.Slot != EquipSlot.None)
			{
				if (_eq.BelongTo == EquipmentBelongTo.EQUIP)
				{
					_eq.TryToTakeOff();
				}
				else
				{
                    if (_eq.Family == EquipmentFamily.MOUNTEQUIP)
                        _eq.TryToWieldMountEquip();
                    else
					    _eq.TryToUse();
				}
			}
			break;
		case ItemActionType.NormalMiddle:
			if (_eq.Slot != EquipSlot.None)
			{
				if(_eq.BelongTo == EquipmentBelongTo.BACKPACK)
				{
					if(_eq.Family != EquipmentFamily.COSMETIC && _eq.Family != EquipmentFamily.GEM)
						 _eq.TryToDecompose();
				}else if(_eq.BelongTo == EquipmentBelongTo.EQUIP)
				{
					if(_eq.Family != EquipmentFamily.COSMETIC && _eq.Family != EquipmentFamily.GEM)
						_eq.TryToStrengEquip();
				}
			}else if (_eq.CanDiscard)
			{
				_eq.TryToDiscard();
			}
			break;
		case ItemActionType.NormalRight:
			_eq.TryToFlaunt();
			break;
        case ItemActionType.TryToUse:
             _eq.TryToUse();
			break;
        case ItemActionType.StoreSell:
            _eq.TryToSell();
            break;
        case ItemActionType.TryTakeOff:
			_eq.TryToTakeOff();
			break;
        case ItemActionType.StrengEquipment:
            _eq.TryToStrengEquip();
			break;
        case ItemActionType.ChangeCosmeticState:
			 _eq.ChangeCosmeticState();
			break;
		case ItemActionType.MallBuy:
			 _eq.TryToMallBuy();
			break;
        case ItemActionType.HonorMallBuy:
			 _eq.TryToHonorMallBuy();
			break;
		case ItemActionType.StoreBuy:
			_eq.TryToStoreBuy();
			break;
		case ItemActionType.CityShopBuy:
			_eq.TryToCityStoreBuy();
			break;
        case ItemActionType.Redeem:
            _eq.TryToRedeem();
            break;
		case ItemActionType.PutInStorage:
			_eq.TryToPutInStorage();
			break; 
		case ItemActionType.TakeOutStorage:
			_eq.TryTakeOutStorage();
			break;
        case ItemActionType.SelectAdd:
            _eq.TryToSelect();
            break; 
		case ItemActionType.Putaway:
			_eq.TryToPutaway();
			break;
        case ItemActionType.Flaunt:
            _eq.TryToFlaunt();
            break;
		case ItemActionType.TryToDestory:
			_eq.TryToDiscard();
			break;
		case ItemActionType.Inlay:
			_eq.TryToInlay();
			break;
		case ItemActionType.Synthetic:
			_eq.TryToOpenSynthetic();
			break;
		case ItemActionType.UnInlay:
			_eq.TryToUnInlay();
			break;
        case ItemActionType.Trade:
            _eq.TryToTrade();
            break;
        case ItemActionType.ToForever:
            _eq.TryToForever();
            break;
        case ItemActionType.BLESSING:
            _eq.TryToBless();
            break;
        case ItemActionType.TAKEOUT:
            _eq.TryToTakeOut();
            break;
        case ItemActionType.MIX:
             _eq.TryToMix(); 
            break; 
		default:
            _eq.TryToDefault();
            break;
        }
    }

    public static void TryToSelect(this EquipmentInfo _eq)
    {
        GameCenter.mercenaryMng.seleteEquip = _eq;
        if (GameCenter.mercenaryMng.OnSeleteUpdate != null) { GameCenter.mercenaryMng.OnSeleteUpdate(); }
    }

    public static void TryToWieldMountEquip(this EquipmentInfo _eq)
    {
        if (GameCenter.newMountMng.HaveTheSlotMountEquip(_eq.Slot))
        {
            GameCenter.messageMng.AddClientMsg(505);
            return;
        }
        GameCenter.newMountMng.C2S_ReqWieldMountEquip(_eq.InstanceID);
    }

    public static void TryToUse(this EquipmentInfo _eq)
    { 
		if(!_eq.IsEquip &&  !_eq.CanUse)
		{
			GameCenter.messageMng.AddClientMsg(428);
			return;
		}
		if(_eq.UseReqLevel > GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel)
		{
			if(_eq.IsEquip)
				GameCenter.messageMng.AddClientMsg(247);
			else
				GameCenter.messageMng.AddClientMsg(13);
			return;
		}
		if(_eq.IsEquip)
		{
			if(!_eq.CheckClass(GameCenter.mainPlayerMng.MainPlayerInfo.Prof))
			{
				GameCenter.messageMng.AddClientMsg(246);
				return;
			}
            EquipmentInfo info = GameCenter.inventoryMng.GetEquipFromEquipDicBySlot(_eq.Slot);
            bool isOpenExtend = GameCenter.mainPlayerMng.FunctionIsOpen(FunctionType.INHERITANCE);
            if (isOpenExtend && info != null && (info.EquipmentGs < _eq.EquipmentGs) && info.CanExtend && (info.UpgradeLv > _eq.UpgradeLv || info.LuckyLv > _eq.LuckyLv || info.EquOne > 0))
            {
                MessageST mst = new MessageST();
                mst.messID = 554;
                mst.delYes = (x) =>
                    {
                        GameCenter.uIMng.SwitchToSubUI(SubGUIType.EQUIPMENTEXTEND);
                        GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo = info;
                        GameCenter.equipmentTrainingMng.CurViceEquipmentInfo = _eq;
                    };
                mst.delNo = (y) =>
                    {
                        GameCenter.inventoryMng.C2S_ReplaceItem(_eq);
                    };
                GameCenter.messageMng.AddClientMsg(mst);
            }
            else
            {
                GameCenter.inventoryMng.C2S_ReplaceItem(_eq);
            }
		}else
		{
			if(_eq.ActionType == EquipActionType.open_ui)
			{ 
                if (GameCenter.inventoryMng != null)
                {
                    GameCenter.inventoryMng.SkipWndById(_eq.OpenUiType);
                }
                else
                {
                    Debug.Log(" inventoryMng 为null, 不能使用跳转界面功能");
                } 

                //string[] uiType = _eq.OpenUiType.Split(',');
                //Debug.Log("_eq.OpenUiType:"+_eq.OpenUiType);
                //if(uiType.Length == 2)
                //{
                //    if(uiType[0].Equals("2"))
                //        GameCenter.uIMng.SwitchToSubUI((SubGUIType)System.Enum.Parse(typeof(SubGUIType),uiType[1]));
                //    else
                //        GameCenter.uIMng.SwitchToUI((GUIType)System.Enum.Parse(typeof(GUIType),uiType[1]));
                //}else
                //{
                //    GameCenter.messageMng.AddClientMsg("open_ui参数配置错误!");
                //}

			}
            else if (_eq.ActionType == EquipActionType.activate_title)//激活并使用该称号
            {
                //GameCenter.titleMng.C2S_UseTitle(_eq.EID, 2);
                GameCenter.inventoryMng.C2S_UseItem(_eq);
                GameCenter.fashionMng.OpenFinshionTitleWnd();
            }
            else
            {
                if (_eq.ActionType == EquipActionType.add_lucky)//使用祝福油跳转到祝福界面
                {
                    EquipmentInfo info = null;
                    foreach (EquipmentInfo data in GameCenter.inventoryMng.GetPlayerEquipDic().Values)
                    {
                        if (data.Slot == EquipSlot.weapon)
                        {
                            info = data;
                        }
                    }
                    if (info != null)
                    {
                        GameCenter.inventoryMng.curEquWeapon = info;
                        GameCenter.uIMng.GenGUI(GUIType.BLESSWND, true);
                    }
                    else
                        GameCenter.messageMng.AddClientMsg(438);
                    return;
                }
                if (_eq.CanUseBatch && _eq.StackCurCount > 1)//可批量开启的物品使用
                {
                    //GameCenter.inventoryMng.OpenBatchUseWnd(_eq);
                    GameCenter.inventoryMng.CurSelectInventory = _eq;
                    GameCenter.uIMng.GenGUI(GUIType.IMMEDIATEUSE, true);
                    return;
                }
                if (_eq.ActionType == EquipActionType.activate_illusion)//使用幻化物品跳到幻化界面
                {
                    if (GameCenter.mainPlayerMng.FunctionIsOpen(FunctionType.UNREAL))
                    {
                        GameCenter.newMountMng.ShowMountSkinModel(_eq);
                    }
                    else
                    {
                        GameCenter.messageMng.AddClientMsg(389);
                        return;
                    }
                    //GameCenter.uIMng.SwitchToSubUI(SubGUIType.ILLUSION);
                }
                if (_eq.Family == EquipmentFamily.POTION)
                {
                    if (!GameCenter.mainPlayerMng.MainPlayerInfo.IsCanUseDrug)
                    {
                        GameCenter.messageMng.AddClientMsg(469);
                        return;
                    }
                }
                if (_eq.ActionType == EquipActionType.rename)//改名卡
                {
                    GameCenter.inventoryMng.InstanceID = _eq.InstanceID;
                    GameCenter.uIMng.GenGUI(GUIType.RENAMECARD, true);
                    return;
                }
                GameCenter.inventoryMng.C2S_UseItem(_eq);
            }
		}
    }

    public static void TryToUseAll(this EquipmentInfo _eq)
    {
        if (!_eq.IsEquip && !_eq.CanUse)
        {
            GameCenter.messageMng.AddClientMsg(428);
            return;
        }
        if (_eq.UseReqLevel > GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel)
        {
            if (_eq.IsEquip)
                GameCenter.messageMng.AddClientMsg(247);
            else
                GameCenter.messageMng.AddClientMsg(13);
            return;
        }
        if (_eq.IsEquip)
        {
            if (!_eq.CheckClass(GameCenter.mainPlayerMng.MainPlayerInfo.Prof))
            {
                GameCenter.messageMng.AddClientMsg(246);
                return;
            }
            GameCenter.inventoryMng.C2S_ReplaceItem(_eq);
        }
        else
        {
            if (_eq.ActionType == EquipActionType.open_ui)
            {
                if (GameCenter.inventoryMng != null)
                {
                    GameCenter.inventoryMng.SkipWndById(_eq.OpenUiType);
                }
                else
                {
                    Debug.Log(" inventoryMng 为null, 不能使用跳转界面功能");
                } 
            }
            else if (_eq.ActionType == EquipActionType.activate_title)//激活并使用该称号
            { 
                GameCenter.inventoryMng.C2S_UseItem(_eq);
                GameCenter.fashionMng.OpenFinshionTitleWnd();
            }
            else
            {
                if (_eq.ActionType == EquipActionType.add_lucky)//使用祝福油跳转到祝福界面
                {
                    EquipmentInfo info = null;
                    foreach (EquipmentInfo data in GameCenter.inventoryMng.GetPlayerEquipDic().Values)
                    {
                        if (data.Slot == EquipSlot.weapon)
                        {
                            info = data;
                        }
                    }
                    if (info != null)
                    {
                        GameCenter.inventoryMng.curEquWeapon = info;
                        GameCenter.uIMng.GenGUI(GUIType.BLESSWND, true);
                    }
                    else
                        GameCenter.messageMng.AddClientMsg(438);
                    return;
                } 
                if (_eq.ActionType == EquipActionType.activate_illusion)//使用幻化物品跳到幻化界面
                {
                    if (GameCenter.mainPlayerMng.FunctionIsOpen(FunctionType.UNREAL))
                    {
                        GameCenter.newMountMng.ShowMountSkinModel(_eq);
                    }
                    else
                    {
                        GameCenter.messageMng.AddClientMsg(389);
                        return;
                    } 
                }
                if (_eq.Family == EquipmentFamily.POTION)
                {
                    if (!GameCenter.mainPlayerMng.MainPlayerInfo.IsCanUseDrug)
                    {
                        GameCenter.messageMng.AddClientMsg(469);
                        return;
                    }
                }
                if (_eq.ActionType == EquipActionType.rename)//改名卡
                {
                    GameCenter.inventoryMng.InstanceID = _eq.InstanceID;
                    GameCenter.uIMng.GenGUI(GUIType.RENAMECARD, true);
                    return;
                }
                if (_eq.CanUseBatch && _eq.StackCurCount > 1)//批量使用所有物品
                {
                    GameCenter.inventoryMng.CurSelectInventory = _eq;
                    GameCenter.inventoryMng.C2S_UseItems(_eq, _eq.StackCurCount);
                    return;
                }
                GameCenter.inventoryMng.C2S_UseItem(_eq);
            }
        }
    }

    public static void TryToSell(this EquipmentInfo _eq)
    {
       // GameCenter.inventoryMng.C2S_SellItem(_eq);
        List<int> list = new List<int>();
        list.Add(_eq.InstanceID);
        GameCenter.buyMng.C2S_AskSellItem(list);
    }
    public static void TryToStrengEquip(this EquipmentInfo _eq)
    {
        //GameCenter.uIMng.ReleaseGUI(GUIType.BACKPACK);
		//GameCenter.uIMng.SwitchToUI(GUIType.EQUIPMENTTRAINING);
		GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo = _eq;
		GameCenter.uIMng.SwitchToUI(GUIType.EQUIPMENTTRAINING);
    }
	public static void TryToDefault(this EquipmentInfo _eq){
		Debug.Log("物品"+_eq.ItemName+"的热感按钮类型不对");
	}

	public static void TryToAccess(this EquipmentInfo _eq)
	{
		Debug.Log("TryToAccess:"+_eq);
		ToolTipMng.ShowEquipmentAccessTooltip(_eq);
	}
	
    public static void TryToDecompose(this EquipmentInfo _eq ) 
    {
		Debug.Log("TryToDecompose" + _eq.ToString());
		GameCenter.uIMng.SwitchToSubUI(SubGUIType.DECOMPOSITION);
     }
    /// <summary>
    /// 确认销毁装备
    /// </summary>
    /// <param name="_eq"></param>
    public static void SureToDestory(this EquipmentInfo _eq)
    {
        if (!_eq.CanDiscard) return;
        if (_eq.BelongTo == EquipmentBelongTo.BACKPACK)
        {
           // GameCenter.inventoryMng.C2S_AskForDeleteItem(_eq);
        }
    }
    /// <summary>
    /// 尝试脱下装备 by贺丰
    /// </summary>
    /// <param name="_eq"></param>
    public static void TryToTakeOff(this EquipmentInfo _eq)
    {
        if (_eq.BelongTo == EquipmentBelongTo.EQUIP)
        {
            switch (_eq.Family)
            { 
                case EquipmentFamily.GEM:
                    GameCenter.equipmentTrainingMng.C2S_UnstallGem(GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo.GetUnstallPos(_eq.EID), GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo.InstanceID);
                    break;
                case EquipmentFamily.MOUNTEQUIP:
                    GameCenter.newMountMng.C2S_ReqUnWieldMountEquip(_eq.InstanceID);
                    break;
                default:
                    GameCenter.inventoryMng.C2S_TakeOff(_eq.InstanceID);
                    break;
            }
        }
    }

    /// <summary>
    /// 改变指定时装的显示状态
    /// </summary>
    /// <param name="_eq"></param>
    public static void ChangeCosmeticState(this EquipmentInfo _eq)
    {
        if (_eq == null || _eq.Family != EquipmentFamily.COSMETIC) return;
        //GameCenter.mainPlayerMng.C2S_ChangeCosmeticState(_eq.OldSort, !GameCenter.mainPlayerMng.MainPlayerInfo.GetCosmeticState(_eq.Slot));
        
    }

	/// <summary>
	/// 市场上架 by 黄洪兴
	/// <summary>
	public static void TryToPutaway(this EquipmentInfo _eq)
	{
		if (_eq.BelongTo == EquipmentBelongTo.BACKPACK)
		{
			GameCenter.marketMng.OpenPutawayWnd (_eq);
			//GameCenter.mallMng.OpenMallBuyTip();
		}
	}
    /// <summary>
    /// 祝福
    /// </summary>
    public static void TryToBless(this EquipmentInfo _eq)
    {
        if (_eq.BelongTo == EquipmentBelongTo.EQUIP)
        {
            GameCenter.inventoryMng.curEquWeapon = _eq;
            GameCenter.uIMng.GenGUI(GUIType.BLESSWND, true);
        }
        else
            GameCenter.messageMng.AddClientMsg(437);
    }
    /// <summary>
    /// 炫耀 by 黄洪兴
    /// <summary>
    public static void TryToFlaunt(this EquipmentInfo _eq)
    {
        if (GameCenter.chatMng.CurChatType == ChatInfo.Type.Private && GameCenter.chatMng.CurTargetName == string.Empty)
        {
            GameCenter.messageMng.AddClientMsg(365); return;
        }
        if (_eq.InstanceID > 0)
        {
            ChatInfo Info = new ChatInfo(Vector3.zero, 0, _eq.InstanceID);
            GameCenter.chatMng.C2S_SendContent(Info);
        }
        else
        {
            ChatInfo Info = new ChatInfo(_eq.EID);
            GameCenter.chatMng.C2S_SendContent(Info);
        }
    }

    /// <summary>
    /// 时装变为永久 by 黄洪兴
    /// <summary>
    public static void TryToForever(this EquipmentInfo _eq)
    {
        if (_eq.Family == EquipmentFamily.COSMETIC&&GameCenter.fashionMng.CurTargetFashion!=null)
        {
            MessageST mst = new MessageST();
            mst.messID = 378;
            mst.words = new string[1] { GameCenter.fashionMng.CurTargetFashion.UseItemNum.ToString() };
            mst.delYes = delegate
            {
                GameCenter.fashionMng.ToFover();
            };
            GameCenter.messageMng.AddClientMsg(mst);

        }

    }





    /// <summary>
    /// 交易 by黄洪兴
    /// </summary>
    public static void TryToTrade(this EquipmentInfo _eq)
    {
        if (GameCenter.tradeMng.TradeMyLockState)
        {
            //GameCenter.messageMng.AddClientMsg();
            return;//交易锁定中不能放入
        }
        GameCenter.tradeMng.CurTradeItemEQ = _eq;
        if (_eq.StackCurCount == 0 || _eq.StackCurCount == 1)
        {
            GameCenter.tradeMng.AddTradeItem(1);
        }
        else
        {
            GameCenter.uIMng.GenGUI(GUIType.BATCHNUM, true);
        }
        //Debug.Log("交易成功");
    }

    /// <summary>
    /// 取出要交易的物品
    /// </summary>
    public static void TryToTakeOut(this EquipmentInfo _eq)
    {
        if (GameCenter.tradeMng.TradeMyLockState)
        {
            //GameCenter.messageMng.AddClientMsg();
            return;//交易锁定中不能取出
        }
        GameCenter.tradeMng.CurTradeOutEQ = _eq;
        GameCenter.tradeMng.TakeOutTradeItem();
    }

    /// <summary>
    /// 使用宠物蛋融合增加宠物资质
    /// </summary>
    /// <param name="_eq"></param>
    public static void TryToMix(this EquipmentInfo _eq)
    {
        GameCenter.mercenaryMng.isOpenMixWndAndUseEgg = true;
        GameCenter.inventoryMng.CurSelectInventory = _eq;
        //GameCenter.mercenaryMng.seleteEggToMix = _eq;
        GameCenter.uIMng.GenGUI(GUIType.IMMEDIATEUSE, true);
    }

	/// <summary>
	/// 丢弃
	/// <summary>
	public static void TryToDiscard(this EquipmentInfo _eq)
	{
		MessageST mst = new MessageST();
		mst.messID = 254;
		mst.delYes = (x)=>
		{
			GameCenter.inventoryMng.C2S_DiscardItem(_eq);
		};
		GameCenter.messageMng.AddClientMsg(mst);
	}
	/// <summary>
	/// 镶嵌
	/// <summary>
	public static void TryToInlay(this EquipmentInfo _eq)
	{
		if(GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo == null)
			return;
		GameCenter.equipmentTrainingMng.C2S_InlayGem(_eq.InstanceID,GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo.InstanceID,GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo.CanInlayPos);
	}
	/// <summary>
	/// 卸下宝石
	/// </summary>
	public static void TryToUnInlay(this EquipmentInfo _eq)
	{
		if(GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo == null)
			return;
		GameCenter.equipmentTrainingMng.C2S_UnstallGem(_eq.Postion,GameCenter.equipmentTrainingMng.CurSelectEquipmentInfo.InstanceID);
	}
	/// <summary>
	/// 跳转合成
	/// </summary>
	/// <param name="_eq">Eq.</param>
	public static void TryToOpenSynthetic(this EquipmentInfo _eq)
	{
		GameCenter.uIMng.SwitchToSubUI(SubGUIType.BSynthesis);
	}

	/// <summary>
	/// 商城购买 by 黄洪兴
	/// <summary>
    public static void TryToMallBuy(this EquipmentInfo _eq)
    {
        if (_eq.BelongTo == EquipmentBelongTo.PREVIEW)
        {
			GameCenter.newMallMng.OpenMall();
			//GameCenter.mallMng.OpenMallBuyTip();
    	}
	}
    public static void TryToHonorMallBuy(this EquipmentInfo _eq)
    {
        if (_eq.BelongTo == EquipmentBelongTo.PREVIEW)
        {
            //GameCenter.mallMng.OpenHonorMallBuyTip();
        }
    }
    public static void TryToSpecialBuy(this EquipmentInfo _eq)
    {
        if (_eq.BelongTo == EquipmentBelongTo.PREVIEW)
        {
            //GameCenter.mallMng.OpenSpecialBuyTip();
        }
    }
    public static void TryToLimitMallBuy(this EquipmentInfo _eq)
    {
        if (_eq.BelongTo == EquipmentBelongTo.PREVIEW)
        {
            //GameCenter.mallMng.OpenLimitMallBuyTip();
        }
    }
	
	/// <summary>
	/// 商店购买 by 邓成
	/// <summary>
    public static void TryToStoreBuy(this EquipmentInfo _eq)
    {
        if (_eq.BelongTo == EquipmentBelongTo.SHOPWND)
        {

            GameCenter.buyMng.OpenBuyWnd(_eq,BuyType.SHOP);
    	}
        if (_eq.BelongTo == EquipmentBelongTo.GUILDSHOP)
        {
            GameCenter.buyMng.OpenBuyWnd(_eq, BuyType.GUILDSHOP, GameCenter.buyMng.restrictionNum);
        }

	}
	/// <summary>
	/// 城内商店购买 by 邓成
	/// <summary>
	public static void TryToCityStoreBuy(this EquipmentInfo _eq)
	{
		if (_eq.BelongTo == EquipmentBelongTo.SHOPWND)
		{
			GameCenter.buyMng.OpenBuyWnd(_eq,BuyType.CITYSHOP,GameCenter.buyMng.restrictionNum);
		}
	}
    /// <summary>
    /// 商店回购by 黄洪兴
    /// <summary>
    public static void TryToRedeem(this EquipmentInfo _eq)
    {
        if (_eq.BelongTo == EquipmentBelongTo.REDEEM)
        {
            //GameCenter.buyMng.AskRedeem(_eq, GUIType.SHOPWND, BuyType.REDEEM, _eq.StackCurCount);
            GameCenter.buyMng.C2S_AskBuyItem(_eq.StackCurCount, (int)BuyType.REDEEM);
        }
    }



	/// <summary>
	/// 放入仓库
	/// </summary>
	public static void TryToPutInStorage(this EquipmentInfo _eq)
	{
		if (_eq.BelongTo == EquipmentBelongTo.BACKPACK)
		{
			StorageWnd storageWnd = GameCenter.uIMng.GetGui<StorageWnd>();
			if(storageWnd != null)
			{
				GameCenter.inventoryMng.C2S_AskPutInStorage((uint)_eq.InstanceID);
			}else
			{
				GuildStorageWnd guildStorageWnd = GameCenter.uIMng.GetGui<GuildStorageWnd>();
				if(guildStorageWnd != null)
					GameCenter.guildMng.C2S_MoveStorageItem(1,_eq.InstanceID);
			}
		}else
		{
			Debug.Log("操作异常");
		}
	}
	/// <summary>
	/// 从仓库取出
	/// </summary>
	public static void TryTakeOutStorage(this EquipmentInfo _eq)
	{
		if (_eq.BelongTo == EquipmentBelongTo.STORAGE)
		{
			StorageWnd storageWnd = GameCenter.uIMng.GetGui<StorageWnd>();
			if(storageWnd != null)
			{
				GameCenter.inventoryMng.C2S_AskTakeOutStorage((uint)_eq.InstanceID);
			}else
			{
				GuildStorageWnd guildStorageWnd = GameCenter.uIMng.GetGui<GuildStorageWnd>();
				if(guildStorageWnd != null)
				{
					if(GameCenter.guildMng.MyGuildInfo != null && GameCenter.guildMng.MyGuildInfo.MyPosition != GuildMemPosition.MEMBER)
						GameCenter.guildMng.C2S_MoveStorageItem(2,_eq.InstanceID);
					else
						GameCenter.guildMng.C2S_MemCheckOutItem(_eq.InstanceID);
				}
			}
		}
        else if (_eq.BelongTo == EquipmentBelongTo.WAREHOUSE)
        { 
            List<int> list = new List<int>();
            list.Add(_eq.InstanceID);
            GameCenter.treasureHouseMng.C2S_ReqTakeOutHouse(list,false);
        }
        else
        {
            Debug.Log("操作异常");
        }
	}


    /// <summary>
    /// 检查职业是否兼容 by吴江
    /// </summary>
    /// <param name="needProfession"></param>
    /// <param name="curProfession"></param>
    /// <returns></returns>
    public static bool CheckClass(this EquipmentInfo _eq, int curProfession)
    {
        if (_eq.NeedProf <= 0 || curProfession <= 0)
        {
            return true;
        }
        if (curProfession == _eq.NeedProf)
            return true;
        else
            return false;
    }

    public static bool CheckUse(this EquipmentInfo _eq, PlayerBaseInfo _player)
    {
        if (_eq == null || _player == null) return false;
        return (_eq.NeedProf == _player.Prof||_eq.NeedProf == 0) && ((ulong)_eq.UseReqLevel <= _player.Level);
    }


}
