//----------------------------------------------
//add by sxj
//----------------------------------------------

using UnityEngine;

/// <summary>
/// Tween the object's int.
/// </summary>

[AddComponentMenu("NGUI/lyn/Tween/intNum")]
public class TweenNum : UITweener
{
	public int from;
	public int to;
	
	UILabel mUIlabel;
	/// <summary>
	/// Current num.
	/// </summary>
	public int Num
	{
		get
		{
			if (mUIlabel != null) return int.Parse(mUIlabel.text);
			return 0;
		}
		set
		{
			if (mUIlabel != null) mUIlabel.text = value.ToString();
		}
	}

	/// <summary>
	/// Find all needed components.
	/// </summary>

	void Awake ()
	{
		mUIlabel = this.gameObject.GetComponent<UILabel>();
		if (mUIlabel == null) mUIlabel = this.gameObject.AddComponent<UILabel>();
	}

	/// <summary>
	/// Interpolate and update the color.
	/// </summary>

	protected override void OnUpdate(float factor, bool isFinished)
	{
		Num = (int)(from * (1f - factor) + to * factor); 
	}

	/// <summary>
	/// Start the tweening operation.
	/// </summary>

	static public TweenNum Begin (GameObject go, float duration, int num)
	{
		TweenNum comp = UITweener.Begin<TweenNum>(go, duration);
		comp.from = comp.Num;
		comp.to = num;

		if (duration <= 0f)
		{
			comp.Sample(1f, true);
			comp.enabled = false;
		}
		return comp;
	}
}