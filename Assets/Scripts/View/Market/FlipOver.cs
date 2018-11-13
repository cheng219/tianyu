//=========================================
//作者：黄洪兴
//日期：2016/04/12
//用途：市场翻页
//=========================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 市场翻页
/// </summary>
public class FlipOver : MonoBehaviour
{
    public Vector3 Point;
    public GameObject Obj;
    public float PointY;
    public bool canFlip;
    public MarketItemContainer marketItemContainer;


    void Awake()
    {
        Point = this.transform.localPosition;
        PointY = Point.y;
        canFlip = true;
       // Debug.Log("开始的位置为" + Point.y);

    }

    void Update()
    {
        if (canFlip && marketItemContainer != null)
        {
            if (marketItemContainer.MarketItemContainers.Count > 2)
            {
                PointY = Point.y + (marketItemContainer.MarketItemContainers.Count - 2) * 125;
            }
            else
            {
                PointY = this.transform.localPosition.y;
            }
            if ((this.transform.localPosition.y - PointY) > 30)
            {

                //Debug.Log("触发下一页" + PointY);
                GameCenter.marketMng.C2S_AskMarketItem(GameCenter.marketMng.marketPage + 1);
                canFlip = false;

            }
        }

    }

   
}
