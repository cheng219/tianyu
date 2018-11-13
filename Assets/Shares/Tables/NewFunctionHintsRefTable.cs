//=========================
//作者：黄洪兴
//日期：2016/7/8
//用途：新功能预告静态配置表
//=========================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewFunctionHintsRefTable : AssetTable
{
    public List<NewFunctionHintsRef> infoList = new List<NewFunctionHintsRef>();
}

[System.Serializable]
public class NewFunctionHintsRef
{

    public int id;

    public int Task;

    public int Step;

    public string Icon;

    public string des;

    public string text;

    public List<ItemValue> reward = new List<ItemValue>();

    public string title;

    public int modelId;

    public string animName;

    public Vector3 rotation;

    public float modelScale;

    public ObjectType modelType;
}
