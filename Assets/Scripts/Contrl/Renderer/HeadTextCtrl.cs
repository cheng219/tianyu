//==============================================
//作者：吴江
//日期：2015/5/24
//用途：对象头顶文字控制器
//==============================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum FlagType
{ 
    Relationship,
    PetTitle,
    PetName,
    OwnerName,
    Name,
    GuildName,
    VIPLevel,
    PKLevel,
    PKTitle,
    /// <summary>
    /// 任务标签 问号 感叹号
    /// </summary>
    TaskTag,
    /// <summary>
    /// 头顶泡泡文字
    /// </summary>
    Bubble,
    /// <summary>
    /// 头顶血条
    /// </summary>
    BloodSlider,
    /// <summary>
    /// 称号
    /// </summary>
    Title,
	/// <summary>
	/// 采集进度条
	/// </summary>
	CollectProgress,
    /// <summary>
    /// 倒计时
    /// </summary>
    CountDown,
    /// <summary>
    /// 头顶特效
    /// </summary>
    TipEffect,
    /// <summary>
    /// 领袖标志
    /// </summary>
    LearderTag,
}


public class HeadTextCtrl : MonoBehaviour
{
    #region 数据
    /// <summary>
    /// 名字的字体
    /// </summary>
    public UIFont name_font = null;
    /// <summary>
	/// 伤害敌方造成爆击的字体(UIFont_crit_dmg)
    /// </summary>
    public UIFont bj_font = null;
    /// <summary>
	/// 伤害敌方字体(UIFont_dmg)
    /// </summary>
    public UIFont font_dmg = null;
	/// <summary>
	/// 幸运一击字体
	/// </summary>
	public UIFont xy_font = null;
	/// <summary>
	/// 宠物造成伤害字体
	/// </summary>
	public UIFont font_pet_dmg = null;
    /// <summary>
	/// 友方被伤害的字体,包括暴击、幸运一击、闪避等(UIFont_get_dmg)
    /// </summary>
    public UIFont font_get_dmg = null;
    /// <summary>
	/// 治疗字体(UIFont_heal)
    /// </summary>
    public UIFont zl_font = null;


    /// <summary>
    /// 倒计时字体
    /// </summary>
    public UIFont cd_font = null;


    protected UIAtlas taskAtlas = null;
    protected UIAtlas titleAtlas = null;

    public GameObject textParent = null;
    public GameObject TextParent
    {
        get
        {
            if (textParent == null)
            {
                InitTextParent();
            }
            return textParent;
        }
    }

    protected HeadTextPrefab petOwnerNameObj = null;
    protected HeadTextPrefab petNameObj = null;
    protected HeadTextPrefab petTitleObj = null;
    protected HeadTextPrefab nameObj = null;
    protected HeadTextPrefab relationObj = null;
    protected HeadTextPrefab guildNameObj = null;
    protected HeadTextPrefab titleObj = null;
    protected GameObject headEffectObj = null;
    protected CountDownPrefab countDownObj = null;
    protected BubblePrefab bubbleObj = null;
    protected BloodSliderPrefab bloodSliderObj = null;
    protected LeaderTagPrefab leaderTagPrefab = null;

    protected CollectProgressPrefab collectProgressPrefab = null;
    protected InteractiveObject interActive = null;
    protected InteractiveObject InterActive
    {
        get
        {
            if (interActive == null)
            {
                interActive = this.gameObject.GetComponent<InteractiveObject>();
                if (interActive == null)
                {
                    Transform parent = this.gameObject.transform.parent;
                    if (parent != null)
                        interActive = parent.gameObject.GetComponent<InteractiveObject>();
                }
            }
            return interActive;
        }
    }
    protected Dictionary<FlagType, GameObject> flagList = new Dictionary<FlagType, GameObject>();

    /// <summary>
    /// 是否正在泡泡说话，如果是，则要隐藏名字
    /// </summary>
    protected bool isBubbling = false;
     
    #endregion

    #region UNITY
    void Awake()
    {
        InitTextParent();
    }
    #endregion


    protected void InitTextParent()
    {
        if (InterActive != null)
        {
            textParent = new GameObject("HeadText");
            textParent.AddComponent<UIPanel>();
            textParent.transform.parent = InterActive.transform;
            textParent.transform.localScale = Vector3.one;
            textParent.transform.parent = this.transform;
            textParent.transform.localEulerAngles = Vector3.zero;
			textParent.transform.localPosition = new Vector3(0, InterActive.NameHeight*InterActive.ScaleBuffValue, 0);
            textParent.SetMaskLayer(InterActive.gameObject.layer);


            LookAt lookAt = TextParent.AddComponent<LookAt>();
            lookAt.target = GameCenter.cameraMng.mainCamera;
            //图集
            if (taskAtlas == null)
            {
                taskAtlas = Resources.Load("NGUI/Atlas/sundryAtlas/sundryAtlas", typeof(UIAtlas)) as UIAtlas;
            }
            if (titleAtlas == null)
            {
                titleAtlas = Resources.Load("NGUI/NewAtlas/Title/Title", typeof(UIAtlas)) as UIAtlas;
            }
        }
    }
	/// <summary>
	/// buff影响模型大小,改变了名字高度
	/// </summary>
	public void UpdateParentHight()
	{
		if(InterActive != null && textParent != null)
			textParent.transform.localPosition = new Vector3(0, InterActive.NameHeight*InterActive.ScaleBuffValue, 0);
	}

    /// <summary>
    /// 调整名字高度
    /// </summary>
    /// <param name="_hight"></param>
    public void CaculatPos(float _hight)
    {
        textParent.transform.localPosition = new Vector3(0, _hight, 0);
    }

