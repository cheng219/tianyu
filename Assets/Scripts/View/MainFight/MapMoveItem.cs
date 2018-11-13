//======================================================
//作者:黄洪兴
//日期:2016/7/21
//用途:地图移动组件
//======================================================


using UnityEngine;
using System.Collections;

public class MapMoveItem : MonoBehaviour {

    public UILabel objName;
    public GameObject GoObj;
    public int sceneid;

    Vector3 point = Vector3.zero;
    public void Init(string _name,Vector3 _point,int _sceneid)
    {
        if (objName != null)
            objName.text = _name;
        sceneid = _sceneid;
        point = _point;
        if (GoObj != null)
        {
            UIEventListener.Get(GoObj).onClick -= GoToPoint;
            UIEventListener.Get(GoObj).onClick += GoToPoint;
        }
    }

    void GoToPoint(GameObject _go)
    {
        GameCenter.curMainPlayer.CancelCommands();
        //Command_MoveTo moveto = new Command_MoveTo();
        //moveto.destPos = ActorMoveFSM.LineCast(new Vector3(point.x, 0, point.y), true);
        //moveto.maxDistance = 0f;
        //GameCenter.curMainPlayer.commandMng.PushCommand(moveto);
        GameCenter.curMainPlayer.GoTraceTarget(GameCenter.mainPlayerMng.MainPlayerInfo.SceneID, (int)point.x, (int)point.y);
        //GameCenter.curMainPlayer.GoTraceTarget(sceneid, (int)point.x, (int)point.y);
       // Debug.Log("点击移动到坐标" + point.x + ":" + point.y + ":" + point.z);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
