///////////////////////////////////////////////////////////////////////////////
// 作者：吴江
// 日期：2015/4/29
// 用途：游戏相机管理类
///////////////////////////////////////////////////////////////////////////////



using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum CameraModel
{
    /// <summary>
    /// 什么都不看
    /// </summary>
    None,
    /// <summary>
    /// 常规状态
    /// </summary>
    Normal,
    /// <summary>
    /// 过场动画状态
    /// </summary>
    SceneAnima,
}

public class CameraMng : MonoBehaviour
{


    #region 数据
    /// <summary>
    /// 界面摄像机
    /// </summary>
    public Camera uiCamera = null;
    /// <summary>
    /// 主像机
    /// </summary>
    public Camera mainCamera = null;
    /// <summary>
    /// 预览相机
    /// </summary>
    public Camera previewCamera = null;
    /// <summary>
    /// 地图相机
    /// </summary>
    public Camera mapCamera = null;
    /// <summary>
    /// 当前主相机的锁定跟随目标
    /// </summary>
    protected InteractiveObject curTarget = null;

    public float dragCtrlValue = 0.5f;
    /// <summary>
    /// 当前的主相机控制器
    /// </summary>
    [System.NonSerialized]
    public CamController currentCtrl;
    /// <summary>
    /// 主相机迭代控制器
    /// </summary>
    [System.NonSerialized]
    public CamController nextCtrl;


    protected Vector3 srcCamPosition = Vector3.zero;
    protected Quaternion srcCamRotation = Quaternion.identity;

    protected Vector3 curCamPosition = Vector3.zero;
    protected Quaternion curCamRotation = Quaternion.identity;

    protected bool crossFading = false;
    protected float fadeTimer = 0.0f;
    protected float fadeDuration = 1.0f;

    protected float lastShiftTime = 0;
    protected float shiftCD = 1.0f;

    /// <summary>
    /// 当前的相机高度状态 by吴江
    /// </summary>
    protected CameraHigh curCameraStateType = CameraHigh.Normal;
    //记录上一次手机触摸位置判断用户是在左放大还是缩小手势   by吴江
    private Vector2 oldPosition1;
    private Vector2 oldPosition2;
    #endregion

    #region UNITY
    void Awake()
    {
        mainCamera.enabled = false;
        Camera.SetupCurrent(mainCamera);

        GameObject cameraCtrlGO = Instantiate(exResources.GetResource(ResourceType.OTHER, "CamController")) as GameObject;
        cameraCtrlGO.name = "CameraCtrl 0";
        if (cameraCtrlGO)
        {
            currentCtrl = cameraCtrlGO.GetComponent<CamController>();
            cameraCtrlGO.SetActive(true);
            GameObject.DontDestroyOnLoad(cameraCtrlGO);
        }
        cameraCtrlGO = Instantiate(exResources.GetResource(ResourceType.OTHER, "CamController")) as GameObject;
        cameraCtrlGO.name = "CameraCtrl 1";
        if (cameraCtrlGO)
        {
            nextCtrl = cameraCtrlGO.GetComponent<CamController>();
            cameraCtrlGO.SetActive(false);
            GameObject.DontDestroyOnLoad(cameraCtrlGO);
        }

        if (mapCamera == null)
        {
            GameObject obj = GameObject.Find("MapCamera");
            if (obj != null)
            {
                mapCamera = obj.GetComponent<Camera>();
            }
        }


    }


    void Update()
    {
        //判断触摸数量为多点触摸 by吴江
        //if (Input.touchCount > 1)
        //{
        //    if (GameCenter.curGameStage != null && GameCenter.curGameStage is CityStage)
        //    {
        //        //前两只手指触摸类型都为移动触摸 by吴江
        //        if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)
        //        {
        //            //计算出当前两点触摸点的位置 by吴江
        //            var tempPosition1 = Input.GetTouch(0).position;
        //            var tempPosition2 = Input.GetTouch(1).position;
        //            //函数返回真为放大，返回假为缩小 by吴江
        //            float diff = isEnlarge(oldPosition1, oldPosition2, tempPosition1, tempPosition2);
        //            if (Mathf.Abs(diff) >= dragCtrlValue && diff != 0)
        //            {
        //                CameraSmoothNearFar(diff);
        //            }

        //            //备份上一次触摸点的位置，用于对比 by吴江
        //            oldPosition1 = tempPosition1;
        //            oldPosition2 = tempPosition2;
        //        }
        //    }
        //    else if (Input.GetAxis("Mouse ScrollWheel") != 0)
        //    {
        //        CameraSmoothNearFar(Input.GetAxis("Mouse ScrollWheel"));
        //    }
        //}
    }

