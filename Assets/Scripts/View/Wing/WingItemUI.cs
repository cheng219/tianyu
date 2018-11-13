//==================
//日期：2016/3/24
//作者：鲁家旗
//用途描述:翅膀页签
//==================
using UnityEngine;
using System.Collections;

public class WingItemUI : MonoBehaviour {

    public UILabel nameLabel;

    WingRef info;
    public WingRef Info 
    {
        get
        {
            return info;
        }
    }
    
    public void ShowWingInfo(WingRef _info)
    {
        info = _info;
        if(nameLabel != null) nameLabel.text = _info.name;
    }
}
