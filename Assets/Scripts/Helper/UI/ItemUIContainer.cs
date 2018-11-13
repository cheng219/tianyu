//=========================================
//作者：吴江
//日期：2015/7/1
//用途：物品UI的容器
//=========================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 物品UI的容器 by吴江
/// </summary>
public class ItemUIContainer : MonoBehaviour {
    /// <summary>
    /// 完成创建事件  by邓成
    /// </summary>
    public System.Action OnFinish;

	[HideInInspector]public ItemShowUIType showUIType = ItemShowUIType.NONE;
	public ItemUI itemPrefab;
    /// <summary>
    /// 背包所有格子字典
    /// </summary>
    public Dictionary<int, ItemUI> girdDic = new Dictionary<int, ItemUI>();
    /// <summary>
    /// 当前选中的格子
    /// </summary>
    protected ItemUI selectItemUI = null;
    /// <summary>
    /// 当前选中的格子
    /// </summary>
    public ItemUI SelectItemUI
    {
        get { return selectItemUI; }
    }
    /// <summary>
    /// 选择格子的事件
    /// </summary>
    public System.Action<ItemUI> OnSelectItemEvent = null;
    /// <summary>
    /// 当前的物品显示筛选类型
    /// </summary>
    protected FilterType curFilterType = FilterType.ALL;
    /// <summary>
    /// 上一次胡的物品显示筛选类型
    /// </summary>
    protected FilterType lastFilterType = FilterType.ALL;
    /// <summary>
    /// 当前的物品显示筛选类型
    /// </summary>
    public FilterType CurFilterType
    {
        get { return curFilterType; }
        set {
            if (curFilterType != value)
            {
                curFilterType = value;
                FliterItems(curFilterType);
            }
        }
    }
	public void FliterItemsForMini()
	{
		FliterItems(curFilterType);
	}
    /// <summary>
    /// grid管理组件
    /// </summary>
    protected UIExGrid gridMgr;
    /// <summary>
    /// grid管理组件
    /// </summary>
    protected UIExGrid GridMgr
    {
        get
        {
            if (gridMgr == null)
            {
                InitGridMng();
            }
            return gridMgr;
        }
    }
	/// <summary>
	/// ItemUI显示模型 by邓成
	/// </summary>
	public bool showModel = false;

    /// <summary>
    /// 格子数量
    /// </summary>
    protected int maxGridCount = 0;//默认为0,否则会先创建几个格子
    /// <summary>
    /// 格子数量
    /// </summary>
    public int MaxGridCount
    {
        set
        {
            maxGridCount = value;
        }
        get { return maxGridCount; }
    }
    /// <summary>
    /// 格子数量是否正在修正中 by吴江
    /// </summary>
    protected bool gridCountDirty = true;
    /// <summary>
    /// 是否处于回收模式下
    /// </summary>
    private bool isRecycle = false;
    public bool IsRecycle
    {
        get { return isRecycle; }
    }
    /// <summary>
    /// 行排序还是列排序
    /// </summary>
    public UIExGrid.Arrangement Arrangement
    {
        set
        {
            GridMgr.arrangement = value;
        }
    }

    /// <summary>
    /// 行宽
    /// </summary>
    public float CellWidth
    {
        set
        {
            GridMgr.cellWidth = value;
        }
    }
    /// <summary>
    /// 是否展示物品详细信息热感框
    /// </summary>
    protected bool showToolTip = true;
    /// <summary>
    /// 是否展示物品详细信息热感框
    /// </summary>
    public bool ShowToolTip
    {
        get
        {
            return showToolTip;
        }
        set
        {
            showToolTip = value;
        }
    }
    /// <summary>
    /// 当前试图异步选中的物品序列号
    /// </summary>
    protected int curAsyncSelectIndex = -1;
    /// <summary>
    /// 物品堆叠数量为0时,是否显示不可用状态
    /// </summary>
    public bool showZeroCount = false;
    /// <summary>
    /// 行高
    /// </summary>
    public float CellHeight
    {
        set
        {
            GridMgr.cellHeight = value;
        }
    }

