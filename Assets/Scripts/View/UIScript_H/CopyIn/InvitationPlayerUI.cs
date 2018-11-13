/// <summary>
/// 何明军
/// 招募玩家UI
/// 2016/6/22
/// </summary>

using UnityEngine;
using System.Collections;

public class InvitationPlayerUI : MonoBehaviour {
	
	public UILabel name;
	public UILabel lev;
	public UILabel fight;
	public UILabel profName;
	public UISprite icon;
	
	public GameObject btn;
	public UIToggle isSelect;
	
	OtherPlayerInfo player;
	public OtherPlayerInfo GetPlayer(){
		return player;
	}
	
	public bool GetSelect(){
		return isSelect== null ? false : isSelect.value;
	}

	public void SetPlayer(OtherPlayerInfo _player){
		player = _player;
		if(player == null){
			return ;
		}
		if(name != null)name.text = player.Name;
		if(lev != null)lev.text = player.LevelDes;
		if(fight != null)fight.text = player.FightValue.ToString();
		if(profName != null)profName.text = player.ProfName;
		if(icon != null)icon.spriteName = player.IconName;
		if(isSelect != null){
			isSelect.value = false;
			isSelect.GetComponent<BoxCollider>().enabled = true;
		}
	}
	
//	void SelectOnChange(){
//		if(isSelect != null && isSelect.value){
//			
//		}
//	}
	
//	void OnEnable(){
//		if(isSelect != null){
//			isSelect.startsActive = false;
//			isSelect.GetComponent<BoxCollider>().enabled = true;
//		}
//	}
//	
	void OnDisable(){
		if(isSelect != null){
			isSelect.value = false;
			isSelect.GetComponent<BoxCollider>().enabled = true;
		}
		player = null;
	}
}