    void LateUpdate()
    {
        if (crossFading)
        {
            fadeTimer += Time.deltaTime;
            float ratio = fadeTimer / fadeDuration;
            ratio = exEase.Smooth(ratio);
            curCamPosition = Vector3.Lerp(srcCamPosition,
                                            nextCtrl.transform.position,
                                            ratio);
            curCamRotation = Quaternion.Slerp(srcCamRotation,
                                                nextCtrl.transform.rotation,
                                                ratio);
            if (fadeTimer >= fadeDuration)
            {
                crossFading = false;
                CamController tmp = currentCtrl;
                currentCtrl = nextCtrl;
                nextCtrl = tmp;

                nextCtrl.gameObject.SetActive(false);

                curCamPosition = currentCtrl.transform.position;
                curCamRotation = currentCtrl.transform.rotation;
                BlackCoverAll(false);
            }
        }
        else
        {
            if (currentCtrl != null && currentCtrl.gameObject.activeSelf)
            {
                curCamPosition = currentCtrl.transform.position;
                curCamRotation = currentCtrl.transform.rotation;
            }
        }

        mainCamera.transform.position = curCamPosition;
        mainCamera.transform.rotation = curCamRotation;


    }
    #endregion

    #region 主相机相关操作

    /// <summary>
    /// 相机距离调整 by吴江
    /// </summary>
    /// <param name="_far">远小于0   近大于0</param>
    public void CameraSmoothNearFar(float _far)
    {
        if (Time.time - lastShiftTime < shiftCD) return;
        if (_far < 0) //变远
        {
            if (curCameraStateType + 1 < CameraHigh.Count)
            {
                curCameraStateType++;
            }
        }
        else if (_far > 0) //变近
        {
            if (curCameraStateType > 0)
            {
                curCameraStateType--;
            }
        }
        CameraShift(curCameraStateType);
        lastShiftTime = Time.time;

    }

    /// <summary>
    /// 函数返回真为放大，返回假为缩小 by吴江  
    /// </summary>
    /// <param name="_oP1"></param>
    /// <param name="_oP2"></param>
    /// <param name="_nP1"></param>
    /// <param name="_nP2"></param>
    /// <returns></returns>
    protected float isEnlarge(Vector2 _oP1, Vector2 _oP2, Vector2 _nP1, Vector2 _nP2)
    {
        //函数传入上一次触摸两点的位置与本次触摸两点的位置计算出用户的手势 by吴江   
        var leng1 = Mathf.Sqrt((_oP1.x - _oP2.x) * (_oP1.x - _oP2.x) + (_oP1.y - _oP2.y) * (_oP1.y - _oP2.y));
        var leng2 = Mathf.Sqrt((_nP1.x - _nP2.x) * (_nP1.x - _nP2.x) + (_nP1.y - _nP2.y) * (_nP1.y - _nP2.y));
        return leng2 - leng1;
    }



    public void InitMainCamera(int _sceneID)
    {
        SceneRef sceneRef = ConfigMng.Instance.GetSceneRef(_sceneID);

        
        if (GameCenter.curMainPlayer != null)
        {
            currentCtrl.target = GameCenter.curMainPlayer.gameObject.transform;
        }
        currentCtrl.y = sceneRef.cam_y;
        currentCtrl.aimY = sceneRef.cam_y;
        currentCtrl.distance = sceneRef.cam_distance;
        currentCtrl.aimDistance = sceneRef.cam_distance; 
        currentCtrl.xMin = sceneRef.cam_x_min;
        currentCtrl.xMax = sceneRef.cam_x_max;
        currentCtrl.zMax = sceneRef.cam_z_max;
        currentCtrl.zMin = sceneRef.cam_z_min;

        nextCtrl.y = sceneRef.cam_y;
        nextCtrl.distance = sceneRef.cam_distance;
        nextCtrl.aimY = sceneRef.cam_y;
        nextCtrl.aimDistance = sceneRef.cam_distance;
        nextCtrl.xMin = sceneRef.cam_x_min;
        nextCtrl.xMax = sceneRef.cam_x_max;
        nextCtrl.zMax = sceneRef.cam_z_max;
        nextCtrl.zMin = sceneRef.cam_z_min;


        //uiCamera.GetComponent<UICamera>().allowMultiTouch = sceneRef.sort == SceneType.DUNGEON;
    }


