//==================================
//作者：朱素云
//日期：2016/3/7
//用途：单个随从数据
//=================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SingleSpritAnimal : MonoBehaviour
{

    #region 控件数据
    /// <summary>
    /// 随从头像
    /// </summary>
    public UISprite headIcon;
    /// <summary>
    /// 随从名字
    /// </summary>
    public UILabel nameLabel;
    /// <summary>
    /// 随从战斗力
    /// </summary>
    public UILabel powerLabel;
    /// <summary>
    /// 随从出战标记
    /// </summary>
    public UISprite leaderTag;
    /// <summary>
    /// 守护标志
    /// </summary>
    public UISprite guardTag;
    /// <summary>
    /// 边框品质
    /// </summary>
    public UISprite quality;
    /// <summary>
    /// 宠物id
    /// </summary>
    public int id;
    protected MercenaryInfo info = null; 
    public MercenaryInfo MercenaryInfoData
    {
        get
        {
            return info;
        }
        set
        {
            if (value != null) info = value;
            info.OnPetNameUpdate -= RefreshName;
            info.OnPetNameUpdate += RefreshName; 
        }
    }

    protected List<Color> qualityList = new List<Color>();
    
    
    #endregion 
    #region 构造
    public static SingleSpritAnimal CeateNew(GameObject _parent,int _index)
    { 
        if (_index < 0)
        {
            GameSys.LogError("位置序数不能为负数！");
            return null;
        } 
        Object prefab = null;
        if (prefab == null)
        {
            prefab = exResources.GetResource(ResourceType.GUI, "SpiritAnimal/Single_Congwu");
        }
        if (prefab == null)
        {
            GameSys.LogError("找不到预制：SpiritAnimal/Single_Congwu");
            return null;
        }
        GameObject obj = Instantiate(prefab) as GameObject;
        prefab = null;
        obj.transform.parent = _parent.transform; 
        obj.transform.localPosition = new Vector3(0, - _index * 110, 0);
        obj.transform.localScale = Vector3.one;
        SingleSpritAnimal itemUI = obj.GetComponent<SingleSpritAnimal>();
        if (itemUI == null)
        {
            GameSys.LogError("预制上找不到组件：<SingleSpritAnimal>");
            return null;
        }
        return itemUI;
    }
    #endregion


    #region 刷新
    void RefreshName(string _newName)
    { 
        nameLabel.text = info.PetName;
    }
    public void SetInfo(MercenaryInfo _info)
    {
        MercenaryInfoData = _info;
        //if (qualityList.Count < 6)
        //{
        //    qualityList.Clear();
        //    qualityList.Add(new Color(231f / 255, 1, 232f / 255));
        //    qualityList.Add(new Color(110f / 255, 245f / 255, 116f / 255));
        //    qualityList.Add(new Color(60f / 255, 179f / 255, 1));
        //    qualityList.Add(new Color(189f / 255, 84f / 255, 1));
        //    qualityList.Add(new Color(1, 106 / 255, 44f / 255));
        //    qualityList.Add(new Color(1, 0, 0));
        //}
        headIcon.spriteName = _info.Icon; 
        powerLabel.text = _info.Power.ToString();
        id = _info.ConfigId;
        nameLabel.text = _info.PetName;
        quality.color = _info.PetNameColor;

        //int qualityVal = _info.Aptitude;
        //if (qualityVal < 21)
        //{
        //    if (qualityList.Count > 0) quality.color = qualityList[0];
        //}
        //else if (qualityVal < 41)
        //{
        //    if (qualityList.Count > 1) quality.color = qualityList[1];
        //}
        //else if (qualityVal < 61)
        //{
        //    if (qualityList.Count > 2) quality.color = qualityList[2];
        //}
        //else if (qualityVal < 81)
        //{
        //    if (qualityList.Count > 3) quality.color = qualityList[3];
        //}
        //else if (qualityVal < 101)
        //{
        //    if (qualityList.Count > 4) quality.color = qualityList[4];
        //}
        //else
        //{
        //    if (qualityList.Count > 5) quality.color = qualityList[5];
        //}
        if (_info.IsActive == (int)PetChange.GUARD)//守护
        {
            leaderTag.gameObject.SetActive(false);
            guardTag.gameObject.SetActive(true);
        }
        else if (_info.IsActive == (int)PetChange.FINGHTING)//出战
        {
            leaderTag.gameObject.SetActive(true);
            guardTag.gameObject.SetActive(false);
        }
        else//没有出战状态
        {
            leaderTag.gameObject.SetActive(false);
            guardTag.gameObject.SetActive(false);
        }
    }
    #endregion


}
