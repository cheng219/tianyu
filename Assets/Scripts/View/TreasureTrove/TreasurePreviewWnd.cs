//==================================
//作者：邓成
//日期：2017/4/6
//用途：宝藏活动奖励预览界面类
//=================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class TreasurePreviewWnd : SubWnd {
    public ItemUIContainer items;
    public UIButton btnOpenOne;
    public UILabel openOnePrice;
    public UILabel openOneReturn;
    public UIButton btnOpenTen;
    public UILabel openTenPrice;
    public UILabel openTenReturn;
    private List<EquipmentInfo> itemList = new List<EquipmentInfo>();

    public RoyalRewardUI rewardUi;
    public GameObject effect;

    #region OnOpen,OnClose
    protected override void OnOpen()
    {
        base.OnOpen();
        //RefreshWnd();
    }
    protected override void OnClose()
    {
        base.OnClose();
    }
    #endregion
    #region 事件句柄
    protected override void HandEvent(bool _bind)
    {
        if (_bind)
        {
            GameCenter.treasureTroveMng.updateTreasurePreview += RefreshWnd;
            GameCenter.treasureTroveMng.OnGetTreasureRewardUpdate += GetResult;
            if (btnOpenOne!=null)
                UIEventListener.Get(btnOpenOne.gameObject).onClick += OpenOne;
            if(btnOpenTen!=null)
                UIEventListener.Get(btnOpenTen.gameObject).onClick += OpenTen;
        }
        else
        {
            GameCenter.treasureTroveMng.updateTreasurePreview -= RefreshWnd;
            GameCenter.treasureTroveMng.OnGetTreasureRewardUpdate -= GetResult;
            if (btnOpenOne != null)
                UIEventListener.Get(btnOpenOne.gameObject).onClick -= OpenOne;
            if (btnOpenTen != null)
                UIEventListener.Get(btnOpenTen.gameObject).onClick -= OpenTen;
        }
    }
    #endregion
    #region 界面刷新
    void RefreshWnd()
    {
        itemList = GameCenter.treasureTroveMng.GetReward();
        //Debug.Log("GameCenter.treasureTroveMng.RewardOne.eid:" + GameCenter.treasureTroveMng.RewardOne.eid);
        string name = ConfigMng.Instance.GetEquipmentRef(GameCenter.treasureTroveMng.RewardOne.eid).name;
        int count = GameCenter.treasureTroveMng.RewardOne.count;
        string title = ConfigMng.Instance.GetEquipmentRef(GameCenter.treasureTroveMng.RewardTwo.eid).name;
        int num = GameCenter.treasureTroveMng.RewardTwo.count;
        if (itemList!=null)
        {
                if (items != null)
                {
                    items.RefreshItems(itemList, 3, itemList.Count);
                }
        }
        if (openOnePrice!=null&& GameCenter.treasureTroveMng.PriceOne!=0)
        {
            openOnePrice.text = GameCenter.treasureTroveMng.PriceOne.ToString();
        }
        if(openTenPrice!=null && GameCenter.treasureTroveMng.PriceTwo!=0)
        {
            openTenPrice.text = GameCenter.treasureTroveMng.PriceTwo.ToString();
        }
        if (openOneReturn!=null&&!string.IsNullOrEmpty(name)&&count!=0)
        {
            openOneReturn.text = ConfigMng.Instance.GetUItext(356)+name+ "x"+count.ToString();
        }
        if(openTenReturn!=null&& !string.IsNullOrEmpty(title) && num!=0)
        {
            openTenReturn.text = ConfigMng.Instance.GetUItext(356) + title + "x" + num.ToString();
        } 
    }
 
    void GetResult()
    {
        if (rewardUi != null) rewardUi.gameObject.SetActive(true); 
        if (effect != null) effect.SetActive(true); 
        CancelInvoke("ShowReward");
        Invoke("ShowReward", 1.8f); 
    }
    void ShowReward()
    {
        List<EquipmentInfo> list = GameCenter.treasureTroveMng.treasureRewardList; 
        if (rewardUi != null)
        { 
            rewardUi.CreateRewardItem(list);
        }
    }

    #endregion
    #region UI控件的响应
    void OpenOne(GameObject _obj)
    {
        if ((ulong)GameCenter.treasureTroveMng.PriceOne > GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount)
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
        GameCenter.treasureTroveMng.C2S_ReqOpenTreasureOnce(1);
    }
    void OpenTen(GameObject _obj)
    {
        if ((ulong)GameCenter.treasureTroveMng.PriceTwo > GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount)
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
        GameCenter.treasureTroveMng.C2S_ReqOpenTreasureOnce(2);
    }
    #endregion
}
