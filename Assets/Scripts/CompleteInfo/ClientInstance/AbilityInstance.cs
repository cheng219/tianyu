//==============================================================
//作者：吴江
//日期：2015/7/15
//用途：客户端自定义数据层（客户端自定义数据层实例命名以Instance为结尾，其中包含三部分   Ref - 静态配置   访问器   以及其他客户端自定义数据）
//==============================================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AbilityInstance {

    /// <summary>
    /// 使用基础符文的技能 by吴江
    /// </summary>
    /// <param name="_abilityID"></param>
    /// <param name="_level"></param>
    /// <param name="_rune"></param>
    /// <param name="_user"></param>
    /// <param name="_target"></param>
    public AbilityInstance(int _abilityID, int _level, SmartActor _user, SmartActor _target)
    {
        abilityID = _abilityID;
        level = _level;
        runeID = RefData == null ? -1 : RefData.baseRune;
        UserActor = _user;
        target = _target;
    }

    /// <summary>
    /// 使用自定义符文的技能 by吴江
    /// </summary>
    /// <param name="_abilityID"></param>
    /// <param name="_level"></param>
    /// <param name="_rune"></param>
    /// <param name="_user"></param>
    /// <param name="_target"></param>
    public AbilityInstance(int _abilityID, int _level, int _rune,SmartActor _user, SmartActor _target)
    {
        abilityID = _abilityID;
        level = _level;
        runeID = _rune;
        UserActor = _user;
        target = _target;
    }

    public AbilityInstance(pt_scene_skill_aleret_c021 _pt, SmartActor _user)
    {
        abilityID = (int)_pt.skill;
        runeID = (int)_pt.skill_rune;
        level = (int)_pt.lev;
        UserActor = _user;
        target = GameCenter.curGameStage.GetInterActiveObj((int)_pt.target_id) as SmartActor;
        warnningX = _pt.aleret_x;
        warnningY = _pt.aleret_y;
        warnningZ = _pt.aleret_z;
        warnningDir = _pt.aleret_dir;
        UserActor.FaceToNoLerp(_pt.dir);
        target = null;
        HasServerConfirm = false;
        needPrepare = true; 
    }



    public AbilityInstance(pt_scene_skill_effect_c006 _pt,SmartActor _user)
    {
        abilityID = (int)_pt.skill;
        runeID = (int)_pt.rune;
        level = (int)_pt.lev;
        UserActor = _user;
        endPosX = _pt.shift_x;
        endPosY = _pt.shift_y;
        endPosZ = _pt.shift_z;
        target = GameCenter.curGameStage.GetInterActiveObj((int)_pt.target_id) as SmartActor;
        HasServerConfirm = true;
        needPrepare = false;
        result_list = _pt.effect_list;
        if (UserActor != null)
        {
            UserActor.AbilityMove(this);
        }
        for (int i = 0; i < result_list.Count; i++)
        {
            int instanceID = (int)result_list[i].target_id;
            ObjectType type = (ObjectType)result_list[i].target_sort;
            ActorInfo actor = null;

            switch (type)
            {
                case ObjectType.Player:
                    if (instanceID == GameCenter.curMainPlayer.id)
                    {
                        actor = GameCenter.mainPlayerMng.MainPlayerInfo;
                    }
                    if (actor == null) actor = GameCenter.sceneMng.GetOPCInfo(instanceID);
                    break;
                case ObjectType.MOB:
                    actor = GameCenter.sceneMng.GetMobInfo(instanceID);
                    break;
                case ObjectType.Entourage:
                    if (GameCenter.curMainEntourage != null && instanceID == GameCenter.curMainEntourage.id)
                    {
                        actor = GameCenter.mercenaryMng.curMercernaryInfo;
                    }
                    else
                    {
                        actor = GameCenter.sceneMng.GetEntourageInfo(instanceID);
                    }
                    break;
            }

            if (actor != null)
            {
                actor.BeInfluencedByOther(this, result_list[i]);
            }
            else
            {
                Debug.LogError("找不到技能承受对象 " + instanceID);
            }
        }
    }
    /// <summary>
    /// 等级更新 by 贺丰
    /// </summary>
    /// <param name="_info"></param>
    public void Update(SkillInfo _info)
    {
        abilityID = (int)_info.SkillID;
        level = (int)_info.SkillLv;
        //if (runeID != (int)_info.rune_use)
        //{
        //    runeID = (int)_info.rune_use;
        //    hasServerConfirm = true;
        //    FullCD();
        //}
    }

    /// <summary>
    /// 设置使用者和承受者 by 贺丰
    /// </summary>
    /// <param name="_user"></param>
    /// <param name="_target"></param>
    public void SetActor(SmartActor _user, SmartActor _target)
    {
        UserActor = _user;
        target = _target;
    }

    public void AbilityResult(pt_scene_skill_effect_c006 _pt)
    {
        HasServerConfirm = true;
        result_list = _pt.effect_list;
        endPosX = _pt.shift_x;
        endPosY = _pt.shift_y;
        endPosZ = _pt.shift_z;
        runeID = (int)_pt.rune;
		needHitAnim = (_pt.effect_sort != (byte)(int)AbilityResultCAUSEType.BUFF);

        if (UserActor != null)
        {
            UserActor.AbilityMove(this);
        }
        for (int i = 0; i < result_list.Count; i++)
        {
            int instanceID = (int)result_list[i].target_id;
            ObjectType type = (ObjectType)result_list[i].target_sort;
            ActorInfo actor = null;
            switch (type)
            {
                case ObjectType.Player:
                    if (instanceID == GameCenter.curMainPlayer.id)
                    {
                        actor = GameCenter.mainPlayerMng.MainPlayerInfo;
                    }
                    if (actor == null) actor = GameCenter.sceneMng.GetOPCInfo(instanceID);
                    break;
                case ObjectType.MOB:
                    actor = GameCenter.sceneMng.GetMobInfo(instanceID);
                    break;
                case ObjectType.Entourage:
                    if (GameCenter.curMainEntourage != null && instanceID == GameCenter.curMainEntourage.id)
                    {
                        actor = GameCenter.mercenaryMng.curMercernaryInfo;
                    }
                    else
                    {
                        actor = GameCenter.sceneMng.GetEntourageInfo(instanceID);
                    }
                    break;
            }
            if (actor != null)
            {
                actor.BeInfluencedByOther(this, result_list[i]);
            }
            else
            {
                Debug.LogError("找不到技能承受对象 " + instanceID);
            }
        }
    }

    /// <summary>
    /// 清理结果
    /// </summary>
    public void ResetResult(SmartActor _target)
    {
        target = _target;
        ArrowFinished = false;
        targetTransforms.Clear();
        if (HasServerConfirm)
        {
            HasServerConfirm = false;
            lastTryTime = -1;
            if (OnTringEnd != null)
            {
                OnTringEnd(serializeID);
            }
            serializeID = 0;
            if (result_list != null)
            {
                result_list.Clear();
            }
        }
    }

    public void ResetTarget(SmartActor _target)
    {
        target = _target;
    }

    /// <summary>
    /// 强制开始冷却
    /// </summary>
    public void FullCD()
    {
        lastComfirmTime = Time.time;
    }

     
    /// <summary>
    /// 自定义数据
    /// </summary>
    protected int abilityID;
    protected int runeID;
    protected int level;
    public float endPosX;
    public float endPosY;
    public float endPosZ;
    public float warnningX;
    public float warnningY;
    public float warnningZ;
    public float warnningDir;
    protected SmartActor user = null;
    protected SmartActor target = null;
    /// <summary>
    /// 技能效果，服务端发送 by吴江
    /// </summary>
    protected List<st.net.NetBase.skill_effect> result_list;
    /// <summary>
    /// 技能效果，服务端发送 by吴江
    /// </summary>
    public List<st.net.NetBase.skill_effect> Result_list
    {
        get
        {
            return result_list;
        }
    }
	/// <summary>
	/// 是否需要播放被击动作(buff造成的技能结果不播被击动作)  by邓成
	/// </summary>
	public bool needHitAnim = true;

    /// <summary>
    /// 技能配置数据 by吴江
    /// </summary>
    protected SkillMainConfigRef refData;
    /// <summary>
    /// 技能配置数据 by吴江
    /// </summary>
    protected SkillMainConfigRef RefData
    {
        get
        {
            if (refData == null || refData.skillId != abilityID)
            {
                refData = ConfigMng.Instance.GetSkillMainConfigRef(abilityID);
            }
            return refData;
        }
    }
    /// <summary>
    /// 技能等级配置数据 by吴江
    /// </summary>
    protected SkillLvDataRef lvDataRef = null;
    /// <summary>
    /// 技能等级配置数据 by吴江
    /// </summary>
    public SkillLvDataRef LvDataRef
    {
        get
        {
            if (lvDataRef == null || lvDataRef.skillId != RuneRef.performanceID || lvDataRef.skillLv != level)
            {
                lvDataRef = ConfigMng.Instance.GetSkillLvDataRef(RuneRef.performanceID, level);
            }
            return lvDataRef;
        }
    }
    /// <summary>
    /// 技能表现配置数据 by吴江
    /// </summary>
    protected SkillPerformanceRef performanceRef = null;
    /// <summary>
    /// 技能表现配置数据 by吴江
    /// </summary>
    public SkillPerformanceRef PerformanceRef
    {
        get {
            if (performanceRef == null || performanceRef.skillId != RuneRef.performanceID)
            {
                performanceRef = ConfigMng.Instance.GetSkillPerformanceRef(RuneRef.performanceID);
            }
            return performanceRef;
        }
    }
    /// <summary>
    /// 技能符文配置数据 by吴江
    /// </summary>
    protected SkillRuneRef runeRef;
    /// <summary>
    /// 技能符文配置数据 by吴江
    /// </summary>
    public SkillRuneRef RuneRef
    {
        get
        {
            if (runeID <= 0) runeID = RefData.baseRune;
            if (runeRef == null || runeRef.runeId != runeID)
            {
                runeRef = ConfigMng.Instance.GetSkillRuneRef(runeID);
            }
            return runeRef;
        }
    }
	/// <summary>
	/// 放技能也有上浮提示
	/// </summary>
	public int SkillReportID
	{
		get
		{
			return performanceRef == null?0:performanceRef.reportId;
		}
	}

    protected float lastTryTime;
    /// <summary>
    /// 上一次技能尝试使用时间
    /// </summary>
    public float LastTryTime
    {
        get
        {
            return lastTryTime;
        }
        set
        {
            lastTryTime = value;
        }
    }

    /// <summary>
    /// 是否自动使用
    /// </summary>
    protected bool isAutoUse = true;
    /// <summary>
    /// 是否自动使用
    /// </summary>
    public bool IsAutoUse
    {
        get { return isAutoUse; }
        set { isAutoUse = value; }
    }

    /// <summary>
    /// 技能目标类型
    /// </summary>
    public SkillTargetType ThisSkillTargetType
    {
        get
        {
            return PerformanceRef == null ? SkillTargetType.NONE : PerformanceRef.skillTargetType;
        }
    }

    /// <summary>
    /// 是否尝试结束(从开始申请施放一直到过了保护时间仍然没有得到后台确认即视做失败) by吴江
    /// </summary>
    public bool IsTringEnd
    {
        get
        {
            if (lastTryTime <= 0) return true;
            if (HasServerConfirm)
            {
                lastTryTime = -1;
                if (OnTringEnd != null)
                {
                    OnTringEnd(serializeID);
                }
                serializeID = 0;
                return true;
            }
            float diff = Time.time - lastTryTime;
            float need = Mathf.Max(LvDataRef.cd, PerformanceRef.protectTime);
            need = Mathf.Max(PerformanceRef.timeYz, PerformanceRef.protectTime);
            if (diff >= need)
            {
                if (need > 0)
                {
                    Debug.LogError(AbilityName + "技能释放超时" + Time.time);
                }
                lastTryTime = -1;
                if (OnTringEnd != null)
                {
                    OnTringEnd(serializeID);
                }
                serializeID = 0;
            }
            return true;
        }
    }

    public System.Action<uint> OnTringEnd;
    /// <summary>
    /// 申请序列号
    /// </summary>
    public uint serializeID = 0;

    /// <summary>
    /// 技能等级 by吴江
    /// </summary>
    public int Level
    {
        get
        {
            return level;
        }
    }
    /// <summary>
    /// 预警位置 by吴江
    /// </summary>
    public Vector3 WarnningPos
    {
        get
        {
            return new Vector3(warnningX,warnningY,warnningZ);
        }
    }
    /// <summary>
    /// 宠物使用该技能的几率
    /// </summary>
    public float PetUseRate
    {
        get
        {
            return RefData == null ? 0 : RefData.probability;
        }
    }

    /// <summary>
    /// 预警时间 by吴江
    /// </summary>
    public float WarnningTime
    {
        get
        {
            return PerformanceRef == null ? 0 : PerformanceRef.alertTimesShow;
        }
    }

    /// <summary>
    /// 预警长度 by吴江
    /// </summary>
    public float WarnningLength
    {
        get
        {
            return PerformanceRef == null ? 0 : PerformanceRef.alertAreaLength;
        }
    }

    /// <summary>
    /// 预警宽度 by吴江
    /// </summary>
    public float WarnningWidth
    {
        get
        {
            return PerformanceRef == null ? 0 : PerformanceRef.alertAreaWidth;
        }
    }

    /// <summary>
    /// 预警类型 by吴江
    /// </summary>
    public AlertAreaType WarnningType
    {
        get
        {
            return PerformanceRef == null ? AlertAreaType.NONE : PerformanceRef.alertAreaType;
        }
    }

    /// <summary>
    /// 上一次技能使用时间 by吴江
    /// </summary>
    protected float lastComfirmTime;
    /// <summary>
    /// 剩余冷却时间 by吴江
    /// </summary>
    public float RestCD
    {
        get
        {
            float diff = Time.time - lastComfirmTime;
            return LvDataRef == null ? 0.0f : Mathf.Max(0,LvDataRef.cd - diff); 
        }
    }
	/// <summary>
	/// 技能CD,暂时用于宠物普攻
	/// </summary>
	public float AbilityCD
	{
		get
		{
			return LvDataRef == null?3:LvDataRef.cd;
		}
	}

    /// <summary>
    /// 释放蓝是否足够 
    /// </summary>
    public bool HadMp
    {
        get
        {
            return LvDataRef == null ? false : (ulong)LvDataRef.mp <= UserActor.curMp; 
        }
    }
    #region 访问器
    /// <summary>
    /// 技能ID by吴江
    /// </summary>
    public int AbilityID
    {
        get { return abilityID; }
    }
    /// <summary>
    /// 技能名称 by吴江
    /// </summary>
    public string AbilityName
    {
        get { return PerformanceRef != null ? PerformanceRef.skillName : string.Empty; }
    }
    /// <summary>
    /// 符文ID by 贺丰
    /// </summary>
    public int RuneID
    {
        get { return runeID; }
    }
    /// <summary>
    /// 后台是否已经确认结果
    /// </summary>
    protected bool hasServerConfirm = false;
    /// <summary>
    /// 技能释放完事件 by 贺丰
    /// </summary>
    public System.Action HasConfirmSkill;
    /// <summary>
    /// 后台是否已经确认结果
    /// </summary>
    public bool HasServerConfirm
    {
        get
        {
            return hasServerConfirm;
        }
        protected set
        {
            hasServerConfirm = value;
            if (hasServerConfirm)
            {
                serializeID = 0;
                lastComfirmTime = Time.time;
                if (HasConfirmSkill != null)
                    HasConfirmSkill();
            }
        }
    }
    /// <summary>
    /// 是否需要做前摇动作表现 by吴江
    /// </summary>
    protected bool needPrepare = true;
    /// <summary>
    /// 是否需要做前摇动作表现 by吴江
    /// </summary>
    public bool NeedPrepare
    {
        get
        {
            return needPrepare;
        }
    }
    /// <summary>
    /// 是否需要预警
    /// </summary>
    public bool HasWarnning
    {
        get
        {
            return PerformanceRef == null ? false : PerformanceRef.alertAreaType != AlertAreaType.NONE;
        }
    }

    /// <summary>
    /// 前台特殊表现类型
    /// </summary>
    public ClientShowType CurClientShowType
    {
        get
        {
            return PerformanceRef == null ? ClientShowType.NONE : PerformanceRef.clientShowType;
        }
    }

    /// <summary>
    /// 是否能对目标施放 by吴江
    /// </summary>
    /// <param name="_target"></param>
    /// <returns></returns>
    public bool CanUseFor(InteractiveObject _target)
    {
        if (_target != null)
        {
            if (UserActor != null)
            {
                Vector3[] path = GameStageUtility.StartPath(UserActor.transform.position, _target.transform.position);
                if (path != null && path.Length > 0)
                {
                    float distance = path.CountPathDistance();
                    return distance <= AttackDistance;//计算方式修改,解决穿墙放冲锋技能bug by邓成
                }
                else
                {
                    return (UserActor.transform.position - _target.transform.position).sqrMagnitude <= AttackDistance * AttackDistance;
                }
            }
        }
        return PerformanceRef.castType == CastType.NOTARGET;
    }

    /// <summary>
    /// 是否停止追踪
    /// </summary>
    /// <param name="_target"></param>
    /// <returns></returns>
    public bool CanPushAbilityCommand(InteractiveObject _target)
    {
        if (_target != null)
        {
            if (UserActor != null)
            {
                Vector3[] path = GameStageUtility.StartPath(UserActor.transform.position, _target.transform.position);
                if (path != null && path.Length > 0)
                {
                    float distance = path.CountPathDistance();
                    return distance <= 0.5f*AttackDistance;//计算方式修改,解决穿墙放冲锋技能bug by邓成
                }
                else
                {
                    return (UserActor.transform.position - _target.transform.position).sqrMagnitude <= 0.5f*AttackDistance * AttackDistance;
                }
            }
        }
        return PerformanceRef.castType == CastType.NOTARGET;
    }

    /// <summary>
    /// 攻击距离
    /// </summary>
    public float AttackDistance
    {
        get
        {
            if (PerformanceRef != null)
            {
                return PerformanceRef.castRange;
            }
            return 1.0f;
        }
    }


    /// <summary>
    /// 全程特效 by吴江
    /// </summary>
    public string WholetimeEffect
    {
        get
        {
            return PerformanceRef == null ? string.Empty : PerformanceRef.wholetimeEffect;
        }
    }


    /// <summary>
    /// 释放者位置 by吴江
    /// </summary>
    public Vector3 UserPostion
    {
        get
        {
            return UserActor == null ? Vector3.zero : UserActor.transform.position;
        }
    }

    /// <summary>
    /// 释放技能的转向类型
    /// </summary>
    public TurnType DirTurnType
    {
        get
        {
            return PerformanceRef == null ? TurnType.TURN : PerformanceRef.turnType;
        }
    }
    /// <summary>
    /// 技能分类
    /// </summary>
    public SkillMode thisSkillMode
    {
        get
        {
            return RefData == null ? SkillMode.NORMALSKILL : RefData.skillMode;
        }
    }

    protected Vector3 targetPostion = Vector3.zero;
    /// <summary>
    /// 目标位置   by吴江
    /// </summary>
    public Vector3 TargetPostion
    {
        get
        {
            return target == null ? targetPostion : target.transform.position;
        }
    }
    /// <summary>
    /// 释放者
    /// </summary>
    public SmartActor UserActor
    {
        get
        {
            return user;
        }
        protected set
        {
            user = value;
        }
    }

    /// <summary>
    /// 目标
    /// </summary>
    public SmartActor TargetActor
    {
        get
        {
            return target;
        }
    }

    /// <summary>
    /// 自动战斗时的动作持续保护时间
    /// </summary>
    public float ProtectDuration
    {
        get { return PerformanceRef == null ? 0 : PerformanceRef.protectTime; }
    }

    /// <summary>
    /// 前摇动作名称 by吴江
    /// </summary>
    public string PrepareAnimationName
    {
        get { return PerformanceRef == null ? string.Empty : PerformanceRef.frontAction; }
    }
    /// <summary>
    /// 前摇特效名称 by吴江
    /// </summary>
    public string PrepareEffectName
    {
        get { return PerformanceRef == null ? string.Empty : PerformanceRef.frontEffect; }
    }
    /// <summary>
    /// 前摇动作时间 by吴江
    /// </summary>
    public float PrepareDuration
    {
        get { return PerformanceRef == null ? 0.0f : PerformanceRef.frontTime; }
    }

    /// <summary>
    /// 过程动作名称列表 by吴江
    /// </summary>
    public List<string> ProcessAnimationNameList
    {
        get { return PerformanceRef == null ? new List<string>() : PerformanceRef.processActionList; }
    }
    /// <summary>
    /// 过程特效名称列表 by吴江
    /// </summary>
    public List<string> ProcessEffectList
    {
        get { return PerformanceRef == null ? new List<string>() : PerformanceRef.processEffectList; }
    }

    /// <summary>
    /// 过程中世界矩阵特效
    /// </summary>
    public List<AbilityDelayEffectRefData> AbilityDelayEffectRefDataList
    {
        get
        {
            return PerformanceRef == null ? new List<AbilityDelayEffectRefData>() : PerformanceRef.abilityDelayEffectRefDataList;
        }
    }

    /// <summary>
    /// 过程被击特效名称列表 by吴江
    /// </summary>
    public List<string> ProcessDefEffectList
    {
        get { return PerformanceRef == null ? new List<string>() : PerformanceRef.hitEffectList; }
    }
    /// <summary>
    /// 过程动作时间列表 by吴江
    /// </summary>
    public List<float> ProcessDurationList
    {
        get { return PerformanceRef == null ? new List<float>() : PerformanceRef.processTimeList; }
    }
    /// <summary>
    /// 技能过程攻击音效队列
    /// </summary>
    public List<SkillSoundPair> ProcessSoundPairList
    {
        get { return PerformanceRef == null ? new List<SkillSoundPair>() : PerformanceRef.soundAtkRes; }
    }

    /// <summary>
    /// 过程震动朝向列表 by吴江
    /// </summary>
    public Vector3 ProcessShakeV3
    {
        get { return PerformanceRef == null ? Vector3.zero : PerformanceRef.shakeDirection; }
    }
    /// <summary>
    /// 过程震动强度列表 by吴江
    /// </summary>
    public List<float> ProcessShakePowerList
    {
        get { return PerformanceRef == null ? new List<float>() : PerformanceRef.shakePowerGroupList; }
    }

    public List<int> ProcessShakeTimeList
    {
        get { return PerformanceRef == null ? new List<int>() : PerformanceRef.shakeTimeList; }
    }

    /// <summary>
    /// 后摇动作名称 by吴江
    /// </summary>
    public string EndingAnimationName
    {
        get { return PerformanceRef == null ? string.Empty : PerformanceRef.afterAction; }
    }
    /// <summary>
    /// 后摇特效名称 by吴江
    /// </summary>
    public string EndingEffectName
    {
        get { return PerformanceRef == null ? string.Empty : PerformanceRef.afterEffect; }
    }
    /// <summary>
    /// 后摇动作时间 by吴江
    /// </summary>
    public float EndingDuration
    {
        get { return PerformanceRef == null ? 0.0f : PerformanceRef.afterTime; }
    }

    /// <summary>
    /// 移动到目标点的差值 正数为超过  by吴江
    /// </summary>
    public float AimOffset
    {
        get
        {
            return PerformanceRef == null ? 0.0f : PerformanceRef.shiftDirection;
        }
    }

    /// <summary>
    /// 弹道ID,走配置的弹道默认为锁定弹道
    /// </summary>
    public int ArrowID
    {
        get
        {
            return PerformanceRef == null || PerformanceRef.skillType != SkillType.FOLLOWARROW ? -1 : PerformanceRef.arrowEffect;
        }
    }

    public bool ArrowFinished = false;


    /// <summary>
    /// 陷阱ID by吴江
    /// </summary>
    public int TrapID
    {
        get
        {
            return PerformanceRef == null ? -1 : PerformanceRef.aoeEffect;
        }
    }



    protected List<string> allNeedEffectNames = new List<string>();
    /// <summary>
    /// 该技能所需要的所有特效,用于预加载 by吴江
    /// </summary>
    public List<string> AllNeedEffectNames
    {
        get
        {
            if (allNeedEffectNames.Count == 0)
            {
                if (PrepareEffectName != string.Empty && !allNeedEffectNames.Contains(PrepareEffectName))
                {
                    allNeedEffectNames.Add(PrepareEffectName);
                }

                for (int i = 0; i < ProcessEffectList.Count; i++)
                {
                    if (ProcessEffectList[i] != string.Empty && !allNeedEffectNames.Contains(ProcessEffectList[i]))
                    {
                        allNeedEffectNames.Add(ProcessEffectList[i]);
                    }
                }

                if (EndingEffectName != string.Empty && !allNeedEffectNames.Contains(EndingEffectName))
                {
                    allNeedEffectNames.Add(EndingEffectName);
                }

                for (int i = 0; i < AbilityDelayEffectRefDataList.Count; i++)
                {
                    if (AbilityDelayEffectRefDataList[i].effectName != string.Empty && !allNeedEffectNames.Contains(AbilityDelayEffectRefDataList[i].effectName))
                    {
                        allNeedEffectNames.Add(AbilityDelayEffectRefDataList[i].effectName);
                    }
                }


                //弹道部分
                if (ArrowID > 0)
                {
                    ArrowRef arrow = ConfigMng.Instance.GetArrowRef(ArrowID);
                    if (arrow != null && !allNeedEffectNames.Contains(arrow.effect_res) && arrow.effect_res != string.Empty)
                    {
                        allNeedEffectNames.Add(arrow.effect_res);
                    }
                }

                if (TrapID > 0)
                {
                    //陷阱部分
                    TrapRef trap = ConfigMng.Instance.GetTrapRef(TrapID);
                    if (trap != null && !allNeedEffectNames.Contains(trap.effectRes) && trap.effectRes != string.Empty)
                    {
                        allNeedEffectNames.Add(trap.effectRes);
                    }
                }

                //Buff部分
                if (LvDataRef != null)
                {
                    //对自己的buff
                    if (LvDataRef.selfBuff.Count > 0)
                    {
                        for (int i = 0; i < LvDataRef.selfBuff.Count; i++)
                        {
                            SkillBuffRef buffref = ConfigMng.Instance.GetSkillBuffRef(LvDataRef.selfBuff[i].buffId);
                            if (buffref != null && buffref.effect_res != string.Empty && !allNeedEffectNames.Contains(buffref.effect_res))
                            {
                                allNeedEffectNames.Add(buffref.effect_res);
                            }
                        }
                    }
                    //对目标的buff
                    if (LvDataRef.targetBuff.Count > 0)
                    {
                        for (int i = 0; i < LvDataRef.targetBuff.Count; i++)
                        {
                            SkillBuffRef buffref = ConfigMng.Instance.GetSkillBuffRef(LvDataRef.targetBuff[i].buffId);
                            if (buffref != null && buffref.effect_res != string.Empty && !allNeedEffectNames.Contains(buffref.effect_res))
                            {
                                allNeedEffectNames.Add(buffref.effect_res);
                            }
                        }
                    }
                }

                //被击部分
                for (int i = 0; i < ProcessDefEffectList.Count; i++)
                {
                    if (ProcessDefEffectList[i] != string.Empty && !allNeedEffectNames.Contains(ProcessDefEffectList[i]))
                    {
                        allNeedEffectNames.Add(ProcessDefEffectList[i]);
                    }
                }
            }
            return allNeedEffectNames;
        }
        
    }

    ///// <summary>
    ///// 是否为锁定技能
    ///// </summary>
    //public bool IsLock
    //{
    //    get
    //    {
    //        return PerformanceRef.castType == CastType.target;
    //    }
    //}



    //public int BlinkLateTime
    //{
    //    get { return PerformanceRef != null ? PerformanceRef.redLateTime : 0; }
    //}

    //public int DefEffectTime
    //{
    //    get { return  PerformanceRef != null ? PerformanceRef.defTime : 0; }
    //}





    /// <summary>
    /// 是否需要目标 by吴江
    /// </summary>
    public bool NeedTarget
    {
        get
        {
            return PerformanceRef == null ? true : PerformanceRef.castType == CastType.TARGET;
        }
    }
    /// <summary>
    /// 技能冒泡ID
    /// </summary>
    public int PopID
    {
        get
        {
            return PerformanceRef == null ? 0 : PerformanceRef.popId;
        }
    }

    /// <summary>
    /// 技能释放需求的魔法值
    /// </summary>
    public int NeedMP
    {
        get
        {
            return LvDataRef == null ? 0 : LvDataRef.mp;
        }
    }


    public float RigidityTime
    {
        get { return  PerformanceRef != null ?  PerformanceRef.timeYz :0; }
    }

    public float RigidityStartTime
    {
        get { return  PerformanceRef != null ? PerformanceRef.timeYzStart: 0; }
    }

    public SelfShiftType MoveType
    {
        get
        {
            if (PerformanceRef == null) return SelfShiftType.NO;
            return PerformanceRef.selfShiftType;
        }
    }

    public float RushValue
    {
        get { return PerformanceRef != null ? PerformanceRef.selfShiftRange : 0; }
    }


    public float RushHoldTime
    {
        get
        {
            float totalTime = 0;
            for (int i = 0; i < PerformanceRef.processTimeList.Count; i++)
            {
                totalTime += PerformanceRef.processTimeList[i];
            }
            return PerformanceRef != null ? totalTime : 0;
        }
    }

    protected int curTarTransformIndex = 0;
    protected List<Transform> targetTransforms = new List<Transform>();
    public List<Transform> TargetTransforms
    {
        get
        {
            return targetTransforms;
        }
    }
    public void PushTarTransform(Transform _transform)
    {
        if (!targetTransforms.Contains(_transform))
        {
            targetTransforms.Add(_transform);
        }
    }

    public Transform PopTarTransform()
    {
        Transform tar = null;
        if (targetTransforms.Count > curTarTransformIndex)
        {
            tar = targetTransforms[curTarTransformIndex];
            curTarTransformIndex++;
            if (curTarTransformIndex >= targetTransforms.Count)
            {
                curTarTransformIndex = 0;
            }
        }
        return tar;
    }



    public int DirY
    {
        get {
            if (DirTurnType == TurnType.TURN && target != null)
            {
                Vector3 dir = target.transform.position - UserActor.transform.position;
                Quaternion quat = Quaternion.identity;
                if (dir.sqrMagnitude > 0.001f)
                {
                    quat.SetLookRotation(dir);
                }
                return (int)quat.eulerAngles.y;
            }
            else
            {
                return (int)UserActor.transform.eulerAngles.y;
            }
        }
    }

    /// <summary>
    /// 是否虚拟体状态 by吴江
    /// </summary>
    public bool IsDummy
    {
        get
        {
            if (UserActor == null || TargetActor == null) return false;
            bool bothDummy = (UserActor == null || UserActor.isDummy || !UserActor.IsShowing) && (TargetActor == null || TargetActor.isDummy || !TargetActor.IsShowing);
            if (bothDummy)
            {
                ArrowFinished = true;
                return true;
            }
            float angle = Mathf.Acos(Vector3.Dot((UserActor.transform.position - TargetActor.transform.position).normalized, (UserActor.AttackPoint.position - TargetActor.HitPoint.position).normalized)) * Mathf.Rad2Deg;
            if (Mathf.Abs(angle) > 90)
            {
                ArrowFinished = true;
                return true;
            }
            return false;
        }
    }


    ///// <summary>
    ///// 攻击音效列表 by吴江
    ///// </summary>
    //public List<SkillSoundPair> SoundAtkRes
    //{
    //    get
    //    {
    //        return RefData == null ? new List<SkillSoundPair>() : RefData.soundAtkRes;
    //    }
    //}
    ///// <summary>
    ///// 被击音效列表 by吴江
    ///// </summary>
    //public List<SkillSoundPair> SoundDefRes
    //{
    //    get
    //    {
    //        return RefData == null ? new List<SkillSoundPair>() : RefData.soundDefRes;
    //    }
    //}
    #endregion
}