    /// <summary>
    /// 获取文字的挂点坐标
    /// </summary>
    /// <param name="_type"></param>
    /// <returns></returns>
    protected Vector3 InitAchor(FlagType _type)
    {
        Vector3 pos = Vector3.zero;
        float height = 0;
        switch (_type)
        { 
            case FlagType.Relationship:
                pos = new Vector3(-1.0f, height, 0);
                break;
            case FlagType.PetTitle:
                height += 0.88f;
                pos = new Vector3(0.1f, height, 0f);
                break;
            case FlagType.PetName:
                height += 0.4f;
                pos = new Vector3(0.1f, height, 0f);
                break;
            case FlagType.OwnerName: 
                pos = Vector3.zero;
                 break;
            case FlagType.Name:
                 if (relationObj != null && relationObj.gameObject.activeSelf)
                 {
                     pos = new Vector3(0.37f, 0, 0);
                 }
                 else 
                 {
                     pos = Vector3.zero;
                 }
                break;
            case FlagType.GuildName:
                height += 0.45f;
                pos = new Vector3(0.1f, height, 0f);
                break;
            case FlagType.VIPLevel:
                break;
            case FlagType.PKLevel:
                break;
            case FlagType.PKTitle:
                break;
            case FlagType.TaskTag:
                height += 1.2f;
                pos = new Vector3(0, height, 0);
                break;
            case FlagType.Bubble:
                pos = Vector3.zero;
                break;
            case FlagType.BloodSlider:
                height -= 0.3f;
                pos = new Vector3(0, height, 0);
                break;
            case FlagType.Title:
                if (InterActive.typeID == ObjectType.NPC)
                    height += 0.45f;
                else if (InterActive.typeID == ObjectType.Entourage)
                    height += 1.2f;
                else
                    height += 0.35f;
                if (guildNameObj != null && guildNameObj.gameObject.activeSelf)
                {
                    height += 0.5f;
                }
                pos = new Vector3(0, height, 0);
                break;
			case FlagType.CollectProgress:
				height += 1.0f;
				pos = new Vector3(0f,height,0f);
				break;
            case FlagType.CountDown:
                height += 2.0f;
                pos = new Vector3(0f, height, 0f);
                break;
            case FlagType.LearderTag:
                height += 0.45f;
                if (guildNameObj != null && guildNameObj.gameObject.activeSelf)
                {
                    height += 0.45f;
                }
                if (titleObj != null && titleObj.gameObject.activeSelf)
                {
                    height += 0.45f;
                }
                pos = new Vector3(0, height, 0);
                break;
            default:
                break;
        }
        return pos;
    }

    /// <summary>
    /// 激活/关闭所有的文字内容
    /// </summary>
    /// <param name="_acitve"></param>
    public void SetFlagsActive(bool _acitve)
    {
        if (_acitve && isBubbling) return;
        foreach (var item in flagList.Values)
        {
            if (item != null)
            {
                item.SetActive(_acitve);
            }
        }
    }


    public void UpdateFlagsHeight()
    {
        foreach (FlagType item in flagList.Keys)
        { 
            if (flagList[item] !=null&& flagList[item].gameObject.activeSelf)
            {
                flagList[item].gameObject.transform.localPosition = InitAchor(item);
            }
        }
    }

    #region 任务头标
    /// <summary>
    /// 任务标签（问号，感叹号） by吴江
    /// </summary>
    protected UISprite taskTag = null;
    /// <summary>
    /// 设置任务标签（问号，感叹号） by吴江
    /// </summary>
    /// <param name="_state"></param>
    public void SetTaskTag(NPCTaskState _state)
    {
        if (taskTag == null)
        {
            GameObject tagObj = new GameObject("TaskTag");
            tagObj.transform.parent = TextParent.transform;
            tagObj.transform.localEulerAngles = Vector3.zero;
            tagObj.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
            tagObj.transform.localPosition = InitAchor(FlagType.TaskTag);
            tagObj.SetMaskLayer(InterActive.gameObject.layer);
            taskTag = tagObj.AddComponent<UISprite>();
            taskTag.width = 32;
            taskTag.height = 44;
            taskTag.atlas = taskAtlas;
        }
        if (taskTag != null)
        {
            switch (_state)
            {
                case NPCTaskState.None:
                    taskTag.gameObject.SetActive(false);
                    break;
                case NPCTaskState.CanTake:
                    taskTag.gameObject.SetActive(true);
                    taskTag.spriteName = "Pic_jingtanghao";
                    break;
                case NPCTaskState.CanCommit:
                    taskTag.gameObject.SetActive(true);
                    taskTag.spriteName = "Pic_wenhao";//
                    break;
                case NPCTaskState.HasTake:
                    taskTag.gameObject.SetActive(true);
                    taskTag.spriteName = "Pic_wenhaohui";
                    break;
                default:
                    break;
            }
        }
        else
        {
            Debug.LogError("找不到任务标记挂在组件！");
        }
    }
    #endregion

    #region 称号


    #region  称号图片
    /// <summary>
    /// 父节点
    /// </summary>
    GameObject parentObj = null;
    /// <summary>
    /// 称号
    /// </summary>
    GameObject tagObj = null;
    /// <summary>
    /// 称号图片
    /// </summary>
    protected UISprite titleSprite = null;
    /// <summary>
    /// 设置称号图片
    /// </summary>
    /// <param name="_state"></param>
    public void SetTitleSprite(string _titleName)
    { 
        if (string.IsNullOrEmpty(_titleName))
        { 
            if (parentObj != null)
            {
                Destroy(parentObj.gameObject);   
            }
            if (tagObj != null)
            {
                Destroy(tagObj.gameObject);
                titleSprite = null;
            }
            return;
        }
        if (titleSprite == null)
        {
            if (parentObj == null)
            {
                parentObj = new GameObject("TitleSpriteParent");
                flagList[FlagType.Title] = parentObj;
            }
            if (tagObj == null) tagObj = new GameObject("TitleSprite");
            parentObj.transform.parent = TextParent.transform;
            tagObj.transform.parent = parentObj.transform; 
            parentObj.transform.localEulerAngles = Vector3.zero;
            parentObj.transform.localScale = new Vector3(0.018f, 0.018f, 0.018f);
            parentObj.transform.localPosition = InitAchor(FlagType.Title); 
            parentObj.SetMaskLayer(InterActive.gameObject.layer);
            titleSprite = tagObj.AddComponent<UISprite>();
            titleSprite.transform.localPosition = Vector3.zero; 
            titleSprite.atlas = titleAtlas;
            titleSprite.MakePixelPerfect();
            titleSprite.pivot = UIWidget.Pivot.Bottom;
        }
        if (titleSprite != null)
        {
            titleSprite.gameObject.SetActive(true);
            titleSprite.spriteName = _titleName;
            titleSprite.MakePixelPerfect();
            titleSprite.transform.localPosition = Vector3.zero; 
        }
        else
        {
            Debug.LogError("找不到任务标记挂在组件！");
        }
    }
    #endregion


