//============================
//作者：何明军
//日期：2016/3/23
//用途：活动大厅系统数据
//============================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;

public class ActivityDataInfo{
	enum Looptype{
		/// <summary>
		/// 日循环
		/// </summary>
		DayLoop = 1,
		/// <summary>
		/// 周循环
		/// </summary>
		WeekLoop,
		/// <summary>
		/// 月循环
		/// </summary>
		MonthLoop,
	}
	int id;
	/// <summary>
	/// 活动id
	/// </summary>
	public ActivityType ID{
		get{
			return (ActivityType)id;
		}
	}
	int num = 0;
	/// <summary>
	/// 次数
	/// </summary>
	public int Num{
		get{
			return Times - num;
		}
	}
	/// <summary>
	/// 最大次数
	/// </summary>
	public int Times{
		get{
			if((ActivityType)id == ActivityType.DAILYTRANSPORTDART){
				return GameCenter.vipMng.VipData.DartNum;
			}
			return RefData != null?RefData.times:0;
		}
	}
	/// <summary>
	/// 构造
	/// </summary>
	public ActivityDataInfo(int _id){
		id = _id;
		StateUpdateTime();
	}
	/// <summary>
	/// 构造
	/// </summary>
	public ActivityDataInfo(){}
	/// <summary>
	/// 构造
	/// </summary>
	public ActivityDataInfo(st.net.NetBase.activity_list _data){
		Update(_data);
		StateUpdateTime();
	}
	/// <summary>
	/// 更新
	/// </summary>
	public void Update(st.net.NetBase.activity_list _data){
		id = _data.id;
		num = _data.challenge_num;
//		Debug.Log("name   =  "+RefData.name +"   次数    =" + num);
	}
	
	/// <summary>
	/// 更新
	/// </summary>
	public void Update(int _state,int _time){
        if (!SerVerCententTime)
        {
            Debug.LogError("活动" + RefData.name + "不是由服务器控制时间的!");
            return;
        }
		State = _state==1?ActivityState.HASENDED:ActivityState.NOTATTHE;
		
		UpdateTimePoint = _time;
		if(State == ActivityState.NOTATTHE && _time == 0){
			updateTimePoint = 0;
			State = ActivityState.ONGOING;
		}
	}
    public void UnderBossBeKilled()
    {
        State = ActivityState.NOTATTHE;
    }
	/// <summary>
	/// 状态改变事件
	/// </summary>
	public System.Action OnActivityStateUpdate;
	ActivityState state = ActivityState.NOTATTHE;
	/// <summary>
	/// 获取状态
	/// </summary>
	public ActivityState State{
		get{
			if(SerVerCententTime){
				if(UpdateTimePoint > 0 && UpDateTime <= 0){
					SetState();
				}
				return state;
			}
			if(UpDateTime <= 0){
				StateUpdateTime();
			}
			return state;
		}
		set{
			if(state != value){
				state = value;
				if(OnActivityStateUpdate != null)OnActivityStateUpdate();
			}
		}
	}
	
	/// <summary>
	/// 设置成进行中,未开始与结束由服务端通知
	/// </summary>
	public void SetState(){
		if(SerVerCententTime){
			updateTimePoint = 0;
			state = ActivityState.ONGOING;
            Debug.Log("进入到,未开始与结束由服务端通知");
		}
	}
	/// <summary>
	/// 重新初始化非服务端控制的活动的时间与状态
	/// </summary>
	public void StateUpdateTime(){
		if(!SerVerCententTime){
			state = GetInTime();
		}
	}
    /// <summary>
    /// 强制性更改活动字典中的活动数据状态
    /// </summary>
    public void SetActivityState(ActivityState _type)
    {
        state = _type;
    }
    /// <summary>
    /// 距离下次更新最短时间
    /// </summary>
    public int UpDateTime{
		get{
			int aTime = UpdateTimePoint - GameCenter.instance.CurServerTimeSecond+60;
			return aTime > 0 ? aTime :0;
		}
	}
	/// <summary>
	/// 是否到达活动等级要求
	/// </summary>
	public bool ActivityLev{
		get{
			if(RefData != null ){
				if(RefData.level <= GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel){
					return true;
				}
			}
			return false;
		}
	}

	/// <summary>
	/// 在主界面中显示返回正整数,
	/// </summary>
	public int InMainRedShow{
		get{
			if(!ActivityLev){
				return -1;
			}
			int time = UpDateTime;
			if(State == ActivityState.ONGOING || (UpdateTimePoint > 0 && State == ActivityState.NOTATTHE && time <= ActivityMng.ShowTime)){
				return time;
			}
			return -1;
		}
	}
	//时间不在今天与已结束的活动为0，由服务器控制时间的活动除未开始倒计时不为0，其他状态下都为0.
	int updateTimePoint = 0;
	/// <summary>
	/// 下次更新最短时间的时间点.秒
	/// </summary>
	int UpdateTimePoint{
		get{
			return updateTimePoint;
		}set{
			updateTimePoint = value >0 ? value + GameCenter.instance.CurServerTimeSecond : 0;
//			Debug.Log("updateTimePoint   = "+updateTimePoint);
		}
	}
	
	
	ActivityListRef refData;
	/// <summary>
	/// 配表数据
	/// </summary>
	ActivityListRef RefData{
		get{
			if(refData == null){
				refData = ConfigMng.Instance.GetActivityListRef(id);
			}
			return refData;
		}
	}
    /// <summary>
    /// 用来排序的数
    /// </summary>
    public int SortNum
    {
        get
        {
            return refData != null ? refData.listNum : 0;
        }
    }
	/// <summary>
	/// 活动按钮
	/// </summary>
	public List<int> Buttontype{
		get{
			return RefData != null ? RefData.buttontype : new List<int>();
		}
	}
	/// <summary>
	/// 时间由服务器控制
	/// </summary>
	public bool SerVerCententTime{
		get{
			return RefData != null ? RefData.mainInterfaceButton == (int)ActivityUIType.ServerActivityTime : false;
		}
	}
	
