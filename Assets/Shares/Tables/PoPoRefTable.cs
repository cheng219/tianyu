//======================================================
//作者:鲁家旗
//日期:2016/12/28
//用途:泡泡文本静态配置
//======================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoPoRefTable : AssetTable
{
    public List<PoPoRef> infoList = new List<PoPoRef>();
}
[System.Serializable]
public class PoPoRef
{
    /// <summary>
    /// 对话ID
    /// </summary>
    public int id;
    /// <summary>
    /// 对话的时间间隔
    /// </summary>
    public int time;
    /// <summary>
    /// 对话内容
    /// </summary>
    public string content;
    /// <summary>
    /// 概率
    /// </summary>
    public int probability;
    
}
