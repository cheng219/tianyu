/// <summary>
/// 何明军
/// 2016/4/19
/// 邮件系统
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class XXMailWnd : SubWnd {
	
	public List<UIToggle> uiTog;
	
	public XXMailItemUI etyItems;
	public XXMailPageUI etyPages;
	
	public XXMailItemUI itemContent;
	public XXMailWriteUI itemWrite;
	public GameObject noThing;
	
	public UIButton mAllDel;
	public UIButton mAllIsReadDel;
	public UIButton mAllExtract;
	public UIButton toMailWrite;
	
	public GameObject mItemPanel;
	public UIScrollView scrollView;
	
	public GameObject btnToFriend;
	public InvitationPlayerWnd toFriend;
	int page = 1;
	int pageTotal = 1;
	int pageInNum = 4;
	float etyItemWidth = 745;
	float etyItemHeiht = -88;
	
	Dictionary<int,XXMailItemUI> listMail = new Dictionary<int,XXMailItemUI>();
	
	Dictionary<int,XXMailPageUI> listPage = new Dictionary<int,XXMailPageUI>();
	int uiTogType = 0;
	
	UIGrid pageGrid;
	Vector2 loPosition = Vector2.zero;
	Vector3 pos = Vector3.zero;
	SpringPanel sp;
	MailWriteData write = new MailWriteData(string.Empty);
	
	void InIt(){
		for(int i=0,len=uiTog.Count;i<len;i++){
			if(uiTog[i] != null){
				UIEventListener.Get(uiTog[i].gameObject).onClick = ClickToggleEvent;
				//EventDelegate.Add(uiTog[i].onChange,UITogOnChange);
			}
		}

		if(toMailWrite != null)UIEventListener.Get(toMailWrite.gameObject).onClick = delegate {
			itemWrite.InWriteData(write);
		};
		if(mAllDel != null)UIEventListener.Get(mAllDel.gameObject).onClick = delegate {
			List<uint> idList = new List<uint>();
			Dictionary<int,MailData> list = GameCenter.mailBoxMng.mailDic;
			foreach(MailData _data in list.Values){
				if(_data.items.Count <= 0)idList.Add((uint)_data.id);
			}
			GameCenter.mailBoxMng.C2S_DeleteMail(idList);
		};
		if(mAllIsReadDel != null)UIEventListener.Get(mAllIsReadDel.gameObject).onClick = delegate {
			List<uint> idList = new List<uint>();
			Dictionary<int,MailData> list = GameCenter.mailBoxMng.MailDic(XXMailMng.MailState.IsRead);
			foreach(MailData _data in list.Values){
				if(_data.items.Count <= 0)idList.Add((uint)_data.id);
			}
			GameCenter.mailBoxMng.C2S_DeleteMail(idList);
		};
		if(mAllExtract != null)UIEventListener.Get(mAllExtract.gameObject).onClick = delegate {
			List<uint> idList = new List<uint>();
//			foreach(int id in GameCenter.mailBoxMng.mailDic.Keys){
//				if(GameCenter.mailBoxMng.mailDic[id].items.Count > 0)idList.Add((uint)id);
//			}
			GameCenter.mailBoxMng.C2S_ExtractMail(idList);
		};
		if(btnToFriend != null)UIEventListener.Get(btnToFriend).onClick = delegate {
			GameCenter.friendsMng.C2S_ReqFriendsList();
		};
		if(mItemPanel != null)UIEventListener.Get(mItemPanel).onPress = OnPressItemPanel;

		if(etyItems != null)etyItems.gameObject.SetActive(false);
		if(etyPages != null)etyPages.gameObject.SetActive(false);

		pageGrid = etyPages.transform.parent.GetComponent<UIGrid>();

		CreateMailItem();
	}

    protected UIToggle lastChangeToggle = null;
    protected void ClickToggleEvent(GameObject go)
    {
        UIToggle toggle = go.GetComponent<UIToggle>();
        if (toggle != lastChangeToggle)
        {
            UITogOnChange();
        }
        if (toggle != null && toggle.value) lastChangeToggle = toggle;
    }

	void InItPanel(){
		if(scrollView == null)return ;
		scrollView.transform.localPosition = Vector3.zero;
		sp = scrollView.GetComponent<SpringPanel>();
		if (sp == null) sp = scrollView.gameObject.AddComponent<SpringPanel>();
		SpringPanel.Begin(scrollView.gameObject,Vector3.zero,1);
		scrollView.GetComponent<UIPanel>().clipOffset = Vector2.zero;
		page = 1;
		if(listPage.Count >= page)listPage[page - 1].SetSelect(true);
	}
	
	void OnPressItemPanel(GameObject game, bool state){
		if(scrollView == null)return ;
		if(state){
			loPosition = UICamera.lastEventPosition;
		}else{
			if(loPosition != Vector2.zero){
				float x = UICamera.lastEventPosition.x - loPosition.x;
				if(Mathf.Abs(x) > 20f){
					if(x < 0){
						if(page < pageTotal)page++;
						pos = sp.target + new Vector3(-745,0,0);
						if(pos.x > GameCenter.mailBoxMng.TotalPage * -745)SpringPanel.Begin(scrollView.gameObject,pos,10f);
					}
					if(x > 0){
						if(page >1)page--;
						pos = sp.target + new Vector3(745,0,0);
						if(pos.x < 745)SpringPanel.Begin(scrollView.gameObject,pos,10f);
					}
					listPage[page - 1].SetSelect(true);
				}
			}
		}
	}
	
	void CreatePages(int num){
		foreach(XXMailPageUI ui in listPage.Values){
			ui.gameObject.SetActive(false);
		}
		
		pageTotal = num % pageInNum > 0 ? num/pageInNum + 1 : num/pageInNum;
		if(pageTotal <= 0){
			pageTotal = 1;
		}
		if(num == 0){
			pageTotal = 0;
		}
		
		pageGrid.maxPerLine = pageTotal;
		
		GameCenter.mailBoxMng.TotalPage = pageTotal;
		
		XXMailPageUI mailPageUI = null;
		for(int i=0;i<pageTotal;i++){
			if(!listPage.ContainsKey(i)){
				GameObject go = (GameObject)GameObject.Instantiate(etyPages.gameObject);
				go.transform.parent = etyPages.transform.parent;
				go.transform.localPosition = Vector3.zero;
				go.transform.localScale = Vector3.one;
				go.name = etyPages.gameObject.name + i;
				
				mailPageUI = go.GetComponent<XXMailPageUI>();
				if(mailPageUI != null){
					mailPageUI.InItSelect(i+1 == page);
					listPage[i] = mailPageUI;
				}
				go.SetActive(true);
				
			}else{
				listPage[i].transform.localPosition = Vector3.zero;
				listPage[i].gameObject.SetActive(true);
				listPage[i].SetSelect(i+1 == page);
			}
		}
		pageGrid.repositionNow = true;
	}
	
	void CreateMailItem(){
		InItPanel();
		
		XXMailItemUI mailItemUI = null;
		int i = 0;
		
		Dictionary<int,MailData> list =  GameCenter.mailBoxMng.MailDic((XXMailMng.MailState)uiTogType);
		
		CreatePages(list.Count);
		
		noThing.SetActive(list.Count <= 0);
		
		foreach(MailData data in list.Values){
			if(!listMail.ContainsKey(data.id)){
				GameObject go = (GameObject)GameObject.Instantiate(etyItems.gameObject);
				go.transform.parent = etyItems.transform.parent;
				go.transform.localPosition = new Vector3(i/4 * etyItemWidth,i%4*etyItemHeiht,etyItems.transform.localPosition.z);
				go.transform.localScale = Vector3.one;
	
				mailItemUI = go.GetComponent<XXMailItemUI>();
	
				if(mailItemUI != null){
					mailItemUI.InMailData = data;
					listMail[data.id] = mailItemUI;
				}
				go.SetActive(true);
				
				UIEventListener.Get(go).onClick = OnClickMailItem;
				UIEventListener.Get(go).onPress = OnPressItemPanel;
			}else{
				listMail[data.id].transform.localPosition = new Vector3(i/4 * etyItemWidth,i%4*etyItemHeiht,etyItems.transform.localPosition.z);
				listMail[data.id].InMailData = data;
				listMail[data.id].gameObject.SetActive(true);
			}
			i++;
		}
	}
	
	protected override void OnOpen ()
	{
		base.OnOpen ();
		InIt();
		GameCenter.mailBoxMng.OnMailListUpdate += OnMailListUpdate;
		GameCenter.mailBoxMng.OnMailWriteDataUpdate += OnMailWriteDataUpdate;
		GameCenter.friendsMng.OnFriendsDicUpdata += OpenFriend;
		if(GameCenter.mailBoxMng.mailWriteData != null){
			itemWrite.InWriteData(GameCenter.mailBoxMng.mailWriteData);
			GameCenter.mailBoxMng.mailWriteData = null;
		}
	}
	
	protected override void OnClose ()
	{
		base.OnClose ();
		GameCenter.mailBoxMng.OnMailListUpdate -= OnMailListUpdate;
		GameCenter.mailBoxMng.OnMailWriteDataUpdate -= OnMailWriteDataUpdate;
		GameCenter.friendsMng.OnFriendsDicUpdata -= OpenFriend;
	}
	
	void OpenFriend(){
		if(toFriend == null)return ;
		toFriend.ListPlayer = GameCenter.duplicateMng.GetOtherPlayerInfoS();
		toFriend.gameObject.SetActive(true);
	}
	
	void OnMailWriteDataUpdate(){
		if(GameCenter.mailBoxMng.mailWriteData != null){
			itemWrite.InWriteData(GameCenter.mailBoxMng.mailWriteData);
			GameCenter.mailBoxMng.mailWriteData = null;
			if(toFriend != null)toFriend.gameObject.SetActive(false);
		}
	}
	
	void OnDisable(){
		GameCenter.mailBoxMng.OnMailListUpdate -= OnMailListUpdate;
	}
	
    //void OnDestroy(){
    //    for(int i=0,len=uiTog.Count;i<len;i++){
    //        if(uiTog[i] != null){
    //            EventDelegate.Remove(uiTog[i].onChange,UITogOnChange);
    //        }
    //    }
    //}
	
	void UITogOnChange(){
		for(int i=0,len=uiTog.Count;i<len;i++){
			if(uiTog[i] != null && uiTog[i].value){
				uiTogType = i;
				break;
			}
		}
		foreach(XXMailItemUI ui in listMail.Values){
			ui.gameObject.SetActive(false);
		}
		CreateMailItem();
	}
	void OnClickMailItem(GameObject games){
		XXMailItemUI mailItem = games.GetComponent<XXMailItemUI>();
		if(mailItem != null){
			itemContent.InMailData = mailItem.InMailData;
			if(mailItem.InMailData.IsRead || !mailItem.InMailData.IsNewContent)GameCenter.mailBoxMng.C2S_ReadMail(mailItem.InMailData.id);
			itemContent.gameObject.SetActive(true);
		}
	}
	void OnMailListUpdate(){
		foreach(XXMailItemUI ui in listMail.Values){
			ui.gameObject.SetActive(false);
		}
		CreateMailItem();
	}
}
