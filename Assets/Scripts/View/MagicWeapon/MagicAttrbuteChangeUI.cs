//==================================
//作者：鲁家旗
//日期：2016/3/7
//用途：法宝提升界面
//=================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;

public class MagicAttrbuteChangeUI : MonoBehaviour
{
    #region 控件数据
    /// <summary>
    /// 提升按钮
    /// </summary>
    public UIButton promoteButton;
    /// <summary>
    /// 注灵按钮
    /// </summary>
    public UIButton addSoulButton;
    /// <summary>
    /// 属性
    /// </summary>
    public List<UILabel> attributeList = new List<UILabel>();
    /// <summary>
    /// 数值
    /// </summary>
    public List<UILabel> numList = new List<UILabel>();
    /// <summary>
    /// 属性提升箭头
    /// </summary>
    public List<UISprite> addSp = new List<UISprite>();
    /// <summary>
    /// 增加的数值
    /// </summary>
    public List<UILabel> addNumList = new List<UILabel>();
    /// <summary>
    /// 技能标题
    /// </summary>
    public UILabel skillLabel;
    /// <summary>
    /// 技能描述
    /// </summary>
    public UILabel skillDesLabel;
    /// <summary>
    /// 经验条
    /// </summary>
    public UISlider expSlider;
    /// <summary>
    /// 星星
    /// </summary>
    public List<UISpriteEx> starList = new List<UISpriteEx>();
    /// <summary>
    /// 材料名字
    /// </summary>
    public UILabel consumName;
    /// <summary>
    /// 材料和铜钱数量
    /// </summary>
    public List<UILabel> consumNum = new List<UILabel>();
    /// <summary>
    /// 消耗材料按钮
    /// </summary>
    public UIButton consumBtn;
    /// <summary>
    /// 材料所需的元宝
    /// </summary>
    public UILabel consumYb;

    /// <summary>
    /// 淬炼页签
    /// </summary>
    public UIToggle toggleTemper;
    /// <summary>
    /// 注灵页签
    /// </summary>
    public UIToggle toggleAddSoul;
    /// <summary>
    /// 是否勾选上使用元宝
    /// </summary>
    public UIToggle toggleYb;
    /// <summary>
    /// 经验条上的百分比
    /// </summary>
    public UILabel numLabel;

    //标志位
    //是否是淬炼提升(亮星)
    protected bool isRefinePromote = false;
    //是否是注灵提升
    protected bool isAddSoulPromote = false;

    //老等级
    protected int oldLev = 0;
    //是否升阶成功
    protected bool isAddLev = false;
    //老星级
    protected int oldStar = 0;
    //是否升星成功
    protected bool isAddStar = false;

    protected MagicWeaponInfo data;
    /// <summary>
    /// 进度特效
    /// </summary>
    public UIFxAutoActive effect;
    #endregion

