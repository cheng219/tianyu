//======================================================
//作者:鲁家旗
//日期:2016/12/6
//用途:翅膀、坐骑、宠物模型预览UI
//======================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
public class ShowModelWnd : GUIBase {

    public UITexture modelTex;
    public UILabel nameLabel;
    public UIButton okBtn;
    public GameObject attGo;
    /// <summary>
    /// 试用属性名称
    /// </summary>
    public List<UILabel> attriLabel = new List<UILabel>();
    /// <summary>
    /// 属性值
    /// </summary>
    public List<UILabel> attriNum = new List<UILabel>();

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
    void Awake()
    {
        layer = GUIZLayer.TIP;
        mutualExclusion = false; 
    }
    protected override void OnOpen()
    {
        base.OnOpen();
        if (attGo != null) attGo.SetActive(false);
        ShowTrialWingModel();
        ShowModel();
        CancelInvoke("CloseWnd");
        Invoke("CloseWnd", 2.0f);
    }
    protected override void OnClose()
    {
        base.OnClose(); 
        switch (GameCenter.wingMng.modelType)
        {
            case ModelType.WING: GameCenter.uIMng.SwitchToSubUI(SubGUIType.SUBWING); break;
            case ModelType.PET: GameCenter.uIMng.SwitchToSubUI(SubGUIType.PETINFORMATION); break;
            case ModelType.MOUNT: GameCenter.uIMng.SwitchToSubUI(SubGUIType.MOUNT); break;
            case ModelType.ILLUSION: 
                GameCenter.newMountMng.curChooseSkin = GameCenter.wingMng.needShowMountInfo;
                GameCenter.uIMng.SwitchToSubUI(SubGUIType.ILLUSION); break;
        }
        GameCenter.wingMng.needShowWingInfo = null;
        GameCenter.wingMng.needShowMountInfo = null;
        GameCenter.wingMng.needShowPetInfo = null;
        GameCenter.wingMng.modelType = ModelType.NONE;
        GameCenter.wingMng.isNotShowTrialWingModel = false;
        modelTex.mainTexture = null;
        //CancelInvoke("CloseWnd");
    }
    /// <summary>
    /// 关闭窗口延时2秒
    /// </summary>
    void CloseWnd()
    {
        if (okBtn != null) UIEventListener.Get(okBtn.gameObject).onClick = delegate
        {
            GameCenter.uIMng.ReleaseGUI(GUIType.SHOWMODELUI);
        }; 
    }
    /// <summary>
    /// 试用翅膀模型
    /// </summary>
    void ShowTrialWingModel()
    {
        if (GameCenter.wingMng.isNotShowTrialWingModel) return;
        if (GameCenter.wingMng.WingDic.Count != 0 )
        {
            GameCenter.uIMng.ReleaseGUI(GUIType.SHOWMODELUI);
            GameCenter.uIMng.SwitchToUI(GUIType.NONE);
            return;
        }
        if (attGo != null) attGo.SetActive(true);
        GameCenter.wingMng.C2S_RequestChangeWing(WingRef.id, true);
        if (nameLabel != null) nameLabel.text = "[b]" + WingRef.name;
        for (int i = 0; i < attriLabel.Count; i++)
        {
            AttributeTypeRef attributeRef = ConfigMng.Instance.GetAttributeTypeRef((ActorPropertyTag)Enum.ToObject(typeof(ActorPropertyTag), WingRef.property_list[i].eid));
            if (attriLabel[i] != null) attriLabel[i].text = attributeRef == null ? string.Empty : attributeRef.stats;
            if (attriNum[i] != null) attriNum[i].text = WingRef.property_list[i].count.ToString();
        }
        if (modelTex != null)
        {
            GameCenter.previewManager.TryPreviewSingleEquipment(new EquipmentInfo(WingRef.itemui, EquipmentBelongTo.PREVIEW), modelTex);
        }
        //展示翅膀模型的时候直接开启一次固定的锁屏引导
        //Debug.Log("展示翅膀模型开启引导");
        
        //Debug.Log("当前玩家的Vip等级："+ GameCenter.vipMng.VipData.vLev);
        if(GameCenter.vipMng.VipData.vLev < 1)
        Invoke("DelayShow",2.0f);
    }
    /// <summary>
    /// 展示模型
    /// </summary>
    void ShowModel()
    {
        switch (GameCenter.wingMng.modelType)
        {
            case ModelType.WING: 
                WingInfo _wingInfo = GameCenter.wingMng.needShowWingInfo;
                if (_wingInfo != null)//刷新通用翅膀模型
                {
                    GameCenter.previewManager.TryPreviewSingleEquipment(new EquipmentInfo(_wingInfo.refData.itemui, EquipmentBelongTo.PREVIEW), modelTex);
                    if (nameLabel != null) nameLabel.text = "[b]" + _wingInfo.WingName;
                }
                break;
            case ModelType.ILLUSION:
            case ModelType.MOUNT:
                MountInfo _mountInfo = GameCenter.wingMng.needShowMountInfo;
                if (_mountInfo != null)
                {
                    GameCenter.previewManager.TryPreviewSingelMount(_mountInfo, modelTex);
                    if (nameLabel != null) nameLabel.text = "[b]" + _mountInfo.MountName;
                }
                break;
            case ModelType.PET:
                MercenaryInfo _petInfo = GameCenter.wingMng.needShowPetInfo;
                if (_petInfo != null)
                {
                    GameCenter.previewManager.TryPreviewSingelEntourage(_petInfo, modelTex);
                    if (nameLabel != null) nameLabel.text = "[b]" + _petInfo.PetName;
                }
                break;
        }
    }
    /// <summary>
    /// 延迟翅膀引导
    /// </summary>
    void DelayShow()
    {
      GameCenter.noviceGuideMng.OpenGuide(100041, 1);
    }
}