    public void SetTitle(string _title)
    {
        if (titleObj == null)
        {
            if (_title == string.Empty) return;
            InitTitleObj();
        }
        //else
        //{
        //    titleObj.gameObject.SetActive(true);//不建议直接操作titleObj的显示隐藏,由SetFlagsActive接口控制 by邓成
        //}
        if (_title == string.Empty || _title.Length == 0)
        {
            Destroy(titleObj.gameObject);
            Destroy(titleObj);
            titleObj = null;
            UpdateFlagsHeight();
            return;
        }
 
        string temp = string.Empty;//转下表现形式
        //if (_title.Equals(""))
        //    temp = _title;
        //else
        //    temp = new System.Text.StringBuilder().Append("<").Append(_title).Append(">").ToString();
        temp = _title;
        SetTitleObjText(temp);
        UpdateFlagsHeight();
    }

    protected void InitTitleObj()
    {
        if (InterActive == null)
        {
            Debug.LogError("基本元素才可以带有头顶文字！");
            return;
        }
        if (TextParent == null)
        {
            Debug.LogError("组件启动出错！");
            return;
        }
        if (!flagList.ContainsKey(FlagType.Title)) flagList[FlagType.Title] = null;
        GameObject prefab = flagList[FlagType.Title];
     
        if (prefab == null)
        {
            UnityEngine.Object obj = null;

            switch (InterActive.typeID)
            {
                case ObjectType.Player:
                    obj = exResources.GetResource(ResourceType.TEXT, "PlayerName");
                    break;
                case ObjectType.Entourage:
                    obj = exResources.GetResource(ResourceType.TEXT, "PlayerName");
                    break;
                case ObjectType.Model:
                    obj = exResources.GetResource(ResourceType.TEXT, "PlayerName");
                    break;
                case ObjectType.CGObject:
                    obj = exResources.GetResource(ResourceType.TEXT, "PlayerName");
                    break;
                case ObjectType.FlyPoint:
                    obj = exResources.GetResource(ResourceType.TEXT, "FlyPointName");
                    break;
                case ObjectType.NPC:
                    obj = exResources.GetResource(ResourceType.TEXT, "NPCName");
                    break;
                case ObjectType.MOB:
                    obj = exResources.GetResource(ResourceType.TEXT, "MonsterName");
                    break;
                case ObjectType.SceneItem:
                    obj = exResources.GetResource(ResourceType.TEXT, "MonsterName");
                    break;

                default:
                    break;
            }
            if (obj != null)
            {
                prefab = Instantiate(obj) as GameObject;
                obj = null;
            }
            flagList[FlagType.Title] = prefab;
        }
        if (prefab != null)
        {
            prefab.transform.parent = TextParent.transform;
            prefab.transform.localEulerAngles = Vector3.zero;
            prefab.transform.localScale = Vector3.one;
            prefab.SetMaskLayer(InterActive.gameObject.layer);
            titleObj = prefab.GetComponent<HeadTextPrefab>();

        }
        else
        {
            Debug.LogError("找不到文字预制！");
        }
    }
    protected void SetTitleObjText(string _title)
    {
        if (titleObj != null)
        {
            titleObj.SetText(_title);
            titleObj.SetColor(nameColor);
            //titleObj.SetOutLineColor(outLineColor);
            //Debug.Log("设置称号颜色" + nameColor + ":" + outLineColor);
        }
    }	
    #endregion

    #region 倒计时
    public void SetCountDown(int _time)
    {
        if (countDownObj == null)
        {
            InitCountDownObj();
        }
        if (countDownObj != null)
        {
            countDownObj.SetTime(_time);
        }
    }
    public void InitCountDownObj()
    {
        if (InterActive == null)
        {
            Debug.LogError("基本元素才可以带有头顶文字！");
            return;
        }
        if (TextParent == null)
        {
            Debug.LogError("组件启动出错！");
            return;
        }
        if (!flagList.ContainsKey(FlagType.CountDown)) flagList[FlagType.CountDown] = null;
        GameObject prefab = flagList[FlagType.CountDown];
        if (prefab == null)
        {
            UnityEngine.Object obj = exResources.GetResource(ResourceType.TEXT, "Time");
            if (obj != null)
            {
                prefab = Instantiate(obj) as GameObject;
                obj = null;
            }
            flagList[FlagType.CountDown] = prefab;
        }
        if (prefab != null)
        {
            prefab.transform.parent = TextParent.transform;
            prefab.transform.localEulerAngles = Vector3.zero;
            prefab.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
            prefab.transform.localPosition = InitAchor(FlagType.CountDown);
            prefab.SetMaskLayer(InterActive.gameObject.layer);
            countDownObj = prefab.GetComponent<CountDownPrefab>();

        }
        else
        {
            Debug.LogError("找不到文字预制！");
        }
    }
    #endregion