    void OnEnable()
    {
        if (promoteButton != null) EventDelegate.Add(promoteButton.onClick, OnClickProButton);
        if (addSoulButton != null) EventDelegate.Add(addSoulButton.onClick, OnClickAddSoulButton);
        if (consumBtn != null) EventDelegate.Add(consumBtn.onClick, OnClickConsum);
        GameCenter.magicWeaponMng.OnProgressChange -= showEffect;
        GameCenter.magicWeaponMng.OnProgressChange += showEffect;
    }
    void OnDisable()
    {
        if (promoteButton != null) EventDelegate.Remove(promoteButton.onClick, OnClickProButton);
        if (addSoulButton != null) EventDelegate.Remove(addSoulButton.onClick, OnClickAddSoulButton);
        if (consumBtn != null) EventDelegate.Remove(consumBtn.onClick, OnClickConsum);
        GameCenter.magicWeaponMng.OnProgressChange -= showEffect;
        //CancelInvoke("HideStar");
    }
    /// <summary>
    /// 淬炼
    /// </summary>
    void OnClickProButton()
    {
        //if (data.RefineLev == GameCenter.magicWeaponMng.MaxMagicLev() && data.RefineStar == GameCenter.magicWeaponMng.MaxMagicStar())
        //{
        //    Debug.Log("提示法宝已达到满级！！！");
        //    starList[starList.Count - 1].gameObject.SetActive(false);
        //    return;
        //}
        oldStar = data.RefineStar;
        oldLev = data.RefineLev;
        isRefinePromote = true;
        //材料不足提示
        int num = GameCenter.inventoryMng.GetNumberByType(data.ConsumeItemId);
        if (data.ConsumeItemNum > num && toggleYb.value == false )//材料不足且没勾选上用元宝购买的按钮
        {
            //添加错误提示(是否花费元宝购买)
            MessageST mst = new MessageST();
            mst.messID = 142;
            mst.words = new string[1] { data.ConsumePrice.ToString() };
            mst.delYes = RefineUseYB;
            GameCenter.messageMng.AddClientMsg(mst);
        }
        else if (data.ConsumeItemNum > num && toggleYb.value)//材料不足且勾选上直接用元宝购买的按钮
        {
            RefineUseYB(null);
        }
        // 绑铜不足的提示
        else if ((ulong)data.ConsumeCoinNum > GetAllCoin())
        {
            RefinePrompt();
        }
        else
        {
            GameCenter.magicWeaponMng.C2S_RequestAddMagicStar(data.ConfigID, false);
        }
    }
    /// <summary>
    /// 注灵
    /// </summary>
    void OnClickAddSoulButton()
    {
        //if (data.AddSoulLev == GameCenter.magicWeaponMng.MaxMagicLev() && data.AddSoulStar == GameCenter.magicWeaponMng.MaxMagicStar())
        //{
        //    Debug.Log("提示法宝已达到满级！！！");
        //    return;
        //}
        oldLev = data.AddSoulLev;
        oldStar = data.AddSoulStar;
        isAddSoulPromote = true;

        int num = GameCenter.inventoryMng.GetNumberByType(data.AddSoulConsumeItemId);
        if (data.AddSoulConsumeItemNum > num && toggleYb.value == false)//材料不足且没勾选上用元宝购买的按钮
        {
            //添加错误提示(是否花费元宝购买)
            MessageST mst = new MessageST();
            mst.messID = 142;
            mst.delYes = AddSoulUseYB;
            mst.words = new string[1] { data.AddSoulConsumePrice.ToString() };
            GameCenter.messageMng.AddClientMsg(mst);
        }
        else if (data.AddSoulConsumeItemNum > num && toggleYb.value)//材料不足且勾选上直接用元宝购买的按钮
        {
            AddSoulUseYB(null);
        }
        //绑铜不足的提示 
        else if ((ulong)data.AddSoulConsumeCoinNum > GetAllCoin())
        {
            AddSoulPrompt();
        }
        else
        {
            GameCenter.magicWeaponMng.C2S_RequestAddMagicSoul(data.ConfigID, false);
        }
    }

