//============================================
//作者：吴江
//日期：2016/2/14
//用途：预览随从的表现层对象
//==============================================


using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// 预览随从的表现层对象 by吴江
/// </summary>
public class PreviewEntourage : EntourageBase
{
    /// <summary>
    /// 是否为互斥对象 by吴江
    /// </summary>
    public bool mutualExclusion = true;

    public new MercenaryInfo actorInfo
    {
        get { return base.actorInfo as MercenaryInfo; }
        set
        {
            base.actorInfo = value;
        }
    }


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

    public string idleAnimName = string.Empty;

    protected new EntourageAnimFSM animFSM = null;

    /// <summary>
    /// 创建净数据对象
    /// </summary>
    /// <param name="_id"></param>
    /// <returns></returns>
    public static PreviewEntourage CreateDummy(MercenaryInfo _info)
    {
        GameObject newGO = null;
        if (GameCenter.instance.dummyOpcPrefab != null)
        {
            newGO = Instantiate(GameCenter.instance.dummyOpcPrefab) as GameObject;
            newGO.name = "Dummy OPE [" + _info.ServerInstanceID + "]";
        }
        else
        {
            newGO = new GameObject("Dummy OPE[" + _info.ServerInstanceID + "]");
        }
        newGO.tag = "Entourage";
        newGO.SetMaskLayer(LayerMask.NameToLayer("Preview"));
        PreviewEntourage newOPC = newGO.AddComponent<PreviewEntourage>();
        newOPC.isDummy_ = true;
        newOPC.id = _info.ServerInstanceID;
        newOPC.actorInfo = _info;
        newOPC.transform.localRotation = new Quaternion(0, _info.RotationY, 0, 0);
        return newOPC;
    }


    public void StartAsyncCreate(System.Action<PreviewEntourage> _callback = null)
    {
        StartCoroutine(CreateAsync(_callback));
    }

    IEnumerator CreateAsync(System.Action<PreviewEntourage> _callback = null)
    {
        if (isDummy_ == false)
        {
            GameSys.LogError("You can only start create other player in dummy: " + actorInfo.ServerInstanceID);
            yield break;
        }
        //
        isDownloading_ = true;				//判断是否正在下载，防止重复创建

        PreviewEntourage opc = null;
        SmartActorRendererCtrl myRendererCtrl = null;
        bool faild = false;
        pendingDownload = Create(actorInfo, delegate(PreviewEntourage _opc, EResult _result)
        {

            if (_result != EResult.Success)
            {
                faild = true;
                return;
            }

            opc = _opc;
            pendingDownload = null;
            myRendererCtrl = opc.gameObject.GetComponentInChildrenFast<SmartActorRendererCtrl>();
            myRendererCtrl.Show(false);
        });
        if (mutualExclusion)
        {
            GameCenter.previewManager.PushDownLoadTask(pendingDownload);
        }
        while (opc == null || opc.inited == false)
        {
            if (faild) yield break;
            yield return null;
        }
        if (mutualExclusion)
        {
            GameCenter.previewManager.EndDownLoadTask(pendingDownload);
        }
        pendingDownload = null;
        isDownloading_ = false;


        if (_callback != null)
        {
            _callback(opc);
        }
    }


    protected AssetMng.DownloadID Create(MercenaryInfo _info,
                                    System.Action<PreviewEntourage, EResult> _callback)
    {
        return exResources.GetEntourage(_info.AssetName,
                                        delegate(GameObject _asset, EResult _result)
                                        {
                                            if (_result != EResult.Success)
                                            {
                                                _callback(null, _result);
                                                return;
                                            }
                                            this.gameObject.name = "Other Entourage [" + _info.ServerInstanceID + "]";

                                            GameObject newGo = Instantiate(_asset) as GameObject;
                                            newGo.name = _asset.name;
                                            newGo.transform.parent = this.gameObject.transform;
                                            newGo.transform.localEulerAngles = Vector3.zero;
                                            newGo.transform.localPosition = Vector3.zero;
                                            newGo.AddComponent<EntourageAnimFSM>();
                                            newGo.AddComponent<SmartActorRendererCtrl>();
                                            newGo.transform.localScale *= actorInfo.ModelScale;
                                            newGo.SetActive(false);
                                            newGo.SetActive(true);
                                            FXCtrl fxCtrl = this.gameObject.AddComponent<FXCtrl>();
                                            fxCtrl.canShowShadow = false;


                                            isDummy_ = false;
                                            animationRoot = newGo.transform;
                                            Init();

                                            _callback(this, _result);
                                        });
    }



    protected override void Init()
    {
        base.Init();
        gameObject.SetMaskLayer(LayerMask.NameToLayer("Preview"));

        rendererCtrl.ResetOriginalLayer(LayerMask.NameToLayer("Preview"));

        rendererCtrl.ResetLayer();
        if (mouseCollider == null) mouseCollider = gameObject.GetComponent<BoxCollider>();

        animFSM = base.animFSM as EntourageAnimFSM;

        inited_ = true;

        if (animFSM != null)
        {
            InitAnimation();
            animFSM.IdleImmediate();
        }
        stateMachine.Start();
        fxCtrl.DoShadowEffect(true);
    }





    protected new void Awake()
    {
        typeID = ObjectType.Player;
        base.Awake();
    }

    protected override void Regist()
    {
        base.Regist();
        actorInfo.OnAwakeUpdate += StartFight;
    }

    public override void UnRegist()
    {
        base.UnRegist();
        actorInfo.OnAwakeUpdate -= StartFight;
    }

    /// <summary>
    /// 勿删，Start（）实际在dummy状态被调用，要在启动时做的事情请去自定义的Init()接口 by吴江
    /// </summary>
    void Start()
    {
    }
    protected override void OnAliveStateUpdate(bool _alive)
    {
    }

    protected override void InitAnimation()
    {
        base.InitAnimation();
        animFSM.SetInCombat(false);
        if (idleAnimName != string.Empty)
            animFSM.SetupIdleAndMoveAnimationName(idleAnimName,"move2");
        animFSM.SetStartFightAnimationName("attack1",null);
    }
    /// <summary>
    /// 开始移动
    /// </summary>
    public override void MoveStart()
    {
        base.MoveStart();
        if (animFSM != null)
        {
            animFSM.SetMoveSpeed(CurRealSpeed / (actorInfo.AnimationMoveSpeedBase * actorInfo.ModelScale));
        }
    }

    public System.Action OnBeDestroy;

    /// <summary>
    /// 预览时的坐标
    /// </summary>
    public Vector3 PreviewPosition
    {
        get
        {
            return actorInfo.PreviewPosition;
        }
    }

    /// <summary>
    /// 预览时的角度
    /// </summary>
    public Vector3 PreviewRotation
    {
        get
        {
            return actorInfo.PreviewRotation;
        }
    }


    public void StartFight(bool _hasAwake)
    {
        if (_hasAwake)
        {
            animFSM.StartFight();
        }
    }

}