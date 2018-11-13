//===============================
//作者：邓成
//日期：2016/5/16
//用途：仙域守护排行界面类
//===============================

using UnityEngine;
using System.Collections;
using st.net.NetBase;

public class RankItemUI : MonoBehaviour {
	public UILabel labRank;
	public UILabel labName;
	public UILabel labValue;

	public void SetData(guild_guard_rank data,int index)
	{
		if(labRank != null)labRank.text = (index+1).ToString();
		if(labName != null)labName.text = data.name.ToString();
		if(labValue != null)labValue.text = data.damage.ToString();
	}

	public static RankItemUI CreateNew(Transform _parent)
	{
		GameObject go = null;
		UnityEngine.Object prefab = exResources.GetResource(ResourceType.GUI, "Ranking/RankingItem");
		go = Instantiate(prefab) as GameObject;
		go.transform.parent = _parent;
		go.transform.localPosition = Vector3.zero;
		go.transform.localScale = Vector3.one;
		go.SetActive(true);
		prefab = null;
		RankItemUI applyItem = go.GetComponent<RankItemUI>();
		if (applyItem == null) applyItem = go.AddComponent<RankItemUI>();
		return applyItem;
	}


}
