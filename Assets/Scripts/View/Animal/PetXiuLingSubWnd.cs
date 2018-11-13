//==================================
//作者：朱素云
//日期：2016/3/18
//用途：宠物灵修界面
//=================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PetXiuLingSubWnd : SubWnd
{
    #region 数据 
    public UIButton getMat;//获得材料至单人副本界面 
    public UIButton tianHunTrainBtn;//修炼天魂按钮
    public UIButton diHunTrainBtn;//修炼地魂
    public UIButton mingHunTrainBtn;//修炼命魂  
    public List<UISpriteEx> tianHunStarList = new List<UISpriteEx>();//天魂星星
    public List<UISpriteEx> diHunStarList = new List<UISpriteEx>();//地魂星星
    public List<UISpriteEx> mingHunStarList = new List<UISpriteEx>();//命魂星星 
    public List<UITexture> expSp = new List<UITexture>(); 
    public UILabel AttLabAfterTrainTH;//修炼天魂增加的 经验
    public UILabel HitLabAfterTrainDH;//修炼地魂增加的经验
    public UILabel CrazyHitLabAfterTrainMH;//修炼命魂增加的经验
    public UILabel []expRatio; 
    public UIToggle isAutomaticBuy;//是否开启自动购买 
    public MercenaryInfo curInfo;//当前宠物   
    public UILabel lxPillLab;//灵修丹
    public UILabel coinLab;
    public UILabel diamondLab;
    protected int itemId = 0;
    protected EquipmentRef itemRef = null;
    protected MainPlayerInfo MainPlayerInfo
    {
        get
        {
            return GameCenter.mainPlayerMng.MainPlayerInfo;
        }
    }
    public Transform tianfullLev;//满级
    public Transform difullLev;
    public Transform mingfullLev;  
    /// <summary>
    /// 天魂特效
    /// </summary>
    public PracticeEffect tianEffect;
    /// <summary>
    /// 地魂特效
    /// </summary>
    public PracticeEffect diEffect;
    /// <summary>
    /// 命魂特效
    /// </summary>
    public PracticeEffect lifeEffect;

    public UISprite tianRedRemind;
    public UISprite diRedRemind;
    public UISprite mingRedRemind;
    #endregion

    void Awake()
    {
        if (getMat != null) UIEventListener.Get(getMat.gameObject).onClick = delegate
            {
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
        if (tianHunTrainBtn != null) UIEventListener.Get(tianHunTrainBtn.gameObject).onClick = OnClickTianHunTrainBtn;
        if (diHunTrainBtn != null) UIEventListener.Get(diHunTrainBtn.gameObject).onClick = OnClickDiHunTrainBtn;
        if (mingHunTrainBtn != null) UIEventListener.Get(mingHunTrainBtn.gameObject).onClick = OnClickMingHunTrainBtn;  
    } 
    protected override void OnOpen()
    {
        base.OnOpen(); 
        RefreshXiuLingInfo(); 
    } 
    protected override void OnClose()
    {
        base.OnClose(); 
    }
    protected override void HandEvent(bool _bind)
    {
        base.HandEvent(_bind);
        if (_bind)
        { 
            GameCenter.mercenaryMng.OnMercenaryListUpdate += RefreshXiuLingInfo;
            GameCenter.inventoryMng.OnBackpackUpdate += RefreshXiuLingInfo;
            GameCenter.mercenaryMng.OnLingXiuExpUpdate += ExpUpdate;
            GameCenter.mercenaryMng.OnLingXiuLevUpdate += LevUpdate;
            GameCenter.mercenaryMng.OnLingXiuStarUpdate += RefreshStar; 
        }
        else
        { 
            GameCenter.mercenaryMng.OnMercenaryListUpdate -= RefreshXiuLingInfo;
            GameCenter.inventoryMng.OnBackpackUpdate -= RefreshXiuLingInfo;
            GameCenter.mercenaryMng.OnLingXiuExpUpdate -= ExpUpdate;
            GameCenter.mercenaryMng.OnLingXiuLevUpdate -= LevUpdate;
            GameCenter.mercenaryMng.OnLingXiuStarUpdate -= RefreshStar;
        }
    }
    void ExpUpdate(LingXiuType _type)
    {
        switch (_type)
        { 
            case LingXiuType.TIAN:
                if (tianEffect != null) tianEffect.ShowLXEffect(); 
                break;
            case LingXiuType.DI:
                if (diEffect != null) diEffect.ShowLXEffect();
                break;
            case LingXiuType.LIFE:
                if (lifeEffect != null) lifeEffect.ShowLXEffect();
                break;
        }
    }
    void LevUpdate(LingXiuType _type)
    {
        if (curInfo != null)
        {
            switch (_type)
            {
                case LingXiuType.TIAN:
                    if (tianEffect != null)
                    {
                        if (tianHunStarList.Count > (curInfo.Tian_soul - 1) % 8)
                        {
                            tianEffect.ShowLXEffect(tianHunStarList[(curInfo.Tian_soul - 1) % 8].transform.localPosition, LingXiuType.TIAN);
                        }
                    }
                    break;
                case LingXiuType.DI:
                    if (diEffect != null)
                    {
                        if (diHunStarList.Count > (curInfo.Di_soul - 1) % 8)
                        { 
                            diEffect.ShowLXEffect(diHunStarList[(curInfo.Di_soul - 1) % 8].transform.localPosition, LingXiuType.DI);
                        } 
                    }
                    break;
                case LingXiuType.LIFE:
                    if (lifeEffect != null)
                    {
                        if (mingHunStarList.Count > (curInfo.Life_soul - 1) % 8)
                        {
                            lifeEffect.ShowLXEffect(mingHunStarList[(curInfo.Life_soul - 1) % 8].transform.localPosition, LingXiuType.LIFE);
                        }  
                    }
                    break;
            }
        }
    }
    /// <summary>
    /// 修炼天魂(修炼物品都一样（钱和修炼丹），都用的地魂物品)
    /// </summary> 
    void OnClickTianHunTrainBtn(GameObject go)
    { 
       if (curInfo != null) CheckCondition(PetChange.PRACTICETIANSOUL); 
    }
    /// <summary>
    /// 修炼地魂
    /// </summary> 
    void OnClickDiHunTrainBtn(GameObject go)
    { 
       if (curInfo != null) CheckCondition(PetChange.PRACTICEDISOUL); 
    }
    /// <summary>
    /// 修炼命魂
    /// </summary> 
    void OnClickMingHunTrainBtn(GameObject go)
    { 
       if (curInfo != null) CheckCondition(PetChange.PRACTICELIFESOUL); 
    } 
    /// <summary>
    /// 判断物品是否充足
    /// </summary>
    void CheckCondition(PetChange _id)
    { 
        //int id = 0;
        //if (_id == PetChange.PRACTICETIANSOUL) id = curInfo.Tian_soul;
        //else if (_id == PetChange.PRACTICEDISOUL) id = curInfo.Di_soul;
        //else id = curInfo.Life_soul; 
        if (curInfo != null)
        {
            if (curInfo.LxItem.Count > 0)
            {
                int coinNum = 0;
                int itemNum = 0;
                for (int i = 0; i < curInfo.LxItem.Count; i++)
                {
                    if (curInfo.LxItem[i].eid == 5)
                    {
                        coinNum = curInfo.LxItem[i].count;
                    }
                    else
                    {
                        itemNum = curInfo.LxItem[i].count;
                    }
                }
                if (MainPlayerInfo != null && MainPlayerInfo.TotalCoinCount >= (ulong) coinNum)
                {
                    if (GameCenter.inventoryMng.GetNumberByType(itemId) < itemNum && itemRef != null)//灵修丹 2200005
                    {
                        if (isAutomaticBuy != null && isAutomaticBuy.value)
                        {
                            if (MainPlayerInfo.TotalDiamondCount >= itemRef.diamonPrice)
                            {
                                GameCenter.mercenaryMng.C2S_ReqPromote(_id, GameCenter.mercenaryMng.curPetId, (isAutomaticBuy.value == true) ? 1 : 0);
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
                            mst.words = new string[2] { itemRef.diamonPrice.ToString(), itemRef.name };
                            mst.delYes = delegate
                            {
                                if (MainPlayerInfo.TotalDiamondCount >= itemRef.diamonPrice)
                                    GameCenter.mercenaryMng.C2S_ReqPromote(_id, GameCenter.mercenaryMng.curPetId, 1);
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
                        GameCenter.mercenaryMng.C2S_ReqPromote(_id, GameCenter.mercenaryMng.curPetId, 0);
                }
                else
                {
                    GameCenter.messageMng.AddClientMsg(155);
                }
            }
        }
    } 
    /// <summary>
    /// 星星颜色变化(绿8、蓝16、紫24、橙32、红40、五色)
    /// </summary> 
    void ShowStar(List<UISpriteEx> _starList, int _num)
    {
        for (int val = 0; val < 32; val += 8)
        {
            StarTypeRef lastStarTypeRef = ConfigMng.Instance.GetStarTypeRef((val / 8 > 0) ? (val / 8) : 1);
            StarTypeRef starTypeRef = ConfigMng.Instance.GetStarTypeRef(val / 8 + 1);
            if (_num > val && _num <= val + 8 && lastStarTypeRef != null && starTypeRef != null) 
            {
                for (int i = 0; i < _num - val; i++)
                {
                    _starList[i].spriteName = starTypeRef.icon;
                    _starList[i].IsGray = UISpriteEx.ColorGray.normal;
                }
                for (int i = _num - val; i < 8; i++)
                {
                    _starList[i].IsGray = UISpriteEx.ColorGray.normal;
                    if (val / 8 > 0) _starList[i].spriteName = lastStarTypeRef.icon;
                    else _starList[i].IsGray = UISpriteEx.ColorGray.Gray;
                }
            }
        }
    }  
    void NoStar(List<UISpriteEx> _starList)
    {
        StarTypeRef starTypeRef = ConfigMng.Instance.GetStarTypeRef(1);
        if (starTypeRef != null)
        {
            for (int i = 0; i < _starList.Count; i++)
            {
                _starList[i].spriteName = ConfigMng.Instance.GetStarTypeRef(1).icon;
                _starList[i].IsGray = UISpriteEx.ColorGray.Gray;
            }
        } 
    }
    void RefreshStar(LingXiuType _type)
    {
        switch (_type)
        {
            case LingXiuType.TIAN:
                ShowStar(tianHunStarList, curInfo.Tian_soul);
                break;
            case LingXiuType.DI:
                ShowStar(diHunStarList, curInfo.Di_soul);
                break;
            case LingXiuType.LIFE:
                ShowStar(mingHunStarList, curInfo.Life_soul);
                break;
        }
    }
    /// <summary>
    /// 刷新界面
    /// </summary>
    void RefreshXiuLingInfo()
    { 
        FDictionary mercenaryInfoList = GameCenter.mercenaryMng.mercenaryInfoList; 
        if (mercenaryInfoList.Count <= 0)
        {
            if (AttLabAfterTrainTH != null) AttLabAfterTrainTH.text = "0";
            tianRedRemind.gameObject.SetActive(false);
            tianfullLev.gameObject.SetActive(false);
            tianHunTrainBtn.gameObject.SetActive(false);
            if (expSp.Count > 2 && expRatio.Length > 2)
            {
                expSp[0].fillAmount = 0;
                expRatio[0].text = "0";
                expSp[1].fillAmount = 0;
                expRatio[1].text = "0";
                expSp[2].fillAmount = 0;
                expRatio[2].text = "0";
            }
            if (HitLabAfterTrainDH != null) HitLabAfterTrainDH.text = "0";
            if (CrazyHitLabAfterTrainMH != null) CrazyHitLabAfterTrainMH.text = "0";
            if (coinLab != null) coinLab.text = "0";
            if (lxPillLab != null) lxPillLab.text = "0";
            NoStar(mingHunStarList);
            NoStar(tianHunStarList);
            NoStar(diHunStarList);
            return;
        }
        if (curInfo == null || (curInfo != null && curInfo.ConfigId != GameCenter.mercenaryMng.curPetId))
        {
            curInfo = GameCenter.mercenaryMng.GetMercenaryById(GameCenter.mercenaryMng.curPetId); 
        } 
        if (curInfo != null)
        {
            ShowStar(tianHunStarList, curInfo.Tian_soul);
            ShowStar(diHunStarList, curInfo.Di_soul);
            ShowStar(mingHunStarList, curInfo.Life_soul);
            NewPetDataRef tHPetData = ConfigMng.Instance.GetPetDataRef(curInfo.Tian_soul > 0 ? curInfo.Tian_soul + 1 : 1); 
            NewPetDataRef dHPetData = ConfigMng.Instance.GetPetDataRef(curInfo.Di_soul > 0 ? curInfo.Di_soul + 1 : 1);  
            NewPetDataRef lifePetData = ConfigMng.Instance.GetPetDataRef(curInfo.Life_soul > 0 ? curInfo.Life_soul + 1 : 1); 
            if (curInfo.Tian_soul <= 0) NoStar(tianHunStarList);
            if (curInfo.Di_soul <= 0)  NoStar(diHunStarList);
            if (curInfo.Life_soul <= 0) NoStar(mingHunStarList);
            bool isCoinEnough = false;
            bool isItemEnough = false;
            if (curInfo.LxItem.Count > 0)
            {
                for (int i = 0; i < curInfo.LxItem.Count; i++)
                {
                    if (curInfo.LxItem[i].eid == 5)
                    {
                        if (MainPlayerInfo != null)
                        {

                            if (MainPlayerInfo.TotalCoinCount < (ulong)curInfo.LxItem[i].count)
                            {
                                coinLab.text = curInfo.LxItem[i].count + "/" + "[ff0000]" + MainPlayerInfo.TotalCoinCount;
                            }
                            else
                            {
                                isCoinEnough = true;
                                coinLab.text = curInfo.LxItem[i].count + "/" + "[6ef574]" + MainPlayerInfo.TotalCoinCount;
                            }
                        }
                    }
                    else
                    {
                        itemId = curInfo.LxItem[i].eid;
                        itemRef = ConfigMng.Instance.GetEquipmentRef(itemId);
                        if (GameCenter.inventoryMng.GetNumberByType(curInfo.LxItem[i].eid) >= curInfo.LxItem[i].count)
                        {
                            isItemEnough = true;
                            if (lxPillLab != null) lxPillLab.text = curInfo.LxItem[i].count + "/" + "[6ef574]" + GameCenter.inventoryMng.GetNumberByType(curInfo.LxItem[i].eid);
                        }
                        else
                        {
                            if (lxPillLab != null) lxPillLab.text = curInfo.LxItem[i].count + "/" + "[ff0000]" + GameCenter.inventoryMng.GetNumberByType(curInfo.LxItem[i].eid);
                        }
                    }
                }
            }
            if (MainPlayerInfo != null)
            {
                if (!isItemEnough)
                {
                    if (itemRef != null && diamondLab != null) diamondLab.text = itemRef.diamonPrice + "/" + MainPlayerInfo.TotalDiamondCount.ToString();
                }
                else
                {
                    if (diamondLab != null) diamondLab.text = "0/" + MainPlayerInfo.TotalDiamondCount.ToString();
                }
            } 
            if (curInfo.Tian_soul >= 32)
            {
                tianRedRemind.gameObject.SetActive(false); 
                tianfullLev.gameObject.SetActive(true);
                tianHunTrainBtn.gameObject.SetActive(false);
            }
            else
            { 
                if (isCoinEnough && isItemEnough)
                {
                    tianRedRemind.gameObject.SetActive(true); 
                }
                else
                {
                    tianRedRemind.gameObject.SetActive(false); 
                } 
                tianfullLev.gameObject.SetActive(false);
                tianHunTrainBtn.gameObject.SetActive(true);
            }
            if (curInfo.Di_soul >= 32)
            {
                diRedRemind.gameObject.SetActive(false); 
                difullLev.gameObject.SetActive(true);
                diHunTrainBtn.gameObject.SetActive(false);
            }
            else
            {
                difullLev.gameObject.SetActive(false);
                diHunTrainBtn.gameObject.SetActive(true);
                if (isCoinEnough && isItemEnough)
                { 
                    diRedRemind.gameObject.SetActive(true); 
                }
                else
                { 
                    diRedRemind.gameObject.SetActive(false); 
                } 
            }
            if (curInfo.Life_soul >= 32)
            {
                mingRedRemind.gameObject.SetActive(false);
                mingfullLev.gameObject.SetActive(true);
                mingHunTrainBtn.gameObject.SetActive(false);
            }
            else
            {
                mingfullLev.gameObject.SetActive(false);
                mingHunTrainBtn.gameObject.SetActive(true);
                if (isCoinEnough && isItemEnough)
                { 
                    mingRedRemind.gameObject.SetActive(true);
                }
                else
                { 
                    mingRedRemind.gameObject.SetActive(false);
                } 
            } 
           
            for (int j = 0, max = curInfo.SoulPropertyList.Count; j < max; j++)
            {
                if (curInfo.SoulPropertyList[j].type == (int)PetProperty.PETATT && tHPetData != null)
                { 
                    float attr = curInfo.SoulPropertyList[j].num;
                    float needVal = 0;
                    if (tHPetData.lXExp.Count > 0 && curInfo.Tian_soul < 32)needVal = tHPetData.lXExp[0];
                    else needVal = attr; 
                    float posy = -128;
                    if (needVal != 0) posy = -128 + (attr / needVal) * 136;
                    if (AttLabAfterTrainTH != null) AttLabAfterTrainTH.text = (attr).ToString();
                    if (expSp.Count > 0) expSp[0].transform.localPosition = new Vector3
                        (expSp[0].transform.localPosition.x, posy, 0);
                    if (expRatio.Length > 0) expRatio[0].text = attr + "/" + needVal; 
                }
                if (curInfo.SoulPropertyList[j].type == (int)PetProperty.PETHIT && dHPetData != null)
                {
                    float val = curInfo.SoulPropertyList[j].num;
                    float needVal = 0;
                    if (dHPetData.lXExp.Count > 1 && curInfo.Di_soul< 32) needVal = dHPetData.lXExp[1];
                    else needVal = val; 
                    float posy = -128;
                    if (needVal != 0) posy = -128 + (val / needVal) * 136;
                    if (HitLabAfterTrainDH != null) HitLabAfterTrainDH.text = val.ToString();
                    if (expSp.Count > 1) expSp[1].transform.localPosition = new Vector3
                        (expSp[1].transform.localPosition.x, posy, 0);
                    if (expRatio.Length > 1) expRatio[1].text = val + "/" + needVal;
                }
                if (curInfo.SoulPropertyList[j].type == (int)PetProperty.PETCRI && lifePetData != null)
                {
                    float val = curInfo.SoulPropertyList[j].num;
                    float needVal = 0;
                    if (lifePetData.lXExp.Count > 2 && curInfo.Life_soul < 32) needVal = lifePetData.lXExp[2];
                    else needVal = val; 
                    float posy = -128;
                    if (needVal != 0) posy= -128+ (val / needVal) * 136; 
                    if (CrazyHitLabAfterTrainMH != null) CrazyHitLabAfterTrainMH.text = val.ToString();
                    if (expSp.Count > 2) expSp[2].transform.localPosition = new Vector3
                    (expSp[2].transform.localPosition.x, posy, 0);
                    if (expRatio.Length > 2) expRatio[2].text = val + "/" + needVal; 
                }
            }      
        }
    } 
}
