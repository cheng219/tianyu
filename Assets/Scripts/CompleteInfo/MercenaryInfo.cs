//==================================
//作者：朱素云
//日期：2016/3/7
//用途：随从数据层
//=================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase; 

public class PetData : ActorData
{
    public int configId;
    public int type;
    public int status;
    public int fight_score;
    public int aptitude;
    public int grow_up;
    public int lev;
    public string pet_name = string.Empty;
    public string ownerName;
    public List<pet_property_list> all_property = new List<pet_property_list>();
    public List<pet_property_list> grow_up_property = new List<pet_property_list>();
    public List<pet_property_list> jinghun_property = new List<pet_property_list>();
    public int tian_soul;
    public int di_soul;
    public int life_soul;
    public List<uint> pet_skill = new List<uint>();
    public List<int> petSkill = new List<int>();
    public int grow_up_exp;
    public int aptitude_exp; 
    public bool hasChange = false; 
    public int ownerInstanceID;
    public string petTitleName = string.Empty;
    public PetData()
    {
    }
    public PetData(scene_entourage _data) 
    {
        serverInstanceID = (int)_data.eid;
        type = (int)_data.type;
        camp = (int)_data.camp;
        baseValueDic[ActorBaseTag.Level] = _data.level;
        startPosX = _data.x;
        startPosY = _data.y;
        startPosZ = _data.z;
        dir = (int)_data.dir;
        ownerInstanceID = (int)_data.owner;
        propertyValueDic[ActorPropertyTag.HPLIMIT] = (int)_data.max_hp;
        propertyValueDic[ActorPropertyTag.MPLIMIT] = (int)_data.max_mp;
        baseValueDic[ActorBaseTag.CurHP] = _data.hp;
        baseValueDic[ActorBaseTag.CurMP] = _data.mp;
        grow_up = _data.pet_grow_up;
        aptitude = _data.pet_aptitude_lev;
        ownerName = _data.owner_name;
        pet_name = _data.pet_name; 
    }
    public PetData(pet_base_info pet_info)
    { 
        configId = (int)pet_info.id;
        //serverInstanceID = (int)pet_info.id; 
        type = (int)pet_info.type;
        status = (int)pet_info.status;
        fight_score = (int)pet_info.fight_score;
        aptitude = (int)pet_info.aptitude;
        grow_up = (int)pet_info.grow_up;
        lev = (int)pet_info.lev; 
        pet_name = pet_info.pet_name;
        all_property = pet_info.all_property;
        grow_up_property = pet_info.grow_up_property;
        jinghun_property = pet_info.jinghun_property;
        tian_soul = (int)pet_info.tian_soul;
        di_soul = (int)pet_info.di_soul;
        life_soul = (int)pet_info.life_soul; 
        pet_skill = pet_info.pet_skill; 
        for (int i = 0, max = pet_info.pet_skill.Count; i < max; i++)
        { 
            NewPetSkillRef skill = ConfigMng.Instance.GetPetSkillRef((int)pet_info.pet_skill[i]);
            if (skill != null && skill.skillId > 0) petSkill.Add(skill.skillId);
        } 
        grow_up_exp = (int)pet_info.grow_up_exp;
        aptitude_exp = (int)pet_info.aptitude_exp;
        hasChange = true;
        ownerInstanceID = GameCenter.mainPlayerMng.MainPlayerInfo.ServerInstanceID;
        ownerName = GameCenter.mainPlayerMng.MainPlayerInfo.Name;
    } 
}

public class MercenaryInfo : ActorInfo
{
    public System.Action OnOwnerNameUpdateEvent;


