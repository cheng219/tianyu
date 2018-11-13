//==================================
//作者：邓成
//日期：2017/4/6
//用途：宝藏活动界面类
//=================================

using UnityEngine;
using System.Collections;

public class TreasureTroveWnd : GUIBase {
    public UIToggle[] toggles;
    public UIButton btnClose;
    public GameObject redPoint;
    void Awake()
    {
        layer = GUIZLayer.NORMALWINDOW;
        mutualExclusion = true;
    }
    protected override void OnOpen()
    {
        DefaultSelectToggle();
        DefaultSelectSubWnd();
        GameCenter.treasureTroveMng.C2S_ReqTreasurePreviewInfo();
        base.OnOpen();
        RefreshRedPoint();
    }
    protected override void OnClose()
    {
        base.OnClose();
    }
    protected override void HandEvent(bool _bind)
    {
        if (_bind)
        {
            GameCenter.treasureTroveMng.updateTreasurebigPrize += RefreshRedPoint;
            if (btnClose!=null)
            {
                UIEventListener.Get(btnClose.gameObject).onClick = CloseWnd;
            }
            for(int i=0,len=toggles.Length;i<len;i++)
            {
                if (toggles[i] != null)
                    UIEventListener.Get(toggles[i].gameObject).onClick += ClickToggle;
            }
        }
        else
        {
            GameCenter.treasureTroveMng.updateTreasurebigPrize -= RefreshRedPoint;
            for (int i = 0, len = toggles.Length; i < len; i++)
            {
                if (toggles[i] != null)
                    UIEventListener.Get(toggles[i].gameObject).onClick -= ClickToggle;
            }
        }
    }
    protected override void InitSubWndState()
    {
        base.InitSubWndState();
    }
    #region 界面的刷新与显示
    /// <summary>
    /// 刷新红点
    /// </summary>
    void RefreshRedPoint()
    {
        //Debug.Log("GameCenter.treasureTroveMng.RedPoint:"+ GameCenter.treasureTroveMng.RedPoint);
        if (redPoint != null)
            redPoint.gameObject.SetActive(GameCenter.treasureTroveMng.RedPoint);
    }
    #region 界面的默认初始化设置
    /// <summary>
    /// 默认打开的子窗口
    /// </summary>
    void DefaultSelectSubWnd()
    {
        if (InitSubGUIType == SubGUIType.NONE)
        {
            InitSubGUIType = SubGUIType.TREASURETROVEPREVIEW;
        }
    }
    /// <summary>
    /// 默认选中的Toggle
    /// </summary>
    void DefaultSelectToggle()
    {
        if (toggles[0]!=null)
        toggles[0].value = true;
    }
    #endregion
    #region 界面刷新
    /// <summary>
    /// 切换子界面
    /// </summary>
    void ChangeSubWnd(int toggleCount)
    {
        //Debug.Log("ChangeSubWnd(int toggleCount):" + toggleCount);
        switch (toggleCount)
        {
            case 0:
                SwitchToSubWnd(SubGUIType.TREASURETROVEPREVIEW);
                break;
            case 1:
                SwitchToSubWnd(SubGUIType.TREASURETROVEREWARD);
                break;
            case 2:
                SwitchToSubWnd(SubGUIType.TREASURETROVERANK);
                break;
            //default:
            //    SwitchToSubWnd(SubGUIType.TREASURETROVEPREVIEW);
            //    break;
        }
    }
    #endregion
    #endregion
    #region UI控件的响应
    /// <summary>
    /// Toggle的点击响应
    /// </summary>
    /// <param name="_obj"></param>
    void ClickToggle(GameObject _obj)
    {
        //Debug.Log("进入ClickToggle(GameObject _obj)");
        UIToggle toggle = _obj.GetComponent<UIToggle>();
        if (toggle == null)
        {
            //Debug.LogError("宝藏活动预制上的UIToggle组件丢失");
            return;
        }
        toggle.value = true;
        for (int i = 0, len = toggles.Length; i < len; i++)
            {
                if (toggles[i] != null)
                {
                    //Debug.LogWarning(toggles[i].name + "     " + toggles[i].value);
                    if (toggles[i].value)
                    {
                        //Debug.Log("toggles[i]:  " +i+","+ toggles[i].gameObject.name);
                        ChangeSubWnd(i);
                    }
                }
            }
    }
    /// <summary>
    /// 关闭窗口
    /// </summary>
    void CloseWnd(GameObject _obj)
    {
        //Debug.Log("关闭窗口");
        GameCenter.uIMng.ReleaseGUI(GUIType.TREASURETROVE);
    }
    /// <summary>
    /// 返回宝藏预览
    /// </summary>
    public void ReturnPreviewWnd()
    {
        SwitchToSubWnd(SubGUIType.TREASURETROVEPREVIEW);
    }
    #endregion
}
