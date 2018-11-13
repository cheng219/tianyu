//======================================================
//作者:黄洪兴
//日期:2016/7/21
//用途:聊天主界面类
//======================================================



using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// 聊天主窗口
/// </summary>
public class ChatWnd : GUIBase
{

    #region 查看其他玩家
    
    public GameObject informationObj;
    public GameObject checkInformationBtn;
    public GameObject privateInformationBtn;
    public GameObject teamInformationBtn;
    public GameObject firendInformationBtn;
    public GameObject addGuildBtn;
    public GameObject blackInformationBtn;
    public GameObject mailInformationBtn;
    public GameObject informationBackBtn;

    ChatInfo informationChatInfo;
    Vector3 point;

    #endregion


    #region 选择频道
    public GameObject channelBackBtn;
    public GameObject hornChannel;
    public GameObject guildChannel;
    public GameObject teamChannel;
    public GameObject wordChannel;
    public GameObject channelObj;
    public GameObject channelBtn;
    #endregion

    public UILabel privateRedNum;
    public GameObject privateRedObj;


    /// <summary>
	/// 关闭按钮
	/// </summary>
	public UIButton closeBt;
    /// <summary>
    /// 聊天类型Toggle
    /// </summary>
    public UIToggle[] toggle;
    /// <summary>
    /// 普通聊天内容UI
    /// </summary>
	public ChatContentUI comChatContentUI;
	/// <summary>
	/// 传闻天内容UI
	/// </summary>
	public ChatContentUI storyChatContentUI;
	/// <summary>
	/// 私聊好友聊天内容UI
	/// </summary>
	public ChatContentUI friendChatContentUI;
	/// <summary>
	/// 私聊自己聊天天内容UI
	/// </summary>
	public ChatContentUI ownChatContentUI;


    /// <summary>
    /// 聊天内容父物体
    /// </summary>
	public GameObject chatContentUIParent;
    /// <summary>
    /// 聊天的scrollView
    /// </summary>
    public UIScrollView view;
    /// <summary>
    /// 聊天的panel
    /// </summary>
    public UIPanel viewPanel;
    /// <summary>
    /// 聊天玩家的Toggle
    /// </summary>
    public ChatPlayerOperateUI chatPlayerOperateUI;
    /// <summary>
    /// 聊天输入
    /// </summary>
    public ChatInput chatInput;
    /// <summary>
    /// 发送按钮
    /// </summary>
    public GameObject sendObj;

    /// <summary>
    /// 聊天内容UI列表
    /// </summary>
    protected List<ChatContentUI> chatUIList = new List<ChatContentUI>();
    #region 大喇叭
    public GameObject trumpetBtn;
    public GameObject trumpetObj;
    public UILabel trumpetNum;
    public UIInput trumpetContent;
    public GameObject sendTrumpetObj;
    public UILabel contentLabel;
    public GameObject closeTrumpetObj;
    public UILabel trumpetContentNum;


    EventDelegate edt;
    #endregion
    #region 好友界面
    public GameObject friendBtn;
    public GameObject friendWnd;
    public GameObject friendWndXBtn;
    public GameObject firendInstantiate;
    public GameObject grid;
    public UIScrollView scrollView;
    public UIToggle[] friendTypeTog;
    public GameObject[] noFriend;
    /// <summary>
    /// 0 好友 1 结义 2 仙侣 3 仇人 4 黑名单
    /// </summary>
    protected int friendType = 0;

    #endregion

    #region 私聊
    /// <summary>
    /// 聊天玩家的操作UI
    /// </summary>
    public ChatPlayerToggle[] ChatPlayerToggle;

    public GameObject privateChatObj;
    public UILabel privateChatName;
    public GameObject closePrivateBtn;

    public GameObject inVoiceBtn;
    //public GameObject outVoiceBtn;
    //public GameObject recordVoiceBtn;
    public GameObject inputObj;
    //public GameObject voiceObj;
    public GameObject voiceSprite;

    #endregion



    public GameObject PointBtn;
    public UITimer PointCDtimer;
    public UILabel PointCDlabel;
    public UISprite PointCDSprite;
    public GameObject PointCDObj;
    bool isfirstTimeCD = true;
    protected float timer = 0; 

    #region 数据定义
    //List<ChatInfo> privateChatList = new List<ChatInfo>();

    #endregion
    protected override void OnOpen()
    {
        base.OnOpen();
        GameCenter.chatMng.SetPauseFalse();
        GameCenter.chatMng.GetSavedContent();
        if (GameCenter.chatMng.savedChatType != ChatInfo.Type.All)
        {
            if (!string.IsNullOrEmpty(GameCenter.chatMng.SavedChat))
            {
                if (GameCenter.chatMng.savedChatType == ChatInfo.Type.Team)
                {
                    if (GameCenter.teamMng.TeamId != 0) GameCenter.chatMng.CurChatType = GameCenter.chatMng.savedChatType;
                }
                else
                    GameCenter.chatMng.CurChatType = GameCenter.chatMng.savedChatType;
            }
        }
        if (toggle.Length > (int)GameCenter.chatMng.CurChatType)
        toggle[(int)GameCenter.chatMng.CurChatType].value = true;
        if ((int)GameCenter.chatMng.CurChatType!=0&&toggle.Length>0)
        toggle[0].value = false;
       // Debug.Log(toggle[0].value + ":" + toggle[(int)GameCenter.chatMng.CurChatType].value);
        for (int i = 0; i < toggle.Length; i++)
        {
            EventDelegate.Add(toggle[i].onChange, SelectToggleIsTrue);
        }
        RefreshPointCD();
        RefreshPrivateRed();
        if (firendInstantiate != null)
            firendInstantiate.gameObject.SetActive(false);
    }

