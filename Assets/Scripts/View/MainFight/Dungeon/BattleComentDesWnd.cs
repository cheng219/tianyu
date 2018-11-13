//======================================================
//作者:朱素云
//日期:2017/3/6
//用途:火焰山评分说明界面
//======================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleComentDesWnd : GUIBase
{
    public UIButton closeBtn;
    public BattleComentUi battleComentUi;
    protected Dictionary<int, BattleComentUi> allItems = new Dictionary<int, BattleComentUi>();
    public UIGrid grid; 

    void Awake()
    {
        mutualExclusion = true;
        Layer = GUIZLayer.TOPWINDOW;
        if (closeBtn != null) UIEventListener.Get(closeBtn.gameObject).onClick = delegate
            {
                GameCenter.uIMng.ReleaseGUI(GUIType.BATTLECOMENTDES);
            };
    }

    protected override void OnOpen()
    {
        base.OnOpen();
        Show();
    }

    protected override void OnClose()
    {
        base.OnClose();
    }

    void Show()
    {
        FDictionary battleFieldRefTable = ConfigMng.Instance.GetBattleFieldRefTable();
        foreach (BattleFieldRef battleFieldRef in battleFieldRefTable.Values)
        {
            BattleComentUi item = null;
            int id = battleFieldRef.id;
            if (!allItems.TryGetValue(id, out item))
            {
                if (grid != null) item = battleComentUi.CreateNew(grid.transform);
                allItems[id] = item;
            }
            item = allItems[id];
            if (item != null)
            {
                item.battleFieldRef = battleFieldRef;
                item.gameObject.SetActive(true);
            }
        }
        if (grid != null) grid.repositionNow = true;
    }
}
