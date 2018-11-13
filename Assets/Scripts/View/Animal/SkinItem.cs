//==================================
//作者：朱素云
//日期：2016/4/15
//用途：坐骑幻化ui
//=================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkinItem : MonoBehaviour {

    public UILabel nameLab; 
    public ItemUI item;
    public UITimer timer;
    public UILabel timeLab; 
    public GameObject choosed;

    private MountRef mountRef;
    public MountRef MountRefDate
    {
        get
        {
            return mountRef;
        }
        set
        {
            if (value != null)
                mountRef = value;
            ShowMountInfo();
        }
    }
    void Awake()
    {
        choosed.SetActive(false);
    }
    public static SkinItem CeateNew(int _index, int _id, GameObject gameParent)
    {  
        Object prefab = null;
        if (prefab == null)
        {
            prefab = exResources.GetResource(ResourceType.GUI, "SpiritAnimal/ChangeList");
        }
        if (prefab == null)
        {
            GameSys.LogError("找不到ChangeList预制");
            return null;
        }
        GameObject obj = Instantiate(prefab) as GameObject;
        prefab = null;
        obj.transform.parent = gameParent.transform;
        obj.transform.localScale = new Vector3(0.8f,0.8f,0.8f);
        SkinItem itemUI = obj.GetComponent<SkinItem>(); 
        if (itemUI == null)
        {
            GameSys.LogError("预制上找不到组件：<SkinItem>");
            return null;
        }
        return itemUI;
    }
    void ShowMountInfo()
    { 
        //显示幻兽信息
        if (MountRefDate != null)
        {
            int id = MountRefDate.mountId; 
            FDictionary mountSkinInfoDic = GameCenter.newMountMng.mountSkinList;
            nameLab.text = MountRefDate.mountName;
            if (item != null)
            {
                item.FillInfo(new EquipmentInfo(MountRefDate.itemID, EquipmentBelongTo.PREVIEW));
                item.itemName.gameObject.SetActive(false);
                if (item.itemIcon.GetComponent<UISpriteEx>() != null)
                    item.itemIcon.GetComponent<UISpriteEx>().IsGray = UISpriteEx.ColorGray.normal;
                if (!mountSkinInfoDic.ContainsKey(id))
                {
                    if (item.itemIcon.GetComponent<UISpriteEx>() != null)
                        item.itemIcon.GetComponent<UISpriteEx>().IsGray = UISpriteEx.ColorGray.Gray;
                }
                else
                {
                    //MountInfo skin = mountSkinInfoDic[id] as MountInfo;  
                }
            }  
            if (mountSkinInfoDic.ContainsKey(id))//该皮肤玩家已经拥有
            { 
                MountInfo info = mountSkinInfoDic[id] as MountInfo;
                if (info.SkinRemainTime != 0)//限时
                {
                    timer.StartIntervalTimer(info.SkinRemainTime);
                    timer.onTimeOut = (x) =>
                    {
                        timeLab.text = ConfigMng.Instance.GetUItext(84);
                    };
                }
                else//这个幻兽是永久拥有的
                {
                    timeLab.gameObject.SetActive(false);
                }
                return;
            }
            timeLab.text = ConfigMng.Instance.GetUItext(84);
        }
    }
}
