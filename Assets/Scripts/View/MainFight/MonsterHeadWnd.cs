//==============================================
//作者：邓成
//日期：2015/8/13
//用途：怪物头像栏


//修改人:吴江
//日期:2015/11/13
//修改内容:选中人物后的相关功能
//=================================================



using UnityEngine;
using System.Collections;

public class MonsterHeadWnd : GUIBase
{
	#region 公共
    /// <summary>
    /// 头像
    /// </summary>
    public UISprite CurTargetIcon;
    /// <summary>
    /// 名字
    /// </summary>
    public UILabel CurTargetName;
    /// <summary>
    /// 等级
    /// </summary>
    public UILabel CurTargetLv;
	#endregion

	#region 怪物
    /// <summary>
    /// 怪物
    /// </summary>
    public GameObject Creamobj;

	/// <summary>
	/// 血条
	/// </summary>
	public UIProgressBar notBossProgress;
	public GameObject notBossObj;
	public UILabel labNotBossHp;

	public GameObject bossObj;
	public UIProgressBar bossHpProgress;
	public UILabel labBossHp;
    public UILabel bossHpNum;
	#endregion

	#region 玩家
	/// <summary>
	/// 血条
	/// </summary>
	public UIProgressBar progressHp;
	/// <summary>
	/// 血量
	/// </summary>
	public UILabel HpLable;

	public GameObject playerObj;

    /// <summary>
    /// 玩家相关按钮的父控件 by吴江
    /// </summary>
    public GameObject btnParent;
    /// <summary>
    /// 决斗按钮 by吴江
    /// </summary>
    public UIButton fightBtn;
    /// <summary>
    /// 组队按钮 by吴江
    /// </summary>
    public UIButton teamBtn;
    /// <summary>
    /// 察看按钮 by吴江
    /// </summary>
    public UIButton checkBtn;
    /// <summary>
    /// 取消选择目标并关闭按钮
    /// </summary>
    public GameObject closeBtn;
    /// <summary>
    /// 加为好友按钮 by zsy
    /// </summary>
    public UIButton addFriendBtn;
    /// <summary>
    /// 邀请仙盟按钮
    /// </summary>
    public UIButton addGuildBtn;
    /// <summary>
    /// 交易按钮 by  黄洪兴
    /// </summary>
    public GameObject tradeBtn;

    /// <summary>
    /// 私聊按钮  by黄洪兴
    /// </summary>
    public GameObject privetChatBtn;
    /// <summary>
    /// 邮件按钮 
    /// </summary>
    public GameObject mailBtn;
    string playerName=string.Empty;
	/// <summary>
	/// BOSS界面导致地图隐藏
	/// </summary>
	protected bool resultMapDisable = false;


    #region 击杀BOSS最高伤害者 add 鲁家旗
    public UISprite playerIcon;
    public UILabel levAndNameLabel;
    public UILabel hpBar;
    public UISlider hpSlider;
    public GameObject go;
    protected MainPlayerInfo mainPlayerInfo = null;
    protected OtherPlayerInfo data = null;
    protected int curId = 0;
    protected int bossBloodNum = 1;
    #endregion
	#endregion



    #region 数据
    protected ActorInfo actorInfo;
    #endregion

    #region UNITY
    void Awake()
    {
        
    }

    void Start()
    {

    }

