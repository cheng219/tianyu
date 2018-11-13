using System;
using UnityEngine;

[ExecuteInEditMode]
public class CamControllerForArt : MonoBehaviour
{
    public Transform target;
	public float distance = 14;
    public float x = 0;
    public float y = 40;
    public int yMaxLimit = 50;
    public int yMinLimit = 16;
   // public float xSpeed = 250f;
   // public float ySpeed = 120f;
//	public float xMin = 0;
//	public float xMax = 100;
//	public float zMin = -7;
//	public float zMax = 100;
	
	static private GameObject thisCamera;
    static private CamControllerForArt instance;
	static public CamControllerForArt Instance
	{
	    get
		{
			if (instance == null) {
	            instance =thisCamera.GetComponent<CamControllerForArt>();
	        }
	        return instance;
		}
    }
	void Awake()
	{
		thisCamera = this.gameObject;
	}

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
    public void Update()
    {
		if (this.target != null)
        {			
            this.y = ClampAngle(this.y, (float)this.yMinLimit, (float)this.yMaxLimit);
            Quaternion quaternion = Quaternion.Euler(this.y, this.x, (float)0);
            Vector3 vector = ((Vector3)(quaternion * new Vector3((float)0, (float)1, -this.distance))) + this.target.position;
//			vector.z = Mathf.Clamp(vector.z,zMin,zMax);
//			vector.x = Mathf.Clamp(vector.x,xMin,xMax);
            this.transform.rotation = quaternion;
            this.transform.position = vector;
        }
    }
}