//=========================================
//作者：吴江
//日期：2015/5/15
//用途：智能物体对象（预期：作为怪物和玩家以及npc的基类）
//=========================================



using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 智能物体对象（预期：作为怪物和玩家以及npc的基类） by吴江
/// </summary>
public class SmartActor : Actor
{
    #region 数据
    protected new ActorInfo actorInfo
    {
        get { return base.actorInfo as ActorInfo; }
        set
        {
            base.actorInfo = value;
            if (value != null) id = base.actorInfo.ServerInstanceID;
        }
    }
    protected string actorName_ = "Unknown";
    public string ActorName
    {
        get { return actorInfo != null ? actorInfo.Name : actorName_; }
    }
	/// <summary>
	/// 最大蓝
	/// </summary>
	public int MaxMp
	{
        get { return actorInfo.MaxMP; }
	}
	public ulong curMp
	{
		get{
            return actorInfo.CurMP;
		}
	}
    public int MaxHp
    {
        get { return actorInfo.MaxHP; }
    }
    public ulong curHp
    {
        get { return actorInfo.CurHP; }
    }

    /// <summary>
    /// 是否为演员对象（非真实对象）
    /// </summary>
    public virtual bool IsActor
    {
        get
        {
            return actorInfo.IsActor;
        }
    }
    protected bool mainPlayerFocus = false;
    /// <summary>
    /// 主角是否关注
    /// </summary>
    public virtual bool MainPlayerFocus
    {
        get
        {
            return mainPlayerFocus;
        }
        protected set
        {
            mainPlayerFocus = value;
        }
    }

    ///<summary>
    ///对象渲染控制器 by吴江
    ///</summary>
    [System.NonSerialized]
    public new SmartActorRendererCtrl rendererCtrl = null;
    /// <summary>
    /// 动画控制组件 by吴江
    /// </summary>
    [System.NonSerialized]
    protected new SmartActorAnimFSM animFSM = null;
    /// <summary>
    /// 是否正在施放技能动作(仅判断动作有无完成)
    /// </summary>
    public bool isCasting
    {
        get
        {
            if (animFSM == null || moveFSM == null)
                return true;
            return animFSM.IsCasting || moveFSM.isMoveLocked;
        }
    }
    /// <summary>
    /// 是否沉默状态 
    /// </summary>
    protected bool isSilent = false;
    /// <summary>
    /// 是否沉默状态 
    /// </summary>
    public bool IsSilent
    {
        get
        {
            return isSilent;
        }
        protected set
        {
            isSilent = value;
        }
    }

    /// <summary>
    /// 是否正在施放技能特效(仅判断特效有无完成)
    /// </summary>
    public bool isCastingAttackEffect
    {
        get
        {
            if (fxCtrl == null)
                return false;
            return fxCtrl.IsCastingAttackEffect;
        }
    }
    /// <summary>
    /// 是否正在施放技能僵直(不能主动做别的事)
    /// </summary>
    public bool isRigidity
    {
        get
        {
            if (animFSM == null || moveFSM == null)
                return false;
            return animFSM.IsRigiditing || moveFSM.isMoveLocked;
        }
    }

    public bool IsProtecting
    {
        get
        {
            if (animFSM == null || moveFSM == null)
                return false;
            return animFSM.IsProtecting || moveFSM.isMoveLocked;
        }
    }
    /// <summary>
    /// 是否与主角为敌对关系 by吴江
    /// </summary>
    protected bool isFriend = false;
    /// <summary>
    ///  是否与主角为敌对关系 by吴江
    /// </summary>
    public bool IsFriend
    {
        get
        {
            return isFriend;
        }
        protected set
        {
            isFriend = value;
        }
    }
    /// <summary>
    /// 碰撞半径 by吴江
    /// </summary>
    public float ColliderRadius
    {
        get
        {
            return actorInfo.ColliderRadius;
        }
    }
    /// <summary>
    /// 攻击弹道骨骼出发点 by吴江
    /// </summary>
    protected Transform attackPoint = null;
    /// <summary>
    /// 攻击弹道骨骼出发点 by吴江
    /// </summary>
    public Transform AttackPoint
    {
        get
        {
            if (attackPoint == null)//针对美术糟糕的命名习惯的无奈处理
            {
                attackPoint = MeshHelper.GetBone(this.transform, "shotPoint");
                if (attackPoint == null)
                {
                    attackPoint = MeshHelper.GetBone(this.transform, "shotpoint");
                    if (attackPoint == null)
                    {
                        attackPoint = MeshHelper.GetBone(this.transform, "ShotPoint");
                        if (attackPoint == null)
                        {
                            attackPoint = MeshHelper.GetBone(this.transform, "Shotpoint");
                        }
                    }
                }
            }
            if (attackPoint == null)
            {
                attackPoint = transform;
            }
            return attackPoint;
        }
    }

