//==================================
//作者：朱素云
//日期：2016/3/25
//用途：宠物技能书
//=================================
using UnityEngine;
using System.Collections;

public class BookItem : MonoBehaviour
{ 
    public UIButton chooseBtn;//选择
    public UISpriteEx chooseEx;
    public UIButton chooseYetBtn;//已选择
    public ChooseType curType; 

    protected NewPetSkillRef petSkillRef;
    public NewPetSkillRef petSkillRefDate 
    {
        get
        {
            return petSkillRef;
        }
        set
        {
            if (value != null)
                petSkillRef = value; 
        }
    }

    void Awake()
    {
        if (chooseBtn != null) chooseBtn.gameObject.SetActive(true);
        if (chooseYetBtn != null) chooseYetBtn.gameObject.SetActive(false);
        curType = ChooseType.GETBOOK;
        if (chooseEx != null) chooseEx.IsGray = UISpriteEx.ColorGray.normal;
    } 
    public static BookItem CeateNew(int _index,GameObject gameParent, bool isFilt)
    { 
        Object prefab = null;
        if (prefab == null)
        {
            prefab = exResources.GetResource(ResourceType.GUI, "SpiritAnimal/AnimalItem");
        }
        if (prefab == null)
        {
            GameSys.LogError("找不到AnimalItem预制");
            return null;
        }  
        GameObject obj = Instantiate(prefab) as GameObject;
        obj.transform.parent = gameParent.transform;
        if (isFilt) obj.transform.localPosition = new Vector3((_index % 2) * 120, -(_index / 2) * 168);
        else
            obj.transform.localPosition = new Vector3(90*_index, 0);
        obj.transform.localScale = Vector3.one;
        BookItem itemUI = obj.GetComponent<BookItem>(); 
        if (itemUI == null)
        {
            GameSys.LogError("预制上找不到组件：<BookItem>");
            return null;
        }
        return itemUI;
    }
    public static BookItem CeateBook(int _index, GameObject gameParent ,bool _isSetPos)
    { 
        Object prefab = null;
        if (prefab == null)
        {
            prefab = exResources.GetResource(ResourceType.GUI, "SpiritAnimal/AnimalItem");
        }
        if (prefab == null)
        {
            GameSys.LogError("找不到AnimalItem预制");
            return null;
        }
        GameObject obj = Instantiate(prefab) as GameObject;
        obj.transform.parent = gameParent.transform;
        if (_isSetPos) obj.transform.localPosition = new Vector3(_index % 4 * 144 + 50, -_index / 4 * 120 - 60);
        else
        {
            if (GameCenter.mercenaryMng.petSkillByCopyAll.Count == 1)
                obj.transform.localPosition = new Vector3(200, -50);
            else
                obj.transform.localPosition = new Vector3((_index%5)*120, -(_index/5)*150);
        }
        obj.transform.localScale = Vector3.one;
        BookItem itemUI = obj.GetComponent<BookItem>(); 
        if (itemUI == null)
        {
            GameSys.LogError("预制上找不到组件：<BookItem>");
            return null;
        }
        return itemUI;
    }    
}

public enum ChooseType
{ 
    /// <summary>
    /// 获得技能书放入背包
    /// </summary>
    GETBOOK,
    /// <summary>
    /// 将背包中的技能书放入材料
    /// </summary>
    GETMAT,
}
