//======================================================
//作者:朱素云
//日期:2016/4/10
//用途:修行飞升数据类
//======================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PracticeData : ActorData
{
    /// <summary>
    /// 铜币运功次数
    /// </summary>
    public uint coinTime = 0;
    /// <summary>
    /// 元宝运动次数
    /// </summary>
    public uint diamondTime = 0;
    /// <summary>
    /// 当前星星
    /// </summary>
    public int stratNum;
    public System.Action OnPracticeInfoUpdata;
    /// <summary>
    /// 生命、防御、暴击、韧性、命中、闪避、攻击
    /// </summary> 
    public List<int> propertyVal = new List<int>();
    protected MainPlayerData data = null;
    /// <summary>
    /// 玩家数据
    /// </summary>
    public MainPlayerInfo MainData
    {
        get
        {
            return GameCenter.mainPlayerMng.MainPlayerInfo;
        }
    }
    /// <summary>
    /// 飞升，参数等级
    /// </summary> 
    public PracticeData(int _start)
    {
        stratNum = _start;
        data = MainData.GetServerData();
        propertyVal.Clear();
        if (data.propertyValueDic.ContainsKey(ActorPropertyTag.HPLIMIT)) propertyVal.Add(data.propertyValueDic[ActorPropertyTag.HPLIMIT]);
        if (data.propertyValueDic.ContainsKey(ActorPropertyTag.DEFDOWN)) propertyVal.Add(data.propertyValueDic[ActorPropertyTag.DEFDOWN]);
        if (data.propertyValueDic.ContainsKey(ActorPropertyTag.CRI)) propertyVal.Add(data.propertyValueDic[ActorPropertyTag.CRI]);
        if (data.propertyValueDic.ContainsKey(ActorPropertyTag.TOUGH)) propertyVal.Add(data.propertyValueDic[ActorPropertyTag.TOUGH]);
        if (data.propertyValueDic.ContainsKey(ActorPropertyTag.HIT)) propertyVal.Add(data.propertyValueDic[ActorPropertyTag.HIT]);
        if (data.propertyValueDic.ContainsKey(ActorPropertyTag.DOD)) propertyVal.Add(data.propertyValueDic[ActorPropertyTag.DOD]);
        if (data.propertyValueDic.ContainsKey(ActorPropertyTag.ATKDOWN)) propertyVal.Add(data.propertyValueDic[ActorPropertyTag.ATKDOWN]);
    }
    /// <summary>
    /// 修行
    /// </summary>
    /// <param name="_coinTime">元宝运功次数</param>
    /// <param name="_diamondTime">铜钱运功次数</param>
    public PracticeData(uint _coinTime, uint _diamondTime)
    {
        coinTime = _coinTime;
        diamondTime = _diamondTime;
    }
    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="_coinTime">元宝运功次数</param>
    /// <param name="_diamondTime">铜钱运功次数</param>
    public void UpdatePracticeData(uint _coinTime, uint _diamondTime)
    {
        coinTime = _coinTime;
        diamondTime = _diamondTime;
        if (OnPracticeInfoUpdata != null) OnPracticeInfoUpdata();
    }
    /// <summary>
    /// 更新，参数飞升等级
    /// </summary> 
    public void Updata(int _start)
    {
        stratNum = _start;
        data = MainData.GetServerData();
        propertyVal.Clear();
        if (data.propertyValueDic.ContainsKey(ActorPropertyTag.HPLIMIT)) propertyVal.Add(data.propertyValueDic[ActorPropertyTag.HPLIMIT]);
        if (data.propertyValueDic.ContainsKey(ActorPropertyTag.DEFDOWN)) propertyVal.Add(data.propertyValueDic[ActorPropertyTag.DEFDOWN]);
        if (data.propertyValueDic.ContainsKey(ActorPropertyTag.CRI)) propertyVal.Add(data.propertyValueDic[ActorPropertyTag.CRI]);
        if (data.propertyValueDic.ContainsKey(ActorPropertyTag.TOUGH)) propertyVal.Add(data.propertyValueDic[ActorPropertyTag.TOUGH]);
        if (data.propertyValueDic.ContainsKey(ActorPropertyTag.HIT)) propertyVal.Add(data.propertyValueDic[ActorPropertyTag.HIT]);
        if (data.propertyValueDic.ContainsKey(ActorPropertyTag.DOD)) propertyVal.Add(data.propertyValueDic[ActorPropertyTag.DOD]);
        if (data.propertyValueDic.ContainsKey(ActorPropertyTag.ATKDOWN)) propertyVal.Add(data.propertyValueDic[ActorPropertyTag.ATKDOWN]);
        if (OnPracticeInfoUpdata != null) OnPracticeInfoUpdata();
    }
    /// <summary>
    /// 铜币运功需要的铜币值
    /// </summary>
	public ulong CoinYG
    {
        get
        {
            int len = ConfigMng.Instance.GetStyliteMoneyRefTable().Count - 1;
            if (coinTime + 1 > len)
                return ConfigMng.Instance.GetStyliteMoneyRef(len) != null ?
					(ulong)ConfigMng.Instance.GetStyliteMoneyRef(len).money : 0;
            else
                return ConfigMng.Instance.GetStyliteMoneyRef((int)coinTime + 1) != null ?
					(ulong)ConfigMng.Instance.GetStyliteMoneyRef((int)coinTime + 1).money : 0;
        }
    }
    /// <summary>
    /// 元宝运功需要的元宝值
    /// </summary>
    public int DiamondYG
    {
        get
        {
            int len = ConfigMng.Instance.GetStyliteMoneyRefTable().Count - 1;
            if (diamondTime + 1 > len)
                return ConfigMng.Instance.GetStyliteMoneyRef(len) != null ?
                     ConfigMng.Instance.GetStyliteMoneyRef(len).specialMoney : 0;
            else
                return ConfigMng.Instance.GetStyliteMoneyRef((int)diamondTime + 1) != null ?
                   ConfigMng.Instance.GetStyliteMoneyRef((int)diamondTime + 1).specialMoney : 0;
        }
    }
    /// <summary>
    /// 获取十倍运功需要的元宝
    /// </summary> 
    public int GetTenTimesYgDiamond()
    {
        int len = ConfigMng.Instance.GetStyliteMoneyRefTable().Count - 1;
        int num = 0;

        if (diamondTime + 10 > len) num = 0;
        else
        {
            for (int i = 0; i < 10; i++)
            {
                StyliteMoneyRef styliteMoneyRef = ConfigMng.Instance.GetStyliteMoneyRef((int)diamondTime + i + 1);
                if (null == styliteMoneyRef)
                {
                    num = 0;
                    break;
                }
                else
                {
                    num += styliteMoneyRef.specialMoney;
                }
            }
        }
        return num;
    }
}