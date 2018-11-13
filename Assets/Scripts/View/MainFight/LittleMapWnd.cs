//==============================================
//作者：黄洪兴
//日期：2016/7/1
//用途：小地图
//=================================================


using UnityEngine;
using AnimationOrTween;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class LittleMapWnd : GUIBase
{
	/// <summary>
	/// 伸缩菜单的按钮
	/// </summary>
	public UIButton btnMapMenu;

    public TweenPosition[] menuTweens;

    public UIGrid[] menuGrid;
	/// <summary>
	/// 按钮的父物体
	/// </summary>
	public GameObject buttonGo;
    /// <summary>
    /// 每日必做
    /// </summary>
    public UIButton btnDaily;
    /// <summary>
    /// 挑战BOSS
    /// </summary>
    public UIButton btnChallengeBoss;
    #region 玩法
    /// <summary>
    /// 玩法
    /// </summary>
    public UIButton btnPlayMethod;
    /// <summary>
    /// 无尽
    /// </summary>
    public UIButton btnEndless;
    /// <summary>
    /// 副本
    /// </summary>
    public UIButton btnDungeon;
    /// <summary>
    /// 活动
    /// </summary>
    public UIButton btnActivity;

    /// <summary>
    /// 竞技场
    /// </summary>
    public UIButton btnArena;
    /// <summary>
    /// 修行
    /// </summary>
    public UIButton btnPractice;
    /// <summary>
    /// 镇魔塔
    /// </summary>
    public UIButton btnTower;
    /// <summary>
    /// 环式任务
    /// </summary>
    public UIButton btnRingTask;
    /// <summary>
    /// 挂机副本按钮
    /// </summary>
    public UIButton btnHangUp;
    #endregion

    #region 奖励
    /// <summary>
    /// 下载奖励
    /// </summary>
    public UIButton btnDownReward;
    /// <summary>
    /// 打开奖励
    /// </summary>
    public GameObject openRewardBtn;
    /// <summary>
    /// 点击福利打开的界面
    /// </summary>
    public GameObject openGo;
    /// <summary>
    /// 奖励
    /// </summary>
    public UIButton btnReward;
    /// <summary>
    /// 铸魂
    /// </summary>
    public UIButton btnZhuhun;
    /// <summary>
    /// 福利
    /// </summary>
    public UIButton btnWelfare;
    /// <summary>
    /// 红点提示
    /// </summary>
    public Transform welfaleRedRemind;
    /// <summary>
    /// 等级
    /// </summary>
    public UIButton btnLevReward;
    /// <summary>
    /// 在线奖励时间
    /// </summary>
    public UITimer OnlineRewardTimer;

    public UITimer OnlineRewardOutTimer;
    public GameObject canGetOnlineReward;




    #endregion 
    /// <summary>
    /// 节日活动
    /// </summary>
    public UIButton btnHoliday;
    /// <summary>
    /// 精彩活动
    /// </summary>
    public UIButton btnWonderfulActivity;
    /// <summary>
    /// 开服活动
    /// </summary>
    public UIButton btnOpenServerActivity;
    /// <summary>
    /// 合服活动
    /// </summary>
    public UIButton btnCombineServerActivity;
    /// <summary>
    /// 开服贺礼
    /// </summary>
    public UIButton btnOpenServerGift;


    /// <summary>
    /// 七天
    /// </summary>
    public UIButton btnSevenDay;
    /// <summary>
    /// 藏宝库
    /// </summary>
    public UIButton btnHideTreasrue;
	/// <summary>
	/// 首充大礼
	/// </summary>
	public UIButton btnFirstCharge;
    /// <summary>
    /// 优惠充值
    /// </summary>
    public UIButton btnPrivilege;
    /// <summary>
    /// 市场
    /// </summary>
    public UIButton btnMarket;
    /// <summary>
    /// 商城
    /// </summary>
    public UIButton btnMall;
    /// <summary>
    /// 商店
    /// </summary>
    public UIButton btnStore;
    /// <summary>
    /// 仙盟商店
    /// </summary>
    public UIButton btnGuildStore;
    /// <summary>
    /// 城内商店
    /// </summary>
    public UIButton btnSiegeStore;
    /// <summary>
    /// 皇室宝箱
    /// </summary>
    public UIButton royalBoxBtn;
    private MainPlayer mainPlayer;
    /// <summary>
    /// 宝藏
    /// </summary>
    public UIButton btnTreasure;
    /// <summary>
    /// 宝藏红点
    /// </summary>
    public GameObject treasureRedPoint;
    public TweenPosition treasureTweenPos;
    public TweenScale treasureTweenScale;
    /// <summary>
    /// 每日首充返利
    /// </summary>
    public UIButton dailyFirstRecharge;
    #region 入口部分
    public GameObject wanfaGo;
    public GameObject jiangliGo;
    #endregion
    #region 小地图部分

    public GameObject targePointObj;

    public GameObject mapObj;

    public UILabel manName;
    /// <summary>
    /// 小地图父控件原始坐标 
    /// </summary>
    protected Vector2 mapCtrlOriginPos;
    /// <summary>
    /// 小地图边框尺寸
    /// </summary>
    protected Vector3 mapFrameScale;
    /// <summary>
    /// 小地图边框
    /// </summary>
    public UIPanel mapFrame;
    /// <summary>
    /// 小地图父控件
    /// </summary>
    protected Transform mapCtrl;
    /// <summary>
    /// 小地图图片
    /// </summary>
    public UITexture mapTex2D;
    /// <summary>
    /// 主角图标
    /// </summary>
    public Transform mainPlayerPoint;
    /// <summary>
    /// 怪物图标用例
    /// </summary>
    public GameObject mobPointInstance;
    /// <summary>
    /// NPC图标用例
    /// </summary>
    public GameObject npcPointInstance;
    /// <summary>
    /// 传送门图标用例
    /// </summary>
    public GameObject flyPointInstance;

    protected FDictionary mobPointDic = new FDictionary();
    protected FDictionary npcPointDic = new FDictionary();
    protected FDictionary flyPointDic = new FDictionary();

    private float limitXMax;
    private float limitXMin;
    private float limitYMax;
    private float limitYMin;

    private Vector3 playerOldPoint;

    #endregion
    void Awake()
    {
        if (dailyFirstRecharge != null) UIEventListener.Get(dailyFirstRecharge.gameObject).onClick = OpenDailyFirstRecharge;
        if (btnDaily != null) UIEventListener.Get(btnDaily.gameObject).onClick = OpenDaily;
        if (btnChallengeBoss != null) UIEventListener.Get(btnChallengeBoss.gameObject).onClick = OpenChallengeBoss;
        if (btnReward != null) UIEventListener.Get(btnReward.gameObject).onClick = OpenReward;
        if (btnPlayMethod != null) UIEventListener.Get(btnPlayMethod.gameObject).onClick = OpenPlayMethod; 
        if (btnEndless != null) UIEventListener.Get(btnEndless.gameObject).onClick = OpenEndless;
        if (btnDungeon != null) UIEventListener.Get(btnDungeon.gameObject).onClick = OpenDungeon;
        if (btnActivity != null) UIEventListener.Get(btnActivity.gameObject).onClick = OpenActivity;
        if (btnMall != null) UIEventListener.Get(btnMall.gameObject).onClick = OpenMall;
        if (btnArena != null) UIEventListener.Get(btnArena.gameObject).onClick = OpenArena;
        if (btnPractice != null) UIEventListener.Get(btnPractice.gameObject).onClick = OpenPractice;
        if (btnTower != null) UIEventListener.Get(btnTower.gameObject).onClick = OpenTower;
        if (btnHangUp != null) UIEventListener.Get(btnHangUp.gameObject).onClick = OpenHangUpCoppy;
        if (btnHoliday != null) UIEventListener.Get(btnHoliday.gameObject).onClick = OpenHoliday;
        if (btnWonderfulActivity != null) UIEventListener.Get(btnWonderfulActivity.gameObject).onClick = OpenWonderfulActivity;
        if (btnOpenServerActivity != null) UIEventListener.Get(btnOpenServerActivity.gameObject).onClick = OpenOpenServerActivity;
        if (btnCombineServerActivity != null) UIEventListener.Get(btnCombineServerActivity.gameObject).onClick = OpenCombineServerActivity;
        if (btnOpenServerGift != null) UIEventListener.Get(btnOpenServerGift.gameObject).onClick = OpenOpenServerGift;
        if (btnPrivilege != null) UIEventListener.Get(btnPrivilege.gameObject).onClick = OpenPrivilege;
        if (btnDownReward != null) UIEventListener.Get(btnDownReward.gameObject).onClick = OnClickDownBtn;
        if (btnSevenDay != null) UIEventListener.Get(btnSevenDay.gameObject).onClick = OpenSevenDay;
        if (btnHideTreasrue != null) UIEventListener.Get(btnHideTreasrue.gameObject).onClick = OpenHideTreasrue;
        if (btnStore != null) UIEventListener.Get(btnStore.gameObject).onClick =OpenStore;
        if (btnGuildStore != null) UIEventListener.Get(btnGuildStore.gameObject).onClick = OpenGuildStore;
        if (btnSiegeStore != null) UIEventListener.Get(btnSiegeStore.gameObject).onClick = OpenSiegeStore;
        if (btnZhuhun != null) UIEventListener.Get(btnZhuhun.gameObject).onClick =OpenZhuhun;
        if (btnWelfare != null) UIEventListener.Get(btnWelfare.gameObject).onClick =OpenWelfare;
	    if(btnFirstCharge != null)UIEventListener.Get(btnFirstCharge.gameObject).onClick = OpenFirstChargeWnd;
	    if (btnLevReward != null) UIEventListener.Get(btnLevReward.gameObject).onClick = OpenLevReward;
        if (mapObj != null) UIEventListener.Get(mapObj.gameObject).onClick = OpenLargeMap; 
		if(btnMapMenu != null)UIEventListener.Get(btnMapMenu.gameObject).onClick = ClickMapMenuBtn;
        if (btnMarket != null) UIEventListener.Get(btnMarket.gameObject).onClick = OpenMarket;
        if (openRewardBtn != null) UIEventListener.Get(openRewardBtn).onClick = OpenRewardGame;
        if (royalBoxBtn != null) UIEventListener.Get(royalBoxBtn.gameObject).onClick = OnClickRoyalBoxBtn;
        if (btnTreasure != null) UIEventListener.Get(btnTreasure.gameObject).onClick = OpenTreasure;
        if (btnRingTask != null) UIEventListener.Get(btnRingTask.gameObject).onClick = OpenRingTask;
        if (treasureRedPoint != null) treasureRedPoint.gameObject.SetActive(GameCenter.treasureTroveMng.RedPoint);
        mapTex2D.mainTexture = GameCenter.cameraMng.curLogicMapTex2D;
        Vector3 scale = Vector3.zero;
        if (mapTex2D.mainTexture != null)
        {
            scale = new Vector3(mapTex2D.mainTexture.width * Mathf.PI / 2.0f, mapTex2D.mainTexture.height * Mathf.PI / 2.0f, 1);
        } 
        mapTex2D.transform.localScale = scale;
        mapTex2D.transform.localPosition = new Vector3(scale.x, scale.y, 0);
        mapCtrl = mapTex2D.transform.parent;
        limitXMax = mapCtrl.localPosition.x;
        limitXMin = mapCtrl.localPosition.x - scale.x*2.0f+165f;
        limitYMax = mapCtrl.localPosition.y;
        limitYMin = mapCtrl.localPosition.y - scale.y*2.0f+135f;
        mapCtrlOriginPos = mapCtrl.localPosition;
        mapFrameScale = mapFrame.GetViewSize();
        mainPlayer = GameCenter.curMainPlayer;
        playerOldPoint = mainPlayerPoint.localPosition;
        
    } 
    
	void ClickMapMenuBtn(GameObject go)
	{
		GameCenter.uIMng.SetMapMenuState();
        if (GameCenter.uIMng.MapMenuActive)
        {
            if (menuGrid != null)
            {
                for (int i = 0, length = menuGrid.Length; i < length; i++)
                {
                    menuGrid[i].repositionNow = true;
                }
            }
        }
        else
        {
            if (menuTweens != null)
            {
                for (int i = 0, length = menuTweens.Length; i < length; i++)
                {
                    if (menuTweens[i] != null)
                    {
                        //menuTweens[i].ResetToBeginning();//这里此方法会导致快速点击的时候,按钮错乱
                        menuTweens[i].PlayForward();
                    }
                }
            }
        }
	}

    void OpenDailyFirstRecharge(GameObject go)
    {
        GameCenter.uIMng.SwitchToUI(GUIType.DAILYFIRSTRECHARGE);
    }

    void OpenDaily(GameObject go)
    {
		GameCenter.uIMng.SwitchToUI(GUIType.DAILYMUSTDO);
    }
    void OpenChallengeBoss(GameObject go)
    {
        GameCenter.uIMng.SwitchToUI(GUIType.BOSSCHALLENGE);
    }
    void OpenReward(GameObject go)
    {
        GameCenter.uIMng.SwitchToUI(GUIType.ONLINEREWARD);
    }
    void OpenStore(GameObject go)
    {
        GameCenter.uIMng.SwitchToUI(GUIType.SHOPWND);
    }
    void OpenGuildStore(GameObject go)
    {
        if (GameCenter.mainPlayerMng.MainPlayerInfo.HavaGuild == false)
        {
            GameCenter.messageMng.AddClientMsg(235);
        }
        else
        {
            GameCenter.guildMng.NeedOpenGuildWnd = false;
            GameCenter.uIMng.SwitchToUI(GUIType.GUILDSHOP);
        }
    }
    void OpenSiegeStore(GameObject go)
    {
    //    GameCenter.uIMng.SwitchToUI(GUIType.GUILDSIEGE);
        if (GameCenter.mainPlayerMng.MainPlayerInfo.HavaGuild == false)
        {
            GameCenter.messageMng.AddClientMsg(235);
        }
        else
        {
            GameCenter.guildMng.NeedOpenGuildWnd = false;
            GameCenter.uIMng.SwitchToSubUI(SubGUIType.GUILDCITYSTORE);
        }
    }
    void OpenZhuhun(GameObject go)
    {
        if (openGo != null) openGo.SetActive(false);
        GameCenter.uIMng.SwitchToUI(GUIType.CASTSOUL);
    }
    void OnClickDownBtn(GameObject go)
    {
        GameCenter.uIMng.SwitchToUI(GUIType.DOWNLOADBONUS);
    }
    void OpenWelfare(GameObject go)
    {
        GameCenter.uIMng.SwitchToUI(GUIType.EVERYDAYREWARD);
    }
    void OpenLevReward(GameObject go)
    { 
        GameCenter.uIMng.SwitchToUI(GUIType.RANKREWARD);
    }
    void OpenPlayMethod(GameObject go)
    {

    }
    void OpenRewardGame(GameObject go)//打开关闭奖励
    {
        if (openGo != null) openGo.SetActive(!openGo.activeSelf);
    } 
    void OpenFirstChargeWnd(GameObject go)
    {
	GameCenter.uIMng.SwitchToUI(GUIType.FIRSTCHARGEBONUS);
    }
    void OpenEndless(GameObject go)
    {
        if (wanfaGo != null) wanfaGo.SetActive(false);
        GameCenter.uIMng.SwitchToUI(GUIType.ENDLESSWND);
    }
    void OpenDungeon(GameObject go)
    {
        GameCenter.uIMng.SwitchToUI(GUIType.COPYINWND);
    }
    void OpenActivity(GameObject go)
    {
        GameCenter.uIMng.SwitchToUI(GUIType.ATIVITY);
    }
    void OpenMall(GameObject go)
    {
        GameCenter.newMallMng.CurMallType = MallItemType.RESTRICTION;
        GameCenter.uIMng.SwitchToUI(GUIType.NEWMALL);
    }
    void OpenMarket(GameObject go)
    {
        GameCenter.uIMng.SwitchToUI(GUIType.MARKET);
    }
    void OpenArena(GameObject go)
    { 
        GameCenter.uIMng.SwitchToUI(GUIType.ARENE);
    }
    void OpenPractice(GameObject go)
    {
        if (openGo != null) openGo.SetActive(false);
		GameCenter.uIMng.SwitchToUI(GUIType.PRACTICE);
    }
    void OpenTower(GameObject go)
    {
        GameCenter.uIMng.SwitchToUI(GUIType.MagicTowerWnd);
    }
    void OpenRingTask(GameObject go)
    {
        GameCenter.uIMng.SwitchToUI(GUIType.RINGTASKTYPE);
    }
    void OpenHangUpCoppy(GameObject go)
    {
        GameCenter.uIMng.SwitchToUI(GUIType.HANGUPCOPPY);
    } 
    void OpenHoliday(GameObject go)
    {
        GameCenter.wdfActiveMng.CurWdfActiveMainType = WdfType.HOLIDAY;
        GameCenter.uIMng.SwitchToUI(GUIType.WDFACTIVE);
    }
    void OpenWonderfulActivity(GameObject go)
    {
        GameCenter.wdfActiveMng.CurWdfActiveMainType = WdfType.WONDERFUL;
        GameCenter.uIMng.SwitchToUI(GUIType.WDFACTIVE);
    }
    void OpenCombineServerActivity(GameObject go)
    {
        GameCenter.wdfActiveMng.CurWdfActiveMainType = WdfType.COMBINE;
        GameCenter.uIMng.SwitchToUI(GUIType.WDFACTIVE);
    }
    void OpenOpenServerGift(GameObject go)
    {
        GameCenter.openServerRewardMng.isAccord = true;
        GameCenter.uIMng.SwitchToUI(GUIType.OPENSERVER);
    }
    void OpenOpenServerActivity(GameObject go)
    {
        GameCenter.wdfActiveMng.isAccord = true;
        GameCenter.wdfActiveMng.CurWdfActiveMainType = WdfType.OPEN;
        GameCenter.uIMng.SwitchToUI(GUIType.WDFACTIVE);
    }
    void OpenSevenDay(GameObject go)
    {
        if (jiangliGo != null) jiangliGo.SetActive(false);
        GameCenter.uIMng.SwitchToUI(GUIType.SEVENDAYREWARD);
    }
    void OpenHideTreasrue(GameObject go)
    {
        if (openGo != null) openGo.SetActive(false);
        GameCenter.uIMng.SwitchToUI(GUIType.TREASUREHOUSE);
    }
    void OpenLargeMap(GameObject go)
    {
        GameCenter.uIMng.SwitchToUI(GUIType.LARGEMAP);
    }
    /// <summary>
    /// 打开优惠充值界面，包括充值优惠、爱心礼包。周卡充值
    /// </summary>
    /// <param name="go"></param>
    void OpenPrivilege(GameObject go)
    {
        GameCenter.uIMng.SwitchToUI(GUIType.PRIVILEGE);
    }
    void OnClickRoyalBoxBtn(GameObject go)
    {
        GameCenter.uIMng.SwitchToUI(GUIType.ROYALBOXWND);
    }
    void OpenTreasure(GameObject _obj)
    {
        GameCenter.uIMng.GenGUI(GUIType.TREASURETROVE,true);
    }

    void SetDailyFirstRechargeOpen()
    { 
        if (dailyFirstRecharge != null)
        {
            if (GameCenter.openServerRewardMng.reminTime > 0 && GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel >= 8)
            {
                dailyFirstRecharge.gameObject.SetActive(true);
            }
            else
            {
                dailyFirstRecharge.gameObject.SetActive(false);
            }
        }
    }

    void SetDailyFirstRechargeOpen(ActorBaseTag tag, ulong value, bool _fromAbility)
    {
        if (tag == ActorBaseTag.Level)
        {
            SetDailyFirstRechargeOpen();
        }
    }

    /// <summary>
    /// 宝藏活动开启
    /// </summary>
    void TreasureOpen()
    {
        //Debug.Log("GameCenter.treasureTroveMng.IsOpen："+ GameCenter.treasureTroveMng.IsOpen);
        if(btnTreasure.gameObject!=null)
        {
            if (GameCenter.treasureTroveMng.IsOpen)
            {
                btnTreasure.gameObject.SetActive(true);
                GameCenter.treasureTroveMng.C2S_ReqTreasurebigPrize();
            }
            else
                btnTreasure.gameObject.SetActive(false);
            if (menuGrid != null)
            {
                for (int i = 0, length = menuGrid.Length; i < length; i++)
                {
                    menuGrid[i].repositionNow = true;
                }
            }
        }
    }
    /// <summary>
    /// 宝藏活动红点
    /// </summary>
    void treasureActivityRedPoint()
    {
        if(treasureRedPoint!=null)
        {
            if (GameCenter.treasureTroveMng.RedPoint)
                treasureRedPoint.gameObject.SetActive(true);
            else
                treasureRedPoint.gameObject.SetActive(false);
        }       
    }

    protected override void OnOpen()
    {
        base.OnOpen();
        Invoke("InitMap", 0.3f);
        RefreshFirstChargeBtn();
        RefreshLoveReward();
        TreasureOpen();
        SetDailyFirstRechargeOpen();
        SceneUiType uiType = GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneUiType;
        if (buttonGo != null) buttonGo.SetActive(uiType != SceneUiType.BATTLEFIGHT && uiType != SceneUiType.GUILDFIRE && uiType != SceneUiType.LIRONGELAND && uiType != SceneUiType.GUILDPROTECT
			&& uiType != SceneUiType.GUILDWAR && uiType != SceneUiType.GODSWAR && uiType != SceneUiType.UNDERBOSS && uiType != SceneUiType.SEALBOSS && uiType != SceneUiType.RAIDERARK);
        RefreshCastSoulRed();
        //GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.CASTINGSOUL, GameCenter.castSoulMng.GetCastSoulRed());
        RefreshOnlineRewardTime();
        RefreshWdfActive();
        RefreshSevendayActive();
        if (GameCenter.treasureTroveMng.IsOpen)
        {
            GameCenter.treasureTroveMng.C2S_ReqTreasurebigPrize();
        }
        GameCenter.sevenChallengeMng.C2S_ReqSevenChallengeInfo(1, 0);
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
            if (mainPlayer == null) mainPlayer = GameCenter.curMainPlayer;
            //mainPlayer.OnTargetChange += ChangeTarget;
            mainPlayer.onSectorChanged += OnMainPlayerPositionChange;
            // GameCenter.mailBoxMng.ContainerMailIsnotRewardEvent += SetMailBoxTip;
            SceneMng.OnMobInfoListUpdate += OnAddMonster;
            SceneMng.OnNPCInfoListUpdate += OnAddNpcs;
            SceneMng.OnDelInterObj += OnDeleteObj; 
            //GameCenter.dungeonMng.OnDungeonMatchUpdate += DungeonMatchRef;
            //GameCenter.dungeonMng.OnStoptMatchUpdate += DungeonMatchRef;
            //GameCenter.dungeonMng.OnMatchReadyCancleUpdate += DungeonMatchRef;
            //GameCenter.dungeonMng.OnDungeonTimeUpdate += RefreshDungeonTime;
            //GameCenter.dungeonMng.OnExpTrialWaveUpdate += RefExpTrialWave;
            //GameCenter.commonRewardMng.CommonRewardIsCanGetEvent += SetCommondRewardTip;
            //GameCenter.adventureMng.SetAdventureCanDoDicEvent += SetAdventureTip;
            //GameCenter.chatMng.ChatNumChangeEvent += SetChatTip;
            //GameCenter.inventoryMng.OnBackpackUpdate += SetJackpotTip;
            //GameCenter.mainPlayerMng.UpdateOpenFun += SysOpenFunction;
            GameCenter.firstChargeBonusMng.OnFirstChargeBonus += RefreshFirstChargeBtn;
            GameCenter.lovePackageMng.OnOpenLoveRechargeUpdate += RefreshLoveReward;
            GameCenter.swornMng.OnOpenSwornUpdate += OpenSwornWnd;
            GameCenter.onlineRewardMng.OnGetOnlineRewardInfo += RefreshOnlineRewardTime;
            GameCenter.vipMng.OnVIPDataUpdate += GameCenter.sevenDayMng.SetRedPoint;
            GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate += GameCenter.newMountMng.SetRedRemind;
            GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate += GameCenter.practiceMng.SetPracticeRedMind;
            GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate += GameCenter.practiceMng.SetSoarRedMind;
            GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate += SetDailyFirstRechargeOpen;
            GameCenter.wdfActiveMng.RefreshShow += RefreshWdfActive;
            GameCenter.openServerRewardMng.OnGetAllOpenServerInfo += RefreshWdfActive;
            GameCenter.taskMng.TryTrace += RefreshTargetPoint;
            GameCenter.curMainPlayer.CannotMoveTo += HideTargetPoin;
            GameCenter.curMainPlayer.OnMoveStart += ShowTargetPoin;
            GameCenter.castSoulMng.UpdateSoulNum += RefreshCastSoulRed;
            GameCenter.treasureTroveMng.treasureOpen += TreasureOpen;
            GameCenter.treasureTroveMng.updateTreasurebigPrize += treasureActivityRedPoint;
            GameCenter.openServerRewardMng.OnDailyRechargeUpdate += SetDailyFirstRechargeOpen;
            GameCenter.royalTreasureMng.OnGetNewTreasureBoxEvent += ShowTreasureFx;
        }
        else
        {
            //mainPlayer.OnTargetChange -= ChangeTarget;
            mainPlayer.onSectorChanged -= OnMainPlayerPositionChange;
            mainPlayer = null;
            //GameCenter.mailBoxMng.ContainerMailIsnotRewardEvent -= SetMailBoxTip;
            SceneMng.OnMobInfoListUpdate -= OnAddMonster;
            SceneMng.OnNPCInfoListUpdate -= OnAddNpcs;
            SceneMng.OnDelInterObj -= OnDeleteObj;
            //GameCenter.dungeonMng.OnDungeonMatchUpdate -= DungeonMatchRef;
            //GameCenter.dungeonMng.OnStoptMatchUpdate -= DungeonMatchRef;
            //GameCenter.dungeonMng.OnMatchReadyCancleUpdate -= DungeonMatchRef;
            //GameCenter.dungeonMng.OnDungeonTimeUpdate -= RefreshDungeonTime;
            //GameCenter.dungeonMng.OnExpTrialWaveUpdate -= RefExpTrialWave;
            //GameCenter.commonRewardMng.CommonRewardIsCanGetEvent -= SetCommondRewardTip;
            //GameCenter.adventureMng.SetAdventureCanDoDicEvent -= SetAdventureTip;
            //GameCenter.chatMng.ChatNumChangeEvent -= SetChatTip;
            //GameCenter.inventoryMng.OnBackpackUpdate -= SetJackpotTip;
            //GameCenter.mainPlayerMng.UpdateOpenFun -= SysOpenFunction;
            GameCenter.firstChargeBonusMng.OnFirstChargeBonus -= RefreshFirstChargeBtn; 
            GameCenter.lovePackageMng.OnOpenLoveRechargeUpdate -= RefreshLoveReward;
            GameCenter.swornMng.OnOpenSwornUpdate -= OpenSwornWnd;
            GameCenter.onlineRewardMng.OnGetOnlineRewardInfo -= RefreshOnlineRewardTime;
            GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate -= GameCenter.newMountMng.SetRedRemind;
            GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate -= GameCenter.practiceMng.SetPracticeRedMind;
            GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate -= GameCenter.practiceMng.SetSoarRedMind;
            GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate -= SetDailyFirstRechargeOpen;
            GameCenter.wdfActiveMng.RefreshShow -= RefreshWdfActive;
            GameCenter.openServerRewardMng.OnGetAllOpenServerInfo -= RefreshWdfActive;
            GameCenter.taskMng.TryTrace -= RefreshTargetPoint;
            GameCenter.vipMng.OnVIPDataUpdate -= GameCenter.sevenDayMng.SetRedPoint;
            GameCenter.curMainPlayer.CannotMoveTo -= HideTargetPoin;
            GameCenter.curMainPlayer.OnMoveStart -= ShowTargetPoin;
            GameCenter.castSoulMng.UpdateSoulNum -= RefreshCastSoulRed;
            GameCenter.treasureTroveMng.treasureOpen -= TreasureOpen;
            GameCenter.treasureTroveMng.updateTreasurebigPrize -= treasureActivityRedPoint;
            GameCenter.openServerRewardMng.OnDailyRechargeUpdate -= SetDailyFirstRechargeOpen;
            GameCenter.royalTreasureMng.OnGetNewTreasureBoxEvent -= ShowTreasureFx;
        }
    }



    void RefreshWdfActive()
    {
        GameCenter.mainPlayerMng.SetServerActiveOpen(FunctionType.OPENSERVER, GameCenter.wdfActiveMng.isGiftOpen);
        GameCenter.mainPlayerMng.SetServerActiveOpen(FunctionType.FESTIVAL, GameCenter.wdfActiveMng.isHolidayOpen);
        GameCenter.mainPlayerMng.SetServerActiveOpen(FunctionType.WONDERFUL, GameCenter.wdfActiveMng.isWdfOpen);
        GameCenter.mainPlayerMng.SetServerActiveOpen(FunctionType.OPENACTIVE, GameCenter.wdfActiveMng.isOpenOpen);
        GameCenter.mainPlayerMng.SetServerActiveOpen(FunctionType.COMBINESERVER, GameCenter.wdfActiveMng.isCombineOpen);
    }

    void RefreshSevendayActive()
    {
        if (GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel < 14)
            GameCenter.mainPlayerMng.SetServerActiveOpen(FunctionType.SEVENDAYS, false);
        if (!GameCenter.sevenDayMng.notGetAll)
        {
            GameCenter.sevenDayMng.RefreshSevenOpen();
        }
    }
    void RefreshOnlineRewardTime()
    {
        if (OnlineRewardOutTimer == null || OnlineRewardTimer == null)
            return;
        if (OnlineRewardTimer != null)
        {
            if (!GameCenter.onlineRewardMng.IsGetedAll)
            {
                if (GameCenter.onlineRewardMng.TrueCurInfo != null)
                {
                    int remainTime = GameCenter.onlineRewardMng.TrueCurInfo.RemainTime - ((int)Time.time - GameCenter.onlineRewardMng.TrueCurInfo.ReceiveTime);
                    if (remainTime > 0)
                    {
                        OnlineRewardTimer.StartIntervalTimer(remainTime);
                        OnlineRewardTimer.gameObject.SetActive(true);
                        OnlineRewardOutTimer.StartIntervalTimer(remainTime);
                        if (canGetOnlineReward != null)
                            canGetOnlineReward.SetActive(false);
                        GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.ONLINEREWARDS, false);
                    }
                    else
                    {
                        OnlineRewardTimer.StartIntervalTimer(0);
                        OnlineRewardTimer.gameObject.SetActive(false);
                        OnlineRewardOutTimer.StartIntervalTimer(0);
                        if (canGetOnlineReward != null)
                            canGetOnlineReward.SetActive(true);
                        GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.ONLINEREWARDS, true);
                    }
                }
                else
                {
                    OnlineRewardTimer.StartIntervalTimer(0);
                    OnlineRewardTimer.gameObject.SetActive(false);
                    OnlineRewardOutTimer.StartIntervalTimer(0);
                    GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.ONLINEREWARDS, false);
                }
            }
            else
            {
                OnlineRewardTimer.gameObject.SetActive(false);
                GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.ONLINEREWARDS, false);
            }
             
                OnlineRewardOutTimer.onTimeOut = (x) =>
                {
                    if (canGetOnlineReward != null)
                        canGetOnlineReward.SetActive(true);
                    OnlineRewardTimer.gameObject.SetActive(false);
                    GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.ONLINEREWARDS, true);
                };
        }

    }
    /// <summary>
    /// 领取首冲后隐藏入口
    /// </summary>
    void RefreshFirstChargeBtn()
    {
        if (GameCenter.firstChargeBonusMng.firstChargeBonusStates == (int)FirstChargeState.HAVECHARGEGET)
        { 
            if (GameCenter.mainPlayerMng != null)
            {
                int prof = GameCenter.mainPlayerMng.MainPlayerInfo.Prof;
                int stage = GameCenter.lovePackageMng.stage;
                LoveSpreeRef love = ConfigMng.Instance.GetLoveSpreeRef(prof, stage);
                if (love != null)
                {
                    GameCenter.lovePackageMng.isOpenLoveReward = true;
                    GameCenter.lovePackageMng.SetLoveRedRemind();
                }
            }
            if (btnFirstCharge != null)
                GameCenter.mainPlayerMng.SetServerActiveOpen(FunctionType.FIRSTCHARGE,false);
            //开启理财周卡  
            if (btnPrivilege != null) btnPrivilege.gameObject.SetActive(true);
           // GameCenter.mainPlayerMng.SetServerActiveOpen(FunctionType.PRIVILEGE, true); 
        } 
        else
        { 
            GameCenter.lovePackageMng.isOpenLoveReward = false;
            if (btnPrivilege != null) btnPrivilege.gameObject.SetActive(false);
            //GameCenter.mainPlayerMng.SetServerActiveOpen(FunctionType.PRIVILEGE, false); 
        }
    }
    /// <summary>
    /// 爱心礼包开启
    /// </summary>
    void RefreshLoveReward()
    { 
        //CancelInvoke("openLove");
        //if (GameCenter.lovePackageMng.isOpenLoveReward)
        //{
        //    //if (btnPrivilege != null)
        //    //{
        //    //    GameCenter.mainPlayerMng.SetServerActiveOpen(FunctionType.LOVEREWARD, true);
        //    //    GameCenter.lovePackageMng.SetLoveRedRemind();
        //    //} 
        //    //if (GameCenter.lovePackageMng.isCloseFirstBonus)
        //    //{ 
        //    //    Invoke("openLove", 0.5f);
        //    //} 
        //}
        //else
        //{
        //    if (btnPrivilege != null)
        //    {
        //        GameCenter.mainPlayerMng.SetServerActiveOpen(FunctionType.LOVEREWARD, false);
        //    }
        //}
    }
    //void openLove()
    //{
    //    GameCenter.lovePackageMng.isCloseFirstBonus = false;
    //    GameCenter.uIMng.SwitchToUI(GUIType.PRIVILEGE); 
    //}
    protected void OnAddMonster()
    {
        List<Monster> mobs = GameCenter.curGameStage.GetMobs();
        for (int i = 0; i < mobs.Count; i++)
        {
            Monster instance = mobs[i];
            if (mobPointDic.ContainsKey(instance.id) || instance.isDead) continue;
            GameObject myPoint = Instantiate(mobPointInstance) as GameObject;
            myPoint.transform.parent = mapCtrl;
            myPoint.transform.localScale = Vector3.one;
            myPoint.transform.localPosition = new Vector3(instance.curSector.c * Mathf.PI, instance.curSector.r * Mathf.PI, 0);
            SmartActorMapPoint p = myPoint.AddComponent<SmartActorMapPoint>();
            p.SetTarget(instance);
            mobPointDic.Add(instance.id, myPoint);
        }
    }


    protected void OnAddNpcs()
    {
        List<NPC> npcs = GameCenter.curGameStage.GetNPCs();
        for (int i = 0; i < npcs.Count; i++)
        {
            NPC instance = npcs[i];
            if (npcPointDic.ContainsKey(instance.id) || instance.isDead) continue;
            GameObject myPoint = Instantiate(npcPointInstance) as GameObject;
            myPoint.transform.parent = mapCtrl;
            myPoint.transform.localScale = Vector3.one;
            myPoint.transform.localPosition = new Vector3(instance.curSector.c * Mathf.PI, instance.curSector.r * Mathf.PI, 0);
            SmartActorMapPoint p = myPoint.AddComponent<SmartActorMapPoint>();
            p.SetTarget(instance);
            npcPointDic.Add(instance.id, myPoint);
        }
    }

    protected void OnDeleteObj(ObjectType _type, int _instanceID)
    {
        switch (_type)
        {
            case ObjectType.Player:
                break;
            case ObjectType.MOB:
                if (mobPointDic.ContainsKey(_instanceID))
                {
                    Destroy(mobPointDic[_instanceID] as GameObject);
                    mobPointDic.Remove(_instanceID);
                }
                break;
            case ObjectType.NPC:
                if (npcPointDic.ContainsKey(_instanceID))
                {
                    Destroy(npcPointDic[_instanceID] as GameObject);
                    npcPointDic.Remove(_instanceID);
                }
                break;
            case ObjectType.FlyPoint:
                break;
            default:
                break;
        }
    }





    protected void InitMap()
    {
        if (manName != null)
        {
            manName.text = GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneRef.name;
        }
        if (mapCtrl != null) mapCtrl.localPosition = new Vector3(mapCtrlOriginPos.x - mainPlayer.curSector.c * Mathf.PI + mapFrameScale.x / 2.0f, mapCtrlOriginPos.y - mainPlayer.curSector.r * Mathf.PI + mapFrameScale.y / 2.0f, 0);
        List<Monster> mobs = GameCenter.curGameStage.GetMobs();
        for (int i = 0; i < mobs.Count; i++)
        {
            Monster instance = mobs[i];
            if (mobPointDic.ContainsKey(instance.id) || instance.isDead) continue;
            GameObject myPoint = Instantiate(mobPointInstance) as GameObject;
            myPoint.transform.parent = mapCtrl;
            myPoint.transform.localScale = Vector3.one;
            myPoint.transform.localPosition = new Vector3(instance.curSector.c * Mathf.PI, instance.curSector.r * Mathf.PI, 0);
            SmartActorMapPoint p = myPoint.AddComponent<SmartActorMapPoint>();
            p.SetTarget(instance);
            mobPointDic.Add(instance.id, myPoint);
        }
        List<NPC> npcs = GameCenter.curGameStage.GetNPCs();
        for (int i = 0; i < npcs.Count; i++)
        {
            NPC instance = npcs[i];
            if (npcPointDic.ContainsKey(instance.id)) continue;
            GameObject myPoint = Instantiate(npcPointInstance) as GameObject;
            myPoint.transform.parent = mapCtrl;
            myPoint.transform.localScale = Vector3.one;
            myPoint.transform.localPosition = new Vector3(instance.curSector.c * Mathf.PI, instance.curSector.r * Mathf.PI, 0);
            SmartActorMapPoint p = myPoint.AddComponent<SmartActorMapPoint>();
            p.SetTarget(instance);
            npcPointDic.Add(instance.id, myPoint);
        }
        List<FlyPoint> flys = GameCenter.curGameStage.GetFlypoints();
        for (int i = 0; i < flys.Count; i++)
        {
            FlyPoint instance = flys[i];
            if (flyPointDic.ContainsKey(instance.id)) continue;
            GameObject myPoint = Instantiate(flyPointInstance) as GameObject;
            myPoint.transform.parent = mapCtrl;
            myPoint.transform.localScale = Vector3.one;
            myPoint.transform.localPosition = new Vector3(instance.curSector.c * Mathf.PI, instance.curSector.r * Mathf.PI, 0);
            flyPointDic.Add(instance.id, myPoint);
        }
        LimitMapCtr();
    }
    

    /// <summary>
    /// 主角移动,小地图移动
    /// </summary>
    /// <param name="_old"></param>
    /// <param name="_new"></param>
    protected void OnMainPlayerPositionChange(GameStage.Sector _old, GameStage.Sector _new)
    {
        mapCtrl.localPosition = new Vector3(mapCtrlOriginPos.x - _new.c * Mathf.PI + mapFrameScale.x / 2.0f, mapCtrlOriginPos.y - _new.r * Mathf.PI + mapFrameScale.y / 2.0f, 0);
        LimitMapCtr();

        // RefreshPos(_new.c, _new.r);
    }

    void LimitMapCtr()
    {
        if (mapCtrl != null)
        {
            if (mainPlayerPoint != null)
                mainPlayerPoint.localPosition = playerOldPoint;
            if (mapCtrl.localPosition.x > limitXMax)
            {
                float num = mapCtrl.localPosition.x - limitXMax;
                if (mainPlayerPoint != null)
                {
                    mainPlayerPoint.localPosition = new Vector3(mainPlayerPoint.localPosition.x - num, mainPlayerPoint.localPosition.y, mainPlayerPoint.localPosition.z);
                }
                mapCtrl.localPosition = new Vector3(limitXMax, mapCtrl.localPosition.y, mapCtrl.localPosition.z);
            }
            if (mapCtrl.localPosition.x < limitXMin)
            {
                float num = limitXMin - mapCtrl.localPosition.x;
                if (mainPlayerPoint != null)
                    mainPlayerPoint.localPosition = new Vector3(mainPlayerPoint.localPosition.x +num, mainPlayerPoint.localPosition.y, mainPlayerPoint.localPosition.z);
                mapCtrl.localPosition = new Vector3(limitXMin, mapCtrl.localPosition.y, mapCtrl.localPosition.z);
            }
            if (mapCtrl.localPosition.y > limitYMax)
            {
                float num = mapCtrl.localPosition.y - limitYMax;
                if (mainPlayerPoint != null)
                    mainPlayerPoint.localPosition = new Vector3(mainPlayerPoint.localPosition.x, mainPlayerPoint.localPosition.y - num, mainPlayerPoint.localPosition.z);
                mapCtrl.localPosition = new Vector3(mapCtrl.localPosition.x, limitYMax, mapCtrl.localPosition.z);
            }
            if (mapCtrl.localPosition.y < limitYMin)
            {
                float num = limitYMin - mapCtrl.localPosition.y;
                if (mainPlayerPoint != null)
                    mainPlayerPoint.localPosition = new Vector3(mainPlayerPoint.localPosition.x, mainPlayerPoint.localPosition.y + num, mainPlayerPoint.localPosition.z);
                mapCtrl.localPosition = new Vector3(mapCtrl.localPosition.x, limitYMin, mapCtrl.localPosition.z);
            }

        }
    }

    void OpenSwornWnd()
    {
        if (GameCenter.swornMng.isOpenSworn == OpenType.SWORN)
        {
            GameCenter.swornMng.isOpenSworn = OpenType.NONE;
            GameCenter.uIMng.SwitchToSubUI(SubGUIType.SWORN);
        }
        //if (GameCenter.swornMng.isOpenSworn == OpenType.COUPLE)
        //{
        //    GameCenter.swornMng.isOpenSworn = OpenType.NONE;
        //    GameCenter.uIMng.SwitchToSubUI(SubGUIType.COUPLE);
        //}
    }

    void RefreshTargetPoint(int _sceneID, Vector3 _point)
    {
        if (_sceneID == GameCenter.mainPlayerMng.MainPlayerInfo.SceneID)
        {
            if (targePointObj != null && mapTex2D!=null)
            {
                targePointObj.transform.localPosition = new Vector3((mapTex2D.transform.localPosition.x *2/ GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneRef.sceneWidth) * _point.x, (mapTex2D.transform.localPosition.y*2 / GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneRef.sceneLength) * _point.z, targePointObj.transform.localPosition.z);
                targePointObj.SetActive(true);
            }

        }

    }

    void ShowTargetPoin(bool _b)
    {
        if (targePointObj != null&&!_b)
        {
            targePointObj.SetActive(false);
        }
    }

    void HideTargetPoin()
    {
        if (targePointObj != null)
        {
            targePointObj.SetActive(false);
        }
    }

    void RefreshCastSoulRed()
    {  
        CastsoulRewardRef castsoulRewardRef = ConfigMng.Instance.GetcastsoulRewardRef(GameCenter.castSoulMng.CurSoulRewardId + 1);
        bool isCastRed = false; 
        if (castsoulRewardRef != null)
        { 
            if (GameCenter.castSoulMng.CurSoulNum >= castsoulRewardRef.num)
            {
                isCastRed = true; 
            }  
        } 
        GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.CASTINGSOUL, isCastRed); 
    }

    void ShowTreasureFx()
    {
        if (treasureTweenPos != null)
        {
            treasureTweenPos.ResetToBeginning();
            treasureTweenPos.transform.position = Vector3.zero;//位置在屏幕正中间
            treasureTweenPos.from = treasureTweenPos.transform.localPosition;
            if (GameCenter.uIMng.MapMenuActive)
                treasureTweenPos.to = Vector3.zero;
            else
                treasureTweenPos.to = new Vector3(193, 75, 0);//缩到伸缩按钮位置
            treasureTweenPos.enabled = true;
        }
        if (treasureTweenScale != null)
        {
            treasureTweenScale.ResetToBeginning();
            treasureTweenScale.enabled = true;
        }
    }
}
