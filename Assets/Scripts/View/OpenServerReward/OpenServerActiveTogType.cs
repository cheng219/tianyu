//======================================================
//作者:朱素云
//日期:2016/7/13
//用途:开服活动Type组件
//======================================================
using UnityEngine;
using System.Collections;

public class OpenServerActiveTogType : MonoBehaviour
{

    public UILabel TypeName;
    public GameObject checkMark;
    public GameObject checkBtn;
    public GameObject redObj;
    public OpenServerType type;

    public GameObject timeLimite;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }




    public void Refresh(string _name, OpenServerType _id, bool _islimit)
    {
        //Debug.Log("刷新TYPE" + GameCenter.wdfActiveMng.CurWdfActiveType);
        type = _id; 
        if (TypeName != null)
        {
            TypeName.text = _name;
        }
        if (timeLimite != null)
        {
            timeLimite.SetActive(_islimit);//连充豪礼
        }
        if (checkMark != null)
        {
            checkMark.SetActive(type == GameCenter.openServerRewardMng.curOpenServerType);
        }
        if (checkBtn != null)
            UIEventListener.Get(checkBtn).onClick = OnCheckBtn;

    }


    void OnCheckBtn(GameObject _go)
    {
        GameCenter.openServerRewardMng.curOpenServerType = type;
        if (checkMark != null)
        {
            checkMark.SetActive(true);
        }
    }

    public OpenServerActiveTogType CreateNew(Transform _parent)
    {
        GameObject obj = Instantiate(this.gameObject) as GameObject;
        obj.transform.parent = _parent;
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = Vector3.zero;
        obj.SetActive(true);
        return obj.GetComponent<OpenServerActiveTogType>();
    }

    public void SetRed(bool _b)
    {
        if (redObj != null)
            redObj.SetActive(_b);
    }
}
