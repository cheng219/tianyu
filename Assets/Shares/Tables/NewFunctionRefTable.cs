//=========================
//作者：黄洪兴
//日期：2016/05/10
//用途：NPC对话配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewFunctionRefTable : AssetTable
{
    public List<NewFunctionRef> infoList = new List<NewFunctionRef>();
}


[System.Serializable]
public class NewFunctionRef
{
    public int id;
    public string name;
    public int NPC_UI_type;
    public string NPC_UI_name;
    public int UI_type;
    public string UI_name;
    public int Function_type;


}


