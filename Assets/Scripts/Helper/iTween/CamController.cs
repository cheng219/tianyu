using System;
using UnityEngine;


public class CamController : MonoBehaviour
{
    public Transform target = null;
    // public GameObject cameraAchor = null;
    public float distance = 24;
    public float maxDistance = 48;
    public float minDistance = 12;
    public float x = 0;
    public float y = 45;
    public float yMaxLimit = 50;
    public float yMinLimit = 16;
    // public float xSpeed = 250f;
    // public float ySpeed = 120f;
    public float xMin = 0;
    public float xMax = 100;
    public float zMin = -7;
    public float zMax = 100;

    public Vector3 currentVelocity = Vector3.zero;

    private Vector3 TargetVector;
    //private int movecount = 0;
    public enum FollowMode
    {
        Nomal,
        Smooth,
    }
    private FollowMode cameraMoveMode = FollowMode.Smooth;


    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
        {
            angle += 360;
        }
        if (angle > 360)
        {
            angle -= 360;
        }
        return Mathf.Clamp(angle, min, max);
    }


    void Awake()
    {
        // Shift(CameraHigh.Normal);
    }

    [System.NonSerialized]
    public float dampSpeed = 0;
    [System.NonSerialized]
    public float lastY = 0;
    [System.NonSerialized]
    public float lastDistance = 0;
    [System.NonSerialized]
    public float aimY = 0;
    [System.NonSerialized]
    public float aimDistance = 0;
    [System.NonSerialized]
    public float nearFarSpeed = 1.6f;


    public void Shift(CameraHigh _type)
    {
        switch (_type)
        {
            case CameraHigh.Low:
                distance = 14;
                y = 35;
                break;
            case CameraHigh.Normal:
                distance = 18;
                y = 40;
                break;
            case CameraHigh.High:
                distance = 24;
                y = 45;
                break;
            default:
                break;
        }
        lastY = y;
        aimY = y;
        lastDistance = distance;
        aimDistance = distance;
    }

    public void Update()
    {
        if (this.target != null)
        {
            if (float.IsNaN(dampSpeed)) dampSpeed = 0.0f;
            //if (y != aimY)
            //{
            //    y = Mathf.SmoothDampAngle(lastY, aimY, ref dampSpeed, nearFarSpeed);
            //}
            //else
            //{
            //    lastY = y;
            //}
            //if (distance != aimDistance)
            //{
            //    distance = Mathf.SmoothDampAngle(lastDistance, aimDistance, ref dampSpeed, nearFarSpeed);
            //}
            //else
            //{
            //    lastDistance = distance;
            //}


            y = ClampAngle(y, yMinLimit, yMaxLimit);
            Quaternion quaternion = Quaternion.Euler(this.y, this.x, (float)0);
            TargetVector = ((Vector3)(quaternion * new Vector3((float)0, (float)1, -this.distance))) + this.target.position;
            TargetVector.z = Mathf.Clamp(TargetVector.z, zMin, zMax);
            TargetVector.x = Mathf.Clamp(TargetVector.x, xMin, xMax);
            this.transform.rotation = quaternion;
            CameraMove(cameraMoveMode, TargetVector);
        }
        HandleMouseInput();
    }


    public bool globalAcceptInput = false;
    protected Vector3 rightMouseDownPos = Vector3.zero;
    public bool isRotate = false;
    public bool allowZoom = false;
    protected float curZoomDamping;

    void HandleMouseInput()
    {
        if (GameCenter.instance.isDevelopmentPattern)
        {
            if (Input.GetMouseButtonDown(1))
            {
                rightMouseDownPos = Input.mousePosition;
            }

            if (Input.GetMouseButton(1))
            {
                float axisX = Input.GetAxis("Mouse X") * 5;
                float axisY = Input.GetAxis("Mouse Y") * 5;
                x += axisX;
                y -= axisY;

                isRotate = isRotate || (rightMouseDownPos - Input.mousePosition).sqrMagnitude > 16.0f;
            }

            if (y >= 180.0f)
                y -= 360.0f;

            y = Mathf.Clamp(y, -50.0f, 80.0f);


            float zoomDelta = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(zoomDelta) >= 0.01f)
            {
                curZoomDamping = 0.3f; // zoomDamping;

                //if (collided)
                //{
                //    if (zoomDelta > 0.0f)
                //    {
                //        if (collided)
                //        {
                //            distanceNoCollision = distanceWithCollision;
                //            distanceNoCollision *= (1.0f - zoomDelta);
                //            distanceWithCollision = distanceNoCollision;
                //        }
                //    }
                //}
                //else
                //{
                    distance *= (1.0f - zoomDelta);
                //}
            }
            distance = Mathf.Clamp(distance, minDistance, maxDistance);
        }
    }


    /// <summary>
    /// 直接校准相机位置
    /// </summary>
    public void CorrectCameraAnchor()
    {
        if (target != null)
        {
            y = aimY;
            lastY = y;
            distance = aimDistance;
            lastDistance = distance;
            y = ClampAngle(y, yMinLimit, yMaxLimit);
            Quaternion quaternion = Quaternion.Euler(this.y, this.x, (float)0);
            TargetVector = ((Vector3)(quaternion * new Vector3((float)0, (float)1, -this.distance))) + this.target.position;
            TargetVector.z = Mathf.Clamp(TargetVector.z, zMin, zMax);
            TargetVector.x = Mathf.Clamp(TargetVector.x, xMin, xMax);
            this.transform.rotation = quaternion;
            CameraMove(FollowMode.Nomal, TargetVector);
        }
    }

    /// <summary>
    /// 相机移动方法  sxj
    /// </summary>
    /// <param name='Mode'>
    /// Mode.
    /// </param>
    /// <param name='TargetVector'>
    /// Target vector.
    /// </param>/
    public void CameraMove(FollowMode Mode, Vector3 TargetVector)
    {
        if (GameCenter.instance.isDevelopmentPattern)
        {
            Mode = FollowMode.Nomal;
        }
        switch (Mode)
        {
            case FollowMode.Nomal:
                this.transform.position = ActorMoveFSM.CameraLineCast(TargetVector);
                break;
            case FollowMode.Smooth:
                #region 调试
                //Vector3 lastPos = TargetVector;
                //if(movecount%2==0)
                //{
                //    Debug.DrawLine(lastPos,transform.position,Color.green,8f,true);
                //    Debug.DrawLine(lastPos,lastPos + new Vector3(0f,0.2f,0f),Color.green,8f,true);
                //}
                //else
                //{
                //    Debug.DrawLine(lastPos,transform.position,Color.white,8f,true);
                //    Debug.DrawLine(lastPos,lastPos + new Vector3(0f,0.2f,0f),Color.red,8f,true);
                //}
                //movecount++;
                #endregion
                //Debug.DrawLine(TargetVector,TargetVector + new Vector3(0f,0.5f,0f),Color.yellow,8f,true);
                TargetVector = ActorMoveFSM.CameraLineCast(TargetVector);
                this.transform.position = Vector3.SmoothDamp(transform.position, TargetVector, ref currentVelocity, 0.1f);
                break;
        }
    }


    public void CameraShake(Vector3 _rotation, float _power)
    {
        iTween.ShakePosition(this.gameObject, _rotation, _power);
    }

    //震动屏幕相关
    private void CameraShake()
    {
        //iTween.ShakePosition(Camera.main.gameObject,new Vector3(0,0.8f,0),0.2f);
        iTween.ShakePosition(this.gameObject, new Vector3(0, 0.8f, 0), 0.2f);
#if UNITY_ANDROID
        if (GameCenter.systemSettingMng.OpenVibrate)
        {
            Handheld.Vibrate();
        }
#endif
    }



    public void CameraShakeDelay(float delaytime)
    {
        this.Invoke("CameraShake", delaytime);
        print("相机震动1次" + delaytime);
    }

    public void CameraShakeDelay_copy(float delaytime)
    {
        iTween.ShakePosition(this.gameObject, new Vector3(0, 0.8f, 0), 0.2f);
    }

    //震动屏幕的三维角度
    private Vector3 v3 = Vector3.zero;
    private float time = 0;
    public void CameraShakeDelay(Vector3 _v3, int _num, float _time, bool vibrate = true)//
    {
        v3 = _v3;
        time = _time / (float)_num;
        if (vibrate)
        {
            for (int i = 0; i < _num; i++)
            {
                this.Invoke("CameraShake_V3", time * i);
            }
        }
        else													//只震屏，手机不震动
        {
            for (int i = 0; i < _num; i++)
                this.Invoke("CameraShakeNoVibrate", time * i);
        }

    }

    //按照维度的所给的三维角度震动指定的次数
    private void CameraShake_V3()
    {
        iTween.ShakePosition(this.gameObject, v3, time * 0.9f);
#if UNITY_ANDROID
        if (GameCenter.systemSettingMng.OpenVibrate)
        {
            Handheld.Vibrate();
        }
#endif
    }
    private void CameraShakeNoVibrate()
    {
        iTween.ShakePosition(this.gameObject, v3, time * 0.9f);
    }
}

