//==================================
//作者：邓成
//日期：2017/4/6
//用途：宝藏活动排行奖励界面类
//=================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class TreasureRankItemUI : MonoBehaviour {
    public UILabel labRank;
    public UILabel labName;
    public UILabel labProfName;
    public UILabel labRankValue;

	void Start () {
	
	}
    public void SetData(int rank,st.net.NetBase.rank_info_base _info)
    {
        //Debug.Log("其他玩家排名_rank：" + rank);
        //Debug.Log("_info:" + _info.value1 + "," + _info.value2);
        if (labRank!=null)
        {
            labRank.text = rank.ToString();
        }
        if(labName!=null)
        {
            labName.text = _info.name;
        }
        if (labProfName != null)
        {
            labProfName.text = ConfigMng.Instance.GetPlayerConfig(_info.value1).name;
        }
        if(labRankValue!=null)
        {
            labRankValue.text = _info.value2.ToString();
        }
    }
    public void SetData(int _rank,int _openTimes)
    {
        if (labRank != null)
        {
            labRank.text = _rank.ToString();
        }
        if (labName != null)
        {
            //Debug.Log("GameCenter.curMainPlayer.ActorName:"+ GameCenter.mainPlayerMng.MainPlayerInfo.Name);
            labName.text = GameCenter.mainPlayerMng.MainPlayerInfo.Name;
        }
        if (labProfName != null)
        {
            labProfName.text = GameCenter.mainPlayerMng.MainPlayerInfo.ProfName;
        }
        if (labRankValue != null)
        {
            labRankValue.text = _openTimes.ToString();
        }
    }
    public static TreasureRankItemUI CreateNew(Transform _parent,GameObject item)
    {
        GameObject go = null;
        if(item == null || item.gameObject == null)
            go = Instantiate(exResources.GetResource(ResourceType.GUI, "")) as GameObject ;//从Assets下面加载预制
        else
            go = Instantiate(item.gameObject);//copy一份
        if (go != null)
        {
            go.transform.parent = _parent;
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            TreasureRankItemUI itemUI = go.GetComponent<TreasureRankItemUI>();
            return itemUI;
        }
        return null;
    }
    public static TreasureRankItemUI Create(Transform _parent, GameObject item, Color _color)
    {
        GameObject go = null;
        if (item == null || item.gameObject == null)
            go = Instantiate(exResources.GetResource(ResourceType.GUI, "")) as GameObject;//从Assets下面加载预制
        else
            go = Instantiate(item.gameObject);//copy一份
        if (go != null)
        {
            go.transform.parent = _parent;
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            TreasureRankItemUI itemUI = go.GetComponent<TreasureRankItemUI>();
            if (itemUI.labRank != null)
                itemUI.labRank.color = _color;
            if(itemUI.labName!=null)
                itemUI.labName.color = _color;
            if (itemUI.labProfName != null)
                itemUI.labProfName.color = _color;
            if (itemUI.labRankValue != null)
                itemUI.labRankValue.color = _color;
            return itemUI;
        }
        return null;
    }
}
