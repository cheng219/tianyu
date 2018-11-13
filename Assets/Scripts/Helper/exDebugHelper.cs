//================================
//作者：吴江
//日期：2015/7/16
//用途：屏幕打印帮助
//=================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;



#if !EX2D
[ExecuteInEditMode]
#endif
public class exDebugHelper : MonoBehaviour {

    protected static exDebugHelper instance = null;


    public static void ScreenPrint ( string _text ) {
        if ( instance.showScreenPrint_ ) {
            instance.txtPrint = instance.txtPrint + _text + "\n"; 
        }
    }


    public static void ScreenPrint ( Vector2 _pos, string _text, GUIStyle _style = null ) {
        if ( instance.showScreenDebugText ) {
            TextInfo info = new TextInfo( _pos, _text, _style ); 
            instance.debugTextPool.Add(info);
        }
    }


    public enum LogType {
        None,
        Normal,
        Warning,
        Error,
    }

    public static void ScreenLog ( string _text, LogType _logType = LogType.None, GUIStyle _style = null ) {
        LogInfo info = new LogInfo( _text, _style, 5.0f );
        instance.pendingLogs.Enqueue(info);
        if ( _logType != LogType.None ) {
            switch ( _logType ) {
            case LogType.Normal: Debug.Log(_text); break;
            case LogType.Warning: Debug.LogWarning(_text); break;
            case LogType.Error: Debug.LogError(_text); break;
            }
        }
    }


    public static void SetFPSColor ( Color _color ) {
        instance.fpsStyle.normal.textColor = _color;
    }


    public static float GetFPS () { return instance.fps; }


    public Vector2 offset = new Vector2 ( 10.0f, 10.0f );

    public GUIStyle printStyle = null;
    public GUIStyle fpsStyle = null;
    public GUIStyle logStyle = null;
    public GUIStyle timeScaleStyle = null;

    protected string txtPrint = "screen print: ";
    protected string txtFPS = "fps: ";

    // for screen debug text
    public class TextInfo {
        public Vector2 screenPos = Vector2.zero;
        public string text;
        public GUIStyle style = null;

        public TextInfo ( Vector2 _screenPos, string _text, GUIStyle _style ) {
            screenPos = _screenPos;
            text = _text;
            style = _style;  
        }
    }
    protected List<TextInfo> debugTextPool = new List<TextInfo>();

    // for screen log
    public class LogInfo {
        public string text;
        public GUIStyle style = null;

        public float ratio { get { return (timer >= lifetime - instance.logFadeOutDuration) ? (timer - (lifetime-instance.logFadeOutDuration))/instance.logFadeOutDuration : 0.0f; } }
        public bool canDelete { get { return timer >= lifetime; } }

        // internal
        float speed = 1.0f;
        float timer = 0.0f;
        float lifetime = 5.0f;

        public LogInfo ( string _text, GUIStyle _style, float _lifetime ) {
            text = _text;
            style = _style;  
            lifetime = _lifetime;
        }

        public void Dead () {
            float deadTime = lifetime - instance.logFadeOutDuration;
            if ( timer < deadTime - 1.0f) {
                timer = deadTime - 1.0f;
            }
        }

        public void Tick () {
            timer += Time.deltaTime * speed;
        }
    }
    float logFadeOutDuration = 0.3f;
    protected List<LogInfo> logs = new List<LogInfo>();
    protected Queue<LogInfo> pendingLogs = new Queue<LogInfo>();

    // fps
    [SerializeField] protected bool showFps_ = true;
    public bool showFps {
        get { return showFps_; }
        set {
            if ( showFps_ != value ) {
                showFps_ = value;
            }
        }
    }
    public TextAnchor fpsAnchor = TextAnchor.UpperLeft;

    // timescale
    [SerializeField] protected bool enableTimeScaleDebug_ = true;
    public bool enableTimeScaleDebug {
        get { return enableTimeScaleDebug_; }
        set {
            if ( enableTimeScaleDebug_ != value ) {
                enableTimeScaleDebug_ = value;
            }
        }
    }

