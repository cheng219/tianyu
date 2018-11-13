//===============================
//日期：2016/4/29
//作者：鲁家旗
//用途描述:成就界面类
//===============================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AchievementItemUI : MonoBehaviour
{
    #region 控件数据
    public UILabel nameTitle;
    public UILabel achievementLabel;
    public UILabel rewardLabel;
    public UILabel chenHao;
    public UILabel chenHaoName;
    public UIButton getBtn;
    public Transform notComplete;
    public Transform complete;
    public UISprite completeSp;
    //是否满足条件
    protected bool isReach = false;
    //是否领取奖励
    protected bool isGetReward = false;
    protected int achievementNum = 0;
    protected FDictionary dic
    {
        get
        {
            return GameCenter.achievementMng.curhaveAchieve;
        }
    }
    #endregion

    
    #region 刷新
    public void SetAchievementItemData(AchievementRef _data,int _index)
    {
        if(getBtn != null)
            UIEventListener.Get(getBtn.gameObject).onClick = delegate { GameCenter.achievementMng.C2S_ReqGetReward(_data.id); };
        if(nameTitle != null) nameTitle.text = _data.levelName;
        if (chenHaoName != null && chenHao != null)
        {
            if (_data.titleName != "0")
            {
                chenHao.gameObject.SetActive(true);
                chenHaoName.gameObject.SetActive(true);
                chenHaoName.text = _data.titleName;
            }
            else
            {
                chenHao.gameObject.SetActive(false);
                chenHaoName.gameObject.SetActive(false);
            }
        }
        string[] str = null;
        //已拥有的成就数量
        achievementNum = 0;
        switch (_index)
        { 
            case (int)AchievementType.COIN://铜钱
                if (GameCenter.achievementMng.currentAchievementNum >= _data.judgeNum1)
                {
                    isReach = true;
                    str = new string[1] { _data.judgeNum1.ToString() };
                }
                else
                {
                    isReach = false;
                    str = new string[1] { "[ff0000]" + GameCenter.achievementMng.currentAchievementNum + "[-]" };
                }
                break;
            case (int)AchievementType.LEV://道行
                if(_data.judgeNum2 == 0)
                {
                    if (GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel >= _data.judgeNum1)
                    {
                        isReach = true;
                        str = new string[1] { ConfigMng.Instance.GetLevelDes(_data.judgeNum1)};
                    }
                    else
                    {
                        isReach = false;
                        str = new string[1] { "[ff0000]" + ConfigMng.Instance.GetLevelDes(GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel) + "[-]" };
                    }
                }
                break;
            case (int)AchievementType.FIGHT://修为
                if (_data.judgeNum2 == 0)
                {
                    if (GameCenter.mainPlayerMng.MainPlayerInfo.FightValue >= _data.judgeNum1)
                    {
                        isReach = true;
                        str = new string[1] { _data.judgeNum1.ToString() };
                    }
                    else
                    {
                        isReach = false;
                        str = new string[1] { "[ff0000]" + GameCenter.mainPlayerMng.MainPlayerInfo.FightValue + "[-]" };
                    }
                    if (dic.ContainsKey(_data.id) || GameCenter.achievementMng.achievementTogRed.ContainsKey(_data.id))
                    {
                        isReach = true;
                        str = new string[1] { _data.judgeNum1.ToString() };
                    }
                }
                break;
            case (int)AchievementType.SKILLLEV://术法
                List<SkillInfo> list = GameCenter.skillMng.GetOwnSkill();
                for (int i = 0; i < list.Count; i++)
                {
                    SkillInfo info = list[i];
                    if (info.SkillLv >= _data.judgeNum1)
                    {
                        achievementNum++;
                    }
                }
                if (achievementNum >= _data.judgeNum2)
                {
                    isReach = true;
                    str = new string[1] { _data.judgeNum2.ToString()};
                }
                else
                {
                    isReach = false;
                    str = new string[1] { "[ff0000]" + achievementNum + "[-]" };
                }
                break;
            case (int)AchievementType.STRENTHEQULEV://强化
                foreach (EquipmentInfo info in GameCenter.inventoryMng.GetPlayerEquipList().Values)//穿在身上的装备
                {
                    if (info.UpgradeLv >= _data.judgeNum1)
                    {
                        achievementNum++;
                    }
                }
                if (achievementNum >= _data.judgeNum2)
                {
                    isReach = true;
                    str = new string[1] { _data.judgeNum2.ToString() };
                }
                else
                {
                    isReach = false;
                    str = new string[1] { "[ff0000]" + achievementNum + "[-]" };
                }
                if (dic.ContainsKey(_data.id) || GameCenter.achievementMng.achievementTogRed.ContainsKey(_data.id))
                {
                    isReach = true;
                    str = new string[1] { _data.judgeNum2.ToString() };
                }
                break;
            case (int)AchievementType.EQULEV://淬炼
                foreach (EquipmentInfo info in GameCenter.inventoryMng.GetPlayerEquipList().Values)
                {
                    if (info.LV >= _data.judgeNum1)
                    {
                        achievementNum++;
                    }
                }
                if (achievementNum >= _data.judgeNum2)
                {
                    isReach = true;
                    str = new string[1] { _data.judgeNum2.ToString() };
                }
                else
                {
                    isReach = false;
                    str = new string[1] { "[ff0000]" + achievementNum + "[-]" };
                }
                if (dic.ContainsKey(_data.id) || GameCenter.achievementMng.achievementTogRed.ContainsKey(_data.id))
                {
                    isReach = true;
                    str = new string[1] { _data.judgeNum2.ToString() };
                }
                break;
            case (int)AchievementType.GEM://宝石
                achievementNum = GameCenter.inventoryMng.GetPlayerInlayGemNumByLv(_data.judgeNum1);
                if (achievementNum >= _data.judgeNum2)
                {
                    isReach = true;
                    str = new string[1] { achievementNum.ToString() };
                }
                else
                {
                    isReach = false;
                    str = new string[1] { "[ff0000]" + achievementNum + "[-]" };
                }
                if (dic.ContainsKey(_data.id) || GameCenter.achievementMng.achievementTogRed.ContainsKey(_data.id))
                {
                    isReach = true;
                    str = new string[1] { achievementNum.ToString() };
                }
                break;
            case (int)AchievementType.SOPHISTICATION://洗练
                achievementNum = GameCenter.inventoryMng.GetWashAttrNumByQuality();
                if (achievementNum >= _data.judgeNum1)
                {
                    isReach = true;
                    str = new string[1] { _data.judgeNum1.ToString() };
                }
                else
                {
                    isReach = false;
                    str = new string[1] { "[ff0000]" + achievementNum + "[-]" };
                }
                if (dic.ContainsKey(_data.id) || GameCenter.achievementMng.achievementTogRed.ContainsKey(_data.id))
                {
                    isReach = true;
                    str = new string[1] { _data.judgeNum1.ToString() };
                }
                break;
            case (int)AchievementType.PETGRROW://宠物成长
                achievementNum = GameCenter.mercenaryMng.GetPetGrowUp(PetPropertyType.GROWUP, _data.judgeNum1);
                if (achievementNum >= 1)
                {
                    isReach = true;
                    str = new string[1] { "1" };
                }
                else
                {
                    isReach = false;
                    str = new string[1] { "[ff0000]" + achievementNum + "[-]" };
                }
                if (dic.ContainsKey(_data.id) || GameCenter.achievementMng.achievementTogRed.ContainsKey(_data.id))
                {
                    isReach = true;
                    str = new string[1] { "1" };
                }
                break;
            case (int)AchievementType.PETSPIRITUAL://宠物灵修
                if(dic.ContainsKey(_data.id))
                {
                    isReach = true;
                    str = new string[1] { "1" };
                }
                else
                {
                    isReach = false;
                    str = new string[1] { "[ff0000]0[-]"};
                }
                //if (dic.ContainsKey(_data.id) || GameCenter.achievementMng.achievementTogRed.ContainsKey(_data.id))
                //{
                //    isReach = true;
                //    str = new string[1] { "1" };
                //}
                break;
            case (int)AchievementType.APTITUDE://宠物资质
                achievementNum = GameCenter.mercenaryMng.GetPetGrowUp(PetPropertyType.APTITUDE, _data.judgeNum1);
                if (achievementNum >= 1)
                {
                    isReach = true;
                    str = new string[1] { "1" };
                }
                else
                {
                    isReach = false;
                    str = new string[1] { "[ff0000]" + achievementNum + "[-]" };
                }
                if (dic.ContainsKey(_data.id) || GameCenter.achievementMng.achievementTogRed.ContainsKey(_data.id))
                {
                    isReach = true;
                    str = new string[1] { "1" };
                }
                break;
            case (int)AchievementType.MOUNT://坐骑
                achievementNum = GameCenter.newMountMng.CurLev > 0 ? GameCenter.newMountMng.CurLev : 0;
                RidePropertyRef mountRef = ConfigMng.Instance.GetMountPropertyRef(achievementNum);
                if (achievementNum >= _data.judgeNum1)
                {
                    isReach = true;
                    RidePropertyRef mountAcRef = ConfigMng.Instance.GetMountPropertyRef(_data.judgeNum1);
                    if (mountAcRef != null)
                        str = new string[1] { mountAcRef.name };
                }
                else
                {
                    isReach = false;
                    if (mountRef != null)
                        str = new string[1] { "[ff0000]" + mountRef.name + "[-]" };
                    else
                        str = new string[1] { "[ff0000]" +achievementNum +"[-]"+"阶"+ "[ff0000]" + achievementNum +"[-]" +"星"};
                }
                break;
            case (int)AchievementType.COPY://副本
                if (GameCenter.achievementMng.currentAchievementNum >= _data.judgeNum1)
                {
                    isReach = true;
                    str = new string[1] { _data.judgeNum1.ToString() };
                }
                else
                {
                    isReach = false;
                    str = new string[1] { "[ff0000]" + GameCenter.achievementMng.currentAchievementNum + "[-]" };
                }
                break;
            case (int)AchievementType.LABOUR://仙盟
                if (GameCenter.achievementMng.currentAchievementNum >= _data.judgeNum1)
                {
                    isReach = true;
                    str = new string[1] { _data.judgeNum1.ToString() };
                }
                else
                {
                    isReach = false;
                    str = new string[1] { "[ff0000]" + GameCenter.achievementMng.currentAchievementNum + "[-]" };
                }
                break;
            case (int)AchievementType.FASHION://时装
                if (GameCenter.achievementMng.currentAchievementNum >= _data.judgeNum1)
                {
                    isReach = true;
                    str = new string[1] { _data.judgeNum1.ToString() };
                }
                else
                {
                    isReach = false;
                    str = new string[1] { "[ff0000]" + GameCenter.achievementMng.currentAchievementNum + "[-]" };
                }
                break;
            default:
                isReach = false;
                break;
        }
        if (dic.ContainsKey(_data.id))
        {
            if ((dic[_data.id] as AchievementData).RewardState)
            {
                isGetReward = true;
            }
            else
            {
                isGetReward = false;
            }
        }
        else
        {
            isGetReward = false;
        }

        if (achievementLabel != null)
        {
            achievementLabel.text = UIUtil.Str2Str(_data.des, str);
        }
        if (isReach && !isGetReward)//达到领取状态
        {
            if (notComplete != null) notComplete.gameObject.SetActive(false);
            if (getBtn != null) getBtn.gameObject.SetActive(true);
            if (complete != null) complete.gameObject.SetActive(false);
            if (completeSp != null) completeSp.gameObject.SetActive(true);
        }
        else if (!isReach && !isGetReward)
        {
            if (notComplete != null) notComplete.gameObject.SetActive(true);
            if (getBtn != null) getBtn.gameObject.SetActive(false);
            if (complete != null) complete.gameObject.SetActive(false);
            if (completeSp != null) completeSp.gameObject.SetActive(false);
        }
        if (isGetReward)//已领奖
        {
            if (notComplete != null) notComplete.gameObject.SetActive(false);
            if (getBtn != null) getBtn.gameObject.SetActive(false);
            if (complete != null) complete.gameObject.SetActive(true);
            if (completeSp != null) completeSp.gameObject.SetActive(true);
        }
    }
    #endregion
}
public enum AchievementType
{ 
    /// <summary>
    /// 铜钱
    /// </summary>
    COIN = 0,
    /// <summary>
    /// 道行(等级)
    /// </summary>
    LEV = 1,
    /// <summary>
    /// 修为(战力)
    /// </summary>
    FIGHT = 2,
    /// <summary>
    /// 术法(技能等级)
    /// </summary>
    SKILLLEV = 3,
    /// <summary>
    /// 强化(装备强化等级)
    /// </summary>
    STRENTHEQULEV = 4,
    /// <summary>
    /// 淬炼(装备等级)
    /// </summary>
    EQULEV = 5,
    /// <summary>
    /// 宝石(镶嵌几个达到几级的宝石)
    /// </summary>
    GEM = 6,
    /// <summary>
    /// 洗练(身上有几条橙色的属性)
    /// </summary>
    SOPHISTICATION = 7,
    /// <summary>
    /// 宠物成长(有几个宠物达到多少成长)
    /// </summary>
    PETGRROW = 8,
    /// <summary>
    /// 宠物灵修
    /// </summary>
    PETSPIRITUAL = 9,
    /// <summary>
    /// 宠物资质
    /// </summary>
    APTITUDE = 10,
    /// <summary>
    /// 坐骑
    /// </summary>
    MOUNT = 11,
    /// <summary>
    /// 副本
    /// </summary>
    COPY = 12,
    /// <summary>
    /// 仙盟
    /// </summary>
    LABOUR = 13,
    /// <summary>
    /// 时装
    /// </summary>
    FASHION = 14,
}