    /// <summary>
    /// 是否隐藏空格子
    /// </summary>
    public bool isHideNullSlot = false;
	/// <summary>
	/// 动态创建的装备是否显示LightFlag by邓成
	/// </summary>
	public bool showLightFlag;

    /// <summary>
    /// 要展示的物品数据缓存  by吴江
    /// </summary>
    protected List<EquipmentInfo> eqList = new List<EquipmentInfo>();

    /// <summary>
    /// 按钮功能类型
    /// </summary>
    protected ItemActionType leftAction = ItemActionType.None;
    protected ItemActionType middleAction = ItemActionType.None;
    protected ItemActionType rightAction = ItemActionType.None;
    protected void InitGridMng()
    {
		if(this == null)return;
        gridMgr = this.gameObject.GetComponent<UIExGrid>();
        if (gridMgr == null) gridMgr = this.gameObject.AddComponent<UIExGrid>();
        if (gridMgr != null)
        {
            for (int i = gridMgr.transform.childCount - 1; i >= 0; i--)
            {
                NGUITools.Destroy(gridMgr.transform.GetChild(i).gameObject);
            }
        }
        girdDic.Clear();
    }


    #region 创建格子
    /// <summary>
    /// 错峰创建所有需要的格子  by吴江
    /// </summary>
    void Update()
    {
        if (Time.frameCount % 2 == 0 ) //这个方法不宜太过频繁执行 by吴江
        {
            if (girdDic.Count < maxGridCount)
            {
                gridCountDirty = true;
                RefreshGrid(5);
                RefreshItems();
            }
            else if (maxGridCount > girdDic.Count)
            {
                gridCountDirty = true;
                RefreshGrid(-5);
                RefreshItems();
            }
            else if (gridCountDirty)
            {
                gridCountDirty = false;
                RefreshItems();
                if (OnFinish != null) OnFinish();
            }
        }
    }

    /// <summary>
    /// 创建指定个数的格子 by吴江
    /// </summary>
    /// <param name="_addNumPerFrame"></param>
    protected void RefreshGrid(int _addNumPerFrame)
    {
        int index = girdDic.Count;
        if (_addNumPerFrame > 0) //如果是增加格子  by吴江
        {
            int diff = maxGridCount - index;
            _addNumPerFrame = Mathf.Min(_addNumPerFrame, diff);
            if (_addNumPerFrame > 0)
            {
                for (int i = 0; i < _addNumPerFrame; i++)
                {
                    AddOneGrid(index);
                    index++;
                }
            }
        }
        else//如果是减少格子  by吴江
        {
            int diff = index - maxGridCount;
            int deleteCount = Mathf.Min(Mathf.Abs(_addNumPerFrame), diff);
            if (deleteCount > 0)
            {
                for (int i = 0; i < deleteCount; i++)
                {
                    HideOneGrid(index);
                    index--;
                }
            }
        }
    }

    /// <summary>
    /// 是否挂靠外框
    /// </summary>
    public bool triggerGridFrame ;
    
    /// <summary>
    /// 创建一个格子  by吴江
    /// </summary>
    /// <param name="_index"></param>
    void AddOneGrid(int _index)
    {
		if (girdDic.ContainsKey(_index))
        {
			girdDic[_index].FillInfo(null);
			girdDic[_index].gameObject.SetActive(true);
            return;
        }
		ItemUI go = null;
		if(itemPrefab != null)
		{
			go = ItemUI.CreatNewByPrefab(GridMgr, _index,itemPrefab.gameObject);
		}else
		{
			go = ItemUI.CreatNew(GridMgr, _index, triggerGridFrame ? GridMgr.gameObject : null);
		}
        if (go != null)
        {
            go.ShowTooltip = ShowToolTip;
            go.showZeroCount = showZeroCount;
			go.showUIType = showUIType;
            go.SetActionBtn(leftAction, middleAction, rightAction);
            go.FillInfo(null);
            go.OnSelectEvent += OnSelectItemUI;
			girdDic[_index] = go;
        }

    }


