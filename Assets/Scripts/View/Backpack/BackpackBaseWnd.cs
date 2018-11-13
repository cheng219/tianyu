//====================================================
//作者：邓成
//日期：2016/3/2
//用途：背包基础界面
//======================================================

using UnityEngine;
using System.Collections;

public class BackpackBaseWnd : GUIBase {
	public UIButton btnClose;

    public UIToggle[] toggles;
    enum ToggleType
    {
        Backpack,
        Synthetic,
        Decomposition,
        Storage,
        Seal
    }

	public UISprite storageCloseBack;
	void Awake()
	{
		layer = GUIZLayer.TOPWINDOW;
		mutualExclusion = true;
        allSubWndNeedInstantiate = true;
		if (btnClose != null) UIEventListener.Get(btnClose.gameObject).onClick = OnClickCloseBtn;
        if (toggles != null)
        {
            for (int i = 0, length = toggles.Length; i < length; i++)
            {
                if (toggles[i] != null) UIEventListener.Get(toggles[i].gameObject).onClick = ClickToggleEvent;
            }
        }
	}
	protected override void OnOpen ()
	{
        if (initSubGUIType == SubGUIType.NONE)
        {
            initSubGUIType = SubGUIType.BACKPACKBACKWND;
            if (toggles != null && toggles.Length > (int)ToggleType.Backpack) lastChangeToggle = toggles[(int)ToggleType.Backpack];
        }
		base.OnOpen ();
		if(storageCloseBack != null)
			storageCloseBack.enabled = (GameCenter.vipMng.VipData == null || GameCenter.vipMng.VipData.vLev < 1);
	}
	protected override void OnClose ()
	{
		base.OnClose ();
	}
	protected override void HandEvent (bool _bind)
	{
		base.HandEvent (_bind);
		if(_bind)
		{
			
		}else
		{
			
		}
	}
	protected override void InitSubWndState ()
	{
		base.InitSubWndState ();
        UIToggle toggle = null;
        switch (initSubGUIType)
        {
            case SubGUIType.BACKPACKBACKWND:
                if (toggles != null && toggles.Length > (int)ToggleType.Backpack) toggle = toggles[(int)ToggleType.Backpack];
                break;
            case SubGUIType.DECOMPOSITION:
                if (toggles != null && toggles.Length > (int)ToggleType.Decomposition) toggle = toggles[(int)ToggleType.Decomposition];
                break;
            case SubGUIType.BSynthesis:
                if (toggles != null && toggles.Length > (int)ToggleType.Synthetic) toggle = toggles[(int)ToggleType.Synthetic];
                break;
            case SubGUIType.SEALEQU:
                if (toggles != null && toggles.Length > (int)ToggleType.Seal) toggle = toggles[(int)ToggleType.Seal];
                break;
            case SubGUIType.WAREHOUSE:
                if (toggles != null && toggles.Length > (int)ToggleType.Storage) toggle = toggles[(int)ToggleType.Storage];
                break;
            default:
                if (toggles != null && toggles.Length > (int)ToggleType.Backpack) toggle = toggles[(int)ToggleType.Backpack];
                break;
        }
        if (toggle != null)
        {
            toggle.value = true;
            ClickToggleEvent(toggle.gameObject);
        }
	}
	protected UIToggle lastChangeToggle = null;
	protected void ClickToggleEvent(GameObject go)
	{
		UIToggle toggle = go.GetComponent<UIToggle>();
        if (toggle != null && toggles != null && toggles.Length > (int)ToggleType.Storage && toggle == toggles[(int)ToggleType.Storage] 
            && GameCenter.vipMng.VipData != null && GameCenter.vipMng.VipData.vLev < 1)
		{
			GameCenter.messageMng.AddClientMsg(295);
			if(lastChangeToggle != null)lastChangeToggle.value = true;
			return;
		}
		if(toggle != lastChangeToggle)
		{
			OnChange();
		}
		if(toggle != null && toggle.value)lastChangeToggle = toggle;
	}
	protected void OnChange()
	{
        for (int i = 0, max = toggles.Length; i < max; i++)
        {
            if (toggles[i].value)
            {
                CloseAllSubWnd();
                ToggleType toggleType = (ToggleType)i;
                SwitchToSubWnd(subWndArray[i].type);
                switch (toggleType)
                {
                    case ToggleType.Storage:
                        GameCenter.uIMng.GenGUI(GUIType.BACKPACKWND,true);
			            GameCenter.uIMng.GenGUI(GUIType.STORAGE,true);
                        break;
                }
            }
            else
            {
                subWndArray[i].CloseUI();
            }
        }
	}
	void CloseAllSubWnd()
	{
        GameCenter.uIMng.ReleaseGUI(GUIType.STORAGE);
        GameCenter.uIMng.ReleaseGUI(GUIType.BACKPACKWND);
        GameCenter.uIMng.ReleaseGUI(GUIType.PREVIEW_MAIN);
	}
	/// <summary>
	/// 关闭窗口
	/// </summary>
	/// <param name="obj"></param>
	void OnClickCloseBtn(GameObject obj)
	{
		CloseAllSubWnd();
		GameCenter.uIMng.SwitchToUI(GUIType.NONE);
	}
}
