//===============================================
//作者：吴江
//日期：2015/7/28
//用途：怪物的表现层对象
//===============================================




using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 怪物的表现层对象 by吴江
/// </summary>
public class Monster : SmartActor
{
    #region 数据
    /// <summary>
    /// 完整数据层对象的引用 by吴江
    /// </summary>
    public new MonsterInfo actorInfo
    {
        get { return base.actorInfo as MonsterInfo; }
        set
        {
            base.actorInfo = value;
        }
    }
    /// <summary>
    /// 对象动画控制器  by吴江 
    /// </summary>
    [System.NonSerialized]
    protected new MobAnimFSM animFSM = null;
    ///<summary>
    ///对象渲染控制器 by吴江
    ///</summary>
    [System.NonSerialized]
    public new MobRendererCtrl rendererCtrl = null;
    /// <summary>
    /// 是否为boss
    /// </summary>
    public bool IsBoss
    {
        get
        {
            return actorInfo == null ? false : actorInfo.IsBoss;
        }
    }

    public bool IsDart
    {
        get
        {
            return actorInfo == null ? false : actorInfo.IsDart;
        }
    }
    public bool IsOwnerDart
    {
        get
        {
            return actorInfo == null ? false : actorInfo.IsOwnerDart;
        }
    }
    /// <summary>
    /// 客户端创建时间(不包含虚拟体)
    /// </summary>
    protected float creatTime = -1;
    /// <summary>
    /// 默认创建状态持续时间
    /// </summary>
    protected float waitCreatTime = 3.0f;
    /// <summary>
    /// 模型对象 by吴江 
    /// </summary>
    protected GameObject mobModel;

    public int ConfigID
    {
        get
        {
            return actorInfo.Prof;
        }
    }
    public string IconName
    {
        get { return actorInfo.IconName; }
    }
    public int ScaleType
    {
        get { return actorInfo.ScaleType; }
    }
    #endregion

    #region 构造
    /// <summary>
    /// 创建净数据对象 by吴江
    /// </summary>
    /// <param name="_info"></param>
    /// <returns></returns>
    public static Monster CreateDummy(MonsterInfo _info)
    {
        GameObject newGO = null;
        if (GameCenter.instance.dummyMobPrefab != null)
        {
            newGO = Instantiate(GameCenter.instance.dummyMobPrefab) as GameObject;
            newGO.name = "Dummy Monster [" + _info.ServerInstanceID + "]";
        }
        else
        {
            newGO = new GameObject("Dummy Monster[" + _info.ServerInstanceID + "]");
        }
        newGO.tag = "Monster";
        newGO.SetMaskLayer(LayerMask.NameToLayer("Monster"));
        Monster mob = newGO.AddComponent<Monster>();
        mob.actorInfo = _info;
        mob.isDummy_ = true;
        mob.moveFSM = newGO.AddComponent<ActorMoveFSM>();
        mob.curMoveSpeed = mob.actorInfo.StaticSpeed * MOVE_SPEED_BASE;
        mob.CurRealSpeed = mob.curMoveSpeed;
        mob.RegistMoveEvent(true);
        mob.creatTime = Time.time;
        GameCenter.curGameStage.PlaceGameObjectFromServer(mob, _info.ServerPos.x, _info.ServerPos.z, _info.RotationY);
        GameCenter.curGameStage.AddObject(mob);

        return mob;
    }

