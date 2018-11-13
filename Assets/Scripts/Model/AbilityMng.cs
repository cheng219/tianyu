//=================================================
//作者：吴江
//日期：2015/5/29
//用途：技能管理类(局部管理类，从属于某一个玩家对象)
//=================================================



using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;

public class AbilityMng
{

    #region 数据
    //======================================玩家部分=============================
    protected PlayerBase player;
    protected int curDefaultAbilityIndex = -1;
    protected int defaultAbilityCount = 0;
    protected List<AbilityInstance> defaultAbilityList = new List<AbilityInstance>();
	protected AbilityInstance lastDefaultAbility = null;
	public AbilityInstance LastDefaultAbility
	{
		get
		{
			return lastDefaultAbility;
		}
	}

	protected AbilityInstance roundAbility = null;
    /// <summary>
    /// 等待后台确认的技能列表  by吴江 
    /// </summary>
    protected FDictionary waitServerConfirmAbilityDic = new FDictionary();
    /// <summary>
    /// 击退，即倒等尚未消失的效果列表
    /// </summary>
    protected Dictionary<int, AbilityResultInfo> lockStateList = new Dictionary<int, AbilityResultInfo>();
    /// <summary>
    /// 翅膀被动技能触发抛出事件
    /// </summary>
    public System.Action<int> OnPassiveWingSkillTrigger;
    //======================================佣兵部分=============================
    protected EntourageBase entourage;

    protected List<AbilityInstance> entourageDefaultAbilityList = new List<AbilityInstance>();
    protected AbilityInstance entourageNormalAbility = null;
    /// <summary>
    /// 主角试图攻击的事件
    /// </summary>
    public System.Action OnMainPlayerUseAbility;
    /// <summary>
    /// 主角被攻击的事件
    /// </summary>
    public System.Action<SmartActor> OnMainPlayerBeHit;
	/// <summary>
	/// 主角被攻击的事件(被玩家攻击,间隔5秒)
	/// </summary>
	public System.Action<OtherPlayer> OnMainPlayerBeAttack;

    //==========连击部分======
    /// <summary>
    /// 最大间隔时间，超过这个时间，连击中断
    /// </summary>
    protected float maxInterTime = 2.0f;
    /// <summary>
    /// 当前连击数量
    /// </summary>
    protected int continuCount = 0;
    ///// <summary>
    ///// 当前连击数量
    ///// </summary>
    //public int ContinuCount
    //{
    //    get { return continuCount; }
    //    set
    //    {
    //        if (continuCount != value)
    //        {
    //            continuCount = value;
    //            if (OnContinuCountUpdate != null && continuCount > 0)
    //            {
    //                OnContinuCountUpdate(continuCount);
    //            }
    //        }
    //    }
    //}
    /// <summary>
    /// 连击数量变化的事件
    /// </summary>
    public System.Action<float> OnContinuCountUpdate;
    /// <summary>
    /// 当前是否在连击状态中
    /// </summary>
    protected bool isContinuHiting = false;
    /// <summary>
    /// 当前是否在连击状态中
    /// </summary>
    public bool IsContinuHiting
    {
        get { return isContinuHiting; }
        protected set
        {
            if (isContinuHiting != value)
            {
                isContinuHiting = value;
                if (OnContinuStateUpdate != null)
                {
                    OnContinuStateUpdate(isContinuHiting);
                }
            }
        }
    }
    /// <summary>
    /// 连击状态变化的事件
    /// </summary>
    public System.Action<bool> OnContinuStateUpdate;
    /// <summary>
    /// 上一次攻击的时间
    /// </summary>
    protected float lastHitTime = 0;
    #endregion

