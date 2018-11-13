//====================================
//作者：黄洪兴
//日期：2016/04/26
//用途：系统设置窗口
//=====================================


using UnityEngine;
using System.Collections;

public class SettingWnd : GUIBase {

	public enum MenuType
	{
		render = 0,
		helper = 1,
		notic=2,
		onhook=3,
        voice = 4,
	}


	#region 控件引用
	/// <summary>
	/// 界面选择
	/// </summary>
	public UIToggle[] menuTogArray = new UIToggle[4];

	/// <summary>
	/// 关闭按钮
	/// </summary>
	public GameObject closeBtn;
    /// <summary>
    /// 注销账号按钮
    /// </summary>
    public GameObject switchBtn;
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
                GameCenter.littleHelperMng.NeedType = LittleHelpType.STRONG;
                SwitchToSubWnd(SubGUIType.SYSTEM_SETTING_RENDER);
                break;
            case MenuType.helper:
                SwitchToSubWnd(SubGUIType.LITTLEHELPER);
                break;
            case MenuType.notic:
                GameCenter.littleHelperMng.NeedType = LittleHelpType.STRONG;
                SwitchToSubWnd(SubGUIType.NOTICESUBWND);
                break; 
            case MenuType.onhook:
                SwitchToSubWnd(SubGUIType.HANGUP);
                break;
            case MenuType.voice:
                SwitchToSubWnd(SubGUIType.VOICEPLAY);
                break;
            default:
                break;
        }

	}

	protected void CloseWnd(GameObject _obj)
	{
		GameCenter.uIMng.SwitchToUI (GUIType.NONE);
	}


    protected void SwitchAccount(GameObject go)
    {
        MessageST mst = new MessageST();
        mst.messID = 475;
        mst.delYes = (x) =>
            {
                GameCenter.instance.SwitchAccount();
            };
        GameCenter.messageMng.AddClientMsg(mst);
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
        if (closeBtn != null) UIEventListener.Get(closeBtn).onClick = CloseWnd;
        if (switchBtn != null) UIEventListener.Get(switchBtn).onClick = SwitchAccount;
	}

	protected override void OnOpen()
	{
		base.OnOpen();
		Layer = GUIZLayer.NORMALWINDOW;
//		showCountLabel.text = GameCenter.systemSettingMng.MaxPlayer.ToString();
//		opcShowCount.value = GameCenter.systemSettingMng.MaxPlayer / 20.0f;
	}

	protected override void HandEvent(bool _bind)
	{
		base.HandEvent(_bind);
		if (_bind)
		{
            switch (initSubGUIType)
            {
                case SubGUIType.SYSTEM_SETTING_RENDER:
                    if (menuTogArray.Length > 0) menuTogArray[0].value = true;
                    break;
                case SubGUIType.LITTLEHELPER:
                    if (menuTogArray.Length > 1) menuTogArray[1].value = true;
                    break;
                case SubGUIType.NOTICESUBWND:
                    if (menuTogArray.Length > 2) menuTogArray[2].value = true;
                    break;
                case SubGUIType.HANGUP:
                    if (menuTogArray.Length > 3) menuTogArray[3].value = true;
                    break;
                case SubGUIType.VOICEPLAY:
                    if (menuTogArray.Length > 4) menuTogArray[4].value = true;
                    break;
                default:
                    if (menuTogArray.Length > 0) menuTogArray[0].value = true;
                    break;
            }
			for (int i = 0; i < menuTogArray.Length; i++)
			{
				EventDelegate.Add(menuTogArray[i].onChange, OnClickMenuTog);
			}
		}
		else
		{
			for (int i = 0; i < menuTogArray.Length; i++)
			{
				EventDelegate.Remove(menuTogArray[i].onChange, OnClickMenuTog);
			}
		}
	}

    void OnDestroy()
    {
        GameCenter.littleHelperMng.NeedType = LittleHelpType.STRONG;
    }
	#endregion

	#region 辅助逻辑

	#endregion

}
