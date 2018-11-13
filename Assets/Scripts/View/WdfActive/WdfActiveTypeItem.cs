//======================================================
//作者:朱素云
//日期:2016/7/13
//用途:精彩活动Type组件
//======================================================
using UnityEngine;
using System.Collections;

public class WdfActiveTypeItem : MonoBehaviour {

    public UILabel TypeName;
    public GameObject checkMark;
    public GameObject checkBtn;
    public GameObject redObj;
    int typeID;
    public  int type;

    public GameObject timeLimite;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}




    public void Refresh(string _name,int _type,int _id)
    {
        //Debug.Log("刷新TYPE" + GameCenter.wdfActiveMng.CurWdfActiveType);
        typeID = _id;
        type = _type;
        if (TypeName != null)
        {
            TypeName.text = _name;
        }
        if (timeLimite != null)
        {
            timeLimite.SetActive(_type == 11);//连充豪礼
        }
        if (checkMark != null)
        {
            checkMark.SetActive(_type == GameCenter.wdfActiveMng.CurWdfActiveType);
        }
        if (checkBtn != null)
            UIEventListener.Get(checkBtn).onClick = OnCheckBtn;

    }


    void OnCheckBtn(GameObject _go)
    {
        GameCenter.wdfActiveMng.CurWdfActiveType = type;
        GameCenter.wdfActiveMng.needReset = true;
       // Debug.Log("改变TYPE"+type);
        if (GameCenter.wdfActiveMng.OnGetAllActiveTypes != null)
            GameCenter.wdfActiveMng.OnGetAllActiveTypes();
        GameCenter.wdfActiveMng.C2S_AskActivitysInfoByID(typeID);
    }


    public void SetRed(bool _b)
    {
        if (redObj != null)
            redObj.SetActive(_b);
    }
}