    protected override void OnClose()
    {
        base.OnClose();
        if (GameCenter.chatMng.isStopMusic)
        {
            GameCenter.chatMng.ResetBGM();
        }
       // Debug.Log("OnClose");
        chatUIList.Clear();
        GameCenter.chatMng.InitCloseWnd();
        GameCenter.chatMng.InitChatType();
        GameCenter.chatMng.SetCurAutoPlayTypeNoPlay();
        for (int i = 0; i < toggle.Length; i++)
        {
            EventDelegate.Remove(toggle[i].onChange, SelectToggleIsTrue);
        }

    }

    protected override void HandEvent(bool _bind)
    {
        base.HandEvent(_bind);
        if (_bind)
        {
            edt = new EventDelegate(this, "RefreshTrumpetContentNum");
            if (friendBtn != null) UIEventListener.Get(friendBtn).onClick += OpenFriendWnd;
            if (friendWndXBtn != null) UIEventListener.Get(friendWndXBtn).onClick += CloseFriendWnd;
            if (trumpetBtn != null) UIEventListener.Get(trumpetBtn).onClick += OpenTrumpetWnd;
            if (closeTrumpetObj != null) UIEventListener.Get(closeTrumpetObj).onClick += CloseTrumpetWnd;
            if (closePrivateBtn != null) UIEventListener.Get(closePrivateBtn).onClick += closePrivet;
            if (sendTrumpetObj != null) UIEventListener.Get(sendTrumpetObj).onClick += scendTrumpetMsg;
            if (trumpetContent != null) trumpetContent.onChange.Add(edt);
            if (PointBtn != null) UIEventListener.Get(PointBtn).onClick += sendPoint;
            if (channelBtn != null) UIEventListener.Get(channelBtn).onClick += OpenChannelObj;
            if (wordChannel != null) UIEventListener.Get(wordChannel).onClick += OpenWordChannel;
            if (teamChannel != null) UIEventListener.Get(teamChannel).onClick += OpenTeamChannel;
            if (guildChannel != null) UIEventListener.Get(guildChannel).onClick += OpenGuildChannel;
            if (hornChannel != null) UIEventListener.Get(hornChannel).onClick += OpenTrumpetWnd;
            if (channelBackBtn != null) UIEventListener.Get(channelBackBtn).onClick += CloseChannelObj;
            if (checkInformationBtn != null) UIEventListener.Get(checkInformationBtn).onClick += CheckInformation;
            if (privateInformationBtn != null) UIEventListener.Get(privateInformationBtn).onClick += PrivateInformation;
            if (teamInformationBtn != null) UIEventListener.Get(teamInformationBtn).onClick += TeamInformation;
            if (firendInformationBtn != null) UIEventListener.Get(firendInformationBtn).onClick += FirendInformation;
            if (addGuildBtn != null) UIEventListener.Get(addGuildBtn.gameObject).onClick = OnClickReqGuild;
            if (blackInformationBtn != null) UIEventListener.Get(blackInformationBtn).onClick += BlackInformation ;
            if (mailInformationBtn != null) UIEventListener.Get(mailInformationBtn).onClick += MailInformation;
            if (informationBackBtn != null) UIEventListener.Get(informationBackBtn).onClick += CloseInformationObj;
            if (closeBt != null) UIEventListener.Get(closeBt.gameObject).onClick += OnClickCloseBt;
            //if (inVoiceBtn != null) UIEventListener.Get(inVoiceBtn).onClick += OnClickInVoiceBt;
            if (inVoiceBtn != null) UIEventListener.Get(inVoiceBtn).onPress += RecordVoice;
            //if (outVoiceBtn != null) UIEventListener.Get(outVoiceBtn).onClick += OnClickOutVoiceBt;
            //if (recordVoiceBtn != null) UIEventListener.Get(recordVoiceBtn).onPress += RecordVoice;


            //GameCenter.chatMng.ChatInfoEvent += ShowChatContent;
            GameCenter.chatMng.SelectChatTypeEvent += SelectChatType;

            //GameCenter.chatMng.SelectPlayerEvent += OpenPlayerOperatUI;
            GameCenter.chatMng.OnShowInformation += OpenInformationObj;
            GameCenter.chatMng.OnUpdatePrivateRed += RefreshPrivateRed;
            GameCenter.chatMng.OnUpdatePrivateNewNum += RefreshPrivateWnd;
            GameCenter.inventoryMng.OnBackpackUpdate += RefreshTrumpetNum; 
           
        }
        else
        {
            if (friendBtn != null) UIEventListener.Get(friendBtn).onClick -= OpenFriendWnd;
            if (friendWndXBtn != null) UIEventListener.Get(friendWndXBtn).onClick -= CloseFriendWnd;
            if (trumpetBtn != null) UIEventListener.Get(trumpetBtn).onClick -= OpenTrumpetWnd;
            if (closeTrumpetObj != null) UIEventListener.Get(closeTrumpetObj).onClick -= CloseTrumpetWnd;
            if (closePrivateBtn != null) UIEventListener.Get(closePrivateBtn).onClick -= closePrivet;
            if (sendTrumpetObj != null) UIEventListener.Get(sendTrumpetObj).onClick -= scendTrumpetMsg;
            if (trumpetContent != null) trumpetContent.onChange.Remove(edt);
            if (PointBtn != null) UIEventListener.Get(PointBtn).onClick -= sendPoint;
            if (channelBtn != null) UIEventListener.Get(channelBtn).onClick -= OpenChannelObj;
            if (wordChannel != null) UIEventListener.Get(wordChannel).onClick -= OpenWordChannel;
            if (teamChannel != null) UIEventListener.Get(teamChannel).onClick -= OpenTeamChannel;
            if (guildChannel != null) UIEventListener.Get(guildChannel).onClick -= OpenGuildChannel;
            if (hornChannel != null) UIEventListener.Get(hornChannel).onClick -= OpenTrumpetWnd;
            if (channelBackBtn != null) UIEventListener.Get(channelBackBtn).onClick -= CloseChannelObj;
            if (checkInformationBtn != null) UIEventListener.Get(checkInformationBtn).onClick -= CheckInformation;
            if (privateInformationBtn != null) UIEventListener.Get(privateInformationBtn).onClick -= PrivateInformation;
            if (teamInformationBtn != null) UIEventListener.Get(teamInformationBtn).onClick -= TeamInformation;
            if (firendInformationBtn != null) UIEventListener.Get(firendInformationBtn).onClick -= FirendInformation;
            if (blackInformationBtn != null) UIEventListener.Get(blackInformationBtn).onClick -= BlackInformation;
            if (mailInformationBtn != null) UIEventListener.Get(mailInformationBtn).onClick -= MailInformation;
            if (informationBackBtn != null) UIEventListener.Get(informationBackBtn).onClick -= CloseInformationObj;
            if (closeBt != null) UIEventListener.Get(closeBt.gameObject).onClick -= OnClickCloseBt; 
            //if (inVoiceBtn != null) UIEventListener.Get(inVoiceBtn).onClick -= OnClickInVoiceBt;
            if (inVoiceBtn != null) UIEventListener.Get(inVoiceBtn).onPress -= RecordVoice;
            //if (outVoiceBtn != null) UIEventListener.Get(outVoiceBtn).onClick -= OnClickOutVoiceBt;
            //if (recordVoiceBtn != null) UIEventListener.Get(recordVoiceBtn).onPress -= RecordVoice;


            //GameCenter.chatMng.ChatInfoEvent -= ShowChatContent;
            GameCenter.chatMng.SelectChatTypeEvent -= SelectChatType;

           // GameCenter.chatMng.SelectPlayerEvent -= OpenPlayerOperatUI;
            GameCenter.chatMng.OnShowInformation -= OpenInformationObj;
            GameCenter.chatMng.OnUpdatePrivateRed -= RefreshPrivateRed;
            GameCenter.chatMng.OnUpdatePrivateNewNum -= RefreshPrivateWnd;
            GameCenter.inventoryMng.OnBackpackUpdate -= RefreshTrumpetNum; 
        }
    }

