//=========================================================
//作者：吴江
//日期：2015/5/9
//用途：用来加载和存放游戏静态配置
//========================================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System;
using System.Text;



/// <summary>
/// 用来加载和存放游戏静态配置 by吴江
/// </summary>
public class ConfigMng
{
    /// <summary>
    /// 资源名后缀（研发版本与发布版本做差异）
    /// </summary>
    protected string extension = ".unity3d";

    /// <summary>
    /// 当前加载进程数量
    /// </summary>
    protected int pendings = 0;
    /// <summary>
    /// 当前加载进程数量
    /// </summary>
    public int Pendings
    {
        get { return pendings; }
    }



    protected static ConfigMng instance;

    public static ConfigMng Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ConfigMng(false);
            }
            return instance;
        }
    }


    public ConfigMng(bool _isDevelopmentPattern)
    {
        //TO DO:如果是开发环境，读哪一种配置
        extension = _isDevelopmentPattern ? ".unity3d" : ".unity3d";
    }


    public void InitSingleTable<T>(string _name, System.Action<T> _callBack) where T : AssetTable
    {
        pendings++;
        AssetMng.instance.LoadAsset<T>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + _name + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                if (_callBack != null)
                {
                    _callBack(x);
                }
            }

        });
    }
	
	#region 加载图片 by 何明军
	/// <summary>
	/// 获取在磁盘中的UI大图
	/// </summary>
	public void GetBigUIIcon(string iconPath,System.Action<Texture2D> OnComplete)
	{
		AssetMng.instance.LoadAsset<Texture>(AssetMng.GetPathWithoutExtension("icon/", AssetPathType.PersistentDataPath) + iconPath + ".texture", (x, y) =>
        {
			try
			{
                if (y == EResult.Success)
                {
					OnComplete(x as Texture2D);
					//调用RemoveBigUIIcon之后,bigUIIcons从的缓存数据已经没了，所以缓存到bigUIIcons无意义 去掉了  by邓成
				}
			}catch(System.Exception e){
				GameSys.LogError(ConfigMng.Instance.GetUItext(211)+iconPath + ConfigMng.Instance.GetUItext(212) + e.ToString());
			}
		});
	}
	/// <summary>
	/// 清除Texture的引用，使用了GetBigUIIcon方法的界面在OnClose()里调用
	/// </summary>
	public void RemoveBigUIIcon(string iconPath){
		AssetMng.instance.UnloadUrl(AssetMng.GetPathWithoutExtension("icon/", AssetPathType.PersistentDataPath) +iconPath +".texture");
	}
	#endregion

    //===================================A=============================




    #region 自动嗑药 by黄洪兴

    protected FDictionary autoItemRefTable = new FDictionary();
    public void InitAutoItemRefTable()
    {
        pendings++;
        autoItemRefTable.Clear();
        AssetMng.instance.LoadAsset<AutoItemRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "autoItem" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    autoItemRefTable[x.infoList[i].level] = x.infoList[i];
                }
            }
        });
    }



    public List<int> GetHpAutoItemByLev(int _lev)
    {
        List<int> list=new List<int>(); 
        foreach (var item in autoItemRefTable.Values)
        {
            AutoItemRef Ref = item as AutoItemRef;
            if (Ref.level <= _lev)
            {
                list.Add(Ref.hpItem);
            }
            //if (_lev>=30&&Ref.level == 99999)
            if (_lev >= 30 && Ref.level == 99999)
            {
                list.Add(Ref.hpItem);
            }
            
        }
        int exchange = 0;
        for (int j = 0, max = list.Count; j < max; j++)
        {
            for (int i = 0; i < max - j - 1; i++)
            {
                if (list[i] > list[i + 1])
                {
                    exchange = list[i];
                    list[i] = list[i + 1];
                    list[i + 1] = exchange;
                }
            }
        }
        //从小到大排列
        return list;
    }
     

    public List<int> GetMpAutoItemByLev(int _lev)
    {
        List<int> list = new List<int>();
        foreach (var item in autoItemRefTable.Values)
        {
            AutoItemRef Ref = item as AutoItemRef;
            if (Ref.level <= _lev)
            {
                list.Add(Ref.mpItem);
            }
        }
        return list;
    }


    public int GetAutoItemPriceByLev(int _lev,int _type)
    {
        int price=0;
        foreach (var item in autoItemRefTable.Values)
        {
            AutoItemRef Ref = item as AutoItemRef;
            if (_lev >= Ref.level)
            {
                if (_type == 1)
                {
                    price = Ref.hpPrice;
                }
                else
                {
                    price = Ref.mpPrice;
                }
            }
        }


        return price;
    }


    public EquipmentInfo GetAutoItemTypeByLev(int _lev, int _type)
    {
        EquipmentInfo info = null;
        foreach (var item in autoItemRefTable.Values)
        {
            AutoItemRef Ref = item as AutoItemRef;
            if (_lev >= Ref.level)
            {
                if (_type == 1)
                {
                    info = new EquipmentInfo(Ref.hpItem,EquipmentBelongTo.PREVIEW);
                }
                else
                {
                    info = new EquipmentInfo(Ref.mpItem, EquipmentBelongTo.PREVIEW);
                }
            }
        }
        return info;
    }



    #endregion

    #region 弹道配置
    protected FDictionary arrowRefTable = new FDictionary();


    public void InitArrowRefTable()
    {
        pendings++;
        equipmentRefTable.Clear();
        AssetMng.instance.LoadAsset<ArrowRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "ArrowConfig" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    arrowRefTable[x.infoList[i].arrowId] = x.infoList[i];
                }
            }
        });
    }


    public ArrowRef GetArrowRef(int _id)
    {
        if (!arrowRefTable.ContainsKey(_id))
        {
            GameSys.LogError("ArrowRefTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return arrowRefTable[_id] as ArrowRef;
    }
    #endregion
	
	#region 人物等级经验配置 何明军
	protected FDictionary attributeRefTable = new FDictionary();


	public void InitAttributeRefTable()
	{
		pendings++;
		attributeRefTable.Clear();
		AssetMng.instance.LoadAsset<AttributeRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "attribute" + extension, (x, y) =>
			{
				pendings--;
				if (y == EResult.Success)
				{
					for (int i = 0; i < x.infoList.Count; i++)
					{
						attributeRefTable[x.infoList[i].id] = x.infoList[i];
					}
				}
			});
	}


	public AttributeRef GetAttributeRef(int _id)
	{
		if (!attributeRefTable.ContainsKey(_id))
		{
            GameSys.LogError("attributeRefTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
			return null;
		}
		return attributeRefTable[_id] as AttributeRef;
	}
	//多少级文本
	public string GetLevelDes(int lev){
		AttributeRef attData = GetAttributeRef(lev > 0 ? lev : 1);
		if(attData.reborn > 0){
			return GetUItext(12,new string[2]{attData.reborn.ToString(),attData.display_level.ToString()});
		}else{
			return GetUItext(13,new string[1]{attData.display_level.ToString()});
		}
		//return "";
	}
	//多少级开启文本
	public string GetLevelOpenDes(int lev){
		AttributeRef attData = GetAttributeRef(lev > 0 ? lev : 1);
		if(attData.reborn > 0){
			return GetUItext(10,new string[2]{attData.reborn.ToString(),attData.display_level.ToString()});
		}else{
			return GetUItext(11,new string[1]{attData.display_level.ToString()});
		}
		//return "";
	}
	#endregion
	
	#region 人物等级经验配置 何明军
	protected FDictionary activityListRefTable = new FDictionary();

	public void InitActivityListRefTable()
	{
		pendings++;
		activityListRefTable.Clear();
		AssetMng.instance.LoadAsset<ActivityListRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "activityList" + extension, (x, y) =>
			{
				pendings--;
				if (y == EResult.Success)
				{
					for (int i = 0; i < x.infoList.Count; i++)
					{
						activityListRefTable[x.infoList[i].id] = x.infoList[i];
					}
				}
			});
	}
	public FDictionary GetActivityList()
	{
		return activityListRefTable;
	}
    public ActivityListRef GetActivityListRef(int _id)
    {
        if (!activityListRefTable.ContainsKey(_id))
        {
            GameSys.LogError(ConfigMng.Instance.GetUItext(201) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return activityListRefTable[_id] as ActivityListRef;
    }



	protected FDictionary activityButtonRefTable = new FDictionary();
	public void InitActivityButtonRefTable()
	{
		pendings++;
		activityButtonRefTable.Clear();
		AssetMng.instance.LoadAsset<ActivityButtonRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "activityButton" + extension, (x, y) =>
			{
				pendings--;
				if (y == EResult.Success)
				{
					for (int i = 0; i < x.infoList.Count; i++)
					{
						activityButtonRefTable[x.infoList[i].id] = x.infoList[i];
					}
				}
			});
	}
	public ActivityButtonRef GetActivityButtonRef(int _id)
	{
		if (!activityButtonRefTable.ContainsKey(_id))
		{
            GameSys.LogError(ConfigMng.Instance.GetUItext(201) + _id + ConfigMng.Instance.GetUItext(202));
			return null;
		}
		return activityButtonRefTable[_id] as ActivityButtonRef;
	}
	#endregion

    #region 人物属性配置 by邓成
    protected FDictionary attributeTypeRefTable = new FDictionary();
    public void InitAttributeTypeRefTable()
    {
        pendings++;
        attributeTypeRefTable.Clear();
        AssetMng.instance.LoadAsset<AttributeTypeRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "CharPropertyConfig" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    attributeTypeRefTable[(ActorPropertyTag)x.infoList[i].id] = x.infoList[i];
                }
            }
        });
    }

    public AttributeTypeRef GetAttributeTypeRef(ActorPropertyTag _tag)
    {
        if (!attributeTypeRefTable.ContainsKey(_tag))
        {
            GameSys.LogError(ConfigMng.Instance.GetUItext(201) + _tag + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return attributeTypeRefTable[_tag] as AttributeTypeRef;
    }

    public string GetAttributeTypeName(ActorPropertyTag _tag)
    {
        if (!attributeTypeRefTable.ContainsKey(_tag))
        {
            return "UnKnow Property : " + _tag.ToString();
        }
        return (attributeTypeRefTable[_tag] as AttributeTypeRef).stats;
    }
    #endregion
    #region 每日活跃度 by邓成
	protected FDictionary livelyRefTable = new FDictionary();
    /// <summary>
    /// 初始化数据
    /// </summary>
	public void InitLivelyRefTable()
    {
        pendings++;
		livelyRefTable.Clear();
		AssetMng.instance.LoadAsset<LivelyRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "livelyConfig" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
					livelyRefTable[x.infoList[i].id] = x.infoList[i];
                }
            }

        });
    }

	public FDictionary GetLivelyRefTable()
    {
		return livelyRefTable;
    }

	public LivelyRef GetlivelyRefByID(int _id)
    {
		if (!livelyRefTable.ContainsKey(_id))
        {
            GameSys.LogError(ConfigMng.Instance.GetUItext(201) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
		return livelyRefTable[_id] as LivelyRef;
    }

    protected FDictionary livelyRewardRefTable = new FDictionary();
    /// <summary>
    /// 初始化数据
    /// </summary>
    public void InitLivelyRewardRefTable()
    {
        pendings++;
        livelyRewardRefTable.Clear();
        AssetMng.instance.LoadAsset<LivelyRewardRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "livelyrewardConfig" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    livelyRewardRefTable[x.infoList[i].id] = x.infoList[i];
                }
            }

        });
    }

    public FDictionary GetLivelyRewardRefTable()
    {
        return livelyRewardRefTable;
    }

    public LivelyRewardRef GetLivelyRewardRef(int _livelyID)
    { 
        if(livelyRewardRefTable.ContainsKey(_livelyID))
            return livelyRewardRefTable[_livelyID] as LivelyRewardRef;
        return null;
    }
    #endregion

	#region 镇魔塔副本 by邓成
	protected FDictionary towerRefTable = new FDictionary();
	/// <summary>
	/// 初始化数据
	/// </summary>
	public void InitTowerRefTable()
	{
		pendings++;
		towerRefTable.Clear();
		AssetMng.instance.LoadAsset<TowerRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "Town" + extension, (x, y) =>
			{
				pendings--;
				if (y == EResult.Success)
				{
					for (int i = 0; i < x.infoList.Count; i++)
					{
						towerRefTable[x.infoList[i].id] = x.infoList[i];
					}
				}
			});
	}

	public TowerRef GetTowerRefByLayer(int _layer,int _sceneID)
	{
		foreach(var item in towerRefTable.Values)
		{
			TowerRef towerRef = item as TowerRef;
			if(towerRef != null && towerRef.lel == _layer && towerRef.Scene == _sceneID)
				return towerRef;
		}
		Debug.LogError("找不到数据,layer:"+_layer+",sceneID:"+_sceneID);
		return null;
	}
	#endregion

    #region 法宝注灵配置表 by鲁家旗
    protected FDictionary addSoulRefTable = new FDictionary();

    public void InitAddSoulRefTable()
    {
        pendings++;
        addSoulRefTable.Clear();
        AssetMng.instance.LoadAsset<AddSoulRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "Zhuling" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    addSoulRefTable[x.infoList[i].id] = x.infoList[i];
                }
            }
        });
    }

    public AddSoulRef GetAddSoulRef(int _id)
    {
        if (!addSoulRefTable.ContainsKey(_id))
        {
            GameSys.LogError(ConfigMng.Instance.GetUItext(201) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return addSoulRefTable[_id] as AddSoulRef;
    }

    public AddSoulRef GetAddSoulRef(int _type,int _stage,int _star)
    {
        foreach (AddSoulRef data in addSoulRefTable.Values)
        {
            if (data.relationID == _type && data.quality == _stage && data.star == _star)
            {
                return data;
            }
        }
        return null;
    }
    #endregion

    #region 成就分类静态配置 by鲁家旗
    protected FDictionary achieveTypeRefTable = new FDictionary();

    public void InitAchieveTypeRefTable()
    {
        pendings++;
        achieveTypeRefTable.Clear();
        AssetMng.instance.LoadAsset<AchieveTypeRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "achieveType" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    achieveTypeRefTable[x.infoList[i].type] = x.infoList[i];
                }
            }
        });
    }
    public AchieveTypeRef GetAchieveTypeRef(int _id)
    {
        if (!achieveTypeRefTable.ContainsKey(_id))
        {
            GameSys.LogError(ConfigMng.Instance.GetUItext(201) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return achieveTypeRefTable[_id] as AchieveTypeRef;
    }
    public FDictionary GetAchieveTypeRefTable()
    {
         return achieveTypeRefTable;
    }
    public int GetAchieveType(int _id)
    {
        foreach (AchieveTypeRef data in achieveTypeRefTable.Values)
        {
            if (_id >= data.numId[0] && _id <= data.numId[data.numId.Count - 1])
            {
                return data.type;
            }
        }
        return 1;
    }
    #endregion

    #region 成就静态配置 by鲁家旗
    protected FDictionary achievementRefTable = new FDictionary();
    public void InitAchievementRefTable()
    {
        pendings++;
        achievementRefTable.Clear();
        AssetMng.instance.LoadAsset<AchievementRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "achievement" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    achievementRefTable[x.infoList[i].id] = x.infoList[i];
                }
            }
        });
    }
    public AchievementRef GetAchievementRef(int _id)
    {
        if (!achievementRefTable.ContainsKey(_id))
        {
            GameSys.LogError(ConfigMng.Instance.GetUItext(201) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return achievementRefTable[_id] as AchievementRef;
    }
    public FDictionary GetAchievementRefTable()
    {
        return achievementRefTable;
    }
    #endregion

    #region 竞技场排名静态数据

    protected FDictionary arenaRankRefTable = new FDictionary();
	public void InitArenaRankRefTable()
	{
		pendings++;
		arenaRankRefTable.Clear();
		AssetMng.instance.LoadAsset<ArenaRankRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "rankingReward" + extension, (x, y) =>
			{
				pendings--;
				if (y == EResult.Success)
				{
					for (int i = 0; i < x.infoList.Count; i++)
					{
						arenaRankRefTable[x.infoList[i].ranking] = x.infoList[i];
					}
				}
			});
	}

	public ArenaRankRef GetArenaRankRef(int _id)
	{

		if (arenaRankRefTable.ContainsKey(_id))
		{
			return arenaRankRefTable[_id] as ArenaRankRef;
		}
		else 
		{
            GameSys.LogError(ConfigMng.Instance.GetUItext(201) + _id + ConfigMng.Instance.GetUItext(202));
			return null;
		}

	}
	#endregion

    #region 区域型buff静态数据 by邓成

    protected FDictionary areaBuffRefTable = new FDictionary();
    public void InitAreaBuffRefTable()
    {
        pendings++;
        areaBuffRefTable.Clear();
        AssetMng.instance.LoadAsset<AreaBuffRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "areaBuff" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    areaBuffRefTable[x.infoList[i].id] = x.infoList[i];
                }
            }
        });
    }
    /// <summary>
    /// 获取场景中的所有区域型buff配置
    /// </summary>
    /// <param name="_sceneID"></param>
    /// <returns></returns>
    public List<AreaBuffRef> GetAreaBuffRefBySceneID(int _sceneID)
    {
        List<AreaBuffRef> areaBuffList = new List<AreaBuffRef>();
        foreach (var item in areaBuffRefTable.Values)
        {
            AreaBuffRef areaBuffRef = item as AreaBuffRef;
            if (areaBuffRef != null && areaBuffRef.sceneID == _sceneID)
                areaBuffList.Add(areaBuffRef);
        }
        return areaBuffList;
    }
    #endregion
    //===================================B=============================

    #region 火焰山战场等级评分配置 by 朱素云

    protected FDictionary battleFieldRefTable = new FDictionary();
    public void InitBattleFieldRefTable() 
    {
        pendings++;
        battleFieldRefTable.Clear();
        AssetMng.instance.LoadAsset<BattleFieldRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "Battlefield" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    battleFieldRefTable[x.infoList[i].id] = x.infoList[i]; 
                }
            }
        });
    }

    /// <summary>
    /// 根据积分查找BattleFieldRef
    /// </summary>
    /// <param name="_id"></param>
    /// <returns></returns>
    public BattleFieldRef GetBattleFieldRefByScore(int _score)
    {
        int id = 0;
        foreach (BattleFieldRef battle in battleFieldRefTable.Values)
        {
            if (_score <= battle.rewardConditionList[1])
            {
                ++id;
            }
            else
                break; 
        }
        //Debug.Log("  根据积分查找BattleFieldRef     _score : " + _score + "   , id : " + id);
        if (!battleFieldRefTable.ContainsKey(id))
        {
            GameSys.LogError(ConfigMng.Instance.GetUItext(201) + id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return battleFieldRefTable[id] as BattleFieldRef;
    }

    public FDictionary GetBattleFieldRefTable() 
    {
        return battleFieldRefTable;
    }
    #endregion

    #region 火焰山战场结算奖励配置 by 朱素云

    protected FDictionary battleSettlementBonusRefTable = new FDictionary();
    public void InitBattleSettlementBonusRefTable()
    {
        pendings++;
        battleSettlementBonusRefTable.Clear();
        AssetMng.instance.LoadAsset<BattleSettlementBonusRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "SettlementBonus" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    battleSettlementBonusRefTable[x.infoList[i].id] = x.infoList[i];
                }
            }
        });
    }

    public BattleSettlementBonusRef GetBattleSettlementBonusRef(int id) 
    {
        if (!battleSettlementBonusRefTable.ContainsKey(id))
        {
            GameSys.LogError("battleSettlementBonusRefTable 中找不到索引为" + id + "的配置!");
            return null;
        }
        return battleSettlementBonusRefTable[id] as BattleSettlementBonusRef;
    }
    #endregion
    //===================================C=============================
	
	#region 副本入口信息配置 by何明军

	protected FDictionary copyGroupRefTable = new FDictionary();
	public void InitCopyGroupRefTable()
	{
		pendings++;
		copyGroupRefTable.Clear();
		AssetMng.instance.LoadAsset<CopyGroupRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "CopyGroup" + extension, (x, y) =>
			{
				pendings--;
				if (y == EResult.Success)
				{
					for (int i = 0; i < x.infoList.Count; i++)
					{
						copyGroupRefTable[x.infoList[i].id] = x.infoList[i];
					}
				}
			});
	}
	
	public FDictionary CopyGroupRefTable(){
		return copyGroupRefTable;
	}
	
	public List<CopyGroupRef> GetCopyGroupRefTable(int type){
		List<CopyGroupRef> list = new List<CopyGroupRef>();
		foreach(CopyGroupRef refData in copyGroupRefTable.Values){
			if(refData.sort ==type){
				list.Add(refData);
			}
		}
		return list;
	}
	public CopyGroupRef GetCopyGroupRef(int _id)
	{
		if (!copyGroupRefTable.ContainsKey(_id))
		{
            GameSys.LogError(ConfigMng.Instance.GetUItext(201) + _id + ConfigMng.Instance.GetUItext(202));
			return null;
		}
		return copyGroupRefTable[_id] as CopyGroupRef;
	}
	
	protected FDictionary copyRefTable = new FDictionary();
	public void InitCopyRefTable()
	{
		pendings++;
		copyRefTable.Clear();
		AssetMng.instance.LoadAsset<CopyRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "Copy" + extension, (x, y) =>
			{
				pendings--;
				if (y == EResult.Success)
				{
					for (int i = 0; i < x.infoList.Count; i++)
					{
						copyRefTable[x.infoList[i].id] = x.infoList[i];
					}
				}
			});
	}
	public CopyRef GetCopyRef(int _id)
	{
		if (!copyRefTable.ContainsKey(_id))
		{
			GameSys.LogError(ConfigMng.Instance.GetUItext(203)+ _id + ConfigMng.Instance.GetUItext(202));
			return null;
		}
		return copyRefTable[_id] as CopyRef;
	}

	#endregion
	
	#region 无尽挑战信息配置 by 何明军

	protected FDictionary chapterRefTable = new FDictionary();
	public void InitChapterRefTable()
	{
		pendings++;
		chapterRefTable.Clear();
		AssetMng.instance.LoadAsset<ChapterRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "Chapter" + extension, (x, y) =>
			{
				pendings--;
				if (y == EResult.Success)
				{
					for (int i = 0; i < x.infoList.Count; i++)
					{
						chapterRefTable[x.infoList[i].id] = x.infoList[i];
					}
				}
			});
	}
	
	public FDictionary GetChapterRefTable()
	{
		return chapterRefTable;
	}
	public ChapterRef GetChapterRefData(int _id)
	{
		if (!chapterRefTable.ContainsKey(_id))
		{
            GameSys.LogError("chapterRefTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
			return null;
		}
		return chapterRefTable[_id] as ChapterRef;
	}
	public List<ChapterReward> GetChapterReward(int _id)
    {
        if (!chapterRefTable.ContainsKey(_id))
        {
            GameSys.LogError("chapterRefTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return (chapterRefTable[_id] as ChapterRef).rewardData;
    }
    protected FDictionary checkPointRefTable = new FDictionary();
	protected FDictionary frontGateCheckPoint = new FDictionary();
	public void InitCheckPointRefTable()
	{
		pendings++;
		checkPointRefTable.Clear();
		AssetMng.instance.LoadAsset<CheckPointRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "Checkpoint" + extension, (x, y) =>
			{
				pendings--;
				if (y == EResult.Success)
				{
					for (int i = 0; i < x.infoList.Count; i++)
					{
						checkPointRefTable[x.infoList[i].id] = x.infoList[i];
						frontGateCheckPoint[x.infoList[i].frontGate] = x.infoList[i];
					}
				}
			});
	}
	/// <summary>
	/// 获得已当前关卡为前置关卡的数据
	/// </summary>
	public CheckPointRef GetFrontGateCheckPoint(int _id)
	{
		if (!frontGateCheckPoint.ContainsKey(_id))
		{
            GameSys.LogError("frontGateCheckPoint " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
			return null;
		}
		return frontGateCheckPoint[_id] as CheckPointRef;
	}
	public CheckPointRef GetCheckPointRef(int _id)
	{
		if (!checkPointRefTable.ContainsKey(_id))
		{
            GameSys.LogError("checkPointRefTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
			return null;
		}
		return checkPointRefTable[_id] as CheckPointRef;
	}
	
	protected FDictionary lineRefTable = new FDictionary();
	public void InitLineRefTable()
	{
		pendings++;
		lineRefTable.Clear();
		AssetMng.instance.LoadAsset<LineRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "Line" + extension, (x, y) =>
			{
				pendings--;
				if (y == EResult.Success)
				{
					for (int i = 0; i < x.infoList.Count; i++)
					{
						lineRefTable[x.infoList[i].chapter] = x.infoList[i];
					}
				}
			});
	}
	public LineRef GetLineRef(int _id)
	{
		if (!lineRefTable.ContainsKey(_id))
		{
            GameSys.LogError("lineRefTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
			return null;
		}
		return lineRefTable[_id] as LineRef;
	}

	#endregion

	#region 铸魂 by黄洪兴

	protected FDictionary castSoulRefTable = new FDictionary();
	protected FDictionary castSoulTimeRefTable = new FDictionary();
	public void InitCastSoulRefTable()
	{
		pendings++;
		castSoulRefTable.Clear();
		AssetMng.instance.LoadAsset<CastSoulRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "forgeSoul" + extension, (x, y) =>
			{
				pendings--;
				if (y == EResult.Success)
				{
					for (int i = 0; i < x.infoList.Count; i++)
					{
						castSoulRefTable[x.infoList[i].id] = x.infoList[i];
					}
				}
			});
	}
	public void InitCastSoulTimeRefTable()
	{
		pendings++;
		castSoulTimeRefTable.Clear();
		AssetMng.instance.LoadAsset<CastSoulTimeRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "forgeSoulTime" + extension, (x, y) =>
			{
				pendings--;
				if (y == EResult.Success)
				{
					for (int i = 0; i < x.infoList.Count; i++)
					{
						castSoulTimeRefTable[x.infoList[i].time] = x.infoList[i];
					}
				}
			});
	}



	public CastSoulRef GetCastSoulRef(int _id)
	{
		if (!castSoulRefTable.ContainsKey(_id))
		{
            GameSys.LogError("castSoulRefTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
			return null;
		}
		return castSoulRefTable[_id] as CastSoulRef;
	}

	public CastSoulTimeRef GetCastSoulTimeRef(int _id)
	{
		if (!castSoulTimeRefTable.ContainsKey(_id))
		{
            GameSys.LogError("castSoulTimeRefTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
			return null;
		}
		return castSoulTimeRefTable[_id] as CastSoulTimeRef;
	}


	public FDictionary GetCastSoulRefTable()
	{
		return castSoulRefTable;
	}
	public FDictionary GetCastSoulTimeRefTable()
	{
		return castSoulTimeRefTable;
	}

	#endregion

    #region 铸魂奖励配置 by 朱素云

    protected FDictionary castsoulRewardRefTable = new FDictionary();
    public void InitCastsoulRewardRefTable()
    {
        pendings++;
        castsoulRewardRefTable.Clear();
        AssetMng.instance.LoadAsset<CastsoulRewardRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "SoulReward" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    castsoulRewardRefTable[x.infoList[i].id] = x.infoList[i]; 
                }
            }
        });
    }

    public FDictionary GetCastsoulRewardRefTable()
    {
        return castsoulRewardRefTable;
    }
    public CastsoulRewardRef GetcastsoulRewardRef(int _id)
    {
        if (!castsoulRewardRefTable.ContainsKey(_id))
        {
            GameSys.LogError("castsoulRewardRefTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return castsoulRewardRefTable[_id] as CastsoulRewardRef;
    }
    #endregion

    //===================================D=============================
    
    #region 七日奖励 by鲁家旗
    protected FDictionary sevenDayRefTable = new FDictionary();
    public void InitSevenDayRefTable()
    {
        pendings++;
        sevenDayRefTable.Clear();
        AssetMng.instance.LoadAsset<SevenDaysRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "sevenDays" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    sevenDayRefTable[x.infoList[i].prof] = x.infoList[i];
                }
            }
        });
    }
    public FDictionary GetSevendayRefTable()
    {
        return sevenDayRefTable;
    }
    /// <summary>
    /// 通过职业和天数获取本地数据
    /// </summary>
    /// <param name="_prof"></param>
    /// <param name="_day"></param>
    /// <returns></returns>
    public SevenDaysRef GetSevenDayRef(int _prof, int _day)
    {
        if (!sevenDayRefTable.ContainsKey(_prof))
        {
            GameSys.LogError("dailyRewardRefTable " + ConfigMng.Instance.GetUItext(204) + _prof +ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return (sevenDayRefTable[_prof] as SevenDaysProfRef).GetLevelRewardRef(_day);
    }
    public List<SevenDaysRef> GetSevendayRefTable(int _prof)
    {
        List<SevenDaysRef> list = new List<SevenDaysRef>((sevenDayRefTable[_prof] as SevenDaysProfRef).stepList);
        return list;
    }
    #endregion

    #region 二冲奖励 by李邵南
    protected FDictionary twoChargeRefTable = new FDictionary();
    public void InitTwoChargeRefTable()
    {
        pendings++;
        twoChargeRefTable.Clear();
        AssetMng.instance.LoadAsset<TwoChargeRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "TwoCharge" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    twoChargeRefTable[x.infoList[i].id] = x.infoList[i];
                }
            }
        });
    }
    public FDictionary GetTwoChargeRefTable()
    {
        return twoChargeRefTable;
    }
    public TwoChargeRef GetTwoChargeRef(int _id)
    {
        if (!twoChargeRefTable.ContainsKey(_id))
        {
            GameSys.LogError("twoChargeRefTable" + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return twoChargeRefTable[_id] as TwoChargeRef;
    }
    #endregion

    #region 通用说明弹窗 by 鲁家旗
    protected FDictionary descriptionRefTable = new FDictionary();
    public void InitDescriptionRefTable()
    {
        pendings++;
        descriptionRefTable.Clear();
        AssetMng.instance.LoadAsset<DescriptionRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "DescriptionConfig" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    descriptionRefTable[x.infoList[i].id] = x.infoList[i];
                }
            }
        });
    }
    public DescriptionRef GetDescriptionRef(int _id)
    {
        if (!descriptionRefTable.ContainsKey(_id))
        {
            GameSys.LogError("descriptionRefTable" + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return descriptionRefTable[_id] as DescriptionRef;
    }
    #endregion

    #region 下载奖励静态配置 by zsy
    protected FDictionary downloadBonusRefTable = new FDictionary();
    public void InitDownloadBonusRefTable()
    {
        pendings++;
        downloadBonusRefTable.Clear();
        AssetMng.instance.LoadAsset<DownloadBonusRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "downloadBonus" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    downloadBonusRefTable[x.infoList[i].id] = x.infoList[i];
                }
            }
        });
    }
    public DownloadBonusRef GetDownloadBonusRef(int _id)
    {
        if (!downloadBonusRefTable.ContainsKey(_id))
        {
            GameSys.LogError("downloadBonusRefTable" + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return downloadBonusRefTable[_id] as DownloadBonusRef;
    }
    #endregion

    #region 对话表静态配置 by zsy
    protected FDictionary dialogueRefTable = new FDictionary();
    public void InitDialogueRefTable()
    {
        pendings++;
        dialogueRefTable.Clear();
        AssetMng.instance.LoadAsset<DialogueRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "dialogue" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    if (!dialogueRefTable.ContainsKey(x.infoList[i].id))
                    {
                        List<DialogueRef> list = new List<DialogueRef>();
                        list.Add(x.infoList[i]);
                        dialogueRefTable[x.infoList[i].id] = list;
                    }
                    else
                    {
                        List<DialogueRef> oldList = dialogueRefTable[x.infoList[i].id] as List<DialogueRef>;
                        if (oldList != null)
                        {
                            oldList.Add(x.infoList[i]);
                            dialogueRefTable[x.infoList[i].id] = oldList;
                        }
                        else
                        {
                            GameSys.LogError(ConfigMng.Instance.GetUItext(205));
                        }

                    } 
                }
            }
        });
    }
    public List<DialogueRef> GetDialogueRef(int _id)
    { 
        if (!dialogueRefTable.ContainsKey(_id))
        {
            GameSys.LogError("dialogueRefTable" + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return dialogueRefTable[_id] as List<DialogueRef>;
    }
 
    #endregion


    #region 登陆红利静态配置 by zsy
    protected FDictionary dividendRefTable = new FDictionary();
    public void InitDividendRefTable()
    {
        pendings++;
        dividendRefTable.Clear();
        AssetMng.instance.LoadAsset<DividendRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "dividend" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    dividendRefTable[x.infoList[i].ID] = x.infoList[i];
                }
            }
        });
    }
    public DividendRef GetDividendRef(int _id)
    {
        if (!dividendRefTable.ContainsKey(_id))
        {
            GameSys.LogError("dividendRefTable" + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return dividendRefTable[_id] as DividendRef;
    }
    public FDictionary GetDividendRefTable()
    {
        return dividendRefTable;
    }
    #endregion

    #region 仙盟捐献静态配置 by zsy
    protected FDictionary guildDonateRefTable = new FDictionary();
    public void InitGuildDonateRefTable()
    {
        pendings++;
        guildDonateRefTable.Clear();
        AssetMng.instance.LoadAsset<GuildDonateRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "GuildllivelycostConfig" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    guildDonateRefTable[x.infoList[i].id] = x.infoList[i]; 
                }
            }
        });
    }
    public GuildDonateRef GetGuildDonateRef(int _id)
    {
        if (!guildDonateRefTable.ContainsKey(_id))
        {
            GameSys.LogError("guildDonateRefTable中找不到索引为" + _id + "的配置");
            return null;
        }
        return guildDonateRefTable[_id] as GuildDonateRef;
    }
    public FDictionary GetGuildDonateRefTable()
    {
        return guildDonateRefTable;
    }
    #endregion

    //===================================E============================

    #region 物品配置
    protected FDictionary equipmentRefTable = new FDictionary();

    protected FDictionary resIDRefTable = new FDictionary();

    public void InitEquipmentRefTable()
    {
        pendings++;
        equipmentRefTable.Clear();
        resIDRefTable.Clear();
        AssetMng.instance.LoadAsset<EquipmentRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "ItemConfig" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    equipmentRefTable[x.infoList[i].id] = x.infoList[i];
                    if (!resIDRefTable.ContainsKey(x.infoList[i].serverResId) && x.infoList[i].serverResId!=0)
                        resIDRefTable[x.infoList[i].serverResId] = x.infoList[i];
                }
            }
        });
    }


    public EquipmentRef GetEquipmentRef(int _id)
    {
        if (!equipmentRefTable.ContainsKey(_id))
        {
			if(_id != 0)//输出太多卡
                GameSys.LogError("equipmentRefTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return equipmentRefTable[_id] as EquipmentRef;
    }

    public EquipmentRef GetEquipByResIDRef(int _resID)
    {
        if (!resIDRefTable.ContainsKey(_resID))
        {
            return null;
        }
        return resIDRefTable[_resID] as EquipmentRef;
    }
    #endregion

    #region 每日奖励 by zsy
    protected FDictionary everydayRewardRefTable = new FDictionary();
    public void InitEverydayRewardRefTable()
    {
        pendings++;
        everydayRewardRefTable.Clear();
        AssetMng.instance.LoadAsset<EverydayRewardRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "everydayReward" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    everydayRewardRefTable[x.infoList[i].id] = x.infoList[i];
                }  
            }
        });
    }


    public EverydayRewardRef GetEverydayRewardRef(int _id) 
    {
        if (!everydayRewardRefTable.ContainsKey(_id))
        {
            GameSys.LogError("everydayRewardRef " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
            return null; 
        }
		return everydayRewardRefTable[_id] as EverydayRewardRef;
    }

    public FDictionary GetEverydayRewardTable()
    {
        return everydayRewardRefTable;
    }
    #endregion

    //===================================F=============================
    #region  特殊飞行配置 by 黄洪兴
    protected List<FlyExRef> flyExRefTable = new List<FlyExRef>();
    public void InitFlyExRefTableRefTable()
    {
        pendings++;
        flyExRefTable.Clear();
        AssetMng.instance.LoadAsset<FlyExRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "flyEx" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    flyExRefTable.Add(x.infoList[i]);
                }
            }

        });
    }
    public List<FlyExRef> GetAllFlyExRef()
    {
        return flyExRefTable;
    }
    #endregion


    #region 传送点静态配置
    protected FDictionary flyPointRefTable = new FDictionary();
    protected FDictionary sceneFlyPointRefTable = new FDictionary();


    public void InitFlyPointRefTable()
    {
        pendings++;
        flyPointRefTable.Clear();
        sceneFlyPointRefTable.Clear();
        AssetMng.instance.LoadAsset<FlyPointRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "flyPointConfig" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    flyPointRefTable[x.infoList[i].id] = x.infoList[i];
                }
                foreach (FlyPointRef item in flyPointRefTable.Values)
                {
                    List<FlyPointRef> list = null;
                    if (!sceneFlyPointRefTable.ContainsKey(item.scene))
                    {
                        list = new List<FlyPointRef>();
                        sceneFlyPointRefTable[item.scene] = new List<FlyPointRef>();
                    }
                    list = sceneFlyPointRefTable[item.scene] as List<FlyPointRef>;
                    list.Add(item);
                }
            }

        });
    }


    public FlyPointRef GetFlyPointRef(int _id)
    {
        if (!flyPointRefTable.ContainsKey(_id))
        {
            GameSys.LogError("flyPointRefTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return flyPointRefTable[_id] as FlyPointRef;
    }

    /// <summary>
    /// 根据当前所在场景和目标场景获得传送信息
    /// </summary>
    /// <param name="_id"></param>
    /// <returns></returns>
    public FlyPointRef GetFlyPointRef(int _id, int _target)
    {
        List<FlyPointRef> curScene = new List<FlyPointRef>();

        foreach (FlyPointRef item in flyPointRefTable.Values)
        {
            if (item.scene == _id)
            {
                curScene.Add(item);
            }
        }
        for (int i = 0, max = curScene.Count; i < max; i++)
        {
            if (curScene[i].targetScene == _target)
            {
                return curScene[i];
            }
        }
       //Debug.Log("flyPointRefTable 中找不到索引所在场景为" + _id + ", 目标场景为 ： " + _target + "  的配置");
        return null;
    }


    public List<FlyPointRef> GetFlyPointRefByScene(int _sceneID)
    {
        if (sceneFlyPointRefTable.ContainsKey(_sceneID))
        {
            return sceneFlyPointRefTable[_sceneID] as List<FlyPointRef>;
        }
        return new List<FlyPointRef>();
    }
    #endregion

    #region 首冲大礼 by 鲁家旗
    protected FDictionary firstChargeRefTable = new FDictionary();
    public void InitFirstChargeRefTable()
    {
        pendings++;
        firstChargeRefTable.Clear();
        AssetMng.instance.LoadAsset<FirstChargeRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "firstCharge" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    firstChargeRefTable[x.infoList[i].prof] = x.infoList[i];
                }
            }
        });
    }
    public FirstChargeRef GetFirstChargeRefTable(int _prof)
    {
        if (!firstChargeRefTable.ContainsKey(_prof))
        {
            GameSys.LogError("firstChargeRefTable " + ConfigMng.Instance.GetUItext(204) + _prof + ConfigMng.Instance.GetUItext(202) );
            return null;
        }
        else
        {
            return firstChargeRefTable[_prof] as FirstChargeRef;
        }
    }
    /// <summary>
    /// 通过职业获得所对应的职业奖励
    /// </summary>
    public List<int> GetRewardId(int _prof)
    {
        if (!firstChargeRefTable.ContainsKey(_prof))
        {
            GameSys.LogError("firstChargeRefTable " + ConfigMng.Instance.GetUItext(204) + _prof + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        else
        {
            List<int> list = new List<int>();
            FirstChargeRef data = (firstChargeRefTable[_prof] as FirstChargeRef);
            for (int i = 0; i < data.reward.Count; i++)
            {
                list.Add(data.reward[i].eid);
            }
            return list;
        }
    }
    /// <summary>
    /// 通过职业获得所对应的奖励数量
    /// </summary>
    public List<int> GetRewardNum(int _prof)
    {
        if (!firstChargeRefTable.ContainsKey(_prof))
        {
            GameSys.LogError("firstChargeRefTable " + ConfigMng.Instance.GetUItext(204) + _prof + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        else
        {
            List<int> list = new List<int>();
            FirstChargeRef data = (firstChargeRefTable[_prof] as FirstChargeRef);
            for (int i = 0; i < data.reward.Count; i++)
            {
                list.Add(data.reward[i].count);
            }
            return list;
        }
    }
    #endregion
    //===================================G=============================
    #region 礼包领取 by鲁家旗
    public FDictionary cdKeyRefTable = new FDictionary();
    public void InitCdKeyRefTable()
    {
        pendings++;
        cdKeyRefTable.Clear();
        AssetMng.instance.LoadAsset<CDKeyRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "cdkey" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    cdKeyRefTable[x.infoList[i].id] = x.infoList[i];
                }
            }
        });
    }
    public CDKeyRef GetCdKeyRefTable(int _id)
    {
        if (!cdKeyRefTable.ContainsKey(_id))
        {
            GameSys.LogError("cdKeyRefTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return cdKeyRefTable[_id] as CDKeyRef;
    }
    public FDictionary GetCdKeyRefTable()
    {
        return cdKeyRefTable;
    }
    #endregion

    #region 时装 by黄洪兴

    protected FDictionary fashionRefTable = new FDictionary();
	public void InitFashionRefTable()
	{
		pendings++;
		fashionRefTable.Clear();
		AssetMng.instance.LoadAsset<FashionRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "showConfig" + extension, (x, y) =>
			{
				pendings--;
				if (y == EResult.Success)
				{
					for (int i = 0; i < x.infoList.Count; i++)
					{
						fashionRefTable[x.infoList[i].id] = x.infoList[i];
					}
				}
			});
	}

	/// <summary>
	///通过id获得本地数据
	/// </summary>
	public FashionRef GetFashionRef(int _id)
	{
		if (!fashionRefTable.ContainsKey(_id))
		{
            GameSys.LogError("fashionRefTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
			return null;
		}
		return fashionRefTable[_id] as FashionRef;
	}
	public List<FashionRef> FashionList()
	{
		List<FashionRef> list = new List<FashionRef>();
		foreach (var item in fashionRefTable.Values)
		{
			list.Add(item as FashionRef);
		}
		return list;
	}


	/// <summary>
	///获得静态表
	/// </summary>
	public FDictionary GetFashionRefTable()
	{

		return fashionRefTable;
	}

	#endregion

    #region 仙盟活跃度 by邓成
    protected FDictionary guildLivelyRefTable = new FDictionary();
    /// <summary>
    /// 初始化数据
    /// </summary>
    public void InitGuildLivelyRefTable()
    {
        pendings++;
        guildLivelyRefTable.Clear();
        AssetMng.instance.LoadAsset<GuildLivelyRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "guildlivelyconfig" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    guildLivelyRefTable[x.infoList[i].id] = x.infoList[i];
                }
            }

        });
    }

    public FDictionary GetGuildLivelyRefTable()
    {
        return guildLivelyRefTable;
    }

    public GuildLivelyRef GetGuildlivelyRefByID(int _id)
    {
        if (!guildLivelyRefTable.ContainsKey(_id))
        {
            GameSys.LogError("guildLivelyRefTable 中找不到索引为" + _id + "的配置!");
            return null;
        }
        return guildLivelyRefTable[_id] as GuildLivelyRef;
    }

    protected FDictionary guildLivelyRewardRefTable = new FDictionary();
    /// <summary>
    /// 初始化数据
    /// </summary>
    public void InitGuildLivelyRewardRefTable()
    {
        pendings++;
        guildLivelyRewardRefTable.Clear();
        AssetMng.instance.LoadAsset<GuildLivelyRewardRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "guildllivelyrewardconfig" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    guildLivelyRewardRefTable[x.infoList[i].id] = x.infoList[i];
                }
            }

        });
    }

    public FDictionary GetGuildLivelyRewardRefTable()
    {
        return guildLivelyRewardRefTable;
    }

    public int GetGuildLivelyMaxCount()
    {
        int maxLively = 0;
        foreach (var item in guildLivelyRewardRefTable.Values)
        {
            GuildLivelyRewardRef reward = item as GuildLivelyRewardRef;
            maxLively = Mathf.Max(reward.need, maxLively);
        }
        return maxLively;
    }

    public GuildLivelyRewardRef GetGuildLivelyRewardRef(int _livelyID)
    {
        if (guildLivelyRewardRefTable.ContainsKey(_livelyID))
            return guildLivelyRewardRefTable[_livelyID] as GuildLivelyRewardRef;
        return null;
    }
    #endregion

	//===================================I=============================








    //===================================H=============================



    //===================================I=============================

    #region 强化套装静态配置 by鲁家旗
    protected FDictionary strengSuitTable = new FDictionary();
    public FDictionary StrengSuitTable
    {
        get { return strengSuitTable; }
    }
    public void InitStrengSuitRefTable()
    {
        pendings++;
        strengSuitTable.Clear();
        AssetMng.instance.LoadAsset<StrengthenSuitRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "str_suit" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    strengSuitTable[x.infoList[i].id] = x.infoList[i];
                }
            }
        });
    }

    /// <summary>
    ///通过ID获得本地数据
    /// </summary>
    public StrengthenSuitRef GetStrengSuitRef (int _id)
    {
        if (!strengSuitTable.ContainsKey(_id))
        {
            GameSys.LogError("strengSuitTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return strengSuitTable[_id] as StrengthenSuitRef;
    }
    /// <summary>
    /// 返回指定类型的数据 1为套装属性 2为光环特效
    /// </summary>
    /// <param name="_type"></param>
    /// <returns></returns>
    public List<StrengthenSuitRef> GetStrengSuitRefList(int _type)
    {
        List<StrengthenSuitRef> list = new List<StrengthenSuitRef>();
        foreach (StrengthenSuitRef data in strengSuitTable.Values)
        {
            if (_type == data.type)
            {
                list.Add(data);
            }
        }
        return list;
    }
    #endregion


    //===================================J============================= 

    #region 聚宝盆（塔罗牌）静态配置 by朱素云
    protected FDictionary cornucopiaRefTable = new FDictionary();
    public void InitCornucopiaRefTable()
    {
        pendings++;
        cornucopiaRefTable.Clear();
        AssetMng.instance.LoadAsset<CornucopiaRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "cornucopia" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    cornucopiaRefTable[x.infoList[i].id] = x.infoList[i];
                }
            }
        });
    }
     
    public CornucopiaRef GetCornucopiaRef(int _id)
    {
        int id = cornucopiaRefTable.Count - _id + 1;
        if (!cornucopiaRefTable.ContainsKey(id))
        {
            GameSys.LogError("cornucopiaRefTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return cornucopiaRefTable[id] as CornucopiaRef;
    }

    public int GetCornucopiaRefCount()
    {
        return cornucopiaRefTable.Count;
    }
    #endregion

    //===================================K=============================
    //===================================L=============================

    #region 等级奖励表 zsy

    protected FDictionary levelRewardTable = new FDictionary();
    public void InitLevelRewardRefTable()
    {
        pendings++;
        levelRewardTable.Clear();
        AssetMng.instance.LoadAsset<LevelRewardRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "levelReward" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    levelRewardTable[x.infoList[i].prof] = x.infoList[i];
                }       
            }
        });
    }

    /// <summary>
    ///通过职业获得本地数据
    /// </summary>
    public List <LevelRewardLevelRef> GetLevelRewardList(int prof) 
    {
        if (!levelRewardTable.ContainsKey(prof))
        {
            GameSys.LogError("levelRewardTable " + ConfigMng.Instance.GetUItext(204) + prof + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return (levelRewardTable[prof] as LevelRewardRef).stepList;
        
    }
     
    public FDictionary GetLevelRewardTable()
    {
        return levelRewardTable;
    }

    #endregion

    #region 首冲爱心静态配置 zsy

    protected FDictionary loveSpreeRefTable = new FDictionary();
    public void InitloveSpreeRefTable() 
    {
        pendings++;
        loveSpreeRefTable.Clear();
        AssetMng.instance.LoadAsset<LoveSpreeRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "LoveSpree" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    if (!loveSpreeRefTable.ContainsKey(x.infoList[i].stage))
                    {
                        List<LoveSpreeRef> list = new List<LoveSpreeRef>();
                        list.Add(x.infoList[i]);
                        loveSpreeRefTable[x.infoList[i].stage] = list;
                    }
                    else
                    {
                        List<LoveSpreeRef> oldList = loveSpreeRefTable[x.infoList[i].stage] as List<LoveSpreeRef>;
                        if (oldList != null)
                        {
                            oldList.Add(x.infoList[i]);
                            loveSpreeRefTable[x.infoList[i].stage] = oldList;
                        }
                        else
                        {
                            GameSys.LogError(ConfigMng.Instance.GetUItext(206));
                        }
                       
                    }
                }
            }
        });
    }

    /// <summary>
    ///通过职业获得首冲爱心数据
    /// </summary>
    public LoveSpreeRef GetLoveSpreeRef(int _prof,int _stage)
    {
        if (!loveSpreeRefTable.ContainsKey(_stage))
        {
            GameSys.LogError("loveSpreeRefTable " + ConfigMng.Instance.GetUItext(204) +_stage + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        else
        {
            List<LoveSpreeRef> RefList = loveSpreeRefTable[_stage] as List<LoveSpreeRef>;
            if (RefList != null)
            {
                for (int i = 0; i < RefList.Count; i++)
                {
                    if (RefList[i].prof == _prof || RefList[i].prof==0)
                        return RefList[i];
                }
                return null;
            }
            else
            {
                return null;
            }
        }

    }

    public  FDictionary GetLoveRewardTable
    {
        get
        {
            return loveSpreeRefTable;
        }
    }
    #endregion


    //===================================M=============================

    #region 怪物静态配置
    protected FDictionary monsterTable = new FDictionary();

    public void InitMonsterTable()
    {
        pendings++;
        monsterTable.Clear();
        AssetMng.instance.LoadAsset<MonsterRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "MonsterConfig" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    monsterTable[x.infoList[i].id] = x.infoList[i];
                }
            }

        });
    }


    public MonsterRef GetMonsterRef(int _id)
    {
        if (!monsterTable.ContainsKey(_id))
        {
            GameSys.LogError("monsterTable " + ConfigMng.Instance.GetUItext(204) +_id+ ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return monsterTable[_id] as MonsterRef;
    }
    #endregion

    #region 怪物分布静态配置 add by 龙英杰
    protected FDictionary monsterDistributionTable = new FDictionary();
    protected FDictionary sceneMonsterDistributionRefTable = new FDictionary();

    public void InitMonsterDistributionRef()
    {
        pendings++;
        monsterDistributionTable.Clear();
        sceneMonsterDistributionRefTable.Clear();
        AssetMng.instance.LoadAsset<MonsterDistributionRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "MonsterDistributionConfig" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    monsterDistributionTable[x.infoList[i].id] = x.infoList[i];
                }
                foreach (MonsterDistributionRef item in monsterDistributionTable.Values)
                {
                    List<MonsterDistributionRef> list = null;
                    if (!sceneMonsterDistributionRefTable.ContainsKey(item.sceneId))
                    {
                        list = new List<MonsterDistributionRef>();
                        sceneMonsterDistributionRefTable[item.sceneId] = new List<MonsterDistributionRef>();
                    }
                    list = sceneMonsterDistributionRefTable[item.sceneId] as List<MonsterDistributionRef>;
                    list.Add(item);
                }
            }

        });
    }
    public MonsterDistributionRef GetMonsterDistributionRef(int _id)
    {
        if (!monsterDistributionTable.ContainsKey(_id))
        {
            GameSys.LogError("monsterDistributionTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return monsterDistributionTable[_id] as MonsterDistributionRef;
    }

    public List<MonsterDistributionRef> GetMonsterDistributionRefByScene(int _sceneID)
    {
        if (sceneMonsterDistributionRefTable.ContainsKey(_sceneID))
        {
            return sceneMonsterDistributionRefTable[_sceneID] as List<MonsterDistributionRef>;
        }
        return new List<MonsterDistributionRef>();
    }

    #endregion

    #region 坐骑配置 by朱素云
    protected FDictionary mountRefTable = new FDictionary();
    public void InitMountRefTable()
    {
        pendings++;
        mountRefTable.Clear();
        AssetMng.instance.LoadAsset<MountRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "mount" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    mountRefTable[x.infoList[i].mountId] = x.infoList[i]; 
                }     
            }
        });
    }


    public MountRef GetMountRef(int _id)
    {
        if (mountRefTable.ContainsKey(_id))
        {
            MountRef data = mountRefTable[_id] as MountRef;
            return data;
        }
        return null;
    }
    public FDictionary MountRefTable
    {
        get
        {
            return mountRefTable;
        }
    }
    #endregion

    #region 坐骑属性配置 by朱素云
    protected FDictionary mountPropertyRefTable = new FDictionary();
    public void InitMountPropertyRefTable()
    {
        pendings++;
        mountPropertyRefTable.Clear();
        AssetMng.instance.LoadAsset<RidePropertyRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "rideProperty" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    x.infoList[i].SetAttr();
                    mountPropertyRefTable[x.infoList[i].level] = x.infoList[i]; 
                }     
            }
        });
    }
    public RidePropertyRef GetMountPropertyRef(int _id)
    {
        if (mountPropertyRefTable.ContainsKey(_id))
        {
            RidePropertyRef data = mountPropertyRefTable[_id] as RidePropertyRef;
            return data;
        }
        return null;
    }
    public FDictionary RidePropertyRefTable
    {
        get
        {
            return mountPropertyRefTable;
        }
    }
    #endregion

    #region 坐骑幻化配置 by朱素云
    protected FDictionary skinPropertyRefTable = new FDictionary();
    public void InitSkinPropertyRefTable()
    {
        pendings++;
        skinPropertyRefTable.Clear();
        AssetMng.instance.LoadAsset<SkinPropertyRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "skinProperty" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    x.infoList[i].SetAttr();
                    skinPropertyRefTable[x.infoList[i].id] = x.infoList[i];
                }      
            }
        });
    }

    public SkinPropertyRef GetSkinPropertyRef(int _id)
    {
        if (skinPropertyRefTable.ContainsKey(_id))
        {
            SkinPropertyRef data = skinPropertyRefTable[_id] as SkinPropertyRef;
            return data;
        }
        return null;
    }

    public FDictionary SkinPropertyRefTable
    {
        get
        {
            return skinPropertyRefTable;
        }
    }
    #endregion

    #region 法宝配置 by鲁家旗
    protected FDictionary magicWeaponRefTabel = new FDictionary();
    public FDictionary MagicWeaponRefTabel
    {
        get { return magicWeaponRefTabel; }
    }
    public void InitMagicWeaponRefTabel()
    {
        pendings++;
        magicWeaponRefTabel.Clear();
        AssetMng.instance.LoadAsset<MagicWeaponRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "Fabao" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    magicWeaponRefTabel[x.infoList[i].id] = x.infoList[i];
                }
            }
        });
    }
    public MagicWeaponRef GetMagicWeaponRef(int _id)
    {
        if (!magicWeaponRefTabel.ContainsKey(_id))
        {
            GameSys.LogError("magicWeaponRefTabel" + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return magicWeaponRefTabel[_id] as MagicWeaponRef;
    }
    #endregion

    #region 骑装品质配置 by邓成
    protected FDictionary mountEquipQuality = new FDictionary();
    public FDictionary MountEquipQuality
    {
        get { return mountEquipQuality; }
    }
    public void InitMountEquipQualityRefTabel()
    {
        pendings++;
        mountEquipQuality.Clear();
        AssetMng.instance.LoadAsset<MountEquQuailtRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "quality" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    mountEquipQuality[x.infoList[i].id] = x.infoList[i];
                }
            }
        });
    }
    public MountEquQuailtRef GetMountEquipQualityRef(int _quality, EquipSlot _slot)
    {
        foreach (var val in mountEquipQuality.Values)
        {
            MountEquQuailtRef attr = val as MountEquQuailtRef;
            if (attr.quality == _quality && attr.position == (int)_slot)
            {
                return attr;
            }
        }
        return null;
    }
    #endregion

    #region 骑装品质属性配置 by邓成
    protected FDictionary mountEquipQualityAttr = new FDictionary();
    public FDictionary MountEquipQualityAttr
    {
        get { return mountEquipQualityAttr; }
    }
    public void InitMountEquQualityAttributeRefTable()
    {
        pendings++;
        mountEquipQualityAttr.Clear();
        AssetMng.instance.LoadAsset<MountEquQualityAttributeRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "quality_attr" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    mountEquipQualityAttr[x.infoList[i].id] = x.infoList[i];
                }
            }
        });
    }
    public MountEquQualityAttributeRef GetMountEquQualityAttributeRef(int _quality,EquipSlot _slot)
    {
        foreach (var val in mountEquipQualityAttr.Values)
        {
            MountEquQualityAttributeRef attr = val as MountEquQualityAttributeRef;
            if (attr.quality == _quality && attr.position == (int)_slot)
            {
                return attr;
            }
        }
        GameSys.LogError(ConfigMng.Instance.GetUItext(207) + _quality + ConfigMng.Instance.GetUItext(202));
        return null;
    }
    #endregion

    #region 骑装品质最大强化等级配置 by邓成
    protected FDictionary mountEquipMaxLvTable = new FDictionary();
    public void InitMountEquipMaxLvTable()
    {
        pendings++;
        mountEquipMaxLvTable.Clear();
        AssetMng.instance.LoadAsset<MountEquQuailtMaxRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "max_lev" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    mountEquipMaxLvTable[x.infoList[i].quality] = x.infoList[i];
                }
            }
        });
    }
    public int GetMountEquQualityMaxUpgradeLv(int _quality)
    {
        if (mountEquipMaxLvTable.ContainsKey(_quality))
        {
            MountEquQuailtMaxRef max = mountEquipMaxLvTable[_quality] as MountEquQuailtMaxRef;
            return max.maxLev;
        }
        GameSys.LogError("mountEquipMaxLvTable" + ConfigMng.Instance.GetUItext(208) + _quality + ConfigMng.Instance.GetUItext(202));
        return 0;
    }
    #endregion

    #region 骑装培养等级配置 by邓成
    protected FDictionary mountStrenLevRefTable = new FDictionary();
    public FDictionary MountStrenLevRefTable
    {
        get { return mountStrenLevRefTable; }
    }
    public void InitMountStrenLevRefTable()
    {
        pendings++;
        mountStrenLevRefTable.Clear();
        AssetMng.instance.LoadAsset<MountStrenLevRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "stren_lev" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    mountStrenLevRefTable[x.infoList[i].id] = x.infoList[i];
                }
            }
        });
    }
    public MountStrenLevRef GetMountStrenLevRef(int _lev,EquipSlot _slot)
    {
        foreach (var val in mountStrenLevRefTable.Values)
        {
            MountStrenLevRef attr = val as MountStrenLevRef;
            if (attr.lev == _lev && attr.position == (int)_slot)
            {
                return attr;
            }
        }
        GameSys.LogError("mountStrenLevRefTable"+ConfigMng.Instance.GetUItext(209) + _lev +ConfigMng.Instance.GetUItext(202));
        return null;
    }
    #endregion

    #region 骑装培养消耗配置 by邓成
    protected FDictionary mountEquipStrenConsume = new FDictionary();
    public FDictionary MountEquipStrenConsume
    {
        get { return mountEquipStrenConsume; }
    }
    public void InitMountStrenConsumeRefTable()
    {
        pendings++;
        mountEquipStrenConsume.Clear();
        AssetMng.instance.LoadAsset<MountStrenConsumeRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "stren_consum" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    mountEquipStrenConsume[x.infoList[i].lev] = x.infoList[i];
                }
            }
        });
    }
    public MountStrenConsumeRef GetMountStrenConsumeRef(int _lev)
    {
        if (!mountEquipStrenConsume.ContainsKey(_lev))
        {
            GameSys.LogError("mountEquipStrenConsume"+ConfigMng.Instance.GetUItext(204) + _lev + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return mountEquipStrenConsume[_lev] as MountStrenConsumeRef;
    }
    #endregion

    #region 骑装套装配置 by邓成
    protected FDictionary mountSuitRefTable = new FDictionary();
    public FDictionary MountSuitRefTable
    {
        get { return mountSuitRefTable; }
    }
    public void InitMountSuitRefTable()
    {
        pendings++;
        mountSuitRefTable.Clear();
        AssetMng.instance.LoadAsset<MountSuitRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "suit" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    mountSuitRefTable[x.infoList[i].quality] = x.infoList[i];
                }
            }
        });
    }
    public MountSuitRef GetMountSuitRef(int _quality)
    {
        if (!mountSuitRefTable.ContainsKey(_quality))
        {
            GameSys.LogError("mountSuitRefTable" + ConfigMng.Instance.GetUItext(204) + _quality + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return mountSuitRefTable[_quality] as MountSuitRef;
    }
    #endregion
    //===================================N=============================

    #region 功能预告 by黄洪兴

    protected FDictionary newFunctionHintsRefTable = new FDictionary();
    protected List<int> newFunctionHintsSteps = new List<int>();
    public void InitNewFunctionHintsRefTable()
    {
        pendings++;
        newFunctionHintsRefTable.Clear();
        newFunctionHintsSteps.Clear();
        AssetMng.instance.LoadAsset<NewFunctionHintsRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "NewFunctionHintsConfig" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
					for (int i = 0; i < x.infoList.Count; i++)
					{
						newFunctionHintsRefTable[x.infoList[i].Step] = x.infoList[i];
						newFunctionHintsSteps.Add(x.infoList[i].Step);
					} 
            }
        });
    }



    public List<int> GetAllNewFunctionHintsSteps()
    {
        return newFunctionHintsSteps;
    }

    public NewFunctionHintsRef GetNewFunctionHintsRef(int _step)
    {
        if (!newFunctionHintsRefTable.ContainsKey(_step))
        {
            GameSys.LogError("npcRefTable " + ConfigMng.Instance.GetUItext(204) + _step + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return newFunctionHintsRefTable[_step] as NewFunctionHintsRef;
    }

    public NewFunctionHintsRef GetNewFunctionHintsById(int _id)
    {
        NewFunctionHintsRef refData = null;
        foreach (NewFunctionHintsRef data in newFunctionHintsRefTable.Values)
        {
            if (data.id == _id)
            {
                refData = data;
                refData.des = refData.des.Replace("\\n", "\n");
                refData.text = refData.text.Replace("\\n", "\n");
            }

        }
        return refData;
    }

    #endregion



	#region 随机名字静态配置 何明军
	protected FDictionary nameRefTable = new FDictionary();
	protected FDictionary profNameCount = new FDictionary();
	public void InitNameRefTable()
	{
		pendings++;
		nameRefTable.Clear();
		
		AssetMng.instance.LoadAsset<NameRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "NameConfig" + extension, (x, y) =>
			{
				pendings--;
				if (y == EResult.Success)
				{
					for (int i = 0; i < x.infoList.Count; i++)
					{
						nameRefTable[x.infoList[i].id] = x.infoList[i];
					}
				}
			});
	}
	public NameRef GetNameRef(int _id)
	{
		if (!nameRefTable.ContainsKey(_id))
		{
            GameSys.LogError("NameConfig " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
			return null;
		}
		return nameRefTable[_id] as NameRef;
	}
	public FDictionary GetNameRefTable()
	{
		return nameRefTable;
	}
	/// <summary>
	/// 获得职业随机名字的数量
	/// </summary>
	public int ProfNameCount(int prof){
		
		if(!profNameCount.ContainsKey(prof)){
			int count = 0;
			foreach(NameRef data in nameRefTable.Values){
				if(data.names.Count > (prof - 1) && !string.Empty.Equals(data.names[prof - 1]) && !"0".Equals(data.names[prof - 1])){
					count ++;
				}
			}
			profNameCount[prof] = count;
		}
		return (int)profNameCount[prof];
	}
	#endregion
	
    #region NPC静态配置
    protected FDictionary npcRefTable = new FDictionary();


    public void InitNPCRefTable()
    {
        pendings++;
        npcRefTable.Clear();
        AssetMng.instance.LoadAsset<SceneNPCRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "SceneNPCConfig" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    npcRefTable[x.infoList[i].id] = x.infoList[i];
                }
            }

        });
    }


    public SceneNPCRef GetNPCRef(int _id)
    {
        if (!npcRefTable.ContainsKey(_id))
        {
            GameSys.LogError("npcRefTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return npcRefTable[_id] as SceneNPCRef;
    }


    public FDictionary GetNpcRefTable()
    {
        return npcRefTable;
    }

    #endregion

    #region NPC类型静态配置 by吴江
    protected FDictionary npcTypeRefTable = new FDictionary();


    public void InitNPCTypeRefTable()
    {
        pendings++;
        npcTypeRefTable.Clear();
        AssetMng.instance.LoadAsset<NPCTypeRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "NpcType" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    npcTypeRefTable[x.infoList[i].id] = x.infoList[i];
                }
            }

        });
    }


    public NPCTypeRef GetNPCTypeRef(int _id)
    {
        if (!npcTypeRefTable.ContainsKey(_id))
        {
            GameSys.LogError("npcTypeRefTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return npcTypeRefTable[_id] as NPCTypeRef;
    }

    #endregion

    #region NPC行为静态配置 by吴江
    protected FDictionary npcActionRefTable = new FDictionary();


    public void InitNPCActionRefTable()
    {
        pendings++;
        npcActionRefTable.Clear();
        AssetMng.instance.LoadAsset<NPCActionRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "NPCActionConfig" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    npcActionRefTable[x.infoList[i].actionId] = x.infoList[i];
                }
            }

        });
    }


    public NPCActionRef GetNPCActionRef(int _id)
    {
        if (!npcActionRefTable.ContainsKey(_id))
        {
            GameSys.LogError("npcActionRefTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return npcActionRefTable[_id] as NPCActionRef;
    }
    #endregion

    #region NPC AI静态配置 by吴江
    protected FDictionary npcAIRefTable = new FDictionary();


    public void InitNPCAIRefTable()
    {
        pendings++;
        npcAIRefTable.Clear();
        AssetMng.instance.LoadAsset<NPCAIRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "NPCAIConfig" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    npcAIRefTable[x.infoList[i].id] = x.infoList[i];
                }
            }

        });
    }


    public NPCAIRef GetNPCAIRef(int _id)
    {
        if (!npcAIRefTable.ContainsKey(_id))
        {
            GameSys.LogError("npcAIRefTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return npcAIRefTable[_id] as NPCAIRef;
    }

    public NPCAIRef GetNPCAIRefByType(int _type)
    {
        foreach (NPCAIRef item in npcAIRefTable.Values)
        {
            if (item.npcId == _type)
            {
                return item;
            }
        }
        GameSys.LogError("npcAIRefTable " + ConfigMng.Instance.GetUItext(204) + _type + ConfigMng.Instance.GetUItext(202));
        return null;
    }

    public List<NPCAIRef> GetNPCAIRefByScene(int _scene)
    {
        List<NPCAIRef> list = new List<NPCAIRef>();
        foreach (NPCAIRef item in npcAIRefTable.Values)
        {
            if (item.scene == _scene)
            {
                list.Add(item);
            }
        }
        
        return list;
    }

    #endregion
	
	#region NPC静态配置
	protected FDictionary newFunctionRefTable = new FDictionary();


	public void InitNewFunctionRefTable()
	{
		pendings++;
		newFunctionRefTable.Clear();
		AssetMng.instance.LoadAsset<NewFunctionRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "NewFunction" + extension, (x, y) =>
			{
				pendings--;
				if (y == EResult.Success)
				{
					for (int i = 0; i < x.infoList.Count; i++)
					{
						newFunctionRefTable[x.infoList[i].id] = x.infoList[i];
					}
				}

			});
	}


	public NewFunctionRef GetNewFunctionRef(int _id)
	{
		if (!newFunctionRefTable.ContainsKey(_id))
		{
            GameSys.LogError("newFunctionRefTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
			return null;
		}
		return newFunctionRefTable[_id] as NewFunctionRef;
	}
	#endregion
    //===================================O=============================
    //===================================P=============================
    #region 角色配置
    protected FDictionary playerConfigRefTable = new FDictionary();


    public void InitPlayerConfigRefTable()
    {
        pendings++;
        playerConfigRefTable.Clear();
        AssetMng.instance.LoadAsset<PlayerConfigRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "PlayerCharConfig" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
					for (int i = 0; i < x.infoList.Count; i++)
					{
						playerConfigRefTable[x.infoList[i].id] = x.infoList[i];
					}
            }
        });
    }


    public PlayerConfig GetPlayerConfig(int _id)
    {
        if (!playerConfigRefTable.ContainsKey(_id))
        {
            GameSys.LogError("PlayerConfigRefTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return playerConfigRefTable[_id] as PlayerConfig;
    }

    #endregion

	#region 合成静态配置

	protected FDictionary blendRefTable = new FDictionary();
	public void InitBlendRefTable()
	{
		pendings++;
		blendRefTable.Clear();
		AssetMng.instance.LoadAsset<BlendRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "blend" + extension, (x, y) =>
			{
				pendings--;
				if (y == EResult.Success)
				{
					for (int i = 0; i < x.infoList.Count; i++)
					{
						blendRefTable[x.infoList[i].id] = x.infoList[i];
					}
				}
			});
	}

	public FDictionary GetBlendRefTable(){
		return blendRefTable;
	}
	public BlendRef GetBlendRef(int _id)
	{
		if (!blendRefTable.ContainsKey(_id))
		{
            GameSys.LogError("blendRefTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
			return null;
		}
		return blendRefTable[_id] as BlendRef;
	}

	public List<BlendRef> GetTypeBlend(int type){
		List<BlendRef> blendRefs = new List<BlendRef>();
		foreach(BlendRef refa in blendRefTable.Values){
			if(type == refa.sort){
				blendRefs.Add(refa);
			}
		}
		return blendRefs;
	}

	#endregion

    #region 被动技能
    protected FDictionary passiveSkillRefTable = new FDictionary();
    public void InitPassiveSkillRefTable()
    {
        pendings++;
        passiveSkillRefTable.Clear();
        AssetMng.instance.LoadAsset<PassiveSkillRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "passiveSkillConfig" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
					for (int i = 0; i < x.infoList.Count; i++)
					{
						passiveSkillRefTable[x.infoList[i].id] = x.infoList[i];
					}
            }
        });
    }
    public PassiveSkillRef GetPassiveSkillRef(int _id)
    {
        if (!passiveSkillRefTable.ContainsKey(_id))
        {
            GameSys.LogError("passiveSkillRefTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return passiveSkillRefTable[_id] as PassiveSkillRef;
    }
    #endregion

    #region 冒泡配置
    protected FDictionary poPoRefTable = new FDictionary();
    public void InitPoPoRefTable()
    {
        pendings++;
        poPoRefTable.Clear();
        AssetMng.instance.LoadAsset<PoPoRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "PopoConfig" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    poPoRefTable[x.infoList[i].id] = x.infoList[i];
                }
            }
        });
    }
    public PoPoRef GetPoPoRef(int _id)
    {
        if (!poPoRefTable.ContainsKey(_id))
        {
            GameSys.LogError("poPoRefTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return poPoRefTable[_id] as PoPoRef;
    }
    #endregion

    #region 宠物冒泡配置
    protected FDictionary poPoPetRefTable = new FDictionary();
    public void InitPoPoPetRefTable()
    {
        pendings++;
        poPoPetRefTable.Clear();
        AssetMng.instance.LoadAsset<PoPoPetRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "popPet" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    poPoPetRefTable[x.infoList[i].id] = x.infoList[i];
                }
            }
        });
    }
    public PoPoPetRef GetPoPoPetRef(int _id)
    {
        if (!poPoPetRefTable.ContainsKey(_id))
        {
            GameSys.LogError("poPoPetRefTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return poPoPetRefTable[_id] as PoPoPetRef;
    }

    public PoPoPetRef GetPoPoPetRef(int _petID, EntourageTalkType _entourageTalkType)
    {
        foreach (PoPoPetRef item in poPoPetRefTable.Values)
        {
            if (item.petId == _petID && item.type == (int)_entourageTalkType)
                return item;
        }
        Debug.LogError("找不到_petID:" + _petID + ",type:" +(int) _entourageTalkType+"的宠物冒泡数据");
        return null;
    }
    #endregion

    #region 推送静态配置
    protected FDictionary pushedRefTable = new FDictionary();
    public void InitPushedReftable()
    {
        pendings++;
        pushedRefTable.Clear();
        AssetMng.instance.LoadAsset<PushedRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "pushed" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    pushedRefTable[x.infoList[i].id] = x.infoList[i];
                }
            }
        });
    }
    public PushedRef GetPushedRef(int _id)
    {
        if (!pushedRefTable.ContainsKey(_id))
        {
            GameSys.LogError("pushedRefTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return pushedRefTable[_id] as PushedRef;
    }
    #endregion
    //===================================Q=============================
    //===================================R=============================

    #region 复活 by黄洪兴

    protected FDictionary rebornRefTable = new FDictionary();
    public void InitRebornRefTable()
    {
        pendings++;
        rebornRefTable.Clear();
        AssetMng.instance.LoadAsset<RebornRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "reborn" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
					for (int i = 0; i < x.infoList.Count; i++)
					{
						rebornRefTable[x.infoList[i].id] = x.infoList[i];
					}
            }
        });
    }



    public RebornRef GetRebornRef(int _id)
    {
        if (!rebornRefTable.ContainsKey(_id))
        {
            GameSys.LogError("rebornRefTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        else
        {
            RebornRef Ref = rebornRefTable[_id] as RebornRef;
            return Ref;
        }
    }


    public bool CanUseItemReborn(int _id)
    {
        if (!rebornRefTable.ContainsKey(_id))
        {
            GameSys.LogError("rebornRefTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
            return false;
        }
        else
        {
            RebornRef Ref = rebornRefTable[_id] as RebornRef;
            if (Ref != null)
            {
                return Ref.item_use;
            }
            return false;
        }
    }



    #endregion



    #region 充值 by黄洪兴

    protected FDictionary rechargeRefTable = new FDictionary();
    public void InitRechargeRefTable()
    {
        pendings++;
        rechargeRefTable.Clear();
        AssetMng.instance.LoadAsset<RechargeRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "ChargeConfig" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
					for (int i = 0; i < x.infoList.Count; i++)
					{
						rechargeRefTable[x.infoList[i].id] = x.infoList[i];
						RechargeRefList.Add(x.infoList[i]);
					}
            }
        });
    }


    public List<RechargeRef> RechargeRefList = new List<RechargeRef>();

    public RechargeRef GetRechargeRef(int _eid)
    {
        if (rechargeRefTable.ContainsKey(_eid))
        {
            return rechargeRefTable[_eid] as RechargeRef;
        }
        return null;
    }



    #endregion


    #region 阵营配置表
    protected FDictionary relationRefTable = new FDictionary();

    /// <summary>
    /// 读取阵营关系表 by吴江
    /// </summary>
    public void InitRelationRefTable()
    {
        pendings++;
        relationRefTable.Clear();
        AssetMng.instance.LoadAsset<RelationRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "RelationConfig" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    x.infoList[i].InitData();
                    relationRefTable[x.infoList[i].camp1] = x.infoList[i];
                }
            }

        });
    }

    /// <summary>
    /// 根据阵营，获取跟对方的关系数据（可能为空，为空的话则说明不可攻击） by吴江
    /// </summary>
    /// <param name="_selfCamp"></param>
    /// <param name="_targetCamp"></param>
    /// <returns></returns>
    public RelationCompareRef GetRelationRef(int _selfCamp, int _targetCamp, SceneType _sceneType)
    {
        if (!relationRefTable.ContainsKey(_selfCamp))
        {
            return null;
        }
        RelationRef refData = relationRefTable[_selfCamp] as RelationRef;
        if (refData == null) return null;
        return refData.GetCompareRelation(_sceneType,_selfCamp,_targetCamp);
    }

    /// <summary>
    /// 根据阵营，获取对方的可攻击状态 by吴江
    /// </summary>
    /// <param name="_selfCamp"></param>
    /// <param name="_targetCamp"></param>
    /// <returns></returns>
    public RelationType GetRelationType(int _selfCamp, int _targetCamp, SceneType _sceneType)
    {
        if (!relationRefTable.ContainsKey(_selfCamp))
        {
            return RelationType.NO_ATTAK;
        }
        RelationRef refData = relationRefTable[_selfCamp] as RelationRef;
        if (refData == null) return RelationType.NOAUTOMATEDATTACKS;
        RelationCompareRef data = refData.GetCompareRelation(_sceneType, _selfCamp, _targetCamp);
        return data == null ? RelationType.AUTOMATEDATTACKS : data.relation;
    }
    /// <summary>
    /// 根据阵营，获取对方显示的名字颜色 by吴江
    /// </summary>
    /// <param name="_selfCamp"></param>
    /// <param name="_targetCamp"></param>
    /// <returns></returns>
    public Color GetRelationColor(int _selfCamp, int _targetCamp, SceneType _sceneType)
    {
        if (!relationRefTable.ContainsKey(_selfCamp))
        {
            return Color.yellow;
        }
        RelationRef refData = relationRefTable[_selfCamp] as RelationRef;
        if (refData == null)
        {
            return Color.red;
        }
        RelationCompareRef data = refData.GetCompareRelation(_sceneType,_selfCamp,_targetCamp);
        return data == null ? Color.yellow : data.color;
    }

    #endregion
    
    #region 法宝淬炼配置表 by鲁家旗
    protected FDictionary refineRefTable = new FDictionary();

    public void InitRefineRefTable()
    {
        pendings++;
        refineRefTable.Clear();
        AssetMng.instance.LoadAsset<RefineRefTable>(AssetMng.GetPathWithoutExtension("config/",AssetPathType.PersistentDataPath) + "Chuilian" + extension, (x,y)=>
        {
            pendings--;
            if(y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    refineRefTable[x.infoList[i].id] = x.infoList[i];
                }
            }
        });
    }

    public RefineRef GetRefineRef(int _id)
    {
        if (!refineRefTable.ContainsKey(_id))
        {
            GameSys.LogError("refineRefTable" + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return refineRefTable[_id] as RefineRef;
    }
    public FDictionary GetRefineRefTable()
    {
        return refineRefTable;
    }
    public RefineRef GetRefineRef(int _type,int _stage,int _star)
    {
        foreach(RefineRef data in refineRefTable.Values)
        {
            if (data.relationID == _type && data.stage == _stage && data.star == _star)
            {
                return data;
            }
        }
        return null;
    }
    #endregion

    #region 皇室宝箱静配置
    protected FDictionary royalBoxRefTable = new FDictionary();

    public void InitRoyalBoxRefTable()
    {
        pendings++;
        royalBoxRefTable.Clear();
        AssetMng.instance.LoadAsset<RoyalBoxRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "box" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    royalBoxRefTable[x.infoList[i].boxItemID] = x.infoList[i];
                }
            }
        });
    }

    public RoyalBoxRef GetRoyalBoxRef(int _id)
    {
        if (!royalBoxRefTable.ContainsKey(_id))
        {
            GameSys.LogError("royalBoxRefTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return royalBoxRefTable[_id] as RoyalBoxRef;
    }
    #endregion

    #region 宠物成长资质灵修配置 by朱素云

    protected FDictionary petDataRefTable = new FDictionary();

    public void InitPetDataRefTable() 
    {
        pendings++;
        petDataRefTable.Clear();
        AssetMng.instance.LoadAsset<NewPetDataRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "petData" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    petDataRefTable[x.infoList[i].level] = x.infoList[i];
                } 
            }
        });
    }


    public NewPetDataRef GetPetDataRef(int _lv)
    {
        if (!petDataRefTable.ContainsKey(_lv))
        {
            GameSys.LogError("petDataRefTable " + ConfigMng.Instance.GetUItext(204) + _lv + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return petDataRefTable[_lv] as NewPetDataRef;
    }

    public FDictionary GetPetDataRefTable
    {
        get  
        {
            return petDataRefTable;
        }
    }

    #endregion

    #region 宠物静态配置 by朱素云

    protected FDictionary newPetRefTable = new FDictionary();
    public FDictionary NewPetRefTable
    {
        get
        {
            return newPetRefTable;
        }
    }

    public void InitNewPetRefTable()
    {
        pendings++;
        newPetRefTable.Clear();
        AssetMng.instance.LoadAsset<NewPetRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "pet" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                   newPetRefTable[x.infoList[i].id] = x.infoList[i];
                }   
            }
        });
    }


    public NewPetRef GetNewPetRef(int _id)
    {
        if (newPetRefTable.ContainsKey(_id))
        {
            NewPetRef data = newPetRefTable[_id] as NewPetRef;
            return data;
        }
        return null;
    } 
    #endregion

    #region  宠物技能合成配置 by朱素云

    protected FDictionary petSkillComposeTable = new FDictionary();
    public void InitPetSkillComposeRefTable()
    {
        pendings++;
        petSkillComposeTable.Clear();
        AssetMng.instance.LoadAsset<NewPetSkillComposeRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "petSkillCompose" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    petSkillComposeTable[x.infoList[i].item] = x.infoList[i];
                }    
            }
        });
    }


    public NewPetSkillComposeRef GetPetSkillComposeRef(int _id)
    {
        if (!petSkillComposeTable.ContainsKey(_id))
        {
            GameSys.LogError("petSkillComposeTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        } 
        return petSkillComposeTable[_id] as NewPetSkillComposeRef;
    }
    #endregion

    #region 宠物技能槽配置 by朱素云

    protected FDictionary petSkillNumRefTable = new FDictionary();

    public void InitPetSkillNumRefTable()
    {
        pendings++;
        petSkillNumRefTable.Clear();
        AssetMng.instance.LoadAsset<NewPetSkillNumRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "petSkillNum" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    petSkillNumRefTable[x.infoList[i].id] = x.infoList[i];
                }    
            }
        });
    }


    public NewPetSkillNumRef GetPetSkillNumRef(int _id)
    {
        if (petSkillNumRefTable.ContainsKey(_id))
        {
            NewPetSkillNumRef data = petSkillNumRefTable[_id] as NewPetSkillNumRef;
            return data;
        }
        return null;
    }
    public  FDictionary GetPetSkillNumRefTable() 
    {
        return petSkillNumRefTable; 
    }
    #endregion

    #region 宠物技能配置 by朱素云

    protected FDictionary petSkillRefTable = new FDictionary();
    protected FDictionary petSkillRef = new FDictionary();

    public void InitPetSkillRefTable()
    {
        pendings++;
        petSkillRefTable.Clear();
        petSkillRef.Clear();
        AssetMng.instance.LoadAsset<NewPetSkillRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "petSkill" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    petSkillRefTable[x.infoList[i].id] = x.infoList[i];
                    if (x.infoList[i].book.Count > 0) petSkillRef[x.infoList[i].book[0].eid] = x.infoList[i]; 
                }    
            }
        });
    }

    public FDictionary GetPetSkillRefTable
    {
        get
        {
            return petSkillRefTable;
        }
    }
    /// <summary>
    /// 获取该等级的技能
    /// </summary> 
    public List<NewPetSkillRef> GetDividedSkillByLev(int _lev)
    { 
        List<NewPetSkillRef> skillList = new List<NewPetSkillRef>();
        foreach (NewPetSkillRef skill in petSkillRefTable.Values)
        {
            if (skill.book.Count > 0)
            {
                if (ConfigMng.Instance.GetEquipmentRef(skill.book[0].eid).psetSkillLevel == _lev)
                {
                    skillList.Add(skill);
                } 
            }
        }
        return skillList;
    }

    public NewPetSkillRef GetPetSkillRef(int _id)
    {
        if (!petSkillRefTable.ContainsKey(_id))
        {
            GameSys.LogError("petSkillRefTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return petSkillRefTable[_id] as NewPetSkillRef;
    }
    public NewPetSkillRef GetPetSkillRefByBook(int _id)
    {
        if (!petSkillRef.ContainsKey(_id))
        {
            GameSys.LogError("petSkillRef " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return petSkillRef[_id] as NewPetSkillRef;
    }
    #endregion
    //===================================S=============================
    #region 时装等级静态配置表 by zsy
    protected FDictionary fashionLevelRefTable = new FDictionary();
    public void InitFashionLevelRefTable()
    {
        pendings++;
        fashionLevelRefTable.Clear();
        AssetMng.instance.LoadAsset<FashionLevelRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "showlevel" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    x.infoList[i].SetAttr();
                    fashionLevelRefTable[x.infoList[i].level] = x.infoList[i]; 
                }
            }
        });
    }

    public FashionLevelRef GetFashionLevelRef(int _id) 
    {
        if (!fashionLevelRefTable.ContainsKey(_id))
        {
            GameSys.LogError("fashionLevelRefTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return fashionLevelRefTable[_id] as FashionLevelRef;
    }

    public FDictionary GetFashionLevelRefTable()
    {
        return fashionLevelRefTable;
    }
    #endregion

	#region 系统邮件内容配置
	protected FDictionary systemMailRefTable = new FDictionary();


	public void InitSystemMailRefTable()
	{
		pendings++;
		systemMailRefTable.Clear();
		AssetMng.instance.LoadAsset<SystemMailRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "System_mail" + extension, (x, y) =>
			{
				pendings--;
				if (y == EResult.Success)
				{
					for (int i = 0; i < x.infoList.Count; i++)
					{
						systemMailRefTable[x.infoList[i].id] = x.infoList[i];
					}
				}
			});
	}


	public SystemMailRef GetSystemMailRef(int _id)
	{
		if (!systemMailRefTable.ContainsKey(_id))
		{
            GameSys.LogError("System_mail " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
			return null;
		}
		return systemMailRefTable[_id] as SystemMailRef;
	}

	#endregion
    #region 场景配置
    protected FDictionary sceneRefTable = new FDictionary();


    public void InitSceneRefTable()
    {
        pendings++;
        sceneRefTable.Clear();
        AssetMng.instance.LoadAsset<SceneRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "SceneConfig" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
					for (int i = 0; i < x.infoList.Count; i++)
					{
						sceneRefTable[x.infoList[i].id] = x.infoList[i];
					}
            }
        });
    }


    public SceneRef GetSceneRef(int _id)
    {
        if (!sceneRefTable.ContainsKey(_id))
        {
            GameSys.LogError("sceneRefTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return sceneRefTable[_id] as SceneRef;
    }

    public string GetSceneName(int _id)
    {
        if (!sceneRefTable.ContainsKey(_id))
        {
            GameSys.LogError("sceneRefTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
            return string.Empty;
        }
        SceneRef  Ref =sceneRefTable[_id] as SceneRef;
        if (Ref != null)
            return Ref.name;
        return string.Empty;
    }
    #endregion
	
	#region 阶梯消耗配置  何明军
	protected FDictionary stepConsumptionRefTable = new FDictionary();


	public void InitStepConsumptionRefTable()
	{
		pendings++;
		stepConsumptionRefTable.Clear();
		AssetMng.instance.LoadAsset<StepConsumptionRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "StepConsumption" + extension, (x, y) =>
			{
				pendings--;
				if (y == EResult.Success)
				{
					for (int i = 0; i < x.infoList.Count; i++)
					{
						stepConsumptionRefTable[x.infoList[i].id] = x.infoList[i];
					}
				}
			});
	}


	public StepConsumptionRef GetStepConsumptionRef(int _id)
	{
		if (!stepConsumptionRefTable.ContainsKey(_id))
		{
            GameSys.LogError("stepConsumptionRefTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
			return null;
		}
		return stepConsumptionRefTable[_id] as StepConsumptionRef;
	}

	#endregion

    #region 场景NPC表 by吴江
    protected FDictionary sceneNPCRefTable = new FDictionary();


    public void InitSceneNPCRefTable()
    {
        pendings++;
        sceneNPCRefTable.Clear();
        AssetMng.instance.LoadAsset<SceneNPCRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "SceneNPCConfig" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    sceneNPCRefTable[x.infoList[i].id] = x.infoList[i];
                }
            }

        });
    }


    public SceneNPCRef GetSceneNPCRef(int _id)
    {
        if (!sceneNPCRefTable.ContainsKey(_id))
        {
            GameSys.LogError("sceneNPCRefTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return sceneNPCRefTable[_id] as SceneNPCRef;
    }


    public FDictionary GetSceneNPCRefTable
    {
        get
        {
            return sceneNPCRefTable;
        }
    }
    #endregion

    #region 场景物品类型表 by吴江
    protected FDictionary sceneItemRefTable = new FDictionary();


    public void InitSceneItemRefTable()
    {
        pendings++;
        sceneNPCRefTable.Clear();
        AssetMng.instance.LoadAsset<SceneItemRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "SceneItem" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    sceneItemRefTable[x.infoList[i].id] = x.infoList[i];
                }
            }

        });
    }


    public SceneItemRef GetSceneItemRef(int _id)
    {
        if (!sceneItemRefTable.ContainsKey(_id))
        {
            GameSys.LogError("sceneItemRefTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return sceneItemRefTable[_id] as SceneItemRef;
    }
    #endregion

    #region 场景物品实例表 by吴江
    protected FDictionary sceneItemDisRefTable = new FDictionary();


    public void InitSceneItemDisRefTable()
    {
        pendings++;
        sceneItemDisRefTable.Clear();
        AssetMng.instance.LoadAsset<SceneItemDisRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "SceneItemDisConfig" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    sceneItemDisRefTable[x.infoList[i].id] = x.infoList[i];
                }
            }

        });
    }


    public SceneItemDisRef GetSceneItemDisRef(int _id)
    {
        if (!sceneItemDisRefTable.ContainsKey(_id))
        {
            GameSys.LogError("sceneItemDisRefTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return sceneItemDisRefTable[_id] as SceneItemDisRef;
    }
    #endregion

    #region 技能主表配置 by吴江
    protected FDictionary skillMainRefTable = new FDictionary();
    /// <summary>
    /// 以职业为索引的技能配置表
    /// </summary>
    protected FDictionary skillProfRefTable = new FDictionary();



    public void InitSkillMainRefTable()
    {
        pendings++;
        skillMainRefTable.Clear();
        skillProfRefTable.Clear();
        AssetMng.instance.LoadAsset<SkillMainConfigRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "skillMainConfig" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    skillMainRefTable[x.infoList[i].skillId] = x.infoList[i];
                    if (!skillProfRefTable.ContainsKey(x.infoList[i].skillRole))
                    {
                        skillProfRefTable[x.infoList[i].skillRole] = new List<SkillMainConfigRef>();
                    }
                    List<SkillMainConfigRef> list = skillProfRefTable[x.infoList[i].skillRole] as List<SkillMainConfigRef>;
                    list.Add(x.infoList[i]);
                }
            }
        });
    }


    public SkillMainConfigRef GetSkillMainConfigRef(int _id)
    {
        if (!skillMainRefTable.ContainsKey(_id))
        {
            GameSys.LogError("skillMainRefTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return skillMainRefTable[_id] as SkillMainConfigRef;
    }


    public List<SkillMainConfigRef> GetProfSkillList(int _prof)
    {
        if (skillProfRefTable.ContainsKey(_prof))
        {
            return skillProfRefTable[_prof] as List<SkillMainConfigRef>;
        }
        return null;
    }
    #endregion

    #region 技能表现配置 by吴江
    protected FDictionary skillPerformanceRefTable = new FDictionary();


    public void InitSkillPerformanceRefTable()
    {
        pendings++;
        skillPerformanceRefTable.Clear();
        AssetMng.instance.LoadAsset<SkillPerformanceRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "skillPerformanceConfig" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    skillPerformanceRefTable[x.infoList[i].skillId] = x.infoList[i];
                }
            }
        });
    }


    public SkillPerformanceRef GetSkillPerformanceRef(int _id)
    {
        if (!skillPerformanceRefTable.ContainsKey(_id))
        {
            GameSys.LogError("skillPerformanceRefTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return skillPerformanceRefTable[_id] as SkillPerformanceRef;
    }
    #endregion

    #region 技能符文表配置 by 贺丰
    protected FDictionary skillRuneRefTable = new FDictionary();


    public void InitSkillRuneRefTable()
    {
        pendings++;
        skillRuneRefTable.Clear();
        AssetMng.instance.LoadAsset<SkillRuneRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "skillRuneConfig" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    skillRuneRefTable[x.infoList[i].runeId] = x.infoList[i];
                }
            }
        });
    }


    public SkillRuneRef GetSkillRuneRef(int _id)
    {
        if (!skillRuneRefTable.ContainsKey(_id))
        {
            GameSys.LogError("skillRuneRefTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return skillRuneRefTable[_id] as SkillRuneRef;
    }
    #endregion

    #region 技能升级表配置 by 贺丰
    protected FDictionary skillMainLvRefTable = new FDictionary();


    public void InitSkillMainLvRefTable()
    {
        pendings++;
        skillMainLvRefTable.Clear();
        AssetMng.instance.LoadAsset<SkillMainLvRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "skillMainLvlConfig" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    if (!skillMainLvRefTable.ContainsKey(x.infoList[i].skillId))
                    {
                        skillMainLvRefTable[x.infoList[i].skillId] = new FDictionary();
                    }
                    FDictionary dic = skillMainLvRefTable[x.infoList[i].skillId] as FDictionary;
                    dic[x.infoList[i].skillLv] = x.infoList[i];
                }
            }
        });
    }


    public SkillMainLvRef GetSkillMainLvRef(int _id,int _lv)
    {
         if (!skillMainLvRefTable.ContainsKey(_id))
        {
            GameSys.LogError("skillMainLvRefTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        FDictionary dic = skillMainLvRefTable[_id] as FDictionary;
        if (dic != null)
        {
            return dic[_lv] as SkillMainLvRef;
        }
        GameSys.LogError("skillLvDataRefTable "+ConfigMng.Instance.GetUItext(204) + _id + " , " + _lv +ConfigMng.Instance.GetUItext(202));
        return null;
    }
    #endregion
	
	#region 转生表配置 by 何明军
	protected FDictionary superLifeRefTable = new FDictionary();


	public void InitSuperLifeRefTable()
	{
		pendings++;
		superLifeRefTable.Clear();
		AssetMng.instance.LoadAsset<SuperLifeRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "Superlife" + extension, (x, y) =>
			{
				pendings--;
				if (y == EResult.Success)
				{
					for (int i = 0; i < x.infoList.Count; i++)
					{
						x.infoList[i].SetAttr();
						superLifeRefTable[x.infoList[i].id] = x.infoList[i];
					}
				}
			});
	}


	public SuperLifeRef GetSuperLifeRef(int _id)
	{
		if (!superLifeRefTable.ContainsKey(_id))
		{ 
			return null;
		}
		return superLifeRefTable[_id] as SuperLifeRef;
	}
	#endregion


    #region 飞升静态表配置 by zsy
    protected FDictionary flyUpRefTable = new FDictionary();


    public void InitFlyUpRefTable()
    {
        pendings++;
        flyUpRefTable.Clear();
        AssetMng.instance.LoadAsset<FlyUpRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "feiSheng" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    flyUpRefTable[x.infoList[i].jingJieId] = x.infoList[i];
                } 
            }
        });
    }


    public FlyUpRef GetFlyUpRef(int _id)
    {
        if (!flyUpRefTable.ContainsKey(_id))
        {
            GameSys.LogError("flyUpRefTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return flyUpRefTable[_id] as FlyUpRef;
    }

    #endregion

    #region 修行静态表配置 by zsy
    protected FDictionary styliteRefTable = new FDictionary();
    public void InitStyliteRefTable()
    {
        pendings++;
        styliteRefTable.Clear();
        AssetMng.instance.LoadAsset<StyliteRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "xiuXing" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    x.infoList[i].SetAttr();
                    styliteRefTable[x.infoList[i].id] = x.infoList[i];
                } 
            }
        });
    }
     
    public StyliteRef GetStyliteRefByStart(int _id)
    {
        if (!styliteRefTable.ContainsKey(_id))
        {
            GameSys.LogError("styliteRefTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return styliteRefTable[_id] as StyliteRef;
    }

    public FDictionary GetStyliteRefTable() {
        return styliteRefTable;
    }
    #endregion


    #region 修行消耗静态表配置 by zsy
    protected FDictionary styliteMoneyRefTable = new FDictionary();

    public void InitStyliteMoneyRefTable()
    {
        pendings++;
        styliteMoneyRefTable.Clear();
        AssetMng.instance.LoadAsset<StyliteMoneyRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "xiuXingMoney" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    styliteMoneyRefTable[x.infoList[i].time] = x.infoList[i];
                }  
            }
        });
    }

    public StyliteMoneyRef GetStyliteMoneyRef(int _id)
    {
        if (!styliteMoneyRefTable.ContainsKey(_id))
        {
            GameSys.LogError("styliteMoneyRefTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return styliteMoneyRefTable[_id] as StyliteMoneyRef;
    }

    public FDictionary GetStyliteMoneyRefTable()
    {
        return styliteMoneyRefTable;
    }
    #endregion

    #region 技能BUFF静态配置数据 by龙英杰

    protected FDictionary skillBuffRefTable = new FDictionary();


    public void InitSkillBuffRefTable()
    {
        pendings++;
        skillBuffRefTable.Clear();
        AssetMng.instance.LoadAsset<SkillBuffRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "buffConfig" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    skillBuffRefTable[x.infoList[i].id] = x.infoList[i];
                }
            }
        });
    }


    public SkillBuffRef GetSkillBuffRef(int _id)
    {
        if (!skillBuffRefTable.ContainsKey(_id))
        {
            GameSys.LogError("SkillBuffRefTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return skillBuffRefTable[_id] as SkillBuffRef;
    }


    #endregion

    #region 技能等级配置 by吴江
    protected FDictionary skillLvDataRefTable = new FDictionary();


    public void InitSkillLvDataRefTable()
    {
        pendings++;
        skillLvDataRefTable.Clear();
        AssetMng.instance.LoadAsset<SkillLvDataRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "skillLevelDataConfig" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    if (!skillLvDataRefTable.ContainsKey(x.infoList[i].skillId))
                    {
                        skillLvDataRefTable[x.infoList[i].skillId] = new FDictionary();
                    }
                    FDictionary dic = skillLvDataRefTable[x.infoList[i].skillId] as FDictionary;
                    dic[x.infoList[i].skillLv] = x.infoList[i];
                }
            }
        });
    }


    public SkillLvDataRef GetSkillLvDataRef(int _skillID, int _level)
    {
        if (!skillLvDataRefTable.ContainsKey(_skillID))
        {
            GameSys.LogError("skillLvDataRefTable " + ConfigMng.Instance.GetUItext(204) + _skillID + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        FDictionary dic = skillLvDataRefTable[_skillID] as FDictionary;
        if (dic != null)
        {
            return dic[_level] as SkillLvDataRef;
        }
        GameSys.LogError("skillLvDataRefTable "+ConfigMng.Instance.GetUItext(204) + _skillID + " , " + _level + ConfigMng.Instance.GetUItext(202));
        return null;
    }
    #endregion

	#region 仙盟技能配置 by黄洪兴
	protected FDictionary guildSkillTable = new FDictionary();
	public void InitGuildSkillRefTable()
	{
		pendings++;
		guildSkillTable.Clear();
		AssetMng.instance.LoadAsset<GuildSkillRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "Guildskill" + extension, (x, y) =>
			{
				pendings--;
				if (y == EResult.Success)
				{
                    for (int i = 0; i < x.infoList.Count; i++)
                    {
                        guildSkillTable[x.infoList[i].id] = x.infoList[i];
                    }
				}
			});
	}
	public GuildSkillRef GetGuildSkillRef(int _id)
	{
		if (guildSkillTable.ContainsKey(_id))
		{
			GuildSkillRef data = guildSkillTable[_id] as GuildSkillRef;
			return data;
		}
		return null;
	}
	#endregion



	#region 仙盟商店配置 by黄洪兴
	protected FDictionary guildShopRefTable = new FDictionary();
	public void InitGuildShopRefTable()
	{
		pendings++;
		guildShopRefTable.Clear();
		AssetMng.instance.LoadAsset<GuildShopRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "Guildshop" + extension, (x, y) =>
			{
				pendings--;
				if (y == EResult.Success)
				{
                    for (int i = 0; i < x.infoList.Count; i++)
                    {
                        guildShopRefTable[x.infoList[i].id] = x.infoList[i];
                        AllGuildShopItem[x.infoList[i].id] = x.infoList[i];
                    }
				}
			});
	}
	public Dictionary<int,GuildShopRef> AllGuildShopItem=new Dictionary<int, GuildShopRef>();
	public GuildShopRef GetGuildShopRef(int _id)
	{
		if (guildShopRefTable.ContainsKey(_id))
		{
			GuildShopRef data = guildShopRefTable[_id] as GuildShopRef;
			return data;
		}
		return null;
	}
	#endregion


	#region 商店配置 by黄洪兴
	protected FDictionary shopRefTable = new FDictionary();
	public void InitShopRefTable()
	{
		pendings++;
		shopRefTable.Clear();
		AssetMng.instance.LoadAsset<ShopRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "ShopConfig" + extension, (x, y) =>
			{
				pendings--;
				if (y == EResult.Success)
				{
                    for (int i = 0; i < x.infoList.Count; i++)
                    {
                        shopRefTable[x.infoList[i].id] = x.infoList[i];
                        AllShopItem[x.infoList[i].id] = x.infoList[i];
                    }
				}
			});
	}

	public Dictionary<int,ShopRef> AllShopItem=new Dictionary<int, ShopRef>();
	public ShopRef GetShopRef(int _id)
	{
		if (shopRefTable.ContainsKey(_id))
		{
			ShopRef data = shopRefTable[_id] as ShopRef;
			return data;
		}
		return null;
	}
	#endregion

	#region 市场 by黄洪兴
	protected FDictionary marketTypeRefTable = new FDictionary();
	public void InitMarketTypeRefTable()
	{
		pendings++;
		marketTypeRefTable.Clear();
		AssetMng.instance.LoadAsset<MarketRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "MarketConfig" + extension, (x, y) =>
			{
				pendings--;
				if (y == EResult.Success)
				{
                    for (int i = 0; i < x.infoList.Count; i++)
                    {
                        marketTypeRefTable[x.infoList[i].id] = x.infoList[i];
                        AllMarketTypeItem[x.infoList[i].id] = x.infoList[i];
                    }
				}
			});
	}

	public Dictionary<int,MarketRef> AllMarketTypeItem=new Dictionary<int, MarketRef>();
	public MarketRef GetMarketTypeRef(int _id)
	{
		if (marketTypeRefTable.ContainsKey(_id))
		{
			MarketRef data = marketTypeRefTable[_id] as MarketRef;
			return data;
		}
		return null;
	}
	#endregion

	#region 在线奖励 by黄洪兴
	protected FDictionary onlineRewardRefTable = new FDictionary();
    protected List<OnlineRewardRef> allOnlineRewardRef=new List<OnlineRewardRef>();
	public void InitOnlineRewardRefTable()
	{
		pendings++;
		onlineRewardRefTable.Clear();
		AssetMng.instance.LoadAsset<OnlineRewardRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "onlineReward" + extension, (x, y) =>
			{
				pendings--;
				if (y == EResult.Success)
				{
                    for (int i = 0; i < x.infoList.Count; i++)
                    {
                        onlineRewardRefTable[x.infoList[i].id] = x.infoList[i];
                        allOnlineRewardRef.Add(x.infoList[i]);
                    }
				}
			});
	}

	public OnlineRewardRef GetOnlineRewardRef(int _id)
	{
		if (onlineRewardRefTable.ContainsKey(_id))
		{
			OnlineRewardRef data = onlineRewardRefTable[_id] as OnlineRewardRef;
			return data;
		}
		return null;
	}

    public List<OnlineRewardRef> GetAllOnlineRewardRef()
    {

        return allOnlineRewardRef;

    }


	#endregion

    #region 小助手 by黄洪兴
    protected FDictionary littleHelperRefTable = new FDictionary();
    protected FDictionary littleHelperTypeRefTable = new FDictionary();
    public void InitLittleHelperRefTable()
    {
        pendings++;
        littleHelperRefTable.Clear();
        AssetMng.instance.LoadAsset<LittleHelperRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "assistant" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
//               int t = 0;

                for (int i = 0; i < x.infoList.Count; i++)
                {
                    littleHelperRefTable[x.infoList[i].id] = x.infoList[i];
                    if (LittleHelperDic.ContainsKey(x.infoList[i].type))
                    {
                        LittleHelperDic[x.infoList[i].type].Add(x.infoList[i]);
                    }
                    else
                    {
                        LittleHelperDic[x.infoList[i].type] = new List<LittleHelperRef>();
                        LittleHelperDic[x.infoList[i].type].Add(x.infoList[i]);
                    }
                }

            }
        });
    }

    public void InitLittleHelperTypeRefTable()
    {
        pendings++;
        littleHelperTypeRefTable.Clear();
        AssetMng.instance.LoadAsset<LittleHelperTypeRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "assistantType" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {

                for (int i = 0; i < x.infoList.Count; i++)
                {
                    littleHelperTypeRefTable[x.infoList[i].id] = x.infoList[i];
                    LittleHelperTypeDic[x.infoList[i].type] = x.infoList[i];
                }

            }
        });
    }
   Dictionary<int, LittleHelperTypeRef> LittleHelperTypeDic = new Dictionary<int, LittleHelperTypeRef>();
   Dictionary<int, List<LittleHelperRef>> LittleHelperDic = new Dictionary<int, List<LittleHelperRef>>();
   public Dictionary<int, List<LittleHelperRef>> GetLittleHelperDic()
    {
        Dictionary<int, List<LittleHelperRef>> Dic = new Dictionary<int, List<LittleHelperRef>>();
        using (var e = LittleHelperDic.GetEnumerator())
        {
            while (e.MoveNext())
            {
                Dic[e.Current.Key] = e.Current.Value;
            }
        }
        return Dic;
    }
   public Dictionary<int, LittleHelperTypeRef> GetLittleHelperTypeDic()
   {
       Dictionary<int, LittleHelperTypeRef> Dic = new Dictionary<int, LittleHelperTypeRef>();
       using (var e = LittleHelperTypeDic.GetEnumerator())
       {
           while(e.MoveNext())
           {
               Dic[e.Current.Key] = e.Current.Value;
           }
       }
       return Dic;
   }
    #endregion

    #region 读条 by黄洪兴
    protected FDictionary sustainRefTable = new FDictionary();
    public void InitSustainRefTable()
    {
        pendings++;
        sustainRefTable.Clear();
        AssetMng.instance.LoadAsset<SustainRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "sustain" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    sustainRefTable[x.infoList[i].id] = x.infoList[i];
                }
            }
        });
    }

    public SustainRef GetSustainRef(int _id)
    {
        if (sustainRefTable.ContainsKey(_id))
        {
            SustainRef data = sustainRefTable[_id] as SustainRef;
            return data;
        }
        return null;
    }
    #endregion





    #region 商城配置 by 何明军
    protected FDictionary mallRefTable = new FDictionary();
    public void InitMallRefTable()
    {
        pendings++;
        mallRefTable.Clear();
		AssetMng.instance.LoadAsset<MallRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "MallConfig" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
					for (int i = 0; i < x.infoList.Count; i++)
					{
						mallRefTable[x.infoList[i].id] = x.infoList[i];
						AllMallItem[x.infoList[i].id]=x.infoList[i];
						MallItemByEquipmentID[x.infoList[i].item] = x.infoList[i];
					}
            }
        });
    }

    public Dictionary<int, MallRef> MallItemByEquipmentID = new Dictionary<int, MallRef>();
	public Dictionary<int,MallRef> AllMallItem=new Dictionary<int, MallRef>();
	public MallRef GetMallRef(int _id)
    {
        if (mallRefTable.ContainsKey(_id))
        {
			MallRef data = mallRefTable[_id] as MallRef;
            return data;
        }
        return null;
    }
    public MallRef GetMallRefByEID(int _eid)
    {
        foreach (MallRef mallRef in mallRefTable.Values)
        {
            if (mallRef.item == _eid)
                return mallRef;
        }
        Debug.LogError("商城中没有_eid：" + _eid + "的物品购买");
        return null;
    }
    #endregion

    #region 服务端提示表 add by 易睿
    protected FDictionary serverMSGRefTable = new FDictionary();//何明军

    /// <summary>
    /// 初始化数据
    /// Inits the server message reference table.
    /// </summary>
    public void InitServerMSGRefTable()
    {
        pendings++;
        serverMSGRefTable.Clear();
        AssetMng.instance.LoadAsset<ServerMSGRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "ErrorCodeConfig" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    serverMSGRefTable[x.infoList[i].id] = x.infoList[i];
                }
            }

        });
    }
    /// <summary>
    /// 获取数据
    /// Gets the server message reference.
    /// </summary>
    /// <returns>
    /// The server message reference.
    /// </returns>
    /// <param name='_id'>
    /// _id.
    /// </param>
    public ServerMSGRef GetServerMSGRef(int _id)
    {
        if (!serverMSGRefTable.ContainsKey(_id))
        {
            GameSys.LogError("ServerMSGRefTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return serverMSGRefTable[_id] as ServerMSGRef;
    }
    #endregion

    #region 开启新功能表配置 by 何明军
	protected FDictionary openNewFunctionRefTable = new FDictionary();
	public void InitOpenNewFunctionRefTable()
    {
        pendings++;
		openNewFunctionRefTable.Clear();
		AssetMng.instance.LoadAsset<OpenNewFunctionRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "NewFunctionOpenConfig" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
					for (int i = 0; i < x.infoList.Count; i++)
					{
						openNewFunctionRefTable[x.infoList[i].id] = x.infoList[i];
					}
            }

        });
    }
	public OpenNewFunctionRef GetOpenNewFunctionRef(int _id)
	{
		if (!openNewFunctionRefTable.ContainsKey(_id))
		{
            GameSys.LogError("openNewFunctionRefTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
			return null;
		}
		return openNewFunctionRefTable[_id] as OpenNewFunctionRef;
	}
	
	public OpenNewFunctionRef GetOpenNewFunctionRef(int _funcType,int _prof)
    {
		foreach(OpenNewFunctionRef data in openNewFunctionRefTable.Values){
			if(data.prof > 0 && data.prof != _prof)continue;
			if(data.func_type == _funcType){
				return data;
			}
		}
		return null;
    }

	public FDictionary OpenNewFunctionRefTable()
    {
		return openNewFunctionRefTable;
    }
    #endregion
	
	#region  指引静态表配置 by 何明军
	protected FDictionary openNewFunctionGuideRefTable = new FDictionary();
	public void InitOpenNewFunctionGuideRefTable()
	{
		pendings++;
		openNewFunctionGuideRefTable.Clear();
		AssetMng.instance.LoadAsset<OpenNewFunctionGuideRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "GuideConfig" + extension, (x, y) =>
			{
				pendings--;
				if (y == EResult.Success)
				{
					for (int i = 0; i < x.infoList.Count; i++)
					{
						openNewFunctionGuideRefTable[x.infoList[i].id] = x.infoList[i];
					}
				}

			});
	}
	
	public OpenNewFunctionGuideRef GetOpenNewFunctionGuideRef(int _type,int _step)
	{
		foreach(OpenNewFunctionGuideRef data in openNewFunctionGuideRefTable.Values){
			if(_type == data.type && _step == data.step){
				return data;
			}
		}
//		Debug.LogError("新手引导表木有type = "+_type+"，step = "+_step+"的数据！");
		return null;
	}

	public OpenNewFunctionGuideRef GetOpenNewFunctionGuideRef(int _id)
	{
		if(openNewFunctionGuideRefTable.ContainsKey(_id)){
			return openNewFunctionGuideRefTable[_id] as OpenNewFunctionGuideRef;
		}
//		Debug.LogError("新手引导表木有ID = "+_id+"的数据！");
		return null;
	}
	#endregion
	
	#region  系统消息静态配置 by 黄洪兴
	protected FDictionary chatTemplatesRefTable = new FDictionary();
	public void InitChatTemplatesRefTable()
	{
		pendings++;
		chatTemplatesRefTable.Clear();
		AssetMng.instance.LoadAsset<ChatTemplatesRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "ChatTemplates" + extension, (x, y) =>
			{
				pendings--;
				if (y == EResult.Success)
				{
                    for (int i = 0; i < x.infoList.Count; i++)
                    {
                        chatTemplatesRefTable[x.infoList[i].id] = x.infoList[i];
                    }
				}

			});
	}
	public ChatTemplatesRef GetChatTemplatesRef(int _id)
	{
		if (!chatTemplatesRefTable.ContainsKey(_id))
		{
            GameSys.LogError("chatTemplatesRefTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
			return null;
		}
		return chatTemplatesRefTable[_id] as ChatTemplatesRef;
	}
	#endregion

    #region 特殊字段配置 by朱素云
    protected FDictionary specialRefTable = new FDictionary();
    public void InitSpecialRefTable()
    {
        pendings++;
        specialRefTable.Clear();
        AssetMng.instance.LoadAsset<SpecialRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "special" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    specialRefTable[x.infoList[i].id] = x.infoList[i];
                } 
            }
        });
    }


    public SpecialRef GetSpecialRef(int _id)
    {
        if (!specialRefTable.ContainsKey(_id))
        {
            GameSys.LogError("specialRefTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
            return null; 
        }
        return specialRefTable[_id] as SpecialRef;
    }
    #endregion

    #region 结义静态表配置 by 朱素云
    protected FDictionary swornRefTable = new FDictionary();

    public void InitSwornRefTable()
    {
        pendings++;
        swornRefTable.Clear();
        AssetMng.instance.LoadAsset<SwornRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "Sworn" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    swornRefTable[x.infoList[i].id] = x.infoList[i];
                } 
            }
        });
    }


    public SwornRef GetSwornRef(int _id)
    {
        if (!swornRefTable.ContainsKey(_id))
        {
            GameSys.LogError("swornRefTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return swornRefTable[_id] as SwornRef;
    }
    #endregion


    #region 星星类型静态配置 by 朱素云
    protected FDictionary starTypeRefTable = new FDictionary();

    public void InitStarTypeRefTable()
    {
        pendings++;
        starTypeRefTable.Clear();
        AssetMng.instance.LoadAsset<StarTypeRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "starType" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    starTypeRefTable[x.infoList[i].level] = x.infoList[i];
                } 
            }
        });
    }


    public StarTypeRef GetStarTypeRef(int _id)
    {
        if (!starTypeRefTable.ContainsKey(_id))
        {
            GameSys.LogError("starTypeRefTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return starTypeRefTable[_id] as StarTypeRef;
    }
    #endregion
    //===================================T=============================
    #region 任务配置表
    /// <summary>
    /// 以任务编号为key的所有任务的字典
    /// </summary>
    protected FDictionary taskConfigTable = new FDictionary();

    /// <summary>
    /// 以起始npcID为key的起始任务字典
    /// </summary>
    protected FDictionary startTaskDic = new FDictionary();

    /// <summary>
    /// 每日任务列表
    /// </summary>
    protected FDictionary dailyTaskRefList = new FDictionary();

    /// <summary>
    /// 加载和初始化任务静态配置
    /// </summary>
    public void InitTaskConfigTable()
    {
        pendings++;
        taskConfigTable.Clear();
        dailyTaskRefList.Clear();
        startTaskDic.Clear();
        AssetMng.instance.LoadAsset<TaskConfigRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "TaskConfig" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    x.infoList[i].InitData();
                    taskConfigTable[x.infoList[i].task] = x.infoList[i];

                    //foreach (var itemRef in x.infoList[i].stepList)
                    //{
                    //    if (itemRef.sort == (TaskType)5)
                    //    {
                    //        dailyTaskRefList[itemRef.task] = itemRef;
                    //    }
                    //}
                    for (int j = 0; j < x.infoList[i].stepList.Count; j++)
                    {
                        if (x.infoList[i].stepList[j].sort == (TaskType)5)
                        {
                            dailyTaskRefList[x.infoList[i].stepList[j].task] = x.infoList[i].stepList[j];
                        }
                    }
                    TaskStepRef start = x.infoList[i].GetTaskStepRef(1);
                    if (start != null)
                    {
                        int npcID = start.takeFromNpc.npcID;
                        if (!startTaskDic.ContainsKey(npcID))
                        {
                            startTaskDic[npcID] = new List<TaskStepRef>();
                        }
                        List<TaskStepRef> list = startTaskDic[npcID] as List<TaskStepRef>;
                        list.Add(start);
                    }
                }
            }

        });
    }


    /// <summary>
    /// 获取指定任务静态配置
    /// </summary>
    /// <param name="_task"></param>
    /// <returns></returns>
    public TaskConfigRef GetTaskConfigRef(int _task)
    {
        if (!taskConfigTable.ContainsKey(_task))
        {
            GameSys.LogError("taskConfigTable " + ConfigMng.Instance.GetUItext(204) + _task + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return taskConfigTable[_task] as TaskConfigRef;
    }

    /// <summary>
    ///  获取指定任务，指定步骤的静态配置
    /// </summary>
    /// <param name="_task">任务id</param>
    /// <param name="_step">步骤</param>
    /// <returns></returns>
    public TaskStepRef GetTaskStepRef(int _task, int _step)
    {
        if (!taskConfigTable.ContainsKey(_task))
        {
            GameSys.LogError("taskConfigTable " + ConfigMng.Instance.GetUItext(204) + _task + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        TaskConfigRef refData = taskConfigTable[_task] as TaskConfigRef;
        if (refData == null) return null;
        return refData.GetTaskStepRef(_step);
    }

    /// <summary>
    /// 根据NPCID得到该npc身上所有起始任务的key列表 by吴江
    /// </summary>
    /// <param name="_npcID"></param>
    /// <returns></returns>
    public List<TaskStepRef> GetStartTaskListFromNPC(int _npcID)
    {
        if (!startTaskDic.ContainsKey(_npcID))
        {
            return null;
        }
        return startTaskDic[_npcID] as List<TaskStepRef>;
    }

    /// <summary>
    /// 每日任务列表
    /// </summary>
    public FDictionary GetDailyTaskRefList()
    {
        return dailyTaskRefList;
    }

    #endregion

    #region 环任务奖励静态配置
    protected FDictionary taskRingRewardRefTable = new FDictionary();
    public void InitTaskRingRewardRefTable()
    {
        pendings++;
        taskRingRewardRefTable.Clear();
        AssetMng.instance.LoadAsset<TaskRingRewardRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "task_ring_reward" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    taskRingRewardRefTable[x.infoList[i].difficuilty] = x.infoList[i]; 
                }
            }
        });
    }
    public TaskRingRewardRef GetTaskRingRewardRef(int _id)
    {
        if (!taskRingRewardRefTable.ContainsKey(_id))
        {
            GameSys.LogError("rewardGroupRefTable中找不到索引为" + _id + "的配置");
            return null;
        }
        return taskRingRewardRefTable[_id] as TaskRingRewardRef;
    }
    #endregion

    #region 陷阱配置表
    protected FDictionary trapRefTable = new FDictionary();

    /// <summary>
    /// 初始化数据
    /// </summary>
    public void InitTrapRefTable()
    {
        pendings++;
        trapRefTable.Clear();
        AssetMng.instance.LoadAsset<TrapRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "TrapConfig" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    trapRefTable[x.infoList[i].trapId] = x.infoList[i];
                }
            }

        });
    }
    /// <summary>
    /// 获取数据
    /// </param>
    public TrapRef GetTrapRef(int _id)
    {
        if (!trapRefTable.ContainsKey(_id))
        {
            GameSys.LogError("trapRefTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return trapRefTable[_id] as TrapRef;
    }
    #endregion

    #region 称号表配置 by 黄洪兴
    protected FDictionary titleRefTable = new FDictionary();
    public void InitTitleRefTable()
    {
        pendings++;
        titleRefTable.Clear();
        AssetMng.instance.LoadAsset<TitleRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "titles" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    titleRefTable[x.infoList[i].type] = x.infoList[i];
                }
            }

        });
    }
    public TitleRef GetTitlesRef(int _id)
    {
        if (!titleRefTable.ContainsKey(_id))
        {
            GameSys.LogError("titleRefTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return titleRefTable[_id] as TitleRef;
    }

    public List<TitleRef> TitlesList()
    {
        List<TitleRef> list = new List<TitleRef>();
        foreach (var item in titleRefTable.Values)
        {
            list.Add(item as TitleRef);
        }
        return list;
    }
    #endregion

    #region 称号类型静态配置 by 朱素云
    protected FDictionary titleTypeRefTable = new FDictionary();

    public void InitTitleTypeRefTable()
    {
        pendings++;
        titleTypeRefTable.Clear();
        AssetMng.instance.LoadAsset<TitleTypeRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "titles2" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    titleTypeRefTable[x.infoList[i].type] = x.infoList[i];
                } 
            }

        });
    }
    /// <summary>
    /// 根据称号类型获取称号链表
    /// </summary> 
    public List<int> GetTitleListByType(int _id)
    {
        if (!titleTypeRefTable.ContainsKey(_id))
        {
            GameSys.LogError("titleRefTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        TitleTypeRef type = titleTypeRefTable[_id] as TitleTypeRef;
        return type.id;
    }
     
    #endregion

    #region 小提示配置

    List<TipsRef> tipsRefTable = new List<TipsRef>();

    public void InitTipsRefTable()
    {
        pendings++;
        tipsRefTable.Clear();
        AssetMng.instance.LoadAsset<TipsRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "tips" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    tipsRefTable.Add(x.infoList[i]);
                }
            }
        });
    }
    public string GetOnCreateTip()
    {
        if (tipsRefTable == null || tipsRefTable.Count == 0) return null;
        return tipsRefTable[0].tips;
    }
    public string GetRandomTipsRef()
    {
        if (tipsRefTable == null || tipsRefTable.Count == 0) return null;
        int count = tipsRefTable.Count - 1;
        System.Random randomer = new System.Random();
        int index = randomer.Next(0, count);
        return tipsRefTable[index].tips;
    }
    #endregion
    
    #region 仙侣信物静态配置表 by zsy
    protected FDictionary tokenLevRefTable = new FDictionary();

    public void InitTokenLevRefTable()
    {
        pendings++;
        tokenLevRefTable.Clear();
        AssetMng.instance.LoadAsset<TokenLevRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "token_lev" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    tokenLevRefTable[x.infoList[i].id] = x.infoList[i]; 
                }
            }

        });
    }
    /// <summary>
    /// 信物等级作为索引
    /// </summary>
    public TokenLevRef GetTokenLevRef(int _id)
    {
        if (!tokenLevRefTable.ContainsKey(_id))
        {
            GameSys.LogError("tokenLevRefTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return tokenLevRefTable[_id] as TokenLevRef;
    }
    #endregion 


    #region 藏宝阁
    protected FDictionary treasureHouseRefTable = new FDictionary();
    public void InitTreasureHouseRefTable()
    {
        pendings++;
        treasureHouseRefTable.Clear();
        AssetMng.instance.LoadAsset<TreasureHouseRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "TreasureHouse" + extension, (x, y) =>
            {
                pendings--;
                if (y == EResult.Success)
                {
                    for (int i = 0; i < x.infoList.Count; i++)
                    {
                        treasureHouseRefTable[x.infoList[i].chest] = x.infoList[i];
                    }
                }
            });
    }
    public TreasureHouseRef GetTreasureHouseRef(int _id)
    {
        if (!treasureHouseRefTable.ContainsKey(_id))
        {
            GameSys.LogError("treasureHouseRefTable" + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return treasureHouseRefTable[_id] as TreasureHouseRef;
    }
    public FDictionary GetTreasureHouseRefTable()
    {
        return treasureHouseRefTable;
    }
    #endregion

    #region 藏宝阁预览配置
    protected FDictionary rewardGroupRefTable = new FDictionary();
    public void InitRewardGroudRefTable()
    {
        pendings++;
        rewardGroupRefTable.Clear();
        AssetMng.instance.LoadAsset<RewardGroupRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "RewardGroup" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    rewardGroupRefTable[x.infoList[i].id] = x.infoList[i];
                }
            }
        });
    }
    public RewardGroupRef GetRewardGroupRef(int _id)
    {
        if (!rewardGroupRefTable.ContainsKey(_id))
        {
            GameSys.LogError("rewardGroupRefTable" + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return rewardGroupRefTable[_id] as RewardGroupRef;
    }

    public FDictionary GetRewardGroupRefTable()
    {
        FDictionary dic = new FDictionary();
        foreach (RewardGroupRef data in rewardGroupRefTable.Values)
        {
            if (GameCenter.mainPlayerMng != null && (data.occupation == 0 || GameCenter.mainPlayerMng.MainPlayerInfo.Prof == data.occupation))
            {
                dic[data.id] = data;
            }
        }
        return dic;
    }
    #endregion

    #region 藏宝阁奖励成员配置
    protected FDictionary rewardGroupMemberRefTable = new FDictionary();
    public void InitRewardGroudMemberRefTable()
    {
        pendings++;
        rewardGroupMemberRefTable.Clear();
        AssetMng.instance.LoadAsset<RewardGroupMemberRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "RewardGroupMember" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    rewardGroupMemberRefTable[x.infoList[i].id] = x.infoList[i];
                }
            }
        });
    }
    public RewardGroupMemberRef GetRewardGroupMemberRef(int _id)
    {
        if (!rewardGroupMemberRefTable.ContainsKey(_id))
        {
            GameSys.LogError("rewardGroupRefTable" + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return rewardGroupMemberRefTable[_id] as RewardGroupMemberRef;
    }
    public FDictionary GetRewardGroupMemberRefTable()
    {
        return rewardGroupMemberRefTable;
    }
    #endregion
    //===================================U=============================
    #region UI层级配置表 by吴江
    protected FDictionary uiLevelConfigRefTable = new FDictionary();
    protected FDictionary subTypeBelongTable = new FDictionary();


    public void InitUILevelConfigRefTable()
    {
        pendings++;
        uiLevelConfigRefTable.Clear();
        AssetMng.instance.LoadAsset<UILevelConfigRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "UILevelConfig" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    UILevelConfigRef refData = x.infoList[i];
                    uiLevelConfigRefTable[refData.levelOneUI] = refData;
                    if (refData.levelTwoUIGroup.Count > 0)
                    {
                        for (int j = 0; j < refData.levelTwoUIGroup.Count; j++)
                        {
                            subTypeBelongTable[refData.levelTwoUIGroup[j]] = refData.levelOneUI;
                        }
                    }
                    
                }
            }

        });
    }


    public UILevelConfigRef GetUILevelConfigRef(GUIType _type)
    {
        if (!uiLevelConfigRefTable.ContainsKey(_type))
        {
            GameSys.LogError("uiLevelConfigRefTable " + ConfigMng.Instance.GetUItext(204) + _type + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return uiLevelConfigRefTable[_type] as UILevelConfigRef;
    }

    public GUIType GetSubGUITypeBelong(SubGUIType _subType)
    {
        if (subTypeBelongTable.ContainsKey(_subType))
        {
            return (GUIType)subTypeBelongTable[_subType];
        }
        return GUIType.NONE;
    }
    #endregion

    #region UI跳转配置 by 朱素云

    protected FDictionary uiSkipRefTable = new FDictionary();
    public void InitUISkipRefTable()
    {
        pendings++;
        uiSkipRefTable.Clear();
        AssetMng.instance.LoadAsset<UISkipRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "UIJump" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                { 
                    uiSkipRefTable[x.infoList[i].id] = x.infoList[i]; 
                }
            }
        });
    }
    public UISkipRef GetUISkipRef(int _id)
    {
        if (!uiSkipRefTable.ContainsKey(_id))
        {
            GameSys.LogError("uiSkipRefTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return uiSkipRefTable[_id] as UISkipRef;
    }
    #endregion

    //===================================V====================da=========
	#region VIP by 何明军

	protected FDictionary vIPRefTable = new FDictionary();
	public void InitVIPRefTable()
	{
		pendings++;
		vIPRefTable.Clear();
		AssetMng.instance.LoadAsset<VIPRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "VIPConfig" + extension, (x, y) =>
			{
				pendings--;
				if (y == EResult.Success)
				{
					for (int i = 0; i < x.infoList.Count; i++)
					{
						x.infoList[i].SetItems();
						vIPRefTable[x.infoList[i].id] = x.infoList[i];
					}
				}
			});
	}
	public VIPRef GetVIPRef(int _id)
	{
		if (!vIPRefTable.ContainsKey(_id))
		{
            GameSys.LogError("vIPRefTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
			return null;
		}
		return vIPRefTable[_id] as VIPRef;
	}
	public FDictionary GetVIPRefTable()
	{
        FDictionary Dic = new FDictionary();
        foreach (var item in vIPRefTable.Keys)
        {
            Dic[item] = vIPRefTable[item];
        }
        return Dic;
	}
    /// <summary>
    /// 根据当前vip等级获取增加无尽次数的等级
    /// </summary>
    /// <param name="_id"></param>
    /// <returns></returns>
    public int GetEndlessNumVIPLevByCurVip()
    {
        int curnum = 0;
        if (GameCenter.vipMng.VipData != null)
        {
            curnum = GetVIPRef(GameCenter.vipMng.VipData.vLev).endlessNum;
        }
        foreach (VIPRef item in vIPRefTable.Values)
        {
            if (item.endlessNum > curnum)
            {
                return item.id;
            }
        }
        return 0;
    }

    /// <summary>
    /// 获得经验比例 by黄洪兴
    /// </summary>
    /// <param name="_exp"></param>
    /// <returns></returns>
    public List<int> GetVipExpTarget(int _exp)
    {
        int allExp = 0;
        int curExp = _exp;
        List<int> list=new List<int>();
        for (int i = 0; i < vIPRefTable.Count; i++)
        {
            if(GetVIPRef(i + 1)==null)
                return list;
            allExp += GetVIPRef(i + 1).exp;
            if (allExp > _exp)
            {
                list.Add(curExp);
                list.Add(GetVIPRef(i + 1).exp);
                return list;
            }
            curExp -= GetVIPRef(i + 1).exp;
        }
            if(GetVIPRef(vIPRefTable.Count)==null)
                return list;
        list.Add(GetVIPRef(vIPRefTable.Count).exp);
        list.Add(GetVIPRef(vIPRefTable.Count).exp);
        return list;
    }

	#endregion
	
	#region UItext by 何明军

	protected FDictionary uITextRefTable = new FDictionary();
	public void InitUITextRefTable()
	{
		pendings++;
		uITextRefTable.Clear();
		AssetMng.instance.LoadAsset<UIwenbenRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "UIwenben" + extension, (x, y) =>
			{
				pendings--;
				if (y == EResult.Success)
				{
					for (int i = 0; i < x.infoList.Count; i++)
					{
						uITextRefTable[x.infoList[i].id] = x.infoList[i];
					}
				}
			});
	}
	public UIwenbenRef GetUITextRef(int _id)
	{
		if (!uITextRefTable.ContainsKey(_id))
		{
            GameSys.LogError("uITextRefTable 中找不到索引为" + _id + "的配置!");
			return null;
		}
		return uITextRefTable[_id] as UIwenbenRef;
	}
	public string GetUItext(int id,string[] str = null){
		if (!uITextRefTable.ContainsKey(id))
		{
            GameSys.LogError("uITextRefTable 中找不到索引为" + id + "的配置!");
			return String.Empty;
		}
		UIwenbenRef refdata = uITextRefTable[id] as UIwenbenRef;
		return UIUtil.Str2Str(refdata.text,str).Replace("\\n","\n");
	}
	public FDictionary GetUITextRefTable()
	{
		return uITextRefTable;
	}

	#endregion


    //===================================W=============================
    #region 翅膀配置 by 鲁家旗
    protected FDictionary wingRefTable = new FDictionary();
    
    public void InitWingRefTabel()
    {
        pendings++;
        wingRefTable.Clear();
        AssetMng.instance.LoadAsset<WingRefTable>(AssetMng.GetPathWithoutExtension("config/",AssetPathType.PersistentDataPath)+"wingConfig"+extension,(x,y)=>
            {
                pendings--;
                if (y == EResult.Success)
                {
                    for (int i = 0; i < x.infoList.Count; i++)
                    {
                         wingRefTable[x.infoList[i].id] = x.infoList[i];
                    }
                }
            });
    }
    public WingRefEty GetWingRef(int _id)
    {
        if (!wingRefTable.ContainsKey(_id))
        {
            GameSys.LogError("wingRefTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return wingRefTable[_id] as WingRefEty;
    }
    public WingRef GetWingRef(int _id, int _lev)
    {
        if (!wingRefTable.ContainsKey(_id))
        {
            GameSys.LogError("wingRefTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        WingRefEty data = wingRefTable[_id] as WingRefEty;
        if (data == null) return null;
        return data.getWingRef(_lev);
    }
    /// <summary>
    /// 通用翅膀数据用来创建翅膀
    /// </summary>
    public Dictionary<int, WingRef> GetWingRefTable()
    {
        Dictionary<int, WingRef> dic = new Dictionary<int, WingRef>();
        foreach (WingRefEty data in wingRefTable.Values)
        {
            if (data.lev == 1 && data.getWingRef(data.lev) != null && data.getWingRef(data.lev).type == 1)
            {
                dic.Add(data.id, data.getWingRef(data.lev));
            }
        }
        return dic;
    }
    /// <summary>
    /// 试用翅膀数据
    /// </summary>
    public WingRef GetRef()
    {
        WingRef wingRef = null;
        foreach (WingRefEty data in wingRefTable.Values)
        {
            if (data.lev == 1 && data.getWingRef(data.lev) != null && data.getWingRef(data.lev).type == 2)
            {
                wingRef = data.getWingRef(data.lev);
            }
        }
        return wingRef;
    }
    /// <summary>
    /// 翅膀开始拥有技能的等级
    /// </summary>
    /// <returns></returns>
    public int GetWingHaveSkillLev(int _id)
    {
        WingRefEty data = wingRefTable[_id] as WingRefEty;
        if (data != null)
        {
            for (int i = 0; i < data.wingRefList.Count; i++)
            {
                if (data.wingRefList[i].passivity_skill.skillid != 0)
                    return data.wingRefList[i].lev;
            }
        }
        return 0;
    }
    /// <summary>
    /// 翅膀最大等级
    /// </summary>
    public int GetWingMaxLev(int _id)
    {
        int maxLev = 1;
        WingRef wingRef = null;
        WingRefEty data = wingRefTable[_id] as WingRefEty;
        if (data != null && data.wingRefList.Count != 0)
             wingRef = data.wingRefList[data.wingRefList.Count - 1];
        if (wingRef != null)
        {
            maxLev = wingRef.lev;
        }
        return maxLev;
    }
    /// <summary>
    /// 翅膀的三个阶段的等级
    /// </summary>
    public List<int> GetWingItemLev(int _id)
    {
        List<int> list = new List<int>();
        List<int> levList = new List<int>();
        WingRefEty data = wingRefTable[_id] as WingRefEty;
        if (data != null)
        {
            for (int i = 0; i < data.wingRefList.Count; i++)
            {
                list.Add(data.wingRefList[i].itemui);
            }
            for (int i = 0; i < list.Count; i++)
            {
                if (i == 0)
                {
                    levList.Add(1);
                }
                if (i + 1 < list.Count && list[i] != list[i + 1])
                {
                    levList.Add(data.wingRefList[i + 1].lev);
                }
            }
        }
        return levList;
    }
    #endregion

    #region 仙侣静态配置表 by zsy
    protected FDictionary weddingRefTable = new FDictionary();
    
    public void InitWeddingRefTable()
    {
        pendings++;
        weddingRefTable.Clear();
        AssetMng.instance.LoadAsset<WeddingRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "Wedding_type" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    weddingRefTable[x.infoList[i].token_id] = x.infoList[i];
                } 
            }

        });
    }
    /// <summary>
    /// 信物id作为索引
    /// </summary>
    public WeddingRef GetWeddingRef(int _id)
    {
        if (!weddingRefTable.ContainsKey(_id))
        {
            GameSys.LogError("weddingRefTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return weddingRefTable[_id] as WeddingRef;
    }
    public FDictionary GetWeddingRefTable()
    { 
       return weddingRefTable; 
    }
    #endregion

    #region 周卡静态配置表 by zsy
    protected FDictionary weekCardRefTable = new FDictionary();

    public void InitWeekCardRefTable() 
    {
        pendings++;
        weekCardRefTable.Clear();
        AssetMng.instance.LoadAsset<WeekCardRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "Week_card" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    weekCardRefTable[x.infoList[i].id] = x.infoList[i];
                }
            }

        });
    } 
    public WeekCardRef GetWeekCardRef(int _id)
    {
        if (!weekCardRefTable.ContainsKey(_id))
        {
            GameSys.LogError("weekCardRefTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return weekCardRefTable[_id] as WeekCardRef;
    }
    #endregion

    #region 仙侣副本配表 by zsy
    protected FDictionary weddingCoppyRefTable = new FDictionary();

    public void InitWeddingCoppyRefTable()
    {
        pendings++;
        weddingCoppyRefTable.Clear();
        AssetMng.instance.LoadAsset<WeddingCoppyRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "Wedding_copy" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0,max = x.infoList.Count; i < max; i++)
                {
                    x.infoList[i].SetDes();
                    weddingCoppyRefTable[x.infoList[i].id] = x.infoList[i]; 
                }
            }

        });
    } 

    public WeddingCoppyRef GetWeddingCoppyRef(int _id)
    {
        if (!weddingCoppyRefTable.ContainsKey(_id))
        {
            GameSys.LogError("weddingCoppyRefTable " + ConfigMng.Instance.GetUItext(204) + _id + ConfigMng.Instance.GetUItext(202));
            return null;
        }
        return weddingCoppyRefTable[_id] as WeddingCoppyRef;
    }
    #endregion
    //===================================X=============================
    //===================================Y=============================
    //===================================Z=============================

	#region 多语言配置
	protected FDictionary uiLabelTranslationRefTable = new FDictionary();

	public void InitUILabelTranslationRefTable()
	{
		pendings++;
		uiLabelTranslationRefTable.Clear();
		AssetMng.instance.LoadAsset<UILabelTranslationRefTable>(AssetMng.GetPathWithoutExtension("Config/", AssetPathType.PersistentDataPath) + "UIText" + extension, (x, y) =>
			{
				pendings--;
				if (y == EResult.Success)
				{
					for (int i = 0; i < x.infoList.Count; i++)
					{
						uiLabelTranslationRefTable[x.infoList[i].des] = x.infoList[i];
					}
				}

			});
	}

	/// <summary>
	/// UILabel文字翻译
	/// </summary>
	/// <param name="_des"></param>
	/// <returns></returns>
	public UILabelTranslationProp GetUILabelTranslationRef(string _des)
	{
		if (!uiLabelTranslationRefTable.ContainsKey(_des))
		{
			//Debug.LogWarning("uiLabelTranslationRefTable 中找不到索引为" + _des + "的配置!");
			return null;
		}
		return uiLabelTranslationRefTable[_des] as UILabelTranslationProp;
	}

	#endregion


    #region 文本配置 by吴江
    protected string fontArray = string.Empty;

    public void InitFontArray()
    {
        TextAsset str = exResources.GetResource(ResourceType.FONT, "MSYH_LTT/Font_txt") as TextAsset;
        if (str != null)
        {
            fontArray = str.text;
            str = null;
        }
        else
        {
            GameSys.LogError(ConfigMng.Instance.GetUItext(210));
        }

    }

    /// <summary>
    /// 字符集中是否包含该文字. 如果不包含,在UI上可能表现为空!
	/// 替换成"■"//
    /// </summary>
    /// <param name="_str"></param>
    /// <returns></returns>
    public string ContainsChar(string _str)
    {
        int count = _str.Length;
//        if (count > 1)
//        {
            for (int i = 0; i < count; i++)
            {
				if (!fontArray.Contains(_str[i].ToString())) {
					_str = _str.Replace(_str[i].ToString(),"■");
				}
            }
			return _str;
//        }
//        return fontArray.Contains(_str);
    }
	/// <summary>
	/// 判断输入的字符串中的所有字都在字集中  
	/// </summary>
	public bool ContainsAllChar(string _str)
	{
		int count = _str.Length;
		while(count > 0)
		{
			count--;
			if (!fontArray.Contains(_str[count].ToString())) 
				return false;
		}
		return true;
	}
    /// <summary>
    /// 字符集中是否包含该文字. 如果不包含,在UI上可能表现为空!
    /// </summary>
    /// <param name="_str"></param>
    /// <returns></returns>
    public bool ContainsChar(char _str)
    {
        return fontArray.Contains(_str.ToString());
    }
    #endregion

	#region 强化属性配置 by邓成
	protected FDictionary strengthAttrTable = new FDictionary();
	public void InitStrengthAttrRefTable()
	{
		pendings++;
		strengthAttrTable.Clear();
		AssetMng.instance.LoadAsset<StrengthenAttrRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "str_attr" + extension, (x, y) =>
			{
				pendings--;
				if (y == EResult.Success)
				{
                    for (int i = 0; i < x.infoList.Count; i++)
                    {
                        strengthAttrTable[x.infoList[i].id] = x.infoList[i];
                    }
				}
			});
	}
	/// <summary>
	/// 获取强化属性
	/// </summary>
	public StrengthenAttrRef GetStrengthenAttrRefByLv(int level,EquipSlot slot)
	{
		foreach(StrengthenAttrRef attrRef in strengthAttrTable.Values)
		{
			if(attrRef.lev == level && attrRef.position == (int)slot)
				return attrRef;
		}
        Debug.LogError("找不到强化属性level:" + level+",slot:"+slot);
		return null;
	}
	#endregion

	#region 强化消耗表
	protected FDictionary strengthRefTable = new FDictionary();
	public void InitStrengthRefTable()
	{
		pendings++;
		strengthRefTable.Clear();
		AssetMng.instance.LoadAsset<StrengthenRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "str_Lev" + extension, (x, y) =>
			{
				pendings--;
				if (y == EResult.Success)
				{
                    for (int i = 0; i < x.infoList.Count; i++)
                    {
                        strengthRefTable[x.infoList[i].id] = x.infoList[i];
                    }
				}
			});
	}
	/// <summary>
	/// 获取强化消耗
	/// </summary>
	public StrengthenRef GetStrengthenRefByLv(int level)
	{
		foreach(StrengthenRef strengthenRef in strengthRefTable.Values)
		{
			if(strengthenRef.lev == level)
				return strengthenRef;
		}
		return null;
	}
	#endregion

    #region 强化等级描述表
    protected FDictionary strengthLvDesRefTable = new FDictionary();
    public void InitStrengthDesRefTable()
    {
        pendings++;
        strengthLvDesRefTable.Clear();
        AssetMng.instance.LoadAsset<StrengthenDesRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "suiconfig" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    strengthLvDesRefTable[x.infoList[i].id] = x.infoList[i];
                }
            }
        });
    }
    /// <summary>
    /// 获取强化等级描述
    /// </summary>
    public string GetStrengthenLvDesfByLv(int level)
    {
        if (strengthLvDesRefTable.ContainsKey(level))
        {
            StrengthenDesRef strengthLvDesRef = strengthLvDesRefTable[level] as StrengthenDesRef;
            return strengthLvDesRef == null ? string.Empty : strengthLvDesRef.text;
        }
        return string.Empty;
    }
    #endregion

    #region 镶嵌开启表
    protected FDictionary inlayOpenRefTable = new FDictionary();
    public void InitInlayOpenRefTable()
    {
        pendings++;
        inlayOpenRefTable.Clear();
        AssetMng.instance.LoadAsset<InsetRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "Equi_Mosaic" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    inlayOpenRefTable[x.infoList[i].id] = x.infoList[i];
                }
            }
        });
    }
    /// <summary>
    /// 获取强化后的装备是否开启槽数量
    /// </summary>
    public List<InsetRef> GetInlaySlotsByLv(int level)
    {
        List<InsetRef> slots = new List<InsetRef>();
        foreach (var inlayOpenRef in inlayOpenRefTable.Values)
        {
            InsetRef inlay = inlayOpenRef as InsetRef;
            if (inlay.openType == 2 && inlay.num <= level)
                slots.Add(inlay);
        }
        return slots;
    }
    #endregion

	#region 橙炼消耗表
	protected FDictionary orangeRefineRefTable = new FDictionary();
	public void InitOrangeRefineRefTable()
	{
		pendings++;
		orangeRefineRefTable.Clear();
		AssetMng.instance.LoadAsset<OrangeRefineRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "Orange_refining" + extension, (x, y) =>
			{
				pendings--;
				if (y == EResult.Success)
				{
                    for (int i = 0; i < x.infoList.Count; i++)
                    {
                        orangeRefineRefTable[x.infoList[i].id] = x.infoList[i];
                    }
				}
			});
	}
	/// <summary>
	/// 获取橙炼详情
	/// </summary>
	public OrangeRefineRef GetOrangeRefineRefByEid(int eid)
	{
		foreach(OrangeRefineRef orangeRefineRef in orangeRefineRefTable.Values)
		{
			if(orangeRefineRef.start_item == eid)
				return orangeRefineRef;
		}
		return null;
	}
	#endregion

	#region 升阶消耗表
	protected FDictionary promoteRefTable = new FDictionary();
	public void InitPromoteRefTable()
	{
		pendings++;
		promoteRefTable.Clear();
		AssetMng.instance.LoadAsset<PromoteRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "promote" + extension, (x, y) =>
			{
				pendings--;
				if (y == EResult.Success)
				{
                    for (int i = 0; i < x.infoList.Count; i++)
                    {
                        promoteRefTable[x.infoList[i].id] = x.infoList[i];
                    }
				}
			});
	}
	/// <summary>
	/// 获取升阶详情
	/// </summary>
	public PromoteRef GetPromoteRefByEid(int eid)
	{
		foreach(PromoteRef promoteRef in promoteRefTable.Values)
		{
			if(promoteRef.start_item == eid)
				return promoteRef;
		}
		return null;
	}
	#endregion

	#region 继承幸运消耗表
	protected FDictionary inheritLuckyRefTable = new FDictionary();
	public void InitInheritLuckyRefTable()
	{
		pendings++;
		inheritLuckyRefTable.Clear();
		AssetMng.instance.LoadAsset<InheritLuckyRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "Inherit_Lucky" + extension, (x, y) =>
			{
				pendings--;
				if (y == EResult.Success)
				{
                    for (int i = 0; i < x.infoList.Count; i++)
                    {
                        inheritLuckyRefTable[x.infoList[i].id] = x.infoList[i];
                    }
				}
			});
	}
	/// <summary>
	/// 获取继承幸运消耗
	/// </summary>
	public ItemValue GetInheritConsumeByLv(int minLev,int maxLev)
	{
		ItemValue maxCount = null;
		ItemValue minCount = null;
		foreach(InheritLuckyRef inheritLuckyRef in inheritLuckyRefTable.Values)
		{
			if(inheritLuckyRef.lev == maxLev)
				maxCount = inheritLuckyRef.consumeItem[0];
			if(inheritLuckyRef.lev == minLev)
				minCount = inheritLuckyRef.consumeItem[0];
		}
		return (maxCount == null || minCount == null)?null:new ItemValue(maxCount.eid,maxCount.count - minCount.count);
	}
	/// <summary>
	/// 获取继承幸运消耗
	/// </summary>
	public InheritLuckyRef GetInheritLuckyRefByID(int id)
	{
		foreach(InheritLuckyRef inheritLuckyRef in inheritLuckyRefTable.Values)
		{
			if(inheritLuckyRef.id == id)
				return inheritLuckyRef;
		}
		return null;
	}
	#endregion

	#region 洗练消耗表
	protected FDictionary washConsumeRefTable = new FDictionary();
	public void InitWashConsumeRefTable()
	{
		pendings++;
		washConsumeRefTable.Clear();
		AssetMng.instance.LoadAsset<EquipmentWashConsumeRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "att_consume" + extension, (x, y) =>
			{
				pendings--;
				if (y == EResult.Success)
				{
                    for (int i = 0; i < x.infoList.Count; i++)
                    {
                        washConsumeRefTable[x.infoList[i].quality] = x.infoList[i];
                    }
				}
			});
	}
	/// <summary>
	/// 获取洗练消耗
	/// </summary>
	public EquipmentWashConsumeRef GetEquipmentWashConsumeRefByQuality(int quality)
	{
		foreach(EquipmentWashConsumeRef equipmentWashConsumeRef in washConsumeRefTable.Values)
		{
			if(equipmentWashConsumeRef.quality == quality)
				return equipmentWashConsumeRef;
		}
		return null;
	}
	#endregion

	#region 洗练属性表
	protected FDictionary washValueRefTable = new FDictionary();
	public void InitWashValueRefTable()
	{
		pendings++;
		washValueRefTable.Clear();
		AssetMng.instance.LoadAsset<EquipmentWashValueRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "att_value" + extension, (x, y) =>
			{
				pendings--;
				if (y == EResult.Success)
				{
                    for (int i = 0; i < x.infoList.Count; i++)
                    {
                        washValueRefTable[x.infoList[i].id] = x.infoList[i];
                    }
				}
			});
	}
	/// <summary>
	/// 获取洗练属性
	/// </summary>
	public EquipmentWashValueRef GetEquipmentWashValueRefByID(int id)
	{
		foreach(EquipmentWashValueRef equipmentWashValueRef in washValueRefTable.Values)
		{
			if(equipmentWashValueRef.id == id)
				return equipmentWashValueRef;
		}
		return null;
	}
	/// <summary>
	/// 根据属性ID获取属性的文字显示,包括品质颜色的显示
	/// </summary>
	public string GetValueStringByID(int id)
	{
		EquipmentWashValueRef valueRef = GetEquipmentWashValueRefByID(id);
		if(valueRef != null)
		{
			string attrName = GetAttributeTypeName((ActorPropertyTag)valueRef.att_type);
			StringBuilder builder = new StringBuilder();
			builder.Append(EquipmentInfo.ItemColorStr(valueRef.att_quality)).Append(attrName).Append(":").Append(valueRef.value).Append("[-]");
			return builder.ToString();
		}
		return string.Empty;
	}
	#endregion

	#region 环任务奖励配置表
	protected FDictionary taskSurroundRewardRefTable = new FDictionary();
	public void InitTaskSurroundRewardRefTable()
	{
		pendings++;
		taskSurroundRewardRefTable.Clear();
		AssetMng.instance.LoadAsset<TaskSurroundRewardRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "task_surround_reward" + extension, (x, y) =>
			{
				pendings--;
				if (y == EResult.Success)
				{
                    for (int i = 0; i < x.infoList.Count; i++)
                    {
                        taskSurroundRewardRefTable[x.infoList[i].Lev] = x.infoList[i];
                    }
				}
			});
	}
	/// <summary>
	/// 获取环任务奖励
	/// </summary>
	public TaskSurroundRewardRef GetTaskSurroundRewardRefLv(int level)
	{
		if(taskSurroundRewardRefTable.ContainsKey(level))
		{
			return taskSurroundRewardRefTable[level] as TaskSurroundRewardRef;
		}
		Debug.LogError("找不到等级为:"+level+"的环任务奖励!");
		return null;
	}
	#endregion

	#region 分解获取物1配置表
	protected FDictionary resolveLevelRefTable = new FDictionary();
	public void InitResolveLevelRefTable()
	{
		pendings++;
		resolveLevelRefTable.Clear();
		AssetMng.instance.LoadAsset<ResolveLevelRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "lccky_lev" + extension, (x, y) =>
			{
				pendings--;
				if (y == EResult.Success)
				{
                    for (int i = 0; i < x.infoList.Count; i++)
                    {
                        resolveLevelRefTable[x.infoList[i].id] = x.infoList[i];
                    }
				}
			});
	}
	/// <summary>
	/// 获取分解获取物
	/// </summary>
	public List<ItemValue> GetEquipmentListByLuckyLv(int luckyLv)
	{
		List<ItemValue> list = new List<ItemValue>();
		foreach(ResolveLevelRef resolveLevelRef in resolveLevelRefTable.Values)
		{
			if(resolveLevelRef.Lucky_level == luckyLv)
			{
				list = resolveLevelRef.dec_item;
			}
		}
		return list;
	}
    public ResolveLevelRef GetResolveRef(int luckylv)
    {
        foreach (ResolveLevelRef resolveLevelRef in resolveLevelRefTable.Values)
        {
            if (resolveLevelRef.Lucky_level == luckylv)
            {
                return resolveLevelRef;
            }
        }
        return null;
    }
	#endregion

	#region 分解获取物2配置表
	protected FDictionary resolveRefTable = new FDictionary();
	public void InitResolveRefTable()
	{
		pendings++;
		resolveRefTable.Clear();
		AssetMng.instance.LoadAsset<ResolveRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "equi_deco" + extension, (x, y) =>
			{
				pendings--;
				if (y == EResult.Success)
				{
                    for (int i = 0; i < x.infoList.Count; i++)
                    {
                        resolveRefTable[x.infoList[i].id] = x.infoList[i];
                    }
				}
			});
	}
	/// <summary>
	/// 获取分解物品
	/// </summary>
	public List<ItemValue> GetResolveItemListByLv(int Lev,int quality,int slot)
	{
		List<ItemValue> list = new List<ItemValue>();
		foreach(ResolveRef resolveRef in resolveRefTable.Values)
		{
			if(resolveRef.Lev == Lev && resolveRef.quality == quality && resolveRef.position == slot)
				list = resolveRef.Will_item;
		}
		return list;
	}
	#endregion

	#region 等级套装配置表
	protected FDictionary equipmentSetRefTable = new FDictionary();
	public void InitEquipmentSetRefTable()
	{
		pendings++;
		equipmentSetRefTable.Clear();
		AssetMng.instance.LoadAsset<EquipmentSetRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "Equipment_set" + extension, (x, y) =>
			{
				pendings--;
				if (y == EResult.Success)
				{
                    for (int i = 0; i < x.infoList.Count; i++)
                    {
                        equipmentSetRefTable[x.infoList[i].id] = x.infoList[i];
                    }
				}
			});
	}
	/// <summary>
	/// 获取等级套装数据type表示攻击套装or防御套装
	/// </summary>
	public EquipmentSetRef GetEquipmentSetRefByLv(int Lev,int quality,int type)
	{
		foreach(EquipmentSetRef equipmentSetRef in equipmentSetRefTable.Values)
		{
			if(equipmentSetRef.lev == Lev && equipmentSetRef.quality == quality && equipmentSetRef.type == type)
				return equipmentSetRef;
		}
		return null;
	}
	#endregion

	#region 仙盟配置表
	protected FDictionary guildRefTable = new FDictionary();
	public void InitGuildRefTable()
	{
		pendings++;
		guildRefTable.Clear();
		AssetMng.instance.LoadAsset<GuildRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "Guild" + extension, (x, y) =>
			{
				pendings--;
				if (y == EResult.Success)
				{
                    for (int i = 0; i < x.infoList.Count; i++)
                    {
                        guildRefTable[x.infoList[i].lev] = x.infoList[i];
                    }
				}
			});
	}
	/// <summary>
	/// 获取仙盟数据
	/// </summary>
	public GuildRef GetGuildRefByLv(int level)
	{
		foreach(GuildRef guildRef in guildRefTable.Values)
		{
			if(guildRef.lev == level)
				return guildRef;
		}
		return null;
	}
	#endregion

	#region 日志配置表
	protected FDictionary logRefTable = new FDictionary();
	public void InitGlogRefTable()
	{
		pendings++;
		logRefTable.Clear();
		AssetMng.instance.LoadAsset<GlogRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "Glog" + extension, (x, y) =>
			{
				pendings--;
				if (y == EResult.Success)
				{
                    for (int i = 0; i < x.infoList.Count; i++)
                    {
                        logRefTable[x.infoList[i].id] = x.infoList[i];
                    }
				}
			});
	}
	/// <summary>
	/// 获取日志数据
	/// </summary>
	public string GetGlogStringByID(int id)
	{
		foreach(GlogRef logRef in logRefTable.Values)
		{
			if(logRef.id == id)
				return logRef.txt;
		}
		return string.Empty;
	}
	#endregion

	#region 城内商店配置表
	protected FDictionary cityShopRefTable = new FDictionary();
	public void InitCityShopRefTable()
	{
		pendings++;
		cityShopRefTable.Clear();
        AssetMng.instance.LoadAsset<CityShopRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "Shabakeshopconfig" + extension, (x, y) =>
			{
				pendings--;
				if (y == EResult.Success)
				{
                    for (int i = 0; i < x.infoList.Count; i++)
                    {
                        cityShopRefTable[x.infoList[i].id] = x.infoList[i];
                    }
				}
			});
	}
	/// <summary>
	/// 获取城内商店数据
	/// </summary>
	public List<CityShopRef> GetCityShopRefLisyByPage(int page)
	{
		List<CityShopRef> shopList = new List<CityShopRef>();
		foreach(CityShopRef cityShopRef in cityShopRefTable.Values)
		{
			if(cityShopRef.page == page)
				shopList.Add(cityShopRef);
		}
		return shopList;
	}

	public CityShopRef GetShopRefByID(int id)
	{
		foreach(CityShopRef cityShopRef in cityShopRefTable.Values)
		{
			if(cityShopRef.id == id)
				return cityShopRef;
		}
		return null;
	}
	#endregion
    
	#region BOSS挑战配置表
	protected FDictionary bossRefTable = new FDictionary();
	public void InitBossRefTable()
	{
		pendings++;
		bossRefTable.Clear();
		AssetMng.instance.LoadAsset<BossRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "boss" + extension, (x, y) =>
			{
				pendings--;
				if (y == EResult.Success)
				{
                    for (int i = 0; i < x.infoList.Count; i++)
                    {
                        bossRefTable[x.infoList[i].monsterId] = x.infoList[i];
                    }
				}
			});
	}
	/// <summary>
	/// 获取BOSS挑战数据
	/// </summary>
	public List<int> GetBossRefByType(int type)
	{
		List<int> list = new List<int>();
		foreach(BossRef bossRef in bossRefTable.Values)
		{
			if(bossRef.type == type)
				list.Add(bossRef.monsterId);
		}
		list.Sort();
		return list;
	}

    public List<BossRef> GetBossRefListByType(int type)
    {
        List<BossRef> list = new List<BossRef>();
        foreach (BossRef bossRef in bossRefTable.Values)
        {
            if (bossRef.type == type)
                list.Add(bossRef);
        }
        return list;
    }

	/// <summary>
	/// 获取BOSS挑战数据
	/// </summary>
	public BossRef GetBossRefByID(int id)
	{
        if (bossRefTable.ContainsKey(id))
            return bossRefTable[id] as BossRef;
		return null;
	}
    #endregion
    #region 离线奖励配置
    protected FDictionary offLineRewardRefTable = new FDictionary();
    public FDictionary OffLineRewardTable
    {
        get
        {
            return offLineRewardRefTable;
        }
    }

    public void InitOffLineRewardRefTable()
    {
        pendings++;
        offLineRewardRefTable.Clear();
        AssetMng.instance.LoadAsset<OffLineRewardRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "offLineReward" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.OffLineRewardList.Count; i++)
                {
                    offLineRewardRefTable[x.OffLineRewardList[i].playerLevel] = x.OffLineRewardList[i];
                }
            }
        });
    }
    public OffLineRewardRef GetOffLineRewardRef(int _level)
    {
        if (newPetRefTable.ContainsKey(_level))
        {
            OffLineRewardRef data = offLineRewardRefTable[_level] as OffLineRewardRef;
            return data;
        }
        return null;
    }
    #region 七日挑战任务配置
    protected FDictionary sevenChallengeTaskRefTable = new FDictionary();
    public FDictionary SevenChallengeTaskRefTable
    {
        get
        {
            return sevenChallengeTaskRefTable;
        }
    }

    public void InitSevenChallengeTaskRefTable()
    {
        pendings++;
        sevenChallengeTaskRefTable.Clear();
        AssetMng.instance.LoadAsset<SevenDaysTaskRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "SevenDaysTaskConfig" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                   
                    sevenChallengeTaskRefTable[x.infoList[i].id] = x.infoList[i];
                    //Debug.Log("infoList[i].id:"+ x.infoList[i].id);
                }
            }
        });
    }
    public SevenDaysTaskRef GetSevenChallengeTaskRef(int _id)
    {
        //Debug.Log("_id:"+ _id);
        //foreach (SevenDaysTaskRef data in sevenChallengeRewardRefTable.Values)
        //{
        //    if (data.id == _id)
        //        return data;
        //}
        //return null;
        if (sevenChallengeTaskRefTable.ContainsKey(_id))
        {
            SevenDaysTaskRef data = sevenChallengeTaskRefTable[_id] as SevenDaysTaskRef;
            return data;
        }
        return null;
    }
    public List<SevenDaysTaskRef>GetSevenChallengeTaskListRef(int _day)
    {
        //Debug.Log("_id:"+ _id);
        List<SevenDaysTaskRef> list = new List<SevenDaysTaskRef>();
        foreach (SevenDaysTaskRef data in sevenChallengeTaskRefTable.Values)
        {
            if (data.day == _day)
                list.Add(data);
        }
        return list;
    }
    #endregion
    #region 七日挑战奖励配置
    protected FDictionary sevenChallengeRewardRefTable = new FDictionary();
    public FDictionary SevenChallengeRewardRefTable
    {
        get
        {
            return sevenChallengeRewardRefTable;
        }
    }

    public void InitSevenChallengeRewardRefTable()
    {
        pendings++;
        sevenChallengeRewardRefTable.Clear();
        AssetMng.instance.LoadAsset<SevenDaysTaskRewardRefTable>(AssetMng.GetPathWithoutExtension("config/", AssetPathType.PersistentDataPath) + "SevenDaysTaskRewardConfig" + extension, (x, y) =>
        {
            pendings--;
            if (y == EResult.Success)
            {
                for (int i = 0; i < x.infoList.Count; i++)
                {
                    sevenChallengeRewardRefTable[x.infoList[i].day] = x.infoList[i];
                }
            }
        });
    }
    public SevenDaysTaskRewardRef GetSevenChallengeRewardRef(int _day)
    {
        if (sevenChallengeRewardRefTable.ContainsKey(_day))
        {
            SevenDaysTaskRewardRef data = sevenChallengeRewardRefTable[_day] as SevenDaysTaskRewardRef;
            return data;
        }
        Debug.LogError("找不到ID为:"+_day+"的七日挑战数据,请检查后台数据");
        return null;
    }
    #endregion
}

public class VersionAsset
{
    public string originStr = "";
    public string subDir = "";
    public string assetName = "";
    public string md5Value = "";
    public int assetSize = 0;
    public ActionType actionType = ActionType.NONE;
    public enum ActionType
    {
        NONE,
        ADD,
        DELETE,
        UPDATE,
    }
}
#endregion


/// <summary>
/// sdk的平台
/// </summary>
public enum SdkPlatform
{
    UnKown,
    /// <summary>
    /// 360
    /// </summary>
    f360,
    /// <summary>
    /// 小米
    /// </summary>
    XiaoMi,
    UC,
    DangLe,
    YiDong,
    IOS,
}
   