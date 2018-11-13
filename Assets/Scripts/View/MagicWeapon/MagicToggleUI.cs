//====================
//作者：鲁家旗
//日期：2016/3/7
//用途：法宝页签
//====================
using UnityEngine;
using System.Collections;

public class MagicToggleUI : MonoBehaviour
{

    #region 控件数据
    /// <summary>
    /// 按钮名字
    /// </summary>
    public UILabel nameLabel;
    /// <summary>
    /// 法宝图片
    /// </summary>
    public UISpriteEx magicIcon;
    /// <summary>
    /// 标签
    /// </summary>
    public UILabel magicTag;
    /// <summary>
    /// 淬炼页签
    /// </summary>
    public UIToggle toggleTemper;
    /// <summary>
    /// 注灵页签
    /// </summary>
    public UIToggle toggleAddSoul;

    #endregion

    #region 刷新
    MagicWeaponInfo data = null;
    public MagicWeaponInfo GetData
    {
        get 
        {
            return data;
        }
    }
    public void SetMagicInfo(MagicWeaponInfo _info)
    {
        data = _info;
        //法宝图片
        if (magicIcon != null)
        {
            EquipmentInfo magicInfo = new EquipmentInfo(_info.MagicId, EquipmentBelongTo.PREVIEW);
            if (magicInfo != null)
                magicIcon.spriteName = magicInfo.IconName;
        }
        if(nameLabel != null) nameLabel.text =  _info.Name;
        //法宝名字
        if (!_info.EquActive && nameLabel != null && magicIcon != null)
        {
            nameLabel.color = Color.gray;
            magicIcon.IsGray = UISpriteEx.ColorGray.Gray;
        }
        else
        {
            nameLabel.color = Color.yellow;
            magicIcon.IsGray = UISpriteEx.ColorGray.normal;
        }
        if (toggleTemper.value)
        {
            //淬炼标签
            if(magicTag != null) magicTag.text = _info.RefineStageTag;
        }
        else if (toggleAddSoul.value)
        {
            //注灵标签
            if(magicTag != null) magicTag.text = _info.AddSoulStageTag;
        }
    }
    #endregion
}
