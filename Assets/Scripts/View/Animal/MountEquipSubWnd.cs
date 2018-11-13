//==================================
//作者：邓成
//日期：2016/12/19
//用途：坐骑装备界面
//=================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MountEquipSubWnd : SubWnd {

    public UITexture modelTexture;

    public ItemUI[] itemUis;
    public UIButton btnSuit;
    public UILabel labSuitAttr;
    public UILabel[] labAttr;

    public UIButton btnOpenUpgrade;
    public UISprite upgradeRedTip;
    public UIButton btnCloseUpgrade;
    public MountEquipUpgradeWnd mountEquipUpgradeWnd;

    public UIButton btnSuitAttr;
    public GameObject noNextSuit;
    public UILabel curSuitAttr;
    public UILabel curSuitDes;

    public GameObject haveNextSuit;
    public UILabel curAllSuitAttr;//所有属性已激活
    public UILabel nextSuitAttr;
    public UILabel curAllSuitDes;
    public UILabel nextSuitDes;

    void Awake()
    {
        if (btnOpenUpgrade != null) UIEventListener.Get(btnOpenUpgrade.gameObject).onClick = OpenUpgradeWnd;
        if (btnCloseUpgrade != null) UIEventListener.Get(btnCloseUpgrade.gameObject).onClick = CloseUpgradeWnd;
        if (btnSuitAttr != null) UIEventListener.Get(btnSuitAttr.gameObject).onClick = ShowSuitWnd;
    }
    protected override void OnOpen()
    {
        base.OnOpen();
        RefreshItems();
        if (upgradeRedTip != null) upgradeRedTip.enabled = GameCenter.mainPlayerMng.GetFunctionIsRed(FunctionType.RIDINGSUIT);
    }
    protected override void OnClose()
    {
        base.OnClose();
        if (mountEquipUpgradeWnd != null) mountEquipUpgradeWnd.CloseUI();
    }

    protected override void HandEvent(bool _bind)
    {
        base.HandEvent(_bind);
        if (_bind)
        {
            GameCenter.newMountMng.OnMountEquipUpdate += RefreshItems;
        }
        else
        {
            GameCenter.newMountMng.OnMountEquipUpdate -= RefreshItems;
        }
    }

    void RefreshItems()
    {
        MountInfo mountInfo = GameCenter.newMountMng.curMountInfo;
        if (modelTexture != null && mountInfo != null) GameCenter.previewManager.TryPreviewSingelMount(mountInfo, modelTexture);
        List<AttributePair> attrList = new List<AttributePair>();
        if (itemUis != null)
        {
            List<EquipmentInfo> equipList = new List<EquipmentInfo>(GameCenter.newMountMng.MountEquipDic.Values);
            for (int i = 0, length = itemUis.Length; i < length; i++)
            {
                if (itemUis[i] != null) itemUis[i].FillInfo(null);
            }
            /////填充已经装备好的数据
            for (int i = 0, max = equipList.Count; i < max; i++)
            {
                EquipmentInfo item = equipList[i];
                attrList = CountAttributePair(attrList, item.StrengthValue);
                if (itemUis.Length > (int)item.Slot - (int)EquipSlot.Headband && itemUis[(int)item.Slot - (int)EquipSlot.Headband] != null)
                {
                    itemUis[(int)item.Slot - (int)EquipSlot.Headband].FillInfo(item);
                }
            }
        }
        if (labAttr != null)
        {
            for (int i = 0,length=labAttr.Length; i < length; i++)
            {
                int val = 0;
                switch(i)
                {
                    case 0:
                        val = GetAttributeValue(ActorPropertyTag.HPLIMIT, attrList);
                        break;
                    case 1:
                        val = GetAttributeValue(ActorPropertyTag.ATK, attrList);
                        break;
                    case 2:
                        val = GetAttributeValue(ActorPropertyTag.DEF, attrList);
                        break;
                    case 3:
                        val = GetAttributeValue(ActorPropertyTag.HIT, attrList);
                        break;
                    case 4:
                        val = GetAttributeValue(ActorPropertyTag.DOD, attrList);
                        break;
                    case 5:
                        val = GetAttributeValue(ActorPropertyTag.CRI, attrList);
                        break;
                    case 6:
                        val = GetAttributeValue(ActorPropertyTag.TOUGH, attrList);
                        break;
                }
                labAttr[i].text = val.ToString();
            }
        }
    }

    protected int GetAttributeValue(ActorPropertyTag _tag, List<AttributePair> _attrList)
    {
        for (int i = 0,length=_attrList.Count; i < length; i++)
        {
            if (_attrList[i].tag == _tag)
            {
                return _attrList[i].value;
            }
        }
        return 0;
    }

    List<AttributePair> CountAttributePair(List<AttributePair> _attrList,List<AttributePair> _addList)
    {
        List<AttributePair> attrs = _attrList;
        for (int i = 0, length = _addList.Count; i < length; i++)
        {
            AttributePair attI = _addList[i];
            bool contain = false;
            for (int j = 0, lenJ = attrs.Count; j < lenJ; j++)
            {
                AttributePair attJ = attrs[j];
                if (attI.tag == attJ.tag)
                {
                    attJ.Update(attI.tag, attI.value + attJ.value);
                    contain = true;
                }
            }
            if (!contain) attrs.Add(attI);
        }
        //Debug.Log("Count:"+attrs.Count);
        return attrs;
    }

    void OpenUpgradeWnd(GameObject go)
    {
        List<EquipmentInfo> equipList = new List<EquipmentInfo>(GameCenter.newMountMng.MountEquipDic.Values);
        if (equipList.Count < 1)
        {
            GameCenter.messageMng.AddClientMsg(487);
            return;
        }
        if (mountEquipUpgradeWnd != null) mountEquipUpgradeWnd.OpenUI();
    }

    void CloseUpgradeWnd(GameObject go)
    {
        if (mountEquipUpgradeWnd != null) mountEquipUpgradeWnd.CloseUI();
        if (upgradeRedTip != null) upgradeRedTip.enabled = GameCenter.mainPlayerMng.GetFunctionIsRed(FunctionType.RIDINGSUIT);
    }

    void ShowSuitWnd(GameObject go)
    {
        int curQuality = 5;
        int curQualityCount = 0;
        int nextQualityCount = 0;
        bool haveSuitAttr = false;
        List<EquipmentInfo> equipList = new List<EquipmentInfo>(GameCenter.newMountMng.MountEquipDic.Values);
        if (equipList.Count < 8)
        {
            curQuality = 1;
            for (int i = 0, length = equipList.Count; i < length; i++)
            {
                if (equipList[i].Quality >= curQuality)
                {
                    curQualityCount++;
                }
            }
        }
        else
        {
            haveSuitAttr = true;
            for (int i = 0,length=equipList.Count; i < length; i++)
            {
                curQuality = Mathf.Min(curQuality, equipList[i].Quality);//curQuality默认为5
            }
            for (int i = 0, length = equipList.Count; i < length; i++)
            {
                if (equipList[i].Quality > curQuality)
                {
                    nextQualityCount++;
                }
                if (equipList[i].Quality >= curQuality)
                {
                    curQualityCount++;
                }
            }
        }
        bool showNextSuit = (haveSuitAttr && curQuality < 5);
        if (noNextSuit != null && haveNextSuit != null)
        {
            noNextSuit.SetActive(!showNextSuit);
            haveNextSuit.SetActive(showNextSuit);
        }
        MountSuitRef mountSuit = ConfigMng.Instance.GetMountSuitRef(curQuality);//全身橙色不显示下一阶段套装
        if(mountSuit != null)
        {
            string attrString = (haveSuitAttr?"[00ff00]":"[ffffff]") + GameHelper.GetAttributeNameAndValue(mountSuit.attrs)+"[-]";
            if (curSuitAttr != null) curSuitAttr.text = attrString;
            if (curAllSuitAttr != null) curAllSuitAttr.text = attrString;
            if (curSuitDes != null) curSuitDes.text = (haveSuitAttr ? mountSuit.curDes : mountSuit.nextDes) + "(" + curQualityCount + "/8)";
            //Debug.Log("curQualityCount:" + curQualityCount);
        }
        if (showNextSuit)
        {
            MountSuitRef nextMountSuit = ConfigMng.Instance.GetMountSuitRef(curQuality+1);
            if (nextSuitAttr != null && nextMountSuit != null) nextSuitAttr.text = GameHelper.GetAttributeNameAndValue(nextMountSuit.attrs);
            if (curAllSuitDes != null) curAllSuitDes.text = mountSuit.curDes + "(8/8)";
            if (nextSuitDes != null) nextSuitDes.text = nextMountSuit.nextDes + "([ff0000]" + nextQualityCount + "[-]/8)";
        }
    }
}
