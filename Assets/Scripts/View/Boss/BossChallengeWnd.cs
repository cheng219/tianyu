//===============================
//作者：邓成
//日期：2016/4/26
//用途：挑战BOSS界面类
//===============================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossChallengeWnd : GUIBase {
	public UIButton btnClose;
	public UIToggle[] toggles;

	public UILabel address;
	public UITexture textureBoss;
	public UIButton btnFight;
	public UILabel labDes;
	public GameObject timeGo;
	public GameObject appearGo;
	public GameObject killGo;
	public UITimer timer;
	public UILabel labTip;
	public UILabel lastKiller;
    public GameObject lastKillerGo;
	public ItemUIContainer rewardItem;

    public GameObject bossCoppyGo;
    public UIButton btnBuyTimes;
    public UILabel remainTimes;
    public BossBuyNumUI buyTimesGo;

	public BossToggleItemUI toggleGo;
	public UIPanel  parent;
	public Vector4 positionInfo;
    public GameObject information;
	protected Dictionary<int,BossToggleItemUI> toggleDic = new Dictionary<int, BossToggleItemUI>();
	public enum ToggleType
	{
		CanKill = 0,
		UnderBoss = 1,//地宫BOSS
		SceneBoss = 2,//场景BOSS
		SealBoss = 3,//封印BOSS
		RongEBoss = 4,//熔恶之地
		LiRongEBoss = 5,//里熔恶之地
        BOSSCOPPY = 6,//boss副本
	}
	void Awake()
	{
		mutualExclusion = true;
        if (btnBuyTimes != null) UIEventListener.Get(btnBuyTimes.gameObject).onClick = AddBossCoppyTimes;
        if (btnFight != null) UIEventListener.Get(btnFight.gameObject).onClick = ChallengeBoss;
	}
	void Start()
	{
		if(btnClose != null)UIEventListener.Get(btnClose.gameObject).onClick = CloseWnd;
		if(toggles != null)
		{
			for (int i = 0,max=toggles.Length; i < max; i++) {
				if (toggles[i] != null)UIEventListener.Get(toggles[i].gameObject).onClick = ClickToggleEvent;
			}
		}
	}
	protected UIToggle lastChangeToggle = null;
	protected void ClickToggleEvent(GameObject go)
	{
		UIToggle toggle = go.GetComponent<UIToggle>();
		if(toggle != lastChangeToggle)
		{
			OnChange();
		}
		if(toggle != null && toggle.value)lastChangeToggle = toggle;
	}
	protected override void OnOpen ()
	{
		base.OnOpen ();
		GameCenter.bossChallengeMng.C2S_ReqChallengeBossList();
        ResetWnd();
	}
	protected override void OnClose ()
	{
		base.OnClose ();
	}
	protected override void HandEvent (bool _bind)
	{
		base.HandEvent (_bind);
		if(_bind)
		{
			GameCenter.bossChallengeMng.OnGotChallengListEvent += OnChange;
            GameCenter.bossChallengeMng.OnBossCoppyDataUpdateEvent += OnChange;
		}else
		{
			GameCenter.bossChallengeMng.OnGotChallengListEvent -= OnChange;
            GameCenter.bossChallengeMng.OnBossCoppyDataUpdateEvent -= OnChange;
		}
	}
	protected override void InitSubWndState ()
	{
		base.InitSubWndState ();
		UIToggle toggle = null;
		switch(initSubGUIType)
		{
		case SubGUIType.UNDERBOSS:
			if(toggles != null && toggles.Length>(int)ToggleType.UnderBoss)toggle = toggles[(int)ToggleType.UnderBoss];
			break;
		case SubGUIType.SEALBOSS:
			if(toggles != null && toggles.Length>(int)ToggleType.SealBoss)toggle = toggles[(int)ToggleType.SealBoss];
			break;
		case SubGUIType.SCENEBOSS:
			if(toggles != null && toggles.Length>(int)ToggleType.SceneBoss)toggle = toggles[(int)ToggleType.SceneBoss];
			break;
		case SubGUIType.RONGEBOSS:
			if(toggles != null && toggles.Length>(int)ToggleType.RongEBoss)toggle = toggles[(int)ToggleType.RongEBoss];
			break;
		case SubGUIType.LIRONGEBOSS:
			if(toggles != null && toggles.Length>(int)ToggleType.LiRongEBoss)toggle = toggles[(int)ToggleType.LiRongEBoss];
			break;
        case SubGUIType.BOSSCOPPY:
            if (toggles != null && toggles.Length > (int)ToggleType.BOSSCOPPY) toggle = toggles[(int)ToggleType.BOSSCOPPY];
            break;
		}
		if(toggle != null)
		{
			toggle.value = true;
			ClickToggleEvent(toggle.gameObject);
		}
	}

	void OnChange()
	{
        ResetWnd();
        //每次响应toggle的时候重新设置一下information的状态
        information.SetActive(true);
		ToggleType changeOne = ToggleType.CanKill;
		for (int i = 0,max=toggles.Length; i < max; i++) {
			if(toggles[i] != null && toggles[i].value)
			{	
				changeOne = (ToggleType)i;
                //如果选择的是第一个toggle(可击杀则判断时候应该隐藏掉奖励)
                if(i == 0)
                {
                    CheckHideReward();
                }
				break;
			}
		}
		switch(changeOne)
		{
		case ToggleType.CanKill:
			ShowToggle(GameCenter.bossChallengeMng.CanKillBossList);
			break;
		case ToggleType.UnderBoss:
                GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.UNDERBOSS, false);
            ShowToggle(ConfigMng.Instance.GetBossRefByType((int)changeOne));
            break;
		case ToggleType.SceneBoss:
		case ToggleType.SealBoss:
            GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.SEALBOSS, false);
                ShowToggle(ConfigMng.Instance.GetBossRefByType((int)changeOne));
            break;
		case ToggleType.RongEBoss:
		case ToggleType.LiRongEBoss:
			ShowToggle(ConfigMng.Instance.GetBossRefByType((int)changeOne));
			break;
        case ToggleType.BOSSCOPPY:
            ShowToggle(ConfigMng.Instance.GetBossRefListByType((int)changeOne));
            break;
		}
	}
	/// <summary>
	/// 显示可击杀BOSS
	/// </summary>
	void ShowToggle(List<BossChallengeData> bossList)
	{
		HideToggle();
		int index = 0;
		BossToggleItemUI firstItemUI = null;
		for (int i = 0,max=bossList.Count; i < max; i++) 
		{	
			var item = bossList[i];
			BossToggleItemUI itemUI = null;
			if(!toggleDic.TryGetValue(index,out itemUI))
			{
				BossToggleItemUI go = Instantiate(toggleGo) as BossToggleItemUI;
				toggleDic[index] = go.GetComponent<BossToggleItemUI>();
			}
			itemUI = toggleDic[index];
			if(itemUI != null)
			{
				itemUI.gameObject.SetActive(true);
				itemUI.transform.parent = parent.transform;
				itemUI.transform.localScale = Vector3.one;
				itemUI.transform.localPosition = new Vector3(positionInfo.x,positionInfo.y+positionInfo.w*index,-1);
				itemUI.SetData(item,OnChangeBoss,(index==0));
                if (index == 0)firstItemUI = itemUI;
			}
			index++;
		}
		if(firstItemUI != null)firstItemUI.SetChecked();
		if(parent != null)
		{
			parent.clipOffset = Vector2.zero;
			parent.transform.localPosition = Vector3.zero;
		}
	}
	void ShowToggle(List<int> bossList)
	{
		HideToggle();
		int index = 0;
		BossToggleItemUI firstItemUI = null;
		for (int i = 0,max=bossList.Count; i < max; i++) 
		{
			var item = bossList[i];
			BossToggleItemUI itemUI = null;
			if(!toggleDic.TryGetValue(index,out itemUI))
			{
				BossToggleItemUI go = Instantiate(toggleGo) as BossToggleItemUI;
				toggleDic[index] = go.GetComponent<BossToggleItemUI>();
			}
			itemUI = toggleDic[index];
            if (itemUI != null)
			{
				itemUI.gameObject.SetActive(true);
				itemUI.transform.parent = parent.transform;
				itemUI.transform.localScale = Vector3.one;
				itemUI.transform.localPosition = new Vector3(positionInfo.x,positionInfo.y+positionInfo.w*index,-1);
				itemUI.SetData(item,OnChangeBoss,(index == 0));
                if (index == 0)firstItemUI = itemUI;
			}
			index++;
		}
		if(firstItemUI != null)firstItemUI.SetChecked();
		if(parent != null)
		{
			parent.clipOffset = Vector2.zero;
			parent.transform.localPosition = Vector3.zero;
		}
	}
    /// <summary>
    /// 显示可击杀BOSS
    /// </summary>
    void ShowToggle(List<BossRef> bossList)
    {
        HideToggle();
        int index = 0;
        BossToggleItemUI firstItemUI = null;
        for (int i = 0, max = bossList.Count; i < max; i++)
        {
            var item = bossList[i];
            BossToggleItemUI itemUI = null;
            if (!toggleDic.TryGetValue(index, out itemUI))
            {
                BossToggleItemUI go = Instantiate(toggleGo) as BossToggleItemUI;
                toggleDic[index] = go.GetComponent<BossToggleItemUI>();
            }
            itemUI = toggleDic[index];
            if (itemUI != null)
            {
                itemUI.gameObject.SetActive(true);
                itemUI.transform.parent = parent.transform;
                itemUI.transform.localScale = Vector3.one;
                itemUI.transform.localPosition = new Vector3(positionInfo.x, positionInfo.y + positionInfo.w * index, -1);
                itemUI.SetData(item);
                if (index == 0) firstItemUI = itemUI;
            }
            index++;
        }
        if (firstItemUI != null) firstItemUI.SetChecked();
        if (parent != null)
        {
            parent.clipOffset = Vector2.zero;
            parent.transform.localPosition = Vector3.zero;
        }
        if (bossList.Count > 0)
        {
            BossChallengeData data = new BossChallengeData(bossList[0]);
            OnChangeBoss(data);
        }
    }
	void HideToggle()
	{
		using(var e = toggleDic.GetEnumerator())
		{
			while(e.MoveNext())
			{
				var item = e.Current.Value;
				if(item != null)
				{
					item.ClearData();
				}
			}
		}
	}
	void OnChangeBoss(BossChallengeData data)
	{
        if (data.CurBossRef != null)
		{
			if(address != null)address.text = data.CurBossRef.wayres;
            if (textureBoss != null)
            {
                textureBoss.enabled = true;
                GameCenter.previewManager.TryPreviewSingelMonster(data.CurBossRef.monsterId, textureBoss, PreviewConfigType.Dialog);
            }
			if(labDes != null)labDes.text = data.CurBossRef.res;
			if(timeGo != null)timeGo.SetActive(false);
			if(appearGo != null)appearGo.SetActive(false);
			if(killGo != null)killGo.SetActive(false);
            if (bossCoppyGo != null) bossCoppyGo.SetActive(false);
            if (buyTimesGo != null) buyTimesGo.gameObject.SetActive(false);
            if (lastKillerGo != null) lastKillerGo.SetActive(true);
			switch(data.CurBossRef.type)
			{
			case (int)ToggleType.UnderBoss:
				break;
			case (int)ToggleType.SceneBoss:
				if(timeGo != null)timeGo.SetActive(true);
				if(appearGo != null)appearGo.SetActive(false);
				if(data.AppearTime > (int)Time.realtimeSinceStartup)
				{
					if(timer != null)
					{
						timer.StopTimer();
						timer.StartIntervalTimer(data.AppearTime-(int)Time.realtimeSinceStartup);
						timer.onTimeOut = (x)=>
						{
							if(timeGo != null)timeGo.SetActive(false);
							if(appearGo != null)appearGo.SetActive(true);
						};
					}
				}else
				{
					if(timeGo != null)timeGo.SetActive(false);
					if(appearGo != null)appearGo.SetActive(true);
				}
				break;
			case (int)ToggleType.SealBoss:
				if(timeGo != null)timeGo.SetActive(true);
				if(appearGo != null)appearGo.SetActive(false);
                if (lastKillerGo != null) lastKillerGo.SetActive(false);
				if(data.AppearTime > (int)Time.realtimeSinceStartup)
				{
					if(timer != null)
					{
						timer.StopTimer();
						timer.StartIntervalTimer(data.AppearTime-(int)Time.realtimeSinceStartup);
						timer.onTimeOut = (x)=>
						{
							if(timeGo != null)timeGo.SetActive(false);
							if(appearGo != null)appearGo.SetActive(true);
						};
					}
				}else
				{
					if(data.CanKill)
					{
						if(timeGo != null)timeGo.SetActive(false);
						if(appearGo != null)appearGo.SetActive(true);
					}else
					{
						if(killGo != null)killGo.SetActive(true);
						if(timeGo != null)timeGo.SetActive(false);
					}
				}
				string realDes = data.CurBossRef.res.Replace("#1#",(data.CanKill?"1":"0"));
				realDes = realDes.Replace("#0#",(data.CurBossRef.needLevel > GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel)?"[ff0000]":"[00ff00]");
				if(labDes != null)labDes.text = realDes;//封印BOSS
				break;
			case (int)ToggleType.RongEBoss:
			case (int)ToggleType.LiRongEBoss:
				if(timeGo != null)timeGo.SetActive(true);
				if(appearGo != null)appearGo.SetActive(false);
				if(data.CanKill)
				{
					if(timeGo != null)timeGo.SetActive(false);
					if(appearGo != null)appearGo.SetActive(true);
				}else//已击杀or倒计时
				{
					if(data.AppearTime > (int)Time.realtimeSinceStartup)
					{
						if(timer != null)
						{
							timer.StopTimer();
							timer.StartIntervalTimer(data.AppearTime-(int)Time.realtimeSinceStartup);
							timer.onTimeOut = (x)=>
							{
								if(timeGo != null)timeGo.SetActive(false);
								if(appearGo != null)appearGo.SetActive(true);
							};
						}
					}else//已击杀
					{
						if(killGo != null)killGo.SetActive(true);
						if(timeGo != null)timeGo.SetActive(false);
					}
				}
				break;
            case (int)ToggleType.BOSSCOPPY:
                if (bossCoppyGo != null) bossCoppyGo.SetActive(true);
                if (remainTimes != null) remainTimes.text = GameCenter.bossChallengeMng.RemainBuyBossCoppyTimes.ToString();
                string des = data.CurBossRef.res.Replace("#0#", GameCenter.bossChallengeMng.ChallengeBossCoppyTimes.ToString());
				des = des.Replace("#1#",(data.CurBossRef.needLevel > GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel)?"[ff0000]":"[00ff00]");
				if(labDes != null)labDes.text = des;
                if (lastKillerGo != null) lastKillerGo.SetActive(false);
                break;
			}
			if(labDes != null)labDes.text = labDes.text.Replace("\\n","\n");
			if(labTip != null)labTip.text = data.CurBossRef.tip;
			if(rewardItem != null)
			{
				rewardItem.CellHeight = 80;
				rewardItem.CellWidth = 80;
                //Debug.Log("data.rewardList" + data.rewardList.Count);
				rewardItem.RefreshItems(data.rewardList,3,data.rewardList.Count);
			}
            if (btnFight != null)
            {
                btnFight.enabled = true;
                UIEventListener.Get(btnFight.gameObject).parameter = data;
            }
		}
		if(lastKiller != null)lastKiller.text = data.KillName;
	}
	void ResetWnd()
	{
        if(address != null)address.text = string.Empty;
        if(textureBoss != null)textureBoss.enabled = false;
        if(btnFight != null)btnFight.enabled = false;
        if(labDes != null)labDes.text = string.Empty;
        if(labTip != null)labTip.text = string.Empty;
        if(lastKiller != null)lastKiller.text = string.Empty;
        if(timeGo != null)timeGo.SetActive(false);
        if(appearGo != null)appearGo.SetActive(false);
        if(killGo != null)killGo.SetActive(false);
        if (bossCoppyGo != null) bossCoppyGo.SetActive(false);
	}
	void CloseWnd(GameObject go)
	{
		GameCenter.uIMng.SwitchToUI(GUIType.NONE);
	}

    void ChallengeBoss(GameObject go)
    {
        BossChallengeData data = UIEventListener.Get(go).parameter as BossChallengeData;
        if (data == null) return;
        switch ((ToggleType)data.CurBossRef.type)
        {
            case ToggleType.UnderBoss:
                int num = GameCenter.inventoryMng.GetNumberByType(2600017);
                if (num > 0)
                {
                    GameCenter.bossChallengeMng.C2S_ChallengeBoss(data.bossID, data.CurBossRef.type);
                }
                else
                {
                    EquipmentInfo info = new EquipmentInfo(2600017, EquipmentBelongTo.PREVIEW);
                    MessageST mst = new MessageST();
                    mst.messID = 508;
                    mst.words = new string[] { info.ItemName, info.DiamondPrice.ToString() };
                    mst.delYes = (y) =>
                    {
                        if (GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount < info.DiamondPrice)
                        {
                            MessageST mst1 = new MessageST();
                            mst1.messID = 137;
                            mst1.delYes = (z) =>
                            {
                                GameCenter.uIMng.SwitchToUI(GUIType.RECHARGE);
                            };
                            GameCenter.messageMng.AddClientMsg(mst1);
                        }
                        else
                        {
                            GameCenter.bossChallengeMng.C2S_ChallengeBoss(data.bossID, data.CurBossRef.type);
                        }
                    };
                    GameCenter.messageMng.AddClientMsg(mst);
                }
                break;
            case ToggleType.SealBoss:
                if (data.CurBossRef.needLevel > GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel)
                {
                    GameCenter.messageMng.AddClientMsg(13);
                    return;
                }
                GameCenter.bossChallengeMng.C2S_ChallengeBoss(data.bossID, data.CurBossRef.type);
                break;
            case ToggleType.LiRongEBoss:
                if (GameCenter.vipMng.VipData != null && GameCenter.vipMng.VipData.vLev < 2)
                {
                    GameCenter.messageMng.AddClientMsg(154);
                    return;
                }
                //GameCenter.curMainPlayer.GoTraceTarget(data.CurBossRef.sceneID,data.CurBossRef.sceneX,data.CurBossRef.sceneY);
                //GameCenter.uIMng.SwitchToUI(GUIType.NONE);
                GameCenter.bossChallengeMng.C2S_ChallengeBoss(data.bossID, data.CurBossRef.type);
                break;
            case ToggleType.BOSSCOPPY:
                if (GameCenter.bossChallengeMng.ChallengeBossCoppyTimes > 0)
                {
                    GameCenter.activityMng.C2S_FlyHangUpCoppy(160013);
                }
                else
                {
                    GameCenter.messageMng.AddClientMsg(168);
                }
                break;
            default:
                GameCenter.curMainPlayer.GoNormal();
                GameCenter.curMainPlayer.GoTraceTarget(data.CurBossRef.sceneID, data.CurBossRef.sceneX, data.CurBossRef.sceneY);
                //Debug.Log("X:"+data.CurBossRef.sceneX+",Y:"+data.CurBossRef.sceneY);
                //GameCenter.uIMng.SwitchToUI(GUIType.NONE);
                Invoke("InvokeCloseWnd", 0.1f);
                break;
        }
    }

    void AddBossCoppyTimes(GameObject go)
    {
        if (buyTimesGo) buyTimesGo.SetToBuyShow(GameCenter.bossChallengeMng.RemainBuyBossCoppyTimes);
    }

	void InvokeCloseWnd()
	{
		GameCenter.uIMng.SwitchToUI(GUIType.NONE);
	}

	public static void OpenAndGoWndByType(ToggleType toggleType)
	{
		GameCenter.uIMng.SwitchToUI(GUIType.BOSSCHALLENGE);
		BossChallengeWnd wnd = GameCenter.uIMng.GetGui<BossChallengeWnd>();
		if(wnd != null)
		{
			wnd.OpenWndByType(toggleType);
		}
	}
	public void OpenWndByType(ToggleType toggleType)
	{
		if(toggles != null)
		{
			for (int i = 0,max=toggles.Length; i < max; i++) 
			{
				if(toggles[i] != null)
				{
					toggles[i].value = (i == (int)toggleType);
					if(i == (int)toggleType)
					{
						ClickToggleEvent(toggles[i].gameObject);
					}
				}
			}
		}
	}
    //在选择可击杀的Toggle的时候如果没有可以挑战的boss奖励要隐藏
   public void CheckHideReward()
    {
        if (GameCenter.bossChallengeMng.CanKillBossList.Count == 0)
        {
            if(information.activeSelf)
            {
                information.SetActive(!information.activeSelf);
            }
        }
    }
}
