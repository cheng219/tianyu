///////////////////////////////////////////////////////////////////////////////////////////
//作者：沙新佳 qq：287490904
//最后修改时间：2016/2/29
//脚本描述：闪电链移动简化版控制器  
///////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;


[ExecuteInEditMode]
[RequireComponent(typeof (LineRenderer))]
public class LightningEmitter : MonoBehaviour {

	public Transform Target1;
	public Transform Target2;
	public Material mat;

	LineRenderer LR;

	public float elementLength =2f;
	Vector3 distancesqr=Vector3.zero;
	float distance = 0;

	public int TilingX =1 ;
	public int TilingY =4;
	public int tileNum=4;
	public float animSpeed = 1f;
	//中间变量
	int achievementIndex=0;	
	float cellHeight = 0;
	float cellWidth = 0;


	void Awake()
	{
		this.LR = this.gameObject.GetComponent<LineRenderer>();
		this.mat =this. GetComponent<Renderer>().sharedMaterial;
	}
	// Use this for initialization
	void Start () {
		cellHeight=1.0f/(float)TilingY;//iconwidth/texWidth;		
		cellWidth=1.0f/(float)TilingX;//iconheight/texHeight;
		
		//InvokeRepeating("AnimationTexture",0,0.1f);

	}

	// Update is called once per frame
	void Update () {
			if(Target1 && Target2)
			{
					//第1参数，对齐当前对象位置
					LR.SetPosition(0,Target1.position);
					LR.SetPosition(1,Target2.position);
			}else if(Target1 && !Target2)
			{
				LR.SetPosition(0,Target1.position);
				LR.SetPosition(1,Target1.position);
			}
			else if(Target2 && !Target1)
			{
				LR.SetPosition(0,Target2.position);
				LR.SetPosition(1,Target2.position);
			}
			else
			{
				LR.SetPosition(0,Vector3.zero);
				LR.SetPosition(1,Vector3.zero);
			}
		animSpeed = Mathf.Clamp(animSpeed,0f,10f);
		if(Time.frameCount % (int)(10/animSpeed) == 0) 
		{
			if(Target2 && Target1)
			{
				distancesqr = Target2.position - Target1.position;
				distance =  Vector3.Distance(Target2.position , Target1.position);
			}
			else
			{
				distance = 0f;
			}
			AnimationTexture();
		}

	}

	void AnimationTexture()		
	{   

		if(tileNum >TilingX*TilingY)tileNum = TilingX*TilingY;
		if(achievementIndex>(tileNum-1))
			
		{
			
			achievementIndex=0;
			
		}

		//第几行  
		int vIndex=achievementIndex/TilingX;
		//第几列
		int rowIndex=achievementIndex%TilingX;
		
		
		
		float vNums=vIndex*cellHeight;
		float hNums=rowIndex*cellWidth;

		Vector2 size=new Vector2(cellWidth*distance/elementLength,cellHeight);
		//*distance.sqrMagnitude*0.1f
		//mat.SetTextureScale
		//设置offset
		mat.SetTextureOffset("_MainTex",new Vector2(hNums,vNums));
		//设置缩放
		mat.SetTextureScale("_MainTex",size);
		
		achievementIndex++;
		
		
		
	}

}
