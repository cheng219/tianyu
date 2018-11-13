//======================================================
//作者:黄洪兴
//日期:2016/7/18
//用途:聊天信息UI组件
//======================================================

using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// 聊天内容UI
/// </summary>
public class ChatContentUI : MonoBehaviour {

    /// <summary>
    /// 头像图片
    /// </summary>
    public UISprite headSprite;
    public GameObject headObj;

    /// <summary>
    /// VIPObj
    /// </summary>
    public GameObject vipObj;
    /// <summary>
    /// 发送者的VIP等级
    /// </summary>
    public UILabel vipLv;

    /// <summary>
    /// 聊天内容
    /// </summary>
    public UILabel chatContent;

    public UILabel sendName;
    public UILabel sendTime;


    //public GameObject wordSprit;
    //public GameObject teamSprite;
    public GameObject systemSprite;
    public GameObject otherWords;//别人发送信息背景
    public GameObject selfWords;//自己发送信息背景

    public  GameObject chatContentObj;

    public ChatFunction chatfuncshion;
    public GameObject voiceSprite;
    public UILabel voiceCount;
    public UISprite voiceContentSprite;
    public UISprite selfVoiceContentSprite;
    public GameObject voiceRedObj;
    public UISpriteAnimation spAnimation;
    public UISprite stopPlay;

    /// <summary>
    /// 当前的聊天数据
    /// </summary>
    protected ChatInfo curInfo = null;


    #region unity3D

    void Awake()
    {

    }

    void OnEnable()
    {
        GameCenter.chatMng.OnAutoPlayWndVoiceUpdate += SetVoiceRedFalse;
        GameCenter.chatMng.OnAutoPlayVoiceUpdate += ShowPlayAni;
    }

    void OnDisable()
    {
        GameCenter.chatMng.OnAutoPlayWndVoiceUpdate -= SetVoiceRedFalse;
        GameCenter.chatMng.OnAutoPlayVoiceUpdate -= ShowPlayAni;
        
    }

    void OnDestroy() 
    {
        GameCenter.chatMng.OnAutoPlayWndVoiceUpdate -= SetVoiceRedFalse;
        GameCenter.chatMng.OnAutoPlayVoiceUpdate -= ShowPlayAni;
    }

   

    /// <summary>
    /// 刷新聊天内容数据
    /// </summary>
   public  void ShowContent(ChatInfo _info) 
    {
        if (_info == null) return;
        curInfo = _info; 
        if (chatfuncshion != null)
        { 
            chatfuncshion.init(_info);
        }
        if (headSprite != null )
        {

            if (_info.sendProf>0)
            {
                PlayerConfig Ref = ConfigMng.Instance.GetPlayerConfig(_info.sendProf);
                if (Ref != null)
                {
                    headSprite.spriteName = Ref.res_head_Icon;
                    headSprite.MakePixelPerfect();
                }
            }
            else
            {
                headSprite.gameObject.SetActive(false);
                if (headObj != null)
                    headObj.SetActive(false);
            }
        }
        if (vipLv != null)
        {
            vipLv.text = _info.sendVipLv.ToString();
            if (vipObj!=null)
                vipObj.gameObject.SetActive(_info.sendVipLv > 0);
        }
        if (chatContentObj == null)
        {
            if (chatContent == null)
                return;
            if (_info.chatTypeID == 5)
            {
                chatContent.transform.localPosition = new Vector3(chatContent.transform.localPosition.x - 30, chatContent.transform.localPosition.y, chatContent.transform.localPosition.z);
            }
            chatContent.text = _info.ChatContent;
            if (NGUIMath.CalculateRelativeWidgetBounds(chatContent.transform).size.x >= 650)
            {
                chatContent.overflowMethod = UILabel.Overflow.ResizeHeight;
                chatContent.width = 650;
            }
            ShowSprite(_info.isSystemInfo);
            //switch (_info.chatTypeID)
            //{
            //    case 5: ShowSprite(3); break;
            //    case 2: ShowSprite(2); break;
            //    default: ShowSprite(1);
            //        break;
            //}
        }
        else
        {
            if (chatContent!=null)
            chatContent.text = _info.ChatContent;
            if (NGUIMath.CalculateRelativeWidgetBounds(chatContent.transform).size.x >= 422)
            {
                chatContent.overflowMethod = UILabel.Overflow.ResizeHeight;
                chatContent.width = 422;
            }
            chatContentObj.SetActive(true);
        } 
        if (otherWords != null)
        {
            otherWords.SetActive(_info.senderID != GameCenter.mainPlayerMng.MainPlayerInfo.ServerInstanceID && !_info.isSystemInfo);
        }
        if (selfWords != null)
        {
            selfWords.SetActive(_info.senderID == GameCenter.mainPlayerMng.MainPlayerInfo.ServerInstanceID && !_info.isSystemInfo);
        } 
    }


