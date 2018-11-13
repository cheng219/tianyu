//===============================
//日期：2016/4/29
//作者：鲁家旗
//用途描述:成就界面页签类
//===============================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AchievementTogUI : MonoBehaviour  {
    public UILabel togName;
    public UILabel progress;
    public UISprite[] progressSp;
    public UISprite redPoint;
    protected FDictionary dic
    {
        get
        {
            return GameCenter.achievementMng.achieveNumDic;
        }
    }
    protected FDictionary redPointDic
    {
        get
        {
            return GameCenter.achievementMng.curhaveAchieve;
        }
    }
    protected FDictionary togRedDic
    {
        get
        {
            return GameCenter.achievementMng.achievementTogRed;
        }
    }
    public void SetTogData(AchieveTypeRef _data,int _index)
    {
        if (togName != null) togName.text = _data.typeName;
        if (progress != null)
        {
            if (dic.ContainsKey(_data.type))
            {
                progress.text = dic[_data.type] + "/5";
            }
            else
                progress.text = "0/5";
        }
        for (int i = 0; i < progressSp.Length; i++)
        {
            if (dic.ContainsKey(_index + 1) && i < (int)dic[_index + 1])
            {
                progressSp[i].gameObject.SetActive(true);
            }
            else
                progressSp[i].gameObject.SetActive(false);
        }
        foreach (int type in togRedDic.Keys)
        {
            int togType = ConfigMng.Instance.GetAchieveType(type);
            if (_data.type == togType)
            {
                if (redPoint != null) redPoint.gameObject.SetActive(true);
            }
        }
        foreach (AchievementData data in redPointDic.Values)
        {
            if (ConfigMng.Instance.GetAchieveType(data.AchieveId) == _data.type)
            {
                if (data.RewardState)
                {
                    if (redPoint != null) redPoint.gameObject.SetActive(false);
                }
                else
                    if (redPoint != null) redPoint.gameObject.SetActive(true);
            }
        }
    }
}
