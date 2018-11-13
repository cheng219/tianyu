///////////////////////////////////////////////////////////////////////////////
//作者：吴江
//日期：2015/10/30
//用途：坐骑渲染控制器
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// 坐骑渲染控制器
/// </summary>
public class MountRendererCtrl : ActorRendererCtrl
{
    /// <summary>
    /// 特效控制器
    /// </summary>
    protected FXCtrl fxCtrl;

    protected MountInfo actorInfo;

    public void Init(MountInfo _actorInfo)
    {
        actorInfo = _actorInfo;
    }




    protected new void Awake()
    {
        Renderer[] allRendererList = GetComponentsInChildren<Renderer>();

        for (int i = 0; i < allRendererList.Length; ++i)
        {
            Renderer r = allRendererList[i];
            if (!(r is ParticleRenderer) &&
                 !(r is ParticleSystemRenderer))
            {
                rendererList.Add(r);
            }
        }
        for (int i = 0; i < rendererList.Count; i++)
        {
            if (rendererList[i] == null) continue;
            if (rendererList[i] is ParticleRenderer == false)
            {
                originalLayer = rendererList[i].gameObject.layer;
            }
            originalMaterialsList.Add(rendererList[i].sharedMaterials);
        }

        for (int i = 0; i < rendererList.Count; ++i)
        {
            Renderer r = rendererList[i];
            r.material.shader = Shader.Find(r.material.shader.name);
            r.material.color = Color.white;
        }



    }

    public override bool Show(bool _show, bool _force = false)
    {
        if (base.Show(_show, _force))
        {
            Renderer[] allRendererList = this.gameObject.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < allRendererList.Length; ++i)
            {
                allRendererList[i].enabled = _show;
            }
            if (fxCtrl != null)
            {
                fxCtrl.ShowBoneEffect(_show);
            }
            return true;
        }
        return false;
    }


}
