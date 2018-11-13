/// <summary>
/// 何明军
/// 2016/4/7
/// 副本入口界面
/// </summary>

using UnityEngine;
using System.Collections;

public class CopyInItemTeamUI : MonoBehaviour {

	public GameObject inPlayer;
	public GameObject noPlayer;
	
	public GameObject isTeamlShow;
	public GameObject btnToAvatar;
    //public GameObject btnToTeamFri;
	public GameObject btnOutTeam;
	public GameObject btnOtherOut;
	
	public UILabel pLev;
	public UILabel pfight;
	public UILabel pNum;
	
	public GameObject isLeader;
	public GameObject isAvatar;
	
	public GameObject perpare;
	public GameObject noPerpare;
	
	public UISprite pIcon;
    public UILabel name;
	TeamMenberInfo teamData;
//	public void TeamData(TeamMenberInfo data,int _copyGroupID){
//		teamData = data;
//		copyGroupID = _copyGroupID;
//		if(isAvatar != null)isAvatar.SetActive(false);
//		ShowData();
//	}
    CopyInItemDataInfo data = null;
    public InvitationPlayerWnd InvitationPlayer;

	public void TeamData(CopySceneTeamPlayerInfo player,CopyInItemDataInfo _data)//int _copyGroupID)
    {
        data = _data;
		if(player == null){
			teamData = null;
			if(isAvatar != null)isAvatar.SetActive(false);
		}else{
			OtherPlayerInfo info = GameCenter.duplicateMng.GetInvitationPlayerData(player.pId);
			if(info != null){
				st.net.NetBase.team_member_list baseInfo = new st.net.NetBase.team_member_list();
				baseInfo.uid = (uint)info.ServerInstanceID;
				baseInfo.name = info.Name;
				baseInfo.prof = (uint)info.Prof;
				baseInfo.fighting = (uint)info.FightValue;
				baseInfo.lev = (uint)info.Level;
				teamData = new TeamMenberInfo(baseInfo);
				if(isAvatar != null)isAvatar.SetActive(player.isAvatar);
			}else{
				if(player.pId == GameCenter.teamMng.LeaderId && GameCenter.teamMng.isLeader){
					st.net.NetBase.team_member_list mPlayer = new st.net.NetBase.team_member_list();
					mPlayer.uid = (uint)GameCenter.mainPlayerMng.MainPlayerInfo.ServerInstanceID;
					mPlayer.prof = (uint)GameCenter.mainPlayerMng.MainPlayerInfo.Prof;
					mPlayer.fighting = (uint)GameCenter.mainPlayerMng.MainPlayerInfo.FightValue;
					mPlayer.lev = (uint)GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel;
                    mPlayer.name = GameCenter.mainPlayerMng.MainPlayerInfo.Name;
					teamData = new TeamMenberInfo(mPlayer);
				}else{
					if(GameCenter.teamMng.TeammatesDic.ContainsKey(player.pId))teamData = GameCenter.teamMng.TeammatesDic[player.pId] as TeamMenberInfo;
				}
				
				if(isAvatar != null)isAvatar.SetActive(false);
			}
		}
		ShowData();
	}
	
	public TeamMenberInfo IsData(){
		return teamData;
	}
	