    //服务端数据 
    protected new PetData serverData
    {
        get { return base.serverData as PetData; }
        set { base.serverData = value; }
    } 
    //客户端主体数据静态配置
    protected NewPetRef refData = null;
    protected NewPetRef RefData
    {
        get
        {
            if (refData == null)
            {
                refData = ConfigMng.Instance.GetNewPetRef((int)serverData.type) != null ?
                    ConfigMng.Instance.GetNewPetRef((int)serverData.type) : ConfigMng.Instance.GetNewPetRef(1);
            }
            return refData;
        }
    }
    protected PlayerBaseInfo ownerInfo;
    /// <summary>
    /// 服务端数据构造
    /// </summary> 
    public MercenaryInfo(PetData _data, PlayerBaseInfo _info)
    {
        serverData = _data;
        ownerInfo = _info;
        SetTitle();
    }

    /// <summary>
    /// 服务端数据构造
    /// </summary> 
    public MercenaryInfo(NewPetRef _refData)
    {
        refData = _refData;
        serverData = new PetData();
        serverData.serverInstanceID = _refData.type;
        SetTitle();
    }

    /// <summary>
    /// 服务端数据构造
    /// </summary> 
    public MercenaryInfo(pet_base_info _data, PlayerBaseInfo _info)
    {
        PetData data = new PetData(_data);
        serverData = data;
        ownerInfo = _info;
        SetTitle();
    }


    public MercenaryInfo(scene_entourage _data, bool _isInFight)
    {
        //Debug.Log("*************************scene_entourage  " + _data.pet_grow_up + "   , type : " + _data.type + "  , peteidid : " + _data.eid + "  , aptitude : " + _data.pet_aptitude_lev);  
        serverData = new PetData(_data);  
        SetTitle();
    }


    public void Update(scene_entourage _data)
    {
        //Debug.Log("*************************scene_entourage  " + _data.pet_grow_up + "   , type : " + _data.type + "  , peteidid : " + _data.eid); 
        //serverData = new PetData(_data); 
        serverData.serverInstanceID = (int)_data.eid;
        serverData.type = (int)_data.type;
        serverData.camp = (int)_data.camp;
        serverData.baseValueDic[ActorBaseTag.Level] = _data.level;
        serverData.startPosX = _data.x;
        serverData.startPosY = _data.y;
        serverData.startPosZ = _data.z;
        serverData.dir = (int)_data.dir;
        serverData.ownerInstanceID = (int)_data.owner;
        serverData.propertyValueDic[ActorPropertyTag.HPLIMIT] = (int)_data.max_hp;
        serverData.propertyValueDic[ActorPropertyTag.MPLIMIT] = (int)_data.max_mp;
        serverData.baseValueDic[ActorBaseTag.CurHP] = _data.hp;
        serverData.baseValueDic[ActorBaseTag.CurMP] = _data.mp;
        serverData.grow_up = _data.pet_grow_up;
        serverData.ownerName = _data.owner_name;
        Aptitude = _data.pet_aptitude_lev;
        PetName = _data.pet_name; 
        SetTitle();
    }

    /// <summary>
    /// 刷新列表数据
    /// </summary> 
	public void Update(PetData _data,int _serverInstanceID)
    {
        serverData = _data; 
		serverData.serverInstanceID = _serverInstanceID;
    }

