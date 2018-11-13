
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// UI Panel is responsible for collecting, sorting and updating widgets in addition to generating widgets' geometry.
/// </summary>

[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/NGUI PanelEx")]
public class UIPanelEx : UIPanel
{	
	void LateUpdate ()
	{
	//	base.LateUpdate();

			// Update all draw calls, making them draw in the right order
			for (int i = 0, imax = list.Count; i < imax; ++i)
			{
				//UIPanel p = list[i];
				//if(p.GetType())
				//{
				UIPanelEx p = list[i] as UIPanelEx;
				if(p)
				{
					p.updateUISort();
				}
			//	Debug.Log("updateUISort:"+p.GetType());
				//}
			}

	}
	
	
	List<UISortBehavior> container = new List<UISortBehavior>();
	
	public void addUISort(UISortBehavior uiSort)
	{
		if(container.Contains(uiSort))
		{
			return;
		}
		container.Add (uiSort);
	}
	
	public void removeUISort(UISortBehavior uiSort)
	{
		container.Remove (uiSort);
	}
	
	public void updateUISort()
	{
		for(int i = 0; i < container.Count; i++)
		{
			container[i].UpdateSortUI();
		}
	}
	
	
	
	


}