    /// <summary>
    /// 有新的聊天记录更新聊天内容
    /// </summary>
    void ShowChatContent(ChatInfo _info) 
    { 
        ChatInfo.Type curChatType = GameCenter.chatMng.CurChatType;
        List<ChatInfo> infoList = GameCenter.chatMng.GetInfoList((int)curChatType);
        int chatCount = infoList.Count;
        int type = 1;
        if (chatUIList.Count < chatCount)
        {            
            switch (_info.chatTypeID)
            {
                //case 5: type = 5; break;
                case 1:
                case 2:
                case 3:
                case 4:
                    if (_info.senderID != GameCenter.mainPlayerMng.MainPlayerInfo.ServerInstanceID)
                    {
                        type = 4;
                    }
                    else
                    {
                        type = 2; 
                    }
                    break;
                default:
                    break;
            }
            ChatContentUI chatUI = InstantiateChatContentUI(type);
            if (type == 2 || type == 3 || type == 4)
            {
                chatUI.ShowPrivateContent(_info); 
            }
            else
            {
                chatUI.ShowContent(_info);
            }
            chatUIList.Add(chatUI);
        }
        else 
        {
            if (type != 4 && type != 3 && type != 2 && type != 1)
            {
                chatUIList[chatCount - 1].ShowContent(_info);//
            }
            else
            {
                chatUIList[chatCount - 1].ShowPrivateContent(_info);
            }
            chatUIList[chatCount - 1].gameObject.SetActive(true);
        }
        if(chatUIList.Count >1) AddNewChatContentUI(chatUIList.Count - 2);
      //  chatContentUIParent.Reposition();
        view.ResetPosition();
        if (view.shouldMoveVertically) view.SetDragAmount( 0.01f,1.0f, false);

        //BoxCollider box = chatContentUIParent.GetComponent<BoxCollider>();
        //if (box != null)
        //{
        //    box.enabled = infoList.Count >= 5;
        //    box.size = new Vector3(box.size.x, (float)83 * infoList.Count, box.size.z);
        //    box.center = new Vector3(box.center.x, -41.5f * (infoList.Count - 1), box.center.z);
        //}
        
    }

