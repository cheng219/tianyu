//======================================================
//作者:黄洪兴
//日期:2016/7/21
//用途:聊天玩家操作组件
//======================================================



using UnityEngine;
using System.Collections;

/// <summary>
/// 聊天玩家操作UI
/// </summary>
public class ChatPlayerOperateUI : MonoBehaviour {

    protected ChatInfo curChatInfo = null;

    #region unity3D

    void Awake()
    {       

    }


    protected void SelectToggle()
    {

    }

    void OnEnable()
    {

    }



    void SetVisiual(bool _visiual) 
    {
        gameObject.SetActive(_visiual);
    }

    void OnDisable()
    {
        SetVisiual(false);

    }

    #endregion

    public void SetCurChatInfo(ChatInfo _info) 
    {
        if(_info ==null) return;
        curChatInfo = _info;
    }


    /// <summary>
    /// 私聊
    /// </summary>
    public void Chat() 
    {
        Debug.Log("Chat");
        SetVisiual(false);
    }

    /// <summary>
    /// 申请组队
    /// </summary>
    public void InviteTeam()
    {
        Debug.Log("InviteTeam");
        SetVisiual(false);
        GameCenter.teamMng.C2S_TeamInvite(curChatInfo.senderID);
    }

    /// <summary>
    /// 申请公会
    /// </summary>
    public void ApplicationGuild()
    {
        SetVisiual(false);
        GameCenter.messageMng.AddClientMsg(ConfigMng.Instance.GetUItext(273));
    }

    /// <summary>
    /// 查看其它玩家
    /// </summary>
    public void LookPlayerInformation()
    {
        SetVisiual(false);
        GameCenter.messageMng.AddClientMsg(ConfigMng.Instance.GetUItext(273));
    } 

}
