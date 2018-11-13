//==============================================
//作者：贺丰
//日期：2015/12/16
//用途：血条渐变UI
//==============================================

using UnityEngine;
using System.Collections;

public class HpTweenUI : MonoBehaviour {


    #region 外部控件
    /// <summary>
    /// 血条背景
    /// </summary>
    public UISprite backSlider;
    /// <summary>
    /// 当前血量
    /// </summary>
    public UISprite curSlider;
    #endregion

    protected float lastRateTime = 0;
    protected bool start = false;
    /// <summary>
    /// 当前值
    /// </summary>
    private float curValue = 1.0f;
    /// <summary>
    /// 血量变化时间
    /// </summary>
    public float rateTime = 1.0f;
    public float diffTime = 0.5f;
	// Use this for initialization
	void Start () 
    {

	}
	void OnEnable()
    {
        start = false;
    }
	// Update is called once per frame
    void Update()
    {
        if (start)
        {
            float rate = (Time.time - lastRateTime) / diffTime;
            rate = Mathf.Clamp01(rate);
            backSlider.fillAmount = Mathf.Lerp(backSlider.fillAmount, curValue, rate);
        }
	}

    public void SetValue(float _value)
    {
        lastRateTime = Time.time;
        _value = Mathf.Clamp01(_value); 
        if (!start)
        {
            start = true;
            backSlider.fillAmount = _value;
        }
        curValue = _value;
        curSlider.fillAmount = curValue;
    }
}
