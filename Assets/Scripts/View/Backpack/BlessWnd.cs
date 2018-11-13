//======================================================
//作者:鲁家旗
//日期:2016/10/18
//用途:祝福界面
//======================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class BlessWnd : GUIBase
{
    #region 数据
    public ItemUI itemEq;
    public ItemUI itemCommonBless;
    public ItemUI itemHighBless;
    public UILabel luckyChance;
    public UILabel luckyNum;
    public UILabel damageLabel;
    public UILabel[] commonLabel;
    public UILabel[] higerLabel;
    public UIButton commonBtn;
    public UIButton higerBtn;
    public UIButton closeBtn;
    public UIToggle chooseTog;
    public UILabel commonGoldLabel;
    public UILabel HighGoldLabel;
    public UILabel desHigh;

    public UISprite[] ybSp;
    protected EquipmentInfo curWeaponInfo;
    protected EquipmentInfo commonBless;
    protected EquipmentInfo highBless;
    public GameObject notLargeGo;
    public GameObject largetGo;

    protected int commonNum = 0;
    protected int highNum = 0;
    #endregion

    #region 特效
    int oldLuckLv = 0;
    int newLuckLv = 0;
    public UIFxAutoActive commonEfx;
    public UIFxAutoActive highEfx;
    public UIFxAutoActive addEfx;
    public UIFxAutoActive dontEfx;
    public UIFxAutoActive reduceEfx;
    #endregion
    void Awake()
    {
        mutualExclusion = false;
        Layer = GUIZLayer.TOPWINDOW;
        if (closeBtn != null) UIEventListener.Get(closeBtn.gameObject).onClick = delegate
        {
            GameCenter.uIMng.ReleaseGUI(GUIType.BLESSWND);
        };
        if (chooseTog != null) EventDelegate.Add(chooseTog.onChange, OnChangeTog);
            
    }
    protected override void OnOpen()
    {
        base.OnOpen();
        curWeaponInfo = GameCenter.inventoryMng.curEquWeapon;
        if (curWeaponInfo != null)
        {
            Refresh(curWeaponInfo);
        }
        oldLuckLv = curWeaponInfo.LuckyLv;
        //普通祝福油
        if (commonBtn != null && commonBless != null) UIEventListener.Get(commonBtn.gameObject).onClick = delegate
        {
            oldLuckLv = curWeaponInfo.LuckyLv;
            if (commonNum == 0)
            {
                if (chooseTog.value)
                {
                    if (DiamondAch(commonBless))
                    {
                        //快捷购买
                        GameCenter.inventoryMng.C2S_AskBlessWeapon(0);
                        commonEfx.ReShowFx(PlayBlessEffect);
                    }
                    else
                        ToRecharge();
                }
                else
                    GameCenter.messageMng.AddClientMsg(141);
            }
            else
            {
                GameCenter.inventoryMng.C2S_UseItem(commonBless);
                commonEfx.ReShowFx(PlayBlessEffect);
            }
        };
        //高级祝福油
        if (higerBtn != null) UIEventListener.Get(higerBtn.gameObject).onClick = delegate
        {
            oldLuckLv = curWeaponInfo.LuckyLv;
            if (highNum == 0)
            {
                if (chooseTog.value)
                {
                    if (DiamondAch(highBless))
                    {
                        //快捷购买
                        GameCenter.inventoryMng.C2S_AskBlessWeapon(1);
                        highEfx.ReShowFx(PlayBlessEffect);
                    }
                    else
                        ToRecharge();
                }
                else
                    GameCenter.messageMng.AddClientMsg(141);
            }
            else
            {
                GameCenter.inventoryMng.C2S_UseItem(highBless);
                highEfx.ReShowFx(PlayBlessEffect);
            }
        };
        GameCenter.inventoryMng.OnEquipUpdate += RefreshEqu;
    }
    protected override void OnClose()
    {
        base.OnClose();
        GameCenter.inventoryMng.OnEquipUpdate -= RefreshEqu;
        curWeaponInfo.BelongTo = EquipmentBelongTo.EQUIP;
        curWeaponInfo = null;
        commonBless = null;
        highBless = null;
        if (chooseTog != null) EventDelegate.Remove(chooseTog.onChange, OnChangeTog);
    }
    /// <summary>
    /// 播放特效
    /// </summary>
    void PlayBlessEffect()
    {
        if (newLuckLv > oldLuckLv)
            addEfx.ReShowFx();
        else if (newLuckLv < oldLuckLv)
            reduceEfx.ReShowFx();
        else
            dontEfx.ReShowFx();
    }
    void OnChangeTog()
    {
        if (commonGoldLabel != null) commonGoldLabel.gameObject.SetActive(chooseTog.value);
        if (HighGoldLabel != null) HighGoldLabel.gameObject.SetActive(chooseTog.value);
        if (desHigh != null) desHigh.gameObject.SetActive(!chooseTog.value);
        for (int i = 0; i < ybSp.Length; i++)
        {
            if (ybSp[i] != null) ybSp[i].gameObject.SetActive(chooseTog.value);
        }
    }
    void RefreshEqu()
    {
        Refresh(curWeaponInfo);
    }
    void Refresh(EquipmentInfo _info)
    {
        if (_info != null)
        {
            newLuckLv = _info.LuckyLv;
            _info.BelongTo = EquipmentBelongTo.PREVIEW;
        }
        if (itemEq != null) itemEq.FillInfo(_info);
        if (luckyNum != null) luckyNum.text = _info.LuckyLv.ToString();
        if (luckyChance != null) luckyChance.text = _info.LuckyLv * 11 + "%";
        if (damageLabel != null) damageLabel.text = ConfigMng.Instance.GetUItext(269) + GameCenter.mainPlayerMng.MainPlayerInfo.AttackStr;
        if (_info.LuckyLv < 9)
        {
            if (notLargeGo != null) notLargeGo.SetActive(true);
            if (largetGo != null) largetGo.SetActive(false);
        }
        else
        {
            if (notLargeGo != null) notLargeGo.SetActive(false);
            if (largetGo != null) largetGo.SetActive(true);
            return;
        }
        commonNum = 0;
        highNum = 0;
        foreach (var item in GameCenter.inventoryMng.BackPackDictionary)
        {
            if (item.Value.EID == 2400005)
            {
                commonNum += item.Value.StackCurCount;
                commonBless = item.Value;
            }
            if (item.Value.EID == 2400006)
            {
                highNum += item.Value.StackCurCount;
                highBless = item.Value;
            }
        }
        if (commonNum == 0)
            commonBless = new EquipmentInfo(2400005, EquipmentBelongTo.PREVIEW);
        if (highNum == 0)
            highBless = new EquipmentInfo(2400006, EquipmentBelongTo.PREVIEW);

        if (itemCommonBless != null && commonBless != null)
        {
            itemCommonBless.FillInfo(commonBless);
            itemCommonBless.itemCount.gameObject.SetActive(true);
            itemCommonBless.itemCount.text = ((commonNum == 0) ? "[ff0000]0[-]" : commonNum.ToString())+ "/1";
        }
        if (itemHighBless != null && highBless != null)
        {
            itemHighBless.FillInfo(highBless);
            itemHighBless.itemCount.gameObject.SetActive(true);
            itemHighBless.itemCount.text = ((highNum == 0) ? "[ff0000]0[-]" : highNum.ToString()) + "/1";
        }
        ResolveLevelRef resolveRef = ConfigMng.Instance.GetResolveRef(_info.LuckyLv == 0 ? 1 : _info.LuckyLv);
        if (resolveRef != null)
        {
            if (commonLabel[0] != null && resolveRef.lccke_Test[0] != null) commonLabel[0].text = resolveRef.lccke_Test[0].str1;
            if (commonLabel[1] != null && resolveRef.lccke_Test[0] != null) commonLabel[1].text = resolveRef.lccke_Test[0].str2;
            if (higerLabel[0] != null && resolveRef.lccke_Test[1] != null) higerLabel[0].text = resolveRef.lccke_Test[1].str1;
            if (higerLabel[1] != null && resolveRef.lccke_Test[1] != null) higerLabel[1].text = resolveRef.lccke_Test[1].str2;
        }
        if (commonGoldLabel != null) commonGoldLabel.text = commonBless.DiamondPrice + "/" + GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount;
        if (HighGoldLabel != null) HighGoldLabel.text = highBless.DiamondPrice + "/" + GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount;
    }
    /// <summary>
    /// 元宝是否充足
    /// </summary>
    bool DiamondAch( EquipmentInfo _info)
    {
        return GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount >= _info.DiamondPrice;
    }
    
    void ToRecharge()
    {
        MessageST msg = new MessageST();
        msg.messID = 137;
        msg.delYes = delegate
        {
            GameCenter.uIMng.ReleaseGUI(GUIType.BLESSWND);
            GameCenter.uIMng.SwitchToUI(GUIType.RECHARGE);
        };
        GameCenter.messageMng.AddClientMsg(msg);
    }
}
