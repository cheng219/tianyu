//======================================================
//作者:朱素云
//日期:2016/11/1
//用途:语音设置界面
//======================================================
using UnityEngine;
using System.Collections;

public class VoiceSetSubWnd : SubWnd {

    /// <summary>
    /// 世界频道自动播放语音
    /// </summary>
    public UIToggle worldVoiceTog;
    /// <summary>
    /// 仙盟频道自动播放语音
    /// </summary>
    public UIToggle allyVoiceTog;
    /// <summary>
    /// 队伍频道自动播放语音
    /// </summary>
    public UIToggle teamVoiceTog;
    /// <summary>
    /// 私聊频道自动播放语音
    /// </summary>
    public UIToggle chatVoiceTog;

    #region UNITY
    void Awake()
    {
        type = SubGUIType.VOICEPLAY;
    }
    protected override void HandEvent(bool _bind)
    {
        base.HandEvent(_bind);
        if (_bind)
        {
            EventDelegate.Add(worldVoiceTog.onChange, TogOnChange);
            EventDelegate.Add(allyVoiceTog.onChange, TogOnChange);
            EventDelegate.Add(teamVoiceTog.onChange, TogOnChange);
            EventDelegate.Add(chatVoiceTog.onChange, TogOnChange);
        }
        else
        {
            EventDelegate.Remove(worldVoiceTog.onChange, TogOnChange);
            EventDelegate.Remove(allyVoiceTog.onChange, TogOnChange);
            EventDelegate.Remove(teamVoiceTog.onChange, TogOnChange);
            EventDelegate.Remove(chatVoiceTog.onChange, TogOnChange);
        }
    }
    protected override void OnOpen()
    {
        base.OnOpen();
        refresh();
    }

    protected override void OnClose()
    {
        base.OnClose();
    }
    #endregion

    void refresh()
    {
        worldVoiceTog.value = GameCenter.systemSettingMng.AutoPlayWorldVoice;
        allyVoiceTog.value = GameCenter.systemSettingMng.AutoPlayAllyVoice;
        teamVoiceTog.value = GameCenter.systemSettingMng.AutoPlayTeamVoice;
        chatVoiceTog.value = GameCenter.systemSettingMng.AutoPlayChatVoice;
    }

    void TogOnChange()
    {
        GameCenter.systemSettingMng.AutoPlayWorldVoice = worldVoiceTog.value;
        GameCenter.systemSettingMng.AutoPlayAllyVoice = allyVoiceTog.value;
        GameCenter.systemSettingMng.AutoPlayTeamVoice = teamVoiceTog.value;
        GameCenter.systemSettingMng.AutoPlayChatVoice = chatVoiceTog.value;
    }
}
