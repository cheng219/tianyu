//============================================
//作者：吴江
//日期：2015/11/3
//用途：【其他玩家的随从】的表现层对象
//==============================================


using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// 【其他玩家的随从】的表现层对象 by吴江
/// </summary>
public class OtherEntourage : EntourageBase
{

    /// <summary>
    /// 从属主人
    /// </summary>
    protected OtherPlayer ownerPlayer = null;
    /// <summary>
    /// 从属主人
    /// </summary>
    public OtherPlayer OwnerPlayer
    {
        get
        {
            if (ownerPlayer == null)
            {
                ownerPlayer = GameCenter.curGameStage.GetOtherPlayer(actorInfo.OwnerID);
            }
            return ownerPlayer;
        }
    }


    public new MercenaryInfo actorInfo
    {
        get { return base.actorInfo as MercenaryInfo; }
        set
        {
            base.actorInfo = value;
        }
    }


    /// <summary>
    /// 与指定坐标的距离 by吴江
    /// </summary>
    /// <param name="_aim"></param>
    /// <returns></returns>
    public float GetDistanceSqr(Vector3 _aim)
    {
        return (this.transform.position - GameCenter.curMainPlayer.transform.position).sqrMagnitude;
    }
    /// <summary>
    /// 创建净数据对象
    /// </summary>
    /// <param name="_id"></param>
    /// <returns></returns>
    public static OtherEntourage CreateDummy(MercenaryInfo _info)
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
        newGO.SetMaskLayer(LayerMask.NameToLayer("Entourage"));
        OtherEntourage newOPC = newGO.AddComponent<OtherEntourage>();
        newOPC.isDummy_ = true;
        newOPC.moveFSM = newGO.AddComponent<ActorMoveFSM>();
        newOPC.id = _info.ServerInstanceID;
        newOPC.actorInfo = _info;
        newOPC.curMoveSpeed = newOPC.actorInfo.StaticSpeed * MOVE_SPEED_BASE;
        newOPC.CurRealSpeed = newOPC.curMoveSpeed;
        newOPC.RegistMoveEvent(true);
        newOPC.transform.localRotation = new Quaternion(0, _info.RotationY, 0, 0);
        GameCenter.curGameStage.PlaceGameObjectFromServer(newOPC, _info.ServerPos.x, _info.ServerPos.z, (int)newOPC.transform.localEulerAngles.y);
        GameCenter.curGameStage.AddObject(newOPC);
        return newOPC;
    }


    public void StartAsyncCreate(System.Action _callback = null)
    {
        StartCoroutine(CreateAsync(_callback));
    }

    IEnumerator CreateAsync(System.Action _callback = null)
    {
        if (isDummy_ == false)
        {
            Debug.LogError("You can only start create other player in dummy: " + actorInfo.ServerInstanceID);
            yield break;
        }
        //
        isDownloading_ = true;				//判断是否正在下载，防止重复创建

        OtherEntourage opc = null;
        SmartActorRendererCtrl myRendererCtrl = null;
        bool faild = false;
        pendingDownload = Create(actorInfo, delegate(OtherEntourage _opc, EResult _result)
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
        while (opc == null || opc.inited == false)
        {
            if (faild) yield break;
            yield return null;
        }

        pendingDownload = null;
        isDownloading_ = false;

        if (!actorInfo.IsAlive)
        {
            opc.Dead(true);
        }
        if (_callback != null)
        {
            _callback();
        }
    }


    protected AssetMng.DownloadID Create(MercenaryInfo _info,
                                    System.Action<OtherEntourage, EResult> _callback)
    {
        return exResources.GetEntourage(_info.AssetName,
                                        delegate(GameObject _asset, EResult _result)
                                        {
                                            if (GameCenter.IsDummyDestroyed(ObjectType.Entourage, _info.ServerInstanceID))
                                            {
                                                _callback(null, EResult.Failed);
                                                return;
                                            }
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
                                            newGo.AddComponent<SmartActorAnimFSM>();
                                            newGo.AddComponent<SmartActorRendererCtrl>();
                                            newGo.transform.localScale *= actorInfo.ModelScale;
                                            newGo.SetActive(false);
                                            newGo.SetActive(true);
                                            this.gameObject.AddComponent<FXCtrl>();


                                            isDummy_ = false;
                                            animationRoot = newGo.transform;
                                            Init();

                                            _callback(this, _result);
                                        });
    }



    protected override void Init()
    {
        base.Init();
        gameObject.SetMaskLayer(LayerMask.NameToLayer("Entourage"));

        rendererCtrl.ResetOriginalLayer(LayerMask.NameToLayer("Entourage"));

        rendererCtrl.ResetLayer();
        if (mouseCollider == null) mouseCollider = gameObject.GetComponent<BoxCollider>();

        if (!actorInfo.IsActor) //过场动画演员不展示名字
        {
            headTextCtrl.SetPetName(actorInfo.NoColorName);
            headTextCtrl.SetPetNameColor(actorInfo.PetNameColor);
            headTextCtrl.SetPetOwnerName(actorInfo.NoColorOwnerName);
            if (actorInfo.PetTitleName != string.Empty)
            {
                headTextCtrl.SetTitleSprite(actorInfo.PetTitleName);
            }
            InitNameColor();
        }
        

        inited_ = true;

        if (animFSM != null)
        {
            InitAnimation();
        }
        stateMachine.Start();
        fxCtrl.DoShadowEffect(true);
    }


    protected override void InitAnimation()
    {
        animFSM.SetupIdleAndMoveAnimationName("idle2", "move2");
        animFSM.SetupCombatAnimationName("idle2", null);
        animFSM.SetRandIdleAnimationName("waiting", null);
        animFSM.SetupInjuryAnimation("kickdown", null, false);
        animFSM.SetupInjuryUpAnimation("getup", null, false);
        animFSM.SetupDeathAnimation("striked", null);
        animFSM.fxCtrlRef = fxCtrl;
        animFSM.headTextCtrlRef = headTextCtrl;
        animFSM.moveFSMRef = moveFSM;
        CityStage city = GameCenter.curGameStage as CityStage;
        animFSM.SetInCombat(city == null);
        animFSM.StartStateMachine();
    }

    protected override void Regist()
    {
        base.Regist();
        GameCenter.teamMng.onUpdateNameColorEvent += UpdateNameColor;
        actorInfo.OnPetNameUpdate += UpdateName;
        actorInfo.OnPetAptitudeUpdate += UpdateNameColor;
    }
    public override void UnRegist()
    {
        base.UnRegist();
        GameCenter.teamMng.onUpdateNameColorEvent -= UpdateNameColor;
        actorInfo.OnPetNameUpdate -= UpdateName;
        actorInfo.OnPetAptitudeUpdate -= UpdateNameColor;
    }
    /// <summary>
    /// 初始化其他玩家的名字颜色 
    /// </summary>
    protected void InitNameColor()
    {
        if (GameCenter.teamMng == null || GameCenter.teamMng.TeammatesDic.Count == 0)
            return;
        if (Owner != null)
        {
            if (GameCenter.teamMng.TeammatesDic.ContainsKey(Owner.id))
                headTextCtrl.SetNameColor(Owner.id, true);
        }
    }

    /// <summary>
    /// 更新名字 
    /// </summary>
    public void UpdateNameColor(int _pid, bool _isTeammate) 
    {
        if (this == null)
        {
            return;
        }
        if (Owner != null && Owner.id == _pid)
        {
            if (headTextCtrl == null) headTextCtrl = this.gameObject.AddComponent<HeadTextCtrl>();
            headTextCtrl.SetNameColor(id, _isTeammate);
        }
    } 
    /// <summary>
    /// 更新宠物名字 
    /// </summary>
    public void UpdateName(string _name)
    {
        if (headTextCtrl == null) headTextCtrl = this.gameObject.AddComponent<HeadTextCtrl>();
        headTextCtrl.SetPetName(_name);
    }
    public void UpdateNameColor(Color _color)
    {
        if (headTextCtrl == null) headTextCtrl = this.gameObject.AddComponent<HeadTextCtrl>();
        headTextCtrl.SetPetNameColor(_color);
    } 

    protected new void Awake()
    {
        typeID = ObjectType.Player;
        base.Awake();
    }

    /// <summary>
    /// 勿删，Start（）实际在dummy状态被调用，要在启动时做的事情请去自定义的Init()接口 by吴江
    /// </summary>
    void Start()
    {
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



}