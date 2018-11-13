//============================================
//作者：吴江
//日期：2014/7/30
//用途：过场动画玩家表现层对象
//==============================================


using UnityEngine;
using System.Collections;

/// <summary>
/// 【其他玩家】的表现层对象 by吴江
/// </summary>
public class CGPlayer : PlayerBase
{

    /// <summary>
    /// 是否为演员对象（非真实对象）
    /// </summary>
    public override bool IsActor
    {
        get
        {
            return true;
        }
    }


    /// <summary>
    /// 创建净数据对象
    /// </summary>
    /// <param name="_id"></param>
    /// <returns></returns>
    public static CGPlayer CreateDummy(PlayerBaseInfo _info)
    {
        GameObject newGO = null;
        if (GameCenter.instance.dummyOpcPrefab != null)
        {
            newGO = Instantiate(GameCenter.instance.dummyOpcPrefab) as GameObject;
			newGO.name = "Dummy CG Player [" + _info.ServerInstanceID + "]";
        }
        else
        {
			newGO = new GameObject("Dummy CG Player[" + _info.ServerInstanceID + "]");
        }
        newGO.tag = "Player";
        newGO.SetMaskLayer(LayerMask.NameToLayer("CGPlayer"));
        newGO.AddComponent<ActorMoveFSM>();
        CGPlayer newOPC = newGO.AddComponent<CGPlayer>();
        newOPC.isDummy_ = true;
		newOPC.id = _info.ServerInstanceID;
        newOPC.actorInfo = _info;
        newOPC.typeID = ObjectType.CGObject;
        newOPC.curMoveSpeed = newOPC.actorInfo.StaticSpeed * MOVE_SPEED_BASE;
        newOPC.CurRealSpeed = newOPC.curMoveSpeed;
        newOPC.transform.localRotation = new Quaternion(0, _info.RotationY, 0, 0);
        GameCenter.curGameStage.PlaceGameObjectFromServer(newOPC, (int)_info.ServerPos.x, (int)_info.ServerPos.y, (int)newOPC.transform.localEulerAngles.y);
        GameCenter.curGameStage.AddObject(newOPC);
        return newOPC;
    }

	public bool changeProf = false;
	/// <summary>
	/// 重建模型，一般是因为转职
	/// </summary>
	protected override void ReStartAsyncCreate(int _prof)
	{
		base.ReStartAsyncCreate(_prof);
		changeProf = true;
	}

    public void StartAsyncCreate()
    {
        StartCoroutine(CreateAsync());
    }

    IEnumerator CreateAsync()
    {
        if (isDummy_ == false)
        {
			GameSys.LogError("You can only start create other player in dummy: " + actorInfo.ServerInstanceID);
            yield break;
        }
        //
        isDownloading_ = true;				//判断是否正在下载，防止重复创建

        CGPlayer opc = null;
        bool faild = false;
        pendingDownload = Create(actorInfo, delegate(CGPlayer _opc, EResult _result)
        {
            if (_result != EResult.Success)
            {
                faild = true;
                return;
            }

            opc = _opc;
            pendingDownload = null;
        });
        while (opc == null || opc.inited == false)
        {
            if (faild) yield break;
            yield return null;
        }

        pendingDownload = null;
        isDownloading_ = false;

        if (!actorInfo.IsAlive)
        {
            opc.Dead();
        }
    }


    protected AssetMng.DownloadID Create(PlayerBaseInfo _info,
                                    System.Action<CGPlayer, EResult> _callback)
    {
        return exResources.GetRace(_info.Prof,
                                        delegate(GameObject _asset, EResult _result)
                                        {
                                            if (_result != EResult.Success)
                                            {
                                                _callback(null, _result);
                                                return;
                                            }
											this.gameObject.name = "CG Player [" + _info.ServerInstanceID + "]";

                                            GameObject newGo = Instantiate(_asset) as GameObject;
                                            newGo.name = _asset.name;
                                            newGo.transform.parent = this.gameObject.transform;
                                            newGo.transform.localEulerAngles = Vector3.zero;
                                            newGo.transform.localPosition = Vector3.zero;
                                            newGo.AddComponent<PlayerAnimFSM>();
                                            newGo.AddComponent<PlayerRendererCtrl>();
                                            newGo.AddComponent<FXCtrl>();
      

                                            isDummy_ = false;
                                            animationRoot = newGo.transform;
                                            Init();

                                            _callback(this, _result);
                                        });
    }



    protected override void Init()
    {
        base.Init();

        rendererCtrl.ResetOriginalLayer(LayerMask.NameToLayer("CGPlayer"));
        rendererCtrl.ResetLayer();
        if (headTextCtrl == null) headTextCtrl = this.gameObject.AddComponent<HeadTextCtrl>();
       // headTextCtrl.SetName(actorInfo.Name); //演员不要展示名字


        //abilityMng = AbilityMng.CreateNew(this, ref abilityMng);

        inited_ = true;

        if (animFSM != null)
        {
            InitAnimation();
        }
        stateMachine.Start();
        if (fxCtrl != null)
        {
            fxCtrl.DoShadowEffect(true);
        }
        
    }

    //演员不要展示名字
    protected override void OnNameChange(string _newName)
    {
        return;
    }

    protected new void Awake()
    {
        typeID = ObjectType.CGObject;
        base.Awake();
    }

    /// <summary>
    /// 勿删，Start（）实际在dummy状态被调用，要在启动时做的事情请去自定义的Init()接口 by吴江
    /// </summary>
    void Start()
    {  
    }


        /// <summary>
    /// 重新实例化坐骑
    /// </summary>
    protected override void ReBuildMount()
    {
    }


       /// <summary>
    /// 上坐骑
    /// </summary>
    public override void RideMount(bool _ride, bool _isChange)
    {
    }

    protected override void EquipUpdate(EquipSlot _slot)
    {
        return;
    }

    /// <summary>
    /// 开始移动，执行移动动画
    /// </summary>
    public override void MoveStart()
    {
        //		Debug.Log("MoveStartMoveStartMoveStartMoveStart");
        if (animFSM)
        {
            animFSM.Move();
        }
    }
    /// <summary>
    /// 移动结束，执行站立动画
    /// </summary>
    public override void MoveEnd()
    {
        if (animFSM)
        {
            animFSM.StopMoving();
        }
    }

    protected void InitAnimation()
    {
        //animFSM.SetIdleAndMoveAnimationName("stand", "run");
        //animFSM.SetCombatAnimationName("wait");
        //animFSM.SetRandIdleAnimationName("waiting");
        animFSM.StartStateMachine();
        animFSM.Idle();
    }


    public void ReEnable()
    {
        ReLive();
        TickAnimation();
    }

}
