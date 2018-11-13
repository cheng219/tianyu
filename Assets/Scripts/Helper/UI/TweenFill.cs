using UnityEngine;
//using System.Collections;

public class TweenFill : UITweener {

	public float from = 1f;
	public float to = 0f;
	
	UISprite mSprite;
	
	Vector2 mSize = Vector2.zero;

	/// <summary>
	/// Current alpha.
	/// </summary>

	public float Fill
	{
		get
		{
			if (mSprite != null) 
			{
				if(mSprite.type == UISprite.Type.Filled)
				{
					return mSprite.fillAmount;
				}
				else
				{
					return mSprite.transform.localScale.x/mSize.x;
				}
			}
			else
			{
				return 0f;
			}
		}
		set
		{
			if (mSprite != null )
			{
				if(mSprite.type == UISprite.Type.Filled)
				{
					mSprite.fillAmount = value;
				}
				else
				{
					Vector3 scale = new Vector3(mSize.x*value,mSize.y*value,mSprite.transform.localScale.z);
					mSprite.transform.localScale = scale;
	
					if (mSprite != null)
					{
						if (value > 0.001f)
						{
							mSprite.enabled = true;
							mSprite.MarkAsChanged();
						}
						else
						{
							mSprite.enabled = false;
						}
					}
				}
			}
		}
	}

	/// <summary>
	/// Find all needed components.
	/// </summary>

	void Awake ()
	{
		mSprite = GetComponent<UISprite>();
		mSize = transform.localScale;
	}

	/// <summary>
	/// Interpolate and update the alpha.
	/// </summary>

	protected override void OnUpdate (float factor, bool isFinished) { Fill = Mathf.Lerp(from, to, factor); }

	/// <summary>
	/// Start the tweening operation.
	/// </summary>

	static public TweenFill Begin (GameObject go, float duration)
	{
		TweenFill comp = UITweener.Begin<TweenFill>(go, duration);
		comp.Fill = comp.from;

		if (duration <= 0f)
		{
			comp.Sample(1f, true);
			comp.enabled = false;
		}
		else
		{
			comp.enabled = true;
		}
		return comp;
	}

	static public TweenFill Begin (GameObject go, float duration,float from,float to)
	{
		TweenFill comp = UITweener.Begin<TweenFill>(go, duration);
	//	comp.Fill = comp.from;
		comp.from = from;
		comp.to = to;

		if (duration <= 0f)
		{
			comp.Sample(1f, true);
			comp.enabled = false;
		}
		else
		{
			comp.enabled = true;
		}
		return comp;
	}

	static public TweenFill Begin (GameObject go, float duration, float scale)
	{
		TweenFill comp = UITweener.Begin<TweenFill>(go, duration);
		comp.Fill = comp.from;
		comp.to = scale;

		if (duration <= 0f)
		{
			comp.Sample(1f, true);
			comp.enabled = false;
		}
		else
		{
			comp.enabled = true;
		}
		return comp;
	}
}
