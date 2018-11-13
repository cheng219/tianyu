//============================================
//作者：吴江
//日期：2015/7/7
//用途：【其他玩家】的表现层对象
//==============================================


using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// 【其他玩家】的表现层对象 by吴江
/// </summary>
public class OtherPlayer : PlayerBase
{
    public new OtherPlayerInfo actorInfo
    {
        get { return base.actorInfo as OtherPlayerInfo; }
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
    public static OtherPlayer CreateDummy(OtherPlayerInfo _info)
    {
        GameObject newGO = null;
        if (GameCenter.instance.dummyOpcPrefab != null)
        {
            newGO = Instantiate(GameCenter.instance.dummyOpcPrefab) as GameObject;
            newGO.name = "Dummy OPC [" + _info.ServerInstanceID + "]";
        }
        else
        {
            newGO = new GameObject("Dummy OPC[" + _info.ServerInstanceID + "]");
        }
        newGO.tag = "Player";
        if (_info.IsActor)
        {
            newGO.SetMaskLayer(LayerMask.NameToLayer("CGPlayer"));
        }
        else
        {
            newGO.SetMaskLayer(LayerMask.NameToLayer("OtherPlayer"));
        }
        OtherPlayer newOPC = newGO.AddComponent<OtherPlayer>();
        newOPC.isDummy_ = true;
        newOPC.moveFSM = newGO.AddComponent<ActorMoveFSM>();
        newOPC.id = _info.ServerInstanceID;
        newOPC.actorInfo = _info;
        
		if(_info.Movespd != 0)
		{
			newOPC.curMoveSpeed = (float)newOPC.actorInfo.StaticSpeed*(float)_info.Movespd/100f;//由c002获得
		}else
		{
			newOPC.curMoveSpeed = newOPC.actorInfo.StaticSpeed * MOVE_SPEED_BASE;
		}
        //Debug.Log("_info.Movespd:" + _info.Movespd + ",newOPC.actorInfo.StaticSpeed:" + newOPC.actorInfo.StaticSpeed);
        newOPC.CurRealSpeed = newOPC.curMoveSpeed;
        newOPC.RegistMoveEvent(true);
        newOPC.transform.localRotation = new Quaternion(0, _info.RotationY, 0, 0);
        GameCenter.curGameStage.PlaceGameObjectFromServer(newOPC, _info.ServerPos.x, _info.ServerPos.z, (int)newOPC.transform.localEulerAngles.y);
        GameCenter.curGameStage.AddObject(newOPC);
        if (!newOPC.actorInfo.IsActor) //过场动画演员不展示名字
        {
            newOPC.height = newOPC.actorInfo.NameHeight;
            newOPC.nameHeight = newOPC.height;
            newOPC.headTextCtrl = newOPC.gameObject.AddComponent<HeadTextCtrl>(); 
            newOPC.headTextCtrl.SetRelationship(newOPC.GetRelationship());
            newOPC.headTextCtrl.SetRelationColor(newOPC.GetRelationshipColor()); 
            newOPC.headTextCtrl.SetName("[b]" + (newOPC.actorInfo.Name));
            newOPC.headTextCtrl.SetGuildName(newOPC.actorInfo.GuildName);
            newOPC.headTextCtrl.SetTitleSprite(newOPC.actorInfo.TitleIcon);
            newOPC.InitNameColor();
        }
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
            GameSys.LogError("You can only start create other player in dummy: " + actorInfo.ServerInstanceID);
            yield break;
        }
        //
        isDownloading_ = true;				//判断是否正在下载，防止重复创建

        OtherPlayer opc = null;
        PlayerRendererCtrl myRendererCtrl = null;
        bool faild = false;
        pendingDownload = Create(actorInfo, delegate(OtherPlayer _opc, EResult _result)
        {

            if (_result != EResult.Success)
            {
                faild = true;
                return;
            }

            opc = _opc;
            pendingDownload = null;
            myRendererCtrl = opc.gameObject.GetComponentInChildrenFast<PlayerRendererCtrl>();
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


    protected AssetMng.DownloadID Create(OtherPlayerInfo _info,
                                    System.Action<OtherPlayer, EResult> _callback)
    {
        return exResources.GetRace(_info.Prof,
                                        delegate(GameObject _asset, EResult _result)
                                        {
                                            if (GameCenter.IsDummyDestroyed(ObjectType.Player, _info.ServerInstanceID))
                                            {
                                                _callback(null, EResult.Failed);
                                                return;
                                            }
                                            if (_result != EResult.Success)
                                            {
                                                _callback(null, _result);
                                                return;
                                            }
                                            this.gameObject.name = "Other Player [" + _info.ServerInstanceID + "]";

                                            GameObject newGo = Instantiate(_asset) as GameObject;
                                            newGo.name = _asset.name;
                                            newGo.transform.parent = this.gameObject.transform;
                                            newGo.transform.localEulerAngles = Vector3.zero;
                                            newGo.transform.localPosition = Vector3.zero;
                                            newGo.AddComponent<PlayerAnimFSM>();
                                            newGo.AddComponent<PlayerRendererCtrl>();
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
        InitSpeed();
        if (actorInfo.IsActor)
        {
            gameObject.SetMaskLayer(LayerMask.NameToLayer("CGPlayer"));
        }
        else
        {
            gameObject.SetMaskLayer(LayerMask.NameToLayer("OtherPlayer"));
        }

        if (actorInfo.IsActor)
        {
            rendererCtrl.ResetOriginalLayer(LayerMask.NameToLayer("CGPlayer"));
        }
        else
        {
            rendererCtrl.ResetOriginalLayer(LayerMask.NameToLayer("OtherPlayer"));
        }

        rendererCtrl.ResetLayer();
        if (mouseCollider == null) mouseCollider = gameObject.GetComponent<BoxCollider>();
        ActiveBoxCollider(isShowing, actorInfo.ColliderRadius);//隐藏的玩家不激活碰撞器 dy邓成
        if (!actorInfo.IsActor) //过场动画演员不展示名字
        {
            string str = GetRelationship(); 
            headTextCtrl.SetRelationship(str);
            headTextCtrl.SetRelationColor(GetRelationshipColor());
            headTextCtrl.SetName("[b]" + actorInfo.Name);
            headTextCtrl.SetGuildName(actorInfo.GuildName); 
			UpdateItemName ();
            InitNameColor();
        }


        inited_ = true;

        if (animFSM != null)
        {
            InitAnimation();
        }
        stateMachine.Start();
        fxCtrl.DoShadowEffect(true);
        if(fxCtrl!=null)
            fxCtrl.SetHide(!GameCenter.systemSettingMng.OtherPlayerSkill);
        PlayOtherStrenEffect();
    }

    /// <summary>
    /// 初始化一下速度,其他玩家下线再上,后台先发的初始速度,然后再更新属性改成如:137,但是此时还未Regist事件(actorInfo.OnPropertyUpdate += UpdateSpeed)
    /// 所以再此初始化一下
    /// </summary>
    void InitSpeed()
    {
        if (actorInfo.Movespd != 0)
        {
            CurRealSpeed = (float)actorInfo.StaticSpeed * (float)actorInfo.Movespd / 100f;
        }
    }

    /// <summary>
    /// 其他玩家强化特效
    /// </summary>
    public void PlayOtherStrenEffect()
    {
        if (fxCtrl != null)
        {
            if (actorInfo.SmallStrenLev != 0)
            {
                GameCenter.inventoryMng.PlayEquStrengEffectName(actorInfo.SmallStrenLev, fxCtrl);
            }
            else
                fxCtrl.ClearStrengthEffect();
        }
    }

    /// <summary>
    /// 更新其他玩家称号
    /// </summary>
   public  void UpdateItemName()
    {
		if (!GameCenter.systemSettingMng.OtherPlayerTitle) {
            headTextCtrl.SetTitleSprite(string.Empty);
		} else {
            headTextCtrl.SetTitleSprite(actorInfo.TitleIcon);
		}
    }

    /// <summary>
    /// 初始化其他玩家的名字颜色 
    /// </summary>
    protected void InitNameColor()
    {
        //if (GameCenter.teamMng == null || GameCenter.teamMng.TeammatesDic.Count == 0)
        //    return;
        //if (GameCenter.teamMng.TeammatesDic.ContainsKey(id))
        //    headTextCtrl.SetNameColor(id, true);
    }

    /// <summary>
    /// 更新其他玩家与我的关系分仇人-1（红色）和队友1（蓝色）  无关系0
    /// </summary>
    protected string GetRelationship()
    {
        if (GameCenter.teamMng != null && GameCenter.teamMng.TeammatesDic.Count > 0)
        { 
            if (GameCenter.teamMng.TeammatesDic.ContainsKey(id))
                return "队友";
        }
        if (GameCenter.friendsMng != null && GameCenter.friendsMng.allFriendDic.ContainsKey(4))
        {
            if (GameCenter.friendsMng.allFriendDic[4].ContainsKey(id))
                return "仇人";
        }
        return string.Empty;
    }
    protected Color GetRelationshipColor()
    {
        if (GameCenter.teamMng != null && GameCenter.teamMng.TeammatesDic.Count > 0)
        {
            if (GameCenter.teamMng.TeammatesDic.ContainsKey(id))
            {
                Color col = new Color(79/225, 86/255 , 229/255 , 255/255);
                return col;
            }
        }
        if (GameCenter.friendsMng != null && GameCenter.friendsMng.allFriendDic.ContainsKey(4))
        {
            if (GameCenter.friendsMng.allFriendDic[4].ContainsKey(id))
            {
                Color col = new Color(1 / 225, 141 / 255, 141 / 255, 255 / 255);
                return col;
            }
        }
        return Color.white;
    }

    /// <summary>
    /// 更新他人与我的关系
    /// </summary>
    public void UpdateRelationship()
    { 
        if (this == null)
        {
            return;
        }
        if (headTextCtrl == null) headTextCtrl = this.gameObject.AddComponent<HeadTextCtrl>();
        headTextCtrl.SetRelationship(GetRelationship());
        headTextCtrl.SetRelationColor(GetRelationshipColor());
    }

    /// <summary>
    /// 更新队友名字 
    /// </summary>
    public void UpdateNameColor(int _pid, bool _isTeammate)
    {
        if (this == null)
        {
            return;
        }
        if (headTextCtrl == null) headTextCtrl = this.gameObject.AddComponent<HeadTextCtrl>();
        headTextCtrl.SetNameColor(_pid, _isTeammate);
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


    protected override void Regist()
    {
        base.Regist();
        actorInfo.OnJump += Jump;
        actorInfo.OnEquipStrengEffectUpdate += PlayOtherStrenEffect;
        if (GameCenter.teamMng != null)
        { 
            GameCenter.teamMng.onTeammateUpdateEvent += UpdateRelationship; 
        }
        if (GameCenter.friendsMng != null)
        {
            GameCenter.friendsMng.OnEnemyDicUpdata += UpdateRelationship;
        }
    }

    public override void UnRegist()
    {
        base.UnRegist();
        actorInfo.OnJump -= Jump;
        actorInfo.OnEquipStrengEffectUpdate -= PlayOtherStrenEffect;
        if (GameCenter.teamMng != null)
        { 
            GameCenter.teamMng.onTeammateUpdateEvent -= UpdateRelationship; 
        }
        if (GameCenter.friendsMng != null)
        {
            GameCenter.friendsMng.OnEnemyDicUpdata -= UpdateRelationship;
        }
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



    /// <summary>
    /// 重建模型，一般是因为转职
    /// </summary>
    protected override void ReStartAsyncCreate(int _prof)
    {
        base.ReStartAsyncCreate(_prof);
        StartAsyncCreate(null);
    }

}