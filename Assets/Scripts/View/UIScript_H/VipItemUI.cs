/// <summary>
/// 何明军
/// 2016/6/22
/// Vip等级
/// </summary>

using UnityEngine;
using System.Collections;

public class VipItemUI : MonoBehaviour {
	
	public UILabel vipLev;
    public UISprite redMind;
	

	VIPRef vipdata;
	public VIPRef RefData{
		get{
			return vipdata;
		}
		set{
			if(value != null){
				vipdata = value;
			}
			ShowVipInfo();
		}
	}
    void ShowVipInfo()
    {
        vipLev.text = vipdata.id.ToString();
    }
}
