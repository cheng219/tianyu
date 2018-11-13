//=====================================
//作者:黄洪兴
//日期:2016/03/01
//用途:技能组件
//========================================


using UnityEngine;
using System.Collections;
/// <summary>
/// 技能UI组件
/// </summary>
public class SkillUI : MonoBehaviour
{
    #region 控件数据


    /// <summary>
    /// 技能升级特效
    /// </summary>
    public UIFxAutoActive skillUpEffect;

	/// <summary>
	/// 是否使用中
	/// </summary>
	public bool isUse=false;
	/// <summary>
	/// 是否被选中
	/// </summary>
	public bool isChoose
	{
		get{
			if (GameCenter.skillMng.CurSkillInfo == null) {
				return false;
			}
			else {
				if (skillinfo != null)
					return skillinfo.SkillID == GameCenter.skillMng.CurSkillInfo.SkillID;
				else {
					return false;
				}
			}
		}
	}
	/// <summary>
	/// 是否为空技能
	/// </summary>
	public bool isnull=true;
	/// <summary>
	/// 是否为选择技能的技能
	/// </summary>
	public bool isEnable=false;
	/// <summary>
	/// 是否为已经学会的
	/// </summary>
	public bool isLearn=false;
    /// <summary>
    /// 图标
    /// </summary>
    public UISprite itemIcon;
    /// <summary>
    /// 名字
    /// </summary>
    public UILabel itemName;
	public  UISprite chooseSprite;
	public  UISprite useSprite;
	public GameObject skillTypeObj;
	public UISprite lockSprite;
    public UILabel lockNum;
    public UILabel skillType;
    public GameObject redObj;
    public UILabel skillLev;

    #endregion 
    #region 数据
    /// <summary>
    /// 选择事件
    /// </summary>
    public System.Action<SkillUI> OnSelectEvent = null;
    /// <summary>
    /// 当前填充的数据
    /// </summary>
	public SkillInfo skillinfo;
//	protected SkillInfo oldSkillinfo;  //for upgrade effect -by ms
    public SkillInfo CurSkillInfo
    {
        get { return skillinfo; }
    }
    public static SkillInfo GetEqInfo(GameObject _obj)
    {
        SkillUI itemUI = _obj.GetComponent<SkillUI>();
        if (itemUI == null)
        {
            Debug.LogError("请在控件对象上加上<SkillUI>组件!!");
            return null;
        }
        return itemUI.CurSkillInfo;
    }
    #endregion
    // Use this for initialization
	void Start () {

	}
    void OnEnable()
    {
        GameCenter.skillMng.OnShowSkillEffect += ShowEffect;
    }
    void OnDisable()
    {
        GameCenter.skillMng.OnShowSkillEffect -= ShowEffect;
    }


    void ShowEffect()
    {
        if (chooseSprite != null)
        {
            if (chooseSprite.gameObject.activeSelf)
            if (skillUpEffect != null)
                skillUpEffect.ShowFx();
        }
    }
    void OnSelectThis(GameObject obj)
    {
//		SkillUpgradesWnd.curSkillUIinfo=this.skillinfo;
//        if (OnSelectEvent != null)
//        {
//            OnSelectEvent(this);
//        }
    }
    /// <summary>
    /// 填充数据
    /// </summary>
    /// <param name="_info"></param>
    public void FillInfo(SkillInfo _info)
    {
		if (_info == null) {
			isnull = true;
			skillinfo = null;
		} 
		else {
			isnull = false;
			skillinfo = _info;
			isLearn = _info.isEnable;
//			oldSkillinfo = skillinfo;
		}
		RefreshSkill ();
    }
    /// <summary>
    /// 刷新表现
    /// </summary>
	public void RefreshSkill()
    {
		if (skillinfo != null) {
			if(useSprite!=null) useSprite.gameObject.SetActive (isUse);
			if(lockSprite!=null)lockSprite.gameObject.SetActive (!isLearn);

			if (lockNum != null && ConfigMng.Instance.GetSkillMainLvRef (skillinfo.SkillID, skillinfo.SkillLv) != null) {
				if (ConfigMng.Instance.GetLevelDes (ConfigMng.Instance.GetSkillMainLvRef (skillinfo.SkillID, skillinfo.SkillLv).learnLv) == null)
					return;
                string[] words = { ConfigMng.Instance.GetLevelDes(ConfigMng.Instance.GetSkillMainLvRef(skillinfo.SkillID, skillinfo.SkillLv).learnLv) };
                string st = ConfigMng.Instance.GetUItext(54);
                if(st!=null)
                lockNum.text = UIUtil.Str2Str(st, words);
			}
		} else {
			if(useSprite!=null) useSprite.gameObject.SetActive (false);
			if(lockSprite!=null) lockSprite.gameObject.SetActive (false);
		}
        if (chooseSprite != null)
        {
            chooseSprite.gameObject.SetActive(isChoose&&isEnable);
        }
		if (isnull) {
			if(itemIcon!=null)itemIcon.gameObject.SetActive (false);
            if (itemName != null)
            {
                itemName.gameObject.SetActive(isEnable);
            }
		} else {
//			Debug.Log ("技能图标已经显示"+ConfigMng.Instance.GetSkillMainConfigRef (skillinfo.SkillID).skillIcon);
			if(itemIcon!=null)itemIcon.gameObject.SetActive (true);
			//if (skillinfo.SkillLv > 0) {
			if(itemIcon!=null)itemIcon.spriteName = ConfigMng.Instance.GetSkillMainConfigRef (skillinfo.SkillID).skillIcon;
            if (itemName != null)
            {
                itemName.text = "[b]" + skillinfo.SkillName;
                itemName.gameObject.SetActive(isEnable);
            }
			//} else {
				//itemIcon.spriteName = ConfigMng.Instance.GetSkillMainConfigRef (skillinfo.SkillID).skillIcon;
				//itemName.text = "[b]" + skillinfo.SkillName;

			//}
		}
        if (isnull)
        {
            if (skillTypeObj!=null)
            skillTypeObj.SetActive(false);
        }
        else
        {
            if (skillTypeObj != null)
            skillTypeObj.SetActive(true);
            if (skillinfo.SkillType == 1) skillType.text =ConfigMng.Instance.GetUItext(51);
            if (skillinfo.SkillType == 2) skillType.text = ConfigMng.Instance.GetUItext(52);
            if (skillinfo.SkillType == 3) skillType.text = ConfigMng.Instance.GetUItext(53);


        }
        if (skillinfo != null)
        {
            SkillInfo NextSkill = new SkillInfo(skillinfo.SkillID, skillinfo.SkillLv + 1);
            if (skillinfo.isFullLevel || isnull || !isLearn || !NextSkill.CoinEnough || !NextSkill.ResEnough || !NextSkill.LevEnough)
            {
                if (redObj != null)
                    redObj.SetActive(false);

            }
            else
            {
                if (redObj != null && isEnable)
                    redObj.SetActive(true);
            }
        }
        else
        {
            if (redObj!=null)
            redObj.SetActive(false);
        }

        if (skillLev != null && skillinfo != null)
        {
            skillLev.text = "Lv." + skillinfo.SkillLv.ToString();
        }
        skillLev.gameObject.SetActive(isEnable && skillinfo != null);

    }
}
