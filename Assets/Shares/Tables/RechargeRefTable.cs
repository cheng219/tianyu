//=========================
//作者：黄洪兴
//日期：2016/06/18
//用途：充值静态配置
//=========================

using UnityEngine;
using System.Collections;
using System.Collections.Generic; 

public class RechargeRefTable : AssetTable
{

    public List<RechargeRef> infoList = new List<RechargeRef>();
}

[System.Serializable]
public class RechargeRef
{
    public int id;
    public string chargeName;
    public int chargeAmount;
    public int chargeDiamond;
    public int ios;
    public int viplevel;
    public bool isIOS;
    /// <summary>
    /// 是否有热销标志
    /// </summary>
    public bool hotflag;
    /// <summary>
    /// 标志
    /// </summary>
    public List<RechargeFlage> remindDes = new List<RechargeFlage>();
}
[System.Serializable]
public class RechargeFlage
{
    public int type;
    public string des;
    public RechargeFlage(int _type, string _des)
    {
        this.type = _type;
        this.des = _des;
    }
}
