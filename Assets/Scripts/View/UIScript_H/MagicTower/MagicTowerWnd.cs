/// <summary>
/// 何明军
/// 镇魔塔
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MagicTowerWnd : GUIBase {

    public UILabel isChallenged;
    //public GameObject btnL;
    //public GameObject btnR;
	public GameObject btnX;
    public UIToggle[] diffcuiltyTog;
	
	public GameObject btnReward;
	public GameObject btnOK;
	
	public UISprite childCurName;
	public UISprite curName;
	public UILabel lev;
	public UILabel fight;
	
	public ItemUI[] items;
	
	public UITexture textIcon;
	public GameObject[] uifx;
	
	int CurCopyWndType = 3;
	CopyGroupRef curData = null;
	int index = 0;
	int Index {
		get{
			return index;
		}
		set{
			if(value >= 0 && value <4){
				index = value;
				ShowChild();
			}
		}
	}
	
	void ShowChild(){
		if(curData == null)return ;
		CopyRef refd = ConfigMng.Instance.GetCopyRef(curData.copy[Index]);
		if(refd == null)return ;
		if(curName != null)curName.spriteName = refd.Icon;
		if(childCurName != null)childCurName.spriteName = refd.Icon;
		for(int i=0;i<uifx.Length;i++){
			if(uifx[i] == null)continue;
			if(i == Index){
				uifx[i].SetActive(true);
			}else{
				uifx[i].SetActive(false);
			}
		}
		AttributeRef attributeRef = ConfigMng.Instance.GetAttributeRef(refd.lvId > 0 ? refd.lvId : 1);
		if(attributeRef.reborn > 0){
			if(lev != null)lev.text = ConfigMng.Instance.GetUItext(12,new string[2]{attributeRef.reborn.ToString(),attributeRef.display_level.ToString()});
		}else{
			if(lev != null)lev.text = ConfigMng.Instance.GetUItext(13,new string[1]{attributeRef.display_level.ToString()});
		}
		if(fight != null)fight.text = refd.fighting.ToString();
	}
	
	void Awake(){
        GameCenter.duplicateMng.C2S_GetTowerChallengeNum();
		mutualExclusion = true;
		Layer = GUIZLayer.TOPWINDOW;
		GameCenter.duplicateMng.CopyType = (DuplicateMng.CopysType)CurCopyWndType;
		List<CopyGroupRef> dataList = ConfigMng.Instance.GetCopyGroupRefTable(CurCopyWndType);
		if(dataList.Count > 0){
			curData = dataList[0];
		}
        if (btnX != null) UIEventListener.Get(btnX).onClick = delegate
            {
			GameCenter.uIMng.SwitchToUI(GUIType.NONE);
		};
        //UIEventListener.Get(btnL).onClick = delegate {
        //    Index--;
        //};
        //UIEventListener.Get(btnR).onClick = delegate {
        //    Index++;
        //};
        if (btnReward != null) UIEventListener.Get(btnReward).onClick = delegate
            {
			GameCenter.shopMng.OpenWndByType(ShopItemType.SCORES);
		};
        for (int i = 0, max = diffcuiltyTog.Length; i < max; i++)
        {
            UIEventListener.Get(diffcuiltyTog[i].gameObject).onClick = OnTogChange;
            UIEventListener.Get(diffcuiltyTog[i].gameObject).parameter = i;
        }
        if (btnOK != null) UIEventListener.Get(btnOK).onClick = delegate
        { 
            if (GameCenter.duplicateMng.isChallengeMagicTower)
            {
                GameCenter.messageMng.AddClientMsg(559);
                return;
            }
            //DateTime date = DateTime.Now;
            DateTime date = GameCenter.instance.CurServerTime;
            if (date.Hour >= 12)
            {
                CopyRef refd = ConfigMng.Instance.GetCopyRef(curData.copy[Index]);
                bool isShow = refd.lvId <= GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel;
                if (!isShow)
                {
                    GameCenter.messageMng.AddClientMsg(341);
                    return;
                }
                if (GameCenter.teamMng.isInTeam && !GameCenter.teamMng.isLeader)
                {
                    if (GameCenter.duplicateMng.CopyTeams.Count <= 0 || curData.id != GameCenter.duplicateMng.copyGroupID)
                    {
                        GameCenter.messageMng.AddClientMsg(165);
                        return;
                    }
                    GameCenter.uIMng.SwitchToUI(GUIType.COPYMULTIPLEWND, GUIType.MagicTowerWnd);
                }
                GameCenter.duplicateMng.C2S_ReqCopyInTeamData(curData.id, curData.copy[Index]);
            }
            else
            {
                GameCenter.messageMng.AddClientMsg(173);
            }
        };
	}
    void OnTogChange(GameObject go)
    {
        Index = (int)UIEventListener.Get(go).parameter;
    }
	protected override void OnOpen ()
	{
		base.OnOpen ();
		Index = 0;
		for(int i=0,len=items.Length;i<len;i++){
			if(items == null)continue;
			if(curData.reward.Count > i){
				items[i].FillInfo(new EquipmentInfo(curData.reward[i],EquipmentBelongTo.PREVIEW));
				if(items[i].itemName != null)items[i].itemName.enabled = false;
				items[i].gameObject.SetActive(true);
			}else{
				items[i].FillInfo(null);
				items[i].gameObject.SetActive(false);
			}
		}
		
		if(textIcon != null)ConfigMng.Instance.GetBigUIIcon("Pic_jjc_bg",delegate(Texture2D texture){
			textIcon.mainTexture = texture;
		});
        GameCenter.duplicateMng.OnMagicChallengeUpdate += ShowChallenge;
	}
	
	protected override void OnClose ()
	{
		base.OnClose ();
		if(textIcon != null)textIcon.mainTexture = null;
		ConfigMng.Instance.RemoveBigUIIcon("Pic_jjc_bg");
        GameCenter.duplicateMng.OnMagicChallengeUpdate -= ShowChallenge;
	}

    void ShowChallenge()
    {
        if (isChallenged != null) isChallenged.gameObject.SetActive(GameCenter.duplicateMng.isChallengeMagicTower);
    }

//	void OnDestroy(){
//		ConfigMng.Instance.UnloadBigUIIcon();
//	}
}
