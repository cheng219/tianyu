//==================================
//作者：吴江
//日期：2015/11/5
//用途：技能弹道的完整数据
//================================


using UnityEngine;
using System.Collections;

 ///<summary>
 ///技能弹道的完整数据 by吴江
 ///</summary>
public class AbilityBallisticCurveInfo {

    /// <summary>
    /// 服务端数据
    /// </summary>
    protected st.net.NetBase.scene_arrow data;

    protected AbilityInstance lockData;

    protected int configID = -1;

    protected int instanceID = -1;
    /// <summary>
    /// 锁定类弹道的目标.非锁定弹道则该值为空
    /// </summary>
    protected Actor target;
    /// <summary>
    /// 锁定类弹道的目标.非锁定弹道则该值为空
    /// </summary>
    public Actor Target
    {
        get
        {
            return target;
        }
    }

    protected bool isLock = false;
    /// <summary>
    /// 是否为锁定弹道
    /// </summary>
    public bool IsLock
    {
        get
        {
            return isLock;
        }
    }
    /// <summary>
    /// 客户端静态数据
    /// </summary>
    protected ArrowRef refData = null;


    /// <summary>
    /// 起始点
    /// </summary>
    protected Vector3 startPos = Vector3.zero;
    /// <summary>
    /// 朝向
    /// </summary>
    protected Vector3 direction = Vector3.zero;

    public ArrowRef RefData
    {
        get
        {
            if (refData == null || refData.arrowId != configID)
            {
                refData = ConfigMng.Instance.GetArrowRef(configID);
            }
            return refData;
        }
    }

    public AbilityBallisticCurveInfo(st.net.NetBase.scene_arrow _data)
    {
        data = _data;
        configID = (int)_data.type;
        instanceID = (int)_data.aid;
        isLock = false;
        startPos = new Vector3(data.x, data.y, data.z);
        direction = Quaternion.Euler(0.0f, data.dir, 0.0f) * new Vector3(0,0,1);
    }



    public AbilityBallisticCurveInfo(AbilityInstance _data)
    {
        lockData = _data;
        configID = (int)_data.ArrowID;
        instanceID = CreateLockInstanceID();
        target = _data.TargetActor;
        isLock = true;
        if (target != null)
        {
            direction = (target.transform.position - startPos).normalized;
        }
    }



    #region 访问器

    public int InstanceID
    {
        get
        {
            return instanceID;
        }
    }

    public int ConfigID
    {
        get
        {
            return configID;
        }
    }
    /// <summary>
    /// 起始点
    /// </summary>
    public Vector3 StartPos
    {
        get
        {
            if (isLock)
            {
                SmartActor user = lockData.UserActor;
                if (user != null)
                {
                    if (user.AttackPoint != null)
                    {
                        startPos = user.AttackPoint.position;
                    }
                    else
                    {
                        startPos = user.transform.position;
                    }
                }
            }
            return startPos;
        }
    }
    /// <summary>
    /// 对应的技能id
    /// </summary>
    public int AbilityID
    {
        get
        {
            return lockData == null ? -1 : lockData.AbilityID;
        }
    }

    /// <summary>
    /// 对应的技能等级
    /// </summary>
    //public int AbilityLv
    //{
    //    get
    //    {
    //        return lockData == null ? -1 : lockData.AbilityID;
    //    }
    //}

    /// <summary>
    /// 朝向
    /// </summary>
    public Vector3 Direction
    {
        get
        {
            return direction;
        }
    }

    /// <summary>
    /// 结束特效
    /// </summary>
    public string FinishEffect
    {
        get
        {
            return RefData == null ? string.Empty : RefData.finisheffect;
        }
    }

    /// <summary>
    /// 弹道飞行高度 by吴江
    /// </summary>
    public float FlyHight
    {
        get
        {
            return RefData == null ? 0.0f : RefData.arrowFlyHeight;
        }
    }

    /// <summary>
    /// 音效
    /// </summary>
    public string PlaySound
    {
        get
        {
            return RefData == null ? string.Empty : RefData.soundArrowRes;
        }
    }

    /// <summary>
    /// 速度
    /// </summary>
    public float Speed
    {
        get
        {
            return RefData != null ? RefData.speed : 0;
        }
    }
    public float AddSpeed
    {
        get
        {
            return RefData != null ? RefData.add_speed : 0;
        }
    }
    /// <summary>
    /// 最大距离
    /// </summary>
    public float MaxDistance
    {
        get
        {
            return RefData != null ? RefData.max_dis : 0;
        }
    }
    /// <summary>
    /// 最小距离
    /// </summary>
    public float MinDistance
    {
        get
        {
            return RefData != null ? RefData.min_dis : 0;
        }
    }

    /// <summary>
    /// 特效资源名称
    /// </summary>
    public string EffectName
    {
        get
        {
            return RefData != null ? RefData.effect_res : string.Empty;
        }
    }
    /// <summary>
    /// 音效名称
    /// </summary>
    public string SoundName
    {
        get
        {
            return RefData != null ? RefData.effect_res : string.Empty;
        }
    }

    /// <summary>
    /// 弹道到达 ，可是开始表现伤害了 by吴江
    /// </summary>
    public void OnReached()
    {
        if (!IsLock) return;
        lockData.ArrowFinished = true;
    }


    protected static int lockInstanceID = -1;
    protected static int CreateLockInstanceID()
    {
        if (lockInstanceID < 0)
        {
            lockInstanceID = 1000000000;
        }
        return ++lockInstanceID;
    }

    #endregion
}
