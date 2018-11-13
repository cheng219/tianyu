//======================================================
//作者:黄洪兴
//日期:2016/7/21
//用途:主界面聊天窗口类
//======================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 主界面聊天窗口
/// </summary>
public class ChatMainUI : MonoBehaviour
{

    #region zsy

    public List<VoiceObjUi> voiceTypeList = new List<VoiceObjUi>();
    public UILabel curVoiceType; 
    public UIButton sendVoiceBtn;
    public UILabel unreadCount;
    public UISprite backSp;
    public GameObject voiceSprite;
    public GameObject playVoiceObj;
    public UILabel channal;
    public UILabel voiceName;
    public UILabel voiceLen;
    public UIButton playX;
    public UIButton pauseBtn;
    public UISpriteAnimation anim;
    public UISprite voiceContentSprite;
    protected ChatInfo curInfo = null;
    public UISprite mark;//箭头标志
    public GameObject menuObj;
    
   
    //public UIButton play;
    //protected ChatInfo curVoice = null;
    //public UIButton closePlay; 

    #endregion

    public UIButton chatBtn;
    public UIScrollView view;
    public UIGrid gride;
    public ChatContentUI chatContentUI;
    #region unity3D

    protected ChatInfo chatinfo = null;
     
    protected bool isClick = false;
    protected float recordStar = 0;

    protected int voiceCount = 0;

    void Awake()
    {
        HideAllUI();
        if (chatBtn != null) UIEventListener.Get(chatBtn.gameObject).onClick = OpenChatWnd;
        if (playX != null) UIEventListener.Get(playX.gameObject).onClick = ClosePlayMind;
        if (pauseBtn != null) UIEventListener.Get(pauseBtn.gameObject).onClick = OnPause;
        voiceCount = GameCenter.chatMng.GetVoiceTypeList().Count;
    }

    void Update()
    {
        if (!menuObj.activeSelf)
        {
            if (mark.flip != UIBasicSprite.Flip.Nothing) mark.flip = UIBasicSprite.Flip.Nothing;
        }
        else
        {
            if (voiceCount > 0) if (mark.flip != UIBasicSprite.Flip.Vertically) mark.flip = UIBasicSprite.Flip.Vertically;
        }
        ChatMng chatMng = GameCenter.chatMng;
        if (isClick)
        {
            if (chatMng.isRcordingVoice)
            { 
                if (Time.time - recordStar > 26)
                { 
                    isClick = false;
                    SendRecordVoice(false);
                }
            }
            else
            { 
                ShowVoiceMenu(); 
            }
        }
        if (chatMng.IsAutoPlayingVoice)
        {
            if (chatMng.curVoiceInfo != null && chatMng.curVoiceInfo.IsVoiceTimeOut)
            {
                chatMng.AutoPlayNextVoice();
            }
        }
    }

    void OnPause(GameObject go)
    {
        GameCenter.chatMng.IsPausePlayVoice = !GameCenter.chatMng.IsPausePlayVoice;
        playX.gameObject.SetActive(GameCenter.chatMng.IsPausePlayVoice);
        if (GameCenter.chatMng.IsPausePlayVoice)
        {
            YvVoiceSdk.YvVoiceStopPlayVoice();
            anim.Pause();
        }
        else
        {
            anim.Play();
        }
    }

    void OpenChatWnd(GameObject _obj) 
    {
        if (isClick) isClick = false;
        //GameCenter.chatMng.InterruptVoiceSelfPlaying = true;//中断语音播放
        GameCenter.uIMng.SwitchToUI(GUIType.CHAT);
        FillInfo(chatinfo);
    }