    public void MainCameraActive(bool _active)
    {
        if (mainCamera != null)
        {
            mainCamera.enabled = _active;
            mainCamera.gameObject.SetActive(_active);
        }
    }

    protected bool playerActive = true;
    public void MainCameraActivePlayer(bool _active)
    {
        if (playerActive == _active) return;
        if (!_active)
        {
            if (mainCamera != null)
            {
                mainCamera.cullingMask = (mainCamera.cullingMask - (1 << LayerMask.NameToLayer("Player")));
                playerActive = false;
            }
        }
        else
        {
            if (mainCamera != null)
            {
                mainCamera.cullingMask = (mainCamera.cullingMask + (1 << LayerMask.NameToLayer("Player")));
                playerActive = true;
            }
        }
    }

    public void MainCameraSetGray(bool _state)
    {
        GrayscaleEffect effect = mainCamera.GetComponent<GrayscaleEffect>();
        if(effect.shader==null)
        effect.shader = Shader.Find("Shader/GrayscaleEffect");
        if (effect != null)
        {
            effect.enabled = _state;
        }
    }

    public void MainCameraBlackWhite(float _from, float _to, float _time)
    {
        //if (mainCamera == null) return;
        //mainCamera.gameObject.SetActive(true);
        //mainCamera.enabled = true;
        //colorCorrectionCurves = (ColorCorrectionCurves)mainCamera.gameObject.GetComponent("ColorCorrectionCurves");
        //if (colorCorrectionCurves == null)
        //{
        //    colorCorrectionCurves = (ColorCorrectionCurves)mainCamera.gameObject.AddComponent("ColorCorrectionCurves");
        //}
        //StopCoroutine("ColorCurves");
        //if (_time <= 0 && _to >= 1)
        //{
        //    colorCorrectionCurves.saturation = _to;
        //    colorCorrectionCurves.enabled = false;
        //}
        //else
        //{
        //    colorCorrectionCurves.enabled = true;
        //    colorCorrectionCurves.saturation = _from;
        //    StartCoroutine("ColorCurves",new CurvesData(_from,_to,_time));
        //}
    }

    public void MainCameraShake(Vector3 _rotation, float _power)
    {
        if (!GameCenter.systemSettingMng.OpenVibrate) return;
        if (currentCtrl != null)
        {
            currentCtrl.CameraShake(_rotation, _power);
        }
    }

    public void SetMainCameraModel(int _layer)
    {
        if (mainCamera == null) return;
        mainCamera.cullingMask = _layer;
    }


    public void CameraShift(CameraHigh _type)
    {
        if (crossFading) return;
        curCameraStateType = _type;
        nextCtrl.target = currentCtrl.target;
        currentCtrl.Shift(_type);
        nextCtrl.Shift(_type);
    }

    /// <summary>
    /// 立即校准相机,开始下一次移动
    /// </summary>
    public void CorrectCameraNow()
    {

        currentCtrl.CorrectCameraAnchor();
        nextCtrl.CorrectCameraAnchor();
    }


    protected void StopCrossFade()
    {
        if (crossFading)
        {
            fadeTimer = fadeDuration;
            crossFading = false;
            CamController tmp = currentCtrl;
            currentCtrl = nextCtrl;
            nextCtrl = tmp;

            nextCtrl.gameObject.SetActive(false);

            curCamPosition = currentCtrl.transform.position;
            curCamRotation = currentCtrl.transform.rotation;
            BlackCoverAll(false);
        }
    }

