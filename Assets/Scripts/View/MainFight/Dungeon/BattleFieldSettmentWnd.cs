//======================================================
//作者:朱素云
//日期:2017/3/2
//用途:火焰山战场结算界面
//======================================================
using UnityEngine;
using System.Collections;

public class BattleFieldSettmentWnd:GUIBase
{
    /// <summary>
    /// 胜利
    /// </summary>
    public GameObject win;
    /// <summary>
    /// 平局
    /// </summary>
    public GameObject draw;
    /// <summary>
    /// 失败
    /// </summary>
    public GameObject fail;  
    public BattleFeildSettleUI battleFeildSettleUI;
      

    void Awake()
    {
        mutualExclusion = true;
        Layer = GUIZLayer.TOPWINDOW; 
        if (battleFeildSettleUI != null) battleFeildSettleUI.gameObject.SetActive(false);
    }

    protected override void OnOpen()
    {
        base.OnOpen();
        ShowSettlement(); 
    }

    protected override void OnClose()
    {
        base.OnClose(); 
    }

    void ShowSettlement()
    {
        SettlementData settlementData = GameCenter.battleFightMng.settlementData;
        if (settlementData != null)
        {
            if (win != null) win.SetActive(settlementData.fightState == 1);
            if (fail != null) fail.SetActive(settlementData.fightState == 0);
            if (draw != null) draw.SetActive(settlementData.fightState < 0); 
        }
        
        CancelInvoke("ShowSettleUi");
        Invoke("ShowSettleUi", 2.0f);
    }

    void ShowSettleUi()
    {
        if (win != null) win.SetActive(false);
        if (fail != null) fail.SetActive(false);
        if (draw != null) draw.SetActive(false); 
        if (battleFeildSettleUI != null) battleFeildSettleUI.gameObject.SetActive(true);
    }
}
