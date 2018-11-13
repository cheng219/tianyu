//======================================================
//作者:朱素云
//日期:2016/7/11
//用途:爱心大礼包界面
//======================================================
using UnityEngine;
using System.Collections;

public class LovePackageWnd: SubWnd
{
    #region 数据
    /// <summary>
    /// 奖励物品
    /// </summary>
    public ItemUI[] items;
    /// <summary>
    /// 充值按钮
    /// </summary>
    public UIButton rechargeBtn;
    /// <summary>
    /// 领取按钮
    /// </summary>
    public UIButton takeRewardBtn;
    /// <summary>
    /// 关闭界面
    /// </summary>
    public UIButton closeBtn;
    /// <summary>
    /// 充值额度
    /// </summary>
    public UILabel rechargeLab;
    /// <summary>
    /// 需要达到的充值额度
    /// </summary>
    public UILabel rechargeNeedLab;
    protected LoveSpreeRef love = null;

    #endregion

    #region 构造 

    void Awake()
    {
        GameCenter.lovePackageMng.C2S_ReqGetLoveInfo(); 
        if (closeBtn != null) UIEventListener.Get(closeBtn.gameObject).onClick = OnCloseWnd;
        if (rechargeBtn != null) UIEventListener.Get(rechargeBtn.gameObject).onClick = 
            delegate { GameCenter.uIMng.SwitchToUI(GUIType.RECHARGE); };//跳转至充值界面
        if (takeRewardBtn != null) UIEventListener.Get(takeRewardBtn.gameObject).onClick =
              delegate { if(love != null)GameCenter.lovePackageMng.C2S_ReqTakeReward(love.id); };//请求领取
    }
    protected override void OnOpen()
    { 
        base.OnOpen();
        Refresh();
        GameCenter.lovePackageMng.OnRechargeUpdate += Refresh;
    }
    protected override void OnClose()
    {
        base.OnClose();
        GameCenter.lovePackageMng.OnRechargeUpdate -= Refresh;
    }
    void OnCloseWnd(GameObject go)
    {
        GameCenter.uIMng.SwitchToUI(GUIType.NONE); 
    }


    #endregion

    void Refresh()
    { 
        int chargeVal = GameCenter.lovePackageMng.rechargeVal;
        int prof = GameCenter.mainPlayerMng.MainPlayerInfo.Prof;
        int stage = GameCenter.lovePackageMng.stage; 
        love = ConfigMng.Instance.GetLoveSpreeRef(prof, stage); 
        if (love != null)
        {
            if (rechargeLab != null) rechargeLab.text = chargeVal + "/" + love.money;
            if (chargeVal >= love.money)
            {
                if (rechargeBtn != null) rechargeBtn.gameObject.SetActive(false);
                if (takeRewardBtn != null) takeRewardBtn.gameObject.SetActive(true);
            }
            else
            {
                if (rechargeBtn != null) rechargeBtn.gameObject.SetActive(true);
                if (takeRewardBtn != null) takeRewardBtn.gameObject.SetActive(false);
            }
            if (rechargeNeedLab != null) rechargeNeedLab.text = love.money.ToString();
            for (int i = 0, max = items.Length; i < max; i++)
            {
                if (love.reward.Count > i)
                {
                    items[i].FillInfo(new EquipmentInfo(love.reward[i].eid, love.reward[i].count, EquipmentBelongTo.PREVIEW));
                }
                else
                    items[i].gameObject.SetActive(false);
            }
        }
    }

}
