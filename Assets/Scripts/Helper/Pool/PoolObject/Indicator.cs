//===============================================
//作者：吴江
//日期：2015/5/30
//用途：地面点击特效的对象（派生自对象池对象)
//===================================================



using UnityEngine;
using System.Collections;

public class Indicator : PoolObject
{
    /// <summary>
    /// 自动归还时间
    /// </summary>
    protected float duration = 0.5f;

    public override void OnSpawned()
    {
        if (!this.gameObject.activeSelf)
        {
            LoadUtil.SetActive(this.gameObject, true);
        }
        FXCtrl.RePlay(this.gameObject);
        base.OnSpawned();
        DontDestroyOnLoad(this);

        Invoke("ReturnBySelf", duration);
    }



    public override void OnDespawned()
    {
        FXCtrl.Stop(this.gameObject);
        base.OnDespawned();
    }

    /// <summary>
    /// 对象自主归还对象池
    /// </summary>
    public void ReturnBySelf()
    {
        if (GameCenter.spawner != null)
        {
            GameCenter.spawner.DespawnIndicator(this);
        }
    }

}