    /// <summary>
    /// 删除一个格子  by吴江
    /// </summary>
    /// <param name="_index"></param>
    void HideOneGrid(int _index)
    {
		if (!girdDic.ContainsKey(_index))
        {
            GameSys.LogError("试图隐藏的格子位置已经不存在！");
            return;
        }
		ItemUI go = girdDic[_index];
        if (go != null)
        {
            go.gameObject.SetActive(false);
            go.FillInfo(null);
        }

    }
    #endregion


    void OnItemPosChange(EquipmentInfo _eq, int _oldPos)
    {
        ItemUI tarPosGameObject;
        if (_oldPos >= 0 && girdDic.TryGetValue(_oldPos, out tarPosGameObject))
        {
            tarPosGameObject.FillInfo(null);
        }
        if (girdDic.TryGetValue(_eq.Postion, out tarPosGameObject))
        {
            tarPosGameObject.FillInfo(_eq);
        }
    }
	/// <summary>
	/// 更新对应下标的格子里的数据
	/// </summary>
	public void UpdateItemByIndex(int index,EquipmentInfo _eq)
	{
		if(girdDic.ContainsKey(index))
		{
			girdDic[index].FillInfo(_eq);
		}
	}
	/// <summary>
	/// 更新对应下标的格子里的批量显示数据
	/// </summary>
	public void UpdateItemBatch(int index,EquipmentInfo _eq,bool b)
	{
		if(girdDic.ContainsKey(index)&&!b)
		{
			girdDic[index].showBatch=false;
			girdDic [index].batchObj.GetComponent<UIToggle> ().enabled = false;
			girdDic [index].batchObj.GetComponent<UIToggle> ().value = false;
			GameCenter.buyMng.SellEqList.Clear ();
			if(GameCenter.buyMng.OnSellEqChange!=null)
			GameCenter.buyMng.OnSellEqChange ();
		}
		if(girdDic.ContainsKey(index)&&b)
		{
			girdDic[index].showBatch=true;
			girdDic [index].batchObj.GetComponent<UIToggle> ().enabled = true;
			girdDic [index].batchObj.GetComponent<UIToggle> ().value = false;
		}
	}
    public void UpdateItemForBatchSell(bool _batchSell)
    {
        foreach (var item in girdDic.Values)
        {
            if (item != null)
            {
                if (_batchSell)
                {
                    item.showBatch = true;
                    item.batchObj.GetComponent<UIToggle>().enabled = true;
                    item.batchObj.GetComponent<UIToggle>().value = false;
                }
                else
                { 
                    item.showBatch=false;
			        item.batchObj.GetComponent<UIToggle> ().enabled = false;
			        item.batchObj.GetComponent<UIToggle> ().value = false;
			        GameCenter.buyMng.SellEqList.Clear ();
                    if (GameCenter.buyMng.OnSellEqChange != null)
                        GameCenter.buyMng.OnSellEqChange();
                }
            }
        }
    }

    void OnSelectItemUI(ItemUI _itemUI)
    {
        selectItemUI = _itemUI;
        curAsyncSelectIndex = -1;
        if (OnSelectItemEvent != null)
        {
            OnSelectItemEvent(selectItemUI);
        }
    }

