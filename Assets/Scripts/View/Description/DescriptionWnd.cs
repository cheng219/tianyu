//==========================
//作者：鲁家旗
//日期：2016/4/25
//用途：说明系统界面类
//==========================

using UnityEngine;
using System.Collections;

public class DescriptionWnd : GUIBase {
    public UIButton closeBtn;
    public UILabel titleLabel;
    public UILabel desLabel;
    void Awake()
    {
        mutualExclusion = false;
        layer = GUIZLayer.TIP;
        if(closeBtn != null) UIEventListener.Get(closeBtn.gameObject).onClick = delegate
        {
            GameCenter.uIMng.ReleaseGUI(GUIType.DESCRIPTION);
        };
    }
    protected override void HandEvent(bool _bind)
    {
        base.HandEvent(_bind);
        if (_bind)
		{
            GameCenter.descriptionMng.OnDescription += ShowWnd;
			GameCenter.descriptionMng.OnUpdateLoginNoticeEvent += ShowLoginNotice;
		}else
		{
            GameCenter.descriptionMng.OnDescription -= ShowWnd;
			GameCenter.descriptionMng.OnUpdateLoginNoticeEvent -= ShowLoginNotice;
		}
    }
    void ShowWnd(DescriptionRef descrip)
    {
        if (descrip == null) return;
        if(titleLabel != null) titleLabel.text = descrip.title;
        if(desLabel != null) desLabel.text = descrip.content.Replace("\\n", "\n");
    }
	void ShowLoginNotice(string notice)
	{
		if(titleLabel != null) titleLabel.text =ConfigMng.Instance.GetUItext(275);
		if(desLabel != null) desLabel.text = notice.Replace("\\n", "\n");
	}
}
