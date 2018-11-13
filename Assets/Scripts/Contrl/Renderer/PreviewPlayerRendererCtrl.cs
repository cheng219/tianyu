///////////////////////////////////////////////////////////////////////////////
//作者：吴江
//日期：2015/5/15
//用途：预览人物渲染控制器
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PreviewPlayerRendererCtrl : PlayerRendererCtrl
{
    /// <summary>
    /// 试穿的装备数据 （这份数据和人物原本的装备信息共同构成了 curShowEquipments）
    /// </summary>
    protected Dictionary<EquipSlot, EquipmentInfo> tryShowEquipments = new Dictionary<EquipSlot, EquipmentInfo>();


    /// <summary>
    /// 初始化特效
    /// </summary>
    protected override void InitEffects()
    {
        if (fxCtrl == null) return;
        if (tryShowEquipments.Count == 0)
        {
            base.InitEffects();
        }
        else
        {
            foreach (EquipmentInfo item in actorInfo.CurShowDictionary.Values)
            {
				if(item == null)continue;
                if (tryShowEquipments.ContainsKey(item.Slot))
                {
                    EquipmentInfo tryItem = tryShowEquipments[item.Slot];
                    if (tryItem != null && tryItem.BoneEffectList.Count > 0)
                    {
                        for (int i = 0; i < tryItem.BoneEffectList.Count; i++)
                        {
                            fxCtrl.SetBoneEffect(tryItem.BoneEffectList[i].boneName, tryItem.BoneEffectList[i].effectName, actorInfo.ModelScale);
                        }
                    }
                }
                else
                {
                    if (item != null && item.BoneEffectList.Count > 0)
                    {
                        for (int i = 0; i < item.BoneEffectList.Count; i++)
                        {
                            fxCtrl.SetBoneEffect(item.BoneEffectList[i].boneName, item.BoneEffectList[i].effectName, actorInfo.ModelScale);
                        }
                    }
                }
            }
        }
    }

    public void SetCurTryShowEquip(EquipSlot _slot, EquipmentInfo _eq)
    {
        tryShowEquipments[_slot] = _eq;
    }
}
