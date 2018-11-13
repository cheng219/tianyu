//==================================
//作者：易睿
//日期：2016/06/07
//用途：应答号管理类
//=================================

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MsgLoackingMng {

    /// <summary>
    /// 问答号等待列表 by吴江
    /// </summary>
    protected List<int> serializeList = new List<int>();

    /// <summary>
    /// 强制问答号等待列表 by吴江
    /// </summary>
    protected List<int> forceSerializeList = new List<int>();

    public System.Action OnUpdateCmdDictionary;
	#region 构造
	/// <summary>
	/// 构造单例
	/// </summary>
	public static MsgLoackingMng CreateNew()
	{
		if (GameCenter.msgLoackingMng == null)
		{
			MsgLoackingMng panelLoadingMng = new MsgLoackingMng();
			panelLoadingMng.Init();
			return panelLoadingMng;
		}
		else
		{
			GameCenter.msgLoackingMng.UnRegist();
			GameCenter.msgLoackingMng.Init();
			return GameCenter.msgLoackingMng;
		}
	}
	
	protected void Init()
	{
		//构建记录表
        serializeList.Clear();
	}
	
	protected void UnRegist()
	{

	}
	
	#endregion
	/// <summary>
	/// cmd协议号，(请求压入true，相应压入false)
	/// </summary>
    public void UpdateSerializeList(int _serializeID, bool _putIn,bool _force = false)
    {
        if (_force)
        {
            if (_putIn)
            {
                if (!forceSerializeList.Contains(_serializeID))
                {
                    forceSerializeList.Add(_serializeID);
                }
            }
            else if (forceSerializeList.Contains(_serializeID))
            {
                forceSerializeList.Remove(_serializeID);
            }
        }
        else
        {
            if (_putIn)
            {
                if (!serializeList.Contains(_serializeID))
                {
                    serializeList.Add(_serializeID);
                }
            }
            else if (serializeList.Contains(_serializeID))
            {
                serializeList.Remove(_serializeID);
            }
        }
        if (forceSerializeList.Count == 0 && serializeList.Count == 0)
        {
            NetMsgMng.ResetSerializeID();
        }
        if (OnUpdateCmdDictionary != null)
        {
            OnUpdateCmdDictionary();
        }
	}

    //protected void OnUpdateCmdDictionary()
    //{
    //    if (GameCenter.msgLoackingMng.HasSerializeWaiting)
    //    {
    //        GameCenter.uIMng.GenGUI(GUIType.PANELLOADING, true);
    //    }
    //    else
    //    {
    //        GameCenter.uIMng.ReleaseGUI(GUIType.PANELLOADING);
    //    }
    //}

    /// <summary>
    /// 是否有问答号等待协议  by吴江
    /// </summary>
    public bool HasSerializeWaiting
    {
        get
        {
            return serializeList.Count > 0 || forceSerializeList.Count > 0;
        }
    }
    /// <summary>
    /// 是否有强制等待
    /// </summary>
    public bool HasForceSerializeWaiting
    {
        get
        {
            return forceSerializeList.Count > 0;
        }
    }

    /// <summary>
    /// 清空问答号等待列表  by吴江
    /// </summary>
    public void CleanSerializeList()
    {
        serializeList.Clear();
        if (serializeList.Count == 0)
        {
            NetMsgMng.ResetSerializeID();
        }
        if (OnUpdateCmdDictionary != null)
        {
            OnUpdateCmdDictionary();
        }
    }
}
