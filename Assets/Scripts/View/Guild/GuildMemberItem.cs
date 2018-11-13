//==================================
//作者：邓成
//日期：2016/4/14
//用途：仙盟成员子界面类
//=================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GuildMemberItem : MonoBehaviour {

    public UILabel memberName;
    public UILabel memberLv;
    public UILabel memberContribution;
    public UILabel memFightValue;
    public UILabel memState;
	public UIButton btnAgree;
	public UIButton btnRefuse;
	public UIButton btnManage;
	public UISprite presidentIcon;
	public UISprite vicePresidentIcon;
	public UISprite elderIcon;

	public UIToggle toggleSelect;
	protected GuildMemberData guildMemberData;
	protected System.Action<GuildMemberData,bool> selectCallback;

    public void SetDate(GuildMemberData data, System.Action<GuildMemberData,bool> _callback)
    {
		if(memberName != null)memberName.text = data.memberName;
		if(memberLv != null)memberLv.text = ConfigMng.Instance.GetLevelDes(data.memberLv);// + "pos:"+data.memberPosition.ToString();

		if(presidentIcon != null)presidentIcon.enabled = false;
		if(elderIcon != null)elderIcon.enabled = false;
		if(vicePresidentIcon != null)vicePresidentIcon.enabled = false;
        switch (data.memberPosition)
        {
            case GuildMemPosition.MEMBER:
                break;
            case GuildMemPosition.ELDER:
				if(elderIcon != null)elderIcon.enabled = true;
                break;
            case GuildMemPosition.VICECHAIRMAN:
				if(vicePresidentIcon != null)vicePresidentIcon.enabled = true;
                break;
            case GuildMemPosition.CHAIRMAN:
				if(presidentIcon != null)presidentIcon.enabled = true;
                break;
        }
		if(memberContribution != null)memberContribution.text = data.donate.ToString()+"/"+data.allDonate.ToString();
		if(memFightValue != null)memFightValue.text = data.fightValue.ToString();
		//if(memState != null)memState.text = data.memberPosition.ToString();
		if (data.recentTime == 0) {
			if (memState != null)
				memState.text = ConfigMng.Instance.GetUItext(284);
		} else {
			if (memState != null)
				memState.text = GameHelper.GetRecentTimeStr(data.recentTime);
		}
		selectCallback = _callback;
		guildMemberData = data;
		if(toggleSelect != null)
		{
			EventDelegate.Remove(toggleSelect.onChange,OnChange);
			if(data.memberID != GameCenter.mainPlayerMng.MainPlayerInfo.ServerInstanceID)
				EventDelegate.Add(toggleSelect.onChange,OnChange);
		}
		if(btnManage != null)
		{
			btnManage.isEnabled = (data.memberID != GameCenter.mainPlayerMng.MainPlayerInfo.ServerInstanceID);
			UIEventListener.Get(btnManage.gameObject).onClick = ManageMem;
		}
    }
	public void SetDate(GuildMemberData data)
	{
		if(memberName != null)memberName.text = data.memberName;
		if(memberLv != null)memberLv.text = ConfigMng.Instance.GetLevelDes(data.memberLv);
		if(memFightValue != null)memFightValue.text = data.fightValue.ToString();
        if (memState != null) memState.text = (data.isOnline == 1) ? ConfigMng.Instance.GetUItext(284) : ConfigMng.Instance.GetUItext(285);
		if(btnAgree != null)UIEventListener.Get(btnAgree.gameObject).onClick = (x)=>
		{
			if(GameCenter.guildMng.MyGuildInfo == null || GameCenter.guildMng.MyGuildInfo.MyPosition == GuildMemPosition.MEMBER)
			{
				GameCenter.messageMng.AddClientMsg(55);
				return;
			}
			GameCenter.guildMng.C2S_ReplyJoin(data.memberID,1);
		};
		if(btnRefuse != null)UIEventListener.Get(btnRefuse.gameObject).onClick = (y)=>
		{
			if(GameCenter.guildMng.MyGuildInfo == null || GameCenter.guildMng.MyGuildInfo.MyPosition == GuildMemPosition.MEMBER)
			{
				GameCenter.messageMng.AddClientMsg(55);
				return;
			}
			GameCenter.guildMng.C2S_ReplyJoin(data.memberID,0);
		};
	}

	void OnChange()
	{
		if(toggleSelect != null && toggleSelect.value && selectCallback != null)
			selectCallback(guildMemberData,true);
	}
	void ManageMem(GameObject go)
	{
		if(selectCallback != null)
			selectCallback(guildMemberData,true);
	}

	public static GuildMemberItem CreateNew(Transform _parent,Vector3 vector3)
    {
        GameObject go = null;
        UnityEngine.Object prefab = exResources.GetResource(ResourceType.GUI, "Guild/GuildMemberItem");
        go = Instantiate(prefab) as GameObject;
        prefab = null;
        go.transform.parent = _parent;
		go.transform.localPosition = vector3;
        go.transform.localScale = Vector3.one;
        go.SetActive(true);

        GuildMemberItem memberItem = go.GetComponent<GuildMemberItem>();
        if (memberItem == null) memberItem = go.AddComponent<GuildMemberItem>();
        return memberItem;
    }
	public static GuildMemberItem CreateNewJoin(Transform _parent,Vector3 vector3)
	{
		GameObject go = null;
		UnityEngine.Object prefab = exResources.GetResource(ResourceType.GUI, "Guild/GuildJoinMemItem");
		go = Instantiate(prefab) as GameObject;
        prefab = null;
		go.transform.parent = _parent;
		go.transform.localPosition = vector3;
		go.transform.localScale = Vector3.one;
		go.SetActive(true);

		GuildMemberItem memberItem = go.GetComponent<GuildMemberItem>();
		if (memberItem == null) memberItem = go.AddComponent<GuildMemberItem>();
		return memberItem;
	}
}
