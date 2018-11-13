//=========================
//作者：黄洪兴
//日期：2016/5/13
//用途：系统邮件静态配置表
//=========================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SystemMailRefTable : AssetTable
{
    public List<SystemMailRef> infoList = new List<SystemMailRef>();
}

[System.Serializable]
public class SystemMailRef
{


    public int id;
    public string title;
    public string content;
   


}
