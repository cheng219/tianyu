//========================================
//作者：吴江
//日期：2015/6/3
//用途：对象脚下光圈的对象池对象
//========================================



using UnityEngine;
using System.Collections;

public class RingEffecter : PoolObject
{

    public void OnSpawned(Color _color)
    {
        if (!this.gameObject.activeSelf)
        {
            LoadUtil.SetActive(this.gameObject, true);
        }
        MeshRenderer rd = GetComponentInChildren<MeshRenderer>();
        if (rd != null)
        {
            Material[] mat = rd.materials;
            foreach (Material item in mat)
            {
                if (item.HasProperty("_TintColor"))
                    item.SetColor("_TintColor", _color);
                else
                    item.color = _color;
            }
        }
        FXCtrl.RePlay(this.gameObject);
        base.OnSpawned();
		if(!GameHelper.haveDontDestroyRingEffect)
		{
            //Debug.Log("GameHelper.haveDontDestroyRingEffect = :"+ GameHelper.haveDontDestroyRingEffect);
        	DontDestroyOnLoad(this);
			GameHelper.haveDontDestroyRingEffect = true;//只需要执行一次就可以了
		}
    }



    public override void OnDespawned()
    {
        //Debug.Log("OnDespawned");
        FXCtrl.Stop(this.gameObject);
        //GameObject.DestroyImmediate(this.gameObject);
        base.OnDespawned();
    }
}
