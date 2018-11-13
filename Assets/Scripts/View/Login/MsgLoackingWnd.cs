/// <summary>
/// 作者：何明军
/// 日期：2015/8/13
/// 加载滚动UI
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MsgLoackingWnd : GUIBase
{

    /// <summary>
    /// 开窗时间 by吴江
    /// </summary>
    protected float startCacuTime = -1;

    protected LoginStage Stage
    {
        get
        {
            LoginStage stage = GameCenter.curGameStage as LoginStage;
            return stage;
        }
    }

	void Awake () {
		mutualExclusion = false;
		layer = GUIZLayer.TIP + 1000;
	}

    void Update()
    {
        if (startCacuTime > 0 && Time.time - startCacuTime > SystemSettingMng.TIME_OUT_LIMIT)
        {
            if (GameCenter.sceneMng != null && GameCenter.sceneMng.EnterSceneSerlizeID > 0)
            {
                GameCenter.uIMng.SwitchToUI(GUIType.AUTO_RECONNECT);
                int sceneID = 0;
                PlayGameStage stage = GameCenter.curGameStage as PlayGameStage;
                if (stage != null)
                {
                    sceneID = stage.SceneID;
                }
                if (GameCenter.mainPlayerMng != null && GameCenter.mainPlayerMng.MainPlayerInfo.Level == 1)
                {
                    LynSdkManager.Instance.ReportConnectionLose(sceneID.ToString(), "1级时有掉线重连!");
                }
                LynSdkManager.Instance.ReportConnectionLose(sceneID.ToString(), "问答号异常,后台回复问答号超时,判断为断线!开启断线重连窗口!");
                GameCenter.uIMng.ReleaseGUI(GUIType.PANELLOADING);
                ClientTip();
                GameCenter.msgLoackingMng.CleanSerializeList();
                return;
            }
            else if (Stage != null && Stage.GetServerInfoSerlizeID > 0)
            {

                //NGUIDebug.Log("---wnd");
                Stage.TipReLoadServerInfo();//超时弹框
                GameCenter.uIMng.ReleaseGUI(GUIType.PANELLOADING);
                ClientTip();
                GameCenter.msgLoackingMng.CleanSerializeList();
                return;
            }
            if (GameCenter.msgLoackingMng.HasForceSerializeWaiting)
            {
                return;
            }
            GameCenter.uIMng.ReleaseGUI(GUIType.PANELLOADING);
            ClientTip();
            GameCenter.msgLoackingMng.CleanSerializeList();
        }
    }

	protected override void OnOpen ()
	{
		base.OnOpen ();
		OnUpdateCmdDictionary();
	}

    void ClientTip() //问答号超过时间限制提示LZR
    {
        //MessageST msg = new MessageST();
        //msg.messID = 200;
        //GameCenter.messageMng.AddClientMsg(msg);
        int sceneID = 0;
        if (GameCenter.mainPlayerMng != null && GameCenter.mainPlayerMng.MainPlayerInfo != null)
            sceneID = GameCenter.mainPlayerMng.MainPlayerInfo.SceneID;
        LynSdkManager.Instance.ReportConnectionLose(sceneID.ToString(),"问答号超时,判断为断线!开启断线重连窗口");
    }

	void OnUpdateCmdDictionary(){
        if (GameCenter.msgLoackingMng.HasSerializeWaiting)
        {
            startCacuTime = Time.time;
//			GameCenter.uIMng.GenGUI(GUIType.PANELLOADING,true);
		}else{
            GameCenter.uIMng.ReleaseGUI(GUIType.PANELLOADING);
		}
	}


	protected override void OnClose(){
        base.OnClose();
        startCacuTime = -1;
	}
}
