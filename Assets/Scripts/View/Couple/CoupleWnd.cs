//==================================
//作者：朱素云
//日期：2016/5/15
//用途：仙侣界面
//=================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;

public class CoupleWnd : SubWnd
{
    #region 数据
    public GameObject notMerrage;
    public GameObject merrage;
    public GameObject divorcePop;
    public UIButton sureDivorce;
    public UIButton cancle; 
    public UIButton yuelaoBtn;//月老
    public UILabel desAfterMaxLev;
    public GameObject things;
    /// <summary>
    /// 结婚时间
    /// </summary>
    public UILabel timeLab; 
    /// <summary>
    /// 玩家头像
    /// </summary>
    public UISprite myIcon;
    /// <summary>
    /// 对象头像
    /// </summary>
    public UISprite objIcon;
    /// <summary>
    /// 亲密度
    /// </summary>
    public UILabel intimacyLab;
    /// <summary>
    /// 亲密度按钮跳转到送花界面
    /// </summary>
    public UIButton intimacyBtn;
    /// <summary>
    /// 当前称号
    /// </summary>
    public UISprite curTitleIcon;
    public UILabel noTitle;
    /// <summary>
    /// 下级要达到的亲密度
    /// </summary>
    public UILabel nextIntimacyLab;
    /// <summary>
    /// 下级亲密度的称号
    /// </summary>
    public UISprite nextTitleIcon;
    public GameObject noNextLabel;
    /// <summary>
    /// 提升信物需要消耗的物品名
    /// </summary>
    public UILabel itemName; 
    /// <summary>
    /// 金币：需要的/拥有的
    /// </summary>
    public UILabel coinCount;
    /// <summary>
    /// 信物
    /// </summary>
    public ItemUI tokenItem;
    /// <summary>
    /// 信物经验条
    /// </summary>
    public UISlider tokenExpSli;
    /// <summary>
    /// 信物经验比例
    /// </summary>
    public UILabel expLab;
    /// <summary>
    /// 信物名
    /// </summary>
    public UILabel tokenNameLab;
    /// <summary>
    /// 离婚
    /// </summary>
    public UIButton divorceBtn;
    /// <summary>
    /// 补办婚礼
    /// </summary>
    public UIButton holdMerrageBtn;
    /// <summary>
    /// 提升信物
    /// </summary>
    public UIButton upToken;
    /// <summary>
    /// 仙侣副本
    /// </summary>
    public UIButton copyCouple;
    /// <summary>
    /// 可进入仙侣副本的次数
    /// </summary>
    public UILabel time;
    /// <summary>
    /// 信件等级对应的星星
    /// </summary>
    public UISprite[] start;
    protected EquipmentRef consume;//信物
    /// <summary>
    /// 升级需要的数量
    /// </summary>
    protected ItemValue needItem = null; 
    protected CoupleData coupleData = null;
    public UISpriteEx merraigeEx;

    #endregion

    #region 构造

    protected MainPlayerInfo MainData
    {
        get
        {
            return GameCenter.mainPlayerMng.MainPlayerInfo;
        }
    }

    void Awake()
    {
        GameCenter.coupleMng.C2S_ReqMerrageInfo(); 
    }

    protected override void HandEvent(bool _bind)
    {
        base.HandEvent(_bind);
        if (_bind)
        {
            GameCenter.coupleMng.OnCoupleDataUpdata += Refresh;
            GameCenter.inventoryMng.OnBackpackUpdate += Refresh;
            GameCenter.coupleMng.OnCoupleTitleUpdata += CoupleTitleUpdata;
        }
        else
        {
            GameCenter.coupleMng.OnCoupleDataUpdata -= Refresh;
            GameCenter.inventoryMng.OnBackpackUpdate -= Refresh;
            GameCenter.coupleMng.OnCoupleTitleUpdata -= CoupleTitleUpdata;
        }
    }

