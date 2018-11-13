//=========================================
//作者：黄洪兴
//日期：2016/05/10
//用途：武道会排名的容器
//=========================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 武道会排名的容器
/// </summary>
public class BudokaiRankContainer : MonoBehaviour
{


    /// <summary>
    /// 拍卖的物品
    /// </summary>
    public List<BudokaiRankInfo> budokaiRankList = new List<BudokaiRankInfo>();
    /// <summary>
    /// 静态预制引用，用来实例化一个对象
    /// </summary>
    protected static Object budokaiRankprefab = null;

    public Dictionary<int, BudokaiRankUI> BudokaiRankContainers = new Dictionary<int, BudokaiRankUI>();
    // Use this for initialization
    void Start()
    {
    }

    /// <summary>
    /// 刷新数据
    /// </summary>
    /// <param name="_skList"></param>
    public void RefreshItems(List<BudokaiRankInfo> _skList)
    {
        budokaiRankList.Clear();
        BudokaiRankContainers.Clear();
        if (_skList != null)
        {
            for (int i = 0; i < _skList.Count; i++)
            {
                budokaiRankList.Add(_skList[i]);
            }
        }
        RefreshItems();
    }
    public void RefreshItems(Dictionary<int, BudokaiRankInfo> _Dic)
    {
        budokaiRankList.Clear();
        BudokaiRankContainers.Clear();
        if (_Dic != null)
        {
            using (var e = _Dic.GetEnumerator())
            {
                while (e.MoveNext())
                {
                    budokaiRankList.Add(e.Current.Value);
                }
            }
        }
        RefreshItems();
    }
    /// <summary>
    /// 刷新表现
    /// </summary>
    public void RefreshItems()
    {
        budokaiRankprefab = null;
        if (budokaiRankprefab == null)
        {
            budokaiRankprefab = exResources.GetResource(ResourceType.GUI, "MartialArts/MartialArtsList");
        }
        if (budokaiRankprefab == null)
        {
            GameSys.LogError("找不到预制：MartialArts/MartialArtsList");
            return;
        }
        Vector3 V3 = Vector3.zero;
        for (int i = 0; i < budokaiRankList.Count; i++)
        {
            if (!BudokaiRankContainers.ContainsKey(i))
            {
                GameObject obj = Instantiate(budokaiRankprefab) as GameObject;
                Transform parentTransf = this.gameObject.transform;
                obj.transform.parent = parentTransf;
                obj.transform.localPosition = V3;
                obj.transform.localScale = Vector3.one;
                V3 = new Vector3(V3.x, V3.y - 28, V3.z);
                BudokaiRankUI marketItemUI = obj.GetComponent<BudokaiRankUI>();
                marketItemUI.FillInfo(budokaiRankList[i]);
                BudokaiRankContainers[i] = marketItemUI;

            }
        }
        budokaiRankprefab = null;
    }
}
