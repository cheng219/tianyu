//---------------------------------------------
//作者：龙英杰
//时间：2015.7.27
//用途：阵营设计
//---------------------------------------------


using UnityEngine;
using System.Collections.Generic;

public class RelationRefTable : AssetTable
{
    public List<RelationRef> infoList = new List<RelationRef>();
}

[System.Serializable]
public class RelationRef
{


    public struct RelationKey 
    {
        public int camp1;
        public int camp2;
        public SceneType sceneType;
        public RelationKey(SceneType _type, int _camp1, int _camp2)
        {
            camp1 = _camp1;
            camp2 = _camp2;
            sceneType = _type;
        }
    }

    public int camp1;
    
    public List<RelationCompareRef> compareList = new List<RelationCompareRef>();
    protected FDictionary compareDic = new FDictionary();
    /// <summary>
    /// 初始化数据
    /// </summary>
    public void InitData()
    {
        for (int i = 0; i < compareList.Count; i++)
        {
            RelationCompareRef refData = compareList[i];
            compareDic[new RelationKey(refData.sceneType, refData.camp1, refData.camp2)] = refData;
        }
    }


    public RelationCompareRef GetCompareRelation(SceneType _sceneType, int _camp1, int _camp2)
    {
        RelationKey key = new RelationKey(_sceneType, _camp1, _camp2);
        if (compareDic.ContainsKey(key))
        {
            return compareDic[key] as RelationCompareRef;
        }
        return null;

    }
}


[System.Serializable]
public class RelationCompareRef
{
    /// <summary>
    /// 阵营1
    /// </summary>
    public int camp1;
    /// <summary>
    /// 阵营2
    /// </summary>
    public int camp2;
    /// <summary>
    /// 阵营关系
    /// </summary>
    public RelationType relation;
    /// <summary>
    /// 场景类型
    /// </summary>
    public SceneType sceneType;
    /// <summary>
    /// 名称颜色
    /// </summary>
    public Color color;
    /// <summary>
    /// 名字外框颜色
    /// </summary>
    public Color colSide;
}

/// <summary>
/// 阵营的攻击类型
/// 1 可攻击
/// 2 不主动攻击
/// </summary>
public enum RelationType
{
    /// <summary>
    /// 默认不可攻击
    /// </summary>
    NO_ATTAK = 0,
    /// <summary>
    /// 可攻击
    /// </summary>
    AUTOMATEDATTACKS = 1,
    /// <summary>
    /// 不主动攻击
    /// </summary>
    NOAUTOMATEDATTACKS = 2,
}