    protected override void OnOpen()
    {
       
        base.OnOpen();   
        GameCenter.coupleMng.GeTTitleRef(); 

        if (GameCenter.coupleMng.coupleData != null)
        { 
             Refresh(); 
        }
        if (yuelaoBtn != null) UIEventListener.Get(yuelaoBtn.gameObject).onClick = OnClickFindYuelao; 
        if (divorceBtn != null) UIEventListener.Get(divorceBtn.gameObject).onClick = delegate
        {
            if (divorcePop != null) divorcePop.SetActive(true);
            if (sureDivorce != null) UIEventListener.Get(sureDivorce.gameObject).onClick = delegate { GameCenter.coupleMng.C2S_ReqDivorce(); };
            if (cancle != null) UIEventListener.Get(cancle.gameObject).onClick = delegate { divorcePop.SetActive(false); }; 
        };
        if (upToken != null) UIEventListener.Get(upToken.gameObject).onClick = OnClickUpToken; 
        if (copyCouple != null) UIEventListener.Get(copyCouple.gameObject).onClick = delegate { GameCenter.coupleMng.C2S_ReqGoToCopy(); };
        if (myIcon != null) UIEventListener.Get(myIcon.gameObject).onClick = delegate 
        {
            if (MainData != null)
            {
                GameCenter.previewManager.C2S_AskOPCPreview(MainData.ServerInstanceID); 
            }
        };
        if (objIcon != null) UIEventListener.Get(objIcon.gameObject).onClick = delegate
        {
            if (coupleData != null) GameCenter.previewManager.C2S_AskOPCPreview(coupleData.objId); 
        };
        if (intimacyBtn != null) UIEventListener.Get(intimacyBtn.gameObject).onClick = delegate
        {
            if (coupleData != null)
            {
                GameCenter.friendsMng.SendFlowerType = 20;
                GameCenter.friendsMng.sendFlowerToOne = coupleData.objId;
                GameCenter.uIMng.GenGUI(GUIType.SENDFLOWER, true);
            }
        };
        if (holdMerrageBtn != null) UIEventListener.Get(holdMerrageBtn.gameObject).onClick = delegate
         {
             if (!GameCenter.teamMng.isInTeam || GameCenter.teamMng.TeammateCount != 2)
             {
                 //上浮提示两人队伍才可结为夫妻 
                 GameCenter.messageMng.AddClientMsg(343);
                 return;
             }
             if (merraigeEx.IsGray == UISpriteEx.ColorGray.normal)
                 GameCenter.uIMng.GenGUI(GUIType.MARRIAGE, true); 
         };
    }

    protected override void OnClose()
    {
        base.OnClose();  
    }
    void OnClickFindYuelao(GameObject go)
    {
        NPCAIRef yuelao = ConfigMng.Instance.GetNPCAIRefByType(500049);//月老 
        if (yuelao != null)
        { 
            if (GameCenter.curMainPlayer.GoTraceTarget(yuelao.scene, yuelao.sceneX, yuelao.sceneY))
            {
                GameCenter.uIMng.SwitchToUI(GUIType.NONE);
                GameCenter.uIMng.ReleaseGUI(GUIType.MAIL);
            }
        }
    }
    void OnClickUpToken(GameObject go)
    { 
        ulong coinNum = 0; 
        
        if (coupleData != null)
        {
            if (coupleData.tokenLev >= 10)
            { 
                //提示等级已满
                return;
            }
            for (int i = 0, max = coupleData.Items.Count; i < max; i++)
            {
                if (coupleData.Items[i].eid == 5 || coupleData.Items[i].eid == 6)//消耗金币
                {
                    coinNum = (ulong)coupleData.Items[i].count;
                } 
            }
            if (consume != null && needItem != null && GameCenter.mainPlayerMng.MainPlayerInfo.TotalCoinCount >= coinNum)
            { 
                if (GameCenter.inventoryMng.GetNumberByType(consume.id) >= needItem.count)
                {
                    GameCenter.coupleMng.C2S_ReqUpToken();
                }
                else//物品不足
                {
                    GameCenter.practiceMng.ReminderWnd(12, consume.name);
                } 
            }
            else//金幣不足
            {
                GameCenter.messageMng.AddClientMsg(155);
            }
        }
    }
    #endregion

    #region 刷新

