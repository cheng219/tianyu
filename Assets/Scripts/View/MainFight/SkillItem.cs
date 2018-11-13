//==============================================
//作者：黄洪兴
//日期：2016/6/27
//用途：技能释放UI
//=================================================




using UnityEngine;
using System.Collections;

public class SkillItem : MonoBehaviour {

    /// <summary>
    /// 技能图标
    /// </summary>
    public UISpriteEx SkillIcon;
    /// <summary>
    /// 未激活状态
    /// </summary>
    public GameObject lockobj;
    /// <summary>
    /// 剩余解锁等级
    /// </summary>
    public UILabel locklable;
    /// <summary>
    /// 魔法不足显示
    /// </summary>
    public GameObject noMane;
    /// <summary>
    /// 遮罩
    /// </summary>
    public UISprite CoolingMask;
    /// <summary>
    /// 剩余时间
    /// </summary>
    public UILabel TimeLable;
    /// <summary>
    /// 冷却时间
    /// </summary>
    public float CoolingTime = 10.0f;
    /// <summary>
    /// 当前冷却时间
    /// </summary>
    public float CurTime;
    /// <summary>
    /// 按下
    /// </summary>
    private bool isdown = false;
    /// <summary>
    /// 是否在冷却中
    /// </summary>
    private bool isCool = false;
    public bool IsDown
    {
        get { return isdown; }
        set { isdown = value; }
    }
    /// <summary>
    /// 是否有足够魔法
    /// </summary>
    private bool hadmp = false;
    public bool HadMp
    {
        get { return hadmp; }
    }
    public bool needTarget
    {
        get { return ConfigMng.Instance.GetSkillPerformanceRef(ConfigMng.Instance.GetSkillRuneRef(skillInfo.SkillCurRune).performanceID).castType == CastType.TARGET; }
    }
    /// <summary>
    /// 是否已经学习
    /// </summary>
    private bool hadlearn = false;
    public bool HadLearn
    {
        get { return hadlearn; }
    }
    /// <summary>
    /// 技能数据
    /// </summary>
	public SkillInfo skillInfo = null;
    private AbilityInstance ability = null;
    public void FillInfo(SkillInfo _info , bool _iscanuse)
    {
        skillInfo = _info;

        if (_info != null)
        {
            hadlearn = GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel >= skillInfo.UnlockLv;
            if (SkillIcon != null) SkillIcon.spriteName = _info.SkillIcon;
            //RefreshCurSkill();
            lockobj.SetActive(GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel < skillInfo.UnlockLv);
            if (_iscanuse)
            {
                if (_info.SkillLv > 0)
                {
                    CoolingTime = ConfigMng.Instance.GetSkillLvDataRef(ConfigMng.Instance.GetSkillRuneRef(_info.SkillCurRune).performanceID, _info.SkillLv).cd;
                    CurTime = CoolingTime;
                } 
                if (SkillIcon != null) SkillIcon.IsGray = UISpriteEx.ColorGray.normal; 
                RefreshMane();
            }
            else
            {
                if (SkillIcon != null) SkillIcon.IsGray = UISpriteEx.ColorGray.Gray; 
                if (CoolingMask != null) CoolingMask.gameObject.SetActive(false);
                if (TimeLable != null) TimeLable.gameObject.SetActive(false);
                locklable.text = skillInfo.UnlockLv.ToString() + ConfigMng.Instance.GetUItext(303);
            }
        }
        else
        {
            //Debug.Log("SkillInfo = null");
        }

//        if (_info != null) 
//        {
//            if (_info.SkillLv > 0) 
//            {
//                CoolingTime = ConfigMng.Instance.GetSkillLvDataRef (ConfigMng.Instance.GetSkillRuneRef (_info.SkillCurRune).performanceID, _info.SkillLv).cd;
//                CurTime = CoolingTime;
//            }
//            //if (SkillIcon != null) SkillIcon.gameObject.SetActive(true);
//            if (SkillIcon != null) SkillIcon.IsGray = UISpriteEx.ColorGray.normal;
//            if (SkillIcon != null) SkillIcon.spriteName = _info.SkillIcon;
////			Debug.Log ("刷新的图片名字为"+_info.SkillIcon);
//            //if (this != null) this.GetComponent<BoxCollider>().enabled = true;
//            RefreshCurSkill();
//            RefreshMane();
//        } else 
//        {
//            //if(SkillIcon!=null)SkillIcon.gameObject.SetActive (false);
//            if (SkillIcon != null) SkillIcon.IsGray = UISpriteEx.ColorGray.Gray;
//            //if(this!=null)this.GetComponent<BoxCollider> ().enabled = false;
//            if (CoolingMask != null) CoolingMask.gameObject.SetActive(false);
//            if (TimeLable != null) TimeLable.gameObject.SetActive(false);
//        }
    }