    #region 构造
    public static AbilityMng CreateNew()
    {
        if (GameCenter.abilityMng == null)
        {
            AbilityMng abilityMng = new AbilityMng();
            abilityMng.Init();
            return abilityMng;
        }
        else
        {
            GameCenter.abilityMng.UnRegist();
            GameCenter.abilityMng.Init();
            return GameCenter.abilityMng;
        }
    }
    /// <summary>
    /// 初始化
    /// </summary>
    protected virtual void Init()
    {
        MsgHander.Regist(0xC006, S2C_AbilityResult);
        MsgHander.Regist(0xC008, S2C_BuffUpdate);
        MsgHander.Regist(0xC009, S2C_BuffDelete);
        MsgHander.Regist(0xC021, S2C_OnAbilityWarnning);
        MsgHander.Regist(0xC022, S2C_OnAbilityCancelWarnning);
        MsgHander.Regist(0xC00B, S2C_EquipmentPassiveSkillTrriger);
    }
    /// <summary>
    /// 注销
    /// </summary>
    protected virtual void UnRegist()
    {
        MsgHander.UnRegist(0xC006, S2C_AbilityResult);
        MsgHander.UnRegist(0xC008, S2C_BuffUpdate);
        MsgHander.UnRegist(0xC009, S2C_BuffDelete);
        MsgHander.UnRegist(0xC021, S2C_OnAbilityWarnning);
        MsgHander.UnRegist(0xC022, S2C_OnAbilityCancelWarnning);
        MsgHander.UnRegist(0xC00B, S2C_EquipmentPassiveSkillTrriger);
    }
    #endregion

	protected Dictionary<int,float> abilityResultUserDic = new Dictionary<int, float>();

