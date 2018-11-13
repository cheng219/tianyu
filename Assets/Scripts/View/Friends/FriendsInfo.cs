//==================================
//作者：朱素云
//日期：2016/4/10
//用途：仙友数据层
//=================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;

public class FriendsInfo 
{
    /// <summary>
    /// 类型
    /// </summary>
    public int type;
    /// <summary>
    /// 好友id
    /// </summary>
    public int configId; 
    /// <summary>
    /// 是否在线
    /// </summary>
    public bool isOnline;
    /// <summary>
    /// 好友职业
    /// </summary>
    public int prof;
    /// <summary>
    /// 好友名字
    /// </summary>
    public string name;
    /// <summary>
    /// 好友等级
    /// </summary>
    public int lev;
    /// <summary>
    /// 亲密度
    /// </summary>
    public int intimacy;
    /// <summary>
    /// 所在场景
    /// </summary>
    public int placeName;
    /// <summary>
    /// 战力
    /// </summary>
	public int fight;
    /// <summary>
    /// 更新好友链表
    /// </summary>
    public System.Action OnFriendsListUpdate;
    /// <summary>
    /// 查找的好友更新
    /// </summary>
    public System.Action OnFindFirendUpdate;
    /// <summary>
    /// 构造
    /// </summary> 
    public FriendsInfo(relation_list _data)
    {
        type = _data.type;
        configId = _data.uid;
        isOnline = _data.offline_state == 0;
        prof = _data.prof;
        name = _data.name;
        lev = _data.lev;
        intimacy = _data.intimacy;
        placeName = _data.scene;
		fight = _data.fight_score;
    }
    /// <summary>
    /// 结义数据构造 聊天界面需要
    /// </summary>
    /// <param name="_data"></param>
    public FriendsInfo(brothers_list _data)
    {
        configId = _data.uid;
        isOnline = _data.oline_state == 1;
        prof = _data.prof;
        name = _data.name;
        lev = _data.lev; 
    }
    /// <summary>
    /// 仙侣数据构造 聊天界面需要
    /// </summary>
    /// <param name="_data"></param>
    public FriendsInfo(CoupleData _data)
    {
        configId = _data.objId;
        isOnline = _data.onlineState != 1;
        prof = _data.career;
        name = _data.objName;
        lev = _data.objLev; 
    }
    /// <summary>
    /// 其他玩家构造
    /// </summary> 
    public FriendsInfo(OtherPlayerInfo _data)
    { 
        configId = _data.ServerInstanceID;
        isOnline = true;
        prof = _data.Prof;
        name = _data.Name;
        lev = (int)_data.Level;
        //intimacy = _data.i;
        //placeName = _data.sc;
        fight = _data.FightValue;
    }
    public void Updata(OtherPlayerInfo _data)
    {
        configId = _data.ServerInstanceID;
        isOnline = true;
        prof = _data.Prof;
        name = _data.Name;
        lev = (int)_data.Level;
        //intimacy = _data.i;
        //placeName = _data.sc;
        fight = _data.FightValue;
    }
    /// <summary>
    /// 构造
    /// </summary> 
    public FriendsInfo(pt_update_find_friend_d710 _data)
    {
        configId = _data.uid;
        name = _data.name;
        lev = _data.lev;
        prof = _data.prof;
        if (OnFindFirendUpdate != null) OnFindFirendUpdate();
    }
    /// <summary>
    /// 查找好友更新
    /// </summary> 
    public void Updata(pt_update_find_friend_d710 _data)
    {
        configId = _data.uid;
        name = _data.name;
        lev = _data.lev;
        prof = _data.prof;
        if (OnFindFirendUpdate != null) OnFindFirendUpdate();
    }
    /// <summary>
    /// 好友更新
    /// </summary> 
    public void Updata(relation_list _data) 
    {
        type = _data.type;
        configId = _data.uid;
        isOnline = _data.offline_state == 0;
        prof = _data.prof;
        name = _data.name;
        lev = _data.lev;
        intimacy = _data.intimacy;
        placeName = _data.scene;
		fight = _data.fight_score;
        if (OnFriendsListUpdate != null) OnFriendsListUpdate();
    }
    /// <summary>
    /// 更新亲密度
    /// </summary>
    /// <param name="_intimacy"></param>
    public void Updata(int _intimacy)
    {
        intimacy = _intimacy;
        if (OnFriendsListUpdate != null) OnFriendsListUpdate();
    }
    #region 访问器 
    /// <summary>
    /// 是否在线
    /// </summary>
    public bool IsOnline
    {
        get
        {
            return isOnline;
        }
    }
    /// <summary>
    /// 精灵图像
    /// </summary>
    public string Icon
    {
        get
        {
            PlayerConfig ply = ConfigMng.Instance.GetPlayerConfig(prof);
            return ply != null?ply.res_head_Icon:string.Empty;
        }
    } 
    /// <summary>
    /// 等级
    /// </summary>
    public string Lev
    {
        get
        { 
            return ConfigMng.Instance.GetLevelDes(lev);
        }
    }
    /// <summary>
    /// 亲密度
    /// </summary>
    public int Intimacy
    {
        get
        {
            return intimacy;
        }
    }
    /// <summary>
    /// 玩家所在场景
    /// </summary>
    public string PlaceName
    {
        get
        { 
            SceneRef scene = ConfigMng.Instance.GetSceneRef(placeName); 
            return scene != null ? scene.name:string.Empty;
        }
    }
    #endregion
}