public class AbilityResultInfo
{

    protected st.net.NetBase.skill_effect result;

    protected AbilityInstance instance;

    protected List<float> damageTimeList;

    protected List<int> damageRateList;

    protected List<string> hitEffectList;

    protected List<float> hitEffectTimeList;

    protected List<string> hitAnimList = new List<string>();

    protected List<SkillSoundPair> hitSoundList;

    protected ulong totalDamage = 0;

    protected ulong hasShowDamage = 0;

    protected List<ulong> damageList = new List<ulong>();

    protected List<Vector3> moveList = new List<Vector3>();

    protected List<float> moveTimes = new List<float>();

    protected List<float> rigidityTimes = new List<float>();

    protected List<bool> moveFlag = new List<bool>();

    protected Vector3 dir;

    protected int index;

    protected float startTime;

    protected bool lockFinished = false;

    public InteractiveObject target;

    protected SmartActor myTargetActor = null;

    public AbilityResultInfo(AbilityInstance _instance, st.net.NetBase.skill_effect _result)
    {
        DefResultType type = (DefResultType)_result.def_sort;
        bool isNoKick = type == DefResultType.DEF_SORT_NOSTIFLE || type == DefResultType.DEF_SORT_NOKICKDOWN || type == DefResultType.DEF_SORT_NOKICK;
        hasShowDamage = 0;
        instance = _instance;
        result = _result;
        startTime = Time.time;
        SkillPerformanceRef refData = _instance.PerformanceRef;
        if (refData != null)
        {
            damageTimeList = refData.damageTimeList;
            damageRateList = refData.damageRateList;
            totalDamage = _result.demage;
            Vector3 dest = new Vector3(_result.effect_x, _result.effect_y, _result.effect_z);


            Actor tar = GameCenter.curGameStage.GetInterActiveObj((int)_result.target_id) as Actor;
            if (tar != null)
            {
                switch (_instance.CurClientShowType)
                {
                    case ClientShowType.Lightingskill:
                    case ClientShowType.Invinciblechop:
                        _instance.PushTarTransform(tar.HitPoint);
                        break;
                }
            }
            if (dest != Vector3.zero || refData.kickTimes > 0)
            {
                if (instance.TargetActor == null)
                {
                    if (tar != null)
                    {
                        target = tar;
                    }
                }
                else
                {
                    if (refData.kickTimes > 0 && !isNoKick)
                    {
                        instance.TargetActor.StopMovingTo();
                    }
                    target = instance.TargetActor;
                }
            }

            if (dest != Vector3.zero)
            {
                Vector3 from = target.transform.position;
                float distance = Vector3.Distance(from, dest);
                dir = (dest - from).normalized;
                if (refData.kickDisSectionList.Count > 0)
                {
                    float rate = 0;
                    for (int i = 0; i < refData.kickDisSectionList.Count; i++)
                    {
                        rate += refData.kickDisSectionList[i];
                        moveList.Add(from + dir * (distance * rate));
                        moveTimes.Add(refData.kickShowTimes * refData.kickDisSectionList[i]);
                        float rigiFloat = 0;
                        for (int j = i; j < refData.kickDisSectionList.Count; j++)
                        {
                            rigiFloat += refData.kickDisSectionList[j];
                        }
                        rigidityTimes.Add(refData.kickTimes * rigiFloat);
                        moveFlag.Add(refData.kickDisSectionList[i] > 0);
                    }
                }
                else
                {
                    moveList.Add(dest);
                    moveTimes.Add(refData.kickShowTimes);
                    rigidityTimes.Add(refData.kickTimes);
                    moveFlag.Add(false);
                }
            }
            if (refData.kickTimes > 0 && target != null && !isNoKick)
            {
                SmartActor sa = target as SmartActor;
                if (sa != null)
                {
                    sa.StopMovingTo();
                }
            }

            if (refData.damageRateList.Count > 0)
            {
                for (int i = 0; i < refData.damageRateList.Count; i++)
                {
					int singleDamage = Mathf.RoundToInt(totalDamage * (damageRateList[i] / 100.0f));
					damageList.Add((ulong)singleDamage);
                }
            }
            else
            {
                damageList.Add(totalDamage);
            }
            hitEffectList = refData.hitEffectList;
            hitEffectTimeList = refData.hitEffectTimeList;
            hitSoundList = refData.soundDefRes;




            int animCount = Mathf.Max(1, damageTimeList.Count);
            switch ((DefResultType)_result.def_sort)
            {
                case DefResultType.DEF_SORT_NO:
                    for (int i = 0; i < animCount; i++)
                    {
                        hitAnimList.Add("hit");
                    }
                    break;
                case DefResultType.DEF_SORT_TREAT:
                    break;
                //case DefResultType.DEF_SORT_DIE:
                //    for (int i = 0; i < animCount; i++)
                //    {
                //        hitAnimList.Add("hit");
                //    }
                //    break;
                case DefResultType.DEF_SORT_STIFLE:
                    for (int i = 0; i < animCount; i++)
                    {
                        hitAnimList.Add("hit");
                    }
                    break;
                case DefResultType.DEF_SORT_KICK2:
                    for (int i = 0; i < animCount; i++)
                    {
                        hitAnimList.Add("kickdown");
                    }
                    break;
                case DefResultType.DEF_SORT_KICK:
                    for (int i = 0; i < animCount; i++)
                    {
                        hitAnimList.Add("kickfly");
                    }
                    break;
                case DefResultType.DEF_SORT_UNTREAT:
                    break;
                case DefResultType.DEF_SORT_NOKICKDOWN:
                    break;
                case DefResultType.DEF_SORT_NOSTIFLE:
                    break;
                case DefResultType.DEF_SORT_NOKICK:
                    break;
                default:
                    break;
            }
            //如果是脚本技能,在经过后台确认后,直接播放声音队列 by吴江
            if (_instance.thisSkillMode == SkillMode.SCRIPTSKILL)
            {
                GameCenter.soundMng.PlaySkillSoundList(refData.soundAtkRes);
            }
        }
    }


