//======================================================
//作者:鲁家旗
//日期:2016/12/28
//用途:宠物冒泡静态配置
//======================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoPoPetRefTable : AssetTable
{
    public List<PoPoPetRef> infoList = new List<PoPoPetRef>();
}
[System.Serializable]
public class PoPoPetRef
{
    /// <summary>
    /// 索引ID
    /// </summary>
    public int id;
    /// <summary>
    /// 宠物ID
    /// </summary>
    public int petId;
    /// <summary>
    /// 冒泡的类型
    /// </summary>
    public int type;
    /// <summary>
    /// 冒泡的ID
    /// </summary>
    public int poPoId;

}