    /// <summary>
    /// 设置每条聊天记录的位置
    /// </summary>
    void AddNewChatContentUI(int _index) 
    {

        Vector3 vector3 = chatUIList[_index].transform.localPosition;
        float sizeY = chatUIList[_index].chatContent.localSize.y;
        float sizeX = chatUIList[_index + 1].gameObject.transform.localPosition.x;
        chatUIList[_index + 1].gameObject.transform.localPosition = new Vector3(sizeX, vector3.y - sizeY - 60.0f, vector3.z);

    }

    void RefreshView(int _type)
    {
        if (view == null)
            return;
        if (_type == (int)ChatInfo.Type.Private)
        {
            if (viewPanel != null)
                viewPanel.SetRect(0, -22.5f, 863, 320);
             //view.transform.localPosition = new Vector3(view.transform.localPosition.x, -32f, view.transform.localPosition.z);
        }
        else
        {
            if (viewPanel != null)
                viewPanel.SetRect(0, 0, 863, 362);
           // view.transform.localPosition = new Vector3(view.transform.localPosition.x, -9.5f, view.transform.localPosition.z);
        }


    }


    /// <summary>
    /// 选择其他聊天类型更新聊天内容
    /// </summary>
    void SelectChatType(int _type)
    { 
        if(toggle.Length>(int)GameCenter.chatMng.CurChatType)
        toggle[(int)GameCenter.chatMng.CurChatType].value = true;
        //Debug.Log("选择的聊天类型为" + (int)GameCenter.chatMng.CurChatType);
        HideAllChatContent();
        RefreshView(_type);
        List<ChatInfo> infoList = GameCenter.chatMng.GetInfoList(_type);
        if (_type == (int)ChatInfo.Type.Private)
        {
            infoList = GameCenter.chatMng.GetPrivateInfoList(GameCenter.chatMng.CurTargetName);
            RefreshPrivateWnd();
        }
        int chatCount = infoList.Count;
        int type = 1;
        for (int i = 0; i < chatCount; i++)
        {
            type = 1;
            if (infoList[i] == null) break; 
             switch (infoList[i].chatTypeID)
             {
                 //case 5: type = 5; break; 
                 case 1: 
                 case 2:
                 case 3:
                     type = 5;
                     if (infoList[i].isSystemInfo)
                     {
                         type = 1;
                     } 
                     break;
                 case 4:
                     if (infoList[i].isSystemInfo)
                     {
                         type = 1;
                     }
                     if (infoList[i].senderID != GameCenter.mainPlayerMng.MainPlayerInfo.ServerInstanceID)
                     {
                         type = 4;
                     }
                     else
                     {
                         type = 2;
                     }
                     break;
                 default:
                     break;
             }
            if (i >= chatUIList.Count)
            {
                ChatContentUI chatUI = InstantiateChatContentUI(type);
                chatUIList.Add(chatUI);
            } 
            if (type == 1)
            {
                if (infoList[i] != null) chatUIList[i].ShowContent(infoList[i]);
            } 
            else
            { 
                chatUIList[i].ShowPrivateContent(infoList[i]);
            } 
            chatUIList[i].gameObject.SetActive(i < chatCount);
            if (i > 0) AddNewChatContentUI(i - 1);
        }
     //   chatContentUIParent.Reposition();
        view.ResetPosition();
        if (view.shouldMoveVertically)
        {
            view.SetDragAmount(0.01f, 1.0f, false);
        }


        //BoxCollider box = chatContentUIParent.GetComponent<BoxCollider>();
        //if (box != null)
        //{
        //    box.enabled = infoList.Count >= 5;
        //    box.size = new Vector3(box.size.x, (float)83 * infoList.Count, box.size.z);
        //    box.center = new Vector3(box.center.x,-41.5f * (infoList.Count - 1), box.center.z);
        //}
    }

    /// <summary>
    /// 选择私聊
    /// </summary>
    void SelectPrivateChat(string _name) 
    {
        SelectToggleByPlayer((int)GameCenter.chatMng.CurChatType - 1);
    }





    ///// <summary>
    ///// 打开玩家操作UI
    ///// </summary>
    //void OpenPlayerOperatUI(ChatInfo _info) 
    //{
        
    //    chatPlayerOperateUI.gameObject.SetActive(true);
    //    chatPlayerOperateUI.SetCurChatInfo(_info);
    //}

    ///// <summary>
    ///// 隐藏玩家操作UI
    ///// </summary>
    //void HidePlayerOperatUI(GameObject _obj)
    //{
        
    //    chatPlayerOperateUI.gameObject.SetActive(false);
    //    HideEmotionWnd();
    //}

    /// <summary>
    /// 隐藏表情窗口
    /// </summary>
    void HideEmotionWnd()
    {
        chatInput.emotionObj.SetActive(false);
        
    }
    #region 控件事件

    void OpenFriendWnd(GameObject obj)
    {
        if (friendWnd != null)
        {
            friendWnd.SetActive(true);
            GameCenter.swornMng.C2S_ReqGetSwornInfo();
            GameCenter.coupleMng.C2S_ReqMerrageInfo();
        }
        if (!GameCenter.friendsMng.allFriendDic.ContainsKey(3))
        {
            GameCenter.friendsMng.C2S_ReqFriendsList(3);//黑名单
        }
        if (!GameCenter.friendsMng.allFriendDic.ContainsKey(4))
        {
            GameCenter.friendsMng.C2S_ReqFriendsList(4);//仇人
        }
        RefreshFirendWnd();

        for (int i = 0, len = friendTypeTog.Length; i < len; i++)
        {
            if (friendTypeTog[i] != null)
            {
                UIEventListener.Get(friendTypeTog[i].gameObject).onClick = ClickToggleEvent; 
            }
        }
    }
    protected UIToggle lastChangeToggle = null;
    protected void ClickToggleEvent(GameObject go)
    {
        UIToggle toggle = go.GetComponent<UIToggle>();
        if (toggle != lastChangeToggle)
        {
            RefreshFirendWnd();
        }
        if (toggle != null && toggle.value) lastChangeToggle = toggle;
    }
    void CloseFriendWnd(GameObject obj)
    {
        if (friendWnd != null)
            friendWnd.SetActive(false);


    }

