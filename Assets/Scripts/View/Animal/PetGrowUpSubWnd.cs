//==================================
//作者：朱素云
//日期：2016/3/4
//用途：宠物成长界面
//=================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PetGrowUpSubWnd : SubWnd
{
    #region 数据 
    public UIButton getMat;//获得材料至单人副本界面 
    public UIButton promoteBtn;//提升
    public UIButton leftBtn;//前一页
    public UIButton rightBtn;//后一页 
    public UIToggle isAutomaticBuy;//是否开启自动购买 
    public UISprite curSp;
    public UISprite nextSp; 
    public UILabel curNameLab;//名字
    public UILabel nameLab;
    public UILabel curGrowUpValLab;//当前成长值
    public UILabel attValLab;//攻击
    public UILabel hitValLab;//命中
    public UILabel crazyHitValLab;//暴击
    public UILabel GrowUpAfterPromoteLab;//提升后的成长值
    public UILabel attAfterPromoteLab;//提升后的攻击
    public UILabel hitAfterPromoteLab;//提升后的命中
    public UILabel crazyHitAfterPromoteLab;//提升后的暴击
    public UILabel expLab;
    public UILabel foodLab;//消耗口粮
    private int itemId = 0;
    private EquipmentRef itemRef = null;
    public UILabel coinLab;
    public UILabel diamondLab; 
    public UISlider upLevelUislider;//当前成长经验/升级所需成长经验 
    public MercenaryInfo curInfo;//当前宠物  
    protected List<ItemValue> growItem = new List<ItemValue>(); 
    public GameObject parent; 
    public UIScrollView scro;
    protected List<PetHonor> petHonorList = new List<PetHonor>();
    protected List<TitleRef> honorList = new List<TitleRef>();
    protected MainPlayerInfo MainPlayerInfo
    {
        get
        {
            return GameCenter.mainPlayerMng.MainPlayerInfo;
        }
    }
    public UIFxAutoActive effect;//升级特效
    private int czid = 0;
    private int len = 0;
    public Transform fullLev;

    public UISprite upBtnRedSp;//提升按钮上的红点
    #endregion

    #region 构造
    void Awake()
    {
        if (getMat != null) UIEventListener.Get(getMat.gameObject).onClick = delegate {
            if (GameCenter.mainPlayerMng != null && GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel >= 42)
            {
                //GameCenter.endLessTrialsMng.CopyType = EndLessTrialsMng.CopysType.ONESCOPY;
                GameCenter.duplicateMng.CopyType = DuplicateMng.CopysType.ONESCOPY;
                GameCenter.uIMng.SwitchToUI(GUIType.COPYINWND);
            }
            else
            {
                GameCenter.messageMng.AddClientMsg(389);
            }
        }; 
        if (promoteBtn != null) UIEventListener.Get(promoteBtn.gameObject).onClick = OnClickPromoteBtn;
        if (leftBtn != null) UIEventListener.Get(leftBtn.gameObject).onClick = OnClickLeftBtn;
        if (rightBtn != null) UIEventListener.Get(rightBtn.gameObject).onClick = OnClickRightBtn;
        honorList = GameCenter.mercenaryMng.HonorList;
    } 
    protected override void OnOpen()
    {
        base.OnOpen(); 
        RefreshPromoteInfo(); 
        GameCenter.mercenaryMng.OnMercenaryListUpdate += RefreshPromoteInfo;
        GameCenter.mercenaryMng.OnPetGrowUpUpdate += ShowEffect;
        GameCenter.inventoryMng.OnBackpackUpdate += RefreshPromoteInfo;
    }

    protected override void OnClose()
    {
        base.OnClose(); 
        GameCenter.mercenaryMng.OnMercenaryListUpdate -= RefreshPromoteInfo;
        GameCenter.inventoryMng.OnBackpackUpdate -= RefreshPromoteInfo;
        GameCenter.mercenaryMng.OnPetGrowUpUpdate -= ShowEffect;
    } 
    #endregion

    #region 事件
    /// <summary>
    /// 前一页
    /// </summary>
    /// <param name="go"></param>
    void OnClickLeftBtn(GameObject go)
    {
        if (scro != null)
        {
            SpringPanel sp = scro.GetComponent<SpringPanel>();
            if (sp == null) sp = scro.gameObject.AddComponent<SpringPanel>();
            if (sp.target.x < 0)
                SpringPanel.Begin(scro.gameObject, new Vector3(270, 0, 0) + sp.target, 9.5f);
        }
    }
    /// <summary>
    /// 后一页
    /// </summary>
    /// <param name="go"></param>
    void OnClickRightBtn(GameObject go)
    {
        if (scro != null)
        {
            SpringPanel sp = scro.GetComponent<SpringPanel>();
            if (sp == null) sp = scro.gameObject.AddComponent<SpringPanel>();
            if (sp.target.x > -270 * (honorList.Count - 2))
                SpringPanel.Begin(scro.gameObject, new Vector3(-270, 0, 0) + sp.target, 9.5f);
        }
    }
    /// <summary>
    /// 成长提升
    /// </summary>
    /// <param name="go"></param>
    void OnClickPromoteBtn(GameObject go)
    { 
        if (growItem.Count > 0)
        {
			ulong coinNum = 0;
            int itemNum = 0;
            for (int i = 0; i < growItem.Count; i++)
            {
                if (growItem[i].eid == 5)
                {
					coinNum = (ulong)growItem[i].count;
                }
                else
                {
                    itemNum = growItem[i].count;
                    itemRef = ConfigMng.Instance.GetEquipmentRef(growItem[i].eid);
                }
            }
            if (MainPlayerInfo != null && itemRef != null && MainPlayerInfo.TotalCoinCount >= coinNum)
            {
                if (GameCenter.inventoryMng.GetNumberByType(itemId) < itemNum)
                {
                    if (isAutomaticBuy != null && isAutomaticBuy.value)
                    {
                        if (MainPlayerInfo.TotalDiamondCount >= itemRef.diamonPrice)
                        {
                            GameCenter.mercenaryMng.C2S_ReqPromote(PetChange.GROWUP, GameCenter.mercenaryMng.curPetId, (isAutomaticBuy.value == true) ? 1 : 0);
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
                        mst.words = new string[2] { itemRef.diamonPrice.ToString(), itemRef.name };
                        mst.delYes = delegate
                        {
                            if (MainPlayerInfo.TotalDiamondCount >= itemRef.diamonPrice)
                                GameCenter.mercenaryMng.C2S_ReqPromote(PetChange.GROWUP, GameCenter.mercenaryMng.curPetId, 1);
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
                else
                    GameCenter.mercenaryMng.C2S_ReqPromote(PetChange.GROWUP, GameCenter.mercenaryMng.curPetId, 0);
            }
            else
            {
                GameCenter.messageMng.AddClientMsg(155);
            }
        }  
    }
    #endregion

    #region 刷新 
    void ShowEffect()
    {
        if (effect != null)
        {
            effect.ReShowFx();
        }
    }
    /// <summary>
    /// 刷新显示升级信息
    /// </summary>
    void RefreshPromoteInfo()
    { 
        FDictionary mercenaryInfoList = GameCenter.mercenaryMng.mercenaryInfoList;
        if (mercenaryInfoList.Count <= 0 || GameCenter.mercenaryMng.curPetId == MercenaryMng.noPet)
        {
            if (curNameLab != null) curNameLab.text = string.Empty;
            if (nameLab != null) nameLab.text = string.Empty;
            if (curGrowUpValLab != null) curGrowUpValLab.text = "0";
            if (curSp != null) curSp.gameObject.SetActive(false);
            if (nextSp != null) nextSp.gameObject.SetActive(false);
            if (attValLab != null) attValLab.text = "0";
            if (hitValLab != null) hitValLab.text = "0";
            if (crazyHitValLab != null) crazyHitValLab.text = "0";
            if (GrowUpAfterPromoteLab != null) GrowUpAfterPromoteLab.text = "0";
            if (attAfterPromoteLab != null) attAfterPromoteLab.text = "0";
            if (hitAfterPromoteLab != null) hitAfterPromoteLab.text = "0";
            if (crazyHitAfterPromoteLab != null) crazyHitAfterPromoteLab.text = "0";
            if (coinLab != null) coinLab.text = "0";
            if (foodLab != null) foodLab.text = "0";
            if (upLevelUislider != null) upLevelUislider.value = 0;
            if (upBtnRedSp != null) upBtnRedSp.gameObject.SetActive(false);
            expLab.text = 0 + "/" + 0;
            for (int i = 0, max = petHonorList.Count; i < max; i++)
            {
                if (petHonorList[i] != null) petHonorList[i].gameObject.SetActive(false);
            }
            return;
        } 
        curInfo = GameCenter.mercenaryMng.GetMercenaryById(GameCenter.mercenaryMng.curPetId);
        if (curInfo == null) return;
        NewPetDataRef petData = null;
        NewPetDataRef nextPetData = null;
        czid = curInfo.GrowUp;
        len = ConfigMng.Instance.GetPetDataRefTable.Count - 1;
        if (czid < len)
        {
            fullLev.gameObject.SetActive(false);
            promoteBtn.gameObject.SetActive(true); 
        }
        else
        {
            fullLev.gameObject.SetActive(true); 
            promoteBtn.gameObject.SetActive(false); 
        }
        if (czid <= len && czid <= len) petData = ConfigMng.Instance.GetPetDataRef(czid);
        if ((czid + 1) <= len && (czid + 1) <= len) nextPetData = ConfigMng.Instance.GetPetDataRef(czid + 1);
        else
            nextPetData = petData;
        if (curNameLab != null) curNameLab.text = curInfo.PetName;
        if (nameLab != null) nameLab.text = curInfo.PetName;
        if (curGrowUpValLab != null) curGrowUpValLab.text = czid.ToString();//当前成长值
        if (curSp != null)
        {
            curSp.gameObject.SetActive(true);
            curSp.spriteName = curInfo.Icon;
        }
        int growTitle = 0;
        if (nextSp != null)
        {
            nextSp.gameObject.SetActive(true);
            nextSp.spriteName = curInfo.Icon;
        }
        int curatt = 0;
        if(czid <= 0)
        {
            attValLab.text = "0";
            hitValLab.text = "0";
            crazyHitValLab.text = "0";
            upLevelUislider.value = 0;//当前成长经验/升级所需成长经验  
            if (expLab != null && nextPetData != null) expLab.text = 0 + "/" + nextPetData.cZExp;
        }
        else if (petData != null)
        {
            int curExp = 0;
            if (czid < len)
            {
                curExp = curInfo.GrowUpExp;
            }
            else
            {
                curExp = petData.cZExp;
            }
            if (upLevelUislider != null && nextPetData != null) upLevelUislider.value = (float)curExp / nextPetData.cZExp;//当前成长经验/升级所需成长经验  
            if (expLab != null) expLab.text = curExp + "/" + nextPetData.cZExp; 
            if(petData != null)growTitle =  petData.cZTitle; 
            for (int j = 0; j < petData.chengZhang.Count; j++)
            {
                if (petData.chengZhang[j].eid == (int)PetProperty.PETATT)
                {
                    curatt = petData.chengZhang[j].count;
                    if (attValLab != null) attValLab.text = curatt.ToString();
                }
                if (petData.chengZhang[j].eid == (int)PetProperty.PETHIT)
                {
                    if (hitValLab != null) hitValLab.text = petData.chengZhang[j].count.ToString();
                }
                if (petData.chengZhang[j].eid == (int)PetProperty.PETCRI)
                {
                    if (crazyHitValLab != null) crazyHitValLab.text = petData.chengZhang[j].count.ToString();
                }
            }
        } 
        if (GrowUpAfterPromoteLab != null)
        {
            if (czid < (ConfigMng.Instance.GetPetDataRefTable.Count - 1))
            {
                GrowUpAfterPromoteLab.text = (curInfo.GrowUp + 1).ToString();//提升后的成长值
            }
            else
            {
                GrowUpAfterPromoteLab.text = curInfo.GrowUp.ToString();
            }
        }
        int att = 0;int hit = 0;int cri = 0;
        if (nextPetData != null)
        {
            growItem = nextPetData.cZIem;
            for (int i = 0; i < nextPetData.chengZhang.Count; i++)
            {
                if (nextPetData.chengZhang[i].eid == (int)PetProperty.PETATT)
                    att = nextPetData.chengZhang[i].count;
                if (nextPetData.chengZhang[i].eid == (int)PetProperty.PETHIT)
                    hit = nextPetData.chengZhang[i].count;
                if (nextPetData.chengZhang[i].eid == (int)PetProperty.PETCRI)
                    cri = nextPetData.chengZhang[i].count;
            }
        }
        if (attAfterPromoteLab != null) attAfterPromoteLab.text = (att).ToString();//提升后的攻击
        if (hitAfterPromoteLab != null) hitAfterPromoteLab.text = (hit).ToString();//提升后的命中
        if (crazyHitAfterPromoteLab != null) crazyHitAfterPromoteLab.text = (cri).ToString();//提升后的暴击 
        bool isItemEnough = false;
        bool isCoinEnough = false;
        if (growItem.Count > 0 && MainPlayerInfo != null)
        {
            for (int i = 0; i < growItem.Count; i++)
            {
                if (growItem[i].eid == 5)
                {
                            
                    if (MainPlayerInfo.TotalCoinCount < (ulong)growItem[i].count)
                    {
                        coinLab.text =growItem[i].count + "/" +  "[ff0000]" + MainPlayerInfo.TotalCoinCount;
                    }
                    else
                    {  
                        coinLab.text = growItem[i].count + "/" + "[6ef574]" + MainPlayerInfo.TotalCoinCount;//需要的金币/拥有的
                        isCoinEnough = true; 
                    }
                }
                else
                { 
                    itemId = growItem[i].eid;
                    itemRef = ConfigMng.Instance.GetEquipmentRef(itemId);
                    if (GameCenter.inventoryMng.GetNumberByType(growItem[i].eid) >= growItem[i].count)
                    {
                        isItemEnough = true;
                        if (foodLab != null) foodLab.text =growItem[i].count + "/" +  "[6ef574]" + GameCenter.inventoryMng.GetNumberByType(growItem[i].eid);
                    }
                    else
                    {
                        if (foodLab != null) foodLab.text =growItem[i].count + "/" +  "[ff0000]" + GameCenter.inventoryMng.GetNumberByType(growItem[i].eid);
                    }
                }
            }
        }  
        if (!isItemEnough)
        {
            if (diamondLab != null && MainPlayerInfo != null && itemRef != null)
            {
                diamondLab.text = itemRef.diamonPrice + "/" + MainPlayerInfo.TotalDiamondCount.ToString();
            }
        }
        else
        {
            if (diamondLab != null && MainPlayerInfo != null) diamondLab.text = "0/" + MainPlayerInfo.TotalDiamondCount.ToString(); 
        }
        if (isItemEnough && isCoinEnough && czid < (ConfigMng.Instance.GetPetDataRefTable.Count - 1))
        {
            upBtnRedSp.gameObject.SetActive(true);
        }
        else
        {
            upBtnRedSp.gameObject.SetActive(false);
        }
        ShowHonor(growTitle);
    } 
    /// <summary>
    /// 显示头衔
    /// </summary>
    void ShowHonor(int _growTitle)
    { 
        if (parent != null)
        {
            UIExGrid grid = parent.GetComponent<UIExGrid>();
            if (grid != null)
            {
                grid.maxPerLine = honorList.Count;

                for (int i = 0, max = petHonorList.Count; i < max; i++)
                {
                    petHonorList[i].gameObject.SetActive(false);
                }

                for (int i = 0; i < honorList.Count; i++)
                {
                    if (petHonorList.Count <= i)
                    {
                        PetHonor item = PetHonor.CeateNew(i, honorList[i].type, parent);
                        item.gameObject.SetActive(true);
                        if (_growTitle >= honorList[i].type)
                        { 
                            item.honorIcon.IsGray = UISpriteEx.ColorGray.normal;
                        }
                        else
                        {
                            item.honorIcon.IsGray = UISpriteEx.ColorGray.Gray;
                        }
                        petHonorList.Add(item);
                    }
                    else
                    {
                        petHonorList[i].TitleRef = ConfigMng.Instance.GetTitlesRef(honorList[i].type);
                        petHonorList[i].gameObject.SetActive(true);
                        if (petHonorList[i].TitleRef != null)
                        {
                            if (_growTitle >= honorList[i].type)
                            { 
                                petHonorList[i].honorIcon.IsGray = UISpriteEx.ColorGray.normal;
                            }
                            else
                            {
                                petHonorList[i].honorIcon.IsGray = UISpriteEx.ColorGray.Gray;
                            }
                        }
                    }
                }
                grid.repositionNow = true;
            }
        } 
    } 
    #endregion 
}
