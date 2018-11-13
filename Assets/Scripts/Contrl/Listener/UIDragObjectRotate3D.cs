//----------------------------------------------
//      
// by sxj
//----------------------------------------------

using UnityEngine;
using System.Collections;


[AddComponentMenu("NGUI/lyn/UIDragObjectRotate")]
public class UIDragObjectRotate3D : IgnoreTimeScale
{
	/// <summary>
	/// Target object that will be dragged.
	/// </summary>
	
	public Actor target;
	private Shader viewShader;
	/// <summary>
	/// Scale value applied to the drag delta. Set X or Y to 0 to disallow dragging in that direction.
	/// </summary>
	
	public Vector3 axis = new Vector3(1,0,0);

	
	
	Plane mPlane;
	Vector3 mLastPos;
	
	void Awake()
	{
		//Unlit/Premultiplied Colored
		viewShader = Shader.Find("Unlit/Premultiplied Colored");
		this.gameObject.GetComponent<UITexture>().shader = viewShader;
		
	}

	public void ResetPosition()
	{
		objRotY = 180f;
		curRotYVel = 0.0f;
	}

	/// <summary>
	/// Create a plane on which we will be performing the dragging.
	/// </summary>
	
	void OnPress (bool pressed)
	{
		if (enabled && NGUITools.GetActive(gameObject) && target != null)
		{
			if (pressed)
			{
				// Remember the hit position
				mLastPos = UICamera.lastHit.point;
				// Create the plane to drag along
				Transform trans = UICamera.currentCamera.transform;
				mPlane = new Plane((trans.rotation) * Vector3.back, mLastPos);
			}
		}
	}
	
	/// <summary>
	/// Drag the object along the plane.
	/// </summary>
    protected float objRotY = 180f;
    protected float curRotYVel = 0.0f;
    protected float rotDampingDuration = 0.15f;


	void OnDrag (Vector2 delta)
	{
		if (enabled && NGUITools.GetActive(gameObject) && target != null)
		{
			UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;

            
			Ray ray = UICamera.currentCamera.ScreenPointToRay(UICamera.currentTouch.pos);
			float dist = 0f;
			
			if (mPlane.Raycast(ray, out dist))
			{
				Vector3 currentPos = ray.GetPoint(dist);
				Vector3 offset = currentPos - mLastPos;
				mLastPos = currentPos;
				
				if (offset.x != 0f || offset.y != 0f)
				{
					offset.Scale(axis);
				}else
				{
					return;
				}
				if(offset.x == 0f && offset.y == 0)
					return;
                //这种方式是直接修改角度，虽然比较块，但略生硬 by吴江
                //target.transform.Rotate(Vector3.Cross(offset, Vector3.forward).normalized, offset.magnitude * 100f, Space.World);

                //改用平滑角度修改，缺点是计算量会稍大一点 by吴江
                objRotY += Vector3.Cross(offset, Vector3.forward).normalized.y * Mathf.Abs(offset.x) * 50f;
                objRotY %= 360f;
                if (float.IsNaN(curRotYVel)) curRotYVel = 0.0f;
				float newCameraRotSide = Mathf.SmoothDampAngle(target.transform.localEulerAngles.y, objRotY, ref curRotYVel, rotDampingDuration);
                newCameraRotSide = newCameraRotSide == 0 ? 0.1f : newCameraRotSide;
                target.FaceToNoLerp(newCameraRotSide);
			}
		}
	}
}