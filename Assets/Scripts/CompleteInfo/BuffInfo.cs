//====================================
//作者：吴江
//日期：2015/9/8
//用途：buff数据层对象
//=====================================



using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// buff数据层对象 by吴江
/// </summary>
public class BuffInfo
{

    public BuffInfo(pt_scene_chg_buff_c008 _buffData)
    {
		startTime = Time.realtimeSinceStartup;
        serverData = _buffData;
    }


    /// <summary>
    /// 服务端数据
    /// </summary>
    protected pt_scene_chg_buff_c008 serverData;
    /// <summary>
    /// 客户端静态数据
    /// </summary>
    protected SkillBuffRef refData = null;
    /// <summary>
    /// 客户端静态数据
    /// </summary>
    protected SkillBuffRef RefData
    {
        get
        {
            if (refData == null || refData.id != serverData.buff_type)
            {
                refData = ConfigMng.Instance.GetSkillBuffRef((int)serverData.buff_type);
            }
            return refData;
        }
    }
    /// <summary>
    /// 开始时间
    /// </summary>
    protected float startTime;
    #region 访问器
    /// <summary>
    /// 施放对象的id
    /// </summary>
    public int ActorID
    {
        get { return (int)serverData.oid; }
    }

    /// <summary>
    /// buff类型ID
    /// </summary>
    public int BuffTypeID
    {
        get
        {
            return (int)serverData.buff_type;
        }
    }
    /// <summary>
    /// 是否无限时间的buff
    /// </summary>
    public bool IsUnTimeLimit
    {
        get
        {
            return serverData.buff_len <= 0;
        }
    }
    /// <summary>
    /// 开始时间
    /// </summary>
    public float StartTime
    {
        get
        {
            return startTime;
        }
    }

    ///// <summary>
    ///// 提升BUFF还是下降BUFF
    ///// </summary>
    //public string PerUpOrDown
    //{
    //    get
    //    {
    //        if(RefData == null){
    //            return string.Empty;
    //        }
    //        if(RefData.perUpOrDown == 1){
    //            return "Icon_tisheng";
    //        }else if(RefData.perUpOrDown == 2){
    //            return "Icon_xiajiang";
    //        }
    //        return string.Empty;
    //    }
    //}

    ///// <summary>
    ///// Buff描述
    ///// </summary>
    public string BuffDes
    {
        get
        {
            return RefData == null ? string.Empty : RefData.buffDes;
        }
    }

    /// <summary>
    /// 特效名称
    /// </summary>
    public string EffectName
    {
        get
        {
            return RefData == null ? string.Empty : RefData.effect_res;
        }
    }

	public BuffMapCleanType MapCleanType
	{
		get
		{
			return RefData == null ? BuffMapCleanType.NONE : RefData.mapCleanType;
		}
	}

    /// <summary>
    /// 动作
    /// </summary>
    public string AnimName
    {
        get
        {
            return RefData == null ? string.Empty : RefData.buffAction;
        }
    }

    /// <summary>
    /// buff属性类型
    /// </summary>
    public BuffType Sort
    {
        get
        {
            return RefData == null ? BuffType.NONE : RefData.sort;
        }
    }


    /// <summary>
    /// buff控制类型
    /// </summary>
    public BuffControlSortType ContrlType
    {
        get
        {
            return RefData == null ? BuffControlSortType.NONE : RefData.controlSort;
        }
    }

    /// <summary>
    /// 值
    /// </summary>
    public int Value
    {
        get
        {
            return (int)(serverData.buff_power * serverData.buff_mix_lev);
        }
    }

    /// <summary>
    /// 层数
    /// </summary>
    public int Count
    {
        get
        {
            return (int)serverData.buff_mix_lev;
        }
    }
    /// <summary>
    /// 客户端是否显示
    /// </summary>
    public bool ClientShow
    {
        get
        {
            return RefData == null ? false : RefData.bshow == 1;
        }
    }

    /// <summary>
    /// 显示时间
    /// </summary>
    public float HoldTime
    {
        get
        {
            return serverData.buff_len;
        }
    }
    /// <summary>
    /// 剩余时间
    /// </summary>
    public int RestSeconds
    {
        get
        {
			return Mathf.Max(0, (int)(serverData.buff_len/1000 - (Time.realtimeSinceStartup - startTime)));
        }
    }

    /// <summary>
    /// 图标资源名称
    /// </summary>
    public string IconName
    {
        get
        {
            return RefData == null ? string.Empty : RefData.icon;
        }
    }
    /// <summary>
    /// 动作优先级，级别越高越优先显示  by吴江
    /// </summary>
    public int AnimLevel
    {
        get
        {
            return RefData == null ? 0 : RefData.actionLevel;
        }
    }

    /// <summary>
    /// 换装Buff的模型替换索引
    /// </summary>
    public int ChangeModelKey
    {
        get
        {
            return RefData == null ? -1 : RefData.buff_model;
        }
    }

    /// <summary>
    /// 延迟时间
    /// </summary>
    public int DelayTime
    {
        get
        {
            return RefData == null ? -1 : RefData.delaytime;
        }
    }
	/// <summary>
	/// 增益还是减益buff
	/// </summary>
	/// <value>The type of the current buff attr.</value>
	public BuffAttrType CurBuffAttrType
	{
		get
		{
			return RefData == null ? BuffAttrType.NONE : (BuffAttrType)RefData.bdamage;
		}
	}

    /// <summary>
    /// 是否增益
    /// </summary>
    public bool Positive
    {
        get
        {
            return RefData == null ? false : RefData.bdamage !=1;
        }
    }
    /// <summary>
    /// 属性值
    /// </summary>
    public ActorPropertyTag Data1
    {
        get
        {
            return RefData == null ? ActorPropertyTag.TOTAL : RefData.data1;
        }
    }

    /// <summary>
    /// 属性值
    /// </summary>
    public int Data2
    {
        get
        {
            return RefData == null ? -1 : RefData.data2;
        }
    }

    /// <summary>
    /// 获得时的音效 by吴江
    /// </summary>
    public string AddSound
    {
        get
        {
            return RefData == null ? string.Empty : RefData.soundBuffRes;
        }
    }
    #endregion



}


