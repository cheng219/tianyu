//==============================================
//作者：唐源
//日期：2017/4/15
//用途：七日挑战单个窗口
//==============================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SevenChallengeSingle : MonoBehaviour {
    #region 字段
    public  SevenChallengeSingleUI singleItem;
    protected List<SevenChallengeSingleUI> singleItemList = new List<SevenChallengeSingleUI>();
    private List<SevenDaysTaskRef> sevenDaysTaskList = new List<SevenDaysTaskRef>();
    #endregion
    #region UI控件
    public UIButton getReward;
    public UILabel finish;
    public ItemUIContainer items;
    private SevenDaysTaskRewardRef dataRef;
    private SevenDaysTaskRef taskDataRef;
    public UISprite image;
    public UIGrid grid;
    //public GameObject item;
    public UILabel title;
    public UILabel content;
    public UISpriteEx reward;
    public UIButton btnCloseSingle;
    public UILabel dayLab;
    public BoxCollider rewardBox;
    public UIScrollView scrollView;
    #endregion
    #region unity函数
    void Awake()
    {
        GameCenter.sevenChallengeMng.updateSevenChallengeSingleUI += Refresh;
        if(btnCloseSingle!=null)
        {
            UIEventListener.Get(btnCloseSingle.gameObject).onClick = CloseSingle;
        }
       
    }
    void OnDestroy()
    {
        GameCenter.sevenChallengeMng.updateSevenChallengeSingleUI -= Refresh;
    }
    #endregion
    #region 界面的刷新显示
    public void Refresh(int _day)
    {
        dataRef = ConfigMng.Instance.GetSevenChallengeRewardRef(_day);
        if (dataRef == null)
        {
            return;
        }
        bool state = GameCenter.sevenChallengeMng.rewardSevenChallenge.ContainsKey(_day)?GameCenter.sevenChallengeMng.rewardSevenChallenge[_day]:false;//没判断是否Dictionary中是否有这个key
        //FillRef(_day);
        //return;
        //int day = GameCenter.sevenChallengeMng.CurDay;

        //FillRef(day);
        if (dataRef!=null)
        {
            if (title != null)
                title.text = dataRef.des1;
        }
        if(content!=null)
        {
            dataRef.des2.Replace("\\n", "\n");
            content.text = dataRef.des2;
        }
        if(image!=null)
        {
            image.spriteName = dataRef.Pic;
        }
        if (getReward != null)
            UIEventListener.Get(getReward.gameObject).onClick = GetReward;

        if (dayLab != null)
            dayLab.text = _day.ToString();
        if(state)
        {
            if (reward != null)
            {
                reward.IsGray = UISpriteEx.ColorGray.Gray;
                if (rewardBox != null)
                {
                    rewardBox.enabled = false;
                }
            }
            if (finish != null)
                finish.text = ConfigMng.Instance.GetUItext(339);
        }
        else
        {
           if(GameCenter.sevenChallengeMng.CurTimes==7)
            {
                if (rewardBox != null)
                {
                    rewardBox.enabled = true;
                }
                if (reward!=null)
                    reward.IsGray = UISpriteEx.ColorGray.normal;
                if (finish != null)
                    finish.text = ConfigMng.Instance.GetUItext(340);
            }
           else 
            {
                if(reward!=null)
                {
                    reward.IsGray = UISpriteEx.ColorGray.Gray;
                    if (rewardBox != null)
                    {
                        rewardBox.enabled = false;
                    }
                }
                if (finish != null)
                    finish.text = ConfigMng.Instance.GetUItext(340);
            }
        }
        List<ItemValue> list = new List<ItemValue>();
        for (int j = 0, count = dataRef.reward.Count; j < count; j++)
        {
            ItemValue Iitem = new ItemValue(dataRef.reward[j], dataRef.rewardnum[j]);
            list.Add(Iitem);
        }
        if (items != null)
        {
            items.RefreshItems(list, list.Count, list.Count);
            UIExGrid IGrid = items.GetComponent<UIExGrid>();
            if (IGrid!=null)
            {
                IGrid.cellWidth = 85;
                IGrid.cellHeight = 85;
            }
        }
        FillRef(_day);
    }
    //关闭小窗口
    void CloseSingle(GameObject _obj)
    {
        this.gameObject.SetActive(false);
    }
    void GetReward(GameObject _obj)
    {
        GameCenter.sevenChallengeMng.C2S_ReqSevenChallengeInfo(3, GameCenter.sevenChallengeMng.CurDay);
        if (reward != null)
        {
            reward.IsGray = UISpriteEx.ColorGray.Gray;
        }
        if (rewardBox != null)
        {
            rewardBox.enabled = false;
        }
        if (finish != null)
            finish.text = ConfigMng.Instance.GetUItext(339);
    }
    public void FillRef(int _day)
    {
        sevenDaysTaskList = ConfigMng.Instance.GetSevenChallengeTaskListRef(_day);
        for (int i = 0,length=singleItemList.Count; i < length; i++)
        {
            if (singleItemList[i] != null) singleItemList[i].gameObject.SetActive(false);
        }
        for (int i = 0,length=sevenDaysTaskList.Count; i < length; i++)
        {
            SevenDaysTaskRef data = sevenDaysTaskList[i];
            if (i >= singleItemList.Count)
            {
                SevenChallengeSingleUI single = SevenChallengeSingleUI.CreateNew(grid.transform,singleItem.gameObject);
                if(single != null)singleItemList.Add(single);
            }
            SevenChallengeSingleUI item = singleItemList[i];
            if (item != null && data != null)
            {
                item.gameObject.SetActive(true);
                item.FillInfo(data);
            }
        }
        if (grid != null) grid.repositionNow = true;
        if (scrollView != null) scrollView.SetDragAmount(0, 1, false);
    }
    int Compare(st.net.NetBase.single_day_info data1, st.net.NetBase.single_day_info data2)
    {
        if ((int)data1.task_id > (int)data2.task_id)
            return 1;
        else if ((int)data1.task_id < (int)data2.task_id)
            return -1;
        return 0;
    }
    #endregion
}
