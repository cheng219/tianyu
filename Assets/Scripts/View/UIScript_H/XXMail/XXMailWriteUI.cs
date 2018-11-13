/// <summary>
/// 写邮件
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class XXMailWriteUI : MonoBehaviour {
	public UIInput name;
	public UIInput titil;
	public UIInput content;
	
	public UIButton btnSend;
	public UIButton openFriend;
	
	public void InWriteData(MailWriteData data){
		name.value = data.name;
		titil.value = string.Empty;
		content.value = string.Empty;
		gameObject.SetActive(true);
	}
	
	void OnEnable(){
		titil.value = string.Empty;
		content.value = string.Empty;
		
		if(btnSend != null)UIEventListener.Get(btnSend.gameObject).onClick = delegate {
			if(!GameCenter.loginMng.CheckBadWord(titil.value)){return;}
			if(!GameCenter.loginMng.CheckBadWord(content.value)){return;}
			if(string.IsNullOrEmpty(name.value)){
				GameCenter.messageMng.AddClientMsg(373);
				return ;
			}
			MailWriteData data = new MailWriteData(name.value,titil.value,content.value);
			GameCenter.mailBoxMng.C2S_ReplyMail(data);
		};
		GameCenter.mailBoxMng.OnSendSuccessMail = delegate {
			gameObject.SetActive(false);	
		};
		
		if(name != null)EventDelegate.Remove(name.onChange,OnChangeName);
		if(titil != null)EventDelegate.Remove(titil.onChange,OnChangeTitil);
		if(content != null)EventDelegate.Remove(content.onChange,OnChangeContent);
		if(name != null)EventDelegate.Add(name.onChange,OnChangeName);
		if(titil != null)EventDelegate.Add(titil.onChange,OnChangeTitil);
		if(content != null)EventDelegate.Add(content.onChange,OnChangeContent);
	}
	
	void OnChangeName(){
		string contents = GameCenter.loginMng.FontHasCharacter(name.label.bitmapFont,name.value);
		if(!string.IsNullOrEmpty(contents)){
			name.value = contents;
			GameCenter.messageMng.AddClientMsg(300);
		}
	}
	
	void OnChangeTitil(){
		string contents = GameCenter.loginMng.FontHasCharacter(titil.label.bitmapFont,titil.value);
		if(!string.IsNullOrEmpty(contents)){
			titil.value = contents;
			GameCenter.messageMng.AddClientMsg(300);
		}
	}
	
	void OnChangeContent(){
		string contents = GameCenter.loginMng.FontHasCharacter(content.label.bitmapFont,content.value);
		if(!string.IsNullOrEmpty(contents)){
			content.value = contents;
			GameCenter.messageMng.AddClientMsg(300);
		}
	}
}
