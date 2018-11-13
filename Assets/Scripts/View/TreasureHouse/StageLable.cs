//===============================
//作者：鲁家旗
//日期：2016/4/15
//用途：藏宝库预览界面里的宝物类型
//===============================
using UnityEngine;
using System.Collections;

public class StageLable : MonoBehaviour {

    public UILabel stageLable;

    public void SetStageLable(RewardGroupMemberRef _data)
    {
        if (_data.name == "0" && stageLable != null)
        {
            stageLable.text = "";
        }
        else
        {
            stageLable.text = _data.name;
        }
    }
}
