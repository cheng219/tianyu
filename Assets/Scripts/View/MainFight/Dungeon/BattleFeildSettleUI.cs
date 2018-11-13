//======================================================
//作者:朱素云
//日期:2017/3/2
//用途:火焰山结算ui
//======================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;

public class BattleFeildSettleUI : MonoBehaviour
{ 
    /// <summary>
    /// 当前查看0为仙界1为妖界
    /// </summary>
    protected int curChack = 0;
    public UIButton closeBtn;
    public UIToggle[] uiTog;
    public UIGrid rankGrid;
    /// <summary>
    /// 排行ui
    /// </summary>
    public BattleFieldRankUi battleFieldRankUi;
    protected Dictionary<int, BattleFieldRankUi> allItems = new Dictionary<int, BattleFieldRankUi>();
    public UILabel rewardUp10;
    public UILabel rewardUp30;
    public GameObject noReward;
    public UIScrollView scrollView;

    public GameObject fairyWinGo;
    public GameObject fairyLoseGo;

    void Awake()
    {
        if (closeBtn != null) UIEventListener.Get(closeBtn.gameObject).onClick = delegate {
            GameCenter.uIMng.SwitchToUI(GUIType.NONE);
            GameCenter.duplicateMng.C2S_OutCopy();
        };
        for (int i = 0, max = uiTog.Length; i < max; i++)
        { 
            EventDelegate.Remove(uiTog[i].onChange, uiTogOnChange); 
            EventDelegate.Add(uiTog[i].onChange, uiTogOnChange); 
        } 
    }
    void uiTogOnChange()
    {
        for (int i = 0, max = uiTog.Length; i < max; i++)
        {
            if (uiTog[i].value)
            {
                curChack = i;
            }
        }
        ShowRankUI();
    }

    /// <summary>
    /// 结算排行
    /// </summary>
    void ShowRankUI()
    {
        int myCampFaily = 0;//我方 0仙界 1妖界
        int fightState = 0;//战况 1胜利2失败3平局 根据这个取结算奖励表的数据

        HideAllItems();
        SettlementData settlementData = GameCenter.battleFightMng.settlementData;
        if (settlementData == null) return; 
        bool haveSelf = false;
        List<mountain_flames_win> battleRankList = new List<mountain_flames_win>();
         
        if (settlementData.fightState < 0)//平局
        {
            if (noReward != null) noReward.SetActive(false);
            fightState = 3;
        }
        else
        {
            if (noReward != null) noReward.SetActive(true);
        }

        if (curChack == 0)//查看仙界排行
        {
            battleRankList = settlementData.failyData; 
        }
        else//查看妖界排行
        {
            battleRankList = settlementData.demonData; 
        }

        bool fairyWin = false;
        if (settlementData.fightState == 0)//我方失败
        {
            fightState = 2;
            if (GameCenter.mainPlayerMng.MainPlayerInfo.Camp == 1)//我方为仙界
            {
                if (rewardUp10 != null) rewardUp10.gameObject.SetActive(curChack == 0);
                if (rewardUp30 != null) rewardUp30.gameObject.SetActive(curChack == 1);
                myCampFaily = 0;
                fairyWin = false;
            }
            else
            {
                if (rewardUp10 != null) rewardUp10.gameObject.SetActive(curChack == 1);
                if (rewardUp30 != null) rewardUp30.gameObject.SetActive(curChack == 0);
                fairyWin = true;
                myCampFaily = 1;
            }
        }
        if (settlementData.fightState == 1)//我方胜利
        {
            fightState = 1;
            if (GameCenter.mainPlayerMng.MainPlayerInfo.Camp == 1)//我方为仙界
            {
                if (rewardUp10 != null) rewardUp10.gameObject.SetActive(curChack == 1);
                if (rewardUp30 != null) rewardUp30.gameObject.SetActive(curChack == 0);
                fairyWin = true;
                myCampFaily = 0;
            }
            else
            {
                if (rewardUp10 != null) rewardUp10.gameObject.SetActive(curChack == 0);
                if (rewardUp30 != null) rewardUp30.gameObject.SetActive(curChack == 1);
                fairyWin = false;
                myCampFaily = 1;
            }
        }

        if (fairyWinGo != null) fairyWinGo.SetActive(fairyWin);//仙界胜利
        if (fairyLoseGo != null) fairyLoseGo.SetActive(!fairyWin);//仙界失败

        for (int i = 0, max = battleRankList.Count; i < max; i++)
        {
            BattleFieldRankUi item;
            if (!allItems.TryGetValue(i, out item))
            {
                if (rankGrid != null) item = battleFieldRankUi.CreateNew(rankGrid.transform);
                allItems[i] = item;
            }
            item = allItems[i];
            if (item != null) item.gameObject.SetActive(true);
            if (item != null) item.SetSettlementData(battleRankList[i], i + 1, curChack == myCampFaily, fightState);
            if (battleRankList[i].uid == GameCenter.mainPlayerMng.MainPlayerInfo.ServerInstanceID)
            {
                if (battleFieldRankUi != null)
                {
                    if (curChack == 0)
                        battleFieldRankUi.SetSettlementData(battleRankList[i], i + 1, curChack == myCampFaily, fightState);
                    battleFieldRankUi.gameObject.SetActive(true);
                }
                haveSelf = true;
            }
            //item.transform.localPosition = new Vector3(0, 43 * i, 0);  排序bug
            //item.gameObject.SetActive(true);
        }
        if (rankGrid != null)
        {
            rankGrid.repositionNow = true;
        }
        if (!haveSelf) battleFieldRankUi.gameObject.SetActive(false);
        if (scrollView != null) scrollView.SetDragAmount(0, 0, false);
    }

    void HideAllItems()
    {
        foreach (BattleFieldRankUi item in allItems.Values)
        {
            if (item != null)
            {
                item.gameObject.SetActive(false);
            }
        }
    }
}