    /// <summary>
    /// 选取指定位置的物品,如果参数为负数，则取消之前的选中
    /// </summary>
    /// <param name="_index"></param>
    public void SelectItem(int _index)
    {

    }
    /// <summary>
    /// 设置容器内物品功能按钮行为类型
    /// </summary>
    /// <param name="_left"></param>
    /// <param name="_middle"></param>
    /// <param name="_right"></param>
    public void SetActionBtn(ItemActionType _left, ItemActionType _middle, ItemActionType _right)
    {
        leftAction = _left;
        middleAction = _middle;
        rightAction = _right;
        if (girdDic.Count > 0)
        {
			using(var e = girdDic.GetEnumerator())
			{
				while(e.MoveNext())
				{
					ItemUI item = e.Current.Value;
					item.SetActionBtn(_left, _middle, _right);
				}
			}
        }
    }
    public void SetActionBtn(ItemActionType _left, ItemActionType _middle, ItemActionType _right,ItemActionType _access)
    {
        leftAction = _left;
        middleAction = _middle;
        rightAction = _right;
        if (girdDic.Count > 0)
        {
			using(var e = girdDic.GetEnumerator())
			{
				while(e.MoveNext())
				{
					ItemUI item = e.Current.Value;
					item.SetActionBtn(_left, _middle, _right);
				}
			}
        }
    }

    #region 表现类型切换 装备，普通，材料 等等
    /// <summary>
    /// 查看类型
    /// </summary>
    public enum FilterType
    {
        ALL,//所有
        Equip,//装备
        Consumables,//消耗品
        Others//其他
    }

    /// <summary>
    /// 当前容器内只展示哪一类物品列表  by吴江
    /// </summary>
    /// <param name="_type"></param>
    protected void FliterItems(FilterType _type)
    {
        switch (_type)
        {
            case FilterType.ALL:
                GridMgr.gridFilter = FilterAll;
                break;
            case FilterType.Equip:
                GridMgr.gridFilter = FilterEquip;
                break;
            case FilterType.Consumables:
                GridMgr.gridFilter = FilterConsumables;
                break;
            case FilterType.Others:
                GridMgr.gridFilter = FilterOthers;
                break;
            default:
                break;
        }
        GridMgr.Reposition();
        if (lastFilterType != _type)
        {
            lastFilterType = _type;
            ToTop();
        }
    }

    void ToTop()
    {
        //if(transform.parent == null) return;
        //UIDraggablePanel udp = transform.parent.gameObject.GetComponent<UIDraggablePanel>();
        //if (udp == null) return;
        //udp.ResetPosition();
        //udp.SetDragAmount(0f, 0f, false);
    }


    bool FilterAll(GameObject _go)
    {
        EquipmentInfo eq = ItemUI.GetEqInfo(_go);
        if (eq != null || !isHideNullSlot) return true;
        return false;
    }

	
    bool FilterEquip(GameObject _go)
    {
        EquipmentInfo eq = ItemUI.GetEqInfo(_go);
        if (eq != null && (eq.Family == EquipmentFamily.WEAPON || eq.Family == EquipmentFamily.ARMOR || eq.Family == EquipmentFamily.JEWELRY || eq.Family == EquipmentFamily.COSMETIC)) return true;
        return false;
    }

    bool FilterConsumables(GameObject _go)
    {
        EquipmentInfo eq = ItemUI.GetEqInfo(_go);
        if (eq != null && (eq.Family == EquipmentFamily.PET || eq.Family == EquipmentFamily.POTION || eq.Family == EquipmentFamily.CONSUMABLES || eq.Family == EquipmentFamily.MOUNT || eq.Family == EquipmentFamily.RENAME)) return true;
        return false;
    }

    bool FilterOthers(GameObject _go)
    {
        EquipmentInfo eq = ItemUI.GetEqInfo(_go);
        if (eq != null && (eq.Family == EquipmentFamily.MATERIAL || eq.Family == EquipmentFamily.TASK || eq.Family == EquipmentFamily.GEM)) return true;
        return false;
    }

    bool FilterMat(GameObject _go)
    {
        EquipmentInfo eq = ItemUI.GetEqInfo(_go);
        if (eq != null && eq.Family == EquipmentFamily.MATERIAL) return true;
        return false;
    }
    #endregion

