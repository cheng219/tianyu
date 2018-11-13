/////////////////////////////////////////////////////////////////////////////////
////作者：吴江
////日期：2015/5/25
////用途：游戏平台基类
/////////////////////////////////////////////////////////////////////////////////


using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class GameStage : FSMBase
{
    public enum EventType
    {
        Run = fsm.Event.USER_FIELD + 1,
        Over,
        Reset,
        Restart,
        Resume,
        Pause,
        USER_FIELD,

    }

    protected SceneType sceneType = SceneType.NONE;
    /// <summary>
    /// 场景类型 by吴江
    /// </summary>
    public SceneType SceneType
    {
        get
        {
            return sceneType;
        }
    }

    [System.NonSerialized]
    public string sceneName = "Unknown";
    [System.NonSerialized]
    protected int sceneID = -1;
    /// <summary>
    /// 当前场景ID
    /// </summary>
    public int SceneID
    {
        get { return sceneID; }
        protected set
        {
            sceneID = value;
            SceneRef scene = ConfigMng.Instance.GetSceneRef(sceneID);
            if (scene != null)
            {
                sceneType = scene.sort;
            }
        }
    }


    public bool deinited { get { return deinited_; } }
    public string stageName = "Default Stage";
    protected bool deinited_ = false;
    protected fsm.State mainLoop = null;

    public SceneMng sceneMng = null;


    /// <summary>
    /// 当前场景的装备资源内存列表，更换场景时清理，优化内存，但会提高io消耗 by吴江 
    /// </summary>
    protected Dictionary<string, Object> equipmentCacheUrlList = new Dictionary<string, Object>();
    /// <summary>
    /// 记录一个装备资源的内存占用，在切换场景时便于清理 by吴江 
    /// </summary>
    /// <param name="_url"></param>
    public void CacheEquipmentURL(string _url, UnityEngine.Object _obj)
    {
        if (!equipmentCacheUrlList.ContainsKey(_url))
        {
            equipmentCacheUrlList.Add(_url, _obj);
        }
    }

    /// <summary>
    /// 卸载其他玩家的资源 by吴江 
    /// </summary>
    protected void UnLoadOPCEquipments()
    {
        //装备资源卸载 by吴江
        List<string> mainPlayerEquipments = new List<string>();
        MainPlayerInfo info = GameCenter.mainPlayerMng.MainPlayerInfo;
        if (info != null)
        {
            foreach (EquipmentInfo item in info.CurShowDictionary.Values)
            {
                if (item != null)
                {
                    mainPlayerEquipments.Add(item.ShortUrl);
                }
            }
            if (info.CurMountInfo != null)
            {
                mainPlayerEquipments.Add(info.CurMountInfo.AssetURL);
            }
        }
        foreach (string url in equipmentCacheUrlList.Keys)
        {
            if (!mainPlayerEquipments.Contains(url))
            {
                DestroyImmediate(equipmentCacheUrlList[url], true);
                AssetMng.instance.UnloadUrl(url);
            }
        }
        equipmentCacheUrlList.Clear();
    }

    public virtual void PreGuiUpdate()
    {
    }

    public virtual void GuiUpdate()
    {
    }


    protected override void InitStateMachine()
    {
        fsm.State start = new fsm.State("start", stateMachine);
        start.onEnter += EnterStartState;
        start.onExit += ExitStartState;
        start.onAction += UpdateStartState;

        mainLoop = new fsm.State("mainLoop", stateMachine);
        mainLoop.onEnter += EnterMainLoopState;
        mainLoop.onExit += ExitMainLoopState;
        mainLoop.onAction += UpdateMainLoopState;

        fsm.State pause = new fsm.State("pause", stateMachine);
        pause.onEnter += EnterPauseState;
        pause.onExit += ExitPauseState;
        pause.onAction += UpdatePauseState;

        fsm.State over = new fsm.State("over", stateMachine);
        over.onEnter += EnterOverState;
        over.onExit += ExitOverState;
        over.onAction += UpdateOverState;

        fsm.State restart = new fsm.State("restart", stateMachine);
        restart.onEnter += EnterRestartState;
        restart.onExit += ExitRestartState;
        restart.onAction += UpdateRestartState;

        start.Add<fsm.EventTransition>(mainLoop, (int)EventType.Run);
        start.Add<fsm.EventTransition>(over, (int)EventType.Over);

        mainLoop.Add<fsm.EventTransition>(over, (int)EventType.Over);
        mainLoop.Add<fsm.EventTransition>(restart, (int)EventType.Reset);
        mainLoop.Add<fsm.EventTransition>(pause, (int)EventType.Pause);

        pause.Add<fsm.EventTransition>(mainLoop, (int)EventType.Resume);
        pause.Add<fsm.EventTransition>(restart, (int)EventType.Reset);

        over.Add<fsm.EventTransition>(restart, (int)EventType.Reset);

        restart.Add<fsm.EventTransition>(start, (int)EventType.Restart);


    }

    public string myLastIP = string.Empty;




    //protected new void Awake()
    //{
    //    //base.Awake();

    //    //// if we are in Test Stage
    //    //if (Game.instance == false)
    //    //{
    //    //    Game.CreateDefault();
    //    //    Game.instance.Init();
    //    //}
    //    //Game.gameStage = this;
    //}

    void Start()
    {
        if (Init())
        {
            // now start the game
            if (stateMachine != null)
                stateMachine.Start();

        }
       // GameCenter.OnConnectStateChange += ConnectStateChange;
    }


    void OnDestroy()
    {
       // GameCenter.OnConnectStateChange -= ConnectStateChange;
        List<UID> needDelete = new List<UID>();
        using (var e = idToObject.GetEnumerator())
        {
            while (e.MoveNext())
            {
                if (e.Current.Value != null && e.Current.Value != GameCenter.curMainEntourage && e.Current.Value != GameCenter.curMainPlayer)
                {
                    needDelete.Add(e.Current.Key);
                }
            }
        }
        for (int i = 0; i < needDelete.Count; i++)
        {
            DestroyImmediate(idToObject[needDelete[i]], true);
        }
    }


    protected virtual void Update()
    {
        if (stateMachine != null)
            stateMachine.Update();
    }




    protected virtual bool Init()
    {
        return true;
    }


    protected virtual void Deinit()
    {
    }


    public override void Reset()
    {
        base.Reset();
    }


    public void Run()
    {
        stateMachine.Send((int)EventType.Run);
    }



    public void Pause()
    {
        stateMachine.Send((int)EventType.Pause);
    }




    public void Resume()
    {
        stateMachine.Send((int)EventType.Resume);
    }




    public void Restart()
    {
        stateMachine.Send((int)EventType.Reset);
    }



    public void ReadyToStart()
    {
        stateMachine.Send((int)EventType.Restart);
    }





    public void Over()
    {
        stateMachine.Send((int)EventType.Over);
    }

    protected fsm.State reconnectState;
    protected bool lastConnectState = false;
    /// <summary>
    /// 是否已经连接上过排队服务器
    /// </summary>
    protected bool hasConnectedQueueServer = false;
    protected float startConnectTime = 0;
    protected bool hasNetClosed = false;
    protected int failCount = 0;
    protected float closeTime = 0;
    protected float startConnectTimeForConetWnd = 0;
    protected virtual void EnterReconnectState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        LynSdkManager.Instance.ReportConnectionLose(SceneID.ToString(), "游戏过程中,Socket的连接装备变为false,可能是服务端主动断开或者其他网络异常!");
        hasConnectedQueueServer = false;
        lastConnectState = false;
        failCount = 0;
        hasNetClosed = false;
        if (GameCenter.sceneMng != null)
        {
            GameCenter.sceneMng.CleanAll();
        }
        GameCenter.instance.IsReConnecteding = true;
        NetMsgMng.ConectClose();
        startConnectTime = Time.time;
        startConnectTimeForConetWnd = Time.time;
        GameCenter.uIMng.SwitchToUI(GUIType.AUTO_RECONNECT);
        GameCenter.uIMng.ReleaseGUI(GUIType.PANELLOADING);

    }

    protected virtual void UpdateReconnectState(fsm.State _curState)
    {
        if (!hasNetClosed)
        {
            if (Time.time - startConnectTime >= 1.0f)
            {
                GameCenter.loginMng.C2S_ConectQueueServer(GameCenter.loginMng.Quaue_IP, GameCenter.loginMng.Quaue_port);
                GameCenter.instance.IsReConnecteding = true;
                if (GameCenter.mainPlayerMng != null && GameCenter.mainPlayerMng.MainPlayerInfo != null) GameCenter.mainPlayerMng.MainPlayerInfo.CleanBuff();
                hasNetClosed = true;
            }
        }
        else
        {
            if (lastConnectState != NetCenter.Connected)
            {
                lastConnectState = NetCenter.Connected;
                if (lastConnectState && GameCenter.loginMng.CurConnectServerType == LoginMng.ConnectServerType.Queue)//连接排队服务器成功,登陆排队服务器,取得角色列表,在loginmng中如果判断是重新连接中,那么自动选择上一次选择的角色登陆
                {
                    hasConnectedQueueServer = true;
                    GameCenter.instance.PingTime = 10;
                    GameCenter.loginMng.C2S_Login();
                }
                else if (!lastConnectState && GameCenter.loginMng.CurConnectServerType == LoginMng.ConnectServerType.Queue)//自动选择角色登陆后,会断开排队服务器,此时连接游戏服务器
                {
                    if (hasConnectedQueueServer && GameCenter.loginMng.IsActiveDisconnection)
                    {
                        GameCenter.loginMng.C2S_ConectGameServer();
                    }
                    else
                    {
                        GameCenter.loginMng.C2S_ConectQueueServer(GameCenter.loginMng.Quaue_IP, GameCenter.loginMng.Quaue_port);
                    }
                }
                else if (lastConnectState && GameCenter.loginMng.CurConnectServerType == LoginMng.ConnectServerType.Game)//如果游戏服务器连接成功,则进入游戏
                {
                    PlayGameStage ps = GameCenter.curGameStage as PlayGameStage;
                    if (ps != null)
                    {
                        GameCenter.instance.PingTime = 10;
                        //链接成功，申请进入游戏
                        GameCenter.loginMng.C2S_EnterGame();
                    }
                }
            }



            if (NetCenter.IsConnectedFaild && !GameCenter.loginMng.IsActiveDisconnection)
            {
                if (failCount == 0)
                {
                    failCount++;
                    GameCenter.loginMng.C2S_ConectQueueServer(GameCenter.loginMng.Quaue_IP, GameCenter.loginMng.Quaue_port);
                }
                else if (failCount >= 10)
                {
                    if (failCount == 5)
                    {
                        GameCenter.uIMng.SwitchToUI(GUIType.RECONNECT);
                        GameCenter.uIMng.ReleaseGUI(GUIType.AUTO_RECONNECT);
                    }
                    failCount++;
                }
                else
                {
                    if (closeTime == 0)
                    {
                        NetMsgMng.ConectClose();
                        closeTime = Time.time;
                    }
                    else if (Time.time - closeTime >= 2)
                    {
                        failCount++;
                        GameCenter.loginMng.C2S_ConectQueueServer(GameCenter.loginMng.Quaue_IP, GameCenter.loginMng.Quaue_port);
                        closeTime = Time.time;
                    }
                }
            }
        }
        //超过10秒弹出重新连接窗口 ljq
        if (Time.time - startConnectTimeForConetWnd >= 30)
        {
            if (GameCenter.uIMng.CurOpenType != GUIType.RECONNECT)
            {
                startConnectTimeForConetWnd = Time.time;
                GameCenter.uIMng.SwitchToUI(GUIType.RECONNECT);
                GameCenter.uIMng.ReleaseGUI(GUIType.AUTO_RECONNECT);
            }
        }
    }

    protected virtual void ExitReconnectState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        GameCenter.uIMng.ReleaseGUI(GUIType.AUTO_RECONNECT);
        GameCenter.uIMng.ReleaseGUI(GUIType.PANELLOADING);
    }

    protected virtual void EnterStartState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        //deinited_ = false;
        //Debug.LogInternal("[Stage] Start: " + stageName);
        //Run();
    }




    protected virtual void ExitStartState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {

    }

    protected virtual void UpdateStartState(fsm.State _curState)
    {
    }


    protected virtual void EnterMainLoopState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        //if (_from.name == "pause")
        //    Debug.LogInternal("[Stage] Resume: " + stageName);
    }


    protected virtual void ExitMainLoopState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
    }



    protected virtual void UpdateMainLoopState(fsm.State _curState)
    {
        //if (Game.isTestStage)
        //    exDebugHelper.ScreenPrint("Test Stage");

        //exDebugHelper.ScreenPrint("Time.time = " + Time.time.ToString("f2"));
        //exDebugHelper.ScreenLog("Time.frameCount = " + Time.frameCount);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Pause();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
    }




    protected virtual void EnterPauseState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        //Debug.LogInternal("[Stage] Paused: " + stageName);
    }



    protected virtual void ExitPauseState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
    }



    protected virtual void UpdatePauseState(fsm.State _curState)
    {
        // exDebugHelper.ScreenPrint( "Pause" );

        // if ( Input.GetKeyDown( KeyCode.Space ) ) {
        //     Resume();
        // }
        // else if ( Input.GetKeyDown( KeyCode.R ) ) {
        //     Restart ();
        // }
    }

    protected virtual void EnterOverState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        //Debug.LogInternal("[Stage] Over: " + stageName);
        //Deinit();
        //deinited_ = true;
    }



    protected virtual void ExitOverState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
    }




    protected virtual void UpdateOverState(fsm.State _curState)
    {
    }



    protected virtual void EnterRestartState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        //Debug.LogInternal("[Stage] Restart");
        //ReadyToStart();
    }


    protected virtual void ExitRestartState(fsm.State _from, fsm.State _to, fsm.Event _event)
    {
        Reset();
    }


    protected virtual void UpdateRestartState(fsm.State _curState)
    {
    }

    /// <summary>
    /// 根据一个对象设置另外一个对象的位置
    /// </summary>
    /// <param name="_goA">被设置的对象</param>
    /// <param name="_goB">用来参考的对象</param>
    public void PlaceGameObjectFromSceneAnima(GameObject _goA, GameObject _goB)
    {
        if (_goA == null || _goB == null) return;
        _goA.transform.position = _goB.transform.position;
        _goA.transform.localPosition = _goB.transform.localPosition;
        Actor actor = _goA.GetComponent<Actor>();
        if (actor != null)
        {
            actor.FaceToNoLerp(_goB.transform.eulerAngles.y);
        }
        else
        {
            _goA.transform.forward = _goB.transform.forward;
        }
    }

    public void PlaceGameObjectFromSceneAnima(GameObject _go, int _x, int _y, int _rotation = -1)
    {
        if (_go == null) return;
        _go.transform.localPosition = new Vector3(_x, _go.transform.localPosition.y, _y);
        Vector3 dir = Utils.Int2ToDir(_rotation);

        if (_rotation >= 0)
        {
            Actor actor = _go.GetComponent<Actor>();
            if (actor != null)
            {
                actor.FaceToNoLerp(_rotation);
            }
            else
            {
                _go.transform.forward = dir;
            }
        }
    }

    public void PlaceGameObjectFromServer(InteractiveObject _go, float _x, float _y, int _rotation, float _hight = 0)
    {
        if (_go == null || _go.gameObject == null) return;
        if (_hight == 0)
        {
            _go.transform.position = ActorMoveFSM.LineCast(new Vector3(_x, 0, _y),true);
        }
        else
        {
            _go.transform.position = new Vector3(_x, _hight, _y);
        }
        Vector3 dir = Utils.Int2ToDir(_rotation);

        Actor actor = _go.GetComponent<Actor>();
        if (actor != null)
        {
            actor.FaceToNoLerp(_rotation);
			actor.PositionChange();
        }
        else
        {
            _go.transform.forward = dir;
        }

        OnPositionChanged(_go);
    }


    public void PlaceGameObjectFromStaticRef(InteractiveObject _go, int _x, int _y, int _rotation,float _hight = 0)
    {
        if (_go == null || _go.gameObject == null) return;
        if (_hight == 0)
        {
            _go.transform.position = ActorMoveFSM.LineCast(new Vector3(_x, 0, _y),true);
        }
        else
        {
            _go.transform.position = new Vector3(_x, _hight, _y);
        }

        Actor actor = _go.GetComponent<Actor>();
        if (actor != null)
        {
            actor.FaceToNoLerp(_rotation);
        }
        else
        {
            _go.transform.localEulerAngles = new Vector3(0, _rotation, 0);
        }
        OnPositionChanged(_go);
    }



    #region 坐标格管理
    public class Sector
    {
        public int r = -1;
        public int c = -1;
        public List<InteractiveObject> objects = new List<InteractiveObject>();
    }



    [System.NonSerialized]
    public int startWidthPos = 0;
    [System.NonSerialized]
    public int startLengthPos = 0;
    [System.NonSerialized]
    public int width = -1;
    [System.NonSerialized]
    public int length = -1;
    [System.NonSerialized]
    public int scale = 1;

    protected Dictionary<UID, InteractiveObject> idToObject = new Dictionary<UID, InteractiveObject>();
    protected Sector[,] sectors;
    protected int maxRow = -1;
    protected int maxCol = -1;
    protected int sectorSize = 10;


    public virtual void InitSector(int _width, int _length, int _scale = 1, int _sectorSize = 10, int _startWidthPos = 0, int _startLengthPos = 0)
    {
        startWidthPos = _startWidthPos;
        startLengthPos = _startLengthPos;
        width = _width;
        length = _length;
        scale = _scale;
        sectorSize = _sectorSize;

        maxRow = Mathf.CeilToInt((float)length * scale / (float)sectorSize);
        maxCol = Mathf.CeilToInt((float)width * scale / (float)sectorSize);

        sectors = new Sector[maxRow, maxCol];
        for (int r = 0; r < maxRow; ++r)
        {
            for (int c = 0; c < maxCol; ++c)
            {
                Sector sector = new Sector();
                sector.r = r;
                sector.c = c;
                sectors[r, c] = sector;
            }
        }
    }

    public bool IsSectorInRange(Sector _check, Sector _center, int _range)
    {
        if (_check != null && _center != null)
        {
            return System.Math.Abs(_center.r - _check.r) <= _range &&
                System.Math.Abs(_center.c - _check.c) <= _range;
        }
        return false;
    }

    public Sector GetSector(int _r, int _c)
    {
        if (_r < 0 || _r > maxRow - 1 || _c < 0 || _c > maxCol - 1)
        {
            return null;
        }
        return sectors[_r, _c];
    }

    /// <summary>
    /// 根据位置得到坐标
    /// </summary>
    /// <param name="_pos"></param>
    /// <returns></returns>
    public Sector GetSectorByPosition(Vector3 _pos)
    {
        _pos = _pos.SetX(_pos.x - startWidthPos);
        _pos = _pos.SetZ(_pos.z - startLengthPos);
        int row = Mathf.FloorToInt(_pos.z / sectorSize);
        int col = Mathf.FloorToInt(_pos.x / sectorSize);

        if (row < 0 || row > maxRow - 1 || col < 0 || col > maxCol - 1)
        {
            Debug.LogError("Can't get sector at (" + row + "," + col + ") sectorSize: " + sectorSize + " length: " + length + " width: " + width + " maxRow: " + maxRow + " maxCol: " + maxCol + " scale: " + scale);
            return null;
        }

        return sectors[row, col];
    }

    /// <summary>
    /// 根据坐标得到坐标的中心位置
    /// </summary>
    /// <returns></returns>
    public Vector3 GetCenterPosBySector(Sector _sector)
    {
        float diff = sectorSize / 2f;
        return new Vector3(_sector.c * sectorSize + diff + startWidthPos, 0, _sector.r * sectorSize + diff + startLengthPos);
    }


    public void GetSectorRowCol(Sector _sector, out int _row, out int _col)
    {
        _row = _sector.r;
        _col = _sector.c;
    }



    public void DrawSector(float _height)
    {
        Gizmos.color = Color.blue;
        for (int i = 0; i <= maxRow; ++i)
        {
            Vector3 start = new Vector3(startWidthPos, _height,i * sectorSize);
            Vector3 end = new Vector3(startWidthPos + maxCol * sectorSize, _height,i * sectorSize);
            Gizmos.DrawLine(start, end);
        }
        for (int i = 0; i <= maxCol; ++i)
        {
            Vector3 start = new Vector3(i * sectorSize, _height, startLengthPos);
            Vector3 end = new Vector3(i * sectorSize, _height,startLengthPos + maxRow * sectorSize);
            Gizmos.DrawLine(start, end);
        }
    }


    public void DrawSectorObjectCountAt(Sector _sector, float _height, int _rowCount = 1, int _colCount = 1)
    {
        if (_sector != null)
        {
            int row = _sector.r;
            int col = _sector.c;
            Camera mainCamera = GameCenter.cameraMng.mainCamera;

            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.red;
            style.fontStyle = FontStyle.Bold;
            for (int r = row - _rowCount; r < row + _rowCount + 1; ++r)
            {
                for (int c = col - _colCount; c < col + _colCount + 1; ++c)
                {
                    Sector curSector = GetSector(r, c);
                    if (curSector != null)
                    {
                        Vector3 pos = new Vector3((c + 0.5f) * sectorSize, _height, (r + 0.5f) * sectorSize);
                        Vector2 screenPos = mainCamera.WorldToScreenPoint(pos);
                        screenPos = new Vector2(Mathf.Round(screenPos.x), Mathf.Round(screenPos.y));
                        exDebugHelper.ScreenPrint(screenPos, curSector.objects.Count.ToString(), style);
                    }
                }
            }
        }
    }


    public void OnPositionChanged(InteractiveObject _object)
    {
        if (!_object)
        {
            return;
        }
        Sector newSector = GetSectorByPosition(_object.transform.position);
        if (newSector != null && newSector != _object.curSector)
        {
            Sector oldSector = _object.curSector;
            if (oldSector != null)
                oldSector.objects.Remove(_object);

            newSector.objects.Add(_object);
            _object.curSector = newSector;
            if (_object.onSectorChanged != null) _object.onSectorChanged(oldSector, newSector);
        }
    }





    #endregion

    #region 对象管理
    public struct UID
    {
        public int typeID;
        public int instanceID;

        public UID(int _typeID, int _instanceID)
        {
            typeID = _typeID;
            instanceID = _instanceID;
        }

        public override bool Equals(object _obj)
        {
            if (_obj is UID)
            {
                UID c = (UID)_obj;
                return (typeID == c.typeID && instanceID == c.instanceID);
            }
            else
            {
                return false;
            }
        }

        public bool Equals(UID c)
        {
            return (typeID == c.typeID && instanceID == c.instanceID);
        }

        public override int GetHashCode() { return typeID ^ (instanceID << 5); }
    }


    
    protected virtual void Regist()
    {
    }

    public virtual void UnRegist()
    {
    }

    public void UnRegistAll()
    {
        foreach (InteractiveObject item in idToObject.Values)
        {
            if (item == GameCenter.curMainPlayer || item == GameCenter.curMainEntourage) continue;
            item.UnRegist();
        }
        if (GameCenter.curMainPlayer != null)
        {
            GameCenter.curMainPlayer.UnLockRef();
            if (GameCenter.curMainPlayer.CurTarget != null)
            {
                GameCenter.curMainPlayer.CurTarget = null;
            }
        }
        if (GameCenter.curMainEntourage != null)
        {
            GameCenter.curMainEntourage.UnLockRef();
        }
        if (GameCenter.abilityMng != null)
        {
            GameCenter.abilityMng.UnLockRef();
        }
        UnRegist();
    }

    public void AddObject(InteractiveObject _object)
    {
        UID uid = new UID((int)_object.typeID, _object.id);
        InteractiveObject obj;
        if (idToObject.TryGetValue(uid, out obj))
        {
            if (obj.isDummy)
            {
                RemoveActorNoCheck(obj);
                _object.CopyFromDummy(obj);
                GameObject.Destroy(obj);
            }
            else
            {
                Debug.LogError("Failed to add object " + _object.name + " id = " + _object.id);
                return;
            }
        }
        //
        _object.gameStage = this;
        idToObject.Add(uid, _object);
        OnPositionChanged(_object);
    }

    public void RemoveObject(InteractiveObject _object)
    {
        if (!RemoveActorNoCheck(_object))
        {
            Debug.Log("Failed to remove object " + _object.name + " id = " + _object.id);
        }
    }


    protected bool RemoveActorNoCheck(InteractiveObject _object)
    {
        UID uid = new UID((int)_object.typeID, _object.id);
        bool existing = idToObject.Remove(uid);
        if (_object.curSector != null)
        {
            _object.curSector.objects.Remove(_object);
            _object.curSector = null;
        }
        _object.gameStage = null;
        return existing;
    }


    public InteractiveObject GetObject(ObjectType _type, int _id)
    {
        UID uid = new UID((int)_type, _id);
        InteractiveObject obj;
        if (idToObject.TryGetValue(uid, out obj))
            return obj;
        return null;
    }


    public List<T> GetObjects<T>() where T : InteractiveObject
    {
        List<T> lst = new List<T>();
        foreach (InteractiveObject obj in idToObject.Values)
        {
            T casted = obj as T;
            if (casted != null)
            {
                lst.Add(casted);
            }
        }
        return lst;
    }

    public List<T> GetObjects<T>(ObjectType _type) where T : InteractiveObject
    {
        List<T> lst = new List<T>();
        foreach (InteractiveObject obj in idToObject.Values)
        {
            if (obj.typeID == _type)
            {
                lst.Add((T)obj);
            }
        }
        return lst;
    }

    #endregion

}