    void OpenTrumpetWnd(GameObject obj)
    {
        if (trumpetObj != null)
            trumpetObj.SetActive(true);
        if (trumpetNum != null)
            trumpetNum.text = GameCenter.inventoryMng.GetNumberByType(2600022).ToString();
        if(contentLabel!=null)
        contentLabel.text=ConfigMng.Instance.GetUItext(82);
        if (trumpetContent != null)
            trumpetContent.value = "";
        if (channelObj != null)
            channelObj.SetActive(false);

    }
    void CloseTrumpetWnd(GameObject obj)
    {
        if (trumpetObj != null)
            trumpetObj.SetActive(false);
    }


    void RefreshTrumpetNum()
    {
        if (trumpetNum != null)
            trumpetNum.text = GameCenter.inventoryMng.GetNumberByType(2600022).ToString();
    }

    #endregion



    #region unity3D

    void Awake()
    {
        mutualExclusion = true;
        layer = GUIZLayer.TOPWINDOW;
       // UIEventListener.Get(gameObject).onClick = HidePlayerOperatUI;
        if (!GameCenter.chatMng.IsSelectByAnotherFun) SelectToggleByPlayer((int)ChatInfo.Type.World - 1);
    }
    void Update()
    {
        if (GameCenter.chatMng.isRcordingVoice)
        {  
            if (Time.time-timer  > 25)
            { 
                if (voiceSprite != null)
                    voiceSprite.SetActive(false);
                SendRecordVoice(false);
            }  
        }  
    }
    /// <summary>
    /// 选择聊天类型
    /// </summary>
    void SelectToggleIsTrue() 
    {
        for (int i = 0; i < toggle.Length; i++)
        {
            if (toggle[i].value) { SelectToggle(i + 1); }
        }
    }

    /// <summary>
    /// 选择聊天类型
    /// </summary>
    void SelectToggleByPlayer(int _index) 
    {
        if (toggle[_index] != null) toggle[_index].value = true;
    }

    void SelectToggle(int _index) 
    { 
        //if (GameCenter.chatMng.CurAutoPlayType == CurAutoPlayVoiceType.CHATWND)//自动播放切换界面中断自动播放
        //{ 
        //    GameCenter.chatMng.SetCurAutoPlayTypeNoPlay();
        //}
        if (privateChatObj != null)
            privateChatObj.SetActive(false);
        if (chatInput!=null)
		chatInput.gameObject.SetActive (true);
        if (sendObj != null)
            sendObj.SetActive(true);
        if (trumpetBtn != null)
            trumpetBtn.SetActive(true);
        if (inVoiceBtn != null)
            inVoiceBtn.SetActive(false);
        if (inputObj != null) 
            inputObj.SetActive(true);
        //if (voiceObj != null)
        //    voiceObj.SetActive(false);
        switch (_index)
        {
            case 1: GameCenter.chatMng.SelectChatType((int)ChatInfo.Type.All); CouldPlayVoice(); break;
            case 2: GameCenter.chatMng.SelectChatType((int)ChatInfo.Type.World); CouldPlayVoice(); break;
            case 3: GameCenter.chatMng.SelectChatType((int)ChatInfo.Type.Team); CouldPlayVoice();
                if (GameCenter.teamMng.TeamId == 0) { sendObj.SetActive(false); GameCenter.messageMng.AddClientMsg(252);}break;
            case 4: GameCenter.chatMng.SelectChatType((int)ChatInfo.Type.Guild); CouldPlayVoice(); break;
		    case 6:GameCenter.chatMng.SelectChatType ((int)ChatInfo.Type.System);chatInput.gameObject.SetActive (false); break;
            case 5: GameCenter.chatMng.SelectChatType((int)ChatInfo.Type.Private); CouldPlayVoice(); break;
           // case 7: GameCenter.chatMng.SelectChatType((int)ChatInfo.Type.System); break;
            default: GameCenter.chatMng.SelectChatType((int)ChatInfo.Type.All); break;
        }


        if (chatInput != null) chatInput.ShowSavedChat();
    }

    void CouldPlayVoice()
    {
        //if (trumpetBtn != null)
        //    trumpetBtn.SetActive(false);
        if (inVoiceBtn != null)
            inVoiceBtn.SetActive(true);      
    }

    void OnEnable()
    {

    }

    void OnDisable()
    {

    }

    #endregion


