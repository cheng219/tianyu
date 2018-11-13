//===================
//作者：鲁家旗
//日期：2016/4/15
//用途：藏宝库主界面
//===================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class TreasureHouseWnd : GUIBase
{
    #region 控件数据
    public UIButton closeBtn;
    public UIButton previewBtn;
    public UIButton warehouseBtn;

    public UIButton extractOne;
    public UIButton extract;
    public UIButton extractTen;
    public UIButton extractFifty;
    public UILabel extractOneLabel;
    public UILabel extractTenLabel;
    public UILabel extractFiftyLabel;
    public UILabel keyLabel;
    public UILabel goldLabel;

    public UIButton closePreviewBtn;
    public UIButton closeWarehouseBtn;
    public UIButton closeRewardBtn;
    public GameObject item;
    public UIGrid rewardDrid;
    public List<RewardRecord> RewardRecordList= new List<RewardRecord>();

    //小奖励
    public List<ItemUI> smallItems = new List<ItemUI>();
    //大奖励
    public List<ItemUI> bigItems = new List<ItemUI>();

    ////获奖名单 
    public UILabel[] rewardLabel;
    public UILabel[] cdLabel;
    //抽到道具
    public UILabel[] goodsLabel;
    protected FDictionary treasureHouseDic = null;
    //玩家名
    List<string> playerName
    {
        get
        {
            return GameCenter.treasureHouseMng.playerName;
        }
    }
    //玩家抽取到的道具
    List<EquipmentInfo> goodsList
    {
        get
        {
            return GameCenter.treasureHouseMng.goodsList;
        }
    }
    protected float len = 0;
    protected float china_len = 0;
    protected float english_len = 0;
    protected TreasureHouseRef treasureRef = null;
    public UISprite redPoint;
    public GameObject halfPrice;
    #endregion

    #region 构造
    void Awake()
    {
        layer = GUIZLayer.NORMALWINDOW;
        mutualExclusion = true;
        treasureHouseDic = ConfigMng.Instance.GetTreasureHouseRefTable();
    }
    void OnEnable()
    {
        //关闭主界面
        if (closeBtn != null) UIEventListener.Get(closeBtn.gameObject).onClick = delegate
        {
            GameCenter.uIMng.SwitchToUI(GUIType.NONE);
        };
        //跳到预览界面
        if (previewBtn != null)  UIEventListener.Get(previewBtn.gameObject).onClick = delegate
        {
            SwitchToSubWnd(SubGUIType.PREVIEWREWAD);
        };
        //打开临时仓库
        if (warehouseBtn != null) UIEventListener.Get(warehouseBtn.gameObject).onClick = delegate
        {
            SwitchToSubWnd(SubGUIType.WAREHOUSE);
            //请求临时仓库信息，发送请求协议
            GameCenter.treasureHouseMng.C2S_ReqGetHouse();
        };
        //关闭预览界面
        if (closePreviewBtn != null) UIEventListener.Get(closePreviewBtn.gameObject).onClick = OnClickCloseBtn;
        //关闭仓库界面
        if (closeWarehouseBtn != null) UIEventListener.Get(closeWarehouseBtn.gameObject).onClick = OnClickCloseBtn;
        //关闭奖励界面
        if (closeRewardBtn != null) UIEventListener.Get(closeRewardBtn.gameObject).onClick = OnClickCloseBtn;
        //抽取
        if (extractOne != null) EventDelegate.Add(extractOne.onClick, OnClickExtractOneBtn);
        if(extract!=null) EventDelegate.Add(extract.onClick, OnClickExtractBtn);
        if (extractTen != null) EventDelegate.Add(extractTen.onClick, OnClickExtractTenBtn);
        if (extractFifty != null) EventDelegate.Add(extractFifty.onClick, OnClickExtractFiftyBtn);
    }
    void OnDisable()
    {
        if (extractOne != null) EventDelegate.Remove(extractOne.onClick, OnClickExtractOneBtn);
        if (extractTen != null) EventDelegate.Remove(extractTen.onClick, OnClickExtractTenBtn);
        if (extractFifty != null) EventDelegate.Remove(extractFifty.onClick, OnClickExtractFiftyBtn);
        if (closePreviewBtn != null) UIEventListener.Get(closePreviewBtn.gameObject).onClick -= OnClickCloseBtn;
        if (closeWarehouseBtn != null) UIEventListener.Get(closeWarehouseBtn.gameObject).onClick -= OnClickCloseBtn;
        if (closeRewardBtn != null) UIEventListener.Get(closeRewardBtn.gameObject).onClick -= OnClickCloseBtn;
    }
    protected override void OnOpen()
    {
        base.OnOpen();
        GameCenter.treasureHouseMng.C2S_ReqTreasureRecord();
        Refresh();
        //RefreshRecord();
        RefreshRed();
        RefreshReWardRecord();
        //SetExtract();
    }
    protected override void OnClose()
    {
        base.OnClose();
        Destroyitem();
        RewardRecordList.Clear();
    }
    protected override void HandEvent(bool _bind)
    {
        base.HandEvent(_bind);
        if (_bind)
        {
            GameCenter.treasureHouseMng.OnTreasureUpdate += Refresh;
            GameCenter.treasureHouseMng.OnTreasureUpdate += ToReward;
            GameCenter.treasureHouseMng.OnRecordUpdate += RefreshReWardRecord;
            GameCenter.treasureHouseMng.OnStorageChangeUpdate += RefreshRed;
        }
        else
        {
            GameCenter.treasureHouseMng.OnTreasureUpdate -= Refresh;
            GameCenter.treasureHouseMng.OnTreasureUpdate -= ToReward;
            GameCenter.treasureHouseMng.OnRecordUpdate -= RefreshReWardRecord;
            GameCenter.treasureHouseMng.OnStorageChangeUpdate -= RefreshRed;
        }
    }
    #endregion

    #region 控件事件
    //关闭子界面 
    void OnClickCloseBtn(GameObject obj)
    {
        SwitchToSubWnd(SubGUIType.NONE);
    }
    /// <summary>
    /// 免费抽取一次
    /// </summary>
    void OnClickExtractBtn()
    {
        if(GameCenter.treasureHouseMng.IsCanFree)
        {
            GameCenter.treasureHouseMng.C2S_ReqGetTreasure(GetTreasureType.GETTREASUREONE);
        }
    }
    /// <summary>
    /// 抽取一次
    /// </summary>
    void OnClickExtractOneBtn()
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
    /// <summary>
    /// 抽取十次
    /// </summary>
    void OnClickExtractTenBtn()
    {
        //int keyNum = GameCenter.inventoryMng.GetNumberByType(treasureRef.keyID);
        int needDiamond = (int)((10 /*- keyNum*/) *(18.8));
        if (GameCenter.treasureHouseMng.IsCanHalfPrice)
            needDiamond = (int)Mathf.Floor(needDiamond * 0.5f);//向下取整
        //抽取十次消耗元宝188或者10个钥匙（当钥匙小于10个，购买10份需要消耗到元宝时给予提示）
        //注释了钥匙这个判断条件十连抽不能用钥匙了(不知道怎么想的感觉迟早又要改回来)
        if (/*keyNum < 10 && keyNum >= 1 && */GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount >= (ulong)needDiamond)
        {
            GameCenter.treasureHouseMng.C2S_ReqGetTreasure(GetTreasureType.GETTREASURETEN);
            //MessageST mst = new MessageST();
            //mst.messID = 492;
            //mst.words = new string[2] { (10).ToString(), /*(keyNum).ToString(), */needDiamond.ToString()};
            //mst.delYes = delegate
            //{
            //    GameCenter.treasureHouseMng.C2S_ReqGetTreasure(GetTreasureType.GETTREASURETEN);
            //};
            //GameCenter.messageMng.AddClientMsg(mst);
        }
        else if (/*keyNum >= 10 || */GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount >= (ulong)treasureRef.extract10)
        {
            GameCenter.treasureHouseMng.C2S_ReqGetTreasure(GetTreasureType.GETTREASURETEN);
        }
        else
        {
            ToRecharge();
        }
    }
    /// <summary>
    /// 抽取五十次
    /// </summary>
    void OnClickExtractFiftyBtn()
    {
        int keyNum = GameCenter.inventoryMng.GetNumberByType(treasureRef.keyID);
        int needDiamond = (int)((50 - keyNum) *(17.76));
        //抽取五十次消耗元宝888或者50个钥匙（当钥匙小于50个，购买50份需要消耗到元宝时给予提示）
        if (keyNum < 50 && keyNum >= 1 && GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount >= (ulong)needDiamond)
        {
            MessageST mst = new MessageST();
            mst.messID = 492;
            mst.words = new string[3] { (50).ToString(), (keyNum).ToString(), needDiamond.ToString() };
            mst.delYes = delegate
            {
                GameCenter.treasureHouseMng.C2S_ReqGetTreasure(GetTreasureType.GETTREASUREFIFTY);
            };
            GameCenter.messageMng.AddClientMsg(mst);
        }
        else if (GameCenter.inventoryMng.GetNumberByType(treasureRef.keyID) >= 50 || GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount >= (ulong)treasureRef.extract50)
        {
            GameCenter.treasureHouseMng.C2S_ReqGetTreasure(GetTreasureType.GETTREASUREFIFTY);
        }
        else
        {
            ToRecharge();
        }
    }
    /// <summary>
    /// 跳转到充值界面
    /// </summary>
    void ToRecharge()
    {
        //提示
        MessageST mst = new MessageST();
        mst.messID = 137;
        mst.delYes = delegate
        {
            // 跳转到充值界面
            GameCenter.uIMng.SwitchToUI(GUIType.RECHARGE);
        };
        GameCenter.messageMng.AddClientMsg(mst);
    }
    /// <summary>
    /// 跳转到抽奖界面
    /// </summary>
    void ToReward()
    {
        if (GameCenter.treasureHouseMng.idList.Count != 0)
            SwitchToSubWnd(SubGUIType.REWARD);
    }
    #endregion

    #region 刷新
    void Refresh()
    {
        SetExtract();
        foreach (TreasureHouseRef data in treasureHouseDic.Values)
        {
            treasureRef = data;
            for (int i = 0; i < smallItems.Count; i++)
            {
                if (i < data.smallAward.Count)
                    smallItems[i].FillInfo(new EquipmentInfo(data.smallAward[i], EquipmentBelongTo.PREVIEW));
            }
            int prof = GameCenter.mainPlayerMng.MainPlayerInfo.Prof;
            for (int i = 0; i < bigItems.Count; i++)
            {
                if (i < data.bigAward.Count && prof == 1)//战士
                {
                    bigItems[i].FillInfo(new EquipmentInfo(data.bigAward[i], EquipmentBelongTo.PREVIEW));
                }
                else if (i < data.bigAward1.Count && prof == 2)//法师
                {
                    bigItems[i].FillInfo(new EquipmentInfo(data.bigAward1[i], EquipmentBelongTo.PREVIEW));
                }
                else if (i < data.bigAward2.Count && prof == 3)//刺客
                {
                    bigItems[i].FillInfo(new EquipmentInfo(data.bigAward2[i], EquipmentBelongTo.PREVIEW));
                }
            }
            if(extractOneLabel != null) extractOneLabel.text = data.extract1.ToString();
            if(extractTenLabel != null)extractTenLabel.text = data.extract10.ToString();
            if(extractFiftyLabel != null)extractFiftyLabel.text = data.extract50.ToString();
            //钥匙
            if(keyLabel != null) keyLabel.text = GameCenter.inventoryMng.GetNumberByType(data.keyID).ToString();
            //元宝
            if (goldLabel != null) goldLabel.text = GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount.ToString();
        }
        if (redPoint != null)
            redPoint.gameObject.SetActive(GameCenter.treasureHouseMng.isRed);
    }
    /// <summary>
    /// 刷新抽奖记录
    /// </summary>
    void RefreshRecord()
    {
        for (int i = 0; i < rewardLabel.Length; i++)
        {
            if (i < goodsList.Count)
            {
                rewardLabel[i].text = "[u]" + playerName[playerName.Count -1-i];
                len = 0;
                china_len = 0;
                english_len = 0;
                for (int j = 0; j < playerName[playerName.Count - 1 - i].Length; j++)
                {
                    byte[] byte_len = Encoding.Default.GetBytes(playerName[playerName.Count - 1 - i].Substring(j,1));
                    if (byte_len.Length > 1)
                        china_len += 1;
                    else
                        english_len += 0.5f;  
                }
                //english_len = english_len / 2.0f;
                len = china_len + english_len;
                if (cdLabel[i] != null)
                {
                    cdLabel[i].text = ConfigMng.Instance.GetUItext(22);
                    cdLabel[i].transform.localPosition = new Vector3(rewardLabel[i].transform.localPosition.x + len * rewardLabel[i].fontSize, rewardLabel[i].transform.localPosition.y, 0);
                }

                if (goodsLabel[i] != null)
                {
                    goodsLabel[i].text = "[u]" + goodsList[goodsList.Count - 1 - i].ItemStrColor + goodsList[goodsList.Count - 1 - i].ItemName;
                    goodsLabel[i].transform.localPosition = new Vector3(cdLabel[i].transform.localPosition.x + cdLabel[i].text.Length * cdLabel[i].fontSize, cdLabel[i].transform.localPosition.y, 0);
                }
            }
            else
            {
                if (rewardLabel[i] != null) rewardLabel[i].text = "";
                if (cdLabel[i] != null) cdLabel[i].text = "";
                if (goodsLabel[i]) goodsLabel[i].text = "";
            }
        }
        //点击抽奖人物名和道具
        for (int i = 0; i < rewardLabel.Length; i++)
        {
            if (goodsLabel[i] != null && i < goodsList.Count)
            {
                UIButton goodBtn = goodsLabel[i].GetComponent<UIButton>();
                UIEventListener.Get(goodBtn.gameObject).onClick -= OnClickGoods;
                UIEventListener.Get(goodBtn.gameObject).onClick += OnClickGoods;
                UIEventListener.Get(goodBtn.gameObject).parameter = goodsList[goodsList.Count - 1 - i].EID;
            }
            if (rewardLabel[i] != null && i < playerName.Count)
            {
                UIButton nameBtn = rewardLabel[i].GetComponent<UIButton>();
                UIEventListener.Get(nameBtn.gameObject).onClick -= OnClickName;
                UIEventListener.Get(nameBtn.gameObject).onClick += OnClickName;
                UIEventListener.Get(nameBtn.gameObject).parameter = playerName.Count - 1 - i;
            }
        }
    }
    /// <summary>
    /// 刷新抽奖记录
    /// </summary>
    void RefreshReWardRecord()
    {
        SetExtract();
        //Debug.Log("打印刷新抽奖记录");
        //Debug.Log("goodsList.Count"+ goodsList.Count);
        Destroyitem();
        if(item!=null)
        {
            for (int i = 0; i < goodsList.Count; i++)
            {
                //Debug.Log("实例化预制");
                GameObject go = GameObject.Instantiate(item);
                go.transform.parent = item.transform.parent;
                //go.transform.localPosition =  Vector3.zero;
                go.transform.localScale = Vector3.one;
                go.gameObject.SetActive(true);
                RewardRecord record = go.GetComponent<RewardRecord>();
                string str1 = GameCenter.treasureHouseMng.playerName[i];
                string str2 = goodsList[i].ItemStrColor + goodsList[i].ItemName;
                int[] arrayId = { i, goodsList[i].EID };
                //Debug.Log("玩家的名字：=  " + GameCenter.treasureHouseMng.playerName[i]);
                //string str2 = ConfigMng.Instance.GetUItext(22)
                if(arrayId!=null&&!string.IsNullOrEmpty(str1)&& !string.IsNullOrEmpty(str2))
                {
                    record.FillInfo(str1, str2, arrayId);
                    //Debug.Log("str1:=     " + str1);
                    RewardRecordList.Add(record);
                }
            }
            if (rewardDrid != null)
                rewardDrid.Reposition();
        }
        else
        {
            Debug.LogError("名为 item 的预制为空");
        }
        //rewardDrid.Reposition();
    }
    /// <summary>
    /// 点击玩家名后
    /// </summary>
    void OnClickName(GameObject obj)
    {
        int id = (int)UIEventListener.Get(obj).parameter;
        GameCenter.previewManager.C2S_AskOPCPreview(GameCenter.treasureHouseMng.playerId[id]);
    }
    /// <summary>
    /// 点击抽到道具后
    /// </summary>
    void OnClickGoods(GameObject obj)
    {
        int id = (int)UIEventListener.Get(obj).parameter;
        ToolTipMng.ShowEquipmentTooltip(new EquipmentInfo(id, EquipmentBelongTo.PREVIEW), ItemActionType.None, ItemActionType.None, ItemActionType.None, ItemActionType.None);
    }

    /// <summary>
    /// 临时仓库红点
    /// </summary>
    void RefreshRed()
    {
        if (redPoint != null)
            redPoint.gameObject.SetActive(GameCenter.treasureHouseMng.isRed);
    }
    /// <summary>
    /// 销毁预制
    /// </summary>
    void Destroyitem()
    {
        for (int i = 0; i < RewardRecordList.Count; i++)
        { 
            //Debug.Log("删除预制");
            rewardDrid.RemoveChild(RewardRecordList[i].transform);
        }
    }
        /// <summary>
    /// 设置是否可以免费抽取
    /// </summary>
    void SetExtract()
    {
        //Debug.Log("设置按钮");
        if (extract != null && extractOne != null)
        {
            //Debug.Log("GameCenter.treasureHouseMng.isCanFree:=  "+GameCenter.treasureHouseMng.isCanFree);
            extract.gameObject.SetActive(GameCenter.treasureHouseMng.IsCanFree);
            extractOne.gameObject.SetActive(!GameCenter.treasureHouseMng.IsCanFree);
        }
        if (halfPrice != null)
        {
            if (GameCenter.treasureHouseMng.IsCanHalfPrice)
            {
                halfPrice.SetActive(true);
            }
            else
            {
                halfPrice.SetActive(false);
            }
        }
    }
    #endregion
}