    #region S2C
    protected void S2C_AbilityResult(Pt _info)
    {
        if (GameCenter.sceneMng == null || !GameCenter.sceneMng.EnterSucceed) return;
        pt_scene_skill_effect_c006 pt = _info as pt_scene_skill_effect_c006;

        if (pt != null)
        {
			//Debug.Log("effectSort:"+(AbilityResultCAUSEType)pt.effect_sort+",targetID:"+pt.target_id+",ObjectType:"+(ObjectType)pt.obj_sort);
            SmartActor user = null;
            ObjectType type = (ObjectType)pt.obj_sort;
			MainPlayerInfo mainPlayerInfo = GameCenter.mainPlayerMng.MainPlayerInfo;
            switch (type)
            {

                case ObjectType.Player:
					if ((int)pt.oid == mainPlayerInfo.ServerInstanceID)
                    {
                        user = GameCenter.curMainPlayer;
                    }
                    else
                    {
                        user = GameCenter.curGameStage.GetOtherPlayer((int)pt.oid) as SmartActor;
                    }
                    break;
                case ObjectType.MOB:
                    user = GameCenter.curGameStage.GetMOB((int)pt.oid) as SmartActor;
                    break;
                case ObjectType.Entourage:
                    user = GameCenter.curGameStage.GetEntourage((int)pt.oid) as EntourageBase;
                    break;
                default:
                    break;
            }


            for (int i = 0; i < pt.effect_list.Count; i++)
            {
                st.net.NetBase.skill_effect ef = pt.effect_list[i];
                ActorInfo info = null;
                type = (ObjectType)ef.target_sort;
                //if (user == GameCenter.curMainPlayer)
                //{
                //    if ((AttackResultType)ef.atk_sort != AttackResultType.ATT_SORT_DODGE)
                //    {
                //        if (OnContinuCountUpdate != null)
                //        {
                //            OnContinuCountUpdate(Time.time);
                //        }
                //    }
                //}
                switch (type)
                {
                    case ObjectType.Player:
						if (ef.target_id == mainPlayerInfo.ServerInstanceID)
                        {
							mainPlayerInfo.Update(ef);
                            if ((DefResultType)ef.def_sort != DefResultType.DEF_SORT_ADDMP && (DefResultType)ef.def_sort != DefResultType.DEF_SORT_TREAT)//如果是非增益后果
                            {
                                if (OnMainPlayerBeHit != null && user != null)//主角被打的事件
                                {
                                    OnMainPlayerBeHit(user);
                                }
								OtherPlayer other = user as OtherPlayer;
								if(other != null && mainPlayerInfo.CurSceneType == SceneType.SCUFFLEFIELD)
								{
									if(!abilityResultUserDic.ContainsKey(other.id) || Time.time - abilityResultUserDic[other.id] > 5f)//被玩家攻击提示,5秒不重复
									{
										abilityResultUserDic[other.id] = Time.time;
										GameCenter.messageMng.AddClientMsg((mainPlayerInfo.KillingValue > 1000 || mainPlayerInfo.IsCounterAttack)?434:433,new string[1]{other.ActorName});
										if(OnMainPlayerBeAttack != null)
											OnMainPlayerBeAttack(other);
									}
	                            }
							}
                        }
                        else
                        {
                            info = GameCenter.sceneMng.GetOPCInfo((int)ef.target_id) as OtherPlayerInfo;
                            if (info != null) info.Update(ef);
                        }
                        break;
                    case ObjectType.MOB:
                        info = GameCenter.sceneMng.GetMobInfo((int)ef.target_id) as MonsterInfo;
                        if (info != null)
                        {
                            info.Update(ef);
                        }
                        break;
                    case ObjectType.Entourage:
                        if (GameCenter.mercenaryMng.curMercernaryInfo != null && ef.target_id == GameCenter.mercenaryMng.curMercernaryInfo.ServerInstanceID)
                        {
                            GameCenter.mercenaryMng.curMercernaryInfo.Update(ef);
                        }
                        else
                        {
                            info = GameCenter.sceneMng.GetEntourageInfo((int)ef.target_id) as MercenaryInfo;
                            if (info != null) info.Update(ef);
                        }
                        break;
                }
            }


            AbilityInstance instance = null;

            if ((int)pt.oid == GameCenter.curMainPlayer.id || (GameCenter.curMainEntourage != null && (int)pt.oid == GameCenter.curMainEntourage.id))
            {
                if (waitServerConfirmAbilityDic.ContainsKey(pt.seq))
                {
                    instance = waitServerConfirmAbilityDic[pt.seq] as AbilityInstance;
                    if (instance != null)
                    {
                        instance.AbilityResult(pt);
                        instance.OnTringEnd -= OnAbilityInstanceTringEnd;
                        waitServerConfirmAbilityDic.Remove(pt.seq);
                    }
                }
                else
                {
                    if (pt.seq != 0)
                    {
                        GameSys.LogError("找不到等待技能 " + pt.seq);
                    }
                    else
                    {
                        if (user != null)
                        {
							if(pt.skill == 0)
							{
								Debug.LogError(user.name+"使用技能ID为0的技能");
							}else
							{
								instance = new AbilityInstance(pt, user);
							}
                            
                        }
                    }
                }
            }
            else
            {
				bool useSkill = (pt.effect_sort == 2);
                if (user != null)
                {
                    if (user.curTryUseAbility != null && user.curTryUseAbility.AbilityID == pt.skill && user.curTryUseAbility.RuneID == pt.rune
                    && user.curTryUseAbility.Level == pt.lev && user.curTryUseAbility.NeedPrepare && !user.curTryUseAbility.HasServerConfirm)
                    {
                        instance = user.curTryUseAbility;
                    }
                    else
                    {
                        instance = new AbilityInstance(pt, user);
						if((ObjectType)pt.obj_sort == ObjectType.Player)//这里的判断不能用type
						{
							//技能动作完成了才继续放技能,否则其他大风车技能会瞬移  by邓成
							//这种处理同时导致了,法师动作比特效慢，因为一个技能放完,实际还在isCasting
							if(useSkill)
							{
								user.UseAbility(instance);
							}
						}else
						{
							if(useSkill)//怪物也需要加这个,否则SkillReportID出现两次
								user.UseAbility(instance);
						}
                    }
                }
                else
                {
                    instance = new AbilityInstance(pt, null);
                }
                instance.AbilityResult(pt);
            }
            if (instance != null && user != null)
            {
                switch (instance.CurClientShowType)
                {
                    case ClientShowType.Lightingskill:
                        user.fxCtrl.CastLightingShake(instance);
                        break;
                }
            }
        }
    }

