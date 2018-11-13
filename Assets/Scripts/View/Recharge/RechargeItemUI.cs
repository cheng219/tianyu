//=====================================
//作者:黄洪兴
//日期:2016/6/18
//用途:充值组件
//========================================


using UnityEngine;
using System.Collections;
/// <summary>
/// 充值组件
/// </summary>
public class RechargeItemUI : MonoBehaviour
{
    #region 控件数据
    public UILabel needMoney;
    public GameObject GoChargeObj;
    public UILabel price;
   // public UILabel diamo;
    public GameObject hot;
    public UILabel remind;




    #endregion
    #region 数据
    /// <summary>
    /// 当前填充的数据
    /// </summary>
    public RechargeRef curInfo;

    #endregion
    // Use this for initialization
    void Start()
    {
        if (GoChargeObj != null)
            UIEventListener.Get(GoChargeObj).onClick = Recharge;

    }


    void Recharge(GameObject _go)
    {
		GameCenter.rechargeMng.C2S_RequestRecharge(curInfo);
    }



    /// <summary>
    /// 填充数据
    /// </summary>
    /// <param name="_info"></param>
    public void FillInfo(RechargeRef _info)
    {
        if (_info == null)
        {
            curInfo = null;
            return;
        }
        else
        {
            curInfo = _info;
        }
        Refresh();
    }

    /// <summary>
    /// 刷新表现
    /// </summary>
    public void Refresh()
    {
       
        if (price != null)
        {
            //string[] word = { curInfo.chargeAmount.ToString(), curInfo.chargeDiamond.ToString() };
            //string s = ConfigMng.Instance.GetUItext(87, word);
            price.text = curInfo.chargeDiamond.ToString();
        }
        if (needMoney != null) needMoney.text = "￥ " + curInfo.chargeAmount;
        if (hot != null) hot.SetActive(curInfo.hotflag);
        if (remind != null)
        {
            remind.transform.parent.gameObject.SetActive(false);
        }
        for (int i = 0, max = curInfo.remindDes.Count; i < max; i++)
        {
            if (curInfo.remindDes[i].type == 0)//永久显示
            {
                if (!string.IsNullOrEmpty(curInfo.remindDes[i].des))
                {
                    if (remind != null)
                    { 
                        remind.text = curInfo.remindDes[i].des;
                        remind.transform.parent.gameObject.SetActive(true);
                    }
                }
                else
                {
                    if (remind != null)
                    {
                        remind.transform.parent.gameObject.SetActive(false);
                    }
                }
            }
            else
            { 
                if (GameCenter.vipMng.rechargeFlagDic.ContainsKey(curInfo.remindDes[i].type) && 
                    GameCenter.vipMng.rechargeFlagDic[curInfo.remindDes[i].type])
                {
                    if (remind != null)
                    { 
                        remind.text = curInfo.remindDes[i].des;
                        remind.transform.parent.gameObject.SetActive(true);
                        break;
                    }
                }
                else
                {
                    if (remind != null)
                    {
                        remind.transform.parent.gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}
