//====================================
//作者：吴江
//日期：20170620
//====================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using System;
using System.Linq;    
using System.Text;    
using System.IO;    
using System.Net;    
using System.Net.Sockets;    
using System.Text.RegularExpressions;    
using System.Runtime.InteropServices;  
using System.Runtime;
using st.net.NetBase;



public class TimeCallbackInfo
{
    public object msg;
    public int waitSeconds = 0;
    public System.Action<object> callBack;

    public TimeCallbackInfo(int _waitSeconds, System.Action<object> _callBack,object _msg = null)
    {
        waitSeconds = _waitSeconds;
        callBack = _callBack;
        msg = _msg;
    }
}

/// <summary>
/// 时间消息管理类
/// </summary>
public class TimeMng
{

    #region 数据定义
    /// <summary>
    /// 当前的时间计时
    /// </summary>
    private Timer curTimer;
    /// <summary>
    /// 当前游戏的运行时间
    /// </summary>
    protected int curRunTime;

    protected List<TimeCallbackInfo> timeCallBackList = new List<TimeCallbackInfo>();
    /// <summary>
    /// 服务器时间
    /// </summary>
    public int timeDate;
    public int offsetTimeDate;
    public DateTime TimeDate
    {
        get
        {
            return TimeStamp.GetUinxTime(timeDate + (int)Time.time - offsetTimeDate);
        }
    }

    /// <summary>
    /// 当前时间（秒数）
    /// </summary>
    public int CurTime
    {
        get
        {
            return (timeDate + (int)Time.time - offsetTimeDate);
        }
    }


    protected System.Text.StringBuilder timeNowStrBuilder = new System.Text.StringBuilder(128);
    /// <summary>
    /// 当前时间 
    /// </summary>
    /// <returns></returns>
    public string TimeNow
    {
        get
        {
            timeNowStrBuilder.Remove(0, timeNowStrBuilder.Length);
            timeNowStrBuilder.Append(TimeDate.Hour).Append(":").Append(TimeDate.Minute);
            return timeNowStrBuilder.ToString();
        }
    }
    #endregion

    #region 初始化

    public static TimeMng CreateNew()
    {
        if (GameCenter.timeMng == null)
        {
            TimeMng timeMng = new TimeMng();
            timeMng.Init();
            return timeMng;
        }
        else
        {
            GameCenter.timeMng.UnRegist();
            GameCenter.timeMng.Init();
            return GameCenter.timeMng;
        }
    }

    /// <summary>
    /// 注册
    /// </summary>
    void Init()
    {
        //MsgHander.Regist(0xD236, S2C_GetTimeResult);
        BeginTimeStart();

    }

    /// <summary>
    /// 注销
    /// </summary>
    void UnRegist()
    {
        //MsgHander.UnRegist(0xD236, S2C_GetTimeResult);
        EndTime();

    }




    #endregion

    #region 协议

    #region S2C
    /// <summary>
    /// 获取服务器系统时间
    /// </summary>
    /// <param name="_info"></param>
    //protected void S2C_GetTimeResult(Pt _info)
    //{
    //    pt_system_time_d236 msg = _info as pt_system_time_d236;
    //    if (msg != null)
    //    {
    //        timeDate = (msg.time);
    //        offsetTimeDate = (int)Time.time;
    //    }
    //}
    #endregion

    #region C2S
    /// <summary>
    /// 请求当前时间
    /// </summary>
    public void C2S_AskNowTime()
    {
        pt_action_d002 msg = new pt_action_d002();
        msg.action = 42;
        NetMsgMng.SendMsg(msg);
    }
    #endregion

    #endregion


    #region 辅助逻辑

    /// <summary>
    /// 开始计时
    /// </summary>
    protected void BeginTimeStart() 
    {
        if (curTimer == null)
        {
            curTimer = new Timer();
        }
        curRunTime = (int)Time.realtimeSinceStartup;
        curTimer.Interval = 1000;
        curTimer.Enabled = true;
        curTimer.Elapsed += new ElapsedEventHandler(theout);
    }

    /// <summary>
    /// 时间事件的抛出（每秒）
    /// </summary>
    public System.Action TimeEvent;

