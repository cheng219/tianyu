using UnityEngine;
using System.Collections;

public class Scroll_UV : MonoBehaviour 
{
	public float scrollSpeed_X = 0.5f;
	public float scrollSpeed_Y = 0.5f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		float offsetX = Time.time * scrollSpeed_X;
		float offsetY = Time.time * scrollSpeed_Y;
		GetComponent<Renderer>().material.mainTextureOffset =new  Vector2 (offsetX,offsetY);
	}
}
