//==============================================
//作者：邓成
//日期：2016/10/14
//用途：开启宝箱界面
//=================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoxShowWnd : GUIBase 
{
    /// <summary>
    /// 物品排序
    /// </summary>
    public UIExGrid grid;
    /// <summary>
    /// 确定按钮
    /// </summary>
    public UIButton buttonOk;
    void Awake()
    {
        mutualExclusion = false;
        layer = GUIZLayer.COVER;
    }

	void Start () {

        AddItem();
        if (buttonOk != null) UIEventListener.Get(buttonOk.gameObject).onClick -= OnClickBut;
        if (buttonOk != null) UIEventListener.Get(buttonOk.gameObject).onClick += OnClickBut;
	}

    /// <summary>
    /// 初始化添加物品
    /// </summary>
    void AddItem()
    {
        int index = 0;
        if (grid != null)
        {
            //Debug.Log("数量为"+GameCenter.inventoryMng.BoxGotItems.Count);
			for (int i = 0,max=GameCenter.inventoryMng.BoxGotItems.Count; i < max; i++)
            {
                ItemUI go = ItemUI.CreatNew(grid, index);
                go.FillInfo(GameCenter.inventoryMng.BoxGotItems[i]);
                index++;
            }
        }
    }
    /// <summary>
    /// 点击确定按钮
    /// </summary>
    /// <param name="obj"></param>
    void OnClickBut(GameObject obj)
    {
		GameCenter.uIMng.ReleaseGUI(GUIType.BOXREWARD);
    }
	
}