    public void CrossFade(CamController _from, CamController _to, float _duration)
    {
        crossFading = true;
        fadeDuration = _duration;
        fadeTimer = 0.0f;
    }
    /// <summary>
    /// 黑屏. 一般用来做状态切换时空场景状态的遮盖
    /// </summary>
    public void BlackCoverAll(bool _active)
    {
        if (uiCamera == null)
        {
            // GameSys.LogError("UI Camera is null ,can't process black cover ! please check it!");
            return;
        }
        uiCamera.enabled = false;
        uiCamera.enabled = true;
        if (_active)
        {
            uiCamera.backgroundColor = Color.black;
            uiCamera.clearFlags = CameraClearFlags.SolidColor;
        }
        else
        {
            uiCamera.clearFlags = CameraClearFlags.Depth;
        }

    }
    /// <summary>
    /// 相机跳转
    /// </summary>
    /// <param name="_target"></param>
    /// <param name="_duration"></param>
    public void FocusOn(InteractiveObject _target, float _duration = 0.5f)
    {
        if (_target == null) return;
        curTarget = _target;
        StopCrossFade();
        if (currentCtrl.enabled && currentCtrl.target == _target.gameObject.transform) return;
        nextCtrl.gameObject.SetActive(true);
        nextCtrl.target = _target.gameObject.transform;
        srcCamPosition = curCamPosition;
        srcCamRotation = curCamRotation;

        SceneRef sceneRef = ConfigMng.Instance.GetSceneRef(GameCenter.curGameStage.SceneID);
        if (sceneRef != null)
        {
            currentCtrl.y = sceneRef.cam_y;
            currentCtrl.aimY = sceneRef.cam_y;
            currentCtrl.distance = sceneRef.cam_distance;
            currentCtrl.aimDistance = sceneRef.cam_distance;
            currentCtrl.xMin = sceneRef.cam_x_min;
            currentCtrl.xMax = sceneRef.cam_x_max;
            currentCtrl.zMax = sceneRef.cam_z_max;
            currentCtrl.zMin = sceneRef.cam_z_min;
        }


        CorrectCameraNow();


        CrossFade(currentCtrl, nextCtrl, _duration);
    }

    /// <summary>
    /// 相机跳转
    /// </summary>
    /// <param name="_target"></param>
    /// <param name="_duration"></param>
    public void FocusOn(InteractiveObject _target, float _x, float _y, float _distance, float _duration = 0.5f)
    {
        if (_target == null) return;
        StopCrossFade();
        curTarget = _target;
        if (currentCtrl.enabled && currentCtrl.target == _target.gameObject.transform) return;
        nextCtrl.gameObject.SetActive(true);
        nextCtrl.target = _target.gameObject.transform;
        nextCtrl.aimDistance = _distance;
        nextCtrl.maxDistance = 100;
        nextCtrl.minDistance = 0;
        nextCtrl.aimY = _y;
        nextCtrl.yMaxLimit = 360.0f;
        nextCtrl.yMinLimit = -360.0f;
        nextCtrl.x = _x;
        nextCtrl.xMin = -360.0f;
        nextCtrl.xMax = 360.0f;
        srcCamPosition = curCamPosition;
        srcCamRotation = curCamRotation;

        SceneRef sceneRef = ConfigMng.Instance.GetSceneRef(GameCenter.curGameStage.SceneID);
        if (sceneRef != null)
        {
            currentCtrl.y = sceneRef.cam_y;
            currentCtrl.aimY = sceneRef.cam_y;
            currentCtrl.distance = sceneRef.cam_distance;
            currentCtrl.aimDistance = sceneRef.cam_distance;
            currentCtrl.xMin = sceneRef.cam_x_min;
            currentCtrl.xMax = sceneRef.cam_x_max;
            currentCtrl.zMax = sceneRef.cam_z_max;
            currentCtrl.zMin = sceneRef.cam_z_min;
        }

        CorrectCameraNow();


        CrossFade(currentCtrl, nextCtrl, _duration);
    }

    /// <summary>
    /// 相机跳转
    /// </summary>
    /// <param name="_pos"></param>
    /// <param name="_rotY"></param>
    /// <param name="_duration"></param>
    public void FocusOn(Vector3 _pos, float _rotY, float _duration = 0.5f)
    {
        CorrectCameraNow();
        srcCamPosition = curCamPosition;
        srcCamRotation = curCamRotation;
        if (nextCtrl != null)
        {
            nextCtrl.enabled = true;
            nextCtrl.gameObject.SetActive(true);
            nextCtrl.target = null;
            nextCtrl.transform.position = _pos;
            nextCtrl.transform.localEulerAngles = new Vector3(_rotY, 0, 0);
        }
        CrossFade(currentCtrl, nextCtrl, _duration);
    }

