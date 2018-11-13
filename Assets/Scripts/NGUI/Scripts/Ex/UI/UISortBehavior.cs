using UnityEngine;
using System.Collections.Generic;

public class UISortBehavior : MonoBehaviour
{
	public UIPanelEx panel;
	public UIWidget widgetInFrontOfMe;
	public bool AddQueue=true;
	
	[System.NonSerialized]
	Renderer[] m_renderers;
	
	void Awake() {
		m_renderers = this.GetComponentsInChildren<Renderer>(true);
	}
	
	public void initPanel(UIPanelEx p)
	{
		panel = p;
	}
	
	void Start()
	{
		if(panel==null)
			panel = this.GetComponentInParent<UIPanelEx>();
		if(panel==null)
		{
//			UIPanel p = this.GetComponentInParent<UIPanel>();
//			GameObject obj = p.gameObject;
//			Destroy(p);
//			panel = obj.AddComponent<UIPanelEx>();
			Debug.LogError("使用该脚本，需要有UIPanelEx的父对象");
		}
		else

		panel.addUISort (this);
	}
	
	public void UpdateSortUI()
	{
		if (this.widgetInFrontOfMe != null && this.widgetInFrontOfMe.drawCall != null) {
			int rq = this.widgetInFrontOfMe.drawCall.renderQueue + 1;
			if(!AddQueue)
				rq -= 2;
			foreach (var m_renderer in m_renderers)
			{
				if(m_renderer)
				{
					foreach (Material material in m_renderer.materials)
					{
						if(material.renderQueue != rq)
						{
							material.renderQueue = rq;
						}
					}
				}
			}



		}
	}
	
	void OnDestroy()
	{
		if(panel != null)
			panel.removeUISort (this);
	}
	
}
