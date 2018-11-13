using UnityEngine;
using System.Collections.Generic;

public class UIExGridPos : MonoBehaviour,IComparer<UIExGridPos> {
	public int mPos;
	
	static public UIExGridPos Get(GameObject go)
	{
		UIExGridPos comp = go.GetComponent<UIExGridPos>();
		if(comp == null) comp=go.AddComponent<UIExGridPos>();
		return comp;
	}
	
	public int Compare(UIExGridPos x, UIExGridPos y)
	{
		return x.mPos.CompareTo(y.mPos);
	}
}
