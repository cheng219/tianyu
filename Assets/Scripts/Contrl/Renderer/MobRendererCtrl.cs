///////////////////////////////////////////////////////////////////////////////
//作者：吴江
//日期：2015/7/20
//用途：怪物渲染控制器
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class MobRendererCtrl : SmartActorRendererCtrl
{



    protected new void Awake()
    {
        base.Awake();

        originalLayer = LayerMask.NameToLayer("Monster");
    }

    public override void Init()
    {
        base.Init();

    }




    public override void ResetMaterials()
    {
        base.ResetMaterials();

        for (int i = 0; i < rendererList.Count; ++i)
        {
            Renderer r = rendererList[i];
            // NOTE: it is possible we destroy the renderer
            if (r != null)
            {
                r.sharedMaterials = originalMaterialsList[i];
            }
            if (skinnedMeshRenderer != null)
            {
                skinnedMeshRenderer.sharedMaterials = originalMaterials;
            }
        }
    }


    public override void SetMaterial(Material _material)
    {
        base.SetMaterial(_material);

        for (int i = 0; i < rendererList.Count; ++i)
        {
            Renderer r = rendererList[i];

            if (r is ParticleRenderer ||
                 r is ParticleSystemRenderer)
                continue;


            Material[] matList = new Material[r.sharedMaterials.Length];
            for (int j = 0; j < matList.Length; ++j)
            {
                matList[j] = _material;
            }
            r.materials = matList;
        }
    }


    public override void SetLayer(int _layer)
    {
        base.SetLayer(_layer);

        foreach (Renderer r in rendererList)
        {
            if (r == null) continue;
            if (r is ParticleRenderer == false)
                r.gameObject.layer = _layer;
        }
    }


    public override void ResetLayer()
    {
        base.ResetLayer();

        foreach (Renderer r in rendererList)
        {
            if (r == null) continue;
            if (r is ParticleRenderer == false)
                r.gameObject.layer = originalLayer;
        }
    }
}
