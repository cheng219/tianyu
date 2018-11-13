//======================================================
//作者:鲁家旗
//日期:2016/11/8
//用途:仙侣副本配表
//======================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeddingCoppyRefTable : AssetTable
{
    public List<WeddingCoppyRef> infoList = new List<WeddingCoppyRef>();
}


[System.Serializable]
public class WeddingCoppyRef
{
    /// <summary>
    /// ID
    /// </summary>
    public int id;
    /// <summary>
    /// 描述一
    /// </summary>
    public string des1;
    /// <summary>
    /// 描述二
    /// </summary>
    public string des2;


    public string Des1;
    public string Des2;
    public void SetDes()
    {
        Des1 = des1.Replace(",", "");
        Des2 = des2.Replace(",", "");
    }

}