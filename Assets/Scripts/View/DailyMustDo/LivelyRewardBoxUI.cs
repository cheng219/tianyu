//===============================
//作者：邓成
//日期：2017/1/23
//用途：活跃度箱子奖励详情界面类
//===============================

using UnityEngine;
using System.Collections;

public class LivelyRewardBoxUI : MonoBehaviour {
    public UILabel labName;
    public ItemUIContainer items;
    public UIButton btnReward;

    public void SetData(LivelyRewardRef _livelyRewardRef)
    {
        if (gameObject != null) gameObject.SetActive(true);
        if (labName != null) labName.text = _livelyRewardRef.name.ToString();
        if (items != null) items.RefreshItems(_livelyRewardRef.reward, 4, _livelyRewardRef.reward.Count);
        if (btnReward != null)
        {
            bool haveGot = GameCenter.dailyMustDoMng.MustDoStateDic.ContainsKey(_livelyRewardRef.id) ? GameCenter.dailyMustDoMng.MustDoStateDic[_livelyRewardRef.id] : false;
            if (haveGot || GameCenter.dailyMustDoMng.CurLivelyCount < _livelyRewardRef.need)
            {
                btnReward.isEnabled = false;
            }
            else
            {
                btnReward.isEnabled = true;
                UIEventListener.Get(btnReward.gameObject).onClick = (x) =>
                {
                    GameCenter.dailyMustDoMng.C2S_ReqGetLivelyReward(_livelyRewardRef.id);
                };
            }
            
        }
    }

    public void SetData(GuildLivelyRewardRef _livelyRewardRef)
    {
        if (gameObject != null) gameObject.SetActive(true);
        if (labName != null) labName.text = _livelyRewardRef.name.ToString();
        if (items != null) items.RefreshItems(_livelyRewardRef.reward, 4, _livelyRewardRef.reward.Count);
        if (btnReward != null)
        {
            bool haveGot = GameCenter.guildMng.HaveGotLivelyReward(_livelyRewardRef.id);
            //Debug.Log("haveGot:" + haveGot + ",GameCenter.guildMng.CurLivelyCount:"+GameCenter.guildMng.CurLivelyCount+ ",_livelyRewardRef.need:" + _livelyRewardRef.need);
            if (haveGot || GameCenter.guildMng.CurLivelyCount < _livelyRewardRef.need)
            {
                btnReward.isEnabled = false;
            }
            else
            {
                btnReward.isEnabled = true;
                UIEventListener.Get(btnReward.gameObject).onClick = (x) =>
                {
                    GameCenter.guildMng.C2S_ReqLivelyReward(_livelyRewardRef.id);
                };
            }

        }
    }
}
