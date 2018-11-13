//==============================================
//作者：鲁家旗
//日期：2016/3/4
//用途：法宝界面
//=================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MagicWeaponWnd : GUIBase
{
    #region 控件对象
    /// <summary>
    /// 关闭按钮
    /// </summary>
    public UIButton btnClose;
    /// <summary>
    /// 佩戴按钮
    /// </summary>
    public UIButton btnWear;
    /// <summary>
    /// 卸下按钮
    /// </summary>
    public UIButton btnUnload;
    /// <summary>
    /// 激活按钮
    /// </summary>
    public UIButton btnActive;
    /// <summary>
    /// 淬炼页签
    /// </summary>
    public UIToggle toggleRefine;
    /// <summary>
    /// 注灵页签
    /// </summary>
    public UIToggle toggleAddSoul;
    /// <summary>
    /// 淬炼标题
    /// </summary>
    public UISprite labelRefineTilid;
    /// <summary>
    /// 注灵标题
    /// </summary>
    public UISprite labelAddSoulTilid;
    /// <summary>
    /// 提升按钮
    /// </summary>
    public UIButton promoteBtn;
    /// <summary>
    /// 注灵按钮
    /// </summary>
    public UIButton addSoulBtn;
    /// <summary>
    /// 底下淬炼描述信息
    /// </summary>
    public UILabel refineLabel;
    /// <summary>
    /// 淬炼法宝未解锁
    /// </summary>
    public UILabel notRefinelabel;
    /// <summary>
    /// 注灵描述
    /// </summary>
    public UILabel addSoulLabel;
    /// <summary>
    /// 注灵法宝未解锁时
    /// </summary>
    public UILabel notAddSoulLabel;
    /// <summary>
    /// 注灵点不中时，弹窗提示点击按钮
    /// </summary>
    public UIButton clickBtn;
    /// <summary>
    /// 属性界面脚本
    /// </summary>
    public MagicAttributeUI magicAttributeUI;
    /// <summary>
    /// 提升界面脚本
    /// </summary>
    public MagicAttrbuteChangeUI changeUI;

    /// <summary>
    /// 要实例化的法宝
    /// </summary>
    public GameObject magicGo;
    //信息列表
    protected List<MagicWeaponInfo> refList = new List<MagicWeaponInfo>();
    //法宝列表
    protected List<GameObject> magicGoList = new List<GameObject>();
    //法宝数据
    protected FDictionary MagicListDic
    {
        get
        {
            return GameCenter.magicWeaponMng.magicInfoDic;
        }
    }
    public GameObject openPrivew;
    public GameObject privewGo;
    public UIButton btnPrivewClose;
    protected int clickMagic = 0;
    /// <summary>
    /// 当前选中的法宝信息
    /// </summary>
    protected MagicWeaponInfo MagicWeaponInfo
    {
        get
        {
            if (MagicListDic.ContainsKey(clickMagic))
            {
                return MagicListDic[clickMagic] as MagicWeaponInfo;
            }
            return MagicListDic[refList[0].Type] as MagicWeaponInfo;
        }
    }
    protected bool isActive = false;
    protected int curItemID = 0;
    public UITexture model;
    //public Load3DObject model;
    public Load3DObject model1;
    public Load3DObject model2;
    public Load3DObject model3;
    public GameObject show;
    public RefineSubWnd refineSubWnd;
    public AddSoulSubWnd addsoulSubWnd;
    #endregion


    #region 构造
    void Awake()
    {
        layer = GUIZLayer.NORMALWINDOW;
        mutualExclusion = true;
        //关闭窗口
        if(btnClose != null) UIEventListener.Get(btnClose.gameObject).onClick = delegate
        {
            GameCenter.uIMng.SwitchToUI(GUIType.NONE);
        };
        if (btnActive != null) UIEventListener.Get(btnActive.gameObject).onClick = OnClickBtnActive;
        if (btnWear != null) UIEventListener.Get(btnWear.gameObject).onClick = OnClickWearBtn;
        if (btnUnload != null) UIEventListener.Get(btnUnload.gameObject).onClick = OnClickUnloadBtn;

        //法宝到达2阶解锁注灵 提示
        if (clickBtn != null) UIEventListener.Get(clickBtn.gameObject).onClick = delegate
         {
             if (refList.Count != 0 && refList[0].RefineLev < 2 || refList[1].RefineLev < 2)
             {
                 GameCenter.messageMng.AddClientMsg(180);
             }
         };
        //打开预览界面
        if (openPrivew != null) UIEventListener.Get(openPrivew).onClick = (x)=>
        {
            RefreshModel();
        };
        if (btnPrivewClose != null) UIEventListener.Get(btnPrivewClose.gameObject).onClick = (x)=>
        {
            privewGo.SetActive(false);
        };
        if(show != null)show.SetActive(false);
        if (refineSubWnd != null) refineSubWnd.gameObject.SetActive(false);
        if (addsoulSubWnd != null) addsoulSubWnd.gameObject.SetActive(false);
	}
    protected override void OnOpen()
    {
        if (initSubGUIType == SubGUIType.NONE) initSubGUIType = SubGUIType.MAGICREFINE;
        base.OnOpen();
        //创建法宝
        CreateMagicUI();
        RefrshTog();
        OnUITogChange(null);
        for (int i = 0; i < magicGoList.Count; i++)
        {
            UIEventListener.Get(magicGoList[i].gameObject).onClick += OnUITogChange;
        }
        if (toggleRefine != null)
        {
            EventDelegate.Remove(toggleRefine.onChange, OnChange);
            EventDelegate.Add(toggleRefine.onChange, OnChange);
        }
        if (toggleAddSoul != null)
        {
            EventDelegate.Remove(toggleAddSoul.onChange, OnChange);
            EventDelegate.Add(toggleAddSoul.onChange, OnChange);
        }
        //属性变化事件
        GameCenter.magicWeaponMng.OnMagicTypeUpdate += RefreshAll;
    }
    protected override void OnClose()
    {
        base.OnClose();
        for (int i = 0; i < magicGoList.Count; i++)
        {
            UIEventListener.Get(magicGoList[i].gameObject).onClick -= OnUITogChange;
        }
        GameCenter.magicWeaponMng.OnMagicTypeUpdate -= RefreshAll;
        if (toggleRefine != null) EventDelegate.Remove(toggleRefine.onChange, OnChange);
        if (toggleAddSoul != null) EventDelegate.Remove(toggleAddSoul.onChange, OnChange);
        GameCenter.previewManager.ClearModel();
    }
    protected override void InitSubWndState()
    {
        base.InitSubWndState();
        switch (InitSubGUIType)
        { 
            case SubGUIType.MAGICREFINE:
                toggleRefine.value = true;
                break;
            case SubGUIType.MAGICADDSOUL:
                toggleAddSoul.value = true;
                break;
            default:
                break;
        }
    }
    #endregion

    #region 控件事件
    /// <summary>
    /// 激活按钮
    /// </summary>
    void OnClickBtnActive(GameObject go)
    {
        if (GameCenter.inventoryMng.GetNumberByType(MagicWeaponInfo.MagicId) < 1)
        {
            //上浮提示(道具不足)
            GameCenter.messageMng.AddClientMsg(141);
        }
        else
        {
            //激活法宝
            GameCenter.magicWeaponMng.C2S_RequestActiveMagic(MagicWeaponInfo.Type);
        }
    }
    /// <summary>
    /// 佩戴按钮
    /// </summary>
    /// <param name="obj"></param>
    void OnClickWearBtn(GameObject go)
    {
        if (MagicWeaponInfo != null && !MagicWeaponInfo.EquState)
        {
            GameCenter.magicWeaponMng.C2S_RequestWearMagic(MagicWeaponInfo.ConfigID);
        }
    }
    /// <summary>
    /// 卸下按钮
    /// </summary>
    /// <param name="obj"></param>
    void OnClickUnloadBtn(GameObject go)
    {
        if (MagicWeaponInfo != null && MagicWeaponInfo.EquState)
        {
            GameCenter.magicWeaponMng.C2S_RequestUnloadMagic(MagicWeaponInfo.ConfigID);
        }
    }
    #endregion

    #region 创建法宝与刷新
    /// <summary>
    /// 刷新法宝属性和模型情况
    /// </summary>
    void RefreshAll()
    {
        LabelDes();
        RefshButton();
        OnMagicTypeUpdate();
        RefrshMagicModel();
    }
    /// <summary>
    /// 创建法宝
    /// </summary>
    void CreateMagicUI()
    {
        int index = 0;
        foreach (MagicWeaponInfo data in MagicListDic.Values)
        {
            GameObject go = GameObject.Instantiate(magicGo);
            go.transform.parent = magicGo.transform.parent;
            go.transform.localScale = Vector3.one;
            go.transform.localPosition = new Vector3(142, 24 - index * 76, 0);
            //默认是选中虚灵珠
            if (index == 0) go.GetComponent<UIToggle>().startsActive = true;
            //EventDelegate.Remove(go.GetComponent<UIToggle>().onChange, OnUITogChange);
            //EventDelegate.Add(go.GetComponent<UIToggle>().onChange, OnUITogChange);
            magicGoList.Add(go);
            refList.Add(data);
            MagicToggleUI magicToggleUI = go.GetComponent<MagicToggleUI>();
            if (magicToggleUI != null)
                magicToggleUI.SetMagicInfo(data);
            index++;
        }
        magicGo.SetActive(false);
    }

    /// <summary>
    /// 淬炼和注灵页签的切换
    /// </summary>
    protected void OnChange()
    {
        refineSubWnd.CloseUI();
        addsoulSubWnd.CloseUI();
        if (toggleRefine.value)
        {
            refineSubWnd.OpenUI();
        }
        else
        {
            addsoulSubWnd.OpenUI();
        }
        OnMagicTypeUpdate();
        LabelDes();
        RefshButton();
    }
    /// <summary>
    /// 法宝之间的切换
    /// </summary>
    void OnUITogChange(GameObject go) 
    {
        for (int i = 0; i < magicGoList.Count; i++)
        {
            if (magicGoList[i].GetComponent<UIToggle>().value)
            {
                //刷新法宝属性
                magicAttributeUI.SetMagicAttribute(refList[i]);
                //刷新提升界面
                changeUI.SetMagicAttributeChange(refList[i]);
                //把点击的是哪个法宝存储起来
                MagicToggleUI magicToggleUI = magicGoList[i].GetComponent<MagicToggleUI>();
                if (magicToggleUI != null)
                    clickMagic = magicToggleUI.GetData.Type;
                //标签开关
                magicGoList[i].transform.FindChild("TagSprite").gameObject.SetActive(true);
                //预览模型
                if (model != null && magicToggleUI != null)
                {
                    //model.configID = magicToggleUI.GetData.ItemID;
                    //model.StartLoad();
                    GameCenter.previewManager.TryPreviewSingleEquipment(new EquipmentInfo(magicToggleUI.GetData.ItemID, EquipmentBelongTo.PREVIEW), model);
                }
            }
            else if (!magicGoList[i].GetComponent<UIToggle>().value)
            {
                magicGoList[i].transform.FindChild("TagSprite").gameObject.SetActive(false);
            }
        }
        //刷新按钮
        RefshButton();
        LabelDes();
    }
    /// <summary>
    /// 刷新界面
    /// </summary>
    void OnMagicTypeUpdate()
    {
        //数据变更，刷新界面
        for (int i = 0; i < magicGoList.Count; i++)
        {
            //刷新法宝
            magicGoList[i].GetComponent<MagicToggleUI>().SetMagicInfo(refList[i]);
        }
        if (!isActive)
        {
            for (int i = 0; i < refList.Count; i++)
            {
                if (refList[i].RefineLev >= 2)
                {
                    isActive = true;
                    if (toggleAddSoul != null) toggleAddSoul.GetComponent<BoxCollider>().enabled = isActive;
                    break;
                }
            }
        }
        //刷新属性
        magicAttributeUI.SetMagicAttribute(MagicWeaponInfo);
        //刷新提升界面
        changeUI.SetMagicAttributeChange(MagicWeaponInfo);
    }
    /// <summary>
    /// 当两个法宝都没达到2阶时，注灵页签选不中
    /// </summary>
    void RefrshTog()
    {
        for (int i = 0; i < refList.Count; i++)
        {
            if (refList[i].RefineLev < 2)
                isActive = false;
            else
            {
                isActive = true;
                break;
            }
        }
        if (toggleAddSoul != null) toggleAddSoul.GetComponent<BoxCollider>().enabled = isActive;
    }
    /// <summary>
    /// 刷新按钮
    /// </summary>
    void RefshButton()
    {
        //标题的显示
        if (toggleRefine.value)
        {
            if (labelRefineTilid != null) labelRefineTilid.enabled = true;
            if (labelAddSoulTilid != null) labelAddSoulTilid.enabled = false;
        }
        else
        {
            if (labelRefineTilid != null) labelRefineTilid.enabled = false;
            if (labelAddSoulTilid != null) labelAddSoulTilid.enabled = true;
        }
        //按钮的刷新
        if (!MagicWeaponInfo.EquActive)
        {
            if (btnActive != null) btnActive.gameObject.SetActive(true);
            if (btnUnload != null) btnUnload.gameObject.SetActive(false);
            if (btnWear != null) btnWear.gameObject.SetActive(false);

            if (promoteBtn != null) promoteBtn.gameObject.SetActive(false);
            if (addSoulBtn != null) addSoulBtn.gameObject.SetActive(false);
        }
        else if (!MagicWeaponInfo.EquState)
        {
            if (btnActive != null) btnActive.gameObject.SetActive(false);
            if (btnWear != null) btnWear.gameObject.SetActive(true);
            if (btnUnload != null) btnUnload.gameObject.SetActive(false);
        }
        else if (MagicWeaponInfo.EquState)
        {
            if (btnActive != null) btnActive.gameObject.SetActive(false);
            if (btnWear != null) btnWear.gameObject.SetActive(false);
            if (btnUnload != null) btnUnload.gameObject.SetActive(true);
        }
    }
    
    /// <summary>
    /// 升级动态刷新模型
    /// </summary>
    void RefrshMagicModel()
    {
        if (MagicWeaponInfo.ItemID != MagicWeaponInfo.NextItemID)
        {
            curItemID = MagicWeaponInfo.NextItemID;
        }
        if (model != null && MagicWeaponInfo != null && MagicWeaponInfo.ItemID == curItemID)
        {
            curItemID = 0;
            //model.configID = MagicWeaponInfo.ItemID;
            //model.StartLoad();
            GameCenter.previewManager.ClearModel();
            GameCenter.previewManager.TryPreviewSingleEquipment(new EquipmentInfo(MagicWeaponInfo.ItemID, EquipmentBelongTo.PREVIEW), model);
        }
    }
    /// <summary>
    /// 底层描述信息
    /// </summary>
    void LabelDes()
    {
        //选中注灵，法宝没激活时
        if (toggleAddSoul.value && !MagicWeaponInfo.EquActive)
        {
            if (notAddSoulLabel != null) notAddSoulLabel.enabled = true;
            if (addSoulLabel != null) addSoulLabel.enabled = false;
            if (refineLabel != null) refineLabel.enabled = false;
            if (notRefinelabel != null) notRefinelabel.enabled = false;
        }//选中注灵，法宝激活时
        else if (toggleAddSoul.value && MagicWeaponInfo.EquActive)
        {
            if (notAddSoulLabel != null) notAddSoulLabel.enabled = false;
            if (addSoulLabel != null) addSoulLabel.enabled = true;
            if (refineLabel != null) refineLabel.enabled = false;
            if (notRefinelabel != null) notRefinelabel.enabled = false;
        }//选中淬炼，法宝激活时
        else if (toggleRefine.value && MagicWeaponInfo.EquActive)
        {
            if (notAddSoulLabel != null) notAddSoulLabel.enabled = false;
            if (addSoulLabel != null) addSoulLabel.enabled = false;
            if (refineLabel != null) refineLabel.enabled = true;
            if (notRefinelabel != null) notRefinelabel.enabled = false;
        }//选中淬炼，法宝没激活时
        else if (toggleRefine.value && !MagicWeaponInfo.EquActive )
        {
            if (notAddSoulLabel != null) notAddSoulLabel.enabled = false;
            if (addSoulLabel != null) addSoulLabel.enabled = false;
            if (refineLabel != null) refineLabel.enabled = false;
            if (notRefinelabel != null) notRefinelabel.enabled = true;
        }
    }
    /// <summary>
    /// 刷新静态模型
    /// </summary>
    void RefreshModel()
    {
        if (privewGo != null) privewGo.SetActive(true);
        if (MagicWeaponInfo.Type == 1)
        {
            RefineRef data1 = ConfigMng.Instance.GetRefineRef(1, 3, 0);
            if (data1 != null && model1 != null)
            {
                model1.configID = data1.model;
                model1.StartLoad();
            }

            RefineRef data2 = ConfigMng.Instance.GetRefineRef(1, 5, 0);
            if (data2 != null && model2 != null)
            {
                model2.configID = data2.model;
                model2.StartLoad();
            }
            RefineRef data3 = ConfigMng.Instance.GetRefineRef(1, 7, 0);
            if (data3 != null && model3 != null)
            {
                model3.configID = data3.model;
                model3.StartLoad();
            }
        }
        else
        {
            RefineRef data1 = ConfigMng.Instance.GetRefineRef(2, 3, 0);
            if (data1 != null && model1 != null)
            {
                model1.configID = data1.model;
                model1.StartLoad();
            }
            RefineRef data2 = ConfigMng.Instance.GetRefineRef(2, 5, 0);
            if (data2 != null && model2 != null)
            {
                model2.configID = data2.model;
                model2.StartLoad();
            }
            RefineRef data3 = ConfigMng.Instance.GetRefineRef(2, 7, 0);
            if (data3 != null && model3 != null)
            {
                model3.configID = data3.model;
                model3.StartLoad();
            }
        }
    }
    #endregion
}