    public void Update(pt_entourage_create_d01f _awake)
    { 
        serverData.serverInstanceID = (int)_awake.oid;
        serverData.camp = GameCenter.mainPlayerMng.MainPlayerInfo.Camp;
        serverData.ownerInstanceID = GameCenter.mainPlayerMng.MainPlayerInfo.ServerInstanceID; 
        HasAwake = true;
    }
    /// <summary>
    /// 融合
    /// </summary> 
    public void UpdateAfterChange(pt_fuse_info_d410 _msg)
    {
        int id = GameCenter.mercenaryMng.fuPetId;
        //Debug.logger.Log("融合反馈id ：" + _msg.pet_type + "   ,资质 ：  " + _msg.aptitude_lev + "   ,资质经验： " + _msg.aptitude_exp + "  , 成长 ： " + _msg.grow_up_lev + "  , 天 ：" + _msg.tian_soul + " ,地 ：" + _msg.di_soul + "  ,命 ： " + _msg.life_soul);
        if (GameCenter.mercenaryMng.mercenaryInfoList.ContainsKey(id)) 
            GameCenter.mercenaryMng.mercenaryInfoList.Remove(id);
        GameCenter.mercenaryMng.fuPetId = MercenaryMng.noPet;
        Aptitude = _msg.aptitude_lev; 
        serverData.grow_up = _msg.grow_up_lev;  
        serverData.tian_soul = _msg.tian_soul;
        serverData.di_soul = _msg.di_soul;
        serverData.life_soul = _msg.life_soul;
        serverData.aptitude_exp = _msg.aptitude_exp; 
        GameCenter.mercenaryMng.zhuPetId = _msg.pet_type;
        GameCenter.mercenaryMng.curPetId = _msg.pet_type; 
        SetTitle();
        if (GameCenter.mercenaryMng.OnFuPetUpdate != null) GameCenter.mercenaryMng.OnFuPetUpdate();
        if (GameCenter.mercenaryMng.OnZhuPetUpdate != null) GameCenter.mercenaryMng.OnZhuPetUpdate();
    } 
    /// <summary>
    /// 改名字
    /// </summary> 
    public void UpdateAfterChange(pt_update_pet_name_d426 _msg)
    {
        PetName = _msg.new_name; 
    }

    public void UpdateOwnerName(string _ownerName)
    {
        if (serverData.ownerName != _ownerName)
        {
            serverData.ownerName = _ownerName;
            if (OnOwnerNameUpdateEvent != null)
                OnOwnerNameUpdateEvent();
        }
    }

