///////////////////////////////////////////////////////////////////////////////
//作者：吴江
//日期：2015/5/23
//用途：npc的渲染控制器
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class NPCRendererCtrl : SmartActorRendererCtrl
{

    List<Renderer> outlineRenderers = new List<Renderer>();
    List<Renderer> renderers = new List<Renderer>();

    List<int> originalLayers = new List<int>();



    protected new void Awake()
    {
        base.Awake();


        Renderer[] myRenderers = GetComponentsInChildren<Renderer>();
        renderers.AddRange(myRenderers);


        for (int i = 0; i < myRenderers.Length; ++i)
        {
            Renderer r = myRenderers[i];
            if (!(r is ParticleRenderer) &&
                 !(r is ParticleSystemRenderer))
            {
                outlineRenderers.Add(r);
            }
        }

    }


    public override bool Show(bool _show, bool _force = false)
    {
        if (base.Show(_show, _force))
        {
            foreach (Renderer r in renderers)
            {
                r.enabled = _show;
            }
            if (GetComponent<Animation>() != null) GetComponent<Animation>().enabled = _show;
            return true;
        }
        return false;
    }

    public override void SetLayer(int _layer)
    {
        base.SetLayer(_layer);
        for (int i = 0; i < outlineRenderers.Count; ++i)
        {
			originalLayers.Add(outlineRenderers[i].gameObject.layer);
            outlineRenderers[i].gameObject.layer = _layer;
        }
    }



    public override void ResetLayer()
    {
        base.ResetLayer();
        for (int i = 0; i < outlineRenderers.Count; ++i)
        {
			if(outlineRenderers[i] != null)
            	outlineRenderers[i].gameObject.layer = originalLayers[i];
        }
    }

}
