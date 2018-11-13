//===================================================
//作者：吴江
//日期：2015/5/10
//用途：玩家角色基类
//====================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


/// <summary>
/// 玩家角色基类 by吴江
/// </summary>

public class PlayerBase : SmartActor
{
    #region 数据
    /// <summary>
    /// 渲染控制器
    /// </summary>
    [System.NonSerialized]
    public new PlayerRendererCtrl rendererCtrl = null;
    /// <summary>
    /// 动画控制器
    /// </summary>
    [System.NonSerialized]
    protected new PlayerAnimFSM animFSM = null;
    /// <summary>
    /// 技能施放管理器
    /// </summary>
    //[System.NonSerialized]
    // public AbilityMng abilityMng = null;
    /// <summary>
    /// 数据层对象引用
    /// </summary>
    protected new PlayerBaseInfo actorInfo
    {
        get { return base.actorInfo as PlayerBaseInfo; }
        set
        {
            base.actorInfo = value;
            if (value != null) id = actorInfo.ServerInstanceID;
        }
    }



    /// <summary>
    /// 所属层
    /// </summary>
    public virtual string ObjLayer
    {
        get
        {
            return "Player";
        }
    }
    /// <summary>
    /// 等级
    /// </summary>
    public ulong Level
    {
        get
        {
            return actorInfo == null ? 0 : actorInfo.Level;
        }
    }
    ///// <summary>
    ///// 头像图片
    ///// </summary>
    //public string IconName
    //{
    //    get { return actorInfo.IconName; }
    //}
    ///// <summary>
    ///// 职业名称
    ///// </summary>
    //public string ClassName
    //{
    //    get { return actorInfo.ClassName; }
    //}
    /// <summary>
    /// 职业ID
    /// </summary>
    public int Prof
    {
        get { return actorInfo.Prof; }
    }

    public bool IsInSafetyArea
    {
        get
        {
            return actorInfo.IsInSafetyArea;
        }
    }

    public PkMode CurPkMode
    {
        get
        {
            return actorInfo.CurPkMode;
        }
    }

    public string GuildName
    {
        get
        {
            return actorInfo.GuildName;
        }
    }

    public int SlaSevel
    {
        get
        {
            return actorInfo.SlaLevel;
        }
    }

	public bool IsCounterAttack
	{
		get
		{
			return actorInfo.IsCounterAttack;
		}
	}



    /// <summary>
    /// 默认普通攻击技能列表
    /// </summary>
    //public List<string> DefaultAbilityEffectList
    //{
    //    get
    //    {
    //        if (abilityMng == null)
    //        {
    //            //    Debug.LogError("技能管理类尚未初始化！");
    //            return null;
    //        }
    //        return abilityMng.GetDefaultAbilityEffectNames();
    //    }
    //}
    /// <summary>
    /// 不影响到外形的首饰部位
    /// </summary>
    protected EquipSlot[] jewelrySlots = { EquipSlot.ring, EquipSlot.necklace, EquipSlot.necklace, EquipSlot.badge, EquipSlot.bracers };

    /// <summary>
    /// 灵魂
    /// </summary>
    protected GameObject soulObject = null;

    protected Mount myMount;
    #endregion

    #region UNITY
    /// <summary>
    /// dummy状态也需要准确的typeID 因此使用Awake() by吴江
    /// </summary>
    protected new void Awake()
    {
        typeID = ObjectType.Player;
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
        animFSM = base.animFSM as PlayerAnimFSM;
        if (animFSM == null)
        {
            animFSM = this.gameObject.GetComponentInChildrenFast<PlayerAnimFSM>(true);
        }

        rendererCtrl = base.rendererCtrl as PlayerRendererCtrl;
        if (rendererCtrl == null)
        {
            rendererCtrl = this.gameObject.GetComponentInChildrenFast<PlayerRendererCtrl>(true);
        }



        ActiveBoxCollider(true, actorInfo.ColliderRadius);
        rendererCtrl.Init(actorInfo, fxCtrl);
        animationRoot.gameObject.transform.localScale *= actorInfo.ModelScale;

        if (animFSM != null)
        {
            animFSM.SetMoveSpeed(CurRealSpeed / (actorInfo.AnimationMoveSpeedBase * actorInfo.ModelScale));
            animFSM.OnDeadEnd = OnDeadEnd;
        }

        ReBuildMount();
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
        if (IsShowing && fxCtrl != null)
        {
            //fxCtrl.DoDeadEffect("die_B02");
            if (actorInfo.ServerInstanceID == GameCenter.mainPlayerMng.MainPlayerInfo.ServerInstanceID)
            {
                //  GameCenter.uIMng.SwitchToUI(GUIType.DEADWND);
            }
            //if (actorInfo.IsOnMount)
            //{
            //    fxCtrl.DoRideEffect("player_mount_B02");
            //}
        }
    }

