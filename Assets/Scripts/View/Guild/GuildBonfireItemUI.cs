//=====================================
//作者:黄洪兴
//日期:2016/5/23
//用途:仙盟篝火成员
//========================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 仙盟篝火成员组件
/// </summary>
public class GuildBonfireItemUI : MonoBehaviour
{
    #region 控件数据
    /// <summary>
    /// 排名
    /// </summary>
    public UILabel rank;
    /// <summary>
    /// 公会名字
    /// </summary>
    public UILabel guildName;
    /// <summary>
    /// 公会等级
    /// </summary>
    public UILabel lev;
    /// <summary>
    /// 公会战力
    /// </summary>
    public UILabel pow;
    /// <summary>
    /// 进入按钮
    /// </summary>
    public GameObject goObj;
    #endregion
    #region 数据
    /// <summary>
    /// 当前填充的数据
    /// </summary>
    public st.net.NetBase.other_bonfire_list guildBonfireInfo;
    //	protected SkillInfo oldSkillinfo;  //for upgrade effect -by ms
    #endregion
    // Use this for initialization
    void Start()
    {

        if (goObj != null)
        {
            UIEventListener.Get(goObj).onClick -= GoIn;
            UIEventListener.Get(goObj).onClick += GoIn;
        }

    }

    void GoIn(GameObject obj)
    {
        if (guildBonfireInfo == null)
            return;
        GameCenter.activityMng.C2S_FlyBonfire(2, guildBonfireInfo.guild_id);
    }




    /// <summary>
    /// 填充数据
    /// </summary>
    /// <param name="_info"></param>
    public void FillInfo(st.net.NetBase.other_bonfire_list _info)
    {
        if (_info == null)
        {
            guildBonfireInfo = null;
            return;
        }
        else
        {
            guildBonfireInfo = _info;

        }
        RefreshMarketItem();
    }
    /// <summary>
    /// 刷新表现
    /// </summary>
    public void RefreshMarketItem()
    {

        if (rank != null) rank.text = guildBonfireInfo.rank.ToString();
        if (guildName != null) guildName.text = guildBonfireInfo.guild_name;
        if (lev != null) lev.text = guildBonfireInfo.guild_lev.ToString();
        if (pow != null) pow.text = guildBonfireInfo.guild_fight.ToString();

    }

    void RefreshMarketPage()
    {
    }




}
