///////////////////////////////////////////////////////////////////////////////
// 作者：吴江
// 日期：2015/4/29
// 用途：界面窗口的基类
///////////////////////////////////////////////////////////////////////////////



using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 界面窗口的基类 by吴江
/// </summary>
public class GUIBase : MonoBehaviour
{
    /// <summary>
    /// 子窗口数组
    /// </summary>
    public SubWnd[] subWndArray;
	/// <summary>
	/// 子界面的加载状态
	/// </summary>
	protected Dictionary<SubGUIType,bool> subWndInstantiateState = new Dictionary<SubGUIType, bool>();
	/// <summary>
	/// 是否所有的子界面全部需要从预制加载
	/// </summary>
	protected bool allSubWndNeedInstantiate = false;
    /// <summary>
    /// 是否为互斥窗口 by吴江
    /// </summary>
    protected bool mutualExclusion = false;
    /// <summary>
    /// 是否在打开ui的时候需要关闭主相机
    /// </summary>
    protected bool needCloaseMainCamera = false;
    /// <summary>
    /// 是否为互斥窗口 by吴江
    /// </summary>
    public bool MutualExclusion
    {
        get { return mutualExclusion; }
    }
    /// <summary>
	/// 默认初始打开的子窗口类型  在base.OnOpen()之前赋初值()  因为注释掉了//StartCoroutine(StartInitSubWndState()) by吴江
    /// </summary>
    protected SubGUIType initSubGUIType = SubGUIType.NONE;
    /// <summary>
    /// 默认初始打开的子窗口类型 by吴江
    /// </summary>
    public SubGUIType InitSubGUIType
    {
        set
        {
			initSubGUIType = value;
        }
        get
        {
            return initSubGUIType;
        }
    }
    /// <summary>
    /// 当前打开的子窗口类型 by吴江 
    /// </summary>
    protected SubGUIType curSubGUIType = SubGUIType.NONE;
    /// <summary>
    /// 当前打开的子窗口类型 by吴江 
    /// </summary>
    public SubGUIType CurSubGUIType
    {
        get
        {
            return curSubGUIType;
        }
    }

    /// <summary>
    /// 依赖背景窗口 吴江
    /// </summary>
    protected GUIType baseOn = GUIType.NONE;
    /// <summary>
    /// 子窗口列表 by吴江
    /// </summary>
    protected FDictionary subWndDictionary = new FDictionary();
    /// <summary>
    /// 本界面的UI深度 by吴江
    /// </summary>
	protected GUIZLayer layer = GUIZLayer.BASE;
    /// <summary>
    /// 本界面的UI深度 by吴江
    /// </summary>
	 public GUIZLayer Layer
    {
        get { return layer; }
		
		protected set
		{
			layer=value;
			this.transform.localPosition = new Vector3(this.transform.localPosition.x,
				this.transform.localPosition.y,-(float)layer/100);
		}
    }
    /// <summary>
    /// 是否为第一次打开 by吴江
    /// </summary>
   private bool isFirstTime = true;	
	
	

    /// <summary>
    /// 窗口刚加载进来默认是关闭，需要手动调用OpenUI（） by吴江
    /// </summary>
    void Awake()
    {
        this.gameObject.SetActive(false);
    }

    //void Start () {
	
    //}
	
    //void Update () {
	
    //}


    /// <summary>
    /// 供外部调用的关闭接口  by吴江
    /// </summary>
    /// <returns></returns>
    public virtual GUIBase CloseUI()
    {
        if (this == null) return this;
        if (this.gameObject == null) return null;
        if (this.gameObject.activeSelf)
        {
            this.gameObject.SetActive(false);
            OnClose();
        }
        return this;    
    }


    /// <summary>
    /// 供外部调用的打开接口  by吴江
    /// </summary>
    /// <returns></returns>
    public virtual GUIBase OpenUI(GUIType _baseOn = GUIType.NONE)
    {
		baseOn = _baseOn; 
        if (!this.gameObject.activeSelf)
        {
            this.gameObject.SetActive(true);
            TweenAlphaAllChild x = this.gameObject.GetComponent<TweenAlphaAllChild>();
            if (x != null)
            {
                x.ResetToBeginning();
                x.enabled = true;
            }

            subWndDictionary.Clear();
            for (int i = 0; i < subWndArray.Length; i++)
            {
				if(subWndArray[i] != null)subWndDictionary[subWndArray[i].type] = subWndArray[i];
            }

            OnOpen();
        }
        return this; 
    }

