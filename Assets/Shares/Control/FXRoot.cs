//=========================================
//作者:吴江
//日期：2015/8/31
//用途：特效速度，重放的逻辑管理器
//============================================




using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FXRoot : MonoBehaviour
{
    public bool isWorldMatrix = false;
    [HideInInspector]
    public ParticleSystem[] particlesystems = null;
    [HideInInspector]
    public Animator[] particheAnimators = null;
    [HideInInspector]
    public Transform[] childs;
    public float initdelaytime = 0f;
    public float initspeed = 1f;

    protected float delaytime = 0f;
    protected float speed = 1f;
    protected bool isPlaying = false;
    public float length = 0;

    protected IEnumerator lastIEnumerator;

    protected List<float> particleDelayList = new List<float>();


    protected List<TrailRenderer> trailRenderer = null;

    protected LightningEmitter lightningEmitter = null;

    /// <summary>
    /// 延迟时间
    /// </summary>
    public float Delaytime
    {
        get
        {
            return delaytime;
        }
        set
        {
            if (value >= 0 && value <= 10)
            {
                delaytime = value;
                initdelaytime = value;
            }
            else if (value < 0)
            {
                delaytime = 0f;
                initdelaytime = 0f;
            }
            else if (value > 10)
            {
                delaytime = 10f;
                initdelaytime = 0f;
            }
        }
    }

    /// <summary>
    /// 速度
    /// </summary>
    public float Speed
    {
        get
        {
            return speed;
        }
        set
        {
            if (value >= 0 && value <= 10)
            {
                SetAllSpeed(value);
            }
            else if (value < 0)
            {
                SetAllSpeed(0f);
            }
            else if (value > 10)
            {
                SetAllSpeed(10f);
            }
        }
    }

    /// <summary>
    /// 是否正在播放
    /// </summary>
    public bool IsPlaying
    {
        get
        {
            return isPlaying;
        }
    }

    public void InitLength()
    {
        if (particlesystems == null || particlesystems.Length == 0) particlesystems = GetComponentsInChildren<ParticleSystem>(true);
        if (particheAnimators == null || particheAnimators.Length == 0) particheAnimators = GetComponentsInChildren<Animator>(true);
        length = 0;
        for (int i = 0; i < particlesystems.Length; i++)
        {
            length = Mathf.Max(length, particlesystems[i].duration + particlesystems[i].startDelay);
        }
        for (int i = 0; i < particheAnimators.Length; i++)
        {
            if (particheAnimators[i].runtimeAnimatorController == null) continue;
            AnimationClip[] clips = particheAnimators[i].runtimeAnimatorController.animationClips;
            for (int j = 0; j < clips.Length; j++)
            {
                length = Mathf.Max(length, clips[j].length);
            }
        }
    }

    void Awake()
    {
        //初始化 对象引用
        int count = this.transform.childCount;
        childs = new Transform[count];
        //for (int i = count - 1; i >= 0; i--)
        //{
        //    childs[i] = this.transform.GetChild(i);
        //    if (childs[i].gameObject.activeInHierarchy == false)
        //    {
        //        childs[i].gameObject.SetActive(true);
        //    }
        //}

        lightningEmitter = this.gameObject.GetComponent<LightningEmitter>();

        if (particlesystems == null || particlesystems.Length == 0) particlesystems = GetComponentsInChildren<ParticleSystem>(true);
        if (particheAnimators == null || particheAnimators.Length == 0) particheAnimators = GetComponentsInChildren<Animator>(true);

        trailRenderer = this.gameObject.GetComponentsInChildrenFast<TrailRenderer>(true);

        mySerlizedID = CreateSerlizedID();
    }


    void Start()
    {
        //初始化 初始数据
        Delaytime = initdelaytime;
        Speed = initspeed;
    }
    #region 播放特效接口
    [ContextMenu("Play")]
    public void Play()
    {
        if (IsPlaying)
        {
            Stop();
        }
        if (delaytime > 0)
        {
            if (lastIEnumerator != null)
            {
                StopCoroutine(lastIEnumerator);
            }
            lastIEnumerator = EnableAllPartiche(delaytime);
            StartCoroutine(lastIEnumerator);
        }
        else
        {
            EnableAllPartiche();
        }
    }




    public void Play(float delay, float speed)
    {
        Delaytime = delay;
        Speed = speed;
        Play();
    }
    #endregion

    #region 停止特效
    [ContextMenu("Stop")]
    public void Stop()
    {
        //for (int i = 0; i < childs.Length; i++)
        //{
        //    if (childs[i] != null)
        //    {
        //        childs[i].gameObject.SetActive(false);
        //    }
        //}
        for (int i = 0; i < particlesystems.Length; i++)
        {
            if (particlesystems[i] != null)
            {
                particlesystems[i].Clear();
            }
        }
        if (lightningEmitter != null)
        {
            lightningEmitter.Target1 = null;
            lightningEmitter.Target2 = null;
        }
        isPlaying = false;
    }
    #endregion

    void OnEnable()
    {
        //for (int i = 0; i < childs.Length; i++)
        //{
        //    childs[i].gameObject.SetActive(false);
        //}
        if (trailRenderer != null && trailRenderer.Count > 0)
        {
            for (int i = 0; i < trailRenderer.Count; i++)
            {
                trailRenderer[i].Clear();
            }
        }
        curFxSerlizedIDList.Add(mySerlizedID);
    }

    void OnDisable()
    {
        curFxSerlizedIDList.Remove(mySerlizedID);
    }

    IEnumerator EnableAllPartiche(float _delayTime)
    {
        yield return new WaitForSeconds(_delayTime);
        EnableAllPartiche();
    }

    public void EnableAllPartiche()
    {
        //for (int i = 0; i < childs.Length; i++)
        //{
        //    childs[i].gameObject.SetActive(true);
        //}
        isPlaying = true;
    }


    void SetAllSpeed(float value)
    {
        if (particleDelayList.Count < particlesystems.Length)
        {
            particleDelayList.Clear();
            for (int i = 0; i < particlesystems.Length; i++)
            {
                particleDelayList.Add(particlesystems[i].startDelay);
            }
        }
        //处理partiche system 
        for (int i = 0; i < particlesystems.Length; i++)
        {
            if (particlesystems[i] != null)
            {
                particlesystems[i].playbackSpeed = value;
                value = Mathf.Max(0.00001f, value);
                particlesystems[i].startDelay = particleDelayList[i] / value;
            }
            else
            {
                Debug.LogWarning("某些特效组件已销毁，这不符合可重用特效");
            }
        }
        //处理 animation
        for (int i = 0; i < particheAnimators.Length; i++)
        {
            if (particheAnimators[i] != null)
            {
                particheAnimators[i].speed = value;
            }
            else
            {
                Debug.LogWarning("某些特效组件已销毁，这不符合可重用特效");
            }
        }
        speed = value;
        initspeed = value;
    }

#if UNITY_EDITOR
    void Update()
    {
        if (delaytime != initdelaytime)
        {
            Delaytime = initdelaytime;
        }

        if (speed != initspeed)
        {
            Speed = initspeed; ;
        }
        if (isWorldMatrix)
        {
            transform.rotation = Quaternion.identity;
        }
    }
#endif



    #region 特效上限
    protected int mySerlizedID = -1;

    protected static int curSerlizedID = 0;

    protected static int CreateSerlizedID()
    {
        if (curSerlizedID >= int.MaxValue)
        {
            curSerlizedID = 0;
        }
        return ++curSerlizedID;
    }

    protected static List<int> curFxSerlizedIDList = new List<int>();

    /// <summary>
    /// 当前的游戏内激活的特效总数 by吴江
    /// </summary>
    public static int CurFxCount
    {
        get
        {
            return curFxSerlizedIDList.Count;
        }
    }
    #endregion
}
