//====================================================
//作者: 黄洪兴
//日期：2016/3/28
//用途：技能单位的数据层对象
//======================================================




using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SkillServerData
{
    public int id;
    public int Lv;
	public bool isEnable;
    public List<int> rune_list;
    public int rune_use;
}

/// <summary>
/// 技能单位数据层对象 
/// </summary>
public class SkillInfo 
{
    #region 服务端数据 
    SkillServerData skillData;
    #endregion

    #region 静态配置数据 
    SkillMainConfigRef skillRef = null;
    public SkillMainConfigRef SkillRef
    {
        get
        {
            if (skillRef != null) return skillRef;
            skillRef = ConfigMng.Instance.GetSkillMainConfigRef(skillData.id);
            return skillRef;
        }
    }
    #region 构造 
    public SkillInfo(int _id, int _lv)
    {
        skillData = new SkillServerData();
		skillData.isEnable = false;
        skillData.id = _id;
        skillData.Lv = _lv;
    }
    public SkillInfo(int _id, int _lv,int _curRune)
    {
        skillData = new SkillServerData();
        skillData.id = _id;
        skillData.Lv = _lv;
		skillData.isEnable = false;
       // skillData.rune_use = _curRune;
    }
    public SkillInfo(st.net.NetBase.normal_skill_list _info)
    {
        skillData = new SkillServerData();
		skillData.isEnable = true;
        skillData.id = (int)_info.skill_id;
        skillData.Lv = (int)_info.skill_lev;
//        skillData.rune_list = new List<int>();
//        skillData.rune_list.Add((int)_info.rune_one);
//        skillData.rune_list.Add((int)_info.rune_two);
//        skillData.rune_list.Add((int)_info.rune_three);
//        skillData.rune_list.Add((int)_info.rune_four);
//        skillData.rune_list.Add((int)_info.rune_five);
//        skillData.rune_list.Add((int)_info.rune_six);
//        skillData.rune_use = (int)_info.rune_use;
    }


    SkillMainLvRef LvRef
    {
        get
        {
            if (skillData == null)
                return null;
            SkillMainLvRef skill = ConfigMng.Instance.GetSkillMainLvRef(SkillID, SkillLv);
            if (skill == null) Debug.LogError("找不到SkillMainLvRef数据,name:" + SkillName + ",SkillID:" + SkillID + ",SkillLv:" + SkillLv);
            return skill;
        }
    }


    #endregion

    #region 访问器

    /// <summary>
    /// 是否满级
    /// </summary>
    public bool isFullLevel
    {
        get
        {
            return skillData.Lv >= SkillRef.skillLevelLimit;
        }

    }


    /// <summary>
    /// 是否有足够的金钱学习该技能
    /// </summary>
    public bool CoinEnough
    {
        get
        {
            if (LvRef == null)
            {
                Debug.LogError("技能等级表找不到相关数据  by黄洪兴");
                return false;
            }
			return (ulong)LvRef.learnCoin <= (GameCenter.mainPlayerMng.MainPlayerInfo.BindCoinCount + GameCenter.mainPlayerMng.MainPlayerInfo.UnBindCoinCount);
        }
    }

    /// <summary>
    /// 是否有足够的资源
    /// </summary>
    public bool ResEnough
    {

        get
        {
            if (LvRef == null)
            {
                Debug.LogError("技能等级表找不到相关数据  by黄洪兴");
                return false;
            }
            return LvRef.learnSp <= GameCenter.mainPlayerMng.MainPlayerInfo.SkillRes;

        }
    }

    /// <summary>
    /// 是否等级足够
    /// </summary>
    public bool LevEnough
    {

        get
        {
            if (LvRef == null)
            {
                Debug.LogError("技能等级表找不到相关数据  by黄洪兴");
                return false;
            }
            return LvRef.learnLv <= GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel;

        }

    }


    /// <summary>
    /// 学习需求等级
    /// </summary>
    public int LearnLv
    {
        get
        {
            if (LvRef == null)
            {
                Debug.LogError("技能等级表找不到相关数据  by黄洪兴");
                return 0;
            }
            return LvRef.learnLv;

        }
    }


    /// <summary>
    /// 学习需求资源
    /// </summary>
    public int LearnSp
    {
        get
        {
            if (LvRef == null)
            {
                Debug.LogError("技能等级表找不到相关数据  by黄洪兴");
                return 0;
            }
            return LvRef.learnSp;

        }
    }

