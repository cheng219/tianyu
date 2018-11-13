//==============================
//作者：黄洪兴
//日期：2016/3/22
//用途：坐骑表静态配置
//==============================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MountRefTable : AssetTable
{
    public List<MountRef> infoList = new List<MountRef>();
}


[System.Serializable]
public class MountRef
{
    /// <summary>
    /// id
    /// </summary>
	public int mountId;
    /// <summary>
    /// type
    /// </summary>
	public int type;
    /// <summary>
    ///1坐骑，2幻兽
    /// </summary>
	public int kind;
    /// <summary>
    /// 坐骑名字
    /// </summary>
	public string mountName;
    /// <summary>
    /// 解锁改坐骑所需要的坐骑等级
    /// </summary>
    public int level;
    /// <summary>
    /// 模型资源
    /// </summary>
	public string mountModel;
	/// <summary>
	/// 第4级所附加的特效
	/// </summary>
	public string AssetEffect1;
	/// <summary>
	/// 第7级所附加的特效
	/// </summary>
	public string AssetEffect2;
    /// <summary>
    /// 特效1的挂点
    /// </summary>
    public List<string> hangPoint1 = new List<string>();
    /// <summary>
    /// 特效2的挂点
    /// </summary>
    public List<string> hangPoint2 = new List<string>();


	/// <summary>
	/// 解锁该坐骑需要的物品以及数量
	/// </summary>
	public List<ItemValue> item=new List<ItemValue>();
	/// <summary>
	/// 解锁之后给予的时间 永久则填0
	/// </summary>
	public int time;
	/// <summary>
	/// 幻化一次该坐骑所获得的幻化经验
	/// </summary>
	public int explain;
	/// <summary>
	/// 下一阶段解锁的坐骑ID
	/// </summary>
	public int nextLevelId;












	/// <summary>
	/// 介绍
	/// </summary>
	public string text;



	/// <summary>
	/// 喂养的材料ID
	/// </summary>
	public int feedID;



	/// <summary>
	/// 对应移动速度ID
	/// </summary>
	public int speedBuff;




	/// <summary>
	/// 物品ID
	/// </summary>
	public int itemID;




	/// <summary>
	/// 名字高度
	/// </summary>
	public float addNameHigh;



	/// <summary>
	/// 前点
	/// </summary>
	public Vector3 frontPoint;
	/// <summary>
	/// 后点
	/// </summary>
	public Vector3 behindPoint;
	/// <summary>
	/// 预览的时候，模型位置
	/// </summary>
	public Vector3 previewPscale = Vector3.zero;
	/// <summary>
	/// 预览的时候，模型旋转角度
	/// </summary>
	public Vector3 previewRscale = Vector3.zero;
	/// <summary>
	/// 属性类型数组
	/// </summary>
	public List<MountProp> propList = new List<MountProp>();

	public RecastType recastType; 

   /// <summary>
   /// 骑乘挂点
   /// </summary>
    public string riderPoint;

    /// <summary>
    /// 坐骑特效
    /// </summary>
    public List<MountEffect> mountEffectList = new List<MountEffect>();

}
[System.Serializable]
public class MountProp
{
	public ActorPropertyTag tag;
	public int value;
}


public enum RecastType
{
	NONE = 0,
	/// <summary>
	/// 单点射线
	/// </summary>
	SINGLE = 1,
	/// <summary>
	/// 双点射线
	/// </summary>
	DOUBLE = 2,
}



[System.Serializable]
public class MountEffect
{
    public int effectLev;
    public string boneName;
    public string effectName;


    public MountEffect(int _effectLev, string _boneName, string _effectName)
    {
        this.effectLev = _effectLev;
        this.boneName = _boneName;
        this.effectName = _effectName;

    }

    public MountEffect()
    {
    }

}
	