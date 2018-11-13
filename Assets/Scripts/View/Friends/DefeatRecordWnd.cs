//======================================================
//作者:朱素云
//日期:2017/4/13
//用途:战败记录界面
//======================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DefeatRecordWnd : GUIBase
{
    public UIButton closeBtn;
    public UILabel nameLab;//击杀我的人
    public UILabel itemLab;//我掉落的物品
    public UIButton goToHerBtn;//去他身边
    public UIButton checkInfoBtn;//查看资料


    void Awake()
    {
        if (closeBtn != null) UIEventListener.Get(closeBtn.gameObject).onClick = delegate {
            GameCenter.uIMng.SwitchToUI(GUIType.NONE); 
            GameCenter.uIMng.ReleaseGUI(GUIType.DEFEATRECORD); };
        if (goToHerBtn != null) UIEventListener.Get(goToHerBtn.gameObject).onClick =  OnClickGoTOHerBtn;
        if (checkInfoBtn != null) UIEventListener.Get(checkInfoBtn.gameObject).onClick = delegate {
            if (GameCenter.resurrectionMng.ResurrectionInfo != null)
            { 
                GameCenter.previewManager.C2S_AskOPCPreview(GameCenter.resurrectionMng.ResurrectionInfo.kill_uid);
            }
        };
    }
    protected override void OnOpen()
    {
        base.OnOpen();
        Show();
    }
    protected override void OnClose()
    {
        base.OnClose(); 
    }
     
    void Show()
    {
        if (GameCenter.resurrectionMng.ResurrectionInfo != null)
        {
            nameLab.text = GameCenter.resurrectionMng.ResurrectionInfo.kill_name;
        }
        if (itemLab != null)
        {
            if (GameCenter.resurrectionMng.ResurrectionInfo.drop_item != 0)
            {
                itemLab.gameObject.SetActive(true);
                EquipmentInfo info = new EquipmentInfo(GameCenter.resurrectionMng.ResurrectionInfo.drop_item, EquipmentBelongTo.PREVIEW);
                itemLab.text = ConfigMng.Instance.GetUItext(277)+info.ItemName; 
            }
            else
            {
                itemLab.gameObject.SetActive(false);
            }
        }
    }

    void OnClickGoTOHerBtn(GameObject go)
    {
        if (GameCenter.friendsMng.allFriendDic.ContainsKey(4) && GameCenter.resurrectionMng.ResurrectionInfo != null)
        {
            if (GameCenter.friendsMng.allFriendDic[4].ContainsKey(GameCenter.resurrectionMng.ResurrectionInfo.kill_uid))
            {
                MessageST mst = new MessageST();
                mst.messID = 398;
                mst.delYes = delegate
                {
                    GameCenter.friendsMng.C2S_ReqDoSomething(FriendOperation.DELETEFRIEND, GameCenter.resurrectionMng.ResurrectionInfo.kill_uid); 
                };
                GameCenter.messageMng.AddClientMsg(mst);
                GameCenter.uIMng.SwitchToUI(GUIType.NONE);
                GameCenter.uIMng.ReleaseGUI(GUIType.DEFEATRECORD);
            }
            else
                GameCenter.messageMng.AddClientMsg(68);
        }
    }
}