    protected Color abilityWarnningColor = new Color(1,0.2f,0);
    #endregion 

    #region 构造
    /// <summary>
    /// 尽量避免使用Awake等unity控制流程的接口来初始化，而改用自己调用的接口 by吴江
    /// </summary>
    protected override void Init()
    {
        base.Init();
        if (actorInfo != null)
        {
            id = actorInfo.ServerInstanceID;
        }
        rendererCtrl = base.rendererCtrl as SmartActorRendererCtrl;
        animFSM = base.animFSM as SmartActorAnimFSM;
        foreach (BuffInfo item in actorInfo.BuffList.Values)
        {
            OnBuffUpdate(item.BuffTypeID, true);
        }
        if (GameCenter.curMainPlayer != null)
        {
            IsFriend = !PlayerAutoFightFSM.IsEnemy(this);
        }
    }


    protected virtual void InitAnimation()
    {
        //animFSM.SetupIdleAndMoveAnimationName("idle1", "move1");
        //animFSM.SetupCombatAnimationName("idle1", null);
        animFSM.SetRandIdleAnimationName("waiting", null);
        animFSM.SetupInjuryAnimation("kickdown", null, false);
        animFSM.SetupInjuryUpAnimation("getup", null, false);
        animFSM.SetupDeathAnimation("striked", null);
        animFSM.fxCtrlRef = fxCtrl;
        animFSM.headTextCtrlRef = headTextCtrl;
        animFSM.moveFSMRef = moveFSM;
        CityStage city = GameCenter.curGameStage as CityStage;
        animFSM.SetInCombat(city == null);
        animFSM.StartStateMachine();
    }

    /// <summary>
    /// 注册事件
    /// </summary>
    protected override void Regist()
    {
        base.Regist();
        if (actorInfo != null)
        {
            actorInfo.OnAliveUpdate += OnAliveStateUpdate;
            actorInfo.OnBuffUpdate += OnBuffUpdate;
            //actorInfo.OnCleanBuff += OnCleanBuff;
            actorInfo.OnBaseUpdate += OnBaseUpdate;
            actorInfo.OnPropertyUpdate += OnPropertyUpdate;
            actorInfo.OnFightStateUpdate += SetInCombat;
            actorInfo.OnCampUpdate += OnCampUpdate;
        }
        SystemSettingMng.OnUpdateRealTimeShadow += OnRealTimeShadowUpdate;
        SystemSettingMng.OnUpdateShowBloodSlider += OnUpdateBloodSliderShow;
        if (GameCenter.mainPlayerMng != null && GameCenter.mainPlayerMng.MainPlayerInfo != null)
        {
            GameCenter.mainPlayerMng.MainPlayerInfo.OnCampUpdate += OnCampUpdate;
        }
    }
    /// <summary>
    /// 注销事件
    /// </summary>
    public override void UnRegist()
    {
        base.UnRegist();
        if (actorInfo != null)
        {
            actorInfo.OnAliveUpdate -= OnAliveStateUpdate;
            actorInfo.OnBuffUpdate -= OnBuffUpdate;
            //actorInfo.OnCleanBuff -= OnCleanBuff;
            actorInfo.OnBaseUpdate -= OnBaseUpdate;
            actorInfo.OnPropertyUpdate -= OnPropertyUpdate;
            actorInfo.OnFightStateUpdate -= SetInCombat;
            actorInfo.OnCampUpdate -= OnCampUpdate;
        }
        SystemSettingMng.OnUpdateRealTimeShadow -= OnRealTimeShadowUpdate;
        SystemSettingMng.OnUpdateShowBloodSlider -= OnUpdateBloodSliderShow;
        if (GameCenter.mainPlayerMng != null && GameCenter.mainPlayerMng.MainPlayerInfo != null)
        {
            GameCenter.mainPlayerMng.MainPlayerInfo.OnCampUpdate -= OnCampUpdate;
        }

    }
    #endregion

    #region UNITY

    /// <summary>
    /// 即使dummy状态，id也必须保持一致，因此这里用了Awake() by吴江
    /// </summary>
    protected new void Awake()
    {
        base.Awake();
        if (actorInfo != null)
        {
            id = actorInfo.ServerInstanceID;
        }
        moveFSM = this.gameObject.GetComponent<ActorMoveFSM>();
    }

