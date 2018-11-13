//==================================
//作者：朱素云
//日期：2016/3/20
//用途：宠物获得技能书ui
//=================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GetBookUi : MonoBehaviour {

    public UIButton copyBtn;//抄写
    public UIButton batchCopyBtn;//批量抄写
    public UILabel copyLab;
    public UILabel copyAllLab; 
    public UIToggle useGold;//材料不足使用元宝 
    public UIButton closeBtn;
    public UILabel diamondLab;
    /// <summary>
    /// 批量抄写获得的技能书
    /// </summary>
    protected FDictionary bookByCopyAll = new FDictionary(); 
    protected List<BookItem> bookByCopyAllList = new List<BookItem>();
    /// <summary>
    /// 服务端发来的抄写数据
    /// </summary>
    protected Dictionary<int, int> petSkillByCopyAll
    {
        get
        {
            return GameCenter.mercenaryMng.petSkillByCopyAll;
        }
    } 
    public GameObject parent;
    private EquipmentRef eqt = null;
    protected MainPlayerInfo MainPlayerInfo
    {
        get
        {
            return GameCenter.mainPlayerMng.MainPlayerInfo;
        }
    }
    protected BookItem bookItem = null;

    void Awake()
    {
        if (copyBtn != null)
        {
            UIEventListener.Get(copyBtn.gameObject).onClick -= OnClickCopyBtn;
            UIEventListener.Get(copyBtn.gameObject).onClick += OnClickCopyBtn;
        }
        if (batchCopyBtn != null)
        {
            UIEventListener.Get(batchCopyBtn.gameObject).onClick -= OnClickBatchCopyBtn;
            UIEventListener.Get(batchCopyBtn.gameObject).onClick += OnClickBatchCopyBtn;
        }
        if (closeBtn != null) UIEventListener.Get(closeBtn.gameObject).onClick = OnClickCloseBtn;  
    } 
    void OnEnable()
    {
        if(eqt == null)
            eqt = ConfigMng.Instance.GetEquipmentRef(2200001);
        ShowBook();
        GameCenter.mercenaryMng.OnCopyBookUpdate += ShowBook;
        GameCenter.inventoryMng.OnBackpackUpdate += ShowBook;
    }
    void OnDisable()
    {
        foreach (BookItem book in bookByCopyAll.Values)
        {
            book.gameObject.SetActive(false);
        }
        if (GameCenter.mercenaryMng.choosedSkillId != 0)
        { 
            petSkillByCopyAll.Clear();
            GameCenter.mercenaryMng.choosedSkillId = 0;
        }
        GameCenter.mercenaryMng.OnCopyBookUpdate -= ShowBook;
        GameCenter.inventoryMng.OnBackpackUpdate -= ShowBook;
    }
    /// <summary>
    /// 抄写
    /// </summary> 
    void OnClickCopyBtn(GameObject go)
    { 
        if (eqt != null && MainPlayerInfo != null)
        {
            if (GameCenter.inventoryMng.GetNumberByType(2200001) >= 1)
            { 
                GameCenter.mercenaryMng.C2S_ReqMercenaryInfo(PetChange.COPY, 0);
            }
            else
            {
                if (useGold != null && useGold.value)
                {
                    if (MainPlayerInfo.TotalDiamondCount >= eqt.diamonPrice)
                    {
                        GameCenter.mercenaryMng.C2S_ReqMercenaryInfo(PetChange.COPY, 1);
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
                else GameCenter.practiceMng.ReminderWnd(12, eqt.name);
            }
        }
    }
    /// <summary>
    /// 批量抄写
    /// </summary> 
    void OnClickBatchCopyBtn(GameObject go)
    {
        if (GameCenter.inventoryMng.GetNumberByType(2200001) >= 10 && useGold != null)
        {
            GameCenter.mercenaryMng.C2S_ReqMercenaryInfo(PetChange.COPYALL, (useGold.value == true) ? 1 : 0); 
        }
        else
        {
            if (useGold != null && useGold.value && MainPlayerInfo != null)
            {
                if (eqt != null && GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount >= eqt.diamonPrice * 10)
                {
                    GameCenter.mercenaryMng.C2S_ReqMercenaryInfo(PetChange.COPYALL, 1);
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
            else GameCenter.practiceMng.ReminderWnd(12,ConfigMng.Instance.GetUItext(268));
        }
    }
    void OnClickCloseBtn(GameObject go)
    {
        foreach (BookItem book in bookByCopyAll.Values)
        {
            book.gameObject.SetActive(false);
        }
        if (GameCenter.mercenaryMng.choosedSkillId != 0)
        { 
            petSkillByCopyAll.Clear();
            GameCenter.mercenaryMng.choosedSkillId = 0;
        }
        GameCenter.mercenaryMng.OnCopyBookUpdate -= ShowBook;
        GameCenter.inventoryMng.OnBackpackUpdate -= ShowBook;
        this.gameObject.SetActive(false);
    }
    /// <summary>
    /// 选择技能书到背包
    /// </summary> 
    void OnClickChooseToBag(GameObject go)
    { 
        if (GameCenter.mercenaryMng.choosedSkillId == 0)
        {
            BookItem bookitem = null;
            int id = (int)UIEventListener.Get(go.gameObject).parameter;
            if (bookByCopyAll.ContainsKey(id))
            {
                bookitem = bookByCopyAll[id] as BookItem;
                if (petSkillByCopyAll.ContainsKey(id))
                {
                    if (bookitem.chooseEx != null && bookitem.chooseEx.IsGray == UISpriteEx.ColorGray.normal)
                    {
                        GameCenter.mercenaryMng.choosedSkillId = id;
                        bookitem.chooseBtn.gameObject.SetActive(false);
                        bookitem.chooseYetBtn.gameObject.SetActive(true);
                        GameCenter.mercenaryMng.C2S_ReqAddBookToBag(id);
                    }
                }
            }
            ShowBook();
        }
    }
    void OnClickChooseOneToBag(GameObject go)
    { 
        if (GameCenter.mercenaryMng.choosedSkillId == 0)
        {
            int id = (int)UIEventListener.Get(go.gameObject).parameter;
            if (bookItem != null)
            {
                if (petSkillByCopyAll.ContainsKey(id))
                {
                    if (bookItem.chooseEx != null && bookItem.chooseEx.IsGray == UISpriteEx.ColorGray.normal)
                    {
                        GameCenter.mercenaryMng.choosedSkillId = id;
                        bookItem.chooseBtn.gameObject.SetActive(false);
                        bookItem.chooseYetBtn.gameObject.SetActive(true);
                        GameCenter.mercenaryMng.C2S_ReqAddBookToBag(id);
                    }
                }
            }
            ShowBook();
        }
    }
    #region 抄写单个
    void SetItemActive()
    {
        if (bookItem != null)
        {
            bookItem.gameObject.SetActive(false);
        }
    }
    void FillAItem(int _copyKey)
    {
        if (petSkillByCopyAll.ContainsKey(_copyKey))
        {
            int skillId = petSkillByCopyAll[_copyKey];
            if (bookItem == null)
            {
                bookItem = BookItem.CeateBook(0, parent, false);
                bookItem.gameObject.SetActive(true);
                bookItem.transform.localPosition = new Vector3(200, -50);
                ItemUI bookItemUI = bookItem.GetComponent<ItemUI>();
                bookItemUI.FillInfo(new EquipmentInfo(skillId, EquipmentBelongTo.PREVIEW));
                bookItem.transform.parent = parent.transform;
                bookItem.chooseBtn.gameObject.SetActive(true);
                bookItem.chooseYetBtn.gameObject.SetActive(false); 
                if (bookItem.chooseBtn != null)
                {
                    UIEventListener.Get(bookItem.chooseBtn.gameObject).onClick -= OnClickChooseOneToBag;
                    UIEventListener.Get(bookItem.chooseBtn.gameObject).onClick += OnClickChooseOneToBag;
                    UIEventListener.Get(bookItem.chooseBtn.gameObject).parameter = _copyKey;
                }
            }
            else
            {
                ItemUI bookItemUi = bookItem.GetComponent<ItemUI>();
                bookItemUi.FillInfo(new EquipmentInfo(skillId, EquipmentBelongTo.PREVIEW));
                bookItem.transform.parent = parent.transform;
                bookItem.chooseBtn.gameObject.SetActive(true);
                bookItem.chooseYetBtn.gameObject.SetActive(false);
                if (_copyKey == GameCenter.mercenaryMng.choosedSkillId)
                {
                    bookItem.chooseBtn.gameObject.SetActive(false);
                    bookItem.chooseYetBtn.gameObject.SetActive(true);
                }
                if (bookItem.chooseBtn != null)
                {
                    UIEventListener.Get(bookItem.chooseBtn.gameObject).onClick -= OnClickChooseOneToBag;
                    UIEventListener.Get(bookItem.chooseBtn.gameObject).onClick += OnClickChooseOneToBag;
                    UIEventListener.Get(bookItem.chooseBtn.gameObject).parameter = _copyKey;
                }
            }
        }
    }
    #endregion

    /// <summary>
    /// 获得的技能书(抄写成功)
    /// </summary>
    void ShowBook() 
    { 
        if (eqt == null)
        {
            eqt = ConfigMng.Instance.GetEquipmentRef(2200001);
        }
        if (diamondLab != null && eqt != null && MainPlayerInfo != null)
        {
            diamondLab.text = eqt.diamonPrice + "/" + MainPlayerInfo.TotalDiamondCount.ToString();
        }
        if (copyLab != null) copyLab.text = "1" + "/" + GameCenter.inventoryMng.GetNumberByType(2200001);
        if (copyAllLab != null) copyAllLab.text = "10" + "/" + GameCenter.inventoryMng.GetNumberByType(2200001); 
        foreach (BookItem book in bookByCopyAll.Values)
        { 
            book.gameObject.SetActive(false);  
        } 
        int len = petSkillByCopyAll.Count;  
        if (len > 0)
        {
            if (len == 1)//如果只抄写了一本书
            { 
                using (var key = petSkillByCopyAll.GetEnumerator())
                { 
                    while (key.MoveNext())
                    {
                        FillAItem(key.Current.Key); 
                    }
                } 
            }
            else//如果批量抄写
            {  
                int i = 0;
                SetItemActive();
                using (var key = petSkillByCopyAll.GetEnumerator())
                {
                    while (key.MoveNext())
                    {
                        int listlen = bookByCopyAllList.Count;
                        int skillId = petSkillByCopyAll[key.Current.Key];
                        if (listlen < 10)
                        {
                            BookItem item = BookItem.CeateBook(i, parent, false);
                            item.gameObject.SetActive(true);
                            ItemUI bookItem = item.GetComponent<ItemUI>();
                            bookItem.FillInfo(new EquipmentInfo(skillId, EquipmentBelongTo.PREVIEW));
                            bookByCopyAll[key.Current.Key] = item;
                            bookByCopyAllList.Add(item);
                            item.chooseBtn.gameObject.SetActive(true);
                            item.chooseYetBtn.gameObject.SetActive(false);
                            if (item.chooseEx != null) item.chooseEx.IsGray = UISpriteEx.ColorGray.normal;
                            if (GameCenter.mercenaryMng.choosedSkillId != 0 && key.Current.Key != GameCenter.mercenaryMng.choosedSkillId)
                            {
                                if (item.chooseEx != null) item.chooseEx.IsGray = UISpriteEx.ColorGray.Gray;
                            }
                            else
                            {
                                if (key.Current.Key == GameCenter.mercenaryMng.choosedSkillId)
                                {
                                    item.chooseBtn.gameObject.SetActive(false);
                                    item.chooseYetBtn.gameObject.SetActive(true);
                                }
                            }
                            if (item.chooseBtn != null)
                            {
                                UIEventListener.Get(item.chooseBtn.gameObject).onClick -= OnClickChooseToBag;
                                UIEventListener.Get(item.chooseBtn.gameObject).onClick += OnClickChooseToBag;
                                UIEventListener.Get(item.chooseBtn.gameObject).parameter = key.Current.Key;
                            }
                            i++;
                        }
                        else
                        {
                            if (listlen > i)
                            {
                                BookItem item = bookByCopyAllList[i];
                                item.gameObject.SetActive(true);
                                ItemUI bookItem = item.GetComponent<ItemUI>();
                                bookItem.FillInfo(new EquipmentInfo(skillId, EquipmentBelongTo.PREVIEW));
                                bookByCopyAll[key.Current.Key] = item;
                                bookByCopyAllList.Add(item);
                                item.chooseBtn.gameObject.SetActive(true);
                                item.chooseYetBtn.gameObject.SetActive(false);
                                if (item.chooseEx != null) item.chooseEx.IsGray = UISpriteEx.ColorGray.normal;
                                if (GameCenter.mercenaryMng.choosedSkillId != 0 && key.Current.Key != GameCenter.mercenaryMng.choosedSkillId)
                                {
                                    if (item.chooseEx != null) item.chooseEx.IsGray = UISpriteEx.ColorGray.Gray;
                                }
                                else
                                {
                                    if (key.Current.Key == GameCenter.mercenaryMng.choosedSkillId)
                                    {
                                        item.chooseBtn.gameObject.SetActive(false);
                                        item.chooseYetBtn.gameObject.SetActive(true);
                                    }
                                }
                                if (item.chooseBtn != null)
                                {
                                    UIEventListener.Get(item.chooseBtn.gameObject).onClick -= OnClickChooseToBag;
                                    UIEventListener.Get(item.chooseBtn.gameObject).onClick += OnClickChooseToBag;
                                    UIEventListener.Get(item.chooseBtn.gameObject).parameter = key.Current.Key;
                                }
                                i++;
                            } 
                        } 
                    }
                } 
            } 
        } 
    }
}
