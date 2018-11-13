using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
[AddComponentMenu("NGUI_EX/Interaction/Grid")]
public class UIExGrid : MonoBehaviour {

	public enum Arrangement
	{
		Horizontal,
		Vertical,
	}

	public Arrangement arrangement = Arrangement.Horizontal;
	public int maxPerLine = 5;
	public float cellWidth = 100f;
	public float cellHeight = 100f;
	public bool repositionNow = false;
	public bool sorted = false;
//	public bool hideInactive = true;

	bool mStarted = false;
	
	int mX = 0;
	int mY = 0;
	
	Transform mTrans;
	Transform myTransform{
		get
		{
			if(mTrans == null) mTrans = transform;
			return mTrans;
		}
	}
	
	public delegate bool GridFilter(GameObject go);
	public GridFilter gridFilter;
	bool Filter(Transform solt)
	{
		GameObject go = solt.gameObject;
		bool ret = true;
		if(gridFilter != null/* && !gridFilter(go)*/)
		{
			ret = gridFilter(go);
//			NGUITools.SetActiveSelf(go,false);
//			return false;
		}
		NGUITools.SetActiveSelf(go,ret);//这里默认会把所有组件显示LZR
		return ret;
	}

	public delegate int GridCompare(GameObject a, GameObject b);
	public GridCompare gridCompare;
	int SortByOther (Transform a, Transform b) { return gridCompare(a.gameObject, b.gameObject);}
	
	static public int SortDefault (Transform a, Transform b)
	{
		//-----------------------
		//吐槽:这个东西写在这是不合理的,此组件是作为公用组件设计,不应该在这里引用游戏具体的UI组件,而产生不合理的依赖关系.
        ItemUI aUI = a.gameObject.GetComponent<ItemUI>();
        ItemUI bUI = b.gameObject.GetComponent<ItemUI>();
        if(aUI != null && bUI != null)
		{
            return aUI.pos.CompareTo(bUI.pos);
		}
		//-----------------------
		
		UIExGridPos aPosComp = a.gameObject.GetComponent<UIExGridPos>();
		UIExGridPos bPosComp = b.gameObject.GetComponent<UIExGridPos>();
		if(aPosComp != null && bPosComp != null)
		{
			return aPosComp.mPos.CompareTo(bPosComp.mPos);
		}
		
		Debug.Log("确定是使用名字排序??");
		return string.Compare(a.name, b.name); 
	}

	void Start()
	{
		mStarted = true;
		Reposition();
	}

	void Update ()
	{
		if (repositionNow)
		{
			repositionNow = false;
			Reposition();
		}
	}
	
	public bool AddSolt(Transform solt)
	{
		int pos = myTransform.childCount;
		
		return AddSolt(solt, pos);
//		solt.parent = myTransform;
//
////		if (!NGUITools.GetActive(solt.gameObject) && hideInactive) return false;
//		if (!Filter(solt)) return false;
//
//		float depth = -1;
//		solt.localPosition = (arrangement == Arrangement.Horizontal) ?
//			new Vector3(cellWidth * mX, -cellHeight * mY, depth) :
//			new Vector3(cellWidth * mY, -cellHeight * mX, depth);
//
//		if (++mX >= maxPerLine && maxPerLine > 0)
//		{
//			mX = 0;
//			++mY;
//		}
//		
//		return true;
	}
	
	public bool AddSolt(Transform solt, int pos)
	{
		UIExGridPos.Get(solt.gameObject).mPos = pos;
		Vector3 scale = solt.localScale;
		solt.parent = myTransform;
		solt.localScale = scale;

//		if (!NGUITools.GetActive(solt.gameObject) && hideInactive) return false;

		float depth = -1;
		solt.localPosition = (arrangement == Arrangement.Horizontal) ?
			new Vector3(cellWidth * mX, -cellHeight * mY, depth) :
			new Vector3(cellWidth * mY, -cellHeight * mX, depth);
		
		if (!Filter(solt)) return false;
		
		if (++mX >= maxPerLine && maxPerLine > 0)
		{
			mX = 0;
			++mY;
		}
		
		return true;
	}

	/// <summary>
	/// Recalculate the position of all elements within the grid, sorting them alphabetically if necessary.
	/// </summary>

