//======================================
//作者:吴江
//日期:2016/1/4
//用途:场景控制器
//======================================


using UnityEngine;
using System.Collections;

public class SceneRoot : MonoBehaviour {

    public Light directionalLight;
	public SceneEffectHelper[] effecthelpers;//场景特效helper

    protected static SceneRoot sceneRootInstance = null;
    public static SceneRoot GetInstance()
    {
        return sceneRootInstance;
    }


    void Awake()
    {
        sceneRootInstance = this;
        if (directionalLight == null)
        {
            directionalLight = transform.GetComponentInChildrenFast<Light>();
        }
		if (effecthelpers == null || effecthelpers.Length<1) {
			effecthelpers = transform.GetComponentsInChildren<SceneEffectHelper> ();
		}
    }

    public void DirectionalLightActive(bool _active)
    {
        if (directionalLight != null)
        {
            directionalLight.gameObject.SetActive(_active);
        }
    }

    public void InitDirectionalLightLayer()
    {
        if (directionalLight != null)
        {
            directionalLight.cullingMask = LayerMng.sceneDirLightMask;
        }
    }

}
