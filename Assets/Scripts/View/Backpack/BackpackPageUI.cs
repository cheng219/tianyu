//==============================================
//作者：邓成
//日期：2017/1/4
//用途：背包分页界面
//=================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BackpackPageUI : MonoBehaviour
{
    #region 控件
    /// <summary>
    /// 背包容器,每一页一个容器
    /// </summary>
    protected ItemUIContainer[] itemUIContainer = new ItemUIContainer[3];
    /// <summary>
    /// 容器的父节点
    /// </summary>
    public GameObject[] itemContainerParent;

    public Transform itemUIContainerAnchor;
    /// <summary>
    /// 翻页控制的进度条
    /// </summary>
    public UIProgressBar dragProgressBar;
    /// <summary>
    /// 翻页显示的进度条
    /// </summary>
    public UISlider progressBar;
    public UIScrollView scrollView;
    #endregion

    #region 可配置参数
    /// <summary>
    /// Item元素大小
    /// </summary>
    public int cellWidth = 85;
    /// <summary>
    /// 最大翻页数
    /// </summary>
    public int MaxPage = 6;
    /// <summary>
    /// 每页显示数量
    /// </summary>
    public int PerPageCount = 20;
    #endregion


    
    /// <summary>
    /// 当前页数
    /// </summary>
    protected float CurBagPage = 1;
    /// <summary>
    /// 拖动前的鼠标位置
    /// </summary>
    protected float oldMouseX = 0f;
    /// <summary>
    /// 拖动后的鼠标位置(松开鼠标的当前位置)
    /// </summary>
    protected float curMouseX = 0f;
    /// <summary>
    /// 拖动前进度条数字(此进度条只可能为0,0.5,1)
    /// </summary>
    protected float oldDragProgressValue = 0f;
    /// <summary>
    /// 是否可以拖动(初始三页全部创建出来后可以拖动)
    /// </summary>
    protected bool canDrag = false;

    /// <summary>
    /// 当前需要显示的物品集合
    /// </summary>
    protected List<EquipmentInfo> curItemList = new List<EquipmentInfo>();
    void Awake()
    {
        if (scrollView != null) scrollView.onDragFinished = OnDragFinish;
        if (scrollView != null) scrollView.onDragStarted = OnDragStart;
    }

    protected void OnDragStart()
    {
        oldMouseX = Input.mousePosition.x;
        if (dragProgressBar != null) oldDragProgressValue = dragProgressBar.value;
    }
    protected void OnDragFinish()
    {
        if (!canDrag) return;
        float wantValue = 0f;
        int add = 0;
        curMouseX = Input.mousePosition.x;
        if (curMouseX > oldMouseX)
        {
            add = -1;
            CurBagPage = (CurBagPage - 1f);
            if (CurBagPage == 0)
            {
                CurBagPage = 1;
                add = 0;
            }
        }
        else if (curMouseX < oldMouseX)
        {
            add = 1;
            CurBagPage = (CurBagPage + 1f);
            if (CurBagPage == MaxPage+1)
            {
                CurBagPage = MaxPage;
                add = 0;
            }
        }
        if (CurBagPage != 1 && CurBagPage != MaxPage)
        {
            RefreshItems(curItemList);
            wantValue = 0.5f;
        }
        else
        {
            wantValue = oldDragProgressValue + add * 0.5f;
        }
        dragProgressBar.value = wantValue;
        //Debug.Log("dragProgressBar.value:" + dragProgressBar.value + ",wantValue:" + wantValue);
        if (Mathf.Approximately(dragProgressBar.value, wantValue))//相等的时候也执行onChange
            EventDelegate.Execute(dragProgressBar.onChange);
        progressBar.value = (CurBagPage - 1f) * (1f/((float)MaxPage-1f));
        //如:MaxPage为6时,progressBar.value控制6个点,分别为0,0.2,0.4,0.6,0.8,1.0
    }

    void SetScrollViewDragEnable(bool _enable)
    {
        if (scrollView != null)
        {
            scrollView.movement = _enable ? UIScrollView.Movement.Horizontal : UIScrollView.Movement.Vertical;
            if (_enable)//设置为可水平拖动
            {
                scrollView.movement = UIScrollView.Movement.Horizontal;
            }
            else//设置为不可拖动
            {
                scrollView.movement = UIScrollView.Movement.Custom;
                scrollView.customMovement = Vector2.zero;
            }
            scrollView.horizontalScrollBar = _enable ? dragProgressBar : null;
            if (dragProgressBar != null && _enable) EventDelegate.Add(dragProgressBar.onChange, scrollView.OnScrollBar);
        }
        canDrag = _enable;
    }
    protected int refreshIndex = 0;
    protected int refreshCount = 0;
    protected void RefreshContainer()
    {
        refreshCount++;
        if (CurBagPage == 1)
        {
            refreshIndex = refreshCount-1;//0.1.2
        }
        else if (CurBagPage == MaxPage)
        {
            refreshIndex = itemUIContainer.Length - refreshCount;//2.1.0
        }
        else
        {
            refreshIndex = refreshCount;//1.2.0
            switch (refreshCount)
            {
                case 1: refreshIndex = 1;
                    break;
                case 2: refreshIndex = 2;
                    break;
                case 3: refreshIndex = 0;
                    break;
            }
        }
        if (refreshCount > itemUIContainer.Length)
        {
            SetScrollViewDragEnable(true);
            return;
        }
        if (itemContainerParent.Length > refreshIndex && itemContainerParent[refreshIndex] != null)
        {
            if (itemUIContainer[refreshIndex] == null)
            {
                itemUIContainer[refreshIndex] = itemContainerParent[refreshIndex].AddComponent<ItemUIContainer>();
                itemUIContainer[refreshIndex].OnFinish = RefreshContainer;
                itemUIContainer[refreshIndex].showUIType = curItemShowUIType;
                RefreshItems(curItemList);
            }
            else
            {
                RefreshContainer();//已经创建则重新计算
            }
        }
        
    }

    /// <summary>
    /// 创建所有物品
    /// </summary>
    protected void RefreshItems(List<EquipmentInfo> _itemList)
    {
        curItemList = _itemList;
        Dictionary<int, EquipmentInfo> onePageItems = new Dictionary<int, EquipmentInfo>();
        int startPage = (int)CurBagPage;
        if (startPage <= 1)
        {
            startPage = 1;//UI上三页表现为:|口口口
        }
        else if (startPage >= MaxPage)
        {
            startPage = MaxPage - 2;//UI上三页表现为:口口口|
        }
        else
        {
            startPage = startPage - 1;//UI上三页表现为:|..口口口..|
        }
        int page = 0;
        int index = 0;
        for (int i = 0, max = curItemList.Count; i < max; i++)
        {
            EquipmentInfo info = curItemList[i];
            index++;
            if (index <= (startPage-1) * PerPageCount) continue;//从当前页or前一页开始数
            if (page >= 3) break;//数到第三页结束
            onePageItems[info.Postion] = info;
            if (index % PerPageCount == 0)//每页20个格子
            {
                if (itemUIContainer != null && itemUIContainer.Length > page && itemUIContainer[page] != null)
                {
                    itemUIContainer[page].transform.localPosition = new Vector3(5 * cellWidth * page + 23, 165, -1);
                    itemUIContainer[page].CellWidth = cellWidth;
                    itemUIContainer[page].CellHeight = cellWidth;
                    itemUIContainer[page].RefreshItems(onePageItems, 5, PerPageCount);
                }
                onePageItems = new Dictionary<int, EquipmentInfo>();
                page++;
            }
        }
    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="_curPage">初始页数,针对打开背包直接显示第N页的需求</param>
    /// <param name="_itemList">初始显示物品集合</param>
    public void Init(int _curPage, List<EquipmentInfo> _itemList)
    {
        curItemList = _itemList;
        CurBagPage = _curPage;
        if (progressBar != null) progressBar.value = (CurBagPage - 1f) * (1f / ((float)MaxPage - 1f));
        InitDragProgress();
        SetScrollViewDragEnable(false);//设置不可拖动
        RefreshContainer();//实例化物品容器
    }
    /// <summary>
    /// 更新物品集合,在数据变化的时候调用
    /// </summary>
    /// <param name="_itemList"></param>
    public void UpdateItems(List<EquipmentInfo> _itemList)
    {
        curItemList = _itemList;
        RefreshItems(curItemList);
    }
    protected void InitDragProgress()
    {
        if (itemUIContainerAnchor != null)
        {
            float oldValue = dragProgressBar.value;
            if (CurBagPage == 1)
            {
                dragProgressBar.value = 0f;
                itemUIContainerAnchor.localPosition = Vector3.zero;
            }
            else if (CurBagPage == MaxPage)
            {
                dragProgressBar.value = 1f;
                itemUIContainerAnchor.localPosition = new Vector3(-5 * cellWidth*2, 0, 0);
            }
            else
            {
                dragProgressBar.value = 0.5f;
                itemUIContainerAnchor.localPosition = new Vector3(-5 * cellWidth, 0, 0);
            }
        }
        //if (Mathf.Approximately(oldValue, dragProgressBar.value))
        //    EventDelegate.Execute(dragProgressBar.onChange);
    }


    #region 额外需求
    protected ItemShowUIType curItemShowUIType = ItemShowUIType.NONE;
    /// <summary>
    /// 设置ItemShowUIType,在Init方法之前调用
    /// </summary>
    /// <param name="_itemShowUIType"></param>
    public void SetUITypeForShop(ItemShowUIType _itemShowUIType)
    {
        curItemShowUIType = _itemShowUIType;
    }
    /// <summary>
    /// 因批量出售而更新显示
    /// </summary>
    /// <param name="_putaway"></param>
    public void RefreshForBatchSell(bool _batchSell)
    {
        if (itemUIContainer != null)
        {
            for (int i = 0, length = itemUIContainer.Length; i < length; i++)
            {
                if (itemUIContainer[i] != null)
                    itemUIContainer[i].UpdateItemForBatchSell(_batchSell);
            }
        }
        RefreshItems(curItemList);
    }
    #endregion
}
