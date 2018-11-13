//====================
//作者：鲁家旗
//日期：2016/3/7
//用途：法宝属性UI
//====================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MagicAttributeUI : MonoBehaviour {
    #region 控件数据
    /// <summary>
    /// 法宝名字
    /// </summary>
    public UILabel nameLabel;
    /// <summary>
    /// 战力
    /// </summary>
    public UILabel powerLabel;
    /// <summary>
    /// 属性
    /// </summary>
    public List<UILabel> attributeList = new List<UILabel>();
    /// <summary>
    /// 属性数值
    /// </summary>
    public List<UILabel> numList = new List<UILabel>();
    /// <summary>
    /// 上升箭头
    /// </summary>
    public List<UISprite> addSp = new List<UISprite>();
    /// <summary>
    /// 增加的属性值
    /// </summary>
    public List<UILabel> addAttriList = new List<UILabel>();
    /// <summary>
    /// 技能图片
    /// </summary>
    public UISpriteEx skillIcon;
    /// <summary>
    /// 技能说明
    /// </summary>
    public UILabel skillLabel;
    /// <summary>
    /// 提升按钮
    /// </summary>
    public UIButton promoteButton;
    /// <summary>
    /// 注灵按钮
    /// </summary>
    public UIButton addSoulButton;
    /// <summary>
    /// 提升界面
    /// </summary>
    public GameObject promoteGo;
    /// <summary>
    /// 属性界面
    /// </summary>
    public GameObject attrubuteGo;
    /// <summary>
    /// 淬炼页签
    /// </summary>
    public UIToggle toggleTemper;
    /// <summary>
    /// 注灵页签
    /// </summary>
    public UIToggle toggleAddSoul;
    /// <summary>
    /// 提升界面脚本
    /// </summary>
    public MagicAttrbuteChangeUI changeUI;
    /// <summary>
    /// 激活材料获取途径
    /// </summary>
    public UIButton materialBtn;
    /// <summary>
    /// 点击任意点关闭该页面
    /// </summary>
    public UIButton closePromoteBtn;
    /// <summary>
    /// 激活条件信息
    /// </summary>
    public UILabel activeInfo;
    /// <summary>
    /// 激活物品名字
    /// </summary>
    public UILabel activeName;

    protected MagicWeaponInfo info = null;
    #endregion

    void Awake()
    {
        if (promoteButton != null) EventDelegate.Add(promoteButton.onClick, OnClickPromoteBtn);
        if (addSoulButton != null) EventDelegate.Add(addSoulButton.onClick, OnClickZhuLingBtn);
        if (materialBtn != null) EventDelegate.Add(materialBtn.onClick, OnClickMaterialBtn);
        if (closePromoteBtn != null) EventDelegate.Add(closePromoteBtn.onClick, OnClickCloseProBtn);
    }
    /// <summary>
    /// 点击提升按钮后
    /// </summary>
    /// <param name="obj"></param>
    void OnClickPromoteBtn()
    {
        promoteGo.SetActive(true);
        attrubuteGo.SetActive(false);
        //刷新提升界面
        changeUI.SetMagicAttributeChange(info);
    }
    /// <summary>
    /// 点击注灵按钮后
    /// </summary>
    void OnClickZhuLingBtn()
    {
        promoteGo.SetActive(true);
        attrubuteGo.SetActive(false);
        //刷新注灵界面
        changeUI.SetMagicAttributeChange(info);
    }
    /// <summary>
    /// 激活材料获取途径
    /// </summary>
    void OnClickMaterialBtn()
    {
        ToolTipMng.ShowEquipmentTooltip(new EquipmentInfo(info.MagicId, EquipmentBelongTo.PREVIEW), ItemActionType.None, ItemActionType.None, ItemActionType.None, ItemActionType.None);
    }
    /// <summary>
    /// 点击提升界面任意点关闭提升界面，打开属性界面
    /// </summary>
    void OnClickCloseProBtn()
    {
        promoteGo.SetActive(false);
        attrubuteGo.SetActive(true);
    }

    #region 刷新
    public void SetMagicAttribute(MagicWeaponInfo _info)
    {
        info = _info;
        //法宝名字
        if(nameLabel != null) nameLabel.text = _info.Name;

        if (activeName != null) activeName.text = "[u]" + _info.Name;
        int attributeNum = 0;
        ////淬炼
        if (toggleTemper.value)
        {
            //注灵按钮不可见
            addSoulButton.gameObject.SetActive(false);
            //提升按钮可见
            if (_info.EquActive)
                promoteButton.gameObject.SetActive(true);
            else
            {
                promoteGo.SetActive(false);
                attrubuteGo.SetActive(true);
            }
            //淬炼战斗力
            if (powerLabel != null)
            {
                powerLabel.text = "" + _info.RefineFightPower;
            }
            attributeNum = _info.RefineAttributeType.Count;
            //淬炼属性
            for (int i = 0; i < attributeList.Count; i++)
            {
                if (i < _info.RefineAttributeType.Count && attributeList[i] != null)
                {
                    attributeList[i].text = "[00ff00]" + _info.RefineAttributeType[i] + ":";
                    attributeList[i].gameObject.SetActive(true);
                }
                else
                    attributeList[i].gameObject.SetActive(false);
            }
            //淬炼属性数值
            for (int i = 0; i < numList.Count; i++)
            {
                if (i < _info.RefineAttributeNum.Count && numList[i] != null)
                {
                    numList[i].text = "+" + _info.RefineAttributeNum[i];
                    numList[i].gameObject.SetActive(true);
                }
                else
                    numList[i].gameObject.SetActive(false);
            }
            if (_info.EquActive)
            {
                //淬炼增加的属性数值
                for (int i = 0; i < addAttriList.Count; i++)
                {
                    if (i < _info.RefineAddAttribute.Count && _info.RefineAddAttribute[i] >= 0 && addAttriList[i] != null)
                        addAttriList[i].text = "+" + _info.RefineAddAttribute[i];
                    else if (i < _info.RefineAddAttribute.Count && _info.RefineAddAttribute[i] < 0)
                        addAttriList[i].text = _info.RefineAddAttribute[i].ToString();
                    else
                        addAttriList[i].gameObject.SetActive(false);
                }
            }
        }
        ////注灵
        else if (toggleAddSoul.value)
        {
            //注灵按钮可见
            if (_info.RefineLev > 1)
                addSoulButton.gameObject.SetActive(true);
            else
            {
                addSoulButton.gameObject.SetActive(false);
                //错误提示(注灵在法宝2阶时开启)
                GameCenter.messageMng.AddClientMsg(140);
                promoteGo.SetActive(false);
                attrubuteGo.SetActive(true);
            }
            //提升按钮不可见
            promoteButton.gameObject.SetActive(false);
            //注灵战斗力
            if (powerLabel != null)
            {
                powerLabel.text = "" + _info.AddSoulFightPower;
            }
            attributeNum = _info.AddSoulAttributeType.Count;
            //注灵属性
            for (int i = 0; i < attributeList.Count; i++)
            {
                if (i < _info.AddSoulAttributeType.Count && attributeList[i] != null)
                {
                    attributeList[i].text = "[00ff00]" + _info.AddSoulAttributeType[i] + ":";
                    attributeList[i].gameObject.SetActive(true);
                }
                else
                    attributeList[i].gameObject.SetActive(false);
            }
            //注灵数值
            for (int i = 0; i < numList.Count; i++)
            {
                if (i < _info.AddSoulAttributeNum.Count && numList[i] != null)
                {
                    numList[i].text = "+" + _info.AddSoulAttributeNum[i];
                    numList[i].gameObject.SetActive(true);
                }
                else
                    numList[i].gameObject.SetActive(false);
            }
            //注灵增加的属性数值
            if (_info.EquActive)
            {
                for (int i = 0; i < addAttriList.Count; i++)
                {
                    if (i < _info.AddSoulAddAttribute.Count && _info.AddSoulAddAttribute[i] >= 0 && addAttriList[i] != null)
                        addAttriList[i].text = "+" + _info.AddSoulAddAttribute[i];
                    else if (i < _info.AddSoulAddAttribute.Count && _info.AddSoulAddAttribute[i] < 0)
                        addAttriList[i].text = _info.AddSoulAddAttribute[i].ToString();
                    else
                        addAttriList[i].gameObject.SetActive(false);
                }
            }
        }
        //属性上升箭头
        for (int i = 0; i < addSp.Count; i++)
        {
            //淬炼满级
            if (toggleTemper.value && _info.RefineLev == GameCenter.magicWeaponMng.maxLev && _info.RefineStar == GameCenter.magicWeaponMng.maxStar)
            {
                addSp[i].gameObject.SetActive(false);
                addAttriList[i].gameObject.SetActive(false);
            }
            //注灵满级
            else if (toggleAddSoul.value && _info.AddSoulLev == GameCenter.magicWeaponMng.maxLev && _info.AddSoulStar == GameCenter.magicWeaponMng.maxStar)
            {
                addSp[i].gameObject.SetActive(false);
                addAttriList[i].gameObject.SetActive(false);
            }
            else
            {
                if (addSp[i] != null && _info.EquActive && i < attributeNum)
                {
                    addSp[i].gameObject.SetActive(true);
                    addAttriList[i].gameObject.SetActive(true);
                }
                else
                {
                    addSp[i].gameObject.SetActive(false);
                    addAttriList[i].gameObject.SetActive(false);
                }
            }
         }
        
        if (skillIcon != null)
        {
            skillIcon.spriteName = _info.SkillIcon;
        }
        //技能图片
        if (skillIcon != null && _info.EquActive)
        {
            skillIcon.IsGray = UISpriteEx.ColorGray.normal;
        }
        else
        {
            skillIcon.IsGray = UISpriteEx.ColorGray.Gray;
        }
        //技能描述
        if (skillLabel != null)
        {
            skillLabel.text = _info.SkillDes;
        }
        //激活条件信息
        if (activeInfo != null && _info.EquActive)
        {
            activeInfo.gameObject.SetActive(false);
        }
        else
        {
            activeInfo.gameObject.SetActive(true);
        }
    }
    #endregion

    void OnDestroy()
    {
        if (promoteButton != null) EventDelegate.Remove(promoteButton.onClick, OnClickPromoteBtn);
        if (addSoulButton != null) EventDelegate.Remove(addSoulButton.onClick, OnClickZhuLingBtn);
        if (materialBtn != null) EventDelegate.Remove(materialBtn.onClick, OnClickMaterialBtn);
        if (closePromoteBtn != null) EventDelegate.Remove(closePromoteBtn.onClick, OnClickCloseProBtn);
    }
}
