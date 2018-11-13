//===============================
//作者：邓成
//日期：2017/1/22
//用途：活跃度奖励Item界面类
//===============================

using UnityEngine;
using System.Collections;

public class LivelyRewardItemUI : MonoBehaviour {
    public UILabel livelyAmount;
    public UILabel doubleRewardDes;
    public UISpriteEx getPic;
    public UISprite cantGetPic;
    public UISprite redTipIcon;

    string format = ConfigMng.Instance.GetUItext(274);
    protected System.Action<LivelyRewardRef> OnClickRewardEvent;
    protected LivelyRewardRef curLively = null;

    protected System.Action<GuildLivelyRewardRef> OnClickGuildRewardEvent;
    protected GuildLivelyRewardRef curGuildLively = null;
    void Awake()
    { 
    
    }

    public void SetData(LivelyRewardRef _livelyRewardRef, System.Action<LivelyRewardRef> _OnClick)
    {
        if (_livelyRewardRef == null) return;
        OnClickRewardEvent = _OnClick;
        curLively = _livelyRewardRef;
        if (livelyAmount != null) livelyAmount.text = ConfigMng.Instance.GetUItext(138, new string[1] { _livelyRewardRef.need.ToString() }); ;
        if (doubleRewardDes != null) doubleRewardDes.text = string.Format(format, _livelyRewardRef.vip);
        bool haveGot = GameCenter.dailyMustDoMng.MustDoStateDic.ContainsKey(_livelyRewardRef.id)?GameCenter.dailyMustDoMng.MustDoStateDic[_livelyRewardRef.id]:false;
        bool canGet = GameCenter.dailyMustDoMng.CurLivelyCount >= _livelyRewardRef.need;
        int curLivelyCount = GameCenter.dailyMustDoMng.CurLivelyCount;
        if (haveGot)
        {
            UIEventListener.Get(gameObject).onClick = null;
        }
        else
        { 
            UIEventListener.Get(gameObject).onClick = OnClickReward;
        }
        if (getPic != null)
        {
            getPic.enabled = canGet;
            getPic.IsGray = haveGot ? UISpriteEx.ColorGray.Gray : UISpriteEx.ColorGray.normal;
        }
        if (cantGetPic != null) cantGetPic.enabled = !canGet;//不可领显示
        if (redTipIcon != null) redTipIcon.enabled = canGet && !haveGot;
    }

    public void SetData(GuildLivelyRewardRef _livelyRewardRef, System.Action<GuildLivelyRewardRef> _OnClick)
    {
        if (_livelyRewardRef == null) return;
        OnClickGuildRewardEvent = _OnClick;
        curGuildLively = _livelyRewardRef;
        if (livelyAmount != null) livelyAmount.text = ConfigMng.Instance.GetUItext(138, new string[1] { _livelyRewardRef.need.ToString() }); ;
        if (doubleRewardDes != null) doubleRewardDes.enabled = false;
        bool haveGot = GameCenter.guildMng.HaveGotLivelyReward(_livelyRewardRef.id);
        bool canGet = GameCenter.guildMng.CurLivelyCount >= _livelyRewardRef.need;
        int curLivelyCount = GameCenter.dailyMustDoMng.CurLivelyCount;
        if (haveGot)
        {
            UIEventListener.Get(gameObject).onClick = null;
        }
        else
        {
            UIEventListener.Get(gameObject).onClick = OnClickReward;
        }
        if (getPic != null)
        {
            getPic.enabled = canGet;
            getPic.IsGray = haveGot ? UISpriteEx.ColorGray.Gray : UISpriteEx.ColorGray.normal;
        }
        if (cantGetPic != null) cantGetPic.enabled = !canGet;//不可领显示
        if (redTipIcon != null) redTipIcon.enabled = canGet && !haveGot;
    }

    void OnClickReward(GameObject go)
    {
        if (OnClickRewardEvent != null && curLively != null)
            OnClickRewardEvent(curLively);
        if (OnClickGuildRewardEvent != null && curGuildLively != null)
            OnClickGuildRewardEvent(curGuildLively);
    }
}