    #region 宠物称号、宠物名字、宠物主人名字单独写出来方便排序和设置坐标add by zsy 
    #region 宠物称号
    public void SetPetTitle(string _title)
    {
        if (petTitleObj == null)
        {
            if (_title == string.Empty) return;
            petTitleObj = InitObj(FlagType.PetTitle);
        } 
        if (_title == string.Empty || _title.Length == 0)
        {
            Destroy(petTitleObj.gameObject);
            Destroy(petTitleObj);
            petTitleObj = null;
            UpdateFlagsHeight();
            return;
        } 
        string temp = string.Empty;//转下表现形式 
        temp = _title;
        SetPetTitleText(temp);
        UpdateFlagsHeight();
    }  
    protected void SetPetTitleText(string _petOwnerName) 
    {
        if (petTitleObj != null)
        {
            petTitleObj.SetText(_petOwnerName);
            petTitleObj.SetColor(nameColor); 
        }
    }	
    #endregion 
    #region 宠物名字
    public void SetPetName(string _name)
    {
        if (petNameObj == null)
        {
            petNameObj = InitObj(FlagType.PetName);
        }
        SetPetNameText(_name);
    } 
    protected void SetPetNameText(string _petOwnerName)
    {
        if (petNameObj != null)
        {
            name_font = petNameObj.name_font;
            bj_font = petNameObj.bj_font;
            font_dmg = petNameObj.font_dmg;
            font_get_dmg = petNameObj.font_get_dmg;
            zl_font = petNameObj.zl_font;
            font_pet_dmg = petNameObj.font_pet_dmg;
            xy_font = petNameObj.xy_font;
            petNameObj.SetText(_petOwnerName);
        }  
    }
    public void SetPetNameColor(Color _color)
    {
        if (petNameObj != null)
        {
            petNameObj.SetColor(_color);
        }
    }
    #endregion 
    #region 宠物主人名字
    public void SetPetOwnerName(string _name)
    {
        if (petOwnerNameObj == null)
        {
            petOwnerNameObj = InitObj(FlagType.OwnerName);

        }
        SetPetOwnerNameText(_name);
    } 
    protected void SetPetOwnerNameText(string _petOwnerName)
    { 
        if (petOwnerNameObj != null)
        {
            name_font = petOwnerNameObj.name_font;
            bj_font = petOwnerNameObj.bj_font;
            font_dmg = petOwnerNameObj.font_dmg;
            font_get_dmg = petOwnerNameObj.font_get_dmg;
            zl_font = petOwnerNameObj.zl_font;
            font_pet_dmg = petOwnerNameObj.font_pet_dmg;
            xy_font = petOwnerNameObj.xy_font;
            petOwnerNameObj.SetText(_petOwnerName);
        }

        SetNameColorAlongInterActive();
    }
    #endregion 

    /// <summary>
    /// 设置镖车名字颜色
    /// </summary>
    public void SetCarNameColor()
    {
        if (nameObj != null)
        {
            nameObj.SetColor(new Color(0, 1, 1, 1));
        }
    }
    /// <summary>
    /// 初始化获取预制
    /// </summary> 
    protected HeadTextPrefab InitObj(FlagType _type)
    {
        if (InterActive == null)
        {
            Debug.LogError("基本元素才可以带有头顶文字！");
            return null;
        }
        if (TextParent == null)
        {
            Debug.LogError("组件启动出错！");
            return null;
        }
        if (!flagList.ContainsKey(_type)) flagList[_type] = null;
        GameObject prefab = flagList[_type];
        HeadTextPrefab HeadText = null;
        if (prefab == null)
        {
            UnityEngine.Object obj = null;
            obj = exResources.GetResource(ResourceType.TEXT, "PlayerName");
            if (obj != null)
            {
                prefab = Instantiate(obj) as GameObject;
                obj = null;
            }
            flagList[_type] = prefab;
        }
        if (prefab != null)
        {
            prefab.transform.parent = TextParent.transform;
            prefab.transform.localEulerAngles = Vector3.zero;
            prefab.transform.localScale = Vector3.one;
            prefab.transform.localPosition = InitAchor(_type);
            prefab.SetMaskLayer(InterActive.gameObject.layer);
            HeadText = prefab.GetComponent<HeadTextPrefab>(); 
        }
        return HeadText;
    } 
    #endregion


    #region 关系

    public void SetRelationship(string _name)
    { 
        if (relationObj == null)
        {
            InitRelationObj();
        }
        if (_name == string.Empty || _name.Length == 0)
        {
            Destroy(relationObj.gameObject);
            Destroy(relationObj);
            relationObj = null; 
            return;
        } 
        SetRelationshipText(_name);
    }
    protected void InitRelationObj()
    {
        if (InterActive == null)
        {
            Debug.LogError("基本元素才可以带有头顶文字！");
            return;
        }
        if (TextParent == null)
        {
            Debug.LogError("组件启动出错！");
            return;
        }
        if (!flagList.ContainsKey(FlagType.Relationship)) flagList[FlagType.Relationship] = null;
        GameObject prefab = flagList[FlagType.Relationship];
        Color nameColor = Color.white;
        if (prefab == null)
        {
            UnityEngine.Object obj = null;
            obj = exResources.GetResource(ResourceType.TEXT, "PlayerName");
            if (obj != null)
            {
                prefab = Instantiate(obj) as GameObject;
                obj = null;
            }
            flagList[FlagType.Relationship] = prefab;
        }
        if (prefab != null)
        {
            prefab.transform.parent = TextParent.transform;
            prefab.transform.localEulerAngles = Vector3.zero;
            prefab.transform.localScale = Vector3.one;
            prefab.transform.localPosition = InitAchor(FlagType.Relationship);
            prefab.SetMaskLayer(InterActive.gameObject.layer);
            relationObj = prefab.GetComponent<HeadTextPrefab>();
        }
    }
    /// <summary>
    /// 设置名字文字
    /// </summary>
    /// <param name="_name"></param>
    protected void SetRelationshipText(string _name)
    {
        if (relationObj != null)
        {
            relationObj.SetText(_name);
            name_font = relationObj.name_font;
            bj_font = relationObj.bj_font;
            font_dmg = relationObj.font_dmg;
            font_get_dmg = relationObj.font_get_dmg;
            zl_font = relationObj.zl_font;
            font_pet_dmg = relationObj.font_pet_dmg;
            xy_font = relationObj.xy_font;
        }
         
    }
    /// <summary>
    /// 设置关系颜色
    /// </summary>
    /// <param name="_color"></param>
    public void SetRelationColor(Color _color)
    {
        if (relationObj != null)
        {
            relationObj.SetOutLineColor(_color);
        }
    }
    #endregion

