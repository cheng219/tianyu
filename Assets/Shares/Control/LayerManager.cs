///////////////////////////////////////////////////////////////////////////////
// 作者：吴江
// 日期：2015/4/29
// 用途：layer管理类
///////////////////////////////////////////////////////////////////////////////


using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum LayerMaskType
{
    None,
    NormalView,
    SceneAnimView,
    MouseInput,
    LightCast,
    LogicMap,
    ColorMap,
    NGUI,
    SCENE_ITEM,
    BLOCK,
}


public static class LayerMng
{
    public static int noneMask;
    public static int mouseInputLayerMask;
    public static int lineCastMask;
    public static int sceneItemMask;
    public static int normalViewMask;
    public static int sceneAnimViewMask;
    public static int sceneDirLightMask;
    public static int uiMask;
    public static int blockMask;

    public static void Init()
    {
        noneMask = GetLayerMask(LayerMaskType.None);
        mouseInputLayerMask = GetLayerMask(LayerMaskType.MouseInput);
        normalViewMask = GetLayerMask(LayerMaskType.NormalView);
        sceneAnimViewMask = GetLayerMask(LayerMaskType.SceneAnimView);
        sceneDirLightMask = GetLayerMask(LayerMaskType.LightCast);
        uiMask = GetLayerMask(LayerMaskType.NGUI);
        sceneItemMask = GetLayerMask(LayerMaskType.SCENE_ITEM);
        blockMask = GetLayerMask(LayerMaskType.BLOCK);
        lineCastMask = LineCastLayerMask();
    }


    public static int GetLayerMask(LayerMaskType _type)
    {
        int value = 0;
        List<int> list = GetLayerList(_type);
        if (list != null)
        {
            foreach (var item in list)
            {
                value += (1 << item);
            }
        }
        return value;
    }



    public static int LineCastLayerMask()
    {
        int ground = LayerMask.NameToLayer("Terrain");
        int layerMask = 1 << ground;
        return layerMask;
    }



    public static List<int> GetLayerList(LayerMaskType _type)
    {
        List<int> list = new List<int>();
        switch (_type)
        {
            case LayerMaskType.None:
                return list;
            case LayerMaskType.NormalView:
                list.Add(LayerMask.NameToLayer("Default"));
                list.Add(LayerMask.NameToLayer("TransparentFX"));
                list.Add(LayerMask.NameToLayer("Ignore Raycast"));
                list.Add(LayerMask.NameToLayer("Water"));
                list.Add(LayerMask.NameToLayer("Terrain"));
                list.Add(LayerMask.NameToLayer("Player"));
                list.Add(LayerMask.NameToLayer("NPC"));
                list.Add(LayerMask.NameToLayer("Monster"));
                list.Add(LayerMask.NameToLayer("Static"));
                list.Add(LayerMask.NameToLayer("DraggView"));
                list.Add(LayerMask.NameToLayer("Map"));
                list.Add(LayerMask.NameToLayer("NGUI3D"));
                list.Add(LayerMask.NameToLayer("CastShadow"));
                list.Add(LayerMask.NameToLayer("OtherPlayer"));
                list.Add(LayerMask.NameToLayer("DropItem"));
                list.Add(LayerMask.NameToLayer("SceneItem"));
                list.Add(LayerMask.NameToLayer("Entourage"));
                list.Add(LayerMask.NameToLayer("Pet"));
                list.Add(LayerMask.NameToLayer("SceneEffect"));
                return list;
            case LayerMaskType.SceneAnimView:
                list.Add(LayerMask.NameToLayer("Default"));
                list.Add(LayerMask.NameToLayer("TransparentFX"));
                list.Add(LayerMask.NameToLayer("Ignore Raycast"));
                list.Add(LayerMask.NameToLayer("Water"));
                list.Add(LayerMask.NameToLayer("Terrain"));
                list.Add(LayerMask.NameToLayer("NPC"));
                list.Add(LayerMask.NameToLayer("Monster"));
                list.Add(LayerMask.NameToLayer("Static"));
                list.Add(LayerMask.NameToLayer("DraggView"));
                list.Add(LayerMask.NameToLayer("Map"));
                list.Add(LayerMask.NameToLayer("NGUI3D"));
                list.Add(LayerMask.NameToLayer("CastShadow"));
                list.Add(LayerMask.NameToLayer("CGPlayer"));
                return list;
            case LayerMaskType.MouseInput:
                list.Add(LayerMask.NameToLayer("Terrain"));
                list.Add(LayerMask.NameToLayer("NPC"));
                list.Add(LayerMask.NameToLayer("Monster"));
                list.Add(LayerMask.NameToLayer("Static"));
                list.Add(LayerMask.NameToLayer("DraggView"));
                list.Add(LayerMask.NameToLayer("NGUI3D"));
                list.Add(LayerMask.NameToLayer("OtherPlayer"));
                list.Add(LayerMask.NameToLayer("DropItem"));
                list.Add(LayerMask.NameToLayer("SceneItem"));
                list.Add(LayerMask.NameToLayer("Entourage"));
                list.Add(LayerMask.NameToLayer("Block"));
                return list;
            case LayerMaskType.LightCast:
                list.Add(LayerMask.NameToLayer("Terrain"));
                list.Add(LayerMask.NameToLayer("NPC"));
                list.Add(LayerMask.NameToLayer("Monster"));
                list.Add(LayerMask.NameToLayer("OtherPlayer"));
                list.Add(LayerMask.NameToLayer("Entourage"));
                list.Add(LayerMask.NameToLayer("Player"));
                list.Add(LayerMask.NameToLayer("CastShadow"));
                return list;
            case LayerMaskType.LogicMap:
                list.Add(LayerMask.NameToLayer("Map"));
                return list;
            case LayerMaskType.ColorMap:
                list.Add(LayerMask.NameToLayer("CastShadow"));
                list.Add(LayerMask.NameToLayer("Grass"));
                list.Add(LayerMask.NameToLayer("Terrain"));
                list.Add(LayerMask.NameToLayer("Default"));
                return list;
            case LayerMaskType.NGUI:
                list.Add(LayerMask.NameToLayer("NGUI"));
                list.Add(LayerMask.NameToLayer("NGUI3D"));
                list.Add(LayerMask.NameToLayer("UI"));
                return list;
            case LayerMaskType.SCENE_ITEM:
                list.Add(LayerMask.NameToLayer("SceneItem"));
                return list;
            case LayerMaskType.BLOCK:
                list.Add(LayerMask.NameToLayer("Block"));
                return list;
            default:
                return list;
        }
    }


}