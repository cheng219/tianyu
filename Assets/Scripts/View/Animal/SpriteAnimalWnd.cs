//==================================
//作者：朱素云
//日期：2016/3/4
//用途：宠物窗口类
//=================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpriteAnimalWnd : GUIBase
{

    #region  数据
    public UIButton closeBtn;//关闭按钮  
    public GameObject petBack;//宠物界面背景   
    /// <summary>
    /// 子界面控件
    /// </summary> 
    public UIToggle petWndTog;
    public UISprite markPet; 

    public UIToggle[] petSubWndTog;
     /// <summary>
    /// 随从列表的父控件
    /// </summary>
    public GameObject animaListParent; 
    protected FDictionary singleSpritAnimalList =  new FDictionary();
    protected FDictionary mercenaryInfoList
    {
        get
        {
            return GameCenter.mercenaryMng.mercenaryInfoList;
        }
    }
    protected PetSubWndType CurPetSubWndType =  PetSubWndType.none;

    private bool isCreateItems = false;
    #endregion 
   
     
    #region 构造

    void Awake()
    { 
        mutualExclusion = true;
        layer = GUIZLayer.NORMALWINDOW;
        allSubWndNeedInstantiate = true;
    }
    void Start()
    {
        if (closeBtn != null) UIEventListener.Get(closeBtn.gameObject).onClick = OnClickCloseBtn;
        if (petWndTog != null) UIEventListener.Get(petWndTog.gameObject).onClick = OnClickPetTog; 
    }
    void Update()
    {
        if (isCreateItems)
        {
            isCreateItems = false;
            RefreshList();
        }
    }
    protected override void OnOpen()
    {
        if (initSubGUIType == SubGUIType.NONE) initSubGUIType = SubGUIType.PETINFORMATION;
        base.OnOpen();
        for (int i = 0, max = petSubWndTog.Length; i < max; i++)
        {
            if (petSubWndTog[i] != null) UIEventListener.Get(petSubWndTog[i].gameObject).onClick = ClickToggleEvent;
        } 
        if (GameCenter.mercenaryMng.curMercernaryInfo != null)
            GameCenter.mercenaryMng.curPetId = GameCenter.mercenaryMng.curMercernaryInfo.ConfigId;
        isCreateItems = true;  
    }
    protected override void OnClose()
    { 
        base.OnClose();
        isCreateItems = false; 
    }
    protected override void HandEvent(bool _bind)
    {
        base.HandEvent(_bind);
        if (_bind)
        {  
            GameCenter.mercenaryMng.OnMercenaryListUpdate += RefreshList; 
        }
        else
        { 
            GameCenter.mercenaryMng.OnMercenaryListUpdate -= RefreshList; 
        }
    }
    protected UIToggle lastChangeToggle = null;
    protected void ClickToggleEvent(GameObject go)
    {
        UIToggle toggle = go.GetComponent<UIToggle>();
        if (toggle != lastChangeToggle)
        {
            OnChange();
        }
        if (toggle != null && toggle.value) lastChangeToggle = toggle;
    }
    protected void OnClickPetTog(GameObject go)
    {  
        petWndTog.value = true;
        UIToggle toggle = null;
        if (petSubWndTog != null && petSubWndTog.Length > (int)PetSubWndType.PETINFORMATION) toggle = petSubWndTog[(int)PetSubWndType.PETINFORMATION]; 
        if (toggle != null)
        { 
            toggle.value = true;
            ClickToggleEvent(toggle.gameObject);
        }
    }
    protected override void InitSubWndState()
    {
        base.InitSubWndState();
        UIToggle toggle = null;
        markPet.gameObject.SetActive(true);
        petBack.gameObject.SetActive(true);
        petWndTog.value = true;
        switch (InitSubGUIType)
        {
            case SubGUIType.PETINFORMATION:
                if (petSubWndTog != null && petSubWndTog.Length > (int)PetSubWndType.PETINFORMATION) toggle = petSubWndTog[(int)PetSubWndType.PETINFORMATION]; 
                break;
            case SubGUIType.GROWUP:
                if (petSubWndTog != null && petSubWndTog.Length > (int)PetSubWndType.GROWUP) toggle = petSubWndTog[(int)PetSubWndType.GROWUP]; 
                break;
            case SubGUIType.LINGXIU:
                if (petSubWndTog != null && petSubWndTog.Length > (int)PetSubWndType.LINGXIU) toggle = petSubWndTog[(int)PetSubWndType.LINGXIU]; 
                break;
            case SubGUIType.FUSE:
                if (petSubWndTog != null && petSubWndTog.Length > (int)PetSubWndType.FUSE) toggle = petSubWndTog[(int)PetSubWndType.FUSE]; 
                break;
            case SubGUIType.GUARD:
                if (petSubWndTog != null && petSubWndTog.Length > (int)PetSubWndType.GUARD) toggle = petSubWndTog[(int)PetSubWndType.GUARD]; 
                break;
            case SubGUIType.PETSKILL: 
                if (petSubWndTog != null && petSubWndTog.Length > (int)PetSubWndType.PETSKILL) toggle = petSubWndTog[(int)PetSubWndType.PETSKILL]; 
                break;
            case SubGUIType.MOUNT:
                if (petSubWndTog != null && petSubWndTog.Length > (int)PetSubWndType.MOUNT) toggle = petSubWndTog[(int)PetSubWndType.MOUNT];
                petBack.gameObject.SetActive(false);
                petWndTog.value = false; 
                markPet.gameObject.SetActive(false); 
                break;
            case SubGUIType.ILLUSION:
                if (petSubWndTog != null && petSubWndTog.Length > (int)PetSubWndType.ILLUSION) toggle = petSubWndTog[(int)PetSubWndType.ILLUSION];
                petBack.gameObject.SetActive(false);
                petWndTog.value = false; 
                markPet.gameObject.SetActive(false); 
                break;
            case SubGUIType.MOUNTEQUIP:
                if (petSubWndTog != null && petSubWndTog.Length > (int)PetSubWndType.MOUNTEQUIP) toggle = petSubWndTog[(int)PetSubWndType.MOUNTEQUIP];
                break;
            default: 
                break;
        }
        if (toggle != null)
        { 
            toggle.value = true; 
            ClickToggleEvent(toggle.gameObject);
        }
    } 
    protected void OnChange()
    {
        CloseAllSubWnd(); 
        for (int i = 0, max = petSubWndTog.Length; i < max; i++)
        {
            if (petSubWndTog[i].value)
            {  
                markPet.gameObject.SetActive(true);
                petBack.gameObject.SetActive(true);
                petWndTog.value = true;
                CurPetSubWndType = (PetSubWndType)i; 
                SwitchToSubWnd(subWndArray[i].type); 
                switch (CurPetSubWndType)
                { 
                    case PetSubWndType.MOUNT: 
                    case PetSubWndType.ILLUSION: 
                    case PetSubWndType.MOUNTEQUIP:
                        petBack.gameObject.SetActive(false);
                        petWndTog.value = false;
                        markPet.gameObject.SetActive(false);
                        return;
                    default: 
                        return;
                }
            }
            else
            {
                subWndArray[i].CloseUI();
            } 
        }  
    } 
    /// <summary>
    /// 关闭宠物所有子界面
    /// </summary>
    void CloseAllSubWnd()
    {
        for (int i = 0, max = petSubWndTog.Length; i < max; i++)
        {
            if (petSubWndTog[i].value)
            { 
                subWndArray[i].CloseUI();
            }
        } 
    }
    #endregion


    #region 控件事件
    /// <summary>
    /// 关闭界面
    /// </summary>
    /// <param name="go"></param>
    void OnClickCloseBtn(GameObject go)
    {  
        GameCenter.uIMng.SwitchToUI(GUIType.NONE);
        GameCenter.uIMng.ReleaseGUI(GUIType.SPRITEANIMAL);
    }
    void OnClickSingleAnimal(GameObject _go)
    { 
        if (_go.GetComponent<SingleSpritAnimal>() != null)
        { 
            int petId = _go.GetComponent<SingleSpritAnimal>().id;
            if (GameCenter.mercenaryMng.zhuPetId != MercenaryMng.noPet)
            {
                if (GameCenter.mercenaryMng.zhuPetId == petId)
                {
                    GameCenter.mercenaryMng.zhuPetId = MercenaryMng.noPet;
                    if (GameCenter.mercenaryMng.OnZhuPetUpdate != null) GameCenter.mercenaryMng.OnZhuPetUpdate();
                }
                else
                {
                    if (GameCenter.mercenaryMng.fuPetId != petId)
                    {
                        GameCenter.mercenaryMng.fuPetId = petId;
                        if (GameCenter.mercenaryMng.OnFuPetUpdate != null) GameCenter.mercenaryMng.OnFuPetUpdate();
                    }
                    else
                    {
                        GameCenter.mercenaryMng.fuPetId = MercenaryMng.noPet;
                        if (GameCenter.mercenaryMng.OnFuPetUpdate != null) GameCenter.mercenaryMng.OnFuPetUpdate();
                    }
                }
            }
            else
            {
                if (petId != GameCenter.mercenaryMng.fuPetId)
                {
                    GameCenter.mercenaryMng.zhuPetId = petId;
                    if (GameCenter.mercenaryMng.OnZhuPetUpdate != null) GameCenter.mercenaryMng.OnZhuPetUpdate();
                }
                else
                {
                    GameCenter.mercenaryMng.fuPetId = MercenaryMng.noPet;
                    if (GameCenter.mercenaryMng.OnFuPetUpdate != null) GameCenter.mercenaryMng.OnFuPetUpdate();
                }
            }
            //控制信息界面的显示 
            GameCenter.mercenaryMng.curPetId = petId; 
            if (GameCenter.mercenaryMng.OnMercenaryListUpdate != null) GameCenter.mercenaryMng.OnMercenaryListUpdate(); 
        } 
    }
    #endregion

    #region 刷新
    protected void RefreshList()
    { 
        foreach (SingleSpritAnimal sprite in singleSpritAnimalList.Values)
        {
            sprite.gameObject.SetActive(false); 
        }  
        if (mercenaryInfoList.Count>0)
        {
            CreateAnimal(); 
        } 
    }

    void CreateAnimal()
    {
        int id = GameCenter.mercenaryMng.curPetId;   
        int index = 0;
        foreach (MercenaryInfo item in mercenaryInfoList.Values)
        {
            if (!singleSpritAnimalList.ContainsKey(item.ConfigId))
            {
                singleSpritAnimalList[item.ConfigId] = SingleSpritAnimal.CeateNew(animaListParent, index);
            }
            SingleSpritAnimal animal = singleSpritAnimalList[item.ConfigId] as SingleSpritAnimal;
            animal.gameObject.SetActive(true);
            animal.transform.localPosition = new Vector3(0, -index * 110, 0);
            animal.SetInfo(item);
            UIEventListener.Get(animal.gameObject).onClick -= OnClickSingleAnimal;
            UIEventListener.Get(animal.gameObject).onClick += OnClickSingleAnimal;
            index++;
        }
        if (id != MercenaryMng.noPet)
        {
            SingleSpritAnimal animal = singleSpritAnimalList[id] as SingleSpritAnimal;
            if (animal != null)
            {
                UIToggle tog = animal.GetComponent<UIToggle>();
                if (tog != null) tog.value = true;
            }
        }
    }
    #endregion
     
}

public enum PetSubWndType
{
    PETINFORMATION,
    GROWUP,
    LINGXIU,
    FUSE,
    GUARD,
    PETSKILL,
    MOUNT,
    ILLUSION,
    MOUNTEQUIP,
    none,
}
