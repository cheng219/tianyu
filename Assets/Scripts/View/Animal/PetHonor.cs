//==================================
//作者：朱素云
//日期：2016/3/10
//用途：宠物头衔ui
//=================================
using UnityEngine;
using System.Collections;

public class PetHonor : MonoBehaviour 
{
    /// <summary>
    /// 头衔头像
    /// </summary>
    public UISpriteEx honorIcon; 
    /// 成长条件
    /// </summary>
    public UILabel growLab;
    /// <summary>
    /// 增加的属性值
    /// </summary>
    public UILabel addLab;  
    private TitleRef titleRef;
    public TitleRef TitleRef
    {
        set
        {
            if (value != null)
                titleRef = value;
            ShowHonorInfo();
        }
        get
        {
            return titleRef;
        }
    }

    public static PetHonor CeateNew(int _index, int _id,  GameObject gameParent)
    {
         
        Object prefab = exResources.GetResource(ResourceType.GUI, "SpiritAnimal/PetTitle"); 
        if (prefab == null)
        {
            GameSys.LogError("找不到宠物头衔预制");
            return null;
        }
        GameObject obj = Instantiate(prefab) as GameObject;
        obj.transform.parent = gameParent.transform;
        obj.transform.localPosition = new Vector3();
        obj.transform.localScale = Vector3.one;
        prefab = null;
        PetHonor itemUI = obj.GetComponent<PetHonor>(); 
        if (itemUI == null)
        {
            GameSys.LogError("预制上找不到组件：<PetHonor>");
            return null;
        }
        itemUI.TitleRef = ConfigMng.Instance.GetTitlesRef(_id); 
        return itemUI;
    }
    void ShowHonorInfo()
    {
        if (TitleRef == null) return;
        if (honorIcon != null)
        {
            honorIcon.spriteName = TitleRef.icon;
            honorIcon.MakePixelPerfect();
        }
        if (growLab != null && TitleRef.judgeNum.Count > 0) growLab.text = TitleRef.judgeNum[0].ToString();
        int val = 0;
        if (TitleRef.attribute.Count > 0)
        { 
            for (int i = 0,max = TitleRef.attribute.Count; i < max; i++)
            { 
               val += TitleRef.attribute[i].count;
            }
          addLab.text = val.ToString();//攻击上限 + 攻击下限
        } 
    }
}