    protected new void Update()
    {
        base.Update();
        if (IsActor) return;
        List<AbilityResultInfo> unFinishedAbilityResultList = actorInfo.unFinishedAbilityResultList;
        if (unFinishedAbilityResultList.Count > 0)
        {
            AbilityResultInfo hasFinishedInstance = null;
            for (int i = 0; i < unFinishedAbilityResultList.Count; i++)
            {
                AbilityResultInfo info = unFinishedAbilityResultList[i];
                if (info.NeedShow)
                {
                    info.Show(this);
                }
                else if (info.HasFinished && info.HasMoveFinished)
                {
                    hasFinishedInstance = info;
                }
            }
            if (hasFinishedInstance != null)
            {
                unFinishedAbilityResultList.Remove(hasFinishedInstance);
            }
        }
    }

    protected new void OnDestroy()
    {
        if (GameCenter.curMainPlayer != null && GameCenter.curMainPlayer.CurTarget == this)
        {
            GameCenter.curMainPlayer.CurTarget = null;
        }
        DoRingEffect(Color.white, false, 1);
        UnRegist();
        base.OnDestroy();
    }
    #endregion

    #region 施放技能相关 by吴江

    protected void OnCampUpdate(int _camp)
    {
        if (IsActor) return;
        IsFriend = !PlayerAutoFightFSM.IsEnemy(this);
        if (headTextCtrl != null)
        {
//            if (headTextCtrl.HasShowBlood)
//            {
//                headTextCtrl.SetBlood(actorInfo.MaxHP <= 0 ? 0 : actorInfo.CurHP / (float)actorInfo.MaxHP, IsFriend);
//            }
            headTextCtrl.SetNameColorAlongInterActive();
        }
    }



    public AbilityInstance curTryUseAbility = null;
    public AbilityShadowEffecter abilityShadowEffecter = null;

    /// <summary>
    /// 使用技能 by吴江
    /// </summary>
    public virtual void UseAbility(AbilityInstance _instance)
    {
		if (IsSilent && _instance.thisSkillMode == SkillMode.CLIENTSKILL) return;//客户端技能 沉默了才不能放
        if (moveFSM != null && moveFSM.isMovingTo)
        {
            moveFSM.StopMovingTo();
        }
        curTryUseAbility = _instance;
        if (abilityShadowEffecter != null)
        {
            GameCenter.spawner.DespawnAbilityShadowEffecter(abilityShadowEffecter);
        }
        if (_instance == null) return;
        if (_instance.HasWarnning && _instance.NeedPrepare && !_instance.HasServerConfirm)
        {
            Vector3 pos = ActorMoveFSM.LineCast(_instance.WarnningPos, !isDummy);
            pos = pos.SetY(pos.y + 10.0f);
           abilityShadowEffecter =  GameCenter.spawner.SpawnAbilityShadowEffecter(pos, _instance.warnningDir, _instance.WarnningWidth,
                _instance.WarnningLength, _instance.WarnningType, abilityWarnningColor, _instance.WarnningTime);
        }
        if (animFSM != null)
        {
            animFSM.Cast(_instance);
        }
        if (_instance.DirTurnType == TurnType.TURN)
        {
            FaceToNoLerp(_instance.DirY);
        }
		if(_instance.SkillReportID != 0)
			GameCenter.messageMng.AddClientMsg(_instance.SkillReportID);
        //StartCoroutine(DoAbilityHitEffect(_instance.BlinkLateTime / 1000f));
    }

    /// <summary>
    /// 技能移动,一般是后台确认后才能调用该方法 by吴江
    /// </summary>
    /// <param name="_instance"></param>
    public virtual void AbilityMove(AbilityInstance _instance)
    {
        if (moveFSM != null)
        {
            moveFSM.UseAbility(_instance);
        }
    }


    /// <summary>
    /// 计算被击特效的播放位置
    /// </summary>
    /// <param name="_from">施放者坐标</param>
    /// <param name="_accept">承受者坐标</param>
    /// <param name="_radius">承受者的碰撞半径</param>
    /// <returns></returns>
    public static Vector3 CaculateDefEffectPos(Vector3 _from,Vector3 _accept,float _radius,float _atkDis = 1.0f)
    {
        float dis = Vector3.Distance(_accept , _from);
        float atkRate = _atkDis / dis;
        Vector3 atkMaxPos = Vector3.Lerp(_from, _accept, atkRate);
        Vector3 defMaxPos = Vector3.Lerp(_accept, _from, _radius / dis);
        Vector3 showPos = _accept;
        if (atkRate > 1)//攻击极限距离大于两者距离,那么受打击者自身中心点作为特效点
        {
        }
        else
        {
            if ((_atkDis + _radius) < dis)
            {
                showPos = Vector3.Lerp(_accept, _from, _radius / dis);
            }
            else
            {
                showPos = (atkMaxPos + defMaxPos) / 2.0f;
            }
        }
        return showPos;
    }




