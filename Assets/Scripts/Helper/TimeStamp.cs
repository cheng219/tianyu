using UnityEngine;
using System.Collections;
using System;

public class TimeStamp {

    /// <summary>
    /// 时间数字 转成 字符串 add bu gc
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public static string TimeToString(int time)
    {
        int year = GetUinxTime(time).Year;
        int mouth = GetUinxTime(time).Month;
        int day = GetUinxTime(time).Day;
        int m = GetUinxTime(time).Minute;
        int h = GetUinxTime(time).Hour;
        return string.Format("{0:D4}/{1:D2}-{2:D2}", year, mouth, day) + " " + string.Format("{0:D2}:{1:D2}", h, m);
    }
    public static string TimeToChineseString(int time)
    {
        int year = GetUinxTime(time).Year;
        int mouth = GetUinxTime(time).Month;
        int day = GetUinxTime(time).Day;
        return string.Format("{0:D4}年{1:D2}月{2:D2}日", year, mouth, day);
    }
    /// <summary>
    /// 将一个整型时间差转换成小时分钟格式 by 唐源
    /// </summary>
    public static string TimeToHMString(int time)
    {
        int h = time / 60;
        int m = time % 60;
        //int m = GetUinxTime(time).Minute;
        //int h = GetUinxTime(time).Hour;
        return string.Format("{0:G}小时{1:D2}分钟", h, m);
    }
    /// <summary>
    /// 中式的时间显示年月日时分秒 by 唐源
    /// </summary>
    public static string TimeToChineseFormat(int time)
    {
        int year = GetUinxTime(time).Year;
        int mouth = GetUinxTime(time).Month;
        int day = GetUinxTime(time).Day;
        int m = GetUinxTime(time).Minute;
        int h = GetUinxTime(time).Hour;
        return string.Format("{0:D4}年{1:D2}月{2:D2}日{3:D2}时{4:D2}分", year, mouth, day,h,m);
    }
    /// <summary>
    /// 时间差 方法 add by gc.
    /// </summary>
    private static long mill = 60;
    private static long hour = mill * 60;
    private static long day = hour * 24;
    private static long mounth = day * 30;
    private static long year = day * 365;
    /// <summary>
    ///  时间差显示
    /// </summary>
    /// <param name="time">当前时间 time</param>
    /// <returns> 返回 xx前</returns>
    public static string DateStringFromNow(long time)//dt=new DateTime(long);
    {
        long timenow = TimeNowToInt();

        time = timenow - time;
        if (time < 0)
        {
            time = -time;//如果是未来
        }

        if (time < 1)
        {
            return "1秒";
        }
        if (time < mill)
        {
            return time + "秒";
        }
        if (time < hour)
        {
            return time / mill + "分钟";
        }
        if (time < day)
        {
            return time / hour + "小时";
        }
        if (time < mounth)
        {
            return time / day + "天";
        }
        if (time < year)
        {
            return time / mounth + "个月";
        }

        return time / year + "年";
    }

    /// <summary>
    /// 返回当前时间  add by gc.
    /// </summary>
    /// <returns></returns>
    public static int TimeNowToInt()
    {
        DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        int timenow = (int)(((DateTime.UtcNow - Jan1st1970).TotalMilliseconds) / 1000);

        return timenow;
    }

    public static DateTime GetUinxTime(long _time)
    {
        DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        long lTime = long.Parse(_time.ToString() + "0000");
        //Debug.Log("lTime=" + lTime);
        TimeSpan toNow = new TimeSpan(lTime);
        DateTime tNow = dtStart.Add(toNow);
        //Debug.Log("tNow=" + tNow);
        return tNow;
    }

    public static DateTime GetUinxTime(int _time)
    {
        DateTime datetime = new DateTime(1970, 1, 1).AddSeconds((double)_time);
        //北京时间 东八区
        datetime = datetime.AddHours((double)8);
        return datetime;
    }


}
