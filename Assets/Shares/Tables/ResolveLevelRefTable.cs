//=========================
//作者：黄洪兴
//日期：2016/05/31
//用途：分解等级静态配置
//=========================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResolveLevelRefTable : AssetTable
{

    public List<ResolveLevelRef> infoList = new List<ResolveLevelRef>();
}

[System.Serializable]
public class ResolveLevelRef
{
    public int id;
    public int Lucky_level;
    public List<ItemValue> dec_item = new List<ItemValue>();
    public List<ResolvePro> pri_pro = new List<ResolvePro>();
    public int kill_pro;
    //public int GS;
    /// <summary>
    /// 幸运值概率显示
    /// </summary>
    public List<ResolvelcckeTest> lccke_Test = new List<ResolvelcckeTest>();
}

[System.Serializable]
public class ResolvelcckeTest
{
    /// <summary>
    /// 描述一
    /// </summary>
    public string str1;
    /// <summary>
    /// 描述二
    /// </summary>
    public string str2;
    public ResolvelcckeTest(string _str1, string _str2)
    {
        str1 = _str1;
        str2 = _str2;
    }
    public ResolvelcckeTest()
    { 
    }
}

[System.Serializable]
public class  ResolvePro
{
    /// <summary>
    /// 物品ID
    /// </summary>
    public int id;
    /// <summary>
    /// 成功概率
    /// </summary>
    public int succeedPro;
    /// <summary>
    /// 不变概率
    /// </summary>
    public int noChangePro;
    /// <summary>
    /// 降一级概率
    /// </summary>
    public int cutdownPro;
    public ResolvePro(int _id, int _succeedPro, int _noChangePro, int _cutdownPro)
    {

        id = _id;
        succeedPro = _succeedPro;
        noChangePro = _noChangePro;
        cutdownPro = _cutdownPro;

    }

    public ResolvePro()
    {
    }



}