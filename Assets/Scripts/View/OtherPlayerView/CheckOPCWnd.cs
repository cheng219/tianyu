//==============================================
//作者：邓成
//日期：2016/4/28
//用途：查看其他玩家信息窗口
//=================================================


using UnityEngine;
using System.Collections;

public class CheckOPCWnd : GUIBase 
{

    #region 外部控件
    /// <summary>
    /// 切换toggle
    /// </summary>
    public UIToggle[] toggles = new UIToggle[3];

    /// <summary>
    /// 关闭按钮
    /// </summary>
    public UIButton CloseButton;
    #endregion
    void Awake()
    {
        mutualExclusion = false;
        layer = GUIZLayer.TIP; 
    }
	// Use this for initialization
	void Start () {
        if (CloseButton != null) UIEventListener.Get(CloseButton.gameObject).onClick = OnClickClose;
        for (int i = 0; i < toggles.Length; i++)
        {
            if (toggles[i] != null) UIEventListener.Get(toggles[i].gameObject).onClick -= SetCurOpenWnd;
            if (toggles[i] != null) UIEventListener.Get(toggles[i].gameObject).onClick += SetCurOpenWnd;
        }
	}

    protected override void OnOpen()
    {  
        initSubGUIType = SubGUIType.PREVIEWEQUIP;
        base.OnOpen();
    }
    #region 控件事件
    /// <summary>
    /// 点击关闭按钮事件
    /// </summary>
    /// <param name="obj"></param>
    void OnClickClose(GameObject obj)
    { 
        GameCenter.uIMng.ReleaseGUI(GUIType.PREVIEWOTHERS);
		//GameCenter.uIMng.CloseGUI(GUIType.PREVIEWOTHERS);
    }
    /// <summary>
    /// 设置当前打开的界面
    /// </summary>
    /// <param name="obj"></param>
    void SetCurOpenWnd(GameObject obj)
    {
        if (toggles[0].value)
        {
            SwitchToSubWnd(SubGUIType.PREVIEWEQUIP);
			//SwitchToSubWnd(SubGUIType.PREVIEWINFORMATION);
            //GameCenter.previewManager.C2S_AskEquitMent();
        }
        else if (toggles[1].value)
        {
            SwitchToSubWnd(SubGUIType.PREVIEWPET);
            GameCenter.previewManager.C2S_AskOpcPetPreview(GameCenter.previewManager.CurAskPlayerInfo.ServerInstanceID);
        }
        else if (toggles[2].value)
        {
            SwitchToSubWnd(SubGUIType.PREVIEWMOUNT);
            GameCenter.previewManager.C2S_AskOpcPetPreview(GameCenter.previewManager.CurAskPlayerInfo.ServerInstanceID);
        }
    }
    #endregion
}