    /// <summary>
    /// 注册事件监听
    /// </summary>
    protected override void Regist()
    {
        base.Regist();
        if (actorInfo != null)
        {
            actorInfo.OnProfUpdate += ReStartAsyncCreate;
            actorInfo.OnNameUpdate += OnNameChange;
            actorInfo.OnCurShowEquipUpdate += EquipUpdate;
            actorInfo.onGuildNameUpdate += OnGuildNameChange;//公会名字改变的事件添加 by 易睿
            actorInfo.OnProfUpdate += OnProfDolevelupeffect;//转职成功特效 
            actorInfo.OnMountUpdate += MountUpdate;
            actorInfo.OnMountRideStateUpdate += RideMount;
            actorInfo.onStartCollectEvent += StartCollect;
            actorInfo.onEndCollectEvent += EndCollect;
            actorInfo.OnTitleUpdate += OnTitleUpdate;
            actorInfo.OnClothesFashionUpdate += OnClothesFashionUpdate;
            actorInfo.OnWeaponFashionUpdate += OnWeaponFashionUpdate;
			actorInfo.OnBaseUpdate += OnNameColorUpdate;
            GameCenter.systemSettingMng.OnUpdateRenderQuality += OnUpdateRenderQuality;
            actorInfo.OnPropertyUpdate += UpdateSpeed;
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
            actorInfo.OnProfUpdate -= ReStartAsyncCreate;
            actorInfo.OnNameUpdate -= OnNameChange;
            actorInfo.OnCurShowEquipUpdate -= EquipUpdate;
            actorInfo.onGuildNameUpdate -= OnGuildNameChange;
            actorInfo.OnProfUpdate -= OnProfDolevelupeffect;
            actorInfo.OnMountUpdate -= MountUpdate;
            actorInfo.OnMountRideStateUpdate -= RideMount;
            actorInfo.onStartCollectEvent -= StartCollect;
            actorInfo.onEndCollectEvent -= EndCollect;
            actorInfo.OnTitleUpdate -= OnTitleUpdate;
            actorInfo.OnClothesFashionUpdate -= OnClothesFashionUpdate;
            actorInfo.OnWeaponFashionUpdate -= OnWeaponFashionUpdate;
			actorInfo.OnBaseUpdate -= OnNameColorUpdate;
            GameCenter.systemSettingMng.OnUpdateRenderQuality -= OnUpdateRenderQuality;
        }
    }
    #endregion

    #region 辅助逻辑
    protected override void OnRealSpeedUpdate()
    {
        base.OnRealSpeedUpdate();
        if (actorInfo.CurMountInfo != null && actorInfo.CurMountInfo.IsRiding)
        {
            if (myMount != null)
            {
                myMount.UpdateAnimSpeed();
            }
        }
    }

    protected void UpdateSpeed(ActorPropertyTag _tag, long _speed, bool _state)
    {
        if (_tag != ActorPropertyTag.MOVESPD) return;
        if (actorInfo.Movespd != 0)
        {
            CurRealSpeed = Mathf.Max(SmartActor.MOVE_SPEED_BASE, (float)actorInfo.StaticSpeed * (float)actorInfo.Movespd / 100f);
        }
    }

    public override void PositionChange()
    {
        if (IsActor) return;
        base.PositionChange();
        SceneMng mng = GameCenter.sceneMng;
		if(mng != null)
		{
			if (!mng.HasSafetyArea)
			{
				actorInfo.IsInSafetyArea = false;
			}else
			{
				Vector2 center = mng.SafetyAreaCenter;
				Vector2 mypos = new Vector2(this.transform.position.x, this.transform.position.z);
				actorInfo.IsInSafetyArea = (mypos - center).sqrMagnitude < mng.SafetyAreaRadius * mng.SafetyAreaRadius;
			}
		}
    }


	protected override void InitAnimation ()
	{
		base.InitAnimation ();
		if(animFSM != null)
		{
			animFSM.SetupIdleAndMoveAnimationName("idle2", "move2");
			animFSM.SetupCombatAnimationName("idle2", null);
		}
	}


