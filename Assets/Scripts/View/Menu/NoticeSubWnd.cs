//====================================
//作者：黄洪兴
//日期：2016/6/12
//用途：游戏公告窗口
//=====================================


using UnityEngine;
using System.Collections;

public class NoticeSubWnd : SubWnd
{

    public UILabel notice;





    #region UNITY
    void Awake()
    {
        type = SubGUIType.NOTICESUBWND;
    }
	protected override void OnOpen ()
	{
		base.OnOpen ();
		Refresh();
	}
    protected override void HandEvent(bool _bind)
    {
        base.HandEvent(_bind);
        if (_bind)
        {
			GameCenter.descriptionMng.OnUpdateLoginNoticeEvent += Refresh;
        }
        else
        {
			GameCenter.descriptionMng.OnUpdateLoginNoticeEvent -= Refresh;
        }
    }
    #endregion

    public void Refresh()
    {
        if (notice != null)
		{
			if(!string.IsNullOrEmpty(GameCenter.descriptionMng.NoticeDes))
            	notice.text = GameCenter.descriptionMng.NoticeDes.Replace("\\n", "\n"); 
			else
				GameCenter.instance.SetNotice();
		}
    }
	void Refresh(string _notice)
	{
		if (notice != null)
		{
			notice.text = _notice.Replace("\\n", "\n"); 
		}
	}



  



    #region 控件事件






    #endregion
}
