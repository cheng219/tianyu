//==================================
//作者：黄洪兴
//日期：2016/6/20
//用途：新技能获得提示界面类
//=================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewSkillMsgWnd : GUIBase
{

    public GameObject closeBtn;
    public UISprite skillSprite;
    public UILabel skillName;
    public UILabel skillDes;

    SkillInfo curInfo;

    //private Dictionary<int, RechargeRef> rechargeItemRefDic = new Dictionary<int, RechargeRef>();
    //MainPlayerInfo mainPlayerInfo = null;

    void Awake()
    {
        mutualExclusion = false;
    }
    protected override void OnOpen()
    {
        base.OnOpen();
        GameCenter.taskMng.OutsideInterruptAutoTask = true;
        Refresh();

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
            if (closeBtn != null)
                UIEventListener.Get(closeBtn).onClick += CloseThisUI;

        }
        else
        {
            if (closeBtn != null)
                UIEventListener.Get(closeBtn).onClick -= CloseThisUI;
        }
    }



    void Refresh()
    {
        if (GameCenter.skillMng.NewSkillList.Count < 1)
        {
            return;
        }
        if (skillSprite != null)
        {
            skillSprite.spriteName = GameCenter.skillMng.NewSkillList[0].SkillIcon;
        }
        if (skillName != null)
        {
            skillName.text = GameCenter.skillMng.NewSkillList[0].SkillName;
        }
        if (skillDes != null)
        {
            SkillMainConfigRef Ref = ConfigMng.Instance.GetSkillMainConfigRef(GameCenter.skillMng.NewSkillList[0].SkillID);
            if (Ref != null)
            {
                skillDes.text = Ref.skillRes.Replace("\\n","\n");
            }
        }






    }




    void CloseThisUI(GameObject _go)
    {
        curInfo = GameCenter.skillMng.NewSkillList[0];
        GameCenter.skillMng.NewSkillList.Clear();
        //GameCenter.skillMng.NewSkillList.Remove(GameCenter.skillMng.NewSkillList[0]);
        //if (GameCenter.skillMng.NewSkillList.Count > 0)
        //{
        //    GameCenter.uIMng.ReleaseGUI(GUIType.NEWSKILL);
        //    GameCenter.uIMng.GenGUI(GUIType.NEWSKILL,true);
        //    return;
        //}
        GameCenter.uIMng.ReleaseGUI(GUIType.NEWSKILL);
        if (GameCenter.skillMng.OnPlayNewSkillGetAnimation!=null)
        GameCenter.skillMng.OnPlayNewSkillGetAnimation(curInfo.SkillID);
    }


}
