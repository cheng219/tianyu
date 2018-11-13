//==============================================
//作者：黄洪兴
//日期：2016/3/9
//用途：技能升级管理类
//=================================================



using UnityEngine;
using System.Collections;
using st.net.NetBase;
using System;
using System.Collections.Generic;

public class SkillMng 
{

    public List<SkillInfo> skillList = new List<SkillInfo>();
    /// <summary>
    /// 新学会的技能列表
    /// </summary>
    public List<SkillInfo> NewSkillList = new List<SkillInfo>();
	/// <summary>
	/// 当前使用的技能列表
	/// </summary>
	public List<SkillInfo> useSkills = new List<SkillInfo>();
    /// <summary>
    /// 技能列表
    /// </summary>
    public Dictionary<int, SkillInfo> skillDic = new Dictionary<int, SkillInfo>();
    /// <summary>
    /// 技能实例列表 
    /// </summary>
    public Dictionary<int, AbilityInstance> abilityDic = new Dictionary<int,AbilityInstance>();
    /// <summary>
    /// 当前职业对应的技能列表
    /// </summary>
    public List<SkillMainConfigRef> allskill = new List<SkillMainConfigRef>();
    /// <summary>
    /// 当前选择的技能
    /// </summary>
    public SkillInfo CurSkillInfo = null;
    /// <summary>
    /// 急救术技能
    /// </summary>
    public SkillInfo AddHpSkillInfo = null;
    public AbilityInstance FirstAidInstance
    {
        get 
        { 
            if (abilityDic.ContainsKey(firstAid))
                return abilityDic[firstAid];
            return null;
        }
        set
        {
            if (abilityDic.ContainsKey(firstAid))
                abilityDic[firstAid] = value;
        }
    }

	/// <summary>
	/// 改变技能位置的事件
	/// </summary>
	public Action OnChangeSkill;
    /// <summary>
    /// 技能列表更新事件
    /// </summary>
    public Action OnUpdateSkillList;
    /// <summary>
    /// 改变技能需要加载新的特效
    /// </summary>
    public Action OnShowSkillEffect;

    public Action<int> OnPlayNewSkillGetAnimation;


      bool isFirst=true;


    /// <summary>
    /// 符文本地保存的ID标识
    /// </summary>
    private string runeConfigID = string.Empty;
    public string RuneConfigID
    {
        get 
        {
            if (runeConfigID == string.Empty)
            {
                if (PlayerPrefs.HasKey("LastName"))
                    runeConfigID = PlayerPrefs.GetString("LastName");
                runeConfigID += GameCenter.curMainPlayer.id;
            }
            return runeConfigID; 
        }
    }
    /// <summary>
    /// 急救术ID
    /// </summary>
    private int firstAid = 0;
    #region 构造
    /// <summary>
    /// 返回该管理类的唯一实例 by 贺丰
    /// </summary>
    /// <returns></returns>
    public static SkillMng CreateNew(ref SkillMng _skillMng)
    {
        if (_skillMng == null)
        {
            SkillMng skillMng = new SkillMng();
            skillMng.Init();
            return skillMng;
        }
        else
        {
            _skillMng.UnRegist();
            _skillMng.Init();
            return _skillMng;
        }
    }
    /// <summary>
    /// 注册
    /// </summary>
    protected virtual void Init()
    {
        MsgHander.Regist(0xD100, S2C_OnGetSkillList);
		MsgHander.Regist(0xD401, S2C_OnGetUseSkillList);
        for (int i = 0; i < 4; i++)
        {
            useSkills.Add(null);
            
        }
        //AddSkill();
       
    }
    /// <summary>
    /// 注销
    /// </summary>
    protected virtual void UnRegist()
    {
        MsgHander.UnRegist(0xD100, S2C_OnGetSkillList);
		MsgHander.UnRegist(0xD401, S2C_OnGetUseSkillList);
        ResetData();
        GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate -= RefreshRed;
    }
    #endregion

