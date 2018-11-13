/// <summary>
/// 何明军
/// 2016/4/19
/// UIToggle 补充组件。
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIToggleCheckState : MonoBehaviour {

	UIToggle[] togs;
	// Use this for initialization
	void Start () {
		togs = gameObject.GetComponentsInChildren<UIToggle>();
		if(togs == null || togs.Length == 0){
			return ;
		}
		for(int i=0,len=togs.Length;i<len;i++){
			if(togs[i] != null){
				togs[i].group = len;
				EventDelegate.Add(togs[i].onChange, onChange);
			}
		}
	}
	
	void OnDisable(){
		if(togs == null || togs.Length == 0){
			return ;
		}
		for(int i=0,len=togs.Length;i<len;i++){
			if(togs[i] != null)EventDelegate.Remove(togs[i].onChange, onChange);
		}
	}
	
	void onChange(){
		for(int i=0,len=togs.Length;i<len;i++){
			if(togs[i] != null && togs[i].gameObject.activeSelf){
//				if(togs[i].value){
				togs[i].instantTween = togs[i].value;
				List<GameObject> listChild = togs[i].GetComponent<UIToggledObjects>().activate;
				foreach(GameObject game in listChild){
					if(game != null)game.SetActive(togs[i].value);
				}
//				}
			}
		}
	}
}