    /// <summary>
    /// 异步创建实体模型  by吴江 
    /// </summary>
    /// <param name="_isSpawn"></param>
    public void StartAsyncCreate(bool _isSpawn = false)
    {
        StartCoroutine(CreateAsync(_isSpawn));
    }
    /// <summary>
    /// 异步创建协程  by吴江 
    /// </summary>
    /// <param name="_isSpawn"></param>
    /// <returns></returns>
    IEnumerator CreateAsync(bool _isSpawn)
    {
        if (isDummy_ == false)
        {
            Debug.LogError("You can only start create Mob in dummy: " + actorInfo.ServerInstanceID);
            yield break;
        }

        Monster mob = null;
        MobRendererCtrl myRendererCtrl = null;
        bool failed = false;
        pendingDownload = Create(actorInfo, delegate(Monster _mob, EResult _result)
        {
            if (_result != EResult.Success)
            {
                failed = true;
                return;
            }
            mob = _mob;
            pendingDownload = null;

            myRendererCtrl = mob.gameObject.GetComponentInChildrenFast<MobRendererCtrl>();
            if (myRendererCtrl != null)
                myRendererCtrl.Show(false);

        });
        while (mob == null || mob.inited == false)
        {
            if (failed) yield break;
            yield return null;
        }
        pendingDownload = null;

        if (GameCenter.curGameStage == null)
        {
            yield break;
        }

        if (!actorInfo.IsAlive)
        {
            mob.Dead(true);
        }

        if (_isSpawn)
        {
            if (moveFSM != null && !moveFSM.isMoving && (ulong)actorInfo.MaxHP == actorInfo.CurHP)
            {
                if (fxCtrl != null && actorInfo.BornEffect != string.Empty && actorInfo.BornEffect != null)
                {
                    //构建中
                    fxCtrl.DoSpawnEffect(actorInfo.BornEffect);
                }
                if (animFSM != null && actorInfo.BornAnim != string.Empty && actorInfo.BornAnim != null)
                {
                    animFSM.SetCreateAnimationName(actorInfo.BornAnim, null);
                   // animFSM.Creating();
                }
            }
        }

        //GetBufInfo();   // 如果是dummy的话，添加buff会无效，因此要在创建好后再请求
        mob.isDownloading_ = false;
    }
    protected AssetMng.DownloadID Create(MonsterInfo _info, System.Action<Monster, EResult> _callback)
    {
        return exResources.GetMob(_info.Prof, delegate(GameObject _asset, EResult _result)
        {
            if (GameCenter.IsDummyDestroyed(ObjectType.MOB, _info.ServerInstanceID))
            {
                _callback(null, EResult.Failed);
                return;
            }
            if (_result != EResult.Success)
            {
                _callback(null, _result);
                return;
            }

            this.gameObject.name = "Monster[" + _info.ServerInstanceID + "]";

            GameObject newGO = Instantiate(_asset) as GameObject;
            newGO.name = _asset.name;
            newGO.transform.parent = this.gameObject.transform;
            newGO.transform.localEulerAngles = Vector3.zero;
            newGO.transform.localPosition = Vector3.zero;
            newGO.transform.localScale = Vector3.one * _info.ModelScale * scaleBuffValue;
            newGO.AddComponent<MobAnimFSM>();
            newGO.AddComponent<MobRendererCtrl>();
            newGO.AddComponent<FXCtrl>();
            newGO.SetActive(false);
            newGO.SetActive(true);
            mobModel = newGO;


            isDummy_ = false;
            animationRoot = newGO.transform;

            Init();
            _callback(this, _result);
        });
    }
    #endregion

    #region UNITY
    protected new void Awake()
    {
        if (typeID == ObjectType.Unknown)
            typeID = ObjectType.MOB;
        base.Awake();
    }
    /// <summary>
    /// 勿删，Start（）实际在dummy状态被调用，要在启动时做的事情请去自定义的Init()接口 by吴江
    /// </summary>
    void Start()
    {
    }
    new void OnDestroy()
    {
        base.OnDestroy();
        FDictionary dic = GameCenter.sceneMng.MobInfoDictionary;
        bool got = false;
        foreach (MonsterInfo item in dic.Values)
        {
            if (item.Prof == actorInfo.Prof)
            {
                got = true;
                break;
            }
        }
        if (!got)//如果场景中已经没有同类怪了，那么卸载资源
        {
            AssetMng.instance.UnloadUrl(AssetMng.GetPathWithExtension(actorInfo.BoneName, AssetPathType.PersistentDataPath));
        }
    }
    #endregion