    protected void S2C_BuffUpdate(Pt _info)
    {
        pt_scene_chg_buff_c008 pt = _info as pt_scene_chg_buff_c008;
        if (pt != null)
        {
            BuffInfo bf = new BuffInfo(pt);
            switch ((ObjectType)pt.obj_sort)
            {
                case ObjectType.Player:
                    if (pt.oid == GameCenter.mainPlayerMng.MainPlayerInfo.ServerInstanceID)
                    {
                        GameCenter.mainPlayerMng.MainPlayerInfo.Update(bf, bf.BuffTypeID, (int)pt.oid);
                    }
                    else
                    {
                        OtherPlayerInfo info = GameCenter.sceneMng.GetOPCInfo((int)pt.oid);
                        if (info != null) info.Update(bf, bf.BuffTypeID, (int)pt.oid);
                    }
                    break;
                case ObjectType.MOB:
                    MonsterInfo mob = GameCenter.sceneMng.GetMobInfo((int)pt.oid);
                    if (mob != null) mob.Update(bf, bf.BuffTypeID, (int)pt.oid);
                    break;
                case ObjectType.Entourage:
                    MercenaryInfo ent = null;
                    if (GameCenter.mercenaryMng.curMercernaryInfo != null && (int)pt.oid == GameCenter.mercenaryMng.curMercernaryInfo.ServerInstanceID)
                    {
                        ent = GameCenter.mercenaryMng.curMercernaryInfo;
                    }
                    else
                    {
                        ent = GameCenter.sceneMng.GetEntourageInfo((int)pt.oid);
                    }
                    if (ent != null) ent.Update(bf, bf.BuffTypeID, (int)pt.oid);
                    break;
                default:
                    break;
            }
        }
    }

    protected void S2C_BuffDelete(Pt _info)
    {
        pt_scene_remove_buff_c009 pt = _info as pt_scene_remove_buff_c009;
        if (pt != null)
        {
            switch ((ObjectType)pt.obj_sort)
            {
                case ObjectType.Player:
                    if (pt.oid == GameCenter.mainPlayerMng.MainPlayerInfo.ServerInstanceID)
                    {
                        GameCenter.mainPlayerMng.MainPlayerInfo.Update(null, (int)pt.buff_type, (int)pt.oid);
                    }
                    else
                    {
                        OtherPlayerInfo info = GameCenter.sceneMng.GetOPCInfo((int)pt.oid) as OtherPlayerInfo;
                        if (info != null) info.Update(null, (int)pt.buff_type, (int)pt.oid);
                    }
                    break;
                case ObjectType.MOB:
                    MonsterInfo mob = GameCenter.sceneMng.GetMobInfo((int)pt.oid) as MonsterInfo;
                    if (mob != null) mob.Update(null, (int)pt.buff_type, (int)pt.oid);
                    break;
                case ObjectType.Entourage:
                     MercenaryInfo ent = null;
                     if (GameCenter.mercenaryMng.curMercernaryInfo != null && (int)pt.oid == GameCenter.mercenaryMng.curMercernaryInfo.ServerInstanceID)
                    {
                        ent = GameCenter.mercenaryMng.curMercernaryInfo;
                    }
                    else
                    {
                        ent = GameCenter.sceneMng.GetEntourageInfo((int)pt.oid);
                    }
                    if (ent != null) ent.Update(null, (int)pt.buff_type, (int)pt.oid);
                    break;
                default:
                    break;
            }
        }
    }

