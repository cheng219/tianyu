using UnityEngine;  
using System.Collections;  

[ExecuteInEditMode]
public class BillboardY: MonoBehaviour  
{  
	public float refashFrame = 5f;
	public Camera cameraToLookAt;  

	void Awake()
	{
		if(cameraToLookAt==null)cameraToLookAt = Camera.main;
		if (cameraToLookAt == null)
			cameraToLookAt = GameObject.Find ("MainCamera").GetComponent<Camera> ();
	}
	void Update()   
	{  
//		if (Time.frameCount % (int)(refashFrame) == 0) {
			if(cameraToLookAt==null)cameraToLookAt = Camera.main;
			if(cameraToLookAt==null)cameraToLookAt = Camera.current;
			if (cameraToLookAt) {
				Vector3 v = cameraToLookAt.transform.position - transform.position;  
				v.x = v.z = 0.0f;  
				transform.LookAt (cameraToLookAt.transform.position - v); 
			} else {
				return;
			}
//		}
	}  
}