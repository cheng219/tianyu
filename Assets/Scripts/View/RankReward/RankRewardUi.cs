//==================================
//作者：朱素云
//日期：2016/4/30
//用途：等级奖励ui
//=================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RankRewardUi : MonoBehaviour {

    public UILabel levLab;
    public List<ItemUI> items = new List<ItemUI>();
    public UIButton takeBtn;
    public UIButton canNotTakeBtn;  
    protected LevelRewardLevelRef LevelRewardRef = null;
    public UISpriteEx takeMark;

    void Awake()
    {
        if (takeBtn != null) UIEventListener.Get(takeBtn.gameObject).onClick = OnClickTakeReward;
    }

    protected void OnClickTakeReward(GameObject go)
    { 
        if (LevelRewardRef != null && takeMark != null && takeMark.IsGray == UISpriteEx.ColorGray.normal)
        { 
            GameCenter.rankRewardMng.C2S_ReqGetLevReward(LevelRewardRef.level);
        }
    }

    public void Show(LevelRewardLevelRef _LevelRewardLevelRef)
    {
        LevelRewardRef = _LevelRewardLevelRef;
        int palyerLev = GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel;
        Dictionary<int, int>  levRewardDic = GameCenter.rankRewardMng.levRewardDic; 
        if (levLab != null)
        {
            levLab.text = ConfigMng.Instance.GetUItext(26, new string[1] { ConfigMng.Instance.GetLevelDes(_LevelRewardLevelRef.level) });
        }
        for (int i = 0; i < items.Count; i++)
        {
            if (_LevelRewardLevelRef.item.Count > i)
                items[i].FillInfo(new EquipmentInfo(_LevelRewardLevelRef.item[i].eid, _LevelRewardLevelRef.item[i].count, EquipmentBelongTo.PREVIEW)); 
            else
                items[i].gameObject.SetActive(false);
        }

        if (palyerLev >= _LevelRewardLevelRef.level)
        {
            if (levRewardDic.ContainsKey(_LevelRewardLevelRef.level))
            {
                takeBtn.gameObject.SetActive(false);
                canNotTakeBtn.gameObject.SetActive(true);
            }
            else
            {
                if (takeBtn != null)
                {
                    takeBtn.gameObject.SetActive(true);
                    if (takeMark != null) takeMark.IsGray = UISpriteEx.ColorGray.normal; 
                }
            }
        }
        else
        {
            canNotTakeBtn.gameObject.SetActive(false);
            if (takeMark != null) takeMark.IsGray = UISpriteEx.ColorGray.Gray;
        } 
    }
    public RankRewardUi CreateNew(Transform _parent , int _i)
    {
        GameObject obj = Instantiate(this.gameObject) as GameObject;
        obj.transform.parent = _parent;
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = new Vector3(0, -_i * 145);
        return obj.GetComponent<RankRewardUi>();
    }
}
