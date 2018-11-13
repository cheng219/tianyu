//====================
//作者：鲁家旗
//日期：2016/5/10
//用途：首冲大礼界面
//====================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class FirstChargeBonus : GUIBase
{
    #region 控件
    public UIButton closeBtn;
    public UIButton getRewardBtn;
    public UIButton vipBtn;
    public UISpriteEx backSp;
    public ItemUI[] item;

    protected List<int> rewardId = null;
    protected List<int> rewardNum = null;
    public UITexture modelOne;
    public Load3DObject modelTwo;
    protected UIEventListener eventListener;
    protected GameObject games = null;
    protected bool isFirstCharge = false;
    #endregion

    public FXCtrl fxCtrlL;
    public FXCtrl fxCtrlR;

    #region 构造
    void Awake()
    {
        if (fxCtrlL != null)
        {
            fxCtrlL.DoNormalEffect("e_c_keep_115", () =>
            {
                UISortBehavior uisb = fxCtrlL.gameObject.AddComponent<UISortBehavior>();
                if (uisb != null) uisb.widgetInFrontOfMe = backSp;
                fxCtrlL.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            });
        }
        if (fxCtrlR != null)
        {
            fxCtrlR.DoNormalEffect("e_c_keep_115", () =>
            {
                UISortBehavior uisb = fxCtrlR.gameObject.AddComponent<UISortBehavior>();
                if (uisb != null) uisb.widgetInFrontOfMe = backSp;
                fxCtrlR.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            });
        }
        mutualExclusion = true;
        Layer = GUIZLayer.NORMALWINDOW;
        if (closeBtn != null) UIEventListener.Get(closeBtn.gameObject).onClick = delegate
        {
            GameCenter.uIMng.SwitchToUI(GUIType.NONE);
        };
        if (vipBtn != null) UIEventListener.Get(vipBtn.gameObject).onClick = delegate
        {
            GameCenter.uIMng.SwitchToUI(GUIType.RECHARGE);
        };
        if (getRewardBtn != null) UIEventListener.Get(getRewardBtn.gameObject).onClick = delegate
        {
            if (GameCenter.firstChargeBonusMng.firstChargeBonusStates == (int)FirstChargeState.HAVECHARGENOTGET)
            {
                GameCenter.lovePackageMng.isCloseFirstBonus = true;
                GameCenter.twoChargeMng.isCloseTwoCharge = true;
                //领取奖励
                GameCenter.firstChargeBonusMng.C2S_ReqGetFirstChargeInfo(2004);
            }
        };
        rewardId = ConfigMng.Instance.GetRewardId(GameCenter.mainPlayerMng.MainPlayerInfo.Prof);
        rewardNum = ConfigMng.Instance.GetRewardNum(GameCenter.mainPlayerMng.MainPlayerInfo.Prof);
        games = GameObject.Find("Item_Wnd");
    }
    protected override void OnOpen()
    {
        base.OnOpen();
        isFirstCharge = true;
        FillItemInfo();
        GameCenter.firstChargeBonusMng.C2S_ReqGetFirstChargeInfo(2003);
        for (int i = 0; i < item.Length; i++)
        {
            eventListener = item[i].gameObject.GetComponent<UIEventListener>();
            if (eventListener == null) eventListener = item[i].gameObject.AddComponent<UIEventListener>();
            eventListener.onClick += RefreItemWnd;
        }
    }
    protected override void OnClose()
    {
        base.OnClose();
        if (games != null)
			games.transform.localPosition = Vector3.zero;
        modelOne.mainTexture = null;
		GameCenter.previewManager.ClearModel();
        ConfigMng.Instance.RemoveBigUIIcon("Pic_sc_bg");
        if (fxCtrlL != null) fxCtrlL.ClearNormalEffect();
        if (fxCtrlR != null) fxCtrlR.ClearNormalEffect(); 
    }
    protected override void HandEvent(bool _bind)
    {
        base.HandEvent(_bind);
        if (_bind)
        {
            GameCenter.firstChargeBonusMng.OnFirstChargeBonus += Refresh;
        }
        else
        {
            GameCenter.firstChargeBonusMng.OnFirstChargeBonus -= Refresh;
            for (int i = 0; i < item.Length; i++)
            {
                item[i].gameObject.GetComponent<UIEventListener>().onClick -= RefreItemWnd;
            }
        }
    }
    #endregion
    void RefreItemWnd(GameObject obj)
    {
        if (isFirstCharge)
        {
            CancelInvoke("RefreItem");
            Invoke("RefreItem", 0.2f);
        }
    }
    void RefreItem()
    {
        isFirstCharge = false;
        if (games == null)
            games = GameObject.Find("Item_Wnd");
        if (games == null)
        {
            UIRoot root = GameObject.FindObjectOfType<UIRoot>();
            Transform itemWnd = root.transform.Find("Item_Wnd");
            if (itemWnd != null)
            {
                games = itemWnd.gameObject;
            }
        }
        if (games != null)
        {
            games.transform.localPosition = new Vector3(0, 0, -200);
        }
    }
    void FillItemInfo()
    {
        for (int i = 0; i < item.Length; i++)
        {
            item[i].FillInfo(new EquipmentInfo(rewardId[i], rewardNum[i], EquipmentBelongTo.PREVIEW));
        }
    }
    void Refresh()
    {
        FirstChargeRef data = ConfigMng.Instance.GetFirstChargeRefTable(GameCenter.mainPlayerMng.MainPlayerInfo.Prof);
        //剑
//        if (modelOne != null && data != null)
//        {
//            GameCenter.previewManager.TryPreviewSingleEquipment(new EquipmentInfo(int.Parse(data.lModel), EquipmentBelongTo.PREVIEW), modelOne);
//        }
		if (fxCtrlL != null && data != null)
		{
			fxCtrlL.DoNormalEffect(data.lModel, (x) =>
			{
				UISortBehavior uisb = fxCtrlL.gameObject.AddComponent<UISortBehavior>();
				if (uisb != null) uisb.widgetInFrontOfMe = backSp;
				if(x != null)x.transform.localScale = 550*Vector3.one;
			});
		}
        //翅膀
        if (modelTwo != null && data != null)
        {
            modelTwo.configID = int.Parse(data.rModel);
            modelTwo.StartLoad();
        }
        //没有完成首充
        if (GameCenter.firstChargeBonusMng.firstChargeBonusStates == (int)FirstChargeState.NOTCHARGE)
        {
            backSp.IsGray = UISpriteEx.ColorGray.Gray;
        }
        //完成首充没有领奖
        else if (GameCenter.firstChargeBonusMng.firstChargeBonusStates == (int)FirstChargeState.HAVECHARGENOTGET)
        {
            backSp.IsGray = UISpriteEx.ColorGray.normal;
        }
        //完成首充并领奖
        else if (GameCenter.firstChargeBonusMng.firstChargeBonusStates == (int)FirstChargeState.HAVECHARGEGET)
        {
            GameCenter.uIMng.SwitchToUI(GUIType.NONE);
        }
    }
}