    #region 刷新物品列表
    protected void RefreshItems()
    {
        ///根据数据层的数据刷新UI
        if (this == null) return;
        List<int> refreshList = new List<int>();
        int index = 0;
        //eqList是当前的数据集引用
        //遍历该数据集
		for(int i=0,max=eqList.Count;i<max;i++)
        {
			EquipmentInfo eq = eqList[i];
            //如果数据集中有空数据，那么根据策划需求，不展示该空位，直接跳过
            if (eq == null)
            {
                index++;
                continue;
            }
            ItemUI go = null;
            refreshList.Add(index);
            //如果该物品的数据中的从属属性为预览，比如说在某些奖励界面，那么可能需要展示高光效果提醒玩家。做区别处理。 如果从属属性是已经属于玩家，
            //比如是从属背包，身上，仓库，那么不需要做该处理。 这个应该是邓成加的，我个人不建议在上层管理界面做这样的事情，应该交由ItemUI组件对象内部封装处理，上层
            //只调用ItemUI的统一接口。但是可能当时他这么做有一些机制上的难处，无伤大雅。修改需要谨慎
            if (eq.BelongTo == EquipmentBelongTo.PREVIEW)
            {
                //刷新列表存入该刷新记录，这种新建列表的方式一般是出于本次执行本身就是在一个列表的for循环内部，在列表的for循环内部是禁止对列表本身进行操作的。所以有
                //时候需要新建一个列表来记录一些事情，当for循环结束以后，再用这个记录去操作原列表
                //根据序列index尝试去界面容器中获取表现层UI对象
                if (girdDic.TryGetValue(index, out go))
                {
                    //如果获取成功，则将数据层对象 填入 表现层UI对象 ， 表现层UI对象 在获取数据层对象后，自行刷新
                    go.FillInfo(eq);
					if(showLightFlag)
						go.ShowLightFlag();
                    //表现层对象激活
                    go.gameObject.SetActive(true);
                }
            }else
            {
				if (girdDic.TryGetValue(index, out go))
                {
                    go.FillInfo(eq);
                    go.gameObject.SetActive(true);
                }
            }
            index++;
        }
        //便利界面容器中的表现层对象列表
		using(var e = girdDic.GetEnumerator())
        {
			while(e.MoveNext())
			{
				int item = e.Current.Key;
				girdDic[item].Forbidden(false);
				//如果发现之前数据刷新过程中，不包含针对该表现层ui对象的处理，那么说明本次刷新，没这个表现层ui什么事，那么他应该表现为一个空格子，或者隐藏掉， 
				if (!refreshList.Contains(item))
				{
					//所以需要给他填入一个空数据，让他自己去处理对应的表现
					girdDic[item].FillInfo(null);
					//根据本容器的属性 即 isHideNullSlot  或者是否为分类刷新（根据龙之契约的策划要求，分类刷新是一概不表现空格子的），来决定，这里是表现一个空格子，还是隐藏
					if (isHideNullSlot || curFilterType != FilterType.ALL)
					{
						girdDic[item].gameObject.SetActive(false);
					}
					else
					{
						girdDic[item].gameObject.SetActive(true);
					}
				}
				if (isRecycle && girdDic[item].EQInfo!=null)
				{
					girdDic[item].ShowTooltip = false;
					girdDic[item].GetComponent<UIToggle>().group = 0;
					if (girdDic[item].EQInfo.RecycleType == GoodsRecycleType.NO)
					{
						girdDic[item].Forbidden(true);
					}
				}
			}
        }
        //根据当前的刷新类型来进行排序和隐藏
        FliterItems(curFilterType);
        //        }
        //如果之前有选中物品的异步任务，那么在刷新结束之后，完成该任务，选中某个格子
        if (curAsyncSelectIndex >= 0)
        {
            SelectItem(curAsyncSelectIndex);
        }
    }

    /////////////////////////////////////////////List<ItemValue> 部分

