/// <summary>
/// 邀请好友，选择好友或者招募化身
/// 何明军
/// 2016/6/22
/// </summary>


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InvitationPlayerWnd : MonoBehaviour {

	public UIPanel panel;
	
	public InvitationPlayerUI itemPlayerUI;
	
	public GameObject btnAll;

    public UILabel noInvitate;
	
	List<OtherPlayerInfo> listPlayer = new List<OtherPlayerInfo>();
	public List<OtherPlayerInfo> ListPlayer{
		get{
			return listPlayer;
		}
		set{
			listPlayer = value;
		}
	}
	
	Vector3 panelStartPosition = Vector3.zero;
	UIGrid grid = null;
	List<InvitationPlayerUI> items = new List<InvitationPlayerUI>();
	
	void Awake(){
		if(itemPlayerUI == null) return ;
		items.Add(itemPlayerUI);
		if(panel != null)panelStartPosition = panel.transform.localPosition;
		grid = itemPlayerUI.transform.parent.GetComponent<UIGrid>();
	}
	
	void ShowPlayer(){
		if(itemPlayerUI == null) return ;
		if(ListPlayer.Count <= 0){
            if (noInvitate != null) noInvitate.gameObject.SetActive(true);
			itemPlayerUI.gameObject.SetActive(false);
			return ;
		}
        if (noInvitate != null) noInvitate.gameObject.SetActive(false);
		InvitationPlayerUI playerUI = null;
		GameObject go = null;
		OtherPlayerInfo data = null;
        for (int j = 0; j < items.Count; j++)
        {
            items[j].gameObject.SetActive(false);
        }
        int i = 0;
		for(;i<ListPlayer.Count;i++){
			data = ListPlayer[i];
            if (GameCenter.duplicateMng.CopyTeams.ContainsKey(data.ServerInstanceID)) continue;
			if(items.Count <= i){
				go = (GameObject)GameObject.Instantiate(itemPlayerUI.gameObject);
				go.transform.parent = itemPlayerUI.transform.parent;
				go.transform.localPosition = Vector3.zero;
				go.transform.localScale = Vector3.one;
				go.name = "InvitationPlayer" + data.ServerInstanceID;
				playerUI = go.GetComponent<InvitationPlayerUI>();
				items.Add(playerUI);
			}else{
				go = items[i].gameObject;
				playerUI = items[i];
			}
			playerUI.SetPlayer(data);
			if(playerUI.btn != null){
				UIEventListener.Get(playerUI.btn).onClick = OnClickFriend;
				UIEventListener.Get(playerUI.btn).parameter = data;
			}
			if(playerUI.isSelect != null){
				EventDelegate.Remove(playerUI.isSelect.onChange,SelectOnChange);
				EventDelegate.Add(playerUI.isSelect.onChange,SelectOnChange);
			}
			go.SetActive(true);
		}
		
		for(;i < items.Count;i++){
			items[i].gameObject.SetActive(false);
		}
		
		if(grid != null)grid.repositionNow = true;
		if(panel != null)SpringPanel.Begin(panel.gameObject,panelStartPosition,10f);
		SetToggleBox();
	}
	/// <summary>
	/// 邮件好友
	/// </summary>
	void OnClickMail(GameObject games){
		InvitationPlayerUI playerUI = games.GetComponent<InvitationPlayerUI>();
		if(playerUI != null && playerUI.GetPlayer() != null){
			GameCenter.mailBoxMng.mailWriteData = new MailWriteData(playerUI.GetPlayer().Name);
			GameCenter.mailBoxMng.OnMailWriteDataUpdate();
		}
	}
	/// <summary>
	/// 多人准备好友
	/// </summary>
	void OnClickFriend(GameObject games){
		OtherPlayerInfo data = UIEventListener.Get(games).parameter as OtherPlayerInfo;
		if(!GameCenter.duplicateMng.CopyTeams.ContainsKey(data.ServerInstanceID))GameCenter.duplicateMng.C2S_ReqCopyInFriend(data.ServerInstanceID);
	}
	///<summary>
	///招募化身逻辑：服务端只记录被招募的化身ID并在进入副本时模拟通知化身加入队伍，服务端化身没有真实的进入队伍。
	/// 故：刷新队伍，刷新化身显示，存储化身数据是客服端维护的。并在副本退出请求时清除队伍中的化身数据。
	/// </summary>
	List<int> SelectIDs = new List<int>();
	/// <summary>
	/// 选中化身
	/// </summary>
	void SelectOnChange(){
		UIToggle tog = null;
		
		for(int i=0;i<items.Count;i++){
			tog = items[i].isSelect;
			if(tog == null || items[i].GetPlayer() == null)continue;
			int id = items[i].GetPlayer().ServerInstanceID;
			if(tog.value){
				if(!SelectIDs.Contains(id) && (SelectIDs.Count + count < 3)){
//					Debug.Log("SelectIDs  add   " + id);
					SelectIDs.Add(id);
				}
			}else{
				if(SelectIDs.Contains(id)){
//					Debug.Log("SelectIDs  Remove   " + id);
					SelectIDs.Remove(id);
				}
			}
		}
		SetToggleBox();
	}
	
	void SetToggleBox(){
		UIToggle tog = null;
		for(int i=0;i<items.Count;i++){
			tog = items[i].isSelect;
			if(tog != null && items[i].GetPlayer() != null && !SelectIDs.Contains(items[i].GetPlayer().ServerInstanceID)){
				tog.GetComponent<BoxCollider>().enabled = (SelectIDs.Count + count) < 3;
			}
		}
	}
	int count = 0;
	void OnEnable(){
		SelectIDs.Clear();
		count = GameCenter.duplicateMng.CopyTeams.Count;
		ShowPlayer();
		if(btnAll != null)UIEventListener.Get(btnAll).onClick = delegate {
			GameCenter.duplicateMng.AddInvitationToCopyTeams(SelectIDs);
		};
	}
	
	void OnDisable(){
		for(int i =0;i < items.Count;i++){
			if(items[i].isSelect == null)continue;
			EventDelegate.Remove(items[i].isSelect.onChange,SelectOnChange);
			items[i].isSelect.GetComponent<BoxCollider>().enabled = true;
			items[i].isSelect.value = false;
		}
	}
}
