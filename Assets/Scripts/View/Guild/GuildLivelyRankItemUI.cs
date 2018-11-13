using UnityEngine;
using System.Collections;

public class GuildLivelyRankItemUI : MonoBehaviour {
    public UILabel labName;
    public UILabel labLivelyCount;
    public UILabel labOnlineState;

	void Start () {
	
	}

    public void SetData(st.net.NetBase.guild_liveness_member_info data)
    {
        if (labName != null) labName.text = data.name;
        if (labLivelyCount != null) labLivelyCount.text = data.liveness.ToString();
        if (labOnlineState != null) labOnlineState.text = data.is_online == 1?"在线":"离线";
    }

    public GuildLivelyRankItemUI CreateNew(Transform _parent)
    {
        GameObject obj = Instantiate(this.gameObject) as GameObject;
        obj.transform.parent = _parent;
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = Vector3.zero;
        return obj.GetComponent<GuildLivelyRankItemUI>();
    }
}
