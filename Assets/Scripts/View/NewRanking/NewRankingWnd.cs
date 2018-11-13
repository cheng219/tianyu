//====================
//作者：鲁家旗
//日期：2016/4/19
//用途：排行榜主界面
//====================
using UnityEngine;
using System.Collections;

public class NewRankingWnd : GUIBase
{
    public UIButton closeBtn;
    public UIToggle rankTog;
    public UIToggle achieveTog;
    public UISprite rankTitle;
    public UISprite achievementTitle;
    void Awake()
    {
        mutualExclusion = true;
        Layer = GUIZLayer.NORMALWINDOW;
        allSubWndNeedInstantiate = true;

        if (closeBtn != null) UIEventListener.Get(closeBtn.gameObject).onClick = delegate
        {
            GameCenter.uIMng.SwitchToUI(GUIType.NONE);
        };
    }
    protected override void OnOpen()
    {
        base.OnOpen();
        if (rankTog != null) UIEventListener.Get(rankTog.gameObject).onClick = delegate
        {
            rankTitle.gameObject.SetActive(true);
            achievementTitle.gameObject.SetActive(false);
            SwitchToSubWnd(SubGUIType.NEWRANKINGSUBWND);
        };
        if (achieveTog != null) UIEventListener.Get(achieveTog.gameObject).onClick = delegate
        {
            rankTitle.gameObject.SetActive(false);
            achievementTitle.gameObject.SetActive(true);
            SwitchToSubWnd(SubGUIType.ACHIEVEMENT);
        };
    }
    protected override void OnClose()
    {
        base.OnClose();
    }
    protected override void InitSubWndState()
    {
        base.InitSubWndState();
        switch (initSubGUIType)
        { 
            case SubGUIType.NEWRANKINGSUBWND:
                rankTog.value = true;
                rankTitle.gameObject.SetActive(true);
                achievementTitle.gameObject.SetActive(false);
                break;
            case SubGUIType.ACHIEVEMENT:
                achieveTog.value = true;
                rankTitle.gameObject.SetActive(false);
                achievementTitle.gameObject.SetActive(true);
                break;
            default: break;
        }
    }
}