//==================================
//作者：朱素云
//日期：2016/3/6
//用途：宠物信息界面
//=================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PetInfoSubWnd : SubWnd
{
    #region 数据
     
    public UIButton releaseBtn;//放生
    public UIButton restBtn;//休息
    public UIButton fightBtn;//出战
    public UIButton changeNameBtn;//改名
    public UIButton leftBtn;//前一页
    public UIButton rightBtn;//后一页 
    public UILabel ziZhiLab;//宠物资质
    public UILabel levelLabe;//等级
    public UILabel growUpLabel;//成长 
    public UITexture petTex;//宠物模型 
    public UIInput petNameInput;//名字 
    public MercenaryInfo curInfo;//当前宠物  
    public List<UISpriteEx> tianHunStarList = new List<UISpriteEx>();//天魂星星
    public List<UISpriteEx> diHunStarList = new List<UISpriteEx>();//地魂星星
    public List<UISpriteEx> mingHunStarList = new List<UISpriteEx>();//命魂星星 
    public UILabel[] propertyInfoVal;
    protected List<SingleLearnedSkill> petLearedSkillList = new List<SingleLearnedSkill>();
    public PetSkillDes skillDesUi;//技能描述框
    public GameObject parent;  
    public UIScrollView scro;
    private bool isCreateItems = false;
    #endregion

    #region 构造
 
    void Start()
    { 
        if (releaseBtn != null) UIEventListener.Get(releaseBtn.gameObject).onClick = OnClickReleaseBtn;
        if (restBtn != null && GameCenter.mercenaryMng.curPetId != MercenaryMng.noPet)
            UIEventListener.Get(restBtn.gameObject).onClick = OnClickRestBtn;
        if (fightBtn != null) UIEventListener.Get(fightBtn.gameObject).onClick = OnClickFightBtn;
        if (changeNameBtn != null) UIEventListener.Get(changeNameBtn.gameObject).onClick = OnClickChangeNameBtn;
        if (leftBtn != null) UIEventListener.Get(leftBtn.gameObject).onClick = OnClickLeftBtn;
        if (rightBtn != null) UIEventListener.Get(rightBtn.gameObject).onClick = OnClickRightBtn;
    }
    protected override void OnOpen()
    {
        base.OnOpen();
        isCreateItems = true;
        GameCenter.mercenaryMng.OnMercenaryListUpdate += RefreshPetInfoWnd;
    } 
    protected override void OnClose()
    {
        base.OnClose();
        isCreateItems = false; 
        petTex.mainTexture = null;
        GameCenter.mercenaryMng.OnMercenaryListUpdate -= RefreshPetInfoWnd;
    }
    void Update()
    {
        if (isCreateItems)
        {
            isCreateItems = false;
            RefreshPetInfoWnd();
        }
    }
    #endregion

    /// <summary>
    /// 放生宠物
    /// </summary>
    void OnClickReleaseBtn(GameObject go)
    {
        if (GameCenter.mercenaryMng.curPetId == MercenaryMng.noPet) return;
        MessageST mst = new MessageST();
        mst.messID = 159; 
        mst.delYes = delegate
        {
            GameCenter.mercenaryMng.C2S_ReqMercenaryInfo(PetChange.FREE, GameCenter.mercenaryMng.curPetId);
        };
        GameCenter.messageMng.AddClientMsg(mst);
        
    }
    /// <summary>
    /// 休息
    /// </summary> 
    void OnClickRestBtn(GameObject go)
    {
        GameCenter.mercenaryMng.C2S_ReqMercenaryInfo(PetChange.REST, GameCenter.mercenaryMng.curPetId);
    }
    /// <summary>
    /// 出战
    /// </summary> 
    void OnClickFightBtn(GameObject go)
    {  
        bool iscanFight = true;
        FDictionary mercenaryInfoList = GameCenter.mercenaryMng.mercenaryInfoList;
        if (curInfo != null)
        {
            if (curInfo.IsActive == 2)
            {
                GameCenter.messageMng.AddClientMsg(250);
                iscanFight = false;
                return;
            }
        }
        //foreach (MercenaryInfo info in mercenaryInfoList.Values)
        //{ 
        //    if (info.IsActive == 1)
        //    {
        //        MessageST mst = new MessageST();
        //        mst.messID = 144;
        //        GameCenter.messageMng.AddClientMsg(mst);
        //        iscanFight = false;
        //        break;
        //    } 
        //}
        if(iscanFight)GameCenter.mercenaryMng.C2S_ReqMercenaryInfo(PetChange.FINGHTING, GameCenter.mercenaryMng.curPetId);
    }
    /// <summary>
    /// 改名字
    /// </summary> 
    void OnClickChangeNameBtn(GameObject go)
    {
        if (petNameInput != null && GameCenter.loginMng.CheckBadWord(petNameInput.value))
        {
            string contents = GameCenter.loginMng.FontHasCharacter(petNameInput.label.bitmapFont, petNameInput.value);
            if (!string.IsNullOrEmpty(contents))
            {
                petNameInput.value = contents;
                GameCenter.messageMng.AddClientMsg(300);
            }
            GameCenter.mercenaryMng.C2S_ReqChangeName(GameCenter.mercenaryMng.curPetId, petNameInput.value);
        }
    }
    /// <summary>
    /// 前一页
    /// </summary> 
    void OnClickLeftBtn(GameObject go)
    {
        if (scro != null)
        {
            SpringPanel sp = scro.GetComponent<SpringPanel>();
            if (sp == null) sp = scro.gameObject.AddComponent<SpringPanel>();
            if (sp.target.x < 0)
                SpringPanel.Begin(scro.gameObject, new Vector3(115, 0, 0) + sp.target, 9.5f);
        }
    }
    /// <summary>
    /// 后一页
    /// </summary> 
    void OnClickRightBtn(GameObject go)
    {
        if (scro != null)
        {
            SpringPanel sp = scro.GetComponent<SpringPanel>();
            if (sp == null) sp = scro.gameObject.AddComponent<SpringPanel>();
            if (sp.target.x >= -115 * (curInfo.SkillList.Count - 5))
                SpringPanel.Begin(scro.gameObject, new Vector3(-115, 0, 0) + sp.target, 9.5f);
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
    void ShowNoPet()
    {
        if (ziZhiLab != null) ziZhiLab.text = "0";
        if (levelLabe != null) levelLabe.text = "0";
        if (growUpLabel != null) growUpLabel.text = "0";
        if (petTex != null) petTex.mainTexture = null;
        if (petNameInput != null) petNameInput.value = string.Empty; 
        for (int i = 0; i < propertyInfoVal.Length; i++)
        {
            propertyInfoVal[i].text = "0";
        }
        for (int i = 0, max = petLearedSkillList.Count; i < max; i++)
        {
            petLearedSkillList[i].gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// 刷新界面宠物信息
    /// </summary>
    public void RefreshPetInfoWnd()
    { 
        NoStar(mingHunStarList);
        NoStar(tianHunStarList);
        NoStar(diHunStarList);
        if (GameCenter.mercenaryMng.curPetId == MercenaryMng.noPet)
        {
            ShowNoPet();
            return;
        } 
        curInfo = GameCenter.mercenaryMng.GetMercenaryById(GameCenter.mercenaryMng.curPetId);
        if (curInfo == null)
        {
            ShowNoPet();
            return;
        }  
        if (ziZhiLab != null) ziZhiLab.text = curInfo.Aptitude.ToString();
        if (levelLabe != null) levelLabe.text = curInfo.Level.ToString();
        if (growUpLabel != null) growUpLabel.text = curInfo.GrowUp.ToString();
        if (petTex != null)
        {
            GameCenter.previewManager.TryPreviewSingelEntourage(curInfo, petTex);
        }
        if (petNameInput != null) petNameInput.value = curInfo.NoColorName;
        //属性、成长、精魂数值
        if (propertyInfoVal.Length >= 9)
        {
            for (int j = 0; j < curInfo.PropertyList.Count; j++)
            {
                if (curInfo.PropertyList[j].type == 1)
                    propertyInfoVal[0].text = curInfo.PropertyList[j].num.ToString();
                if (curInfo.PropertyList[j].type == 7)
                    propertyInfoVal[1].text = curInfo.PropertyList[j].num.ToString();
                if (curInfo.PropertyList[j].type == 9)
                    propertyInfoVal[2].text = curInfo.PropertyList[j].num.ToString();
            }
            NewPetDataRef petData = ConfigMng.Instance.GetPetDataRef(curInfo.GrowUp);
            if (petData != null)
            {
                for (int j = 0; j < petData.chengZhang.Count; j++)
                {
                    if (petData.chengZhang[j].eid == 1)
                        propertyInfoVal[3].text = petData.chengZhang[j].count.ToString();
                    if (petData.chengZhang[j].eid == 7)
                        propertyInfoVal[4].text = petData.chengZhang[j].count.ToString();
                    if (petData.chengZhang[j].eid == 9)
                        propertyInfoVal[5].text = petData.chengZhang[j].count.ToString();
                }
            }
            if (curInfo.GrowUp <= 0)
            {
                propertyInfoVal[3].text = "0";
                propertyInfoVal[4].text = "0";
                propertyInfoVal[5].text = "0";
            }
            for (int j = 0; j < curInfo.SoulPropertyList.Count; j++)
            {
                if (curInfo.SoulPropertyList[j].type == 1)
                    propertyInfoVal[6].text = curInfo.SoulPropertyList[j].num.ToString();
                if (curInfo.SoulPropertyList[j].type == 7)
                    propertyInfoVal[7].text = curInfo.SoulPropertyList[j].num.ToString();
                if (curInfo.SoulPropertyList[j].type == 9)
                    propertyInfoVal[8].text = curInfo.SoulPropertyList[j].num.ToString();
            }
        } 
        ShowStar(tianHunStarList, curInfo.Tian_soul);
        ShowStar(diHunStarList, curInfo.Di_soul);
        ShowStar(mingHunStarList, curInfo.Life_soul);
        ShowSkill();
        //出战状态（出战、休息两状态切换）
        if (curInfo.IsActive == 1)
        {
            if (fightBtn != null) fightBtn.gameObject.SetActive(false);
            if (restBtn != null) restBtn.gameObject.SetActive(true);
        }
        else
        {
            if (fightBtn != null) fightBtn.gameObject.SetActive(true);
            if (restBtn != null) restBtn.gameObject.SetActive(false);
        } 
    } 
    /// <summary>
    /// 显示技能
    /// </summary>
    void ShowSkill()
    {
        if (parent != null)
        {
            UIGrid grid = parent.GetComponent<UIGrid>();
            if (grid != null)
            {
                grid.maxPerLine = curInfo.SkillList.Count;
                for (int i = 0, max = petLearedSkillList.Count; i < max; i++)
                {
                    petLearedSkillList[i].SkillRef = null;
                    petLearedSkillList[i].gameObject.SetActive(false);
                }
                for (int i = 0,max = curInfo.SkillList.Count; i < max; i++)
                {
                    if (petLearedSkillList.Count <= i)
                    {
                        SingleLearnedSkill item = SingleLearnedSkill.CeateNew(i, (int)curInfo.SkillList[i], parent);
                        item.gameObject.SetActive(true);
                        item.SkillRef = ConfigMng.Instance.GetPetSkillRef((int)curInfo.SkillList[i]);  
                        if (item != null)
                        {
                            petLearedSkillList.Add(item);
                            UIEventListener.Get(item.gameObject).onClick -= OnClickSkill;
                            UIEventListener.Get(item.gameObject).onClick += OnClickSkill;
                            UIEventListener.Get(item.gameObject).parameter = (int)curInfo.SkillList[i];
                        }
                    }
                    else
                    {
                        petLearedSkillList[i].SkillRef = ConfigMng.Instance.GetPetSkillRef((int)curInfo.SkillList[i]); 
                        petLearedSkillList[i].gameObject.SetActive(true);
                        UIEventListener.Get(petLearedSkillList[i].gameObject).onClick -= OnClickSkill;
                        UIEventListener.Get(petLearedSkillList[i].gameObject).onClick += OnClickSkill;
                        UIEventListener.Get(petLearedSkillList[i].gameObject).parameter = (int)curInfo.SkillList[i];
                    } 
                }
                grid.repositionNow = true;
            }
        }
    }
    /// <summary>
    /// 点击技能弹出技能描述框
    /// </summary> 
    void OnClickSkill(GameObject go)
    { 
        skillDesUi.gameObject.SetActive(true);
        int num = (int)UIEventListener.Get(go).parameter;
        skillDesUi.SkillRef = ConfigMng.Instance.GetPetSkillRef(num);
        skillDesUi.fengYinBtn.gameObject.SetActive(false);
        skillDesUi.yiWangBtn.gameObject.SetActive(false);
    }
}
