//===================
//作者：鲁家旗
//日期：2016/4/15
//用途：藏宝库预览界面
//===================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PreviewUI : SubWnd
{
    public GameObject titleGo;
    public GameObject memberGo;
    public GameObject itemGo;
    public GameObject spGo;
    public UIScrollView scrollView;
    protected FDictionary rewardDic = null;
    protected FDictionary rewardMemberDic = null;
    protected List<GameObject> nameGoList = new List<GameObject>();
    protected List<GameObject> itemGoList = new List<GameObject>();
    void Awake()
    {
        rewardDic = ConfigMng.Instance.GetRewardGroupRefTable();
        rewardMemberDic = ConfigMng.Instance.GetRewardGroupMemberRefTable();
    }
    protected override void OnOpen()
    {
        base.OnOpen();
        CreateNew();
        if (scrollView != null) scrollView.ResetPosition();
    }
    protected override void OnClose()
    {
        base.OnClose();
    }
    void HideNameGo()
    {
        for (int i = 0; i < nameGoList.Count; i++)
        {
            nameGoList[i].SetActive(false);
        }
        for (int i = 0; i < itemGoList.Count; i++)
        {
            itemGoList[i].SetActive(false);
        }
    }
    void SetItemTrue()
    {
        for (int i = 0; i < itemGoList.Count; i++)
        {
            itemGoList[i].SetActive(true);
        }
    }
    int GetAllItemNum()
    {
        int num = 0;
        foreach (RewardGroupRef data in rewardDic.Values)
        {
            for (int i = 0; i < data.memberId.Count; i++)
            {
                if (rewardMemberDic.ContainsKey(data.memberId[i]))
                {
                    RewardGroupMemberRef rewardGroupRef = rewardMemberDic[data.memberId[i]] as RewardGroupMemberRef;
                    if (rewardGroupRef != null)
                    {
                        for (int j = 0; j < rewardGroupRef.item.Count; j++)
                        {
                            num++;
                        }
                    }
                }
            }
        }
        return num;
    }
    void CreateNew()
    {
        int currentY = 140;
        int index = 0;
        HideNameGo();
        int allItem = GetAllItemNum();
        foreach (RewardGroupRef data in rewardDic.Values)
        {
            //过滤不对应职业的宝物
                //创建类型名
                if (index == 0) currentY = 140;
                else currentY = currentY - 30;
                if (nameGoList.Count < rewardDic.Count)
                {
                    GameObject nameGo = CreateGo(titleGo, 129, currentY, 0);
                    nameGo.GetComponent<Title>().SetTitle(data);
                    nameGoList.Add(nameGo);
                }
                nameGoList[index].SetActive(true);
                for (int i = 0; i < data.memberId.Count; i++)
                {
                    //创建等阶名
                    currentY = currentY - 60;
                    if (rewardMemberDic.ContainsKey(data.memberId[i]))
                    {
                        GameObject stageGo = CreateGo(memberGo, -56, currentY, 0);
                        RewardGroupMemberRef rewardGroupRef = rewardMemberDic[data.memberId[i]] as RewardGroupMemberRef;
                        if (rewardGroupRef != null)
                        {
                            stageGo.GetComponent<StageLable>().SetStageLable(rewardGroupRef);
                            currentY = currentY - 50;
                            for (int j = 0; j < rewardGroupRef.item.Count; j++)
                            {
                                if (j % 6 == 0 && j > 0) currentY = currentY - 90;
                                //创建背景
                                if (j == 0)//|| j % 12 == 0)
                                {
                                    spGo.GetComponent<UISprite>().height = (rewardGroupRef.item.Count % 6 == 0 ? rewardGroupRef.item.Count / 6 : rewardGroupRef.item.Count / 6 + 1) * 90;
                                    CreateGo(spGo, 37, currentY + 45, 0);
                                }
                                GameObject go = null;
                                //创建格子
                                if (itemGoList.Count < allItem)
                                {
                                    go = CreateGo(itemGo, 87 + 85 * (j % 6), currentY, 0);
                                    go.GetComponent<ItemUI>().FillInfo(new EquipmentInfo(rewardGroupRef.item[j], EquipmentBelongTo.PREVIEW));
                                    itemGoList.Add(go);
                                }
                                else
                                    SetItemTrue();
                            }
                        }
                    }
                }
                index++;
        }
        titleGo.SetActive(false);
        memberGo.SetActive(false);
        itemGo.SetActive(false);
        spGo.SetActive(false);
    }
    GameObject CreateGo(GameObject go, int x, int y,int z)
    {
        GameObject gameObject = GameObject.Instantiate(go);
        gameObject.transform.parent = go.transform.parent;
        gameObject.transform.localPosition = new Vector3(x, y, z);
        gameObject.transform.localScale = Vector3.one;
        return gameObject;
    }
}

    
