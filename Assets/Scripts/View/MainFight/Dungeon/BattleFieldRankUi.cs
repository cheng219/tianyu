//======================================================
//作者:朱素云
//日期:2017/1/12
//用途:火焰山战场排名ui
//======================================================
using UnityEngine;
using System.Collections;
using System.Text;

public class BattleFieldRankUi : MonoBehaviour {

    /// <summary>
    /// 排名
    /// </summary>
    public UILabel rankLab;
    /// <summary>
    /// 名字
    /// </summary>
    public UILabel nameLab;
    /// <summary>
    /// 积分
    /// </summary>
    public UILabel jifenLab;
    /// <summary>
    /// 评分
    /// </summary>
    public UILabel commentLab;

    #region 结算ui

    /// <summary>
    /// 伤害
    /// </summary>
    public UILabel hitLab;
    /// <summary>
    /// 击杀
    /// </summary>
    public UILabel attackLab;
    /// <summary>
    /// 奖励级别
    /// </summary>
    public UISprite rewardLevSp;
    /// <summary>
    /// 奖励
    /// </summary>
    public UILabel rewardDes;

    #endregion

    public void SetData(st.net.NetBase.mountain_flames_rank _rankInfo, int _rank)
    {
        if (_rankInfo.camp == GameCenter.mainPlayerMng.MainPlayerInfo.Camp)//跟我一个阵营
        {
            if (rankLab != null) rankLab.text = "[00FF00]" + _rank;
            if (nameLab != null)
            {
                if (_rankInfo.camp == 1)//仙界
                {
                    nameLab.text = "[00FF00]仙、" + _rankInfo.name;
                }
                else
                {
                    nameLab.text = "[00FF00]妖、" + _rankInfo.name;
                }
            }
            if (jifenLab != null) jifenLab.text = "[00FF00]" + _rankInfo.score;
            BattleFieldRef BattleFieldRef = ConfigMng.Instance.GetBattleFieldRefByScore(_rankInfo.score);
            if (BattleFieldRef != null && commentLab != null) commentLab.text = "[00FF00]" + BattleFieldRef.scoreDes;
        }
        else
        {
            if (rankLab != null) rankLab.text = "[F26354]" + _rank;
            if (nameLab != null)
            {
                if (_rankInfo.camp == 1)//仙界
                {
                    nameLab.text = "[F26354]仙、" + _rankInfo.name;
                }
                else
                {
                    nameLab.text = "[F26354]妖、" + _rankInfo.name;
                }
            }
            if (jifenLab != null) jifenLab.text = "[F26354]" + _rankInfo.score;
            BattleFieldRef BattleFieldRef = ConfigMng.Instance.GetBattleFieldRefByScore(_rankInfo.score);
            if (BattleFieldRef != null && commentLab != null) commentLab.text = "[F26354]" + BattleFieldRef.scoreDes;
        }
    }

    /// <summary>
    /// 结算数据
    /// </summary>
    /// <param name="_rankInfo"></param>
    /// <param name="_rank"></param>
    public void SetSettlementData(st.net.NetBase.mountain_flames_win _rankInfo, int _rank, bool _isSameCamp, int _figntState)
    {
        if(_isSameCamp)//跟我一个阵营 
        {
            if (rankLab != null) rankLab.text = "[00FF00]" + _rank;
            if (nameLab != null) nameLab.text = "[00FF00]" + _rankInfo.name;
            if (jifenLab != null) jifenLab.text = "[00FF00]" + _rankInfo.amount_score;
            if (hitLab != null) hitLab.text = "[00FF00]" + _rankInfo.damage;
            if (attackLab != null) attackLab.text = "[00FF00]" + _rankInfo.kill_num;
            BattleFieldRef BattleFieldRef = ConfigMng.Instance.GetBattleFieldRefByScore(_rankInfo.amount_score);
            if (BattleFieldRef != null && rewardLevSp != null)
            {
                rewardLevSp.spriteName = BattleFieldRef.icon;
                rewardLevSp.MakePixelPerfect();
            }
            if (rewardDes != null)
            {
                BattleSettlementBonusRef battleSettle = ConfigMng.Instance.GetBattleSettlementBonusRef(_figntState);
                if (battleSettle != null)
                { 
                    StringBuilder str = new StringBuilder();
                    for(int i = 0, max = battleSettle.rewardList.Count;i<max;i++)
                    {
                        EquipmentInfo eqinfo = new EquipmentInfo(battleSettle.rewardList[i].eid, EquipmentBelongTo.PREVIEW);
                        str.Append(eqinfo.ItemName).Append("x").Append(battleSettle.rewardList[i].count).Append(" "); 
                    }
                    rewardDes.text = "[00FF00]" + str.ToString();
                }
            }
        }
        else
        {
            int id = 3;//不跟我同一阵营，胜败相反
            if (_figntState == 1) id = 2;
            if (_figntState == 2) id = 1;
            if (rankLab != null) rankLab.text = "[F26354]" + _rank;
            if (nameLab != null) nameLab.text = "[F26354]" + _rankInfo.name;
            if (jifenLab != null) jifenLab.text = "[F26354]" + _rankInfo.amount_score;
            if (hitLab != null) hitLab.text = "[F26354]" + _rankInfo.damage;
            if (attackLab != null) attackLab.text = "[F26354]" + _rankInfo.kill_num;
            BattleFieldRef BattleFieldRef = ConfigMng.Instance.GetBattleFieldRefByScore(_rankInfo.amount_score);
            if (BattleFieldRef != null && rewardLevSp != null)
            {
                rewardLevSp.spriteName = BattleFieldRef.icon;
                rewardLevSp.MakePixelPerfect();
            }
            if (rewardDes != null)
            {
                BattleSettlementBonusRef battleSettle = ConfigMng.Instance.GetBattleSettlementBonusRef(id);
                if (battleSettle != null)
                {
                    StringBuilder str = new StringBuilder();
                    for (int i = 0, max = battleSettle.rewardList.Count; i < max; i++)
                    { 
                        EquipmentInfo eqinfo = new EquipmentInfo(battleSettle.rewardList[i].eid, EquipmentBelongTo.PREVIEW);
                        str.Append(eqinfo.ItemName).Append("x").Append(battleSettle.rewardList[i].count).Append(" "); 
                    }
                    rewardDes.text = "[F26354]" + str.ToString();
                }
            }
        }
    }

    public BattleFieldRankUi CreateNew(Transform _parent)
    {
        GameObject obj = Instantiate(this.gameObject) as GameObject;
        obj.transform.parent = _parent;
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = Vector3.zero;
        obj.SetActive(true);
        return obj.GetComponent<BattleFieldRankUi>();
    }
}