    #region 辅助逻辑
    protected override void Init()
    {
        base.Init();
        animFSM = base.animFSM as MobAnimFSM;
        rendererCtrl = this.gameObject.GetComponentInChildrenFast<MobRendererCtrl>();
        rendererCtrl.ResetOriginalLayer(LayerMask.NameToLayer("Monster"));
        rendererCtrl.SetLayer(this.gameObject.layer);
        if (actorInfo.CurShowDictionary.Count > 0)
        {
            rendererCtrl.Init(actorInfo, fxCtrl);
        }
        rendererCtrl.OnCombineSuceess += OnCombineSucess;
        height = actorInfo.Hight;
        nameHeight = actorInfo.NameHight;
        MainPlayerFocus = true;
        beAttackRadius = actorInfo.ColliderRadius;
        if (headTextCtrl == null) headTextCtrl = this.gameObject.GetComponent<HeadTextCtrl>();
        if (headTextCtrl == null) headTextCtrl = this.gameObject.AddComponent<HeadTextCtrl>();
        if (!actorInfo.IsActor) //过场动画演员不展示名字
        {
            UpdateName();
        }

        inited_ = true;

        if (animFSM != null)
        {
            InitAnimation();
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
            Debug.LogError("怪物" + gameObject.name + "没有动画组件!");
        }

        if (rendererCtrl)
        {
            rendererCtrl.Show(false, true);
        }
        ActiveBoxCollider(true, actorInfo.ColliderRadius);
		if(fxCtrl != null)
		{
			if(actorInfo.IsBoss)
			{
                fxCtrl.DoBossEffect(actorInfo.RingSize, "m_f_006");
			}
            if (actorInfo.IsElite)
            {
                fxCtrl.DoBossEffect(actorInfo.RingSize, "zb_qh_dj_005");
            }
			if(actorInfo.IsSedan)
			{
				fxCtrl.DoSedanEffect();
			}
		}
        stateMachine.Start();

    }

    /// <summary>
    /// 更新怪物名字
    /// </summary>
    public void UpdateName()
    {
        if (!GameCenter.systemSettingMng.MonsterName)
        {
            if (headTextCtrl != null)
                headTextCtrl.SetName("");
        }
        else
        {
            if (headTextCtrl != null)
            { 
                headTextCtrl.SetName(actorInfo.Name);
            }
        }

    }
    protected void OnCombineSucess()
    {
        rendererCtrl.SetCustumColor(actorInfo.CustumColor);
    }

    protected override void Regist()
    {
        base.Regist();
        actorInfo.OnOwnerUpdate += OnOwnerUpdate;
    }

    public override void UnRegist()
    {
        base.UnRegist();
        actorInfo.OnOwnerUpdate -= OnOwnerUpdate;
    }

    protected override void InitAnimation()
    {
        base.InitAnimation();
		if(animFSM != null)
		{
			animFSM.SetupIdleAndMoveAnimationName("idle2", "move2");
			animFSM.SetupCombatAnimationName("idle2", null);
		}
        float baseValue = actorInfo.AnimationMoveSpeedBase * actorInfo.ModelScale;
        if (baseValue != 0)
        {
            animFSM.SetMoveSpeed(CurRealSpeed / baseValue);
        }
        else
        {
            animFSM.SetMoveSpeed(1.0f);
        }
        SetInCombat(true);

    }

    protected void OnOwnerUpdate()
    {
        if (headTextCtrl != null)
        {
        //    headTextCtrl.SetBloodEnable(actorInfo.OwnerID <= 0 || actorInfo.OwnerID == GameCenter.curMainPlayer.id);
        }
    }
    public override void BubbleTalk(string _text, float _time)
    {
        nameHeight = actorInfo.NameHight;
        base.BubbleTalk(_text, _time);
    }
    /// <summary>
    /// 展示/隐藏基本模型
    /// </summary>
    /// <param name="_show"></param>
    public override void TickAnimation()
    {
        base.TickAnimation();
        if (creatTime > 0 && animFSM != null && actorInfo.IsAlive)
        {
            if (Time.time - creatTime < waitCreatTime)
            {
                animFSM.Creating();
            }
        }
    }
    //public override void UseAbility(AbilityInfo _info)
    //{
    //    base.UseAbility(_info);

