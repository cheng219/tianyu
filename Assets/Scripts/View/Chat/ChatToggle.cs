using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 聊天uitoggle
/// </summary>
public class ChatToggle : MonoBehaviour {

    public UISprite tip;

    public ChatInfo.Type curType;

    void Awake()
    {

    }



    void OnEnable()
    {
        Refresh();
        GameCenter.chatMng.ChatNumChangeEvent += Refresh;
    }

    void OnDisable()
    {
        GameCenter.chatMng.ChatNumChangeEvent -= Refresh;

    }

    void Refresh()
    {
        int num = GameCenter.chatMng.GetChatNumIsNotRead(curType);
        tip.gameObject.SetActive(num >0);
    }
}
