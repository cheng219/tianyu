//==================================
//作者：朱素云
//日期：2016/4/10
//用途：修行界面
//=================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PracticeWnd : GUIBase
{
    #region 数据
    public List<PracticeEffect> PracticeEffect = new List<PracticeEffect>();
    public UIButton closeBtn;
    public UIButton gotoFlyBtn;
    public UIButton ruleBtn; 
    public ExchangrUi exchangeUi;
	public UIFxAutoActive YGEffect;//运功特效
	public UIFxAutoActive SGEffect;//收功特效
    /// <summary>
    /// 仙气兑换
    /// </summary>
     public UIButton exchangeBtn;
    /// <summary>
    /// 仙气
    /// </summary>
     public UILabel dustLab;
    /// <summary>
    /// 灵气
    /// </summary>
     public UILabel reikiLab;

    /// <summary>
    /// 运功
    /// </summary>
     public GameObject yunGong;
    /// <summary>
    /// 铜币运功
    /// </summary>
     public UIButton coinYGBtn;
    /// <summary>
    /// 元宝运功
    /// </summary>
     public UIButton diamondYGBtn;
    /// <summary>
    /// 铜币数
    /// </summary>
     public UILabel coinLab;
    /// <summary>
    /// 铜币运功所用次数
    /// </summary>
     public UILabel coinTime;
    /// <summary>
    /// 元宝数
    /// </summary>
     public UILabel diamondLab; 

    /// <summary>
    /// 收功
    /// </summary>
     public GameObject shouGong; 
     public UIButton shouGongBtn;
    /// <summary>
    /// 高人指点
    /// </summary>
     public UIButton masterAdviceBtn; 
    /// <summary>
    /// 仙人指点
    /// </summary>
     public UIButton fairyAdviceBtn;
     public UISpriteEx master;
     public UISpriteEx fairy;
     protected PracticeData curData;
    protected MainPlayerInfo MainPlayerInfo
    {
        get
        {
            return GameCenter.mainPlayerMng.MainPlayerInfo;
        }
    }
    /// <summary>
    /// 仙气们
    /// </summary>
    private List<int> dustList
    {
        get
        {
            return GameCenter.practiceMng.dustList;
        }
    }
    /// <summary>
    /// 灵气们
    /// </summary>
    private List<int> reikiList
    {
        get
        {
            return GameCenter.practiceMng.reikiList;
        }
    }

    private PracticeEffect practiceEffect;
    /// <summary>
    /// 十倍运功
    /// </summary>
    public UIButton tenTimesYg;
    public UISpriteEx tenTimesEx;
    public UILabel tenTimesMoney;
    public UILabel masterAdviceMoney;
    public UILabel fairyAdviceMoney;
    #endregion

     void Awake()
    {
        GameCenter.practiceMng.C2S_ReinState(practiceType.PRACRICE);
        mutualExclusion = true;
        layer = GUIZLayer.NORMALWINDOW;
        practiceEffect = this.GetComponent<PracticeEffect>();
        if (coinYGBtn != null) UIEventListener.Get(coinYGBtn.gameObject).onClick = OnClickCoinYg;
        if (diamondYGBtn != null) UIEventListener.Get(diamondYGBtn.gameObject).onClick = OnClickDiamondYg;  
        if (closeBtn != null) UIEventListener.Get(closeBtn.gameObject).onClick = OnClickCloseBtn;
        if (gotoFlyBtn != null) UIEventListener.Get(gotoFlyBtn.gameObject).onClick = delegate
        {
            GameCenter.practiceMng.C2S_ReqStopExcercise();
            GameCenter.uIMng.SwitchToSubUI(SubGUIType.SOARING);
        };
        if (shouGongBtn != null) UIEventListener.Get(shouGongBtn.gameObject).onClick += OnClickStopYG;
        if (exchangeBtn != null) UIEventListener.Get(exchangeBtn.gameObject).onClick = delegate 
        { 
            exchangeUi.gameObject.SetActive(true);
        };
        if (masterAdviceBtn != null) UIEventListener.Get(masterAdviceBtn.gameObject).onClick += OnClickMasterAdvice;
        if (fairyAdviceBtn != null) UIEventListener.Get(fairyAdviceBtn.gameObject).onClick += OnClickfairyAdvice;
        if (tenTimesYg != null) UIEventListener.Get(tenTimesYg.gameObject).onClick = ClickTenTimesYG;
     } 
     void OnDestroy()
     {
         if (shouGongBtn != null) UIEventListener.Get(shouGongBtn.gameObject).onClick -= OnClickStopYG; 
         if (masterAdviceBtn != null) UIEventListener.Get(masterAdviceBtn.gameObject).onClick -= OnClickMasterAdvice;
         if (fairyAdviceBtn != null) UIEventListener.Get(fairyAdviceBtn.gameObject).onClick -= OnClickfairyAdvice;
     }
     void OnClickMasterAdvice(GameObject go)
     { 
         if (master.IsGray == UISpriteEx.ColorGray.normal)
         {
             if(GameCenter.practiceMng.isTenTimesYG)
                 GameCenter.practiceMng.C2S_ReqExcercise(ExcerciseType.TENMASTERADVICE);
             else 
                 GameCenter.practiceMng.C2S_ReqExcercise(ExcerciseType.MASTERADVICE);
         }
     }
     void OnClickfairyAdvice(GameObject go)
     {
         if (fairy.IsGray == UISpriteEx.ColorGray.normal)
         {
             if(GameCenter.practiceMng.isTenTimesYG)
                 GameCenter.practiceMng.C2S_ReqExcercise(ExcerciseType.TENFAIRYADVICE);
             else
                 GameCenter.practiceMng.C2S_ReqExcercise(ExcerciseType.FAIRYADVICE);
         }
     }
    protected override void OnOpen()
    {
        base.OnOpen();
        if (practiceEffect != null && GameCenter.practiceMng.bodyEffectNum > 0)
        {
            practiceEffect.ShowBodyEffects(GameCenter.practiceMng.bodyEffectNum / 71 + 1);
        }
        yunGong.SetActive(true);
        shouGong.SetActive(false); 
        Show(ActorBaseTag.LowFlyUpRes, 1 ,false);
        if (MainPlayerInfo != null) MainPlayerInfo.OnBaseUpdate += Show;
        GameCenter.practiceMng.OnExerciseUpdata += Exersice; 
    }
    protected override void OnClose()
    {
        base.OnClose();
        GameCenter.practiceMng.C2S_ReqStopExcercise();
        GameCenter.practiceMng.isTenTimesYG = false;
        if (MainPlayerInfo != null) MainPlayerInfo.OnBaseUpdate -= Show;
        GameCenter.practiceMng.OnExerciseUpdata -= Exersice; 
    } 
    void OnClickCloseBtn(GameObject go)
    { 
        GameCenter.uIMng.SwitchToUI(GUIType.NONE);
        GameCenter.uIMng.ReleaseGUI(GUIType.PRACTICE);
        GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate -= Show;
        GameCenter.practiceMng.OnExerciseUpdata -= Exersice; 
    }
	void Show(ActorBaseTag tag, ulong y, bool da)
    {
        if (tag == ActorBaseTag.HighFlyUpRes || tag == ActorBaseTag.LowFlyUpRes)
        {
            curData = GameCenter.practiceMng.data;
            if (curData == null) return;
            dustLab.text = curData.MainData.HighFlyUpRes.ToString();
            reikiLab.text = curData.MainData.LowFlyUpRes.ToString();
            if (yunGong.activeSelf)
            {
                master.IsGray = UISpriteEx.ColorGray.normal;
                fairy.IsGray = UISpriteEx.ColorGray.normal;
                coinLab.text = curData.CoinYG.ToString();
                coinTime.text = curData.coinTime + "/" + 10;
                diamondLab.text = curData.DiamondYG.ToString();
                if(curData.GetTenTimesYgDiamond() == 0)
                {
                    tenTimesMoney.text = "0";
                    tenTimesEx.IsGray = UISpriteEx.ColorGray.Gray;
                }
                else 
                {
                    tenTimesMoney.text = curData.GetTenTimesYgDiamond().ToString();
                    tenTimesEx.IsGray = UISpriteEx.ColorGray.normal;
                }
            } 
        }
    } 
    void Exersice()
    {  
        if (GameCenter.practiceMng.addDust + GameCenter.practiceMng.addReiki > 0)
        {
            shouGong.SetActive(true);
            yunGong.SetActive(false); 
            if (YGEffect != null) YGEffect.ShowFx(() =>
            { 
                for (int i = 0, max = PracticeEffect.Count; i < max; i++)
                {
                    PracticeEffect[i].gameObject.SetActive(true);
                    if (dustList.Count > i)
                    {
                        if (dustList[i] >= 0)
                        {
                            PracticeEffect[i].gameObject.SetActive(true);
                            PracticeEffect[i].practiceVal.gameObject.SetActive(true);
                            PracticeEffect[i].practiceVal.text = dustList[i].ToString();
                            if (dustList[i] == 0)
                            {
                                PracticeEffect[i].effectDust.SetActive(false);
                                PracticeEffect[i].effectReike.SetActive(false);
                                PracticeEffect[i].practiceVal.gameObject.SetActive(false);
                                PracticeEffect[i].lingqiBreak.ShowFx();
                            }
                            else
                            {
                                PracticeEffect[i].effectDust.SetActive(false);
                                PracticeEffect[i].effectReike.SetActive(true);
                                if (GameCenter.practiceMng.isTenTimesYG)
                                {
                                    PracticeEffect[i].effectReike.transform.localScale = new Vector3(1.0f + ((dustList[i]) / 210.0f), 1.0f + ((dustList[i]) / 210.0f));
                                }
                                else
                                    PracticeEffect[i].effectReike.transform.localScale = new Vector3(0.5f + ((dustList[i]) / 21.0f), 0.5f + ((dustList[i]) / 21.0f));
                            }
                        }
                    }
                    else
                    {
                        if ((reikiList.Count > (i - dustList.Count)))
                        {
                            if (reikiList[i - dustList.Count] >= 0)
                            {
                                PracticeEffect[i].gameObject.SetActive(true);
                                PracticeEffect[i].practiceVal.gameObject.SetActive(true);
                                PracticeEffect[i].practiceVal.text = reikiList[i - dustList.Count].ToString();
                                if (reikiList[i - dustList.Count] == 0)
                                {
                                    PracticeEffect[i].effectDust.SetActive(false);
                                    PracticeEffect[i].effectReike.SetActive(false);
                                    PracticeEffect[i].practiceVal.gameObject.SetActive(false);
                                    PracticeEffect[i].lingqiBreak.ShowFx();
                                }
                                else
                                {
                                    PracticeEffect[i].effectReike.SetActive(false);
                                    PracticeEffect[i].effectDust.SetActive(true);
                                    if (GameCenter.practiceMng.isTenTimesYG)
                                    {
                                        PracticeEffect[i].effectDust.transform.localScale = new Vector3(1.0f + ((reikiList[i - dustList.Count]) / 210.0f), 1.0f + ((reikiList[i - dustList.Count]) / 210.0f));
                                    }
                                    else
                                        PracticeEffect[i].effectDust.transform.localScale = new Vector3(0.5f + ((reikiList[i - dustList.Count]) / 21.0f), 0.5f + ((reikiList[i - dustList.Count]) / 21.0f));
                                }
                            }
                        }
                    }
                }
            });
        }
        else
        {
            shouGong.SetActive(false);
            yunGong.SetActive(true);
        } 
        if (master != null) master.IsGray = UISpriteEx.ColorGray.normal;
        if (fairy != null) fairy.IsGray = UISpriteEx.ColorGray.normal;
        if ((GameCenter.practiceMng.addDust + GameCenter.practiceMng.addReiki) == 0)
        {
            master.IsGray = UISpriteEx.ColorGray.Gray;
            fairy.IsGray = UISpriteEx.ColorGray.Gray;
        }
        else
        {
            master.IsGray = UISpriteEx.ColorGray.normal;
            fairy.IsGray = UISpriteEx.ColorGray.normal;
        }
        if (GameCenter.practiceMng.practiceType == 4 || GameCenter.practiceMng.practiceType == 7)//仙人指点变灰
        {
            master.IsGray = UISpriteEx.ColorGray.Gray;
            fairy.IsGray = UISpriteEx.ColorGray.Gray;
        }
        if (GameCenter.practiceMng.isTenTimesYG)
        {
            masterAdviceMoney.text = "200000";
            fairyAdviceMoney.text = "380";
        }
        else
        {
            masterAdviceMoney.text = "20000";
            fairyAdviceMoney.text = "38";
        } 
        Show(ActorBaseTag.LowFlyUpRes, 1, false);   
    }
    /// <summary>
    /// 点击收功
    /// </summary> 
    void OnClickStopYG(GameObject go)
    {
        if (shouGongBtn != null) UIEventListener.Get(shouGongBtn.gameObject).onClick -= OnClickStopYG; 
        if (masterAdviceBtn != null) UIEventListener.Get(masterAdviceBtn.gameObject).onClick -= OnClickMasterAdvice;
        if (fairyAdviceBtn != null) UIEventListener.Get(fairyAdviceBtn.gameObject).onClick -= OnClickfairyAdvice;
        if (GameCenter.practiceMng.addDust + GameCenter.practiceMng.addReiki > 0)
        {
			if (SGEffect != null) SGEffect.ShowFx(()=>
				{
                    GameCenter.practiceMng.isTenTimesYG = false;
                    GameCenter.practiceMng.C2S_ReqStopExcercise();
                    yunGong.SetActive(true);
                    shouGong.SetActive(false);
                    Show(ActorBaseTag.LowFlyUpRes, 1, false);
                    if (shouGongBtn != null) UIEventListener.Get(shouGongBtn.gameObject).onClick += OnClickStopYG; 
                    if (masterAdviceBtn != null) UIEventListener.Get(masterAdviceBtn.gameObject).onClick += OnClickMasterAdvice;
                    if (fairyAdviceBtn != null) UIEventListener.Get(fairyAdviceBtn.gameObject).onClick += OnClickfairyAdvice;
				});
        }
    }
    /// <summary>
    /// 点击铜钱运功
    /// </summary> 
    void OnClickCoinYg(GameObject go)
    {
        if (curData.coinTime <= 0)
        { GameCenter.messageMng.AddClientMsg(168); }
        else
        {
            if (MainPlayerInfo != null && curData.CoinYG > MainPlayerInfo.TotalCoinCount)
            {
                GameCenter.messageMng.AddClientMsg(155);
            }
            else
            {
                for (int i = 0, max = PracticeEffect.Count; i < max; i++)
                {
                    PracticeEffect[i].gameObject.SetActive(false);
                }
                GameCenter.practiceMng.C2S_ReqExcercise(ExcerciseType.COINEXC);
            }
        }
    }
    /// <summary>
    /// 点击元宝运功
    /// </summary> 
    void OnClickDiamondYg(GameObject go)
    {
        if (MainPlayerInfo != null && (ulong)curData.DiamondYG > MainPlayerInfo.TotalDiamondCount)
        {
            MessageST mst1 = new MessageST();
            mst1.messID = 137;
            mst1.delYes = delegate
            {
                GameCenter.uIMng.SwitchToUI(GUIType.RECHARGE);
            };
            GameCenter.messageMng.AddClientMsg(mst1);
        }
        else
        {
            for (int i = 0, max = PracticeEffect.Count; i < max; i++)
            {
                PracticeEffect[i].gameObject.SetActive(false);
            }
            GameCenter.practiceMng.C2S_ReqExcercise(ExcerciseType.DIAMONDEXC);
        }
    }
    /// <summary>
    /// 点击十倍运动
    /// </summary> 
    void ClickTenTimesYG(GameObject go)
    { 
        if (tenTimesEx.IsGray == UISpriteEx.ColorGray.normal)
        {
            if (MainPlayerInfo != null && (ulong)curData.GetTenTimesYgDiamond() > MainPlayerInfo.TotalDiamondCount)
            {
                MessageST mst1 = new MessageST();
                mst1.messID = 137;
                mst1.delYes = delegate
                {
                    GameCenter.uIMng.SwitchToUI(GUIType.RECHARGE);
                };
                GameCenter.messageMng.AddClientMsg(mst1);
            }
            else
            {
                for (int i = 0, max = PracticeEffect.Count; i < max; i++)
                {
                    PracticeEffect[i].gameObject.SetActive(false);
                }
                GameCenter.practiceMng.C2S_ReqExcercise(ExcerciseType.TENTIMESYG);
            }
        }
    }
}
