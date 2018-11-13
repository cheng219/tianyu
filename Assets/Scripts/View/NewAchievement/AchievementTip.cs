//======================================================
//作者:鲁家旗
//日期:2016.7.12
//用途:获得新成就时提示
//======================================================
using UnityEngine;
using System.Collections;

public class AchievementTip : MonoBehaviour {
    public UILabel nameLabel;
    public UIButton clickBtn;
    
    public void SetAchievementTip(AchievementData _data)
    {
        AchievementRef achieveRef = ConfigMng.Instance.GetAchievementRef(_data.AchieveId);
        if (achieveRef != null && nameLabel != null)
            nameLabel.text = achieveRef.levelName;
        UIEventListener.Get(this.gameObject).onClick = delegate
        {
            DestroyImmediate(this.gameObject);
            NewRankingWnd rankWnd = GameCenter.uIMng.GetGui<NewRankingWnd>();
            if(rankWnd == null)
                GameCenter.uIMng.SwitchToSubUI(SubGUIType.ACHIEVEMENT);
        };
    }  
}
