//==============================================
//作者：何明军
//日期：2015/8/13
//用途：人物栏
//=================================================



using UnityEngine;
using System.Collections;

public class CharacterWnd : GUIBase
{
    #region UI控件对象
    /// <summary>
    /// 头像按钮
    /// </summary>
    public GameObject CharacterButton;
    /// <summary>
    /// 血量
    /// </summary>
    public UILabel HpLable;
    /// <summary>
    /// 蓝量
    /// </summary>
    public UILabel MpLable;
    /// <summary>
    /// Vip按钮
    /// </summary>
    public GameObject VipButton;
    /// <summary>
    /// 充值按钮
    /// </summary>
    public GameObject RechargeButton;
    /// <summary>
    /// 人物血条
    /// </summary>
    public UIProgressBar CurPlayerHp;
    /// <summary>
    /// 人物蓝条
    /// </summary>
    public UIProgressBar CurPlayerMp;
    /// <summary>
    /// 主玩家头像
    /// </summary>
    public UISprite CurPlayerIcon;
    /// <summary>
    /// 主玩家等级
    /// </summary>
    public UILabel CurPlayerLv;
	/// <summary>
	/// 队长旗帜
	/// </summary>
	public GameObject teamLeaderFlag;

	public UIButton[] btnPkModes;
	public UILabel CurPlayerMode;
	public GameObject[] CurPkModes;
	public PkModelUI pkModelUI;
	public UILabel name;
	public UILabel fighting;

    /// <summary>
    /// 七日挑战入口
    /// </summary>
    public UIButton sevenChallenge;
    /// <summary>
    /// 七日挑战红点
    /// </summary>
    public GameObject sevenRedPoint;

    public OffLineRewardWnd offRewardWnd;
	
	public GameObject unShow;

    protected MainPlayerInfo actorInfo;
    public UISprite vipRedMind;
    protected VIPDataInfo VipData
    {
        get
        {
            return GameCenter.vipMng.VipData;
        }
    }
    #endregion

    void Awake()
    {
        actorInfo = GameCenter.mainPlayerMng.MainPlayerInfo;
        if (CharacterButton != null) UIEventListener.Get(CharacterButton.gameObject).onClick -= OnClickCharacterBtn;
        if (CharacterButton != null) UIEventListener.Get(CharacterButton.gameObject).onClick += OnClickCharacterBtn;
        if (VipButton != null) UIEventListener.Get(VipButton.gameObject).onClick =  OnClickVipBtn;
		ShowMyVip();
        if (RechargeButton != null) UIEventListener.Get(RechargeButton.gameObject).onClick -= OnClickRechargeBtn;
        if (RechargeButton != null) UIEventListener.Get(RechargeButton.gameObject).onClick += OnClickRechargeBtn;
		if(btnPkModes != null)
		{
			for (int i = 0,max=btnPkModes.Length; i < max; i++) 
			{
				if(btnPkModes[i] != null)
				{
					UIEventListener.Get(btnPkModes[i].gameObject).onClick = SetPkMode;
					UIEventListener.Get(btnPkModes[i].gameObject).parameter = i+1;
				}
			}
		}
        RefreshAll();
		if(unShow != null){
			unShow.SetActive(GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneUiType != SceneUiType.ARENA && GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneUiType != SceneUiType.BUDOKAI);
		}
		
		if(pkModelUI != null){
			pkModelUI.gameObject.SetActive(false);
		}
        if (sevenChallenge != null) UIEventListener.Get(sevenChallenge.gameObject).onClick = OpenSevenChallenge;
    }

