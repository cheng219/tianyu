//=========================
//作者：鲁家旗
//日期：2016/10/28
//用途：下载奖励静态配置
//=========================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DownloadBonusRefTable : AssetTable
{
    public List<DownloadBonusRef> infoList = new List<DownloadBonusRef>();
}

[System.Serializable]
public class DownloadBonusRef
{
    /// <summary>
    /// 副本组ID
    /// </summary>
    public int id;
    /// <summary>
    /// 奖励
    /// </summary>
    public List<ItemValue> reward = new List<ItemValue>();

}

