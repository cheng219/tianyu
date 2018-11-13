//==========================
//作者：鲁家旗
//日期：2016/4/25
//用途：说明系统管理类
//==========================
using UnityEngine;
using System.Collections;

public class DescriptionMng
{
    /// <summary>
    /// 点击规则说明后抛出事件
    /// </summary>
    public System.Action<DescriptionRef> OnDescription;

	public System.Action<string> OnUpdateLoginNoticeEvent;


    private string noticeDes;
    public string NoticeDes
    {
        get
        {
            return noticeDes;
        }
    }

    #region 构造
    public static DescriptionMng CreateNew()
    {
        if (GameCenter.descriptionMng == null)
        {
            DescriptionMng descriptionMng = new DescriptionMng();
            return descriptionMng;
        }
        else
        {
            return GameCenter.descriptionMng;
        }
    }
    #endregion
    /// <summary>
    /// 通过ID拿取数据
    /// </summary>
    /// <param name="_id"></param>
    public void OpenDes(int _id)
    {
        DescriptionRef description = ConfigMng.Instance.GetDescriptionRef(_id);
        if (OnDescription != null)
            OnDescription(description);
    }
	/// <summary>
	/// 显示登录公告
	/// </summary>
	public void ShowLoginNotice(string loginNotice)
	{
        noticeDes = loginNotice;
		GameCenter.uIMng.GenGUI(GUIType.DESCRIPTION,true);
		if (OnUpdateLoginNoticeEvent != null)
			OnUpdateLoginNoticeEvent(loginNotice);
	}
	/// <summary>
	/// 设置公告内容
	/// </summary>
	public void SetNotice(string loginNotice)
	{
		noticeDes = loginNotice;
		if(OnUpdateLoginNoticeEvent != null)
			OnUpdateLoginNoticeEvent(loginNotice);
	}
}
