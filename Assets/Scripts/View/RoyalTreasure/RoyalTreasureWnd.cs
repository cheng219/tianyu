//======================================================
//作者:鲁家旗
//日期:2017/1/19
//用途:皇室宝箱界面
//======================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class RoyalTreasureWnd : GUIBase
{
    public UIButton closeBtn;
    public TreasueItemUI[] treasureItem = new TreasueItemUI[4];
    //今日加速次数
    public UILabel restActiveTimes;
    public GameObject restActiveGo;
    public GameObject normalGo;
    public GameObject rewardGo;
    void Awake()
    {
        mutualExclusion = true;
        layer = GUIZLayer.NORMALWINDOW;
    }
    void Start()
    {
        if (closeBtn != null)
        {
            UIEventListener.Get(closeBtn.gameObject).onClick = delegate
            {
                GameCenter.uIMng.SwitchToUI(GUIType.NONE);
            };
        }
    }
    protected override void OnOpen()
    {
        base.OnOpen();
        GameCenter.royalTreasureMng.C2S_ReqRoyalBoxList();
        //FillTreasueInfo();
        GameCenter.royalTreasureMng.OnRoyalBoxUpdate += FillTreasueInfo;
        GameCenter.royalTreasureMng.OnGetRoyalReward += RefreshReward;
        GameCenter.royalTreasureMng.OnGetRewardOver += RefreshRewardOver;
    }
    protected override void OnClose()
    {
        base.OnClose();
        GameCenter.royalTreasureMng.OnRoyalBoxUpdate -= FillTreasueInfo;
        GameCenter.royalTreasureMng.OnGetRoyalReward -= RefreshReward;
        GameCenter.royalTreasureMng.OnGetRewardOver -= RefreshRewardOver;
    }
    void FillTreasueInfo()
    {
        FDictionary royalDic = GameCenter.royalTreasureMng.royalTreasureDic;
        int index = 0;
        foreach (RoyalTreaureData data in royalDic.Values)
        {
            treasureItem[index].RefreshTreasureItem(data);
            index++;
        }
        for (; index < treasureItem.Length; index++)
        {
            treasureItem[index].RefreshTreasureItem(null);
        }
        if (restActiveTimes != null) restActiveTimes.text = GameCenter.royalTreasureMng.restActiveTimes.ToString();
    }
    /// <summary>
    /// 刷新领奖界面
    /// </summary>
    /// <param name="_list"></param>
    void RefreshReward(List<EquipmentInfo> _list)
    {
        if (restActiveGo != null) restActiveGo.SetActive(false);
        if (normalGo != null) normalGo.SetActive(false);
        if (rewardGo != null) 
        {
            rewardGo.SetActive(true);
            RoyalRewardUI royalRewardUI = rewardGo.GetComponent<RoyalRewardUI>();
            if (royalRewardUI != null)
                royalRewardUI.CreateRewardItem(_list, GameCenter.royalTreasureMng.curGetRewardBoxData);
        }
    }
    /// <summary>
    /// 领奖完毕后重新排序
    /// </summary>
    void RefreshRewardOver()
    {
        if (restActiveGo != null) restActiveGo.SetActive(true);
        if (normalGo != null) normalGo.SetActive(true);
        if (rewardGo != null) rewardGo.SetActive(false);
        FillTreasueInfo();
    }
}
