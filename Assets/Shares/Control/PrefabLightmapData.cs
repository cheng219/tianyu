//=======================================
//作者:吴江
//日期:2015/9/1
//用途:记录光照贴图信息,以及在load的时候重新绑定
//=======================================



using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PrefabLightmapData : MonoBehaviour
{
    [System.Serializable]
    struct RendererInfo
    {
        public Renderer renderer;
        public int lightmapIndex;
        public Vector4 lightmapOffsetScale;
    }

    [SerializeField]
    RendererInfo[] m_RendererInfo;
    [SerializeField]
    Texture2D[] m_FarLightmaps;
    [SerializeField]
    Texture2D[] m_NearLightmaps;
    [SerializeField]
    LightmapsMode m_LightmapsMode;
    [SerializeField]
    LightProbes m_LightProbes;
    [SerializeField]
    ColorSpace m_bakedColorSpace;

    void Awake()
    {
        if (m_RendererInfo == null || m_RendererInfo.Length == 0)
            return;

        var lightmaps = LightmapSettings.lightmaps;
        var combinedLightmaps = new LightmapData[lightmaps.Length + m_FarLightmaps.Length];

        lightmaps.CopyTo(combinedLightmaps, 0);
        for (int i = 0; i < m_FarLightmaps.Length; i++)
        {
            combinedLightmaps[i + lightmaps.Length] = new LightmapData();
            combinedLightmaps[i + lightmaps.Length].lightmapFar = m_FarLightmaps[i];
        }


        for (int i = 0; i < m_NearLightmaps.Length; i++)
        {
            combinedLightmaps[i + lightmaps.Length].lightmapNear = m_NearLightmaps[i];
        }


        ApplyRendererInfo(m_RendererInfo, lightmaps.Length);



        LightmapSettings.lightmapsMode = m_LightmapsMode;
        LightmapSettings.lightProbes = m_LightProbes;

        LightmapSettings.lightmaps = combinedLightmaps;

    }


    static void ApplyRendererInfo(RendererInfo[] infos, int lightmapOffsetIndex)
    {
        for (int i = 0; i < infos.Length; i++)
        {
            var info = infos[i];
            info.renderer.lightmapIndex = info.lightmapIndex + lightmapOffsetIndex;
            info.renderer.lightmapScaleOffset = info.lightmapOffsetScale;
        }
    }

#if UNITY_EDITOR
    [UnityEditor.MenuItem("Assets/Bake Prefab Lightmaps")]
    static void GenerateLightmapInfo()
    {
        if (UnityEditor.Lightmapping.giWorkflowMode != UnityEditor.Lightmapping.GIWorkflowMode.OnDemand)
        {
            Debug.LogError("ExtractLightmapData requires that you have baked you lightmaps and Auto mode is disabled.");
            return;
        }
        UnityEditor.Lightmapping.Bake();

        PrefabLightmapData[] prefabs = FindObjectsOfType<PrefabLightmapData>();


        foreach (var instance in prefabs)
        {
            var gameObject = instance.gameObject;
            var rendererInfos = new List<RendererInfo>();
            var farlightmaps = new List<Texture2D>();
            var nearlightmaps = new List<Texture2D>();

            GenerateLightmapInfo(gameObject, rendererInfos, farlightmaps, nearlightmaps);

            instance.m_RendererInfo = rendererInfos.ToArray();
            instance.m_FarLightmaps = farlightmaps.ToArray();
            instance.m_NearLightmaps = nearlightmaps.ToArray();
            instance.m_LightmapsMode = LightmapSettings.lightmapsMode;
            instance.m_LightProbes = LightmapSettings.lightProbes;
            instance.m_bakedColorSpace = QualitySettings.desiredColorSpace;

            var targetPrefab = UnityEditor.PrefabUtility.GetPrefabParent(gameObject) as GameObject;
            if (targetPrefab != null)
            {
                //UnityEditor.Prefab
                UnityEditor.PrefabUtility.ReplacePrefab(gameObject, targetPrefab);
            }
        }
    }

    static void GenerateLightmapInfo(GameObject root, List<RendererInfo> rendererInfos, List<Texture2D> farlightmaps, List<Texture2D> nearlightmaps)
    {
        var renderers = root.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in renderers)
        {
            if (renderer.lightmapIndex != -1)
            {
                RendererInfo info = new RendererInfo();
                info.renderer = renderer;
                info.lightmapOffsetScale = renderer.lightmapScaleOffset;

                Texture2D farlightmap = LightmapSettings.lightmaps[renderer.lightmapIndex].lightmapFar;

                info.lightmapIndex = farlightmaps.IndexOf(farlightmap);
                if (info.lightmapIndex == -1)
                {
                    info.lightmapIndex = farlightmaps.Count;
                    farlightmaps.Add(farlightmap);
                }

                Texture2D nearlightmap = LightmapSettings.lightmaps[renderer.lightmapIndex].lightmapNear;

                info.lightmapIndex = nearlightmaps.IndexOf(nearlightmap);
                if (info.lightmapIndex == -1)
                {
                    info.lightmapIndex = nearlightmaps.Count;
                    nearlightmaps.Add(nearlightmap);
                }

                rendererInfos.Add(info);
            }
        }
    }
#endif

}