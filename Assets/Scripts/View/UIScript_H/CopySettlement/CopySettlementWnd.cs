/// <summary>
/// 何明军
/// 2016/4/19
/// 副本结算界面
/// </summary>

using UnityEngine;
using System.Collections;

public class CopySettlementWnd : GUIBase {

    public GameObject star;//无尽试练隐藏
	public UISprite[] stars;
	public UILabel time;
    public GameObject timeGo;
    public UITimer timer;
	
	public UILabel exp;
	public UILabel coin;
	
	public UILabel mExp;
	public UISlider mExps;
	public UILabel mLev;
	
	public UITable table;
    public UIScrollView scrollView;
	public UILabel twoStarTime;
	public UILabel threeStarTime;
	
	public GameObject btnOk; 

    public GameObject muiltiple;//多人结算
    public GameObject single;//单人结算
    public GameObject bossCoppy;//BOSS副本结算
    public UILabel labBossNum;
    public GameObject btnBossOk;
    public GameObject btnBossAgain;
    public UITimer bossTimer;

    public ItemUI[] teamReward;//多人结算副本组队
    public UILabel noteamReward; 
	
	void Awake(){
		mutualExclusion = false;
		Layer = GUIZLayer.TOPWINDOW;
	}
	
	CopySettlementDataInfo data{
		get{
			return GameCenter.duplicateMng.CopySettlementDataInfo;
		}
	}


    void ShowSimpleReward()
    { 
        if (mLev != null) mLev.text = GameCenter.mainPlayerMng.MainPlayerInfo.LevelDes;
        float val = ((float)GameCenter.mainPlayerMng.MainPlayerInfo.CurExp / (float)GameCenter.mainPlayerMng.MainPlayerInfo.MaxExp);
        mExps.value = val;
        int valt = (int)(val * 10000);
        mExp.text = valt / 100f + "%";
        SceneRef scene = ConfigMng.Instance.GetSceneRef(GameCenter.mainPlayerMng.MainPlayerInfo.SceneID);
        twoStarTime.text = scene.starTime2.ToString();
        threeStarTime.text = scene.starTime3.ToString();
        twoStarTime.transform.parent.gameObject.SetActive(scene.starTime3 > 0);
    }

    void ShowMuiltypleReward()
    {
        for (int i = 0, len = teamReward.Length; i < len; i++)
        {
            teamReward[i].gameObject.SetActive(false);
        } 
        if (data.teamItems.Count > 0)
        {
            for (int i = 0, len = data.teamItems.Count; i < len; i++)
            {
                if (teamReward.Length > i)
                {
                    teamReward[i].gameObject.SetActive(true);
                    teamReward[i].FillInfo(data.teamItems[i]);
                }
            }
        }
        if (noteamReward != null) noteamReward.gameObject.SetActive(data.teamItems.Count <= 0);
    }


