//==================================
//作者：朱素云
//日期：2016/3/12
//用途：宠物守护界面
//=================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PetGuardSubWnd : SubWnd
{
    #region 数据
    protected MercenaryInfo curInfo;
    public UILabel aptitudeLab;
    public UILabel petAttLab;//攻击
    public UILabel petHitLab;//命中
    public UILabel petCrazyHitLab;//暴击
    public UILabel nameLab;//名字
    public UITexture petTex;
    public UILabel playerAttLab;//人物属性
    public UILabel playerHitLab;
    public UILabel playerCrazyHitLab;
    public UILabel propertyAptitude;
    private int skillProperty = 0;//守护技能增加的属性转化率 
    public UIButton guardBtn;//守护
    public UIButton cancelGuardBtn;//取消守护 
    public GameObject parent;
    /// <summary>
    /// 被动技能
    /// </summary>
    protected List<SingleLearnedSkill> petBDSkillList = new List<SingleLearnedSkill>(); 
    public UIButton leftBtn;//前一页
    public UIButton rightBtn;//后一页
    public UIScrollView scro;
    public PetSkillDes skillDesUi;//技能描述框
    private UIGrid grid;
    #endregion 

    #region 构造

    void Awake()
    {
        if (guardBtn != null) UIEventListener.Get(guardBtn.gameObject).onClick = OnClickGruadBtn;
        if (cancelGuardBtn != null) UIEventListener.Get(cancelGuardBtn.gameObject).onClick = OnClickCancelGuardBtn;
        if (leftBtn != null) UIEventListener.Get(leftBtn.gameObject).onClick = OnClickLeftBtnBtn;
        if (rightBtn != null) UIEventListener.Get(rightBtn.gameObject).onClick = OnClickRightBtnBtn;
        if(parent != null)grid = parent.GetComponent<UIGrid>();
    } 
    protected override void OnOpen()
    { 
        base.OnOpen();
        Refresh(); 
        GameCenter.mercenaryMng.OnMercenaryListUpdate += Refresh;
    }

    protected override void OnClose()
    {
        base.OnClose();
        petTex.mainTexture = null;
        GameCenter.mercenaryMng.OnMercenaryListUpdate -= Refresh;
    } 
    #endregion

    #region 事件
    /// <summary>
    /// 前一页
    /// </summary>
    /// <param name="go"></param>
    void OnClickLeftBtnBtn(GameObject go)
    {
        if (scro != null)
        {
            SpringPanel sp = scro.GetComponent<SpringPanel>();
            if (sp == null) sp = scro.gameObject.AddComponent<SpringPanel>();
            if (sp.target.x < 0)
                SpringPanel.Begin(scro.gameObject, new Vector3(115, 0, 0) + sp.target, 10);
        }
    }
    /// <summary>
    /// 后一页
    /// </summary>
    /// <param name="go"></param>
    void OnClickRightBtnBtn(GameObject go)
    {
        if (scro != null)
        {
            SpringPanel sp = scro.GetComponent<SpringPanel>();
            if (sp == null) sp = scro.gameObject.AddComponent<SpringPanel>();
            if (grid != null)
            {
                if (sp.target.x > -115 * (grid.maxPerLine - 5))
                    SpringPanel.Begin(scro.gameObject, new Vector3(-115, 0, 0) + sp.target, 10);
            }
        }
    }
    /// <summary>
    /// 刷新
    /// </summary>
    void Refresh()
    { 
        FDictionary mercenaryInfoList = GameCenter.mercenaryMng.mercenaryInfoList;
        if (mercenaryInfoList.Count <= 0)
        {
            if (aptitudeLab != null) aptitudeLab.text = "0";
            if (propertyAptitude != null) propertyAptitude.text = ConfigMng.Instance.GetUItext(33, new string[2]{ "0","0"});
            if (petAttLab != null) petAttLab.text = "0";
            if (playerAttLab != null) playerAttLab.text = "0";
            if (petHitLab != null) petHitLab.text = "0";
            if (playerHitLab != null) playerHitLab.text = "0";
            if (petCrazyHitLab != null) petCrazyHitLab.text = "0";
            if (playerCrazyHitLab != null) playerCrazyHitLab.text = "0";
            if (nameLab != null) nameLab.text = null; 
            if (petTex != null) petTex.mainTexture = null; 
            for (int i = 0, max = petBDSkillList.Count; i < max; i++)
            {
                petBDSkillList[i].gameObject.SetActive(false); 
            }
            return;
        }
        if (GameCenter.mercenaryMng.curPetId != MercenaryMng.noPet)
        {
            float propr = 0;
            curInfo = GameCenter.mercenaryMng.GetMercenaryById(GameCenter.mercenaryMng.curPetId);
            if (curInfo != null)
            {
                ShowSkill();
                if (aptitudeLab != null) aptitudeLab.text = curInfo.Aptitude.ToString();
                NewPetDataRef petDataRef = ConfigMng.Instance.GetPetDataRef(curInfo.Aptitude);
                if (petDataRef != null)
                {
                    if (propertyAptitude != null) propertyAptitude.text = ConfigMng.Instance.GetUItext(33, new string[2]{
                (((float)petDataRef.zhiZi) / 10000*100).ToString(),
                skillProperty.ToString() });
                }
                propr = ((float)petDataRef.zhiZi) / 10000 * 100 + skillProperty;
                int attr = 0;
                for (int j = 0; j < curInfo.PropertyList.Count; j++)
                {
                    if (curInfo.PropertyList[j].type == 1 || curInfo.PropertyList[j].type == 2)
                    {
                        attr = curInfo.PropertyList[j].num;
                        if (petAttLab != null) petAttLab.text = attr.ToString();
                        if (playerAttLab != null) playerAttLab.text = ((int)(attr * propr) / 100).ToString();
                    }
                    if (curInfo.PropertyList[j].type == 7)
                    {
                        if (petHitLab !=null) petHitLab.text = curInfo.PropertyList[j].num.ToString();
                        if (playerHitLab != null) playerHitLab.text = ((int)(curInfo.PropertyList[j].num * propr) / 100).ToString();
                    }
                    if (curInfo.PropertyList[j].type == 9)
                    {
                        if (petCrazyHitLab != null) petCrazyHitLab.text = curInfo.PropertyList[j].num.ToString();
                        if (playerCrazyHitLab != null) playerCrazyHitLab.text = ((int)(curInfo.PropertyList[j].num * propr) / 100).ToString();
                    }
                }
                if (nameLab != null) nameLab.text = curInfo.PetName;
                if (petTex != null)
                {
                    GameCenter.previewManager.TryPreviewSingelEntourage(curInfo, petTex); 
                }
                if (curInfo.IsActive == 2)//守护
                {
                    if (cancelGuardBtn != null) cancelGuardBtn.gameObject.SetActive(true);
                    if (guardBtn != null) guardBtn.gameObject.SetActive(false);
                }
                else//没有状态
                {
                    if (cancelGuardBtn != null) cancelGuardBtn.gameObject.SetActive(false);
                    if (guardBtn != null) guardBtn.gameObject.SetActive(true);
                }  
            }
        }
    }
    /// <summary>
    /// 显示所有被动技能
    /// </summary>
    void ShowSkill()
    {
        skillProperty = 0;
        List<NewPetSkillRef> list = new List<NewPetSkillRef>();  
        for (int i = 0, max = curInfo.SkillList.Count; i < max; i++)
        {
            NewPetSkillRef petSkillRef = ConfigMng.Instance.GetPetSkillRef((int)curInfo.SkillList[i]);
            if (petSkillRef != null)
            {
                if (petSkillRef.beidong == 2 || petSkillRef.beidong == 3 || petSkillRef.beidong == 4)
                {
                    if (petSkillRef.beidong == 4)
                    {
                        for (int j = 0; j < petSkillRef.add_attr.Count; j++)
                        {
                            skillProperty += petSkillRef.add_attr[j].value;
                        }
                    }
                    list.Add(petSkillRef);
                }
            }
        }
        if (parent != null)
        {  
            if (grid != null)
            {
                grid.maxPerLine = list.Count;

                for (int i = 0, max = petBDSkillList.Count; i < max; i++)
                {
                    petBDSkillList[i].SkillRef = null;
                    petBDSkillList[i].gameObject.SetActive(false);
                }

                for (int i = 0, max = list.Count; i < max; i++)
                {
                    NewPetSkillRef petSkillRef = list[i];
                    if (petSkillRef != null)
                    { 
                        if (petBDSkillList.Count <= i)
                        {  
                            SingleLearnedSkill item = SingleLearnedSkill.CeateBDSkill(i, petSkillRef.id, parent);  
                            if (item != null)
                            {
                                item.gameObject.SetActive(true);
                                petBDSkillList.Add(item);
                                UIEventListener.Get(item.gameObject).onClick -= OnClickSkill;
                                UIEventListener.Get(item.gameObject).onClick += OnClickSkill;
                                UIEventListener.Get(item.gameObject).parameter = petSkillRef.id;
                            } 
                        }
                        else
                        { 
                            petBDSkillList[i].SkillRef = petSkillRef;
                            petBDSkillList[i].gameObject.SetActive(true);
                            UIEventListener.Get(petBDSkillList[i].gameObject).onClick -= OnClickSkill;
                            UIEventListener.Get(petBDSkillList[i].gameObject).onClick += OnClickSkill;
                            UIEventListener.Get(petBDSkillList[i].gameObject).parameter = petSkillRef.id; 
                        }
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
    /// <summary>
    /// 守护
    /// </summary>
    void OnClickGruadBtn(GameObject go)
    {
        if (GameCenter.mercenaryMng.curPetId != MercenaryMng.noPet)
        {
            int count = 0;//守护的宠物最多两个 
            FDictionary mercenaryInfoList = GameCenter.mercenaryMng.mercenaryInfoList;
            foreach (MercenaryInfo info in mercenaryInfoList.Values)
            {
                if (info.IsActive == 2)
                {
                    count++;
                }
            }
            if (count < 2)
            {
                if (curInfo.IsActive == 1)//出战的宠物不能守护
                {
                    GameCenter.messageMng.AddClientMsg(249);
                }
                else
                {
                    GameCenter.mercenaryMng.C2S_ReqMercenaryInfo(PetChange.GUARD, GameCenter.mercenaryMng.curPetId);
                    if (guardBtn != null) guardBtn.gameObject.SetActive(false);
                    if (cancelGuardBtn != null) cancelGuardBtn.gameObject.SetActive(true);
                }
            }
            else
            {
                GameCenter.messageMng.AddClientMsg(183);
            }
        }
    }
    /// <summary>
    /// 取消守护
    /// </summary>
    void OnClickCancelGuardBtn(GameObject go)
    {
        GameCenter.mercenaryMng.C2S_ReqMercenaryInfo(PetChange.CANCELGUARD, GameCenter.mercenaryMng.curPetId);
        if (cancelGuardBtn != null) cancelGuardBtn.gameObject.SetActive(false);
        if (guardBtn != null) guardBtn.gameObject.SetActive(true);
    }
    #endregion

}
