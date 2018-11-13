//=====================================
//作者：易睿
//日期：2015/5/25
//用途：JL物品系统数据
//==========================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class EquipmentRefTable : AssetTable
{
    public List<EquipmentRef> infoList = new List<EquipmentRef>();
}


[System.Serializable]
public class EquipmentRef
{
    /// <summary>
    /// ID
    /// </summary>
    public int id;
    /// <summary>
    /// 最大堆叠数量
    /// </summary>
    public int max;
    /// <summary>
    /// 名字
    /// </summary>
    public string name;
    /// <summary>
    /// 需求职业
    /// </summary>
    public int req_prof;
    /// <summary>
    /// 需求等级
    /// </summary>
    public int req_lv;
    /// <summary>
    /// 属性
    /// </summary>
    public List<AttributePair> attributePairList = new List<AttributePair>();
    /// <summary>
    /// 描述
    /// </summary>
    public string des;
    /// <summary>
    /// 绑定类型
    /// </summary>
    public int bind;
    /// <summary>
    /// 能否出售
    /// </summary>
    public int business;
    /// <summary>
    /// 能否丢弃
    /// </summary>
    public int discard;
    /// <summary>
    /// 边框颜色
    /// </summary>
    public int quality;
    /// <summary>
    /// action参数，增加体力数量
    /// </summary>
    public string action_arg;
    /// <summary>
    /// 价格
    /// </summary>
    public int price;
    /// <summary>
    /// 
    /// </summary>
    public int oldSort;
    /// <summary>
    /// 装备基础分数
    /// </summary>
    public int gs;
    /// <summary>
    /// 是否可以十连开
    /// </summary>
    public int multi_open;
    /// <summary>
    /// 快捷购买的钻石价格填0为不可快捷购买
    /// </summary>
	public float diamonPrice;
    /// <summary>
    /// 最大强化等级
    /// </summary>
    public int maxPowerLv;
    /// <summary>
    /// 对应后台属性ID，没有则为0
    /// </summary>
    public int serverResId;
    /// <summary>
    /// 属性等级
    /// </summary>
    public int propLv;
    /// <summary>
    /// 装备模型
    /// </summary>
    public string equip_model;
    /// <summary>
    /// 图标资源名
    /// </summary>
    public string item_res;
    /// <summary>
    /// 职业数目
    /// </summary>
	public string profNum;
    /// <summary>
    /// 战斗挂点名字
    /// </summary>
    public string bPoint;
    /// <summary>
    /// 非战斗挂点名字
    /// </summary>
    public string rPoint;
    /// <summary>
    /// 地面显示模型
    /// </summary>
    public string dropModel;
    /// <summary>
    /// 地面显示特效
    /// </summary>
    public string dropEffect;
    /// <summary>
    /// 服务端物品脚本
    /// </summary>
    public EquipActionType action;
    /// <summary>
    /// 道具类别
    /// </summary>
    public EquipmentFamily family;
    /// <summary>
    /// 装备部位
    /// </summary>
    public EquipSlot slot;
    /// <summary>
    /// 挂点还是蒙皮
    /// </summary>
    public ShowType showType;
    /// <summary>
    /// 物品是否可回收
    /// </summary>
    public GoodsRecycleType recycleType;
    /// <summary>
    /// 物品是否需要获得提示
    /// </summary>
    public GoodsAttentionType attentionType;
    /// <summary>
    /// 预览缩放
    /// </summary>
    public float previewScale;
    public Vector3 previewPositon;
    public Vector3 previewRotation;
    /// <summary>
    /// 特效
    /// </summary>
    public List<BoneEffectRef> boneEffectList = new List<BoneEffectRef>();
    /// <summary>
    /// 合成的物品
    /// </summary>
	public List<int> conditionsItem = new List<int>();
    /// <summary>
    /// 合成物品需要的数目
    /// </summary>
	public List<int> conditionsCount = new List<int>();
    /// <summary>
    /// 附加属性的类型
    /// </summary>
    public List<int> attrType = new List<int>();
    /// <summary>
    /// 掉落音效
    /// </summary>
    public string soundDropRes;
	/// <summary>
	/// 宠物技能的类型
	/// </summary>
	public int petSkillType;
	/// <summary>
	/// 宠物技能的级别
	/// </summary>
	public int psetSkillLevel;
    /// <summary>
    /// 使用后打开的UI
    /// </summary>
    public int openUiType;
    //public List<int> access = new List<int>();

    /// <summary>
    /// 冷却信息
    /// </summary>
    public CDValue use_cd;

    /// <summary>
    /// 获取提示
    /// </summary>
    public string attention;

    /// <summary>
    /// 宝石阶层
    /// </summary>
    public int gemLevel;


}
public enum ShowType
{
    /// <summary>
    /// 挂点
    /// </summary>
    HANGINGPOINT,
    /// <summary>
    /// 蒙皮
    /// </summary>
    SKIN,
    /// <summary>
    /// 无效果
    /// </summary>
    NO,
}



[System.Serializable]
public class CDValue
{
    public int id;
    public int time;
   
    public CDValue(int _id,int _time)
    {
        this.id = _id;
        this.time = _time;
    }
    public CDValue()
    {

    }
}





[System.Serializable]
public class AttributePair
{
    public ActorPropertyTag tag;
    public int value;
	public AttributePair(){}
	public AttributePair(AttributePair _temp)
	{
		this.tag = _temp.tag;
		this.value = _temp.value;
	}
	public AttributePair(ActorPropertyTag _tag,int _value)
	{
		this.tag = _tag;
		this.value = _value;
	}
    public void Update(ActorPropertyTag _tag, int _value)
    {
        if (_tag == this.tag)
        {
            this.value = _value;
        }
    }
}


[System.Serializable]
public class BoneEffectRef
{
    public string boneName;
    public string effectName;
    public BoneEffectRef() { }
    public BoneEffectRef(string _boneName, string _effectName)
    {
        this.boneName = _boneName;
        this.effectName = _effectName;
    }
}

//[System.Serializable]
//public class Attention
//{
//    public int isRight;
//    public string des;
//    public int id;
//    public Attention()
//    {
    
//    }
//    public Attention(int _isRight, string _des, int _id) 
//    {
//        this.isRight = _isRight;
//        this.des = _des;
//        this.id = _id;
//    }
//    public Attention(int _isRight, string _des)
//    {
//        this.isRight = _isRight;
//        this.des = _des;
//    }
//}


