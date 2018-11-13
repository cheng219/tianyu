//==============================================
//作者：唐源
//日期：2017/4/14
//用途：七日挑战列表单个UI
//==============================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SevenChallengeUI : MonoBehaviour {
    #region UI控件
    public UIButton BtnOpenSingle;
    public ItemUI reward;
    public UILabel count;
    public GameObject redPoint;
    public GameObject openShow;
    public GameObject notYetOpen;
    public UISpriteEx image;
    public UILabel title;
    //public UISpriteEx image2;
    //public ItemUI reward2;
    #endregion
    #region unity函数
    void Awake()
    {

    }
    #endregion
    #region 界面的显示与刷新
    public void UpdateUI(int _day,int _count,int _state)
    {
        if (openShow != null && notYetOpen != null)
        {
            openShow.SetActive(true);
            notYetOpen.SetActive(false);
            if (image != null)
            {
                image.IsGray = UISpriteEx.ColorGray.normal;
                //Debug.Log("image.IsGray:"+ image.IsGray);
            }
        }
        SevenDaysTaskRewardRef data = ConfigMng.Instance.GetSevenChallengeRewardRef(_day);
        if(count!=null)
        {
            if (_count < 7)
                count.text = "[ff0000]" + _count+"/"+ 7;
            else if(_count==7)
            {
                //Debug.Log("_count:" + _count);
                count.text = "[FFFF00]" + _count + "/" + 7;
            }
                
        }
        if(redPoint!=null)
        {
            if(_state==1)
            redPoint.SetActive(false);
            else
            {
                if(_count==7)
                {
                    redPoint.SetActive(true);
                }
                else
                {
                    redPoint.SetActive(false);
                }
            }
        }
        if(data!= null)
        {
            if (title != null)
            {
                title.text = data.des1;
            }
            if(reward != null)
            {
                EquipmentInfo info = new EquipmentInfo(data.showreward, EquipmentBelongTo.PREVIEW);
                if (info != null)
                    reward.FillInfo(info);
                else
                    Debug.LogError("七日挑战奖励数据配置有错找尹明");
            }
            if(image!=null)
            {
                image.spriteName = data.Pic;
                //Debug.Log("图片名字:" + data.Pic+","+ image.spriteName);
            }

        }
        else
        {
            Debug.LogError("读取不到当前的七天奖励数据为空");
        }
    }
    //填充静态的预制UI
   public void FillUI(int _day, int _count, int _state)
   {
        SevenDaysTaskRewardRef data = ConfigMng.Instance.GetSevenChallengeRewardRef(_day);
        if (count != null)
        {
            if (_count < 7)
                count.text = "[ff0000]" + _count + "/" + 7;
            else
                count.text = "[14E615FF]" + _count + "/" + 7;
        }
        if (redPoint != null)
        {
            if (_state == 1)
                redPoint.SetActive(false);
            else
            {
                if (_count == 7)
                {
                    redPoint.SetActive(true);
                }
                else
                {
                    redPoint.SetActive(false);
                }
            }
        }
        if (data != null)
        {
            if (title != null)
            {
                title.text = data.des1;
            }
            if (reward != null)
            {
                EquipmentInfo info = new EquipmentInfo(data.showreward, EquipmentBelongTo.PREVIEW);
                if (info != null)
                    reward.FillInfo(info);
                else
                    Debug.LogError("七日挑战奖励数据配置有错找尹明");
            }
            if (image != null)
            {
                image.spriteName = data.Pic;
                //Debug.Log("图片名字:" + data.Pic+","+ image.spriteName);
            }

        }
        else
        {
            Debug.LogError("读取不到当前的七天奖励数据为空");
        }
    }
    #endregion
}