    #region 名字
    /// <summary>
    /// 设置名字
    /// </summary>
    /// <param name="_name"></param>
    public void SetName(string _name)
	{
//        if (!_name.Contains("[b]"))
//        {
//            _name = "[b]" + _name;
//        }   
		if(nameObj == null)
		{
			InitNameObj();
		}
		SetNameText(_name); 
	}
    /// <summary>
    /// 初始化名字预制 by吴江
    /// </summary>
    /// <param name="_name"></param>
    protected void InitNameObj()
    {
        if (InterActive == null)
        {
            Debug.LogError("基本元素才可以带有头顶文字！");
            return;
        }
        if (TextParent == null)
        {
            Debug.LogError("组件启动出错！");
            return;
        }
        if (!flagList.ContainsKey(FlagType.Name)) flagList[FlagType.Name] = null;
        GameObject prefab = flagList[FlagType.Name];
        Color nameColor = Color.white;
        if (prefab == null)
        {
            UnityEngine.Object obj = null;
            switch (InterActive.typeID)
            {
                case ObjectType.Player:
                case ObjectType.Model:
                    obj = exResources.GetResource(ResourceType.TEXT, "PlayerName");
                    if (GameCenter.mainPlayerMng != null && GameCenter.mainPlayerMng.MainPlayerInfo != null && InterActive.id == GameCenter.mainPlayerMng.MainPlayerInfo.ServerInstanceID)
                    {
                        nameColor = new Color(0.498f, 0.984f, 0.3843f);
                    }
                    else
                    {
                        nameColor = new Color(1.0f, 1.0f, 1.0f);
                    }
                    break;
                case ObjectType.CGObject:
                    obj = exResources.GetResource(ResourceType.TEXT, "PlayerName");
                    break;
                case ObjectType.Entourage:
                    obj = exResources.GetResource(ResourceType.TEXT, "PlayerName");
                    break;
                case ObjectType.FlyPoint:
                    obj = exResources.GetResource(ResourceType.TEXT, "FlyPointName");
                    break;
                case ObjectType.NPC:
                    obj = exResources.GetResource(ResourceType.TEXT, "NPCName");
                    nameColor = new Color(1.0f, 0.949f, 0.149f);
                    break;
                case ObjectType.MOB:
                    obj = exResources.GetResource(ResourceType.TEXT, "MonsterName");
                    break;
                case ObjectType.SceneItem:
                    obj = exResources.GetResource(ResourceType.TEXT, "MonsterName");
                    break;
                case ObjectType.DropItem:
                    obj = exResources.GetResource(ResourceType.TEXT, "DropItemName");
                    break;
                default:
                    break;
            }
            if (obj != null)
            {
                prefab = Instantiate(obj) as GameObject;
                obj = null;
            }
            flagList[FlagType.Name] = prefab;
        }
        if (prefab != null)
        {
            prefab.transform.parent = TextParent.transform;
            prefab.transform.localEulerAngles = Vector3.zero;
            prefab.transform.localScale = Vector3.one; 
            prefab.transform.localPosition = InitAchor(FlagType.Name); 
            prefab.SetMaskLayer(InterActive.gameObject.layer);
            nameObj = prefab.GetComponent<HeadTextPrefab>(); 
            if (nameColor != Color.white)
            {
                nameObj.colorName = nameColor; 
                if (guildNameObj != null)
                {
                    guildNameObj.colorName = nameColor;
                }
            } 
        }
        else
        {
            Debug.LogError("找不到文字预制！");
        }

    }
	/// <summary>
	/// 设置名字文字
	/// </summary>
	/// <param name="_name"></param>
	protected void SetNameText(string _name)
	{
		if (nameObj != null)
            {
                nameObj.SetText(_name);
                name_font = nameObj.name_font;
                bj_font = nameObj.bj_font;
                font_dmg = nameObj.font_dmg;
                font_get_dmg = nameObj.font_get_dmg;
                zl_font = nameObj.zl_font;
				font_pet_dmg = nameObj.font_pet_dmg;
				xy_font = nameObj.xy_font;
            }

        SetNameColorAlongInterActive();
	}

