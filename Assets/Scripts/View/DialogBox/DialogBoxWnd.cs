//======================================================
//作者:朱素云
//日期:2017/3/15
//用途:对话框小盒子
//======================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogBoxWnd : GUIBase 
{
    protected DialogueRef dialogueRef = null;
    public GameObject rGo;
    public UILabel rlabDes;
    public UISprite rspIcon;
    public UILabel rNameLab;

    protected float timer = 0;
    protected int openTime = 0;
    protected int curDialogNum = 0;
    protected bool isCloseWnd = false;
    
    public GameObject lGo;
    public UILabel llabDes;
    public UISprite lspIcon;
    public UILabel lNameLab;

    protected List<DialogueRef> dialogueRefList
    {
        get
        {
            return GameCenter.rankRewardMng.dialogueRef;
        }
    }

    void Awake()
    {
        mutualExclusion = true;
        Layer = GUIZLayer.TOPWINDOW; 
    }

    void Update()
    {
        if (!isCloseWnd)
        {
            if (Time.time - timer > openTime)
            {
                OpenNextDialog();
            }
        }
    }

    protected override void OnOpen()
    {
        base.OnOpen(); 
        OpenNextDialog(); 
    }

    protected override void OnClose()
    { 
        base.OnClose(); 
    }

    void Show()
    { 
        if (dialogueRef != null)
        {
            if (dialogueRef.icon == "0")
            {
                if (lspIcon != null) lspIcon.spriteName = GameCenter.mainPlayerMng.MainPlayerInfo.HeadIconName;
                if (rspIcon != null) rspIcon.spriteName = GameCenter.mainPlayerMng.MainPlayerInfo.HeadIconName;
            }
            else
            { 
                if (rspIcon != null) rspIcon.spriteName = dialogueRef.icon;
                if (lspIcon != null) lspIcon.spriteName = dialogueRef.icon;
            }
            if (rlabDes != null) rlabDes.text = dialogueRef.text; 
            if (llabDes != null) llabDes.text = dialogueRef.text;
            if (rNameLab != null) rNameLab.text = dialogueRef.name;
            if (lNameLab != null) lNameLab.text = dialogueRef.name;
          
            TweenAlpha.Begin(this.gameObject,1,1);
        }
    } 

    void OpenNextDialog()
    {
        isCloseWnd = true;
        dialogueRef = null;
        for (int i = 0, max = dialogueRefList.Count; i < max; i++)
        {
            if (i == curDialogNum)
            {
                dialogueRef = dialogueRefList[i];  
                ++curDialogNum; 
                break;
            }
        }
        if (dialogueRef != null)
        { 
            rGo.SetActive(dialogueRef.iconPattern == "L");
            lGo.SetActive(dialogueRef.iconPattern == "R"); 
            isCloseWnd = false;
            timer = Time.time;
            TweenAlpha.Begin(this.gameObject, 1, 0);
            Show();
            openTime = dialogueRef.time;
        }
        else
        {
            GameCenter.uIMng.ReleaseGUI(GUIType.DIALOGBOX); 
        }
    } 
     
}
