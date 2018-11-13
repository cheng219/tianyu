//====================
//作者：黄洪兴
//日期：2016/5/31
//用途：大地图
//====================


using UnityEngine;
using AnimationOrTween;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class LargeMapWnd : GUIBase
{
    #region 数据
    public UIPanel mapView;
    public GameObject curMapGoObj;
    public UILabel mapName;
    public GameObject wordMapObjA;
    public GameObject wordMapObjB;
    public GameObject wordMapObjC;
    public GameObject wordMapObjD;
    public GameObject wordMapObjE;
    public GameObject wordMapGoObj;
    public UIToggle[] chooseMapToggles = new UIToggle[2];
    public GameObject curMapObj;
    public GameObject wordMapObj;
    public UIScrollView ScrollView;
    public GameObject closeObj;
    public GameObject mapMoveItemInstance;
    public GameObject flyPointInstance;
    public GameObject mainPlayerPointInstance;
    public GameObject monsterInstance;
    public Transform mainPlayerPoint;
    public GameObject npcNoTaskPointInstance;
    public GameObject npcCompeteTaskPointInstance;
    public GameObject npcUntakeTaskPointInstance;
    public GameObject npcUnCompleteTaskPointInstance;
    public GameObject mapCtrl;
    public UILabel mainplayerPos;
    public UIToggle[] toggles = new UIToggle[3];

    public GameObject targetObj;
    protected MapGo go = null;

    protected float posXRate;
    protected float posYRate;
    protected Transform mainplayer;



    public Transform clickPoint;

    protected FDictionary mobPointDic = new FDictionary();
    protected FDictionary npcPointDic = new FDictionary();
    protected FDictionary flyPointDic = new FDictionary();




    private float mapViewPosX;
    private float mapViewPosY;
    private float maxMapViewX;
    private float maxMapViewY;
    bool initedMap=false;

    bool willCloseWnd = false;
    /// <summary>
    /// 大地图地图画面
    /// </summary>
    public UITexture mapTexture;
    /// <summary>
    /// 世界地图按钮
    /// </summary>
    public GameObject worldMapBtn;
    /// <summary>
    /// 换线按钮
    /// </summary>
    public GameObject changeLineBtn;
    /// <summary>
    /// 当前坐标
    /// </summary>
    public UILabel curPosition;
    /// <summary>
    /// 右侧功能列表的父物体
    /// </summary>
    public GameObject grid;

    protected StringBuilder posTexter = null;

    protected FDictionary objectInfoDic = new FDictionary();
    protected List<MapInterActiveObjectInfo> objectInfoList = new List<MapInterActiveObjectInfo>(); 
    int MapType
    {
        get
        {
            return chooseMapToggles[0].value ? 1 : 2;
        }
    }
    bool showNpc
    {
        get
        {
            if (toggles[0] == null) return false;
            else return toggles[0].value;
        }
    }
    bool showMonster
    {
        get
        {
            if (toggles[1] == null) return false;
            else return toggles[1].value;
        }
    }
    bool showFlyPoint
    {
        get
        {
            if (toggles[2] == null) return false;
            else return toggles[2].value;
        }
    }
    int curScene;
    #endregion
    /// <summary>
    /// 初始化窗口
    /// </summary>
    void Awake()
    {
        
        mutualExclusion = true;
        layer = GUIZLayer.TOPWINDOW;


        if (npcCompeteTaskPointInstance!=null) npcCompeteTaskPointInstance.SetActive(false);
        if (npcUntakeTaskPointInstance != null) npcUntakeTaskPointInstance.SetActive(false);
        if (npcUnCompleteTaskPointInstance != null) npcUnCompleteTaskPointInstance.SetActive(false);
  
        GameObject obj = Instantiate(mainPlayerPointInstance) as GameObject;
        MapPlayerHead head = obj.GetComponent<MapPlayerHead>();
        if (head != null)
            head.Refresh();
        obj.SetActive(true);
        mainPlayerPoint = obj.transform;
        if (mapCtrl!=null)
        mainPlayerPoint.parent = mapCtrl.transform;
        mainPlayerPoint.localScale = Vector3.one;
        mainplayer = GameCenter.curMainPlayer.transform;


        curScene = GameCenter.curGameStage.SceneID;
        SceneRef sceneRef = ConfigMng.Instance.GetSceneRef(GameCenter.curGameStage.SceneID);
        if (mapTexture != null)
        {
            mapTexture.SetDimensions((int)(1000 * sceneRef.sceneWidth / sceneRef.sceneLength), (int)(1000 * sceneRef.sceneLength / sceneRef.sceneWidth));
            mapTexture.transform.parent = null;
            if (mapCtrl != null)
            {
                mapCtrl.transform.localPosition = new Vector3(-mapTexture.localSize.x / 2, -mapTexture.localSize.y / 2, 0);
                mapTexture.transform.parent = mapCtrl.transform;
            }
        }
        if (sceneRef != null)
        {
            posXRate = mapTexture.localSize.x / sceneRef.sceneWidth;
            posYRate = mapTexture.localSize.y / sceneRef.sceneLength;
        } 
        objectInfoDic.Clear();
        FDictionary npcInfo = GameCenter.sceneMng.NPCInfoDictionary;
        foreach (NPCInfo item in npcInfo.Values)
        {
            objectInfoDic[item.ServerInstanceID] = new MapInterActiveObjectInfo(item);
            objectInfoList.Add(objectInfoDic[item.ServerInstanceID] as MapInterActiveObjectInfo);
        }
        List<FlyPointRef> flyList = ConfigMng.Instance.GetFlyPointRefByScene(GameCenter.curGameStage.SceneID);
        if (flyList != null)
        {
            for (int i = 0; i < flyList.Count; i++)
            {
                objectInfoDic[flyList[i].id] = new MapInterActiveObjectInfo(flyList[i]);
                objectInfoList.Add(objectInfoDic[flyList[i].id] as MapInterActiveObjectInfo);
            }
        }
        List<MonsterDistributionRef> monsterList = ConfigMng.Instance.GetMonsterDistributionRefByScene(GameCenter.curGameStage.SceneID);
        if (monsterList != null)
        {
            for (int i = 0; i < monsterList.Count; i++)
            {
                objectInfoDic[monsterList[i].id] = new MapInterActiveObjectInfo(monsterList[i]);
                objectInfoList.Add(objectInfoDic[monsterList[i].id] as MapInterActiveObjectInfo);
            }
        }

        if (mapView != null)
        {
            mapViewPosX = mapView.transform.localPosition.x;
            mapViewPosY = mapView.transform.localPosition.y;
            maxMapViewX =(mapTexture.localSize.x - mapView.GetViewSize().x) / 2;
            maxMapViewY = (mapTexture.localSize.y - mapView.GetViewSize().y) / 2;
        }
    }

    void Update()
    {
        if (willCloseWnd)
        {
            willCloseWnd = false;
            GameCenter.uIMng.SwitchToUI(GUIType.NONE);
        }
    }



    void FixedUpdate()
    {
        //if (mainPlayerPoint != null && Time.frameCount % 5 == 0)
        //{
        //    mainPlayerPoint.localEulerAngles = new Vector3(0, 0, 180.0f - mainplayer.localEulerAngles.y);
        //}
    }
    #region 注册
    protected override void OnOpen()
    {
        base.OnOpen(); 
        Refresh();
        RefreshMap();
        RefreshPos(null, null);
        RefreshShow();
        if (go == null) go = targetObj.GetComponent<MapGo>();
        if (targetObj.activeSelf && go != null)
        {
            go.Init(GameCenter.curMainPlayer.CurTargetPoint.targetPoint, GameCenter.curMainPlayer.CurTargetPoint.scenID);
        } 
    }

    protected override void OnClose()
    {

        base.OnClose();
		GameCenter.cameraMng.ClearMapTexture();
    }

    protected override void HandEvent(bool _bind)
    {
        base.HandEvent(_bind);
        if (_bind)
        {
            for (int i = 0; i < toggles.Length; i++)
            {
                //EventDelegate.Add(toggles[i].onChange, RefreshShow);
                if (toggles[i] != null) UIEventListener.Get(toggles[i].gameObject).onClick = ClickToggleEvent;
            }
            for (int i = 0; i < chooseMapToggles.Length; i++)
            {
                EventDelegate.Add(chooseMapToggles[i].onChange, RefreshMapType);
            }
            GameCenter.curMainPlayer.onSectorChanged += RefreshPos;
            SceneMng.OnDelInterObj += OnDeleteObj;
            if (closeObj != null) UIEventListener.Get(closeObj).onClick += OnClose;
            if (mapTexture != null) UIEventListener.Get(mapTexture.gameObject).onClick += OnClickMapTexture;
            if (worldMapBtn != null) UIEventListener.Get(worldMapBtn.gameObject).onClick += OnClickOpenWorldMap;
            if (changeLineBtn != null) UIEventListener.Get(changeLineBtn.gameObject).onClick += OnClickOpenChangeLine;
            if (wordMapObjA != null) UIEventListener.Get(wordMapObjA).onClick += OnClickWordMap;
            if (wordMapObjB != null) UIEventListener.Get(wordMapObjB).onClick += OnClickWordMap;
            if (wordMapObjC != null) UIEventListener.Get(wordMapObjC).onClick += OnClickWordMap;
            if (wordMapObjD != null) UIEventListener.Get(wordMapObjD).onClick += OnClickWordMap;
            if (wordMapObjE != null) UIEventListener.Get(wordMapObjE).onClick += OnClickWordMap;
            GameCenter.taskMng.TryTrace += RefreshTargetPoint;
            GameCenter.curMainPlayer.CannotMoveTo += HideTargetPoin;
            GameCenter.curMainPlayer.OnMoveStart += ShowTargetPoin;
        }
        else
        {
            for (int i = 0; i < chooseMapToggles.Length; i++)
            {
                EventDelegate.Remove(chooseMapToggles[i].onChange, RefreshMapType);
            }
            GameCenter.curMainPlayer.onSectorChanged -= RefreshPos;
            SceneMng.OnDelInterObj -= OnDeleteObj;
            if (closeObj != null) UIEventListener.Get(closeObj).onClick -= OnClose;
            if (mapTexture != null) UIEventListener.Get(mapTexture.gameObject).onClick -= OnClickMapTexture;
            if (worldMapBtn != null) UIEventListener.Get(worldMapBtn.gameObject).onClick -= OnClickOpenWorldMap;
            if (changeLineBtn != null) UIEventListener.Get(changeLineBtn.gameObject).onClick -= OnClickOpenChangeLine;
            if (wordMapObjA != null) UIEventListener.Get(wordMapObjA).onClick -= OnClickWordMap;
            if (wordMapObjB != null) UIEventListener.Get(wordMapObjB).onClick -= OnClickWordMap;
            if (wordMapObjC != null) UIEventListener.Get(wordMapObjC).onClick -= OnClickWordMap;
            if (wordMapObjD != null) UIEventListener.Get(wordMapObjD).onClick -= OnClickWordMap;
            if (wordMapObjE != null) UIEventListener.Get(wordMapObjE).onClick -= OnClickWordMap;
            GameCenter.taskMng.TryTrace -= RefreshTargetPoint;
            GameCenter.curMainPlayer.CannotMoveTo -= HideTargetPoin;
            GameCenter.curMainPlayer.OnMoveStart -= ShowTargetPoin;
        }
    }
    #endregion
    #region 控件事件

    protected UIToggle lastChangeToggle = null;
    protected void ClickToggleEvent(GameObject go)
    {
        UIToggle toggle = go.GetComponent<UIToggle>();
        if (toggle != lastChangeToggle)
        {
            RefreshShow();
        }
        if (toggle != null && toggle.value) lastChangeToggle = toggle;
    }



    void OnClose(GameObject _go)
    {
        willCloseWnd = true;
    }


    void OnClickMapTexture(GameObject _go)
    {
        TouchPoint();
        MousePoint();
    }


    protected void TouchPoint()
    {
        if (Input.touchCount == 0)
            return;
        if (Input.touchCount > 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved) //这个手势是缩放手势
            {
                return;
            }
        }
        for (int i = 0; i < Input.touchCount; ++i)
        {
            Touch input = Input.GetTouch(i);
            bool unpressed = (input.phase == TouchPhase.Canceled) || (input.phase == TouchPhase.Ended);
            if (!unpressed)//只处理触摸弹起的事件
                continue;
            Vector3 pos = new Vector3(input.position.x, input.position.y, 0);
            if (!UICamera.Raycast(pos))//检测是否按到了UI控件
            {
                continue;
            }


            Ray _ray = GameCenter.cameraMng.uiCamera.ScreenPointToRay(pos);

            RaycastHit _hit;
            if (Physics.Raycast(_ray, out _hit, Mathf.Infinity, LayerMng.uiMask))
            {
                clickPoint.transform.position = _hit.point;
                Vector3 diffPoint = clickPoint.localPosition;

                GameCenter.curMainPlayer.CancelCommands();
                GameCenter.curMainPlayer.GoTraceTarget(GameCenter.mainPlayerMng.MainPlayerInfo.SceneID, (int)(diffPoint.x / posXRate), (int)(diffPoint.y / posYRate));
                //zsy
                InitFlyPoint(new Vector3((int)(diffPoint.x / posXRate), (int)(diffPoint.y / posYRate)));
                //Command_MoveTo moveto = new Command_MoveTo();
                //moveto.destPos = ActorMoveFSM.LineCast(new Vector3(diffPoint.x / posXRate, 0, diffPoint.y / posYRate), true);
                //moveto.maxDistance = 0f;
                //GameCenter.curMainPlayer.commandMng.PushCommand(moveto);
                
                //GameCenter.taskMng.CurTargetPoint = moveto.destPos;//显示出寻路中。。
                //GameCenter.uIMng.GenGUI(GUIType.TASK_FINDING, true);
            }
        }
    }
    void InitFlyPoint(Vector3 _point)
    {
        if(go == null)go = targetObj.GetComponent<MapGo>();
        if (go != null)
        {
            go.Init(_point);
        }
    }
    protected void MousePoint()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray _ray = GameCenter.cameraMng.uiCamera.ScreenPointToRay(Input.mousePosition);

            RaycastHit _hit;
            if (Physics.Raycast(_ray, out _hit, Mathf.Infinity, LayerMng.uiMask))
            {
                clickPoint.transform.position = _hit.point;
                Vector3 diffPoint = clickPoint.localPosition;

                GameCenter.curMainPlayer.CancelCommands();
                GameCenter.curMainPlayer.GoTraceTarget(GameCenter.mainPlayerMng.MainPlayerInfo.SceneID, (int)(diffPoint.x / posXRate), (int)(diffPoint.y / posYRate));
                //zsy
                InitFlyPoint(new Vector3((int)(diffPoint.x / posXRate), (int)(diffPoint.y / posYRate)));

                //Command_MoveTo moveto = new Command_MoveTo();
                //moveto.destPos = ActorMoveFSM.LineCast(new Vector3(diffPoint.x / posXRate, 0, diffPoint.y / posYRate), true);
                //moveto.maxDistance = 0f;
                //GameCenter.curMainPlayer.commandMng.PushCommand(moveto);

                //GameCenter.taskMng.CurTargetPoint = moveto.destPos;//显示出寻路中。。
                //GameCenter.uIMng.GenGUI(GUIType.TASK_FINDING, true);
            }
        }
    }

    /// <summary>
    /// 点击打开世界地图
    /// </summary>
    /// <param name="_obj"></param>
    void OnClickOpenWorldMap(GameObject _obj)
    {
        SwitchToSubWnd(SubGUIType.WORLDMAP);
    }
    /// <summary>
    /// 点击打开换线
    /// </summary>
    /// <param name="_obj"></param>
    void OnClickOpenChangeLine(GameObject _obj)
    {
        SwitchToSubWnd(SubGUIType.CHANGELINE);
    }

    void OnClickWordMap(GameObject _obj)
    {
        if (wordMapGoObj != null)
        {
            WordMapItem item = _obj.transform.GetComponent<WordMapItem>();
            if (item != null)
            {
                MapGo mapGo = wordMapGoObj.GetComponent<MapGo>();
                if (mapGo != null)
                {
                    //FlyPointRef Ref = ConfigMng.Instance.GetFlyPointRef(item.ID);
                    FlyPointRef Ref = ConfigMng.Instance.GetFlyPointRef(GameCenter.mainPlayerMng.MainPlayerInfo.SceneID, item.sceneID);
                    if (Ref == null) Ref = ConfigMng.Instance.GetFlyPointRef(item.ID);
                    mapGo.Init(Ref.targetVector, item.sceneID);
                }
            }
            wordMapGoObj.SetActive(true);
            wordMapGoObj.transform.localPosition = _obj.transform.localPosition;
        }
    }
    void OnClickMapItem(GameObject _obj)
    {
        if (targetObj != null)
        { 
            MapItem item = _obj.transform.parent.transform.GetComponent<MapItem>();
            //zsy   将curMapGoObj改为targetObj
            GameCenter.curMainPlayer.CancelCommands();
            GameCenter.curMainPlayer.GoTraceTarget(GameCenter.mainPlayerMng.MainPlayerInfo.SceneID, (int)item.point.x, (int)item.point.y);

            if (item != null)
            {
                if (go == null) go = targetObj.GetComponent<MapGo>();
                if (go != null)
                {
                   go.Init(item.point);
                }
            }
            targetObj.SetActive(true);
            targetObj.transform.position = _obj.transform.position;
        }
    }

    void OnClickItemName(GameObject go)//zsy
    { 
        MapItem item = (MapItem)UIEventListener.Get(go).parameter;
        if (item != null)
        { 
            InitFlyPoint(item.point);
        }
    }
    #endregion

    #region 刷新

    void RefreshMapType()
    {

        if (MapType == 1)
        {
            if (mapName != null) mapName.text = GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneRef.name;
            if (curMapObj!=null) curMapObj.SetActive(true);
            if (wordMapObj!=null) wordMapObj.SetActive(false);
        }
        else if (MapType == 2)
        {
            if (mapName != null) mapName.text = "地图";
            if (curMapObj != null) curMapObj.SetActive(false);
            if (wordMapObj != null) wordMapObj.SetActive(true);
            if (wordMapGoObj != null) wordMapGoObj.SetActive(false);
        }
        else
        {
            curMapObj.SetActive(false);
            wordMapObj.SetActive(false);


        }

    }



    void RefreshShow()
    {

//        Debug.Log("刷新地图物品");
        if (curMapGoObj != null)
            curMapGoObj.SetActive(false);
        foreach (var item in npcPointDic.Values)
        {
            GameObject Obj = item as GameObject;
            Destroy(Obj);
        }
        foreach (var item in flyPointDic.Values)
        {
            GameObject Obj = item as GameObject;
            Destroy(Obj);
        }
        foreach (var item in mobPointDic.Values)
        {
            GameObject Obj = item as GameObject;
            Destroy(Obj);
        }
        if (grid != null)
        {
            grid.transform.DestroyChildren();
        }
        if (showNpc)
        {
            RefreshNpc();
            if (ScrollView != null)
                ScrollView.ResetPosition();
            Vector3 V = Vector3.zero;
            foreach (var item in npcPointDic.Values)
            {
                GameObject Obj = Instantiate(mapMoveItemInstance) as GameObject;
                Obj.transform.parent = grid.transform;
                Obj.transform.localPosition = V;
                Obj.transform.localScale = new Vector3(1, 1, 1);
                Obj.SetActive(true);
                MapMoveItem mapItem = Obj.GetComponent<MapMoveItem>();
                mapItem.enabled = true;
                GameObject O=item as GameObject;
                if (mapItem != null&&O!=null)
                {
                    mapItem.Init(O.GetComponent<MapItem>().Name, O.GetComponent<MapItem>().point, O.GetComponent<MapItem>().sceneid);
                }
                if (Obj != null)//zsy
                {
                    UIEventListener.Get(mapItem.GoObj).onClick -= OnClickItemName;
                    UIEventListener.Get(mapItem.GoObj).onClick += OnClickItemName;
                    UIEventListener.Get(mapItem.GoObj).parameter = O.GetComponent<MapItem>();
                }
                V = new Vector3(V.x, V.y - 60, V.z);
            }
        }
        else if (showFlyPoint)
        {
            RefreshFlyPoint();
            if (ScrollView != null)
                ScrollView.ResetPosition();
            Vector3 V = Vector3.zero;
            foreach (var item in flyPointDic.Values)
            {
                GameObject Obj = Instantiate(mapMoveItemInstance) as GameObject;
                Obj.transform.parent = grid.transform;
                Obj.transform.localPosition = V;
                Obj.transform.localScale = new Vector3(1, 1, 1);
                Obj.SetActive(true);
                MapMoveItem mapItem = Obj.GetComponent<MapMoveItem>();
                mapItem.enabled = true;
                GameObject O = item as GameObject;
                if (mapItem != null && O != null)
                {
                    mapItem.Init(O.GetComponent<MapItem>().Name, O.GetComponent<MapItem>().point, O.GetComponent<MapItem>().sceneid);
                }
                if (Obj != null)
                {
                    UIEventListener.Get(mapItem.GoObj).onClick -= OnClickItemName;
                    UIEventListener.Get(mapItem.GoObj).onClick += OnClickItemName;
                    UIEventListener.Get(mapItem.GoObj).parameter = O.GetComponent<MapItem>();
                }
                V = new Vector3(V.x, V.y - 60, V.z);
            }
        }
        else if (showMonster)
        {
            RefreshFlyMonster();
            if (ScrollView != null)
                ScrollView.ResetPosition();
            Vector3 V = Vector3.zero;
            foreach (var item in mobPointDic.Values)
            {
                GameObject Obj = Instantiate(mapMoveItemInstance) as GameObject;
                Obj.transform.parent = grid.transform;
                Obj.transform.localPosition = V;
                Obj.transform.localScale = new Vector3(1, 1, 1);
                Obj.SetActive(true);
                MapMoveItem mapItem = Obj.GetComponent<MapMoveItem>();
                mapItem.enabled = true;
                GameObject O = item as GameObject;
                if (mapItem != null && O != null)
                {
                    mapItem.Init(O.GetComponent<MapItem>().Name, O.GetComponent<MapItem>().point, O.GetComponent<MapItem>().sceneid);
                }
                if (Obj != null)
                {
                    UIEventListener.Get(mapItem.GoObj).onClick -= OnClickItemName;
                    UIEventListener.Get(mapItem.GoObj).onClick += OnClickItemName;
                    UIEventListener.Get(mapItem.GoObj).parameter = O.GetComponent<MapItem>();
                }
                V = new Vector3(V.x, V.y - 60, V.z);
            }
        }

        if (GameCenter.curMainPlayer.CurTargetPoint != null)
        {
            RefreshTargetPoint(GameCenter.curMainPlayer.CurTargetPoint.scenID, GameCenter.curMainPlayer.CurTargetPoint.targetPoint);
        }
    }
    protected void RefreshPos(GameStage.Sector _old, GameStage.Sector _new)
    {
        GameStage.Sector curSector = GameCenter.curMainPlayer.curSector;
        if (mainplayerPos != null)
        {
            posTexter = new StringBuilder(32);
            posTexter.Append(curSector.c.ToString());
            posTexter.Append(",");
            posTexter.Append(curSector.r.ToString());
            mainplayerPos.text = posTexter.ToString();
        }
        if (mainPlayerPoint != null)
        {
            mainPlayerPoint.localPosition = new Vector3(posXRate * curSector.c , posYRate * curSector.r , 0);
            initMap();

        }

    }


    protected void RefreshNpc()
    {
        List<NPCAIRef> NpcList= ConfigMng.Instance.GetNPCAIRefByScene(curScene);
        //List<NPC> npcs = GameCenter.curGameStage.GetNPCs();
        for (int i = 0; i < NpcList.Count; i++)
        {
            GameObject myPoint = null;
            myPoint = Instantiate(npcNoTaskPointInstance) as GameObject;
            myPoint.SetActive(true);
            myPoint.transform.parent = mapCtrl.transform;
            myPoint.transform.localScale = Vector3.one;
            myPoint.transform.localPosition = new Vector3(NpcList[i].sceneX * posXRate , NpcList[i].sceneY * posYRate , 0);
            npcPointDic.Add(NpcList[i].npcId, myPoint);
             MapItem item= myPoint.GetComponent<MapItem>();
             if (item != null)
             {
                 item.Init(ConfigMng.Instance.GetNPCTypeRef(NpcList[i].npcId).name, new Vector3(NpcList[i].pointX, NpcList[i].pointY), NpcList[i].scene);
                 if (item.Obj != null)
                 {
                     UIEventListener.Get(item.Obj).onClick -= OnClickMapItem;
                     UIEventListener.Get(item.Obj).onClick += OnClickMapItem;
                 }
             }

        }
    }

    public void RefreshFlyPoint()
    {
        List<FlyPointRef> flyPointList = ConfigMng.Instance.GetFlyPointRefByScene(curScene);
       // List<FlyPoint> flys = GameCenter.curGameStage.GetFlypoints();
        for (int i = 0; i < flyPointList.Count; i++)
        {
            GameObject myPoint = Instantiate(flyPointInstance) as GameObject;
            myPoint.SetActive(true);
            myPoint.transform.parent = mapCtrl.transform;
            myPoint.transform.localScale = Vector3.one;
            myPoint.transform.localPosition = new Vector3(flyPointList[i].sceneVector.x * posXRate, flyPointList[i].sceneVector.z * posYRate, 0);
            flyPointDic.Add(flyPointList[i].id, myPoint);
            MapItem item = myPoint.GetComponent<MapItem>();
            if (item != null)
            {
                item.Init(flyPointList[i].name, new Vector3(flyPointList[i].sceneVector.x, flyPointList[i].sceneVector.z), flyPointList[i].scene);
                if (item.Obj != null)
                {
                    UIEventListener.Get(item.Obj).onClick -= OnClickMapItem;
                    UIEventListener.Get(item.Obj).onClick += OnClickMapItem;
                }
            }
           
        }
    }

    public void RefreshFlyMonster()
    {
            List<MonsterDistributionRef> list= ConfigMng.Instance.GetMonsterDistributionRefByScene(curScene);
            for (int i = 0; i < list.Count; i++)
            {
            GameObject myPoint = Instantiate(monsterInstance) as GameObject;
            myPoint.SetActive(true);
            myPoint.transform.parent = mapCtrl.transform;
            myPoint.transform.localScale = Vector3.one;
            myPoint.transform.localPosition = new Vector3(list[i].position.x * posXRate, list[i].position.z* posYRate, 0);
            mobPointDic.Add(list[i].monsterId, myPoint);
            MapItem item = myPoint.GetComponent<MapItem>();
            if (item != null)
            {
                if (list[i].refreshObjType == 2)
                {
                    SceneItemDisRef dis = ConfigMng.Instance.GetSceneItemDisRef(list[i].monsterId);
                    if (dis != null)
                    {
                        SceneItemRef S = ConfigMng.Instance.GetSceneItemRef(dis.sceneItemId);
                        if (S != null)
                        {
                            string moName = S.name;
                            item.Init(moName, new Vector3(list[i].position.x, list[i].position.z), list[i].sceneId);
                        }
                    }
                }
                else
                {
                    MonsterRef R = ConfigMng.Instance.GetMonsterRef(list[i].monsterId);
                    if (R != null)
                    {
                        string moName = R.name + "(" + R.lv + "级)";
                        item.Init(moName, new Vector3(list[i].position.x, list[i].position.z), list[i].sceneId);
                    }
                }
                if (item.Obj != null)
                {
                    UIEventListener.Get(item.Obj).onClick -= OnClickMapItem;
                    UIEventListener.Get(item.Obj).onClick += OnClickMapItem;
                }

            }
            }
           

        
    }



    public void RefreshMap()
    {
        GameCenter.cameraMng.GetCurSceneRealColorMap(GameCenter.curGameStage.SceneID, () =>
        {
            mapTexture.mainTexture = GameCenter.cameraMng.curRealColorMapTex2D;
        });
    }

    public void Refresh()
    {

    }
    void initMap()
    {
        if (initedMap)
            return;
        if (mapCtrl != null)
        {
            mapView.clipOffset = Vector2.zero;
            float sizeX = mapTexture.localSize.x / 2 - mainPlayerPoint.localPosition.x;
            float sizeY = mapTexture.localSize.y / 2 - mainPlayerPoint.localPosition.y;
            if (sizeX >= 0)
            {
                if (sizeX > maxMapViewX)
                {
                    mapView.clipOffset = new Vector2(-maxMapViewX, mapView.clipOffset.y);
                    mapView.transform.localPosition = new Vector3(mapViewPosX + maxMapViewX, mapViewPosY, mapView.transform.localPosition.z);
                }
                else
                {
                    mapView.clipOffset = new Vector3(-sizeX, mapView.clipOffset.y);
                    mapView.transform.localPosition = new Vector3(mapViewPosX + sizeX, mapViewPosY, mapView.transform.localPosition.z);
                }
            }
            else
            {
                if (-sizeX > maxMapViewX)
                {
                    mapView.clipOffset = new Vector2(maxMapViewX, mapView.clipOffset.y);
                    mapView.transform.localPosition = new Vector3(mapViewPosX - maxMapViewX, mapViewPosY, mapView.transform.localPosition.z);
                }
                else
                {
                    mapView.clipOffset = new Vector3(-sizeX, mapView.clipOffset.y);
                    mapView.transform.localPosition = new Vector3(mapViewPosX + sizeX, mapViewPosY, mapView.transform.localPosition.z);
                }

            }
            if (sizeY >= 0)
            {
                if (sizeY > maxMapViewY)
                {
                    mapView.clipOffset = new Vector2(mapView.clipOffset.x, -maxMapViewY);
                    mapView.transform.localPosition = new Vector3(mapView.transform.localPosition.x, mapViewPosY + maxMapViewY, mapView.transform.localPosition.z);
                }
                else
                {
                    mapView.clipOffset = new Vector2(mapView.clipOffset.x, -sizeY);
                    mapView.transform.localPosition = new Vector3(mapView.transform.localPosition.x, mapViewPosY + sizeY, mapView.transform.localPosition.z);
                }
            }
            else
            {
                if (-sizeY > maxMapViewY)
                {
                    mapView.clipOffset = new Vector2(mapView.clipOffset.x, maxMapViewY);
                    mapView.transform.localPosition = new Vector3(mapView.transform.localPosition.x, mapViewPosY - maxMapViewY, mapView.transform.localPosition.z);
                }
                else
                {
                    mapView.clipOffset = new Vector2(mapView.clipOffset.x, -sizeY);
                    mapView.transform.localPosition = new Vector3(mapView.transform.localPosition.x, mapViewPosY + sizeY, mapView.transform.localPosition.z);

                }
            }
            initedMap = true;
        }


    }



    IEnumerator DelayCloseWnd()
    {
        yield return new WaitForSeconds(0.3f);
        GameCenter.uIMng.SwitchToUI(GUIType.NONE);
    }

    protected void OnDeleteObj(ObjectType _type, int _instanceID)
    {
        switch (_type)
        {
            case ObjectType.Player:
                break;
            case ObjectType.MOB:
                if (mobPointDic.ContainsKey(_instanceID))
                {
                    Destroy(mobPointDic[_instanceID] as GameObject);
                    mobPointDic.Remove(_instanceID);
                }
                break;
            case ObjectType.NPC:
                if (npcPointDic.ContainsKey(_instanceID))
                {
                    Destroy(npcPointDic[_instanceID] as GameObject);
                    npcPointDic.Remove(_instanceID);
                }
                break;
            case ObjectType.FlyPoint:
                break;
            default:
                break;
        }
    }


    void RefreshTargetPoint(int _sceneID, Vector3 _point)
    {
        if (_sceneID == GameCenter.mainPlayerMng.MainPlayerInfo.SceneID)
        {
            if (targetObj != null && mapTexture != null)
            { 
                targetObj.transform.localPosition = new Vector3((mapTexture.transform.localPosition.x * 2 / GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneRef.sceneWidth) * _point.x, (mapTexture.transform.localPosition.y * 2 / GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneRef.sceneLength) * _point.z, targetObj.transform.localPosition.z);
                targetObj.SetActive(true); 
            }

        }

    }

    void ShowTargetPoin(bool _b)
    {
        if (targetObj != null && !_b)
        {
            targetObj.SetActive(false);
        }
    }

    void HideTargetPoin()
    {
        if (targetObj != null)
        {
            targetObj.SetActive(false);
        }
        GameCenter.messageMng.AddClientMsg(279);
    }







    #endregion
}


