//===============================
//日期：2016/3/24
//作者：鲁家旗
//用途描述:翅膀窗口界面
//===============================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WingWnd : SubWnd
{
    #region 控件数据
    /// <summary>
    /// 未激活界面
    /// </summary>
    public GameObject notActiveGo;
    /// <summary>
    /// 激活界面
    /// </summary>
    public GameObject ActiveGo;
    /// <summary>
    /// 翅膀数据
    /// </summary>
    FDictionary WingDicton
    {
        get
        {
            return GameCenter.wingMng.WingDic;
        }
    }
    /// <summary>
    /// 翅膀预制
    /// </summary>
    public GameObject wingGo;
    protected List<WingItemUI> listGame = new List<WingItemUI>();
    /// <summary>
    /// 未激活界面脚本
    /// </summary>
    public WingNotActiveUI wingNotActive;
    /// <summary>
    /// 激活界面脚本
    /// </summary>
    public WingActiveUI wingActive;
    /// <summary>
    /// 用来创建翅膀的数据
    /// </summary>
    protected Dictionary<int, WingRef> DataDic 
    {
        get
        {
            return ConfigMng.Instance.GetWingRefTable();
        }
    }
    /// <summary>
    /// 当前选中翅膀的数据
    /// </summary>
    protected WingInfo curWingInfo = null;
    public UITexture activeWing;
    public UITexture notActiveWing;
    #endregion

    #region 构造
    //打开窗口的时候
    protected override void OnOpen()
    {
        base.OnOpen();
        CreateWing();
        ChooseToggle();
        OnChangeToggle(null);
        for (int i = 0; i < listGame.Count; i++)
        {
            UIEventListener.Get(listGame[i].gameObject).onClick += OnChangeToggle;
        }
        GameCenter.wingMng.OnWingAdd += OnChangeToggle;
        GameCenter.wingMng.OnWingUpdate += OnWingUpdate;
    }
    //直接跳转到选中的翅膀界面
    void ChooseToggle()
    {
        for (int i = 0; i < listGame.Count; i++)
        {
            if (GameCenter.wingMng.CurUseWingInfo != null && GameCenter.wingMng.CurUseWingInfo.WingType != 2)
            {
                if (listGame[i].Info.id == GameCenter.wingMng.CurUseWingInfo.WingId)
                {
                    listGame[i].GetComponent<UIToggle>().value = true;
                }
            }
            else
                listGame[0].GetComponent<UIToggle>().value = true;
        }
    }
    //关闭窗口的时候
    protected override void OnClose()
    {
        base.OnClose();
        for (int i = 0; i < listGame.Count; i++)
        {
            UIEventListener.Get(listGame[i].gameObject).onClick -= OnChangeToggle;
        }
        GameCenter.wingMng.OnWingAdd -= OnChangeToggle;
        GameCenter.wingMng.OnWingUpdate -= OnWingUpdate;
        GameCenter.previewManager.ClearModel();
    }
    #endregion

    #region 刷新
    /// <summary>
    /// 动态创建翅膀
    /// </summary>
    void CreateWing()
    {
        int index = 0;
        if (wingGo != null) wingGo.SetActive(true);
        for (int i = 0; i < listGame.Count; i++)
        {
            DestroyImmediate(listGame[i].gameObject);
        }
        listGame.Clear();
        using (var data = DataDic.GetEnumerator())
        {
            while (data.MoveNext())
            {
                GameObject go = GameObject.Instantiate(wingGo);
                go.transform.parent = wingGo.transform.parent;
                go.transform.localScale = Vector3.one;
                go.transform.localPosition = new Vector3(-316 + 125 * index, 206, 0);
                //默认是选中第一个翅膀
                //if (index == 0) go.GetComponent<UIToggle>().startsActive = true;
                //刷新翅膀toggle
                WingItemUI ui = go.GetComponent<WingItemUI>();
                if (ui != null)
                {
                    ui.ShowWingInfo(data.Current.Value);
                    listGame.Add(ui);
                }
                index++;
            }
        }
        if (wingGo != null) wingGo.SetActive(false);
    }
    /// <summary>
    /// 切换翅膀
    /// </summary>
    void OnChangeToggle(GameObject go)
    {
        for (int i = 0; i < listGame.Count; i++)
        {
            if (listGame[i] != null && listGame[i].GetComponent<UIToggle>().value)
            {
                //刷新激活界面
                WingItemUI wingItemUI = listGame[i].GetComponent<WingItemUI>();
                WingRef info = null;
                if (wingItemUI != null)
                    info = wingItemUI.Info;
                if (info != null && WingDicton.ContainsKey(info.id))
                {
                    wingActive.SetWingActiveUI(WingDicton[info.id] as WingInfo);
                    if (notActiveGo != null)
                        notActiveGo.SetActive(false);
                    if (ActiveGo != null)
                        ActiveGo.SetActive(true);
                    curWingInfo = WingDicton[info.id] as WingInfo;
                    RefreshModel(activeWing);
                }
                //刷新未激活界面
                else if (info != null)
                {
                    wingNotActive.SetWingNotActiveUI(info);
                    if (notActiveGo != null)
                        notActiveGo.SetActive(true);
                    if (ActiveGo != null)
                        ActiveGo.SetActive(false);
                    curWingInfo = new WingInfo(info);
                    RefreshModel(notActiveWing);
                }
            }
        }
    }
    /// <summary>
    ///预览穿上翅膀的模型
    /// </summary>
    void RefreshModel(UITexture _tex)
    {
        if (curWingInfo == null) return;
        EquipmentInfo wingEquip = new EquipmentInfo(curWingInfo.WingItemId, EquipmentBelongTo.PREVIEW);
        Dictionary<EquipSlot, EquipmentInfo> showEquipDic = new Dictionary<EquipSlot, EquipmentInfo>(GameCenter.mainPlayerMng.MainPlayerInfo.CurShowDictionary);
        showEquipDic[wingEquip.Slot] = wingEquip;
        GameCenter.previewManager.TryPreviewSinglePlayer(GameCenter.mainPlayerMng.MainPlayerInfo, _tex, showEquipDic);
    }
    /// <summary>
    /// 淬炼后刷新
    /// </summary>
    void OnWingUpdate()
    {
        for (int i = 0; i < listGame.Count; i++)
        {
            if (listGame[i] != null && listGame[i].GetComponent<UIToggle>().value)
            { 
                WingRef info = listGame[i].GetComponent<WingItemUI>().Info;
                if (info != null && WingDicton.ContainsKey(info.id))
                {
                    wingActive.SetWingActiveUI(WingDicton[info.id] as WingInfo);
                    curWingInfo = WingDicton[info.id] as WingInfo;
                    if (GameCenter.wingMng.isRefreshEffect)
                    {
                        GameCenter.wingMng.isRefreshEffect = false;
                        RefreshModel(activeWing);
                    }
                }
            }
        }
    }
    #endregion
}
