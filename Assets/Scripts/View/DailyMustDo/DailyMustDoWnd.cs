//===============================
//作者：邓成
//日期：2016/7/7
//用途：每日必做界面类
//===============================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DailyMustDoWnd : GUIBase {

	public UIToggle[] toggles;
	public UIGrid itemParent;
	public UIScrollView scrollView;
	public DailyMustDoItemUI dailyMustDoItemUI;
	public UIButton btnClose;
    public LivelyRewardItemUI[] livelyItems;
    public UIProgressBar livelyProgress;
    public UILabel livelyCount;
    public LivelyRewardBoxUI boxUI;

	public int preLoadCount = 12;

	protected MustDoType CurMustDoType = MustDoType.UPGRADE;
	protected List<DailyMustDoItemUI> mustDoItemList = new List<DailyMustDoItemUI>();
	void Awake()
	{
		layer = GUIZLayer.NORMALWINDOW;
		mutualExclusion = true;
		if(btnClose != null)UIEventListener.Get(btnClose.gameObject).onClick = CloseWnd;
		if(toggles != null)
		{
			for (int i = 0,max=toggles.Length; i < max; i++) {
				if(toggles[i] != null)
                    EventDelegate.Remove(toggles[i].onChange, OnChange);
					EventDelegate.Add(toggles[i].onChange,OnChange);
			}
		}
	}
	protected override void OnOpen ()
	{
		base.OnOpen ();
		PreLoadItems();
		GameCenter.dailyMustDoMng.C2S_ReqMustDoData();
	}
	protected override void OnClose ()
	{
		base.OnClose ();
	}
	protected override void HandEvent (bool _bind)
	{
		base.HandEvent (_bind);
		if(_bind)
		{
			GameCenter.dailyMustDoMng.OnDailyMustDoUpdateEvent += ShowMustDoItems;
            GameCenter.dailyMustDoMng.OnDailyMustDoStateUpdateEvent += ShowLivelyData;
		}else
		{
			GameCenter.dailyMustDoMng.OnDailyMustDoUpdateEvent -= ShowMustDoItems;
            GameCenter.dailyMustDoMng.OnDailyMustDoStateUpdateEvent -= ShowLivelyData;
		}
	}

	void OnChange()
	{
		if(toggles != null)
		{
			for (int i = 0,max=toggles.Length; i < max; i++) {
				if(toggles[i] != null && toggles[i].value)
					CurMustDoType = (MustDoType)(i+1);
			}
		}
		ShowMustDoItems();
	}

	void ShowMustDoItems()
	{
		for (int i = 0; i < mustDoItemList.Count; i++)
		{
			mustDoItemList[i].gameObject.SetActive(false);
		}
		List<DailyMustDoInfo> itemList = GameCenter.dailyMustDoMng.GetDailyMustDoItemByType(CurMustDoType);
		for (int i = 0,max=itemList.Count; i < max; i++) 
		{
			if (mustDoItemList.Count < i + 1)
			{
				mustDoItemList.Add(dailyMustDoItemUI.CreateNew(itemParent.transform));
			}
			mustDoItemList[i].gameObject.SetActive(true);
			mustDoItemList[i].SetData(itemList[i]);
		}
		if(itemParent != null)itemParent.repositionNow = true;
		if(scrollView != null)scrollView.SetDragAmount(0,0,false);
        ShowLivelyData();
	}
    void ShowLivelyData()
    {
        if (livelyItems != null)
        {
            FDictionary livelyReward = ConfigMng.Instance.GetLivelyRewardRefTable();
            int index = 0;
            foreach (LivelyRewardRef reward in livelyReward.Values)
            {
                if (livelyItems.Length > index && livelyItems[index] != null)
                {
                    livelyItems[index].SetData(reward, ShowLivelyBoxReward);
                }
                index++;
            }
        }
        if (livelyProgress != null) livelyProgress.value = Mathf.Min(1f, (float)GameCenter.dailyMustDoMng.CurLivelyCount / (float)GameCenter.dailyMustDoMng.TotalLivelyCount);
        if (livelyCount != null) livelyCount.text = GameCenter.dailyMustDoMng.CurLivelyCount.ToString();
    }
    void ShowLivelyBoxReward(LivelyRewardRef _livelyRewardRef)
    {
        if (boxUI != null) boxUI.SetData(_livelyRewardRef);
    }
	void PreLoadItems()
	{
		for (int i = 0; i < preLoadCount; i++) 
		{
			if (mustDoItemList.Count < i + 1)
			{
				mustDoItemList.Add(dailyMustDoItemUI.CreateNew(itemParent.transform));
			}
		}
	}
	protected void CloseWnd(GameObject go)
	{
		GameCenter.uIMng.SwitchToUI(GUIType.NONE);
	}
}
