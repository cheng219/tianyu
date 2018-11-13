/// <summary>
/// 何明军
/// 2016/4/7
/// 副本入口界面
/// </summary>
using UnityEngine;
using System.Collections;

public class CopyInNumBuyUI : MonoBehaviour {

	public UILabel name;
	public UILabel num;
	public UILabel diamo;
	
	public GameObject btnAdd;
	public GameObject btnRemove;
	public GameObject btnAddTen;
	public GameObject btnOK;
	
	CopyGroupRef curdata = null;
	int totalNum = 0;
	int curNum;
	int CurNum{
		get{
			return curNum;
		}
		set{
			if(totalNum < value){
				curNum = totalNum;
			}else{
				curNum = value;
			}
			num.text = curNum.ToString();
			diamo.text = GetDiamo().ToString();
			
			
		}
	}
	
	int GetDiamo(){
		VIPRef refData = ConfigMng.Instance.GetVIPRef(GameCenter.vipMng.VipData.vLev);
		CopyTimes times = null;
		int diamo = 0;
		if(refData != null){
			for(int i =0;i<refData.copyPurchasetimes.Count;i++){
				times = refData.copyPurchasetimes[i];
				if(times.copyID == curdata.id && times.copyTimes > 0){
					break;
				}
			}
		}
		if(times == null){
			Debug.LogError("VIP表中的副本最大购买次数有问题，没有找到副本ID="+curdata.id+"的附加购买次数，找左文祥");
			return diamo;
		}
		int count = CurNum + times.copyTimes - totalNum;
		StepConsumptionRef stepConsumptionRef = null;
		for(int i=times.copyTimes - totalNum+1;i<=count;i++){
			stepConsumptionRef = ConfigMng.Instance.GetStepConsumptionRef(i);
			diamo += stepConsumptionRef.copyNumber[0].count;
		}
		return diamo;
	}
	
	void OnEnable(){
		UIEventListener.Get(btnAdd).onClick =delegate(GameObject go) {
			CurNum ++;
		};
		UIEventListener.Get(btnRemove).onClick =delegate(GameObject go) {
			if(CurNum > 1)CurNum --;
		};
		UIEventListener.Get(btnAddTen).onClick =delegate(GameObject go) {
			CurNum = totalNum;
		};
		
		UIEventListener.Get(btnOK).onClick = OnClickToBuy;
	}
	
	public void SetToBuyShow(CopyGroupRef data){
		
		curdata = data;
		if(curdata !=null && GameCenter.duplicateMng.CopyDic.ContainsKey(curdata.id))totalNum =GameCenter.duplicateMng.CopyDic[curdata.id].buyNum;
		CurNum = 1;
		name.text = data.name;
		UIEventListener.Get(btnOK).parameter = data;
	}
	
	void OnClickToBuy(GameObject games){
		CopyGroupRef data = UIEventListener.Get(games).parameter as CopyGroupRef;
		if(CurNum <= 0){
			GameCenter.messageMng.AddClientMsg(242);
			return ;
		}
		if(data != null){
			GameCenter.duplicateMng.C2S_BuyCopyInItem(data.id,CurNum);
		}
	}
}
