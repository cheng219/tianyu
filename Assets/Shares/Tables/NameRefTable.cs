//=========================
//作者：黄洪兴
//日期：2016/6/24
//用途：名字静态配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NameRefTable : AssetTable
{
    public List<NameRef> infoList = new List<NameRef>();
}


[System.Serializable]
public class NameRef
{

    public int id;
    public string Firstname;
    public List<string> names = new List<string>();

    





}