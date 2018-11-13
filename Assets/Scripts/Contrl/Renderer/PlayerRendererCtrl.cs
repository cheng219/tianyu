///////////////////////////////////////////////////////////////////////////////
//作者：吴江
//日期：2015/5/15
//用途：人物渲染控制器
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PlayerRendererCtrl : SmartActorRendererCtrl {


    protected Shader lowShader = null;
    protected Shader midShader = null;
    protected Shader highShader = null;

    public void OnUpdateRenderQuality(SystemSettingMng.RendererQuality _quality)
    {
        switch (_quality)
        {
            case SystemSettingMng.RendererQuality.LOW:
                if (lowShader == null)
                {
                    lowShader = Shader.Find("Unlit/Transparent Cutout Characters_L");
                }
                SetSkinnedMeshShader(lowShader);
                SetWeaponShader(lowShader, Color.white);
                break;
            case SystemSettingMng.RendererQuality.MID:
                if (midShader == null)
                {
                    midShader = Shader.Find("Unlit/Transparent Cutout Characters_M");
                }
                SetSkinnedMeshShader(midShader);
                SetWeaponShader(midShader, Color.white);
                break;
            case SystemSettingMng.RendererQuality.HIGHT:
                if (highShader == null)
                {
                    highShader = Shader.Find("Unlit/Transparent Cutout Characters_H");
                }
                SetSkinnedMeshShader(highShader);
                SetWeaponShader(highShader, Color.white);
                break;
            default:
                break;
        }
    }


    protected override void CombineArmor()
    {
        base.CombineArmor();
        OnUpdateRenderQuality(GameCenter.systemSettingMng.CurRendererQuality);
    }


}
