//======================================================
//作者:何明军
//日期:16/6/17
//用途:竞技场玩家UI
//======================================================

using UnityEngine;
using System.Collections;

public class ArenaPlayerSingle : MonoBehaviour {

	public bool isMain = false;
	
	public UIProgressBar hp;
	public UIProgressBar mp;
	public UILabel hpDes;
	public UILabel mpDes;
	public UILabel name;
	public UILabel lev;
	public UISprite icon;
	
	PlayerBaseInfo info;
	
	void Show(){
		if(hp != null)hp.value = (float)info.CurHP/(float)info.MaxHP;
		if(hpDes != null)hpDes.text = (int)(hp.value * 100) + "%";
		
		if(mp != null)mp.value = (float)info.CurMP/(float)info.MaxMP;
//		mpDes.text = mp.value + "%";
	}
	
	void ShowBase(ActorBaseTag tag, ulong val,bool fes){
		Show();
	}
	void ShowProperty(ActorPropertyTag tag, long val,bool fes){
		Show();
	}
	void OnEnable(){
		OnOPCInfoListUpdate();
		SceneMng.OnOPCInfoListUpdate += OnOPCInfoListUpdate;
	}
	
	void OnOPCInfoListUpdate(){
		if(isMain){
			info = GameCenter.mainPlayerMng.MainPlayerInfo;
		}else{
			OtherPlayerInfo otherInfo = null;
			foreach(int _info in GameCenter.sceneMng.OPCInfoDictionary.Keys){
				if(otherInfo == null)otherInfo = GameCenter.sceneMng.OPCInfoDictionary[_info]  as OtherPlayerInfo;
			}
			info = otherInfo;
		}
		if(info == null)return ;
		Show();
		if(name != null)name.text = info.Name;
		if(lev != null)lev.text = info.LevelDes;
		if(icon != null)icon.spriteName = info.IconName;
		info.OnBaseUpdate += ShowBase;
		info.OnPropertyUpdate += ShowProperty;
	}
	
	void OnDisable(){
		if(info == null)return ;
		info.OnBaseUpdate -= ShowBase;
		info.OnPropertyUpdate -= ShowProperty;
		SceneMng.OnOPCInfoListUpdate -= OnOPCInfoListUpdate;
	}
}