    /// <summary>
    /// 更新衣服时装
    /// </summary>
    protected void OnClothesFashionUpdate()
    {



    }
    /// <summary>
    /// 更新武器时装
    /// </summary>
    protected void OnWeaponFashionUpdate()
    {



    }




    protected void OnTitleUpdate()
    {
        if (headTextCtrl != null)
        {
            headTextCtrl.SetTitleSprite(actorInfo.TitleIcon);
        }
    }

    protected virtual void OnUpdateRenderQuality(SystemSettingMng.RendererQuality _quality)
    {
        if (rendererCtrl != null) rendererCtrl.OnUpdateRenderQuality(_quality);
    }

    /// <summary>
    /// 跳跃
    /// </summary>
    public virtual void Jump()
    {
        if (isDummy) return;
        if (animFSM != null)
        {
            animFSM.Jump();
        }
    }


    protected bool isCollecting = false;
    /// <summary>
    /// 是否正在采集中
    /// </summary>
    public bool IsCollecting
    {
        get
        {
            return isCollecting;
        }
    }

    /// <summary>
    /// 开始读条交互表现
    /// </summary>
    /// <param name="_info"></param>
    protected void StartCollect(SceneItemInfo _info)
    {
        if (IsActor) return;
        isCollecting = true;
        if (animFSM != null && _info.PlayerAnimName.Length > 0)
        {
            animFSM.Collect(_info.PlayerAnimName, 99);
        }
        if (headTextCtrl != null && _info.OpenTime > 0)
        {
            headTextCtrl.SetProgress(_info.OpenTime, _info.OpenDescription);
        }
    }
    protected void StartCollect(SustainRef _info)
    {
        isCollecting = true;
        if (animFSM != null && _info.action != string.Empty)
        {
			animFSM.Collect(_info.action,_info.time);
        }
        if (headTextCtrl != null && _info.time > 0)
        {
            headTextCtrl.SetProgress(_info.time / 1000, _info.text);
        } 
        if (fxCtrl != null && !string.IsNullOrEmpty(_info.eff))
        { 
            fxCtrl.DoNormalEffect(_info.eff);
        }
    }
    /// <summary>
    /// 停止读条交互表现
    /// </summary>
    /// <param name="_itemID"></param>
    /// <param name="_result"></param>
    protected virtual void EndCollect()
    {
        if (IsActor) return;
        isCollecting = false;
        if (animFSM != null)
        {
			//animFSM.StopDurative();
            //if (moveFSM != null && moveFSM.isMoving)
            //{
            //    animFSM.Move();
            //}
            //else
            //{
            //    animFSM.Idle();
            //}
            animFSM.StopCollect(moveFSM != null && moveFSM.isMoving);
        }
        if (headTextCtrl != null )
        {
            headTextCtrl.EndProgress();
        }
        if (fxCtrl != null)
        {
            fxCtrl.ClearNormalEffect();
        }
    }