    /// <summary>
    /// 相机跳转
    /// </summary>
    /// <param name="_pos"></param>
    /// <param name="_rotY"></param>
    /// <param name="_duration"></param>
    public void FocusOn(Vector3 _pos, Vector3 _rotation, float _duration = 0.5f)
    {
        CorrectCameraNow();
        srcCamPosition = curCamPosition;
        srcCamRotation = curCamRotation;
        if (nextCtrl != null)
        {
            nextCtrl.enabled = true;
            nextCtrl.gameObject.SetActive(true);
            nextCtrl.target = null;
            nextCtrl.transform.position = _pos;
            nextCtrl.transform.localEulerAngles = _rotation;
        }
        CrossFade(currentCtrl, nextCtrl, _duration);
    }
    #endregion

    #region 预览相机相关操作
    public void PreviewCameraActive(bool _active)
    {
        _active = _active && previewCamera.targetTexture;
        if (previewCamera != null)
        {
            previewCamera.transform.gameObject.SetActive(true);
            previewCamera.enabled = _active;
        }
    }
    #endregion

    #region 界面相机相关操作
    /// <summary>
    /// 锁定UI，不可点 by 邓成
    /// </summary>
    public void LockUICamera(bool _lock)
    {
		if(_lock)
		{
			if(uiCamera != null)
			{
				UICamera camera = uiCamera.GetComponent<UICamera>();
				if(camera != null)camera.eventReceiverMask = 0 << LayerMask.NameToLayer("NGUI");
			}
		}else
		{
			if(uiCamera != null)
			{
				UICamera camera = uiCamera.GetComponent<UICamera>();
				if(camera != null)camera.eventReceiverMask = 1 << LayerMask.NameToLayer("NGUI");
			}
		}
    }

    public void ActiveUICamera(bool _active)
    {
        if (!_active)
        {
            if (uiCamera != null)
            {
                uiCamera.cullingMask = 0 << LayerMask.NameToLayer("NGUI");
                UICamera camera = uiCamera.GetComponent<UICamera>();
                if (camera != null)camera.eventReceiverMask = 0 << LayerMask.NameToLayer("NGUI");
            }
        }
        else
        {
            if (uiCamera != null)
            {
                uiCamera.cullingMask = 1 << LayerMask.NameToLayer("NGUI");
                UICamera camera = uiCamera.GetComponent<UICamera>();
                if (camera != null) camera.eventReceiverMask = 1 << LayerMask.NameToLayer("NGUI");
            }
        }
    }
    #endregion

    #region 地图相机相关操作
    /// <summary>
    /// 简化版逻辑地图
    /// </summary>
    public Texture2D curLogicMapTex2D = null;

    /// <summary>
    /// 真实颜色地图
    /// </summary>
    public Texture2D curRealColorMapTex2D = null;
	
	public void ClearMapTexture(){
		DestroyImmediate(curRealColorMapTex2D,true);
	}

    public void GetCurSceneRealColorMap(int _sceneIndex,Action _callBack)
    {
        if (mapCamera == null) return;
        SceneRef sceneRef = ConfigMng.Instance.GetSceneRef(_sceneIndex);
        if (sceneRef == null) return;
        mapCamera.transform.localPosition = new Vector3(sceneRef.sceneWidth / 2.0f, 150, sceneRef.sceneLength / 2.0f);
        mapCamera.orthographicSize = sceneRef.sceneLength / 2.0f;
        mapCamera.gameObject.SetActive(true);
        mapCamera.aspect = sceneRef.sceneWidth / sceneRef.sceneLength;
        mapCamera.enabled = true;
        mapCamera.cullingMask = LayerMng.GetLayerMask(LayerMaskType.ColorMap);
        if (curRealColorMapTex2D != null)
        {
            DestroyImmediate(curRealColorMapTex2D, true);
            curRealColorMapTex2D = null;
        }
        StartCoroutine(CaptureByCamera(mapCamera, new Rect(0, 0, sceneRef.sceneWidth*3, sceneRef.sceneLength*3), (x) =>
        {
            curRealColorMapTex2D = x;
            mapCamera.gameObject.SetActive(false);
            mapCamera.enabled = false;
            if (_callBack != null)
            {
                _callBack();
            }
        }));
    }