    /// <summary>
    /// 当前是否需要展示了 by吴江
    /// </summary>
    public bool NeedShow
    {
        get
        {
            if (instance.ArrowID > 0 && !lockFinished)
            {
                if (instance.ArrowFinished)
                {
                    startTime = Time.time;
                    lockFinished = true;
                }
                else
                {
                    return false;
                }
            }
            if (damageTimeList == null) return false;
            if (index >= damageTimeList.Count) return false;
            return Time.time - startTime >= damageTimeList[index];
        }
    }
    /// <summary>
    /// 本次的伤害 by吴江
    /// </summary>
    public ulong curDamage
    {
        get
        {
            if (index >= damageList.Count) return 0;
            return damageList[index];
        }
    }
    /// <summary>
    /// 总伤害
    /// </summary>
    public ulong TotalDamage
    {
        get
        {
            return totalDamage;
        }
    }
    /// <summary>
    /// 已经表现过的伤害
    /// </summary>
    public ulong HasShowDamage
    {
        get
        {
            return hasShowDamage;
        }
    }


    /// <summary>
    /// 本次的位移动目标点
    /// </summary>
    public Vector3 curDestPos
    {
        get
        {
            if (index >= moveList.Count)
            {
                return Vector3.zero;
            }
            return moveList[index];
        }
    }


