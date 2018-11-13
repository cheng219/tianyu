//=========================
//作者：黄洪兴
//日期：2016/05/10
//用途：功能开启静态配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OpenNewFunctionGuideRefTable : AssetTable
{
    public List<OpenNewFunctionGuideRef> infoList = new List<OpenNewFunctionGuideRef>();
}


[System.Serializable]
public class OpenNewFunctionGuideRef
{
    public int type;
    public int id;
    public int step;

    
    /// <summary>
    /// true为黑屏 false不黑屏
    /// </summary>
    public bool TypeOne;
    /// <summary>
    /// true点击目标区域 false点击全屏
    /// </summary>
    public bool TypeTwo;


    public Rect rect;
    public string Effect;
    /// <summary>
    /// 特效坐标
    /// </summary>
    public Vector2 effectPoint;

    public string Button;
    public string Text;
    public Vector2 Textone;
    public Vector2 Texttwo;
    public int Arrow;
    public Vector2 ArrowPoint;
    public bool close;
    /// <summary>
    /// 至下一步延迟几秒
    /// </summary>
    public float time;
    /// <summary>
    /// 茅点 0,不需要换坐标,1为向下的锚点,2为向上的锚点
    /// </summary>
    public int anchor;
} 


