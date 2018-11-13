//====================================
//作者：吴江
//日期：2015/10/27
//用途：系统设置窗口
//=====================================


using UnityEngine;
using System.Collections;

public class SystemSettingWnd : GUIBase {

    public enum MenuType
    {
        render = 0,
        id = 1,
    }


	#region 控件引用
    /// <summary>
    /// 界面选择
    /// </summary>
    public UIToggle[] menuTogArray = new UIToggle[2];

    /// <summary>
    /// 实时阴影开关 by吴江
    /// </summary>
    public UIToggle realTimeShadow;
    /// <summary>
    /// 画面设置
    /// </summary>
    public UIToggle[] renderQuality = new UIToggle[3];
    /// <summary>
    /// 音效开关
    /// </summary>
    public UIToggle[] soundOpen = new UIToggle[2];
    /// <summary>
    /// 伤害数字
    /// </summary>
    public UIToggle stateTexterOpen;
    /// <summary>
    /// 屏幕震动
    /// </summary>
    public UIToggle screenShake;
    /// <summary>
    /// 显示血条
    /// </summary>
    public UIToggle bloodShow;
    /// <summary>
    /// 其他玩家显示限制
    /// </summary>
    public UISlider opcShowCount;
    /// <summary>
    /// 其他玩家显示限制
    /// </summary>
    public GameObject opcShowCountTint;
    /// <summary>
    ///  其他玩家显示限制数量
    /// </summary>
    public UILabel showCountLabel;
	/// <summary>
	/// 关闭按钮
	/// </summary>
	public GameObject closeBtn;
	#endregion

	#region 控件事件

    protected void OnClickMenuTog()
    {
        MenuType type = MenuType.render;
        for (int i = 0; i < menuTogArray.Length; i++)
        {
            if (menuTogArray[i].value)
            {
                type = (MenuType)i;
                break;
            }
        }
        switch (type)
        {
            case MenuType.render:
                SwitchToSubWnd(SubGUIType.SYSTEM_SETTING_RENDER);
                break;
            case MenuType.id:
                SwitchToSubWnd(SubGUIType.SYSTEM_SETTING_ID);
                break;
            default:
                break;
        }

    }

	protected void CloseWnd(GameObject _obj)
	{
		GameCenter.uIMng.SwitchToUI (GUIType.NONE);
	}


    protected void OnClickRealTimeShadow(GameObject _obj)
    {
        if (realTimeShadow != null)
        {
            SystemSettingMng.RealTimeShadow = realTimeShadow.value;
            SceneRoot instance = SceneRoot.GetInstance();
            if (instance != null)
            {
                instance.DirectionalLightActive(SystemSettingMng.RealTimeShadow);
            }
        }
    }

    protected void OnClickStateTexterOpen(GameObject _obj)
    {
        if (stateTexterOpen != null)
        {
            SystemSettingMng.ShowStateTexts = stateTexterOpen.value;
        }
    }

    protected void OnClickScreenShake(GameObject _obj)
    {
        if (screenShake != null)
        {
            GameCenter.systemSettingMng.OpenVibrate = screenShake.value;
        }
    }

    protected void OnClickBloodShow(GameObject _obj)
    {
        if (bloodShow != null)
        {
            GameCenter.systemSettingMng.ShowBloodSlider = bloodShow.value;
        }
    }

    protected void OnClickRenderQuality(GameObject _obj)
    {
        if (renderQuality.Length > 0)
        {
            for (int i = 0; i < renderQuality.Length; i++)
            {
                if (renderQuality[i] != null && renderQuality[i].value)
                {
                    GameCenter.systemSettingMng.CurRendererQuality = (SystemSettingMng.RendererQuality)(i + 1);
                }
            }
        }
    }

    protected void OnClickSoundOpen(GameObject _obj)
    {
        if (soundOpen.Length > 0)
        {
            if (soundOpen[0] != null)
            {
                GameCenter.systemSettingMng.OpenSoundEffect = soundOpen[0].value;
            }
            if (soundOpen[1] != null)
            {
                GameCenter.systemSettingMng.OpenMusic = soundOpen[1].value;
            }
        }
    }

    protected void OnClickOpcShowCount(GameObject _obj)
    {
        if (opcShowCount != null)
        {
            GameCenter.systemSettingMng.MaxPlayer = (int)opcShowCount.value * 20;
            showCountLabel.text = GameCenter.systemSettingMng.MaxPlayer.ToString();
        }
    }

    protected void OnDragOpcShowCount(GameObject _obj ,Vector2 delta)
    {
        if (opcShowCount != null)
        {
            GameCenter.systemSettingMng.MaxPlayer = (int)(opcShowCount.value * 20);
            showCountLabel.text = GameCenter.systemSettingMng.MaxPlayer.ToString();
        }
    }
	#endregion

