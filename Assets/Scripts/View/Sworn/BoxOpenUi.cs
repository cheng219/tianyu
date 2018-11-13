//======================================================
//作者:朱素云
//日期:2016/7/15
//用途:宝箱打开和关闭
//======================================================
using UnityEngine;
using System.Collections;

public class BoxOpenUi : MonoBehaviour 
{
    public UISprite openBox;
    public UISprite closeBox;
    /// <summary>
    /// true打开宝箱，false未开
    /// </summary> 
    public void show(bool _isopen)
    {
        if (openBox != null) openBox.gameObject.SetActive(false);
        if (closeBox != null) closeBox.gameObject.SetActive(false);

        if (_isopen)
        {
            if (openBox != null) openBox.gameObject.SetActive(true);
        }
        else
        {
            if (closeBox != null) closeBox.gameObject.SetActive(true); 
        } 
    }
}
