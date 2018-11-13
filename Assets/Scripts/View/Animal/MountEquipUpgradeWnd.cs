//==================================
//作者：邓成
//日期：2016/12/19
//用途：坐骑装备提升界面
//=================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class MountEquipUpgradeWnd : SubWnd {
    public DecomposeItemUI equipGo;
    /// <summary>
    /// 父节点
    /// </summary>
    public UIGrid itemParent;
    public UIScrollView scrollView;

    protected Dictionary<int, DecomposeItemUI> allGoItems = new Dictionary<int, DecomposeItemUI>();

    public GameObject upgradeGo;//升级
    public ItemUI curEquipmentUI;
    public UILabel curLev;
    public UILabel nextLev;
    public UILabel curAttrNum;
    public UILabel nextAttrNum;
    public UIProgressBar progressBar;
    public UIProgressBar progressAll;
    public UILabel strengthExp;
    public UILabel critExp;//爆发进化度
    public GameObject timeGo;
    public UITimer timerGo;
    public UIButton btnStrength;
    public UIButton btnOneKeyUp;
    public UILabel strengthConsume;
    public UIToggle toggleQuickStrength;
    public UILabel quickStrengthCost;
    public UIFxAutoActive progressFx;
    public UIFxAutoActive strengthFx;

    public GameObject upQualityGo;//升品
    public UIButton btnUpgrade;
    public ItemUI curUpgradeItem;
    public UILabel curMaxLev;
    public UILabel nextMaxLev;
    public UILabel curMaxAttrNum;
    public UILabel nextMaxAttrNum;
    public GameObject consumeGo;
    public UILabel upgradeConsume;
    public UIToggle toggleQuickUpgrade;
    public UILabel quickUpgradeCost;
    public UIFxAutoActive upgradeFx;

    void Awake()
    {
        if (btnStrength != null) UIEventListener.Get(btnStrength.gameObject).onClick = StrengthMountEquip;
        if (btnOneKeyUp != null) UIEventListener.Get(btnOneKeyUp.gameObject).onClick = OneKeyStrengthMountEquip;
        if (btnUpgrade != null) UIEventListener.Get(btnUpgrade.gameObject).onClick = UpgradeMountEquip;
    }
    protected override void OnOpen()
    {
        base.OnOpen();
        ShowEquipItems();
    }
    protected override void OnClose()
    {
        base.OnClose();
        foreach (var item in allGoItems.Values)
        {
            if(item != null)Destroy(item.gameObject);
        }
        allGoItems.Clear();
    }

    protected override void HandEvent(bool _bind)
    {
        base.HandEvent(_bind);
        if (_bind)
        {
            GameCenter.newMountMng.OnMountEquipUpdate += ShowEquipItems;
            GameCenter.newMountMng.OnSelectEquipmentUpdate += ShowSelectMountEquip;
            GameCenter.newMountMng.OnStrengthEquipmentEvent += ShowStrengthFx;
            GameCenter.newMountMng.OnUpgradeEquipmentUpdateEvent += ShowUpgradeFx;
        }
        else
        {
            GameCenter.newMountMng.OnMountEquipUpdate -= ShowEquipItems;
            GameCenter.newMountMng.OnSelectEquipmentUpdate -= ShowSelectMountEquip;
            GameCenter.newMountMng.OnStrengthEquipmentEvent -= ShowStrengthFx;
            GameCenter.newMountMng.OnUpgradeEquipmentUpdateEvent -= ShowUpgradeFx;
        }
    }

    void ShowStrengthFx(bool _strength)
    {
        if (_strength && strengthFx != null) strengthFx.ShowFx();
        if (progressFx != null) progressFx.ShowFx();
    }

    void ShowUpgradeFx()
    {
        if (upgradeFx != null) upgradeFx.ShowFx();
    }

    void ShowSelectMountEquip()
    {
        EquipmentInfo equip = GameCenter.newMountMng.CurSelectEquipmentInfo;
        if (curEquipmentUI != null)
        {
            curEquipmentUI.FillInfo(equip);
            curEquipmentUI.SetActionBtn(ItemActionType.None, ItemActionType.None, ItemActionType.None);
        }
        if (curUpgradeItem != null)
        {
            curUpgradeItem.FillInfo(equip);
            curUpgradeItem.SetActionBtn(ItemActionType.None, ItemActionType.None, ItemActionType.None);
        }
        if (equip != null)
        {
            //Debug.Log("UpgradeLv:" + equip.UpgradeLv + ",max:" + mountEquQuality.max_lev + ",quality:" + equip.Quality);
            //MountEquQuailtRef nextMountEquQuality = ConfigMng.Instance.GetMountEquipQualityRef(equip.Quality + 1, equip.Slot);
            int maxLev = ConfigMng.Instance.GetMountEquQualityMaxUpgradeLv(equip.Quality);
            bool canUpgrade = (equip.UpgradeLv == maxLev);//是否达到升品要求,满级了也进升品
            if (canUpgrade)//升品
            {
                int nextQualityMaxLv = ConfigMng.Instance.GetMountEquQualityMaxUpgradeLv(equip.Quality+1);
                if (upgradeGo != null) upgradeGo.SetActive(false);
                if (upQualityGo != null) upQualityGo.SetActive(true);
                if (curMaxLev != null) curMaxLev.text = maxLev.ToString();
                if (nextMaxLev != null) nextMaxLev.text = (nextQualityMaxLv == 0 ? ConfigMng.Instance.GetUItext(120) : nextQualityMaxLv.ToString());
                if (curMaxAttrNum != null) curMaxAttrNum.text = GameHelper.GetAttributeNameAndValue(equip.StrengthValue);
                if (nextMaxAttrNum != null) nextMaxAttrNum.text = nextQualityMaxLv == 0 ? ConfigMng.Instance.GetUItext(120):GameHelper.GetAttributeNameAndValue(equip.NextQualityValue);
                if (consumeGo != null) consumeGo.SetActive(nextQualityMaxLv != 0);
                ShowUpgradeConsume(equip.Quality,equip.Slot);
                if(btnUpgrade != null)btnUpgrade.isEnabled = (nextQualityMaxLv != 0);
            }
            else//升级
            {
                if (upgradeGo != null) upgradeGo.SetActive(true);
                if (upQualityGo != null) upQualityGo.SetActive(false);
                if (curLev != null) curLev.text = equip.UpgradeLv.ToString();
                if (nextLev != null) nextLev.text = (equip.UpgradeLv+1).ToString();
                if (curAttrNum != null) curAttrNum.text = GameHelper.GetAttributeNameAndValue(equip.StrengthValue);
                if (nextAttrNum != null) nextAttrNum.text = GameHelper.GetAttributeNameAndValue(equip.NextStrengthValue);
                int time = equip.StrengthExpTime - (int)Time.time;
                if (strengthExp != null) strengthExp.text = time > 0 ? ((equip.StrengthExp + equip.WashExp).ToString() + "%") : equip.StrengthExp .ToString()+ "%";
                if (critExp != null) critExp.text = equip.WashExp.ToString() + "%";
                if (progressBar != null) progressBar.value = ((float)equip.StrengthExp) / 100f;
                if (progressAll != null) progressAll.value = time > 0?((float)equip.StrengthExp + (float)equip.WashExp) / 100f : 0f;
                //Debug.Log("equip.StrengthExpTime:" + equip.StrengthExpTime);
                
                if (timeGo != null) timeGo.SetActive(time>0);
                if (timerGo != null && time > 0) timerGo.StartIntervalTimer(time);
                ShowStrengthConsume(equip.UpgradeLv);
            }
        }
        else
        {
            if (upgradeGo != null) upgradeGo.SetActive(true);
            if (upQualityGo != null) upQualityGo.SetActive(false);
            if (curLev != null) curLev.text = "0";
            if (nextLev != null) nextLev.text = "0";
            if (curAttrNum != null) curAttrNum.text = string.Empty;
            if (nextAttrNum != null) nextAttrNum.text = string.Empty;
        }
    }

    protected bool enoughStrength = false;
    protected bool enoughCoin = false;
    protected EquipmentInfo lackStrengthItem = null;
    void ShowStrengthConsume(int _lev)
    {
        MountStrenConsumeRef mountStrenConsume = ConfigMng.Instance.GetMountStrenConsumeRef(_lev);
        if (mountStrenConsume != null)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0, max = mountStrenConsume.item.Count; i < max; i++)
            {
                ItemValue item = mountStrenConsume.item[i];
                if (item.eid == 5 || item.eid == 6)
                {
                    builder.Append(GameHelper.GetStringWithBagNumber(item, out enoughCoin));
                }
                else
                {
                    builder.Append(GameHelper.GetStringWithBagNumber(item, out enoughStrength, out lackStrengthItem));
                }
                if (i < max - 1)
                    builder.Append("\n");
            }
            if (strengthConsume != null) strengthConsume.text = builder.ToString();
        }
        if (quickStrengthCost != null)
        {
            List<EquipmentInfo> items = new List<EquipmentInfo>();
            if (lackStrengthItem != null) items.Add(lackStrengthItem);
            int cost = GameCenter.inventoryMng.QuickBuyConsume(items);
            quickStrengthCost.text = GameCenter.inventoryMng.QuickBuyConsume(items).ToString()+"/"+1;
            bool redColor = (ulong)cost > GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount;
            System.Text.StringBuilder builder = new StringBuilder();
            quickStrengthCost.text = builder.Append(cost).Append((redColor ? "/[ff0000]" : "/[00ff00]")).Append(GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount).Append("[-]").ToString();
        }
    }

    protected bool enoughUpgrade = false;
    protected EquipmentInfo lackUpgradeItem = null;
    void ShowUpgradeConsume(int _quality,EquipSlot _slot)
    {
        MountEquQuailtRef upgradeConsumeRef = ConfigMng.Instance.GetMountEquipQualityRef(_quality, _slot);
        if (upgradeConsumeRef != null && upgradeConsume != null)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0, max = upgradeConsumeRef.consume.Count; i < max; i++)
            {
                ItemValue item = upgradeConsumeRef.consume[i];
                if (item.eid == 5 || item.eid == 6)
                {
                    builder.Append(GameHelper.GetStringWithBagNumber(item));
                }
                else
                {
                    builder.Append(GameHelper.GetStringWithBagNumber(item, out enoughUpgrade, out lackUpgradeItem));
                }
                if (i < max - 1)
                    builder.Append("\n");
            }
            if (upgradeConsume != null) upgradeConsume.text = builder.ToString();
        }
    }
    /// <summary>
    /// 显示坐骑身上的装备
    /// </summary>
    void ShowEquipItems()
    {
        HideAllGoItems();
        int index = 0;
        DecomposeItemUI checkOne = null;
        EquipmentInfo selectOne = GameCenter.newMountMng.CurSelectEquipmentInfo;
        List<EquipmentInfo> itemList = new List<EquipmentInfo>(GameCenter.newMountMng.MountEquipDic.Values);
        //Debug.Log("itemList:" + itemList.Count);
        for (int i = 0, count = itemList.Count; i < count; i++)
        {
            EquipmentInfo info = itemList[i];
            if (info == null) continue;
            DecomposeItemUI itemUI = null;
            if (!allGoItems.ContainsKey(index))
            {
                if (equipGo != null) itemUI = equipGo.CreateNew(itemParent.transform);
                allGoItems[index] = itemUI;
            }
            itemUI = allGoItems[index];
            itemUI.gameObject.SetActive(true);
            itemUI.SetData(info, ChooseItem,SubGUIType.MOUNTEQUIP);
            if (selectOne != null)
            {
                if (selectOne.InstanceID == info.InstanceID) checkOne = itemUI;
            }
            else
            {
                if (index == 0) checkOne = itemUI;
            }
            index++;
        }
        if (scrollView != null) scrollView.SetDragAmount(0, 0, false);
        if (itemParent != null) itemParent.repositionNow = true;
        if (checkOne != null)
            checkOne.SetChecked();
        if (itemList.Count == 0)
            GameCenter.newMountMng.CurSelectEquipmentInfo = null;
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
    /// 选中右边的物品
    /// </summary>
    void ChooseItem(GameObject go)
    {
        EquipmentInfo info = (EquipmentInfo)UIEventListener.Get(go).parameter;
        GameCenter.newMountMng.CurSelectEquipmentInfo = info;
    }

    void StrengthMountEquip(GameObject go)
    {
        EquipmentInfo equip = GameCenter.newMountMng.CurSelectEquipmentInfo;
        if (equip != null)
        {
            if (enoughStrength)
            {
                GameCenter.newMountMng.C2S_ReqStrengthMountEquip(equip.InstanceID, 1, false);
                return;
            }
            List<EquipmentInfo> items = new List<EquipmentInfo>();
            if (lackStrengthItem != null) items.Add(lackStrengthItem);
            int cost = GameCenter.inventoryMng.QuickBuyConsume(items);
            if (toggleQuickStrength != null && toggleQuickStrength.value && !enoughStrength)
            {
                if ((ulong)cost > GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount)
                {
                    MessageST mst1 = new MessageST();
                    mst1.messID = 137;
                    mst1.delYes = (y) =>
                    {
                        GameCenter.uIMng.SwitchToUI(GUIType.RECHARGE);
                    };
                    GameCenter.messageMng.AddClientMsg(mst1);
                    return;
                }
                GameCenter.newMountMng.C2S_ReqStrengthMountEquip(equip.InstanceID, 1, true);
                return;
            }
            if (!enoughStrength && toggleQuickStrength != null && !toggleQuickStrength.value)
            {
                MessageST mst = new MessageST();
                mst.messID = 211;
                mst.words = new string[] { cost.ToString() };
                mst.delYes = (x) =>
                    {
                        if ((ulong)cost > GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount)
                        {
                            MessageST mst1 = new MessageST();
                            mst1.messID = 137;
                            mst1.delYes = delegate
                            {
                                GameCenter.uIMng.SwitchToUI(GUIType.RECHARGE);
                            };
                            GameCenter.messageMng.AddClientMsg(mst1);
                            return;
                        }
                        GameCenter.newMountMng.C2S_ReqStrengthMountEquip(equip.InstanceID, 1, true);
                    };
                GameCenter.messageMng.AddClientMsg(mst);
            }
        }
    }

    void OneKeyStrengthMountEquip(GameObject go)
    {
        EquipmentInfo equip = GameCenter.newMountMng.CurSelectEquipmentInfo;
        if (equip != null)
        {
            if (enoughStrength)
            {
                GameCenter.newMountMng.C2S_ReqStrengthMountEquip(equip.InstanceID, 2, false);
                return;
            }
            List<EquipmentInfo> items = new List<EquipmentInfo>();
            if (lackStrengthItem != null) items.Add(lackStrengthItem);
            int cost = GameCenter.inventoryMng.QuickBuyConsume(items);
            if (toggleQuickStrength != null && toggleQuickStrength.value && !enoughStrength)
            {
                if ((ulong)cost > GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount)
                {
                    MessageST mst1 = new MessageST();
                    mst1.messID = 137;
                    mst1.delYes = delegate
                    {
                        GameCenter.uIMng.SwitchToUI(GUIType.RECHARGE);
                    };
                    GameCenter.messageMng.AddClientMsg(mst1);
                    return;
                }
                GameCenter.newMountMng.C2S_ReqStrengthMountEquip(equip.InstanceID, 2, true);
                return;
            }            
            if (lackStrengthItem != null)
            {
                MessageST mst = new MessageST();
                mst.messID = 12;
                mst.words = new string[] { lackStrengthItem.ItemStrColor + lackStrengthItem.ItemName + "[-]" };
                GameCenter.messageMng.AddClientMsg(mst);
            }
        }
    }

    void UpgradeMountEquip(GameObject go)
    {
        EquipmentInfo equip = GameCenter.newMountMng.CurSelectEquipmentInfo;
        if (equip != null)
        {
            if (enoughUpgrade)
            {
                GameCenter.newMountMng.C2S_ReqUpgradeMountEquip(equip.InstanceID);
                return;
            }
            else
            {
                if (lackUpgradeItem != null)
                {
                    MessageST mst = new MessageST();
                    mst.messID = 12;
                    mst.words = new string[] { lackUpgradeItem.ItemStrColor + lackUpgradeItem.ItemName+"[-]" };
                    GameCenter.messageMng.AddClientMsg(mst);
                }
            }
        }
    }
}
