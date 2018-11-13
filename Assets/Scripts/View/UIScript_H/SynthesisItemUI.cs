/// <summary>
/// 何明军
/// 转生属性
/// 2016/6/22
/// </summary>

using UnityEngine;
using System.Collections;

public class SynthesisItemUI : MonoBehaviour {

	public UILabel name;
	public UISprite icon;
	public UISprite itemType;
	public UILabel num;
	public UISprite itemLock;
	public ItemUI item;
	
	BlendRef refa;
	UIToggle tog;
	public BlendRef RefData{
		get{
			return refa;
		}
		set{
			if(value != null){
				refa = value;
				if(tog == null)tog = GetComponent<UIToggle>();
				if(tog != null)tog.value = false;
				SynthesisItemUIShow();
			}
		}
	}
	
	void SynthesisItemUIShow(){
		EquipmentInfo data = new EquipmentInfo(refa.itemsEnd[0].eid,refa.itemsEnd[0].count,EquipmentBelongTo.PREVIEW);
		
		if(data != null){
			if(name != null)name.text = data.ItemName;
			if(icon != null)icon.spriteName = data.IconName;
			if(itemType != null)itemType.spriteName = data.QualityBox;
			if(itemLock != null)itemLock.enabled = data.IsBind;
			int count = GetNum();
			if(num != null)num.text = count.ToString();
			if(num != null)num.enabled = count>0;
			if(num != null)num.transform.parent.gameObject.SetActive(count>0);
			
		}
	}
	/// <summary>
	/// 所需材料是同种材料
	/// </summary>
	/// <returns>The number.</returns>
	public int GetNum(){
		int count = GameCenter.inventoryMng.GetNumberByType(refa.needItems[0].eid);
		if(item != null){
			item.FillInfo(new EquipmentInfo(refa.itemsEnd[0].eid,count,EquipmentBelongTo.PREVIEW));
			if(item.itemCount != null)item.itemCount.text = (count/(refa.needItems.Count * refa.needItems[0].count)).ToString();
		}
		return count/(refa.needItems.Count * refa.needItems[0].count);
	}
	
	public void UpDateShowNum(){
		SynthesisItemUIShow();
	}
}
