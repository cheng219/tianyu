///////////////////////////////////////////////////////////////////////////////
//作者：吴江
//日期：2015/5/5
//用途：自定义管理游戏输出的类
///////////////////////////////////////////////////////////////////////////////




using UnityEngine;
using System.Collections;


public class GameSys : MonoBehaviour
{
    /// <summary>
    /// 游戏管理控制中心（即我们的GameCenter） by吴江
    /// </summary>
    public GameObject gameCenter;

    /// <summary>
    /// 静态唯一实例
    /// </summary>
    public static GameSys instance = null;

    public static bool inited { get { return instance.initialized; } }

    /// <summary>
    /// 报告错误（既会展示在屏幕，也会展示在信息面板） by吴江
    /// </summary>
    /// <param name="_text"></param>
    public static void LogError(string _text) { if (instance.isDebug) exDebugHelper.ScreenLog(_text, exDebugHelper.LogType.Error, new GUIStyle(instance.logStyleError)); }

    /// <summary>
    /// 报告警告（既会展示在屏幕，也会展示在信息面板） by吴江
    /// </summary>
    /// <param name="_text"></param>
    public static void LogWarning(string _text) { if (instance.isDebug) exDebugHelper.ScreenLog(_text, exDebugHelper.LogType.Warning, new GUIStyle(instance.logStyleWarning)); }

    /// <summary>
    /// 报告网络连接内容 by吴江
    /// </summary>
    /// <param name="_text"></param>
    /// <param name="_logToFile"></param>
    public static void LogInternal(string _text, bool _logToFile = true)
    {
        if (instance.isDebug == false)
            return;
        exDebugHelper.ScreenLog(_text,

                                  _logToFile ? exDebugHelper.LogType.Normal : exDebugHelper.LogType.None,
                                  new GUIStyle(instance.logStyleInteral));
    }

    /// <summary>
    /// 报告成功信息 by吴江
    /// </summary>
    /// <param name="_text"></param>
    /// <param name="_logToFile"></param>
    public static void LogSuccess(string _text, bool _logToFile = true)
    {
        if (instance.isDebug == false)
            return;
        exDebugHelper.ScreenLog(_text,

                                  _logToFile ? exDebugHelper.LogType.Normal : exDebugHelper.LogType.None,
                                  new GUIStyle(instance.logStyleSuccess));
    }

    /// <summary>
    /// 普通报告(只会展示在屏幕，不会记录到unity信息面板) by吴江
    /// </summary>
    /// <param name="_text"></param>
    /// <param name="_logToFile"></param>
    public static void Log(string _text, bool _logToFile = false)
    {
        if (instance.isDebug == false)
            return;
        exDebugHelper.ScreenLog(_text,
                                  _logToFile ? exDebugHelper.LogType.Normal : exDebugHelper.LogType.None,
                                  new GUIStyle(instance.logStyleNormal));
    }

    /// <summary>
    /// 指定颜色的普通报告  by吴江
    /// </summary>
    /// <param name="_text"></param>
    /// <param name="_color"></param>
    /// <param name="_logToFile"></param>
    public static void LogColor(string _text, Color _color, bool _logToFile = false)
    {
        if (instance.isDebug == false)
            return;

        GUIStyle style = new GUIStyle(instance.logStyleNormal);
        style.normal.textColor = _color;
        exDebugHelper.ScreenLog(_text,
                                  _logToFile ? exDebugHelper.LogType.Normal : exDebugHelper.LogType.None,
                                  style);
    }

    /// <summary>
    /// 报告服务端消息 by 吴江
    /// </summary>
    /// <param name="_text"></param>
    /// <param name="_logToFile"></param>
    public static void LogS2C(string _text, bool _logToFile = false)
    {
        if (instance.isDebug == false)
            return;
        exDebugHelper.ScreenLog(_text,
                                  _logToFile ? exDebugHelper.LogType.Normal : exDebugHelper.LogType.None,
                                  new GUIStyle(instance.logStyleS2C));
    }

    /// <summary>
    /// 报告客户端消息 by 吴江
    /// </summary>
    /// <param name="_text"></param>
    /// <param name="_logToFile"></param>
    public static void LogC2S(string _text, bool _logToFile = false)
    {
        if (instance.isDebug == false)
            return;

        exDebugHelper.ScreenLog(_text,
                                  _logToFile ? exDebugHelper.LogType.Normal : exDebugHelper.LogType.None,
                                  new GUIStyle(instance.logStyleC2S));
    }

    /// <summary>
    /// 仅输出在unity信息面板，不打印到屏幕 by吴江
    /// </summary>
    /// <param name="_text"></param>
    public static void LogNoPrint(string _text)
    {
        if (instance.isDebug == false)
            return;
        Debug.Log(_text);
    }


    public bool isDebug = true;
    public string settingFile = "default";
    public GUIStyle logStyleNormal = new GUIStyle();
    public GUIStyle logStyleInteral = new GUIStyle();
    public GUIStyle logStyleError = new GUIStyle();
    public GUIStyle logStyleWarning = new GUIStyle();
    public GUIStyle logStyleSuccess = new GUIStyle();
    public GUIStyle logStyleS2C = new GUIStyle();
    public GUIStyle logStyleC2S = new GUIStyle();

   // protected Settings settings_;
    protected bool initialized = false;

    void Awake()
    {
        instance = this;
        if (gameCenter != null)
        {
            GameCenter game = gameCenter.GetComponent<GameCenter>();
            if (game != null)
            {
                isDebug = game.isDevelopmentPattern;
            }
        }
    }


    public bool Init()
    {
        if (initialized == false)
        {
            GameSys.LogInternal("Initializing GameSys...");
            //string settingsPath = "System/Settings/" + settingFile;
            //settings_ = Resources.Load(settingsPath, typeof(Settings)) as Settings;
            //if (settings_ == null)
            //{
            //    GameSys.LogError("Settings file not found " + settingsPath);
            //    return false;
            //}
            //GameSys.LogSuccess("Setting file: " + settingsPath + " loaded!");

            //// init application with settings
            //Application.targetFrameRate = settings_.frameRate;
            //GameSys.LogInternal("Target Frame Rate: " + settings_.frameRate);

            GameSys.LogSuccess("GameSys initialized!");
            initialized = true;
        }
        return true;
    }

    void Update()
    {

    }

}