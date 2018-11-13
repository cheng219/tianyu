//=========================================
//作者：黄洪兴
//日期：2016/03/19
//用途：时装UI的容器
//=========================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 时装UI的容器
/// </summary>
public class FashionUIContainer : MonoBehaviour {



    public GameObject fashionprefab;
	/// <summary>
	/// 是否是第一次运行
	/// </summary>
	protected bool firstRun = true;
	/// <summary>
	/// 选择时装的事件
	/// </summary>
	public System.Action<FashionItemUI> OnSelectItemEvent = null;
	/// <summary>
	/// 当前选中的时装
	/// </summary>
	protected FashionItemUI selectFashionItemUI = null;
	/// <summary>
	/// 时装列表
	/// </summary>
	public List<FashionInfo> fashionList = new List<FashionInfo>();
	/// <summary>
	/// 排列
	/// </summary>
	public UIExGrid grid;

	public Dictionary<int, FashionItemUI> FashionContainers = new Dictionary<int, FashionItemUI>();
	// Use this for initialization
	void Start () {

	}
    void OnEnable()
    {
        GameCenter.fashionMng.OnChangeTargetFashion += RefreshAllItem;
    }
    void OnDisable()
    {
        GameCenter.fashionMng.OnChangeTargetFashion -= RefreshAllItem;
    }


    bool isInited = false;
    bool isFirstTime = true;


    public void RefreshItems(List<FashionInfo> _List)
    {

        fashionList.Clear();
        FashionContainers.Clear();
        for (int i = 0; i < _List.Count; i++)
        {
            fashionList.Add(_List[i]);
        }
        RefreshItems();
        isFirstTime = false;
    }


	/// <summary>
	/// 刷新数据
	/// </summary>
	/// <param name="_skList"></param>

	public void RefreshItems(Dictionary<int,FashionInfo> _Dic)
	{
		fashionList.Clear();
        FashionContainers.Clear();
		if (_Dic != null)
		{
            List<int> id = new List<int>();
            using (var e = _Dic.GetEnumerator())
            {

                while (e.MoveNext())
                {
                    if (e.Current.Value.RemainTime == null && e.Current.Value.Time != 0)
                    {
                        id.Add(e.Current.Value.FashionID);
                    }
                    if (e.Current.Value.RemainTime != null && e.Current.Value.Time != 0)
                    {
                        id.Add(e.Current.Value.TempID);
                    }
                
                }
            }
            bool s = true;
            using (var e = _Dic.GetEnumerator())
            {

                while (e.MoveNext())
                {
                    //for (int i = 0; i < id.Count; i++)
                    //{
                    //    bool b = false;
                    //    if (id[i] == e.Current.Value.FashionID)
                    //    {
                    //        b = true;
                    //    }
                    //    if (!b)
                    //    {
                    //        if (s)
                    //        {
                    //            GameCenter.fashionMng.CurTargetFashion = e.Current.Value;
                    //            s = false;
                    //            if (GameCenter.fashionMng.OnChangeTargetFashion != null )
                    //                GameCenter.fashionMng.OnChangeTargetFashion();
                    //        }
                            fashionList.Add(e.Current.Value);
                    //    }

                    //}
                }
            }
		}
		RefreshItems();
        isFirstTime = false;
	}
	/// <summary>
	/// 刷新表现
	/// </summary>
	public void RefreshItems()
	{
		if (fashionprefab== null)
		{
            return;
		}
		for(int i = 0;i<fashionList.Count;i++)
		{
				GameObject obj = Instantiate(fashionprefab) as GameObject;
				Transform parentTransf = this.gameObject.transform;
                obj.SetActive(true);
				obj.transform.parent = parentTransf;
				obj.transform.localPosition = Vector3.zero;
				obj.transform.localScale = Vector3.one;
				FashionItemUI fashionUI = obj.GetComponent<FashionItemUI>();
				fashionUI.FillInfo(fashionList[i]);
				fashionUI.OnSelectEvent += OnSelectSkillUI;
				FashionContainers[i] = fashionUI;
				//                if (firstRun && skillList[i].SkillLv>0)
				//                {
				//                    firstRun = false;
				//                    selectSkillUI = skillUI;
				//                    UIToggle tog = obj.GetComponent<UIToggle>();
				//                    tog.value = true;
				//                    if (OnSelectItemEvent != null)
				//                    {
				//                        OnSelectItemEvent(selectSkillUI);
				//                    }
				//                }
		}
		if(grid != null)
		{
			grid.Reposition();
		}
        isInited = true;
	}




	/// <summary>
	/// 升级刷新当前技能单元
	/// </summary>
	/// <param name="_info"></param>
	public void RefreshAllItem()
	{
        if (!isInited)
            return;
        using (var e = FashionContainers.GetEnumerator())
        {
            while(e.MoveNext())
            {

                e.Current.Value.RefreshFashion();

            }
        }

	}
	/// <summary>
	/// 选中一个技能
	/// </summary>
	/// <param name="_itemUI"></param>
	void OnSelectSkillUI(FashionItemUI _itemUI)
	{
		selectFashionItemUI = _itemUI;
		if (OnSelectItemEvent != null)
		{
			OnSelectItemEvent(selectFashionItemUI);
		}
	}
}
