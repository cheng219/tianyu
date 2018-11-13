//==================================
//作者：邓成
//日期：2017/4/6
//用途：宝藏活动排行界面类
//=================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class TreasureRankRewardUI : MonoBehaviour {
    public UILabel labRankDes;
    public ItemUIContainer rewardItems;

	void Start () {
	
	}
    public void refreshAll(string _des, List<ItemValue> _list)
    {
        if(labRankDes!=null)
        {
            labRankDes.text = _des;
        }
        if(rewardItems!=null)
        {
            rewardItems.RefreshItems(_list,_list.Count,_list.Count);
        }
    }
}