    void OnClickShowVoiceMenu(GameObject go)
    {  
        isClick = true;
    }
    //显示语音聊天
    void OnAutoPlayVoice(ChatInfo _info)
    {
        if (_info != null)
        {
            if (GameCenter.chatMng.CurAutoPlayType == CurAutoPlayVoiceType.MAINCHAT)
            {
                //if (_info.chatTypeID != (int)GameCenter.chatMng.CurVoiceType) return;
                //if (_info.chatTypeID == (int)ChatInfo.Type.Private)
                //{
                //    if (_info.senderName != GameCenter.chatMng.CurVoicePrivateNmae)
                //    {
                //        return;
                //    }
                //}
                curInfo = _info;
                curInfo.voiceRed = false;
                playVoiceObj.SetActive(GameCenter.chatMng.IsAutoPlayingVoice);
                playX.gameObject.SetActive(!GameCenter.chatMng.IsAutoPlayingVoice);
                if (!GameCenter.chatMng.IsPausePlayVoice) anim.Play();
                channal.text = "[" + GameCenter.chatMng.ChatType(curInfo.chatTypeID) + "] ";
                if (voiceName != null)
                {
                    if (curInfo.senderID != GameCenter.mainPlayerMng.MainPlayerInfo.ServerInstanceID)
                    {
                        voiceName.text = "[u]" + curInfo.senderName;
                    }
                }
                if (curInfo.isVoice)
                {
                    if (voiceLen != null)
                        voiceLen.text = curInfo.voiceCont.ToString() + "\"";
                    if (voiceContentSprite != null)
                    {
                        voiceContentSprite.ResetAnchors();
                        voiceContentSprite.width = 120;
                    }

                }
            }
        }
        else
        {
            if (!GameCenter.chatMng.IsPausePlayVoice)
            {
                playVoiceObj.SetActive(false);
            } 
        }
    }
    /// <summary>
    /// 点击暂停
    /// </summary> 
    //void OnClickPlay(GameObject go) 
    //{
    //    GameCenter.chatMng.InterruptVoiceSelfPlaying = !GameCenter.chatMng.InterruptVoiceSelfPlaying; 
    //    closePlay.gameObject.SetActive(GameCenter.chatMng.InterruptVoiceSelfPlaying);
    //}
    /// <summary>
    /// 清除所有语音缓存
    /// </summary>
    void ClosePlayMind()
    { 
        GameCenter.chatMng.SetCurAutoPlayTypeNoPlay(); 
        GameCenter.chatMng.SetPauseFalse();
        playVoiceObj.SetActive(false);
    }
    /// <summary>
    /// 清除所有语音并恢复播放
    /// </summary> 
    void ClosePlayMind(GameObject go)
    {
        ClosePlayMind();
    }
    void RecordVoice(GameObject _go, bool _b)
    { 
        CancelInvoke("ShowVoiceRecord");
        if (_b)
        { 
            Invoke("ShowVoiceRecord", 0.5f);
        }
        else
        {
            if(GameCenter.chatMng.isRcordingVoice)SendRecordVoice(false);
        }
    }

    void ShowVoiceRecord()
    {
        if (Time.time - recordStar >= 0.5)
        {
            if (GameCenter.chatMng.CurVoiceType == ChatInfo.Type.World || GameCenter.chatMng.CurVoiceType == ChatInfo.Type.All)
            {
                if (GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel < 30)
                {
                    recordStar = 0;
                    isClick = false;
                    GameCenter.messageMng.AddClientMsg(379);
                    return;
                }
            }
            SendRecordVoice(true);
            if (voiceSprite != null)
                voiceSprite.SetActive(true);
        }
        else
        {
            recordStar = 0;
            isClick = false;
        }
    }

    void SendRecordVoice(bool _b)
    {
        if (!_b)
        {
            recordStar = 0;
            isClick = false;
            if (GameCenter.chatMng.isStopMusic)
            {
                GameCenter.chatMng.ResetBGM();
            }
            if (voiceSprite != null)
                voiceSprite.SetActive(false);
        }
        else
        {
            isClick = true;
            recordStar = Time.time;
        }
        if (GameCenter.chatMng.CurVoiceType != ChatInfo.Type.Private)
        {  
            GameCenter.chatMng.VoiceRecordMainWnd(_b, GameCenter.chatMng.CurVoiceType);
        }
        else
        {
            GameCenter.chatMng.VoiceRecordMainWnd(_b, ChatInfo.Type.Private, GameCenter.chatMng.CurVoicePrivateNmae); 
        }
    }

    void OnEnable()
    {
        if (sendVoiceBtn != null) UIEventListener.Get(sendVoiceBtn.gameObject).onClick += OnClickShowVoiceMenu;
        if (sendVoiceBtn != null) UIEventListener.Get(sendVoiceBtn.gameObject).onPress += RecordVoice;
        GameCenter.chatMng.OnAutoPlayVoiceUpdate += OnAutoPlayVoice;
        GameCenter.chatMng.ChatInfoOutEvent += FillInfo;
        GameCenter.chatMng.OnVoiceUpdate += ShowCurVoice;
        GameCenter.teamMng.onTeammateUpdateEvent += RefreshCurVoiceType;
        GameCenter.mainPlayerMng.MainPlayerInfo.onGuildNameUpdate += RefreshCurVoiceType;
        GameCenter.chatMng.ChatNumChangeEvent += RefreshUnRead;
        ShowCurVoice();
        FillInfo(chatinfo);
    }

