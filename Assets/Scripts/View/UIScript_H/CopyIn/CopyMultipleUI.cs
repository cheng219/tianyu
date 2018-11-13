/// <summary>
/// 何明军
/// 2016/4/7
/// 副本入口界面
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CopyMultipleUI : GUIBase {

    public GameObject btnToTeamFri;
	public GameObject btnClose;
	
	public GameObject btnOut;
	public GameObject btnPerpare;
	public GameObject btnNoPerpare;
	
	public GameObject btnBigin;
	public GameObject btnToFriend;
	
	public InvitationPlayerWnd toFriend;
	public InvitationPlayerWnd toInvitation;
	
	public GameObject isTeaml;
	public GameObject noTeaml;

    public UILabel diffcultyName;
	//public UILabel[] difficulty;//难度
	//public ItemUI[] items;
	public UILabel name;

    public CopyMultipleRoboteRewardUi reward1;//一个非化身奖励
    public CopyMultipleRoboteRewardUi reward2;//两个非化身奖励
	
	public CopyInItemTeamUI[] teamPlayer;
	
	public UITexture textIcon;
    public UITimer timerLabel;
    public UISpriteEx spEx;
    public UILabel normalLab;

	public int CopyGroupID{
		get{
			return GameCenter.duplicateMng.copyGroupID;
		}
	}
	
	CopyInItemDataInfo CurData{
		get{
			if(!GameCenter.duplicateMng.CopyDic.ContainsKey(CopyGroupID)){
				return null;
			}
			return GameCenter.duplicateMng.CopyDic[CopyGroupID];
		}
	}
	
	CopyGroupRef RefData{
		get{
			return ConfigMng.Instance.GetCopyGroupRef(CopyGroupID);
		}
	}
	
	void Awake(){
		mutualExclusion = false;
		Layer = GUIZLayer.COVER;

        UIEventListener.Get(btnToTeamFri).onClick = delegate
        {
            if (GameCenter.duplicateMng.isClickAddFri) return;
            GameCenter.duplicateMng.C2S_ReqWoldRecruit();
            GameCenter.duplicateMng.OnClickAddFreiend();
        };
	}
	
	void SetTogValue(){ 
		if(CurData == null)return ;
		int index = RefData.copy.IndexOf(CurData.curCopyScene); 
        //for (int i = 0, max = difficulty.Length; i < max; i++)
        //{
        //    if (i == index) difficulty[index].gameObject.SetActive(true);
        //    else difficulty[i].gameObject.SetActive(false);
        //} 
        CopyRef copy = ConfigMng.Instance.GetCopyRef(CurData.curCopyScene);

        //if (reward1 != null) reward1.SetGetState(GameCenter.teamMng.TeammateCount >= 2);
        //if (reward2 != null) reward2.SetGetState(GameCenter.teamMng.TeammateCount >= 3);
        if (copy != null)
        {
            string str;
            switch (index)
            { 
                case 0:
                    str = "[44D11EFF]" + copy.difficulty;
                    break;
                case 1:
                    str = "[40AFBEFF]" + copy.difficulty;
                    break;
                case 2:
                    str = "[BD54FFFF]" + copy.difficulty;
                    break;
                case 3:
                    str = "[FF2929FF]" + copy.difficulty;
                    break; 
                default :
                    str = copy.difficulty;
                    break;
            }
            if (diffcultyName != null) diffcultyName.text = str;
            int count = 0;
            foreach (CopySceneTeamPlayerInfo data in GameCenter.duplicateMng.CopyTeams.Values)
            {
                if (!data.isAvatar)
                {
                    ++count;
                }
            } 
            if (reward1 != null) reward1.SetData(copy.reward1, count >= 2);
            if (reward2 != null) reward2.SetData(copy.reward2, count >= 3); 
        }
        else
        {
            Debug.Log("副本表中找不到id为：" + CurData.curCopyScene + "的数据");
        }
		ShowCopyPerpare();
	}
	 
    void SetHideOrTrue()
    {
        GameCenter.duplicateMng.isClickAddFri = true; 
        timerLabel.gameObject.SetActive(true);
        normalLab.gameObject.SetActive(false);
        spEx.IsGray = UISpriteEx.ColorGray.Gray;
        timerLabel.StartIntervalTimer(15);
        timerLabel.onTimeOut = SetNormal; 
    }
    void SetNormal(GameObject go)
    {
        GameCenter.duplicateMng.isClickAddFri = false; 
        timerLabel.gameObject.SetActive(false);
        normalLab.gameObject.SetActive(true);
        spEx.IsGray = UISpriteEx.ColorGray.normal; 
    }
	protected override void OnOpen ()
	{
		base.OnOpen ();
		Show();
		GameCenter.duplicateMng.OnCopyItemChange += SetTogValue;
		GameCenter.duplicateMng.OpenInvitationPlayer += OpenInvitationPlayer;
		
		GameCenter.duplicateMng.OnSelectChange += OnSelectChange;
        GameCenter.duplicateMng.OnCopyItemChange += Show;
		GameCenter.friendsMng.OnFriendsDicUpdata += OpenFriend;
        GameCenter.duplicateMng.OnClickAddFreiend += SetHideOrTrue;
        GameCenter.teamMng.onTeammateUpdateEvent += OpenInvitationPlayer;
		if(textIcon != null)ConfigMng.Instance.GetBigUIIcon("Pic_fbdr_ditu",delegate(Texture2D texture){
			textIcon.mainTexture = texture;
		});
        GameCenter.duplicateMng.C2S_InvitationPlayer();
        GameCenter.friendsMng.C2S_ReqFriendsList();
	}
	
	protected override void OnClose ()
	{
		base.OnClose ();
		
		GameCenter.duplicateMng.OnCopyItemChange -= SetTogValue;
		GameCenter.duplicateMng.OpenInvitationPlayer -= OpenInvitationPlayer;
		GameCenter.duplicateMng.OnSelectChange -= OnSelectChange;
        GameCenter.duplicateMng.OnCopyItemChange -= Show;
		GameCenter.friendsMng.OnFriendsDicUpdata -= OpenFriend;
        GameCenter.duplicateMng.OnClickAddFreiend -= SetHideOrTrue;
        GameCenter.teamMng.onTeammateUpdateEvent -= OpenInvitationPlayer;
		if(textIcon != null)textIcon.mainTexture = null;
		ConfigMng.Instance.RemoveBigUIIcon("Pic_fbdr_ditu");
        GameCenter.duplicateMng.isClickAddFri = false;
	}
	 
	void OpenFriend(){
		if(toFriend == null)return ;
		toFriend.ListPlayer = GameCenter.duplicateMng.GetOtherPlayerInfoS(); 
	}

    void ShowFriend()
    {
        toFriend.gameObject.SetActive(true);
    }
	
	void OnCopyItemTeamData(int _copyGroupID){
		Show();
	}
	
	void OnSelectChange(){
		int i =0;
		foreach(CopySceneTeamPlayerInfo data in GameCenter.duplicateMng.CopyTeams.Values){
			if(teamPlayer[i].IsData() != null ){
                if (data.pId != (int)teamPlayer[i].IsData().baseInfo.uid)
                    teamPlayer[i].TeamData(data, CurData);
			}else{
                teamPlayer[i].TeamData(data, CurData);
			}
			i++;
		}
		for(int len = teamPlayer.Length;i<len;i++){
            teamPlayer[i].TeamData(null, CurData);
		}
		if(toInvitation == null){
			return ;
		}
//		toInvitation.ListPlayer = GameCenter.endLessTrialsMng.openInvitationPlayerData;
		toInvitation.gameObject.SetActive(false);
	}

    //void ShowInvitationPlayer()
    //{ 
    //    toInvitation.gameObject.SetActive(true);
    //}
	
	void OpenInvitationPlayer(){ 
        if (toInvitation == null)
        {
            return;
        }
        toInvitation.ListPlayer = GameCenter.duplicateMng.OpenInvitationPlayerData; 
        CopyRef copy = ConfigMng.Instance.GetCopyRef(CurData.curCopyScene);

        int count = 0;
        foreach (CopySceneTeamPlayerInfo data in GameCenter.duplicateMng.CopyTeams.Values)
        {
            if (!data.isAvatar)
            {
                ++count;
            }
        } 
        if (copy != null)
        {
            if (reward1 != null) reward1.SetData(copy.reward1, count >= 2);
            if (reward2 != null) reward2.SetData(copy.reward2, count >= 3);  
        }
        else
        {
            Debug.Log("副本表中找不到id为：" + CurData.curCopyScene + "的数据");
        }
	}
	
	bool IsPerpare{
		get{
			if(GameCenter.duplicateMng.CopyTeams.ContainsKey(GameCenter.mainPlayerMng.MainPlayerInfo.ServerInstanceID)){
				return GameCenter.duplicateMng.CopyTeams[GameCenter.mainPlayerMng.MainPlayerInfo.ServerInstanceID].isPerpare;
			}
			return false;
		}
	}
	
	void ShowCopyPerpare(){
		btnPerpare.SetActive(!IsPerpare);
		btnNoPerpare.SetActive(IsPerpare);
		for(int i=0,len = teamPlayer.Length;i<len;i++){
			teamPlayer[i].SetPerpare();
		}
	}
	void Show(){
		if(CurData == null)return ; 
		bool teaml = GameCenter.teamMng.isInTeam &&  GameCenter.teamMng.isLeader;
		isTeaml.SetActive(teaml);
		noTeaml.SetActive(!teaml);
		name.text = RefData.name;
        //for(int i=0,len=items.Length;i<len;i++){
        //    if(items == null)continue;
        //    if(RefData.reward.Count > i){
         //       items[i].FillInfo(new EquipmentInfo(RefData.reward[i],EquipmentBelongTo.PREVIEW));
        //        if(items[i].itemName != null)items[i].itemName.enabled = false;
        //    }else{
        //        items[i].FillInfo(null);
        //    }
        //}
		
//		SetTeamsPlayer();
		OnSelectChange();
        SetTogValue();
		
		if(btnPerpare != null)btnPerpare.SetActive(!IsPerpare);
		if(btnNoPerpare != null)btnNoPerpare.SetActive(IsPerpare);
		UIEventListener.Get(btnOut).onClick = delegate {
			GameCenter.teamMng.C2S_TeamOut();
		};
        if (btnClose != null) UIEventListener.Get(btnClose).onClick = delegate
            {
            if (CurData != null && CurData.IsMagic)
            {
                if (GameCenter.teamMng.isInTeam && GameCenter.teamMng.isLeader)
                {
                    GameCenter.duplicateMng.C2S_ReqCopyInTeamClose(1);
                    GameCenter.uIMng.ReleaseGUI(GUIType.COPYMULTIPLEWND);
                }
                else if (GameCenter.teamMng.isInTeam && !GameCenter.teamMng.isLeader)
                {
                    MessageST msg = new MessageST();
                    msg.messID = 77;
                    msg.delYes = delegate
                    {
                        GameCenter.duplicateMng.C2S_ReqCopyInTeamClose(0);
                        GameCenter.uIMng.ReleaseGUI(GUIType.COPYMULTIPLEWND);
                    };
                    GameCenter.messageMng.AddClientMsg(msg);
                }
            }
            else
            {
                if (GameCenter.teamMng.isInTeam && GameCenter.teamMng.isLeader)
                {
                    GameCenter.duplicateMng.C2S_ReqCopyInTeamClose(1);
                }
                GameCenter.uIMng.ReleaseGUI(GUIType.COPYMULTIPLEWND);
            }
		};
        if (btnPerpare != null) UIEventListener.Get(btnPerpare).onClick = delegate
            {
			GameCenter.duplicateMng.C2S_ReqCopyInPerpare(1);
		};
        if (btnNoPerpare != null) UIEventListener.Get(btnNoPerpare).onClick = delegate
            {
			GameCenter.duplicateMng.C2S_ReqCopyInPerpare(2);
		};
        if (btnToFriend != null) UIEventListener.Get(btnToFriend).onClick = delegate
            { 
            ShowFriend();
		};
        if (btnBigin != null) UIEventListener.Get(btnBigin).onClick = delegate
            {
			if(toInvitation != null){
                toInvitation.gameObject.SetActive(false);
//				GameCenter.endLessTrialsMng.C2S_InCopyInvitationPlayer(toInvitation.SelectIDs);
			}
            if (toFriend != null) toFriend.gameObject.SetActive(false);
			if(CurData != null)GameCenter.duplicateMng.C2S_ToCopyItem(CopyGroupID,CurData.curCopyScene);
		};
        if (btnToTeamFri != null) btnToTeamFri.SetActive(GameCenter.teamMng.isLeader);
	}
	
	
	
}
