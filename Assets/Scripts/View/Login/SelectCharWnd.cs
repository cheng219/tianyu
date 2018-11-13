//==============================================
//作者：吴江
//日期：2015/5/26
//用途：选角窗口
//=================================================




using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// 登陆窗口 by吴江
/// </summary>
public class SelectCharWnd : GUIBase
{
    #region UI控件对象
    /// <summary>
    /// 选择角色的名字
    /// </summary>
    public UILabel selectPlayerName;
    /// <summary>
    /// 选择角色的等级
    /// </summary>
    public UILabel selectPlayerLv;
    /// <summary>
    /// 选择角色的显示
    /// </summary>
    public GameObject selectShow;
    /// <summary>
    /// 待选角色列表
    /// </summary>
    public SelectPlayerStruct[] selectPlayerStructArray = new SelectPlayerStruct[3];
    /// <summary>
    /// 选中按钮 by吴江
    /// </summary>
    public GameObject selectBtn;
    /// <summary>
    /// 取消按钮 by吴江
    /// </summary>
    public GameObject cancelBtn;
    /// <summary>
    /// 删除角色按钮 by 贺丰
    /// </summary>
    public GameObject deleteBtn;
    /// <summary>
    /// 当前选中的角色
    /// </summary>
    protected PlayerBaseInfo curPlayerBaseInfo = null;

    public PlayerBaseInfo CurPlayerBaseInfo
    {
        get
        {
            return curPlayerBaseInfo;
        }
        set
        {
            if (curPlayerBaseInfo != value)
            {
                curPlayerBaseInfo = value;
                RefreshName();
                CharacterSelectStage characterSelectStage = GameCenter.curGameStage as CharacterSelectStage;
                if (characterSelectStage != null)
                {
                    characterSelectStage.CurSelectRole = curPlayerBaseInfo;
                }
            }
        }
    }
    #endregion

    #region 数据
    /// <summary>
    /// 是否开始连接
    /// </summary>
    protected bool isStartConnect = false;
    /// <summary>
    /// 开始连接的时间
    /// </summary>
    protected float startConnectTime = 0;
    #endregion


    void Awake()
    {
        //非互斥窗口
        mutualExclusion = false;
        //基础层窗口
        Layer = GUIZLayer.BASE;
        if (selectBtn != null) UIEventListener.Get(selectBtn.gameObject).onClick = OnClickSelectBtn;
        if (cancelBtn != null) UIEventListener.Get(cancelBtn.gameObject).onClick = OnClickCancelBtn;
        if (deleteBtn != null) UIEventListener.Get(deleteBtn.gameObject).onClick = OnClickDeleteBtn;
        foreach (SelectPlayerStruct item in selectPlayerStructArray)
        {
            if (item != null)
            {
                UIEventListener.Get(item.gameObject).onClick = OnChoicePlayer;
            }
        }
        
    }


    protected override void OnOpen()
    {
        base.OnOpen();
        Refresh();

        int last = 0;
        for (int i = 1; i < selectPlayerStructArray.Length; i++)
        {
            if (selectPlayerStructArray[last].Info != null && selectPlayerStructArray[i].Info != null)
            {
                if (selectPlayerStructArray[last].LastLoginTime < selectPlayerStructArray[i].LastLoginTime)
                {
                    last = i;
                }
            }
        }
        SelectPlayerStruct s = selectPlayerStructArray[last].gameObject.GetComponent<SelectPlayerStruct>();
        UIToggle t = selectPlayerStructArray[last].gameObject.GetComponent<UIToggle>();
        if (s != null)
        {
            if (s.Info != null)
            {
                CurPlayerBaseInfo = s.Info;
                if (t != null)
                {
                    t.value = true;
                }
            }
        }
    }


    protected override void HandEvent(bool _bind)
    {
        base.HandEvent(_bind);
        if (_bind)
        {
        }
        else
        {
        }
    }


    #region 刷新
    protected void Refresh()
    {
        FDictionary loginPlayerDic = GameCenter.loginMng.LoginPlayerDic;
        int index = 0;
        foreach (PlayerBaseInfo item in loginPlayerDic.Values)
        {
            if (index >= selectPlayerStructArray.Length) break;
            selectPlayerStructArray[index].Refresh(item);
            index++;
        }
    }

    /// <summary>
    /// 刷新名字
    /// </summary>
    protected void RefreshName()
    {
        string add = "[b]";
        if (selectShow != null)
        {
            selectShow.SetActive(curPlayerBaseInfo != null);
        }
        if (curPlayerBaseInfo != null)
        {
            if (selectPlayerName != null)
                selectPlayerName.text = add + curPlayerBaseInfo.Name;
            if (selectPlayerLv != null)
                selectPlayerLv.text = add + curPlayerBaseInfo.Level.ToString();
        }
    }
    #endregion


    #region 控件事件
    /// <summary>
    /// 点击创建按钮的操作
    /// </summary>
    /// <param name="_btn"></param>
    protected void OnClickSelectBtn(GameObject _btn)
    {
        if (CurPlayerBaseInfo != null)
        {
            GameCenter.loginMng.C2S_AskQueue((uint)CurPlayerBaseInfo.ServerInstanceID);
        }
    }
    /// <summary>
    /// 点击删除按钮操作
    /// </summary>
    /// <param name="_btn"></param>
    protected void OnClickDeleteBtn(GameObject _btn)
    {
        if (CurPlayerBaseInfo != null)
        {
            
        }
    }
    /// <summary>
    /// 点击取消按钮的操作
    /// </summary>
    /// <param name="_btn"></param>
    protected void OnClickCancelBtn(GameObject _btn)
    {
        GameCenter.instance.GoPassWord();
    }

    protected void OnChoicePlayer(GameObject _btn)
    {
        SelectPlayerStruct s = _btn.GetComponent<SelectPlayerStruct>();
        if (s != null)
        {
            if (s.Info != null)
            {
                CurPlayerBaseInfo = s.Info;
            }
            else
            {
                GameCenter.instance.GoCreatChar();
            }
        }
    }
    #endregion




}
