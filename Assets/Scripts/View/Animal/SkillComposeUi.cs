//==================================
//作者：朱素云
//日期：2016/3/29
//用途：宠物技能合成ui
//=================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;
using System.Text;

public class SkillComposeUi : MonoBehaviour
{  
    #region 数据
    public UIButton closeBtn;
    public UIButton composeBtn;//合成
    public UIButton putInBagBtn;//一键放入
    public UIButton  composeBookBtn;  
    public UILabel composeMatLab;//合成需要的材料 （材料名 × 数量） 
    public GameObject composeBookUi;//能够合成的所有技能书
    /// <summary>
    /// 背包中的物品（key 高级顶级技能书id, value存放bookitem）
    /// </summary>
    protected FDictionary bookInBag = new FDictionary();
    public List<ItemUI> matItem = new List<ItemUI>();
    public GameObject parent; 
    public GameObject bookParent; 
    public List<UILabel> skillLevDes = new List<UILabel>();
    public UIButton  closeBookBtn; 
    protected List<int> choosedBookInMat
    {
        get
        {
            return GameCenter.mercenaryMng.choosedBookInMat;
        }
    }
    public List<int> choosedPetSkill
    {
        get
        {
            return GameCenter.mercenaryMng.choosedPetSkill;
        }
    }
    //protected List<NewPetSkillRef> seniorSkill { get { return GameCenter.mercenaryMng.seniorSkill; } }
    //protected List<NewPetSkillRef> topSkill { get { return GameCenter.mercenaryMng.topSkill; } }

    protected List<NewPetSkillRef> seniorSkill = new List<NewPetSkillRef>();
    protected List<NewPetSkillRef> topSkill = new List<NewPetSkillRef>();
    private int composeBookId = 0;//需要合成的技能书id
    private int needBookNum = 0;//合成技能书需要的技能书数
    private int needLev = 0;//合成技能书需要的技能书等级

    public GameObject noNeedBook;
    #endregion

