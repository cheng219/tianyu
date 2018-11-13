/// <summary>
/// 竞技场入口界面
/// 何明军
/// 2016/4/19
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArenaInWnd : GUIBase {

	public ArenaInPlayerUI[] player;
	
	public UISprite mIcon;
	public UILabel mRank;
	
	public UITimer killCd;
	public UITimer rewardCd;
	public UILabel mFight;
	public UILabel mKillNum;
	public UILabel[] mKillDes;
	
    public UILabel[] rewardLabel;
	public GameObject reward;
	public GameObject rewardClose;
	public UITimer rewardCloseTime;
    public UILabel[] rewardLabelUI;
	public UILabel mRankUI;
	
	public GameObject rewardBtn;
	public UISpriteEx rewardBtnEx;
	public GameObject rewardBtnEffect;
	public GameObject closeBtn;
	
	public UITexture texture;

    protected ArenaServerDataInfo arenaServerDataInfo = null;
     
	
	void OnArenaServerDataInfo(){
        arenaServerDataInfo = GameCenter.arenaMng.ArenaServerDataInfo;
        if (arenaServerDataInfo == null)
        {
            for (int i = 0, len = player.Length; i < len; i++)
            {
                if (player[i] != null)
                { 
                    player[i].gameObject.SetActive(false); 
                }
            }
            return;
        }

		mIcon.spriteName = GameCenter.mainPlayerMng.MainPlayerInfo.IconHalf;
		mIcon.MakePixelPerfect();
        mRank.text = arenaServerDataInfo.rank.ToString();
        killCd.StartIntervalTimer(arenaServerDataInfo.surplus_time);
		killCd.onTimeOut = delegate {
            arenaServerDataInfo.surplus_time = 0;
		};
        rewardCd.StartIntervalTimer(arenaServerDataInfo.reward_countdown);
		mFight.text = GameCenter.mainPlayerMng.MainPlayerInfo.FightValue.ToString();
        mKillNum.text = arenaServerDataInfo.challenge_num.ToString();
        mRankUI.text = ConfigMng.Instance.GetUItext(23, new string[1] { arenaServerDataInfo.reward_rank.ToString() });
        if (rewardBtnEx != null) rewardBtnEx.IsGray = (arenaServerDataInfo.state > 0) ? UISpriteEx.ColorGray.Gray : UISpriteEx.ColorGray.normal;
		BoxCollider rewardBox = rewardBtn.GetComponent<BoxCollider>();
        if (rewardBox != null) rewardBox.enabled = arenaServerDataInfo.state <= 0;
        rewardBtnEffect.SetActive(arenaServerDataInfo.state <= 0);
        List<string> logList = arenaServerDataInfo.GetLogList();
		for(int i=0,len=mKillDes.Length;i<len;i++){
			if(mKillDes[i] != null){
                if (i < logList.Count)
                {
                    mKillDes[i].text = logList[i];
					mKillDes[i].enabled = true;
				}else{
					mKillDes[i].enabled = false;
				}
			}
		}
        List<OtherPlayerInfo> playerList = arenaServerDataInfo.GetPlayer();
        int playerCount = playerList.Count; 
        if (playerCount < 4)
        {
            for (int i = 0, len = player.Length; i < len; i++)
            {
                if (player[i] != null)
                {
                    if (len - playerCount <= i) 
                    {
                        //Debug.Log(" playerCount[]  " + (i - (len - playerCount)));
                        player[i].gameObject.SetActive(true);
                        player[i].SetArenaPlayer(playerList[i - (len - playerCount)]);
                        UIEventListener.Get(player[i].gameObject).onClick -= OnClickPlayer;
                        UIEventListener.Get(player[i].gameObject).onClick += OnClickPlayer;
                        UIEventListener.Get(player[i].gameObject).parameter = player[i];
                    }
                    else
                    {
                        player[i].gameObject.SetActive(false);
                    }
                }
            }
        }
        else
        {
            for (int i = 0, len = player.Length; i < len; i++)
            {
                if (player[i] != null)
                {
                    if (i < playerCount)
                    {
                        player[i].gameObject.SetActive(true);
                        player[i].SetArenaPlayer(playerList[i]);
                        UIEventListener.Get(player[i].gameObject).onClick -= OnClickPlayer;
                        UIEventListener.Get(player[i].gameObject).onClick += OnClickPlayer;
                        UIEventListener.Get(player[i].gameObject).parameter = player[i];
                    }
                    else
                    {
                        player[i].gameObject.SetActive(false);
                    }
                }
            }
        }
        List<EquipmentInfo> rankRewardItems = arenaServerDataInfo.GetRankRewardItems();
        for (int i = 0, len = rewardLabel.Length; i < len; i++)
        {
            if (rewardLabel[i] != null)
            {
                if (i < rankRewardItems.Count)
                    rewardLabel[i].text = rankRewardItems[i].StackCurCount.ToString();
            }
        }
        for (int i = 0, len = rewardLabelUI.Length; i < len; i++)
        {
            if (rewardLabelUI[i] != null)
            {
                if (i < rankRewardItems.Count)
                    rewardLabelUI[i].text = rankRewardItems[i].StackCurCount.ToString();
            }
        }
	}
	
	void OnClickPlayer(GameObject games){
		ArenaInPlayerUI data = UIEventListener.Get(games).parameter as ArenaInPlayerUI;
		data.btnParent.SetActive(true);
	}
	
	void OnArenaServerReward(){
		rewardCloseTime.StartIntervalTimer(5);
		rewardCloseTime.onTimeOut = delegate {
			reward.SetActive(false);
		};
		reward.SetActive(true);
		if(rewardBtnEx != null)rewardBtnEx.IsGray = (GameCenter.arenaMng.ArenaServerDataInfo.state > 0) ?UISpriteEx.ColorGray.Gray : UISpriteEx.ColorGray.normal;
		BoxCollider rewardBox = rewardBtn.GetComponent<BoxCollider>();
		if(rewardBox != null)rewardBox.enabled = GameCenter.arenaMng.ArenaServerDataInfo.state <= 0;
		rewardBtnEffect.SetActive(GameCenter.arenaMng.ArenaServerDataInfo.state <= 0);
	}
	
	void Awake(){
		mutualExclusion  = true;
		Layer = GUIZLayer.TOPWINDOW;
		
		UIEventListener.Get(closeBtn).onClick = delegate { 
			GameCenter.uIMng.SwitchToUI(GUIType.NONE);	
		};
		
		UIEventListener.Get(rewardBtn).onClick = delegate {
			GameCenter.arenaMng.C2S_RepArenaReward();
		};
		
		UIEventListener.Get(rewardClose).onClick = delegate {
			reward.SetActive(false);
		};

        GameCenter.arenaMng.C2S_RepArenaServer();
	}
	
	protected override void OnOpen ()
	{
		base.OnOpen ();
        OnArenaServerDataInfo();
		reward.SetActive(false);
		GameCenter.arenaMng.OnArenaServerDataInfo += OnArenaServerDataInfo;
		GameCenter.arenaMng.OnArenaServerReward += OnArenaServerReward; 
		if(texture != null)ConfigMng.Instance.GetBigUIIcon("Pic_jjc_bg",delegate(Texture2D x){
			texture.mainTexture = x;
		});
	}
	
	protected override void OnClose ()
	{
		base.OnClose ();
		GameCenter.arenaMng.OnArenaServerDataInfo -= OnArenaServerDataInfo;
		GameCenter.arenaMng.OnArenaServerReward -= OnArenaServerReward;
		ConfigMng.Instance.RemoveBigUIIcon("Pic_jjc_bg");
	}
}
