//===============================
//作者：吴江
//日期：2014/7/24
//用途：3D血条组件
//================================


using UnityEngine;
using System.Collections;

/// <summary>
/// 聊天泡泡组件 by吴江
/// </summary>
public class BloodSliderPrefab : MonoBehaviour
{
    #region 数据
    /// <summary>
    /// 是否友方
    /// </summary>
    protected bool isFreind = false;
    /// <summary>
    /// 实际条
    /// </summary>
    public UISlider mainSlider;
    /// <summary>
    /// 跟随条
    /// </summary>
    public UISlider virtualSlider;
    /// <summary>
    /// 实际条表现
    /// </summary>
    public UISprite process;
    /// <summary>
    /// 背景框
    /// </summary>
    public GameObject backFrame;
    protected string RED_SPRITE_NAME = "Pic_xuetiao_red";
    protected string GRAY_SPRITE_NAME = "Pic_xuetiao_gray";
    protected string GREEN_SPRITE_NAME = "Pic_xuetiao_green";

    protected float curRate = 0;

    public float diffTime = 0.5f;
    protected float lastRateTime = 0;
    protected bool start = false;
    protected bool isEnable = true;
    #endregion


    #region UNITY
    void Awake()
    {
        curRate = 1;
        mainSlider.value = 1;
        virtualSlider.value = 1;
        SetRelationType(isFreind);
    }

    void OnEnable()
    {
        start = false;
    }

    void Update()
    {
        if (start)
        {
            float rate = (Time.time - lastRateTime) / diffTime;
            rate = Mathf.Clamp01(rate);
            virtualSlider.value = Mathf.Lerp(virtualSlider.value, curRate, rate);
        }
    }
    #endregion


    #region 辅助逻辑

    public void SetRelationType(bool _isFreind)
    {
        if (isFreind != _isFreind)
        {
            isFreind = _isFreind;
            if (isEnable)
            {
                process.spriteName = isFreind ? GREEN_SPRITE_NAME : RED_SPRITE_NAME;
            }
            else
            {
                process.spriteName = GRAY_SPRITE_NAME;
            }
        }
    }

    public void SetEnable(bool _enable)
    {
        isEnable = _enable;
        if (_enable)
        {
            process.spriteName = isFreind ? GREEN_SPRITE_NAME : RED_SPRITE_NAME;
        }
        else
        {
            process.spriteName = GRAY_SPRITE_NAME;
        }
    }


    public void Slider(float _rate)
    {
        lastRateTime = Time.time;
        _rate = Mathf.Clamp01(_rate);
        if (!start)
        {
            start = true;
            virtualSlider.value = _rate;
        }
        curRate = _rate;
        mainSlider.value = _rate;
    }

    public bool ShowBackFrame
    {
        set
        {
            if (backFrame != null) backFrame.SetActive(value); 
        }
    }
    #endregion

}