    #region 辅助逻辑
    /// <summary>
    /// 实例化聊天内容UI
    /// </summary>
	protected ChatContentUI InstantiateChatContentUI(int _type)
    { 
		if (_type == 1) {
			if (comChatContentUI != null) {
				ChatContentUI obj = Instantiate (comChatContentUI) as ChatContentUI;
				Transform parentTransf = chatContentUIParent.transform;
				obj.transform.parent = parentTransf;
				obj.transform.localScale = Vector3.one;
				obj.transform.localPosition = Vector3.zero;
				obj.gameObject.SetActive (true);
				return obj;
			}
		}
		if (_type == 5) {
			if (storyChatContentUI != null) {
				ChatContentUI obj = Instantiate (storyChatContentUI) as ChatContentUI;
				Transform parentTransf = chatContentUIParent.transform;
				obj.transform.parent = parentTransf;
				obj.transform.localScale = Vector3.one;
                obj.transform.localPosition = Vector3.zero;

				obj.gameObject.SetActive (true);
				return obj;
			}
		}
		if (_type == 4) {
			if (friendChatContentUI != null) {
				ChatContentUI obj = Instantiate (friendChatContentUI) as ChatContentUI;
				Transform parentTransf = chatContentUIParent.transform;
				obj.transform.parent = parentTransf;
				obj.transform.localScale = Vector3.one;
                obj.transform.localPosition = Vector3.zero;

				obj.gameObject.SetActive (true);
				return obj;
			}
		}
		if (_type == 2) {
			if (ownChatContentUI != null) {
				ChatContentUI obj = Instantiate (ownChatContentUI) as ChatContentUI;
				Transform parentTransf = chatContentUIParent.transform;
				obj.transform.parent = parentTransf;
				obj.transform.localScale = Vector3.one;
                obj.transform.localPosition = new Vector3(670,0,0);

				obj.gameObject.SetActive (true);
				return obj;
			}
		}
        return null;
    }
    /// <summary>
    /// 删除所有的聊天内容UI
    /// </summary>
    void HideAllChatContent() 
    {
        chatContentUIParent.transform.DestroyChildren();
        //for (int i = 0; i < chatUIList.Count; i++) 
        //{
        //    Destroy(chatUIList[i].gameObject);
        //}
        chatUIList.Clear();
    }
    /// <summary>
    /// 隐藏所有的玩家Toggle
    /// </summary>
    void HideAllPlayerToggle()
    {
        for (int i = 0; i < ChatPlayerToggle.Length; i++)
        {
			if(ChatPlayerToggle[i]!=null)
            ChatPlayerToggle[i].gameObject.SetActive(false);
        }
    }




    void RecordVoice(GameObject _go,bool _b)
    {
        if (_b)
        {
            if (GameCenter.chatMng.CurChatType == ChatInfo.Type.World || GameCenter.chatMng.CurChatType == ChatInfo.Type.All)
            {
                if (GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel < 30)
                {
                    GameCenter.messageMng.AddClientMsg(379);
                    return;
                }
            }
            SendRecordVoice(true);
        }
        else
        {
            if (GameCenter.chatMng.isRcordingVoice)
            {
                SendRecordVoice(false);
            } 
        } 
    } 

    void SendRecordVoice(bool _b)
    { 
        if (_b)
        {
            timer = Time.time;
            if (GameCenter.chatMng.CurChatType == ChatInfo.Type.Private)
            {
                if (string.IsNullOrEmpty(GameCenter.chatMng.CurTargetName))
                {
                    GameCenter.messageMng.AddClientMsg(365);
                    return;
                }
            } 
            if (voiceSprite != null)
                voiceSprite.SetActive(true); 

        }
        else
        {
            timer = 0;
            if (voiceSprite != null)
                voiceSprite.SetActive(false);
        }
        if (GameCenter.chatMng.CurChatType == ChatInfo.Type.Private)
        {
            GameCenter.chatMng.VoiceRecordMainWnd(_b, GameCenter.chatMng.CurChatType, GameCenter.chatMng.CurTargetName);
        } 
        else
        {
            GameCenter.chatMng.VoiceRecordMainWnd(_b, GameCenter.chatMng.CurChatType);
        }
    }
    //void OnClickOutVoiceBt(GameObject _go)
    //{
    //    if (inputObj != null)
    //        inputObj.SetActive(true);
    //    if (voiceObj != null)
    //        voiceObj.SetActive(false);

    //}

     
    //void OnClickInVoiceBt(GameObject _go)
    //{
    //    if (inputObj != null)
    //        inputObj.SetActive(false);
    //    if (voiceObj != null)
    //        voiceObj.SetActive(true);
    //}




	void OnClickCloseBt(GameObject go)
	{ 
		GameCenter.uIMng.SwitchToUI (GUIType.NONE);
	}



    void RefreshPrivateRed()
    {
        if (privateRedObj != null)
            privateRedObj.SetActive(GameCenter.chatMng.AllPrivateChatNewNum > 0);
        if (privateRedNum != null)
        {
            privateRedNum.text = GameCenter.chatMng.AllPrivateChatNewNum.ToString();
            privateRedNum.gameObject.SetActive(GameCenter.chatMng.AllPrivateChatNewNum > 0);
        }

    }