    #region UNITY
    /// <summary>
	/// 初始化引用变量
	/// </summary>
    void Awake()
    {
        mutualExclusion = true;
        needCloaseMainCamera = true;
        realTimeShadow.value = SystemSettingMng.RealTimeShadow;
        stateTexterOpen.value = SystemSettingMng.ShowStateTexts;
        screenShake.value = GameCenter.systemSettingMng.OpenVibrate;
        bloodShow.value = GameCenter.systemSettingMng.ShowBloodSlider;
        if (soundOpen.Length > 0)
        {
            if (soundOpen[0] != null)
            {
                soundOpen[0].value = GameCenter.systemSettingMng.OpenSoundEffect;
            }
            if (soundOpen[1] != null)
            {
                soundOpen[1].value = GameCenter.systemSettingMng.OpenMusic;
            }
        }
        opcShowCount.value = GameCenter.systemSettingMng.MaxPlayer; 
		
        for (int i = 0; i < soundOpen.Length; i++)
        {
            UIEventListener.Get(soundOpen[i].gameObject).onClick -= OnClickSoundOpen;
            UIEventListener.Get(soundOpen[i].gameObject).onClick += OnClickSoundOpen;
        }
        if (GameCenter.systemSettingMng.CurRendererQuality == SystemSettingMng.RendererQuality.NONE)
        {
            GameCenter.systemSettingMng.CurRendererQuality = SystemSettingMng.RendererQuality.MID;
        }
        for (int i = 0; i < renderQuality.Length; i++)
        {
            UIEventListener.Get(renderQuality[i].gameObject).onClick -= OnClickRenderQuality;
            UIEventListener.Get(renderQuality[i].gameObject).onClick += OnClickRenderQuality;
            renderQuality[i].value = (int)GameCenter.systemSettingMng.CurRendererQuality == (i + 1);
        }

    }

    protected override void OnOpen()
    {
        base.OnOpen();
        Layer = GUIZLayer.NORMALWINDOW;
        showCountLabel.text = GameCenter.systemSettingMng.MaxPlayer.ToString();
        opcShowCount.value = GameCenter.systemSettingMng.MaxPlayer / 20.0f;
    }

    protected override void HandEvent(bool _bind)
    {
        base.HandEvent(_bind);
        if (_bind)
        {
            UIEventListener.Get(closeBtn).onClick += CloseWnd;
            UIEventListener.Get(realTimeShadow.gameObject).onClick += OnClickRealTimeShadow;
            UIEventListener.Get(stateTexterOpen.gameObject).onClick += OnClickStateTexterOpen;
            UIEventListener.Get(screenShake.gameObject).onClick += OnClickScreenShake;
            UIEventListener.Get(bloodShow.gameObject).onClick += OnClickBloodShow;
            UIEventListener.Get(opcShowCount.gameObject).onDrag += OnDragOpcShowCount;
            UIEventListener.Get(opcShowCountTint).onDrag += OnDragOpcShowCount;
            UIEventListener.Get(opcShowCount.gameObject).onClick += OnClickOpcShowCount;
            UIEventListener.Get(opcShowCountTint).onClick += OnClickOpcShowCount;
            for (int i = 0; i < menuTogArray.Length; i++)
            {
                EventDelegate.Add(menuTogArray[i].onChange, OnClickMenuTog);
            }
        }
        else
        {
            UIEventListener.Get(closeBtn).onClick -= CloseWnd;
            UIEventListener.Get(realTimeShadow.gameObject).onClick -= OnClickRealTimeShadow;
            UIEventListener.Get(stateTexterOpen.gameObject).onClick -= OnClickStateTexterOpen;
            UIEventListener.Get(screenShake.gameObject).onClick -= OnClickScreenShake;
            UIEventListener.Get(bloodShow.gameObject).onClick -= OnClickBloodShow;
            UIEventListener.Get(opcShowCount.gameObject).onDrag -= OnDragOpcShowCount;
            UIEventListener.Get(opcShowCountTint).onDrag -= OnDragOpcShowCount;
            UIEventListener.Get(opcShowCount.gameObject).onClick -= OnClickOpcShowCount;
            UIEventListener.Get(opcShowCountTint).onClick -= OnClickOpcShowCount;
            for (int i = 0; i < menuTogArray.Length; i++)
            {
                EventDelegate.Remove(menuTogArray[i].onChange, OnClickMenuTog);
            }
        }
    }
    #endregion

    #region 辅助逻辑
    
    #endregion

}