    /// <summary>
    /// 重新实例化坐骑
    /// </summary>
    protected virtual void ReBuildMount()
    {
        if (myMount != null)
        {
            if (myMount.isDownloading)
            {
                myMount.CancelDownLoad();
            }
            Destroy(myMount);
            animationRoot.parent = this.transform;
            animationRoot.localPosition = Vector3.zero;
            animationRoot.localEulerAngles = Vector3.zero;
        }
        if (typeID == ObjectType.PreviewPlayer) return;
        if (actorInfo.CurMountInfo == null) return;
        myMount = Mount.CreateDummy(actorInfo.CurMountInfo);
        myMount.transform.parent = this.transform;
        myMount.transform.localPosition = Vector3.zero;
        myMount.transform.localScale = Vector3.one;
        myMount.transform.localEulerAngles = Vector3.zero;
        myMount.StartAsyncCreate((x, y) =>
        {
            if (y == EResult.Success)
            {
                x.gameObject.SetMaskLayer(this.gameObject.layer);
                if (actorInfo.CurMountInfo.IsRiding)
                {
                    GameObject ass = Mount.AssPositionObj(x.gameObject, actorInfo.CurMountInfo.SeatPointName);
                    if (ass != null)
                    {
                        animationRoot.parent = ass.transform;
						animationRoot.localPosition = Vector3.zero;
						animationRoot.localEulerAngles = Vector3.zero;
                    }
                    else
                    {
                        Debug.LogError("坐骑资源上找不到挂点!");
                    }
                    if (headTextCtrl != null)
                    {
                        headTextCtrl.CaculatPos(NameHeight + actorInfo.CurMountInfo.NameHightDiff);
                    }
                    else
                    {
                        Debug.LogError("创建坐骑成功,但找不到文字组件,可能导致文字高度错误!");
                    }
                }
                x.OnMountStateUpate(actorInfo.CurMountInfo == null ? false : actorInfo.CurMountInfo.IsRiding, moveFSM == null ? false : moveFSM.isMoving, IsShowing);
                if (!isDead)
                {
                    animFSM.OnMount(actorInfo.CurMountInfo.IsRiding);
                    SetInCombat(!actorInfo.CurMountInfo.IsRiding && GameCenter.curGameStage as CityStage == null);
                }
            }
            else
            {
                Debug.LogError("坐骑模型创建失败!");
            }
        });
    }
    /// <summary>
    /// 开始移动，执行移动动画
    /// </summary>
    public override void MoveStart()
    {
        if (animFSM)
        {
            if (actorInfo.CurMountInfo != null && actorInfo.CurMountInfo.IsRiding)
            {
                animFSM.MountMove();
                if (myMount != null)
                {
                    myMount.MoveStart();
                }
            }
            else
            {
                animFSM.Move();
            }
        }
    }
    /// <summary>
    /// 移动结束，执行站立动画
    /// </summary>
    public override void MoveEnd()
    {
        if (animFSM)
        {
            if (actorInfo.CurMountInfo != null && actorInfo.CurMountInfo.IsRiding)
            {
                animFSM.MountStopMoving();
                if (myMount != null)
                {
                    myMount.MoveEnd();
                }
            }
            else
            {
                animFSM.StopMoving();
            }
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
    /// 重建模型，一般是因为转职
    /// </summary>
    protected virtual void ReStartAsyncCreate(int _prof)
    {
        if (IsActor) return;
        isDummy_ = true;
        if (myMount != null)
        {
            DestroyImmediate(myMount.gameObject);
            myMount = null;
        }
        if (animationRoot != null && animationRoot.gameObject != null)
        {
            DestroyImmediate(animationRoot.gameObject);
            animationRoot = null;
        }
        if (myMount != null)
        {
            DestroyImmediate(myMount);
            myMount = null;
        }
        for (int i = 0; i < this.transform.childCount; i++)
        {
            GameObject obj = this.transform.GetChild(i).gameObject;
            if (obj != null)
            {
                DestroyImmediate(obj);
            }
        }
        UnRegist();
    }

    public override Transform GetReceivePoint()
    {
        if (receivePoint == null)
        {
            base.GetReceivePoint();
            receivePoint.localPosition = new Vector3(0, actorInfo.NameHeight / 2.0f, 0);
        }
        return receivePoint;
    }

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
        actorInfo.IsInFight = _combat;
    }

    protected override void OnAliveStateUpdate(bool _alive)
    {
        if (isDummy) return;
        if (_alive == isDead)
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
            headTextCtrl.SetName(_newName);
        }
    }
    /// <summary>
    /// 公会名字变化
    /// </summary>
    protected void OnGuildNameChange(string _guildName)
    {
        if (headTextCtrl != null)
        {
            headTextCtrl.SetGuildName(_guildName);
        }
    }

	protected void OnNameColorUpdate(ActorBaseTag _tag,ulong _val,bool _state)
	{
		if(_tag == ActorBaseTag.CounterAttack)
		{
			if (headTextCtrl != null)
			{
				headTextCtrl.SetNameColorAlongInterActive();
			}
		}
	}

