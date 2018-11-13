//====================================================
//作者：沙新佳
//日期：2015/10/19 
//用途：体积雾控制组件
//======================================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(Renderer))]
[ExecuteInEditMode]
public class VolumeFogController : MonoBehaviour {

	static public List<VolumeFogController> listActive = new List<VolumeFogController>();
	static public List<VolumeFogController> listAll = new List<VolumeFogController>();


	[SerializeField]
	public float radius = 30;
	[SerializeField]
	public Color fogColor = Color.white;
	 Material fogMat;
	Transform TargetPos;
	protected  void Awake()
	{
		if(Camera.main!=null)
		{
			Camera.main.depthTextureMode = DepthTextureMode.Depth | DepthTextureMode.DepthNormals;
		}
		else
		{
			Camera cam =  GameObject.FindObjectOfType<Camera>();
			if(cam!=null)
			{
				cam.depthTextureMode = DepthTextureMode.Depth | DepthTextureMode.DepthNormals;
			}
			else
			{
				GameObject camobj = new GameObject("Main Camera");
				cam = camobj.AddComponent<Camera>();
				cam.depthTextureMode = DepthTextureMode.Depth | DepthTextureMode.DepthNormals;
				Debug.LogError("使用体积雾，没有主相机,创建了一个，可能会引起错误");
			}

		}
		listAll.Add(this);
		fogMat = new Material(Shader.Find("FOG/volume_fog"));
		fogMat.SetColor("_FogColor",fogColor);
		//if(renderer.sharedMaterial==null)
			GetComponent<Renderer>().sharedMaterial = fogMat;

	}

	protected virtual void OnEnable () { listActive.Add(this); }
	protected virtual void OnDisable () { listActive.Remove(this); }

	protected virtual void OnDestoty()
	{
		listAll.Remove(this);
	}
	void OnRenderObject()
	{
		transform.localScale = Vector3.one*radius*2;
		Vector3 pos = this.transform.position;
		Vector4 testFloat4 = new Vector4(pos.x,pos.y,pos.z,(float)radius);
		GetComponent<Renderer>().sharedMaterial.SetColor("_FogColor",fogColor);
		GetComponent<Renderer>().sharedMaterial.SetVector("FogParam",testFloat4);
//#if UNITY_EDITOR
//		renderer.sharedMaterial.SetVector("FogParam",testFloat4);
//#else
//		renderer.sharedMaterial.SetVector("FogParam",testFloat4);
//#endif
	}

	void OnDrawGizmos()
	{
		Gizmos.color = new Color(0.4f,0.6f,0f,0.7f);
		Gizmos.DrawWireSphere(this.transform.position,radius);
	}
}
