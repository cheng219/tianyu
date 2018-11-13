//==================================
//作者：朱素云
//日期：2016/3/29
//用途：坐骑界面
//=================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MountSubWnd : SubWnd
{
    #region 数据 
    public GameObject effect;
    public UIButton getMat;//获得材料至单人副本界面 
    public List<UILabel> curPropertyLab = new List<UILabel>();
    public List<UILabel> nextLevAddLab = new List<UILabel>();
    public List<UISpriteEx> starSp = new List<UISpriteEx>();
    public List<UIFxAutoActive> effects = new List<UIFxAutoActive>();
    public UILabel nameLab;//名字
    public UILabel levLab;//阶级
    public UILabel unlockLab;//解锁阶级
    public UILabel itemLab;//需要的/拥有的
    public UILabel coinLab;//金币
    public UILabel diamondLab;//元宝
    public UILabel chanceLab;//转化成功率
    public UITexture mountLookTex;//长相
    public UIToggle atuoBuy;//是否自动购买 
    //public UIButton leftBtn;//前一个
    //public UIButton rightBtn;//后一个
    public UIButton rideBtn;//骑上
    public UIButton disMountBtn;//乘下
    public UIButton illusionBtn;//化形
    public UIButton trainMountBtn;//坐骑培养 
    public UISpriteEx illusionEx;
    public UISpriteEx rideEx;
    public UISpriteEx disrideEx; 
    public Transform levest;//满级

    public GameObject effect4;
    public GameObject effect7;

    public UIButton activateMountBtn;//激活坐骑按钮
    private int len = 0;
    /// <summary>
    /// 坐骑链表
    /// </summary>
    protected List<MountRef> mountList
    {
        get
        {
            return GameCenter.newMountMng.MountList(1);
        }
    }
    /// <summary>
    /// 服务端数据，拥有的坐骑
    /// </summary>
    protected FDictionary mountInfoDic
    {
        get
        {
            return GameCenter.newMountMng.mountInfoList;
        }
    } 
    private int index = 0;//坐骑索引 
    protected int dex = 0;//拥有的第几个坐骑 
    protected int itemId;
    protected EquipmentRef itemRef;
    protected RidePropertyRef mountPropertyRefData = null;
    protected RidePropertyRef MountPropertyRefData
    {
        get
        {
            if (mountPropertyRefData == null || mountPropertyRefData.level != GameCenter.newMountMng.CurLev)
            {
                if (GameCenter.newMountMng.CurLev <= (ConfigMng.Instance.RidePropertyRefTable.Count - 1)) 
                    mountPropertyRefData = ConfigMng.Instance.GetMountPropertyRef(GameCenter.newMountMng.CurLev);
                else
                    mountPropertyRefData = ConfigMng.Instance.GetMountPropertyRef(ConfigMng.Instance.RidePropertyRefTable.Count - 1);
            }
            return mountPropertyRefData;
        }
    }
    protected MountInfo curInfo = null;
    protected MountRef curData = null;
    protected MainPlayerInfo MainPlayerInfo
    {
        get
        {
            return GameCenter.mainPlayerMng.MainPlayerInfo;
        }
    }
    private FDictionary mountDic
    {
        get
        {
            return GameCenter.newMountMng.AllMountDic;
        }
    }
    //private bool isRereshListInvoke = false;

    public UISprite trainRedMind;
    public UISpriteEx trainEx;
    public UILabel trainLab; 
    #endregion

    #region 构造
    void Awake()
    { 
        if (getMat != null) UIEventListener.Get(getMat.gameObject).onClick = delegate
            {
                GameCenter.newMallMng.CurMallType = MallItemType.PET;
				GameCenter.uIMng.GenGUI(GUIType.NEWMALL,true); 
            };
        //if (leftBtn != null) UIEventListener.Get(leftBtn.gameObject).onClick = OnClickLeftBtn;
        //if (rightBtn != null) UIEventListener.Get(rightBtn.gameObject).onClick = OnClickRightBtn;
        if (rideBtn != null)  UIEventListener.Get(rideBtn.gameObject).onClick = OnClickRideBtn; 
        if (disMountBtn != null) UIEventListener.Get(disMountBtn.gameObject).onClick = OnClickDisMountBtn; 
        if (illusionBtn != null) UIEventListener.Get(illusionBtn.gameObject).onClick = OnClickIllusionBtn;
        if (trainMountBtn != null) UIEventListener.Get(trainMountBtn.gameObject).onClick = OnClickTrainMountBtn;
        if (activateMountBtn != null) UIEventListener.Get(activateMountBtn.gameObject).onClick = OnClickActivateMountBtn;
        if (unlockLab != null) unlockLab.gameObject.SetActive(false);
        if (levest != null) levest.gameObject.SetActive(false);
        if (effect != null) effect.SetActive(false); 
    } 
    protected override void OnOpen()
    {  
        base.OnOpen(); 
        //isRereshListInvoke = false;
        if (GameCenter.newMountMng.rideType != 0) index = GameCenter.newMountMng.rideType - 1; 
        RefreshList(); 
        RefreshMountInfo(); 
        if (MainPlayerInfo != null) MainPlayerInfo.OnBaseUpdate += UpdataMonney;
        GameCenter.newMountMng.OnCurMountUpdate += RefreshList;
        GameCenter.newMountMng.OnMountLevUpdate += ShowLevEffect;  
    }
    protected override void OnClose()
    { 
        base.OnClose();
        mountLookTex.mainTexture = null;
        if (MainPlayerInfo != null) MainPlayerInfo.OnBaseUpdate -= UpdataMonney;
        GameCenter.newMountMng.OnCurMountUpdate -= RefreshList;
        GameCenter.newMountMng.OnMountLevUpdate -= ShowLevEffect; 
    } 
    #endregion

    #region 控件事件
    /// <summary>
    /// 请求激活坐骑
    /// </summary> 
    void OnClickActivateMountBtn(GameObject go)
    {
        GameCenter.newMountMng.C2S_ReqActivateMount();
    }
    /// <summary>
    /// 上一个坐骑
    /// </summary> 
    //void OnClickLeftBtn(GameObject go)
    //{
    //    if (index > 0)
    //    {
    //        --index;
    //        RefreshList();
    //    }
    //}
    /// <summary>
    /// 下一个坐骑
    /// </summary> 
    //void OnClickRightBtn(GameObject go)
    //{
    //    if (index < mountList.Count - 1)
    //    {
    //        ++index;
    //        RefreshList();
    //    }
    //}
    /// <summary>
    /// 骑上
    /// </summary> 
    void OnClickRideBtn(GameObject go)
    {
        if (GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneType == SceneType.ARENA)
        {
            GameCenter.messageMng.AddClientMsg(131);
            return;
        }
        int id = GameCenter.newMountMng.rideType;
        if (rideEx != null && rideEx.IsGray == UISpriteEx.ColorGray.normal)
        { 
            GameCenter.newMountMng.C2S_ReqRideMount(ChangeMount.RIDEMOUNT, id > 0 ? id : 1, MountReqRideType.SELT); 
        }
    }
    /// <summary>
    /// 乘下
    /// </summary> 
    void OnClickDisMountBtn(GameObject go)
    { 
        if (disrideEx != null && disrideEx.IsGray == UISpriteEx.ColorGray.normal)
        {
            int id = GameCenter.newMountMng.rideType;  
            GameCenter.newMountMng.C2S_ReqRideMount(ChangeMount.DOWNRIDE, id > 0 ? id : 1, MountReqRideType.SELT); 
        }
    }
    /// <summary>
    /// 化形(切换坐骑)
    /// </summary> 
    void OnClickIllusionBtn(GameObject go)
    {
        if (illusionEx != null && illusionEx.IsGray == UISpriteEx.ColorGray.normal)
        {  
            if (curData != null)
                GameCenter.newMountMng.C2S_ReqRideMount(ChangeMount.ILLUSION, curData.mountId, MountReqRideType.SELT);
        }
    }
    /// <summary>
    /// 坐骑培养
    /// </summary> 
    void OnClickTrainMountBtn(GameObject go)
    {
        if (trainEx.IsGray == UISpriteEx.ColorGray.Gray) return;
        if (curInfo == null) return; 
		ulong coinNum = 0;
        int itemNum = 0;
        RidePropertyRef ridePropertyRef = ConfigMng.Instance.GetMountPropertyRef(GameCenter.newMountMng.CurLev);
        if (ridePropertyRef != null && ridePropertyRef.item != null)
        {
            for (int i = 0; i < ridePropertyRef.item.Count; i++)
            {
                if (ridePropertyRef.item[i].eid == 5)
					coinNum = (ulong)ridePropertyRef.item[i].count;
                else
                {
                    itemNum = curInfo.Item[i].count;
                }
            }
        }
		if (MainPlayerInfo != null && MainPlayerInfo.TotalCoinCount >= coinNum)
        {
            if (GameCenter.inventoryMng.GetNumberByType(itemId) >= itemNum && atuoBuy != null)
                GameCenter.newMountMng.C2S_ReqPromoteMount((atuoBuy.value == true) ? 1 : 0);
            else//物品不足
            {
                if (atuoBuy != null && atuoBuy.value && itemRef != null)
                {
                    if (MainPlayerInfo != null && MainPlayerInfo.TotalDiamondCount >= itemRef.diamonPrice)
                    {
                        GameCenter.newMountMng.C2S_ReqPromoteMount(1);
                    }
                    else
                    {
                        MessageST mst1 = new MessageST();
                        mst1.messID = 137;
                        mst1.delYes = delegate
                        {
                            GameCenter.uIMng.SwitchToUI(GUIType.RECHARGE);
                        };
                        GameCenter.messageMng.AddClientMsg(mst1);
                    }
                }
                else
                {
                    MessageST mst = new MessageST();
                    mst.messID = 158;
                    if (itemRef != null) mst.words = new string[2] { (itemRef.diamonPrice * itemNum).ToString(), itemRef.name };
                    mst.delYes = delegate
                    {
                        if (MainPlayerInfo != null && itemRef != null && MainPlayerInfo.TotalDiamondCount >= itemRef.diamonPrice)
                        {
                            GameCenter.newMountMng.C2S_ReqPromoteMount(1);
                        }
                        else
                        {
                            MessageST mst1 = new MessageST();
                            mst1.messID = 137;
                            mst1.delYes = delegate
                            {
                                GameCenter.uIMng.SwitchToUI(GUIType.RECHARGE);
                            };
                            GameCenter.messageMng.AddClientMsg(mst1);
                        }
                    };
                    GameCenter.messageMng.AddClientMsg(mst);
                }
            }
        }
        else//金幣不足
        {
            GameCenter.messageMng.AddClientMsg(155);
        } 
    } 
    #endregion

    #region 刷新  
    /// <summary>
    /// 刷新坐骑
    /// </summary>
    void RefreshList()
    {  
        //if (curInfo == null)
        //{
        //    foreach (MountInfo info in mountInfoDic.Values)
        //    {
        //        if (info.IsRiding)
        //        {
        //            curInfo = info;
        //            break;
        //        }
        //    }
        //}
        if (curInfo == null)
        {
            if (mountInfoDic.ContainsKey(mountInfoDic.Count))//取字典最后一个
                curInfo = mountInfoDic[mountInfoDic.Count] as MountInfo;
        }
        if (mountInfoDic.Count > 0)
        {
            activateMountBtn.gameObject.SetActive(false);
            rideBtn.gameObject.SetActive(true);
            rideEx.IsGray = UISpriteEx.ColorGray.normal;
        }   
        if (unlockLab != null) unlockLab.gameObject.SetActive(false);
        if (curInfo != null && curInfo.IsRiding)
        {
            if (disMountBtn != null) disMountBtn.gameObject.SetActive(true);
            if (rideBtn != null) rideBtn.gameObject.SetActive(false); 
        }
        else
        {
            if (disMountBtn != null) disMountBtn.gameObject.SetActive(false);
            if (rideBtn != null) rideBtn.gameObject.SetActive(true);
            if (rideEx != null) rideEx.IsGray = UISpriteEx.ColorGray.normal;
        }
        if (mountList.Count > 0)//坐骑现在改为只有一个坐骑可用
        {
            curData = mountList[0];
            if (nameLab != null) nameLab.text = curData.mountName;
            if (mountDic.ContainsKey(curData.mountId))
            {
                GameCenter.previewManager.TryPreviewSingelMount(mountDic[curData.mountId] as MountInfo, mountLookTex);
            }
            if (mountInfoDic.ContainsKey(curData.mountId))//坐骑已经解锁
            {
                if (curInfo != null && mountList[0].mountId == curInfo.ConfigID)
                {
                    if (illusionEx != null) illusionEx.IsGray = UISpriteEx.ColorGray.Gray;
                }
                else
                {
                    if (illusionEx != null) illusionEx.IsGray = UISpriteEx.ColorGray.normal;
                }
            }
            else
            {
                ShowUnOwnMount();
            }
            return;
        }
        //for (int i = 0, max = mountList.Count; i < max; i++)
        //{
        //    if (mountList[i].mountId == curInfo.ConfigID)
        //    {
        //        curData = mountList[i];
        //        Debug.Log("id : " + curData.mountId + "  , name : " + curData.mountName);
        //        if (nameLab != null) nameLab.text = curData.mountName;
        //        if (mountDic.ContainsKey(curData.mountId))
        //        {
        //            GameCenter.previewManager.TryPreviewSingelMount(mountDic[curData.mountId] as MountInfo, mountLookTex);
        //        }
        //        if (mountInfoDic.ContainsKey(curData.mountId))//坐骑已经解锁
        //        {
        //            if (curInfo != null && mountList[i].mountId == curInfo.ConfigID)
        //            {
        //                if (illusionEx != null) illusionEx.IsGray = UISpriteEx.ColorGray.Gray;
        //            }
        //            else
        //            {
        //                if (illusionEx != null) illusionEx.IsGray = UISpriteEx.ColorGray.normal;
        //            }
        //        }
        //        return;
        //    } 
        //}
        ShowUnOwnMount();//这里压根不会进来
    }
    /// <summary>
    /// 根据等级刷新
    /// </summary>
    void RefreshMountInfo()
    {
        if (effect != null) effect.SetActive(false); 
        if (chanceLab != null)
        {
            chanceLab.gameObject.SetActive(false);
        }
        if (trainLab != null) trainLab.text = "坐骑培养";
        if (GameCenter.newMountMng.CurLev % 9 == 0)
        {
            if (trainLab != null) trainLab.text = "坐骑升阶";
            if (chanceLab != null && MountPropertyRefData != null)
            {
                chanceLab.gameObject.SetActive(true);
                chanceLab.text = ((float)MountPropertyRefData.chance / 10000) * 100 + "%";
            }
            if (GameCenter.newMountMng.CurLev >= (ConfigMng.Instance.RidePropertyRefTable.Count - 1))//满级
            {
                if (trainEx != null) trainEx.IsGray = UISpriteEx.ColorGray.Gray;
            }
        }

        for (int i = 0, max = starSp.Count; i < max; i++)
        { 
            starSp[i].IsGray = UISpriteEx.ColorGray.Gray; 
        }
        ShowStar();
        if (trainEx != null) trainEx.IsGray = UISpriteEx.ColorGray.normal; 
        if (GameCenter.newMountMng.CurLev >= (ConfigMng.Instance.RidePropertyRefTable.Count - 1))//满级
        {
            if (trainMountBtn != null) trainMountBtn.gameObject.SetActive(false);
            if (levest != null) levest.gameObject.SetActive(true);
        } 
        if (levLab != null && MountPropertyRefData != null)
        {
            levLab.text = MountPropertyRefData.name; 
        } 
        //当前等级属性
        if (MountPropertyRefData != null && curInfo!= null)
        {
            for (int j = 0; j < curPropertyLab.Count; j++)
            {
                curPropertyLab[j].text = MountPropertyRefData.attr[j].ToString();
            }
        }
        //下级等级属性
        int tableLenth = ConfigMng.Instance.RidePropertyRefTable.Count - 1;
        if (nextLevAddLab.Count > 0 && curInfo != null)
        {
            int next = GameCenter.newMountMng.CurLev + 1;
            if (next >= tableLenth)
            {
                next = tableLenth;//超出表中的数据默认下级为最后一个
            }
            RidePropertyRef nextInfo = ConfigMng.Instance.GetMountPropertyRef(next);
            if (nextInfo != null && MountPropertyRefData != null)
            {
                for (int j = 0; j < nextLevAddLab.Count; j++)
                {
                    nextLevAddLab[j].text = (nextInfo.attr[j] - MountPropertyRefData.attr[j]).ToString();
                }
            }
        } 
        //升级需要的财物
        bool isCoinEnough = false;
        bool isItemEnough = false; 
        int itemCount = 0;
        if (MountPropertyRefData != null && MountPropertyRefData.item != null)
        {
            for (int j = 0; j < MountPropertyRefData.item.Count; j++)
            {
                if (MountPropertyRefData.item[j].eid == 5)
                { 
                    if (MainPlayerInfo.TotalCoinCount < (ulong)MountPropertyRefData.item[j].count)
                    {
                        coinLab.text =MountPropertyRefData.item[j].count + "/" + "[ff0000]" + MainPlayerInfo.TotalCoinCount;
                    }
                    else
                    {
                        isCoinEnough = true;
                        coinLab.text =MountPropertyRefData.item[j].count + "/" +  "[6ef574]" + MainPlayerInfo.TotalCoinCount;//需要的金币/拥有的
                    }
                }
                else
                {
                    itemId = MountPropertyRefData.item[j].eid;
                    itemRef = ConfigMng.Instance.GetEquipmentRef(itemId);
                    itemCount = MountPropertyRefData.item[j].count;
                    if (GameCenter.inventoryMng.GetNumberByType(itemId) >= itemCount) isItemEnough = true;  
                    if (GameCenter.inventoryMng.GetNumberByType(itemId) >= itemCount)
                    {
                        isItemEnough = true;
                        if (itemLab != null) itemLab.text =itemCount + "/" +  "[6ef574]" + GameCenter.inventoryMng.GetNumberByType(itemId);
                    }
                    else
                    {
                        if (itemLab != null) itemLab.text = itemCount + "/" + "[ff0000]" + GameCenter.inventoryMng.GetNumberByType(itemId);
                    }
                }
            }
        } 
        if (!isItemEnough)
        {
            if (itemRef != null && diamondLab != null && MainPlayerInfo != null)
            {
                diamondLab.text = (itemRef.diamonPrice) * itemCount + "/" + MainPlayerInfo.TotalDiamondCount.ToString();
            }
        }
        else
        {
            if (diamondLab != null && MainPlayerInfo != null)
            {
                diamondLab.text = "0/" + MainPlayerInfo.TotalDiamondCount.ToString();
            }
        }
        if (isItemEnough && isCoinEnough && !levest.gameObject.activeSelf)
        {
            trainRedMind.gameObject.SetActive(true);
        }
        else
        {
            trainRedMind.gameObject.SetActive(false);
        }
        //if (curData != null)
        //{
        //    bool isLoadAgin = false;
        //    MountInfo info = mountDic[curData.mountId] as MountInfo;
        //    if (info.MountEffectList.Count > 0)
        //    {
        //        for (int i = 0, max = info.MountEffectList.Count; i < max; i++)
        //        {
        //            MountEffect effect = info.MountEffectList[i];
        //            if ((info.ConfigID - 1) * 9 + effect.effectLev == GameCenter.newMountMng.CurLev)
        //            {
        //                if (mountDic.ContainsKey(curData.mountId))
        //                {
        //                    isLoadAgin = true;
        //                    break;
        //                }
        //            }
        //        } 
        //    }
        //    if (isLoadAgin)//重新加载模型及特效
        //    { 
        //        GameCenter.previewManager.SetPreviewMountEnpty();
        //        GameCenter.previewManager.TryPreviewSingelMount(info, mountLookTex);
        //    }
        //}
        //if (GameCenter.newMountMng.CurLev == 0)//激活坐骑
        //{
        //    RefreshList();
        //} 
        //if ((index != (GameCenter.newMountMng.CurLev - 1) / 9) && isRereshListInvoke)
        //{
        //    CancelInvoke("refresh");//延迟刷新显示当前骑着的
        //    Invoke("refresh", 0.1f);
        //}
        //isRereshListInvoke = true; 
    }
    //void refresh()
    //{
    //    index = (GameCenter.newMountMng.CurLev-1) / 9; 
    //    RefreshList();
    //} 

    void ShowLevEffect()
    { 
        CancelInvoke("RefreshMountInfo");
        if (GameCenter.newMountMng.CurLev > 9 && GameCenter.newMountMng.CurLev % 9 == 1)//升阶
        { 
            if (trainEx != null) trainEx.IsGray = UISpriteEx.ColorGray.Gray;
            if (effect != null) effect.SetActive(true); 
            Invoke("RefreshMountInfo", 2.0f);
        }
        else
        {
            RefreshMountInfo();
        } 
    }
    /// <summary>
    /// 如果没有拥有该坐骑（没有解锁）不能化形显示灰色的骑上
    /// </summary> 
    void ShowUnOwnMount()
    {     
        if (illusionEx != null) illusionEx.IsGray = UISpriteEx.ColorGray.Gray;
        if (unlockLab != null) unlockLab.gameObject.SetActive(true);
        int id = 0;
        id = curData != null ? (curData.level - 9): 1; 
        RidePropertyRef propertyRef = ConfigMng.Instance.GetMountPropertyRef(id); 
        if (unlockLab != null && propertyRef != null) unlockLab.text = ConfigMng.Instance.GetUItext(28, new string[1] { propertyRef.name });
        if (disMountBtn != null) disMountBtn.gameObject.SetActive(false);
        if (mountInfoDic.Count <= 0)
        {
            activateMountBtn.gameObject.SetActive(true);
            rideBtn.gameObject.SetActive(false);
        }
        else
        {
            activateMountBtn.gameObject.SetActive(false);
            rideBtn.gameObject.SetActive(true);
            rideEx.IsGray = UISpriteEx.ColorGray.Gray; 
        }
    } 
    /// <summary>
    /// 星星的显示
    /// </summary>
    void ShowStar()
    { 
        effect4.SetActive(false);
        effect7.SetActive(false);
        if (len != (GameCenter.newMountMng.CurLev - 1) % starSp.Count)
        {
            len = (GameCenter.newMountMng.CurLev - 1) % starSp.Count;
            if (GameCenter.newMountMng.CurLev > 0 && effects.Count > len && effects[len] != null) effects[len].ShowFx();
        } 
        for (int i = 0,max = starSp.Count; i < max; i++)
        {
            if (i <= len)
            {
                if (i == 3) effect4.SetActive(true);
                if (i == 6) effect7.SetActive(true);
                starSp[i].IsGray = UISpriteEx.ColorGray.normal; 
            }
            else {
                starSp[i].IsGray = UISpriteEx.ColorGray.Gray; 
            }
        }
    }
    void UpdataMonney(ActorBaseTag tag, ulong y, bool da)
    {
        if (tag == ActorBaseTag.BindCoin || tag == ActorBaseTag.UnBindCoin || tag == ActorBaseTag.CoinLimit)
            RefreshMountInfo();
    }
     
    #endregion
}
