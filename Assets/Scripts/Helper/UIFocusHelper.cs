using UnityEngine;
using System.Collections;

public class UIFocusHelper : MonoBehaviour {
//	GameObject mOwner;
	UIFocus mOwnerFocus;
//	// Use this for initialization
//	void Start () {
//	
//	}
//	
//	// Update is called once per frame
//	void Update () {
//	
//	}
	
//	static void SetOwner(GameObject go, GameObject owner)
//	{
//		UIFocusHelper helper = go.GetComponent<UIFocusHelper>();
//		if (helper == null) helper = go.AddComponent<UIFocusHelper>();
//		helper.mOwner = owner;
//	}
//	
//	static bool IsOwner(GameObject go, GameObject owner)
//	{
//		UIFocusHelper helper = go.GetComponent<UIFocusHelper>();
//		if (helper == null)
//			return false;
//		if(helper.mOwner != owner)
//			return false;
//		return true;
//	}
	
	void OnPress(bool stat)
	{
		if(stat)
		{
			mOwnerFocus.IsSelectChild = true;
		}
	}
	
	static public void SetOwner(GameObject go, UIFocus ownerFocus)
	{
		UIFocusHelper helper = go.GetComponent<UIFocusHelper>();
		if (helper == null) helper = go.AddComponent<UIFocusHelper>();
		helper.mOwnerFocus = ownerFocus;
	}
}