    public virtual void BeHit(AbilityResultInfo _info)
    {
        if (animFSM != null)
        {
            animFSM.BeRigidity(_info.curRigidityTime);
            if (!isDead)
            {
                switch (_info.DefType)
                {
                    case DefResultType.DEF_SORT_KICK2:
                        animFSM.BeKickDown(_info.curMoveTime, _info.curRigidityTime);
                        break;
                    case DefResultType.DEF_SORT_KICK:
                        animFSM.BeKickFly(_info.curMoveTime, _info.curRigidityTime);
                        break;
                    default:
                        animFSM.BeHit(0.3f, 1.0f, _info.curDefAnimation);
                        break;
                }
            }
        }
//        if (_info.UserActor == GameCenter.curMainPlayer || this == GameCenter.curMainPlayer)
//        {
//            MainPlayerFocus = !isDead;
//        }
//        if (headTextCtrl != null && !IsActor && MainPlayerFocus)
//        {
//            if (isDead)
//            {
//                headTextCtrl.HideBlood();
//            }
//            else
//            {
//                headTextCtrl.SetBlood(actorInfo.MaxHP <= 0 ? 0 : (actorInfo.CurHP + _info.TotalDamage - _info.HasShowDamage) / (float)actorInfo.MaxHP, IsFriend);
//            }
//        }
        if (fxCtrl != null)
        {
            if (this.transform.position == _info.UserActor.transform.position)
            {
				fxCtrl.DoDefEffectFixedPosition(_info.curDefEffect, _info.curDefTime, Vector3.one, Quaternion.identity,HitPoint);
            }
            else
            {
				fxCtrl.DoDefEffectFixedPosition(_info.curDefEffect, _info.curDefTime, Vector3.one, Quaternion.LookRotation(this.transform.position - _info.UserActor.transform.position),HitPoint);
            }
        }
        if (moveFSM != null)
        {
            if (_info.curRigidityTime > 0)
            {
                moveFSM.StopMovingTo();
            }
            moveFSM.UseAbility(_info);
        }
    }


    /// <summary>
    /// 取消技能施放 by吴江
    /// </summary>
    /// <param name="_needNoticeServer"></param>
    public virtual void CancelAbility(bool _isServerNotice = false)
    {
        if (isCasting && !isRigidity)
        {
            if (moveFSM != null)
            {
                moveFSM.CancelAbility();
            }
            if (animFSM != null)
            {
                animFSM.StopCast();
            }
            curTryUseAbility = null;
            if (abilityShadowEffecter != null)
            {
                GameCenter.spawner.DespawnAbilityShadowEffecter(abilityShadowEffecter);
            }
        }
    }
    /// <summary>
    /// 停止技能移动,一般用于怪物,其他玩家,终止后仰击倒等状态
    /// </summary>
    public virtual void CancelAbilityMove(bool _isPathMove)
    {
        if (moveFSM != null)
        {
            if (_isPathMove)
            {
                moveFSM.StopMovingTo();
            }
            moveFSM.CancelAbility(_isPathMove);
        }
    }



