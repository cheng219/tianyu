//==============================
//作者：龙英杰
//日期：2016/11/4
//用途：uilabel翻译
//==============================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UILabelTranslationRefTable : AssetTable
{
    public List<UILabelTranslationProp> infoList = new List<UILabelTranslationProp>();


}



[System.Serializable]
public class UILabelTranslationProp
{
    /// <summary>
    /// des
    /// </summary>
    public string des;
    /// <summary>
    /// 译文
    /// </summary>
    public string translation;
}