    void Start()
    {

        if (GameCenter.mainPlayerMng != null)
        { 
            if (GameCenter.loginMng.isLogin)// && GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneType == SceneType.SCUFFLEFIELD)
            {
                bool isWedding = GameCenter.mainPlayerMng.MainPlayerInfo.IsHide;//花车巡游过程中不弹
                if (!isWedding && GameCenter.sevenDayMng.IsSevendDayTrue())
                {
                    if (GameCenter.rankRewardMng.isFirstPlay)
                    {
                        GameCenter.openServerRewardMng.isOpenFirstRecharge = true;
                    }
                    GameCenter.uIMng.SwitchToUI(GUIType.SEVENDAYREWARD);
                }
                else if (!isWedding && GameCenter.rankRewardMng.isEverdayRedRemind())
                { 
                    if (GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel >= 15)
                    {
                        if (GameCenter.rankRewardMng.isFirstPlay)
                        {
                            GameCenter.openServerRewardMng.isOpenFirstRecharge = true;
                        }
                        GameCenter.uIMng.SwitchToUI(GUIType.EVERYDAYREWARD);
                    }
                }
                else if (GameCenter.rankRewardMng.isFirstPlay && GameCenter.openServerRewardMng.reminTime > 0)
                {
                    GameCenter.uIMng.SwitchToUI(GUIType.DAILYFIRSTRECHARGE);//打开每日首充返利界面
                } 
                GameCenter.loginMng.isLogin = false;
            }
        }
    }
	/// <summary>
	/// 选择设置模式
	/// </summary>
	/// <param name="go">Go.</param>
	void SetPkMode(GameObject go)
	{
		PkMode mode = (PkMode)(int)UIEventListener.Get(go.gameObject).parameter;
		if(GameCenter.mainPlayerMng.pkModelTipShow || mode == PkMode.PKMODE_PEASE){
			GameCenter.mainPlayerMng.C2S_SetCampMode(mode);
		}else{
			if(pkModelUI != null){
				pkModelUI.PkModel = mode;
				pkModelUI.gameObject.SetActive(true);
			}
		}
		
		go.transform.parent.gameObject.SetActive(false);
	}
    /// <summary>
    /// 刷新所有内容
    /// </summary>
    void RefreshAll()
    {
        if (CurPlayerHp != null)
            CurPlayerHp.value = (float)actorInfo.CurHP / actorInfo.MaxHP; 
        if (HpLable != null)
            HpLable.text = actorInfo.CurHPText + "/" + actorInfo.MaxHPText;
        if (CurPlayerMp != null)
            CurPlayerMp.value = (float)actorInfo.CurMP / actorInfo.MaxMP; 
        if (MpLable != null)
            MpLable.text = actorInfo.CurMPText + "/" + actorInfo.MaxMPText;
        if (CurPlayerIcon != null)
			CurPlayerIcon.spriteName = actorInfo.IconName;
        if (CurPlayerLv != null)
			CurPlayerLv.text = ConfigMng.Instance.GetLevelDes(actorInfo.CurLevel);
		if (CurPlayerMode != null){
			CurPlayerMode.text = actorInfo.PkModeName;
			BoxCollider box = CurPlayerMode.transform.parent.gameObject.GetComponent<BoxCollider>();
			if(box != null){
				box.enabled = actorInfo.IsUpdatePkMode;
			}
		}
		if(name != null)name.text = "【"+GameCenter.mainPlayerMng.MainPlayerInfo.Name+"】";
		if(fighting != null)fighting.text = GameCenter.mainPlayerMng.MainPlayerInfo.FightValue.ToString();
		RefreshTeamInfo();
		
    }
 
	void ShowMyVip(){
		UISpriteEx[] vipSp = VipButton.GetComponentsInChildren<UISpriteEx>();
		for(int i=0;i<vipSp.Length;i++){
            vipSp[i].IsGray = VipData.vLev <= 0 ? UISpriteEx.ColorGray.Gray : UISpriteEx.ColorGray.normal;
		}
		UILabel vipLab = VipButton.GetComponentInChildren<UILabel>();
        if (vipLab != null) vipLab.text = VipData.vLev.ToString();
        if (vipRedMind != null) vipRedMind.gameObject.SetActive(VipData.vLev > VipData.vipReward.Count);  
	}

    protected override void OnOpen()
    {
        base.OnOpen();
        SevenChallengeShow();
        //跟七天登陆一样在这里插一段打开离线经验窗口的逻辑
        if (GameCenter.offLineRewardMng.IsOpenWnd)
        {
            if (offRewardWnd != null) offRewardWnd.OpenUI();
        }
        else
        {
            if (offRewardWnd != null) offRewardWnd.CloseUI();
        }
    }

    protected override void HandEvent(bool _bind)
    {
        base.HandEvent(_bind);
        if (_bind)
        {
            GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate += RefreshBaseDate;
            GameCenter.mainPlayerMng.MainPlayerInfo.OnPropertyUpdate += RefreshPropDate;
			GameCenter.teamMng.onTeammateUpdateEvent += RefreshTeamInfo;
			GameCenter.vipMng.OnVIPDataUpdate += ShowMyVip;
            GameCenter.mainPlayerMng.MainPlayerInfo.OnNameUpdate += RefreshName;
            GameCenter.sevenChallengeMng.updateSevenChallengeData += SevenChallengeShow;
        }
        else
        {
            GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate -= RefreshBaseDate;
            GameCenter.mainPlayerMng.MainPlayerInfo.OnPropertyUpdate -= RefreshPropDate;
			GameCenter.teamMng.onTeammateUpdateEvent -= RefreshTeamInfo;
			GameCenter.vipMng.OnVIPDataUpdate -= ShowMyVip;
            GameCenter.mainPlayerMng.MainPlayerInfo.OnNameUpdate -= RefreshName;
            GameCenter.sevenChallengeMng.updateSevenChallengeData -= SevenChallengeShow;
        }
    }


