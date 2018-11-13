//====================================================
//作者: 黄洪兴
//日期：2016/03/23
//用途：称号的数据层对象
//======================================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 称号系统服务端数据
/// </summary>
public class TitleServerData
{
    public int id;
    public bool isOwn;
	public  bool isPut;
	public  bool isForeve=false;
	public List<int> remainTime=new List<int>();
}
/// <summary>
/// 称号系统数据对象
/// </summary>
public class TitleInfo
{
    #region 服务端数据
    TitleServerData titleData;
    #endregion
    #region 静态配置数据
    TitleRef titleRef = null;
    public TitleRef TitleRef
    {
        get
        {
            if (titleRef != null) return titleRef;
            titleRef = ConfigMng.Instance.GetTitlesRef(titleData.id);
            return titleRef;
        }
    }
    #endregion

    #region 构造
	public TitleInfo(int id)
	{
		titleData = new TitleServerData();
		titleData.id = id;
		titleData.isOwn =false;
		titleData.isPut =false;
		titleData.isForeve = true;


	}



	public TitleInfo(st.net.NetBase.title_base_info_list info)
    {
        titleData = new TitleServerData();
		titleData.id = (int)info.title_id;
		titleData.isForeve=(int)info.own_state==1;
		titleData.isOwn =true;
		titleData.isPut =(int)info.put_state==1;
		if (info.time <= 0) {
			titleData.isForeve = true;
		} else {
			titleData.remainTime.Clear ();
			titleData.remainTime.Add ((int)Time.time);
			titleData.remainTime.Add ((int)info.time);
		}
    }
    #endregion

    #region 访问器
    /// <summary>
    /// 称号ID
    /// </summary>
    public int ID
    {
        get { return titleData.id; }
    }
    /// <summary>
    /// 是否获得
    /// </summary>
    public bool IsOwn
    {
		get { return titleData.isOwn; }
    }
	/// <summary>
	/// 是否为永久
	/// </summary>
	/// <value><c>true</c> if this instance is foreve; otherwise, <c>false</c>.</value>
	public bool IsForeve
	{
		get{ return titleData.isForeve; }

	}
	/// <summary>
	/// 是否穿戴
	/// </summary>
	/// <value><c>true</c> if this instance is put; otherwise, <c>false</c>.</value>
	public bool IsPut
	{
		get{ return titleData.isPut; }

	}
	/// <summary>
	/// 剩余时间
	/// </summary>
	/// <value>The remain time.</value>
	public List<int> RemainTime
	{
		get{ return titleData.remainTime; }

	}

    /// <summary>
    /// 名字
    /// </summary>
    public string Name
    {
		get { return TitleRef.name; }
    }
    /// <summary>
    /// 图片名字
    /// </summary>
    public string IconName
    {
        get { return TitleRef.icon; }
    }
	/// <summary>
	/// 称号奖励说明
	/// </summary>
	/// <value>The DES.</value>
	public string Des
	{
		get { return TitleRef.des; }
	}
	/// <summary>
	/// 获得途径说明
	/// </summary>
	/// <value>The way DES.</value>
	public string WayDes
	{
		get { return TitleRef.wayDes; }
	}

	/// <summary>
	/// 判断条件（1大于判断值2小于判断值3等于判断值）
	/// </summary>
	/// <value>The judge.</value>
    //public int Judge
    //{
    //    get { return TitleRef.judge; }
    //}

    ///// <summary>
    ///// 判断的值
    ///// </summary>
    ///// <value>The judge number.</value>
    //public int JudgeNum
    //{
    //    get { return TitleRef.judgeNum; }
    //}

	/// <summary>
	/// 条件不符合时是否移除（1移除，2不移除）
	/// </summary>
	/// <value>The judge number.</value>
	public int RemoveJudge
	{
		get { return TitleRef.removeJudge; }
	}


    /// <summary>
    /// 附加属性
    /// </summary>
    public List<ItemValue> Attribute
    {
        get
        {
            return TitleRef.attribute;
        }
    }


    /// <summary>
    /// 称号文本
    /// </summary>
    public string NameDes
    {
        get
        {
            return TitleRef.nameDes;
        }
    }

    #endregion
}
