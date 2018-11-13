//==================================
//作者：唐源
//日期：2017/2/23
//用途：省电模式界面类
//=================================

using UnityEngine;
using System.Collections;
using System;

public class PowerSavingWnd : GUIBase
{

    public UILabel timeLab;
    public GameObject clickClose;
    public GameObject tipObj;

    public enum ScreenLightSet
    {
        /// <summary>
        /// 还原
        /// </summary>
        RESTORE = 0,
        /// <summary>
        /// 设置为默认的一半
        /// </summary>
        HALF,
    }
    protected int countDownTime = 10;

    protected bool openSound = false;
    protected bool openMusic = false;
    protected static bool shadow = false;
    protected SystemSettingMng.RendererQuality qualityType = SystemSettingMng.RendererQuality.NONE;

    void Awake()
    {
        //UIEventListener.Get(clickClose).onClick = CloseThisTip;
    }
    //void Update()
    //{
    //    if (Input.anyKeyDown)
    //    {
    //        CloseThisTip();
    //    }
    //}
    void OnDestroy()
    {
        Application.targetFrameRate = 30;
    }
    protected override void OnOpen()
    {
        base.OnOpen();
        StartCountDown();
    }
    protected override void OnClose()
    {
        base.OnClose();
    }
    protected override void HandEvent(bool _bind)
    {
        base.HandEvent(_bind);
        if (_bind)
        {
            GameCenter.systemSettingMng.OnPowerSavingEvent += SetDefault;
        }
        else
        {
            GameCenter.systemSettingMng.OnPowerSavingEvent -= SetDefault;
        }
    }
    void StartCountDown()
    {
        int time = countDownTime;
        StartCoroutine(CountDown(time));
    }
    IEnumerator CountDown(int _time)
    {
        timeLab.text = "[b]" + _time;
        yield return new WaitForSeconds(1f);
        _time--;
        if (_time < 0)
        {
            SetPowerSaving();
        }
        else
        {
            StopCoroutine("CountDown");
            StartCoroutine(CountDown(_time));
        }
    }


    void SetPowerSaving()
    {
        openSound = GameCenter.systemSettingMng.OpenSoundEffect;
        qualityType = GameCenter.systemSettingMng.CurRendererQuality;
        openMusic = GameCenter.systemSettingMng.OpenMusic;
        shadow = SystemSettingMng.RealTimeShadow;
        GameCenter.systemSettingMng.RendererQualityNoRecord = SystemSettingMng.RendererQuality.LOW;
        GameCenter.systemSettingMng.OpenMusic = false;
        GameCenter.systemSettingMng.OpenSoundEffect = false;
        SystemSettingMng.RealTimeShadow = false;
        SceneRoot instance = SceneRoot.GetInstance();
        if (instance != null)
        {
            instance.DirectionalLightActive(SystemSettingMng.RealTimeShadow);
        }
        if (GameCenter.instance.isPlatform)
        {
            LynSdkManager.Instance.DCBrightness((int)ScreenLightSet.HALF);
        }
        Application.targetFrameRate = 10;
        GameCenter.systemSettingMng.IsPowerSaving = true;
        tipObj.SetActive(false);
    }
    /// <summary>
    /// 中断后关闭此提示窗口
    /// </summary>
    void CloseThisTip(GameObject go)
    {
        SetDefault();
    }
    void SetDefault()
    {
        StopAllCoroutines();
        if (GameCenter.systemSettingMng.IsPowerSaving)
        {
            GameCenter.systemSettingMng.RendererQualityNoRecord = qualityType;
            GameCenter.systemSettingMng.OpenSoundEffect = openSound;
            GameCenter.systemSettingMng.OpenMusic = openMusic;
            Application.targetFrameRate = 30;
            SystemSettingMng.RealTimeShadow = shadow;
            SceneRoot instance = SceneRoot.GetInstance();
            if (instance != null)
            {
                instance.DirectionalLightActive(SystemSettingMng.RealTimeShadow);
            }
            if (GameCenter.instance.isPlatform)
            {
                LynSdkManager.Instance.DCBrightness((int)ScreenLightSet.RESTORE);
            }
        }
        GameCenter.uIMng.ReleaseGUI(GUIType.POWERSAVING);
        GameCenter.systemSettingMng.IsPowerSaving = false;
    }
}


