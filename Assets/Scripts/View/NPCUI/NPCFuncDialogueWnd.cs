//==============================================
//作者：邓成
//日期：2016/5/24
//用途：NPC功能窗口
//=================================================

using UnityEngine;
using System.Collections;
using System;

public class NPCFuncDialogueWnd : GUIBase {
    public Transform content;
    public Transform unContent;
    public GameObject texiao;

	public UIButton btnFunc;
	public UIButton btnClose;

	public UITexture npcTexture;
	public UILabel npcName;
	public UILabel npcTalk;

	protected NPCInfo curNpcInfo = null;
	protected NPCInfo CurNpcInfo
	{
		set
		{
			curNpcInfo = value;
			ShowNpcInfo();
		}
		get
		{
			return curNpcInfo;
		}
	}
	protected OtherPlayerInfo curModelInfo = null;
	protected OtherPlayerInfo CurModelInfo
	{
		set
		{
			curModelInfo = value;
			ShowModelInfo();
		}
		get
		{
			return curModelInfo;
		}
	}

	void Awake()
	{
		layer = GUIZLayer.NORMALWINDOW;
		mutualExclusion = true;
		if(btnFunc != null)UIEventListener.Get(btnFunc.gameObject).onClick = ClickFunc;
		if(btnClose != null)UIEventListener.Get(btnClose.gameObject).onClick = CloseWnd; 
        if (texiao != null) UIEventListener.Get(texiao).onClick = CloseWnd;
	}
	protected override void OnOpen ()
	{
		base.OnOpen ();
		NPC npc = GameCenter.curMainPlayer.CurTarget as NPC;
		if(npc != null)
		{
			CurNpcInfo = GameCenter.sceneMng.GetNPCInfo(npc.id);
		}
		Model model = GameCenter.curMainPlayer.CurTarget as Model;
		if(model != null)
		{
			CurModelInfo = GameCenter.sceneMng.GetModelInfo(model.id);
		}
			
	}
	protected override void OnClose ()
	{
		base.OnClose ();
	}

	void ShowNpcInfo()
	{
		if(CurNpcInfo == null)return;
		if(npcName != null)npcName.text = CurNpcInfo.Name;
		if(npcTexture != null)GameCenter.previewManager.TryPreviewSingelNPC(CurNpcInfo,npcTexture,PreviewConfigType.Dialog);
		NewFunctionRef funcRef = ConfigMng.Instance.GetNewFunctionRef(CurNpcInfo.Function);
        if (content != null) content.gameObject.SetActive(true);
        if (unContent != null) unContent.gameObject.SetActive(false);
		if(funcRef != null)
		{
			switch(funcRef.id)//这里处理特殊显示
			{
				case 7://结义 
                    if (npcTalk != null)
                    {
                        int count = GameCenter.swornMng.HisName().Count;
                        if (count == 1)//两人结义
                        {
                            npcTalk.text = ConfigMng.Instance.GetUItext(30, new string[1] { GameCenter.swornMng.HisName()[0]}); 
                        }
                        else if (count == 2)//三人结义
                        {
                            npcTalk.text = ConfigMng.Instance.GetUItext(31, new string[2] { GameCenter.swornMng.HisName()[0], GameCenter.swornMng.HisName()[1] });
                        }
                        else
                        {
                            if (content != null) content.gameObject.SetActive(false);
                            if (unContent != null) unContent.gameObject.SetActive(true); 
                        }
                    }
					break;
                case 8://仙侣 
                    if (npcTalk != null && GameCenter.coupleMng.HerName() != string.Empty)
                        npcTalk.text = ConfigMng.Instance.GetUItext(29, new string[1] { GameCenter.coupleMng.HerName() });
                    else
                    {
                        if (content != null) content.gameObject.SetActive(false);
                        if (unContent != null) unContent.gameObject.SetActive(true);
                    }
                    break;
				case 10://充值返利
					if(npcTalk != null)npcTalk.text = ConfigMng.Instance.GetUItext(130, new string[2] {GameCenter.rechargeMng.TestRechargeRewardDiamo.ToString(),GameCenter.rechargeMng.TestRechargeRewardVipExp.ToString() }); 
					break;
                case 9://夺宝奇兵
                    if (npcTalk != null) npcTalk.enabled = (GameCenter.activityMng.rewardId == 0);
                    if (content != null) content.gameObject.SetActive(GameCenter.activityMng.rewardId != 0);
                    if (GameCenter.activityMng.rewardId != 0 && content != null)
                    {
                        ItemUI itemUI = content.GetComponentInChildren<ItemUI>();
                        if (itemUI != null)
                            itemUI.FillInfo(new EquipmentInfo(GameCenter.activityMng.rewardId, EquipmentBelongTo.PREVIEW));
                    }
                    else
                        if (npcTalk != null) npcTalk.text = ConfigMng.Instance.GetUItext(334);
                    break;
			}
		}
	}
	void ShowModelInfo()
	{
		if(CurModelInfo == null)return;
		if(npcName != null)npcName.text = CurModelInfo.Name;
		if(npcTexture != null)GameCenter.previewManager.TryPreviewSinglePlayer(CurModelInfo,npcTexture,true);
		if(npcTalk != null)npcTalk.text = ConfigMng.Instance.GetUItext(335)+CurModelInfo.Name+ConfigMng.Instance.GetUItext(336);
	}
	void ClickFunc(GameObject go)
	{
		if(CurNpcInfo != null)
		{
			NewFunctionRef funcRef = ConfigMng.Instance.GetNewFunctionRef(CurNpcInfo.Function);
			if(funcRef != null)
			{
				switch(funcRef.id)//这里处理按钮功能
				{
				case 7://结义
                        GameCenter.swornMng.C2S_ReqAddBrother(ConfigMng.Instance.GetUItext(337));
					break;
                case 8://仙侣
                    GameCenter.uIMng.GenGUI(GUIType.MARRIAGE,true);
                    break;
				case 10://充值返利
					GameCenter.rechargeMng.C2S_ReqTestChargeReward();
					break;
				default:
					if(funcRef.UI_type == 1)
					{
						GameCenter.uIMng.SwitchToUI((GUIType)Enum.Parse(typeof(GUIType), funcRef.UI_name));
					}else if(funcRef.UI_type == 2)
					{
						GameCenter.uIMng.SwitchToSubUI((SubGUIType)Enum.Parse(typeof(SubGUIType), funcRef.UI_name));
					}
					break;
				}
			}
		}
		if(CurModelInfo != null)
		{
			GameCenter.activityMng.C2S_Morship();
			GameCenter.uIMng.SwitchToUI(GUIType.NONE);
		}
	}
	void CloseWnd(GameObject go)
	{
        if (CurNpcInfo != null)
        {
            NewFunctionRef funcRef = ConfigMng.Instance.GetNewFunctionRef(CurNpcInfo.Function);
            if (funcRef != null)
            {
                switch (funcRef.id)
                {
                    case 9:
                        if (GameCenter.activityMng.rewardId != 0)
                        {
                            //GameCenter.activityMng.rewardId = 0;
                            GameCenter.activityMng.C2S_ReqGetReward();
                            //Debug.Log("领取夺宝奇兵奖励！！！");
                        }
                        break;
                }
            }
        }
        GameCenter.uIMng.SwitchToUI(GUIType.NONE);
	}
}