    public void AddCDTime(AbilityInstance _ability)
    {
        ability = _ability;
        if (ability.HasServerConfirm)
        {
            CurTime = ability.RestCD;
            if (ability.RestCD > 0)
            {
                isdown = true;
            }
        }
        else
        {
            CurTime = CoolingTime;
        }

       // Debug.Log("当前冷却时间为" + CurTime);
    }
	// Use this for initialization
	void Start () {

        GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate -= RefreshState;
        GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate += RefreshState;
	}
	
	// Update is called once per frame
    void Update()
    {
        if (isdown && CoolingMask != null && TimeLable != null)
        {
            if (!isCool )
            {
                CoolingMask.gameObject.SetActive(true);
                TimeLable.gameObject.SetActive(true);
                TimeLable.text = Mathf.Round(CoolingTime).ToString();
                CoolingMask.fillAmount = 1f;
                isCool = true;
            }
            if (isCool )
            {
                CoolingMask.fillAmount = (CurTime / CoolingTime);
                CurTime -= Time.deltaTime;
                TimeLable.text = Mathf.Round(CurTime).ToString();
                if (CurTime <= 0.01f)
                {
                    isCool = false;
                    isdown = false;
                    CoolingMask.fillAmount = 0;
                    CoolingMask.gameObject.SetActive(false);
                    TimeLable.gameObject.SetActive(false);
                    CurTime = CoolingTime;
                }
            }
        }
        
	}
    private void RefreshCurSkill()
    { 
        if (skillInfo == null)
        {
            return;
        }
        if (lockobj != null)
        {
            hadlearn = GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel >= skillInfo.UnlockLv;
            if (lockobj != null) lockobj.SetActive(GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel < skillInfo.UnlockLv);
        }
        if (GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel < skillInfo.UnlockLv)
        {
            if (lockobj != null) lockobj.SetActive(true);
            if (SkillIcon != null) SkillIcon.IsGray = UISpriteEx.ColorGray.Gray;
        }
        else
        {
            if (lockobj != null) lockobj.SetActive(false);
            if (SkillIcon != null) SkillIcon.IsGray = UISpriteEx.ColorGray.normal;
        }
        if (locklable != null)
        {
            locklable.text = skillInfo.UnlockLv.ToString()+ConfigMng.Instance.GetUItext(303);
        }
        if (SkillIcon != null)
        {
           // SkillIcon.spriteName = ConfigMng.Instance.GetSkillRuneRef(skillInfo.SkillCurRune).runeIcon;
        }
    }

    private void RefreshMane()
    {
        if (noMane != null && skillInfo != null && GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel >= skillInfo.UnlockLv)
        { 
            hadmp = GameCenter.mainPlayerMng.MainPlayerInfo.CurMP >= ConfigMng.Instance.GetSkillLvDataRef(ConfigMng.Instance.GetSkillRuneRef(skillInfo.SkillCurRune).performanceID, skillInfo.SkillLv).mp;
            noMane.SetActive(!hadmp); 
        }
    }

	private void RefreshState(ActorBaseTag _tag, ulong _value,bool _fromAbility)
    {
        switch (_tag)
        { 
            case ActorBaseTag.CurMP:
                RefreshMane();
                break;
            case ActorBaseTag.Level:
                RefreshCurSkill();
                break;
        }
    }
    public void EnterCooling()
    {
        IsDown = true;
    }
}