    protected override void OnOpen()
    {
        base.OnOpen();
        Refresh();
        if (fightBtn != null) UIEventListener.Get(fightBtn.gameObject).onClick = OnClickFightBtn;
        if (teamBtn != null) UIEventListener.Get(teamBtn.gameObject).onClick = OnClickTeamBtn;
        if (checkBtn != null) UIEventListener.Get(checkBtn.gameObject).onClick = OnClickCheckBtn;
        if (closeBtn != null) UIEventListener.Get(closeBtn.gameObject).onClick = OnClickCloseBtn;
        if (addFriendBtn != null) UIEventListener.Get(addFriendBtn.gameObject).onClick = OnClickAddFriendBtn;
        if (addGuildBtn != null) UIEventListener.Get(addGuildBtn.gameObject).onClick = OnClickReqGuild;
        if (gameObject != null) UIEventListener.Get(gameObject).onClick = OnClickSelf;
        if (tradeBtn != null) UIEventListener.Get(tradeBtn).onClick = OnClickTradeBtn;
        if (privetChatBtn != null) UIEventListener.Get(privetChatBtn).onClick = OnClickPrivetChatBtn;
        if (mailBtn != null) UIEventListener.Get(mailBtn.gameObject).onClick = OnClickMailBtn;
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
            GameCenter.curMainPlayer.OnTargetChange += Refresh;
            SceneMng.OnMobChange += RefreshMonsterOwner;
        }
        else
        {
            GameCenter.curMainPlayer.OnTargetChange -= Refresh;
            SceneMng.OnMobChange -= RefreshMonsterOwner;
        }
    }

    #endregion


    #region 控件事件 by吴江
    protected void OnClickFightBtn(GameObject _obj)
    {
    }


    protected void OnClickPrivetChatBtn(GameObject _obj)
    {
        if (playerName != string.Empty)
        {
            GameCenter.chatMng.OpenPrivateWnd(playerName);
        }
        else
        {
            Debug.LogError("私聊对象名字为空");
        }


    }


    protected void OnClickTradeBtn(GameObject _obj)
    {
        GameCenter.tradeMng.C2S_AskTrade(actorInfo.ServerInstanceID);
       // Debug.Log("请求与目标交易，交易ID为" + actorInfo.ServerInstanceID);


    }

    protected void OnClickMailBtn(GameObject _obj)
    {
        OtherPlayerInfo info = actorInfo as OtherPlayerInfo;
        if (info != null)
            GameCenter.mailBoxMng.mailWriteData = new MailWriteData(info.Name);
        GameCenter.uIMng.SwitchToSubUI(SubGUIType.BMail);
    }


	protected void OnClickSelf(GameObject _obj)
	{
		if(GameCenter.curMainPlayer.CurTarget.typeID != ObjectType.Player)
		{
			GameCenter.messageMng.AddClientMsg("他不想与你沟通!");
			return;
		}
		if(btnParent != null)btnParent.SetActive(true);
	}

    protected void OnClickAddFriendBtn(GameObject _obj)
    {
        GameCenter.friendsMng.C2S_AddFriend(GameCenter.curMainPlayer.CurTarget.id);
    }

    protected void OnClickReqGuild(GameObject go)
    {
        GameCenter.guildMng.C2S_ReqJoinGuild(GameCenter.curMainPlayer.CurTarget.id);
    }

    protected void OnClickTeamBtn(GameObject _obj)
    {
        if (GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneUiType == SceneUiType.RAIDERARK)
        {
            GameCenter.messageMng.AddClientMsg(481);
            return;
        }
        else
		    GameCenter.teamMng.C2S_TeamInvite(GameCenter.curMainPlayer.CurTarget.id);
    }

    protected void OnClickCheckBtn(GameObject _obj)
    {
        //GameCenter.uIMng.SwitchToUI(GUIType.PREVIEWOTHERS);
        GameCenter.previewManager.C2S_AskOPCPreview(actorInfo.ServerInstanceID);
    }
    #endregion


    protected void Refresh()
    {
        if (btnParent != null) btnParent.SetActive(false);
        if (actorInfo != null) actorInfo.OnBaseUpdate -= RefreshHp;
        if (GameCenter.curMainPlayer.CurTarget != null)
        {
            switch (GameCenter.curMainPlayer.CurTarget.typeID)
            {
                case ObjectType.Player:
                    OtherPlayerInfo opcInfo = GameCenter.sceneMng.GetOPCInfo(GameCenter.curMainPlayer.CurTarget.id);
                    if (opcInfo != null)
                    {
                        actorInfo = opcInfo;
						btnParent.SetActive(false);
                        RefreshPlayer(opcInfo);
                        actorInfo.OnBaseUpdate += RefreshHp;
                        RefreshLable();
                    }
					if(resultMapDisable)
					{
						GameCenter.uIMng.ShowMapMenu(true);
						resultMapDisable = false;
					}
                    break;
                case ObjectType.MOB:
                    MonsterInfo mobInfo = GameCenter.sceneMng.GetMobInfo(GameCenter.curMainPlayer.CurTarget.id);
                    if (mobInfo != null)
                    {
                        actorInfo = mobInfo;
                        btnParent.SetActive(false);
                        RefreshMonster(mobInfo);
                        actorInfo.OnBaseUpdate += RefreshHp;
                        RefreshLable();
						if(mobInfo.IsBoss)
						{
                            if (mobInfo.OwnerID != 0)
                                RefreshMonsterOwner(mobInfo.OwnerID);
							resultMapDisable = true;
						    GameCenter.uIMng.ShowMapMenu(false);
						}else
						{
							if(resultMapDisable)
							{
								GameCenter.uIMng.ShowMapMenu(true);
								resultMapDisable = false;
							}
						}
                    }
                    break;
//                case ObjectType.NPC:
//                    NPCInfo npcInfo = GameCenter.sceneMng.GetNPCInfo(GameCenter.curMainPlayer.CurTarget.id);
//                    if (npcInfo != null)
//                    {
//                        actorInfo = npcInfo;
//                        btnParent.SetActive(false);
//                        RefreshNPC(npcInfo);
//						if (progressHp != null)
//							progressHp.value = (1.0f);
//                        if (HpLable != null)
//                            HpLable.text = 100 + "/" + 100;
//                    }
//                    break;
                default:
                    actorInfo = null;
                    CloseUI();
					if(resultMapDisable)
					{
						GameCenter.uIMng.ShowMapMenu(true);
						resultMapDisable = false;
					}
                    break;
            }
        }
        else
        {
			if(resultMapDisable)
			{
				GameCenter.uIMng.ShowMapMenu(true);
				resultMapDisable = false;
			}
            actorInfo = null;
			CloseUI();
        }
    }


    protected void RefreshPlayer(OtherPlayerInfo _info)
    {
        playerName = _info.Name;
		if(playerObj != null)
			playerObj.SetActive(true);
        if (CurTargetLv != null)
            CurTargetLv.text = "["+_info.LevelDes+"]";
        if (CurTargetName != null)
            CurTargetName.text =_info.Name ;
        if (CurTargetIcon != null)
        {
            CurTargetIcon.spriteName = _info.IconName;
            CurTargetIcon.MakePixelPerfect();
        }
        if (Creamobj != null)
            Creamobj.SetActive(false);
        if (go != null)
            go.SetActive(false);
        _info.OnBaseUpdate -= RefreshHp;
        _info.OnBaseUpdate += RefreshHp;
    }
    
    protected void RefreshMonsterOwner(int _id)//add 鲁家旗刷新最高伤害击杀者
    {
        if (_id == 0)
        {
            go.SetActive(false);
            return;
        }
        go.SetActive(true);
        curId = _id;
        //击杀者就是主玩家
        if (_id == GameCenter.mainPlayerMng.MainPlayerInfo.ServerInstanceID)
        {
            mainPlayerInfo = GameCenter.mainPlayerMng.MainPlayerInfo;
            if (mainPlayerInfo != null)
            {
                if (playerIcon != null)
                {
                    playerIcon.spriteName = mainPlayerInfo.IconName;
                    playerIcon.MakePixelPerfect();
                }
                SetOwerHp();
                if (levAndNameLabel != null) levAndNameLabel.text = mainPlayerInfo.LevelDes + "[" + mainPlayerInfo.Name + "]";
                mainPlayerInfo.OnBaseUpdate -= RefreshMonsterOwnerHp;
                mainPlayerInfo.OnBaseUpdate += RefreshMonsterOwnerHp;
            }
        }
        else//击杀者是其他玩家
        {
            data = GameCenter.sceneMng.GetOPCInfo(_id);
            if (data != null)
            {
                if (playerIcon != null)
                {
                    playerIcon.spriteName = data.IconName;
                    playerIcon.MakePixelPerfect();
                }
                SetOtherHp();
                if (levAndNameLabel != null) levAndNameLabel.text = data.LevelDes + "[" + data.Name + "]";
                data.OnBaseUpdate -= RefreshOtherMonsterOwnerHp;
                data.OnBaseUpdate += RefreshOtherMonsterOwnerHp;
            }
            else
                go.SetActive(false);
        }
    }

    void SetOwerHp()
    {
        int hpPercent = (int)(((float)mainPlayerInfo.CurHP / mainPlayerInfo.MaxHP) * 100);
        if (hpSlider != null) hpSlider.value = ((float)mainPlayerInfo.CurHP / mainPlayerInfo.MaxHP);
        if (hpBar != null) hpBar.text = hpPercent + "%";
    }
    void RefreshMonsterOwnerHp(ActorBaseTag tag, ulong value, bool _bool)
    {
        switch (tag)
        {
            case ActorBaseTag.CurHP:
                if (curId != mainPlayerInfo.ServerInstanceID) return;
                SetOwerHp();
                break;
            case ActorBaseTag.Level:
                if (levAndNameLabel != null) levAndNameLabel.text = mainPlayerInfo.LevelDes + "[" + mainPlayerInfo.Name + "]";
                break;
            default:
                break;
        }
    }
    void SetOtherHp()
    {
        int hpPercent = (int)(((float)data.CurHP / data.MaxHP) * 100);
        if (hpSlider != null) hpSlider.value = ((float)data.CurHP / data.MaxHP);
        if (hpBar != null) hpBar.text = hpPercent + "%";
    }
    void RefreshOtherMonsterOwnerHp(ActorBaseTag tag, ulong value, bool _bool)
    {
        switch (tag)
        {
            case ActorBaseTag.CurHP:
                if (curId != data.ServerInstanceID) return;
                SetOtherHp();
                break;
            case ActorBaseTag.Level:
                if (levAndNameLabel != null) levAndNameLabel.text = data.LevelDes + "[" + data.Name + "]";
                break;
            default:
                break;
        }
    }

    protected void RefreshMonster(MonsterInfo _info)
    {
        if (CurTargetLv != null)
            CurTargetLv.text = "[" + _info.Level + ConfigMng.Instance.GetUItext(288)+"]";
        if (CurTargetName != null)
            CurTargetName.text =  _info.Name;
        if (CurTargetIcon != null)
        {
            CurTargetIcon.spriteName = _info.IconName;
            CurTargetIcon.MakePixelPerfect();
        }
        if (Creamobj != null)
			Creamobj.SetActive(true);
		if(notBossObj != null)
			notBossObj.SetActive(!_info.IsBoss);
		if(bossObj != null)
			bossObj.SetActive(_info.IsBoss);
        if (go != null)
            go.SetActive(_info.IsBoss ? _info.OwnerID != 0 : false);
        if (playerObj != null)
            playerObj.SetActive(false);
        bossBloodNum = _info.BossHpNum;
        _info.OnBaseUpdate -= RefreshHp;
        _info.OnBaseUpdate += RefreshHp;
    }

    //protected void RefreshNPC(NPCInfo _info)
    //{
    //    if (CurTargetName != null)
    //        CurTargetName.text = _info.Name;
    //    if (CurTargetIcon != null)
    //        CurTargetIcon.spriteName = _info.IconName;
    //    if (Creamobj != null)
    //        Creamobj.SetActive(false);
    //    if (go != null)
    //        go.SetActive(false);
    //}

	private void RefreshHp(ActorBaseTag tag,ulong value,bool _bool)
    {
        switch (tag)
        { 
            case ActorBaseTag.CurHP:
                RefreshLable();
                break;
            default:
                break;
        }
    }

    private void RefreshLable()
    {
		if(actorInfo == null)return ;
		int hpPercent = (int)(((float)actorInfo.CurHP/actorInfo.MaxHP)*100);
		if (progressHp != null)
			progressHp.value = ((float)actorInfo.CurHP / actorInfo.MaxHP);
        if (HpLable != null)
			HpLable.text = hpPercent + "%";
        //if (bossHpProgress != null)
        //    bossHpProgress.value = ((float)actorInfo.CurHP / actorInfo.MaxHP);
        if (labBossHp != null)
            labBossHp.text = actorInfo.CurHPText + "/" + actorInfo.MaxHPText;
        if (bossHpProgress != null)
            SetBossHp(bossBloodNum);
		if(notBossProgress != null)
			notBossProgress.value = ((float)actorInfo.CurHP / actorInfo.MaxHP);
		if(labNotBossHp != null)
			labNotBossHp.text = hpPercent + "%";
    }

    private void OnClickCloseBtn(GameObject obj)
    {
        GameCenter.curMainPlayer.CurTarget = null;
    }
    /// <summary>
    /// BOSS的多管血条
    /// </summary>
    /// <param name="_num"></param>
    private void SetBossHp(int _num)
    {
        if (_num == 0) return;
        int m = _num;//BOSS有多少管血
        int statge = 0;//现在处于第几管血
        float statgeHp = actorInfo.MaxHP / m;//平均一管血有多少
        for (int i = 1; i <= m; i++)
        {
            if ((float)actorInfo.CurHP > statgeHp * (m - i) && (float)actorInfo.CurHP <= statgeHp * (m - i + 1))
            {
                statge = (m - i + 1);
                break;
            }
        }
        if (statge == 0 && (int)actorInfo.CurHP == actorInfo.MaxHP)
            statge = m;
        if (bossHpProgress != null)
        {
            bossHpProgress.value = (((float)actorInfo.CurHP - (statgeHp * (statge - 1))) / statgeHp);
            bossHpProgress.backgroundWidget.gameObject.SetActive(true);
        }
        //TODO 每一阶段的颜色处理
        switch (statge)
        {
            case 1:
                bossHpProgress.backgroundWidget.gameObject.SetActive(false);
                bossHpProgress.foregroundWidget.color = new Color(186f/ 255f, 49f/ 255f, 35f/ 255f,1.0f);//红色
                break;
            case 2:
                bossHpProgress.backgroundWidget.color = new Color(186f / 255f, 49f / 255f, 35f / 255f, 1.0f);
                bossHpProgress.foregroundWidget.color = new Color(186f/ 255f, 103f/ 255f, 13f/ 255f,1.0f);//橙色
                break;
            case 3:
                bossHpProgress.backgroundWidget.color = new Color(186f / 255f, 103f / 255f, 13f / 255f, 1.0f);
                bossHpProgress.foregroundWidget.color = new Color(148f/ 255f, 58f/ 255f, 167f/ 255f,1.0f);//紫色
                break;
            case 4:
                bossHpProgress.backgroundWidget.color = new Color(148f / 255f, 58f / 255f, 167f / 255f, 1.0f);
                bossHpProgress.foregroundWidget.color = new Color(26f/255f, 157f/ 255f, 41f/ 255f,1.0f);//绿色
                break;
            case 5:
                bossHpProgress.backgroundWidget.color = new Color(26f / 255f, 157f / 255f, 41f / 255f, 1.0f);
                bossHpProgress.foregroundWidget.color = new Color(31f/255f, 74f/255f, 133f/255f,1.0f);//蓝色
                break;
        }
        //if (labBossHp != null) labBossHp.text = ((int)actorInfo.CurHP == actorInfo.MaxHP ? statgeHp : ((float)actorInfo.CurHP - (statgeHp * (statge - 1))) )+ "/" + statgeHp;
        if (bossHpNum != null)
        {
            if (_num > 1)
                bossHpNum.gameObject.SetActive(true);
            else
                bossHpNum.gameObject.SetActive(false);
            bossHpNum.text = "x" + statge;
        }
    }
}
