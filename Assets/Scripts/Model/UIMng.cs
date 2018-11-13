//==============================================
//作者：吴江
//日期：2015/5/21
//用途：UI的管理类
//=================================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// UI的管理类 by吴江
/// </summary>
public class UIMng : MonoBehaviour
{

    /// <summary>
    /// UI界面的根节点 by吴江
    /// </summary>
    public GameObject uIRoot = null;


    public static bool isClosingWnd = false;


    /// <summary>
    /// 当前UI界面的缓存列表 by吴江
    /// </summary>
    protected Dictionary<string, GUIBase> uiDictionary = new Dictionary<string, GUIBase>();
    /// <summary>
    /// 添加个UI变换的事件,用于:例如打开技能、背包、成长时，隐藏“新”提示  
    /// </summary>
    public System.Action onUpdateOpenUIEvent;

    protected GUIType curOpenType = GUIType.NONE;
    public GUIType CurOpenType
    {
        get
        {
            return curOpenType;
        }
        set
        {
            if (curOpenType != value && value == GUIType.NPCDIALOGUE)
            {
                NPC npc = GameCenter.curMainPlayer.CurTarget as NPC;
                if (npc != null)
                {
                    NPCInfo stateNpcFuction = GameCenter.sceneMng.GetNPCInfo(npc.id);
                    if (stateNpcFuction != null && stateNpcFuction.FocusX != 0 && stateNpcFuction.FocusY != 0)
                    {
                        GameCenter.cameraMng.FocusOn(npc, stateNpcFuction.FocusX, stateNpcFuction.FocusY, stateNpcFuction.FocusDistance, 0.5f);
                        //GameCenter.cameraMng.MainCameraActivePlayer( false);//隐藏所有玩家
                    }
                }
            }
            if (curOpenType == GUIType.NPCDIALOGUE && value != GUIType.NPCDIALOGUE)
            {
                if (GameCenter.curMainPlayer != null)
                {
                    GameCenter.cameraMng.FocusOn(GameCenter.curMainPlayer, 0.5f);
                    //GameCenter.cameraMng.MainCameraActivePlayer(true);
                }
            }
            curOpenType = value;
            if (value == GUIType.NONE)
                ShowMain(true);//显示主界面(主菜单/小地图/任务等)
            if (onUpdateOpenUIEvent != null)
                onUpdateOpenUIEvent();
        }
    }

	protected GUIType curBaseOnType = GUIType.NONE;
	public GUIType CurBaseOnType
	{
		get
		{
			return curBaseOnType;
		}
		set
		{
			if(curBaseOnType != value)
			{
				curBaseOnType = value;
			}
		}
	}

    protected GUIType curMutualExclusionType = GUIType.NONE;
    public GUIType CurMutualExclusionType
    {
        set
        {
            curMutualExclusionType = value;
            ShowMain(value == GUIType.NONE);//value肯定不为GUIType.NONE,这里是隐藏主菜单等界面
            
        }
    }

