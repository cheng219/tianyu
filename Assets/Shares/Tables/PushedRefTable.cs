//======================================================
//作者:鲁家旗
//日期:2017/1/24
//用途:推送静态配置
//======================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PushedRefTable : AssetTable
{
    public List<PushedRef> infoList = new List<PushedRef>();
}
[System.Serializable]
public class PushedRef
{
    public int id;
    public int type;
    public string title;
    public List<int> startTime = new List<int>();
    public string res;
}