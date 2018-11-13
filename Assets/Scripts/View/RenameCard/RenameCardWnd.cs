//===============================
//作者：唐源
//日期：2017/3/31
//用途：改名卡界面类
//===============================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class RenameCardWnd : GUIBase {
    #region 字段(系统生成的随机数)
    private System.Random random = new System.Random();
    #endregion
    #region UI控件
    /// <summary>
    /// 关闭改名卡输入框
    /// </summary>
    public UIInput inputName;
    /// <summary>
    /// 新名字
    /// </summary>
    public UILabel newName;
    /// <summary>
    /// 关闭按钮
    /// </summary>
    public UIButton closeBtn;
    /// <summary>
    /// 随机名字按钮
    /// </summary>
    public UIButton randomBtn;
    /// <summary>
    /// 确定按钮
    /// </summary>
    public UIButton confirmBtn;
    #endregion
    void Awake()
    {
        if(closeBtn!=null)
        {
            UIEventListener.Get(closeBtn.gameObject).onClick = CloseWnd;
        }
        if(randomBtn!=null)
        {
            UIEventListener.Get(randomBtn.gameObject).onClick = RandName;
        }
        if(confirmBtn!=null)
        {
            UIEventListener.Get(confirmBtn.gameObject).onClick = Confirm;
        }
    }

    protected override void OnOpen()
    {
        base.OnOpen();
        RandName(null);
    }

    #region 控件事件的响应
    /// <summary>
    /// 关闭改名卡窗口
    /// </summary>
    void CloseWnd(GameObject _obj)
    {
        GameCenter.uIMng.ReleaseGUI(GUIType.RENAMECARD);
    }
    /// <summary>
    /// 随机名字
    /// </summary>
    void RandName(GameObject _obj)
    {
        int curProf = GameCenter.mainPlayerMng.MainPlayerInfo.Prof;
        int profCount = ConfigMng.Instance.ProfNameCount(curProf);
        int nameCount = ConfigMng.Instance.GetNameRefTable().Count;
        if (profCount <= 0 || nameCount <= 0)
        {
            Debug.LogError("NameConfig配表数据不对，职业" + curProf + "的名字或者性配表长度为0，去找左文祥");
            return;
        }
        int profId = random.Next(1, profCount);
        int nameId = random.Next(1, nameCount);
        NameRef nameRef = ConfigMng.Instance.GetNameRef(nameId);
        string name = nameRef != null ? nameRef.Firstname : string.Empty;
        nameRef = ConfigMng.Instance.GetNameRef(profId);
        name += (nameRef != null && nameRef.names.Count >= curProf) ? nameRef.names[curProf - 1] : string.Empty;
        inputName.value = name;
    }
    /// <summary>
    /// 确定改名
    /// </summary>
    void Confirm(GameObject _obj)
    {
        if (GameCenter.inventoryMng.InstanceID != 0)
        GameCenter.inventoryMng.C2S_UseRenameCard(GameCenter.inventoryMng.InstanceID, inputName.value);
    }
    #endregion
    #region 界面的刷新与显示
    void Refresh()
    {

    }
    #endregion
}
