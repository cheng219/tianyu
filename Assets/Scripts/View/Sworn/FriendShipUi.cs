//==================================
//作者：朱素云
//日期：2016/5/6
//用途：结义UI
//=================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FriendShipUi : MonoBehaviour
{
    public UILabel friendShipLev;
    public UITexture friendShipIcon;
    public UISprite friendShipMark;
    public UILabel friendShipVal;
    public UILabel remindNeedVal;
    public UIInput friendShipSwear;
    public UIButton swearBtn;
    public UIButton attributeBtn; 

    public GameObject attrPop;
    public UIButton closeAttrPop;
    public UILabel[] curAttrLab;
    public UILabel[] nextAttrLab;

    SwornData swornData = null;
    public SwornData SwornData
    {
        get
        {
            return swornData;
        }
        set
        {
            if (value != null) swornData = value;
            swornData.OnSwornDataUpdate -= Show;
            swornData.OnSwornDataUpdate += Show;
            Show();
        }
    } 
	void OnDestroy()
	{
		ConfigMng.Instance.RemoveBigUIIcon("Pic_zhitongdaohe");
	}
    void Show()
    {
        if (friendShipLev != null) friendShipLev.text = swornData.playerSwornLev.ToString();
        if (friendShipIcon != null) if (friendShipIcon != null) ConfigMng.Instance.GetBigUIIcon("Pic_zhitongdaohe", delegate(Texture2D texture)
                {
                    friendShipIcon.mainTexture = texture;
                });
        if (friendShipMark != null) friendShipMark.spriteName = swornData.SwornIcon;
        if (friendShipVal != null) friendShipVal.text = swornData.brotherSworn.ToString();
        if (remindNeedVal != null) remindNeedVal.text = swornData.SwornNextVal.ToString();
        if (friendShipSwear != null) friendShipSwear.value = swornData.brotherOath;
        if(swearBtn!= null)UIEventListener.Get(swearBtn.gameObject).onClick = delegate
        {
            GameCenter.swornMng.C2S_ReqChangeOath(friendShipSwear.value);
        };
        if (attributeBtn != null) UIEventListener.Get(attributeBtn.gameObject).onClick = OnClickFriendShip;
        if (closeAttrPop != null) UIEventListener.Get(closeAttrPop.gameObject).onClick = delegate { attrPop.SetActive(false); };
    } 
    void SetText(UILabel[] _lab,List<float> _val)
    {
        for (int i = 0,max = _lab.Length; i < max; i++)
        {
            _lab[i].text = _val[i].ToString();
            if(i == max - 1)
                _lab[i].text = _val[i] + "%";
        } 
    }

    void OnClickFriendShip(GameObject go)
    {
        attrPop.SetActive(true);
        SetText(curAttrLab, swornData.CurAttr);
        SetText(nextAttrLab, swornData.NextAttr); 
    }
}
