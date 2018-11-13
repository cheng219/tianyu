//=========================
//作者：李邵南
//日期：2017/04/17
//用途：七日挑战静态配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SevenDaysTaskRefTable : AssetTable
{
    public List<SevenDaysTaskRef> infoList = new List<SevenDaysTaskRef>();
}

[System.Serializable]
public class SevenDaysTaskRef
{
    /// <summary>
    /// 任务ID
    /// </summary>
    public int id;
    /// <summary>
    /// 天数
    /// </summary>
    public int day;
    public int task_condition_sort;
    public int task_condition_data;
    public int task_condition_num;
    /// <summary>
    /// 标题
    /// </summary>
    public string des1;
    /// <summary>
    /// 文本描述
    /// </summary>
    public string des2;
    /// <summary>
    /// 是否有寻路前往按钮
    /// </summary>
    public int Action;
    /// <summary>
    /// Ui
    /// </summary>
    public string ui;
    /// <summary>
    /// Ui层级
    /// </summary>
    public int num;
}