    protected void InitMapCamera(int _sceneIndex,System.Action _callBack)
    {
        if (mapCamera == null) return;
        SceneRef sceneRef = ConfigMng.Instance.GetSceneRef(_sceneIndex);
        if (sceneRef == null) return;
        mapCamera.transform.localPosition = new Vector3(sceneRef.sceneWidth /2.0f, 5, sceneRef.sceneLength /2.0f);
        mapCamera.orthographicSize = sceneRef.sceneLength / 2.0f;
        mapCamera.gameObject.SetActive(true);
        mapCamera.aspect = sceneRef.sceneWidth / sceneRef.sceneLength;
        mapCamera.enabled = true;
        mapCamera.cullingMask = LayerMng.GetLayerMask(LayerMaskType.LogicMap);
        if (curLogicMapTex2D != null)
        {
            DestroyImmediate(curLogicMapTex2D, true);
            curLogicMapTex2D = null;
        }
        StartCoroutine(CaptureByCamera(mapCamera, new Rect(0, 0, sceneRef.sceneWidth, sceneRef.sceneLength), (x) =>
            {
                curLogicMapTex2D = x;
                mapCamera.gameObject.SetActive(false);
                mapCamera.enabled = false;
                if (_callBack != null)
                {
                    _callBack();
                }
            }));
    }

    private IEnumerator CaptureByCamera(Camera mCamera, Rect mRect, System.Action<Texture2D> _callBack)
    {
        //等待渲染线程结束
        yield return new WaitForEndOfFrame();

        //初始化RenderTexture
        RenderTexture mRender = new RenderTexture((int)mRect.width, (int)mRect.height, 16);
        //设置相机的渲染目标
        mCamera.targetTexture = mRender;
        //开始渲染
        mCamera.Render();

        //激活渲染贴图读取信息
        RenderTexture.active = mRender;

        Texture2D mTexture = new Texture2D((int)mRect.width, (int)mRect.height, TextureFormat.ARGB32, false);
        //读取屏幕像素信息并存储为纹理数据
        mTexture.ReadPixels(mRect, 0, 0);
        //应用
        mTexture.Apply();

        //释放相机，销毁渲染贴图
        mCamera.targetTexture = null;
        RenderTexture.active = null;
        GameObject.Destroy(mRender);

        //将图片信息编码为字节信息
       // byte[] bytes = mTexture.EncodeToPNG();
        //保存
        //System.IO.File.WriteAllBytes(mFileName, bytes);

        if (_callBack != null)
        {
            _callBack(mTexture);
        }
    }
    #endregion


    #region 辅助逻辑
    public void Init(string _sceneName, int _sceneIndex, Action _callBack)
    {
        InitMainCamera(_sceneIndex);
        if (_callBack != null)
        {
            _callBack();
        }
    }

    public void InitMap(string _sceneName, int _sceneIndex, Action _callBack)
    {
        InitMapCamera(_sceneIndex, () =>
        {
            if (_callBack != null)
            {
                _callBack();
            }
        });
    }

    public class CurvesData{

		public CurvesData(float _from,float _to,float _time)
		{
			this._from = _from;
			this._to = _to;
			this._time = _time;
		}
		public float _from;
		public float _to;
		public float _time;
	}
	IEnumerator ColorCurves(CurvesData _data)
	{
        yield return null;
        //float _from = _data._from;
        //float _to = _data._to;
        //float _time = _data._time;

        //if (colorCorrectionCurves == null) yield break;
        //colorStartCurveTime = Time.time;
        //colorCorrectionCurves.saturation = _from;
        //while (Time.time - colorStartCurveTime <= _time)
        //{
        //    colorCorrectionCurves.saturation = Mathf.Lerp(_from, _to, (Time.time - colorStartCurveTime) / _time);
        //    yield return new WaitForFixedUpdate();
        //}
        //colorCorrectionCurves.saturation = _to;
        //if (_to >= 1.0f && colorCorrectionCurves != null)
        //{
        //    colorCorrectionCurves.enabled = false;
        //}
	}

    protected float colorStartCurveTime = 0;
    IEnumerator ColorCurves(float _from,float _to,float _time)
    {
        yield return null;
        //if (colorCorrectionCurves == null) yield break;
        //colorStartCurveTime = Time.time;
        //colorCorrectionCurves.saturation = _from;
        //while (Time.time - colorStartCurveTime <= _time)
        //{
        //    colorCorrectionCurves.saturation = Mathf.Lerp(_from, _to, (Time.time - colorStartCurveTime) / _time);
        //    yield return new WaitForFixedUpdate();
        //}
        //colorCorrectionCurves.saturation = _to;
        //if (_to >= 1.0f && colorCorrectionCurves != null)
        //{
        //    colorCorrectionCurves.enabled = false;
        //}
    }
    #endregion





}



public enum CameraHigh
{
    Low,
    Normal,
    High,
    Count,
}