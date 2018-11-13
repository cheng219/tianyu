//=====================================
//作者:黄洪兴
//日期:2016/6/30
//用途:仙盟战排行组件
//========================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuildFightRankItemUI : MonoBehaviour
{
    #region 控件数据
    /// <summary>
    /// 公会名字
    /// </summary>
    public UILabel  guildName;
    /// <summary>
    /// 积分
    /// </summary>
    public UILabel score;
    #endregion
    #region 数据
    /// <summary>
    /// 当前填充的数据
    /// </summary>
    public GuildFightRankItemInfo Info;
    //	protected SkillInfo oldSkillinfo;  //for upgrade effect -by ms
    #endregion
    // Use this for initialization
    void Start()
    {



    }





    /// <summary>
    /// 填充数据
    /// </summary>
    /// <param name="_info"></param>
    public void FillInfo(GuildFightRankItemInfo _info)
    {

        Info = _info;
        Refresh();
    }
    /// <summary>
    /// 刷新表现
    /// </summary>
    public void Refresh()
    {
        if (Info != null)
        {
            this.gameObject.SetActive(true);
            //if (playerName != null)
            //{
            //    if (Info.id == GameCenter.mainPlayerMng.MainPlayerInfo.ServerInstanceID)
            //    {
            //        playerName.text = GameCenter.mainPlayerMng.MainPlayerInfo.Name;
            //    }
            //    else
            //    {
            //        OtherPlayerInfo OPC = GameCenter.sceneMng.GetOPCInfo(Info.id);
            //        if (OPC != null)
            //        {
            //            playerName.text = OPC.Name;
            //        }
            //        else
            //        {
            //            Debug.LogError("仙盟战发来的积分数据角色ID找不到  by黄洪兴");
            //        }
            //    }
            //}
            if (guildName != null)
            {
                if (Info.name.Length > 6)
                {
                    string strOut = Info.name.Substring(0,6);
                    guildName.text = strOut + "...";
                }
                else
                    guildName.text = Info.name;
            }
            if (score != null)
                score.text = Info.score.ToString();

        }
        else
        {
            this.gameObject.SetActive(false);

        }


    }






}