    /// <summary>
    /// 语音聊天刷新
    /// </summary>
    /// <param name="_info"></param>
   public void ShowPrivateContent(ChatInfo _info)
   {
       if (_info == null) return;
       if (chatfuncshion != null)
       { 
           chatfuncshion.init(_info);
       }
       curInfo = _info;
       if (voiceRedObj != null)
       {
           voiceRedObj.SetActive(false);
       }
       StopAni();
       if (headSprite != null && _info.sendProf != 0)
       {
           PlayerConfig Ref = ConfigMng.Instance.GetPlayerConfig(_info.sendProf);
           if (Ref != null)
           {
               headSprite.spriteName = Ref.res_head_Icon;
               headSprite.MakePixelPerfect();
           }
           else
           {
               headSprite.gameObject.SetActive(false);
               if (headObj != null)
                   headObj.SetActive(false);
           }
       }

           if (chatContent == null)
               return;
           //聊天类型
           string[] str = _info.ChatContent.Split(':');
           if (str.Length >= 2)
           {
               chatContent.text = str[1];
           }
           else
           {
               Debug.LogError("聊天信息数据错误  by黄洪兴");
           }
           if (NGUIMath.CalculateRelativeWidgetBounds(chatContent.transform).size.x >= 650)
           {
               chatContent.overflowMethod = UILabel.Overflow.ResizeHeight;
               chatContent.width = 650;
           }
           if (sendName != null)
           {
               if (curInfo.senderID != GameCenter.mainPlayerMng.MainPlayerInfo.ServerInstanceID)
               {
                   sendName.text = "[u]" + curInfo.senderName + curInfo.sendTime;
               }
               else
               {
                  UIwenbenRef Ref = ConfigMng.Instance.GetUITextRef(86);
                  if (Ref != null)
                  {
                      sendName.text = curInfo.sendTime + Ref.text;
                  }
                  else
                  {
                      sendName.text = curInfo.sendTime;
                  }
               }
           }
           if (curInfo.isVoice)
           {
               if (chatContent != null)
               {
                   chatContent.text = string.Empty;
                   UIEventListener.Get(chatContent.gameObject).onClick = PlayVoice;
               }
               if (voiceCount != null)
                   voiceCount.text = curInfo.voiceCont.ToString()+"\"";
               if (voiceContentSprite != null)
               {
                   voiceContentSprite.ResetAnchors();
                   voiceContentSprite.width = 120;
               }
               if (selfVoiceContentSprite != null)
               {
                   selfVoiceContentSprite.ResetAnchors();
                   selfVoiceContentSprite.width = 120;
               }
               if (voiceRedObj != null)
                   voiceRedObj.SetActive(curInfo.voiceRed);

           }
           if (voiceSprite != null)
               voiceSprite.SetActive(curInfo.isVoice); 
        if (otherWords != null)
        {
            otherWords.SetActive(_info.senderID != GameCenter.mainPlayerMng.MainPlayerInfo.ServerInstanceID && !_info.isSystemInfo);
        }
        if (selfWords != null)
        {
            selfWords.SetActive(_info.senderID == GameCenter.mainPlayerMng.MainPlayerInfo.ServerInstanceID && !_info.isSystemInfo);
        } 
   }

