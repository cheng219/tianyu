//=====================================
//作者:黄洪兴
//日期:2016/6/11
//用途:聊天界面好友UI
//========================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
/// <summary>
/// 聊天界面好友UI
/// </summary>
public class ChatFirendUI : MonoBehaviour
{
    #region 控件数据
    public UILabel playerName;
    public UILabel lev;
    public UISpriteEx headSprite;
    public GameObject goChatBtn;
    public GameObject FriendWndObj;

    #endregion
    #region 数据


    ChatInfo chatInfo;
    /// <summary>
    /// 当前填充的数据
    /// </summary>
    FriendsInfo friendsInfo;
    //	protected SkillInfo oldSkillinfo;  //for upgrade effect -by ms
    #endregion
    // Use this for initialization
    void Start()
    {


    }

    void SetBtn(GameObject go)
    {
        if (FriendWndObj != null)
            FriendWndObj.SetActive(false);
        if (chatInfo != null)
        {
            GameCenter.chatMng.PrivateChatTo(friendsInfo.name);
        }
    }







    /// <summary>
    /// 填充数据
    /// </summary>
    /// <param name="_info"></param>
    public void FillInfo(FriendsInfo _info)
    {
        if (_info == null)
        {
            friendsInfo = null;
            return;
        }
        else
        {
            friendsInfo = _info;
            chatInfo = new ChatInfo(4, "", friendsInfo.name);
        }
        Refresh();
    }



    /// <summary>
    /// 刷新表现
    /// </summary>
    public void Refresh()
    {
        if (playerName != null)
            playerName.text = friendsInfo.name;
        if (lev != null)
            lev.text = ConfigMng.Instance.GetLevelDes(friendsInfo.lev);
        if (headSprite != null)
        {
            headSprite.spriteName = friendsInfo.Icon;
            headSprite.MakePixelPerfect();
            if (friendsInfo.IsOnline)
            {
                headSprite.IsGray = UISpriteEx.ColorGray.normal;
            }
            else
            {
                headSprite.IsGray = UISpriteEx.ColorGray.Gray;
            }
        }

        if (goChatBtn != null)
            UIEventListener.Get(goChatBtn).onClick = SetBtn;

    }





}
