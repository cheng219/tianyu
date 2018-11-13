//======================================================
//作者:朱素云
//日期:2016/10/26
//用途:时装等级属性ui
//======================================================
using UnityEngine;
using System.Collections;

public class FashionAttrUi : MonoBehaviour 
{

    public UILabel[] attibute;

    public void SetContent(FashionLevelRef cur, FashionLevelRef next = null)
    {
        if (cur != null)
        {
            for (int i = 0, max = attibute.Length; i < max; i++)
            {
                if (attibute[i] != null)
                {
                    if (next != null)
                    {
                        SetLabelText(attibute[i], next.attr[i] - cur.attr[i]);
                    }
                    else
                    {
                        SetLabelText(attibute[i], cur.attr[i]);
                    }
                }
            }
        }
        else
        { 
            for (int i = 0, max = attibute.Length; i < max; i++)
            {
                if (next != null)
                {
                    SetLabelText(attibute[i], next.attr[i]);
                }
                else
                {
                    SetLabelText(attibute[i], 0);
                }
            }
        }
    }

    void SetLabelText(UILabel label, int val)
    {
        label.text = val.ToString();
    }
}
