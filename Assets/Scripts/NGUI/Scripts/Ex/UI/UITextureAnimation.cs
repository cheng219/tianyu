//----------------------------------------------
//作者：沙新佳 qq：287490904
//最后修改时间：2016/7/19
//----------------------------------------------


using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Very simple Texture animation. Attach to a Texture and specify a common  row and Col,and it will cycle through them.
/// </summary>

[ExecuteInEditMode]
[RequireComponent(typeof(UITexture))]
[AddComponentMenu("NGUI/UI/Texture Animation")]
public class  UITextureAnimation: MonoBehaviour
{
	[HideInInspector][SerializeField] protected int mFPS = 30;
//	[HideInInspector][SerializeField] protected string mPrefix = "";
	[HideInInspector][SerializeField] protected int Rows = 4;
	[HideInInspector][SerializeField] protected int Columns = 4;
	[HideInInspector][SerializeField] protected int tileNum=16;
	float cellHeight = 0;
	float cellWidth = 0;
	[HideInInspector][SerializeField] protected bool mLoop = true;
	[HideInInspector][SerializeField] protected bool mSnap = true;

	protected UITexture mTexture;
	protected float mDelta = 0f;
	protected int mIndex = 0;
	protected bool mActive = true;
	//protected List<string> mSpriteNames = new List<string>();

	/// <summary>
	/// Number of frames in the animation.
	/// </summary>

	public int frames {
		get { 
			if(tileNum >Rows*Columns)
				tileNum = Rows*Columns;
			else
				tileNum = Rows*Columns;
			return tileNum;
		} 
	}

	/// <summary>
	/// Animation framerate.
	/// </summary>

	public int framesPerSecond { get { return mFPS; } set { mFPS = value; } }

	/// <summary>
	/// Set the name prefix used to filter sprites from the atlas.
	/// </summary>

	//public string namePrefix { get { return mPrefix; } set { if (mPrefix != value) { mPrefix = value; RebuildSpriteList(); } } }

	/// <summary>
	/// Set the animation to be looping or not
	/// </summary>

	public bool loop { get { return mLoop; } set { mLoop = value; } }

	/// <summary>
	/// Returns is the animation is still playing or not
	/// </summary>
						
	public bool isPlaying { get { return mActive; } }

	/// <summary>
	/// Rebuild the sprite list first thing.
	/// </summary>

	protected virtual void Start () {
		if (mTexture == null) mTexture = GetComponent<UITexture>();
		cellHeight=1.0f/(float)Columns;//iconwidth/texWidth;		
		cellWidth=1.0f/(float)Rows;//iconheight/texHeight;
	}

	/// <summary>
	/// Advance the sprite animation process.
	/// </summary>

	protected virtual void Update ()
	{
		if (mActive && tileNum > 1 && Application.isPlaying && mFPS > 0)
		{
			mDelta += Time.deltaTime; 

			float rate = 1f / mFPS;

			if (rate < mDelta)
			{
				mDelta = (rate > 0f) ? mDelta - rate : 0f;

				if (mIndex++ >= frames)
				{
					mIndex = 0;
					mActive = mLoop;
				}

				if (mActive)
				{
//					mSprite.spriteName = mSpriteNames[mIndex];
//					if (mSnap) mSprite.MakePixelPerfect();
					AnimationTexture();
					if (mSnap) mTexture.MakePixelPerfect();
				}
			}
		}
	}

	void AnimationTexture()		
	{   

//		if(tileNum >Rows*Columns)tileNum = Rows*Columns;
//		if(mIndex>(tileNum-1))
//
//		{
//
//			mIndex=0;
//
//		}

		//第几行  
		int hIndex=mIndex/Rows+1; //小格子左下角为轴心，0行是在边界外
		//第几列
		int vIndex=mIndex%Rows;



        float hNums = 1 - hIndex * cellHeight > 0 ? 1 - hIndex * cellHeight : 0;
        float vNums = vIndex * cellWidth > 0 ? vIndex * cellWidth : 0;
		//Debug.logger.Log (hIndex+" "+ vIndex +" "+hNums +" " +vNums);

		//*distance.sqrMagnitude*0.1f
		//mat.SetTextureScale
		//设置offset
		//mTexture.SetRect(vNums,hNums,cellWidth,cellHeight);
		mTexture.uvRect = new Rect(vNums,hNums,cellWidth,cellHeight);
		//mat.SetTextureOffset("_MainTex",new Vector2(hNums,vNums));
		//设置缩放

		//mIndex++;



	}

	
	/// <summary>
	/// Reset the animation to the beginning.
	/// </summary>

	public void Play () { mActive = true; }

	/// <summary>
	/// Pause the animation.
	/// </summary>

	public void Pause () { mActive = false; }

	/// <summary>
	/// Reset the animation to frame 0 and activate it.
	/// </summary>

	public void ResetToBeginning ()
	{
		mActive = true;
		mIndex = 0;
		if (mTexture != null && tileNum > 0)
		{
			AnimationTexture ();
			if (mSnap) mTexture.MakePixelPerfect();
		}
//
//		if (mSprite != null && mSpriteNames.Count > 0)
//		{
//			mSprite.spriteName = mSpriteNames[mIndex];
//			if (mSnap) mSprite.MakePixelPerfect();
//		}
	}
}
