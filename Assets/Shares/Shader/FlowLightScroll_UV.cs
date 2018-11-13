using UnityEngine;
using System.Collections;

public class FlowLightScroll_UV : MonoBehaviour 
{
	public float scrollSpeed_X = 0.5f;
	public float scrollSpeed_Y = 0.5f;
	public string shaderTexName = "_Mask";
	public int MatIndex = 0;

	void Start () {}
	void Update ()
	{
		float offsetX = Time.time * scrollSpeed_X;
		float offsetY = Time.time * scrollSpeed_Y;
		GetComponent<Renderer>().materials[MatIndex].SetTextureOffset(shaderTexName,new  Vector2 (offsetX,offsetY));
	}
}