    void Refresh()
    { 
        coupleData = GameCenter.coupleMng.coupleData;
        if (coupleData == null || coupleData.tokenId == 0 || coupleData.marrageTime == 0)
        {
            if (notMerrage != null) notMerrage.SetActive(true);
            if (merrage != null) merrage.SetActive(false); 
            return;
        }
        if (coupleData != null)
        {
            if (notMerrage != null) notMerrage.SetActive(false);
            if (merrage != null) merrage.SetActive(true);
			DateTime _time = GameHelper.ToChinaTime(new DateTime(1970, 1, 1)).AddSeconds(coupleData.marrageTime);
            if (timeLab != null) timeLab.text = ConfigMng.Instance.GetUItext(24, new string[6] {
            _time.Year.ToString(), _time.Month.ToString(), _time.Day.ToString(), _time.Hour.ToString(), _time.Minute.ToString(), coupleData.objName });
            if (MainData != null && myIcon != null)
            {
                myIcon.MakePixelPerfect();
                myIcon.spriteName = MainData.IconName; 
            }
            if (objIcon != null)
            {
                objIcon.MakePixelPerfect();
                objIcon.spriteName = coupleData.ObjIcon; 
            }
            if (intimacyLab != null) intimacyLab.text = coupleData.intimacy.ToString();
            if (tokenItem != null) tokenItem.FillInfo(new EquipmentInfo(coupleData.tokenId, EquipmentBelongTo.PREVIEW));
            if (tokenExpSli != null) tokenExpSli.value = (float)coupleData.tokenExp / coupleData.Exp;
            if (expLab != null) expLab.text = coupleData.tokenExp + "/" + coupleData.Exp;
            if (time != null) time.text = coupleData.time.ToString(); 
            if (coupleData.isHoldMerige)
            {
                if (merraigeEx != null) merraigeEx.IsGray = UISpriteEx.ColorGray.Gray;
            }
            else
            {
                if (merraigeEx != null) merraigeEx.IsGray = UISpriteEx.ColorGray.normal;
            }  
            for (int i = 0; i < start.Length; i++)
            {
                if (i < coupleData.tokenLev)
                {
                    start[i].gameObject.SetActive(true);
                }
                else
                    start[i].gameObject.SetActive(false);
            } 
            EquipmentRef eqt = ConfigMng.Instance.GetEquipmentRef(coupleData.tokenId);
            if (eqt != null)
            {
                if (tokenNameLab != null) tokenNameLab.text = eqt.name;
            }
            if (coupleData.tokenLev >= 10)//等级已满 
            {
                if (tokenExpSli !=null) tokenExpSli.value = 1;
                if (expLab != null) expLab.text = coupleData.Exp + "/" + coupleData.Exp;
                if (desAfterMaxLev != null) desAfterMaxLev.gameObject.SetActive(true);
                if (things != null) things.gameObject.SetActive(false);
            }
            else
            { 
                if (desAfterMaxLev != null) desAfterMaxLev.gameObject.SetActive(false);
                if (things != null) things.gameObject.SetActive(true); 
                for (int i = 0, max = coupleData.NextItems.Count; i < max; i++)
                {
                    if (coupleData.NextItems[i].eid == 5 || coupleData.NextItems[i].eid == 6)//消耗金币
                    {
                        if (coinCount != null)
                            coinCount.text = coupleData.NextItems[i].count + "/" + GameCenter.mainPlayerMng.MainPlayerInfo.TotalCoinCount;
                    }
                    else//消耗物品
                    {
                        consume = ConfigMng.Instance.GetEquipmentRef(coupleData.NextItems[i].eid);
                        needItem = coupleData.NextItems[i];
                        if (itemName != null) itemName.text = GameHelper.GetStringWithBagNumber(coupleData.NextItems[i]);
                    }
                } 
            }
        }
    }

    void CoupleTitleUpdata()
    {
        TitleRef curTitle = GameCenter.coupleMng.titleRef;//当前称号 
        if (curTitle != null)
        {
            if (curTitleIcon != null) curTitleIcon.gameObject.SetActive(true);
            if (noTitle != null) noTitle.gameObject.SetActive(false); 
            if (curTitleIcon != null)
            {
                curTitleIcon.spriteName = curTitle.icon;
                curTitleIcon.MakePixelPerfect();
            }
        }
        else
        {
            if (curTitleIcon != null) curTitleIcon.gameObject.SetActive(false);
            if (noTitle != null) noTitle.gameObject.SetActive(true);
        }

        TitleRef nextTitleRef = GameCenter.coupleMng.nextTitleRef;//下级称号
        if (nextTitleRef != null)
        {
            if (nextTitleRef != null && nextTitleRef.judgeNum.Count > 0) nextIntimacyLab.text = ConfigMng.Instance.GetUItext(25, new string[1] { nextTitleRef.judgeNum[0].ToString() });
            if (noNextLabel != null) noNextLabel.SetActive(false);
            if (nextIntimacyLab != null) nextIntimacyLab.gameObject.SetActive(true);
            if (nextTitleIcon != null)
            {
                nextTitleIcon.gameObject.SetActive(true);
                nextTitleIcon.spriteName = nextTitleRef.icon;
                nextTitleIcon.MakePixelPerfect(); 
            }
        }
        else
        {
            if (noNextLabel != null) noNextLabel.SetActive(true);
            if (nextIntimacyLab != null) nextIntimacyLab.gameObject.SetActive(false);
            if (nextTitleIcon != null) nextTitleIcon.gameObject.SetActive(false);
        }
    }
     
    #endregion
}
