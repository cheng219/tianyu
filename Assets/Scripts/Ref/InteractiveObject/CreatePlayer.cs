//============================================
//作者：吴江
//日期：2016/2/3
//用途：创建,待选角色的表现层对象
//==============================================


using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// 创建,待选角色的表现层对象 by吴江
/// </summary>
public class CreatePlayer : PlayerBase
{

	/// <summary>
	/// 动画控制器
	/// </summary>
	[System.NonSerialized]
	protected new CreatePlayerAnimFSM animFSM = null;

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
    /// 创建净数据对象
    /// </summary>
    /// <param name="_id"></param>
    /// <returns></returns>
    public static CreatePlayer CreateDummy(PlayerBaseInfo _info)
    {
        GameObject newGO = null;
        if (GameCenter.instance.dummyOpcPrefab != null)
        {
            newGO = Instantiate(GameCenter.instance.dummyOpcPrefab) as GameObject;
            newGO.name = "Dummy CPC [" + _info.ServerInstanceID + "]";
        }
        else
        {
            newGO = new GameObject("Dummy CPC[" + _info.ServerInstanceID + "]");
        }
        newGO.tag = "Player";
        newGO.SetMaskLayer(LayerMask.NameToLayer("Player"));
        CreatePlayer newOPC = newGO.AddComponent<CreatePlayer>();
        newOPC.isDummy_ = true;
       // newOPC.moveFSM = newGO.AddComponent<ActorMoveFSM>();
        newOPC.id = _info.ServerInstanceID;
        newOPC.actorInfo = _info;
     //   newOPC.curMoveSpeed = newOPC.actorInfo.StaticSpeed * MOVE_SPEED_BASE;
      //  newOPC.CurRealSpeed = newOPC.curMoveSpeed;
      //  newOPC.RegistMoveEvent(true);
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

        CreatePlayer opc = null;
        PlayerRendererCtrl myRendererCtrl = null;
        bool faild = false;
        pendingDownload = Create(actorInfo, delegate(CreatePlayer _opc, EResult _result)
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


    protected AssetMng.DownloadID Create(PlayerBaseInfo _info,
                                    System.Action<CreatePlayer, EResult> _callback)
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
                                            this.gameObject.name = "Login Player [" + _info.ServerInstanceID + "]";

                                            GameObject newGo = Instantiate(_asset) as GameObject;
                                            newGo.name = _asset.name;
                                            newGo.transform.parent = this.gameObject.transform;
                                            newGo.transform.localEulerAngles = Vector3.zero;
                                            newGo.transform.localPosition = Vector3.zero;
											newGo.AddComponent<CreatePlayerAnimFSM>();
                                            newGo.AddComponent<PlayerRendererCtrl>();
                                            newGo.SetActive(false);
                                            newGo.SetActive(true);
                                            this.gameObject.AddComponent<FXCtrl>();


                                            isDummy_ = false;
                                            animationRoot = newGo.transform;
                                            Init();

                                            _callback(this, _result);
										},true);
    }



    protected override void Init()
    {
        base.Init();
        gameObject.SetMaskLayer(LayerMask.NameToLayer("Player"));
		if(rendererCtrl != null)
		{
			rendererCtrl.ResetOriginalLayer(LayerMask.NameToLayer("Player"));
			rendererCtrl.ResetLayer();
			rendererCtrl.ResetShaders();
		}
        
        if (mouseCollider == null) mouseCollider = gameObject.GetComponent<BoxCollider>();


        inited_ = true;
		animFSM = base.animFSM as CreatePlayerAnimFSM;
        if (animFSM != null)
        {
            InitAnimation();
        }
		SetInCombat(true);
        stateMachine.Start();
    }

	public void UseAbility (AbilityInstance _instance,System.Action _shake,System.Action _finish)
	{
		if(animFSM != null)
		{
            animFSM.DoPreviewAnim(_instance, fxCtrl, _shake,_finish);
		}
	}


    protected override void InitAnimation()
    {
        base.InitAnimation();
        if (animFSM != null)
        {
			animFSM.SetupIdleAndMoveAnimationName("idle2", "move1");
			animFSM.SetupCombatAnimationName("idle2", null);
			SetInfoInCombat(true);
            animFSM.IdleImmediate();
        }
    }





}