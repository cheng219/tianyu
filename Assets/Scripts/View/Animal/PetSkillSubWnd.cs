//==================================
//作者：朱素云
//日期：2016/3/21
//用途：宠物技能界面
//=================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PetSkillSubWnd : SubWnd
{
    #region 数据
    public MercenaryInfo curInfo;
    /// <summary>
    /// 技能槽中的技能链表
    /// </summary>
    public List<PetSkill> petSkillList = new List<PetSkill>(); 
    public PetSkillDes skillDesUi;//技能描述框 
    public UIButton getBookBtn;//获得技能书
    public GetBookUi getBookUi;//获得技能书框 
    public UIButton skillPicBtn;//技能图鉴
    public SkillPicUi skillPicUi;//技能图鉴框 
    public UIButton skillComposeBtn;//技能合成
    public SkillComposeUi skillComposeUi;//技能合成框
    /// <summary>
    /// 技能书链表
    /// </summary>
    protected List<BookItem> bookItemList = new List<BookItem>();
    protected Dictionary<int, int> allPetSkillBook
    {
        get
        {
            return GameCenter.mercenaryMng.allPetSkillBook;
        }
    }
    protected List<int> skillBookKey
    {
        get
        {
            List<int> list = new List<int>(allPetSkillBook.Keys);
            list = GameCenter.mercenaryMng.CompareByQuality(list);
            return list;
        }
    }
    public GameObject parent;
    public GameObject skillObj;  
    #endregion

    #region 构造
    void Awake()
    {
        if (getBookBtn != null) UIEventListener.Get(getBookBtn.gameObject).onClick = OnClickGetBookBtn;
        if (skillPicBtn != null) UIEventListener.Get(skillPicBtn.gameObject).onClick = OnClickSkillPicBtn;
        if (skillComposeBtn != null) UIEventListener.Get(skillComposeBtn.gameObject).onClick = OnClickSkillComposeBtn; 
        if (getBookUi != null) getBookUi.gameObject.SetActive(false);
        skillPicUi.gameObject.SetActive(false);
        skillComposeUi.gameObject.SetActive(false);
    }
    protected override void OnOpen()
    {
        base.OnOpen();  
        FreshAllSkill();
        GameCenter.inventoryMng.OnBackpackUpdate += ShowBookItem; 
        GameCenter.mercenaryMng.OnMercenaryListUpdate += FreshAllSkill;
    }
    protected override void OnClose()
    {
        base.OnClose();
        GameCenter.inventoryMng.OnBackpackUpdate -= ShowBookItem; 
        GameCenter.mercenaryMng.OnMercenaryListUpdate -= FreshAllSkill;
    }

    #endregion

    #region 事件　
    /// <summary>
    /// 获得技能书
    /// </summary> 
    void OnClickGetBookBtn(GameObject go)
    {
        if (getBookUi != null) getBookUi.gameObject.SetActive(true); 
    }
    /// <summary>
    /// 技能图鉴
    /// </summary> 
    void OnClickSkillPicBtn(GameObject go)
    {
        skillPicUi.gameObject.SetActive(true);
        skillPicUi.ShowAllSkill();
    }
    /// <summary>
    /// 技能合成
    /// </summary> 
    void OnClickSkillComposeBtn(GameObject go)
    {
        skillComposeUi.gameObject.SetActive(true); 
        skillComposeUi.ShowComposeUi();
    }
    /// <summary>
    /// 点击技能弹出技能描述框
    /// </summary> 
    void OnClickSkill(GameObject go)
    {    
        NewPetSkillRef skillRef = (NewPetSkillRef)UIEventListener.Get(go).parameter; 
        skillDesUi.SkillRef = skillRef;
        skillDesUi.fengYinBtn.gameObject.SetActive(true);
        skillDesUi.yiWangBtn.gameObject.SetActive(true);
        skillDesUi.gameObject.SetActive(true);
    }
    /// <summary>
    /// 点击下方技能书学习技能
    /// </summary>
    void OnClickStudySkill(GameObject go)
    {
        if (GameCenter.mercenaryMng.curPetId == MercenaryMng.noPet || GameCenter.mercenaryMng.mercenaryInfoList.Count <= 0)
        {
            return;
        }
        int skillId = (int)UIEventListener.Get(go.GetComponent<UIButton>().gameObject).parameter;
        NewPetSkillRef skillref = ConfigMng.Instance.GetPetSkillRef(skillId);
        if (skillref != null)
        {  
            MessageST mst = new MessageST();
            mst.messID = 164;
            mst.words = new string[2] { skillref.name, skillref.res};
            mst.delYes = delegate
            {
                bool islenred = IsStudySameSkill(skillref.kind, skillref.quality);
                bool isLearedHighterSkill = IsStudyHighSkill(skillref.kind, skillref.quality);
                //Debug.Log("islenred :  " + islenred + "  , isLearedHighterSkill : " + isLearedHighterSkill + "  , emptyNest : " + GameCenter.mercenaryMng.emptyNest);
                if (!IsStudySameSkill(skillref.kind)&& GameCenter.mercenaryMng.emptyNest <= 0)//若宠物技能栏已满，则弹出上浮提示147
                {
                    GameCenter.messageMng.AddClientMsg(152);
                    return;
                } 
                if (islenred)//若已经学了同类别的技能，则弹出另外的上浮提示148
                {
                    GameCenter.messageMng.AddClientMsg(153);
                }
                else if (!isLearedHighterSkill && skillref.quality > 2)
                {
                    GameCenter.messageMng.AddClientMsg(541);
                } 
                else
                {
                    GameCenter.mercenaryMng.C2S_ReqPromote(PetChange.STUDYSKILL, GameCenter.mercenaryMng.curPetId, skillId);
                }
            };
            GameCenter.messageMng.AddClientMsg(mst);
        }
    }
    bool IsStudySameSkill(int _kind, int _quality)
    { 
        if(curInfo == null) return false;
        for(int i=0,max = curInfo.SkillList.Count;i<max;i++)
        {
            NewPetSkillRef SkillRef = ConfigMng.Instance.GetPetSkillRef((int)curInfo.SkillList[i]);
            if (SkillRef.kind == _kind && SkillRef.quality != _quality - 1)//已经拥有这种类型的技能且这个技能等级不是低一级的技能
                return true;
        } 
        return false;
    }
    bool IsStudySameSkill(int _kind)
    {
        if (curInfo == null) return false;
        for (int i = 0, max = curInfo.SkillList.Count; i < max; i++)
        {
            NewPetSkillRef SkillRef = ConfigMng.Instance.GetPetSkillRef((int)curInfo.SkillList[i]);
            if (SkillRef.kind == _kind)//已经拥有这种类型的技能
                return true;
        }
        return false;
    }
    /// <summary>
    /// 学习中级以上的技能时，必须要先学会低一级的技能
    /// </summary> 
    bool IsStudyHighSkill(int _kind, int _quality)
    {
        //if (_quality <= 3) return true;  
        if (curInfo == null)  return false; 
        if (curInfo.SkillList.Count <= 0) return false;
        for (int i = 0, max = curInfo.SkillList.Count; i < max; i++)
        {
            NewPetSkillRef SkillRef = ConfigMng.Instance.GetPetSkillRef((int)curInfo.SkillList[i]);
            if (SkillRef.kind == _kind && SkillRef.quality == _quality - 1)
            { 
                return true;
            }
        } 
        return false;
    } 

    /// <summary>
    /// 在下方显示背包中的技能书
    /// </summary>
    void ShowBookItem()
    {
        GameCenter.mercenaryMng.GetBookFromBag(); 
        int count = allPetSkillBook.Count; 

        for (int i = 0, max = bookItemList.Count; i < max; i++)
        {
            bookItemList[i].gameObject.SetActive(false);
        }

        if (parent != null)
        {
            UIGrid grid = parent.GetComponent<UIGrid>();
            if (grid != null)
            {
                grid.maxPerLine = count > 6 ? count : 6;
                ItemUI bookItem = null;
                BookItem item = null; 
                for (int i = 0, max = grid.maxPerLine; i < max; i++)
                {
                    if (bookItemList.Count <= i)
                    {
                        item = BookItem.CeateNew(i, parent, false);
                        item.gameObject.SetActive(true);
                        item.chooseBtn.gameObject.SetActive(false);
                        item.chooseYetBtn.gameObject.SetActive(false);
                        bookItem = item.GetComponent<ItemUI>();
                        bookItem.ShowTooltip = false;//关闭热感
                        if (GameCenter.mercenaryMng.curPetId == MercenaryMng.noPet || GameCenter.mercenaryMng.mercenaryInfoList.Count <= 0)
                        {
                            bookItem.ShowTooltip = true;
                        }
                        bookItem.itemName.gameObject.SetActive(false);//隐藏名字 
                        if (count > i)
                        {
                            if (allPetSkillBook.ContainsKey(skillBookKey[i]))
                            {
                                int id = skillBookKey[i];
                                item.gameObject.SetActive(true);
                                item.chooseBtn.gameObject.SetActive(false);
                                item.chooseYetBtn.gameObject.SetActive(false);
                                bookItem = item.GetComponent<ItemUI>();
                                bookItem.FillInfo(new EquipmentInfo(id, allPetSkillBook[id], EquipmentBelongTo.PREVIEW));
                                item.petSkillRefDate = ConfigMng.Instance.GetPetSkillRefByBook(id);
                                UIButton button = item.GetComponent<UIButton>();
                                if (button != null && item.petSkillRefDate != null)
                                {
                                    UIEventListener.Get(button.gameObject).onClick -= OnClickStudySkill;
                                    UIEventListener.Get(button.gameObject).onClick += OnClickStudySkill;
                                    UIEventListener.Get(button.gameObject).parameter = item.petSkillRefDate.id;//技能id
                                }
                            }
                        }
                        bookItemList.Add(item);
                    }
                    else
                    {
                        bookItemList[i].gameObject.SetActive(true);
                        bookItemList[i].chooseBtn.gameObject.SetActive(false);
                        bookItemList[i].chooseYetBtn.gameObject.SetActive(false);
                        bookItem = bookItemList[i].GetComponent<ItemUI>();
                        bookItem.ShowTooltip = false;//关闭热感
                        if (GameCenter.mercenaryMng.curPetId == MercenaryMng.noPet || GameCenter.mercenaryMng.mercenaryInfoList.Count <= 0)
                        {
                            bookItem.ShowTooltip = true;
                        }
                        bookItem.itemName.gameObject.SetActive(false);//隐藏名字 
                        if (count > i)
                        { 
                            int id = skillBookKey[i];
                            bookItemList[i].gameObject.SetActive(true);
                            bookItemList[i].chooseBtn.gameObject.SetActive(false);
                            bookItemList[i].chooseYetBtn.gameObject.SetActive(false);
                            bookItem = bookItemList[i].GetComponent<ItemUI>();
                            bookItem.FillInfo(new EquipmentInfo(id, allPetSkillBook[id], EquipmentBelongTo.PREVIEW));
                            bookItemList[i].petSkillRefDate = ConfigMng.Instance.GetPetSkillRefByBook(id);
                            UIButton button = bookItemList[i].GetComponent<UIButton>();
                            if (button != null && bookItemList[i].petSkillRefDate != null)
                            {
                                UIEventListener.Get(button.gameObject).onClick -= OnClickStudySkill;
                                UIEventListener.Get(button.gameObject).onClick += OnClickStudySkill;
                                UIEventListener.Get(button.gameObject).parameter = bookItemList[i].petSkillRefDate.id;//技能id
                            } 
                        }
                        else
                        {
                            bookItem.FillInfo(null);
                        }
                    }
                }
            }
        }
    } 
    /// <summary>
    /// 显示技能状态
    /// </summary>
    void FreshAllSkill()
    {
        ShowBookItem();
        if (GameCenter.mercenaryMng.curPetId == MercenaryMng.noPet ||
           GameCenter.mercenaryMng.mercenaryInfoList.Count <= 0)
        {
            if (skillObj != null) skillObj.SetActive(false);  
            return;
        }
        if (skillObj != null) skillObj.SetActive(true); 
        GameCenter.mercenaryMng.emptyNest = 0; 
        FDictionary petSkillNumRefTable = ConfigMng.Instance.GetPetSkillNumRefTable();
        curInfo = GameCenter.mercenaryMng.GetMercenaryById(GameCenter.mercenaryMng.curPetId);
        Dictionary<int, NewPetSkillNumRef> skillNumList = new Dictionary<int,NewPetSkillNumRef>(); 
        for (int i = 0; i < petSkillList.Count; i++)
        {
            petSkillList[i].SkillNumRef = null;
            petSkillList[i].SkillRef = null; 
        }
        if (curInfo != null)
        {
            int i = 0; int count = 0;
            foreach (NewPetSkillNumRef dataRef in petSkillNumRefTable.Values)
            {
                if (petSkillList.Count > i && i < 10)
                {
                    petSkillList[i].SkillNumRef = dataRef;
                    int growup = dataRef.chengZhang;
                    int aptitude = dataRef.zizhi;
                    if ((growup <= curInfo.GrowUp) && (aptitude <= curInfo.Aptitude))
                    {
                        ++GameCenter.mercenaryMng.emptyNest;//统计空槽
                    }
                    else
                    {
                        if (dataRef.chengZhang != 0)
                            skillNumList[dataRef.chengZhang] = dataRef;//保存没有解锁的数据
                        else
                            skillNumList[dataRef.zizhi] = dataRef;
                    }
                }
                i++;
            }
            List<int> list = new List<int>(skillNumList.Keys);//按顺序存放数据
            list.Sort();
            count = GameCenter.mercenaryMng.emptyNest; 
            for (int j = 0; j < petSkillList.Count; j++)
            {
                if (j < count)
                {
                    if (curInfo.SkillList.Count > j && GameCenter.mercenaryMng.emptyNest > 0)
                    {
                        --GameCenter.mercenaryMng.emptyNest;
                        petSkillList[j].SkillRef = ConfigMng.Instance.GetPetSkillRef((int)curInfo.SkillList[j]);//填充空槽

                        UIEventListener.Get(petSkillList[j].leanerdSkill.gameObject).onClick -= OnClickSkill;
                        UIEventListener.Get(petSkillList[j].leanerdSkill.gameObject).onClick += OnClickSkill; 
                        UIEventListener.Get(petSkillList[j].leanerdSkill.gameObject).parameter = petSkillList[j].SkillRef;
                    }
                    else
                    { 
                        petSkillList[j].ShowNoLock(); 
                    }
                }
                else
                {
                    if (list.Count > (j - count) && skillNumList.ContainsKey(list[j - count]))
                    {
                        petSkillList[j].SkillNumRef = skillNumList[list[j - count]];//按顺序摆放解锁条件
                        petSkillList[j].ShowUnlockSkillInfo();
                    }
                }
            }
        }  
    } 
    #endregion
}
