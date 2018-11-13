//======================================================
//作者:朱素云
//日期:2017/3/29
//用途:运营活动转盘ui
//======================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;

public class WdfRotateItemUi : MonoBehaviour {

    /// <summary>
    /// 初始速度
    /// </summary>
    protected float rotateV = 10;//初始速度 
    /// <summary>
    /// 减速度变化率
    /// </summary>
    protected float SpeedCudRate = 1f;
    //public ItemUI[] rewardItem;//奖励转盘表
    //public UILabel[] rewardRasiol;//奖池元宝百分比
    //public UISprite[] chooseBack;//选中背景
    public GameObject arrow;//箭头  
    public GameObject arrowCenter;//旋转中心
    protected bool isRotate = false;
    public UISpriteEx oneSpEx;
    public UISpriteEx TenSpEx;

    public int rotateNum = 2;

    protected int curRotate = -1;

    protected lucky_wheel_reward_info rewrdInfo = null;

    public OnlineRewardUi[] onlineRewards;

    void Update()
    {
        if (isRotate && rotateV > 0)//开始旋转
        {
            GameCenter.openServerRewardMng.isRotate = true;
            if (oneSpEx != null) oneSpEx.IsGray = UISpriteEx.ColorGray.Gray;
            if (TenSpEx != null) TenSpEx.IsGray = UISpriteEx.ColorGray.Gray;
            SpeedCudRate = 6.85f - (2.1f / 360) * (45 * (curRotate > 1 ? curRotate : curRotate - 1));

            rotateV -= SpeedCudRate * 0.01f;

            arrow.transform.RotateAround(arrowCenter.transform.position, new Vector3(0, 0, -1), rotateV);

            if (rotateV <= 0)
            {
                GameCenter.openServerRewardMng.isRotate = false;
                if (oneSpEx != null) oneSpEx.IsGray = UISpriteEx.ColorGray.normal;
                if (TenSpEx != null) TenSpEx.IsGray = UISpriteEx.ColorGray.normal;
                isRotate = false;
                GameCenter.openServerRewardMng.isRotate = false;
                rotateV = 10;
                Refresh(curRotate);
            }
        }
    }



    /// <summary>
    /// 开始抽奖
    /// </summary>
    public void BegainRotate(lucky_wheel_reward_info _rewardInfo, bool _rotateType = false)
    { 
        if (_rotateType)//选中进阶转盘
        {
            curRotate = 3;
        }
        else
        {
            int id = 0;
            for (int i = 0, max = onlineRewards.Length; i < max; i++)
            {
                if (onlineRewards[i].item != null && onlineRewards[i].item.EQInfo != null && onlineRewards[i].item.EQInfo.EID == _rewardInfo.item_type)
                { 
                    id = i + 1;
                    break;
                }
                else//抽中元宝
                {
                    if (GameCenter.openServerRewardMng.lotteryData != null && onlineRewards[i].rewardRasiol!= null &&onlineRewards[i].rasiol != 0)
                    {
                        float count = _rewardInfo.amount - onlineRewards[i].rasiol * GameCenter.openServerRewardMng.lotteryData.allRewarCount; 
                        if (Mathf.Abs(count) < 10)
                        { 
                            id = i + 1;
                            break;
                        }
                    }
                }
            } 
            curRotate = id; 
        }
        GameCenter.openServerRewardMng.IsRotateOver = false; 
        isRotate = true;
        rotateV = 10;
    }

    /// <summary>
    /// 抽到奖励刷新
    /// </summary>
    void Refresh(int _id)
    {
        for (int i = 0, max = onlineRewards.Length; i < max; i++)
        {
            if (i == _id - 1)
            {
                if (onlineRewards[i].chooseBack != null) onlineRewards[i].chooseBack.SetActive(true);
            }
            else
            {
                if (onlineRewards[i].chooseBack != null) onlineRewards[i].chooseBack.SetActive(false);
            }
        }
        GameCenter.openServerRewardMng.IsRotateOver = true;
    }

    /// <summary>
    /// 重置数据
    /// </summary>
    public void ResetData()
    {
        rotateV = 10;
        isRotate = false; 
        arrow.transform.localPosition = new Vector3(3.1f, 76f, 0);
        arrow.transform.localRotation = new Quaternion(0, 0, 0, 0);
        for (int i = 0, max = onlineRewards.Length; i < max; i++)
        {
            if (onlineRewards[i].chooseBack != null) onlineRewards[i].chooseBack.SetActive(false);
        }
       
    }

    /// <summary>
    /// key = 1 普通转盘 key = 2 进阶转盘
    /// </summary>
    /// <param name="_key"></param>
    public void SetReward(List<lucky_wheel_reward_info> _rewards , bool _isadvance = false)
    { 
        for (int i = 0, max = onlineRewards.Length; i < max; i++)
        {
            if (_isadvance)
            {
                if (_rewards.Count > i)
                {  
                    if (_rewards[i].item_type == 1 || _rewards[i].item_type == 0)
                    {
                        onlineRewards[i].rasiol = (float)_rewards[i].amount / 10000.0f;
                        if (onlineRewards[i].item != null) onlineRewards[i].item.gameObject.SetActive(false);
                        if (onlineRewards[i].diamondSprite != null) onlineRewards[i].diamondSprite.gameObject.SetActive(true);
                        if (onlineRewards[i].rewardRasiol != null)
                        { 
                            onlineRewards[i].rewardRasiol.gameObject.SetActive(true);
                            onlineRewards[i].rewardRasiol.text = ((float)_rewards[i].amount / 10000.0f * 100) + " % "; 
                        }
                    }
                    else
                    {
                        onlineRewards[i].CreatItem();
                        if (onlineRewards[i].item != null)
                        {
                            onlineRewards[i].rasiol = 0;
                            onlineRewards[i].item.gameObject.SetActive(true);
                            onlineRewards[i].item.FillInfo(new EquipmentInfo((int)_rewards[i].item_type, (int)_rewards[i].amount, EquipmentBelongTo.PREVIEW));
                        }
                        if (onlineRewards[i].rewardRasiol != null) onlineRewards[i].rewardRasiol.gameObject.SetActive(false);  
                    }
                }
            }
            else
            {
                int id = i;
                if (id >= 2) id = i - 1;
                if (_rewards.Count > id)
                {
                    onlineRewards[i].CreatItem();
                    if (onlineRewards[i].item != null) onlineRewards[i].item.FillInfo(new EquipmentInfo((int)_rewards[id].item_type, (int)_rewards[id].amount, EquipmentBelongTo.PREVIEW));
                }
            }
        } 
    }
}