	void ShowData(){
        if (noPlayer != null) noPlayer.SetActive(teamData == null);
        if (inPlayer != null) inPlayer.SetActive(teamData != null);
        //btnToAvatar.SetActive(GameCenter.teamMng.isLeader);
        if (btnToAvatar != null) btnToAvatar.SetActive(GameCenter.teamMng.isLeader);
        //if (btnToTeamFri != null) btnToTeamFri.SetActive(GameCenter.teamMng.isLeader && data.IsMagic);

		if(teamData == null)return ;
		int uid = (int)teamData.baseInfo.uid;

        if (isLeader != null) isLeader.SetActive(GameCenter.teamMng.isLeader && uid == GameCenter.teamMng.LeaderId);
		if(pIcon != null){
			pIcon.spriteName = ConfigMng.Instance.GetPlayerConfig((int)teamData.baseInfo.prof).icon_half;
			pIcon.MakePixelPerfect();
		}
		
		AttributeRef attData = ConfigMng.Instance.GetAttributeRef((int)teamData.baseInfo.lev);
        if (attData.reborn > 0 && pLev != null)
        {
			pLev.text = ConfigMng.Instance.GetUItext(12,new string[2]{attData.reborn.ToString(),attData.display_level.ToString()});
		}else{
			pLev.text = ConfigMng.Instance.GetUItext(13,new string[1]{attData.display_level.ToString()});
		}

        if (pfight != null) pfight.text = teamData.baseInfo.fighting.ToString();
        if (name != null) name.text = teamData.baseInfo.name;
		int num = 0;
		if(GameCenter.duplicateMng.CopyTeams.ContainsKey(uid)){
			num = GameCenter.duplicateMng.CopyTeams[uid].pNum;
            if (pNum != null) pNum.text = num.ToString();
            if (pNum != null) pNum.transform.parent.gameObject.SetActive(!isAvatar.activeSelf && GameCenter.duplicateMng.CopyType < DuplicateMng.CopysType.MAGICTOWER);
			SetPerpare();
		}

        if (isTeamlShow != null) isTeamlShow.SetActive(GameCenter.teamMng.isLeader);
        if (btnOutTeam != null) btnOutTeam.SetActive(GameCenter.teamMng.isLeader && uid == GameCenter.teamMng.LeaderId);
        if (btnOtherOut != null) btnOtherOut.SetActive(GameCenter.teamMng.isLeader && uid != GameCenter.teamMng.LeaderId);
        //btnToAvatar.SetActive(GameCenter.teamMng.isLeader);
        if (btnToAvatar != null) btnToAvatar.SetActive(GameCenter.teamMng.isLeader);
        //if (btnToTeamFri != null) btnToTeamFri.SetActive(GameCenter.teamMng.isLeader && data.IsMagic);
	}
	
	public void SetPerpare(){
		if(teamData == null)return ;
		
		if((int)teamData.baseInfo.uid == GameCenter.teamMng.LeaderId){
			if(perpare != null)perpare.SetActive(true);
			if(noPerpare != null)noPerpare.SetActive(false);
			return ;
		}
		CopySceneTeamPlayerInfo sceneTeam = null;
		if(GameCenter.duplicateMng.CopyTeams.TryGetValue((int)teamData.baseInfo.uid,out sceneTeam)){
			if(perpare != null)perpare.SetActive(sceneTeam.isPerpare);
			if(noPerpare != null)noPerpare.SetActive(!sceneTeam.isPerpare);
		}
	}
	void OnEnable(){
		
		UIEventListener.Get(btnToAvatar).onClick = delegate { 
            if (InvitationPlayer != null) InvitationPlayer.gameObject.SetActive(true);
		};
        //UIEventListener.Get(btnToTeamFri).onClick = delegate
        //{
        //    if (GameCenter.duplicateMng.isClickAddFri) return;
        //    GameCenter.duplicateMng.C2S_ReqWoldRecruit();
        //    GameCenter.duplicateMng.OnClickAddFreiend();
        //};
		UIEventListener.Get(btnOutTeam).onClick = delegate {
			if(GameCenter.teamMng.isInTeam && GameCenter.teamMng.isLeader){
				GameCenter.teamMng.C2S_TeamDissolve();
			}
		};
		UIEventListener.Get(btnOtherOut).onClick = delegate {
			if(teamData == null)return ;
			if(!isAvatar.activeSelf){
				GameCenter.teamMng.C2S_TeamForceOut((int)teamData.baseInfo.uid);
			}else{
				GameCenter.duplicateMng.ReMoveInvitationToCopyTeams((int)teamData.baseInfo.uid);
			}
		};
	}
}
