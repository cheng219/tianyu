///////////////////////////////////////////////////////////////////////////////////////////
//作者：沙新佳 qq：287490904
//最后修改时间：2015/9/18
//脚本描述：仅用于特效k帧用helper
//////////////////////////////////

using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Scroll_Additive_UVOffsetColor : MonoBehaviour 
{
	public float scrollSpeed_X = 0.5f;
	public float scrollSpeed_Y = 0.5f;
	public string shaderTexName = "_Mask";


	public  Color  TintColorA = Color.gray;
	public  Color  TintColorB = Color.gray;
	public int MatIndex = 0;
	
	public Renderer mRenderer;
	
	void Awake()
	{
		mRenderer = GetComponent<Renderer>();
	}
	
	// Use this for initialization
	void Start () {
		if(mRenderer == null)mRenderer = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(mRenderer == null)mRenderer = GetComponent<Renderer>();
		if(MatIndex < mRenderer.sharedMaterials.Length)
		{
			if(mRenderer.sharedMaterials[MatIndex]!=null)
			{
				float offsetX = Time.time * scrollSpeed_X;
				float offsetY = Time.time * scrollSpeed_Y;
				mRenderer.sharedMaterials[MatIndex].SetTextureOffset(shaderTexName,new  Vector2 (offsetX,offsetY));
				mRenderer.sharedMaterials[MatIndex].SetColor("_TintColorA",TintColorA);
				mRenderer.sharedMaterials[MatIndex].SetColor("_TintColorB",TintColorB);
			}
		}
	}
}
