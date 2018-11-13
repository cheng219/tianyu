//=====================================
//作者：吴江
//日期：2015/5/30
//用途：对象池对象基类
//====================================



using UnityEngine;
using System.Collections;

public class PoolObject : MonoBehaviour {

    protected bool beSpawnedNow = false;

    /// <summary>
    /// 最后被使用的时间   PS：归还到对象池的时间 by 吴江
    /// </summary>
    [System.NonSerialized]
    public float lastBeUsedTime = 0;

    /// <summary>
    /// 残留时间
    /// </summary>
    [System.NonSerialized]
    public float residueTime = 5.0f;


    public bool NeedDelete()
    {
        if (!beSpawnedNow && Time.time - lastBeUsedTime >= residueTime)
        {
            return true;
        }
        return false;
    }


    public virtual void OnSpawned()
    {
        beSpawnedNow = true;
        if (GetComponent<Animation>() != null)
        {
            GetComponent<Animation>().enabled = true;
            GetComponent<Animation>().Play();
        }
        enabled = true;
        this.gameObject.SetActive(true);

    }


    public virtual void OnDespawned()
    {
        beSpawnedNow = false;
        if (GetComponent<Animation>() != null)
        {
            GetComponent<Animation>().Rewind();
            GetComponent<Animation>().enabled = false;
        }
        enabled = false;
        this.gameObject.SetActive(false);
    }


}