    /// <summary>
    /// 初始化  by吴江
    /// </summary>
    void Start()
    {
        if (uIRoot != null)
        {
            GameObject.DontDestroyOnLoad(uIRoot);
            UIRoot uiroot = uIRoot.GetComponent<UIRoot>();
            if (uiroot != null)
            {
                uiroot.manualHeight = 1136 * Screen.height / Screen.width;
            }
            else
            {
                Debug.LogError("在UIRoot预制上找不到UIRoot组件！");
            }
        }
        else
        {
            Debug.LogError("ui root is null !");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void Update()
    {

    }

    /// <summary>
    /// 直接跳转到某个子界面,必须是跨界面打开才调用。如果是打开本身界面的子界面，则用本身界面自身的接口 by吴江
    /// </summary>
    /// <param name="_subType"></param>
    /// <param name="_baseOn"></param>
    public void SwitchToSubUI(SubGUIType _subType, GUIType _baseOn = GUIType.NONE)
    {
        GUIType mainType = ConfigMng.Instance.GetSubGUITypeBelong(_subType);
        if (mainType != GUIType.NONE)
        {
            SwitchToUI(mainType, _subType, _baseOn);
        }
    }


    /// <summary>
    /// 跳转至某个功能的UI组   by吴江
    /// </summary>
    /// <param name="_type"></param>
    public void SwitchToUI(GUIType _type, SubGUIType _subType, GUIType _baseOn = GUIType.NONE)
    {
		bool needTip = true;
		if (!CheckFunction(_type, _subType,out needTip))
        {
			if(needTip)GameCenter.messageMng.AddClientMsg(389);
            return;
        }	
        CurOpenType = _type;
		CurBaseOnType = _baseOn;
        List<string> baseList = null;
        if (_baseOn != GUIType.NONE)
        {
            baseList = GetUIByType(_baseOn);
        }
        List<string> needReleaseList = new List<string>();
        foreach (var item in uiDictionary.Keys)
        {
            if (baseList != null && baseList.Contains(item)) continue;//如果有依赖背景窗口的话，那么依赖背景窗口逃避互斥 by吴江 
            if (uiDictionary[item].MutualExclusion)
            {
                needReleaseList.Add(item);
            }
        }
        List<string> needopenlist = GetUIByType(_type);
        for (int i = 0; i < needReleaseList.Count; i++)
        {
            string item = needReleaseList[i];
            if (!needopenlist.Contains(item))
            {
                GUIBase gui = uiDictionary[item];
                gui.CloseUI();
                uiDictionary[item] = null;
                uiDictionary.Remove(item);
                DestroyImmediate(gui.gameObject, true);
				DestroyImmediate(gui,true);
            }
        }

        for (int i = 0; i < needopenlist.Count; i++)
        {
            string item = needopenlist[i];
            if (uiDictionary.ContainsKey(item) && uiDictionary[item] != null)
            {
                if (_subType != SubGUIType.NONE)
                {
                    uiDictionary[item].InitSubGUIType = _subType;
                }
                if (uiDictionary[item].isActiveAndEnabled)
                {
                    uiDictionary[item].CloseUI();
                }
                uiDictionary[item].OpenUI(_baseOn);
                SetCompareTop(_type, _baseOn);
                if (uiDictionary[item].MutualExclusion)
                {
                    CurOpenType = _type;
					CurMutualExclusionType = _type;
                }
            }
            else
            {
                GenGUI(item, true, _subType, _baseOn, (x) =>
                {
                    SetCompareTop(_type, _baseOn);
                    if (x.MutualExclusion)
                    {
                        CurOpenType = _type;
						CurMutualExclusionType = _type;
                    }
                });
            }
        }

        isClosingWnd = true; 
    }


    /// <summary>
    /// 跳转至某个功能的UI组   by吴江
    /// </summary>
    /// <param name="_type"></param>
    public void SwitchToUI(GUIType _type, GUIType _baseOn = GUIType.NONE)
    {
        if (!CheckFunction(_type))
        {
            GameCenter.messageMng.AddClientMsg(389);//功能未开启
            return;
        }
        CurOpenType = _type;
		CurBaseOnType = _baseOn;
        List<string> baseList = null;
        if (_baseOn != GUIType.NONE)
        {
            baseList = GetUIByType(_baseOn);
        }
        List<string> needReleaseList = new List<string>();
        foreach (var item in uiDictionary.Keys)
        {
            if (baseList != null && baseList.Contains(item)) continue;//如果有依赖背景窗口的话，那么依赖背景窗口逃避互斥 by吴江 
            if (uiDictionary[item].MutualExclusion)
            {
                needReleaseList.Add(item);
            }
        }
        List<string> needopenlist = GetUIByType(_type);
        for (int i = 0; i < needReleaseList.Count; i++)
        {
            string item = needReleaseList[i];
            if (!needopenlist.Contains(item))
            {
                GUIBase gui = uiDictionary[item];
                gui.CloseUI();
                uiDictionary[item] = null;
                uiDictionary.Remove(item);
                DestroyImmediate(gui.gameObject, true);
				DestroyImmediate(gui,true);
            }
        }

        for (int i = 0; i < needopenlist.Count; i++)
        {
            string item = needopenlist[i];
            if (uiDictionary.ContainsKey(item) && uiDictionary[item] != null)
            {
                if (uiDictionary[item].isActiveAndEnabled)
                {
                    uiDictionary[item].CloseUI();
                }
                uiDictionary[item].OpenUI(_baseOn);
                SetCompareTop(_type, _baseOn);
                if (uiDictionary[item].MutualExclusion)
                {
                    CurOpenType = _type;
					CurMutualExclusionType = _type;
                }
            }
            else
            {
                GenGUI(item, true, SubGUIType.NONE, _baseOn, (x) =>
                {
                    SetCompareTop(_type, _baseOn);
                    if (x.MutualExclusion)
                    {
                        CurOpenType = _type;
						CurMutualExclusionType = _type;
                    }
                });
            }
        }
        isClosingWnd = true; 
    }
    /// <summary>
    /// 关闭所有界面,同时不播主界面缩放动画
    /// </summary>
    public void CloseAllNoAnimation()
    {
        List<string> needReleaseList = new List<string>();
        foreach (var item in uiDictionary.Keys)
        {
            if (uiDictionary[item].MutualExclusion)
            {
                needReleaseList.Add(item);
            }
        }
        for (int i = 0; i < needReleaseList.Count; i++)
        {
            string item = needReleaseList[i];
            GUIBase gui = uiDictionary[item];
            gui.CloseUI();
            uiDictionary[item] = null;
            uiDictionary.Remove(item);
            DestroyImmediate(gui.gameObject, true);
            DestroyImmediate(gui, true);
        }
        isClosingWnd = true; 
    }

    /// <summary>
    /// 让一个窗口组合在另外一个窗口组合之上by吴江
    /// </summary>
    /// <param name="_top"></param>
    /// <param name="_bottom"></param>
    public void SetCompareTop(GUIType _top, GUIType _bottom)
    {
        if (_top == GUIType.NONE || _bottom == GUIType.NONE) return;
        List<string> toplist = GetUIByType(_top);
        List<string> bottomlist = GetUIByType(_bottom);
        if (toplist.Count == 0 || bottomlist.Count == 0) return;
        List<GUIBase> topGuiList = new List<GUIBase>();
        for (int i = 0; i < toplist.Count; i++)
        {
            if (uiDictionary.ContainsKey(toplist[i]))
            {
                topGuiList.Add(uiDictionary[toplist[i]]);
            }
        }
        if (topGuiList.Count == 0) return;

        List<GUIBase> bottomGuiList = new List<GUIBase>();
        for (int i = 0; i < bottomlist.Count; i++)
        {
            if (uiDictionary.ContainsKey(bottomlist[i]))
            {
                bottomGuiList.Add(uiDictionary[bottomlist[i]]);
            }
        }
        if (topGuiList.Count == 0) return;
        for (int i = 0; i < topGuiList.Count; i++)
        {
            for (int j = 0; j < bottomGuiList.Count; j++)
            {
                SetCompareTop(topGuiList[i], bottomGuiList[j]);
            }
        }
    }

    /// <summary>
    /// 让一个窗口在另外一个窗口之上 by吴江
    /// </summary>
    /// <param name="_top"></param>
    /// <param name="_bottom"></param>
    public void SetCompareTop(GUIBase _top, GUIBase _bottom)
    {
        if (_top == null || _bottom == null) return;
        if (_top.Layer <= _bottom.Layer)
        {
            _top.transform.localPosition = new Vector3(_top.transform.localPosition.x,
                _top.transform.localPosition.y, -(((int)_bottom.Layer + 1000) / 100f));
        }
    }


    /// <summary>
    /// 打开某个界面(不建议使用，更多情况请了解 SwitchToUI 接口) by吴江
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_name"></param>
    /// <returns></returns>
    public T OpenGui<T>(string _name = "") where T : GUIBase
    {
        if (_name != "")
        {
            if (uiDictionary.ContainsKey(_name))
            {
                if (uiDictionary[_name] != null) uiDictionary[_name].OpenUI();
                return uiDictionary[_name] as T;
            }
        }
        else
        {
            T gui = null;
            foreach (GUIBase instance in uiDictionary.Values)
            {
                gui = instance as T;
                if (gui != null)
                {
                    break;
                }
            }
            if (gui != null) gui.OpenUI();
            return gui;
        }
        return null;
    }


    /// <summary>
    /// 关闭某个界面(不建议使用，更多情况请了解 SwitchToUI 接口) by吴江
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_name"></param>
    /// <returns></returns>
    public T CloseGui<T>(string _name = "") where T : GUIBase
    {
        if (_name != "")
        {
            if (uiDictionary.ContainsKey(_name))
            {
                if (uiDictionary[_name] != null) uiDictionary[_name].CloseUI();
                return uiDictionary[_name] as T;
            }
        }
        else
        {
            T gui = null;
            foreach (GUIBase instance in uiDictionary.Values)
            {
                gui = instance as T;
                if (gui != null)
                {
                    break;
                }
            }
            if (gui != null) gui.CloseUI();
            return gui;
        }
        return null;
    }


    /// <summary>
    /// 获取某个界面 by吴江
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_name"></param>
    /// <returns></returns>
    public T GetGui<T>(string _name = "") where T : GUIBase
    {
        if (_name != "")
        {
            if (uiDictionary.ContainsKey(_name))
            {
                return uiDictionary[_name] as T;
            }
        }
        else
        {
            T gui = null;
            foreach (GUIBase instance in uiDictionary.Values)
            {
                gui = instance as T;
                if (gui != null)
                {
                    break;
                }
            }
            return gui;
        }
        return null;
    }
    /// <summary>
    /// 是否有界面遮挡
    /// </summary>
    /// <returns></returns>
    public bool HaveBigWnd()
    {
        if (curOpenType != GUIType.NONE && curOpenType != GUIType.NPCDIALOGUE)
        {
            List<string> openlist = GetUIByType(curOpenType);
            for (int i = 0,length=openlist.Count; i < length; i++)
            {
                if (uiDictionary.ContainsKey(openlist[i]) && uiDictionary[openlist[i]] != null)
                {
                    GUIBase gui = uiDictionary[openlist[i]];
                    if (gui.MutualExclusion)
                        return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// 将一个界面！！！[组合]！！！实例从内存中释放 by吴江
    /// </summary>
    /// <param name="_type"></param>
    public void ReleaseGUI(GUIType _type)
    {
        List<string> needlist = GetUIByType(_type);
        for (int i = 0; i < needlist.Count; i++)
        {
            string name = needlist[i];
            if (uiDictionary.ContainsKey(name) && uiDictionary[name] != null)
            {
                GUIBase gui = uiDictionary[name];
                gui.CloseUI();
                uiDictionary[name] = null;
                uiDictionary.Remove(name);
                DestroyImmediate(gui.gameObject, true);
                DestroyImmediate(gui, true);
                isClosingWnd = true; 
            }
        }
    }

    /// <summary>
    /// 将一个界面！！！[组合]！关闭！ 除非特别了解已经确定，否则不建议使用。 建议使用SwitchToUI接口 by吴江
    /// </summary>
    /// <param name="_type"></param>
    public void CloseGUI(GUIType _type)
    {
        List<string> needlist = GetUIByType(_type);
        foreach (var item in needlist)
        {
            if (uiDictionary.ContainsKey(item) && uiDictionary[item] != null)
            {
                GUIBase gui = uiDictionary[item];
                gui.CloseUI();
            }
            isClosingWnd = true; 
        }
    }


    /// <summary>
    /// 将一个界面实例从内存中释放 by吴江
    /// </summary>
    /// <param name="_type"></param>
    public void ReleaseGUI<T>(string _name = "") where T : GUIBase
    {
        if (_name != "")
        {
            if (uiDictionary.ContainsKey(_name))
            {
                GUIBase gui = uiDictionary[_name];
                uiDictionary[_name] = null;
                uiDictionary.Remove(_name);
                if (gui != null)
                {
                    gui.CloseUI();
                    DestroyImmediate(gui.gameObject, true);
                    DestroyImmediate(gui, true);
                    isClosingWnd = true; 

                }
            }
        }
        else
        {
            T gui = null;
            foreach (GUIBase instance in uiDictionary.Values)
            {
                gui = instance as T;
                if (gui != null)
                {
                    break;
                }
            }
            if (gui != null)
            {
                string name = gui.name;
                if (uiDictionary.ContainsKey(name))
                {
                    uiDictionary[name] = null;
                    uiDictionary.Remove(name);
                }
                if (gui != null)
                {
                    gui.CloseUI();
                    DestroyImmediate(gui.gameObject, true);
                    DestroyImmediate(gui, true);
                    isClosingWnd = true; 

                }
            }
        }
    }


    /// <summary>
    /// 加载某个界面组合  by吴江
    /// </summary>
    /// <param name="_type"></param>
    /// <param name="_needOpen">加载完以后是否需要打开</param>
    public void GenGUI(GUIType _type, bool _needOpen, SubGUIType _subType = SubGUIType.NONE, GUIType _baseOn = GUIType.NONE, System.Action<GUIBase> _callback = null)
    {
        List<string> needopenlist = GetUIByType(_type);
        foreach (var item in needopenlist)
        {
            if (uiDictionary.ContainsKey(item) && uiDictionary[item] != null)
            {
                if (_needOpen)
                {
                    uiDictionary[item].OpenUI();
                }
                if (_callback != null)
                {
                    _callback(uiDictionary[item]);
                }
            }
            else
            {
                GenGUI(item, _needOpen, _subType, _baseOn, _callback);
            }
        }
    }

    /// <summary>
    /// 加载某个界面 by吴江
    /// </summary>
    /// <param name="_name"></param>
    /// <param name="_needOpen">加载以后是否需要打开</param>
    /// <returns></returns>
    public GUIBase GenGUI(string _name, bool _needOpen, SubGUIType _subType = SubGUIType.NONE, GUIType _baseOn = GUIType.NONE, System.Action<GUIBase> _callback = null)
    {
        if (uiDictionary.ContainsKey(_name) && uiDictionary[_name] != null)
        {
            if (_callback != null) _callback(uiDictionary[_name]);
            return _needOpen ? uiDictionary[_name].OpenUI() : uiDictionary[_name].CloseUI();
        }
        Object obj = exResources.GetResource(ResourceType.GUI, _name);
        if (obj == null)
        {
            Debug.LogError("找不到名为：" + _name + "的UI预制");
            return null;
        }
        GameObject gui = Instantiate(obj) as GameObject;
        gui.transform.parent = uIRoot.transform;
        gui.transform.localScale = Vector3.one;
        gui.transform.localPosition = Vector3.zero;
        gui.name = obj.name;
        obj = null;
        UIAnchor anchor = gui.GetComponent<UIAnchor>();
        gui.SetActive(false);
        if (anchor != null)
        {
            anchor.uiCamera = GameCenter.cameraMng.uiCamera;
        }
        GUIBase guibase = gui.GetComponent<GUIBase>();
        if (guibase != null)
        {
            uiDictionary[_name] = guibase;
            if (_subType != SubGUIType.NONE)
            {
                guibase.InitSubGUIType = _subType;
            }
            if (_needOpen)
            {
                guibase.OpenUI(_baseOn);
            }
            if (_callback != null)
            {
                _callback(guibase);
            }
            return guibase;
        }
        else
        {
            Debug.LogError("在" + _name + " UI预制上找不到 GUIBase 组件！");
        }
        return null;
    }
    /// <summary>
    /// 获取某类界面组合下的界面名称列表 by吴江
    /// </summary>
    /// <param name="_type"></param>
    /// <returns></returns>
    protected List<string> GetUIByType(GUIType _type)
    {
        List<string> openlist = new List<string>();
        switch (_type)
        {
            case GUIType.LOGIN:
                openlist.Add("Login/LoginWnd");
				break;
            case GUIType.MIRACLE:
                openlist.Add("Miracle/Miracle");
                break;
			case GUIType.MESSAGE://
                openlist.Add("Systexts_pane/MessgeUI");
                break;
            case GUIType.SECONDCONFIRM:
                openlist.Add("Systexts_pane/SecondConfirmWnd");
                break;
            case GUIType.CREATE_CHAR:
                openlist.Add("CreatePlayer/CreatePlayerWnd");
                break;
            case GUIType.TASK_FINDING:
                openlist.Add("Miscellany/Autopath");
                break;
            case GUIType.WAIT:
                openlist.Add("Land/Waiting_UI_new");               
                break;
            case GUIType.LOADING:
                openlist.Add("Loading/LoadingWnd");
                break;
            case GUIType.RECONNECT:
                openlist.Add("Miscellany/ReconnectBox");
                break;
            case GUIType.RETURN_LOGIN:
                openlist.Add("Miscellany/ReturnLoginWnd");
                break;
            case GUIType.MONSTER_HEAD:
                openlist.Add("mainUI/MonsterHeadWnd");
                break;
            case GUIType.VIP:
				openlist.Add("VIP/VIP_Charge");
                break;
			case GUIType.NPCDIALOGUE://
                openlist.Add("NPC/NPC_dialog");
                break;
			case GUIType.PANELLOADING://问答号加载
				openlist.Add("Panel_Loading/Panel_Loading");
				break;
            case GUIType.PRACTICE://
            	openlist.Add("Practice/Practice");
            	break;
			case GUIType.ATIVITY://
				openlist.Add("Activity/Activity");
                break;
			case GUIType.FORCE://
				openlist.Add("Copy/FailurePrompt");
				break;
			case GUIType.FORCETIP://
				openlist.Add("IncreaseStrength/IncreaseStrength");
				break;
            case GUIType.PREVIEW_MAIN:
				openlist.Add("mainPlayer/playerAvatarWind");
                break;
            case GUIType.ARENE://add 
				openlist.Add("Arena/Arena");
                break;
            case GUIType.ARENERESULT://add 
				openlist.Add("Arena/Settlement");
                break;
			case GUIType.MAIL://邮件窗口 
				openlist.Add("Social_Contact/Social_Contact");
                break;		
			case GUIType.SYSTEMSETTING://系统设置
                openlist.Add("SetUp/SetUp");
                break;
            case GUIType.GUILDMAIN:
					openlist.Add("Guild/GuildMainWnd");
				break;
			case GUIType.COPYWIN://by 何明军
				openlist.Add("Copy/CopySettlement");
				break;
			case GUIType.COPYWINFLOP://by 何明军
				openlist.Add("Copy/Flop");
				break;    
            case GUIType.UPDATEASSET:
                openlist.Add("Unzip/UnzipgWnd");
                break;
            case GUIType.SWEEPCARBON://add by何明军
				openlist.Add("SweepReward/SweepReward");
                break;
            case GUIType.MAINFIGHT:
                openlist.Add("mainUI/Main_Skill");
				openlist.Add("mainUI/Main_Player");
				openlist.Add("mainUI/Main_menu");
				openlist.Add("mainUI/Main_Time");
                break;
			case GUIType.MAINCOPPY:
				openlist.Add("mainUI/Main_copy");
				break;
            case GUIType.LITTLEMAP:
				openlist.Add("mainUI/Main_Map");
                mapMenuActive = true;
                break;
            case GUIType.TASK:
				openlist.Add("mainUI/Main_Task");
                break;
            case GUIType.INFORMATION:
				openlist.Add("Player_information/Player_information");
                break;
            case GUIType.LARGEMAP:
                openlist.Add("Map/Map");
                break;
            case GUIType.BOXGOTITEM://add by 贺丰  应该是没用了 by邓成
                openlist.Add("boxPanel/box");
                break;
            case GUIType.PREVIEWOTHERS://预览其他玩家界面
				openlist.Add("OtherPlayer/OtherPlayer");
                break;
			case GUIType.BOXREWARD://宝箱获取物品 by邓成
				openlist.Add("Backpack/GetItemTips");
                break;
            case GUIType.SPRITEANIMAL:
                openlist.Add("SpiritAnimal/SpiritAnimal");
                break;
            case GUIType.MAGICWEAPON:
                openlist.Add("MagicWeapon/MagicWeapon");
                break;
            case GUIType.SENDFLOWER:
                openlist.Add("Social_contact/Flowers");
                break;
            case GUIType.MARRIAGE:
                openlist.Add("Social_contact/Marriage_Interface");
                break;
            case GUIType.RANKREWARD:
                openlist.Add("RankReward/RankReward");
                break;
            case GUIType.EVERYDAYREWARD:
                openlist.Add("Welfare/Welfare");
                break;
            case GUIType.IMMEDIATEUSE://立即使用弹窗 by 唐源
                openlist.Add("ImmediateUse/Batch_Use");
                break;
            #region 仙侠新添
            case GUIType.FUNCTION:
			openlist.Add("mainUI/Main_function");
			break;
		case GUIType.MERRYGOROUND:
			openlist.Add("mainUI/Main_Merry");
			break;
		case GUIType.GUILDACTIVITYCOPPY:
			openlist.Add("mainUI/Main_GuildActivity");
			break;
			case GUIType.MagicTowerWnd://add by邓成
				openlist.Add("MagicTower/MagicTower");
				break;
			case GUIType.BACKPACK://add by邓成
				openlist.Add("Backpack/Backpack");
				break;
			case GUIType.BACKPACKWND://add by邓成
				openlist.Add("Backpack/BackpackWnd");
				break;
			case GUIType.COPYMULTIPLEWND:
				openlist.Add("Copy/MultipleCopy");
				break;
			case GUIType.COPYINWND:
				openlist.Add("Copy/Copy");
				break;
			case GUIType.ENDLESSWND:
				openlist.Add("EndLessTrials/EndLessTrials");
				break;
			case GUIType.STORAGE:
				openlist.Add("Backpack/Storage");
				break;
			case GUIType.STORAGEBASE:
				openlist.Add("Backpack/StorageBaseWnd");
				break;
			case GUIType.RINGTASK:
                //if(GameCenter.taskMng.FinishAllRingTask)
                //    openlist.Add("NPC/NPC_dialog");
                //else
					openlist.Add("Task/RingTask");
				break;
			case GUIType.TRIALTASK:
				openlist.Add("Task/TrialTask");
				break;
			case GUIType.EQUIPMENTTRAINING:
				openlist.Add("EquipmentTraining/EquipmentTraining");
				break;
            case GUIType.TRIALWING:
                openlist.Add("Chibangtankuang/Chibang");
                break;
            case GUIType.TREASUREHOUSE:
                openlist.Add("TreasureHouse/TreasureHouse");
                break;
            case GUIType.NEWRANKING:
                openlist.Add("Ranking/Ranking");
                break;
            case GUIType.SEVENDAYREWARD:
                openlist.Add("SevenDays/SevenDays");
                break;
            case GUIType.FIRSTCHARGEBONUS:
                openlist.Add("FirstChargeBonus/FirstChargeBonus");
                break;
            case GUIType.DESCRIPTION:
                openlist.Add("Description/Description");
                break;
		    case GUIType.SHOPWND:
			    openlist.Add("Shop/Shop");
			    break;
		    case GUIType.BUYWND:
			    openlist.Add("BuyWindow/BuyWindow");
			    break;
		    case GUIType.CASTSOUL:
			    openlist.Add("CastingSoul/CastingSoul");
			    break;
		    case GUIType.NEWMALL:
			    openlist.Add("Mall/Mall");
			    break;
		    case GUIType.GUILDSHOP:
			    openlist.Add("Guild/GuildShop");
			    break;
			case GUIType.GUILDSTORAGE:
				openlist.Add("Guild/GuildStorageWnd");
				break;
		    case GUIType.GUILDSILL:
			    openlist.Add("Guild/GuildSkill");
			    break;
			case GUIType.GUILDLIST:
				openlist.Add("Guild/GuildList");
				break;
			case GUIType.GUILDPROTECT:
				openlist.Add("GuildActivity/GuildProtect");
				break;
			case GUIType.GUILDSIEGE:
				openlist.Add("GuildActivity/GuildSiege");
				break;
		    case GUIType.DOWNLOADBONUS:
			    openlist.Add("DownloadBonus/DownloadBonus");
			    break;
		    case GUIType.MARKET:
			    openlist.Add("Market/Market");
			    break;
		    case GUIType.PUTAWAY:
			    openlist.Add("Market/PutawayWnd");
			    break;
			case GUIType.BOSSCHALLENGE:
				openlist.Add("BossDekaron/BossDekaron");
				break;
            case GUIType.ONLINEREWARD:
                openlist.Add("OnlineReward/NewOnlineReward");
                break;
            case GUIType.BUDOKAIMATCHING:
                openlist.Add("MartialArts/Matching");
                break;
            case GUIType.DARTWND:		
            	openlist.Add("Dart/Dart");
				break;
            case GUIType.BUDOKAI:
                openlist.Add("MartialArts/MartialArts");
                break;
            case GUIType.RESURRECTION:
                openlist.Add("Resurrection/ResurrectionUI");
                break;
            case GUIType.GUILDFIGHT:
                openlist.Add("GuildActivity/CelestialDomain");
                break;
			case GUIType.NPCDAILYDART:
				openlist.Add("NPC/NPC_dialog_gerenyunbiao");
				break;
			case GUIType.NPCGUILDDART:
				openlist.Add("NPC/NPC_dialog_gonghuiyunbiao");
				break;
			case GUIType.NPCMORSHIP:
				openlist.Add("NPC/NPC_dialog_mobai");
				break;
			case GUIType.NPCXIANLV:
				openlist.Add("NPC/NPC_dialog_xianlv");
				break;
            case GUIType.NPCRECHARGE:
				openlist.Add("NPC/NPC_dialog_chongzhifanli");
				break;
            case GUIType.NPCSWORN:
                openlist.Add("NPC/NPC_dialog_jieyi");
                break;
            case GUIType.NPCRAIDERARK:
                openlist.Add("NPC/NPC_dialog_renshenguo");
                break;
            case GUIType.GUILDBONFIRE:
                   openlist.Add("GuildActivity/GuildBonfireList");
                   break;
            case GUIType.RECHARGE:
                   openlist.Add("VIP/Recharge");
                   break;
            case GUIType.NEWSKILL:
                   openlist.Add("NewSkill/NewSkill");
                   break;
            case GUIType.TRADE:
                   openlist.Add("Transaction/Transaction");
                   break;
            case GUIType.BATCHNUM:
                   openlist.Add("BuyWindow/Batch_NUM");
                   break;
			case GUIType.DAILYMUSTDO:
				openlist.Add("Daydo/Daydo");
				break;
			case GUIType.CHAT:
				openlist.Add("Chatroom/Chat_box");
				break;	
			case GUIType.NOVICETIP:
				openlist.Add("NoviceTips/NoviceTips");
				break;
            case GUIType.WDFACTIVE:
                openlist.Add("Jingcaihuodong/Jingcaihuodong");
                break;
            case GUIType.PRIVILEGE:
                openlist.Add("Preferential/preferential");
                break;
            case GUIType.BUDOKAIEND:
                openlist.Add("MartialArts/MartialArtsWill");
                break; 
            case GUIType.OPENSERVER:
                openlist.Add("OpenService/OpenService");
                break;
			case GUIType.RETURN_EXIT:
				openlist.Add("Systexts_pane/ReturnExitWnd");
				break;
            case GUIType.NEWTITLEMSG:
                openlist.Add("Ranking/receive");
                break;
            case GUIType.ACTIVITYBALANCE:
                openlist.Add("Copy/Activitysettlement");
                break;
            case GUIType.WELCOME:
                openlist.Add("Welcome/Welcome");
                break;
			case GUIType.SCENE_ANIMATION:
				openlist.Add("SceneAnim/SceneAnimWnd");
				break;
			case GUIType.BLACK_SCREEN:
				openlist.Add("SceneAnim/SceneBlackWnd");
				break;
            case GUIType.DRUGLACKWND:
                openlist.Add("SetUp/DrugTips");  //临时去掉此功能
                break;
            case GUIType.BLESSWND:
                openlist.Add("Blessing/Blessing");
                break;
            case GUIType.SHOWMODELUI:
                openlist.Add("Chibangtankuang/Model_show");
                break;
            case GUIType.SHOWFLOWER:
                openlist.Add("Social_contact/SongHua_show");
                break;
            case GUIType.NEWFUNCTIONTIPUI:
                openlist.Add("NPC/Jijiangjinru");
                break;
            case GUIType.UPTIPUI:
                openlist.Add("Systexts_pane/Uptips");
                break;
            case GUIType.FLYREMIND:
                openlist.Add("Systexts_pane/ChuangSong_pop");
                break;
            case GUIType.ROYALBOXWND:
                openlist.Add("Baoxiang/Baoxiang");
                break;
            case GUIType.BATCHUSE:
                openlist.Add("Item_icon/Batch_Use");
                break;
            #endregion
            case GUIType.NEWREWARDPREVIEW:
                openlist.Add("NewRewardWnd/NewRewardPreview");
                break;
            case GUIType.POWERSAVING:
                openlist.Add("PowerSaving/PowerSaving");
                break; 
            case GUIType.BATTLEFIELDSETTMENT:
                openlist.Add("Battlefield/BattlefieldSettlement");
                break;
            case GUIType.NEWGUID:
                openlist.Add("NoviceGuide/NoviceGuide");
                break;
            case GUIType.BATTLECOMENTDES:
                openlist.Add("Battlefield/ScoreDes");
                break;
            case GUIType.OFFLINEREWARD:
                openlist.Add("OfflineReward/OffLineReward");
                break;
            case GUIType.DIALOGBOX:
                openlist.Add("Dialogbox/Dialog_box");
                break; 
            case GUIType.RENAMECARD:
                openlist.Add("Rename/renameUI");
                break;
            case GUIType.TREASURETROVE:
                openlist.Add("Treasure_trove/treasure_trove");
                break;
            case GUIType.DEFEATRECORD:
                openlist.Add("Resurrection/Resurrection");
                break;
            case GUIType.SEVENCHALLENGE:
                openlist.Add("sevenChallenge/sevenChallenge");
                break;
            case GUIType.DAILYFIRSTRECHARGE:
                openlist.Add("Shouchong/NewShouchong");
                break;
            case GUIType.RINGTASKTYPE:
                openlist.Add("Task/RingTaskType");
                break;
            case GUIType.HANGUPCOPPY:
                openlist.Add("HangUpCoppy/hangUpCoppy");
                break;
            case GUIType.GUILDACTIVE:
                openlist.Add("Guild/GuildActive");
                break;
            case GUIType.AUTO_RECONNECT:
                openlist.Add("Miscellany/ReconnectWaiting");
                break;
            default:
                break;
        }
        return openlist;
	}
    /// <summary>
    ///宝箱背景
    /// </summary>
    void LoadJackBackGround(List<string> _openlist)
    {
        _openlist.Add("Hero_infors/topAndBottomWind");
        _openlist.Add("Hero_infors/bgWind(Jackpot)");
    }

    public static void OpenMessegeBox(string _str, System.Action _ok, System.Action _no)
    {
    }

	protected Dictionary<string,bool> stateDic = new Dictionary<string, bool>();
	public bool firstOpen = true;
	/// <summary>
	/// 显示和隐藏某个主界面
	/// </summary>
	protected void ShowSingleMain(string wndPrefab, bool _show)
	{

        GUIBase wnd = GameCenter.uIMng.GetGui<GUIBase>(wndPrefab);
		if(wnd != null)
		{
			UIPlayAnimation playAnimation = wnd.GetComponent<UIPlayAnimation>();
			if(playAnimation != null)
			{
				bool nowState = !stateDic.ContainsKey(wnd.name)?true:stateDic[wnd.name];//当前显示状态
				if(_show == false && nowState==true)
				{
					playAnimation.PlayForward();//此方向都是隐藏UI
					stateDic[wnd.name] = false;
				}
				if(_show == true && nowState == false)
				{
					playAnimation.PlayReverse();//此方向都是显示UI
					stateDic[wnd.name] = true;
				}
				if(_show == true && nowState == true && !stateDic.ContainsKey(wnd.name))//初始情况
				{
					playAnimation.PlayReverse();
					stateDic[wnd.name] = true;
				}
			}
		}

	}
	/// <summary>
	/// 隐藏或者显示主界面所有界面
	/// </summary>
	public void ShowMain(bool _show)
	{
		if(_show)
		{
            ShowSingleMain("mainUI/Main_Task", _show);
            ShowSingleMain("mainUI/Main_Player", _show);
			ShowSkill(!lastMenuState);
			ShowMap(_show);
			ShowMenu(lastMenuState);
		}else
		{
			lastMenuState = menuState;
            ShowSingleMain("mainUI/Main_Task", _show);
            ShowSingleMain("mainUI/Main_Player", _show);
			ShowSkill(false);
			ShowMap(_show);
			ShowMenu(false);
		}

        MainTimeWnd wnd = GameCenter.uIMng.GetGui<MainTimeWnd>();
        if (wnd != null)
        {
            wnd.ShowObj(_show);
            wnd.ShowMiracleAccess(_show);
        }

	}

	public void ShowMap(bool _showMap)
	{
		LittleMapWnd wnd = GameCenter.uIMng.GetGui<LittleMapWnd>();
		if(wnd != null)
		{
			UIPlayAnimation playAnimation = wnd.GetComponent<UIPlayAnimation>();
			if(_showMap)playAnimation.PlayReverse();
			else playAnimation.PlayForward();
		}
	}

	protected bool menuState = false;
	protected bool lastMenuState = false;
	/// <summary>
	/// 隐藏或者显示下方菜单
	/// </summary>
	public void ShowMenu()
	{
		lastMenuState = menuState;
		menuState = !menuState;
		ShowSkill(!menuState);
		ShowMenu(menuState);
	}
	
	/// <summary>
	/// 显示下方菜单by 何明军
	/// </summary>
	public void OpenFuncShowMenu(bool show)
	{
		if(menuState != show)
		{
			lastMenuState = menuState;
			menuState = show;
			ShowSkill(!menuState);
			ShowMenu(menuState);
		}
	}

	public void ShowSkill(bool show)
	{
		SkillWnd wnd = GameCenter.uIMng.GetGui<SkillWnd>();
		if(wnd != null)
		{
			UIPlayAnimation playAnimation = wnd.GetComponent<UIPlayAnimation>();
			if(show)playAnimation.PlayReverse();
			else playAnimation.PlayForward();
		}
	}
	public void ShowMenu(bool show)
	{
		MainFightWnd wnd = GameCenter.uIMng.GetGui<MainFightWnd>();
		if(wnd != null)
		{
			UIPlayAnimation playAnimation = wnd.GetComponent<UIPlayAnimation>();
			if(show)playAnimation.PlayReverse();
			else playAnimation.PlayForward();
		}
	}
	protected bool mapMenuActive = true;
    public bool MapMenuActive
    {
        get
        {
            return mapMenuActive;
        }
    }
	/// <summary>
	/// 显示小地图菜单
	/// </summary>
	public void ShowMapMenu(bool _mapMenuActive)
	{
		if(mapMenuActive != _mapMenuActive)
		{
			GameObject btnMenu = GameObject.Find("title/ButtonMapMenu");
			if(btnMenu != null)
			{
				btnMenu.SendMessage("OnClick",SendMessageOptions.RequireReceiver);
			}
            //SetMapMenuState();  //这里不需要再次设值,SendMessage会调用LittleMapWnd中的OnClick方法
		}
	}
	/// <summary>
	/// 设值小地图菜单的显隐状态
	/// </summary>
	public void SetMapMenuState()
	{
		mapMenuActive = !mapMenuActive;
	}


	/// <summary>
	/// 检查功能是否开启
	/// </summary>
	protected bool CheckFunction(GUIType _guiType,SubGUIType _subType,out bool needTip)
	{
		needTip = true;
		if(_subType == SubGUIType.NONE)
		{
			switch(_guiType)
			{
			case GUIType.EQUIPMENTTRAINING:
				return GameCenter.mainPlayerMng.FunctionIsOpen(FunctionType.STRENGTHENING);
			case GUIType.GUILDMAIN:
				bool isOpen = GameCenter.mainPlayerMng.FunctionIsOpen(FunctionType.FAIRYAU);
				if(isOpen && string.IsNullOrEmpty(GameCenter.mainPlayerMng.MainPlayerInfo.GuildName))
				{
					GameCenter.messageMng.AddClientMsg(235);
					needTip = false;
					return false;
				}
				return isOpen;
			}
		}else
		{
			switch(_subType)
			{
			case SubGUIType.STRENGTHING://强化
				return GameCenter.mainPlayerMng.FunctionIsOpen(FunctionType.STRENGTHENING);
			case SubGUIType.EQUIPMENTUPGRADE://升阶
				return GameCenter.mainPlayerMng.FunctionIsOpen(FunctionType.DEGREE);
			case SubGUIType.ORANGEREFINE://橙炼
				return GameCenter.mainPlayerMng.FunctionIsOpen(FunctionType.ORANGEREFINING);
			case SubGUIType.EQUIPMENTWASH://洗练
				return GameCenter.mainPlayerMng.FunctionIsOpen(FunctionType.WASHSPRACTICE);
			case SubGUIType.EQUIPMENTINLAY://镶嵌
				return GameCenter.mainPlayerMng.FunctionIsOpen(FunctionType.MOSAIC);
			case SubGUIType.EQUIPMENTEXTEND://继承
				return GameCenter.mainPlayerMng.FunctionIsOpen(FunctionType.INHERITANCE);
			case SubGUIType.DECOMPOSITION://分解
				return GameCenter.mainPlayerMng.FunctionIsOpen(FunctionType.DECOMPOSITION);
            case SubGUIType.GROWUP://宠物成长
                return GameCenter.mainPlayerMng.FunctionIsOpen(FunctionType.PETGROWUP);
            case SubGUIType.LINGXIU://宠物灵修
                return GameCenter.mainPlayerMng.FunctionIsOpen(FunctionType.PETTHEKING);
            case SubGUIType.FUSE://宠物成长
                return GameCenter.mainPlayerMng.FunctionIsOpen(FunctionType.PETFUSE);
            case SubGUIType.PETSKILL://宠物成长
                return GameCenter.mainPlayerMng.FunctionIsOpen(FunctionType.PETSKILL);
            case SubGUIType.MOUNT://坐骑
                return GameCenter.mainPlayerMng.FunctionIsOpen(FunctionType.MOUNT);
            case SubGUIType.ILLUSION://幻化
                return GameCenter.mainPlayerMng.FunctionIsOpen(FunctionType.UNREAL);
            case SubGUIType.MAGICREFINE://法宝淬炼
                return GameCenter.mainPlayerMng.FunctionIsOpen(FunctionType.MAGIC);
            case SubGUIType.MAGICADDSOUL://法宝注灵
                return GameCenter.mainPlayerMng.FunctionIsOpen(FunctionType.MAGIC);
            case SubGUIType.UNDERBOSS://地宫BOSS(讨伐令)
                return GameCenter.mainPlayerMng.FunctionIsOpen(FunctionType.CHALLENGEBOSS);
			case SubGUIType.EXPANDMEMBER://仙盟成员扩充(聚义符)
				bool isOpen = GameCenter.mainPlayerMng.FunctionIsOpen(FunctionType.FAIRYAU);
				if(isOpen && string.IsNullOrEmpty(GameCenter.mainPlayerMng.MainPlayerInfo.GuildName))
				{
					GameCenter.messageMng.AddClientMsg(235);
					needTip = false;
					return false;
				}
				return isOpen;
			}
		}
		return true;
	}
	/// <summary>
	/// 检查功能是否开启
	/// </summary>
	protected bool CheckFunction(GUIType _guiType)
	{
		switch(_guiType)
		{
		case GUIType.EQUIPMENTTRAINING:
			return GameCenter.mainPlayerMng.FunctionIsOpen(FunctionType.STRENGTHENING);
        case GUIType.COPYINWND://副本
            return GameCenter.mainPlayerMng.FunctionIsOpen(FunctionType.COPY);
        case GUIType.TREASUREHOUSE://藏宝阁
            return GameCenter.mainPlayerMng.FunctionIsOpen(FunctionType.HIDDENTREASURE);
        case GUIType.CASTSOUL://铸魂
            return GameCenter.mainPlayerMng.FunctionIsOpen(FunctionType.CASTINGSOUL);
        case GUIType.SHOPWND://商店
            return GameCenter.mainPlayerMng.FunctionIsOpen(FunctionType.STORE);
        case GUIType.BOSSCHALLENGE://挑战Boss
            return GameCenter.mainPlayerMng.FunctionIsOpen(FunctionType.CHALLENGEBOSS);
		}
		return true;
	} 
}
