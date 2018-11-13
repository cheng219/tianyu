//===========================
//作者:吴江
//日期：2015/11/13
//用途：组队系统
//
//
//修改：贺丰
//日期：2016/1/12
//
//=============================



using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TeamMemberWnd : GUIBase
{

    #region 控件引用
    /// <summary>
    /// 队员组件实例
    /// </summary>
    public TeamMemberSingle memberInstance;
    public UILabel curFightValue;
    public Transform memberParent;
    public UIButton closeBtn;
    public UIButton quitBtn;
    public UIButton settingBtn;
    public UIButton kickMemberBtn;

    protected List<TeamMemberSingle> memberList = new List<TeamMemberSingle>();
    /// <summary>
    /// 当前锁定的成员信息 by吴江 
    /// </summary>
    protected TeamMenberInfo curTargetInfo = null;
    #endregion


    #region UNITY
    void Awake()
    {
        mutualExclusion = true;
        needCloaseMainCamera = true;
        Layer = GUIZLayer.NORMALWINDOW;

        UIEventListener.Get(closeBtn.gameObject).onClick -= OnClickCloseBtn;
        UIEventListener.Get(quitBtn.gameObject).onClick -= OnClickQuitBtn;
        UIEventListener.Get(settingBtn.gameObject).onClick -= OnClickSettingBtn;
        UIEventListener.Get(kickMemberBtn.gameObject).onClick -= OnClickKickBtn;

        UIEventListener.Get(closeBtn.gameObject).onClick += OnClickCloseBtn;
        UIEventListener.Get(quitBtn.gameObject).onClick += OnClickQuitBtn;
        UIEventListener.Get(settingBtn.gameObject).onClick += OnClickSettingBtn;
        UIEventListener.Get(kickMemberBtn.gameObject).onClick += OnClickKickBtn;

        memberInstance.gameObject.SetActive(false);

    }


    protected override void OnOpen()
    {
        base.OnOpen();
        Refresh();
    }

    protected override void HandEvent(bool _bind)
    {
        base.HandEvent(_bind);
        if (_bind)
        {
            GameCenter.teamMng.onTeammateUpdateEvent += Refresh;
        }
        else
        {
            GameCenter.teamMng.onTeammateUpdateEvent -= Refresh;
        }
    }
    #endregion

    #region 控件事件
    protected void OnClickCloseBtn(GameObject _obj)
    {
        GameCenter.uIMng.SwitchToUI(GUIType.NONE);
    }

    protected void OnClickQuitBtn(GameObject _obj)
    {
        GameCenter.messageMng.AddClientMsg(77,
                delegate
                {
                    GameCenter.teamMng.C2S_TeamOut();
                    GameCenter.uIMng.SwitchToUI(GUIType.NONE);
                },
                delegate
                {

                });
    }


    protected void OnClickMember(GameObject _obj)
    {
        TeamMemberSingle m = _obj.GetComponent<TeamMemberSingle>();
        UIToggle t = _obj.GetComponent<UIToggle>();
        if (m != null)
        {
            curTargetInfo = m.GetInfo();
            if (t!=null)
                kickMemberBtn.gameObject.SetActive(GameCenter.teamMng.isLeader && curTargetInfo != null && t.value == true);
        }
    }

    protected void OnClickSettingBtn(GameObject _obj)
    {
        SwitchToSubWnd(SubGUIType.TEAMSETTING);
    }

    protected void OnClickKickBtn(GameObject _obj)
    {
        if (curTargetInfo != null)
        {
            GameCenter.teamMng.C2S_TeamForceOut((int)curTargetInfo.baseInfo.uid);
        }
    }
    #endregion

    #region 刷新
    protected void Refresh()
    {
        //数据
        int curID = 0;
        if (curTargetInfo != null)
            curID = (int)curTargetInfo.baseInfo.uid;
        FDictionary memberDic = GameCenter.teamMng.TeammatesDic;
        if (!memberDic.ContainsKey(curID))
            curTargetInfo = null;
        else
            curTargetInfo = (TeamMenberInfo)memberDic[curID];
        //实例化出的小部件
        for (int i = 0; i < memberList.Count; i++)
        {
            memberList[i].gameObject.SetActive(false);
            UIToggle tog = memberList[i].GetComponent<UIToggle>();
            if (tog != null)
                tog.value = false;
        }
        int index = 0;
        int fightvalue = 0;
        foreach (TeamMenberInfo item in memberDic.Values)
        {
            if (memberList.Count < index +1)
            {
                memberList.Add(memberInstance.CreateNew(memberParent,index));
                UIEventListener.Get(memberList[index].gameObject).onClick -= OnClickMember;
                UIEventListener.Get(memberList[index].gameObject).onClick += OnClickMember;
            }
            memberList[index].gameObject.SetActive(true);
            memberList[index].SetInfo(item);


            UIToggle tog = memberList[index].GetComponent<UIToggle>();
            if (curTargetInfo != null && curTargetInfo.baseInfo.uid == item.baseInfo.uid)
            {
                if (tog != null)
                    tog.value = true;
            }

            fightvalue += (int)item.baseInfo.fighting;
            index++;
        }
        //页面其他显示
        if (curFightValue != null)
            curFightValue.text = fightvalue.ToString();
        kickMemberBtn.gameObject.SetActive(GameCenter.teamMng.isLeader && curTargetInfo != null);
        settingBtn.gameObject.SetActive(GameCenter.teamMng.isLeader);
    }
    #endregion
}