    /// <summary>
    /// 学习需求金钱
    /// </summary>
    public int LearnCoin
    {
        get
        {
            if (LvRef == null)
            {
                Debug.LogError("技能等级表找不到相关数据  by黄洪兴");
                return 0;
            }
            return LvRef.learnCoin;

        }


    }

    /// <summary>
    /// 技能ID
    /// </summary>
    public int SkillID
    {
        get { return skillData.id; }
    }
    /// <summary>
    /// 技能等级
    /// </summary>
    public int SkillLv
    {
        get { return skillData.Lv; }
    }
    /// <summary>
    /// 技能名称
    /// </summary>
    public string SkillName
    {
        get
        {
            return SkillRef.skillName;
        }
    }
    /// <summary>
    /// 技能描述 
    /// </summary>
    public string SkillDes
    {
        get
        {
            if (SkillRef == null)
            {
                return null;
            }
			string[] words = { "[CFB53B]" + (SkillLvData.powerOne / 100).ToString() + "[-]","[CFB53B]"+ SkillLvData.powerTwo.ToString() + "[-]" };
			return UIUtil.Str2Str(SkillRef.skillDes, words);
        }
    }
	SkillLvDataRef skillLvData = null;
	SkillLvDataRef SkillLvData{
		get{
			if(skillLvData == null){
				skillLvData = ConfigMng.Instance.GetSkillLvDataRef(SkillID,SkillLv);
			}
			return skillLvData;
		}
	}
	/// <summary>
	/// 是否学习
	/// </summary>
	public bool isEnable
	{
		get {
			return skillData.isEnable;
		}
		
	}
    /// <summary>
    /// 技能图标 
    /// </summary>
    public string SkillIcon
    {
        get
        {
            return SkillRef.skillIcon;
        }
    }
    /// <summary>
    /// 技能类型 
    /// </summary>
    public SkillMode CurSkillMode
    {
        get
        {
            return SkillRef.skillMode;
        }
    }
	/// <summary>
	/// 技能种类名字
	/// </summary>
	public string CurSkillTypeName{
		get{
			string name =string.Empty;
            switch (SkillType)
            {
                case 1:
                    name = ConfigMng.Instance.GetUItext(46);
                    break;
                case 2:
                    name = ConfigMng.Instance.GetUItext(47);
                    break;
                case 3:
                    name = ConfigMng.Instance.GetUItext(48);
                    break;
                default:
                    break;
            }
			return name;
		}
	}
    /// <summary>
    /// 技能解锁等级
    /// </summary>
    public int UnlockLv
    {
        get
        {
            return SkillRef.unlockLvl;
        }
    }
    /// <summary>
    /// 技能种类
    /// </summary>
    public int SkillType
    {
        get
        {
            return SkillRef.skilltype;
        }
    }



    /// <summary>
    /// 对应职业 
    /// </summary>
    public int SkillRole
    {
        get
        {
            return SkillRef.skillRole;
        }
    }
    /// <summary>
    /// 等级上限 
    /// </summary>
    public int SkillLevelLimit
    {
        get
        {
            return SkillRef.skillLevelLimit;
        }
    }
    /// <summary>
    /// 基础符文 
    /// </summary>
    public int SkillBaseRune
    {
        get
        {
            return SkillRef.baseRune;
        }
    }
    /// <summary>
    /// 高级符文 
    /// </summary>
    public List<int> SkillProRune
    {
        get
        {
            return SkillRef.proRuneList;
        }
    }
    /// <summary>
    /// 从服务端解锁的符文
    /// </summary>
    public List<int> SkillRuneGet
    {
        get
        {
            return skillData.rune_list;
        }
    }
    /// <summary>
    /// 当前使用的符文
    /// </summary>
    public int SkillCurRune
    {
        get
        {
			return SkillRef.baseRune;
        }
    }
    public float CD
    {
        get
        {
			return ConfigMng.Instance.GetSkillLvDataRef(ConfigMng.Instance.GetSkillRuneRef(SkillCurRune).performanceID, skillData.Lv).cd;
        }
    }
    public int mpNeed
    {
        get
        {
			return ConfigMng.Instance.GetSkillLvDataRef(ConfigMng.Instance.GetSkillRuneRef(SkillCurRune).performanceID, skillData.Lv).mp;
        }
    }
    #endregion
    #endregion
}
