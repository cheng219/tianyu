//======================================================
//作者:黄洪兴
//日期:2016/07/21
//用途:聊天UI中的多种跳转功能插件
//======================================================
using UnityEngine;
using System.Collections;

public class ChatFunction : MonoBehaviour {
    public GameObject headBtn;
    public UILabel labText;
    ChatInfo chatinfo=null;
	// Use this for initialization
	void OnEnable () {
        UIEventListener.Get(gameObject).onClick += ClickLabelBack;
        if (headBtn != null) UIEventListener.Get(headBtn).onClick += ClickHeadBtn;
	}
	

   bool  willCloseChatWnd=false;
    
	// Update is called once per frame
	void Update () {

        if (willCloseChatWnd)
        {
            willCloseChatWnd = false;
            GameCenter.uIMng.SwitchToUI(GUIType.NONE);
        }
	}

    void OnDisable()
    {
        UIEventListener.Get(headBtn).onClick -= ClickHeadBtn;
        if (headBtn != null) UIEventListener.Get(gameObject).onClick -= ClickLabelBack;
    }
    public void init(ChatInfo _info)
    {
        chatinfo = _info;
    }
    void ClickHeadBtn(GameObject go)
    { 
        if (chatinfo == null)
        { 
            return;
        }
        Vector3 point = GameCenter.cameraMng.uiCamera.ScreenToWorldPoint(Input.mousePosition);
        if (GameCenter.chatMng.OnShowInformation != null)
            GameCenter.chatMng.OnShowInformation(chatinfo, point);
    }
    void ClickLabelBack(GameObject go)
    {
        if (chatinfo == null)
            return;
        if (labText == null)
            return;
        string url = labText.GetUrlAtPosition(UICamera.lastWorldPosition);
        if (!string.IsNullOrEmpty(url))
        {
            string[] urlStr = url.Split('|');
            if (urlStr.Length == 2)
            {
                if (urlStr[0].Equals("1"))
                {
                     if (urlStr[1].Equals("0"))
                    {
                        if (chatinfo.playerID.Count > 0)
                        {
                           GameCenter.previewManager.C2S_AskOPCPreview(chatinfo.playerID[0]);
                        }
                    }
                    if (urlStr[1].Equals("1"))
                    {
                        if (chatinfo.playerID.Count > 1)
                        {
                            GameCenter.previewManager.C2S_AskOPCPreview(chatinfo.playerID[1]);
                        }
                    }                 
                }
                if (urlStr[0].Equals("2"))
                {
                    if (urlStr[1].Equals("0"))
                    {
                        if (chatinfo.equipmentRefList.Count > 0)
                        {
                            if (chatinfo.equipmentRefList[0] != null)
                                ToolTipMng.ShowEquipmentTooltip(chatinfo.equipmentRefList[0], ItemActionType.None, ItemActionType.None, ItemActionType.None, ItemActionType.None, this.gameObject);
                        }
                    }
                    if (urlStr[1].Equals("1"))
                    {
                        if (chatinfo.equipmentRefList.Count > 1)
                        {
                            if (chatinfo.equipmentRefList[1] != null)
                                ToolTipMng.ShowEquipmentTooltip(chatinfo.equipmentRefList[1], ItemActionType.None, ItemActionType.None, ItemActionType.None, ItemActionType.None, this.gameObject);
                        }
                    }
                    
                }
                if (urlStr[0].Equals("3"))
                {
                    if (urlStr[1].Equals("0"))
                    {
                        if (chatinfo.equipmentList.Count > 0)
                        {
                            if (chatinfo.equipmentList[0] != null)
                                ToolTipMng.ShowEquipmentTooltip(chatinfo.equipmentList[0], ItemActionType.None, ItemActionType.None, ItemActionType.None, ItemActionType.None, this.gameObject);
                        }
                    }
                    if (urlStr[1].Equals("1"))
                    {
                        if (chatinfo.equipmentList.Count > 1)
                        {
                            if (chatinfo.equipmentList[1] != null)
                                ToolTipMng.ShowEquipmentTooltip(chatinfo.equipmentList[1], ItemActionType.None, ItemActionType.None, ItemActionType.None, ItemActionType.None, this.gameObject);
                        }
                    }

                }
                if (urlStr[0].Equals("4"))
                {
                    if (chatinfo.sceneID != 0 && chatinfo.point != Vector3.zero)
                    {
                        GameCenter.curMainPlayer.GoTraceTarget(chatinfo.sceneID, (int)chatinfo.point.x, (int)chatinfo.point.z);
                        willCloseChatWnd = true;
                    }
                }
                if (urlStr[0].Equals("5"))
                {
                    if (chatinfo.EquipInfo != null)
                        ToolTipMng.ShowEquipmentTooltip(chatinfo.EquipInfo, ItemActionType.None, ItemActionType.None, ItemActionType.None, ItemActionType.None, this.gameObject);
                    else
                    {
                        Debug.LogError("查看的物品数据为空 by黄洪兴");
                    }
                }
                if (urlStr[0].Equals("6"))
                {
                    if (chatinfo != null)
                    {
                        Vector3 point = GameCenter.cameraMng.uiCamera.ScreenToWorldPoint(Input.mousePosition);
                        if (GameCenter.chatMng.OnShowInformation != null)
                            GameCenter.chatMng.OnShowInformation(chatinfo, point);
                    }
                }
                if (urlStr[0].Equals("7"))
                {
                    if (chatinfo != null)
                    {
                        int oth_id = GameCenter.mainPlayerMng.MainPlayerInfo.ServerInstanceID;
                        if (!GameCenter.teamMng.isInTeam)
                        {
                            if (!GameCenter.duplicateMng.CopyTeams.ContainsKey(oth_id) && GameCenter.duplicateMng.CopyTeams.Count < 3)
                            {
                                if (chatinfo.playerID.Count > 0)
                                {
                                    GameCenter.duplicateMng.C2S_CopyInFriendReturn(chatinfo.playerID[0], 2);
                                    GameCenter.duplicateMng.isMagicTowrAddFri = true;
                                }
                            }
                            else
                                GameCenter.messageMng.AddClientMsg(84);
                        }
                        else
                            GameCenter.messageMng.AddClientMsg(423);
                    }
                }

            }
            else
            {
                Debug.Log("数据错误!" + "url:" + url);
            }
        }
    }
}
