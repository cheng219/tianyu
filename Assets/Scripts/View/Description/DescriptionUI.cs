//==========================
//作者：鲁家旗
//日期：2016/4/25
//用途：说明系统
//==========================
using UnityEngine;
using System.Collections;

public class DescriptionUI : MonoBehaviour {
    protected UIButton desBtn;
    public int desId;
    void Awake()
    {
        desBtn = this.GetComponent<UIButton>();
    }
    void Start()
    {
        if (desBtn != null) UIEventListener.Get(desBtn.gameObject).onClick = delegate
        {
            GameCenter.uIMng.GenGUI(GUIType.DESCRIPTION, true);
            GameCenter.descriptionMng.OpenDes(desId);
        };
    }
}
