//===============================================
//作者：吴江
//日期：2015/11/5
//用途：弹道的对象（派生自对象池对象)  
//===================================================


using UnityEngine;
using System.Collections;

/// <summary>
/// 弹道的对象（派生自对象池对象)   by 吴江
/// </summary>
public class AbilityBallisticCurve : PoolObject
{

    protected AbilityBallisticCurveInfo info = null;
    protected float starttime = 0;
    protected PoolEffecter effect = null;
    public bool needDespawed = false;
    protected float speed = 0;
    protected float addSpeed = 0;
    protected Transform targetTransform = null;
    protected float lockLastDis = 0;
    public Vector3 curStartPos = Vector3.zero;


    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void OnSpawned(AbilityBallisticCurveInfo _info)
    {
        info = _info;
        needDespawed = false;
        starttime = 0;
        if (info == null)
        {
            OnDespawned();
            return;
        }
        curStartPos = info.StartPos;
        speed = info.Speed;
        addSpeed = info.AddSpeed;
        lockLastDis = 0;
        if (info.IsLock && info.Target != null)
        {
            targetTransform = info.Target.HitPoint;
        }
        if (effect != null)
        {
            GameCenter.spawner.DespawnEffecter(effect);
        }
        GameCenter.spawner.SpawnEffecter(info.EffectName, -1, (x) =>
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
                this.transform.localEulerAngles = info.Direction;

                Vector3 now = info.StartPos + info.Direction * 10;
                Vector3 velocity = now - transform.position;
                curStartPos = info.StartPos;
                if (velocity != Vector3.zero)
                {
                    transform.forward = velocity;
                }
                starttime = Time.time;
                if (!this.gameObject.activeSelf)
                {
                    LoadUtil.SetActive(this.gameObject, true);
                }
            },false);
        base.OnSpawned();
        transform.position = _info.StartPos;
        GameCenter.soundMng.PlaySound(info.PlaySound, SoundMng.GetSceneSoundValue(transform, GameCenter.curMainPlayer.transform), false, true);

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
        if (!enabled) return;
        if (info == null)
        {
            ReturnBySelf();
            return;
        }
        if (starttime == 0)
            return;



        if (info.IsLock)
        {
            float t = Time.time - starttime;
            float curTotalDis = t * speed + 0.5f * addSpeed * t * t;
            float dis = curTotalDis - lockLastDis;
            lockLastDis = curTotalDis;

            if (needDespawed)
            {
                //DSSkillLevelRef refData = ConfigMng.Instance.GetDSSkillLevelRef(info.AbilityID, info.AbilityLv);
                //if (refData != null)
                //{
                //    FXCtrl fx = info.Target.GetComponent<FXCtrl>();
                //    if (fx != null)
                //    {
                //        fx.DoDefEffect(refData.effectDefRes, refData.defTime / 1000f, targetTransform.localScale);
                //    }
                //}
                ReturnBySelf();
                return;
            }
            if (info.Target == null)//如果目标已经消失
            {
                ReturnBySelf();
                return;
            }

            // Vector3 origin = transform.position;
            Vector3 diff = targetTransform.position - transform.position;
            if (diff.sqrMagnitude <= 0.5f * 0.5f)
            {
                RetrunToPool();
                //ReturnBySelf();
                return;
            }
            Vector3 velocity = diff.normalized * dis;
            //如果已经很近,则限制只飞到目标,并不穿透 by吴江
            diff = targetTransform.position - transform.position;
            if (velocity.sqrMagnitude > diff.sqrMagnitude)
            {
                velocity = diff;
            }
            if (velocity != Vector3.zero)
            {
                transform.forward = velocity;
                transform.Translate(velocity, Space.World);
            }
        }
        else
        {
            float t = Time.time - starttime;
            float dis = t * speed + 0.5f * addSpeed * t * t;

            if (needDespawed)
            {
                if (dis > info.MinDistance)
                {
                    ReturnBySelf();
                    return;
                }
            }
            if (dis > info.MaxDistance)
            {
                ReturnBySelf();
                return;
            }

            Vector3 now = ActorMoveFSM.LineCast(curStartPos + info.Direction * dis,true);
            now = now.SetY(now.y + info.FlyHight);
            Vector3 velocity = now - transform.position;
            if (velocity != Vector3.zero)
            {
                transform.forward = velocity;
                transform.Translate(velocity, Space.World);
            }
        }
    }

    protected void RetrunToPool()
    {
        GameCenter.sceneMng.C2C_OnDeleteAbilityBallisticLockCurve(info.InstanceID);
    }


    /// <summary>
    /// 对象自主归还对象池
    /// </summary>
    public void ReturnBySelf()
    {
        if (GameCenter.spawner != null)
        {
            if (info != null && info.FinishEffect != string.Empty && info.FinishEffect.Length > 0)
            {
                GameCenter.spawner.SpawnEffecter(info.FinishEffect, 2.0f, (x) =>
                    {
                        x.transform.position = this.transform.position;
                        x.transform.rotation = this.transform.rotation;
                    }, false);
            }
            GameCenter.spawner.DespawnAbilityBallisticCurve(this);
        }
    }
}
