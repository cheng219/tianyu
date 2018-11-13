//==================================
//作者：黄洪兴
//日期：2016/7/7
//用途：新获得称号提示界面
//=================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewTitleMsgWnd : GUIBase
{

    public UISprite titleSprite;
    public UILabel titleName;
    public GameObject closeBtn;
    public GameObject getTitleBtn;

    private TitleInfo curInfo;

    void Awake()
    {
        mutualExclusion = false;
    }
    protected override void OnOpen()
    {
        base.OnOpen();
        GameCenter.taskMng.OutsideInterruptAutoTask = true;
        Invoke("CloseThis", 5.5f);
        Refesh();
    }
    protected override void OnClose()
    {
        base.OnClose();
        GameCenter.taskMng.OutsideInterruptAutoTask = false;

    }
    protected override void HandEvent(bool _bind)
    {
        base.HandEvent(_bind);
        if (_bind)
        {
            if (closeBtn != null) UIEventListener.Get(closeBtn).onClick += CloseWnd;
            if (getTitleBtn != null) UIEventListener.Get(getTitleBtn).onClick += GetTitle;
        }
        else
        {
            if (closeBtn != null) UIEventListener.Get(closeBtn).onClick -= CloseWnd;
            if (getTitleBtn != null) UIEventListener.Get(getTitleBtn).onClick -= GetTitle;
        }
    }
   



    void Refesh()
    {
        
        if (GameCenter.titleMng.NewTitle != null)
        {
            curInfo=GameCenter.titleMng.NewTitle;
            if (titleSprite != null)
                titleSprite.spriteName = curInfo.IconName;
                titleSprite.MakePixelPerfect();
            if (titleName != null)
                titleName.text ="\""+ curInfo.NameDes+"\"";
        }
        


    }

    void CloseThis()
    {
        GameCenter.titleMng.NewTitle = null;
        GameCenter.uIMng.ReleaseGUI(GUIType.NEWTITLEMSG);
    }

    void CloseWnd(GameObject _go)
    {
        GameCenter.titleMng.NewTitle = null;
        GameCenter.uIMng.ReleaseGUI(GUIType.NEWTITLEMSG);
    }


    void GetTitle(GameObject _go)
    {
        GameCenter.titleMng.C2S_UseTitle(curInfo.ID,1);
        GameCenter.uIMng.ReleaseGUI(GUIType.NEWTITLEMSG);
        GameCenter.fashionMng.CurFashionWndType = FashionWndType.TITLE;
        GameCenter.uIMng.SwitchToSubUI(SubGUIType.SUBFASHION);
    }
          
          
       



    

    



    
  

   
   





}
