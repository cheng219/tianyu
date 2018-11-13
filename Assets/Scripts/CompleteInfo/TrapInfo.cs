//====================================================
//作者：吴江
//日期：2015/11/9
//用途：陷阱的数据层对象
//======================================================




using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 陷阱的数据层对象 by吴江
/// </summary>
public class TrapInfo
{

    #region 服务端数据 by吴江
    protected st.net.NetBase.scene_trap serverData;

    protected float startTime = 0;
    #endregion

    #region 静态配置数据 by吴江
    TrapRef refData = null;
    protected TrapRef RefData
    {
        get
        {
            if(refData == null || refData.trapId != serverData.type)
            {
                refData = ConfigMng.Instance.GetTrapRef((int)serverData.type);
            }
            return refData;
        }
    }
    #endregion


    #region 构造 by吴江
    public TrapInfo(st.net.NetBase.scene_trap _serverData)
    {
        serverData = _serverData;
		startTime = Time.time;
    }
	
    #endregion

    #region 访问器
    public int InstanceID
    {
        get { return (int)serverData.did; }
    }

    /// <summary>
    /// 坐标x
    /// </summary>
    public float ServerPosX
    {
        get { return serverData.x; }
    }
    /// <summary>
    /// 坐标y
    /// </summary>
    public float ServerPosY
    {
        get { return serverData.z; }
    }

    /// <summary>
    /// 高度
    /// </summary>
    public float Hight
    {
        get
        {
            return serverData.y;
        }
    }

    /// <summary>
    /// 朝向
    /// </summary>
    public float Dir
    {
        get { return serverData.dir; }
    }

    /// <summary>
    /// 特效名称
    /// </summary>
    public string EffectName
    {
        get { return RefData == null ? string.Empty : RefData.effectRes; }
    }
    /// <summary>
    /// 是否已经结束
    /// </summary>
    public bool IsDead
    {
        get
        {
            if (RefData == null) return true;
            return Time.time - startTime >= RefData.trapAllTime;
        }
    }

    /// <summary>
    /// 音效
    /// </summary>
    public string PlaySound
    {
        get { return RefData == null ? string.Empty : RefData.soundTrapRes; }
    }



    #endregion



}


