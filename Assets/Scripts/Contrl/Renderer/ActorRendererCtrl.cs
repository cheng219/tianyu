///////////////////////////////////////////////////////////////////////////////
//作者：吴江
//日期：2015/5/16
//用途：活动物体的渲染控制
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// 活动物体的渲染控制 by吴江
/// </summary>
public class ActorRendererCtrl : RendererCtrl
{
    #region 数据
    protected SkinnedMeshRenderer skinnedMeshRenderer = null;
    protected bool combinedMeshDirty = false;
    protected bool ignoreCombine = false;


    protected MeshHelper.SkinnedMeshInfo rawMeshInfo = new MeshHelper.SkinnedMeshInfo();
    protected MeshHelper.SkinnedMeshInfo combinedMeshInfo = new MeshHelper.SkinnedMeshInfo();

    protected List<Renderer> rendererList = new List<Renderer>();

    protected List<Material[]> originalMaterialsList = new List<Material[]>();

    public System.Action OnCombineSuceess;
    #endregion

    #region Unity
    protected new void Awake()
    {
        base.Awake();

        List<SkinnedMeshRenderer> skrs = gameObject.GetComponentsInChildrenFast<SkinnedMeshRenderer>();
        if (skrs != null)
        {
            for (int i = 0; i < skrs.Count; i++)
            {
                if (skrs[i].material != null && skrs[i].material.shader)
                {
                    skrs[i].material.shader = Shader.Find(skrs[i].material.shader.name);
                }
            }
        }


        skinnedMeshRenderer = gameObject.GetComponent<SkinnedMeshRenderer>();
        if (skinnedMeshRenderer == null)
        {
            MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
            if (mr == null)
            {
                skinnedMeshRenderer = this.gameObject.AddComponent<SkinnedMeshRenderer>();
                skinnedMeshRenderer.material = null;
                skinnedMeshRenderer.sharedMaterial = null;
            }
        }

        if (skinnedMeshRenderer)
        {
            rawMeshInfo.mesh = skinnedMeshRenderer.sharedMesh;
            rawMeshInfo.materials = skinnedMeshRenderer.sharedMaterials;
            rawMeshInfo.bones = skinnedMeshRenderer.bones;

            combinedMeshInfo.mesh = skinnedMeshRenderer.sharedMesh;
            combinedMeshInfo.materials = skinnedMeshRenderer.sharedMaterials;
            combinedMeshInfo.bones = skinnedMeshRenderer.bones;

            originalMaterials = skinnedMeshRenderer.sharedMaterials;
            originalLayer = skinnedMeshRenderer.gameObject.layer;
        }


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
                rendererList[i].gameObject.layer = originalLayer;
            }
            originalMaterialsList.Add(rendererList[i].sharedMaterials);
        }
    }

    protected void LateUpdate()
    {

        if (combinedMeshDirty && ignoreCombine == false)
        {
            combinedMeshDirty = false;
            UnloadUnusedMaterials();

            SkinnedMeshRenderer mySMR = skinnedMeshRenderer;
//			if(combinedMeshInfo == null){
//				return;
//			}
			if(mySMR != null && combinedMeshInfo != null)
			{
	            mySMR.sharedMesh = combinedMeshInfo.mesh;
	            mySMR.bones = combinedMeshInfo.bones;
	            mySMR.sharedMaterials = combinedMeshInfo.materials;
	
	            originalMaterials = mySMR.sharedMaterials;
	            originalLayer = mySMR.gameObject.layer;

                if (OnCombineSuceess != null)
                {
                    OnCombineSuceess();
                }
                CombineSuceess();
			}

        }
    }

    protected void OnDestroy()
    {
        if (combinedMeshInfo != null && combinedMeshInfo.mesh && combinedMeshInfo.mesh != rawMeshInfo.mesh)
        {
            DestroyImmediate(combinedMeshInfo.mesh);
            combinedMeshInfo.materials = null;
            combinedMeshInfo.bones = null;
            combinedMeshInfo = null;
        }
    }
    #endregion

    #region 蒙皮 by吴江

    protected void UnloadUnusedMaterials()
    {
        for (int i = 0; i < skinnedMeshRenderer.materials.Length; i++)
        {
            skinnedMeshRenderer.materials[i].mainTexture = null;
            Object.Destroy(skinnedMeshRenderer.materials[i]);
        }
    }
    /// <summary>
    /// 开始蒙皮骨骼 by 吴江
    /// </summary>
    /// <param name="_smrList"></param>
    /// <param name="_refBones"></param>
    public void CombineSkinnedMesh(List<SkinnedMeshRenderer> _smrList, Dictionary<string, Transform> _refBones)
    {
        //如果已经Combine了 by吴江
        if (combinedMeshInfo.mesh != rawMeshInfo.mesh)
        {
            UnloadUnusedMaterials();

            skinnedMeshRenderer.sharedMesh = rawMeshInfo.mesh;
            skinnedMeshRenderer.sharedMaterials = rawMeshInfo.materials;
            skinnedMeshRenderer.bones = rawMeshInfo.bones;

            DestroyImmediate(combinedMeshInfo.mesh);
            combinedMeshInfo.mesh = null;
            combinedMeshInfo.materials = null;
            combinedMeshInfo.bones = null;
        }

        combinedMeshInfo = MeshHelper.CombineSkinnedMeshRenderer(_smrList, _refBones);

        combinedMeshDirty = true;
    }
    #endregion

    #region 设置材质和着色器 by吴江
    /// <summary>
    /// 重设初始layer by吴江
    /// </summary>
    /// <param name="_layer"></param>
    public virtual void ResetOriginalLayer(int _layer)
    {
        originalLayer = _layer;
    }
    /// <summary>
    /// 设置layer by吴江
    /// </summary>
    /// <param name="_layer"></param>
    public override void SetLayer(int _layer)
    {
        if (skinnedMeshRenderer)
        {
            skinnedMeshRenderer.gameObject.layer = _layer;
        }
    }
    /// <summary>
    /// 将当前layer还原成最初的layer by吴江
    /// </summary>
    public override void ResetLayer()
    {
        if (skinnedMeshRenderer)
        {
            skinnedMeshRenderer.gameObject.layer = originalLayer;
        }
    }

    protected virtual void CombineSuceess()
    {
    }
    /// <summary>
    /// 设置着色器 by吴江
    /// </summary>
    /// <param name="_shader"></param>
    public override void SetSkinnedMeshShader(Shader _shader)
    {
        if (skinnedMeshRenderer)
        {
            for (int i = 0; i < skinnedMeshRenderer.materials.Length; i++)
            {
                Material mat = skinnedMeshRenderer.materials[i];
                if (mat != null && (mat.name.Contains("_glow") || mat.shader.name == "Unlit/Transparent Cutout Flow_H"))
                {
                    continue;
                }
                 skinnedMeshRenderer.materials[i].shader = _shader;
            }
        }
    }

    /// <summary>
    /// 设置材质 by吴江
    /// </summary>
    /// <param name="_material"></param>
    public override void SetMaterial(Material _material)
    {
        if (skinnedMeshRenderer)
        {
            Material[] matList = new Material[skinnedMeshRenderer.sharedMaterials.Length];
            for (int i = 0; i < matList.Length; ++i)
            {
                matList[i] = _material;
            }
            skinnedMeshRenderer.materials = matList;
        }
    }
    /// <summary>
    /// 将当前材质还原成最初的材质 by吴江
    /// </summary>
    public override void ResetMaterials()
    {
        if (skinnedMeshRenderer)
        {
            skinnedMeshRenderer.sharedMaterials = originalMaterials;
        }
        SetCustumColor(custumColor);
    }


    public virtual void ResetShaders()
    {
        Renderer[] allRendererList = GetComponentsInChildren<Renderer>();
        for (int i = 0; i < allRendererList.Length; i++)
        {
            if (allRendererList[i].material != null && allRendererList[i].material.shader != null)
            {
                allRendererList[i].material.shader = Shader.Find(allRendererList[i].material.shader.name);
            }
        }
    }
    /// <summary>
    /// 获取框体 by吴江
    /// </summary>
    /// <param name="bounds"></param>
    /// <returns></returns>
    public bool GetBounds(out Bounds bounds)
    {
        if (skinnedMeshRenderer != null)
        {
            bounds = skinnedMeshRenderer.bounds;
            return true;
        }
        else
        {
            bounds = new Bounds();
            return false;
        }
    }
    #endregion

    #region 被击变色 by吴江
    public override void SetBlinkColor(Color _color)
    {
        base.SetBlinkColor(_color);
        for (int i = 0; i < skinnedMeshRenderer.materials.Length; i++)
        {
            skinnedMeshRenderer.materials[i].SetColor("_RimColor", _color);
            skinnedMeshRenderer.materials[i].SetColor("_Color", Color.gray);
        }
    }
    #endregion

    #region 设置实时阴影  by吴江
    public virtual void CastShadow(bool _cast)
    {
        if (skinnedMeshRenderer != null)
        {
            skinnedMeshRenderer.shadowCastingMode = _cast?  UnityEngine.Rendering.ShadowCastingMode.On : UnityEngine.Rendering.ShadowCastingMode.Off;
        }
    }
    #endregion

    #region 自定义颜色 by吴江

    protected Color custumColor = Color.white;

    public void SetCustumColor(Color _color)
    {
        custumColor = _color;
        if (GameStageUtility.custumColorShader == null)
        {
            GameStageUtility.custumColorShader = Shader.Find("Unlit/Transparent Cutout Monster");
        }
        SetSkinnedMeshShader(GameStageUtility.custumColorShader);
        for (int i = 0; i < skinnedMeshRenderer.materials.Length; i++)
        {
            skinnedMeshRenderer.materials[i].SetColor("_OverlayColor", _color);
        }
		if (originalMaterials.Length > 0 && (originalMaterials[0] == null || originalMaterials[0].shader != GameStageUtility.custumColorShader))
        {
            originalMaterials = skinnedMeshRenderer.materials;
        }
    }
    #endregion

    #region 隐身部分 by吴江
    #endregion

    #region 石化 by吴江
    /// <summary>
    /// 石化
    /// </summary>
    public void Fossil(Color _color)
    {
        if (GameStageUtility.fossilShader == null)
        {
            GameStageUtility.fossilShader = Shader.Find("Unlit/Transparent Cutout GrayDetail");
        }
        SetSkinnedMeshShader(GameStageUtility.fossilShader);
        if (skinnedMeshRenderer)
        {
            for (int i = 0; i < skinnedMeshRenderer.materials.Length; i++)
            {
                Material mat = skinnedMeshRenderer.materials[i];
                if (mat != null && (mat.name.Contains("_glow") || mat.shader.name == "Unlit/Transparent Cutout Flow_H"))
                {
                    continue;
                }
                skinnedMeshRenderer.materials[i].SetColor("_OverlayColor", _color);
            }
        }
        SetWeaponShader(GameStageUtility.fossilShader, _color, "_OverlayColor");
    }
    #endregion
}
