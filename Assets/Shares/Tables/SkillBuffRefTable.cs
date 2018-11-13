//=============================
//作者：龙英杰
//日期：2015/7/15
//用途：技能BUFF静态配置数据
//=============================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillBuffRefTable : AssetTable
{

    public List<SkillBuffRef> infoList = new List<SkillBuffRef>();
}

[System.Serializable]
public class SkillBuffRef
{
    /// <summary>
    /// ID
    /// </summary>
    public int id;
    /// <summary>
    /// 客户端是否显示
    /// </summary>
    public int bshow;
    /// <summary>
    /// buff特效播放延迟时间
    /// </summary>
    public int delaytime;
    /// <summary>
    /// 增益还是减益
    /// </summary>
    public int bdamage;
    /// <summary>
    /// 对象战斗属性
    /// </summary>
    public ActorPropertyTag data1;
    /// <summary>
    /// 影响属性
    /// </summary>
    public int data2;
    /// <summary>
    /// 目标数量
    /// </summary>
    public int targetnum;
    /// <summary>
    /// buff优先级
    /// </summary>
    public int buffLevel;
    //public string effect_vision;
    public int effect_pathtype;
    public int buff_model;
    /// <summary>
    /// 动作表现优先级
    /// </summary>
    public int actionLevel;
    /// <summary>
    /// buff名字
    /// </summary>
    public string name;
    /// <summary>
    /// 图标
    /// </summary>
    public string icon;
    /// <summary>
    /// 所在特效文件
    /// </summary>
    public string effect_res;
    /// <summary>
    /// 特效名字
    /// </summary>
    public string effect;
    /// <summary>
    /// BUFF持续期间单位循环播放的动作（覆盖移动动作）
    /// </summary>
    public string buffAction;
    /// <summary>
    /// BUff音效
    /// </summary>
    public string soundBuffRes;
    /// <summary>
    /// 范围参数
    /// </summary>
    public List<int> areaPara = new List<int>();
    /// <summary>
    /// buff效果类型
    /// </summary>
    public BuffType sort;
    /// <summary>
    ///  影响范围
    /// </summary>
    public BuffImpactAreaType impactArea;
    /// <summary>
    /// 切换地图是否清除该BUFF
    /// </summary>
    public BuffMapCleanType mapCleanType;
    /// <summary>
    /// 在该BUFF下是否可以上下坐骑
    /// </summary>
    public BuffRiderEnableType riderEnable;
    /// <summary>
    /// 对BUFF目标的控制类型
    /// </summary>
    public BuffControlSortType controlSort;
    public string buffDes;
}
public enum BuffAttrType
{
	NONE,
	/// <summary>
	/// 增益buff
	/// </summary>
	UP = 1,
	/// <summary>
	/// 减益buff
	/// </summary>
	DOWN = 2,
}

