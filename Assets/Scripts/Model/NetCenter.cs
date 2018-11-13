//=======================================
//作者：吴江
//日期：2015/5/20
//用途：网络消息中心
//========================================


using UnityEngine;
using System.Collections;
using st.net.NetBase;
using System;
using System.Net;

public class NetCenter
{
    public static int instanceBase = 0;

    protected int instanceID = -1;

    protected MySocketNet net = null;

    /// <summary>
    /// 连接状态（否决的），应当使用Connected访问器
    /// </summary>
    protected static bool connected = false;



    //public bool xxxxxxxxx
    //{
    //    get
    //    {
    //        return net.xxxxxxxxx;
    //    }
    //    set
    //    {
    //        net.xxxxxxxxx = value;
    //    }
    //}
    /// <summary>
    /// 连接状态（访问器）
    /// </summary>
    public static bool Connected
    {
        protected set
        {
            if (connected != value)
            {
                connected = value;
            }
            LastConnectedPing = 0;
        }
        get
        {
            return connected;
        }
    }


    protected static bool isConnectedFaild = false;
    public static bool IsConnectedFaild
    {
        get
        {
            return isConnectedFaild;
        }
        protected set
        {
            if (isConnectedFaild != value)
            {
                isConnectedFaild = value;
            }
        }
    }

    protected static float lastConnectedPing = 0;
    /// <summary>
    /// 心跳
    /// </summary>
    public static float LastConnectedPing
    {
        protected set
        {
            if (lastConnectedPing != value)
            {
                lastConnectedPing = value;
            }
        }
        get
        {
            return lastConnectedPing;
        }
    }


    /// <summary>
    /// 最后一次尝试连接的端口
    /// </summary>
    protected static int lastTryConnectPort = -1;
    /// <summary>
    /// 最后一次尝试连接的端口（访问器）
    /// </summary>
    public static int LastTryConnectPort
    {
        protected set
        {
            if (lastTryConnectPort != value)
            {
                lastTryConnectPort = value;
            }
        }
        get
        {
            return lastTryConnectPort;
        }
    }

    public void init(string server, int port)
    {
        IsConnectedFaild = false;
        instanceID = instanceBase++;
        net = new MySocketNet(server, (uint)port);
        net.Error += new NetEventHandler(OnError);
        // net.Info += new NetEventHandler(OnInfo); ;
        net.Received += new NetEventHandler(OnReceived);
        net.ConnectFailed += new NetEventHandler(OnConnectFailed);
        net.Connected += new NetEventHandler(OnConnected);
        net.Closed += new NetEventHandler(OnClosed);
    }

    public void UnRegist()
    {
        if (net != null)
        {
            net.Error = null;
            net.Received = null;
            net.ConnectFailed = null;
            net.Connected = null;
            net.Closed = null;
            net = null;
        }
    }

    public void start_connect(string ip, int port = 8000)
    {
        IsConnectedFaild = false;
        IPAddress serverIP = System.Net.Dns.GetHostAddresses(ip)[0];
        net.server = serverIP.ToString(); ;
        net.port = (uint)port;
        net.Connect();
    }

    public void send(Pt _pt)
    {
        if (Connected)
        {
            net.Send(_pt);
            NetRecorder.recordNetCmd(_pt);
            LastConnectedPing = Time.deltaTime;
        }
    }


    public void close()
    {
        LastConnectedPing = 0;
        IsConnectedFaild = true;
        if (Connected)
        {
            Connected = false;
            if (net != null)
            {
                net.Close();
            }
        }
    }

    public void ForceClose()
    {
        Connected = false;
        LastConnectedPing = 0;
        IsConnectedFaild = true;
        if (net != null)
        {
            net.Close();
            //  NGUIDebug.Log(net.server + " , " + net.port + " , instanceID = " + instanceID + " , close");
        }
    }

    private void OnInfo(object sender, EventArgs e)
    {
        NetErr error = e as NetErr;

        Debug.Log("OnInfo = " + error.err);
    }

    private void OnError(object sender, EventArgs e)
    {
        NetErr error = e as NetErr;
        if (GameCenter.instance.IsReConnecteding)
        {
            close();
        }
    }

    void TIP(object[] objArr)
    {
        //  MvcServer.GetInstance().HandleEvent(StaticEvent.EVENT_CONNECTCLOSE);
        // MvcServer.GetInstance().HandleEvent(StaticEvent.EVENT_SCENE2LOGIN);
    }

    private void OnReceived(object sender, EventArgs e)
    {
        LastConnectedPing = 0;
        PtMsg pt = e as PtMsg;

        MsgHander.PutCmd(pt.bytes);
    }

    private void OnConnectFailed(object sender, EventArgs e)
    {
        IsConnectedFaild = true;
        LastConnectedPing = 0;
        NetErr error = e as NetErr;
        Debug.Log("OnConnectFailed error = " + error.err);
    }

    private void OnConnected(object sender, EventArgs e)
    {
        IsConnectedFaild = false;
        Connected = true;
        // MvcServer mvc = MvcServer.GetInstance();
        // mvc.HandleEvent(StaticEvent.EVENT_CONNETEDSERVER);
    }

    private void OnClosed(object sender, EventArgs e)
    {
        Connected = false;

        //		MessageST st = new MessageST();
        //		st.messID = 1;
        //		MvcServer.GetInstance().HandleEvent(StaticEvent.EVENT_CLIENT_MESS_SHOW,st);//断网提示
    }

    //重新连接
    public void reConnect(object[] arr)
    {
        Debug.Log("" + arr[0] + arr[1]);
        Connected = true;
        // MvcServer mvc = MvcServer.GetInstance();
        // LoginModel _model = mvc.FindModel(LoginModel.Name) as LoginModel;
        //if (_model.selRole == null)
        //{
        //    string acc = PlayerPrefs.GetString("LastName");
        //    string pass = PlayerPrefs.GetString("LastPass");

        //    LoginMsg.Login(acc, pass);
        //    return;
        //}
        //if (_model.selRole.removeTime > 0)
        //{
        //    Debug.Log("角色正在删除中不能登录！");
        //    return;
        //}
        // LoginMsg.EnterGame(_model.selRole.id);

    }
}