    protected override void OnBaseUpdate(ActorBaseTag _tag, ulong _value, bool _fromAbility)
    {
        base.OnBaseUpdate(_tag, _value, _fromAbility);
        switch (_tag)
        {
			case ActorBaseTag.SLALEVEL://杀戮等级变化会有名字颜色的变化
                if (headTextCtrl != null)
                {
                    headTextCtrl.SetName(actorInfo.Name);
                }
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 展示/隐藏基本模型
    /// </summary>
    /// <param name="_show"></param>
    public override void Show(bool _show)
    {
        base.Show(_show);
        if (myMount != null)
        {
            myMount.Show(_show && actorInfo.CurMountInfo != null && actorInfo.CurMountInfo.IsRiding);
        }
    }
    /// <summary>
    /// 切换场景改变公会名字显示，副本中不显示 by 
    /// </summary>
    protected void BaseUpdate(ActorBaseTag _tag, int _sceneId)
    {
        switch (_tag)
        {
            case ActorBaseTag.CurHP:
                break;
            case ActorBaseTag.CurMP:
                break;
            case ActorBaseTag.Diamond:
                break;
            case ActorBaseTag.Level:
                if (fxCtrl != null) fxCtrl.DoLevelUPEffect("lvlup_B02");
                break;
            case ActorBaseTag.Exp:
                break;
            case ActorBaseTag.TOTAL:
                break;
            default:
                break;
        }
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
    /// 切换坐骑
    /// </summary>
    protected virtual void MountUpdate()
    {
        if (isDummy_) return;
        ReBuildMount();
    }
    /// <summary>
    /// 上坐骑
    /// </summary>
    public virtual void RideMount(bool _ride, bool _isChange)
    {
        if (isDummy_ || isDead) return;
        if (_ride && actorInfo.CurMountInfo == null)
        {
            return;
        }
        animFSM.OnMount(_ride);
        SetInCombat(!_ride && GameCenter.curGameStage as CityStage == null);
        if (_ride)
        {
            if (myMount != null && myMount.ConfigID == actorInfo.CurMountInfo.ConfigID)
            {
                myMount.transform.parent = this.transform;
                myMount.transform.localPosition = Vector3.zero;
                myMount.transform.localScale = Vector3.one;
                myMount.transform.localEulerAngles = Vector3.zero;
                myMount.OnMountStateUpate(true, moveFSM == null ? false : moveFSM.isMoving, IsShowing);
                GameObject ass = Mount.AssPositionObj(myMount.gameObject, actorInfo.CurMountInfo.SeatPointName);
                if (ass != null)
                {
                    if (animationRoot != null)
                    {
                        animationRoot.parent = ass.transform;
						Debug.Log("actorInfo.CenterDiff:"+actorInfo.CenterDiff);
						animationRoot.localPosition = Vector3.zero;
						animationRoot.localEulerAngles = Vector3.zero;
                    }
                    else
                    {
                        Debug.LogError("动画根节点为空!");
                    }
                }
                else
                {
                    Debug.LogError("坐骑资源上找不到挂点!");
                }
            }
            if (headTextCtrl != null)
            {
                headTextCtrl.CaculatPos(NameHeight + actorInfo.CurMountInfo.NameHightDiff);
            }
            else
            {
                Debug.LogError("创建坐骑成功,但找不到文字组件,可能导致文字高度错误!");
            }
        }
        else
        {
            if (animationRoot != null)
            {
                animationRoot.parent = this.transform;
                animationRoot.localPosition = Vector3.zero;
                animationRoot.localEulerAngles = Vector3.zero;
            }
            else
            {
                Debug.LogError("动画根节点为空");
            }
            if (myMount != null)
            {
                myMount.OnMountStateUpate(false, moveFSM == null ? false : moveFSM.isMoving, IsShowing);
            }
            if (headTextCtrl != null)
            {
                headTextCtrl.CaculatPos(NameHeight);
            }
            else
            {
                Debug.LogError("创建坐骑成功,但找不到文字组件,可能导致文字高度错误!");
            }
        }
        if (IsShowing && _isChange && fxCtrl != null)
        {
            //fxCtrl.DoRideEffect("player_mount_B02");
        }
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
            GameCenter.curGameStage.PlaceGameObjectFromServer(this, actorInfo.ServerPos.x, actorInfo.ServerPos.z, (int)this.transform.localEulerAngles.y);
            Show(true);
            if (soulObject != null)
            {
                soulObject.SetActive(false);
            }
            if (animFSM != null)
            {
                animFSM.RandIdle();
            }
            if (actorInfo.CurMountInfo != null && actorInfo.CurMountInfo.IsRiding && myMount != null)
            {
                GameCenter.newMountMng.C2S_ReqRideMount(ChangeMount.DOWNRIDE, actorInfo.CurMountInfo.ConfigID, MountReqRideType.AUTO);
                RideMount(false, true);
            }
        }
    }


    public override void TickAnimation()
    {
        base.TickAnimation();
        if (actorInfo.CurMountInfo != null && actorInfo.CurMountInfo.IsRiding && !IsActor)
        {
            if (moveFSM != null)
            {
                if (moveFSM.isMoving)
                {
                    animFSM.MountMove();
                    rendererCtrl.SetInCombat(false);
                }
                else
                {
                    animFSM.OnMount(true);
                    rendererCtrl.SetInCombat(false);
                }
            }
            if (!actorInfo.IsAlive)
            {
                animFSM.Dead();
            }
        }
    }


     public override void Dead(bool _already = false)
     {
        if (myMount != null)
         {
             RideMount(false, true);
         }
         base.Dead(_already);
     }
    #endregion
}