    Color nameColor = Color.yellow;
    //Color outLineColor = Color.black;
    Color mainEntourageColor = new Color(85f/255f,229f/255f,1f,1f);
    Color otherEntourageColor = new Color(232f/255f,232f/255f,232f/255f,1f);
    /// <summary>
    /// 根据不同对象设置对象颜色 lzr
    /// </summary>
    public void SetNameColorAlongInterActive() 
    {
        if (GameCenter.curMainPlayer != null)
        {
            SceneType sceneType = GameCenter.curGameStage.SceneType;
            RelationCompareRef refData = null;
            switch (InterActive.typeID)
            {
                case ObjectType.Player:
                case ObjectType.Model:
                    PlayerBase pb = this.gameObject.GetComponent<PlayerBase>();
                    if (pb != null)
                    { 
						if(!pb.IsCounterAttack)
						{
							switch(pb.SlaSevel)//玩家名字的颜色由杀戮值决定,与队伍无关公会无关
							{
							case 1:
								nameColor = Color.white;
								break;
							case 2:
								nameColor = Color.yellow;
								break;
							case 3:
								nameColor = Color.red;
								break;
							case 4:
								nameColor = new Color(60f/255,179f/255f,1f);
								break;
							case 5:
								nameColor = new Color(160f/255f,32f/255f,240f/255f);//紫色
								break;
							}
						}else
						{
							nameColor = new Color(128f/255f,64f/255f,0f);//棕色
						}

                        if (GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneUiType == SceneUiType.BATTLEFIGHT)
                        {
                            refData = ConfigMng.Instance.GetRelationRef(GameCenter.curMainPlayer.Camp, pb.Camp, sceneType);
                            if (refData != null)
                            {
                                nameColor = refData.color; 
                            }
                        }

                        nameObj.SetColor(nameColor); 
                        //nameObj.SetOutLineColor(outLineColor);
                        //if (guildNameObj != null) //公会名字颜色不变
                        //{
                        //    guildNameObj.SetColor(nameColor);
                            //guildNameObj.SetOutLineColor(outLineColor);
                        //}
                    }
                    break;
                case ObjectType.MOB:
                    Monster mob = this.gameObject.GetComponent<Monster>();
                    if (mob != null)
                    {
                        if (!mob.IsDart)
                        {
                            refData = ConfigMng.Instance.GetRelationRef(GameCenter.curMainPlayer.Camp, mob.Camp, sceneType);
                            if (refData != null)
                            {
                                nameColor = refData.color;
                                //outLineColor = refData.colSide;
                            }
                        }
                        else
                        {
                            nameColor = mob.IsOwnerDart ? mainEntourageColor : otherEntourageColor;
                        }
                        nameObj.SetColor(nameColor);
                        //nameObj.SetOutLineColor(outLineColor);
                    }
                    break;
                case ObjectType.DropItem:
                    DropItem item = this.gameObject.GetComponent<DropItem>();
                    if (item != null)
                    {
                        nameObj.SetColor(item.QualityColor);
                    }
                    break;
                case ObjectType.Entourage:
                    EntourageBase Entourage = this.gameObject.GetComponent<EntourageBase>();
                    if (Entourage != null)
                    {
                        if (Entourage == GameCenter.curMainEntourage || (Entourage.Owner != null && Entourage.Owner == GameCenter.curMainPlayer))
                        {
                            nameColor = mainEntourageColor;
                        }
                        else
                        {
                            nameColor = otherEntourageColor;
                        }
                        //if (petNameObj != null)
                        //{
                        //    petNameObj.SetColor(nameColor);
                        //}
                        if (petOwnerNameObj != null)
                        {
                            petOwnerNameObj.SetColor(nameColor);
                        }
                        //nameObj.SetOutLineColor(outLineColor);
                    }
                    break;;
                case ObjectType.FlyPoint:
                    FlyPoint flypoint = this.gameObject.GetComponent<FlyPoint>();
                    if (flypoint != null)
                    {
                        //refData = ConfigMng.Instance.GetRelationRef(GameCenter.curMainPlayer.Camp, flypoint.Camp, sceneType);
                        //if (refData != null)
                        //{
                        //    nameColor = refData.color;
                        //    outLineColor = refData.colSide;
                        //}
                        nameObj.SetColor(nameColor);
                        //nameObj.SetOutLineColor(outLineColor);
                    }
                    break;
                default:
                    if (nameObj != null)//其他对象设置为中立
                    {
                        nameObj.SetColor(Color.yellow);
                    }
                    //if (petNameObj != null)
                    //{
                    //    petNameObj.SetColor(Color.yellow);
                    //} 
                    if (petOwnerNameObj != null)
                    {
                        petOwnerNameObj.SetColor(Color.yellow);
                    }
                    if (guildNameObj != null)
                    {
                        guildNameObj.SetColor(Color.yellow);
                    }
                    break;
            }
        }
        else
        {
            if (nameObj != null)
            {
                nameObj.SetColor(Color.yellow);
            }
            //if (petNameObj != null)
            //{
            //    petNameObj.SetColor(Color.yellow);
            //}
            if (petOwnerNameObj != null)
            {
                petOwnerNameObj.SetColor(Color.yellow);
            }
            if (guildNameObj != null)
            {
                guildNameObj.SetColor(Color.yellow);
            }
        }
    }


	/// <summary>
	/// 设置队友的名字颜色
	/// </summary>
    public void SetNameColor(int _pid, bool isTeammate)
    {
        SmartActor pb = this.gameObject.GetComponent<SmartActor>();
        if (pb == null || _pid != pb.id) return;
        Color nameColor = Color.yellow;
        //Color outLineColor = Color.black;
        if (isTeammate)
        {
            nameColor = new Color(0.4549f, 0.906f, 0.933f);
        }
        else
        {
            RelationCompareRef refData = ConfigMng.Instance.GetRelationRef(GameCenter.curMainPlayer.Camp, pb.Camp, GameCenter.curGameStage.SceneType);
            if (refData != null)
            {
                nameColor = refData.color;
                //outLineColor = refData.colSide;
            }
            if (nameObj != null)
            {
                nameObj.SetColor(nameColor);
            }
            //nameObj.SetOutLineColor(outLineColor);
            if (guildNameObj != null)
            {
                guildNameObj.SetColor(nameColor);
                //guildNameObj.SetOutLineColor(outLineColor);
            }
        }
        if (nameObj != null) nameObj.SetColor(nameColor);
        if (guildNameObj != null)
        {
            guildNameObj.colorName = nameColor;
        }
    }
    #endregion

    #region 工会名称
    /// <summary>
	/// 设置公会名字 by吴江
	/// </summary>
	public void SetGuildName(string _guildName)
	{
        if (_guildName == string.Empty)
        {
            if (guildNameObj != null)
            {
                Destroy(guildNameObj.gameObject);
                Destroy(guildNameObj);
                guildNameObj = null;
                UpdateFlagsHeight();
            }
            return;
        }
        _guildName = "[b]" + _guildName;
        if (guildNameObj == null)
        {
            InitGuildNameObj();
        }
        string temp = string.Empty;//转下表现形式
        if (_guildName.Equals(""))
        {
            temp = _guildName;
        }
        else
        {
            temp = new System.Text.StringBuilder().Append("<").Append(_guildName).Append(">").ToString();
        }
        SetGuildNameText(temp);
        UpdateFlagsHeight();
	}

    /// <summary>
    /// 设置公会名字 
    /// </summary>
    protected void InitGuildNameObj()
    {
        if (InterActive == null)
        {
            Debug.LogError("基本元素才可以带有头顶文字！");
            return;
        }
        if (TextParent == null)
        {
            Debug.LogError("组件启动出错！");
            return;
        }
        if (!flagList.ContainsKey(FlagType.GuildName)) flagList[FlagType.GuildName] = null;
        GameObject prefab = flagList[FlagType.GuildName];
        if (prefab == null)
        {
			UnityEngine.Object obj = null;
			obj = exResources.GetResource(ResourceType.TEXT, "PlayerName");       
	        if (obj != null)
	        {
	            prefab = Instantiate(obj) as GameObject;
	        }
	        flagList[FlagType.GuildName] = prefab;
		}
        if (prefab != null)
        {
            prefab.transform.parent = TextParent.transform;
            prefab.transform.localEulerAngles = Vector3.zero;
            prefab.transform.localScale = Vector3.one;
            prefab.transform.localPosition = InitAchor(FlagType.GuildName);
            prefab.SetMaskLayer(InterActive.gameObject.layer);
            guildNameObj = prefab.GetComponent<HeadTextPrefab>();
            guildNameObj.colorName = new Color(1.0f, 0.906f, 0.688f);
 
        }
        else
        {
            Debug.LogError("找不到文字预制！");
        }       
    }	
	protected void SetGuildNameText(string _guildName)
	{
		if (guildNameObj != null)
        {           
			name_font = nameObj.name_font;
			bj_font = nameObj.bj_font;
			font_dmg = nameObj.font_dmg;
			font_get_dmg = nameObj.font_get_dmg;
			zl_font = nameObj.zl_font;
			font_pet_dmg = nameObj.font_pet_dmg;
			xy_font = nameObj.xy_font;
			guildNameObj.SetText(_guildName);
        }
	}	

