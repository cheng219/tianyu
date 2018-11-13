using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ActivityWnd : GUIBase {

	public UILabel aName;
	public UILabel aitemDes;
	public UILabel aDes;
	
	public GameObject item;
	public GameObject[] btnFunc;
	public GameObject btnClose;
	
	public GameObject activityItem;
    public GameObject activityInfomation;
    private UIPanel panel;
    private Vector3 position;
    private Vector2 offect;
	
	void Awake(){
		mutualExclusion = true;
		Layer = GUIZLayer.TOPWINDOW;
		if(btnClose != null)UIEventListener.Get(btnClose).onClick =delegate {
			Invoke("InvokeClose",0.1f);
		};
		if(activityItem != null){
			activityItem.SetActive(false);
			ActivityItemUI item = activityItem.GetComponent<ActivityItemUI>();
			if(item != null)inItemList.Add(item);
		}
        //view = this.item.transform.parent.GetComponent<UIScrollView>();
		GameCenter.activityMng.C2S_ActivityDataInfo();
        //Awake的时候 获取一下初始位置和Clipping Offect
        if (activityInfomation != null)
        {
            position = activityInfomation.transform.position;
            panel = activityInfomation.GetComponent<UIPanel>();
            if (panel != null)
                offect = panel.clipOffset;
            else
                Debug.LogError("名为"+ activityInfomation.gameObject.name+"的预制上没有UIPanel组件");
        }
    }
	void InvokeClose()
	{
		GameCenter.uIMng.SwitchToUI(GUIType.NONE);
	}
	ActivityItemUI curData = null;
	List<GameObject> listItem = new List<GameObject>();
	void ShowItems(){
		for(int i=0;i< inItemList.Count;i++){
			UIToggle tog = inItemList[i].GetComponent<UIToggle>();
			if(tog != null&&tog.value){
				curData = inItemList[i];
				break ;
			}
		}
		if(curData == null || curData.SetData == null){
			return ;
		}
		if(aName != null)aName.text = curData.SetData.title;
		if(aitemDes != null)aitemDes.text = curData.SetData.rewardres;
		if(aDes != null)aDes.text = curData.SetData.res.Replace("\\n","\n");
		
		if(item == null)return ;
		int j=0,len=0;
        for (int m = 0; m < curData.SetData.rewarditem.Count; m++)
        {
            if (curData.SetData.rewarditem[m] != 0)
                len++;
        }
		GameObject go = null;
		for(;j<len;j++){
			if(listItem.Count <= j){
				go = UIUtil.CreateItemUIGame(item);
				go.transform.localPosition = Vector3.zero;
				go.transform.localScale = Vector3.one;
				listItem.Add(go);
			}else{
				go = listItem[j];
			}
			ItemUI itemui = go.GetComponent<ItemUI>();
			if(itemui != null)itemui.FillInfo(new EquipmentInfo(curData.SetData.rewarditem[j],EquipmentBelongTo.PREVIEW));
			go.SetActive(true);
		}
		for(;j<listItem.Count;j++){
			listItem[j].SetActive(false);
		}

		UIGrid grid = item.GetComponent<UIGrid>();
		if(grid != null)grid.repositionNow = true;

		for(int i=0;i<btnFunc.Length;i++){
			if(btnFunc[i] == null)continue;
			if(curData.SetData.buttontype.Count > i){
				ActivityButtonRef refdata = ConfigMng.Instance.GetActivityButtonRef(curData.SetData.buttontype[i]);
				UILabel lab = btnFunc[i].GetComponentInChildren<UILabel>();
				if(lab != null)lab.text = refdata.name;
				UIEventListener.Get(btnFunc[i]).onClick -= OnClikFunc;
				UIEventListener.Get(btnFunc[i]).onClick += OnClikFunc;
				UIEventListener.Get(btnFunc[i]).parameter  = refdata;
				btnFunc[i].gameObject.SetActive(true);
			}else{
				btnFunc[i].gameObject.SetActive(false);
			}
		}
        //刷新之后初始化位置并且初始化Clipping offect 
        if (activityInfomation != null&& panel!=null)
        {
            activityInfomation.transform.position = position;
            panel.clipOffset = offect;
        }
        else
        {
            Debug.LogError("ActivityInfomation上有组件为空");
        }
    }
	
	void OnClikFunc(GameObject games){
		if(curData == null || curData.SetData == null){
			return ;
		}
		ActivityButtonRef refdata = UIEventListener.Get(games).parameter as ActivityButtonRef;
		GameCenter.activityMng.GoActivityButtonFunc(refdata,curData.SetData.id);
	}

    protected Dictionary<int, ActivityListRef> dic = new Dictionary<int, ActivityListRef>();
    protected List<ActivityDataInfo> refActList = new List<ActivityDataInfo>();
    /// <summary>
    /// 排序
    /// </summary>
    void SortActivity()
    {
        refActList.Clear();
        foreach (ActivityListRef refdata in ConfigMng.Instance.GetActivityList().Values)
        {
            ActivityDataInfo data = GameCenter.activityMng.GetActivityDataInfo(refdata.id);
            refActList.Add(data);
        }
        refActList.Sort(GameCenter.activityMng.SortActivity);
        for (int i = 0; i < refActList.Count; i++)
        {
            dic[(int)refActList[i].ID] = ConfigMng.Instance.GetActivityListRef((int)refActList[i].ID);
        }
    }

	List<ActivityItemUI> inItemList = new List<ActivityItemUI>();
	void ShowActivityList(){
		if(activityItem == null)return ;
        SortActivity();//先排序
		GameObject go = null;
		ActivityItemUI endLess = null;
		int i = 0;
		UIToggle uitog = null;
        int index = -1;
        foreach (ActivityListRef data in dic.Values)
        {
			if(data.mainInterfaceButton  == (int)ActivityUIType.NoInActivityWnd || data.level > GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel)continue;
			if(inItemList.Count <= i){
				go = (GameObject)GameObject.Instantiate(activityItem);
				go.transform.parent = activityItem.transform.parent;
				go.transform.localPosition = Vector3.zero;
				go.transform.localScale = Vector3.one;
				go.name = "copyItemety" + i;
				endLess = go.GetComponent<ActivityItemUI>();
				endLess.SetData = data;
				inItemList.Add(endLess);
				uitog = go.GetComponent<UIToggle>();
			}else{
				inItemList[i].SetData = data;
				uitog = inItemList[i].GetComponent<UIToggle>();
				go = inItemList[i].gameObject;
			}
			if(uitog == null)continue;
			EventDelegate.Remove(uitog.onChange,ShowItems);
			EventDelegate.Add(uitog.onChange,ShowItems);
			uitog.startsActive  =false;
			go.SetActive(true);
			if(GameCenter.activityMng.CurSeleteType != ActivityType.NONE && GameCenter.activityMng.CurSeleteType == data.ID){
				uitog.startsActive  =true;
                index = i + 1;
			}
			i++;
		}
		
		for(;i<inItemList.Count;i++){
			inItemList[i].gameObject.SetActive(false);
		}
        
		if(inItemList.Count >= 1 && GameCenter.activityMng.CurSeleteType == ActivityType.NONE){
			uitog = inItemList[0].GetComponent<UIToggle>();
			if(uitog != null)uitog.startsActive  =true;
		}

        //if (view != null && inItemList.Count > 4)
        //{
        //    float setVal = (float)index / inItemList.Count + 0.1f; 
        //    view.ResetPosition();
        //    view.SetDragAmount(1, setVal > 1 ? 1 : setVal, false);
        //}
	}
	
	protected override void OnOpen ()
	{
		base.OnOpen ();
		ShowActivityList();
		GameCenter.activityMng.OnActivityDataInfo += ShowActivityList;
	}
	
	protected override void OnClose ()
	{
		base.OnClose ();
		GameCenter.activityMng.OnActivityDataInfo -= ShowActivityList;
		for(int i=0;i< inItemList.Count;i++){
			UIToggle tog = inItemList[i].GetComponent<UIToggle>();
			if(tog != null)EventDelegate.Remove(tog.onChange,ShowItems);
		}
		GameCenter.activityMng.CurSeleteType = ActivityType.NONE;
	}
}
