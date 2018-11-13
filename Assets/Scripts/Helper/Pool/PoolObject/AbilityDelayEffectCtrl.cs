//======================================
//作者:吴江
//日期:2016//
//用途:技能前台延迟特效控制器
//======================================

using UnityEngine;
using System.Collections;

/// <summary>
/// 技能前台延迟特效控制器 by 吴江
/// </summary>
public class AbilityDelayEffectCtrl : PoolObject
{
    protected string effectName = string.Empty;
    protected float delayTime = 0;
    protected Vector3 worldPos = Vector3.zero;
    protected float duration = 0;
    protected float startTime = 0;
    protected PoolEffecter effect = null;
    protected bool hasInitShow = false;

    void Awake()
    {
    }

    public void OnSpawned(string _effectName, float _delayTime,float _duration, Vector3 _pos)
    {
        hasInitShow = false;
        effectName = _effectName;
        delayTime = _delayTime;
        duration = _duration;
        worldPos = _pos;
        startTime = Time.time;
        if (effect != null)
        {
            GameCenter.spawner.DespawnEffecter(effect);
        }
        base.OnSpawned();
        DontDestroyOnLoad(this);
    }

    public override void OnDespawned()
    {
        if (effect != null)
        {
            GameCenter.spawner.DespawnEffecter(effect);
            effect = null;
        }
        base.OnDespawned();
    }

    void Update()
    {
        float timeDiff = Time.time - startTime;
        if (timeDiff >= delayTime)
        {
            if (timeDiff > delayTime + duration)
            {
                ReturnBySelf();
            }
            else if (!hasInitShow)
            {
                hasInitShow = true;
                GameCenter.spawner.SpawnEffecter(effectName, duration, (x) =>
                {
                    //清理原特效,主要因为两次异步可能导致多个特效加载很慢的特效同时存在 by吴江
                    if (effect != null)
                    {
                        GameCenter.spawner.DespawnEffecter(effect);
                    }
                    int count = this.transform.childCount;
                    if (count > 0)
                    {
                        PoolEffecter[] p = new PoolEffecter[count];
                        for (int i = 0; i < count; i++)
                        {
                            effect = this.transform.GetChild(i).gameObject.GetComponent<PoolEffecter>();
                            p[i] = effect;
                        }
                        foreach (var item in p)
                        {
                            GameCenter.spawner.DespawnEffecter(item);
                        }
                    }
                    count = this.transform.childCount;
                    if (count > 0)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            Destroy(this.transform.GetChild(i).gameObject);
                        }
                    }
                    effect = x;
                    effect.transform.parent = this.transform;
                    effect.transform.localPosition = Vector3.zero;
                    effect.transform.localEulerAngles = Vector3.zero;
                },true);
            }
        }
    }

    /// <summary>
    /// 对象自主归还对象池
    /// </summary>
    public void ReturnBySelf()
    {
        if (GameCenter.spawner != null)
        {
            if (effect != null)
            {
                GameCenter.spawner.DespawnEffecter(effect);
            }
            GameCenter.spawner.DespawnAbilityDelayEffectCtrl(this);
        }
    }

}
