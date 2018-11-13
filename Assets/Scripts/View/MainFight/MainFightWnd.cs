//==============================================
//作者：邓成
//日期：2016/3/3
//用途：主窗口的下方界面
//=================================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MainFightWnd : GUIBase
{
    #region 外部控件aa
    
    /// <summary>
    /// 经验条
    /// </summary>
    public UIProgressBar expProgess;
    /// <summary>
    /// 经验百分比
    /// </summary>
    public UILabel expLable;
	/// <summary>
	/// 菜单界面
	/// </summary>
	public MenuWnd menuWnd;
    	/// <summary>
	/// 组队消息
	/// </summary>
	public GameObject teamMessage;
    /// <summary>
    /// 仙盟邀请消息
    /// </summary>
    public GameObject guildMessage;
	/// <summary>
	/// 被攻击提示
	/// </summary>
	public GameObject btnBeHit;
	public UISprite beHitIcon; 
	public UILabel beHitName;
    /// <summary>
    /// buff按钮
    /// </summary>
    public UIButton[] buffBtn;
    protected List<BuffInfo> curBuffList = new List<BuffInfo>();
    /// <summary>
    /// buff描述框
    /// </summary>
    public GameObject buffDesGo;
    /// <summary>
    /// buff描述
    /// </summary>
    public UILabel buffDes;
    /// <summary>
    /// buff倒计时
    /// </summary>
    public UITimer buffTime;
    ///// <summary>
    ///// 七日挑战入口
    ///// </summary>
    //public UIButton sevenChallenge;
    ///// <summary>
    ///// 七日挑战红点
    ///// </summary>
    //public GameObject redPoint;
    #region 活动提示
    public UIGrid itemGird;
    public GameObject actGo;
    public Dictionary<int, GameObject> actDic = new Dictionary<int, GameObject>();
    #endregion
    /// <summary>
    /// 战败记录
    /// </summary>
    public UIButton defeatRecordBtn;
    #endregion
    void Awake()
    {
		if (expProgess != null)
        {
			expProgess.value = (float)GameCenter.curMainPlayer.actorInfo.CurExp / GameCenter.curMainPlayer.actorInfo.MaxExp;
        }
		if (expLable != null)
        {
			expLable.text = (GameCenter.curMainPlayer.actorInfo.CurExp * 100 / GameCenter.curMainPlayer.actorInfo.MaxExp) + "%";
        }
		if(teamMessage != null)UIEventListener.Get(teamMessage).onClick = OpenTeamMessage;
        if (guildMessage != null) UIEventListener.Get(guildMessage).onClick = OpenGuildMessage;
        if (defeatRecordBtn != null)
        {
            UIEventListener.Get(defeatRecordBtn.gameObject).onClick = OpenDefeatRecordWnd;
            defeatRecordBtn.gameObject.SetActive(false);
        }
        if (buffDesGo != null) UIEventListener.Get(buffDesGo).onClick = delegate
        {
            buffDesGo.SetActive(false);
        };
        if (actGo != null) actGo.SetActive(false);
        //if(sevenChallenge!=null)
        //{
        //    UIEventListener.Get(sevenChallenge.gameObject).onClick = OpenSevenChallenge;
        //}
    }
	protected override void OnOpen ()
	{
		base.OnOpen ();
		CloseAllSubWnd();
		if(menuWnd != null)
		{
			menuWnd.OpenUI();
			GameCenter.uIMng.ShowMenu();
			GameCenter.uIMng.ShowMenu();
		}
        RefreshBuffSp(0,true);
    //    RefreshActivity();
	}
    protected override void OnClose()
    {
        base.OnClose();
        GameCenter.activityMng.ActivityOnGoingList.Clear();
    }
    protected override void HandEvent(bool _bind)
    {
        base.HandEvent(_bind);
        if (_bind)
        {
            GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate += RefreshBaseDate;
			GameCenter.teamMng.OnTeamMessageUpdateEvent += ShowTeamMessage;
            GameCenter.mainPlayerMng.MainPlayerInfo.OnBuffUpdate += RefreshBuffSp;
			GameCenter.abilityMng.OnMainPlayerBeAttack += OnMainPlayerBeAttack;
            //GameCenter.activityMng.OnActivityOnGoing += RefreshActivity;
            //GameCenter.activityMng.OnActivityOver += RefreshActivity;
            GameCenter.guildMng.OnGetReqGuildMessageEvent += ShowGuildMessage;
            GameCenter.resurrectionMng.OnDefeatRecordUpdate += ShowDefeatRecordBtn;
            //GameCenter.sevenChallengeMng.updateSevenChallengeData += SevenChallengeShow;
        }
        else
        {
            GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate -= RefreshBaseDate;
            GameCenter.mainPlayerMng.MainPlayerInfo.OnBuffUpdate -= RefreshBuffSp;
			GameCenter.teamMng.OnTeamMessageUpdateEvent -= ShowTeamMessage;
			GameCenter.abilityMng.OnMainPlayerBeAttack -= OnMainPlayerBeAttack;
            //GameCenter.activityMng.OnActivityOnGoing -= RefreshActivity;
            //GameCenter.activityMng.OnActivityOver -= RefreshActivity;
            GameCenter.guildMng.OnGetReqGuildMessageEvent -= ShowGuildMessage;
            GameCenter.resurrectionMng.OnDefeatRecordUpdate -= ShowDefeatRecordBtn;
            //GameCenter.sevenChallengeMng.updateSevenChallengeData -= SevenChallengeShow;
        }
    }

	protected bool showBeHitGo = false;
	protected float showBeHitTime = 0f;
	protected void OnMainPlayerBeAttack(OtherPlayer _other)
	{
		if(_other != null)
		{
			if(beHitIcon != null)
			{
				beHitIcon.spriteName = _other.actorInfo.IconName;
			}
			if(beHitName != null)
			{
				beHitName.text = _other.actorInfo.Name;
			}
			if(btnBeHit != null)
			{
				btnBeHit.SetActive(true);
				UIEventListener.Get(btnBeHit).onClick = (x)=>
				{
					GameCenter.curMainPlayer.HitTargetOnce(_other);
				};
			}
			showBeHitTime = Time.time;
			showBeHitGo = true;
		}
	}

	void CloseAllSubWnd()
	{
		if(menuWnd != null)menuWnd.CloseUI();
	}
    /// <summary>
    /// 刷新经验条
    /// </summary>
    /// <param name="tag"></param>
    /// <param name="value"></param>
    /// <param name="_fromAbility"></param>
	void RefreshBaseDate(ActorBaseTag tag, ulong value, bool _fromAbility)
    {
        switch (tag)
        {
            case ActorBaseTag.Exp:
                if (expProgess != null)
                {
                    expProgess.value = (float)GameCenter.curMainPlayer.actorInfo.CurExp / GameCenter.curMainPlayer.actorInfo.MaxExp;
                }
                if (expLable != null)
                {
                    expLable.text = (GameCenter.curMainPlayer.actorInfo.CurExp*100 / GameCenter.curMainPlayer.actorInfo.MaxExp)+"%";
                }
                break;
            default:
                break;
        }

    }
    /// <summary>
    /// 刷新BUFF图片
    /// </summary>
    void RefreshBuffSp(int _buffId, bool _add)
    {
        curBuffList.Clear();
        foreach (BuffInfo info in GameCenter.curMainPlayer.actorInfo.BuffList.Values)
        {
            if (info.IconName != string.Empty && info.IconName != "0" && info.HoldTime != 0)
                curBuffList.Add(info);
        }
        if (!_add)
        {
            BuffInfo info = GameCenter.curMainPlayer.actorInfo.GetBuffInfo(_buffId);
            if (curBuffList.Contains(info))
                curBuffList.Remove(info);
        }
        for (int i = 0; i < buffBtn.Length; i++)
        {
            if (i < curBuffList.Count)
            {
                buffBtn[i].gameObject.SetActive(true);
                UIEventListener.Get(buffBtn[i].gameObject).onClick -= OnClickBuff;
                UIEventListener.Get(buffBtn[i].gameObject).onClick += OnClickBuff;
                UIEventListener.Get(buffBtn[i].gameObject).parameter = i;
                buffBtn[i].GetComponentInChildren<UISprite>().spriteName = curBuffList[i].IconName;
            }
            else
            {
                buffBtn[i].gameObject.SetActive(false);
            }
        }
    }
    /// <summary>
    /// 点击BUFF图标
    /// </summary>
    void OnClickBuff(GameObject obj)
    {
        int m = (int)UIEventListener.Get(obj).parameter;
        buffDesGo.SetActive(true);
        if (buffDes != null) buffDes.text = curBuffList[m].BuffDes;
        if (buffTime != null)
        {
            buffTime.StartIntervalTimer(curBuffList[m].RestSeconds);
            buffTime.onTimeOut = delegate
            {
                buffDesGo.SetActive(false);
                buffBtn[m].gameObject.SetActive(false);
            };
        }
    }
	protected void ShowTeamMessage()
	{
		if(teamMessage != null)
			teamMessage.SetActive(true);
        if (itemGird != null)
        {
            itemGird.maxPerLine++;
            itemGird.repositionNow = true;
        }
	}
	protected void OpenTeamMessage(GameObject go)
	{
		MessageST message = GameCenter.teamMng.CurTeamMessage;
		if(message != null)
		{
			GameCenter.messageMng.AddClientMsg(message);
		}
		if(teamMessage != null)
			teamMessage.SetActive(false);
	}

    protected void ShowGuildMessage()
    {
        if (guildMessage != null)
            guildMessage.SetActive(true);
        if (itemGird != null)
        {
            itemGird.maxPerLine++;
            itemGird.repositionNow = true;
        }
    }

    protected void ShowDefeatRecordBtn()
    {
        if (defeatRecordBtn != null)
            defeatRecordBtn.gameObject.SetActive(true);
        if (itemGird != null)
        {
            itemGird.maxPerLine++;
            itemGird.repositionNow = true;
        }
    }
    protected void OpenDefeatRecordWnd(GameObject go)
    {
        GameCenter.uIMng.SwitchToUI(GUIType.DEFEATRECORD);
        if (defeatRecordBtn != null)
        {
            defeatRecordBtn.gameObject.SetActive(false);
        }
    }

    protected void OpenGuildMessage(GameObject go)
    {
        MessageST mst = new MessageST();
        mst.messID = 537;
        mst.delYes = (x) =>
            {
                GameCenter.guildMng.C2S_ApplyGuildInvite(true);
            };
        mst.delNo = (y) =>
            {
                GameCenter.guildMng.C2S_ApplyGuildInvite(false);
            };
        mst.words = new string[2] { GameCenter.guildMng.InvitePlayerName, GameCenter.guildMng.InviteGuildName };
        GameCenter.messageMng.AddClientMsg(mst);
        if (guildMessage != null)
            guildMessage.SetActive(false);
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
    ///// <summary>
    ///// 是否开放七日挑战入口是否显示红点
    ///// </summary>
    //void SevenChallengeShow()
    //{
    //    if (GameCenter.sevenChallengeMng.OpenSevenChallenge && sevenChallenge != null)
    //    {
    //        sevenChallenge.gameObject.SetActive(true);
    //        if (redPoint != null)
    //        {
    //            if (GameCenter.sevenChallengeMng.ShowRedPoint)
    //                redPoint.gameObject.SetActive(true);
    //            else
    //                redPoint.gameObject.SetActive(false);
    //        }     
    //    }
    //    else
    //        sevenChallenge.gameObject.SetActive(false);
    //}
    void Update()
	{
		if(showBeHitGo && Time.frameCount % 10 == 0)
		{
			if(Time.time - showBeHitTime > 10f)
			{
				showBeHitGo = false;
				if(btnBeHit != null)btnBeHit.SetActive(false);
			}
		}
	}
    void RefreshActivity()
    {
    //    RefreshActivity(GameCenter.activityMng.ActivityOnGoingList);
    }
    void RefreshActivity(Dictionary<int,ActivityListRef> _list)
    {
        //Debug.Log("刷新所有的活动提示");
        if (actGo != null)
        {
            foreach (GameObject obj in actDic.Values)
            {
                if (obj != null) obj.SetActive(false);
                //Debug.Log("隐藏活动提示");
            }
            if (itemGird != null) itemGird.maxPerLine = _list.Count;
            int i = 0;
            using (var e = _list.GetEnumerator())
            {
                while (e.MoveNext())
                {
                    ActivityListRef _data = e.Current.Value;
                    ActivityDataInfo info = GameCenter.activityMng.GetActivityDataInfo(_data.id);
                    if (info != null && info.State == ActivityState.HASENDED)
                        continue;
                    //Debug.Log("_data name:" + _data.name + ",id:" + _data.id);
                    GameObject go = null;
                    if (!actDic.ContainsKey(i))
                    {
                        go = Instantiate(actGo) as GameObject;
                        actDic[i]= go;
                    }
                    else
                        go = actDic[i];
                    go.transform.parent = actGo.transform.parent;
                    go.transform.localPosition = Vector3.zero;
                    go.transform.localScale = Vector3.one;
                    go.SetActive(true);
                    //Debug.Log("展示活动提示");
                    if (go != null && _data != null)
                    {
                        ActivityBtnUI activityBtnUI = go.GetComponent<ActivityBtnUI>();
                        if (activityBtnUI != null)
                        {
                            activityBtnUI.Refresh(_data);
                        }
                        UIEventListener.Get(go).onClick = delegate
                        {
                            if (GameCenter.activityMng.ActivityOnGoingList.ContainsKey(_data.id))
                            {
                                GameCenter.activityMng.ActivityOnGoingList.Remove(_data.id);
                                GameCenter.activityMng.haveTipDic[_data.id] = _data;
                            }
                            ActivityType type = (ActivityType)_data.id;
                            if (type == ActivityType.UNDERBOSS)
                            {
                                BossChallengeWnd.OpenAndGoWndByType(BossChallengeWnd.ToggleType.UnderBoss);
                            }
                            else
                            {
                                GameCenter.activityMng.OpenStartSeleteActivity(type);
                            }
                            go.SetActive(false);
                        };
                    }
                    i++;
                }
            }
            if (itemGird != null) itemGird.repositionNow = true;
        }
    }
}
