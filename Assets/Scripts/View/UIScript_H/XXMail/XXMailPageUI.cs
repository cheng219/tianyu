/// <summary>
/// 何明军
/// 2016/4/19
/// 邮件页UI
/// </summary>


using UnityEngine;
using System.Collections;

public class XXMailPageUI : MonoBehaviour {

	public UISprite sprite;
	
	public void SetSelect(bool vals){
		
		if(vals)GetComponent<UIToggle>().value = vals;
	}
	
	public void InItSelect(bool vals){

		if(vals)GetComponent<UIToggle>().startsActive = vals;
	}
}
