//===============================
//日期：2016/3/24
//作者：鲁家旗
//用途描述:试用翅膀界面
//===============================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
public class TrialWingWnd :GUIBase
{
    #region 控件数据
    /// <summary>
    /// 试用翅膀名称
    /// </summary>
    public UILabel nameLabel;
    /// <summary>
    /// 试用属性名称
    /// </summary>
    public List<UILabel> attriLabel = new List<UILabel>();
    /// <summary>
    /// 属性值
    /// </summary>
    public List<UILabel> attriNum = new List<UILabel>();

    /// <summary>
    /// 翅膀到期属性名称
    /// </summary>
    public List<UILabel> attriNextLabel = new List<UILabel>();
    /// <summary>
    /// 翅膀到期属性值
    /// </summary>
    public List<UILabel> attriNextNum = new List<UILabel>();

    /// <summary>
    /// 试穿按钮
    /// </summary>
    public UIButton wearBtn;
    /// <summary>
    /// 达到等级弹出界面
    /// </summary>
    public GameObject starGo;

    /// <summary>
    /// 超过等级弹出界面
    /// </summary>
    public GameObject endGo;
    /// <summary>
    /// 关闭弹出的试用翅膀到期的界面
    /// </summary>
    public UIButton closeBtn;
    /// <summary>
    /// 购买按钮
    /// </summary>
    public UIButton buyBtn;
    /// <summary>
    /// 技能图片
    /// </summary>
    public UISprite skillSP;
    /// <summary>
    /// 技能描述
    /// </summary>
    public UILabel skillDes;

    /// <summary>
    /// 试用翅膀数据
    /// </summary>
    protected WingRef WingRef
    {
        get
        {
            return ConfigMng.Instance.GetRef();
        }
    }
    /// <summary>
    /// 通用翅膀数据
    /// </summary>
    protected Dictionary<int, WingRef> Dic
    {
        get
        {
            return ConfigMng.Instance.GetWingRefTable();
        }
    }
    //public Load3DObject model;
    public UITexture wingTex;
    #endregion

    #region 构造
    void Awake()
    {
        layer = GUIZLayer.TIP;
        mutualExclusion = false;
    }
    void OnEnable()
    {
        if (wearBtn != null) EventDelegate.Add(wearBtn.onClick, OnClickWearBtn);
        if (buyBtn != null) EventDelegate.Add(buyBtn.onClick, OnClickBuyBtn);
        if (closeBtn != null) EventDelegate.Add(closeBtn.onClick, OnClickCloseBtn);
    }
    //打开窗口的时候
    protected override void OnOpen()
    {
        base.OnOpen();
        ChooseRefresh();
    }
    //关闭窗口的时候
    protected override void OnClose()
    {
 	     base.OnClose();
         if (wearBtn != null) EventDelegate.Remove(wearBtn.onClick, OnClickWearBtn);
         if (buyBtn != null) EventDelegate.Remove(buyBtn.onClick, OnClickBuyBtn);
         if (closeBtn != null) EventDelegate.Remove(closeBtn.onClick, OnClickCloseBtn);
         GameCenter.previewManager.ClearModel();
    }
    
    #endregion

    #region 控件事件
    /// <summary>
    /// 试穿
    /// </summary>
    void OnClickWearBtn()
    {
        GameCenter.uIMng.ReleaseGUI(GUIType.TRIALWING);
        GameCenter.uIMng.SwitchToUI(GUIType.NONE);
    }
    /// <summary>
    /// 购买 
    /// </summary>
    void OnClickBuyBtn()
    {
        // 跳到首冲大礼界面
        GameCenter.uIMng.SwitchToUI(GUIType.FIRSTCHARGEBONUS);
        GameCenter.uIMng.ReleaseGUI(GUIType.TRIALWING);
    }
    /// <summary>
    /// 关闭
    /// </summary>
    void OnClickCloseBtn()
    {
        GameCenter.uIMng.ReleaseGUI(GUIType.TRIALWING);
        GameCenter.uIMng.SwitchToUI(GUIType.NONE);
    }

