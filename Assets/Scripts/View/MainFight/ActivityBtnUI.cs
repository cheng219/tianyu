//======================================================
//作者:鲁家旗
//日期:2017/2/7
//用途:活动提示
//======================================================
using UnityEngine;
using System.Collections;

public class ActivityBtnUI : MonoBehaviour {
    public UISprite spAct;
    public UILabel actName;
    public void Refresh(ActivityListRef _data)
    {

        if (spAct != null)
        {
            spAct.spriteName = _data.icon;
            spAct.MakePixelPerfect();
        }
        if (actName != null) actName.text = _data.name;
        //ActivityDataInfo info = GameCenter.activityMng.GetActivityDataInfo(_data.id);
        //if (info.State == ActivityState.HASENDED)
        //        DestroyImmediate(this.gameObject);
    }
}
