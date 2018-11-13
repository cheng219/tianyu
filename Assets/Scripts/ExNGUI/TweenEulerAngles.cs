//===============================
//作者:吴江
//日期:2015/4/20
//用途:UI可进行360旋转的扩展控件 
//================================

using UnityEngine;

/// <summary>
/// Tween the object's rotation.
/// </summary>

[AddComponentMenu("NGUI/Tween/EulerAngles")]
public class TweenEulerAngles : UITweener
{
	public Vector3 from;
	public Vector3 to;

	Transform mTrans;

	public Transform cachedTransform { get { if (mTrans == null) mTrans = transform; return mTrans; } }
	public Quaternion rotation { get { return cachedTransform.localRotation; } set { cachedTransform.localRotation = value; } }

	protected override void OnUpdate (float factor, bool isFinished)
	{
        cachedTransform.localEulerAngles = Vector3.Lerp(from, to, factor);
	}

	/// <summary>
	/// Start the tweening operation.
	/// </summary>

	static public TweenRotation Begin (GameObject go, float duration, Quaternion rot)
	{
		TweenRotation comp = UITweener.Begin<TweenRotation>(go, duration);
		comp.from = comp.value.eulerAngles;
		comp.to = rot.eulerAngles;

		if (duration <= 0f)
		{
			comp.Sample(1f, true);
			comp.enabled = false;
		}
		return comp;
	}
}