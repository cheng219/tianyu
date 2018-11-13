//==================================
//作者：易睿
//日期：2015/12/2
//用途：错误提示界面类
//=================================
using UnityEngine;
using System.Collections;

public class MessageInstanceWnd : GUIBase {
	
	public UIFxAutoActive uiFx;
	
	public PanelMessageBoxCS panelMessage;
	
	public MessageDialogManager messageDialog;
	
	////public QuickBuyWnd quickBuyWnd;

	////public EnterCoppyWnd enterCoppyWnd; 

	void Awake(){
		mutualExclusion = false;
		Layer = GUIZLayer.TIP + 54000;
        this.transform.localPosition = Vector3.zero;
		
	}
	protected override void OnOpen ()
	{
		base.OnOpen ();
		TaskMng.OnTaskFinishEvent += OnTaskFinish;
	}
	protected override void OnClose ()
	{
		base.OnClose ();
		TaskMng.OnTaskFinishEvent -= OnTaskFinish;
	}

    protected override void HandEvent(bool _bind)
    {
        base.HandEvent(_bind);
        if (_bind)
        {
            GameCenter.messageMng.UpdateClientMsg += OnUpdateClientMsg;
            GameCenter.messageMng.UpdateErrorMsgStr += OnUpdateErrorMsgStr;
            GameCenter.messageMng.updateClientStrMsg += UpdateClientStrMsg;
            GameCenter.messageMng.ClearMsg += CannelPanelMessage;
        }
        else
        {
            GameCenter.messageMng.UpdateClientMsg -= OnUpdateClientMsg;
            GameCenter.messageMng.UpdateErrorMsgStr -= OnUpdateErrorMsgStr;
            GameCenter.messageMng.updateClientStrMsg -= UpdateClientStrMsg;
            GameCenter.messageMng.ClearMsg -= CannelPanelMessage;
        }
    }

    void CannelPanelMessage()
    {
        if (panelMessage != null) panelMessage.OnClickCancel();
    }

	void OnTaskFinish()
	{
		if(uiFx != null)uiFx.ShowFx();
	}

	void OnUpdateClientMsg(MessageST mess){
        if (mess.MessgeType != 2 && mess.MessgeType != 12 && mess.MessgeType != 11 && mess.MessgeType != 13 && mess.MessgeType != 14 && mess.MessgeType != 15)
        {
			panelMessage.showClientMess();
		}else{
            GameCenter.messageMng.ClientMsgList.Remove(mess);
			messageDialog.AddFloatTip(mess.RefData);
		}
		if(mess.messID == 8){
			Debug.Log("mess.messID == 8");
		}
	}
	
	void OnUpdateErrorMsgStr(ErrorMsgStr mess){
		if(mess.Sort != 2 && mess.Sort != 12 && mess.Sort != 11){
			panelMessage.showServerMess();
		}else{
            GameCenter.messageMng.ServerMsgList.Remove(mess);
			messageDialog.AddFloatTip(mess);
		}
	}
	
	void UpdateClientStrMsg(string _msg,bool showFrame)
	{
		messageDialog.AddFloatTip(_msg,showFrame);
	}

    //void OpenQuickBuyWnd(bool state)
    //{
    //    if(state)
    //        quickBuyWnd.OpenThisWnd();
    //    else
    //        quickBuyWnd.CloseThisWnd();
    //}
    //void OpenEnterCoppyWnd(int sceneId,int _from)
    //{
    //    enterCoppyWnd.OpenThisWnd();
    //    enterCoppyWnd.SetSceneName(sceneId,_from);
    //    panelMessage.CloseUI();
    //}
    //void CloseEnterCoppyWnd()
    //{
    //    enterCoppyWnd.CloseThisWnd();
    //}
	/// <summary>
	/// 按下Esc的时候,将窗口移动到最上层 
	/// </summary>
    //public void MoveForEsc()
    //{
    //    NewbieGuideWnd wnd = GameCenter.uIMng.GetGui<NewbieGuideWnd>();
    //    if(wnd != null)
    //        Layer = wnd.Layer + 5000;
    //    else
    //        Layer = GUIZLayer.TIP+40000;
    //}
	/// <summary>
	/// 还原窗口位置  
	/// </summary>
	public void RestoreForQuitEsc()
	{
		Layer = GUIZLayer.TIP + 600;
	}
}
