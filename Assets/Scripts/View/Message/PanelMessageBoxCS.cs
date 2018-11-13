//==================================
//作者：易睿
//日期：2015/12/2
//用途：错误提示界面类
//=================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PanelMessageBoxCS : MonoBehaviour 
{
	public UILabel messTitle;
	public UILabel messLab;
	public UIButton oKBt;
	public UIButton canelBt;
	public UIButton closeBt;
	//跳转其他功能提示
	public UILabel labTips;
	public UIButton[] btnOpenWnds = new UIButton[buttonType.Length];
	public GameObject[] tipGos = new GameObject[buttonType.Length];
	public static string[] buttonType = new string[]{"ATIVITY"};
	
	////public UICheckbox check;
    public UIToggle toggle;

	private MessageST selMess;
	ErrorMsgStr serverMsg;
	private bool messageNet = false;//标记该次提示是否已结束

    void Update()
    {
        if (!messageNet && gameObject.activeSelf)
        {
            gameObject.SetActive(false);
            Close();
        }
    }
	void Init(){
        if (toggle != null)
        {
            toggle.gameObject.SetActive(false);
            toggle.value = false;
        }
        ////check.gameObject.SetActive(false);
        ////check.isChecked = false;
	}

	//显示客户端信息
	public void showClientMess()
	{
        if (GameCenter.messageMng.ClientMsgList.Count == 0)
		{
			selMess = null;
			return;
		}
		if(messageNet){
			return;
		}
		
		Init();

        MessageST messSt = GameCenter.messageMng.ClientMsgList[GameCenter.messageMng.ClientMsgList.Count - 1];
        //if (selMess != null && selMess.Equals(messSt))
        //{
        //    GameCenter.messageMng.ClientMsgList.Remove(messSt);
        //    Close();
        //    return;
        //}
        //else
        //{
        //    selMess = messSt;
        //}
        selMess = messSt;
//		GameCenter.messageMng.thisMessageST = messSt;
        string str = messSt.MessageString;
        int type = messSt.MessgeType;
       
//		messLab.text = str;
		messLab.text = str.Replace("\\n","\n");
		if(type!=2){
            messTitle.text = messSt.MessageTitle;
		}
		oKBt.gameObject.SetActive((type&8)==8);
		closeBt.gameObject.SetActive((type&4)==4);
		canelBt.gameObject.SetActive((type&8)==8);
		
		CheckParsShow(selMess.pars);
		
		if(!messSt.isShowColse){
			canelBt.gameObject.SetActive(false);
			oKBt.gameObject.transform.localPosition=Vector3.zero;
		}else{
            oKBt.gameObject.transform.localPosition = new Vector3(171.5f, -56.1f, 0f);
		}

		if(labTips != null)
		{
			labTips.text = messSt.ButtonTips;
		}
		ShowQuickButton(messSt.OpenWndType);

		gameObject.SetActive(true);
		
		messageNet = true;
        GameCenter.messageMng.ClientMsgList.Remove(messSt);
	}

	void ShowQuickButton(GUIType type)
	{
		string temp = type.ToString();
		int index = -1;
		for(int i=0,count=buttonType.Length;i<count;i++)
		{
			if(buttonType[i] == temp)
			{
				index = i;
				tipGos[i].SetActive(true);
			}else
				tipGos[i].SetActive(false);
		}
		if(index >= 0 && buttonType.Length > index)
		{
			UIEventListener.Get(btnOpenWnds[index].gameObject).onClick = (x)=>
			{
                GameCenter.uIMng.SwitchToUI(type);
				this.CloseUI();
			};
		}
	}
	void CheckParsShow(object[] pars){
		if(pars == null){
			return ;
		}
		int len = pars.Length;
		for(int i = 0;i<len;i++ ){
			CheckPars(pars[i]);
		}
	}
	
	void CheckPars(object pars){
		
		int parsType = (int)pars;
		switch(parsType){
			case 1:
				//check.gameObject.SetActive(true); 
                toggle.gameObject.SetActive(true);
				break;
               
		}
	}
	
	object[] GetPars(object[] pars){
		if(pars == null){
			return null;
		}
		int len = pars.Length;
		object[] objPars = new object[len];
		for(int i = 0;i<len;i++ ){
			if((int)pars[i] == 1){
				//objPars[i] = check.isChecked;
                objPars[i] = toggle.value;
			}
		}
		return objPars;
	}

	//显示服务器信息
	public void showServerMess()
	{
        if (GameCenter.messageMng.ServerMsgList.Count == 0)
		{
			serverMsg = null;
			return;
		}
		
		if(messageNet){
			return;
		}
		
		Init();

        ErrorMsgStr msg = GameCenter.messageMng.ServerMsgList[GameCenter.messageMng.ServerMsgList.Count - 1];
		
		if(serverMsg != null && serverMsg.Equals(msg))
		{
            GameCenter.messageMng.ServerMsgList.Remove(msg);
//			serverMsg = null;
			Close();
			return;
		}else{
			serverMsg = msg;
		}
		
        string str = UIUtil.Str2Str(msg.MessStr, msg.msgs);
        messLab.text = str;
        messTitle.text = ConfigMng.Instance.GetUItext(329);//服务端提示标题bug!!!!!!!!!!!!!!!!!!
		oKBt.gameObject.SetActive(false);
		closeBt.gameObject.SetActive(true);
		canelBt.gameObject.SetActive(false);		
		gameObject.SetActive(true);
		
        messageNet = true;
        GameCenter.messageMng.ServerMsgList.Remove(msg);
		if(tipGos != null)
		{
			for(int i=0,count=tipGos.Length;i<count;i++)
			{
				if(tipGos[i] != null)
					tipGos[i].SetActive(false);
				if(labTips != null)
					labTips.text = string.Empty;
			}
		}
	}

	public void OnClicOK()
	{
		//确定按钮点击了
//		GameCenter.messageMng.thisMessageST = null;
        if (selMess!=null&&selMess.delYes != null)
		{
			selMess.delYes();
			if(selMess.delPars != null){
				selMess.delPars(GetPars(selMess.pars));
			}
		}
		this.CloseUI();
	}

	public void CloseUI()
	{
        messageNet = false;
        //gameObject.SetActive(false);
        //Close();
	}
	
//	void Update(){
//		if(!messageNet && gameObject.activeSelf){
//			gameObject.SetActive(false);
//			Close();
//		}
//	}

	void Close(){
        if (GameCenter.messageMng.ServerMsgList.Count > 0)
		{
			this.showServerMess();
        }
        else if (GameCenter.messageMng.ClientMsgList.Count > 0)
		{
			this.showClientMess();
		}else{
			selMess = null;
			serverMsg = null;
		}
	}
	public void OnClickCancel()
	{
//		GameCenter.messageMng.thisMessageST = null;
		if(selMess != null && selMess.delNo != null)
		{
			selMess.delNo();
			if(selMess.delPars != null){
				selMess.delPars(GetPars(selMess.pars));
			}
		}
		this.CloseUI();
	}
}
