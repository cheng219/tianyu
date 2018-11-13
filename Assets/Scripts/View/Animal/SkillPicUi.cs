//==================================
//作者：朱素云
//日期：2016/3/28
//用途：宠物技能图鉴ui
//=================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillPicUi : MonoBehaviour {

    public UIButton closeBtn;
    protected FDictionary allSkill = new FDictionary(); 
    public GameObject parent;
    public PetSkillDes skillDesUi;//技能描述框
    public List<UILabel> skillLevDes = new List<UILabel>(); 
    //protected List<NewPetSkillRef> primarySkill { get { return GameCenter.mercenaryMng.primarySkill; } }
    //protected List<NewPetSkillRef> middleSkill { get { return GameCenter.mercenaryMng.middleSkill; } }
    //protected List<NewPetSkillRef> seniorSkill { get { return GameCenter.mercenaryMng.seniorSkill; } }
    //protected List<NewPetSkillRef> topSkill { get { return GameCenter.mercenaryMng.topSkill; } }

    protected List<NewPetSkillRef> primarySkill = new List<NewPetSkillRef>();
    protected List<NewPetSkillRef> middleSkill = new List<NewPetSkillRef>();
    protected List<NewPetSkillRef> seniorSkill = new List<NewPetSkillRef>();
    protected List<NewPetSkillRef> topSkill = new List<NewPetSkillRef>();

    void Awake()
    { 
        if (closeBtn != null) UIEventListener.Get(closeBtn.gameObject).onClick = OnClickCloseBtn;
        primarySkill = ConfigMng.Instance.GetDividedSkillByLev(1);
        middleSkill = ConfigMng.Instance.GetDividedSkillByLev(2);
        seniorSkill = ConfigMng.Instance.GetDividedSkillByLev(3);
        topSkill = ConfigMng.Instance.GetDividedSkillByLev(4);
    }
    void OnClickCloseBtn(GameObject go)
    {
        this.gameObject.SetActive(false);
    }
    void OnClickDesBtn(GameObject go)
    {
        if (skillDesUi != null)
        {
            skillDesUi.gameObject.SetActive(true);
            UIButton fengyin = skillDesUi.transform.GetComponent<PetSkillDes>().fengYinBtn;
            UIButton yiwang = skillDesUi.transform.GetComponent<PetSkillDes>().yiWangBtn;
            if (fengyin != null) fengyin.gameObject.SetActive(false);
            if (yiwang != null) yiwang.gameObject.SetActive(false);
            int num = (int)UIEventListener.Get(go).parameter;
            skillDesUi.SkillRef = ConfigMng.Instance.GetPetSkillRef(num);
        }
    } 
    public void ShowAllSkill()
    {  
        int middlelen = (middleSkill.Count % 5 > 0) ? (middleSkill.Count / 5 + 1) : middleSkill.Count / 5;
        int seniorlen = (seniorSkill.Count % 5 > 0) ? (seniorSkill.Count / 5 + 1) : seniorSkill.Count / 5;
        int toplen = (topSkill.Count % 5 > 0) ? (topSkill.Count / 5 + 1) : topSkill.Count / 5; 
       
        if (topSkill.Count <= 0)//顶级
        {
            if (skillLevDes.Count > 3) skillLevDes[3].gameObject.SetActive(false);
        }
        else
        {  
            if (skillLevDes.Count > 3)
            {
                skillLevDes[3].gameObject.SetActive(true);
                skillLevDes[3].transform.parent = parent.transform;
                skillLevDes[3].transform.localPosition = new Vector3(210, 0);
            }
            for (int i = 0; i < topSkill.Count; i++)
            {
                CreateBook(i, topSkill[i]);
            }
        }
        if (seniorSkill.Count <= 0)//高级
        {
            if (skillLevDes.Count > 2) skillLevDes[2].gameObject.SetActive(false);
        }
        else
        {
            int addlen = (toplen > 0) ? (toplen + 1) * 5 : 0; 
            if (skillLevDes.Count > 2)
            {
                skillLevDes[2].gameObject.SetActive(true);
                skillLevDes[2].transform.parent = parent.transform;
                skillLevDes[2].transform.localPosition = new Vector3(210, -(80 + 102 * toplen)); 
            }
            for (int i = 0; i < seniorSkill.Count; i++)
            {
                CreateBook(i + addlen, seniorSkill[i]);
            }
        }
        if (middleSkill.Count <= 0)//中级
        {
            if (skillLevDes.Count > 1) skillLevDes[1].gameObject.SetActive(false);
        }
        else
        {
            if (skillLevDes.Count > 1)
            {
                skillLevDes[1].gameObject.SetActive(true);
                skillLevDes[1].transform.parent = parent.transform;
                skillLevDes[1].transform.localPosition = new Vector3(210, -(80 + 102 * ((toplen > 0 ? (toplen + 1) : toplen) + seniorlen)));
            }
            int addlen = ((toplen > 0) ? (toplen + 1) * 5 : 0) + ((seniorlen > 0) ? (seniorlen + 1) * 5 : 0);
            for (int i = 0; i < middleSkill.Count; i++)
            {
                CreateBook(i + addlen, middleSkill[i]);
            }
        }
        if (primarySkill.Count <= 0)//初级
        {
            if (skillLevDes.Count > 0) skillLevDes[0].gameObject.SetActive(false);
        }
        else
        {
            if (skillLevDes.Count > 0)
            {
                skillLevDes[0].gameObject.SetActive(true);
                skillLevDes[0].transform.parent = parent.transform;
                skillLevDes[0].transform.localPosition = new Vector3(210, -(80 + 102 * ((toplen > 0 ? (toplen + 1) : toplen) + (seniorlen > 0 ? (seniorlen + 1) :seniorlen) + middlelen)));
            }
            int addlen = ((toplen > 0) ? (toplen + 1) * 5 : 0) + ((seniorlen > 0) ? (seniorlen + 1) * 5 : 0) + ((middlelen > 0) ? (middlelen + 1) * 5 : 0);
            for (int i = 0; i < primarySkill.Count; i++)
            {
                CreateBook(i + addlen, primarySkill[i]);
            }
        } 
    }
    void CreateBook(int _index, NewPetSkillRef _skillRef)
    {
        if (_skillRef != null)
        {
            int id = _skillRef.id;
            if (!allSkill.ContainsKey(id))
            {
                SingleSkill item = SingleSkill.CeateNew(_index, id, parent);
                item.SkillRef = _skillRef;
                item.gameObject.SetActive(true);
                allSkill[id] = item; 
                UIEventListener.Get(item.gameObject).onClick -= OnClickDesBtn;
                UIEventListener.Get(item.gameObject).onClick += OnClickDesBtn;
                UIEventListener.Get(item.gameObject).parameter = id; 
            }
            else
            {
                SingleSkill skill = allSkill[id] as SingleSkill;
                skill.SkillRef = _skillRef;
                skill.gameObject.SetActive(true);
                UIEventListener.Get(skill.gameObject).onClick -= OnClickDesBtn;
                UIEventListener.Get(skill.gameObject).onClick += OnClickDesBtn;
                UIEventListener.Get(skill.gameObject).parameter = id; 
            }
        }
    } 
}
