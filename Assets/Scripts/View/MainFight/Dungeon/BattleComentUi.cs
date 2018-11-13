//======================================================
//作者:朱素云
//日期:2017/3/6
//用途:火焰山评分ui
//======================================================
using UnityEngine;
using System.Collections;

public class BattleComentUi : MonoBehaviour {
  
    /// <summary>
    /// 评分
    /// </summary>
    public UISprite comentSp;
    /// <summary>
    /// 积分
    /// </summary>
    public UILabel scoreLab;
    /// <summary>
    /// 奖励
    /// </summary>
    public ItemUI[] rewards;

    protected BattleFieldRef battleField;

    public BattleFieldRef battleFieldRef
    {
        get
        {
            return battleField;
        }
        set
        {
            if (value != null)
            {
                battleField = value;
                Show();
            }
        }
    }

    void Show()
    {
        if (battleFieldRef != null)
        {
            if (comentSp != null)
            {
                comentSp.spriteName = battleFieldRef.icon;
                comentSp.MakePixelPerfect();
            }
            if (scoreLab != null && battleFieldRef.rewardConditionList.Count > 0)
            {
                scoreLab.text = battleFieldRef.rewardConditionList[0].ToString();
            }
            for (int i = 0, max = rewards.Length; i < max; i++)
            {
                if (battleFieldRef.rewardList.Count > i)
                {
                    rewards[i].FillInfo(new EquipmentInfo(battleFieldRef.rewardList[i].eid, battleFieldRef.rewardList[i].count, EquipmentBelongTo.PREVIEW));
                }
                else
                {
                    rewards[i].gameObject.SetActive(false);
                }
            }
        }
    }

    public BattleComentUi CreateNew(Transform _parent)
    {
        GameObject obj = Instantiate(this.gameObject) as GameObject;
        obj.transform.parent = _parent;
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = Vector3.zero;
        obj.SetActive(true);
        return obj.GetComponent<BattleComentUi>();
    }
}
