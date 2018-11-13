//==================================
//作者：朱素云
//日期：2016/3/7
//用途：随从管理类
//=================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;

public class MercenaryMng
{ 
    #region 数据
    /// <summary>
    /// 数据是否已经经过服务端修正
    /// </summary>
    protected bool hasServerFixed = false;
    /// <summary>
    /// 没有宠物时id用-1表示
    /// </summary>
    public const int noPet = -1; 
    /// <summary>
    /// 当前点击宠物id
    /// </summary> 
    public int curPetId = noPet;
    /// <summary>
    /// 是否打开融合界面用于使用宠物蛋，打开融合界面使用宠物蛋增加经验
    /// </summary>
    public bool isOpenMixWndAndUseEgg = false;
    /// <summary>
    /// 当前使用宠物蛋增加资质的宠物id
    /// </summary>
    public int curUseEggPetId = noPet;
    protected MercenaryInfo curPetInfo = null; 
     /// <summary>
    /// 当前出战的宠物
    /// </summary> 
    public MercenaryInfo curMercernaryInfo
    {
        get
        {
            return curPetInfo;
        }
        protected set
        {
            if (curPetInfo != value)
            {
                curPetInfo = value; 
                if (OnPetUpdate != null)
                {
                    OnPetUpdate();
                }
            }
        }
    } 
    /// <summary>
    /// 当前主宠
    /// </summary>
    public int zhuPetId = noPet;
    /// <summary>
    /// 当前副宠
    /// </summary>
    public int fuPetId = noPet;
    /// <summary>
    /// 可以放技能的空巢
    /// </summary>
    public int emptyNest = 0; 
    /// <summary>
    /// 宠物技能书合成
    /// </summary>
    public bool isComposedBook = false; 
    /// <summary>
    /// 技能书字典（通过批量抄写获得，key:服务端唯一id,value:技能id）
    /// </summary>
    public Dictionary<int,int> petSkillByCopyAll = new Dictionary<int,int>();
    /// <summary>
    /// 玩家背包中所有的技能书(key :技能书id,,,value:技能书数量)
    /// </summary>
    public Dictionary<int, int> allPetSkillBook = new Dictionary<int, int>();
    /// <summary>
    /// 背包中的技能书(存放技能书的id)(只有高级和顶级的)
    /// </summary>
    public List<int> choosedPetSkill = new List<int>();
    /// <summary>
    /// 选择技能书放入材料框合成技能的材料链表，存放技能书的id
    /// </summary>
    public List<int> choosedBookInMat = new List<int>();  
    /// <summary>
    /// 所有宠物数据
    /// </summary> 
    public FDictionary mercenaryInfoList = new FDictionary();
    /// <summary>
    /// 根据宠物id获取宠物数据
    /// </summary> 
    public MercenaryInfo GetMercenaryById(int _id)
    { 
        MercenaryInfo info = mercenaryInfoList[_id] as MercenaryInfo; 
        return info;   
    } 
    /// <summary>
    /// 玩家背包
    /// </summary>
    protected Dictionary<int, EquipmentInfo> Bag
    {
        get
        {
            return GameCenter.inventoryMng.BackPackDictionary;
        }
    }
    protected List<TitleRef> honorList = new List<TitleRef>();
    /// <summary>
    /// 宠物称号
    /// </summary>
    public List<TitleRef> HonorList
    {
        get
        {
            honorList.Clear();
            List<TitleRef> list = ConfigMng.Instance.TitlesList();//头衔表中所有头衔
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].type == 25 || list[i].type == 26 || list[i].type == 27 || list[i].type == 28)
                    honorList.Add(list[i]);
            }  
            return honorList;
        }
    }
    /// <summary>
    /// 热感选择用于技能合成
    /// </summary>
    public EquipmentInfo seleteEquip;
    /// <summary>
    /// 玩家抄写后选中的技能书id
    /// </summary>
    public int choosedSkillId = 0;
    /// <summary>
    /// 选择消耗的宠物蛋
    /// </summary>
    public EquipmentInfo seleteEggToMix;
    #endregion

    #region 事件

    /// <summary>
    /// 更新宠物列表事件
    /// </summary>
    public System.Action OnMercenaryListUpdate;
    /// <summary>
    /// 宠物成长提升
    /// </summary>
    public System.Action OnPetGrowUpUpdate;
    /// <summary>
    /// 灵修等级提升
    /// </summary>
    public System.Action<LingXiuType> OnLingXiuLevUpdate;
    /// <summary>
    /// 灵修经验提升
    /// </summary>
    public System.Action<LingXiuType> OnLingXiuExpUpdate;
    /// <summary>
    /// 灵修星星爆炸
    /// </summary>
    public System.Action<LingXiuType> OnLingXiuStarUpdate;
    /// <summary>
    /// 显示副宠信息
    /// </summary>
    public System.Action OnFuPetUpdate;
    /// <summary>
    /// 显示主宠信息
    /// </summary>
    public System.Action OnZhuPetUpdate;
    /// <summary>
    /// 融合
    /// </summary>
    public System.Action OnMixUpdate;
    /// <summary>
    /// 抄写
    /// </summary>
    public System.Action OnCopyBookUpdate;
    /// <summary>
    /// 选择技能书(用于技能合成)
    /// </summary>
    public System.Action OnSeleteUpdate; 
    /// <summary>
    /// 宠物更新
    /// </summary>
    public System.Action OnPetUpdate;

    /// <summary>
    /// 用于激活新模型的时候刷新模型展示界面
    /// </summary>
    public System.Action<MercenaryInfo> OnGetNewPetUpdate;
    #endregion
     
    #region 构造
    public static MercenaryMng CreateNew()
    {
        if (GameCenter.mercenaryMng == null)
        { 
            MercenaryMng mercenaryMng = new MercenaryMng();
            mercenaryMng.Init();
            return mercenaryMng;
        }
        else
        {
            GameCenter.mercenaryMng.UnRegist();
            GameCenter.mercenaryMng.Init();
            return GameCenter.mercenaryMng;
        }
    } 
    /// <summary>
    /// 注册
    /// </summary>
    void Init()
    { 
        MsgHander.Regist(0xD402, S2C_GetAllMercenayDataList);
        MsgHander.Regist(0xD404, S2C_FixMercenaryInfo);
        MsgHander.Regist(0xD409, S2C_GetMercenaryPromote);
        MsgHander.Regist(0xD410, S2C_GetUpdataAfterChange);
        MsgHander.Regist(0xD426, S2C_GetPetNewName);
        MsgHander.Regist(0xD751, S2C_GetCopyBook);
        MsgHander.Regist(0xD01F, S2C_EntrouageAwake);
    }
    /// <summary>
    /// 注销
    /// </summary>
    void UnRegist()
    {
        MsgHander.UnRegist(0xD402, S2C_GetAllMercenayDataList);
        MsgHander.UnRegist(0xD404, S2C_FixMercenaryInfo);
        MsgHander.UnRegist(0xD409, S2C_GetMercenaryPromote);
        MsgHander.UnRegist(0xD410, S2C_GetUpdataAfterChange);
        MsgHander.UnRegist(0xD426, S2C_GetPetNewName);
        MsgHander.UnRegist(0xD751, S2C_GetCopyBook);
        MsgHander.UnRegist(0xD01F, S2C_EntrouageAwake); 
        mercenaryInfoList.Clear(); 
        petSkillByCopyAll.Clear();
        allPetSkillBook.Clear();
        choosedBookInMat.Clear(); 
        curPetId = noPet;
        zhuPetId = noPet;
        fuPetId = noPet;
        curUseEggPetId = noPet;
        emptyNest = 0;
        choosedSkillId = 0;
		curPetInfo = null;
    }  
    /// <summary>
    /// 筛选达到条件的宠物个数
    /// </summary> 
    public int GetPetGrowUp(PetPropertyType _type, int _sise)
    { 
        int  count = 0;
        foreach(MercenaryInfo info in mercenaryInfoList.Values)
        {
            if (_type == PetPropertyType.GROWUP)
            {
                if (info.GrowUp >= _sise)
                    ++count;
            }
            else if (_type == PetPropertyType.APTITUDE)
            {
                if (info.Aptitude >= _sise)
                    ++count;
            }
        }
        return count;
    }
    /// <summary>
    /// 宠物红点提示有物品就亮红点
    /// </summary>
    public void SetRedRemind()
    {    
        if (mercenaryInfoList.Count <= 0)
        { 
            GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.PETGROWUP, false);
            GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.PETTHEKING, false);
            GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.PETFUSE, false); 
        }
        else
        {
            //成长红点
            bool isGrowUpCoinEnough = false;
            if (curMercernaryInfo != null)
            {
                List<ItemValue> growItem = curMercernaryInfo.GrowUpItem; 
                if (growItem.Count > 0)
                {
                    for (int i = 0; i < growItem.Count; i++)
                    {
                        if (growItem[i].eid == 5)
                        { 
                            if (GameCenter.mainPlayerMng.MainPlayerInfo.TotalCoinCount >= (ulong)growItem[i].count)
                            {
                                isGrowUpCoinEnough = true;
                            } 
                        }
                    }
                }
 
                if (isGrowUpCoinEnough && GameCenter.inventoryMng.GetNumberByType((int)ItemRedRemind.PETFOOD) >= 1)//宠物成长
                {
                    if (curMercernaryInfo.GrowUp < (ConfigMng.Instance.GetPetDataRefTable.Count - 1))
                        GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.PETGROWUP, true);
                    else
                        GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.PETGROWUP, false);
                }
                else
                {
                    GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.PETGROWUP, false);
                }

                //灵修红点
                List<ItemValue> LXItem = curMercernaryInfo.LxItem;
                bool isLingXiuCoinEnough = false;
                if (LXItem.Count > 0)
                {
                    for (int i = 0; i < LXItem.Count; i++)
                    {
                        if (LXItem[i].eid == 5)
                        {
                            if (GameCenter.mainPlayerMng.MainPlayerInfo.TotalCoinCount < (ulong)LXItem[i].count)
                            {
                                isLingXiuCoinEnough = true;

                            }
                        } 
                    }
                }
                 
                if (isLingXiuCoinEnough && GameCenter.inventoryMng.GetNumberByType((int)ItemRedRemind.LINGXIUPILL) >= 1)//灵修
                {
                    if ((curMercernaryInfo.Tian_soul < 32 || curMercernaryInfo.Di_soul < 32 || curMercernaryInfo.Life_soul < 32))
                        GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.PETTHEKING, true);
                    else
                        GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.PETTHEKING, false);
                }
                else
                {
                    GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.PETTHEKING, false);
                }

                //融合红点
                if (GameCenter.inventoryMng.GetNumberByType((int)ItemRedRemind.APTITUDEPILLPRIMARY) >= 1 ||
                    GameCenter.inventoryMng.GetNumberByType((int)ItemRedRemind.APTITUDEPILLMIDDLE) >= 1 ||
                    GameCenter.inventoryMng.GetNumberByType((int)ItemRedRemind.APTITUDEPILLSENOIR) >= 1)//融合
                {
                    if (curPetInfo.Aptitude < 100)
                        GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.PETFUSE, true);
                    else
                        GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.PETFUSE, false);
                }
                else
                {
                    GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.PETFUSE, false);
                }
            }
        } 
    }
    #endregion 

    #region 通信
    #region S2C 
    /// <summary>
    /// 得到宠物链表信息
    /// </summary> 
    protected void S2C_GetAllMercenayDataList(Pt _msg)
    { 
        pt_pet_list_d402 msg = _msg as pt_pet_list_d402;
        if (msg != null)
        { 
            for (int i = 0; i < msg.pet_list.Count; i++)
            {
                PetData data = new PetData(msg.pet_list[i]);
                //Debug.Log("发来的宠物serverInstanceID: " + data.configId + ", 宠物type: " + data.type + " , 宠物状态： " + data.status + " , 战力：" + data.fight_score + " , 天魂： " + data.tian_soul + " ， 地魂 ： " + data.di_soul + " , 命魂 ： " + data.life_soul); 
                //Debug.Log("  allPetCount :   " + msg.pet_list.Count + "   , name : " + data.pet_name + "    , id : " + data.configId); 
                //for (int j = 0; j < data.pet_skill.Count; j++)
                //{
                //    Debug.Log(" skillid :  " + data.pet_skill[j] + data.configId);
                //}
                    
                if (mercenaryInfoList.ContainsKey((int)data.configId))
                {
                    MercenaryInfo info = mercenaryInfoList[(int)data.configId] as MercenaryInfo;
                    if (info != null)
                    {
						info.Update(data,info.ServerInstanceID);
                        if (curMercernaryInfo != null && curMercernaryInfo.ConfigId == (int)data.configId)
                        {
                            if (info.IsActive != (int)PetChange.FINGHTING)
                                curMercernaryInfo = null;
                        }
                        else
                        {
                            if (info.IsActive == (int)PetChange.FINGHTING) curMercernaryInfo = info;
                        }
                    } 
                } 
                else
                {
                    MercenaryInfo info = new MercenaryInfo(data, GameCenter.mainPlayerMng.MainPlayerInfo);
                    mercenaryInfoList[info.ConfigId] = info;
                    if (OnGetNewPetUpdate != null) OnGetNewPetUpdate(info);
                    if (curMercernaryInfo != null && curMercernaryInfo.ConfigId == (int)data.configId)
                    {
                        if (info.IsActive != (int)PetChange.FINGHTING)
                            curMercernaryInfo = null;
                    }
                    else
                    {
                        if (info.IsActive == (int)PetChange.FINGHTING) curMercernaryInfo = info;
                    }
                }

                if (data.status == (int)PetChange.FINGHTING)
                {
                    curPetId = data.configId;
                }
            } 
            if (msg.pet_list.Count > 0)
            {
                SetRedRemind();
                if (curPetId == noPet)
                {
                    curPetId = (int)msg.pet_list[0].id;//默认当前展示的id为第一个  
                }
            } 
        } 
        //抛出数据变化的事件
        if (OnMercenaryListUpdate != null)
        {
            OnMercenaryListUpdate();
        }
    }
    /// <summary>
    /// 返回宠物最新状态和宠物技能合成状态
    /// </summary> 
    protected void S2C_FixMercenaryInfo(Pt _msg)
    {
        pt_pet_updata_state_d404 msg = _msg as pt_pet_updata_state_d404; 
        if (msg != null)
        { 
            if (msg.state == (int)PetChange.COMPODESKILLBOOK)
            {
                isComposedBook = true; 
            }
            else
            { 
                if (mercenaryInfoList.ContainsKey(msg.pet_type))
                {
                    MercenaryInfo info = mercenaryInfoList[msg.pet_type] as MercenaryInfo; 
                    if (info != null)
                    { 
                        if (curMercernaryInfo != null && curMercernaryInfo.ConfigId == msg.pet_type)
                        {
                            if (msg.state != (int)PetChange.FINGHTING)
                                curMercernaryInfo = null;
                        }
                        else
                        {
                            if (msg.state == (int)PetChange.FINGHTING)
                                curMercernaryInfo = info;
                        }
                        info.UpdateAfterChange(msg);
                    }
                }
                if(curPetId == noPet && mercenaryInfoList.Count <= 0)
                    curMercernaryInfo = null;
            }
            SetRedRemind();
        }
        //抛出数据变化的事件
        if (OnMercenaryListUpdate != null)
        {
            OnMercenaryListUpdate();
        }
    }
    /// <summary>
    ///  返回宠物属性变化信息
    /// </summary>
    protected void S2C_GetMercenaryPromote(Pt _msg)
    { 
        pt_pet_updata_property_d409 msg = _msg as pt_pet_updata_property_d409; 
        if (msg != null)
        {
            MercenaryInfo info = null;
            if (mercenaryInfoList.ContainsKey(msg.pet_type))
            {
                info = mercenaryInfoList[msg.pet_type] as MercenaryInfo;
            }
            if (info != null) info.UpdateAfterChange(msg);
            SetRedRemind();
        }
        //抛出数据变化的事件
        if (OnMercenaryListUpdate != null)
        {
            OnMercenaryListUpdate();
        } 
    }
    /// <summary>
    /// 融合的反馈协议
    /// </summary> 
    protected void S2C_GetUpdataAfterChange(Pt _msg)
    {
        pt_fuse_info_d410 msg = _msg as pt_fuse_info_d410;
        if(msg != null)
        { 
            MercenaryInfo info = null;
            if (mercenaryInfoList.ContainsKey(msg.pet_type))
            {
                 info = mercenaryInfoList[msg.pet_type] as MercenaryInfo;
            }
            if(info!=null) info.UpdateAfterChange(msg);
            SetRedRemind();
        } 
        if (OnMixUpdate != null)
        {
            OnMixUpdate();
        }
    } 
    /// <summary>
    /// 改名字反馈协议
    /// </summary>
    protected void S2C_GetPetNewName(Pt _msg)
    {
        pt_update_pet_name_d426 msg = _msg as pt_update_pet_name_d426;
        MercenaryInfo info = null;
        if (mercenaryInfoList.ContainsKey(msg.pet_type))
        {
            info = mercenaryInfoList[msg.pet_type] as MercenaryInfo;
        }
        if (info != null) info.UpdateAfterChange(msg); 
    }
    /// <summary>
    /// 抄写协议
    /// </summary> 
    protected void S2C_GetCopyBook(Pt _msg)
    {
        pt_rand_box_reward_d751 msg = _msg as pt_rand_box_reward_d751;
        if (msg != null)
        {
            if (msg.rand_box_reward.Count == 1 || msg.rand_box_reward.Count == 10)//重新抄写清空
            {
                choosedSkillId = 0;
                petSkillByCopyAll.Clear();
            }  
            for (int i = 0; i < msg.rand_box_reward.Count; i++)
            {
                int serverId = (int)msg.rand_box_reward[i].id;
                int skillId = (int)msg.rand_box_reward[i].type; 
                petSkillByCopyAll[serverId] = skillId;
                //Debug.Log("serverId：  " + serverId + "   , skillId : " + skillId);
            } 
        }
        if (OnCopyBookUpdate != null)
        {
            OnCopyBookUpdate();
        }
    }
    /// <summary>
    /// 随从激活,开始行动 by吴江
    /// </summary>
    protected void S2C_EntrouageAwake(Pt _pt)
    {
        pt_entourage_create_d01f msg = _pt as pt_entourage_create_d01f;
        if (msg != null)
        { 
            if (curMercernaryInfo != null)
            { 
                curMercernaryInfo.Update(msg); 
            }
        }
    }
    #endregion

    #region C2S
    /// <summary>
    ///请求所有宠物列表
    /// </summary>
    public  void C2S_ReqMercenaryDataList()
    { 
        pt_req_all_pet_list_d415 msg = new pt_req_all_pet_list_d415();
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 403
    /// </summary>
    public void C2S_ReqMercenaryInfo(PetChange _data, int _id)
    {
        //Debug.Log("  d403 :  " + _data + "   ,id : " + _id);
        pt_req_pet_info_d403 msg = new pt_req_pet_info_d403();
        msg.req_state = (uint)_data;
        msg.pet_type = (uint)_id;
        NetMsgMng.SendMsg(msg); 
    }
    /// <summary>
    ///D405
    /// </summary> 
    public void C2S_ReqPromote(PetChange _data, int _id, int _isQuikBuy)
    { 
        pt_req_pet_three_int_d405 msg = new pt_req_pet_three_int_d405();
        msg.req_state = (uint)_data;
        msg.parameter_1 = (uint)_id;
        msg.parameter_2 = (uint)_isQuikBuy;
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 请求使用宠物蛋
    /// </summary> 
    public void C2S_ReqUseEgg(int _id, int _eggId, int _egg)
    {
        //Debug.Log(" 请求使用宠物蛋  petid : " + _id + "   eggId : " + _eggId + "     eggnum : " + _egg); 
        pt_action_pet_egg_add_exp_c106 msg = new pt_action_pet_egg_add_exp_c106();
        msg.pet_id = _id;
        msg.pet_egg_type = _eggId;
        msg.pet_egg_num = _egg;
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 改名字请求协议 D406{宠物type1，宠物名字} 
    /// </summary>
    public void C2S_ReqChangeName(int _id, string _newName)
    {
        pt_req_pet_change_name_d406 msg = new pt_req_pet_change_name_d406();
        msg.pet_type = (uint)_id;
        msg.new_name = _newName;
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 技能合成请求协议
    /// </summary> 
    public void C2S_ReqComposeSkill(int _itemId, List<int> _matList)
    { 
        pt_pet_skill_compound_d407 msg = new pt_pet_skill_compound_d407();
        msg.need_compound_item = _itemId;
        msg.skill_compound_item = _matList;
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 请求添加到背包
    /// </summary> 
    public void C2S_ReqAddBookToBag(int _id)
    {
        pt_req_rand_box_reward_d752 msg = new pt_req_rand_box_reward_d752();
        msg.id = _id;
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 随从激活,请求开始行动 by吴江
    /// </summary>
    public void C2S_EntrouageAwake()
    { 
        pt_entourage_create_d01f msg = new pt_entourage_create_d01f();
        NetMsgMng.SendMsg(msg);
    }
    #endregion
    #endregion

    #region 辅助逻辑 

    /// <summary>
    /// 获取背包中所以的宠物蛋
    /// </summary>
    /// <returns></returns>
    public List<EquipmentInfo> GetPetEggFromBag()
    {
        List<EquipmentInfo> list = new List<EquipmentInfo>();
        using (var info = Bag.GetEnumerator())
        {
            while (info.MoveNext())
            {
                if (info.Current.Value.ActionType == EquipActionType.activate_animal)//宠物蛋
                {
                    list.Add(info.Current.Value);
                }
            }
        }
        return list;
    }

    /// <summary>
    /// 从玩家背包获取技能书
    /// </summary>
    public void GetBookFromBag()
    {
        allPetSkillBook.Clear();
        choosedPetSkill.Clear();
        using (var info = Bag.GetEnumerator())
        {
            while (info.MoveNext())
            {
                if (info.Current.Value.BookLev > 0 &&
                    !allPetSkillBook.ContainsKey(info.Current.Value.EID) &&
                    info.Current.Value.StackCurCount > 0)
                {
                    if (!allPetSkillBook.ContainsKey(info.Current.Value.EID))
                    {
                        if (info.Current.Value.BookLev >= 3)
                        {
                            choosedPetSkill.Add(info.Current.Value.EID);
                        }
                        allPetSkillBook[info.Current.Value.EID] = GameCenter.inventoryMng.GetNumberByType(info.Current.Value.EID);
                    }
                }
            }
        }
        CompareByQuality(choosedPetSkill);
    }
 
    /// <summary>
    /// 让技能书从高品质到低品质排序
    /// </summary> 
    public List<int> CompareByQuality(List<int> _pet_skill)
    {
        int exchange = 0;
        if (_pet_skill.Count > 0)
        {
            for (int j = 0, max = _pet_skill.Count; j < max; j++)
            {
                for (int i = 0; i < max - j - 1; i++)
                {
                    EquipmentRef skill1 = ConfigMng.Instance.GetEquipmentRef(_pet_skill[i]);
                    EquipmentRef skill2 = ConfigMng.Instance.GetEquipmentRef(_pet_skill[i + 1]);
                    if (skill1 != null && skill2 != null)
                    {
                        if (skill1.psetSkillLevel < skill2.psetSkillLevel)
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
    #endregion  
}

#region 宠物枚举
public enum PetProperty
{ 
    /// <summary>
    /// 攻击
    /// </summary>
    PETATT = 1,
    /// <summary>
    /// 命中
    /// </summary>
    PETHIT = 7,
    /// <summary>
    /// 暴击
    /// </summary>
    PETCRI = 9,
}
public enum PetPropertyType
{ 
    /// <summary>
    /// 成长
    /// </summary>
    GROWUP,
    /// <summary>
    /// 资质
    /// </summary>
    APTITUDE,
}
public enum PetChange
{  
    /// <summary>
    /// 没有状态
    /// </summary>
    NONESTATE = 0,
    /// <summary>
    /// 出战
    /// </summary>
    FINGHTING = 1,
    /// <summary>
    /// 守护
    /// </summary>
    GUARD = 2,
    /// <summary>
    /// 放生
    /// </summary>
    FREE = 3,
    /// <summary>
    /// 休息
    /// </summary>
    REST = 4,
    /// <summary>
    /// 成长提升
    /// </summary>
    GROWUP = 5,
    /// <summary>
    /// 修炼天魂
    /// </summary>
    PRACTICETIANSOUL = 6,
    /// <summary>
    /// 提升资质经验
    /// </summary>
    APTITUDEEXPUP = 7,
    /// <summary>
    /// 融合
    /// </summary>
    FUSE = 8,
    /// <summary>
    /// 改名字
    /// </summary>
    CHANGENAME = 9,
    /// <summary>
    /// 取消守护
    /// </summary>
    CANCELGUARD = 10,
    /// <summary>
    /// 封印技能
    /// </summary>
    SEALSKILL = 11,
    /// <summary>
    /// 遗忘技能
    /// </summary>
    FORGETSKILL = 12,
    /// <summary>
    /// 学习技能
    /// </summary>
    STUDYSKILL = 13,
    /// <summary>
    /// 抄写
    /// </summary>
    COPY = 14,
    /// <summary>
    /// 批量抄写
    /// </summary>
    COPYALL = 15,
    /// <summary>
    /// 技能合成
    /// </summary>
    COMPODESKILLBOOK = 16,
    /// <summary>
    /// 修炼地魂
    /// </summary>
    PRACTICEDISOUL = 17,
    /// <summary>
    /// 修炼命魂
    /// </summary>
    PRACTICELIFESOUL = 18,
    /// <summary>
    /// 使用初级资质丹
    /// </summary>
    USEPRIMERYPILL = 19,
    /// <summary>
    /// 使用中级资质丹
    /// </summary>
    USEMIDDLEPILL = 20,
    /// <summary>
    /// 使用高级资质丹
    /// </summary>
    USESENIORPILL = 21, 
}
/// <summary>
/// 有物品就亮红点的物品id
/// </summary>
public enum ItemRedRemind
{ 
    /// <summary>
    /// 宠物口粮
    /// </summary>
    PETFOOD = 2200004,
    /// <summary>
    /// 灵修丹
    /// </summary>
    LINGXIUPILL = 2200005,
    /// <summary>
    /// 初级资质丹
    /// </summary>
    APTITUDEPILLPRIMARY = 2200006,
    /// <summary>
    /// 中级资质丹
    /// </summary>
    APTITUDEPILLMIDDLE = 2200007,
    /// <summary>
    /// 高级资质丹
    /// </summary>
    APTITUDEPILLSENOIR = 2200008,
}
/// <summary>
/// 灵修类型
/// </summary>
public enum LingXiuType
{
    TIAN,
    DI,
    LIFE,
    NONE,
}
#endregion