    protected void S2C_OnAbilityWarnning(Pt _info)
    {
        pt_scene_skill_aleret_c021 msg = _info as pt_scene_skill_aleret_c021;
        if (msg != null)
        {
            ObjectType type = (ObjectType)msg.obj_sort;
            SmartActor user = null;
            switch (type)
            {
                case ObjectType.Player:
                    if ((int)msg.oid == GameCenter.mainPlayerMng.MainPlayerInfo.ServerInstanceID)
                    {
                        user = GameCenter.curMainPlayer;
                    }
                    else
                    {
                        user = GameCenter.curGameStage.GetOtherPlayer((int)msg.oid) as SmartActor;
                    }
                    break;
                case ObjectType.MOB:
                    user = GameCenter.curGameStage.GetMOB((int)msg.oid) as SmartActor;
                    break;
                case ObjectType.Entourage:
                    user = GameCenter.curGameStage.GetEntourage((int)msg.oid) as EntourageBase;
                    break;
                default:
                    break;
            }
            if (user != null)
            {
                AbilityInstance instance = new AbilityInstance(msg, user);
                user.UseAbility(instance);
            }

        }
    }

    protected void S2C_OnAbilityCancelWarnning(Pt _info)
    {
        pt_scene_skill_aleret_cancel_c022 msg = _info as pt_scene_skill_aleret_cancel_c022;
        if (msg != null)
        {
            ObjectType type = (ObjectType)msg.obj_sort;
            SmartActor user = null;
            switch (type)
            {
                case ObjectType.Player:
                    if ((int)msg.oid == GameCenter.mainPlayerMng.MainPlayerInfo.ServerInstanceID)
                    {
                        user = GameCenter.curMainPlayer;
                    }
                    else
                    {
                        user = GameCenter.curGameStage.GetOtherPlayer((int)msg.oid) as SmartActor;
                    }
                    break;
                case ObjectType.MOB:
                    user = GameCenter.curGameStage.GetMOB((int)msg.oid) as SmartActor;
                    break;
                case ObjectType.Entourage:
                    user = GameCenter.curGameStage.GetEntourage((int)msg.oid) as EntourageBase;
                    break;
                default:
                    break;
            }
            if (user != null)
            {
                AbilityInstance instance = user.curTryUseAbility;
                if (instance == null) return;
                if (instance.AbilityID == msg.skill && instance.RuneID == msg.skill_rune
                    && instance.Level == msg.lev && instance.NeedPrepare && !instance.HasServerConfirm)
                {
                    user.CancelAbility();
                }
            }
        }
    }
    /// <summary>
    /// 翅膀、法宝、玩家被动技能被触发时
    /// </summary>
    /// <param name="_info"></param>
    protected void S2C_EquipmentPassiveSkillTrriger(Pt _info)
    {
        pt_passive_skill_effect_c00b msg = _info as pt_passive_skill_effect_c00b;
        if (msg != null)
        {
           PassiveSkillRef data =  ConfigMng.Instance.GetPassiveSkillRef((int)msg.skill_id);
           if (data != null )
           {
               switch (data.type)
               { 
                   //翅膀被动技能
                   case PassiveSkillType.WING:
                        OnPassiveWingSkillTrigger((int)msg.skill_id);
                        break;
               }
           }
        }
    }
    #endregion

    #region C2S
    /// <summary>
    /// 向服务端发送使用技能的请求
    /// </summary>
    /// <param name="_abilityType"></param>
    /// <param name="_pos"></param>
    /// <param name="_dir"></param>
    /// <param name="_targetID"></param>
    public void C2S_UseAbility(AbilityInstance _instance)
    {
        pt_scene_skill_c005 msg = new pt_scene_skill_c005();
        msg.oid = (uint)_instance.UserActor.id;
        msg.skill = (uint)_instance.AbilityID;
        msg.lev = 1;
        msg.target_id = _instance.TargetActor == null ? 0 : (uint)_instance.TargetActor.id;
        msg.target_x = _instance.TargetPostion.x;
        msg.target_y = _instance.TargetPostion.y;
        msg.target_z = _instance.TargetPostion.z;
        msg.x = _instance.UserPostion.x;
        msg.y = _instance.UserPostion.y;
        msg.z = _instance.UserPostion.z;
        msg.dir = _instance.DirY;
        msg.seq = NetMsgMng.CreateNewUnLockSerializeID();
        _instance.LastTryTime = Time.time;
        NetMsgMng.SendMsg(msg);
        _instance.serializeID = msg.seq;
        _instance.OnTringEnd += OnAbilityInstanceTringEnd;
        waitServerConfirmAbilityDic[msg.seq] = _instance;
        if (OnMainPlayerUseAbility != null && _instance.UserActor.id == GameCenter.curMainPlayer.id)
        {
            OnMainPlayerUseAbility();
        }
    }

