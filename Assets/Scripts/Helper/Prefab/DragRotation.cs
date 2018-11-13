//======================================
//作者:吴江
//日期:2016/2/2
//用途:选角界面拖拽控件
//======================================

using UnityEngine;
using System.Collections;

public class DragRotation : MonoBehaviour {


    protected float hSpeed = 0.05f;
    protected Vector3 lastMousePos;
    protected bool startRatate = false;
    protected Vector3 targetRotation = Vector3.zero;

    protected float startTime = 0;
    protected float delayTime = 0.5f;


    protected float objRotY = 180f;
    protected float curRotYVel = 0.0f;
    protected float rotDampingDuration = 0.15f;

    protected bool dragEnable = false;

	void Start () {
	
	}
	
	public void SetObjRotY(float val){
		objRotY = val;
        this.transform.localEulerAngles = Vector3.zero.SetZ(objRotY);
        targetRotation = this.transform.localEulerAngles;
        startRatate = false;//点选玩家的时候会响应Input.GetMouseButtonDown(0),但是弹出的时候dragEnable为false，不会响应Input.GetMouseButtonUp(0)
	}

    public void SetEnable(bool _enable)
    {
        dragEnable = _enable;
    }

	void Update () {
        if (!dragEnable) return;
        if (Input.GetMouseButtonDown(0))
        {
            startRatate = true;
            lastMousePos = Input.mousePosition;
            startTime = Time.time;
            return;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            startRatate = false;
            return;
        }
        if (startRatate)
        {
            Vector3 offset = Input.mousePosition - lastMousePos;
            lastMousePos = Input.mousePosition;

            objRotY += Vector3.Cross(offset, Vector3.forward).normalized.y * Mathf.Abs(offset.x) * hSpeed;
            objRotY %= 360f;
            if (float.IsNaN(curRotYVel)) curRotYVel = 0.0f;

		}else{
			objRotY += 0.03f;
			objRotY %= 360f;
		}

        float newCameraRotSide = Mathf.SmoothDampAngle(targetRotation.z, objRotY, ref curRotYVel, rotDampingDuration);
        newCameraRotSide = newCameraRotSide == 0 ? 0.1f : newCameraRotSide;
        targetRotation = targetRotation.SetZ(newCameraRotSide);
        this.transform.localEulerAngles = targetRotation;
        targetRotation = this.transform.localEulerAngles;
	
	}
}
