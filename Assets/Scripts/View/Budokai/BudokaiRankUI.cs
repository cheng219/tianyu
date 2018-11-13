//=====================================
//作者:黄洪兴
//日期:2016/5/10
//用途:武道会排行组件
//========================================


using UnityEngine;
using System.Collections;
/// <summary>
/// 武道会排行UI组件
/// </summary>
public class BudokaiRankUI : MonoBehaviour
{
    #region 控件数据
    /// <summary>
    /// 排名
    /// </summary>
    public UILabel rank;
    /// <summary>
    /// 角色名字
    /// </summary>
    public UILabel playerName;
    /// <summary>
    /// 战斗力
    /// </summary>
    public UILabel power;
    /// <summary>
    /// 积分
    /// </summary>
    public UILabel score;
    #endregion
    #region 数据

    /// <summary>
    /// 当前填充的数据
    /// </summary>
    public BudokaiRankInfo budokaiRankInfo;

    #endregion
  
    void Start()
    {

    }




    /// <summary>
    /// 填充数据
    /// </summary>
    /// <param name="_info"></param>
    public void FillInfo(BudokaiRankInfo _info)
    {
        if (_info == null)
        {
            budokaiRankInfo = null;
            return;
        }
        else
        {
            budokaiRankInfo = _info;
        }
        RefreshShopItem();
    }
    /// <summary>
    /// 刷新表现
    /// </summary>
    public void RefreshShopItem()
    {


        if (rank != null) rank.text = budokaiRankInfo.Rank.ToString();
        if (playerName != null) playerName.text = budokaiRankInfo.Name.ToString();
        if (power != null) power.text = budokaiRankInfo.Power.ToString();
        if (score != null) score.text = budokaiRankInfo.Score.ToString();
        //Debug.Log("排行榜名字为" + budokaiRankInfo.Name.ToString() + "名次为" + budokaiRankInfo.Rank.ToString() + "战斗力为" + budokaiRankInfo.Power.ToString()+"得分为"+budokaiRankInfo.Score.ToString());



    }
}