    // screen print
    [SerializeField] protected bool showScreenPrint_ = true;
    public bool showScreenPrint {
        get { return showScreenPrint_; }
        set {
            if ( showScreenPrint_ != value ) {
                showScreenPrint_ = value;
            }
        }
    }

    // screen log
    [SerializeField] protected bool showScreenLog_ = true;
    public bool showScreenLog {
        get { return showScreenLog_; }
        set {
            if ( showScreenLog_ != value ) {
                showScreenLog_ = value;
            }
        }
    }
    public int logCount = 10;

    // screen debug text
    public bool showScreenDebugText = false;


    protected int frames = 0;
    protected float fps = 0.0f;
    protected float lastInterval = 0.0f;

   


    void Awake () {
        if ( instance == null )
            instance = this;

        txtPrint = "";
        txtFPS = "";

        if ( showScreenDebugText ) {
            debugTextPool.Clear();
        }
    }


    void Start () {
        InvokeRepeating("UpdateFPS", 0.0f, 1.0f );
    }


    void Update () {
        // count fps
        ++frames;

        //

        // update log
        UpdateLog ();

        // NOTE: the OnGUI call multiple times in one frame, so we just clear text here.
        StartCoroutine ( CleanDebugText() );
    }


    void OnGUI () {
        GUIContent content = null;
        Vector2 size = Vector2.zero;
        float curX = offset.x;
        float curY = offset.y;

        if (showFps && fpsStyle != null)
        {
            content = new GUIContent(txtFPS);
            size = fpsStyle.CalcSize(content);

            //
            switch ( fpsAnchor ) {
                //
            case TextAnchor.UpperLeft: 
                break;

            case TextAnchor.UpperCenter: 
                curX = curX + (Screen.width - size.x) * 0.5f; 
                break;

            case TextAnchor.UpperRight: 
                curX = Screen.width - size.x - curX; 
                break;

                //
            case TextAnchor.MiddleLeft: 
                curY = curY + (Screen.height - size.y) * 0.5f;
                break;

            case TextAnchor.MiddleCenter:
                curX = curX + (Screen.width - size.x) * 0.5f; 
                curY = curY + (Screen.height - size.y) * 0.5f;
                break;

            case TextAnchor.MiddleRight:
                curX = Screen.width - size.x - curX; 
                curY = curY + (Screen.height - size.y) * 0.5f;
                break;

                //
            case TextAnchor.LowerLeft:
                curY = Screen.height - size.y - curY;
                break;

            case TextAnchor.LowerCenter:
                curX = curX + (Screen.width - size.x) * 0.5f; 
                curY = Screen.height - size.y - curY;
                break;

            case TextAnchor.LowerRight:
                curX = Screen.width - size.x - curX; 
                curY = Screen.height - size.y - curY;
                break;
            }

            GUI.Label ( new Rect( curX, curY, size.x, size.y ), txtFPS, fpsStyle );

            curX = 10.0f;
            curY = 10.0f + size.y;
        }

        if ( enableTimeScaleDebug ) {
            string txtTimeScale = "TimeScale = " + Time.timeScale.ToString("f2");
            content = new GUIContent(txtTimeScale);
            //if (timeScaleStyle == null) Debug.Log("1");
            //if (content == null) Debug.Log("2");
            size = timeScaleStyle.CalcSize(content);
            GUI.Label ( new Rect( curX, curY, size.x, size.y ), txtTimeScale, timeScaleStyle );
            curY += size.y;
        }
		
		//输出内存打印 add by 沙新佳
		if ( showFps ) 
		{
            //保留内存/分配内存/未使用的保留内存
            string systemMemorySize = "内存使用 ≈ 总分配:" + (Profiler.GetTotalReservedMemory() / 1024 / 1024).ToString("f0") +
                "/使用中" + (Profiler.GetTotalAllocatedMemory() / 1024 / 1024).ToString("f0") +
                "/未使用" + (Profiler.GetTotalUnusedReservedMemory() / 1024 / 1024).ToString("f0") + "(M)";
            content = new GUIContent(systemMemorySize);
            size = timeScaleStyle.CalcSize(content);
            GUI.Label ( new Rect( curX, curY, size.x, size.y ), systemMemorySize, timeScaleStyle );
            curY += size.y;
		}
		//end
        if ( showScreenPrint ) {
            content = new GUIContent(txtPrint);
            size = printStyle.CalcSize(content);
            GUI.Label ( new Rect( curX, curY, size.x, size.y ), txtPrint, printStyle );
        }

        if ( showScreenLog ) {
            float y = Screen.height - 10.0f;

            for ( int i = logs.Count-1; i >= 0; --i ) {
                LogInfo info = logs[i];

                content = new GUIContent(info.text);
                GUIStyle style = (info.style == null) ? logStyle : info.style;
                size = style.CalcSize(content);

                //
                style.normal.textColor = new Color ( style.normal.textColor.r, 
                                                     style.normal.textColor.g, 
                                                     style.normal.textColor.b, 
                                                     1.0f - info.ratio );

                y -= size.y;
                GUI.Label ( new Rect( Screen.width - 10.0f - size.x, y, size.x, size.y ), info.text, style );
            }
        }

        if ( showScreenDebugText ) {
            for ( int i = 0; i < debugTextPool.Count; ++i ) {
                TextInfo info = debugTextPool[i];
                content = new GUIContent(info.text);
                GUIStyle style = (info.style == null) ? GUI.skin.label : info.style;
                size = style.CalcSize(content);

                Vector2 pos = new Vector2( info.screenPos.x, Screen.height - info.screenPos.y ) - size * 0.5f; 
                GUI.Label ( new Rect( pos.x, pos.y, size.x, size.y ), info.text, style );
            }
        }
    }


