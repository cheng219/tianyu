using UnityEngine;
using System.Collections;

public class UIFocus : MonoBehaviour {
	/// <summary>
	/// 强加一些子对象  by邓成
	/// </summary>
	public BoxCollider[] childBoxs;
	[HideInInspector][SerializeField] public bool IsSelectChild = false;

	void Start()
	{
		for(int i=0,count=transform.childCount; i<count; i++)
		{
			GameObject child = transform.GetChild(i).gameObject;
			BoxCollider childrenBox = child.GetComponent<BoxCollider>();
			if(childrenBox != null)
			{
				UIFocusHelper.SetOwner(child, this);
			}
		}
		if(childBoxs != null)
		{
			for (int i = 0,max=childBoxs.Length; i < max; i++) {
				if(childBoxs[i] != null)
					UIFocusHelper.SetOwner(childBoxs[i].gameObject, this);
			}
		}

		//下面注释掉的方法无法取到activeSelf为false的子对象
//		BoxCollider[] childrenBoxs = transform.GetComponentsInChildren<BoxCollider>();
//		foreach(BoxCollider box in childrenBoxs)
//		{
//			if(box.gameObject != this.gameObject)
//				UIFocusHelper.SetOwner(box.gameObject, this);
//		}
	}
	
	void OnEnable()
	{
		UICamera.selectedObject = this.gameObject;
	}

	
	void OnSelect (bool isSelected)
	{

		if (!isSelected)	//添加是否处在锁屏引导中的判断。
		{
			if(!IsSelectChild)
			{
				if(!GameCenter.noviceGuideMng.StartGuide)this.gameObject.SetActive(false);
			}
			else
			{
				IsSelectChild = false;
			}
		}
		else
		{
 			this.gameObject.SetActive(true);
		}
			
	}
}
