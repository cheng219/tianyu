//==================================
//作者：朱素云
//日期：2016/4/20
//用途：仙友操作ui
//=================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OperateUi : MonoBehaviour
{
    #region 操作界面数据
    /// <summary>
    /// 发送消息
    /// </summary>
    public Transform sendMegBtn;
    /// <summary>
    /// 组队
    /// </summary>
    public Transform teamBtn;
    /// <summary>
    /// 删除
    /// </summary>
    public Transform deleteBtn;
    /// <summary>
    /// 发送邮件
    /// </summary>
    public Transform sendMailBtn;
    /// <summary>
    /// 查看资料
    /// </summary>
    public Transform checkDataBtn;
    /// <summary>
    /// 加入黑名单
    /// </summary>
    public Transform addToBlackBtn;
    /// <summary>
    /// 移出黑名单
    /// </summary>
    public Transform moveFromBlackBtn;
    /// <summary>
    /// 去她身边
    /// </summary>
    public Transform gotoHerSideBtn;
    /// <summary>
    /// 加为好友
    /// </summary>
    public Transform addToFriendBtn;

    public bool isTransfer = false;//是否传送去他身边
    #endregion
     
    public  void show()
    {
        UIEventListener.Get(gameObject).onClick = delegate {this.gameObject.SetActive(false); };
         

        if (GameCenter.friendsMng.curFriend != null)
        { 
            int id = GameCenter.friendsMng.curFriend.configId;
            if (sendMegBtn != null) UIEventListener.Get(sendMegBtn.gameObject).onClick = delegate
            { 
                this.gameObject.SetActive(false); 
                GameCenter.chatMng.OpenPrivateWnd(GameCenter.friendsMng.curFriend.name);
            };
            if (teamBtn != null) UIEventListener.Get(teamBtn.gameObject).onClick = delegate
            { 
                this.gameObject.SetActive(false);
                if (GameCenter.friendsMng.curFriend != null)
                {
                    GameCenter.teamMng.C2S_TeamInvite(id);
                }
            };
            if (deleteBtn != null) UIEventListener.Get(deleteBtn.gameObject).onClick = delegate
            {
                this.gameObject.SetActive(false);
                if (GameCenter.friendsMng.curFriend != null) GameCenter.friendsMng.C2S_DeleteFriend(id);
                GameCenter.friendsMng.OnFriendsDicUpdata();
            };
            if (sendMailBtn != null) UIEventListener.Get(sendMailBtn.gameObject).onClick = delegate
            {
                this.gameObject.SetActive(false);
                GameCenter.mailBoxMng.mailWriteData = new MailWriteData(GameCenter.friendsMng.curFriend.name);
                GameCenter.uIMng.SwitchToSubUI(SubGUIType.BMail);
            };
            if (checkDataBtn != null) UIEventListener.Get(checkDataBtn.gameObject).onClick = delegate
            { 
                this.gameObject.SetActive(false);
                GameCenter.previewManager.C2S_AskOPCPreview(id);
            };
            if (addToBlackBtn != null) UIEventListener.Get(addToBlackBtn.gameObject).onClick = delegate
            {
                this.gameObject.SetActive(false);
                GameCenter.friendsMng.C2S_AddFriendToBlack(id);
            };
            if (moveFromBlackBtn != null) UIEventListener.Get(moveFromBlackBtn.gameObject).onClick = delegate
            {
                this.gameObject.SetActive(false);
                GameCenter.friendsMng.C2S_ReqDoSomething(FriendOperation.ADDFRIEND, id);
            };
            if (gotoHerSideBtn != null) UIEventListener.Get(gotoHerSideBtn.gameObject).onClick = delegate
            {
                this.gameObject.SetActive(false);
                if (GameCenter.friendsMng.curFriend.IsOnline)
                {
                    if (!isTransfer)
                    {
                        GameCenter.curMainPlayer.commandMng.CancelCommands();
                        OtherPlayer other = GameCenter.curGameStage.GetOtherPlayer(id);
                        if (other != null)
                        {
                            Command_TraceTarget trace = new Command_TraceTarget();
                            trace.target = other;
                            trace.minDistance = 1f;
                            GameCenter.curMainPlayer.commandMng.PushCommand(trace);
                            GameCenter.curMainPlayer.CurTarget = other;
                            GameCenter.uIMng.SwitchToUI(GUIType.NONE);
                        }
                        else
                        {
                            Debug.LogError("GetOtherPlayer:" + id + " is null!");
                        }
                    }
                    else
                    {
                        MessageST mst = new MessageST();
                        mst.messID = 398;
                        mst.delYes = delegate
                        {
                            GameCenter.friendsMng.C2S_ReqDoSomething(FriendOperation.DELETEFRIEND, id);
                            GameCenter.uIMng.SwitchToUI(GUIType.NONE);
                        };
                        GameCenter.messageMng.AddClientMsg(mst); 
                    }
                }
                else
                    GameCenter.messageMng.AddClientMsg(68);

            };
            if (addToFriendBtn != null) UIEventListener.Get(addToFriendBtn.gameObject).onClick = delegate
            {
                this.gameObject.SetActive(false);
                if (isTransfer && GameCenter.friendsMng.findFriend != null)
                {
                    id = GameCenter.friendsMng.findFriend.configId;
                    GameCenter.friendsMng.C2S_AddFriend(id);
                }
                else
                    GameCenter.friendsMng.C2S_AddFriend(id);
            };
        }
    }
 
}
