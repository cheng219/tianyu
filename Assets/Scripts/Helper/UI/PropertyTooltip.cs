//=======================================
//作者:吴江
//日期:2015/10/09
//用途:战斗属性信息展示界面
//======================================


using UnityEngine;
using System.Collections;

/// <summary>
/// 战斗属性信息展示界面
/// </summary>
public class PropertyTooltip : GUIBase
{
    public GameObject arrowTopRight;
    public GameObject arrowTopLeft;
    public GameObject arrowButtonRight;
    public GameObject arrowButtonLeft;
    public UILabel propertyName;
    public UILabel properyText;
    public UISprite backGround;

    protected ActorPropertyTag propertyTag;
    protected int propertyValue;
    protected GameObject triggerObj;

    /// <summary>
    /// 设置展示信息 by吴江
    /// </summary>
    /// <param name="_tag"></param>
    /// <param name="_value"></param>
    public void SetData(ActorPropertyTag _tag, int _value,GameObject _triggerObj)
    {
        propertyTag = _tag;
        propertyValue = _value;
        triggerObj = _triggerObj;
        //refData = ConfigMng.Instance.GetAttributeTypeRef(propertyTag);
    }


    protected override void OnOpen()
    {
        base.OnOpen();
    //    RefreshArrow();
        RefreshContent();
    }



    /// <summary>
    /// 刷新四个角箭头的显示和隐藏 by吴江
    /// </summary>
    protected void RefreshArrow()
    {
//        Vector3 pos = Vector3.zero;
//        if (triggerObj == null)
//        {
//            pos = GameCenter.cameraMng.uiCamera.WorldToScreenPoint(transform.position);
//        }
//        else
//        {
//            pos = GameCenter.cameraMng.uiCamera.WorldToScreenPoint(triggerObj.transform.position);
//        }
        //float halfScreenWid = Screen.width / 2.0f;
        //float halfScreenHigh = Screen.height / 2.0f;
//        arrowTopRight.SetActive(pos.x > halfScreenWid && pos.y > halfScreenHigh);
//        arrowTopLeft.SetActive(pos.x < halfScreenWid && pos.y > halfScreenHigh);
//        arrowButtonRight.SetActive(pos.x > halfScreenWid && pos.y < halfScreenHigh);
//        arrowButtonLeft.SetActive(pos.x < halfScreenWid && pos.y < halfScreenHigh);
    }
    /// <summary>
    /// 刷新显示内容 by吴江
    /// </summary>
    protected void RefreshContent()
    {
        //if (refData != null)
        //{
        //    propertyName.text = refData.stats;
        //    properyText.text = refData.explain;
        //}
    }


}