    /// <summary>
    /// 提升
    /// </summary> 
    public void UpdateAfterChange(pt_pet_updata_property_d409 _msg)
    {
        //Debug.Log("收到类型值 ： " + _msg.state + " ， 收到num  ： " + _msg.num + " , 收到等级： " + _msg.lev);
        PetChange state = (PetChange)_msg.state;
        switch (state)
        {
            case PetChange.NONESTATE://使用宠物蛋增加资质经验
                 if (Aptitude < _msg.lev)
                    {
                        Aptitude = _msg.lev;
                        GameCenter.messageMng.AddClientMsg(150);
                    }
                    serverData.aptitude_exp = _msg.num; 
                    if (GameCenter.mercenaryMng.OnZhuPetUpdate != null && _msg.pet_type == GameCenter.mercenaryMng.zhuPetId)
                    {
                        GameCenter.mercenaryMng.OnZhuPetUpdate();
                    }
                    if (GameCenter.mercenaryMng.OnFuPetUpdate != null && _msg.pet_type == GameCenter.mercenaryMng.fuPetId)
                    {
                        GameCenter.mercenaryMng.OnFuPetUpdate();
                    }
                    if (GameCenter.mercenaryMng.OnPetGrowUpUpdate != null)
                    {
                        GameCenter.mercenaryMng.OnPetGrowUpUpdate();
                    }
                break;
            case PetChange.GROWUP:  
                serverData.grow_up_exp = _msg.num;
                if (_msg.lev > serverData.grow_up)
                {
                    serverData.grow_up = _msg.lev;
                    GameCenter.messageMng.AddClientMsg(146); 
                }
                SetTitle();
                if (GameCenter.mercenaryMng.OnPetGrowUpUpdate != null)
                    GameCenter.mercenaryMng.OnPetGrowUpUpdate();
                break;
            case PetChange.PRACTICETIANSOUL:
                int att = 0;
                if (_msg.lev > Tian_soul)
                {
                    serverData.tian_soul = _msg.lev;
                    GameCenter.messageMng.AddClientMsg(323);
                    if (GameCenter.mercenaryMng.OnLingXiuLevUpdate != null)
                        GameCenter.mercenaryMng.OnLingXiuLevUpdate(LingXiuType.TIAN);
                }
                else
                {
                    if (GameCenter.mercenaryMng.OnLingXiuExpUpdate != null)
                        GameCenter.mercenaryMng.OnLingXiuExpUpdate(LingXiuType.TIAN);
                }
                for (int i = 0; i < serverData.jinghun_property.Count; i++)
                {  
                    if (serverData.jinghun_property[i].type == 1)
                    {
                        att = serverData.jinghun_property[i].num;
                        serverData.jinghun_property[i].num = _msg.num; break;
                    } 
                } 
                GameCenter.practiceMng.ReminderWnd(320,(_msg.num - att).ToString()); 
                break;
            case PetChange.SEALSKILL:
                //++GameCenter.mercenaryMng.emptyNest;
                //serverData.pet_skill.Remove((uint)_msg.num); 
                break;
            case PetChange.FORGETSKILL:
                //++GameCenter.mercenaryMng.emptyNest;
                //serverData.pet_skill.Remove((uint)_msg.num); 
                break;
            case PetChange.STUDYSKILL: 
                serverData.pet_skill.Add((uint)_msg.num);
                //Debug.Log("   学习技能 ： " + serverData.pet_skill.Count);
                break;
            case PetChange.PRACTICEDISOUL: 
                int hit = 0;
                if (_msg.lev > Di_soul)
                {
                    serverData.di_soul = _msg.lev;
                    GameCenter.messageMng.AddClientMsg(324);
                    if (GameCenter.mercenaryMng.OnLingXiuLevUpdate != null)
                        GameCenter.mercenaryMng.OnLingXiuLevUpdate(LingXiuType.DI);
                }
                else
                {
                    if (GameCenter.mercenaryMng.OnLingXiuExpUpdate != null)
                        GameCenter.mercenaryMng.OnLingXiuExpUpdate(LingXiuType.DI);
                }
                for (int i = 0; i < serverData.jinghun_property.Count; i++)
                {  
                    if (serverData.jinghun_property[i].type == 7)
                    {
                        hit = serverData.jinghun_property[i].num;
                        serverData.jinghun_property[i].num = _msg.num;break; 
                    }
                } 
                GameCenter.practiceMng.ReminderWnd(322, (_msg.num - hit).ToString()); 
                break;
            case PetChange.PRACTICELIFESOUL:
                int cri = 0;
                if (_msg.lev > Life_soul)
                {
                    serverData.life_soul = _msg.lev;
                    GameCenter.messageMng.AddClientMsg(325);
                    if (GameCenter.mercenaryMng.OnLingXiuLevUpdate != null)
                        GameCenter.mercenaryMng.OnLingXiuLevUpdate(LingXiuType.LIFE);
                }
                else
                {
                    if (GameCenter.mercenaryMng.OnLingXiuExpUpdate != null)
                        GameCenter.mercenaryMng.OnLingXiuExpUpdate(LingXiuType.LIFE);
                }
                for (int i = 0; i < serverData.jinghun_property.Count; i++)
                {  
                    if (serverData.jinghun_property[i].type == 9)
                    {
                        cri = serverData.jinghun_property[i].num;
                        serverData.jinghun_property[i].num = _msg.num; break;
                    }
                } 
                GameCenter.practiceMng.ReminderWnd(321, (_msg.num - cri).ToString()); 
                break;
            case PetChange.USEPRIMERYPILL:
            case PetChange.USEMIDDLEPILL:
            case PetChange.USESENIORPILL: //资质经验提升
                { 
                    if (Aptitude < _msg.lev)
                    {
                        Aptitude = _msg.lev;
                        GameCenter.messageMng.AddClientMsg(150);
                    }
                    serverData.aptitude_exp = _msg.num;
                    GameCenter.practiceMng.ReminderWnd(149, ((_msg.state == 19) ? 10 : ((_msg.state == 20) ? 50 : 200)).ToString());
                    if (GameCenter.mercenaryMng.OnZhuPetUpdate != null && _msg.pet_type == GameCenter.mercenaryMng.zhuPetId) GameCenter.mercenaryMng.OnZhuPetUpdate();
                    if (GameCenter.mercenaryMng.OnFuPetUpdate != null && _msg.pet_type ==  GameCenter.mercenaryMng.fuPetId) GameCenter.mercenaryMng.OnFuPetUpdate();
                }
                if (GameCenter.mercenaryMng.OnPetGrowUpUpdate != null)
                    GameCenter.mercenaryMng.OnPetGrowUpUpdate();
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 状态
    /// </summary> 
    public void UpdateAfterChange(pt_pet_updata_state_d404 _msg)
    {
        //Debug.Log("d404 : " + ", state : " + _msg.state + " , id : " + _msg.pet_type);
        PetChange state = (PetChange)_msg.state;
        switch (state)
        {
            case PetChange.FINGHTING:
                {
                    serverData.status = 1; 
                    //GameCenter.mercenaryMng.FloatingReminger(181);
                    break;
                }
            case PetChange.GUARD:
                serverData.status = 2;break;
            case PetChange.FREE:
                {
                    FDictionary pets = GameCenter.mercenaryMng.mercenaryInfoList;
                    GameCenter.mercenaryMng.mercenaryInfoList.Remove(_msg.pet_type);
                    if (GameCenter.mercenaryMng.mercenaryInfoList.Count > 0)
                    {
                        foreach (MercenaryInfo info in pets.Values)
                        {
                            GameCenter.mercenaryMng.curPetId = info.ConfigId; break;
                        }
                    }
                    else
                        GameCenter.mercenaryMng.curPetId = MercenaryMng.noPet;
                }
                break;
            case PetChange.REST:
            case PetChange.CANCELGUARD:
                serverData.status = 0;break;
            default:
                break;
        }
    }
    #region 访问器 
    /// <summary>
    /// 后台是否已经创建成功并且可以开始行动。需要通过与后台通讯确认 by吴江
    /// </summary>
    protected bool hasAwake = false;
    /// <summary>
    /// 后台是否已经创建成功并且可以开始行动。需要通过与后台通讯确认 by吴江
    /// </summary>
    public bool HasAwake
    {
        get
        {
            return hasAwake;
        }
        set
        {
            if (hasAwake != value)
            {
                hasAwake = value; 
            }
            if (hasAwake)
            {
                if (OnAwakeUpdate != null)
                { 
                    OnAwakeUpdate(hasAwake);
                }
            }
        }
    }
    /// <summary>
    /// 激活状态发生变化的事件 by吴江
    /// </summary>
    public System.Action<bool> OnAwakeUpdate;
    /// <summary>
    /// 某某宠物名字更新
    /// </summary>
    public System.Action<string> OnPetNameUpdate;
    /// <summary>
    /// 宠物称号更新
    /// </summary>
    public System.Action<string> OnPetTtleUpdate;
    /// <summary>
    /// 宠物资质更新名字颜色改变
    /// </summary>
    public System.Action<Color> OnPetAptitudeUpdate;
    
    public new int ServerInstanceID
    {
        get
        {
            return (int)serverData.serverInstanceID;
        }
    }
    /// <summary>
    /// 静态配置类型ID
    /// </summary>
    public int PetId
    {
        get
        {
            return serverData.type;
        }
    }
    /// <summary>
    /// 服务端唯一ID
    /// </summary>
    public int ConfigId
    {
        get
        {
            return serverData.configId;
        }
    }


    /// <summary>
    /// 图标名称
    /// </summary>
    public string Icon
    {
        get
        {
            return RefData == null ? string.Empty : RefData.icon;
        }
    }
    public Color PetNameColor
    {
        get
        {
            if (serverData.aptitude < 21)
            { 
                return new Color(231.0f/255, 1 , 232.0f/255);
            }
            else if (serverData.aptitude > 20 && serverData.aptitude < 41)
            { 
                return new Color(110.0f / 255, 245.0f/255, 116.0f / 255);
            }
            else if (serverData.aptitude > 40 && serverData.aptitude < 61)
            { 
                return new Color(60.0f / 255, 179.0f/255, 1);
            }
            else if (serverData.aptitude > 60 && serverData.aptitude < 81)
            { 
                return new Color(189.0f / 255, 84.0f/255, 1);
            }
            else if (serverData.aptitude > 80 && serverData.aptitude < 101)
            { 
                return new Color(1, 106.0f/255, 41.0f / 255);
            }
            else
            { 
                return new Color(1, 0, 0);
                
            } 
        }
    }
    /// <summary>
    /// 随从名字
    /// </summary> 
    public string PetName
    {

        get
        { 
            if (string.IsNullOrEmpty(serverData.pet_name))
            {
                serverData.pet_name = RefData.petname;
            }
            string name = serverData.pet_name; 
            if (serverData.aptitude < 21)
            { 
                name = "[e7ffe8]" + name + "[-]"; //白
            }
            else if (serverData.aptitude > 20 && serverData.aptitude < 41)
            { 
                name = "[6ef574]" + name + "[-]"; //绿
            }
            else if (serverData.aptitude > 40 && serverData.aptitude < 61)
            { 
                name = "[3cb3ff]" + name + "[-]"; //蓝
            }
            else if (serverData.aptitude > 60 && serverData.aptitude < 81)
            { 
                name = "[bd54ff]" + name + "[-]";//紫
            }
            else if (serverData.aptitude > 80 && serverData.aptitude < 101)
            {
                name = "[ff6a29]" + name + "[-]";//橙
            }
            else
            { 
                name = "[ff0000]" + name + "[-]"; //红 
            }
            return name;
        }

        set
        { 
            if (serverData.pet_name != value)
            {
                serverData.pet_name = value; 
                if (OnPetNameUpdate != null) 
                { 
                    OnPetNameUpdate(serverData.pet_name);
                }
            }
        }
    }
    public string NoColorName
    { 
        get
        {
            return serverData.pet_name != string.Empty ? serverData.pet_name : RefData.petname;
        }
    }
    public string NoColorOwnerName
    {
        get
        {
            return serverData.ownerName + ConfigMng.Instance.GetUItext(214);
        }
    } 
    /// <summary>
    /// 称号名称
    /// </summary>
    public string PetTitleName
    {
        get
        {
            return serverData.petTitleName;
        }
        set
        {
            if (serverData.petTitleName != value)
            {
                serverData.petTitleName = value;
                if (OnPetTtleUpdate != null)
                {
                    OnPetTtleUpdate(serverData.petTitleName);
                }
            }
        }
    }
    /// <summary>
    /// 随从的战斗力
    /// </summary>
    public int Power
    {
        get
        {
            return (int)serverData.fight_score;
        }
    }

    public float StaticSpeed
    {
        get
        {
            return RefData == null ? 0 : RefData.move_spd;
        }
    }

    /// <summary>
    /// 出战状态0没有状态 1 出战 2 守护
    /// </summary>
    public int IsActive
    { 
        get
        {
            return (int)serverData.status;
        }
    }
    /// <summary>
    /// 资质
    /// </summary>
    public int Aptitude
    {
        get 
        {
            return (int)serverData.aptitude;
        }
        set
        {
            if (serverData.aptitude != value)
            {
                serverData.aptitude = value;
                if (OnPetAptitudeUpdate != null)
                { 
                    OnPetAptitudeUpdate(PetNameColor);
                }
            }
        }
    } 
    /// <summary>
    /// 等级
    /// </summary>
    public int Level
    { 
        get
        {
            int lev = 0;
            if (GameCenter.mainPlayerMng.MainPlayerInfo != null)
            {
                lev = GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel;
            }
            AttributeRef attributeRef = ConfigMng.Instance.GetAttributeRef(lev > 0 ? lev : (int)serverData.lev); 
            if (attributeRef.reborn > 0)
            {
                return attributeRef.display_level;
            }
            else
            {
                return (int)serverData.lev;
            } 
        }
    }

    /// <summary>
    /// 预览坐标
    /// </summary>
    public Vector3 PreviewPosition
    {
        get
        {
            return RefData != null ? RefData.previewPosition : Vector3.zero;
        }
    }
    /// <summary>
    /// 预览朝向
    /// </summary>
    public Vector3 PreviewRotation
    {
        get
        {
            return RefData != null ? RefData.previewRotation : Vector3.zero; 
        }
    }

    /// <summary>
    /// 成长
    /// </summary>
    public int GrowUp
    { 
        get 
        { 
            int len = ConfigMng.Instance.GetPetDataRefTable.Count - 1;
            return (int)serverData.grow_up <= len ? (int)serverData.grow_up : len;
        }
    }
    /// <summary>
    /// 灵修需要的物品
    /// </summary>
    public List<ItemValue> LxItem
    {
        get
        {
            NewPetDataRef petDataRef = ConfigMng.Instance.GetPetDataRef(Tian_soul > 0 ? Tian_soul : 1);
            return petDataRef != null ? petDataRef.lXItem : new List<ItemValue>();
        }
    }
    /// <summary>
    /// 成长需要的物品
    /// </summary>
    public List<ItemValue> GrowUpItem
    {
        get
        {
            NewPetDataRef petDataRef = ConfigMng.Instance.GetPetDataRef(GrowUp);
            return petDataRef != null ? petDataRef.cZIem : new List<ItemValue>();
        }
    }
    /// <summary>
    /// 属性链表
    /// </summary>
    public List<pet_property_list> PropertyList
    {
        get
        {
            return serverData.all_property;
        }
    }
    /// <summary>
    /// 成长链表
    /// </summary>
    public List<pet_property_list> GrowUpPropertyList
    {
        get
        {
            return serverData.grow_up_property;
        }
    }
    /// <summary>
    /// 精魂链表
    /// </summary>
    public List<pet_property_list> SoulPropertyList
    {
        get
        {
            return serverData.jinghun_property;
        }
    }
    /// <summary>
    /// 天魂星级
    /// </summary>
    public int Tian_soul
    {
        get
        {
            return (int)serverData.tian_soul;
        }
    }
    /// <summary>
    /// 地魂
    /// </summary>
    public int Di_soul
    {
        get
        {
            return (int)serverData.di_soul;
        } 
    }
    /// <summary>
    /// 命魂
    /// </summary>
    public int Life_soul
    {
        get
        {
            return (int)serverData.life_soul;
        }
    }
    /// <summary>
    /// 宠物成长经验
    /// </summary>
    public int GrowUpExp
    {
        get 
        {
            return (int)serverData.grow_up_exp;
        }
    } 
    /// <summary>
    /// 资质经验
    /// </summary>
     public int AptitudeExp
     { 
         get
         {
             return (int)serverData.aptitude_exp;
         }
     } 
    /// <summary>
    /// 技能链表
    /// </summary>
     public List<uint> SkillList
    {
        get
        {
            CompareByQuality(serverData.pet_skill);
            return serverData != null ? serverData.pet_skill : new List<uint>();
        }
    }
     /// <summary>
     /// 宠物要释放的技能链表
     /// </summary>
     public List<int> PetSkillList
     {
         get
         {
             return serverData != null ? serverData.petSkill : new List<int>();
         }
     }
    /// <summary>
    /// 普通攻击
    /// </summary>
     public int NormalSkill
     {
         get
         {
             return RefData == null ? 0 : RefData.normolSkill;
         }
     }

     /// <summary>
     /// 所有者的服务端的唯一索引ID
     /// </summary>
     public int OwnerID
     {
         get
         {
             return serverData.ownerInstanceID;
         }
     }
     protected string assetName = string.Empty;
     /// <summary>
     /// 资源名称
     /// </summary>
     public string AssetName
     {
         get
         {
             return RefData == null ? string.Empty : RefData.res_name;
         }
     }
    /// <summary>
    /// 跟随范围
    /// </summary>
     public int FollowRange
     {
         get
         {
             return RefData == null ? 0 : RefData.followRange;
         }
     }
     public int StopFollowRange
     {
         get
         {
             return RefData == null ? 0 : RefData.stopFollowRange;
         }
     }
     public int PveAtkRange
     {
         get
         {
             return RefData == null ? 0 : RefData.pveAtkRange;
         }
     }
     public int PveMasterAtkRange
     {
         get
         {
             return RefData == null ? 0 : RefData.pveMasterAtkRange;
         }
     }
     public int PveReturnRange
     {
         get
         {
             return RefData == null ? 0 : RefData.pveReturnRange;
         }
     }
     public int PvpAttackRange
     {
         get
         {
             return RefData == null ? 0 : RefData.pvpAttackRange;
         }
     }
     public int PvpDefenseRange
     {
         get
         {
             return RefData == null ? 0 : RefData.pvpDefenseRange;
         }
     }
     public int PvpMasterAtkRange
     {
         get
         {
             return RefData == null ? 0 : RefData.pvpMasterAtkRange;
         }
     }
     /// <summary>
     /// 传送范围
     /// </summary>
     public int TeleportRange
     {
         get
         {
             return RefData == null ? 0 : RefData.teleportRange;
         }
     }
     /// <summary>
     /// 模型大小
     /// </summary>
     public new float ModelScale
     {
         get
         {
             return RefData == null ? 0 : RefData.modelScale;
         }
     }
     /// <summary>
     /// 初始攻击
     /// </summary>
     public int Att
     {
         get
         {
             return RefData == null ? 0 : RefData.att;
         }
     }
     /// <summary>
     /// 初始命中
     /// </summary>
     public int Hit
     {
         get
         {
             return RefData == null ? 0 : RefData.hit;
         }
     }
     /// <summary>
     /// 初始暴击
     /// </summary>
     public int Cri
     {
         get
         {
             return RefData == null ? 0 : RefData.cri;
         }
     }
     /// <summary>
     /// 移动速度
     /// </summary>
     public int Move_spd
     {
         get
         {
             return RefData == null ? 0 : RefData.move_spd;
         }
     }
     /// <summary>
     /// 步伐幅度
     /// </summary>
     public int Pace_speed
     {
         get
         {
             return RefData == null ? 0 : RefData.pace_speed;
         }
     }
     /// <summary>
     /// AI类型
     /// </summary>
     public int AItype
     {
         get
         {
             return RefData == null ? 0 : RefData.type;
         }
     }
     /// <summary>
     /// 模型资源
     /// </summary>
     public int EquipList
     {
         get
         {
             return RefData == null ? 0 : RefData.equipList;
         }
     } 
    #endregion


     protected void SetTitle()
     {
         string str = string.Empty;
         List<TitleRef> honorList = GameCenter.mercenaryMng.HonorList;
         NewPetDataRef petData = ConfigMng.Instance.GetPetDataRef(GrowUp); 
         if (petData != null)
         {
             int growTitle = petData.cZTitle; 
             for (int i = 0; i < honorList.Count; i++)
             {
                 if (growTitle >= honorList[i].type)
                 { 
                     str = honorList[i].icon;
                 }
             }
             if (str != string.Empty)
             {
                 PetTitleName = str;
                 //Debug.Log("设置宠物称号： " + PetTitleName);
             }
         }
     }

    /// <summary>
    /// 让技能从高品质到低品质排序
    /// </summary>
     private uint exchange = 0;
     private List<uint> CompareByQuality(List<uint> _pet_skill)
     {
         if (_pet_skill.Count > 0)
         {
             for (int j = 0, max = _pet_skill.Count; j < max; j++)
             {
                 for (int i = 0; i < max - j - 1; i++)
                 {
                     NewPetSkillRef skill1 = ConfigMng.Instance.GetPetSkillRef((int)_pet_skill[i]);
                     NewPetSkillRef skill2 = ConfigMng.Instance.GetPetSkillRef((int)_pet_skill[i + 1]);
                     if (skill1 != null && skill2 != null)
                     {
                         if (skill1.quality < skill2.quality)
                         {
                             exchange = _pet_skill[i];
                             _pet_skill[i] = _pet_skill[i + 1];
                             _pet_skill[i + 1] = exchange;
                         }
                     }
                 }
             }
         }
         return _pet_skill;
     }
}