    /// <summary>
    /// 时间事件的抛出
    /// </summary>
    /// <param name="_sender"></param>
    /// <param name="e"></param>
    void theout(object _sender, ElapsedEventArgs e)
    {
        curRunTime++;
        if (TimeEvent != null) TimeEvent();
        if (timeCallBackList.Count > 0)
        {
            List<TimeCallbackInfo> endList = null;
            for (int i = 0; i < timeCallBackList.Count; i++)
            {
                TimeCallbackInfo temp = timeCallBackList[i];
                temp.waitSeconds--;
                if (temp.waitSeconds == 0)
                {
                    if (temp.callBack != null)
                    {
                        temp.callBack(temp.msg);
                    }
                    if (endList == null)
                    {
                        endList = new List<TimeCallbackInfo>();
                        endList.Add(temp);
                    }
                }
            }
            if (endList.Count > 0)
            {
                for (int i = 0; i < endList.Count; i++)
                {
                    timeCallBackList.Remove(endList[i]);
                }
                endList.Clear();
            }
        }
    } 

    /// <summary>
    /// 结束计时
    /// </summary>
    public void EndTime() 
    {
        if (curTimer != null)
        {
            curTimer.Enabled = false;
            curTimer.Close();
        }
    }


    public void PushTimeCallBack(int _waitSeconds, System.Action<object> _callBack, object _msg = null)
    {
        timeCallBackList.Add(new TimeCallbackInfo(_waitSeconds, _callBack, _msg));
    }

    public void CleanTimeCallBack()
    {
        timeCallBackList.Clear();
    }
    #endregion

    #region 访问器

    /// <summary>
    /// 当前游戏的运行时间
    /// </summary>
    public int CurRunTime 
    {
        get 
        {
            return curRunTime;
        }
    }

    /// <summary>
    /// 获取网络时间
    /// </summary>
    /// <returns></returns>
    public DateTime GetBeijingTime() 
    {
        DateTime dt = DateTime.Now;
        WebRequest wrt = null;
        WebResponse wrp = null;
        try 
        {
            wrt = WebRequest.Create("http://www.beijing-time.org/time.asp"); 
            wrp = wrt.GetResponse();
            string html = string.Empty;
            using (Stream stream = wrp.GetResponseStream())
            {
                using (StreamReader sr = new StreamReader(stream, Encoding.UTF8))
                {
                    html = sr.ReadToEnd();
                }
            }
            string[] tempArray = html.Split(';');
            for (int i = 0; i < tempArray.Length; i++)
            {
                tempArray[i] = tempArray[i].Replace("\r\n", "");
            }

            string year = tempArray[1].Split('=')[1];
            string month = tempArray[2].Split('=')[1];
            string day = tempArray[3].Split('=')[1];
            string hour = tempArray[5].Split('=')[1];
            string minite = tempArray[6].Split('=')[1];
            string second = tempArray[7].Split('=')[1];
            dt = DateTime.Parse(year + "-" + month + "-" + day + " " + hour + ":" + minite + ":" + second); 

        }
        catch(WebException) 
        {
            return DateTime.Now; 
        }
        catch(Exception) 
        {
            return DateTime.Now; 
        }
        finally 
        {
            if (wrp != null)  wrp.Close();              
            if (wrt != null)  wrt.Abort(); 
                   
        }
        return dt;
    }

    /// <summary>
    /// 转换成DateTime
    /// </summary>
    /// <param name="_year"></param>
    /// <param name="_month"></param>
    /// <param name="_day"></param>
    /// <param name="_hour"></param>
    /// <param name="_minite"></param>
    /// <param name="_second"></param>
    /// <returns></returns>
    public DateTime ChangeToDateTime(int _year, int _month, int _day, int _hour, int _minite, int _second) 
    {
        DateTime time = DateTime.Now;
        time = DateTime.Parse(_year + "-" + _month + "-" + _day + " " + _hour + ":" + _minite + ":" + _second); 
        return time;
    }


    /// <summary>
    /// 判断时间是否在时间段内(-1早于时间段，0在范围内，1大于时间段)
    /// </summary>
    public int JugeMentTimeIsInside(DateTime _minTime, DateTime _maxTime,DateTime _jugeTime) 
    {
        int timeJuge = 0;
        bool greater = _jugeTime.CompareTo(_minTime) >=0;
        bool less = _jugeTime.CompareTo(_maxTime) <= 0;
        timeJuge = !greater ? -1 : timeJuge;
        timeJuge = !less ? 1 : timeJuge;
        return timeJuge;
    }


    #endregion






}