    void RefreshPrivateWnd()
    {
        RefreshPrivateRed();
        if (GameCenter.chatMng.CurChatType != ChatInfo.Type.Private)
            return;
        if (privateChatObj != null)
            privateChatObj.SetActive(false);
        string curTargetName;
        if (GameCenter.chatMng.CurTargetName != null && GameCenter.chatMng.CurTargetName!=string.Empty)
        {
            curTargetName = GameCenter.chatMng.CurTargetName;
            if (privateChatName != null)
            {
                privateChatName.text = curTargetName;
            }
            if (privateChatObj != null)
                privateChatObj.SetActive(true);
               
        }
        for (int i = 0; i < ChatPlayerToggle.Length; i++)
        {
            if (ChatPlayerToggle[i] != null)
                ChatPlayerToggle[i].gameObject.SetActive(false);
        }
        int j = 0;
       // Debug.Log("刷新私聊界面私聊对象数量" + GameCenter.chatMng.PrivateChatDic.Count);
        using (var e = GameCenter.chatMng.PrivateChatDic.GetEnumerator())
        {
            while(e.MoveNext())
            {
                if (j < ChatPlayerToggle.Length)
                {
                    if (ChatPlayerToggle[j] != null)
                        ChatPlayerToggle[j].FillInfo(e.Current.Key);
                    j++;
                }

            }
        }



    }


    void scendTrumpetMsg(GameObject _go)
    {
        if(trumpetContent!=null)
        {
            if (trumpetContent.value != null && trumpetContent.value != string.Empty)
            {
                if (GameCenter.inventoryMng.GetNumberByType(2600022) > 0)
                {
                    GameCenter.chatMng.SendChatContent(trumpetContent.value, 5, string.Empty);
                    if (trumpetObj != null)
                        trumpetObj.SetActive(false);
                }
                else
                {
                    GameCenter.buyMng.id = 1062;
                    GameCenter.buyMng.CurPrice = 10;
                    GameCenter.buyMng.priceType = 1;
                    GameCenter.buyMng.OpenBuyWnd(new EquipmentInfo(2600022, EquipmentBelongTo.PREVIEW), BuyType.MALL);
                }
            }

        }
    }


    void closePrivet(GameObject _go)
    {
        GameCenter.chatMng.ClosePrivateChat();
    }





    void sendPoint(GameObject _go)
    {
        if (GameCenter.chatMng.PointCD != 0)
        {
            GameCenter.messageMng.AddClientMsg(370);
        }
        else
        {
            if (GameCenter.chatMng.CurChatType == ChatInfo.Type.Private && GameCenter.chatMng.CurTargetName == string.Empty)
            {
                GameCenter.messageMng.AddClientMsg(365); return;
            }
            ChatInfo Info = new ChatInfo(GameCenter.curMainPlayer.transform.localPosition, GameCenter.mainPlayerMng.MainPlayerInfo.SceneID);
            GameCenter.chatMng.C2S_SendContent(Info);
            GameCenter.chatMng.PointCD = Time.time;
            RefreshPointCD();
        }
    }











    void RefreshTrumpetContentNum()
    {
        if (trumpetContentNum != null && trumpetContent!=null)
            trumpetContentNum.text =(30- trumpetContent.value.ToCharArray().Length).ToString();
    }


    protected FDictionary GetFriendTypeDic()
    {
        FDictionary dic = new FDictionary();
        switch (friendType)
        {
            case 0:
                if (GameCenter.friendsMng.allFriendDic.ContainsKey(1))
                {
                    dic = GameCenter.friendsMng.allFriendDic[1];
                }
                break;
            case 1:
                if (GameCenter.swornMng.data != null)
                {
                    List<st.net.NetBase.brothers_list> list = GameCenter.swornMng.data.brothers;
                    for(int i = 0,max = list.Count;i <max;i++)
                    {
                        FriendsInfo info = new FriendsInfo(list[i]);
                        if (!dic.ContainsKey(info.configId) && info.configId != GameCenter.mainPlayerMng.MainPlayerInfo.ServerInstanceID)
                        {
                            dic[info.configId] = info;
                        }
                    }
                }
                break;
            case 2:
                if (GameCenter.coupleMng.coupleData != null)
                {
                    FriendsInfo info = new FriendsInfo(GameCenter.coupleMng.coupleData);
                    dic[info.configId] = info;
                }
                break;
            case 3:
                if (GameCenter.friendsMng.allFriendDic.ContainsKey(4))
                {
                    dic = GameCenter.friendsMng.allFriendDic[4];
                }
                break;
            case 4:
                if (GameCenter.friendsMng.allFriendDic.ContainsKey(3))
                {
                    dic = GameCenter.friendsMng.allFriendDic[3];
                }
                break;
            default:
                if (GameCenter.friendsMng.allFriendDic.ContainsKey(1))
                {
                    dic = GameCenter.friendsMng.allFriendDic[1];
                }
                break;
        }
        return dic;
    }

    void SetNullFriend(bool _isReset)
    {
        if (_isReset)
        {
            for (int i = 0, max = noFriend.Length; i < max; i++)
            { 
                noFriend[i].SetActive(false); 
            }
        }
        else
        {
            for (int i = 0, max = noFriend.Length; i < max; i++)
            {
                if (i == friendType)
                {
                    noFriend[i].SetActive(true);
                }
                else
                {
                    noFriend[i].SetActive(false);
                }
            }
        }
    }