	public void Reposition ()
	{
        if (pivot == UIWidget.Pivot.Center)
        {
            ResetPositionMiddle();
            return;
        }
		if (!mStarted)
		{
			repositionNow = true;
			return;
		}

		Transform myTrans = transform;

		int x = 0;
		int y = 0;

		if (sorted)
		{
			List<Transform> list = new List<Transform>();

			for (int i = 0; i < myTrans.childCount; ++i)
			{
				Transform t = myTrans.GetChild(i);
				
//				if (t && (!hideInactive || NGUITools.GetActive(t.gameObject))) list.Add(t);
				if(Filter(t)) list.Add(t);
			}
			if(gridCompare != null)
			{
				list.Sort(SortByOther);
			}
			else
			{
				list.Sort(SortDefault);
			}

			for (int i = 0, imax = list.Count; i < imax; ++i)
			{
				Transform t = list[i];

//				if (!NGUITools.GetActive(t.gameObject) && hideInactive) continue;

				float depth = t.localPosition.z;
				t.localPosition = (arrangement == Arrangement.Horizontal) ?
					new Vector3(cellWidth * x, -cellHeight * y, depth) :
					new Vector3(cellWidth * y, -cellHeight * x, depth);
				if (++x >= maxPerLine && maxPerLine > 0)
				{
					x = 0;
					++y;
				}
			}
		}
		else
		{
			for (int i = 0; i < myTrans.childCount; ++i)
			{
				Transform t = myTrans.GetChild(i);

//				if (!NGUITools.GetActive(t.gameObject) && hideInactive) continue;
				if(!Filter(t)) continue;

				float depth = t.localPosition.z;
				t.localPosition = (arrangement == Arrangement.Horizontal) ?
					new Vector3(cellWidth * x, -cellHeight * y, depth) :
					new Vector3(cellWidth * y, -cellHeight * x, depth);
				if (++x >= maxPerLine && maxPerLine > 0)
				{
					x = 0;
					++y;
				}
			}
		}
		
		mX = x;
		mY = y;

        //UIDraggablePanel drag = NGUITools.FindInParents<UIDraggablePanel>(gameObject);
        //if (drag != null) drag.UpdateScrollbars(true);
	}
	
	public void Clear()
	{
		Transform myTrans = transform;
		for (int i = myTrans.childCount-1; i >= 0; --i)
		{
			DestroyImmediate(myTrans.GetChild(i).gameObject);
		}
		mX = 0;
		mY = 0;
	}
    /// <summary>
    /// 居中效果的控制,暂时只有选择UIWidget.Pivot.Center会起效果(居中) by邓成
    /// </summary>
    public UIWidget.Pivot pivot = UIWidget.Pivot.TopLeft;

    protected void ResetPositionMiddle()
    {
        if (pivot != UIWidget.Pivot.Center) return;
        List<Transform> list = new List<Transform>();
        for (int i = 0; i < transform.childCount; ++i)
        {
            Transform t = transform.GetChild(i);
            list.Add(t);
        }
        int x = 0;
        int y = 0;
        int maxX = 0;
        int maxY = 0;
        Transform myTrans = transform;

        // Re-add the children in the same order we have them in and position them accordingly
        for (int i = 0, imax = list.Count; i < imax; ++i)
        {
            Transform t = list[i];
            // See above
            //t.parent = myTrans;

            Vector3 pos = t.localPosition;
            float depth = pos.z;

            pos = (arrangement == Arrangement.Horizontal) ? new Vector3(cellWidth * x, -cellHeight * y, depth) :new Vector3(cellWidth * y, -cellHeight * x, depth);

            //if (animateSmoothly && Application.isPlaying && Vector3.SqrMagnitude(t.localPosition - pos) >= 0.0001f)
            //{
            //    SpringPosition sp = SpringPosition.Begin(t.gameObject, pos, 15f);
            //    sp.updateScrollView = true;
            //    sp.ignoreTimeScale = true;
            //}
            //else 
            t.localPosition = pos;

            maxX = Mathf.Max(maxX, x);
            maxY = Mathf.Max(maxY, y);

            if (++x >= maxPerLine && maxPerLine > 0)
            {
                x = 0;
                ++y;
            }
        }

        // Apply the origin offset
        if (pivot != UIWidget.Pivot.TopLeft)
        {
            Vector2 po = NGUIMath.GetPivotOffset(pivot);

            float fx, fy;

            if (arrangement == Arrangement.Horizontal)
            {
                fx = Mathf.Lerp(0f, maxX * cellWidth, po.x);
                fy = Mathf.Lerp(-maxY * cellHeight, 0f, po.y);
            }
            else
            {
                fx = Mathf.Lerp(0f, maxY * cellWidth, po.x);
                fy = Mathf.Lerp(-maxX * cellHeight, 0f, po.y);
            }

            for (int i = 0; i < myTrans.childCount; ++i)
            {
                Transform t = myTrans.GetChild(i);
                SpringPosition sp = t.GetComponent<SpringPosition>();

                if (sp != null)
                {
                    sp.target.x -= fx;
                    sp.target.y -= fy;
                }
                else
                {
                    Vector3 pos = t.localPosition;
                    pos.x -= fx;
                    pos.y -= fy;
                    t.localPosition = pos;
                }
            }
        }
    }
}
