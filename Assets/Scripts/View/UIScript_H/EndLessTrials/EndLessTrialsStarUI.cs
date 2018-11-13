/// <summary>
/// 何明军
/// 2016/4/19
/// 无尽挑战系统
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EndLessTrialsStarUI : MonoBehaviour {

    //public UISprite boxOpen;//宝箱打开
    //public UISprite boxClose;//宝箱关闭
    public UILabel getRewardRemind;//点击领取宝箱文字
    public UILabel nameReward;
	public UILabel lineNum;
	public GameObject rewardSprite;
	public GameObject dontRewardSprite;
	public ItemUI[] items;
	
	public UIButton btnReward; 
    //红点
    public GameObject readPoint;
    protected int curChapter = 1;
	ChapterReward data = null;
	public ChapterReward GetRefData{
		get{
			return data;
		}
	}
    //void OnEnable(){
    //    GameCenter.endLessTrialsMng.OnCurChapterStarUpdate += OnCurChapterStarUpdate;
    //}
	
    //void OnDisable(){
    //    GameCenter.endLessTrialsMng.OnCurChapterStarUpdate -= OnCurChapterStarUpdate;
    //}
	
	void OnCurChapterStarUpdate(){
		if(data == null){
			return ;
		} 
        if (lineNum != null) lineNum.text = data.rewardDes.ToString(); 
		for(int i=0,len=items.Length;i<len;i++){
			if(items[i] != null && i<data.reward.Count){
				items[i].FillInfo(new EquipmentInfo(data.reward[i].eid,data.reward[i].count,EquipmentBelongTo.PREVIEW));
				items[i].gameObject.SetActive(true);
			}else{
				items[i].gameObject.SetActive(false);
			}
		}
        //Debug.Log("data.starNum :  " + data.starNum + "    GetTotalStar : " + GameCenter.endLessTrialsMng.GetTotalStar(curChapter));
		bool isreward = data.starNum > GameCenter.endLessTrialsMng.GetTotalStar(curChapter);
		if(rewardSprite != null){
			rewardSprite.SetActive(!isreward);
			UISpriteEx sp = rewardSprite.GetComponent<UISpriteEx>();
			if(sp != null){
                sp.IsGray = !isreward && !GameCenter.endLessTrialsMng.GetStarReward(curChapter, data.starNum) ? UISpriteEx.ColorGray.normal : UISpriteEx.ColorGray.Gray;
                CheckReadPoint(sp.IsGray== UISpriteEx.ColorGray.normal);
			}
		}
		if(dontRewardSprite != null){dontRewardSprite.SetActive(isreward);}
		if(btnReward != null){
			UISpriteEx sp = btnReward.GetComponentInChildren<UISpriteEx>();
			if(sp != null){
                sp.IsGray = !isreward && !GameCenter.endLessTrialsMng.GetStarReward(curChapter, data.starNum) ? UISpriteEx.ColorGray.normal : UISpriteEx.ColorGray.Gray;
			}
			UIEventListener.Get(btnReward.gameObject).onClick = delegate {
                GameCenter.endLessTrialsMng.C2S_EndReward(curChapter, data.starNum);
            };
		}
	}

    public void SetStar(ChapterReward _data, int _chapter = 0)
    {
        curChapter = _chapter;
		data = _data; 
		OnCurChapterStarUpdate();
	}
    public void CheckReadPoint(bool _t)
    {
        //if (boxOpen != null) boxOpen.gameObject.SetActive(_t);
        if (getRewardRemind != null) getRewardRemind.gameObject.SetActive(_t);
        //if (boxClose != null) boxClose.gameObject.SetActive(!_t);
        if (readPoint != null)
        {
            readPoint.SetActive(_t); 
        }
        else
            Debug.LogError("无尽试炼奖励红点丢失");
    }
    //}
}