    void RefreshFirendWnd()
    {
        for (int i = 0, max = friendTypeTog.Length; i < max; i++)
        {
            if (friendTypeTog[i].value)
            {
                friendType = i;
            }
        }
        if (grid != null)
        {
            grid.transform.DestroyChildren();
        }
        FDictionary dic = GetFriendTypeDic();
        if (dic.Count < 1)
        {
            SetNullFriend(false);
            return;
        }
        if (firendInstantiate == null)
            return;
        Vector3 V3 = Vector3.zero;
        SetNullFriend(true);
        foreach (FriendsInfo item in dic.Values)
        {
            if (firendInstantiate != null)
            {
                GameObject obj = Instantiate(firendInstantiate) as GameObject;
                if (obj != null)
                {
                    obj.SetActive(true);
                    obj.transform.parent = grid.transform;
                    obj.transform.localPosition = V3;
                    obj.transform.localScale = Vector3.one;
                    obj.GetComponent<ChatFirendUI>().FillInfo(item);
                    V3 = new Vector3(V3.x, V3.y - 105, V3.z);

                }

            }

        }

        if (scrollView != null)
            scrollView.ResetPosition();

    }


    void CloseInformationObj(GameObject _go)
    {
        if (informationObj != null)
            informationObj.SetActive(false);

    }
    void BlackInformation(GameObject _go)
    {
        if (informationObj != null)
            informationObj.SetActive(false);
        if (informationChatInfo != null)
        {
            GameCenter.friendsMng.C2S_AddFriendToBlack(informationChatInfo.senderID);
        }
    }
    void MailInformation(GameObject _go)
    {
        if (informationObj != null)
            informationObj.SetActive(false);
        GameCenter.mailBoxMng.mailWriteData = new MailWriteData(informationChatInfo.senderName);
        GameCenter.uIMng.SwitchToSubUI(SubGUIType.BMail);

    }

    void FirendInformation(GameObject _go)
    {
        if (informationObj != null)
            informationObj.SetActive(false);
        if (informationChatInfo != null)
        {
            GameCenter.friendsMng.C2S_AddFriend(informationChatInfo.senderID);
        }

    }

    protected void OnClickReqGuild(GameObject go)
    {
        if (informationChatInfo != null)
            GameCenter.guildMng.C2S_ReqJoinGuild(informationChatInfo.senderID);
    }

    void TeamInformation(GameObject _go)
    {
        if (informationObj != null)
            informationObj.SetActive(false);
        if (informationChatInfo != null)
        {
            GameCenter.teamMng.C2S_TeamInvite(informationChatInfo.senderID);
        }

    }

    void CheckInformation(GameObject _go)
    {
        if (informationObj != null)
            informationObj.SetActive(false);
        if (informationChatInfo != null)
        {
            GameCenter.previewManager.C2S_AskOPCPreview(informationChatInfo.senderID);
        }


    }

    void PrivateInformation(GameObject _go)
    {
        if (GameCenter.chatMng.CurChatType == ChatInfo.Type.Private)
        {
            GameCenter.messageMng.AddClientMsg(468);
            return;
        }
        if (informationChatInfo != null)
            GameCenter.chatMng.PrivateChatTo(informationChatInfo.senderName);
        if (informationObj != null)
            informationObj.SetActive(false);

    }
    void OpenInformationObj(ChatInfo _info,Vector3 _point)
    {
        if (_info == null)
            return;
        if (_info.senderID != GameCenter.mainPlayerMng.MainPlayerInfo.ServerInstanceID)
        {
            informationChatInfo = _info;
            point = _point;
            if (informationObj != null)
                informationObj.SetActive(true);
            informationObj.transform.position = new Vector3(point.x, point.y, 0);
        }
        else
        {
            GameCenter.previewManager.C2S_AskOPCPreview(_info.senderID);
        }

    }



    void CloseChannelObj(GameObject _go)
    {
        if (channelObj != null)
            channelObj.SetActive(false);

    }


    void OpenGuildChannel(GameObject _go)
    {
        GameCenter.chatMng.SelectChatType(3);
        if (channelObj != null)
            channelObj.SetActive(false);
    }


    void OpenTeamChannel(GameObject _go)
    {
        GameCenter.chatMng.SelectChatType(2);
        if (channelObj != null)
            channelObj.SetActive(false);
    }

    void OpenWordChannel(GameObject _go)
    {
        GameCenter.chatMng.SelectChatType(1);
        if (channelObj != null)
            channelObj.SetActive(false);
    }

    void OpenChannelObj(GameObject _go)
    {
        if (channelObj != null)
            channelObj.SetActive(true);
    }



    void RefreshPointCDSprite()
    {
        if (PointCDSprite != null && PointCDlabel != null)
        {
            PointCDSprite.fillAmount = (float)Convert.ToInt32(PointCDlabel.text) / 20;
            if (PointCDSprite.fillAmount <= 0)
            {
                GameCenter.chatMng.PointCD = 0;
                if(PointCDObj!=null)
                PointCDObj.SetActive(false);
            }
        }
    }

    
    void RefreshPointCD()
    {
        if (GameCenter.chatMng.PointCD != 0)
        {
            int remainTime = (int)(Time.time - GameCenter.chatMng.PointCD);
            if (remainTime >= 20)
            {
                GameCenter.chatMng.PointCD = 0f;
                if (PointCDObj != null)
                    PointCDObj.SetActive(false);
                return;
            }
            if (PointCDObj != null)
                PointCDObj.SetActive(true);
            if (PointCDtimer != null)
            {
                PointCDtimer.StartIntervalTimer(20-remainTime);
                if (isfirstTimeCD)
                {
                    InvokeRepeating("RefreshPointCDSprite", 0.1f, 0.5f);
                    isfirstTimeCD = false;
                }
            }


        }



    }













    #endregion

}
