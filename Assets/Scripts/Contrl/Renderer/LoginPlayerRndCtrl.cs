///////////////////////////////////////////////////////////////////////////////
//作者：吴江
//日期：2015/5/16
//用途：登录阶段人物渲染控制器
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class LoginPlayerRndCtrl : ActorRendererCtrl {

  //  protected PlayerBase player = null;


    protected new void Awake () {
        base.Awake();

        originalLayer = LayerMask.NameToLayer("NGUI3D");

        Renderer[] rendererList = GetComponentsInChildren<Renderer>();
        foreach ( Renderer r in rendererList ) {
            if ( r is ParticleRenderer == false )
                r.gameObject.layer = originalLayer;
        }
    }



    public override void Init () {
        base.Init();
     //   player = GetComponent<PlayerBase>();
    } 




  

    public void ActivateCollider(bool _activate)
    {
        //Collider collider =  player.GetComponentInChildrenFast<Collider>();
        //if (collider != null)
        //{
        //    collider.enabled = _activate;
        //}
    }


}
