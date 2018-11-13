//======================================================
//作者:鲁家旗
//日期:2016.6.24
//用途:礼包领取ItemUI
//======================================================
using UnityEngine;
using System.Collections;

public class GiftItemUI : MonoBehaviour {
    public UISprite giftSp;
    public UILabel giftName;
    /// <summary>
    /// 刷新礼包
    /// </summary>
    public void RefreshGift(CDKeyRef _data)
    {
        if (giftSp != null) giftSp.spriteName = _data.icon;
        if (giftName != null) giftName.text = _data.name;
    }
}
