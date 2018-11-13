//=========================
//作者：鲁家旗
//日期：2016/10/26
//用途：时装等级静态配置表
//=========================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FashionLevelRefTable : AssetTable
{
    public List<FashionLevelRef> infoList = new List<FashionLevelRef>();
}

[System.Serializable]
public class FashionLevelRef
{
    /// <summary>
    /// 等级
    /// </summary>
    public int level;
    /// <summary>
    /// 升到下一级所需经验
    /// </summary>
    public int exp;
    /// <summary>
    /// 最大生命值
    /// </summary>
    public int hp;
    /// <summary>
    /// 攻击力
    /// </summary>
    public int att;
    /// <summary>
    /// 防御力
    /// </summary>
    public int def;
    /// <summary>
    /// 暴击
    /// </summary>
    public int cri;
    /// <summary>
    /// 韧性
    /// </summary>
    public int duc;
    /// <summary>
    /// 命中
    /// </summary>
    public int hit;
    /// <summary>
    /// 闪避
    /// </summary>
    public int dgs;
    /// <summary>
    /// 战力
    /// </summary>
    public int gs;

    public List<int> attr = new List<int>();
    public void SetAttr()
    {
        attr.Clear();
        attr.Add(hp);
        attr.Add(att);
        attr.Add(def);
        attr.Add(cri);
        attr.Add(hit);
        attr.Add(dgs);
        attr.Add(duc);
    }
}
