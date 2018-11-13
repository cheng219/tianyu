//==================================
//作者：朱素云
//日期：2016/4/12
//用途：飞升属性ui
//=================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class SoaringAttibuteUi : MonoBehaviour 
{
    public UISprite[] arrawsp;//提升箭头

    public UILabel levName; 

    public UILabel[] attibute;

    public bool isUpData = false;

    public void SetLeft(int _start, bool _isBreak)
    {
        StyliteRef _styliteRef; 
           _styliteRef = ConfigMng.Instance.GetStyliteRefByStart(_start + 1); 
        FlyUpRef flyUpRef = ConfigMng.Instance.GetFlyUpRef(_styliteRef.jingJieId);
        int id = 1;
        id= (_styliteRef.jieDuan - 1) * 7 + 71 * (_styliteRef.jingJieId - 1) > 0 ? (_styliteRef.jieDuan - 1) * 7 + 71 * (_styliteRef.jingJieId - 1) : 7; 
        if ((70 * (_start / 70) + (_start / 70 - 1)) == _start && _start > 0)
            id = _start; 
        StyliteRef stylite = ConfigMng.Instance.GetStyliteRefByStart(id);//左边属性  
        if (isUpData)
        {
            if (_start < 7)
            {
                for (int i = 0, len = attibute.Length; i < len; i++)
                {
                    if (_start % 7 > i)
                    {
                        SetLeftLabelText(attibute[i], stylite.attr[i]);
                    } 
                    else
                    {
                        SetLeftLabelText(attibute[i], 0);
                    } 
                }
                if (flyUpRef != null && levName != null)
                {
                    levName.text = ConfigMng.Instance.GetUItext(21,
                    new string[2] { flyUpRef.name, "0" });
                    levName.transform.localPosition = new Vector3(580, 174.8f, 0);
                }
                return;
            }
            for (int i = 0, len = attibute.Length; i < len; i++)
            { 
                if ((_start - (_start / 71)) % 7 > i)
                {
                    SetLeftLabelText(attibute[i], _styliteRef.attr[i]);
                } 
                else
                {
                    SetLeftLabelText(attibute[i], stylite.attr[i]);
                }
            }
            if ((70 * (_start / 70) + (_start / 70 - 1)) == _start && _start > 0)
            {
                FlyUpRef fly = ConfigMng.Instance.GetFlyUpRef(_styliteRef.jingJieId);
                if (fly != null && levName != null) levName.text = ConfigMng.Instance.GetUItext(21,
                    new string[2] { fly.name, "10" });
            }
            else
            {
                FlyUpRef fly = ConfigMng.Instance.GetFlyUpRef(_styliteRef.jingJieId);
                if (fly != null && levName != null) levName.text = ConfigMng.Instance.GetUItext(21,
                    new string[2] { fly.name, (_styliteRef.jieDuan -  1).ToString() });
            }
            if (_isBreak)
            {
                levName.transform.localPosition = new Vector3(519, 174.8f, 0);
            }
            else
            {
                levName.transform.localPosition = new Vector3(580, 174.8f, 0);
            } 
        }
        else//修改为可以加的才属性才显示
        { 
            if (_start == 0)
            {
                for (int i = 0, len = attibute.Length; i < len; i++)
                {
                    if (_start == i)
                    {
                        attibute[i].gameObject.SetActive(true);
                        SetLabelText(attibute[i], 0, _styliteRef.attr[i] - 0, i);
                    }
                    else
                    {
                        attibute[i].gameObject.SetActive(false);
                        //SetLabelText(attibute[i], _styliteRef.attr[i], 0);
                    }
                } 
            }
            else if (_start < 7)
            {
                for (int i = 0, len = attibute.Length; i < len; i++)
                {
                    if (_start % 7 > i)
                    {
                        attibute[i].gameObject.SetActive(false);
                        //SetLabelText(attibute[i], stylite.attr[i], 0);
                    }
                    else if (_start % 7 == i)
                    {
                        attibute[i].gameObject.SetActive(true);
                        SetLabelText(attibute[i], 0, stylite.attr[i] - 0, i);
                    }
                    else
                    {
                        attibute[i].gameObject.SetActive(false);
                        //SetLabelText(attibute[i], 0, 0);
                    }
                } 
            } 
            else
            { 
                for (int i = 0, len = attibute.Length; i < len; i++)
                {
                    if ((_start - (_start / 71)) % 7 > i)
                    {
                        attibute[i].gameObject.SetActive(false);
                        //SetLabelText(attibute[i], _styliteRef.attr[i], 0);
                    }
                    else if ((_start - (_start / 71)) % 7 == i)
                    {
                        attibute[i].gameObject.SetActive(true);
                        SetLabelText(attibute[i], stylite.attr[i], _styliteRef.attr[i] - stylite.attr[i], i);
                    }
                    else
                    {
                        attibute[i].gameObject.SetActive(false);
                        //SetLabelText(attibute[i], stylite.attr[i], 0);
                    }
                }
            }
            if ((70*(_start/70)+(_start/70 - 1)) == _start && _start > 0)//70/141/212/283.....突破
            { 
                for (int i = 0, len = attibute.Length; i < len; i++)
                {
                    if (flyUpRef != null)
                    {
                        attibute[i].gameObject.SetActive(true);
                        SetLabelText(attibute[i], stylite.attr[i], _styliteRef.attr[i] - stylite.attr[i], -1);
                    }
                }
            }
            if ((70 * (_start / 70) + (_start / 70 - 1)) == _start && _start > 0)
            {
                FlyUpRef fly = ConfigMng.Instance.GetFlyUpRef((_start+1)/71 + 1);
                if (fly != null && levName != null) levName.text = ConfigMng.Instance.GetUItext(21,
                     new string[2] { fly.name, "0"});
            }
            else
            {
                if (flyUpRef != null && levName != null)
                    levName.text = ConfigMng.Instance.GetUItext(21,
                        new string[2] { flyUpRef.name, _styliteRef.jieDuan.ToString() });
            }
            if (_isBreak)
            {
                levName.gameObject.SetActive(true);
            }
            else
            {
                levName.gameObject.SetActive(false);
            } 
        } 
    }

    public void SetFullLev(int _lev)
    {
        StyliteRef _styliteRef;
        _styliteRef = ConfigMng.Instance.GetStyliteRefByStart(_lev);
        FlyUpRef fly = ConfigMng.Instance.GetFlyUpRef(_styliteRef.jingJieId);
       
        if (levName != null) levName.text = ConfigMng.Instance.GetUItext(21, new string[2] { fly.name, _styliteRef.jieDuan.ToString() });
        if (isUpData)
        {
            for (int i = 0, len = attibute.Length; i < len; i++)
            { 
                SetLeftLabelText(attibute[i], _styliteRef.attr[i]);
            }
            levName.transform.localPosition = new Vector3(580, 174.8f, 0);
        }
        else
        {
            for (int i = 0, len = attibute.Length; i < len; i++)
            {
                attibute[i].gameObject.SetActive(false); 
            }
            for (int i = 0, max = arrawsp.Length; i < max; i++)
            {
                arrawsp[i].gameObject.SetActive(false);
            }
            levName.gameObject.SetActive(false);
        } 
    }

    void SetLeftLabelText(UILabel label, int val)
    {
        label.text = val.ToString();
    }

    void SetLabelText(UILabel label, int val, int add, int _i = 0)
    {
        label.text = add > 0 ? add.ToString() : string.Empty;
        if (add > 0)
        {
            if (_i >= 0)
            {
                for (int i = 0, max = arrawsp.Length; i < max; i++)
                {
                    if (i == _i)
                    {
                        arrawsp[i].gameObject.SetActive(true);
                    }
                    else
                    {
                        arrawsp[i].gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                for (int i = 0, max = arrawsp.Length; i < max; i++)
                { 
                    arrawsp[i].gameObject.SetActive(true); 
                }
            }
        }
        //else
        //{
        //    label.text = val.ToString();
        //}
    }

    
}
