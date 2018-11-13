//======================================================
//作者:何明军
//日期:2016/8/22
//用途:按钮点击CD
//======================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIButtonOnClickCD : MonoBehaviour {
	
	static public BetterList<UIButtonOnClickCD> list = new BetterList<UIButtonOnClickCD>();
	
	public int group = 0;
	public float time;
	public UITimer timer;
	
	public List<EventDelegate> onClick = new List<EventDelegate>();

	static public UIButtonOnClickCD current;

	[System.NonSerialized]protected BoxCollider box;
	[System.NonSerialized]protected UIButton btn;
	[System.NonSerialized]protected UIEventListener uIEventListener;
	[System.NonSerialized]protected UISpriteEx[] spriteExs;
	[System.NonSerialized]protected float oldTime = 0;
	[System.NonSerialized]protected float newTime = 0; 
	[System.NonSerialized]protected bool mCd = false;
	
	UIButton mButton{
		get{
			if(btn == null)btn = GetComponent<UIButton>();
			return btn;
		}
	}
	
	UISpriteEx[] mSpriteExs{
		get{
			if(spriteExs == null)spriteExs = GetComponentsInChildren<UISpriteEx>();
			if(spriteExs == null){
				Debug.LogError("没有找到组件UISpriteEx在"+gameObject.name+"对象中，不能执行按钮CD功能");
			}
			return spriteExs;
		}
	}
	
	BoxCollider mBoxCollider{
		get{
			if(box == null)box = GetComponent<BoxCollider>();
			return box;
		}
	}
	
	/// <summary>
	///是否进入CD
	/// </summary>
	public bool Value{
		get{
			return mCd;
		}
		set{
			if(mCd == value)return ;
			OnInit();
			mCd = value;
			if(mCd){
				SetValue(value);
				SetTimer(time - (newTime - oldTime));
				oldTime = mCd ? Time.time : 0;
				mBoxCollider.enabled = false;
				if(group > 0){
					for(int i=0;i<list.size;i++){
						UIButtonOnClickCD cd = list[i];
						if(cd != this && cd.group == group){
							cd.ResetTime();
						}
					}
				}
			}
		}
	}
	/// <summary>
	///背景精灵名字
	/// </summary>
	public string SetSprite{
		get{
			if(mButton != null)return mButton.normalSprite;
			return string.Empty;
		}
		set{
			if(mButton != null){
				if(mButton.normalSprite != value){
					mButton.normalSprite = value;
				}
			}
		}
	}
	
	/// <summary>
	///重现开始CD
	/// </summary>
	public void ResetTime(){
		if(Value)OnInit();
		SetValue(true);
		SetTimer(time - (newTime - oldTime));
		oldTime = mCd ? Time.time : 0;
		mBoxCollider.enabled = false;
	}
	/// <summary>
	/// 变灰
	/// </summary>
	public UISpriteEx.ColorGray IsGray{
		set{
			if(mSpriteExs != null && mSpriteExs.Length > 0){
				for(int i =0;i<mSpriteExs.Length;i++){
					mSpriteExs[i].IsGray = value;
				}
			}
		}
	}
	
	protected virtual void OnInit(){
		mCd = false;
		IsGray = UISpriteEx.ColorGray.normal;
		if(timer != null)timer.gameObject.SetActive(mCd);
		oldTime = 0;
		newTime = 0; 
		if(mBoxCollider != null)mBoxCollider.enabled = true;
		if(mButton != null){
			mButton.enabled = false;
			mButton.enabled = true;
		}
	}
	
	protected virtual void OnEnable () { list.Add(this); }
	protected virtual void OnDisable () { list.Remove(this); }
	
	protected void SetValue(bool _value){
		mCd = _value;
		IsGray = !mCd ? UISpriteEx.ColorGray.normal : UISpriteEx.ColorGray.Gray;
	}
	
	protected void SetTimer(float _time){
		if(timer != null){
			timer.gameObject.SetActive(mCd);
			if(mCd){
				timer.StartIntervalTimer((int)time);
			}
		}
	}
	
	protected virtual void OnClick()
	{
		if(current == null && enabled){
			current = this;
			EventDelegate.Execute(onClick);
			current = null;
			Value = true;
			EventExecute();
		}
	}
	
	protected void EventExecute(){
		if(uIEventListener == null)uIEventListener = GetComponent<UIEventListener>();
		if(uIEventListener != null && uIEventListener.onClick != null)uIEventListener.onClick(this.gameObject);
	}
	
	protected virtual void Update(){
		if(Value && oldTime > 0){
			newTime = Time.time;
			if(newTime - oldTime > time){
				OnInit();
			}
		}
	}
}