    /// <summary>
    /// 使用急救术
    /// </summary>
    /// <param name="_instance"></param>
    public void C2S_UseAddHpAbility(AbilityInstance _instance)
    {
        Debug.logger.Log("使用急救术");
        pt_scene_skill_c005 msg = new pt_scene_skill_c005();
        msg.oid = (uint)_instance.UserActor.id;
        msg.skill = (uint)_instance.AbilityID;
        msg.lev = 1;
        msg.target_id = _instance.TargetActor == null ? 0 : (uint)_instance.TargetActor.id;
        msg.target_x = _instance.TargetPostion.x;
        msg.target_y = _instance.TargetPostion.y;
        msg.target_z = _instance.TargetPostion.z;
        msg.x = _instance.UserPostion.x;
        msg.y = _instance.UserPostion.y;
        msg.z = _instance.UserPostion.z;
        msg.dir = _instance.DirY;
        msg.seq = NetMsgMng.CreateNewUnLockSerializeID();
        _instance.LastTryTime = Time.time;
        NetMsgMng.SendMsg(msg);
        _instance.serializeID = msg.seq;
        _instance.OnTringEnd += OnAbilityInstanceTringEnd;
        waitServerConfirmAbilityDic[msg.seq] = _instance;
        if (OnMainPlayerUseAbility != null && _instance.UserActor.id == GameCenter.curMainPlayer.id)
        {
            OnMainPlayerUseAbility();
        }
    }


    ///// <summary>
    ///// 击退，击倒效果结束
    ///// </summary>
    ///// <param name="_id"></param>
    //public void C2S_FinishedLockState(int _id)
    //{
    //    Cmd cmd = new Cmd(0x2293);
    //    cmd.write_int(_id);
    //    NetMsgMng.SendCMD(cmd);
    //}
    #endregion

    #region 辅助逻辑
    public void UnLockRef()
    {
        for (int i = 0; i < defaultAbilityList.Count; i++)
        {
            defaultAbilityList[i].ResetResult(null);
        }
        for (int i = 0; i < entourageDefaultAbilityList.Count; i++)
        {
            entourageDefaultAbilityList[i].ResetResult(null);
        }
        if (GameCenter.skillMng != null && GameCenter.skillMng.abilityDic != null)
        {
            foreach (AbilityInstance item in GameCenter.skillMng.abilityDic.Values)
            {
                item.ResetResult(null);
            }
        }
    }

    protected void OnAbilityInstanceTringEnd(uint _serializeID)
    {
        if (waitServerConfirmAbilityDic.ContainsKey(_serializeID))
        {
            AbilityInstance ins = waitServerConfirmAbilityDic[_serializeID] as AbilityInstance;
            if (ins != null)
            {
                ins.OnTringEnd -= OnAbilityInstanceTringEnd;
                ins.serializeID = 0;
            }
            waitServerConfirmAbilityDic.Remove(_serializeID);
        }
    }

    /// <summary>
    /// 清理所有的硬直状态(一般是由于切换场景)
    /// </summary>
    //public void CleanLockState()
    //{
    //    foreach (var item in lockStateList.Keys)
    //    {
    //        C2S_FinishedLockState(item);
    //    }
    //    lockStateList.Clear();
    //}

