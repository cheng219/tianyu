//==================================
//作者：朱素云
//日期：2016/3/4
//用途：宠物技能ui
//=================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SingleSkill : MonoBehaviour 
{ 
    public UISprite lockSp;//锁
    public UILabel nameLab;//名字 
    public UISprite icon;
    protected List<Color> qualityList = new List<Color>();
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
            ShowSkill();
        }
    } 
    public static SingleSkill CeateNew(int _index, int _id, GameObject gameParent)
    { 
        Object prefab = null;
        if (prefab == null)
        {
            prefab = exResources.GetResource(ResourceType.GUI, "SpiritAnimal/PetSkill");
        }
        if (prefab == null)
        {
            GameSys.LogError("找不到PetSkill预制");
            return null;
        }
        GameObject obj = Instantiate(prefab) as GameObject;
        prefab = null;
        obj.transform.parent = gameParent.transform;
        obj.transform.localPosition = new Vector3(20 + (_index%5)*102, -(80 + (_index/5)*102));
        obj.transform.localScale = Vector3.one; 
        SingleSkill itemUI = obj.GetComponent<SingleSkill>();
        if (itemUI == null)
        {
            GameSys.LogError("预制上找不到组件：<SingleSkill>");
            return null;
        } 
        return itemUI;
    }
    void ShowSkill()
    { 
        if (lockSp != null)
        { 
            int qualityVal = SkillRef.quality;
            if (qualityList.Count < 5)
            {
                GetColor();
            }
            if (qualityVal == 1)
            {
                if (qualityList.Count > 0) lockSp.color = qualityList[0];
                if (nameLab != null) nameLab.text = "[e7ffe8]" + skillRef.name;
            }
            else if (qualityVal == 2)
            {
                if (qualityList.Count > 1) lockSp.color = qualityList[1];
                if (nameLab != null) nameLab.text = "[6ef574]" + skillRef.name;
            }
            else if (qualityVal == 3)
            {
                if (qualityList.Count > 2) lockSp.color = qualityList[2];
                if (nameLab != null) nameLab.text = "[3cb3ff]" + skillRef.name;
            }
            else if (qualityVal == 4)
            {
                if (qualityList.Count > 3) lockSp.color = qualityList[3];
                if (nameLab != null) nameLab.text = "[bd54ff]" + skillRef.name;
            }
            else
            {
                if (qualityList.Count > 4) lockSp.color = qualityList[4];
                if (nameLab != null) nameLab.text = "[ff6a29]" + skillRef.name;
            }
        } 
        if (icon != null) icon.spriteName = skillRef.skillIcon;
    }

    void GetColor()
    {
        qualityList.Clear();
        qualityList.Add(new Color(231f / 255, 1, 232f / 255));//白色
        qualityList.Add(new Color(110f / 255, 245f / 255, 116f / 255));//绿色
        qualityList.Add(new Color(60f / 255, 179f / 255, 1));//蓝色 
        //qualityList.Add(new Color(189f / 255, 84f / 255, 1));//橙色
        qualityList.Add(new Color(1, 106 / 255, 41));//橙色
        qualityList.Add(new Color(1, 123 / 255, 44f / 255));//紫色  
    }
}
