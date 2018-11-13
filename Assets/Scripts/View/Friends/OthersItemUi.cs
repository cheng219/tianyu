//==================================
//作者：朱素云
//日期：2016/4/11
//用途：仙友UI
//=================================
using UnityEngine;
using System.Collections;

public class OthersItemUi : MonoBehaviour
{
    #region 数据  
    /// <summary>
    /// 与你同屏，附近的人
    /// </summary>
    public GameObject sameScrDes;
    /// <summary>
    /// 送花，好友
    /// </summary>
    public GameObject sendFlowerBtn;
    /// <summary>
    /// 所在场景，仇人 
    /// </summary>
    public UILabel placeLab;
    /// <summary>
    /// 亲密度
    /// </summary>
    public GameObject intimacyLab;
    /// <summary>
    /// 勾选
    /// </summary>
    public UIToggle chooseTol;
    public UISprite sp;
     
    public UISpriteEx iconSp;

    public UILabel nameLab;
    public UILabel levLab; 

    protected FriendsInfo data = null;
    public FriendsInfo FriendsData
    {
        get
        {
            return data;
        }
        set
        {
            if (value != null) data = value;
            data.OnFriendsListUpdate -= Show;
            data.OnFriendsListUpdate += Show;
            Show();
        }
    } 
    #endregion

    void Show()
    {
        if (sendFlowerBtn != null)
        {
            if (sendFlowerBtn.activeSelf)
            {
                UIEventListener.Get(sendFlowerBtn.GetComponent<UIButton>().gameObject).onClick = delegate
                {
                    GameCenter.friendsMng.SendFlowerType = 0;
                    GameCenter.friendsMng.sendFlowerToOne = data.configId;
                    //跳转至送花界面
                    GameCenter.uIMng.GenGUI(GUIType.SENDFLOWER,true);
                }; 
            }
        }
        if (placeLab != null && placeLab.gameObject.activeSelf) 
        {
            if (placeLab.gameObject.activeSelf) 
                placeLab.text = ConfigMng.Instance.GetUItext(85, new string[1] { data.PlaceName });
        }
        if (intimacyLab != null && intimacyLab.activeSelf)
        { 
            UILabel lab = intimacyLab.GetComponentInChildren<UILabel>();
            if(lab != null)lab.text = data.intimacy.ToString(); 
        }
        if (chooseTol != null && chooseTol.gameObject.activeSelf)
        {
            EventDelegate.Remove(chooseTol.onChange, UiOnChange);
            EventDelegate.Add(chooseTol.onChange, UiOnChange); 
        } 
        if (iconSp != null)
        {
            iconSp.spriteName = data.Icon;
            if (data.IsOnline)
            { 
                iconSp.IsGray = UISpriteEx.ColorGray.normal;
            }
            else
            { 
                iconSp.IsGray = UISpriteEx.ColorGray.Gray;
            }
        } 
        if (nameLab != null) nameLab.text = data.name;
        if (levLab != null) levLab.text = data.Lev; 
    }

    void UiOnChange()
    {
        if (chooseTol.value)
        { 
            GameCenter.friendsMng.chooseList.Add(data.configId); 
            //sp.gameObject.SetActive(true); 
        }
        if (!chooseTol.value)
        { 
            GameCenter.friendsMng.chooseList.Remove(data.configId); 
            //sp.gameObject.SetActive(false); 
        }
    } 
	void OnDestroy()
    {
        if (chooseTol != null) EventDelegate.Remove(chooseTol.onChange, UiOnChange); 
    }
    void OnDisable()
    {
        if (data != null) data.OnFriendsListUpdate -= Show;
    }
}