    #endregion



    #region 泡泡文字
    /// <summary>
    /// 设置泡泡
    /// </summary>
    /// <param name="_text"></param>
    public void SetBubble(string _text, float _time)
    {
        ForceDisableBubble();
        if (_text == string.Empty)
        {
            if (bubbleObj != null)
            {
                SetBubbleText(string.Empty);
                bubbleObj.gameObject.SetActive(false);
                SetFlagsActive(true);
            }
            return;
        }
        if (bubbleObj == null)
        {
            InitBubbleObj();
        }
        SetBubbleText(_text);
        bubbleObj.gameObject.SetActive(true);
        isBubbling = true;
        SetFlagsActive(false);
        StartCoroutine(DisableBubble(_time));
    }
    /// <summary>
    /// 关闭泡泡 by吴江
    /// </summary>
    /// <param name="_time"></param>
    /// <returns></returns>
    protected IEnumerator DisableBubble(float _time)
    {
        yield return new WaitForSeconds(_time);
        isBubbling = false;
        SetBubbleText(string.Empty);
        if (bubbleObj != null)
        {
            bubbleObj.gameObject.SetActive(false);
            SetFlagsActive(true);
        }
    }
    /// <summary>
    /// 停止当前的冒泡
    /// </summary>
    public void ForceDisableBubble()
    {
        StopCoroutine("DisableBubble");
        isBubbling = false;
        SetBubbleText(string.Empty);
        if (bubbleObj != null)
        {
            bubbleObj.gameObject.SetActive(false);
            SetFlagsActive(true);
        }
    }


    /// <summary>
    /// 初始化泡泡预制 by吴江
    /// </summary>
    /// <param name="_name"></param>
    protected void InitBubbleObj()
    {
        if (InterActive == null)
        {
            Debug.LogError("基本元素才可以带有头顶文字！");
            return;
        }
        if (TextParent == null)
        {
            Debug.LogError("组件启动出错！");
            return;
        }
        if (bubbleObj == null)
        {
            GameObject prefab = null;
            UnityEngine.Object obj = exResources.GetResource(ResourceType.TEXT, "Bubble");
            if (obj != null)
            {
                prefab = Instantiate(obj) as GameObject;
                obj = null;
            }
            else
            {
                Debug.LogError("找不到预制: Bubble");
            }
            if (prefab != null)
            {
                prefab.transform.parent = TextParent.transform;
                prefab.transform.localEulerAngles = Vector3.zero;
                prefab.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
                prefab.transform.localPosition = InitAchor(FlagType.Bubble);
                prefab.SetMaskLayer(InterActive.gameObject.layer);
                bubbleObj = prefab.GetComponent<BubblePrefab>();

            }
            else
            {
                Debug.LogError("找不到文字预制！");
            }
        }

    }
    /// <summary>
    /// 设置泡泡文字 by吴江
    /// </summary>
    /// <param name="_text"></param>
    protected void SetBubbleText(string _text)
    {
        if (bubbleObj != null)
        {
            bubbleObj.Content(_text);
        }
    }
    #endregion


    #region 领袖标记


    public enum LeaderTagType
    {
        None,
        Horde,
        HordeSecond,
        Alliance,
        AllianceSecond,
    }

    public void SetLeaderTag(LeaderTagType _tag)
    {
        if (_tag == LeaderTagType.None)
        {
            if (leaderTagPrefab != null)
            {
                Destroy(leaderTagPrefab.gameObject);
                Destroy(leaderTagPrefab);
                leaderTagPrefab = null;
                UpdateFlagsHeight();
            }
        }
        else
        {
            if (leaderTagPrefab == null)
            {
                InitLeaderTagPrefab();
            }
            leaderTagPrefab.Active(_tag);
        }
    }


    /// <summary>
    /// 初始化头顶血条预制 by吴江
    /// </summary>
    /// <param name="_name"></param>
    protected void InitLeaderTagPrefab()
    {
        if (InterActive == null)
        {
            Debug.LogError("基本元素才可以带有阵营领袖标记！");
            return;
        }
        if (TextParent == null)
        {
            Debug.LogError("组件启动出错！");
            return;
        }
        if (leaderTagPrefab == null)
        {
            GameObject prefab = null;
            UnityEngine.Object obj = exResources.GetResource(ResourceType.TEXT, "leaderHead");
            if (obj != null)
            {
                prefab = Instantiate(obj) as GameObject;
                obj = null;
            }
            else
            {
                Debug.LogError("找不到预制: leaderHead");
            }
            if (prefab != null)
            {
                prefab.transform.parent = TextParent.transform;
                prefab.transform.localEulerAngles = Vector3.zero;
                prefab.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                prefab.transform.localPosition = InitAchor(FlagType.LearderTag);
                prefab.SetMaskLayer(InterActive.gameObject.layer);
                leaderTagPrefab = prefab.GetComponent<LeaderTagPrefab>();
                flagList[FlagType.LearderTag] = prefab;

            }
            else
            {
                Debug.LogError("找不到领袖标记预制！");
            }
        }

    }
    #endregion


    #region 头顶血条
    public bool HasShowBlood
    {
        get
        {
            return bloodSliderObj != null && bloodSliderObj.isActiveAndEnabled;
        }
    }