    /// <summary>
    /// 在容器内刷新物品列表表现 by吴江
    /// </summary>
    /// <param name="_eqList"></param>
    public void RefreshItems(List<ItemValue> _eqList)
    {
        eqList.Clear();
        if (_eqList != null)
        {
			for(int i=0,max=_eqList.Count;i<max;i++)
            {
				ItemValue item = _eqList[i];
                eqList.Add(new EquipmentInfo(item.eid,item.count, EquipmentBelongTo.PREVIEW));
            }
        }
        RefreshItems();
    }	
	

    /// <summary>
    /// 在容器内刷新物品列表表现 by吴江
    /// </summary>
    /// <param name="_eqList"></param>
    public void RefreshItems(List<ItemValue> _eqList, int _maxPerLine, int _gridCount)
    {
        GridMgr.maxPerLine = _maxPerLine;
        maxGridCount = _gridCount;
        RefreshItems(_eqList);
    }


    /// <summary>
    /// 在容器内刷新物品列表表现 by吴江
    /// </summary>
    /// <param name="_eqList"></param>
    public void RefreshItems(Dictionary<int, EquipmentInfo> _eqList, int _maxPerLine, int _gridCount)
    {
        GridMgr.maxPerLine = _maxPerLine;
        maxGridCount = _gridCount;
        RefreshItems(_eqList);
    }
    /// <summary>
    /// 在容器内刷新物品列表表现 by邓成
    /// </summary>
    /// <param name="_eqList"></param>
    public void RefreshItems(List<EquipmentInfo> _eqList, int _maxPerLine, int _gridCount)
    {
        GridMgr.maxPerLine = _maxPerLine;
        maxGridCount = _gridCount;
        RefreshItems(_eqList);
    }


    /////////////////////////////////////////////Dictionary<int, EquipmentInfo> 部分

    /// <summary>
    /// 在容器内刷新物品列表表现 by吴江
    /// </summary>
    /// <param name="_eqList"></param>
    public void RefreshItems(Dictionary<int, EquipmentInfo> _eqList)
    {
        eqList.Clear();
        if (_eqList != null)
        {
			using(var e = _eqList.GetEnumerator())
			{
				while(e.MoveNext())
				{
					EquipmentInfo item = e.Current.Value;
					if (item.Postion < 10000)
					{
						eqList.Add(item);
					}
				}
			}
        }
        RefreshItems();
    }
    /// <summary>
    /// 在容器内刷新物品列表表现 by邓成
    /// </summary>
    /// <param name="_eqList"></param>
    public void RefreshItems(List<EquipmentInfo> _eqList)
    {
        eqList.Clear();
        if (_eqList != null)
        {
			for(int i=0,max=_eqList.Count;i<max;i++)
            {
				EquipmentInfo item = _eqList[i];
                eqList.Add(item);
            }
        }
        RefreshItems();
    }

    /// <summary>
    /// 在容器内刷新物品列表表现 by吴江
    /// </summary>
    /// <param name="_eqList"></param>
    public void RefreshItems(Dictionary<int, EquipmentInfo> _eqList, FilterType _filterType)
    {
        curFilterType = _filterType;
        RefreshItems(_eqList);
    }
    #endregion
	
}
/// <summary>
/// 排序比较
/// </summary>
public class ItemComparer : IComparer<ItemUI>
{
    static ItemComparer instance;
    public static ItemComparer Instance
    {
        get
        {
            if (instance == null) instance = new ItemComparer();
            return instance;
        }
    }
    ItemComparer()
    {
    }
    public int Compare(ItemUI x, ItemUI y)
    {
        if (x.EQInfo == null)
            return -1;
        if (y.EQInfo == null)
            return 1;
        int ret = x.EQInfo.Quality.CompareTo(y.EQInfo.Quality);
        if (ret != 0) return ret;

        ret = x.EQInfo.Slot.CompareTo(y.EQInfo.Slot);
        if (ret != 0) return ret;

        ret = x.EQInfo.UseReqLevel.CompareTo(y.EQInfo.UseReqLevel);
        if (ret != 0) return ret;


        ret = x.EQInfo.InstanceID.CompareTo(y.EQInfo.InstanceID);

        return ret;
    }

}