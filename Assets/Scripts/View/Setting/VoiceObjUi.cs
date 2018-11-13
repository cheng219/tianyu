//======================================================
//作者:朱素云
//日期:2016/11/2
//用途:语音
//======================================================
using UnityEngine;
using System.Collections;

public class VoiceObjUi : MonoBehaviour 
{
    public UILabel voiceType;
    public UILabel privateName;
    protected VoiceData voiceData = null;

    void Awake()
    {
        UIEventListener.Get(this.gameObject).onClick = SetCurVoice;
    }

    void SetCurVoice(GameObject go)
    { 
        if (voiceData != null)
        { 
            if (voiceData.type == ChatInfo.Type.Private)
            {
                GameCenter.chatMng.CurVoicePrivateNmae = voiceData.privateName;
            }
            else
            {
                GameCenter.chatMng.CurVoicePrivateNmae = null;
                GameCenter.chatMng.CurVoiceType = voiceData.type;
            } 
        }
    }

    public void Show(VoiceData _voiceData)
    { 
        voiceData = _voiceData; 
        voiceType.text = GameCenter.chatMng.ChatType((int)voiceData.type);
        if (string.IsNullOrEmpty(voiceData.privateName) || voiceData.type != ChatInfo.Type.Private)
        {
            privateName.gameObject.SetActive(false);
        }
        else
        {
            privateName.gameObject.SetActive(true);
            privateName.text = voiceData.privateName;
        }
    }
}


public class VoiceData
{
    public ChatInfo.Type type = ChatInfo.Type.World;
    public string privateName = string.Empty;

    public VoiceData(ChatInfo.Type _type)
    {
        type = _type; 
    }
    public VoiceData(string _privateName)
    {
        type = ChatInfo.Type.Private;
        if (_privateName != null)
        {
            privateName = _privateName;
        }
    }
}