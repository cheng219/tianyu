//=========================================
//作者：黄洪兴
//日期：2016/04/12
//用途：仙盟篝火UI的容器
//=========================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 市场拍卖物品的容器
/// </summary>
public class GuildBonfireItemContainer : MonoBehaviour
{




    /// <summary>
    /// 其他仙盟篝火信息列表
    /// </summary>
    public List<st.net.NetBase.other_bonfire_list> guildBonfireItemList = new List<st.net.NetBase.other_bonfire_list>();

    public Dictionary<int, GuildBonfireItemUI> GuildBonfireItemContainers = new Dictionary<int, GuildBonfireItemUI>();
    // Use this for initialization
    void Start()
    {
    }

    /// <summary>
    /// 刷新数据
    /// </summary>
    /// <param name="_skList"></param>
    public void RefreshItems(List<st.net.NetBase.other_bonfire_list> _skList)
    {
        guildBonfireItemList.Clear();
        GuildBonfireItemContainers.Clear();
        if (_skList != null)
        {
            for (int i = 0; i < _skList.Count; i++)
            {
                guildBonfireItemList.Add(_skList[i]);
            }
        }
        RefreshItems();
    }
    public void RefreshItems(Dictionary<int, st.net.NetBase.other_bonfire_list> _Dic)
    {
        guildBonfireItemList.Clear();
        GuildBonfireItemContainers.Clear();
        if (_Dic != null)
        {
            using (var e = _Dic.GetEnumerator())
            {
                while (e.MoveNext())
                {
                    guildBonfireItemList.Add(e.Current.Value);
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
		Object guildBonfireItemprefab = null;
        if (guildBonfireItemprefab == null)
        {
            guildBonfireItemprefab = exResources.GetResource(ResourceType.GUI, "GuildActivity/GuildBonfireListItem");
        }
        if (guildBonfireItemprefab == null)
        {
            GameSys.LogError("找不到预制：GuildActivity/GuildBonfireListItem");
            return;
        }
        Vector3 V3 = Vector3.zero;
        for (int i = 0; i < guildBonfireItemList.Count; i++)
        {
            if (!GuildBonfireItemContainers.ContainsKey(i))
            {
                GameObject obj = Instantiate(guildBonfireItemprefab) as GameObject;
                Transform parentTransf = this.gameObject.transform;
                obj.transform.parent = parentTransf;
                obj.transform.localPosition = V3;
                obj.transform.localScale = Vector3.one;
                V3 = new Vector3(V3.x, V3.y - 57, V3.z);
                GuildBonfireItemUI marketItemUI = obj.GetComponent<GuildBonfireItemUI>();
                marketItemUI.FillInfo(guildBonfireItemList[i]);
                GuildBonfireItemContainers[i] = marketItemUI;

            }
        }
        guildBonfireItemprefab = null;
    }

}
