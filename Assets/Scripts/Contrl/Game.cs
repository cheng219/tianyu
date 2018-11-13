///////////////////////////////////////////////////////////////////////////////
// 作者：吴江
// 日期：2015/5/19
// 用途：游戏控制基类
///////////////////////////////////////////////////////////////////////////////




using UnityEngine;
using System.Collections;


/// <summary>
/// 游戏控制基类 by吴江
/// </summary>
public class Game : FSMBase
{
    /// <summary>
    /// 是否研发模式（正式发布版本为false） by吴江
    /// </summary>
    public bool isDevelopmentPattern = false;
	/// <summary>
	/// 是否接入平台  
	/// </summary>
	public bool isPlatform = false;

	public bool isInsideTest = true;

    public bool isDataEyePattern = false;

    public bool isNotNeedToMd5 = false;

    public bool isIOS_JB = false;
    /// <summary>
    /// 唯一实例
    /// </summary>
    public static Game instance = null;
    /// <summary>
    /// 当前的子控制台
    /// </summary>
    public static GameStage gameStage = null;
    /// <summary>
    /// 是否为测试
    /// </summary>
    public static bool isTestStage { get { return instance.isTestStage_; } }
    /// <summary>
    /// 是否接收操作输入
    /// </summary>
    public static bool acceptInput { get { return instance.acceptInput_; } }


    protected bool isTestStage_ = false;
    protected bool acceptInput_ = false;



    public void AcceptInput(bool _accept)
    {
        acceptInput_ = _accept;
    }



    public static void CreateDefault()
    {
        GameObject prefab = Resources.Load("Prefabs/Game", typeof(GameObject)) as GameObject;
        if (prefab == null)
        {
            Debug.LogError("Can not find default game prefab at Prefabs/MyGame");
        }
        GameObject gameGO = GameObject.Instantiate(prefab) as GameObject;
        gameGO.name = "Game";
        Game.instance.isTestStage_ = true;
    }


    protected new void Awake()
    {
        base.Awake();
        if (instance == null)
            instance = this;
        DontDestroyOnLoad(gameObject);
    }



    protected override void InitStateMachine()
    {
    }


    public void Init()
    {
        //if (GameSys.instance.Init() == false)
        //{
        //    Debug.LogError("Failed to init GameSys, application closed");
        //    return;
        //}

        //TO DO
        //if (AssetMng.instance.Init() == false)
        //{
        //    GameSys.LogError("Failed to init AssetMng, application closed");
        //    return;
        //}

        if (InitSubModules() == false)
        {
            Debug.LogError("Failed to init sub moduels, application closed");
            return;
        }

        if (stateMachine != null)
            stateMachine.Start();
    }



    protected virtual bool InitSubModules()
    {
        // init layer manager
        LayerMng.Init();
        return true;
    }



    protected void Update()
    {
        if (stateMachine != null)
            stateMachine.Update();
    }
}



