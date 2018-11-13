//===================================================
//作者：吴江
//日期：2015/10/29
//用途：随从表现层基类
//====================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;



/// <summary>
/// 玩家角色基类 by吴江
/// </summary>
public class EntourageBase : SmartActor
{
    #region 数据
    /// <summary>
    /// 渲染控制器
    /// </summary>
    [System.NonSerialized]
    public new SmartActorRendererCtrl rendererCtrl = null;
    /// <summary>
    /// 动画控制器
    /// </summary>
    [System.NonSerialized]
    protected new SmartActorAnimFSM animFSM = null;
    /// <summary>
    /// 数据层对象引用
    /// </summary>
    protected new MercenaryInfo actorInfo
    {
        get { return base.actorInfo as MercenaryInfo; }
        set
        {
            base.actorInfo = value;
            if (value != null) id = actorInfo.ServerInstanceID;
        }
    }
    /// <summary>
    /// 等级
    /// </summary>
    public int Level
    {
        get
        {
            return actorInfo == null ? 0 : actorInfo.Level;
        }
    }
    /// <summary>
    /// 灵魂
    /// </summary>
    protected GameObject soulObject = null;

    /// <summary>
    /// 主人
    /// </summary>
    protected PlayerBase owner = null;
    /// <summary>
    /// 主人
    /// </summary>
    public PlayerBase Owner
    {
        get
        {
            if (owner == null)
            {
                if (actorInfo.OwnerID == GameCenter.curMainPlayer.id)
                {
                    owner = GameCenter.curMainPlayer;
                }
                else
                {
                    owner = GameCenter.curGameStage.GetOtherPlayer(actorInfo.OwnerID);
                }
            }
            return owner;
        }
    }

    public new int Camp
    {
        get
        {
            if (Owner != null)
            {
                return Owner.Camp;
            }
            return actorInfo.Camp;
        }
    }
    #endregion

    #region UNITY
    /// <summary>
    /// dummy状态也需要准确的typeID 因此使用Awake() by吴江
    /// </summary>
    protected new void Awake()
    {
        typeID = ObjectType.Entourage;
        base.Awake();
    }


    #endregion

    #region 构造
    /// <summary>
    /// 尽量避免使用Awake等unity控制流程的接口来初始化，而改用自己调用的接口 by吴江
    /// </summary>
    protected override void Init()
    {

        height = actorInfo.NameHeight;
        nameHeight = height;
        base.Init();
        if (headTextCtrl == null)
        {
            headTextCtrl = this.gameObject.AddComponent<HeadTextCtrl>();
        }
        animFSM = base.animFSM as SmartActorAnimFSM;
        if (animFSM == null)
        {
            animFSM = this.gameObject.GetComponentInChildrenFast<SmartActorAnimFSM>(true);
        }

        rendererCtrl = base.rendererCtrl as SmartActorRendererCtrl;
        if (rendererCtrl == null)
        {
            rendererCtrl = this.gameObject.GetComponentInChildrenFast<SmartActorRendererCtrl>(true);
        }
        ActiveBoxCollider(false, 0f);//宠物不可点击,仅设置false不行,在隐藏再显示后会变为true。所以赋值0
        rendererCtrl.Init(actorInfo, fxCtrl);
        animationRoot.gameObject.transform.localScale *= actorInfo.ModelScale;

        if (animFSM != null)
        {
            animFSM.SetMoveSpeed(CurRealSpeed / (actorInfo.AnimationMoveSpeedBase * actorInfo.ModelScale));
            animFSM.OnDeadEnd = OnDeadEnd;
        }

        GameObject obj = new GameObject("ShokWaveObj");
        obj.transform.parent = this.transform;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localEulerAngles = Vector3.zero;
        AfsShockWave afsShockWave = obj.AddComponent<AfsShockWave>();
        afsShockWave.ShockSpeed = 1.0f;
        afsShockWave.ShockPower = 1.0f;
        afsShockWave.ShockDelay = 1.0f;
        Rigidbody rigidbody = obj.AddComponent<Rigidbody>();
        rigidbody.useGravity = false;
        rigidbody.isKinematic = true;
    }

    /// <summary>
    /// 死亡动作结束后的事件 
    /// </summary>
    public virtual void OnDeadEnd()
    {
        //if (IsShowing && fxCtrl != null)
        //{
        //    fxCtrl.DoDeadEffect("die_B02");
        //    if (actorInfo.IsOnMount)
        //    {
        //        fxCtrl.DoRideEffect("player_mount_B02");
        //    }
        //}
    }

    /// <summary>
    /// 注册事件监听
    /// </summary>
    protected override void Regist()
    {
        base.Regist();
        if (actorInfo != null)
        {
        }
    }
    /// <summary>
    /// 注销事件监听
    /// </summary>
    public override void UnRegist()
    {
        base.UnRegist();
        if (actorInfo != null)
        {
        }
    }
    #endregion

