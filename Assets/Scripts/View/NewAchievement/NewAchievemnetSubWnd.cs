//===============================
//日期：2016/4/29
//作者：鲁家旗
//用途描述:成就界面类
//===============================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class NewAchievemnetSubWnd : SubWnd
{
    #region 数据
    public GameObject togGo;
    public GameObject itemGo;
    public UIButton leftBtn;
    public UIButton rightBtn;
    public UIGrid itemGird;
    protected List<UIToggle> togList = new List<UIToggle>();
    protected FDictionary achieveTypeDic = null;
    protected FDictionary achievementDic = null;
    protected List<GameObject> itemList = new List<GameObject>();
    public UIScrollView itemScroll;
    public UISlider progressSlider;
    public UILabel totalLifeNum;
    public UILabel currNum;
    protected FDictionary AchieveNumDic
    {
        get
        {
            return GameCenter.achievementMng.achieveNumDic;
        }
    }
    protected FDictionary AchievementRewardDic
    {
        get
        {
            return GameCenter.achievementMng.curhaveAchieve;
        }
    }
    #endregion

    #region 构造
    void Awake()
    {
        if (leftBtn != null)
        {
            UIEventListener.Get(leftBtn.gameObject).onClick = OnClickLeftBtn;
            leftBtn.gameObject.SetActive(false);
        }
        if (rightBtn != null) UIEventListener.Get(rightBtn.gameObject).onClick = OnClickRightBtn;
        
        achieveTypeDic = ConfigMng.Instance.GetAchieveTypeRefTable();
        achievementDic = ConfigMng.Instance.GetAchievementRefTable();
        if (togGo != null) togGo.SetActive(false);
        if(itemGo != null) itemGo.SetActive(false);
    }
    protected override void OnOpen()
    {
        base.OnOpen();
        GameCenter.achievementMng.C2S_ReqGetAchievementInfo(0);
        CreateTogAndItem();
        ChooseToggle();
        InitToggle(null);
        RefreshTotalLife();
        for (int i = 0; i < togList.Count; i++)
        {
            UIEventListener.Get(togList[i].gameObject).onClick -= InitToggle;
            UIEventListener.Get(togList[i].gameObject).onClick += InitToggle;
        }
        GameCenter.achievementMng.OnAchievementUpdate += RefreshAchievmenInfo;
        GameCenter.achievementMng.OnGetAchievementNum += RefreshTotalLife;
    }
    void ChooseToggle()
    {
        int toggle = 1;
        foreach (int id in GameCenter.achievementMng.achievementTogRed.Keys)
        {
            toggle = ConfigMng.Instance.GetAchieveType(id);
            break;
        }
        foreach (int id in AchievementRewardDic.Keys)
        {
            if (!(AchievementRewardDic[id] as AchievementData).RewardState)
            {
               toggle = ConfigMng.Instance.GetAchieveType(id);
               break;
            }
        }
        for (int i = 0, max = toggle % 6 == 0 ? toggle / 6 - 1 : toggle / 6; i < max; i++)
        {
            OnClickRightBtn(null);
        }
        togList[toggle - 1].value = true;
    }
    protected override void OnClose()
    {
        base.OnClose();
        for (int i = 0; i < togList.Count; i++)
        {
            UIEventListener.Get(togList[i].gameObject).onClick -= InitToggle;
        }
        GameCenter.achievementMng.OnAchievementUpdate -= RefreshAchievmenInfo;
        GameCenter.achievementMng.OnGetAchievementNum -= RefreshTotalLife;
        CancelInvoke("Refresh");
    }
    #endregion

    #region 创建刷新
    /// <summary>
    /// 创建Toggle和Item
    /// </summary>
    void CreateTogAndItem()
    {
        int index = 0;
        HideAllToggle();
        foreach (AchieveTypeRef data in achieveTypeDic.Values)
        {
            GameObject go = null;
            if (togList.Count < achieveTypeDic.Count)
            {
                go = Instantiate(togGo) as GameObject;
                togList.Add(go.GetComponent<UIToggle>());
            }
            go = togList[index].gameObject;
            go.transform.parent = togGo.transform.parent;
            go.transform.localPosition = new Vector3(-253 + index * 105, 2.5f, 0f);
            go.transform.localScale = Vector3.one;
            AchievementTogUI achievementTogUI = go.GetComponent<AchievementTogUI>();
            if (achievementTogUI != null)
                achievementTogUI.SetTogData(data, index);
            //创建Item
            HideAllItem();
            if (itemGird != null) itemGird.maxPerLine = data.numId.Count;
            for (int i = 0; i < data.numId.Count; i++)
            {
                GameObject item = null;
                if (itemList.Count < data.numId.Count)
                {
                    item = Instantiate(itemGo) as GameObject;
                    itemList.Add(item);
                }
                item = itemList[i];
                item.transform.parent = itemGo.transform.parent;
                item.transform.localPosition = Vector3.zero;
                item.transform.localScale = Vector3.one;
                item.SetActive(true);
            }
            if (itemGird != null) itemGird.repositionNow = true;
            index++;
            go.SetActive(true);
       }
    }
    /// <summary>
    /// 隐藏所有Toggle
    /// </summary>
    void HideAllToggle()
    {
        for (int i = 0; i < togList.Count; i++)
        {
            if (togList[i] != null) togList[i].gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// 隐藏所有Item
    /// </summary>
    void HideAllItem()
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i] != null) itemList[i].gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// 切换Toggle
    /// </summary>
    void InitToggle(GameObject go)
    {
        for (int i = 0; i < togList.Count; i++)
        {
            if (togList[i].value)
            {
                GameCenter.achievementMng.IsGetAchieve = false;
                GameCenter.achievementMng.C2S_ReqGetAchievementInfo(i + 1);
                if (itemScroll != null) itemScroll.ResetPosition();
                CancelInvoke("Refresh");
                Invoke("Refresh", 0.1f);
            }
        }
    }
    //如果没有后台返回才刷新
    void Refresh()
    {
        for (int i = 0; i < togList.Count; i++)
        {
            if (togList[i].value && !GameCenter.achievementMng.IsGetAchieve)
            {
                for (int j = 0; j < (achieveTypeDic[i + 1] as AchieveTypeRef).numId.Count; j++)
                {
                    AchievementItemUI achievementItemUI = itemList[j].GetComponent<AchievementItemUI>();
                    if (achievementItemUI != null)
                        achievementItemUI.SetAchievementItemData(achievementDic[(achieveTypeDic[i + 1] as AchieveTypeRef).numId[j]] as AchievementRef, i);
                }
            }
        }
    }
    /// <summary>
    /// 刷新
    /// </summary>
    void RefreshAchievmenInfo()
    {
        for (int i = 0; i < togList.Count; i++)
        {
            if (togList[i].value)
            {
               List<AchievementData> list = GameCenter.achievementMng.GetAchievementDataList(i + 1);
               for (int j = 0; j < list.Count; j++)
               {
                   AchievementItemUI achievementItemUI = itemList[j].GetComponent<AchievementItemUI>();
                   if (achievementItemUI != null)
                       achievementItemUI.SetAchievementItemData(achievementDic[list[j].ID] as AchievementRef, i);
               }
            }
        }
        
    }

    /// <summary>
    /// 刷新总生命值
    /// </summary>
    void RefreshTotalLife()
    {
        for (int i = 0; i < togList.Count; i++)
        {
            AchievementTogUI togUI = togList[i].GetComponent<AchievementTogUI>();
            if (togUI != null && achieveTypeDic.ContainsKey(i + 1))
                togUI.SetTogData(achieveTypeDic[i + 1] as AchieveTypeRef ,i);
        }
        int totalNum = 0;
        foreach (int data in AchieveNumDic.Values)
        {
            totalNum += data ;
        }
        if(currNum != null)
            currNum.text = (totalNum * 1000).ToString();
        if(progressSlider != null)
            progressSlider.value = (float)(totalNum * 1000)/75000;
    }

    /// <summary>
    /// 点击向左切换Toggle按钮
    /// </summary>
    void OnClickLeftBtn(GameObject go)
    {
        if (rightBtn != null)
            rightBtn.gameObject.SetActive(true);
        for (int i = 0; i < togList.Count; i++)
        {
            togList[i].transform.localPosition = new Vector3(togList[i].transform.localPosition.x + 630, togList[i].transform.localPosition.y, -1);
        }
        if (togList[0].transform.localPosition.x >= -255 && leftBtn != null)
            leftBtn.gameObject.SetActive(false);
    }
    /// <summary>
    /// 点击向右切换Toggle按钮
    /// </summary>
    /// <param name="obj"></param>
    void OnClickRightBtn(GameObject go)
    {
        if(leftBtn != null)
            leftBtn.gameObject.SetActive(true);
        for (int i = 0; i < togList.Count; i++)
        {
            togList[i].transform.localPosition = new Vector3(togList[i].transform.localPosition.x - 630, togList[i].transform.localPosition.y, -1);
        }
        if (togList[14].transform.localPosition.x <= 248 && rightBtn != null) 
            rightBtn.gameObject.SetActive(false);
    }
    #endregion
}

