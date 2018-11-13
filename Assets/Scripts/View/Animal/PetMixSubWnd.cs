//==================================
//作者：朱素云
//日期：2016/3/15
//用途：宠物融合界面
//=================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PetMixSubWnd : SubWnd
{
    #region 数据 
    public GameObject zhuPet;//主宠信息
    public Transform zhuKong;//没有主宠时  
    public UILabel zhuChengzhangLab;//成长
    public UILabel zhuZizhiLab;//资质
    public UILabel zhuAttLab;//攻击
    public UILabel zhuHitLab;//命中
    public UILabel zhuCrazyHitLab;//暴击
    public UILabel zhuNameLab;//名字  
    public GameObject fuPet;//副宠信息
    public Transform fuKong;//没有副宠时 
    public UILabel fuChengzhangLab;//成长
    public UILabel fuZizhiLab;//资质
    public UILabel fuAttLab;//攻击
    public UILabel fuHitLab;//命中
    public UILabel fuCrazyHitLab;//暴击
    public UILabel fuNameLab;//名字 
    public UILabel ziZhiExp; 
    public MercenaryInfo zhuInfo;//当前宠物  
    public MercenaryInfo fuInfo;//当前宠物  
    public UISlider expUislider;//宠物的资质经验/资质升级经验
    public UIButton mixBtn;//融合按钮
    public UIButton zhuUpZizhiBtn;//提升主宠资质
    public UIButton fuUpZizhiBtn;//提升副宠资质
    public UIButton zhuHeadPicBtn;//主宠头像
    public UIButton fuHeadPicBtn;//副宠头像
    public UISprite zhuHeadSp;
    public UISprite fuHeadSp;
    public UISpriteEx mixEx;
    //使用资质丹ui
    public GameObject promoteApititudeUi;//提升资质ui
    public UIButton closeZiZhiUi;//关闭资质提升窗口  
    public UILabel diamondLab;//元宝数 
    protected List<ItemValue> zZItem = new List<ItemValue>(); 
    public UIButton usePrimaryPill;//初级资质丹
    public UIButton useMiddlePill;//中级资质丹
    public UIButton useSeniorPill;//高级资质丹 
    public List<UILabel> pillCountLab = new List<UILabel>();
    public List<ItemUI> pillItem = new List<ItemUI>();   
    public UISlider expSli;//宠物的资质经验/资质升级经验
    public UILabel expLab; 
    protected int upZiZhiId;//提升资质的id
    protected MainPlayerInfo MainPlayerInfo
    {
        get
        {
            return GameCenter.mainPlayerMng.MainPlayerInfo;
        }
    }

    public UIFxAutoActive mixEffect;

    public Transform fullZhuApitude;
    public Transform fullFuApitude;
    public UIToggle isAutomaticBuy;//是否开启自动购买 
    public UIFxAutoActive effect1;//升级特效
    public UIFxAutoActive effect2;//升级特效
    private bool isZhuAptitude = false;

    public UISprite zhuRed;
    public UISprite fuRed;

    public GameObject eggObj;//宠物蛋ui
    public UIButton closeEggObj;
    public UIButton zhuUseEgg;//主宠使用宠物蛋
    public UIButton fuUseEgg;//副宠使用宠物蛋
    public ItemUI eggItem;
    public UIGrid grid;
    protected List<ItemUI> eggItems = new List<ItemUI>();
    public UISlider expEggSli;//宠物的资质经验/资质升级经验
    public UILabel expEggLab; 
   
    #endregion

    void Awake()
    {
        if (zhuUpZizhiBtn != null) UIEventListener.Get(zhuUpZizhiBtn.gameObject).onClick = OnClickUpZhuZiZhiBtn;
        if (fuUpZizhiBtn != null) UIEventListener.Get(fuUpZizhiBtn.gameObject).onClick = OnClickUpFuZiZhiBtn; 
        if (mixBtn != null) UIEventListener.Get(mixBtn.gameObject).onClick = ReadyToMixPet;
        if (zhuHeadPicBtn != null) UIEventListener.Get(zhuHeadPicBtn.gameObject).onClick = MoveZhuPet; 
        if (fuHeadPicBtn != null) UIEventListener.Get(fuHeadPicBtn.gameObject).onClick = MoveFuPet; 
        if (closeZiZhiUi != null) UIEventListener.Get(closeZiZhiUi.gameObject).onClick = OnClickCloseZiZhiUi;
        if (usePrimaryPill != null) UIEventListener.Get(usePrimaryPill.gameObject).onClick = OnClickUsePrimaryPill;
        if (useMiddlePill != null) UIEventListener.Get(useMiddlePill.gameObject).onClick = OnClickUseMiddlePill;
        if (useSeniorPill != null) UIEventListener.Get(useSeniorPill.gameObject).onClick = OnClickUseSeniorPill;
        if (zhuUseEgg != null) UIEventListener.Get(zhuUseEgg.gameObject).onClick = OnClickZhuEggs;
        if (fuUseEgg != null) UIEventListener.Get(fuUseEgg.gameObject).onClick = OnClickFuEggs;
        if (closeEggObj != null) UIEventListener.Get(closeEggObj.gameObject).onClick = delegate
        {
            GameCenter.mercenaryMng.isOpenMixWndAndUseEgg = false; eggObj.SetActive(false);
        }; 
        if (promoteApititudeUi != null) promoteApititudeUi.gameObject.SetActive(false);
        if (eggObj != null) eggObj.SetActive(false);
        if (eggItem != null) eggItem.gameObject.SetActive(false);
    } 
    protected override void OnOpen()
    { 
        base.OnOpen(); 
        if (GameCenter.mercenaryMng.curPetId == MercenaryMng.noPet || 
            GameCenter.mercenaryMng.mercenaryInfoList.Count <= 0)
        {
            if (mixEx != null) mixEx.IsGray = UISpriteEx.ColorGray.Gray;
            zhuKong.gameObject.SetActive(true);
            fuKong.gameObject.SetActive(true);
            zhuPet.SetActive(false);
            fuPet.SetActive(false);
            return;
        } 
        GameCenter.mercenaryMng.zhuPetId = GameCenter.mercenaryMng.curPetId;
        GameCenter.mercenaryMng.fuPetId = MercenaryMng.noPet;
        ShowZhuPet();//显示主宠信息  
        ShowFuPet();
        SetRedRemind();
    } 
    protected override void OnClose()
    {
        base.OnClose();
        GameCenter.mercenaryMng.curUseEggPetId = MercenaryMng.noPet;
        GameCenter.mercenaryMng.isOpenMixWndAndUseEgg = false;
    }
    protected override void HandEvent(bool _bind)
    {
        base.HandEvent(_bind);
        if (_bind)
        {
            GameCenter.mercenaryMng.OnFuPetUpdate += ShowFuPet;
            GameCenter.mercenaryMng.OnZhuPetUpdate += ShowZhuPet;
            GameCenter.mercenaryMng.OnMixUpdate += UpdataZhuInfo;
            GameCenter.mercenaryMng.OnMixUpdate += ShowEffect;
            GameCenter.mercenaryMng.OnPetGrowUpUpdate += ShowEffectLev; 
            GameCenter.inventoryMng.OnBackpackUpdate += SetRedRemind;
            GameCenter.inventoryMng.OnBackpackUpdate += ShowAllEggs;
            GameCenter.mercenaryMng.OnPetGrowUpUpdate += RefreshExpUsedEgg;
            GameCenter.mercenaryMng.OnMercenaryListUpdate += UpdataZhuInfo;
        }
        else
        {
            GameCenter.mercenaryMng.OnFuPetUpdate -= ShowFuPet;
            GameCenter.mercenaryMng.OnZhuPetUpdate -= ShowZhuPet;
            GameCenter.mercenaryMng.OnMixUpdate -= UpdataZhuInfo;
            GameCenter.mercenaryMng.OnMixUpdate -= ShowEffect;
            GameCenter.mercenaryMng.OnPetGrowUpUpdate -= ShowEffectLev;
            GameCenter.inventoryMng.OnBackpackUpdate -= SetRedRemind;
            GameCenter.inventoryMng.OnBackpackUpdate -= ShowAllEggs;
            GameCenter.mercenaryMng.OnPetGrowUpUpdate -= RefreshExpUsedEgg;
            GameCenter.mercenaryMng.OnMercenaryListUpdate -= UpdataZhuInfo;
        }
    }
    void ShowEffectLev()
    {
        if (effect1 != null && effect2 != null)
        {
            if(isZhuAptitude)effect1.ReShowFx();
            effect2.ReShowFx();
        }
    }
    void SetRedRemind()
    {
        if (GameCenter.inventoryMng.GetNumberByType((int)ItemRedRemind.APTITUDEPILLPRIMARY) >= 1 ||
               GameCenter.inventoryMng.GetNumberByType((int)ItemRedRemind.APTITUDEPILLMIDDLE) >= 1 ||
               GameCenter.inventoryMng.GetNumberByType((int)ItemRedRemind.APTITUDEPILLSENOIR) >= 1)//融合
        {
            if (zhuRed != null) zhuRed.gameObject.SetActive(true);
            if (fuRed != null) fuRed.gameObject.SetActive(true);
        }
        else
        {
            if (zhuRed != null) zhuRed.gameObject.SetActive(false);
            if (fuRed != null) fuRed.gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// 资质丹窗口的显示
    /// </summary>
    void ShowZiZhiPillInfo(int _id)
    {
        int  id = GameCenter.mercenaryMng.GetMercenaryById(_id).Aptitude; 
        if (id >= 100)
        {
            if (promoteApititudeUi.gameObject.activeSelf) promoteApititudeUi.gameObject.SetActive(false);
        }
        else
        {
            if (!promoteApititudeUi.gameObject.activeSelf) promoteApititudeUi.gameObject.SetActive(true);
        }
        //经验条的显示  
        NewPetDataRef petDataRef = ConfigMng.Instance.GetPetDataRef(id);
        int zzNeedExp = 0;
        if (petDataRef != null)
        {
            zzNeedExp = petDataRef.zZExp;
            zZItem = petDataRef.zZItem;
        }
        int EXP = GameCenter.mercenaryMng.GetMercenaryById(_id).AptitudeExp; 
        if (expSli != null) expSli.value = (float)EXP / zzNeedExp;
        if (expLab != null) expLab.text = EXP + "/" + zzNeedExp;
        if (diamondLab != null && MainPlayerInfo != null) diamondLab.text = MainPlayerInfo.DiamondCountText; 
        if (zZItem.Count > 0)
        {  
            for(int i = 0; i< zZItem.Count ;i++)
            {
                if (zZItem[i].eid == 2200006)
                {
                    if (pillCountLab.Count > 0) pillCountLab[0].text = zZItem[i].count + "/" + GameCenter.inventoryMng.GetNumberByType(2200006);
                    if (pillItem.Count > 0) pillItem[0].FillInfo(new EquipmentInfo(zZItem[i].eid, EquipmentBelongTo.PREVIEW));
                }
                if (zZItem[i].eid == 2200007)
                {
                    if (pillCountLab.Count > 1) pillCountLab[1].text = zZItem[i].count + "/" + GameCenter.inventoryMng.GetNumberByType(2200007);
                    if (pillItem.Count > 1) pillItem[1].FillInfo(new EquipmentInfo(zZItem[i].eid, EquipmentBelongTo.PREVIEW));
                }
                if (zZItem[i].eid == 2200008)
                {
                    if (pillCountLab.Count > 2) pillCountLab[2].text = zZItem[i].count + "/" + GameCenter.inventoryMng.GetNumberByType(2200008);
                    if (pillItem.Count > 2) pillItem[2].FillInfo(new EquipmentInfo(zZItem[i].eid, EquipmentBelongTo.PREVIEW));
                } 
            } 
        } 
    } 

    /// <summary>
    /// 显示所有的宠物蛋
    /// </summary>
    void ShowAllEggs()
    { 
        List<EquipmentInfo> eggs = GameCenter.mercenaryMng.GetPetEggFromBag(); 
        if (grid != null)
        {
            grid.maxPerLine = 5;
            int count = eggs.Count > 10 ? eggs.Count : 10;
            ItemUI itemUi = null;
             
            for (int i = 0, max = count; i < max; i++)
            {
                if (eggItems.Count <= i)
                {
                    GameObject go = Instantiate(eggItem.gameObject) as GameObject;
                    go.transform.parent = grid.transform;
                    go.transform.localPosition = new Vector3((i % 5) * 90, -(i / 5) * 88);
                    go.transform.localScale = Vector3.one;
                    itemUi = go.GetComponent<ItemUI>();
                    if (eggs.Count > i)
                    { 
                        itemUi.FillInfo(eggs[i]);
                        itemUi.SetActionBtn(ItemActionType.None, ItemActionType.None,ItemActionType.MIX);
                    }
                    else
                    {
                        itemUi.SetEmpty();
                    }
                    go.SetActive(true);
                    eggItems.Add(itemUi);
                }
                else
                {
                    eggItems[i].gameObject.SetActive(true);
                    eggItems[i].transform.parent = grid.transform;
                    eggItems[i].transform.localPosition = new Vector3((i % 5) * 90, -(i / 5) * 88); 
                    if (eggs.Count > i)
                    {
                        eggItems[i].FillInfo(eggs[i]);
                        eggItems[i].SetActionBtn(ItemActionType.None, ItemActionType.None, ItemActionType.MIX);
                    }
                    else
                        eggItems[i].SetEmpty();
                }
            } 
        }
    }

    /// <summary>
    /// 使用宠物蛋后刷新资质经验条
    /// </summary>
    void RefreshExpUsedEgg()
    {
        if (GameCenter.mercenaryMng.curUseEggPetId == MercenaryMng.noPet) return;
        int index = GameCenter.mercenaryMng.curUseEggPetId;
        int id = GameCenter.mercenaryMng.GetMercenaryById(index).Aptitude;
        NewPetDataRef petDataRef = ConfigMng.Instance.GetPetDataRef(id);
        int zzNeedExp = 0;
        if (petDataRef != null)
        {
            zzNeedExp = petDataRef.zZExp; 
        } 
        float EXP = GameCenter.mercenaryMng.GetMercenaryById(index).AptitudeExp;
        if (expEggSli != null) expEggSli.value = EXP / zzNeedExp;
        if (expEggLab != null) expEggLab.text = EXP + "/" + zzNeedExp;
    }

    #region 事件
    void MoveZhuPet(GameObject go)
    {
        UIButton btn = zhuHeadPicBtn.GetComponent<UIButton>();
        GameCenter.mercenaryMng.zhuPetId = MercenaryMng.noPet;
        ShowZhuPet();
    }
    void MoveFuPet(GameObject go)
    {
        GameCenter.mercenaryMng.fuPetId = MercenaryMng.noPet;
        ShowFuPet();
    }
    void OnClickZhuEggs(GameObject go)
    {
        GameCenter.mercenaryMng.curUseEggPetId = GameCenter.mercenaryMng.zhuPetId;
        eggObj.SetActive(true); 
        ShowAllEggs();
        RefreshExpUsedEgg();
    }
    void OnClickFuEggs(GameObject go)
    {
        GameCenter.mercenaryMng.curUseEggPetId = GameCenter.mercenaryMng.fuPetId;
        eggObj.SetActive(true); 
        ShowAllEggs();
        RefreshExpUsedEgg();
    }
    /// <summary>
    /// 主宠提升资质
    /// </summary>
    /// <param name="go"></param>
    void OnClickUpZhuZiZhiBtn(GameObject go)
    {
        upZiZhiId = GameCenter.mercenaryMng.zhuPetId;
        isZhuAptitude = true;
        ShowZiZhiPillInfo(upZiZhiId);
    }
    /// <summary>
    /// 副宠提升资质
    /// </summary>
    /// <param name="go"></param>
    void OnClickUpFuZiZhiBtn(GameObject go)
    {
        upZiZhiId = GameCenter.mercenaryMng.fuPetId;
        isZhuAptitude = false;
        ShowZiZhiPillInfo(upZiZhiId);
    } 
    void OnClickUsePrimaryPill(GameObject go)
    {
        CheckCondition(2200006, PetChange.USEPRIMERYPILL); 
    } 
    void OnClickUseMiddlePill(GameObject go)
    {
        CheckCondition(2200007, PetChange.USEMIDDLEPILL);
    }
    void OnClickUseSeniorPill(GameObject go)
    {
        CheckCondition(2200008, PetChange.USESENIORPILL);
    } 
    /// <summary>
    /// 判断资质丹是否够用
    /// </summary> 
    void CheckCondition(int _id, PetChange _req)
    {
        //Debug.Log("判断资质丹是否够用");
        int itemNum = 0;
        if (zZItem.Count > 0)
        { 
            for (int i = 0; i < zZItem.Count; i++)
            {
                if (zZItem[i].eid == _id)
                { 
                    itemNum = zZItem[i].count;
                }
            }
        }
        if (itemNum > GameCenter.inventoryMng.GetNumberByType(_id))//物品不够
        {
            if (isAutomaticBuy != null && isAutomaticBuy.value)//自动购买
            {
                if (MainPlayerInfo != null && MainPlayerInfo.TotalDiamondCount >= ConfigMng.Instance.GetEquipmentRef(_id).diamonPrice)
                {
                    GameCenter.mercenaryMng.C2S_ReqPromote(_req, upZiZhiId, 1);
                }
                else
                {
                    MessageST mst = new MessageST();
                    mst.messID = 137;
                    mst.delYes = delegate
                    {
                        GameCenter.uIMng.SwitchToUI(GUIType.RECHARGE);
                    };
                    GameCenter.messageMng.AddClientMsg(mst);
                }
            }
            else
            {
                MessageST mst = new MessageST();
                mst.messID = 158;
                mst.words = new string[2] { ConfigMng.Instance.GetEquipmentRef(_id).diamonPrice.ToString(), ConfigMng.Instance.GetEquipmentRef(_id).name };
                mst.delYes = delegate
                {
                    if (MainPlayerInfo != null && MainPlayerInfo.TotalDiamondCount >= ConfigMng.Instance.GetEquipmentRef(_id).diamonPrice)
                        GameCenter.mercenaryMng.C2S_ReqPromote(_req, upZiZhiId, 1);
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
        {
            GameCenter.mercenaryMng.C2S_ReqPromote(_req, upZiZhiId, 0);
        }
    } 
    /// <summary>
    /// 关闭资质提升窗口
    /// </summary>
    void OnClickCloseZiZhiUi(GameObject go)
    {
        if (promoteApititudeUi != null) promoteApititudeUi.gameObject.SetActive(false);
    }
    /// <summary>
    /// 融合
    /// </summary>
    void   ReadyToMixPet(GameObject go)
    {
        if (fuInfo == null) return;
        if (zhuInfo != null)
        {
            if (zhuInfo.Aptitude >= 100)
            {
                GameCenter.messageMng.AddClientMsg(328);
                return;
            }
        }
        if (GameCenter.mercenaryMng.zhuPetId != MercenaryMng.noPet && GameCenter.mercenaryMng.fuPetId != MercenaryMng.noPet)
        {
            if (fuInfo.IsActive == (int)PetChange.FINGHTING)
            {
                GameCenter.messageMng.AddClientMsg(227);
                return;
            }
            if (fuInfo.IsActive == (int)PetChange.GUARD)
            {
                GameCenter.messageMng.AddClientMsg(228);
                return;
            } 
            MessageST mst = new MessageST();
            mst.messID = 160;
            mst.delYes = delegate
            { 
                GameCenter.mercenaryMng.C2S_ReqPromote(PetChange.FUSE, GameCenter.mercenaryMng.zhuPetId, GameCenter.mercenaryMng.fuPetId); 
            };
            GameCenter.messageMng.AddClientMsg(mst);
        }
    }
    void ShowEffect()
    {
        if (mixEffect != null) mixEffect.ShowFx(); 
    }
    /// <summary>
    /// 显示主宠
    /// </summary>
    void ShowZhuPet()
    {
        if (GameCenter.mercenaryMng.zhuPetId == MercenaryMng.noPet)
        {
            if (mixEx != null) mixEx.IsGray = UISpriteEx.ColorGray.Gray;
            if (zhuPet != null) zhuPet.gameObject.SetActive(false);
            if (zhuKong != null) zhuKong.gameObject.SetActive(true);
            if (expUislider != null) expUislider.value = 0;
            if (ziZhiExp != null) ziZhiExp.text = "0" + "/" + "0";
            return;
        }
        if (zhuPet != null) zhuPet.gameObject.SetActive(true);
        if (zhuKong != null) zhuKong.gameObject.SetActive(false);
        UpdataZhuInfo();  
    }
    void UpdataZhuInfo()
    {
        if (GameCenter.mercenaryMng.fuPetId != MercenaryMng.noPet)
        {
            if (mixEx != null) mixEx.IsGray = UISpriteEx.ColorGray.normal;
        }
        zhuInfo = GameCenter.mercenaryMng.GetMercenaryById(GameCenter.mercenaryMng.zhuPetId);
        if (zhuInfo == null) return;
        fullZhuApitude.gameObject.SetActive(false);
        zhuUpZizhiBtn.gameObject.SetActive(true);
        zhuUseEgg.gameObject.SetActive(true);
        if (zhuInfo != null && zhuInfo.Aptitude >= 100)
        {
            fullZhuApitude.gameObject.SetActive(true);
            zhuUpZizhiBtn.gameObject.SetActive(false);
            zhuUseEgg.gameObject.SetActive(false);
        }
        NewPetDataRef petDataRef = ConfigMng.Instance.GetPetDataRef(zhuInfo.Aptitude > 0 ? zhuInfo.Aptitude : 1); 
        int zzNeedExp = 0;
        if (petDataRef != null && zhuInfo.Aptitude < 100) zzNeedExp = petDataRef.zZExp;
        else
            zzNeedExp = zhuInfo.AptitudeExp;
        if (zhuInfo != null)
        { 
            if (expUislider != null) expUislider.value = (float)zhuInfo.AptitudeExp / zzNeedExp;
            if (ziZhiExp != null) ziZhiExp.text = zhuInfo.AptitudeExp + "/" + zzNeedExp; 
            if (zhuNameLab != null) zhuNameLab.text = zhuInfo.PetName;
            if (zhuChengzhangLab != null) zhuChengzhangLab.text = zhuInfo.GrowUp.ToString();//成长
            if (zhuZizhiLab != null) zhuZizhiLab.text = zhuInfo.Aptitude.ToString();//资质
            for (int j = 0; j < zhuInfo.SoulPropertyList.Count; j++)
            {
                if (zhuInfo.SoulPropertyList[j].type == (int)PetProperty.PETATT)
                {
                    if (zhuAttLab != null) zhuAttLab.text = zhuInfo.SoulPropertyList[j].num.ToString();
                }
                if (zhuInfo.SoulPropertyList[j].type == (int)PetProperty.PETHIT)
                {
                    if (zhuHitLab != null) zhuHitLab.text = zhuInfo.SoulPropertyList[j].num.ToString();
                }
                if (zhuInfo.SoulPropertyList[j].type == (int)PetProperty.PETCRI)
                {
                    if (zhuCrazyHitLab != null) zhuCrazyHitLab.text = zhuInfo.SoulPropertyList[j].num.ToString();
                }
            }
            if (zhuHeadSp != null) zhuHeadSp.spriteName = zhuInfo.Icon;
        }
        if (promoteApititudeUi != null && promoteApititudeUi.activeSelf)
            ShowZiZhiPillInfo(GameCenter.mercenaryMng.zhuPetId);
    }
    /// <summary>
    /// 显示副宠
    /// </summary>
    void ShowFuPet()
    {
        if (GameCenter.mercenaryMng.fuPetId == MercenaryMng.noPet)
        {
            if (mixEx != null) mixEx.IsGray = UISpriteEx.ColorGray.Gray;
            if (fuPet != null) fuPet.gameObject.SetActive(false);
            if (fuKong != null) fuKong.gameObject.SetActive(true);
            return;
        } 
        if (GameCenter.mercenaryMng.zhuPetId != MercenaryMng.noPet)
        {
            if (mixEx != null) mixEx.IsGray = UISpriteEx.ColorGray.normal;
        }
        if (fuPet != null) fuPet.gameObject.SetActive(true);
        if (fuKong != null) fuKong.gameObject.SetActive(false); 
        fuInfo = GameCenter.mercenaryMng.GetMercenaryById(GameCenter.mercenaryMng.fuPetId);  
        if (fuInfo != null)
        {
            fuUpZizhiBtn.gameObject.SetActive(true);
            fuUseEgg.gameObject.SetActive(true);
            fullFuApitude.gameObject.SetActive(false);
            if (fuInfo.Aptitude >= 100)
            {
                fuUpZizhiBtn.gameObject.SetActive(false);
                fuUseEgg.gameObject.SetActive(false);
                fullFuApitude.gameObject.SetActive(true); 
            }
            if (fuNameLab != null) fuNameLab.text = fuInfo.PetName;
            if (fuChengzhangLab != null) fuChengzhangLab.text = fuInfo.GrowUp.ToString();//成长
            if (fuZizhiLab != null) fuZizhiLab.text = fuInfo.Aptitude.ToString();//资质
            for (int j = 0; j < fuInfo.SoulPropertyList.Count; j++)
            {
                if (fuInfo.SoulPropertyList[j].type == (int)PetProperty.PETATT)
                {
                    if (fuAttLab != null) fuAttLab.text = fuInfo.SoulPropertyList[j].num.ToString();
                }
                if (fuInfo.SoulPropertyList[j].type == (int)PetProperty.PETHIT)
                {
                    if (fuHitLab != null) fuHitLab.text = fuInfo.SoulPropertyList[j].num.ToString();
                }
                if (fuInfo.SoulPropertyList[j].type == (int)PetProperty.PETCRI)
                {
                    if (fuCrazyHitLab != null) fuCrazyHitLab.text = fuInfo.SoulPropertyList[j].num.ToString();
                }
            }
            if (fuHeadSp != null) fuHeadSp.spriteName = fuInfo.Icon;
        }
        if (promoteApititudeUi != null && promoteApititudeUi.activeSelf)
            ShowZiZhiPillInfo(GameCenter.mercenaryMng.fuPetId);
    } 
    #endregion
}