	void RefreshTeamInfo()
	{
		if(teamLeaderFlag != null)teamLeaderFlag.SetActive(GameCenter.teamMng.isLeader);
	}
    void RefreshName(string _name)
    {
        //Debug.Log("RefreshName(string _name):"+ _name);
        if (name != null) name.text = "【" + _name + "】";
    }
	void RefreshBaseDate(ActorBaseTag tag, ulong value,bool _fromAbility)
    {
        switch (tag)
        {
            case ActorBaseTag.CurHP:
                if (CurPlayerHp != null)
                {
                    CurPlayerHp.value = (float)actorInfo.CurHP / actorInfo.MaxHP;
                }
                if (HpLable != null)
                {
                    HpLable.text = actorInfo.CurHPText + "/" + actorInfo.MaxHPText;
                }
                break;
            case ActorBaseTag.CurMP:
                if (CurPlayerMp != null)
                { 
                    CurPlayerMp.value = (float)actorInfo.CurMP / actorInfo.MaxMP;
                }
                if (MpLable != null)
                {
                    MpLable.text = actorInfo.CurMPText + "/" + actorInfo.MaxMPText;
                }
                break;
            case ActorBaseTag.Level:
                if (CurPlayerLv != null)
                {
					CurPlayerLv.text = ConfigMng.Instance.GetLevelDes(GameCenter.curMainPlayer.actorInfo.CurLevel);
                }
//			GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.REIN,GameCenter.mainPlayerMng.MainPlayerInfo.IsRien);
                break;
			case ActorBaseTag.PKMODE:
                if (GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneUiType == SceneUiType.BATTLEFIGHT)
                {
                     SetCampModel();
                }
                else 
                {
                   if (CurPlayerMode != null)
                    {
                        CurPlayerMode.text = GameCenter.mainPlayerMng.MainPlayerInfo.PkModeName;
                    }
                    for (int i = 0, len = CurPkModes.Length; i < len; i++)
                    {
                        if (CurPkModes[i] != null)
                        {
                            CurPkModes[i].SetActive(i + 1 == (int)GameCenter.mainPlayerMng.MainPlayerInfo.CurPkMode);
                        }
                    }
                }
				break;
            case ActorBaseTag.FightValue:
                if (fighting != null)
                {
                    fighting.text = GameCenter.mainPlayerMng.MainPlayerInfo.FightValue.ToString();
                }
                break;
        }
        
    }

    /// <summary>
    /// 设置阵营模式
    /// </summary>
    void SetCampModel()
    { 
        if (CurPlayerMode != null)
        {
            CurPlayerMode.text = ConfigMng.Instance.GetUItext(220);
        }
        for (int i = 0, len = CurPkModes.Length; i < len; i++)
        {
            if (CurPkModes[i] != null)
            {
                if (i + 1 == 6)
                {
                    CurPkModes[i].SetActive(true);
                }
                else
                {
                    CurPkModes[i].SetActive(false);
                }
            }
        } 
    }

    void RefreshPropDate(ActorPropertyTag tag, long value, bool _fromAbility)
    {
        switch (tag)
        {
            case ActorPropertyTag.HPLIMIT:
                if (CurPlayerHp != null)
                {
                    CurPlayerHp.value = (float)actorInfo.CurHP / actorInfo.MaxHP;
                }
                if (HpLable != null)
                {
                    HpLable.text = actorInfo.CurHPText + "/" + actorInfo.MaxHPText;
                }
                break;
            case ActorPropertyTag.MPLIMIT:
                if (CurPlayerMp != null)
                {
                    CurPlayerMp.value = (float)actorInfo.CurMP / actorInfo.MaxMP;
                }
                if (MpLable != null)
                {
                    MpLable.text = actorInfo.CurMPText + "/" + actorInfo.MaxMPText;
                }
                break;
        }
    }

    /// <summary>
    /// 点击打开七日挑战窗口
    /// </summary>
    private void OpenSevenChallenge(GameObject _obj)
    {
        //Debug.Log("打开七日目标");
        GameCenter.uIMng.SwitchToUI(GUIType.SEVENCHALLENGE);
        GameCenter.sevenChallengeMng.C2S_ReqSevenChallengeInfo(1, 0);
    }
    /// <summary>
    /// 是否开放七日挑战入口是否显示红点
    /// </summary>
    void SevenChallengeShow()
    {
        //Debug.Log("GameCenter.sevenChallengeMng.ShowRedPoint:"+ GameCenter.sevenChallengeMng.ShowRedPoint);
        if (GameCenter.sevenChallengeMng.OpenSevenChallenge && sevenChallenge != null)
        {
            sevenChallenge.gameObject.SetActive(true);
            if (sevenRedPoint != null)
            {
                if (GameCenter.sevenChallengeMng.ShowRedPoint)
                    sevenRedPoint.gameObject.SetActive(true);
                else
                    sevenRedPoint.gameObject.SetActive(false);
            }
        }
        else
            sevenChallenge.gameObject.SetActive(false);
    }

    #region 控件事件
    /// <summary>
    /// 人物按钮响应
    /// </summary>
    /// <param name="obj"></param>
    void OnClickCharacterBtn(GameObject obj)
    {
		GameCenter.uIMng.ShowMenu();
    }
    /// <summary>
    /// vip按钮响应
    /// </summary>
    /// <param name="obj"></param>
    void OnClickVipBtn(GameObject obj)
    {
		GameCenter.uIMng.SwitchToUI(GUIType.VIP);
    }
    /// <summary>
    /// 充值按钮响应
    /// </summary>
    /// <param name="obj"></param>
    void OnClickRechargeBtn(GameObject obj)
    {

    }
    
    #endregion
}
