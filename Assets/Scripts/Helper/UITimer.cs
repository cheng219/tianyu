/***************************************
 * 
 *        UI计时显示组件
 * Ver.1.0  Author:黄虹学
 * 
 **************************************/

using UnityEngine;
using System.Collections;

public class UITimer : MonoBehaviour {
	
	public UILabel timeDisplay;
	
	public delegate void VoidDelegate(GameObject go);
	public VoidDelegate onTimeOut;
	public VoidDelegate onTimeWarning;
	/// <summary>
	/// 凌晨事件
	/// </summary>
	public System.Action OnBeforeDawn;
	
	public bool isCountDown = false;
	public bool isCurServerTime = false;
	
	public bool isTimeWarning = false;
	public int warningTime = 0;
	public Color warningColor = Color.red;
    public bool showSecond =true;
	bool isWarned = false;
	Color srcColor;


	public enum TimeShowMode
	{
		Electron,
		Chinese,
		SECONDS,//只显示秒
		ChineseItem,//限时物品特殊显示
	}
	public TimeShowMode timeShowMode = TimeShowMode.Electron;
	
	int targetTime = -1;
	int lastTime = -1;
	
	/// <summary>
	/// 以当前时间开始计时.
	/// </summary>
	public void StartCurServerTimer(int time)
	{
		StartTimer((int)time);
	}
	/// <summary>
	/// 以当前时间开始计时.
	/// </summary>
	public void StartTimer()
	{
		StartTimer((int)Time.realtimeSinceStartup);
	}
	
	/// <summary>
	/// 使用目标时间开始计时.
	/// </summary>
	/// <param name='time'>
	/// 目标时间.顺计时为计时的“起始时间”；倒计时为目标时间点，使用Time.realtimeSinceStartup+间隔时间
	/// </param>
	public void StartTimer(int time)
	{
		targetTime = time;
		ResetWarning();
	}
	/// <summary>
	/// 使用间隔时间开始计时.
	/// </summary>
	/// <returns>
	/// 计时器使用的目标时间点.
	/// </returns>
	/// <param name='time'>
	/// Time.
	/// </param>
	public int StartIntervalTimer(int time)
	{
		int tarTime = (int)Time.realtimeSinceStartup+time;
		StartTimer(tarTime);
		return tarTime;
	}
	
	public void StopTimer()
	{
		targetTime = -1;
		ShowTime(0);
	}
	int stopTime = 0;
	/// <summary>
	/// 是否暂停计时
	/// </summary>
	public bool StopTime{
		get{
			return stopTime > 0;
		}
		set{
			if(value){
				stopTime = (int)Time.realtimeSinceStartup;
			}else{
				targetTime += (int)Time.realtimeSinceStartup - stopTime;
				stopTime = 0;
			}
		}
	}
	
	// Use this for initialization
	void Start () {
		if(timeDisplay != null)
		{
			srcColor = timeDisplay.color;
		}
		stopTime = 0;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(!IsStartTimer || stopTime > 0)
		{
			return;
		}
		int time = (int)Time.realtimeSinceStartup;
		if( lastTime == time )
		{
			return;
		}
		lastTime = time;
		int showTime = 0;
		if(isCountDown && !isCurServerTime)
		{
			showTime = targetTime - time;//从start开始算,而不是从Startup开始算 by邓成
			if(showTime<=0)
			{
				if(onTimeOut != null)
				{
					onTimeOut(gameObject);
				}
				StopTimer();
			}
			if(NeedWarning && showTime < warningTime)
			{
				DoWarning();
			}
		}
		else if(!isCountDown && !isCurServerTime)
		{
			showTime = time - targetTime;
			if(NeedWarning && showTime > warningTime)
			{
				DoWarning();
			}
		}
		else if(!isCountDown && isCurServerTime)
		{
			showTime = targetTime + time;
		}
		
		showTime = showTime<0 ? 0 : showTime;
		if(showTime == GameCenter.instance.BeforeDawnTime){
			if(OnBeforeDawn != null)OnBeforeDawn();
		}
		ShowTime(showTime);
	}
	
	void ShowTime (int time)
	{
		if(timeDisplay == null)
		{
			return;
		}
		int tmp = time;
		int s = tmp%60;
		tmp /= 60;
		int m = tmp%60;
		int h = tmp/60;
        if (h >= 24 && !isCountDown)
        {
			h = h - 24;
		}
		switch(timeShowMode)
		{
		case TimeShowMode.Electron:
			if(h>0)
			{
                    if(showSecond)
				         timeDisplay.text = string.Format("{0:D2}:{1:D2}:{2:D2}",h,m,s);//翔又说任何情况下都是时分秒的格式
                    else
                        timeDisplay.text = string.Format("{0:D2}:{1:D2}", h, m);//翔说不要秒
			}
			else
			{
				if(isCountDown)
				{
					timeDisplay.text = string.Format("{0:D2}:{1:D2}",m,s);//只有倒计时才不需要小时
				}else
				{
					timeDisplay.text = string.Format("{0:D2}:{1:D2}:{2:D2}",h,m,s);
				}
			}
			break;
		case TimeShowMode.Chinese:
			if(h>0)
			{
				timeDisplay.text = string.Format(ConfigMng.Instance.GetUItext(196),h,m,s);
			}
			else if(m>0)
			{
				timeDisplay.text = string.Format(ConfigMng.Instance.GetUItext(197),m,s);
			}
			else
			{
				timeDisplay.text = string.Format(ConfigMng.Instance.GetUItext(198),m,s);
			}
			break;
		case TimeShowMode.ChineseItem:
			if(h>0)
			{
				timeDisplay.text = string.Format(ConfigMng.Instance.GetUItext(196),h,m,s);
			}
			else if(m>0)
			{
				timeDisplay.text = string.Format(ConfigMng.Instance.GetUItext(197),m,s);
			}
			else if(s > 0)
			{
				timeDisplay.text = string.Format("{0}",ConfigMng.Instance.GetUItext(199));
			}else{
				timeDisplay.text = string.Format("{0}",ConfigMng.Instance.GetUItext(200));
			}
			break;
		case TimeShowMode.SECONDS:
			timeDisplay.text  = time.ToString();
			break;
		}
	}
	
	bool IsStartTimer
	{
		get
		{
			return targetTime >= 0;
		}
	}
	
	bool NeedWarning
	{
		get
		{
			return isTimeWarning && !isWarned;
		}
	}
	
	void DoWarning()
	{
		isWarned = true;
		if(timeDisplay != null)
		{
			timeDisplay.color = warningColor;
		}
		if(onTimeWarning != null)
		{
			onTimeWarning(gameObject);
		}
	}
	
	void ResetWarning()
	{
		if(isWarned)
		{
			isWarned = false;
			if(timeDisplay != null)
			{
				timeDisplay.color = srcColor;
			}
		}
	}
}
