///////////////////////////////////////////////////////////////////////////////
//作者：吴江
//日期：2015/5/15
//用途：场景物品渲染控制器
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class SceneObjRendererCtrl : RendererCtrl
{

    Renderer[] rendererList = null;
    List<Material[]> originalMaterialsList = new List<Material[]>();



    protected new void Awake()
    {
        base.Awake();

        rendererList = GetComponentsInChildren<Renderer>();
        originalLayer = this.gameObject.layer;

        foreach (Renderer r in rendererList)
        {
            originalMaterialsList.Add(r.sharedMaterials);
        }
    }



    public override bool Show(bool _show, bool _force = false)
    {
        if (base.Show(_show, _force))
        {
            foreach (Renderer r in rendererList)
                r.enabled = _show;
            return true;
        }
        return false;
    }



    public override void SetLayer(int _layer)
    {
        int outlineLayer = LayerMask.NameToLayer("Outline");
        foreach (Renderer r in rendererList)
        {
            if (outlineLayer == _layer && r is ParticleRenderer)
                continue;

            r.gameObject.layer = _layer;
        }
    }


    public override void ResetLayer()
    {
        foreach (Renderer r in rendererList)
        {
            r.gameObject.layer = originalLayer;
        }
    }


    public override void SetSkinnedMeshShader(Shader _shader)
    {
        foreach (Renderer r in rendererList)
        {
            foreach (Material material in r.materials)
            {
                material.shader = _shader;
            }
        }
    }


    public override void ResetMaterials()
    {
        for (int i = 0; i < rendererList.Length; ++i)
        {
            Renderer r = rendererList[i];
            r.sharedMaterials = originalMaterialsList[i];
        }
    }


    public override void SetOutlineParameters(Color _color, float _width)
    {
        foreach (Renderer r in rendererList)
        {
            if (r is ParticleRenderer)
                continue;

            foreach (Material material in r.materials)
            {
                material.SetColor("_OutlineColor", _color);
                material.SetFloat("_OutlineWidth", _width);
            }
        }
    }
}