    /// <summary>
    /// 强制结束技能，限制切换场景的时候使用
    /// </summary>
    public virtual void ForceCancelAbility()
    {
        if (moveFSM != null)
        {
            moveFSM.CancelAbility();
        }
        if (animFSM != null)
        {
            animFSM.StopCast();
        }
        curTryUseAbility = null;
        TickAnimation();
    }
    /// <summary>
    /// 清除buff
    /// </summary>
    protected virtual void OnCleanBuff()
    {
        CurRealSpeed = curMoveSpeed;
        HideModel(false);
    }
    /// <summary>
    /// buff状态变化
    /// </summary>
    /// <param name="_buffID"></param>
    /// <param name="_add"></param>
    protected virtual void OnBuffUpdate(int _buffID, bool _add)
    {
        //不会
        if (IsActor) return;
        BuffInfo info = actorInfo.GetBuffInfo(_buffID);
        //Debug.Log("_add:" + _add + ",_buffID:" + _buffID + ",ID:" + id + ",info.Sort:" + info.Sort);
        if (fxCtrl != null && info != null)
        {
            if (_add)
            {
                GameCenter.soundMng.PlaySound(info.AddSound, SoundMng.GetSceneSoundValue(transform, GameCenter.curMainPlayer.transform), false, true);
                switch (info.Sort)
                {
                    case BuffType.ATTRIBUTE:
                        if (info.Data1 == ActorPropertyTag.MOVESPD)
                        {
                            if (!(this is PlayerBase))
                                CurRealSpeed = Mathf.Max(MOVE_SPEED_BASE, CurRealSpeed + info.Value); //玩家添加buff不改变属性,改变属性由属性变化协议修改  By邓成
                            //Debug.Log("OnBuffUpdate _add:" + id + ",CurRealSpeed:" + CurRealSpeed);
                        }
                        break;
                    case BuffType.ATTRIBUTEPER:
						if (info.Data1 == ActorPropertyTag.MOVESPD)
						{
                            if (!(this is PlayerBase))
                                CurRealSpeed = Mathf.Max(MOVE_SPEED_BASE, CurRealSpeed + curMoveSpeed * info.Value / 10000f);
                            //Debug.Log("OnBuffUpdate _add:" + id + ",CurRealSpeed:" + CurRealSpeed);
						}
                        break;
					case BuffType.INVISIBLE:
						Show(false);
						if(actorInfo != null)actorInfo.UpdateHide(true);
						break;
                    //case BuffType.Dizzy:
                    //    Dizzy(true, _buffID);
                    //    if (info.ActorID == GameCenter.mainPlayerMng.MainPlayerInfo.ServerInstanceID)
                    //    {
                    //        // CoverSkillUI(true);
                    //    }
                    //    break;
                    //case BuffType.Lock:
                    //    moveFSM.LockMoving();
                    //    break;
                    //case BuffType.HideModel:
                    //    HideModel(true);
                    //    break;
                    //case BuffType.ModelSize:
                    //    //判断是否覆盖
                    //    //if ((info.Value / 10000f > 1 && info.Value / 10000f >= ScaleBuffValue) || (10000 / info.Value > 1f && 10000 / info.Value >= ScaleBuffValue))
                    //    //    ModelSizeUpdate(info);
                    //    break;
                    default:
                        break;
                }
                switch (info.ContrlType)
                {
                    case BuffControlSortType.FREEZE:
                        moveFSM.StopMovingTo();
                        moveFSM.LockMoving();
                        break;
                    case BuffControlSortType.FEAR:
                        ForceCancelAbility();
                        moveFSM.StopMovingTo();
                        break;
                    case BuffControlSortType.SLEEP:
                        ForceCancelAbility();
                        moveFSM.LockMoving();
                        Durative(true, info);
                        break;
                    case BuffControlSortType.BANISH:
                        moveFSM.StopMovingTo();
                        moveFSM.LockMoving();
                        break;
                    case BuffControlSortType.SILENT:
                        Silent(true, info);
                        break;
                    case BuffControlSortType.PERSISTENT:
                        break;
                    case BuffControlSortType.DURATIVE: ;
                        break;
                    case BuffControlSortType.STUN:
                        if (moveFSM != null)
                        {
                            moveFSM.CancelAbility();
                        }
                        if (animFSM != null)
                        {
                            animFSM.StopCast();
                        }
                        curTryUseAbility = null;
                        moveFSM.LockMoving();
                        if (info.ActorID == GameCenter.mainPlayerMng.MainPlayerInfo.ServerInstanceID)
                        {
                            // CoverSkillUI(true);
                        }
                        break;
                    default:
                        break;
                }
                Durative(true, info);
            }
            else
            {
                switch (info.Sort)
                {
                    case BuffType.ATTRIBUTE:
						if (info.Data1 == ActorPropertyTag.MOVESPD)
						{
                            if (!(this is PlayerBase))
	                            CurRealSpeed = Mathf.Max(MOVE_SPEED_BASE, CurRealSpeed - info.Value);
                            //Debug.Log("OnBuffUpdate:" + id + ",CurRealSpeed:" + CurRealSpeed);
						}
                        break;
                    case BuffType.ATTRIBUTEPER:
						if (info.Data1 == ActorPropertyTag.MOVESPD)
						{
                            if (!(this is PlayerBase))
	                            CurRealSpeed = Mathf.Max(MOVE_SPEED_BASE, CurRealSpeed - curMoveSpeed * info.Value / 10000f);
                            //Debug.Log("OnBuffUpdate:" + id + ",CurRealSpeed:" + CurRealSpeed);
						}
                        break;
					case BuffType.INVISIBLE:
						Show(true);
						if(actorInfo != null)actorInfo.UpdateHide(false);
						break;
                    //case BuffType.PropertyRate:
                    //    if (info.Data1 == 25)
                    //    {
                    //        CurRealSpeed = Mathf.Max(MOVE_SPEED_BASE, CurRealSpeed - curMoveSpeed * info.Value / 10000f);
                    //    }
                    //    break;
                    //case BuffType.Dizzy:
                    //    Dizzy(false, _buffID);
                    //    //if (info.ActorID == GameCenter.mainPlayerMng.MainPlayerInfo.ID)
                    //    //    CoverSkillUI(false);
                    //    break;
                    //case BuffType.HideModel:
                    //    HideModel(false);
                    //    break;
                    //case BuffType.ModelSize:
                    //    RefreshModelSize();
                    //    break;
                    default:
                        break;
                }
                switch (info.ContrlType)
                {
                    case BuffControlSortType.FREEZE:
                        CheckUnlockMove(_buffID);
                        break;
                    case BuffControlSortType.FEAR:
                        break;
                    case BuffControlSortType.SLEEP:
                        CheckUnlockMove(_buffID);
                        break;
                    case BuffControlSortType.BANISH:
                        CheckUnlockMove(_buffID);
                        break;
                    case BuffControlSortType.SILENT:
                        Silent(false, info);
                        break;
                    case BuffControlSortType.PERSISTENT:
                        break;
                    case BuffControlSortType.DURATIVE:
                        break;
                    case BuffControlSortType.STUN:
                        CheckUnlockMove(_buffID);
                        break;
                    default:
                        break;
                }
                Durative(false, info);
            }
        }
    }