public class MapInterActiveObjectInfo
{
    public string name;
    public string typeName;
    public string iconName;
    public ObjectType typeID = ObjectType.Unknown;
    public int serverInstanceID = -1;
    public Vector3 position;
    /// <summary>
    /// 位置
    /// </summary>
    public Vector3 Position
    {
        get
        {
            switch (typeID)
            {
                case ObjectType.MOB:
                    break;
                case ObjectType.NPC:
                    NPC npc = GameCenter.curGameStage.GetNPC(serverInstanceID);
                    if (npc != null)
                    {
                        position = npc.transform.position;
                    }
                    else
                    {
                        NPCInfo info = GameCenter.sceneMng.GetNPCInfo(serverInstanceID);
                        if (info != null)
                        {
                            position = info.ServerPos;
                        }
                        else
                        {
                            position = Vector3.zero;
                        }
                    }
                    break;
                case ObjectType.FlyPoint:
                    break;
            }
            return position;
        }

    }

    public MapInterActiveObjectInfo(NPCInfo _npc)
    {
        serverInstanceID = _npc.ServerInstanceID;
        name = _npc.Name;
        typeID = ObjectType.NPC;
        typeName = _npc.AINpcDesName;
        iconName = "Pic_landian";
    }
    public MapInterActiveObjectInfo(FlyPointRef _flyPoint)
    {
        serverInstanceID = _flyPoint.id;
        name = _flyPoint.name;
        typeID = ObjectType.FlyPoint;
        position = _flyPoint.sceneVector;
        typeName = _flyPoint.desName;
        iconName = "Icon_chuansong";
    }

    public MapInterActiveObjectInfo(MonsterDistributionRef _monsterDistribution)
    {
        serverInstanceID = _monsterDistribution.id;
        name = _monsterDistribution.monsterName;
        typeID = ObjectType.MOB;
        typeName = _monsterDistribution.desName;
        iconName = "Pic_hongdian";
        position = _monsterDistribution.position;
    }
}

public class MapComparer : IComparer<MapInterActiveObjectInfo>
{
    static MapComparer instance;
    public static MapComparer Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new MapComparer();
            }
            return instance;
        }
    }
    public int Compare(MapInterActiveObjectInfo _x, MapInterActiveObjectInfo _y)
    {
        if (_x == null || _y == null)
        {
            GameSys.LogError("排序列表中存在空数据,比较失败!");
            return 0;
        }

        if (_y != null && _x != null)
        {
            int ret = _y.typeID.CompareTo(_x.typeID);
            if (ret != 0) return ret;

            ret = _y.serverInstanceID.CompareTo(_x.serverInstanceID);
            if (ret != 0) return ret;

            return ret;
        }
        else
        {
            return 0;
        }
    }
}