    void Awake()
    {
        if (closeBtn != null) UIEventListener.Get(closeBtn.gameObject).onClick = OnClickCloseBtn;
        if (composeBtn != null) UIEventListener.Get(composeBtn.gameObject).onClick = OnClickComposeBtn;
        if (putInBagBtn != null) UIEventListener.Get(putInBagBtn.gameObject).onClick = OnClickPutInBagBtn;
        if (closeBookBtn != null) UIEventListener.Get(closeBookBtn.gameObject).onClick = OnClickCloseBookBtn;
        if (composeBookBtn != null)
        {
            UIEventListener.Get(composeBookBtn.gameObject).onClick -= OnClickComposeBookBtn;
            UIEventListener.Get(composeBookBtn.gameObject).onClick += OnClickComposeBookBtn;
        }
        composeBookUi.gameObject.SetActive(false);
        composeMatLab.gameObject.SetActive(false);
        seniorSkill = ConfigMng.Instance.GetDividedSkillByLev(3);
        topSkill = ConfigMng.Instance.GetDividedSkillByLev(4);
    }
    void OnEnable()
    {  
        GameCenter.mercenaryMng.GetBookFromBag();
        GameCenter.mercenaryMng.OnSeleteUpdate += ChooseBookToCompose;
        GameCenter.mercenaryMng.OnSeleteUpdate += CloseBookBtn;
        GameCenter.mercenaryMng.OnMercenaryListUpdate += ShowComposeUi;
    }
    void OnDisable() 
    {
        GameCenter.mercenaryMng.OnSeleteUpdate -= ChooseBookToCompose;
        GameCenter.mercenaryMng.OnSeleteUpdate -= CloseBookBtn;
        GameCenter.mercenaryMng.OnMercenaryListUpdate -= ShowComposeUi; 
    }
    /// <summary>
    ///关闭技能书
    /// </summary> 
    void OnClickCloseBookBtn(GameObject go)
    {
        composeBookUi.gameObject.SetActive(false);
    }
    void CloseBookBtn()
    {
        composeBookUi.gameObject.SetActive(false);
    }
    /// <summary>
    /// 显示技能书
    /// </summary>
    void OnClickComposeBookBtn(GameObject go)
    { 
        composeBookUi.gameObject.SetActive(true); 
        ItemUI item = composeBookBtn.GetComponent<ItemUI>();
        if (item != null)
        {
            item.ShowTooltip = false;
        }
        //高级
        if (seniorSkill.Count <= 0)
        {
            if (skillLevDes.Count > 2) skillLevDes[2].gameObject.SetActive(false);
        }
        else
        {
            if (skillLevDes.Count > 2)
            {
                skillLevDes[2].gameObject.SetActive(true);
                skillLevDes[2].transform.parent = bookParent.transform;
                skillLevDes[2].transform.localPosition = new Vector3(222, 30);
            }
            for (int i = 0; i < seniorSkill.Count; i++)
            {
                CreateBook(i, seniorSkill[i].book[0].eid);
            }
        }
        //顶级
        if (topSkill.Count <= 0)
        {
            if (skillLevDes.Count > 3) skillLevDes[3].gameObject.SetActive(false);
        }
        else
        {
            if (skillLevDes.Count > 3)
            {
                skillLevDes[3].gameObject.SetActive(true);
                skillLevDes[3].transform.parent = bookParent.transform;
                skillLevDes[3].transform.localPosition = new Vector3(222, -seniorSkill.Count / 4 * 120 - (60 + 120));
            }
            for (int i = 0; i < topSkill.Count; i++)
            {
                CreateBook(seniorSkill.Count-1 + i + 8, topSkill[i].book[0].eid);
            }
        }  
    }
    void CreateBook(int _index, int _id)
    { 
        BookItem item = BookItem.CeateBook(_index, bookParent, true);
        item.gameObject.SetActive(true); 
        item.chooseBtn.gameObject.SetActive(false);
        item.chooseYetBtn.gameObject.SetActive(false);
        ItemUI bookItem = item.GetComponent<ItemUI>(); 
        bookItem.FillInfo(new EquipmentInfo(_id, EquipmentBelongTo.PREVIEW));
        bookItem.SetActionBtn(ItemActionType.None, ItemActionType.None, ItemActionType.SelectAdd); 
     }
    /// <summary>
    /// 选择合成的技能书
    /// </summary>
    void ChooseBookToCompose()
    {
        ItemUI item = composeBookBtn.GetComponent<ItemUI>();
        if (item == null) return;

        if (item.EQInfo == null || item.EQInfo != GameCenter.mercenaryMng.seleteEquip)
        { 
            item.FillInfo(GameCenter.mercenaryMng.seleteEquip);

            for (int i = 0; i < choosedBookInMat.Count; i++)
            {
                if (!bookInBag.ContainsKey(choosedBookInMat[i]))
                    choosedPetSkill.Add(choosedBookInMat[i]);
            }
            choosedBookInMat.Clear();
        }
        ShowComposeUi();
    } 
    /// <summary>
    /// 技能书合成界面
    /// </summary>
    public void ShowComposeUi()
    {
        if (GameCenter.mercenaryMng.isComposedBook)
        {
            choosedBookInMat.Clear();
            GameCenter.mercenaryMng.isComposedBook = false;//技能合成清空
            composeBookId = 0;
            ItemUI item = composeBookBtn.GetComponent<ItemUI>();
            if (item != null)
                item.SetEmpty();
        }
        if (GameCenter.mercenaryMng.seleteEquip != null)
        {
            composeBookId = GameCenter.mercenaryMng.seleteEquip.EID;
            NewPetSkillComposeRef composeRef = ConfigMng.Instance.GetPetSkillComposeRef(composeBookId);
            if (composeRef != null && composeBookId != 0)
            {
                composeMatLab.gameObject.SetActive(true);
                needBookNum = composeRef.needNum;
                needLev = composeRef.needLevel;
                if (composeRef.needLevel == 3)
                    composeMatLab.text = ConfigMng.Instance.GetUItext(35, new string[1] { "[6ef574]" + needBookNum });
                if (composeRef.needLevel == 4)
                    composeMatLab.text = ConfigMng.Instance.GetUItext(36, new string[1] { "[6ef574]" + needBookNum });
            }
        }
        else composeMatLab.text = " ";
        ShowBookInMat();
        ShowBookInBag();
    }
    /// <summary>
    /// 点击技能，放入材料框
    /// </summary> 
    void OnClickBook(GameObject go)
    { 
        int id = (int)UIEventListener.Get(go.GetComponent<UIButton>().gameObject).parameter;
        if(go.GetComponent<BookItem>().curType == ChooseType.GETMAT)choosedBookInMat.Add(id); 
        ShowBookInMat();
    }
    /// <summary>
    /// 合成按钮
    /// </summary> 
    void OnClickComposeBtn(GameObject go)
    {  
        if (GameCenter.mercenaryMng.seleteEquip != null)
        {
            int num = 0; 
            for (int i = 0; i < choosedBookInMat.Count; i++)
            {
                EquipmentRef eqt = ConfigMng.Instance.GetEquipmentRef(choosedBookInMat[i]);
                if (eqt != null && eqt.psetSkillLevel >= needLev)
                {
                    ++num;
                }
            }
            if (num >= needBookNum)
            {
                GameCenter.mercenaryMng.C2S_ReqComposeSkill(composeBookId, choosedBookInMat);
            }
            else//材料不足
            {
                MessageST mst = new MessageST();
                mst.messID = 12;
                mst.words = new string[1] { "材料"};
                GameCenter.messageMng.AddClientMsg(mst);
            }
        }
    }
    /// <summary>
    /// 一键放入
    /// </summary> 
    void OnClickPutInBagBtn(GameObject go)
    {
         ItemUI choodeitem = composeBookBtn.GetComponent<ItemUI>();
         if (choodeitem != null && choodeitem.EQInfo != null && choosedBookInMat.Count < needBookNum)//选择了要合成的技能书了才可点击
         { 
             foreach (int bookId in bookInBag.Keys)
             {
                 EquipmentRef info = ConfigMng.Instance.GetEquipmentRef(bookId);
                 NewPetSkillRef petskill = ConfigMng.Instance.GetPetSkillRefByBook(bookId);//合成材料
                 NewPetSkillRef composeskill = ConfigMng.Instance.GetPetSkillRefByBook(composeBookId);//要合成的书
                 int num = 0;
                 for (int j = 0; j < choosedBookInMat.Count; j++)
                 {
                     if (bookId == choosedBookInMat[j])
                     {
                         ++num;
                     }
                 }
                 int val = GameCenter.inventoryMng.GetNumberByType(bookId) - num; 
                 if (info != null && petskill != null)
                 {
                     if (petskill.kind != composeskill.kind && info.psetSkillLevel >= needLev)//不同类别、且材料等级要高
                     { 
                         while (val > 0 && choosedBookInMat.Count < needBookNum)//判断满足条件的书数量，满足添加到材料链表
                         {
                             choosedBookInMat.Add(bookId);
                             --val; 
                         }
                         if (val <= 0)//如果书都添加到材料链表就从背包移出该书
                             choosedPetSkill.Remove(bookId);
                     }
                 } 
             }
             ShowBookInMat();
             ShowBookInBag();
         }
    }
    /// <summary>
    /// 显示材料框中的技能书
    /// </summary>
    void ShowBookInMat()
    { 
        ItemUI item = composeBookBtn.GetComponent<ItemUI>(); 
        if (item != null)
        {
            for (int i = 0; i < matItem.Count; i++)
            {
                if (matItem[i].putMat != null) matItem[i].putMat.gameObject.SetActive(false);
                if (matItem[i].lockFlag != null) matItem[i].lockFlag.gameObject.SetActive(false);
                matItem[i].FillInfo(null);
                if (item != null && item.EQInfo != null)
                {
                    if (i < needBookNum) matItem[i].putMat.gameObject.SetActive(true);
                    else
                        matItem[i].lockFlag.gameObject.SetActive(true);
                }
            }
            for (int i = 0; i < choosedBookInMat.Count; i++)
            {
                if (i < needBookNum)
                {
                    int id = choosedBookInMat[i];
                    matItem[i].FillInfo(new EquipmentInfo(id, EquipmentBelongTo.PREVIEW));
                    matItem[i].ShowTooltip = false;//关闭热感
                    UIButton button = matItem[i].GetComponent<UIButton>();
                    if (button != null)
                    {
                        UIEventListener.Get(button.gameObject).onClick -= OnClickMat;//将材料移出至背包中
                        UIEventListener.Get(button.gameObject).onClick += OnClickMat;
                        UIEventListener.Get(button.gameObject).parameter = id;//技能书id
                    }
                    matItem[i].itemName.gameObject.SetActive(false);
                    matItem[i].putMat.gameObject.SetActive(false);
                    matItem[i].lockFlag.gameObject.SetActive(false);
                }
            }
        }
    }
    /// <summary>
    /// 选择背包中的技能书放入材料框
    /// </summary> 
    void OnClickChooseToMat(GameObject go)
    {
        ItemUI choodeitem = composeBookBtn.GetComponent<ItemUI>();
        if (choodeitem != null && choodeitem.EQInfo != null)//选择了要合成的技能书了才可点击
        { 
            int id = (int)UIEventListener.Get(go.gameObject).parameter;
            if (bookInBag.ContainsKey(id))
            {
                if (choosedBookInMat.Count < needBookNum)
                {
                    choosedBookInMat.Add(id);
                    int num = 0;
                    for (int j = 0; j < choosedBookInMat.Count; j++)
                    {
                        if (id == choosedBookInMat[j])
                        {
                            ++num;
                        }
                    }
                    if (GameCenter.inventoryMng.GetNumberByType(id)-num <= 0)
                        choosedPetSkill.Remove(id);//将选中了的书移出一个背包
                }
                ShowBookInMat();
                ShowBookInBag();
            } 
        }
    }
    /// <summary>
    /// 将材料移出至背包中
    /// </summary>
    void OnClickMat(GameObject go)
    {
        int id = (int)UIEventListener.Get(go.gameObject).parameter;
        choosedBookInMat.Remove(id);
        bool iscontain = false;
        for (int i = 0; i< choosedPetSkill.Count; i++)
        {
            if (id == choosedPetSkill[i])
            {
                iscontain = true;
                break;
            }
        }
        if (!iscontain) 
            choosedPetSkill.Add(id);
        ShowBookInMat();
        ShowBookInBag();
    } 
    /// <summary>
    /// 显示背包中的高级、顶级技能书
    /// </summary>
    void ShowBookInBag()
    { 
        int lenth = choosedPetSkill.Count;
        if (lenth <= 0)
        {
            if (noNeedBook != null)
            {
                noNeedBook.gameObject.SetActive(true); 
            }
        }
        else noNeedBook.gameObject.SetActive(false);
        if (parent != null)
        {
            UIGrid grid = parent.GetComponent<UIGrid>();
            grid.maxPerLine = lenth / 2 + lenth%2;
        }
        foreach (BookItem book in bookInBag.Values)
        {
            book.gameObject.SetActive(false); 
        }
        ItemUI choodeitem = composeBookBtn.GetComponent<ItemUI>();
        for (int i = 0; i < lenth; i++)
        {
            int id = choosedPetSkill[i];
            if (choodeitem != null && choodeitem.EQInfo != null)
            {
                EquipmentRef info = ConfigMng.Instance.GetEquipmentRef(id);
                if (info.psetSkillLevel < needLev)
                {
                    continue;
                }
            }
            if (!bookInBag.ContainsKey(id))//创建
            {
                BookItem item = BookItem.CeateNew(i, parent, true);
                item.gameObject.SetActive(true);
                item.chooseBtn.gameObject.SetActive(true);
                item.chooseYetBtn.gameObject.SetActive(false);
                item.curType = ChooseType.GETMAT;
                ItemUI bookItem = item.GetComponent<ItemUI>();
                bookItem.FillInfo(new EquipmentInfo(id, GameCenter.inventoryMng.GetNumberByType(id), EquipmentBelongTo.PREVIEW));
                int num = 0;
                for (int j = 0; j < choosedBookInMat.Count; j++)
                {
                    if (id == choosedBookInMat[j])
                    {
                        ++num;
                    }
                }
                bookItem.FillInfo(new EquipmentInfo(id, GameCenter.inventoryMng.GetNumberByType(id) - num, EquipmentBelongTo.PREVIEW));
                if (item.chooseBtn != null)
                {
                    UIEventListener.Get(item.chooseBtn.gameObject).onClick -= OnClickChooseToMat;
                    UIEventListener.Get(item.chooseBtn.gameObject).onClick += OnClickChooseToMat;
                    UIEventListener.Get(item.chooseBtn.gameObject).parameter = id;
                }
                bookInBag[id] = item;
            }
            else//刷新
            {
                BookItem book = bookInBag[id] as BookItem;
                book.gameObject.SetActive(true);
                book.transform.localPosition = new Vector3((i % 2) * 120, -(i / 2) * 168);
                book.chooseBtn.gameObject.SetActive(true);
                book.chooseYetBtn.gameObject.SetActive(false);
                ItemUI bookItem = book.GetComponent<ItemUI>();
                bookItem.FillInfo(new EquipmentInfo(id, GameCenter.inventoryMng.GetNumberByType(id), EquipmentBelongTo.PREVIEW));
                int num = 0;
                for (int j = 0; j < choosedBookInMat.Count; j++)
                {
                    if (id == choosedBookInMat[j])
                    {
                        ++num;
                    }
                }
                bookItem.FillInfo(new EquipmentInfo(id, (GameCenter.inventoryMng.GetNumberByType(id) - num), EquipmentBelongTo.PREVIEW));
            }
        }
    }
    void OnClickCloseBtn(GameObject go)//关闭清空
    {
        composeMatLab.text = " ";
        for (int i = 0; i < choosedBookInMat.Count; i++)
        {
            if (!bookInBag.ContainsKey(choosedBookInMat[i]))
                choosedPetSkill.Add(choosedBookInMat[i]);
        }
        ItemUI item = composeBookBtn.GetComponent<ItemUI>();
        if (item != null)
            item.SetEmpty();
        choosedBookInMat.Clear();
        this.gameObject.SetActive(false);
    }
}