    #region 通信S2C
    /// <summary>
    /// 得到技能列表
    /// </summary>
    /// <param name="_info"></param>
    private void S2C_OnGetSkillList(Pt _info)
	{
        //Debug.Log("获得技能列表");
        bool b = false;
        if (skillDic.Count < 1)
        {
            AddSkill();
            b = true;
        }
		pt_all_skill_d100 _pt = _info as pt_all_skill_d100;
		if (_pt != null) {
           // NewSkillList.Clear();
			for (int i = 0; i < _pt.normal_skill_list.Count; i++) {
				//单独升级技能 或者解锁符文时  服务端列表中只发 单个已经修改的数据
				SkillInfo skillinfo = new SkillInfo (_pt.normal_skill_list[i]);
                //Debug.Log("获得技能" + b + ":" + skillDic.ContainsKey((int)_pt.normal_skill_list[i].skill_id) + "技能ID" + (int)_pt.normal_skill_list[i].skill_id);
                if (!b && skillDic.ContainsKey((int)_pt.normal_skill_list[i].skill_id))
                {
                    if (!skillDic[(int)_pt.normal_skill_list[i].skill_id].isEnable)
                    {
                        NewSkillList.Add(skillinfo);
                    }
                }
                if (skillDic.ContainsKey(skillinfo.SkillID))
                {
                    if (skillDic[skillinfo.SkillID].SkillLv < skillinfo.SkillLv)
                    {
                        if (OnShowSkillEffect != null)
                        {
                            OnShowSkillEffect();
                        }
                    }
                }
                skillDic[(int)_pt.normal_skill_list[i].skill_id] = skillinfo;


				//Debug.Log ("收到的技能:"+_pt.normal_skill_list [i].skill_id+",等级为" + (int)_pt.normal_skill_list [i].skill_lev+"是否学习状态为"+skillinfo.isEnable);
                /////技能实例列表更新
                //if (skillinfo.SkillRole != 0) {                 
                //    if (!abilityDic.ContainsKey (skillinfo.SkillID)) {
                //        AbilityInstance abilityInstance = new AbilityInstance (skillinfo.SkillID, skillinfo.SkillLv, skillinfo.SkillCurRune, GameCenter.curMainPlayer, null);
                //        abilityDic [skillinfo.SkillID] = abilityInstance;
                //    } else {
                //        abilityDic [skillinfo.SkillID].Update (_pt.normal_skill_list [i]);
                //    }
                //}
//               if ( ConfigMng.Instance.GetSkillMainConfigRef(skillinfo.SkillID).skillField != 0 )
//               {                    
//					///这里是需要在选择符文或者学习到新技能触发预加载的事件
//                    bool getNewSkill = skillDic[skillinfo.SkillID].SkillLv == 0 && skillinfo.SkillLv ==1;
//                    if (skillDic[skillinfo.SkillID].SkillLv == skillinfo.SkillLv || getNewSkill)
//                    {
//                        if (OnChangeInstance!=null)
//                            OnChangeInstance();
//                    }
//                    skillDic[skillinfo.SkillID] = skillinfo;
//                    
//                    if (OnGetNewSkill != null && getNewSkill)
//                    {
//                        OnGetNewSkill(skillinfo);
//                    }
			}
                
            
			if (CurSkillInfo != null) { 
				CurSkillInfo = skillDic [CurSkillInfo.SkillID];
			}
        
			if (OnUpdateSkillList != null) {
				OnUpdateSkillList ();
			}

            if (NewSkillList.Count > 0)
            {
                if (GameCenter.uIMng.CurOpenType != GUIType.NEWSKILL)
                {
                    GameCenter.curMainPlayer.StopMovingTo();
                    GameCenter.curMainPlayer.CancelCommands();
                   // Debug.Log("获得新技能");
                    GameCenter.uIMng.SwitchToUI(GUIType.NONE);
                    GameCenter.uIMng.ReleaseGUI(GUIType.NEWSKILL);
                    GameCenter.uIMng.GenGUI(GUIType.NEWSKILL, true);
                }
            }
            if (isFirst)
            {
                GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate += RefreshRed;
                isFirst = false;
            }
            RefreshRed(ActorBaseTag.BindCoin, 0, false);
		}
	}
	private void S2C_OnGetUseSkillList(Pt _info)
	{
		pt_use_skill_list_d401 pt = _info as pt_use_skill_list_d401; 
		if(pt!=null)
		{
            List<int> oldSkillID = new List<int>();
            for (int i = 0; i < 4; i++)
            {
                if (useSkills[i] != null)
                {
                    oldSkillID.Add(useSkills[i].SkillID);
                }
                else
                {
                    oldSkillID.Add(0);
                }
            }
			if (useSkills != null) {
				useSkills.Clear ();
			}
			List<SkillInfo> list = new List<SkillInfo> ();

			for (int i = 0; i < pt.use_skill_list.Count; i++) {
				if (skillDic.ContainsKey (pt.use_skill_list [i].skill_1) && pt.use_skill_list [i].skill_1 != 0) {
					list.Add (skillDic [pt.use_skill_list [i].skill_1]);
				} else {
					list.Add (null);
                    if (oldSkillID[0]!= 0)
                    {
                        abilityDic.Remove(oldSkillID[0]);
                    }
				}
				if (skillDic.ContainsKey (pt.use_skill_list [i].skill_2) && pt.use_skill_list [i].skill_2 != 0) {
					list.Add (skillDic [pt.use_skill_list [i].skill_2]);
				} else {
					list.Add (null);
                    if (oldSkillID[1] != 0)
                    {
                        abilityDic.Remove(oldSkillID[1]);
                    }
				}
				if (skillDic.ContainsKey (pt.use_skill_list [i].skill_3) && pt.use_skill_list [i].skill_3 != 0) {
					list.Add (skillDic [pt.use_skill_list [i].skill_3]);
				} else {
					list.Add (null);
                    if (oldSkillID[2] != 0)
                    {
                        abilityDic.Remove(oldSkillID[2]);
                    }
				}
				if (skillDic.ContainsKey (pt.use_skill_list [i].skill_4) && pt.use_skill_list [i].skill_4 != 0) {
					list.Add (skillDic [pt.use_skill_list [i].skill_4]);
				} else {
					list.Add (null);
                    if (oldSkillID[3] != 0)
                    {
                        abilityDic.Remove(oldSkillID[3]);
                    }
				}
                //Debug.Log("第1个技能是" + pt.use_skill_list[i].skill_1);
                //Debug.Log("第2个技能是" + pt.use_skill_list[i].skill_2);
                //Debug.Log("第3个技能是" + pt.use_skill_list[i].skill_3);
                //Debug.Log("第4个技能是" + pt.use_skill_list[i].skill_4);
			}
			useSkills = list;
            if (useSkills.Count > 0)
            {
                //abilityDic.Clear();
                for (int i = 0; i < useSkills.Count; i++)
                {
                    if (useSkills[i] == null)
                        continue;
                    if (useSkills[i].SkillRole != 0)
                    {
                        if (!abilityDic.ContainsKey(useSkills[i].SkillID))
                        {
                            AbilityInstance abilityInstance = new AbilityInstance(useSkills[i].SkillID, useSkills[i].SkillLv, useSkills[i].SkillCurRune, GameCenter.curMainPlayer, null);
                            abilityDic[useSkills[i].SkillID] = abilityInstance;
                        }
                        else
                        {
                            abilityDic[useSkills[i].SkillID].Update(useSkills[i]);
                        }
                    }

                }
            }
            //Debug.Log("获得技能位置");
			if (OnChangeSkill != null) {
				OnChangeSkill ();
			}

		}



		}


