
using UnityEngine;

public class TweenerUILabel : UITweener
{

    public int from;
    public int to;

    UILabel mTrans;
    /// <summary>
    /// 获取label
    /// </summary>
    /// <value>
    /// The cached U ilabel.
    /// </value>
    public UILabel cachedUIlabel { get { if (mTrans == null) mTrans = transform.GetComponent<UILabel>(); return mTrans; } }
    //	public int str{
    //		get {
    //			int t=0;
    //			int.TryParse(mTrans.text,out t);
    //			return t;
    //		}
    //		set{
    //			mTrans.text=value.ToString();
    //		}
    //	}

    protected override void OnUpdate(float factor, bool isFinished) { cachedUIlabel.text = "" + (int)(from * (1f - factor) + to * factor); }


    /// <summary>
    /// 开始
    /// </summary>
    /// <param name='go'>
    /// Go.
    /// </param>
    /// <param name='duration'>
    /// Duration.
    /// </param>
    /// <param name='pos'>
    /// Position.
    /// </param>
    /// <param name='start'>
    /// Start.
    /// </param>
    static public TweenerUILabel Begin(GameObject go, float duration, int pos, int start)
    {
        TweenerUILabel comp = UITweener.Begin<TweenerUILabel>(go, duration);
        comp.from = start;
        comp.to = pos;

        if (duration <= 0f)
        {
            comp.Sample(1f, true);
            comp.enabled = false;
        }
        return comp;
    }
}