    //    List<Actor> list = new List<Actor>();
    //    list.Add(GameCenter.curMainPlayer);
    //    AbilityInfluence(list, _info);


    //}
    /// <summary>
    /// 死亡
    /// </summary>
    public override void Dead(bool _already)
    {
        base.Dead(_already);
        if (fxCtrl != null)
        {
            if (actorInfo.DeadEffectName != string.Empty)
            {
                //构建中
                //fxCtrl.DoNormalEffect(actorInfo.DeadEffectName);
            }
        }
        StartCoroutine(DeadOffTerrian(actorInfo.DeadDisapearTime, 1, 3.0f));
    }



    IEnumerator DeadOffTerrian(float _waitTime,float _deep,float _time)
    {
        if (_waitTime > 0)
        {
            yield return new WaitForSeconds(_waitTime);
        }
        if (this == null) yield break;
        Vector3 from = this.transform.position;
        Vector3 to = new Vector3(from.x, from.y - _deep, from.z);
        if (_time > 0)
        {
            float startTime = Time.time;
            while (Time.time - startTime < _time)
            {
                this.transform.position = Vector3.Lerp(from, to, (Time.time - startTime) / _time);
                yield return new WaitForFixedUpdate();
            }
        }
        else
        {
            this.transform.position = new Vector3(from.x, from.y - _deep, from.z);
        }
        Destroy(this);
        yield return null;
    }


    /// <summary>
    /// 添加模型改变buff
    /// </summary>
    //protected override void ModelSizeUpdate (BuffInfo info)
    //{
    //    //scaleBuffValue = (float)info.Value/10000f;
    //    //ActiveBoxCollider(true,actorInfo.ColliderRadius*scaleBuffValue);  //碰撞大小
    //    //if(mobModel != null)
    //    //{
    //    //    TweenScale.Begin(mobModel,1f,Vector3.one*actorInfo.ModelScale*scaleBuffValue);
    //    //}
    //    //if(headTextCtrl != null)headTextCtrl.UpdateParentHight();		//名字高度
    //    //fxCtrl.UpdateRingSize(actorInfo,scaleBuffValue);							//脚下光圈
    //}
    /// <summary>
    /// 还原模型大小
    /// </summary>
    protected override void RefreshModelSize()
    {
        //scaleBuffValue = 1.0f;
        //base.RefreshModelSize ();
        //ActiveBoxCollider(true,actorInfo.ColliderRadius);  //碰撞大小
        //if(mobModel != null)
        //    TweenScale.Begin(mobModel,1f,Vector3.one*actorInfo.ModelScale);
        //if(headTextCtrl != null)headTextCtrl.UpdateParentHight();		//名字高度
        //fxCtrl.UpdateRingSize(actorInfo,1);							//脚下光圈
    }
  
    public override void BeHit(AbilityResultInfo _info)
    {
        if (isDummy || !inited) return;
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
        if (headTextCtrl != null && !IsActor)
        {
            if (isDead || !MainPlayerFocus)
            {
                //headTextCtrl.HideBlood();
            }
            else
            {
                //headTextCtrl.SetBlood(actorInfo.MaxHP <= 0 ? 0 : (actorInfo.CurHP + _info.TotalDamage - _info.HasShowDamage) / (float)actorInfo.MaxHP, IsFriend);
                //headTextCtrl.SetBloodEnable(actorInfo.OwnerID <= 0 || actorInfo.OwnerID == GameCenter.curMainPlayer.id);
            }
        }
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

        if (_info.UserActor == GameCenter.curMainPlayer && rendererCtrl != null && !rendererCtrl.ArmorDirty && rendererCtrl.PendingArmors == 0)
        {
            rendererCtrl.Blink(Shader.Find("Unlit/Transparent Cutout/Rim"), Color.white, 0.5f);
        }
    }
    #endregion

}
