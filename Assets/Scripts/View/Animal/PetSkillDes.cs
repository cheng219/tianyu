//==================================
//作者：朱素云
//日期：2016/3/22
//用途：宠物技能描述
//=================================
using UnityEngine;
using System.Collections;
using System.Text;

public class PetSkillDes : MonoBehaviour 
{ 
    public UILabel nameLabel;
    public UILabel skillDesLab;//描述
    public UIButton fengYinBtn;//封印按钮
    public UIButton yiWangBtn;//遗忘按钮 
    public UIButton closeBtn;
    public bool notRemind = false;//不再提示

    protected NewPetSkillRef skillRef;
    public NewPetSkillRef SkillRef
    {
        get
        {
            return skillRef;
        }
        set
        {
            if (value != null)
                skillRef = value;
            ShowSkillDes();
        }
    }
    void Awake()
    {
        if (fengYinBtn != null)
        {
            UIEventListener.Get(fengYinBtn.gameObject).onClick -= OnClickFengYinBtnBtn;
            UIEventListener.Get(fengYinBtn.gameObject).onClick += OnClickFengYinBtnBtn;
        }
        if (yiWangBtn != null)
        {
            UIEventListener.Get(yiWangBtn.gameObject).onClick -= OnClickYiWangBtnBtn; 
            UIEventListener.Get(yiWangBtn.gameObject).onClick += OnClickYiWangBtnBtn;
        }
        if (closeBtn != null) UIEventListener.Get(closeBtn.gameObject).onClick = OnClickCloseBtn;
    } 
    void OnClickCloseBtn(GameObject go)
    {
        this.gameObject.SetActive(false);
    }
    /// <summary>
    /// 封印
    /// </summary> 
    void OnClickFengYinBtnBtn(GameObject go)
    { 
        if (skillRef != null)
        {
            MessageST mst = new MessageST();
            mst.messID = 161;
            mst.words = new string[1] { new EquipmentInfo(skillRef.fengYinItem[0].eid, EquipmentBelongTo.PREVIEW).ItemName };
            mst.delYes = delegate
            {
                GameCenter.mercenaryMng.C2S_ReqPromote(PetChange.SEALSKILL, GameCenter.mercenaryMng.curPetId, skillRef.id);
                this.gameObject.SetActive(false);
            };
            GameCenter.messageMng.AddClientMsg(mst);
        }
    }

    /// <summary>
    /// 遗忘
    /// </summary>
    /// <param name="go"></param>
    void OnClickYiWangBtnBtn(GameObject go)
    { 
        if (skillRef != null)
        {
            MessageST mst = new MessageST();
            mst.messID = 162;
            mst.delYes = delegate
            {
                GameCenter.mercenaryMng.C2S_ReqPromote(PetChange.FORGETSKILL, GameCenter.mercenaryMng.curPetId, skillRef.id);
                this.gameObject.SetActive(false);
            };
            GameCenter.messageMng.AddClientMsg(mst);
        }
    }
    /// <summary>
    /// 技能描述
    /// </summary>
    void ShowSkillDes()
    {
        if (SkillRef != null)
        {
            if (nameLabel != null) nameLabel.text = SkillRef.name;
            if (skillDesLab != null) skillDesLab.text = SkillRef.res;
        }
    }
    

}