    void OnDisable()
    {
        if (sendVoiceBtn != null) UIEventListener.Get(sendVoiceBtn.gameObject).onClick -= OnClickShowVoiceMenu;
        if (sendVoiceBtn != null) UIEventListener.Get(sendVoiceBtn.gameObject).onPress -= RecordVoice;
        GameCenter.chatMng.OnAutoPlayVoiceUpdate -= OnAutoPlayVoice;
        GameCenter.chatMng.ChatInfoOutEvent -= FillInfo;
        GameCenter.chatMng.OnVoiceUpdate -= ShowCurVoice; 
        GameCenter.chatMng.ChatNumChangeEvent -= RefreshUnRead;
        GameCenter.teamMng.onTeammateUpdateEvent -= RefreshCurVoiceType;
        GameCenter.mainPlayerMng.MainPlayerInfo.onGuildNameUpdate -= RefreshCurVoiceType;
    }
    /// <summary>
    /// 填充聊天记录
    /// </summary>
    void FillInfo(ChatInfo _info)
    {
        //List<ChatInfo> infoList = GameCenter.chatMng.GetInfoList((int)ChatInfo.Type.All); 
        //int chatCount = infoList.Count;
        //HideAllUI();
        //if (chatCount < 1)
        //    return;
        //if (!infoList[chatCount - 1].isVoice)
        //{
        //    chatContentUI.gameObject.SetActive(chatCount > 0);
        //    chatContentUI.ShowContent(infoList[chatCount - 1]);
        //}

        //List<ChatInfo> infoList = GameCenter.chatMng.GetInfoList((int)ChatInfo.Type.All);
        //int chatCount = infoList.Count;
        //HideAllUI();//点击打开界面的时候,不隐藏此条信息 
        if (_info == null)
            return;
        if (!_info.isVoice && _info.chatTypeID != (int)ChatInfo.Type.Private)
        {
            chatContentUI.gameObject.SetActive(true);
            chatContentUI.ShowContent(_info);
        }
    }

    void RefreshUnRead()
    {
        int count = 0;  
        count = GameCenter.chatMng.GetChatNumIsNotRead(ChatInfo.Type.Private) + GameCenter.chatMng.GetChatNumIsNotRead(ChatInfo.Type.Team); 

        if (count > 0)
        {
            unreadCount.transform.parent.gameObject.SetActive(true);
            unreadCount.text = count.ToString();
        }
        else
        {
            unreadCount.transform.parent.gameObject.SetActive(false);
        }
    }

    void ShowCurVoice()
    {
        voiceCount = GameCenter.chatMng.GetVoiceTypeList().Count;
        if (GameCenter.chatMng.CurVoiceType == ChatInfo.Type.Private)
        {
            curVoiceType.text = GameCenter.chatMng.CurVoicePrivateNmae;
        }
        else
        {
            curVoiceType.text = GameCenter.chatMng.ChatType((int)GameCenter.chatMng.CurVoiceType);
        }
        RefreshUnRead(); 
    }


    void ShowVoiceMenu()
    {
        isClick = false;
        List<VoiceData> typeList = GameCenter.chatMng.GetVoiceTypeList();
        voiceCount = typeList.Count;
        if (voiceCount < 1)
        {
            backSp.gameObject.SetActive(false);
        }
        else
        { 
            backSp.gameObject.SetActive(true);
            backSp.height = 58 + (typeList.Count - 1) * 50;
        }
        for (int i = 0, max = voiceTypeList.Count; i < max; i++)
        {
            if (i < typeList.Count)
            { 
                voiceTypeList[i].gameObject.SetActive(true);
                voiceTypeList[i].Show(typeList[i]); 
            }
            else
            {
                voiceTypeList[i].gameObject.SetActive(false);
            }
        }
    }
    /// <summary>
    /// 当队伍或仙盟解散的时候更新当前自动播放
    /// </summary>
    void RefreshCurVoiceType()
    { 
        if (GameCenter.chatMng.CurVoiceType == ChatInfo.Type.Team)
        {
            if (!GameCenter.teamMng.isInTeam)
            {
                GameCenter.chatMng.CurVoiceType = ChatInfo.Type.World;
            }
        } 
    } 
    void RefreshCurVoiceType(string _str)
    {
        if (GameCenter.chatMng.CurVoiceType == ChatInfo.Type.Guild)
        {
            if (string.IsNullOrEmpty(GameCenter.mainPlayerMng.MainPlayerInfo.GuildName))
            {
                GameCenter.chatMng.CurVoiceType = ChatInfo.Type.World;
            }
        } 
    }

    /// <summary>
    /// 设置每条聊天记录的位置
    /// </summary>
    void AddNewChatContentUI(int _index)
    {
     //   Vector3 vector3 = chatContentUI[_index].transform.localPosition;
     //   float sizeY = chatContentUI[_index].chatContent.localSize.y;
     ////   Debug.Log(sizeY);
     //   chatContentUI[_index + 1].gameObject.transform.localPosition = new Vector3(vector3.x, vector3.y - sizeY - 5.0f, vector3.z);

    }

    /// <summary>
    /// 隐藏预制（不仅是隐藏的作用，主要还是解决更新内容时，莫名的位置错乱问题）
    /// </summary>
    void HideAllUI() 
    {
        chatContentUI.gameObject.SetActive(false);
    } 
    #endregion
}
