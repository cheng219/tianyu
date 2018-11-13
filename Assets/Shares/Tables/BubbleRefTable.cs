//=============================
//作者：龙英杰
//日期：2015/11/24
//用途：泡泡文本
//=============================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BubbleRefTable : AssetTable
{
    public List<BubbleRef> infoList = new List<BubbleRef>();
}

[System.Serializable]
public class BubbleRef
{
    /// <summary>
    /// ID
    /// </summary>
    public int id;
    /// <summary>
    /// 对话的间隔时间，单位为秒
    /// </summary>
    public int time;
    /// <summary>
    /// 对话内容
    /// </summary>
    public string content;

}