    #endregion

    #region 辅助逻辑
    /// <summary>
    /// 当系统设置是否展示实时阴影变化时  by吴江
    /// </summary>
    protected void OnRealTimeShadowUpdate()
    {
        if (fxCtrl != null)
        {
            fxCtrl.DoShadowEffect(!SystemSettingMng.RealTimeShadow);
        }
        if (rendererCtrl != null)
        {
            rendererCtrl.CastShadow(SystemSettingMng.RealTimeShadow);
        }
    }
    /// <summary>
    /// 当系统设置是否展示实时血条变化时  by吴江
    /// </summary>
    protected void OnUpdateBloodSliderShow()
    {
        if (headTextCtrl != null)
        {
            if (GameCenter.systemSettingMng.ShowBloodSlider)
            {
            }
            else
            {
            //    headTextCtrl.HideBlood();
            }
        }
    }

    /// <summary>
    /// 检查是否仍然在禁止移动状态  by吴江
    /// </summary>
    /// <param name="_deleteBuffID"></param>
    protected void CheckUnlockMove(int _deleteBuffID)
    {
        FDictionary list2 = actorInfo.BuffList;
        if (list2 == null)
        {
            moveFSM.UnLockMoving();
        }
        else
        {
            bool hasOtherLock = false;
            foreach (BuffInfo item in list2.Values)
            {
                if (item.BuffTypeID == _deleteBuffID) continue;
                if (item.ContrlType == BuffControlSortType.STUN || item.ContrlType == BuffControlSortType.SLEEP
                    || item.ContrlType == BuffControlSortType.FREEZE || item.ContrlType == BuffControlSortType.BANISH)
                {
                    hasOtherLock = true;
                    break;
                }
            }
            if (!hasOtherLock)
            {
                moveFSM.UnLockMoving();
            }
        }
    }

    /// <summary>
    /// 设置是否进入战斗
    /// </summary>
    /// <param name="_combat"></param>
    public virtual void SetInCombat(bool _combat)
    {
        if (isDead) return;
        if (animFSM != null)
        {
            animFSM.SetInCombat(_combat);
        }
        if (rendererCtrl != null)
        {
            rendererCtrl.SetInCombat(_combat);
        }
    }
    public HeadTextCtrl GetHeadTextCtrl()
    {
        return headTextCtrl;
    }

    /// <summary>
    /// 死亡
    /// </summary>
    public virtual void Dead(bool _already = false)
    {
        if (actorInfo != null)
        {
            actorInfo.CleanUnFinishedAbilityResult();
        }
        if (isDead) return;
        if (GameCenter.curMainPlayer.CurTarget == this)
        {
            GameCenter.curMainPlayer.CurTarget = null;
        }
        MainPlayerFocus = false;
        if (moveFSM != null)
        {
            if (this == GameCenter.curMainPlayer)
            {
                Debug.logger.Log(this.gameObject.name + " Dead  LockMoving");
            }
            moveFSM.LockMoving();
        }
        ForceCancelAbility();
        ActiveBoxCollider(false,actorInfo.ColliderRadius);
        if (animFSM)
        {
            animFSM.Dead(_already);
        }

        isDead_ = true;

        if (headTextCtrl != null)
        {
            headTextCtrl.SetFlagsActive(false);
        }
        if (fxCtrl != null)
        {
            fxCtrl.DoShadowEffect(false);
        }
        //TO DO：播放死亡音效
    }