    /// <summary>
    /// 本次移动的总时间
    /// </summary>
    public float curMoveTime
    {
        get
        {
            if (index >= moveTimes.Count)
            {
                return 0;
            }
            return moveTimes[index];
        }
    }

    /// <summary>
    /// 本次僵直的总时间
    /// </summary>
    public float curRigidityTime
    {
        get
        {
            if (index >= rigidityTimes.Count) return 0;
            return rigidityTimes[index];
        }
    }

    /// <summary>
    /// 本次是否需要位置移动
    /// </summary>
    public bool NeedMove
    {
        get
        {
            if (index >= moveFlag.Count) return false;
            return moveFlag[index];
        }
    }

    /// <summary>
    /// 本次是否需要位置移动
    /// </summary>
    public Vector3 Dir
    {
        get
        {
            return dir;
        }
    }

    /// <summary>
    /// 本次被击的时间 
    /// </summary>
    public float curTime
    {
        get
        {
            if (index >= damageTimeList.Count || damageTimeList.Count == 0) return 1.0f;
            if(index == 0) return damageTimeList[1];
            return damageTimeList[index] - damageTimeList[index - 1];
        }
    }
    /// <summary>
    /// 本次的被击特效 by吴江
    /// </summary>
    public string curDefEffect
    {
        get
        {
            if (index >= hitEffectList.Count) return string.Empty;
            return hitEffectList[index];
        }
    }
    /// <summary>
    /// 本次的被击特效的播放时间  by吴江
    /// </summary>
    public float curDefTime
    {
        get
        {
            if (index >= hitEffectTimeList.Count) return 0;
            return hitEffectTimeList[index];
        }
    }
    /// <summary>
    /// 本次的被击音效名称  by吴江
    /// </summary>
    public string curDefSound
    {
        get
        {
            if (index >= hitSoundList.Count) return string.Empty;
            return hitSoundList[index].res;
        }
    }

