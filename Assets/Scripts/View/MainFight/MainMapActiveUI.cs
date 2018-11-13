/// <summary>
/// 活动大厅主界面显示UI
/// 何明军
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainMapActiveUI : MonoBehaviour {

	public GameObject target;
	
	public GameObject itemSample;
	
	UIGrid grid;
	List<ActivityItemUI> inItemList = new List<ActivityItemUI>();
	
	void Awake(){
		grid = target.GetComponent<UIGrid>();
		if(itemSample != null){
			itemSample.SetActive(false);
			ActivityItemUI activity = itemSample.GetComponent<ActivityItemUI>();
			inItemList.Add(activity);
		}
	}
	
	
	void ShowMainItem(){
		if(itemSample == null) return ;
		GameObject go = null;
		ActivityItemUI endLess = null;
		int i=0;
		int minTime = 0;
		bool redPoint = false;
		foreach(ActivityListRef data in ConfigMng.Instance.GetActivityList().Values){
			ActivityDataInfo SerData = GameCenter.activityMng.GetActivityDataInfo((int)data.ID);
			if(SerData == null){
				continue;
			}
			int atime = SerData.InMainRedShow;
			ActivityState state = SerData.State;
            //Debug.Log("SetState   = " + data.name + "  ///  ActivityState  ==  " + state + "   ----InMainRedShow---=   " + atime + " === SerData.UpDateTime" + SerData.UpDateTime);
			if(atime < 0){
				if(state == ActivityState.NOTATTHE){
					int curTime = SerData.UpDateTime;
					atime = curTime - ActivityMng.ShowTime > 0 ? curTime - ActivityMng.ShowTime : 0;
					if(minTime == 0 && atime > 0){
						minTime = atime;
					}
					if(atime < minTime && atime > 0){
						minTime = atime;
					}
				}
				continue;
			}
			else{
				if(inItemList.Count <= i){
					go = (GameObject)GameObject.Instantiate(itemSample);
					go.transform.parent = itemSample.transform.parent;
					go.transform.localPosition = Vector3.zero;
					go.transform.localScale = Vector3.one;
					endLess = go.GetComponent<ActivityItemUI>();
					inItemList.Add(endLess);
				}else{
					go = inItemList[i].gameObject;
					endLess = inItemList[i];
				}
//				go.name = "Activity_" + data.id;
				endLess.SetData = data;
//				endLess.SetMainWndItemTime(atime);
				if(state == ActivityState.ONGOING){
					endLess.OnOverTime -= OnOverTime;
					endLess.OnOverTime += OnOverTime;
					if(data.ID != ActivityType.UNDERBOSS)redPoint = true;
				}
				UIEventListener.Get(go).onClick = OnClickGame;
				UIEventListener.Get(go).parameter = SerData;
				go.SetActive(true);
				i++;
			}
		}
//		itemSample.SetActive(i==0);
		for(;i<inItemList.Count;i++){
			inItemList[i].OnOverTime -= OnOverTime;
			inItemList[i].gameObject.SetActive(false);
		}
		GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.ACTIVITY,redPoint);
		if(grid != null)grid.repositionNow = true;
//		Debug.Log("minTime   +++   =  "+minTime);
		if(minTime > 0){
			CancelInvoke("ShowUpdate");
			Invoke("ShowUpdate",(float)minTime);
		}
	}
	
	void OnClickGame(GameObject games){
		ActivityDataInfo SerData = UIEventListener.Get(games).parameter as ActivityDataInfo;
		if(SerData.State == ActivityState.NOTATTHE && SerData.UpDateTime > 0){
			GameCenter.messageMng.AddClientMsg(173);
			return ;
		}
		if(SerData.Buttontype.Count > 1){
			GameCenter.activityMng.OpenStartSeleteActivity(SerData.ID);
		}else{
			ActivityButtonRef refdata = ConfigMng.Instance.GetActivityButtonRef(SerData.Buttontype[0]);
			GameCenter.activityMng.GoActivityButtonFunc(refdata,(int)SerData.ID);
		}
	}
	
	bool start = false;
	void OnOverTime(GameObject games){
//		games.SetActive(false);
		if(!start){
			start = true;
			ShowUpdate();
		}
//		if(grid != null)grid.repositionNow = true;
	}
	
	void ShowUpdate(){
		ShowMainItem();
		start = false;
	}
	
	void OnEnable(){
		GameCenter.activityMng.OnActivityDataInfo += ShowUpdate;
		GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate += OnBaseUpdateMap;
		ShowMainItem();
	}
	
	void OnDisable(){
		GameCenter.activityMng.OnActivityDataInfo -= ShowUpdate;
		GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate -= OnBaseUpdateMap;
		CancelInvoke("ShowUpdate");
	}
	
	public  void OnBaseUpdateMap(ActorBaseTag _a, ulong _b,bool _c)
	{
		if(_a == ActorBaseTag.Level){
			ShowMainItem();
		}
	}
}
