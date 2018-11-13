//=========================
//作者：贺丰
//日期：2016/2/27
//用途：不可用按钮
//=========================

using UnityEngine;
using System.Collections;

public class CanDisButton : MonoBehaviour {

    #region 外部控件
    /// <summary>
    /// 图标
    /// </summary>
    public UISpriteEx icon;
    /// <summary>
    /// 背景
    /// </summary>
    public UISpriteEx backgruand;
    /// <summary>
    /// 未开启
    /// </summary>
    public GameObject lockObj;
    /// <summary>
    /// 开启功能
    /// </summary>
    public GameObject openObj;
    #endregion
    public bool hadOpen = true;

    /// <summary>
    /// 设置开启状态
    /// </summary>
    /// <param name="_state"></param>
    public void SetOpenState(bool state)
    {
        hadOpen = state;
        if (icon != null)
        {
            if (state)
                icon.IsGray = UISpriteEx.ColorGray.normal;
            else
                icon.IsGray = UISpriteEx.ColorGray.Gray;
        }
        if (backgruand != null)
        {
            if (state)
                backgruand.IsGray = UISpriteEx.ColorGray.normal;
            else
                backgruand.IsGray = UISpriteEx.ColorGray.Gray;
        }
        if (lockObj != null)
        {
            lockObj.SetActive(!state);
        }
        if (openObj != null)
        {
            openObj.SetActive(state);
        }
        BoxCollider box = GetComponent<BoxCollider>();
        if (box!=null)
        {
            box.enabled = state;
        }
    }
}