    /// <summary>
    /// 沉默 
    /// </summary>
    /// <param name="_silent"></param>
    /// <param name="_info"></param>
    protected void Silent(bool _silent, BuffInfo _info)
    {
        FDictionary list = actorInfo.BuffList;
        if (list == null || list.Count == 0)
        {
            IsSilent = false;
            return;
        }
        if (_silent)
        {
            ForceCancelAbility();
            IsSilent = true;
        }
        else
        {
            bool hasOtherSilent = false;
            foreach (BuffInfo item in list.Values)
            {
                if (item == _info) continue;
                if (item.ContrlType == BuffControlSortType.SILENT)
                {
                    hasOtherSilent = true;
                    break;
                }
            }
            if (!hasOtherSilent)
            {
                IsSilent = false;
            }
        }
    }

    /// <summary>
    /// 开始移动，执行移动动画
    /// </summary>
    public override void MoveStart()
    {
        if (animFSM)
        {
            animFSM.SetMoveSpeed(CurRealSpeed / (actorInfo.AnimationMoveSpeedBase * actorInfo.ModelScale));
            if (!animFSM.IsMoving)
            {
                animFSM.Move();
            }
        }
    }

    /// <summary>
    /// 持续行为 by吴江
    /// </summary>
    /// <param name="_silent"></param>
    /// <param name="_buffID"></param>
    protected void Durative(bool _durative, BuffInfo _info)
    {
        //会
        if (fxCtrl != null)
        {
            if (_durative)
            {
                fxCtrl.DoBuffEffect(_info.EffectName, animationRoot);
            }
            else
            {
                fxCtrl.HideBuffEffect(_info.EffectName);
            }


        }
        //会
        FDictionary list = actorInfo.BuffList;
        if (list == null || list.Count == 0)
        {
            moveFSM.UnLockMoving();
            return;
        }
        bool hasOtherDurative = false;
        string otherDurativeName = _durative ? _info.AnimName : string.Empty;
        int curLevel = _durative ? _info.AnimLevel : 0;
        foreach (BuffInfo item in list.Values)
        {
            if (item == _info) continue;
            if (item.AnimLevel > curLevel && item.AnimName.Length > 0)
            {
                otherDurativeName = item.AnimName;
                curLevel = item.AnimLevel;
                hasOtherDurative = true;
            }
        }
        if (animFSM != null)
        {
            if ((!_durative && !hasOtherDurative))
            {
                animFSM.StopDurative();
            }
            else if (otherDurativeName != string.Empty)
            {
                animFSM.Durative(otherDurativeName, 999999, WrapMode.Loop);  //这里对导致战士灭地技能切场景后继续做动作  by邓成
            }
        }
    }

    /// <summary>
    /// 播放拾取特效
    /// </summary>
    public void DoPickUpEffect(Vector3 _fromPos)
    {
        if (fxCtrl != null)
        {
            fxCtrl.DoPickUpEffect("e_s_u_021", 1.0f, _fromPos, this);
        }
    }


    //protected void CoverSkillUI(bool _cover)
    //{
    //    if (GameCenter.skillMng.onSkillCoverEvent != null)
    //        GameCenter.skillMng.onSkillCoverEvent(_cover);
    //}
    ///// <summary>
    ///// 添加模型改变buff
    ///// </summary>
    //protected virtual void ModelSizeUpdate(BuffInfo info)
    //{

