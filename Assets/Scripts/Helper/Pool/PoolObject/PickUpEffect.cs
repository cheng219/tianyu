//======================================
//作者:吴江
//日期:2016//
//用途:拾取特效的表现控制器
//======================================

using UnityEngine;
using System.Collections;

public class PickUpEffect : MonoBehaviour {

    protected Vector3 from;
    protected Transform to;
    protected bool hasInited = false;
    protected float startTime;
    protected float wholeTime;
    protected float holdTime;
    protected float curRate = 0;
    protected PoolEffecter thisPoolEffect = null;

    public void Init(Vector3 _from,SmartActor _to,float _time, float _holdTime, PoolEffecter _self)
    {
        if (_to == null) return;
        from = _from;
        to = _to.GetReceivePoint();
        wholeTime = _time;
        startTime = Time.time;
        curRate = 0;
        hasInited = true;
        holdTime = _holdTime;
        thisPoolEffect = _self;
    }


	
	void Update () {
        if (!hasInited) return;
        curRate = (Time.time - startTime) / wholeTime;
        transform.position = Vector3.Lerp(from, to.transform.position, Mathf.Clamp01(curRate));
	
	}
}
