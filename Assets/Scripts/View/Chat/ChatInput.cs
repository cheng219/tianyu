//======================================================
//作者:黄洪兴
//日期:2016/7/21
//用途:聊天输入UI
//======================================================



using UnityEngine;
using System.Collections;

/// <summary>
/// 聊天输入UI
/// </summary>
public class ChatInput : MonoBehaviour {

    /// <summary>
    /// 喇叭
    /// </summary>
    public UIButton horn;

    /// <summary>
    /// 聊天类型
    /// </summary>
    public UILabel chatTypeLabel;
    /// <summary>
    /// 除了私聊的输入
    /// </summary>
    public UIInput conmentInput;
    /// <summary>
    /// 发送
    /// </summary>
    public UIButton send;
    /// <summary>
    /// 除了私聊的聊天输入OBJ
    /// </summary>
    public GameObject conmentTalk;
    /// <summary>
    /// 打开表情输入窗口
    /// </summary>
    public UIButton emotionBtn;
    /// <summary>
    /// 表情输入窗口
    /// </summary>
    public GameObject emotionObj;

    public GameObject closeEmotionBtn;


    /// <summary>
    /// 当前的聊天类型
    /// </summary>
    protected int curType = 0;
    /// <summary>
    /// 私聊对象的名字
    /// </summary>
 //   int receiveID;



    #region unity3D

    void Awake()
    {

    }

    public void ShowSavedChat()
    {
        GameCenter.chatMng.GetSavedContent();

        if (!string.IsNullOrEmpty(GameCenter.chatMng.SavedChat))
        {
            if (GameCenter.chatMng.CurChatType == GameCenter.chatMng.savedChatType)
            {
                conmentInput.value = GameCenter.chatMng.SavedChat;//显示上次保存的文字 
            }
        }
    }

    void OnEnable()
    {
        SelectChatType((int)GameCenter.chatMng.CurChatType);
        ShowSavedChat();
        EventDelegate.Add(conmentInput.onChange, updateInPuts);
        GameCenter.chatMng.OnChangeCurChatType += SelectChatType; 
       if(send.gameObject!=null) UIEventListener.Get(send.gameObject).onClick += SendChatContent;
        if(emotionBtn.gameObject!=null)UIEventListener.Get(emotionBtn.gameObject).onClick += OpenEmotionUI;
        if (closeEmotionBtn != null) UIEventListener.Get(closeEmotionBtn).onClick += CloseEmotionUI;
        GameCenter.chatMng.OnSendMsgSuccess += ResetInput;
    }

    void SelectChatType(int _type)
    { 
        curType = _type;
        chatTypeLabel.text = GameCenter.chatMng.ChatType(_type); 
        ResetInput();
    }

    void SetPrivateID(int _id) 
    {
     //   Debug.Log(_name);
        //receiveID= _id;
    }

    void SendChatContent(GameObject _obj) 
    {
        string chatContent = string.Empty;
        char[] chatConten = conmentInput.value.ToCharArray();
        if (chatConten.Length >= 30)
        {
            chatContent = conmentInput.value.Remove(30, chatConten.Length - 30);
        }
        else
        {
            chatContent = conmentInput.value;
        }
     
        if (curType == (int)ChatInfo.Type.Private)
        {
            //string mainPlayerName = GameCenter.mainPlayerMng.MainPlayerInfo.Name;
            if (string.IsNullOrEmpty(conmentInput.value)) { return; }
            if (GameCenter.chatMng.CurTargetName == string.Empty || GameCenter.chatMng.CurTargetName==null) 
            { GameCenter.messageMng.AddClientMsg(365); return; }
        }
        else 
        {
            if (string.IsNullOrEmpty(conmentInput.value)) { return; }
        }
        if (curType == (int)ChatInfo.Type.Private)
        {
            if(GameCenter.chatMng.SendChatContent(chatContent, curType, GameCenter.chatMng.CurTargetName)) 
                GameCenter.chatMng.RecoverSavedContent(); 
        }
        else
        {
            //if ((ChatInfo.Type)curType == ChatInfo.Type.All || (ChatInfo.Type)curType == ChatInfo.Type.World)
            //{
            //    if (!GameCenter.instance.IsSetWorldChatTime)
            //    {
            //        GameCenter.instance.IsSetWorldChatTime = true;
            //        if(GameCenter.chatMng.SendChatContent(chatContent, curType, string.Empty))
            //            GameCenter.chatMng.RecoverSavedContent(); 
            //        return;
            //    }
            //    else
            //    {
            //        GameCenter.messageMng.AddClientMsg(360);
            //        return;
            //    }
            //}
            if(GameCenter.chatMng.SendChatContent(chatContent, curType, string.Empty))
                GameCenter.chatMng.RecoverSavedContent(); 
        }
    }

    /// <summary>
    /// 打开表情输入窗口
    /// </summary>
    void OpenEmotionUI(GameObject _obj) 
    {
        if (emotionObj!=null)
        emotionObj.SetActive(true);
    }

    void CloseEmotionUI(GameObject _obj)
    {
        if (emotionObj != null)
            emotionObj.SetActive(false);
    }
    void OnDisable()
    {
        EventDelegate.Remove(conmentInput.onChange, updateInPuts);
        GameCenter.chatMng.OnChangeCurChatType -= SelectChatType;
        if (send.gameObject != null) UIEventListener.Get(send.gameObject).onClick -= SendChatContent;
        if (emotionBtn.gameObject != null) UIEventListener.Get(emotionBtn.gameObject).onClick -= OpenEmotionUI;
        if (closeEmotionBtn != null) UIEventListener.Get(closeEmotionBtn).onClick -= CloseEmotionUI;
        GameCenter.chatMng.OnSendMsgSuccess -= ResetInput;
    }



    /// <summary>
    /// 发送提交后重置输入
    /// </summary>
    public void ResetInput() 
    {
        if (conmentInput != null)
        {
            conmentInput.value = string.Empty;
            //置为空时，supportEncoding值为false
            conmentInput.label.supportEncoding = true; 
        }
    }

    public void AddEmotion(UISprite _sprite) 
    {
        if (conmentInput!=null)
        conmentInput.value += "/"+_sprite.spriteName+" ";
        if (emotionObj != null)
            emotionObj.SetActive(false);
    }


    void updateInPuts()
    {
        if (!string.IsNullOrEmpty(conmentInput.value))
        {
            GameCenter.chatMng.savedChatType = GameCenter.chatMng.CurChatType;
            GameCenter.chatMng.SavedChat = conmentInput.value;
        }
        else
        {
            if (GameCenter.chatMng.savedChatType == GameCenter.chatMng.CurChatType)
            {
                GameCenter.chatMng.RecoverSavedContent();
            }
        }
    }

    #endregion
}
