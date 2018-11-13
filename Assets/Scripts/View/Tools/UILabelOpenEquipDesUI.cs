//===============================
//作者：邓成
//日期：2016/6/12
//用途：前往打开物品热显的组件
//===============================

using UnityEngine;
using System.Collections;

public class UILabelOpenEquipDesUI : MonoBehaviour {
	UILabel labEquip;
    protected BoxCollider box;
	void Start()
	{
		labEquip = GetComponent<UILabel>();
		if(labEquip != null)
		{
			box = labEquip.GetComponent<BoxCollider>();
			if(box == null)
				box = labEquip.gameObject.AddComponent<BoxCollider>();
			box.size = new Vector3(labEquip.localSize.x,labEquip.localSize.y,1);
            //labEquip.autoResizeBoxCollider = true;这个不适应换行后的高度,如:升阶
		}
	}

    public void OnChange()
    {
        if (labEquip != null && box != null)
        {
            box.size = new Vector3(labEquip.localSize.x, labEquip.localSize.y, 1);
        }
    }

	void OnClick()
	{
		if(labEquip == null)return;
		string typeStr = labEquip.GetUrlAtPosition(UICamera.lastWorldPosition);
		int type = 0;
		if(int.TryParse(typeStr,out type))
		{
			ToolTipMng.ShowEquipmentTooltip(type);
		}
	}
}
