//=============================================
//作者：吴江
//日期:2015/11/9
//用途：陷阱对象
//=============================================



using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 陷阱对象 吴江
/// </summary>
public class Trap : InteractiveObject
{
    /// <summary>
    /// 数据对象引用 by吴江
    /// </summary>
    protected TrapInfo info = null;

    public GameObject Effect = null;

    /// <summary>
    /// 创建陷阱虚拟体 by吴江
    /// </summary>
    /// <param name="_info"></param>
    /// <returns></returns>
    public static Trap CreateDummy(TrapInfo _info)
    {
        if (_info.IsDead) return null;
        GameObject newGO = new GameObject("Dummy Trap[" + _info.InstanceID + "]");
        newGO.tag = "collider";
        newGO.SetMaskLayer(LayerMask.NameToLayer("DropItem"));
        Trap newTrap = newGO.AddComponent<Trap>();
        newTrap.id = _info.InstanceID;
        newTrap.info = _info;
        newTrap.isDummy_ = true;
        newTrap.typeID = ObjectType.Trap;
        GameCenter.curGameStage.PlaceGameObjectFromStaticRef(newTrap, (int)_info.ServerPosX, (int)_info.ServerPosY, (int)_info.Dir, _info.Hight);
        GameCenter.curGameStage.AddObject(newTrap);
        return newTrap;
    }


    protected new void Awake()
    {
        typeID = ObjectType.Trap;
        base.Awake();
    }

    /// <summary>
    /// 创建陷阱实体 by吴江
    /// </summary>
    public virtual void StartAsyncCreate()
    {
        if (info != null && info.EffectName.Length > 0 && info.EffectName != "0")
        {
            if (fxCtrl == null)
            {
                fxCtrl = this.gameObject.GetComponent<FXCtrl>();
                if (fxCtrl == null)
                {
                    fxCtrl = this.gameObject.AddComponent<FXCtrl>();
                }
            }
            fxCtrl.DoNormalEffect(info.EffectName);
            this.gameObject.name = "Trap[" + info.InstanceID + "]";
            GameCenter.soundMng.PlaySound(info.PlaySound, SoundMng.GetSceneSoundValue(transform, GameCenter.curMainPlayer.transform), false, true);
        }
        isDummy_ = false;
    }



    void Update()
    {
        if (info != null && info.IsDead)
        {
            Destroy(this);
        }
    }

}
