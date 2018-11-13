/// <summary>
/// 何明军
/// 2016/4/19
/// 无尽挑战系统
/// </summary>

using UnityEngine;
using System.Collections;

public class EndLessTrialsItemUI : MonoBehaviour {

    public GameObject  headSp;//已经解锁还没有通关
    public GameObject  flagSp;//已经成功通关过但是今日还未通关/扫荡的副本
    public UILabel chapter;

	public UISprite Icon;
	public UISprite isKill;//今天是否通关
	
	public UILabel name;
	
	public UILabel fight;
	public UILabel killDes;
	
	public GameObject unShow;
	
	public UISpriteEx[] showStar;
	
	public ItemUI[] items;
	
	public ItemUI[] oneItems;
	
	public UIButton btnReward;
	
	CheckPointRef data;
	public CheckPointRef GetRefData{
		get{
			return data;
		}
	}
	
	void OnEnable(){
		GameCenter.endLessTrialsMng.OnCurChapterItemUpdate += OnCurChapterItemUpdate;
	}

	void OnDisable(){
		GameCenter.endLessTrialsMng.OnCurChapterItemUpdate -= OnCurChapterItemUpdate;
	}

	void OnCurChapterItemUpdate(){
		if(data == null)return ;
		if(Icon != null){
			Icon.spriteName = data.icon;
			Icon.MakePixelPerfect();
		}
		if(name != null)name.text = data.name;
		if(fight != null)fight.text = data.fighting;

		bool isShow = (data.frontGate > 0 && !GameCenter.endLessTrialsMng.GetItemDataFront(data.frontGate));
		if(unShow != null)unShow.GetComponent<UISpriteEx>().IsGray = isShow ? UISpriteEx.ColorGray.Gray : UISpriteEx.ColorGray.normal;
//		BoxCollider box = gameObject.GetComponent<BoxCollider>();
//		if(box != null)box.enabled = !isShow;
		
		SceneRef scene = ConfigMng.Instance.GetSceneRef(data.id);
		if(scene != null){
			for(int i=0,len=items.Length;i<len;i++){
				if(items[i] != null){
					if( i<scene.reward.Count){
						items[i].FillInfo(new EquipmentInfo(scene.reward[i].eid,scene.reward[i].count,EquipmentBelongTo.PREVIEW));
						items[i].gameObject.SetActive(true);
					}else{
						items[i].gameObject.SetActive(false);
					}
				}
			}
		}
		for(int i=0,len=oneItems.Length;i<len;i++){
			if(oneItems[i] != null && i<data.firstAward.Count){
				oneItems[i].FillInfo(new EquipmentInfo(data.firstAward[i].eid,data.firstAward[i].count,EquipmentBelongTo.PREVIEW));
				oneItems[i].gameObject.SetActive(true);
			}else{
				oneItems[i].gameObject.SetActive(false);
			}
		}

		if(btnReward != null){
			UISpriteEx spEx = btnReward.GetComponentInChildren<UISpriteEx>();
			if(spEx != null){
				spEx.IsGray = isShow ? UISpriteEx.ColorGray.Gray : UISpriteEx.ColorGray.normal;
			}
			UIEventListener.Get(btnReward.gameObject).onClick = delegate {
				GameCenter.endLessTrialsMng.C2S_InEndItem(GameCenter.endLessTrialsMng.CurChapterID,data.id);
			};
		}
        if (chapter != null)
        {
            chapter.text = data.chapterName;
        }
        if (isKill != null) isKill.gameObject.SetActive(false);
        if (flagSp != null) flagSp.SetActive(false);
        if (headSp != null) headSp.SetActive(false);
		EndLessTrialsDataInfo.EndLessTrialsItemData serData = GameCenter.endLessTrialsMng.GetItemData(data.id);
		if(serData == null){ 
//			if(unShow != null)unShow.SetActive(true);
			if(killDes != null)killDes.text = ConfigMng.Instance.GetUItext(9);
			for(int i=0,len=showStar.Length;i<len;i++){
				if(showStar[i] != null){
					showStar[i].IsGray = UISpriteEx.ColorGray.Gray;
				}
			}
            if (!isShow)
            {
                if (headSp != null) headSp.SetActive(true); 
            }
			return ;
		} 
		if(isKill != null)isKill.gameObject.SetActive(serData.enter > 0);
        if (flagSp != null) flagSp.SetActive(serData.star > 0||serData.enter > 0); 
		if(killDes != null)killDes.text = serData.ItemTime;
		for(int i=0,len=showStar.Length;i<len;i++){
			if(showStar[i]){
				if( serData.star > i){
					showStar[i].IsGray = UISpriteEx.ColorGray.normal;
				}else{
					showStar[i].IsGray = UISpriteEx.ColorGray.Gray;
				}
			}
		}
	}
	
	public void SetEndItem(CheckPointRef _data){
		data = _data;
		OnCurChapterItemUpdate();
	}
}
