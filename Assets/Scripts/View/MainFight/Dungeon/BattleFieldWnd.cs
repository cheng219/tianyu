//======================================================
//作者:朱素云
//日期:2017/1/12
//用途:火焰山战场界面
//======================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleFieldWnd : SubWnd
{
    #region 控件
    /// <summary>
    /// 站场图
    /// </summary>
    public GameObject battlePop;
    /// <summary>
    /// 战场时间
    /// </summary>
    public UITimer timer;
    public UITimer worningTimer;
    public GameObject oneMinWorn;
    /// <summary>
    /// 战场频道
    /// </summary>
    public UILabel channel;
    /// <summary>
    /// 排行ui
    /// </summary>
    public BattleFieldRankUi battleFieldRankUi;
    /// <summary>
    /// 回城按钮
    /// </summary>
    public UIButton goBackCityBtn;
    /// <summary>
    /// 回城背景
    /// </summary>
    public UISpriteEx goBackCityEx; 
    /// <summary>
    /// 回城引导进度条
    /// </summary>
    //public UISlider goBackingSli;
    /// <summary>
    /// 回城CD
    /// </summary>
    public UITimer goBackCityCD; 
    /// <summary>
    /// 查看评分
    /// </summary>
    public GameObject checkScore;
    /// <summary>
    /// 己方阵营
    /// </summary>
    public BattleFieldCamp failyCamp;
    /// <summary>
    /// 敌方阵营
    /// </summary>
    public BattleFieldCamp demonCamp;

    public UIGrid rankGrid;
    public UIScrollView scrollView;

    protected float gobacktimer  = 0;

    protected Dictionary<int, BattleFieldRankUi> allItems = new Dictionary<int, BattleFieldRankUi>();

    public GameObject reardTip;
    public UITimer readyTime;
    public GameObject effect;
    public UILabel xainCamp;
    public UILabel yaoCamp;
    public UISprite goSp;

    #endregion


    #region unity

    protected override void OnOpen()
    {
        base.OnOpen();
        if (effect != null) effect.SetActive(false);
        if (reardTip != null) reardTip.SetActive(false);
        if (xainCamp != null) xainCamp.transform.parent.gameObject.SetActive(false); 
        OnChannelUpdate();
        RefreshGoBackState();
        if (timer != null) timer.transform.parent.gameObject.SetActive(true);
        if (oneMinWorn != null) oneMinWorn.SetActive(false);
        if (battlePop != null) battlePop.SetActive(true);
        if (battleFieldRankUi != null) battleFieldRankUi.gameObject.SetActive(false);
        if (goBackCityBtn != null) UIEventListener.Get(goBackCityBtn.gameObject).onClick = OnClickGoBack;
        if (checkScore != null) UIEventListener.Get(checkScore).onClick = delegate {
            GameCenter.uIMng.GenGUI(GUIType.BATTLECOMENTDES, true); 
        }; 
    }
    protected override void OnClose()
    {
        base.OnClose();
        if (battlePop != null) battlePop.SetActive(false); 
    }
    protected override void HandEvent(bool _bind)
    {
        base.HandEvent(_bind);
        if (_bind)
        {
            GameCenter.dungeonMng.OnDungeonTimeUpdate += ShowTime; 
            GameCenter.battleFightMng.OnBattleRankUpdate += ShowRankUI;
            GameCenter.battleFightMng.OnBattleCampUpdate += RereshCamp;
            GameCenter.battleFightMng.OnGoBackUpdate += RefreshGoBackState;
            GameCenter.battleFightMng.OnFightersUpdate += RereshCamp;
            GameCenter.battleFightMng.OnChannelUpdate += OnChannelUpdate;
            GameCenter.battleFightMng.OnCountDownUpdate += ShowReady;
        }
        else
        {
            GameCenter.dungeonMng.OnDungeonTimeUpdate -= ShowTime; 
            GameCenter.battleFightMng.OnBattleRankUpdate -= ShowRankUI;
            GameCenter.battleFightMng.OnBattleCampUpdate -= RereshCamp;
            GameCenter.battleFightMng.OnGoBackUpdate -= RefreshGoBackState;
            GameCenter.battleFightMng.OnFightersUpdate -= RereshCamp;
            GameCenter.battleFightMng.OnChannelUpdate -= OnChannelUpdate;
            GameCenter.battleFightMng.OnCountDownUpdate -= ShowReady;
        }
    }
     

    #endregion


    #region 事件

    void ShowReady()
    { 
        if (goSp != null) goSp.gameObject.SetActive(false);
        if (reardTip != null) reardTip.SetActive(true);
        if (effect != null) effect.SetActive(true);
        if (readyTime != null)
        { 
            readyTime.StartIntervalTimer(GameCenter.battleFightMng.CountDown);
            readyTime.onTimeOut = (x) =>
            { 
                if (effect != null) effect.SetActive(false);
                if (xainCamp != null) xainCamp.transform.parent.gameObject.SetActive(false); 
                if (goSp != null) goSp.gameObject.SetActive(true);
                readyTime.gameObject.SetActive(false);
                CancelInvoke("CloseGo");
                Invoke("CloseGo", 1.0f);
            };
        }
    }

    void CloseGo()
    {
        if (reardTip != null) reardTip.SetActive(false);
        if (goSp != null) goSp.gameObject.SetActive(false); 
    }
     
    void OnChannelUpdate()
    {
        if (channel != null) channel.text = GameCenter.battleFightMng.Channel.ToString();
    }

    /// <summary>
    /// 点击回城(回城被打断提示 ID 473)
    /// </summary>
    /// <param name="go"></param>
    void OnClickGoBack(GameObject go)
    {  
        GameCenter.battleFightMng.C2S_ReqGoBack(); 
    }

    /// <summary>
    /// 刷新阵营数据
    /// </summary>
    void RereshCamp()
    {
        List<st.net.NetBase.mountain_amount_score> CampScore = GameCenter.battleFightMng.CampScore;
        for (int i = 0, max = CampScore.Count; i < max; i++)
        {
            if (CampScore[i].camp == 1)
            {
                failyCamp.SetScore(CampScore[i]);
            }
            else
            {
                demonCamp.SetScore(CampScore[i]);
            }
        }
    }

    /// <summary>
    /// 刷新战士状态
    /// </summary> 
    void RereshCamp(int _side)
    { 
        if (_side == 1)
        {
            if (GameCenter.battleFightMng.failyState != null)
            {
                failyCamp.SetCampData(GameCenter.battleFightMng.failyState);
            }
        }
        else
        {
            if (GameCenter.battleFightMng.demonState != null)
            {
                demonCamp.SetCampData(GameCenter.battleFightMng.demonState);
            }
        }
    }
    /// <summary>
    /// 刷新回城状态
    /// </summary>
    void RefreshGoBackState()
    {
        int time = GameCenter.battleFightMng.GoBackCD; 
        if (time > 0)//回城CD
        { 
            if (goBackCityEx != null) goBackCityEx.IsGray = UISpriteEx.ColorGray.Gray;
            if (goBackCityCD != null)
            {
                goBackCityCD.transform.parent.gameObject.SetActive(true);
                goBackCityCD.StartIntervalTimer(time);
                goBackCityCD.onTimeOut = (x) =>
                {
                    if (goBackCityEx != null) goBackCityEx.IsGray = UISpriteEx.ColorGray.normal; 
                    goBackCityCD.transform.parent.gameObject.SetActive(false);
                };
            } 
        }
        else
        { 
            if (goBackCityEx != null) goBackCityEx.IsGray = UISpriteEx.ColorGray.normal;
            if (goBackCityCD != null)
            { 
                goBackCityCD.transform.parent.gameObject.SetActive(false);
            }
        } 
    }

    /// <summary>
    /// 战场时间
    /// </summary>
    void ShowTime()
    { 
        int time = GameCenter.dungeonMng.DungeonTime;
        if (timer != null) timer.StartIntervalTimer(time);
        if (worningTimer != null)
        {
            worningTimer.StartIntervalTimer(time - 60);
            worningTimer.onTimeOut = (x) =>
                {
                    timer.transform.parent.gameObject.SetActive(false);
                    if (oneMinWorn != null) oneMinWorn.SetActive(true);
                };
        }
    }

    /// <summary>
    /// 战场排行
    /// </summary>
    void ShowRankUI()
    { 
        HideAllItems();
        if (GameCenter.battleFightMng.rankListInfo.Count <= 0) return; 
        bool haveSelf = false;
        List<st.net.NetBase.mountain_flames_rank> battleRankList = GameCenter.battleFightMng.rankListInfo;
        for (int i = 0, max = battleRankList.Count; i < max; i++)
        {
            BattleFieldRankUi item;
            if (!allItems.TryGetValue(i, out item))
            {
                if (rankGrid != null) item = battleFieldRankUi.CreateNew(rankGrid.transform);
                allItems[i] = item;
            }
            item = allItems[i];
            if (item != null)
            {
                item.gameObject.SetActive(true);
                item.SetData(battleRankList[i], i + 1);
            }
            if (battleRankList[i].uid == GameCenter.mainPlayerMng.MainPlayerInfo.ServerInstanceID)
            {
                if (reardTip != null && reardTip.activeSelf)
                {
                    if (xainCamp != null) xainCamp.transform.parent.gameObject.SetActive(true); 
                    if (xainCamp != null) xainCamp.gameObject.SetActive(battleRankList[i].camp == 1); 
                    if (yaoCamp != null) yaoCamp.gameObject.SetActive(battleRankList[i].camp != 1); 
                }
                if (battleFieldRankUi != null)
                {
                    battleFieldRankUi.SetData(battleRankList[i], i + 1);
                    battleFieldRankUi.gameObject.SetActive(true);
                }
                haveSelf = true;
            }
        }
        if (rankGrid != null)
        {
            rankGrid.repositionNow = true; 
        }
        if (!haveSelf) battleFieldRankUi.gameObject.SetActive(false);
        if (scrollView != null)
        { 
            scrollView.SetDragAmount(0, 0, false);
        }
    }

    void HideAllItems()
    {
        foreach (BattleFieldRankUi item in allItems.Values)
        {
            if (item != null)
                item.gameObject.SetActive(false);
        }
    }
    #endregion
}