    public void HideBlood()
    {
        if (bloodSliderObj != null)
        {
            bloodSliderObj.gameObject.SetActive(false);
        }
    }


    public void SetBloodEnable(bool _enable)
    {
        if (bloodSliderObj != null)
        {
            bloodSliderObj.SetEnable(_enable);
        }
    }

    /// <summary>
    /// 头顶血条
    /// </summary>
    /// <param name="_text"></param>
    public void SetBlood(float _rate, bool _isFreind)
    {
        if (!GameCenter.systemSettingMng.ShowBloodSlider || _rate <= 0.003f)
        {
            HideBlood();
            return;
        }
        if (_rate <= 0.003f) _rate = 0;
        if (bloodSliderObj == null)
        {
            InitBloodSliderObj();
            //bloodSliderObj.ShowBackFrame = true;
        }
        bloodSliderObj.gameObject.SetActive(_rate > 0.003f);
        SetBloodSlider(_rate, _isFreind);
    }

    /// <summary>
    /// 初始化头顶血条预制 by吴江
    /// </summary>
    /// <param name="_name"></param>
    protected void InitBloodSliderObj()
    {
        if (InterActive == null)
        {
            Debug.LogError("基本元素才可以带有头顶文字！");
            return;
        }
        if (TextParent == null)
        {
            Debug.LogError("组件启动出错！");
            return;
        }
        if (bloodSliderObj == null)
        {
            GameObject prefab = null;
            UnityEngine.Object obj = exResources.GetResource(ResourceType.TEXT, "BloodBar");
            if (obj != null)
            {
                prefab = Instantiate(obj) as GameObject;
                obj = null;
            }
            else
            {
                Debug.LogError("找不到预制: BloodBar");
            }
            if (prefab != null)
            {
                prefab.transform.parent = TextParent.transform;
                prefab.transform.localEulerAngles = Vector3.zero;
                prefab.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                prefab.transform.localPosition = InitAchor(FlagType.BloodSlider);
                prefab.SetMaskLayer(InterActive.gameObject.layer);
                bloodSliderObj = prefab.GetComponent<BloodSliderPrefab>();
                flagList[FlagType.BloodSlider] = prefab;

            }
            else
            {
                Debug.LogError("找不到血条预制！");
            }
        }

    }
    /// <summary>
    /// 设置血条 by吴江
    /// </summary>
    /// <param name="_text"></param>
    protected void SetBloodSlider(float _rate,bool _isFreind)
    {
        if (bloodSliderObj != null)
        {
            bloodSliderObj.SetRelationType(_isFreind);
            bloodSliderObj.Slider(_rate);
        }
    }
    #endregion

	#region 采集物品进度条  
    public void EndProgress()
    {
        if (collectProgressPrefab != null)
        {
            collectProgressPrefab.EndProgress();
            collectProgressPrefab.gameObject.SetActive(false);
        }
    }
    public void SetProgress(float _openTime, string des)
    {
        if (collectProgressPrefab == null)
        {
            InitCollectProgressObj();
        }
        collectProgressPrefab.gameObject.SetActive(true);
        SetCollectProgress(_openTime, des);
    }

    void SetCollectProgress(float _openTime, string des)
    {
        if (collectProgressPrefab != null)
            collectProgressPrefab.SetProgress(_openTime, des);
    }

    void InitCollectProgressObj()
    {
        if (InterActive == null)
        {
            Debug.LogError("基本元素才可以带有头顶文字！");
            return;
        }
        if (TextParent == null)
        {
            Debug.LogError("组件启动出错！");
            return;
        }
        if (collectProgressPrefab == null)
        {
            GameObject prefab = null;
            UnityEngine.Object obj = exResources.GetResource(ResourceType.GUI, "Miscellany/Bar_panel");
            if (obj != null)
            {
                prefab = Instantiate(obj) as GameObject;
                obj = null;
            }
            else
            {
                Debug.LogError("找不到预制: Miscellany/Bar_panel");
            }
            if (prefab != null)
            {
                prefab.transform.parent = TextParent.transform;
                prefab.transform.localEulerAngles = Vector3.zero;
                prefab.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
                prefab.transform.localPosition = InitAchor(FlagType.CollectProgress);
                prefab.SetMaskLayer(InterActive.gameObject.layer);
                collectProgressPrefab = prefab.GetComponent<CollectProgressPrefab>();
            }
            else
            {
                Debug.LogError("找不到读条条预制！");
            }
        }
    }
	#endregion


    #region 设置头顶特效（怪物头上的惊叹号）lzr

    public void SetHeadEffect(float _time)
    {
        if (headEffectObj == null)
        {
            InitHeadEffect();
        }
        Invoke("HideHeadEffect", _time);
    }

     public void InitHeadEffect() 
    {
        if (InterActive == null)
        {
            Debug.LogError("基本元素才可以带有头顶特效！");
            return;
        }
        if (TextParent == null)
        {
            Debug.LogError("组件启动出错！");
            return;
        }
        if (!flagList.ContainsKey(FlagType.TipEffect)) flagList[FlagType.TipEffect] = null;
        GameObject prefab = flagList[FlagType.TipEffect];
        if (prefab == null)
        {
            UnityEngine.Object obj = exResources.GetResource(ResourceType.GUI, "MainDungeon/Monster_Warning_Effects");
            if (obj != null)
            {
                prefab = Instantiate(obj) as GameObject;
                obj = null;
            }
            flagList[FlagType.TipEffect] = prefab;
        }
        if (prefab != null)
        {
            prefab.transform.parent = TextParent.transform;
            prefab.transform.localEulerAngles = Vector3.zero;
            prefab.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
            prefab.transform.localPosition = InitAchor(FlagType.CountDown);
            prefab.SetMaskLayer(InterActive.gameObject.layer);
            headEffectObj = prefab;
            headEffectObj.SetActive(true);
        }
        else
        {
            Debug.LogError("找不到特效预制！");
        }
    }

    void HideHeadEffect() 
    {
        if (headEffectObj != null) headEffectObj.SetActive(false);
    }

    #endregion




}
