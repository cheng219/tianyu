/// <summary>
/// add 何明军
/// 2016/4/19
/// XX 人物基础属性预制Player_information
/// </summary>

using UnityEngine;
using System.Collections;

public class XXPlayerFigureUI : SubWnd {
	public UILabel pName;
	public UILabel pVip;
	
	public UISlider pLife;
	public UISlider pSpirit;
	public UISlider pExp;
	public UILabel pLifeText,pSpiritText,pExpText;
	
	public UILabel[] pAttribute;
	public ActorPropertyTag[] attributeType;
	
	public UILabel pLev,pProf,pGuild, pKilling;
	
	MainPlayerInfo GetMainPlayerInfo{
		get{
			return GameCenter.mainPlayerMng.MainPlayerInfo;
		}
	}
	
	void ToVipWnd(GameObject games){
		GameCenter.uIMng.SwitchToUI(GUIType.VIP);
	}

	void OnEnable(){
		Show();
		GetMainPlayerInfo.OnBaseUpdate += OnBaseUpdate;
		GetMainPlayerInfo.OnPropertyUpdate += OnPropertyUpdate;
		GameCenter.uIMng.GenGUI(GUIType.PREVIEW_MAIN,true);
	}
	
	void OnDisable(){
		GetMainPlayerInfo.OnBaseUpdate -= OnBaseUpdate;
		GetMainPlayerInfo.OnPropertyUpdate -= OnPropertyUpdate;
		GameCenter.uIMng.ReleaseGUI(GUIType.PREVIEW_MAIN);
	}
	
	void OnBaseUpdate(ActorBaseTag tag, ulong val,bool fes){
		Show();
	}
	
	void OnPropertyUpdate(ActorPropertyTag tag, long val,bool fes){
		Show();
	}
	
	void Show(){
		pName.text = GetMainPlayerInfo.Name;
		pVip.text = GameCenter.vipMng.VipData.vLev.ToString();
		pLev.text = GetMainPlayerInfo.Level.ToString();
		pProf.text = GetMainPlayerInfo.ProfName;
		pGuild.text = GetMainPlayerInfo.GuildName;
		
		pKilling.text = GetMainPlayerInfo.KillingValue.ToString();
		
		float hpVal = (float)GetMainPlayerInfo.CurHP/(float)GetMainPlayerInfo.MaxHP;
		float mpVal = (float)GetMainPlayerInfo.CurMP/(float)GetMainPlayerInfo.MaxMP;
		pLifeText.text = GetMainPlayerInfo.CurHPText+"/"+GetMainPlayerInfo.MaxHPText;
		pSpiritText.text = GetMainPlayerInfo.CurMPText+"/"+GetMainPlayerInfo.MaxMPText;
		pLife.value = hpVal > 0 ? hpVal : 0;
		pSpirit.value =  mpVal > 0 ? mpVal : 0;
//		PlayerExpRef expRef = ConfigMng.Instance.GetPlayerExpRef(GetMainPlayerInfo.Level + 1);
		if(pExp != null){
			float val = (float)GetMainPlayerInfo.CurExp/(float)GameCenter.curMainPlayer.actorInfo.MaxExp;
			pExp.value = val > 0 ? val : 0;
			pExpText.text = GetMainPlayerInfo.CurExp+"/"+GameCenter.curMainPlayer.actorInfo.MaxExp;
		}
		
		for(int i=0,len=pAttribute.Length;i<len;i++){
			if(GetMainPlayerInfo.GetServerData().propertyValueDic.ContainsKey(attributeType[i])){
				if(attributeType[i] == ActorPropertyTag.ATKDOWN){
					pAttribute[i].text = GetMainPlayerInfo.AttackStr;
				}else if(attributeType[i] == ActorPropertyTag.DEFDOWN){
					pAttribute[i].text = GetMainPlayerInfo.DefStr;
				}else{
					pAttribute[i].text = GetMainPlayerInfo.GetServerData().propertyValueDic[attributeType[i]].ToString();
				}
			}
		}
	}
}
