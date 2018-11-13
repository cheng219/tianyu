//======================================================
//作者:黄洪兴
//日期:2016/06/28
//用途:大地图红点组件
//======================================================
using UnityEngine;
using System.Collections;

public class MapItem : MonoBehaviour {

    public GameObject Obj;
    public UILabel objName;
    public Vector3 point = Vector3.zero;
    public string Name;
    public int sceneid;


    public void  Init(string _name,Vector3 _point,int _sceneid)
{
    if (objName != null)
        objName.text = _name;
    point = _point;
    Name = _name;
    sceneid = _sceneid;

}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
