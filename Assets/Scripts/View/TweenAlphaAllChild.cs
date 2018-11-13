//----------------------------------------------
// 
// 类似TweenAlpha，实现渐隐渐现所有的子对象
//----------------------------------------------

using UnityEngine;


public class TweenAlphaAllChild : UITweener
{
	public float from = 1f;
	public float to = 1f;

	Transform mTrans;
	UIWidget[] mWidget;
	UIPanel mPanel;

	/// <summary>
	/// Current alpha.
	/// </summary>

	public float alpha
	{
		get
		{
			if (mWidget != null && mWidget.Length > 0) return mWidget[0].alpha;
			if (mPanel != null) return mPanel.alpha;
			return 0f;
		}
		set
		{
			if (mWidget != null) {
				for(int i =0,len = mWidget.Length;i<len;i++){
					mWidget[i].alpha = value;
				}
			}
			else if (mPanel != null) mPanel.alpha = value;
		}
	}

	/// <summary>
	/// Find all needed components.
	/// </summary>

	void Awake ()
	{
		mPanel = GetComponent<UIPanel>();
		if (mPanel == null) mWidget = GetComponentsInChildren<UIWidget>();
	}

	/// <summary>
	/// Interpolate and update the alpha.
	/// </summary>

	protected override void OnUpdate (float factor, bool isFinished) { alpha = Mathf.Lerp(from, to, factor); }

	/// <summary>
	/// Start the tweening operation.
	/// </summary>

	static public TweenAlphaAllChild Begin (GameObject go, float duration, float alpha)
	{
		TweenAlphaAllChild comp = UITweener.Begin<TweenAlphaAllChild>(go, duration);
		comp.from = comp.alpha;
		comp.to = alpha;

		if (duration <= 0f)
		{
			comp.Sample(1f, true);
			comp.enabled = false;
		}
		return comp;
	}
}
