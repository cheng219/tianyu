//===================
//作者：鲁家旗
//日期：2016/4/15
//用途：藏宝库抽奖界面
//===================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class TreasureLotteryWnd : SubWnd
{
    #region 数据
    //奖励界面快捷抽取一次
    public UIButton extractOne;
    public UIButton extractTen;
    public UIButton extractFifty;
    public UILabel extractOneLabel;
    public UILabel extractTenLabel;
    public UILabel extractFiftyLabel;
    public UILabel keyLabel;
    public UILabel goldLabel;
    public GameObject itemGo;
    public UIPanel panel;

    protected FDictionary treasureHouseDic = null;
    protected List<GameObject> itemGoList = new List<GameObject>();
    protected TreasureHouseRef treasureRef = null;
    public GameObject effectGo;
    public UILabel coinLabel;
    public GameObject coinGo;

    public List<GameObject> items = new List<GameObject>();
    public GameObject fxAfterOpen;
    #endregion

    void OnEnable()
    {
        if (extractOne != null) EventDelegate.Add(extractOne.onClick, OnClickExtractOne);
        if (extractTen != null) EventDelegate.Add(extractTen.onClick, OnClickExtractTen);
        if (extractFifty != null) EventDelegate.Add(extractFifty.onClick, OnClickExtractFifty);
        treasureHouseDic = ConfigMng.Instance.GetTreasureHouseRefTable();
        if(itemGo != null) itemGo.SetActive(false);
        if (fxAfterOpen != null) fxAfterOpen.SetActive(false);
    }
    void OnDisable()
    {
        if (extractOne != null) EventDelegate.Remove(extractOne.onClick, OnClickExtractOne);
        if (extractTen != null) EventDelegate.Remove(extractTen.onClick, OnClickExtractTen);
        if (extractFifty != null) EventDelegate.Remove(extractFifty.onClick, OnClickExtractFifty);
    }
    /// <summary>
    /// 打开窗口
    /// </summary>
    protected override void OnOpen()
    {
        base.OnOpen();
        PlayEffect();
        //CreateItem();
        Refresh();
    }
    /// <summary>
    /// 关闭窗口
    /// </summary>
    protected override void OnClose()
    {
        base.OnClose();
    }
    //抽取按钮
    void OnClickExtractOne()
    {
        if (GameCenter.inventoryMng.GetNumberByType(treasureRef.keyID) >= 1 || GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount >= (ulong)treasureRef.extract1)
        {
            GameCenter.treasureHouseMng.C2S_ReqGetTreasure(GetTreasureType.GETTREASUREONE);
        }
        else
        {
            ToRecharge();
        }
    }
    void OnClickExtractTen()
    {
        if (GameCenter.inventoryMng.GetNumberByType(treasureRef.keyID) >= 10 || GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount >= (ulong)treasureRef.extract10)
        {
            GameCenter.treasureHouseMng.C2S_ReqGetTreasure(GetTreasureType.GETTREASURETEN);
        }
        else
        {
            ToRecharge();
        }
    }
    void OnClickExtractFifty()
    {
        if (GameCenter.inventoryMng.GetNumberByType(treasureRef.keyID) >= 50 || GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount >= (ulong)treasureRef.extract50)
        {
            GameCenter.treasureHouseMng.C2S_ReqGetTreasure(GetTreasureType.GETTREASUREFIFTY);
        }
        else
        {
            ToRecharge();
        }
    }
    void ToRecharge()
    {
        MessageST mst = new MessageST();
        mst.messID = 137;
        mst.delYes = delegate
        {
            GameCenter.uIMng.SwitchToUI(GUIType.RECHARGE);
        };
        GameCenter.messageMng.AddClientMsg(mst);
    }
    /// <summary>
    /// 播放特效后刷新物品
    /// </summary>
    void PlayEffect()
    {
        coinGo.SetActive(false);
        HideUipel();
        ReleaseGrid();
        NsEffectManager.RunReplayEffect(effectGo, true);
        if (fxAfterOpen != null)
        {
            if (GameCenter.treasureHouseMng.idList.Count >= 10)
                fxAfterOpen.transform.localPosition = Vector3.zero;
            else
                fxAfterOpen.transform.localPosition = new Vector3(210, -50, 0);
        }
        CancelInvoke("CreateItem");
        Invoke("CreateItem", 1.0f);
    }

    void CreateItem()
    {
        if (GameCenter.treasureHouseMng.idList.Count > 10)
        {
             CreateItemIfOver10();
        }
        else
        {
            CreateItemLessThan10();
        }
    }


    /// <summary>
    /// 奖励的物品大于10个放到uipel里面
    /// </summary>
    void CreateItemIfOver10()
    {
        if (fxAfterOpen != null)
        {
            fxAfterOpen.SetActive(false); 
        }
        coinGo.SetActive(true);
        for (int i = 0; i < itemGoList.Count; i++)
        {
            Destroy(itemGoList[i]);
        }
        itemGoList.Clear();
        panel.transform.localPosition = new Vector3(2, -115, 0);
        panel.clipOffset = new Vector2(0,0);
         //创建已抽取到的物品
        for (int i = 0, max = GameCenter.treasureHouseMng.idList.Count; i < max; i++)
        {
            GameObject go = GameObject.Instantiate(itemGo);
            go.transform.parent = itemGo.transform.parent;
            if (GameCenter.treasureHouseMng.idList.Count > 1)
                go.transform.localPosition = new Vector3(-260 + (i % 7) * 90,31 -(i / 7) * 90, 0);
            else go.transform.localPosition = new Vector3(0, 31, 0);
            go.transform.localScale = Vector3.one;
            go.GetComponent<ItemUI>().FillInfo(new EquipmentInfo(GameCenter.treasureHouseMng.idList[i],1, EquipmentBelongTo.PREVIEW));
            go.SetActive(true);
            itemGoList.Add(go);
        }
    }

    /// <summary>
    /// 物品小于10的时候放到特效里面
    /// </summary>
    void CreateItemLessThan10()
    {
        HideUipel();
        if (fxAfterOpen != null) NsEffectManager.RunReplayEffect(fxAfterOpen, true); 
        coinGo.SetActive(true);
        ReleaseGrid();
      
        List<int> idList = GameCenter.treasureHouseMng.idList;

        for (int i = 0, max = items.Count; i < max; i++)
        {
            if (idList.Count > i)
            {
                items[i].SetActive(true);
                ItemUI itemUI = ItemUI.CreatNew(items[i].transform, Vector3.zero, Vector3.one);
                if (itemUI != null)
                    itemUI.FillInfo(new EquipmentInfo(idList[i], 1, EquipmentBelongTo.PREVIEW));
            }
            else
            {
                items[i].SetActive(false);
            }
        } 
    }
    /// <summary>
    /// 刷新
    /// </summary>
    void Refresh()
    {
        if (coinLabel != null) coinLabel.text = GameCenter.treasureHouseMng.coinNum.ToString();
        foreach (TreasureHouseRef data in treasureHouseDic.Values)
        {
            treasureRef = data;
            //抽取按钮上的需要元宝数量
            if (extractOneLabel != null) extractOneLabel.text = data.extract1.ToString();
            if (extractTenLabel != null) extractTenLabel.text = data.extract10.ToString();
            if (extractFiftyLabel != null) extractFiftyLabel.text = data.extract50.ToString();
            //钥匙
            if (keyLabel != null) keyLabel.text = GameCenter.inventoryMng.GetNumberByType(data.keyID).ToString();
            //元宝 
            if (goldLabel != null) goldLabel.text = GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount.ToString();
        }
    }


    void HideUipel()
    {
        for (int i = 0; i < itemGoList.Count; i++)
        {
            itemGoList[i].SetActive(false);
        }
    }

    void ReleaseGrid()
    {
        for (int i = 0, max = items.Count; i < max; i++)
        {
            items[i].SetActive(false);
            if (items[i].GetComponentInChildren<ItemUI>() != null)
                Destroy(items[i].GetComponentInChildren<ItemUI>().gameObject);
        }
    }
}

