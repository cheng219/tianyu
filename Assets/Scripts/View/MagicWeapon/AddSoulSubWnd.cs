//======================================================
//作者:鲁家旗
//日期:2016.7.29
//用途:注灵二级界面
//======================================================
using UnityEngine;
using System.Collections;

public class AddSoulSubWnd : SubWnd {

    public GameObject magicAttributeUI;
    protected override void OnOpen()
    {
        base.OnOpen();
        if (magicAttributeUI != null) magicAttributeUI.SetActive(true);
    }
    protected override void OnClose()
    {
        base.OnClose();
        if (magicAttributeUI != null) magicAttributeUI.SetActive(false);
    }
}
