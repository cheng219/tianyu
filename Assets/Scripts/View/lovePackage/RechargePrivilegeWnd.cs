//======================================================
//作者:朱素云
//日期:2017/4/7
//用途:充值优惠一级界面
//======================================================
using UnityEngine;
using System.Collections;

public class RechargePrivilegeWnd : GUIBase
{
    public GameObject btnClose;

    public UIToggle togPrivilege;
    public UIToggle togLove;
    public UIToggle togWeek;
    public UIToggle togLoginBunus;

    public TwoChargeWnd twoChargeWnd;
    public LovePackageWnd lovePackageWnd;
    public RechargeWeekWnd rechargeWeekWnd;
    public LoginBonusWnd loginBonusWnd;

    protected RechargePrivilegeType rechargeType = RechargePrivilegeType.none;

    void Awake()
    {
        mutualExclusion = true;
        if (btnClose != null) UIEventListener.Get(btnClose).onClick = delegate
            {
                if (twoChargeWnd != null) twoChargeWnd.CloseUI();
                if (lovePackageWnd != null) lovePackageWnd.CloseUI();
                if (rechargeWeekWnd != null) rechargeWeekWnd.CloseUI();
                if (loginBonusWnd != null) loginBonusWnd.CloseUI();
                //GameCenter.uIMng.ReleaseGUI(GUIType.PRIVILEGE);
                GameCenter.uIMng.SwitchToUI(GUIType.NONE);
            }; 
    }

    protected override void OnOpen()
    {
        base.OnOpen();
        OnTogChange();  
    }

    protected override void OnClose()
    {
        base.OnClose(); 
    }

    protected override void HandEvent(bool _bind)
    {
        base.HandEvent(_bind);
        if (_bind)
        {
            if (togPrivilege != null) EventDelegate.Add(togPrivilege.onChange, ShowWnd);
            if (togLove != null) EventDelegate.Add(togLove.onChange, ShowWnd);
            if (togWeek != null) EventDelegate.Add(togWeek.onChange, ShowWnd);
            if (togLoginBunus != null) EventDelegate.Add(togLoginBunus.onChange, ShowWnd);
            GameCenter.lovePackageMng.OnOpenLoveRechargeUpdate += OnTogChange;
            GameCenter.twoChargeMng.OnOpenTwoChargeUpdate += OnTogChange;
            GameCenter.weekCardMng.OnOpenLoginBonusUpdate += OnTogChange;
        }
        else
        {
            if (togPrivilege != null) EventDelegate.Remove(togPrivilege.onChange, ShowWnd);
            if (togLove != null) EventDelegate.Remove(togLove.onChange, ShowWnd);
            if (togWeek != null) EventDelegate.Remove(togWeek.onChange, ShowWnd);
            if (togLoginBunus != null) EventDelegate.Remove(togLoginBunus.onChange, ShowWnd);
            GameCenter.lovePackageMng.OnOpenLoveRechargeUpdate -= OnTogChange;
            GameCenter.twoChargeMng.OnOpenTwoChargeUpdate -= OnTogChange;
            GameCenter.weekCardMng.OnOpenLoginBonusUpdate -= OnTogChange;
        }
    }
    /// <summary>
    /// 功能开启事件
    /// </summary>
    void OnTogChange()
    {
        if (!GameCenter.twoChargeMng.isOpenTwoCharge)
        {
            togPrivilege.value = false;
            togPrivilege.gameObject.SetActive(false);
            if (rechargeType == RechargePrivilegeType.twoRecharge)
            {
                rechargeType = RechargePrivilegeType.none;
            }
        }
        if (!GameCenter.lovePackageMng.isOpenLoveReward)
        {
            togLove.value = false;
            togLove.gameObject.SetActive(false);
            if (rechargeType == RechargePrivilegeType.loveReward)
            {
                rechargeType = RechargePrivilegeType.none;
            }
        }
        if (!GameCenter.weekCardMng.isOpenLoginBonus)
        {
            togLoginBunus.value = false;
            togLoginBunus.gameObject.SetActive(false);
            if (rechargeType == RechargePrivilegeType.weekRecharge)
            {
                rechargeType = RechargePrivilegeType.none;
            }
        }

        if (GameCenter.twoChargeMng.isOpenTwoCharge)
        {
            togPrivilege.gameObject.SetActive(true);
            if (rechargeType == RechargePrivilegeType.none)
            {
                togPrivilege.value = true;
            }
        }
        if (GameCenter.lovePackageMng.isOpenLoveReward)
        {
            togLove.gameObject.SetActive(true);
            if (rechargeType == RechargePrivilegeType.none)
            {
                togLove.value = true;
            }
        }
        if (GameCenter.weekCardMng.isOpenLoginBonus)
        {
            togLoginBunus.gameObject.SetActive(true);
            if (rechargeType == RechargePrivilegeType.none)
            {
                togLoginBunus.value = true;
            }
        }
        if (rechargeType == RechargePrivilegeType.none)
        {
            togWeek.value = true;
        } 
    }

    void ShowWnd()
    { 
        if (togPrivilege.gameObject.activeSelf && togPrivilege.value)
        { 
            if (twoChargeWnd != null) twoChargeWnd.OpenUI();
            if (lovePackageWnd != null) lovePackageWnd.CloseUI();
            if (rechargeWeekWnd != null) rechargeWeekWnd.CloseUI();
            if (loginBonusWnd != null) loginBonusWnd.CloseUI();
        }
        if (togLove.gameObject.activeSelf && togLove.value)
        { 
            if (lovePackageWnd != null) lovePackageWnd.OpenUI();
            if (rechargeWeekWnd != null) rechargeWeekWnd.CloseUI();
            if (twoChargeWnd != null) twoChargeWnd.CloseUI();
            if (loginBonusWnd != null) loginBonusWnd.CloseUI();
        }
        if (togWeek.gameObject.activeSelf && togWeek.value)
        { 
            if (lovePackageWnd != null) lovePackageWnd.CloseUI();
            if (rechargeWeekWnd != null) rechargeWeekWnd.OpenUI();
            if (twoChargeWnd != null) twoChargeWnd.CloseUI();
            if (loginBonusWnd != null) loginBonusWnd.CloseUI();
        }
        if (togLoginBunus.gameObject.activeSelf && togLoginBunus.value)
        {
            if (lovePackageWnd != null) lovePackageWnd.CloseUI();
            if (rechargeWeekWnd != null) rechargeWeekWnd.CloseUI();
            if (twoChargeWnd != null) twoChargeWnd.CloseUI();
            if (loginBonusWnd != null) loginBonusWnd.OpenUI();
        }
    }
     
}

public enum RechargePrivilegeType
{ 
    none,
    twoRecharge,
    loveReward,
    weekRecharge,
    loginBonus,
}
