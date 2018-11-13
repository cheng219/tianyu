//===============================================
//作者：吴江
//日期：2015/5/31
//用途：特效的对象（派生自对象池对象)  
//===================================================






using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 特效的对象（派生自对象池对象)   by吴江
/// </summary>
public class PoolEffecter : PoolObject
{
    protected float speed = 1.0f;

    public void OnSpawned(float _duration, bool _matchingSpeed)
    {
        if (!this.gameObject.activeSelf)
        {
            LoadUtil.SetActive(this.gameObject, true);
        }
        if (_matchingSpeed)
        {
            FXCtrl.RePlayMatchingSpeed(this.gameObject, _duration);
        }
        else
        {
            FXCtrl.RePlay(this.gameObject);
        }
        base.OnSpawned();
        if (_duration > 0)
        {
            Invoke("ReturnBySelf", _duration);
        }
    }


    public override void OnDespawned()
    {
        FXCtrl.Stop(this.gameObject);
        base.OnDespawned();
    }


    public void SetSpeed(float _speed)
    {
        speed = _speed;
    }

    /// <summary>
    /// 对象自主归还对象池
    /// </summary>
    public void ReturnBySelf()
    {
        if (beSpawnedNow && GameCenter.spawner != null)
        {
            GameCenter.spawner.DespawnEffecter(this);
        }
    }




}