    #region 辅助逻辑
    /// <summary>
    /// 开始移动，执行移动动画
    /// </summary>
    public override void MoveStart()
    {
        if (animFSM)
        {
            animFSM.Move();
        }
    }
    /// <summary>
    /// 移动结束，执行站立动画
    /// </summary>
    public override void MoveEnd()
    {
        if (animFSM)
        {
            animFSM.StopMoving();
        }
    }
    /// <summary>
    /// 转职特效 
    /// </summary>
    /// <param name='_prof'>
    /// _prof.
    /// </param>
    protected virtual void OnProfDolevelupeffect(int _prof)
    {
        fxCtrl.DoLevelUPEffect("lvlup_B02");
    }


    /// <summary>
    /// 使用技能
    /// </summary>
    /// <param name="_instance"></param>
    //public override void UseAbility(AbilityInstance _instance, List<System.Action> _animEventList = null)
    //{
    //    if (isDizzy) return;
    //    List<SmartActor> list = new List<SmartActor>();
    //    List<Monster> mobList = GameCenter.curGameStage.GetMobs();
    //    foreach (var item in mobList)
    //    {
    //        if (!item.IsFriend)
    //        {
    //            list.Add(item);
    //        }
    //    }
    //    List<OtherPlayer> opcList = GameCenter.curGameStage.GetOtherPlayers();
    //    foreach (var item in opcList)
    //    {
    //        if (!item.IsFriend)
    //        {
    //            list.Add(item);
    //        }
    //    }
    //    base.UseAbility(_instance, _instance == null ? null : AbilityInfluence(list, _instance));
    //}

    public static bool SlotsArrayContainsEle(EquipSlot[] _array, EquipSlot _ele)
    {
        foreach (EquipSlot slot in _array)
        {
            if (slot == _ele)
            {
                return true;
            }
        }
        return false;
    }

    public virtual void DestorySelf()
    {
        GameObject.DestroyImmediate(this.gameObject);
    }
    /// <summary>
    /// 设置是否进入战斗
    /// </summary>
    /// <param name="_combat"></param>
    public void SetInfoInCombat(bool _combat)
    {
        if (animFSM != null)
        {
            animFSM.SetInCombat(_combat);
        }
    }

    protected override void OnAliveStateUpdate(bool _alive)
    {
        if (isDummy) return;
        if (_alive != !isDead)
        {
            if (_alive)
            {
                ReLive();
            }
            else
            {
                Dead();
            }
        }
    }

    protected virtual void OnNameChange(string _newName)
    {
        if (headTextCtrl != null)
        {
            headTextCtrl.SetPetName(_newName);
        }
    }

    protected virtual void OnPetNameColorChange(Color _color)
    {
        if (headTextCtrl != null)
        { 
            headTextCtrl.SetPetNameColor(_color);
        }
    }

    protected virtual void OnTitleNameChange(string _newName)
    {
        if (headTextCtrl != null)
        {
            headTextCtrl.SetTitleSprite(_newName);
        }
    }

    protected virtual void OnOwnerNameUpdate()
    {
        if (headTextCtrl != null)
        {
            headTextCtrl.SetPetOwnerName(actorInfo.NoColorOwnerName);
        }
    }

    /// <summary>
    /// 展示/隐藏基本模型
    /// </summary>
    /// <param name="_show"></param>
    public override void Show(bool _show)
    {
        base.Show(_show);
    }

    protected virtual void EquipUpdate(EquipSlot _slot)
    {
        if (rendererCtrl != null)
        {
            rendererCtrl.EquipUpdate(_slot);
        }
    }


    protected override void OnBuffUpdate(int _buffID, bool _add)
    {
        base.OnBuffUpdate(_buffID, _add);
        //BuffInfo info = actorInfo.GetBuffInfo(_buffID);
        //if (info != null && info.ChangeModelKey > 0)
        //{
        //    Morph(_add, info.ChangeModelKey);
        //}
    }

    /// <summary>
    /// 变身控制
    /// </summary>
    /// <param name="_morph"></param>
    /// <param name="_refID"></param>
    protected void Morph(bool _morph, int _refID)
    {
        if (rendererCtrl != null)
        {
            if (_morph)
            {
                rendererCtrl.Morph(_refID);
            }
            else
            {
                rendererCtrl.CancelMorph();
            }
        }
    }
    /// <summary>
    /// 复活
    /// </summary>
    public override void ReLive()
    {
        if (isDead)
        {
            base.ReLive();
            Show(true);
            if (soulObject != null)
            {
                soulObject.SetActive(false);
            }
            if (animFSM != null)
            {
                animFSM.RandIdle();
            }
        }
    }


    public override void TickAnimation()
    {
        base.TickAnimation();
        //if (actorInfo.CurMountInfo != null && actorInfo.CurMountInfo.IsRiding && !IsActor)
        //{
        //    if (moveFSM != null)
        //    {
        //        if (moveFSM.isMoving)
        //        {
        //            animFSM.MountMove();
        //        }
        //        else
        //        {
        //            animFSM.OnMount(true);
        //        }
        //    }
        //    if (!actorInfo.IsAlive)
        //    {
        //        animFSM.Dead();
        //    }
        //}
    }
    #endregion
}