    #endregion

    #region C2S
    /// <summary>
    /// 请求技能列表
    /// </summary>
    public void C2S_SkillReq()
    {
        pt_action_d002 msg = new pt_action_d002();
        msg.action = 1012;
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 技能升级
    /// </summary>
    public void C2S_SkillUp(int _id)
    {
        //Debug.Log("技能升级");
        pt_action_two_int_d012 msg = new pt_action_two_int_d012();
        msg.action = 1004;
        msg.data_One = 0;
        msg.data_Two = _id;
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 符文请求 解锁行为 1013  选择行为 1014
    /// </summary>
    public void C2S_RoneAsk(int _skillid ,int _id ,int _action)
    {
        pt_action_two_int_d012 msg = new pt_action_two_int_d012();
        msg.action = _action;
        msg.data_One = _skillid;
        msg.data_Two = _id;
        NetMsgMng.SendMsg(msg);
    }

	public void C2S_changeSkill(List<int> skillList)
	{
		//Debug.Log ("发送技能位置");
		pt_change_skill_d400 msg = new pt_change_skill_d400 ();
		change_skill_list skill = new change_skill_list ();
		skill.skill_1 = skillList [0];
		skill.skill_2 = skillList [1];
		skill.skill_3 = skillList [2];
		skill.skill_4 = skillList [3];
		msg.change_skill_list.Add (skill);
		NetMsgMng.SendMsg(msg);

	}



    #endregion

    /// <summary>
    /// 获取技能界面展示的四个技能
    /// </summary>
    /// <returns></returns>
    public List<SkillInfo> GetShowSkill()
    {
        List<SkillInfo> list = new List<SkillInfo>(); 
        for (int i = 0; i < useSkills.Count; i++)
        {
            if (useSkills[i] != null)
            { 
                list.Add(useSkills[i]); 
            }
        }

        if (list.Count < 4)
        { 
            using (var e = skillDic.GetEnumerator())
            {
                while (e.MoveNext())
                {
                    if (e.Current.Value != null)
                    {
                        if (!abilityDic.ContainsKey(e.Current.Value.SkillID))
                        {
                            if (list.Count < 4)
                            {
                                list.Add(e.Current.Value);
                            }
                            else
                                break;
                        }
                    }
                }
            }
        }
        return list;
    }



    /// <summary>
    /// 重置数据
    /// </summary>
    void ResetData()
    {
        skillDic.Clear();
        abilityDic.Clear();
        useSkills.Clear();
        skillList.Clear();
        CurSkillInfo = null;
        NewSkillList.Clear();
        allskill.Clear();
        isFirst = true;
    }

    /// <summary>
    /// 获得一个已经可以使用技能的实例
    /// </summary>
    /// <returns></returns>
    public AbilityInstance GetAbilityRandom()
    {
        AbilityInstance instance = null;
        using (var e = abilityDic.GetEnumerator())
        {
            while (e.MoveNext())
            {
                if (e.Current.Value.UserActor == null)
                {
                    e.Current.Value.SetActor(GameCenter.curMainPlayer, GameCenter.curMainPlayer.CurTarget as SmartActor);
                }
                if (e.Current.Value.RestCD <= 0 && e.Current.Value.HadMp && e.Current.Value.IsAutoUse)
                {
                    instance = e.Current.Value;
                    break;
                }
            }
        }
        return instance;
    }
    #region 符文配置
    /// <summary>
    /// 设置符文配置
    /// </summary>
    public void SetSkillConfig(int _configID)
    {
        string runeID = RuneConfigID + _configID;
        string skillID;
        using (var e = skillDic.GetEnumerator())
        {
            while (e.MoveNext())
            {
                if (e.Current.Value.SkillLv > 0)
                {
                    skillID = runeID + e.Current.Value.SkillID;
                    PlayerPrefs.SetInt(skillID, e.Current.Value.SkillCurRune);
                }
            }
        }
    }
    /// <summary>
    /// 读取符文配置
    /// </summary>
    public void GetSkillConfig(int _configID)
    {
        string runeID = RuneConfigID + _configID;
        string skillID;
        using (var e = skillDic.GetEnumerator())
        {
            while (e.MoveNext())
            {
                skillID = runeID + e.Current.Value.SkillID;
                if (PlayerPrefs.HasKey(skillID))
                {
                    int LocalRuneId = PlayerPrefs.GetInt(skillID);
                    C2S_RoneAsk(e.Current.Value.SkillID, LocalRuneId, 1014);
                }
                else
                {
                    C2S_RoneAsk(e.Current.Value.SkillID, e.Current.Value.SkillBaseRune, 1014);
                }
            }
        }

    }
    #endregion

    #region 当前职业技能数据
    /// <summary>
    /// 添加上未解锁的技能
    /// </summary>
    void AddSkill()
    {
        int ProfID = GameCenter.mainPlayerMng.MainPlayerInfo.Prof;
        List<int> skillList = ConfigMng.Instance.GetPlayerConfig(ProfID).basic_skill;

       // List<SkillMainConfigRef> allskilltable = ConfigMng.Instance.GetProfSkillList(ProfID);
        allskill.Clear();
        if (skillList != null)
		{
            for (int i = 0; i < skillList.Count; i++)
	        {
                SkillInfo _info = new SkillInfo(skillList[i], 1, ConfigMng.Instance.GetSkillMainConfigRef(skillList[i]).baseRune);
	                skillDic[_info.SkillID] = _info;
			//	Debug.Log ("此时获得当前职业所有技能ID为"+_info.SkillID);
	        }
		}
		//Debug.Log ("当前职业所有的技能数为"+allskill.Count);
       // CurSkillInfo = new SkillInfo(allskill[0].skillId, 0);

        ///急救术 101
        //switch (ProfID)
        //{ 
        //    case 3:
        //        firstAid = 101;
        //        break;
        //    case 6:
        //        firstAid = 102;
        //        break;
        //    case 9:
        //        firstAid = 103;
        //        break;
        //}
        //SkillMainConfigRef addHpRef = ConfigMng.Instance.GetSkillMainConfigRef(firstAid);
        //if(addHpRef != null)
        //    AddHpSkillInfo = new SkillInfo(addHpRef.skillId, 1, addHpRef.baseRune);
        //addHpInstance = new AbilityInstance(AddHpSkillInfo.SkillID,1 , AddHpSkillInfo.SkillCurRune, GameCenter.curMainPlayer, null);
    }

    /// <summary>
    /// 根据人物当前属性改变技能的使用情况
    /// </summary>
    /// <param name="_tag"></param>
    /// <param name="_value"></param>
    /// <param name="_fromAbility"></param>
    protected void ChangeAutoUseSkill(ActorBaseTag _tag, int _value, bool _fromAbility)
    {
        if (_tag == ActorBaseTag.CurHP)
        {
            if (((float)GameCenter.mainPlayerMng.MainPlayerInfo.CurHP / GameCenter.mainPlayerMng.MainPlayerInfo.MaxHP) <= 0.5f)
            {
                FirstAidInstance.IsAutoUse = true;
            }
            else
            {
                FirstAidInstance.IsAutoUse = false;
            }
        }
    }
    /// <summary>
    /// 获取当前激活的技能所需要的特效列表 by吴江
    /// </summary>
    /// <returns></returns>
    public List<string> GetAbilityEffectNames()
    {
        List<string> list = new List<string>();
        if (abilityDic != null)
        {
            using (var e = abilityDic.GetEnumerator())
            {
                while (e.MoveNext())
                {
                    if (e.Current.Value == null || e.Current.Value.AbilityID <= 0) continue;
                    for (int i = 0; i < e.Current.Value.AllNeedEffectNames.Count; i++)
                    {
                        if (e.Current.Value.AllNeedEffectNames[i] != string.Empty && !list.Contains(e.Current.Value.AllNeedEffectNames[i]))
                        {
                            list.Add(e.Current.Value.AllNeedEffectNames[i]);
                        }
                    }
                }
            }
        }
        return list;
    }
    /// <summary>
    /// 更改自动使用技能的状态
    /// </summary>
    /// <param name="_id"></param>
    /// <param name="_use"></param>
    public void ChangeAutoUse(int _id, bool _autoUse)
    {
        if (abilityDic.ContainsKey(_id))
        {
            abilityDic[_id].IsAutoUse = _autoUse;
        }
    }
    /// <summary>
    /// 获取是否自动使用技能
    /// </summary>
    /// <returns></returns>
    public bool GetAbilityIsAuto(int _id)
    {
        if (abilityDic.ContainsKey(_id))
        {
            return abilityDic[_id].IsAutoUse;
        }
        return true;
    }

    /// <summary>
    /// 获取技能列表 1 主动 2 被动
    /// </summary>
    /// <returns></returns>
    public List<SkillInfo> GetSkillBySkillType(int _type)
    {
        List<SkillInfo> list = new List<SkillInfo>();

        using (var e = skillDic.GetEnumerator())
        {
            while (e.MoveNext())
            {
                int type = e.Current.Value.SkillType;
                if(type == 3)type = 2;//3为被动技能
                else type = 1;//1/2为主动技能
                if (type == _type)
                    list.Add(e.Current.Value);
            }
        }
        return list;
    }

    /// <summary>
    /// 获得已经学习的技能列表
    /// </summary>
    /// <returns></returns>
    public List<SkillInfo> GetOwnSkill()
    {
        List<SkillInfo> list = new List<SkillInfo>();

        using (var e = skillDic.GetEnumerator())
        {
            while (e.MoveNext())
            {
                if (e.Current.Value.isEnable)
                    list.Add(e.Current.Value);
            }
        }
        return list;
    }


    #endregion


  public  void RefreshRed(ActorBaseTag _a, ulong _b,bool _c)
    {
        skillList.Clear();
        using (var e = skillDic.GetEnumerator())
      {

            while(e.MoveNext())
            {
                SkillInfo nextSkill = new SkillInfo(e.Current.Value.SkillID, e.Current.Value.SkillLv + 1);
                skillList.Add(nextSkill);

            }
      }
        for (int i = 0; i < skillList.Count; i++)
        {
            if (skillList[i].CoinEnough && skillList[i].ResEnough && skillList[i].LevEnough && !skillList[i].isFullLevel)
            {
                GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.SKILLS,true);
                i = skillList.Count;
                continue;
            }
            if (i == skillList.Count - 1)
            {
                GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.SKILLS, false);
            }
        }
    }



}