    //}
	/// <summary>
	/// 还原模型大小
	/// </summary>
	protected virtual void RefreshModelSize()
	{

	}
    /// <summary>
    /// 技能隐身
    /// </summary>
    protected void HideModel(bool _hide)
    {
        if (_hide)
        {
            if (rendererCtrl != null) rendererCtrl.Show(false,true); 
        }
        else
        {
            if (rendererCtrl != null) rendererCtrl.Show(true); 
        }
    }
    public virtual void ReLive()
    {
        if (isDead)
        {
            ForceCancelAbility();
            //CurRealSpeed = curMoveSpeed;   //复活不重置速度,又后台buff通知
            ActiveBoxCollider(true,actorInfo.ColliderRadius);
            if (animFSM) animFSM.Idle();
            isDead_ = false;
            if (headTextCtrl != null) headTextCtrl.SetFlagsActive(true);
            if (fxCtrl != null)
            {
                fxCtrl.DoShadowEffect(true);
            }
            if (moveFSM != null)
            {
                moveFSM.UnLockMoving();
            }
            MainPlayerFocus = false;
//            if (headTextCtrl != null)
//            {
//                headTextCtrl.SetBlood(actorInfo.MaxHP <= 0 ? 0 : actorInfo.CurHP / (float)actorInfo.MaxHP, IsFriend);
//                headTextCtrl.HideBlood();
//            }
        }
    }
    /// <summary>
    ///生存状态发生改变的事件
    /// </summary>
    /// <param name="_alive"></param>
    protected virtual void OnAliveStateUpdate(bool _alive)
    {
        if (isDummy_) return;
        if (_alive)
        {
            ReLive();
        }
        else
        {
            //死亡以后不能马上倒地，只是不能再被选中。应当等技能效果播放完以后才倒地  by吴江
            ActiveBoxCollider(false);
            if (GameCenter.curMainPlayer.CurTarget == this)
            {
                GameCenter.curMainPlayer.CurTarget = null;
            }
        }
    }

    protected virtual void OnPropertyUpdate(ActorPropertyTag _tag, long _value, bool _fromAbility)
    {
        if (isDead)
        {
            if (headTextCtrl != null)
            {
            //    headTextCtrl.HideBlood();
            }
        }
		if (_fromAbility || IsActor)return;// || !MainPlayerFocus) return;
        switch (_tag)
        {
            case ActorPropertyTag.HPLIMIT:
//                if (headTextCtrl != null)
//                {
//                    headTextCtrl.SetBlood(actorInfo.MaxHP <= 0 ? 0 : actorInfo.CurHP / (float)actorInfo.MaxHP, IsFriend);
//                }
                break;
            default:
                break;
        }
    }

    protected virtual void OnBaseUpdate(ActorBaseTag _tag,ulong _value,bool _fromAbility)
    {
//		Debug.Log("OnBaseUpdate:"+_tag+",value:"+_value+",ID:"+actorInfo.ServerInstanceID+",IsDead:"+isDead);
        if (isDead)
        {
            if (headTextCtrl != null)
            {
            //    headTextCtrl.HideBlood();
            }
        }
		if (_fromAbility || IsActor)return;// || !MainPlayerFocus) return; 先去掉这个关注判断 现在自身复活血量没更新  by邓成
        switch (_tag)
        {
            case ActorBaseTag.CurHP:
                if (headTextCtrl != null)
                {
//                    if (isDead)
//                    {
//                        headTextCtrl.HideBlood();
//                    }
//                    else
//                    {
//                        headTextCtrl.SetBlood(actorInfo.MaxHP <= 0 ? 0 : actorInfo.CurHP / (float)actorInfo.MaxHP, IsFriend);
//                    }
                }
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 限时移动到某地 by吴江
    /// </summary>
    /// <param name="_pos"></param>
    /// <param name="_time"></param>
    public void LerpToPos(Vector3 _pos,float _time,string _animName)
    {
        if (moveFSM != null)
        {
            moveFSM.StopMovingTo();
            moveFSM.LerpToDest(_pos, _time);
        }
        if (animFSM != null && _animName != string.Empty && _time > 0)
        {
            animFSM.Durative(_animName, _time,WrapMode.Once);
        }
    }

    /// <summary>
    /// 展示/隐藏基本模型
    /// </summary>
    /// <param name="_show"></param>
    public override void Show(bool _show)
    {
        if (!isShowing)
        {
            if (animFSM != null)
            {
                if (_show)
                {
                    TickAnimation();
                }
            }
        }
        base.Show(_show);
    }


    public virtual void TickAnimation()
    {
        if (animFSM != null)
        {
            animFSM.StopAnim();
            animFSM.ReStart();
            if (actorInfo.IsAlive)
            {
                if (moveFSM != null)
                {
                    if (moveFSM.isMoving)
                    {
                        animFSM.Move();
                    }
                    else
                    {
                        animFSM.Idle();
                    }
                }
            }
            else
            {
                if (animFSM != null)
                {
                    animFSM.Dead(true);
                }
            }
        }
    }


    protected override void OnUpdateHide(bool _hide)
    {
        base.OnUpdateHide(_hide);
        if (!_hide)
        {
            TickAnimation();
        }
    }
    #endregion

}
