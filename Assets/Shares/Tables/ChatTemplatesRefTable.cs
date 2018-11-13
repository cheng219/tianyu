//=========================
//作者：黄洪兴
//日期：2016/7/9
//用途：系统消息静态配置表
//=========================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChatTemplatesRefTable : AssetTable
{
    public List<ChatTemplatesRef> infoList = new List<ChatTemplatesRef>();
}

[System.Serializable]
public class ChatTemplatesRef
{


    public int id;
    public string text;

    public List<int> channel=new List<int>();
    public List<int> parameter = new List<int>();

}
