//==================================
//作者：邓成
//日期：2016/7/15
//用途：公会申请ui类
//=================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuildItem : MonoBehaviour {

    public UILabel nameLab;
    public UILabel chairManNameLab;
    public UILabel numLab;
    public UILabel levelLab;
    public UISprite campIcon;
	public UILabel fightVal;

    public UIButton applyBtn;
    public GameObject noGuildObj;
	public GameObject havaGuildObj;

    public UIButton checkBtn;
    public UISprite rankIcon;
    public UILabel rankLab;

    public void SetData(GuildData _data)
    {
		if(nameLab != null)nameLab.text = _data.guildName;
		if(chairManNameLab != null)chairManNameLab.text = _data.presidentName;
		if(numLab != null)numLab.text = _data.memberAmount.ToString()+"/"+_data.totalMemAmount;
		if(levelLab != null)levelLab.text = _data.guildLevel.ToString();
		if(fightVal != null)fightVal.text = _data.guildFightValue.ToString();
		if(noGuildObj != null)noGuildObj.gameObject.SetActive(!GameCenter.mainPlayerMng.MainPlayerInfo.HavaGuild);
		if(havaGuildObj != null)havaGuildObj.gameObject.SetActive(GameCenter.mainPlayerMng.MainPlayerInfo.HavaGuild);
		if(rankLab != null)rankLab.text = _data.guildRank.ToString();

		if(noGuildObj != null && applyBtn != null)
		{
			applyBtn.isEnabled = !_data.haveJoined;
	        UIEventListener.Get(applyBtn.gameObject).onClick = (x) =>
	            {
					GameCenter.guildMng.C2S_JoinGuild(_data.guildId);
	            };
		}
		if(checkBtn != null)
		{
	        UIEventListener.Get(checkBtn.gameObject).onClick = (x) =>
	            {
					GameCenter.guildMng.C2S_JoinGuild(_data.guildId);
	            };
		}
    }
    public static GuildItem CreateNew(Transform _parent)
    {
        GameObject go = null;
        UnityEngine.Object prefab = exResources.GetResource(ResourceType.GUI, "Guild/GuildListItem");
        go = Instantiate(prefab) as GameObject;
        prefab = null;
        go.transform.parent = _parent;
        go.SetActive(true);
        go.transform.localPosition = Vector3.zero;
        go.transform.localScale = Vector3.one;
        GuildItem guildItem = go.GetComponent<GuildItem>();
        if (guildItem == null) guildItem = go.AddComponent<GuildItem>();
        return guildItem;
    }
}