    /// <summary>
    /// 设置佣兵信息 by吴江 
    /// </summary>
    /// <param name="_info"></param>
    public void SetEntourage(MainEntourage _entourage,MercenaryInfo _info)
    {
        entourage = _entourage;
        entourageDefaultAbilityList.Clear();
        entourageNormalAbility = null;
        if (entourage == null) return;

        for (int i = 0; i < _info.PetSkillList.Count; i++)
        { 
            AbilityInstance instance = new AbilityInstance((int)_info.PetSkillList[i], 1, 1, entourage, null);
            instance.FullCD();
            entourageDefaultAbilityList.Add(instance);
        }

        entourageNormalAbility = new AbilityInstance(_info.NormalSkill, 1, entourage, null);
		AttackDiffTime = entourageNormalAbility.AbilityCD;
    }




    /// <summary>
    /// 设置所属玩家
    /// </summary>
    /// <param name="_player"></param>
    public void SetPlayer(PlayerBase _player)
    {
        player = _player;
        defaultAbilityList.Clear();
        defaultAbilityCount = 0;
        curDefaultAbilityIndex = 0;
        if (player == null) return;
        PlayerConfig playerRef = ConfigMng.Instance.GetPlayerConfig(player.Prof);
        if (playerRef != null)
        {
            List<int> list = playerRef.mormalSkillList;
            foreach (var item in list)
            {
                AbilityInstance abilityInstance = new AbilityInstance(item, 1, player, null);
                defaultAbilityList.Add(abilityInstance);
            }
            defaultAbilityCount = defaultAbilityList.Count;
            curDefaultAbilityIndex = 0;
			if(playerRef.roundSkill != 0)roundAbility = new AbilityInstance(playerRef.roundSkill, 1, player, null);
        }
    }

    /// <summary>
    /// 获取下一次普通攻击的类型
    /// </summary>
    /// <returns></returns>
    public AbilityInstance GetNextDefaultAbility(SmartActor _target)
    {
		if(LastDefaultAbility != null && LastDefaultAbility.RestCD <= 0)
		{
			ResetDefaultAbility();//普攻CD结束,则重置到第一个普攻
		}
        int curIndex = CountCurIndex(curDefaultAbilityIndex, defaultAbilityCount);
        AbilityInstance instance = defaultAbilityList[curIndex];
        if (instance != null)
        {
            instance.ResetResult(_target);
            return instance;
        }
        return null;
    }

	protected float AttackDiffTime = 0;
	protected float previousAbilityTime = 0;
    /// <summary>
    /// 获取随从要使用的下一个技能
    /// </summary>
    /// <param name="_target"></param>
    /// <returns></returns>
    public AbilityInstance GetNextEntourageAbility(SmartActor _target)
    {
        AbilityInstance curInstance = null;
		//以普攻CD为间隔,大约三秒发动一次攻击 by邓成
		if(!(Time.time - previousAbilityTime > AttackDiffTime))
			return null;
        for (int i = 0; i < entourageDefaultAbilityList.Count; i++)
        {
            if (entourageDefaultAbilityList[i].RestCD <= 0)
            {
                int curRandom = UnityEngine.Random.Range(0, 100);
                if (curRandom / 100f > entourageDefaultAbilityList[i].PetUseRate)
                {
                    curInstance = entourageDefaultAbilityList[i];
                    break;
                }
            }
        }
		if (curInstance == null && entourageNormalAbility != null && entourageNormalAbility.RestCD <= 0)
        {
            curInstance = entourageNormalAbility;
        }
        if (curInstance != null)
        {
            curInstance.ResetResult(_target);
			previousAbilityTime = Time.time;
        }
        if (curInstance != null)
        {
            curInstance.FullCD();
        }
        return curInstance;
    }

