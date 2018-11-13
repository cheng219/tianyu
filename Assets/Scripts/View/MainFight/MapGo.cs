//======================================================
//作者:黄洪兴
//日期:2016/06/27
//用途:地图移动弹窗
//======================================================
using UnityEngine;
using System.Collections;

public class MapGo : MonoBehaviour {




    /// <summary>
    /// 步行
    /// </summary>
    public GameObject walkBtn;
    /// <summary>
    /// 飞行
    /// </summary>
    public GameObject flyBtn;





    //private Vector3 target=Vector3.zero;

    //private int sceneId;

  
  
    public void Init(Vector3 _v3,int _sceneId=0)
    {
        GameCenter.taskMng.flyVec = _v3;
        if (walkBtn != null)
            UIEventListener.Get(walkBtn).onClick = OnWalk;
        if (flyBtn != null)
            UIEventListener.Get(flyBtn).onClick = OnFly;
        GameCenter.taskMng.seceneId = _sceneId;

        GameCenter.taskMng.flyType = 2;
    }

    public void OnWalk(GameObject _go)
    {
        Vector3 target = GameCenter.taskMng.flyVec;
        int sceneId = GameCenter.taskMng.seceneId;
        GameCenter.curMainPlayer.CancelCommands();
        if (sceneId == 0)
        {
            GameCenter.curMainPlayer.GoTraceTarget(GameCenter.mainPlayerMng.MainPlayerInfo.SceneID, (int)target.x, (int)target.y);
            this.gameObject.SetActive(false);
        }
        else
        {
            GameCenter.curMainPlayer.GoTraceTarget(sceneId, (int)GameCenter.taskMng.flyVec.x, (int)target.z);
            this.gameObject.SetActive(false);
        }
        //Command_MoveTo moveto = new Command_MoveTo();
        //moveto.destPos = ActorMoveFSM.LineCast(new Vector3(target.x,0,target.y), true);
        //moveto.maxDistance = 0f;
        //GameCenter.curMainPlayer.commandMng.PushCommand(moveto);
       // GameCenter.uIMng.SwitchToUI(GUIType.NONE);


    }


    public void OnFly(GameObject _go)
    {

        if (GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel >= 10 && GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel < 21)
        {
            FlyTo();
        }
        else if (GameCenter.vipMng.VipData.vLev >= 1)
        {
            FlyTo();
        }
        else
        { 
            if (GameCenter.systemSettingMng.ShowFlyTips)
            {
                FlyTo();
            }
            else
            {
                GameCenter.curMainPlayer.StopMovingTo();
                GameCenter.curMainPlayer.CancelCommands();
                GameCenter.curMainPlayer.GoNormal();
                //MessageST mst = new MessageST();
                //object[] pa = { 1 };
                //mst.pars = pa;
                //mst.delPars = delegate(object[] ob)
                //{
                //    if (ob.Length > 0)
                //    {
                //        bool b = (bool)ob[0];
                //        if (b)
                //            GameCenter.systemSettingMng.ShowFlyTips = false;
                //    }

                //};
                //mst.messID = 357;
                //mst.delYes = delegate
                //{
                //    FlyTo();
                //};
                //GameCenter.messageMng.AddClientMsg(mst);

                GameCenter.uIMng.GenGUI(GUIType.FLYREMIND, true);
            }


        }
    }


    void FlyTo()
    {
        int sceneId = GameCenter.taskMng.seceneId;
        Vector3 target = GameCenter.taskMng.flyVec;
        GameCenter.curMainPlayer.StopMovingTo();
        GameCenter.curMainPlayer.CancelCommands();
        GameCenter.curMainPlayer.GoNormal();
        if (sceneId == 0)
        {
            SceneRef scene = GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneRef;
            if (scene != null)
            {
                if (scene.allow_fly_by_pos_src == 1)
                {
                    GameCenter.mainPlayerMng.C2S_Fly_Pint(GameCenter.mainPlayerMng.MainPlayerInfo.SceneID, (int)target.x, (int)target.y);
                }
                else
                {
                    GameCenter.messageMng.AddClientMsg(86);
                }
            }
            this.gameObject.SetActive(false);
        }
        else
        {
            GameCenter.mainPlayerMng.C2S_Fly_Pint(sceneId, (int)target.x, (int)target.z);
            this.gameObject.SetActive(false);
        }
    }
     
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