	int minTime = 0;
	DateTime newServerTime;
	ActivityState GetInTime(){
		ActivityState newState = ActivityState.NOTATTHE;
		if(RefData == null)
		{
			return ActivityState.HASENDED;
		}
		updateTimePoint = 0;
		minTime = 0;
		newServerTime = GameCenter.instance.CurServerTime;
		if(ID == ActivityType.FAIRYAUSIEGE){
			int time = GameCenter.mainPlayerMng.ServerStartTiem(3,newServerTime);
			if(time > 0){
				//UpdateTimePoint = time;//没必要在这设置距离下次更新的时间点，这里设置会导致到达三天的时间前五分钟会出现图标提示 by ljq
				return newState;
			}
		}
		
		if(RefData.looptype == (int)Looptype.DayLoop){
			newState = GetInDayTime();
		}else if(RefData.looptype == (int)Looptype.WeekLoop){
			for(int i=0;i<RefData.loopday.Count;i++){
				if(RefData.loopday[i] != 0){
					if(newServerTime.DayOfWeek == (DayOfWeek)(RefData.loopday[i]%7)){
						newState = GetInDayTime();
					}
//					else if(newServerTime.DayOfWeek == (DayOfWeek)(RefData.loopday[i]%7 - 1)){
//						return GetInDayTime(true);
//					}
				}
			}
		}else if(RefData.looptype == (int)Looptype.MonthLoop){
			newServerTime = GameCenter.instance.CurServerTime;
			for(int i=0;i<RefData.loopday.Count;i++){
				if(RefData.loopday[i] != 0 ){
					if(newServerTime.Day == RefData.loopday[i]){
						newState = GetInDayTime();
					}
//					else if(cur.Day == RefData.loopday[i] - 1){
//						return GetInDayTime(true);
//					}
				}
			}
		}
		return newState;
	}
	/// <summary>
	/// 获取状态
	/// </summary>
	ActivityState GetInDayTime(bool isTomorrow = false){
		ActivityState cur = ActivityState.NOTATTHE;
		int index = -1;
		for(int i=0;i<RefData.looptime.Count;i++){
			cur = GetInDateTime(RefData.looptime[i],isTomorrow);
			if(cur == ActivityState.ONGOING){
				UpdateTimePoint = minTime;
//				Debug.Log("name  =   "+RefData.name +"："+RefData.looptime[i].start[0]+":"+RefData.looptime[i].start[1] +"："+RefData.looptime[i].end[0]+":"+RefData.looptime[i].end[1]);
				return ActivityState.ONGOING;
			}
			if(cur == ActivityState.HASENDED){
				index ++;
			}
		}
		
		if (index >= RefData.looptime.Count - 1)
		{
			updateTimePoint = 0;
			minTime = 0;
			return ActivityState.HASENDED;
		}
		UpdateTimePoint = minTime;
		return ActivityState.NOTATTHE;
	}
//	string LoopTime(){
//		string str = "Time：";
//		foreach(ClockTimeRegion time in RefData.looptime){
//			DateTime cur = DateTime.Now.Date.AddSeconds((double)GameCenter.instance.CurServerTimeSecond());
//			DateTime sdate = cur.Date.AddHours(time.start[0]).AddSeconds(time.start[1]*60);
//			DateTime edate = cur.Date.AddHours(time.end[0]).AddSeconds(time.end[1]*60);
//			str +=  sdate +"/"+edate+"   ：   ";
//		}
//		return str;
//	}

	/// <summary>
	/// return {1=未开，2=开启，3=已结束}
	/// </summary>
	ActivityState GetInDateTime(ClockTimeRegion date,bool isTomorrow = false){
		if(date.start.Count < 2 || date.end.Count < 2){
			Debug.LogError("活动表中的活动时间格式不对，找谢凯!");
			return ActivityState.NOTATTHE;
		}
		if(newServerTime != null)newServerTime = GameCenter.instance.CurServerTime;
		DateTime sdate = isTomorrow ? newServerTime.Date.AddDays(1).AddHours(date.start[0]).AddSeconds(date.start[1]*60) : newServerTime.Date.AddHours(date.start[0]).AddSeconds(date.start[1]*60);
		DateTime edate = isTomorrow ? newServerTime.Date.AddDays(1).AddHours(date.end[0]).AddSeconds(date.end[1]*60) : newServerTime.Date.AddHours(date.end[0]).AddSeconds(date.end[1]*60);
		
		int time = 0;
		if(newServerTime.CompareTo(sdate) >= 0){
			if(newServerTime.CompareTo(edate) < 0){
				time = (int)(edate-newServerTime).TotalSeconds;
				if(time > 0)minTime = time;
				return ActivityState.ONGOING;
			}
			if(newServerTime.CompareTo(edate) >= 0){
				return ActivityState.HASENDED;
			}
		}		
		time = (int)(sdate - newServerTime).TotalSeconds;
		if(minTime == 0){
			minTime = time;
		}
		if(time < minTime){
			minTime = time;
		}
		return ActivityState.NOTATTHE;
	}
}
