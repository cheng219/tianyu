//======================================
//作者:吴江
//日期:2015/10/8
//用途:UI的二级窗口基类
//======================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// UI的二级窗口基类
/// </summary>
public class SubWnd : MonoBehaviour {

    public SubGUIType type;

    /// <summary>
    /// 是否为第一次打开 by吴江
    /// </summary>
    private bool isFirstTime = true;	


    /// <summary>
    /// 供外部调用的关闭接口  by吴江
    /// </summary>
    /// <returns></returns>
    public virtual SubWnd CloseUI()
    {
        if (this.gameObject.activeSelf)
        {
            this.gameObject.SetActive(false);
            OnClose();
        }
        return this;
    }


    /// <summary>
    /// 供外部调用的打开接口  by吴江
    /// </summary>
    /// <returns></returns>
    public virtual SubWnd OpenUI()
    {
        if (!this.gameObject.activeSelf)
        {
            this.gameObject.SetActive(true);
            TweenAlphaAllChild x = this.gameObject.GetComponent<TweenAlphaAllChild>();
            if (x != null)
            {
                x.ResetToBeginning();
                x.enabled = true;
            }
            OnOpen();
        }
        return this;
    }



    /// <summary>
    /// 实际从打开变成关闭时被调用  by吴江
    /// </summary>
    protected virtual void OnClose()
    {
        // ToolTipMng.CloseAllTooltip();
        HandEvent(false);
      
    }


    /// <summary>
    /// 实际从关闭变成打开时被调用  by吴江
    /// </summary>
    protected virtual void OnOpen()
    {
        if (isFirstTime)
        {
            Init();
            isFirstTime = false;
        }
        HandEvent(true);

    }


    /// <summary>
    /// 第一次开窗时调用  by吴江
    /// </summary>
    protected virtual void Init()
    {
    }

    /// <summary>
    /// 事件绑定接口。在基类的OpenUI()中被调用，参数为true，在基类的CloseUI()中被调用，参数为false.界面关闭以后也需要监听的事件不能在这个接口中注册；  by吴江
    /// </summary>
    /// <param name="_bind"></param>
    protected virtual void HandEvent(bool _bind)
    {
    }

}
