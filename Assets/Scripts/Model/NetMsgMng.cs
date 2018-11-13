//=============================
//作者:吴江
//日期:2015/5/20
//用途:网络层消息收发管理类
//===========================


using UnityEngine;
using System.Collections;
using st.net.NetBase;
using System;
public class NetMsgMng {


    protected static NetCenter netCenter = null;
    protected static MsgHander msgHander = null;


    /// <summary>
    /// 是否在屏幕上打印客户端推送的消息 by吴江
    /// </summary>
    public static bool screen_Debug_Log_C2S = false;

    /// <summary>
    /// 是否在编辑器上打客户端推送的消息 by吴江
    /// </summary>
    public static bool editor_Debug_Log_C2S = false;

    protected static uint serializeID = 0;
    protected static uint unLockSerializeID = 1000000;
    /// <summary>
    /// 获得一个新的问答号
    /// </summary>
    /// <returns></returns>
    public static uint CreateNewSerializeID()
    {
        if (serializeID >= 990000)
        {
            serializeID = 0;
        }
        return ++serializeID;
    }

    /// <summary>
    /// 获得一个新的非锁屏问答号
    /// </summary>
    /// <returns></returns>
    public static uint CreateNewUnLockSerializeID()
    {
        if (unLockSerializeID >= 10000000)
        {
            unLockSerializeID = 1000000;
        }
        return ++unLockSerializeID;
    }
    /// <summary>
    /// 重置问答号 by吴江
    /// </summary>
    public static void ResetSerializeID()
    {
        serializeID = 0;
    }

    /// <summary>
    /// 重置问答号 by吴江
    /// </summary>
    public static void ResetUnLockSerializeID()
    {
        unLockSerializeID = 1000000;
    }

    public static NetMsgMng CreateNew(string server, int port)
    {
        if (GameCenter.netMsgMng == null)
        {
            NetMsgMng netMsgMng = new NetMsgMng();
            netMsgMng.Init(server, port);
            return netMsgMng;
        }
        else
        {
            GameCenter.netMsgMng.UnRegist();
            return GameCenter.netMsgMng;
        }
    }

    public static void ReCreateNetCenter(string _ip, int _port)
    {
        if (netCenter != null)
        {
            netCenter.UnRegist();
            netCenter.close();
        }
        netCenter = null;
        netCenter = new NetCenter();
        netCenter.init(_ip, _port);
    }

    protected void Init(string server, int port)
    {
        netCenter = new NetCenter();
        netCenter.init(server, port);
        msgHander = new MsgHander();
        msgHander.Init();

        screen_Debug_Log_C2S = GameCenter.instance.screen_Debug_Log_C2S;
        editor_Debug_Log_C2S = GameCenter.instance.editor_Debug_Log_C2S;
		
    }


    protected void UnRegist()
    {
    }


    public static void ConectServer(string _ip,int port = 8000)
    {
        ReCreateNetCenter(_ip, port);
        netCenter.start_connect(_ip, port);
    }

    public static void SendMsg(Pt _pt)
    {
        if (screen_Debug_Log_C2S)
        {
            GameSys.LogColor("[C] to [S] :" + Convert.ToString(_pt.GetID(), 16), Color.yellow);
        }
        if (editor_Debug_Log_C2S)
        {
            Debug.Log("[C] to [S] :" + Convert.ToString(_pt.GetID(), 16));
        }
        if (_pt.seq != 0 && _pt.seq < 1000000)
        {
            GameCenter.msgLoackingMng.UpdateSerializeList((int)_pt.seq, true);
        }
        netCenter.send(_pt);
    }


    public static void ConectClose()
    {
        netCenter.close();
    }

    public static void ForceConnectClose()
    {
        netCenter.ForceClose();
    }




    void RegistAll()
    {
        //pt_login_a001  C帐号密码登陆 
        //pt_usr_list_A102 S角色列表
        //pt_req_net_a003 C请求排队

        //pt_queue_info_a104 S后台返回的排队信息    or            //pt_net_info_a105 S排队完以后,去哪个地方登陆

        //重新connect
        //pt_usr_enter_b001 C角色登陆
        //pt_usr_info_b102 S角色详细信息
        //从角色信息里面，获取当前所处场景
        //初始化各种角色子模块信息 C2S S2C
        //pt_usr_enter_scene_b003 C2S请求登陆该场景
        //pt_req_load_scene_b104 S服务端通知开始加载指定场景
        //pt_load_scene_finish_b005 C加载完场景,通知后台
        //pt_scene_info_b106 S服务端得知加载场景完毕,通知客户端可以开始活动


        //pt_error_info_d001   //S错误报告




    }

}