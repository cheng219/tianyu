//==================================
//作者：朱素云
//日期：2016/7/9
//用途：精彩活动界面类
//=================================

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WdfActiveWnd : GUIBase
{
    public GameObject closeBtn;
    //public UISprite activeSpite;
    public UISprite openSpite;//开服活动图片
    public UISprite wonderfulSpite;//精彩活动图片
    public UISprite combineSpite;//和服活动图片
    public UISprite holidaySpite;//节日活动图片
    //public WdfActiveOneItem activeOneItem;
    //public WdfActiveTwoItem activeTwoItem;
    //public WdfActiveOneItem activeThreeItem;//登陆红利
    //public WdfActiveFourItem activeFourItem;//连充豪礼用到
    public GameObject activeTypeInstance;
    public GameObject activeTypeGird;
    private bool isFirstActiveTime = true;

    WdfActiveTypeData curWdfActiveItemInfo;

    List<WdfActiveTypeItem> typeList = new List<WdfActiveTypeItem>();
    void Awake()
    {
        mutualExclusion = true;
        allSubWndNeedInstantiate = true;
    }
    protected override void OnOpen()
    {
        base.OnOpen();
        GameCenter.wdfActiveMng.C2S_AskAllActivitysInfo();

    }
    protected override void OnClose()
    {
        base.OnClose();
        GameCenter.wdfActiveMng.CurWdfActiveType = 0;
        GameCenter.wdfActiveMng.needReset = true;
        GameCenter.wdfActiveMng.ResetRed();
    }
    protected override void HandEvent(bool _bind)
    {
        base.HandEvent(_bind);
        if (_bind)
        {
            if (closeBtn != null) UIEventListener.Get(closeBtn).onClick += CloseThis;
            GameCenter.wdfActiveMng.OnGetAllActiveTypes += RefreshActiveType;
            GameCenter.wdfActiveMng.OnGetAllActiveInfo += RefreshActiveItem;
            GameCenter.wdfActiveMng.RefreshRed += RefreshRed;
        }
        else
        {
            if (closeBtn != null) UIEventListener.Get(closeBtn).onClick -= CloseThis;
            GameCenter.wdfActiveMng.OnGetAllActiveTypes -= RefreshActiveType;
            GameCenter.wdfActiveMng.OnGetAllActiveInfo -= RefreshActiveItem;
            GameCenter.wdfActiveMng.RefreshRed -= RefreshRed;

        }
    }
    //void Update()
    //{

    //}


    void DestroyItem()
    {
        if (activeTypeGird != null)
        {
            activeTypeGird.transform.DestroyChildren();
        }
    }



    void RefreshActiveType()
    {
        if (openSpite != null) openSpite.gameObject.SetActive(GameCenter.wdfActiveMng.CurWdfActiveMainType == WdfType.OPEN);
        if (wonderfulSpite != null) wonderfulSpite.gameObject.SetActive(GameCenter.wdfActiveMng.CurWdfActiveMainType == WdfType.WONDERFUL);
        if (combineSpite != null) combineSpite.gameObject.SetActive(GameCenter.wdfActiveMng.CurWdfActiveMainType == WdfType.COMBINE);
        if (holidaySpite != null) holidaySpite.gameObject.SetActive(GameCenter.wdfActiveMng.CurWdfActiveMainType == WdfType.HOLIDAY);
        int id=0;
        Vector3 V3 = Vector3.zero;
        DestroyItem();
        typeList.Clear();
        for (int i = 0,max = GameCenter.wdfActiveMng.WdfActiveTypeDic.Count; i < max; i++)
        {
            //if (GameCenter.wdfActiveMng.WdfActiveTypeDic[i].type == GameCenter.wdfActiveMng.CurWdfActiveMainType)
            //{
                if (activeTypeInstance == null || activeTypeGird==null)
                    return;
                GameObject obj = Instantiate(activeTypeInstance) as GameObject;
                if (obj == null)
                    return;
                Transform parentTransf = activeTypeGird.transform;
                obj.transform.parent = parentTransf;
                obj.transform.localPosition = V3;
                obj.transform.localScale = Vector3.one;
                obj.SetActive(true);
                V3 = new Vector3(V3.x, V3.y - 58, V3.z);
                WdfActiveTypeItem ui = obj.GetComponent<WdfActiveTypeItem>();
                if (ui != null)
                {
                    if (i == 0 && GameCenter.wdfActiveMng.CurWdfActiveType == 0)
                    {
                        GameCenter.wdfActiveMng.CurWdfActiveType = GameCenter.wdfActiveMng.WdfActiveTypeDic[i].type;
                    }
                    WdfActiveData WdfActiveData = GameCenter.wdfActiveMng.WdfActiveTypeDic[i];
                    if (WdfActiveData != null)
                    {
                        ui.Refresh(WdfActiveData.name, WdfActiveData.type, (int)WdfActiveData.id);
                        typeList.Add(ui);
                    }
                }
                if(id==0)
                id = (int)GameCenter.wdfActiveMng.WdfActiveTypeDic[i].id;
            //}
        }
        if (id != 0 && isFirstActiveTime)
        {
            GameCenter.wdfActiveMng.C2S_AskActivitysInfoByID(id);
            isFirstActiveTime = false;
        }

        RefreshRed();


    }

    void RefreshActiveItem()
    {
        if (GameCenter.wdfActiveMng.CurWdfActiveItemInfo == null)
            return;
        curWdfActiveItemInfo=GameCenter.wdfActiveMng.CurWdfActiveItemInfo;
        //if (activeTwoItem != null) activeTwoItem.gameObject.SetActive(false);
        //if (activeOneItem != null) activeOneItem.gameObject.SetActive(false);
        //if (activeThreeItem != null) activeThreeItem.gameObject.SetActive(false);
        //if (activeFourItem != null) activeFourItem.gameObject.SetActive(false);
        int index = 0;
        if (curWdfActiveItemInfo.type == 4)
        {
            index = 1;
            //if (activeTwoItem != null)activeTwoItem.Refresh(curWdfActiveItemInfo); 
        }
        else if (curWdfActiveItemInfo.type == 10)//登陆红利
        {
            index = 2;
            //if (activeThreeItem != null) activeThreeItem.Refresh(curWdfActiveItemInfo);
        }
        else if(curWdfActiveItemInfo.type == 11)//连充豪礼
        {
            index = 3;
            //if (activeFourItem != null) activeFourItem.Refresh(curWdfActiveItemInfo);
        }
        else
        {
            index = 0;
            //if (activeOneItem != null)activeOneItem.Refresh(curWdfActiveItemInfo);
        }
        if (subWndArray != null && subWndArray.Length > index)
        {
            SwitchToSubWnd(subWndArray[index].type);
        }
    }



    void RefreshRed()
    {
        for (int i = 0; i < typeList.Count; i++)
        {
            if (GameCenter.wdfActiveMng.RedDic.ContainsKey(typeList[i].type))
                typeList[i].SetRed(GameCenter.wdfActiveMng.RedDic[typeList[i].type]);
        }
    }

















    void CloseThis(GameObject obj)
    {
        GameCenter.uIMng.SwitchToUI(GUIType.NONE);
    }

    

}
