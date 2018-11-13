//======================================
//作者:吴江
//日期:2016/3/2
//用途:断线重新连接的窗口
//======================================

using UnityEngine;
using System.Collections;

public class ReconnectWnd : GUIBase
{

    public GameObject show;
    public GameObject unShow;
    public GameObject goBackBtn;
    public GameObject reconnectBtn;
    public UITimer showTime;

    public float ConnectedShowTime = 60f;

    public float oneTime = 2.7f;

    protected float startConnectTime = 0;

    void Awake()
    {
        mutualExclusion = false;
        Layer = GUIZLayer.TIP + 200;
        if (GameCenter.msgLoackingMng != null)
        {
            GameCenter.msgLoackingMng.CleanSerializeList();
        }
        if (GameCenter.instance.IsConnected)
        {
            NetMsgMng.ConectClose();
        }
        if (goBackBtn != null) UIEventListener.Get(goBackBtn).onClick += OnBtnPassWord;
        if (reconnectBtn != null) UIEventListener.Get(reconnectBtn).onClick += OnBtnConnected;

    }



    protected override void OnOpen()
    {
        base.OnOpen();

        if(show != null)show.SetActive(false);
        if(unShow != null)unShow.SetActive(true);

    }


    void OnBtnPassWord(GameObject games)
    {
        //GameCenter.uIMng.ReleaseGUI(GUIType.NEWTHINGSGET);
        GameCenter.instance.IsReConnecteding = false;
        GameCenter.instance.GoPassWord();
        if (GameCenter.mainPlayerMng != null && GameCenter.mainPlayerMng.MainPlayerInfo != null)
        {
            GameCenter.mainPlayerMng.MainPlayerInfo.CleanBuff();//清除buff
        }
        GameCenter.uIMng.ReleaseGUI(GUIType.RECONNECT);

        OtherOperator(false);
    }

    public void OnBtnConnected(GameObject games)
    {
        StartCoroutine(StartConnect());
    }


    IEnumerator StartConnect()
    {
        GameCenter.uIMng.SwitchToUI(GUIType.AUTO_RECONNECT);
        yield return new WaitForSeconds(1.0f);
        GameCenter.loginMng.C2S_ConectQueueServer(GameCenter.loginMng.Quaue_IP, GameCenter.loginMng.Quaue_port);
        GameCenter.instance.IsReConnecteding = true;
        OtherOperator(true);
        GameCenter.uIMng.ReleaseGUI(GUIType.BOXREWARD);//礼品获得界面不是互斥的，特别加下
        GameCenter.uIMng.ReleaseGUI(GUIType.RECONNECT);
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



}