	void ShowSettlement(){


        switch (GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneUiType)
        {
            case SceneUiType.ENDLESS:
                if (star != null) star.SetActive(false);
                break;
            default:
                if (star != null) star.SetActive(true);
                break;
        }

        bool isMultyple = false;
        bool isBossCoppy = (GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneUiType == SceneUiType.BOSSCOPPY);
        if (data.coppyId > 0)
        {
            CopyRef copyRef = ConfigMng.Instance.GetCopyRef(data.coppyId);
            if (copyRef != null)
            {
                CopyGroupRef copyGrop = ConfigMng.Instance.GetCopyGroupRef(copyRef.copyGroup);
                if (copyGrop != null)
                {
                    isMultyple = copyGrop.sort == 2;
                }
            }
        }
        if (muiltiple != null) muiltiple.SetActive(isMultyple && !isBossCoppy);
        if (single != null) single.SetActive(!isMultyple && !isBossCoppy);
        if (bossCoppy != null) bossCoppy.SetActive(isBossCoppy);
        if (timeGo != null) timeGo.SetActive(!isBossCoppy);
        if (labBossNum != null) labBossNum.text = data.bossCount.ToString();
        if (time != null) time.text = GameCenter.duplicateMng.ItemTime(data.time);
		for(int i=0,len=stars.Length;i<len;i++){
			if(stars[i] == null)continue;
			if(data.star > i){
				stars[i].gameObject.SetActive(GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneUiType != SceneUiType.SEALBOSS);
			}else{
				stars[i].gameObject.SetActive(false);
			}
		}
		

        for (int i = 0, len = data.items.Count; i < len; i++)
        {
            if (data.items[i].EID == 3)
            {
                exp.text = data.items[i].StackCurCount.ToString();
            }
            else if (data.items[i].EID == 5)
            {
                coin.text = data.items[i].StackCurCount.ToString();
            }
            else
            {
                ItemUI item = UIUtil.CreateItemUIGame(table.gameObject).GetComponent<ItemUI>();
                item.transform.localPosition = Vector3.zero;
                item.transform.localScale = Vector3.one;
                item.FillInfo(data.items[i]);
                item.gameObject.SetActive(true);
            }
        }
        if (table != null) table.repositionNow = true;
        if (isMultyple) ShowMuiltypleReward();
        else ShowSimpleReward();
        Invoke("SetDragAmount",0.2f);
//		if(GameCenter.mainPlayerMng.MainPlayerInfo.reinNum > 0){
//			if(mLev != null)mLev.text = ConfigMng.Instance.GetUItext(12,
//				new string[2]{GameCenter.mainPlayerMng.MainPlayerInfo.reinNum.ToString(),GameCenter.mainPlayerMng.MainPlayerInfo.Level.ToString()});
//		}else{
//			if(mLev != null)mLev.text = ConfigMng.Instance.GetUItext(13,new string[1]{GameCenter.mainPlayerMng.MainPlayerInfo.Level.ToString()});
//		}
		
	}

    void SetDragAmount()
    {
        if (scrollView != null) scrollView.SetDragAmount(0, 0, false);
    }

	protected override void OnOpen ()
	{
		base.OnOpen ();
		ShowSettlement();
        bool isBossCoppy = (GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneUiType == SceneUiType.BOSSCOPPY);
        if (isBossCoppy)
        {
            if (btnBossOk != null) UIEventListener.Get(btnBossOk).onClick = delegate
            {
                ToFlopWmd();
            };
            if (btnBossAgain != null) UIEventListener.Get(btnBossAgain).onClick = FightAgain;
            bossTimer.StartIntervalTimer(8);
        }
        else
        {
            if (btnOk != null) UIEventListener.Get(btnOk).onClick = delegate
            {
                ToFlopWmd();
            };
            timer.StartIntervalTimer(8);
        }
		CancelInvoke("ToFlopWmd");
		Invoke("ToFlopWmd",8);
		
		GameCenter.curMainPlayer.StopForCheckMsg();
	}
	
	protected override void OnClose ()
	{
		base.OnClose ();
		CancelInvoke("ToFlopWmd");
	}
	
	void ToFlopWmd(){
        SceneUiType uiType = GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneUiType;
        GameCenter.uIMng.ReleaseGUI(GUIType.COPYWIN);
        if (uiType == SceneUiType.SEALBOSS || uiType == SceneUiType.BOSSCOPPY)
        {
			GameCenter.duplicateMng.C2S_OutCopy();
        //}else if(GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneUiType == SceneUiType.TOWER && (data == null || data.items.Count == 0)){
        //    GameCenter.uIMng.SwitchToUI(GUIType.NONE);
        //    GameCenter.duplicateMng.C2S_OutCopy();//镇魔塔要翻牌
        }
		else{
			GameCenter.uIMng.GenGUI(GUIType.COPYWINFLOP,true);
		}
	}

    void FightAgain(GameObject go)
    {
        if (GameCenter.bossChallengeMng.ChallengeBossCoppyTimes > 0)
        {
            GameCenter.uIMng.ReleaseGUI(GUIType.COPYWIN);
            GameCenter.activityMng.C2S_FlyHangUpCoppy(160013);
        }
        else
        {
            GameCenter.messageMng.AddClientMsg(168);
        }
    }
}
