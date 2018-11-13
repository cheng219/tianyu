//==============================
//作者：李邵南
//日期：2017/3/15
//用途：对话表静态配置
//==============================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogueRefTable : AssetTable
{
    public List<DialogueRef> infoList = new List<DialogueRef>();
}

[System.Serializable]
public class DialogueRef 
{
    /// <summary>
    /// 人物id
    /// </summary>
    public int id;
    /// <summary>
    /// 阶段
    /// </summary>
    public int stage;
    /// <summary>
    /// 图片名
    /// </summary>
    public string icon;
    /// <summary>
    /// 持续时间
    /// </summary>
    public int time;
    /// <summary>
    /// 文本描述
    /// </summary>
    public string text;
    /// <summary>
    /// 头像显示模式  R为右  L为左
    /// </summary>
    public string iconPattern;
    /// <summary>
    /// 姓名
    /// </summary>
    public string name;
}