    #endregion

    #region 刷新
    void ChooseRefresh()
    {
        //if (GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel < WingRef.use_lev)
        //    Refresh();
        //else
        // TIP 试用翅膀的弹出更改到模型预览界面去了 
        ChooseWingGo();
    }
    /// <summary>
    /// 试用翅膀弹窗
    /// </summary>
    void Refresh()
    {
        if (GameCenter.wingMng.WingDic.Count != 0)
        {
            GameCenter.uIMng.ReleaseGUI(GUIType.TRIALWING);
            GameCenter.uIMng.SwitchToUI(GUIType.NONE);
            return;
        }
        GameCenter.wingMng.C2S_RequestChangeWing(WingRef.id, true);
        starGo.SetActive(true);
        endGo.SetActive(false);
        if (nameLabel != null) nameLabel.text = "[b]" + WingRef.name;
        for (int i = 0; i < attriLabel.Count; i++)
        {
            AttributeTypeRef attributeRef = ConfigMng.Instance.GetAttributeTypeRef((ActorPropertyTag)Enum.ToObject(typeof(ActorPropertyTag), WingRef.property_list[i].eid));
            if (attriLabel[i] != null) attriLabel[i].text = attributeRef == null ? string.Empty : attributeRef.stats;
            if (attriNum[i] != null) attriNum[i].text = WingRef.property_list[i].count.ToString();
        }
        if (wingTex != null)
        {
            GameCenter.previewManager.TryPreviewSingleEquipment(new EquipmentInfo(WingRef.itemui, EquipmentBelongTo.PREVIEW), wingTex);
            //model.configID = WingRef.itemui;
            //model.StartLoad();
        }
    }

    /// <summary>
    /// 试用翅膀到期
    /// </summary>
    void ChooseWingGo()
    {
        if (GameCenter.wingMng.WingDic.Count != 0)
        {
            GameCenter.uIMng.ReleaseGUI(GUIType.TRIALWING);
            GameCenter.uIMng.SwitchToUI(GUIType.NONE);
            return;
        }
        GameCenter.wingMng.C2S_RequestChangeWing(WingRef.id, false);
        starGo.SetActive(false);
        endGo.SetActive(true);
        using (var info = Dic.GetEnumerator())
        {
            while (info.MoveNext())
            {
                if (nameLabel != null) nameLabel.text = "[b]" + info.Current.Value.name;
                for (int i = 0; i < attriNextLabel.Count; i++)
                {
                    AttributeTypeRef attributeRef = ConfigMng.Instance.GetAttributeTypeRef((ActorPropertyTag)Enum.ToObject(typeof(ActorPropertyTag), info.Current.Value.property_list[i].eid));
                    if (attriNextLabel[i] != null) attriNextLabel[i].text = attributeRef == null ? string.Empty : attributeRef.stats;
                    if (attriNextNum[i] != null) attriNextNum[i].text = "+" + info.Current.Value.property_list[i].count.ToString();
                }
                WingRef wingRef = ConfigMng.Instance.GetWingRef(info.Current.Value.id, ConfigMng.Instance.GetWingMaxLev(info.Current.Value.id));
                if (wingRef != null && skillSP != null)
                {
                    SkillMainConfigRef skillRef = ConfigMng.Instance.GetSkillMainConfigRef(wingRef.passivity_skill.skillid);
                    skillSP.spriteName = skillRef == null ? string.Empty : skillRef.skillIcon;
                }
                if (skillDes != null) skillDes.text = info.Current.Value.not_active_skill;
                //if (model != null)
                //{
                //    model.configID = wingRef.itemui;
                //    model.lookRotation = 360;
                //    model.StartLoad();
                //}
                if (wingTex != null)
                {
                    GameCenter.previewManager.TryPreviewSingleEquipment(new EquipmentInfo(wingRef.itemui, EquipmentBelongTo.PREVIEW), wingTex);
                }
                return;
            }
        }
    }
    #endregion
}