    /// <summary>
    /// 本次相机的震动朝向 by吴江
    /// </summary>
    public Vector3 curCameraShakeV3
    {
        get
        {
            return instance.ProcessShakeV3;
        }
    }

    /// <summary>
    /// 本次震动强度 by吴江
    /// </summary>
    public float curShakePower
    {
        get
        {
            if (index >= instance.ProcessShakePowerList.Count) return 0;
            return instance.ProcessShakePowerList[index];
        }
    }

    /// <summary>
    /// 本次的被击动画 by吴江
    /// </summary>
    public string curDefAnimation
    {
        get
        {
            if (index >= hitAnimList.Count) return string.Empty;
            return hitAnimList[index];
        }
    }
    /// <summary>
    /// 技能释放者 by吴江
    /// </summary>
    public SmartActor UserActor
    {
        get
        {
            return instance.UserActor;
        }
    }



    /// <summary>
    /// 技能承受者 by吴江
    /// </summary>
    public SmartActor TargetActor
    {
        get
        {
            if (myTargetActor == null)
            {
                if (result.target_id == GameCenter.curMainPlayer.id) myTargetActor = GameCenter.curMainPlayer;
                if (myTargetActor == null)
                {
                    myTargetActor = GameCenter.curGameStage.GetInterActiveObj((int)result.target_id) as SmartActor;
                    if (myTargetActor == null)
                    {
                        myTargetActor = GameCenter.curGameStage.GetOtherPlayer((int)result.target_id) as SmartActor;
                        if (myTargetActor == null) myTargetActor = instance.TargetActor;
                    }
                }
            }
            return myTargetActor;
        }
    }
    /// <summary>
    /// 攻击类型 by吴江
    /// </summary>
    public AttackResultType AttackType
    {
        get
        {
            return (AttackResultType)result.atk_sort;
        }
    }
    /// <summary>
    /// 防御类型 by吴江
    /// </summary>
    public DefResultType DefType
    {
        get
        {
            return (DefResultType)result.def_sort;
        }
    }
    /// <summary>
    /// 是否已经完成 by吴江 
    /// </summary>
    public bool HasFinished
    {
        get
        {
            if (damageTimeList == null) return true;
            return index >= damageTimeList.Count;
        }
        protected set
        {
            index = damageTimeList.Count;
        }
    }