    /// <summary>
    /// 淬炼使用元宝
    /// </summary>
    void RefineUseYB(object[] pars)
    {
        if ((ulong)data.ConsumePrice > GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount)// (元宝)
        {
            MessageST mst = new MessageST();
            mst.messID = 137;
            mst.delYes = delegate
            {
                //充值界面
                GameCenter.uIMng.SwitchToUI(GUIType.RECHARGE);
            };
            GameCenter.messageMng.AddClientMsg(mst);
        }
        // 绑铜不足的提示
        else if ((ulong)data.ConsumeCoinNum > GetAllCoin())
        {
            RefinePrompt();
        }
        else
        {
            GameCenter.magicWeaponMng.C2S_RequestAddMagicStar(data.ConfigID, true);
        }
    }
    /// <summary>
    /// 注灵使用元宝
    /// </summary>
    void AddSoulUseYB(object[] pars)
    {
        if ((ulong)data.AddSoulConsumePrice > GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount)//(判断元宝是否充足)
        {
            MessageST mst = new MessageST();
            mst.messID = 137;
            mst.delYes = delegate
            {
                //充值界面
                GameCenter.uIMng.SwitchToUI(GUIType.RECHARGE);
            };
            GameCenter.messageMng.AddClientMsg(mst);
        }
        //绑铜不足的提示 
        else if ((ulong)data.AddSoulConsumeCoinNum > GetAllCoin())
        {
            AddSoulPrompt();
        }
        else
        {
            GameCenter.magicWeaponMng.C2S_RequestAddMagicSoul(data.ConfigID, true);
        }
    }
    /// <summary>
    /// 淬炼绑铜不足提示
    /// </summary>
    void RefinePrompt()
    {
        MessageST mst = new MessageST();
        mst.messID = 12;
        EquipmentRef equipRef = ConfigMng.Instance.GetEquipmentRef(data.ConsumeCoinId);
        mst.words = new string[1] { equipRef == null ? string.Empty : equipRef.name };
        GameCenter.messageMng.AddClientMsg(mst);
    }
    /// <summary>
    /// 注灵绑铜不足提示
    /// </summary>
    void AddSoulPrompt()
    {
        MessageST mst = new MessageST();
        mst.messID = 12;
        EquipmentRef equipRef = ConfigMng.Instance.GetEquipmentRef(data.AddSoulConsumeCoinId);
        mst.words = new string[1] { equipRef == null ? string.Empty : equipRef.name };
        GameCenter.messageMng.AddClientMsg(mst);
    }
    /// <summary>
    /// 点击材料,热显
    /// </summary>
    void OnClickConsum()
    {
        if (toggleTemper.value)
        {
            ToolTipMng.ShowEquipmentTooltip(new EquipmentInfo(data.ConsumeItemId, EquipmentBelongTo.PREVIEW), ItemActionType.None, ItemActionType.None, ItemActionType.None, ItemActionType.None);
        }
        else
        {
            ToolTipMng.ShowEquipmentTooltip(new EquipmentInfo(data.AddSoulConsumeItemId, EquipmentBelongTo.PREVIEW), ItemActionType.None, ItemActionType.None, ItemActionType.None, ItemActionType.None);
        }
    }
    #region 刷新
    void showEffect()
    {
        if (effect != null)
        {
            effect.ReShowFx();
        }
    }
    public void SetMagicAttributeChange(MagicWeaponInfo _info)
    {
        data = _info;
        int attributeNum = 0;
        //////淬炼
        if (toggleTemper.value && _info.EquActive)
        {
            promoteButton.gameObject.SetActive(true);
            addSoulButton.gameObject.SetActive(false);
            //是否升阶成功
            if (oldLev < GameCenter.magicWeaponMng.newRefineLev && isRefinePromote)
            {
                isAddLev = true;
                isRefinePromote = false;
            }
            //淬炼升星成功
            if (oldStar < GameCenter.magicWeaponMng.newRefineStar && isRefinePromote)
            {
                isAddStar = true;
                isRefinePromote = false;
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
            //淬炼属性值
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
            //淬炼增加的属性数值
            for (int i = 0; i < addNumList.Count; i++)
            {
                if (i < _info.RefineAddAttribute.Count && _info.RefineAddAttribute[i] >= 0 && addNumList[i] != null)
                {
                    addNumList[i].text = "+" + _info.RefineAddAttribute[i];
                    addNumList[i].gameObject.SetActive(true);
                }
                else if (i < _info.RefineAddAttribute.Count && _info.RefineAddAttribute[i] < 0)
                {
                    addNumList[i].text = _info.RefineAddAttribute[i].ToString();
                    addNumList[i].gameObject.SetActive(true);
                }
                else
                    addNumList[i].gameObject.SetActive(false);
            }
            //战力增加(错误提示)
            if (_info.RefineAddFightPower > 0 && isAddStar)
            {
                MessageST mst = new MessageST();
                mst.messID = 28;
                mst.words = new string[1] { _info.RefineAddFightPower.ToString() };
                GameCenter.messageMng.AddClientMsg(mst);
                isAddStar = false;
            }
            else if (isAddStar)//战力减少
            {
                MessageST mst = new MessageST();
                mst.messID = 29;
                mst.words = new string[1] { _info.RefineAddFightPower.ToString() };
                GameCenter.messageMng.AddClientMsg(mst);
                isAddStar = false;
            }
            //百分比显示
            if (numLabel != null)
            {
                numLabel.text = _info.RefineRandomExp +"/100";
            }
            //经验条
            if (expSlider != null)
            {
                expSlider.value = (float)_info.RefineRandomExp / 100;
            }
            //到达满级最后一颗星隐藏掉
            //if (_info.RefineLev == GameCenter.magicWeaponMng.MaxMagicLev() && _info.RefineStar == GameCenter.magicWeaponMng.MaxMagicStar())
            //    starList[starList.Count - 1].gameObject.SetActive(false);
            //else
            //    starList[starList.Count - 1].gameObject.SetActive(true);
            //亮星
            for (int i = 0; i < starList.Count; i++)
            {
                if (i < _info.RefineStar)
                    starList[i].IsGray = UISpriteEx.ColorGray.normal;
                else
                    starList[i].IsGray = UISpriteEx.ColorGray.Gray;
            }
            //全部亮一下
            //if (_info.RefineLev > 1 && _info.RefineStar == 0 && _info.RefineRandomExp == 0 && isAddLev)
            //{
            //    for (int i = 0; i < starList.Count; i++)
            //    {
            //        starList[i].IsGray = UISpriteEx.ColorGray.normal;
            //    }
            //    CancelInvoke("HideStar");
            //    Invoke("HideStar", 0.5f);
            //    isAddLev = false;
            //}
            //消耗品名
            if(consumName != null)
            {
                StringBuilder build = new StringBuilder();
                consumName.text = build.Append("[u][b][00ff00]").Append(_info.ConsumeNameList[1]).ToString();
            }
            //消耗数量
            if(consumNum[0] != null)
            {
                consumNum[0].text = _info.ConsumeCoinNum + "/" + ((ulong)_info.ConsumeCoinNum > GetAllCoin() ? "[ff0000]" : "[ffffff]") + GetAllCoin() + "[-]";
            }
            if (consumNum[1] != null)
            {
                int num = GameCenter.inventoryMng.GetNumberByType(_info.ConsumeItemId);
                consumNum[1].text = _info.ConsumeItemNum + "/" + (_info.ConsumeItemNum > num ? "[ff0000]" : "[ffffff]") + num + "[-]";
            }
            if (skillLabel != null)
            {
                skillLabel.gameObject.SetActive(false);
            }
            //元宝
            if (consumYb != null)
            {
                ulong ybNum = GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount;
                consumYb.text = _info.ConsumePrice + "/" + ((ulong)_info.ConsumePrice > ybNum ? "[ff0000]" : "[ffffff]") + ybNum;
            }
        }
        //////注灵(法宝等阶大于一阶时开启)
        else if (toggleAddSoul.value && _info.RefineLev > 1)
        {
            promoteButton.gameObject.SetActive(false);
            addSoulButton.gameObject.SetActive(true);
            //是否升阶成功
            if (oldLev < GameCenter.magicWeaponMng.newAddSoulLev && isAddSoulPromote)
            {
                isAddLev = true;
                isAddSoulPromote = false;
            }
            //注灵升星成功
            if (oldStar < GameCenter.magicWeaponMng.newAddSoulStar && isAddSoulPromote)
            {
                isAddStar = true;
                isAddSoulPromote = false;
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
            //注灵属性值
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
            for (int i = 0; i < addNumList.Count; i++)
            {
                if (i < _info.AddSoulAddAttribute.Count && _info.AddSoulAddAttribute[i] >= 0 && addNumList[i] != null)
                {
                    addNumList[i].text = "+" + _info.AddSoulAddAttribute[i];
                    addNumList[i].gameObject.SetActive(true);
                }
                else if (i < _info.AddSoulAddAttribute.Count && _info.AddSoulAddAttribute[i] < 0)
                {
                    addNumList[i].text = _info.AddSoulAddAttribute[i].ToString();
                    addNumList[i].gameObject.SetActive(true);
                }
                else
                    addNumList[i].gameObject.SetActive(false);
            }
            //战力增加(错误提示)
            if (_info.AddSoulAddFightPower > 0 && isAddStar)
            {
                MessageST mst = new MessageST();
                mst.messID = 28;
                mst.words = new string[1] { _info.AddSoulAddFightPower.ToString() };
                GameCenter.messageMng.AddClientMsg(mst);
                isAddStar = false;
            }
            else if (isAddStar)//战力减少
            {
                MessageST mst = new MessageST();
                mst.messID = 29;
                mst.words = new string[1] { _info.AddSoulAddFightPower.ToString() };
                GameCenter.messageMng.AddClientMsg(mst);
                isAddStar = false;
            }
            //百分比显示
            if (numLabel != null)
            {
                numLabel.text = _info.AddSoulExp + "/100";
            }
            //经验条
            if (expSlider != null)
            {
                expSlider.value = (float)_info.AddSoulExp / 100;
            }
            //到达满级最后一颗星隐藏掉
            //if (_info.AddSoulLev == GameCenter.magicWeaponMng.MaxMagicLev() && _info.AddSoulLev == GameCenter.magicWeaponMng.MaxMagicStar())
            //    starList[starList.Count - 1].gameObject.SetActive(false);
            //else
            //    starList[starList.Count - 1].gameObject.SetActive(true);
            //亮星
            for (int i = 0; i < starList.Count; i++)
            {
                if (i < _info.AddSoulStar)
                    starList[i].IsGray = UISpriteEx.ColorGray.normal;
                else
                    starList[i].IsGray = UISpriteEx.ColorGray.Gray;
            }
            //全部亮一下
            //if (_info.AddSoulLev > 1 && _info.AddSoulStar == 0 && _info.AddSoulExp == 0 && isAddLev)
            //{
            //    for (int i = 0; i < starList.Count; i++)
            //    {
            //        starList[i].IsGray = UISpriteEx.ColorGray.normal;
            //    }
            //    CancelInvoke("HideStar");
            //    Invoke("HideStar", 0.5f);
            //    isAddLev = false;
            //}
            //消耗品
            if (consumName != null)
            {
                consumName.text = "[u][b][00ff00]" + _info.AddSoulConsumeNameList[1];
            }
            //消耗数量
            if (consumNum[0] != null)
            {
                consumNum[0].text = _info.AddSoulConsumeCoinNum + "/" + ((ulong)_info.AddSoulConsumeCoinNum > GetAllCoin() ? "[ff0000]" : "[ffffff]") + GetAllCoin() + "[-]";
            }
            if (consumNum[1] != null)
            {
                int num = GameCenter.inventoryMng.GetNumberByType(_info.AddSoulConsumeItemId);
                consumNum[1].text = _info.AddSoulConsumeItemNum + "/" + (_info.AddSoulConsumeItemNum > num ? "[ff0000]" : "[ffffff]") + num + "[-]";
            }
            //元宝
            if (consumYb != null)
            {
                ulong ybNum = GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount;
                consumYb.text = _info.AddSoulConsumePrice + "/" + ((ulong)_info.AddSoulConsumePrice > ybNum ? "[ff0000]" : "[ffffff]") + ybNum;
            }
            skillLabel.gameObject.SetActive(true);
            // 技能
            if (skillDesLabel != null)
                skillDesLabel.text = _info.SkillDes;
        }
        //上升箭头
        for (int i = 0; i < addSp.Count; i++)
        {
            //到达满级
            if (toggleTemper.value && _info.RefineLev == GameCenter.magicWeaponMng.maxLev && _info.RefineStar == GameCenter.magicWeaponMng.maxStar)
                addSp[i].gameObject.SetActive(false);
            else if (toggleAddSoul.value && _info.AddSoulLev == GameCenter.magicWeaponMng.maxLev && _info.AddSoulStar == GameCenter.magicWeaponMng.maxStar)
                addSp[i].gameObject.SetActive(false);
            else
            {
                if (addSp[i] != null && i < attributeNum)
                    addSp[i].gameObject.SetActive(true);
                else
                    addSp[i].gameObject.SetActive(false);
            }
        }
    }
    /// <summary>
    /// 隐藏所有星星
    /// </summary>
    //void HideStar()
    //{
    //    for (int i = 0; i < starList.Count; i++)
    //    {
    //        starList[i].IsGray = UISpriteEx.ColorGray.Gray;
    //    }
    //}
    /// <summary>
    /// 获得所有铜币
    /// </summary>
    /// <returns></returns>
    ulong GetAllCoin()
    {
        return GameCenter.mainPlayerMng.MainPlayerInfo.TotalCoinCount;
    }
    #endregion
    
}
