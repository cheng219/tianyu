/// <summary>
/// 何明军
/// 2016/4/19
/// 邮件对象UI
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class XXMailItemUI : MonoBehaviour {

	public XXMailWriteUI writeGame;
	
	public GameObject mRead;
	public GameObject mIsRead;
	
	public UILabel mTitil;
	public UILabel mSendTime;
	public UILabel mSendName;
	public UILabel mRestTime;
	public UILabel mContent;
	
	public UIButton mExtract;
	public UIButton mDel;
	public UIButton mReply;
	public List<ItemUI> items;
	
	MailData data = null;
	public MailData InMailData{
		get{
			return data;
		}
		set{
			if(value != null)data = value;
			data.OnMailContentUpdate -= Show;
			data.OnMailContentUpdate += Show;
			Show();
		}
	}
	
	void Show(){
		if(mRead != null)mRead.SetActive(data.IsRead);
		if(mIsRead != null)mIsRead.SetActive(!data.IsRead);
		if(mTitil != null)mTitil.text = data.Title;
		if(mContent != null)mContent.text = data.Content;
		if(mSendName != null)mSendName.text = data.SendName;
		if(mSendTime != null)mSendTime.text = data.SendTime;
		if(mRestTime != null)mRestTime.text = data.ExpireTime;
		
		List<uint> idList = new List<uint>();
		idList.Add((uint)data.id);
		if(mExtract != null){
			mExtract.gameObject.SetActive(data.items.Count > 0);
			UIEventListener.Get(mExtract.gameObject).onClick = delegate {
				GameCenter.mailBoxMng.C2S_ExtractMail(idList);
			};
		}
		if(mDel != null){
//			mDel.gameObject.SetActive(data.items.Count > 0);
			UIEventListener.Get(mDel.gameObject).onClick = delegate {
				if(data.items.Count > 0){
					GameCenter.messageMng.AddClientMsg(380);
					return ;
				}
				GameCenter.mailBoxMng.C2S_DeleteMail(idList);
				gameObject.SetActive(false);
			};
		}
		if(mReply != null){
			mReply.gameObject.SetActive(data.type != 2);
			UIEventListener.Get(mReply.gameObject).onClick = delegate {
				MailWriteData mailWriteData = new MailWriteData(data.SendName);
				if(writeGame != null){
					writeGame.InWriteData(mailWriteData);
					gameObject.SetActive(false);
				}
			};
		}
		int i =0 ;
		for(;i<items.Count;i++){
			if(items[i] == null)continue;
			if(i < 4 && data.items.Count > i){
				items[i].FillInfo(data.items[i]);
				items[i].gameObject.SetActive(true);
				continue;
			}
			if(data.items.Count > i){
				items[i].FillInfo(data.items[i]);
				items[i].gameObject.SetActive(true);
			}else{
				items[i].gameObject.SetActive(false);
			}
		}
	}
	
	public void ShowInfo(MailData _data){
		InMailData = _data;
	}
	
	void OnDisable(){
		if(data != null)data.OnMailContentUpdate -= Show;
	}
	
}