    public bool HasMoveFinished
    {
        get
        {
            if (TargetActor == null) return true;
            return !TargetActor.IsMoving;
        }
    }


    /// <summary>
    /// 展示
    /// </summary>
    /// <param name="_actor"></param>
    public void Show(SmartActor _actor)
    {
        if (_actor == null || UserActor == null || _actor.isDummy)
        {
            HasFinished = true;
        }
        if (_actor.curHp <= 0)
        {
            _actor.Dead();
        }
        if (HasFinished) return;
        bool showText = (UserActor == GameCenter.curMainPlayer || TargetActor == GameCenter.curMainPlayer || UserActor == GameCenter.curMainEntourage || TargetActor == GameCenter.curMainEntourage);
        if (showText)
        {
            GameCenter.spawner.SpawnStateTexter(this);
            if (UserActor == GameCenter.curMainPlayer)
            {
                if ((AttackResultType)DefType != AttackResultType.ATT_SORT_DODGE)
                {
                    if (GameCenter.abilityMng.OnContinuCountUpdate != null)
                    {
                        GameCenter.abilityMng.OnContinuCountUpdate(Time.time);
                    }
                }
            }
            if (curShakePower > 0)
            {
                GameCenter.cameraMng.MainCameraShake(curCameraShakeV3, curShakePower);
            }
            GameCenter.soundMng.PlaySound(curDefSound, SoundMng.GetSceneSoundValue(_actor.transform, GameCenter.curMainPlayer.transform), false, true);
        }
        hasShowDamage += curDamage;
		if(instance != null && instance.needHitAnim)
        	_actor.BeHit(this);
        index++;
    }
}
