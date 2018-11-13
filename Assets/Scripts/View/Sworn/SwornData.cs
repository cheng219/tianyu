//======================================================
//作者:朱素云
//日期:2016/5/4
//用途:结义数据类
//======================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;

public class SwornData
{
    #region 数据
    /// <summary>
    /// 玩家id
    /// </summary>
    public int playerId;
    /// <summary>
    /// 玩家情义等级
    /// </summary>
    public int playerSwornLev;
    /// <summary>
    /// 情义值
    /// </summary>
    public int brotherSworn;
    /// <summary>
    /// 誓言
    /// </summary>
    public string brotherOath;
    /// <summary>
    /// 任务积分
    /// </summary>
    public int taskNum;
    /// <summary>
    /// 义友链表
    /// </summary>
    public List<brothers_list> brothers = new List<brothers_list>();
    /// <summary>
    /// 当前情义属性链表
    /// </summary>
    private List<float> curAttr = new List<float>();
    /// <summary>
    /// 下级情义属性链表
    /// </summary>
    private List<float> nextAttr = new List<float>();
    /// <summary>
    /// 玩家信息
    /// </summary>
    public MainPlayerInfo MainData
    {
        get
        {
            return GameCenter.mainPlayerMng.MainPlayerInfo;
        }
    }

    protected SwornRef swornRef = null;
    protected SwornRef SwornRef
    {
        get
        {
            if (swornRef == null || swornRef.id != playerSwornLev)
            {
                swornRef = ConfigMng.Instance.GetSwornRef(playerSwornLev); 
            } 
            return swornRef;
        }
    }
    protected SwornRef nextSwornRef = null;
    protected SwornRef NextSwornRef
    {
        get
        {
            if (nextSwornRef == null || nextSwornRef.id != (playerSwornLev + 1))
            {
                nextSwornRef = ConfigMng.Instance.GetSwornRef(playerSwornLev + 1);
            }
            return nextSwornRef;
        }
    }

    public System.Action OnSwornDataUpdate;
    #endregion

    #region 构造
    /// <summary>
    /// 构造结义info
    /// </summary>
    /// <param name="_msg">d540</param>
    public SwornData(pt_wsorn_brother_info_d540 _msg)
    {
        this.playerId = _msg.uid;
        this.playerSwornLev = _msg.brothers_frendship_lev;
        this.brotherSworn = _msg.brothers_frendship_num;
        this.brotherOath = _msg.brothers_frendship_oath;
        this.taskNum = _msg.brothers_frendship_integer;
        this.brothers = _msg.brothers_info;
        addTag();
        for (int i = 0; i < tags.Count; i++)
        {
            getVal(tags[i], SwornRef, curAttr); 
        }
        curAttr.Add(SwornRef == null ? 0 : (float)SwornRef.ranks_exp / 10000*100);//组队经验杀怪 
        for (int i = 0; i < tags.Count; i++)
        { 
            getVal(tags[i], NextSwornRef, nextAttr);
        }
        nextAttr.Add(NextSwornRef == null ? 0 : (float)NextSwornRef.ranks_exp / 10000*100);//组队经验杀怪 
    }
    /// <summary>
    /// 刷新d540
    /// </summary>
    public void Updata(pt_wsorn_brother_info_d540 _msg)
    {
        this.playerId = _msg.uid;
        this.playerSwornLev = _msg.brothers_frendship_lev;
        this.brotherSworn = _msg.brothers_frendship_num;
        this.brotherOath = _msg.brothers_frendship_oath;
        this.taskNum = _msg.brothers_frendship_integer;
        this.brothers = _msg.brothers_info;
        addTag(); 
        for (int i = 0; i < tags.Count; i++)
        {
            getVal(tags[i], SwornRef, curAttr);
            getVal(tags[i], NextSwornRef, nextAttr);
        }
        curAttr.Add(SwornRef == null ? 0 : (float)SwornRef.ranks_exp / 1000);//组队经验杀怪  
        nextAttr.Add(NextSwornRef == null ? 0 : (float)NextSwornRef.ranks_exp / 1000);//组队经验杀怪
        if (OnSwornDataUpdate != null) OnSwornDataUpdate();
    }
    /// <summary>
    /// 获取属性值
    /// </summary> 
    protected void getVal(ActorPropertyTag _tag, SwornRef _swornRef, List<float> _attr)
    { 
        if (_swornRef != null)
        { 
            for (int i = 0; i < _swornRef.attrs.Count; i++)
            {
                if (_swornRef.attrs[i].tag == _tag)
                { 
                    _attr.Add(_swornRef.attrs[i].value);
                    break;
                }
                if (i == _swornRef.attrs.Count - 1)
                    _attr.Add(0);
            }
        }
        else
        {
            _attr.Add(0);
        }
    }
    protected List<ActorPropertyTag> tags = new List<ActorPropertyTag>();
    private void addTag()
    {
        tags.Clear();
        curAttr.Clear();
        nextAttr.Clear();
        tags.Add(ActorPropertyTag.HPLIMIT);
        tags.Add(ActorPropertyTag.ATK);
        tags.Add(ActorPropertyTag.DEF);
        tags.Add(ActorPropertyTag.CRI);
        tags.Add(ActorPropertyTag.HIT);
        tags.Add(ActorPropertyTag.DOD);
        tags.Add(ActorPropertyTag.TOUGH);
    }
    #endregion

    #region 访问器
    /// <summary>
    /// 情义图片
    /// </summary>
    public string SwornIcon
    {
        get
        {
            return SwornRef == null ? string.Empty : SwornRef.pic;
        }
    }
    /// <summary>
    /// 升级需要的情义值
    /// </summary>
    public int SwornNextVal
    {
        get
        {
            return NextSwornRef == null ? 0 : NextSwornRef.friend_ship;
        }
    }
    /// <summary>
    /// 当前属性加成
    /// </summary>
    public List<float> CurAttr
    {
        get
        {
            return curAttr;
        }
    }
    /// <summary>
    /// 下级属性加成
    /// </summary>
    public List<float> NextAttr
    {
        get
        {
            return nextAttr;
        }
    }
    /// <summary>
    /// 宝箱奖励
    /// </summary>
    public List<ItemValue> Reward(int type)
    {
        if (SwornRef != null)
        {
            if (type == 0)
                return SwornRef.reward1;
            else if (type == 1)
                return SwornRef.reward2;
            else
                return SwornRef.reward3;
        }
        return null;
    }
    #endregion
}