    /// <summary>
    /// 第一次开窗时调用  by吴江  应该每次开窗都要调用呀
    /// </summary>
    protected virtual void Init()
    {
        if (subWndDictionary.ContainsKey(initSubGUIType))
        {
            curSubGUIType = SwitchToSubWnd(initSubGUIType);
        }
    }


    protected SubGUIType SwitchToSubWnd(SubGUIType _subType)
    {
        foreach (SubWnd item in subWndDictionary.Values)
        {
            item.CloseUI();
        }
		if(allSubWndNeedInstantiate && !subWndInstantiateState.ContainsKey(_subType) && subWndDictionary.ContainsKey(_subType))
		{
			SubWnd subwnd = Instantiate(subWndDictionary[_subType] as SubWnd) as SubWnd;
			if(subwnd != null)
			{
				subwnd.transform.parent = transform;
				subwnd.transform.localPosition = Vector3.zero;
				subwnd.transform.localScale = Vector3.one;
				subWndDictionary[_subType] = subwnd;
				subWndInstantiateState[_subType] = true;
			}
		}
        if (subWndDictionary.ContainsKey(_subType))
        {
			initSubGUIType = _subType;//打开界面的时候赋值  by邓成
            SubWnd aim = subWndDictionary[_subType] as SubWnd;
            if (aim != null)
            {
                aim.OpenUI();
                return _subType;
            }
            else
            {
                Debug.LogError("子窗口：" + _subType + "为空！打开失败！");
                return SubGUIType.NONE;
            }
        }
        else
        {
            if (_subType!=SubGUIType.NONE)
                Debug.LogError("找不到子窗口：" + _subType + " ,请先注册！");
            return SubGUIType.NONE;
        }
    }

    /// <summary>
    /// 实际从打开变成关闭时被调用  by吴江
    /// </summary>
    protected virtual void OnClose()
    {
       // ToolTipMng.CloseAllTooltip();
		 HandEvent(false);

        ///事件注销OnClose by 贺丰
         foreach (SubWnd item in subWndDictionary.Values)
         {
             item.CloseUI();
         }
		subWndDictionary.Clear();
		subWndInstantiateState.Clear();
         if (needCloaseMainCamera)
         {
             GameCenter.cameraMng.MainCameraActive(true);
         }
         //StopAllCoroutines();
    }


    /// <summary>
    /// 实际从关闭变成打开时被调用  by吴江
    /// </summary>
    protected virtual void OnOpen()
    {
        if (isFirstTime)
        {
            isFirstTime = false;
            Layer = layer;
        }
		Init();
        HandEvent(true);
        if (needCloaseMainCamera)
        {
            GameCenter.cameraMng.MainCameraActive(false);
        }
        //StartCoroutine(StartInitSubWndState());
		InitSubWndState();
    }



    protected virtual void InitSubWndState()
    {
    }


    protected IEnumerator StartInitSubWndState()
    {
        yield return new WaitForFixedUpdate();
        InitSubWndState();
    }
    /// <summary>
    /// 事件绑定接口。在基类的OpenUI()中被调用，参数为true，在基类的CloseUI()中被调用，参数为false.界面关闭以后也需要监听的事件不能在这个接口中注册；  by吴江
    /// </summary>
    /// <param name="_bind"></param>
    protected virtual void HandEvent(bool _bind)
    {
    }



    //public static void PaintItemList(List<EquipmentInfo> _eqList, GameObject _parent, int _maxPerLine, 
    //    UIExGrid.Arrangement _arrangement = UIExGrid.Arrangement.Horizontal, float _cellWidth = 100f, float _cellHeight = 100f, bool sorted = false)//可选参数
    //{
    //    UIExGrid grid = _parent.GetComponent<UIExGrid>();
    //    if (grid == null) grid = _parent.AddComponent<UIExGrid>();
    //}
}