	/// <summary>
	/// 获取翻滚技能,可能为null(法师没有冲锋)
	/// </summary>
	/// <returns></returns>
	public AbilityInstance GetRoundAbility(SmartActor _target)
	{
		if(roundAbility != null)roundAbility.ResetResult(_target);
		return roundAbility;
	}

	public bool IsRoundAbility(AbilityInstance _abilityInstance)
	{
        if (roundAbility != null && _abilityInstance != null && _abilityInstance.AbilityID == roundAbility.AbilityID)
		{
			return true;
		}
		return false;
	}

    /// <summary>
    /// 确认使用了一次普通攻击
    /// </summary>
	public void UsedDefaultAbility(AbilityInstance _instance)
    {
		lastDefaultAbility = _instance;
        curDefaultAbilityIndex = CountCurIndex(curDefaultAbilityIndex, defaultAbilityCount);
    }
    /// <summary>
    /// 检查是否为普通攻击
    /// </summary>
    /// <returns></returns>
    public bool CheckIsDefaultAbility(AbilityInstance _instance)
    {
        return defaultAbilityList.Contains(_instance);
    }
    /// <summary>
    /// 当前的普通攻击序数
    /// </summary>
    /// <param name="_lastIndex"></param>
    /// <param name="_maxIndex"></param>
    /// <returns></returns>
    protected int CountCurIndex(int _lastIndex, int _maxIndex)
    {
        if (_lastIndex >= 0)
        {
            ++_lastIndex;
            _lastIndex = _lastIndex % _maxIndex;
        }
        else
        {
            _lastIndex = 0;
        }
        return _lastIndex;
    }
    /// <summary>
    /// 重置默认技能
    /// </summary>
    public void ResetDefaultAbility()
    {
        curDefaultAbilityIndex = -1;
    }
    /// <summary>
    /// 停止连击
    /// </summary>
    public void StopContinHit()
    {
        IsContinuHiting = false;
        continuCount = 0;
    }
    /// <summary>
    /// 获取普通攻击的特效名称列表
    /// </summary>
    /// <returns></returns>
    public List<string> GetDefaultAbilityEffectNames()
    {
        List<string> list = new List<string>();
        foreach (AbilityInstance item in defaultAbilityList)
        {
            for (int i = 0; i < item.AllNeedEffectNames.Count; i++)
            {
                if (item.AllNeedEffectNames[i] != string.Empty && !list.Contains(item.AllNeedEffectNames[i]))
                {
                    list.Add(item.AllNeedEffectNames[i]);
                }
            }
        }
        return list;
    }
    ///// <summary>
    ///// 服务端确认攻击后果
    ///// </summary>
    //public void OnServerHitResult()
    //{
    //    if (!IsContinuHiting)
    //    {
    //        IsContinuHiting = true;
    //    }
    //    else
    //    {
    //        if (Time.time - lastHitTime > maxInterTime)
    //        {
    //           // IsContinuHiting = false;
    //            ContinuCount = 0;
    //        }
    //        else
    //        {
    //            ContinuCount++;
    //        }
    //    }
    //    lastHitTime = Time.time;
    //}

    /// <summary>
    /// 硬直状态更新
    /// </summary>
    //public void LockStateTick()
    //{
    //    if (lockStateList.Count > 0)
    //    {
    //        List<AbilityResultInfo.AbilityInfluenceInfo> removeList = new List<AbilityResultInfo.AbilityInfluenceInfo>();
    //        foreach (var item in lockStateList.Values)
    //        {
    //            if ((Time.time - item.ClientGetTime) >= (item.rushTotalTime / 1000.0f))
    //            {
    //                removeList.Add(item);
    //            }
    //        }
    //        foreach (var item in removeList)
    //        {
    //            C2S_FinishedLockState(item.SerializeID);
    //            lockStateList.Remove(item.SerializeID);
    //        }
    //    }
    //}

    public bool HasLockState
    {
        get
        {
            return lockStateList.Count > 0;
        }
    }
    #endregion

}
