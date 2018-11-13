/// <summary>
/// 何明军
/// 2016/4/19
/// 副本结算界面
/// </summary>

using UnityEngine;
using System.Collections;

public class CopySettlementBrandUI : MonoBehaviour {
	public GameObject bg;
	public GameObject open;
	public ItemUI item;
	public TweenPosition position;
	public GameObject effct;
	
//	bool isFlops = false;
	bool isSelete = false;
	public float totalTime = 1;
	UIToggle tog;
	Vector3 resetPosition;
	void SetBgName(){
		open.SetActive(true);
		
		
		if(tog != null)tog.value = isSelete;
	}
	public void SetSelete(bool _selete){
		isSelete = _selete;
		CancelInvoke("SetTweenPositon");
		if(tog != null)tog.value = false;
		BoxCollider box = gameObject.GetComponent<BoxCollider>();
		if(box != null)box.enabled = false;
		if(position != null)position.enabled = false;
		gameObject.transform.localPosition = resetPosition;
		
		if(bg != null)bg.transform.localRotation = new Quaternion(0,0,0,0);
		if(bg != null)bg.transform.localRotation = Quaternion.Lerp(new Quaternion(0,0,0,0),new Quaternion(0,180f,0,0),totalTime);
		if(effct != null && isSelete){
			bg.gameObject.SetActive(false);
			effct.SetActive(true);
		}else{
			bg.gameObject.SetActive(true);
			effct.SetActive(false);
		}
		Invoke("SetEffect",totalTime/2);
	}
	
	void SetEffect(){
		if(!isSelete){
			bg.gameObject.SetActive(false);
			effct.SetActive(true);
		}
		Invoke("SetBgName",totalTime/2);
	}
	
	void SetTweenPositon(){
		if(position != null)position.enabled = false;
		gameObject.transform.localPosition = resetPosition;
	}
	
	public void SetTween(){
		if(position != null)position.enabled = true;
		Invoke("SetTweenPositon",0.5f);
	}
	
	public void SetItem(EquipmentInfo data){
		if(item != null)item.FillInfo(data);
	}
	
	void OnEnable(){
		if(bg != null)bg.transform.localRotation = new Quaternion(0,0,0,0);
		if(open != null)open.SetActive(false);
		BoxCollider box = gameObject.GetComponent<BoxCollider>();
		if(box != null)box.enabled = true;
		if(item != null)item.gameObject.SetActive(false);
		if(effct != null)effct.SetActive(false);
		tog = gameObject.GetComponent<UIToggle>();
		if(tog != null)tog.value = false;
		resetPosition = gameObject.transform.localPosition;
		isSelete = false;
	}
}
