//===============================
//作者：邓成
//日期：2016/5/16
//用途：攻城战界面类
//===============================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;

public class GuildSiegeWnd : GUIBase {

	public UITexture backTexture;
	/// <summary>
	/// 加入战斗
	/// </summary>
	public UIButton btnJoinSiege;
	/// <summary>
	/// 宣告攻城
	/// </summary>
	public UIButton btnTellSiege;
	public UIButton btnSureTellSiege;

	public UIButton btnApplyList;
	public UIButton btnStore;
	public UILabel labGuildName;
	public UILabel labPresident;
	public UIButton btnClose;
	public UITexture textureShow;

	public GameObject applyGo;
	public UIPanel applyPanel;
	public Vector4 positionInfo;
	protected Dictionary<int,GuildSiegeApplyItemUI> allItemList = new Dictionary<int, GuildSiegeApplyItemUI>();

	void Awake()
	{
		layer = GUIZLayer.NORMALWINDOW;
		mutualExclusion = true;
		if(btnJoinSiege != null)UIEventListener.Get(btnJoinSiege.gameObject).onClick = JoinGuildSiege;
		if(btnSureTellSiege != null)UIEventListener.Get(btnSureTellSiege.gameObject).onClick = TellGuildSiege;
		if(btnApplyList != null)UIEventListener.Get(btnApplyList.gameObject).onClick = OpenApplyList;
		if(btnStore != null)UIEventListener.Get(btnStore.gameObject).onClick = OpenGuildSiegeStore;
		if(btnClose != null)UIEventListener.Get(btnClose.gameObject).onClick = CloseWnd;
	}
	protected override void OnOpen ()
	{
		base.OnOpen ();
		ConfigMng.Instance.GetBigUIIcon("Pic_xmgcz_tu",SetBackTexture);
		GameCenter.activityMng.C2S_ReqGuildSiegeInfo();
	}
	protected override void OnClose ()
	{
		base.OnClose ();
		ConfigMng.Instance.RemoveBigUIIcon("Pic_xmgcz_tu");
	}
	protected override void HandEvent (bool _bind)
	{
		base.HandEvent (_bind);
		if(_bind)
		{
			GameCenter.activityMng.OnGotGuildSiegeInfoEvent += ShowGuildSiegeInfo;
			GameCenter.activityMng.OnGotGuildSiegeApplyListEvent += ShowApplyList;
		}else
		{
			GameCenter.activityMng.OnGotGuildSiegeInfoEvent -= ShowGuildSiegeInfo;
			GameCenter.activityMng.OnGotGuildSiegeApplyListEvent -= ShowApplyList;
		}
	}
	protected void SetBackTexture(Texture2D texture2D )
	{
		if(backTexture != null)
			backTexture.mainTexture = texture2D;
	}
	void ShowGuildSiegeInfo()
	{
		PlayerBaseInfo info = GameCenter.activityMng.GuildSiegeCastellan;
		if(labGuildName != null)//默认文本44:GM军团
		{
			labGuildName.text = string.IsNullOrEmpty(info.GuildName)?ConfigMng.Instance.GetUItext(44):info.GuildName;
		}
		if(labPresident != null)//默认文本43:龙傲天
		{
			labPresident.text = string.IsNullOrEmpty(info.Name)?ConfigMng.Instance.GetUItext(43):info.Name;
		}
		if(textureShow != null)
		{
			if(string.IsNullOrEmpty(info.Name))
			{
				GameCenter.previewManager.TryPreviewSinglePlayer(new PlayerBaseInfo(1,0),textureShow,true);//暂时显示战士模型
			}else
			{
				GameCenter.previewManager.TryPreviewSinglePlayer(info,textureShow,true);
			}
		}
		if(btnTellSiege != null)//守城帮派成员宣告攻城按钮灰掉
			btnTellSiege.isEnabled = (!GameCenter.activityMng.HadRepplySiege && !info.GuildName.Equals(GameCenter.mainPlayerMng.MainPlayerInfo.GuildName));
		if(btnJoinSiege != null)
		{
			bool isOpen = (GameCenter.activityMng.GetActivityState(ActivityType.FAIRYAUSIEGE) == ActivityState.ONGOING);
			btnJoinSiege.isEnabled = isOpen;
		}
	}
	void ShowApplyList()
	{
		HideItems();
		List<req_apply_list> rankList = GameCenter.activityMng.GuildSiegeApplyList;;
		for (int i = 0,max=rankList.Count; i < max; i++) {
			GuildSiegeApplyItemUI item = null;
			if(!allItemList.TryGetValue(i,out item))
			{
				GameObject go = Instantiate(exResources.GetResource(ResourceType.GUI,"GuildActivity/ApplicationItem")) as GameObject;
				if(go != null)
				{
					item = go.GetComponent<GuildSiegeApplyItemUI>();
					if(applyPanel != null)go.transform.parent = applyPanel.transform;
					go.transform.localScale = Vector3.one;
				}
                go = null;
				allItemList[i] = item;
			}
			item = allItemList[i];
			if(item != null)
			{
				item.gameObject.SetActive(true);
				item.transform.localPosition = new Vector3(positionInfo.x+positionInfo.z,positionInfo.y+positionInfo.w*i,0f);
				item.SetData(rankList[i]);
			}
		}
	}
	void HideItems()
	{
		foreach (var item in allItemList.Keys) {
			if(allItemList[item] != null)allItemList[item].gameObject.SetActive(false);
		}
	}
	void JoinGuildSiege(GameObject go)
	{
		GameCenter.activityMng.C2S_ReqJoinGuildSiege();
	}
	void TellGuildSiege(GameObject go)
	{
		GameCenter.activityMng.C2S_ReqApplyGuildSiege();
	}
	void OpenApplyList(GameObject go)
	{
		if(applyGo != null)applyGo.SetActive(true);
		GameCenter.activityMng.C2S_ReqGuildSiegeApplyList();
	}
	void OpenGuildSiegeStore(GameObject go)
	{
		SwitchToSubWnd(SubGUIType.GUILDCITYSTORE);
	}
	void CloseWnd(GameObject go)
	{
        if (GameCenter.guildMng.NeedOpenGuildWnd)
            GameCenter.uIMng.SwitchToUI(GUIType.GUILDMAIN);
        else
            GameCenter.uIMng.SwitchToUI(GUIType.NONE);
	}

}