    void UpdateFPS () {
        float timeNow = Time.realtimeSinceStartup;
        fps = frames / (timeNow - lastInterval);
        frames = 0;
        lastInterval = timeNow;
        txtFPS = "fps: " + fps.ToString("f2");
    }


    public void ResetCamera ( Camera _camera ) {
    }


    void UpdateTimeScale () {
        if ( enableTimeScaleDebug ) {
            if ( Input.GetKey(KeyCode.Minus) ) {
                Time.timeScale = Mathf.Max( Time.timeScale - 0.01f, 0.0f );
            }
            else if ( Input.GetKey(KeyCode.Equals) ) {
                Time.timeScale = Mathf.Min( Time.timeScale + 0.01f, 10.0f );
            }

            if ( Input.GetKey(KeyCode.Alpha0 ) ) {
                Time.timeScale = 0.0f;
            }
            else if ( Input.GetKey(KeyCode.Alpha9 ) ) {
                Time.timeScale = 1.0f;
            }
        }
    }


    IEnumerator CleanDebugText () {
        yield return new WaitForEndOfFrame();

        txtPrint = "";

        if ( showScreenDebugText ) {
            debugTextPool.Clear();
        }
    }


    void UpdateLog () {
        for ( int i = logs.Count-1; i >= 0; --i ) {
            LogInfo info = logs[i];
            info.Tick();
            if ( info.canDelete ) {
                logs.RemoveAt(i);
            }
        }

        // DISABLE { 
        // if ( logTimer < logInterval ) {
        //     logTimer += Time.deltaTime;
        // }
        // else {
        //     if ( pendingLogs.Count > 0 ) {
        //         logTimer = 0.0f;
        //         logs.Add(pendingLogs.Dequeue());

        //         if ( instance.logs.Count > instance.logCount ) {
        //             for ( int i = 0; i < instance.logs.Count - instance.logCount; ++i ) {
        //                 instance.logs[i].Dead();
        //             }
        //         }
        //     }
        // }
        // } DISABLE end 

        if ( pendingLogs.Count > 0 ) {
            int count = Mathf.CeilToInt(pendingLogs.Count/2);

            do {
                logs.Add(pendingLogs.Dequeue());
                --count;

                if ( instance.logs.Count > instance.logCount ) {
                    for ( int i = 0; i < instance.logs.Count - instance.logCount; ++i ) {
                        instance.logs[i].Dead();
                    }
                }
            } while ( count > 0 );
        }
    }
}
