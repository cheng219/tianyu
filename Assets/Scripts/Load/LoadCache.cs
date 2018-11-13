///////////////////////////////////////////////////////////////////////////////////////////
//作者：吴江
//最后修改时间：2016/7/30
//脚本描述：
///////////////////////////////////////////////////////////////////////////////////////////


using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum AssetType
{
    texture,
    material,
    shader
}

public class LoadCache  {

    #region 图片shader材质


    public static bool HasLoaded(string _name, AssetType item)
    {
        switch (item)
        {
            case AssetType.texture:
                return textureRefDic.ContainsKey(_name);
            case AssetType.material:
                return materialRefDic.ContainsKey(_name);
            default:
                break;
        }
        return false;
    }


    public static bool UnLoad(string _name, AssetType item)
    {
        switch (item)
        {
            case AssetType.texture:
                if (textureRefDic.ContainsKey(_name))
                {
                    if (textureRefDic[_name] != null)
                    {
                        AssetMng.instance.UnloadUrl(SceneLoadUtil.instance.MainPath + _name + ".texture");
                        AssetMng.instance.UnloadUrl(EffectLoadUtil.instance.MainPath + _name + ".texture");
                        GameObject.DestroyImmediate(textureRefDic[_name], true);
                        textureRefDic[_name] = null;
                        textureRefDic.Remove(_name);
                    }
                    return true;
                }
                break;
            case AssetType.material:
                if (materialRefDic.ContainsKey(_name))
                {
                    AssetMng.instance.UnloadUrl(SceneLoadUtil.instance.MainPath + _name + ".material");
                    AssetMng.instance.UnloadUrl(EffectLoadUtil.instance.MainPath + _name + ".material");
                    GameObject.DestroyImmediate(materialRefDic[_name], true);
                    materialRefDic[_name] = null;
                    materialRefDic.Remove(_name);
                    return true;
                }
                break;
            default:
                break;
        }
        return false;
    }


    public static Material GetMaterial(string _name)
    {
        if (materialRefDic.ContainsKey(_name)) return materialRefDic[_name] as Material;
        return null;
    }


    public static Texture GetTexture(string _name)
    {
        if (textureRefDic.ContainsKey(_name)) return textureRefDic[_name] as Texture;
        return null;
    }


    /// <summary>
    /// 图片资源缓存
    /// </summary>
    public static Dictionary<string, UnityEngine.Object> textureRefDic = new Dictionary<string, UnityEngine.Object>();

    public static List<string> waitTexture = new List<string>();

    /// <summary>
    /// 材质资源缓存
    /// </summary>
    public static Dictionary<string, UnityEngine.Object> materialRefDic = new Dictionary<string, UnityEngine.Object>();

    public static List<string> waitMaterial = new List<string>();
    #endregion
}
