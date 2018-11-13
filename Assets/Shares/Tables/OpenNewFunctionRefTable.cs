//=========================
//作者：黄洪兴
//日期：2016/05/10
//用途：功能开启静态配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OpenNewFunctionRefTable : AssetTable
{
    public List<OpenNewFunctionRef> infoList = new List<OpenNewFunctionRef>();
}


[System.Serializable]
public class OpenNewFunctionRef
{
    public int id;
    public int func_type;
    public int prof;
    public int vip;
    /// <summary>
    /// 1表示接任务 2表示完成任务
    /// </summary>
    public int open_conditions;
    public List<int> open_conditions_data=new List<int>();
    public string open_UI;
    public int UI_Num;
    public string open_flash_iconOne;
    public string open_flash_iconTwo;
    public Vector2 orbit;
    public int where;
    public int appoint_type;
    public int seqencing;
    public string tips;
    public int next;


}


