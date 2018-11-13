//==================================
//作者：朱素云
//日期：2016/4/10
//用途：好友UI
//=================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FindFriendUi : MonoBehaviour 
{ 
    public UIButton addFriendBtn;
    public UIButton findBtn; 
    public UIInput findInput; 
    public UISprite icon; 
    public UILabel nameLab;
    public UILabel levLab;  
    public UIButton addBtn;
    public UIButton changeBtn; 
    public OthersItemUi items; 
    protected FriendsInfo info;

    public FriendsInfo FriendsData
    {
        get
        {
            return info;
        }
        set
        {
            if (value != null) info = value;
            info.OnFindFirendUpdate -= ShowFindInfo; 
            info.OnFindFirendUpdate += ShowFindInfo; 
            ShowFindInfo(); 
        }
    } 
    /// <summary>
    /// 显示推送列表
    /// </summary>
    public void ShowTuiSong()
    { 
        List<FriendsInfo> list = GameCenter.friendsMng.friendList;  
        if (changeBtn != null) UIEventListener.Get(changeBtn.gameObject).onClick = delegate
            {
                GameCenter.friendsMng.C2S_ReqDoSomething(FriendOperation.ADDBALCKLIST, 0);//换一批
            };
        if (list.Count <= 0)
        { 
            if (addBtn != null)
            {
                UISpriteEx addEx = addBtn.GetComponentInChildren<UISpriteEx>();
                if (addEx != null) addEx.IsGray = UISpriteEx.ColorGray.Gray;
            }
        }
        else
        { 
            if (addBtn != null)
            {
                UISpriteEx addEx = addBtn.GetComponentInChildren<UISpriteEx>();
                if (addEx != null) addEx.IsGray = UISpriteEx.ColorGray.normal;
                if (addEx.IsGray == UISpriteEx.ColorGray.normal) UIEventListener.Get(addBtn.gameObject).onClick = delegate
                {
                    int id = 0;
                    if (GameCenter.friendsMng.curFriend != null)
                    {
                        id = GameCenter.friendsMng.curFriend.configId;
                    }
                    else if (list.Count > 0)
                    {
                        id = list[0].configId;
                    }
                    if(id != 0)GameCenter.friendsMng.C2S_AddFriend(id); 
                };
            } 
        } 
    }
    /// <summary>
    /// 查找好友显示
    /// </summary>
    public void ShowFindInfo()
    { 
        if (info == null)
        {
            if (findInput != null) findInput.value = null;
            if (icon != null) icon.gameObject.SetActive(false);
            if (nameLab != null) nameLab.text = string.Empty;
            if (levLab != null) levLab.gameObject.SetActive(false);
        }
        else
        {
            Refresh(info);
            items.FriendsData = info;
        }
        if (addFriendBtn != null) UIEventListener.Get(addFriendBtn.gameObject).onClick = delegate
        {
            if (info != null && findInput.value != string.Empty)
            { 
                GameCenter.friendsMng.C2S_AddFriend(info.configId);
            }
            else
            { 
                GameCenter.messageMng.AddClientMsg(377);
            }
        };
        if (findBtn != null) UIEventListener.Get(findBtn.gameObject).onClick = delegate
        {
            if (findInput.value != string.Empty)
            {
                GameCenter.friendsMng.C2S_ReqFindFriend(findInput.value);
            }
            else
            {
                GameCenter.messageMng.AddClientMsg(377);
            }
        }; 
    }
    void Refresh(FriendsInfo _data)
    { 
        if (icon != null) { icon.gameObject.SetActive(true); icon.spriteName = _data.Icon; }
        if (levLab != null) { levLab.gameObject.SetActive(true); levLab.text = _data.Lev; }
        if (nameLab != null) { levLab.gameObject.SetActive(true); nameLab.text = _data.name; }
        if (findInput != null) { findInput.value = _data.name; }
    }
    void OnDisable()
    {
        if (info != null) info.OnFindFirendUpdate -= ShowFindInfo;
    }
}