   void PlayAni()
   {
       if (spAnimation != null) spAnimation.gameObject.SetActive(true);
       if (stopPlay != null) stopPlay.gameObject.SetActive(false);
   }
   void StopAni()
   {
       if (spAnimation != null) spAnimation.gameObject.SetActive(false);
       if (stopPlay != null) stopPlay.gameObject.SetActive(true);
   }
    /// <summary>
    /// 播放语音
    /// </summary>
    /// <param name="_go"></param>
   void PlayVoice(GameObject _go)
   { 
       //重新播放已经播放了的或点击自己的声音
       if (!curInfo.voiceRed || curInfo.senderID == GameCenter.mainPlayerMng.MainPlayerInfo.ServerInstanceID)
       {
           PlayAni();
           curInfo.voiceRed = false;
           if (voiceRedObj != null)
               voiceRedObj.SetActive(curInfo.voiceRed);
           GameCenter.chatMng.InterruptVoiceSelfPlaying = true;
           //Debug.Log("播放语音成功"); 
           if (curInfo.voicePath != string.Empty)
               YvVoiceSdk.YvVoicePlayVoiceCallBack(curInfo.voicePath, curInfo.accurateTime,() =>
               {
                   StopAni(); 
               });
       }
       else//点击没有播放的开启自动播放
       {
           if (GameCenter.chatMng.InterruptVoiceSelfPlaying)
           {
               PlayAni();
               curInfo.voiceRed = false;
               if (voiceRedObj != null)
                   voiceRedObj.SetActive(curInfo.voiceRed);
               if (curInfo.voicePath != string.Empty)
                   YvVoiceSdk.YvVoicePlayVoiceCallBack(curInfo.voicePath, curInfo.accurateTime, () =>
                   {
                       StopAni(); 
                   });
           }
           else
           {
               if (GameCenter.chatMng.CurAutoPlayType == CurAutoPlayVoiceType.MAINCHAT)
               {
                   GameCenter.chatMng.SetCurAutoPlayType(curInfo);//主界面自动播放切换为当前聊天界面自动播放
               }
               else
               {
                   PlayAni();
                   curInfo.voiceRed = false;
                   if (voiceRedObj != null)
                       voiceRedObj.SetActive(curInfo.voiceRed);
                   GameCenter.chatMng.InterruptVoiceSelfPlaying = true;
                   if (curInfo.voicePath != string.Empty)
                       YvVoiceSdk.YvVoicePlayVoiceCallBack(curInfo.voicePath, curInfo.accurateTime, () =>
                       {
                           StopAni(); 
                       });
               }
           }
       }
   }

   /// <summary>
   /// 语音播完后停止语音动画
   /// </summary>
   /// <param name="_info"></param>
   void SetVoiceRedFalse(ChatInfo _info)
   {
       if (_info != null && _info.accurateTime == curInfo.accurateTime)
       {
           StopAni(); 
       }
   }
    /// <summary>
    /// 播放语音显示播放动画去掉红点
    /// </summary>
   void ShowPlayAni(ChatInfo _info)
   {
       if (_info != null && _info.accurateTime == curInfo.accurateTime)
       {
           PlayAni(); 
           curInfo.voiceRed = false;
           if (voiceRedObj != null)
               voiceRedObj.SetActive(curInfo.voiceRed);
       }
   }

   void ShowSprite(bool  _type)
   {
       //if (wordSprit != null)
       //    wordSprit.SetActive(_type == 1);
       //if (teamSprite != null)
       //    teamSprite.SetActive(_type == 2);
       if (systemSprite != null)
           systemSprite.SetActive(_type);

   }
    #endregion
}
