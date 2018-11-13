//==================================
//作者：朱素云
//日期：2016/4/13
//用途：送花界面
//=================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SendFlowerWnd : GUIBase
{
    public UIButton closeBtn;
    public List<Flower> list = new List<Flower>();

    void Awake()
    {
		mutualExclusion = true;//改成true了,解决bug:点击花的详情描述,在点商城购买跳转商城 此界面没关  by邓成
        layer = GUIZLayer.TIP;
        if (closeBtn != null) UIEventListener.Get(closeBtn.gameObject).onClick = OnCloseWnd;
    }
    protected override void OnOpen()
    {
        base.OnOpen();
        Show();
        GameCenter.friendsMng.OnSendFlowerToSomeone += Show;
        GameCenter.inventoryMng.OnBackpackUpdate += Show;
    }
    protected override void OnClose()
    {
        base.OnClose();
        GameCenter.friendsMng.OnSendFlowerToSomeone -= Show;
        GameCenter.inventoryMng.OnBackpackUpdate -= Show;
    }
    void OnCloseWnd(GameObject go)
    {
        GameCenter.uIMng.ReleaseGUI(GUIType.SENDFLOWER);
    }
    void Show()
    {
        for (int i = 0; i < list.Count; i++)
        {
            SpecialRef specail = ConfigMng.Instance.GetSpecialRef(1);
            if (specail != null)
            {
                int flowerId = specail.flowers[i];
                int someOneId = GameCenter.friendsMng.sendFlowerToOne;
                int type = GameCenter.friendsMng.SendFlowerType;
                FlowerData data = new FlowerData(flowerId, someOneId, type);
                list[i].FlowerData = data;
            }
        }
    }
}
