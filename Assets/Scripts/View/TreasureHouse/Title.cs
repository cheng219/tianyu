//===============================
//作者：鲁家旗
//日期：2016/4/15
//用途：藏宝库预览界面里的宝物等级
//===============================
using UnityEngine;
using System.Collections;

public class Title : MonoBehaviour {

    public UILabel titleLable;

    public void SetTitle(RewardGroupRef _data)
    {
        if (titleLable != null) titleLable.text = _data.name;
    }

}
