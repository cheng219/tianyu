//======================================
//作者:邓成
//日期:2017/5/23
//用途:断线重新连接的窗口
//======================================

using UnityEngine;
using System.Collections;

public class AutoReconnectWnd : GUIBase
{
    public float ConnectedShowTime = 60f;

    public float oneTime = 2.7f;

    public float startAutoReconnectTime = 0f;

    public bool isGoPass = false;

    void Awake()
    {
        mutualExclusion = true;
        Layer = GUIZLayer.TIP + 200;
        //Debug.Log("Awake");
        if (GameCenter.instance.IsConnected)
        {
            NetMsgMng.ConectClose();
        }

    }
    /// <summary>
    /// 断线重连需要做的其他操作(true为重连，false为返回登录)
    /// </summary>
    void OtherOperator(bool _flag)
    {
        if (_flag)
        {

        }
        else
        {
            if (GameCenter.noviceGuideMng != null)
                GameCenter.noviceGuideMng.OverGuide();
        }
        if (GameCenter.curMainPlayer != null)
        {
            GameCenter.curMainPlayer.GoNormal();
        }
    }
    protected override void OnOpen()
    {
        base.OnOpen();
        startAutoReconnectTime = Time.time;
        isGoPass = true;
    }
    void Update()
    {
        if (isGoPass && GameCenter.curGameStage != null && GameCenter.curGameStage.SceneID <= 0)
        {
            if (Time.time - startAutoReconnectTime > 10.0f)
            {
                isGoPass = false;
                GameCenter.uIMng.ReleaseGUI(GUIType.AUTO_RECONNECT);
                GameCenter.messageMng.AddClientMsg(56, (x) =>
                {
                    GameCenter.instance.IsReConnecteding = false;
                    GameCenter.instance.GoPassWord();
                    if (GameCenter.mainPlayerMng != null && GameCenter.mainPlayerMng.MainPlayerInfo != null)
                    {
                        GameCenter.mainPlayerMng.MainPlayerInfo.CleanBuff();//清除buff
                    }
                    GameCenter.uIMng.ReleaseGUI(GUIType.RECONNECT);
                });
            }
        }
    }


}
