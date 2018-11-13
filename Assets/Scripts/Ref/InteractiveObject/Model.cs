//============================================
//作者：吴江
//日期：2016/2/23
//用途：【雕像】的表现层对象
//==============================================


using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// 【雕像】的表现层对象 by吴江
/// </summary>
public class Model : PlayerBase
{

    protected float scaleValue = 3.0f;

    public new OtherPlayerInfo actorInfo
    {
        get { return base.actorInfo as OtherPlayerInfo; }
        set
        {
            base.actorInfo = value;
        }
    }
    /// <summary>
    /// 创建净数据对象
    /// </summary>
    /// <param name="_id"></param>
    /// <returns></returns>
    public static Model CreateDummy(OtherPlayerInfo _info)
    {
        GameObject newGO = null;
        if (GameCenter.instance.dummyOpcPrefab != null)
        {
            newGO = Instantiate(GameCenter.instance.dummyOpcPrefab) as GameObject;
            newGO.name = "Dummy Model [" + _info.ServerInstanceID + "]";
        }
        else
        {
            newGO = new GameObject("Dummy Model[" + _info.ServerInstanceID + "]");
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
        Model newOPC = newGO.AddComponent<Model>();
        newOPC.isDummy_ = true;
        newOPC.id = _info.ServerInstanceID;
        newOPC.actorInfo = _info;
        newOPC.typeID = ObjectType.Model;
        newOPC.transform.localRotation = new Quaternion(0, _info.RotationY, 0, 0);
        GameCenter.curGameStage.PlaceGameObjectFromServer(newOPC, _info.ServerPos.x, _info.ServerPos.z, (int)newOPC.transform.localEulerAngles.y);
        GameCenter.curGameStage.AddObject(newOPC);
        if (!newOPC.actorInfo.IsActor) //过场动画演员不展示名字
        {
            newOPC.height = newOPC.actorInfo.NameHeight;
            newOPC.nameHeight = newOPC.height * newOPC.scaleValue;
            newOPC.headTextCtrl = newOPC.gameObject.AddComponent<HeadTextCtrl>();
            newOPC.headTextCtrl.SetName(newOPC.actorInfo.Name);
            newOPC.headTextCtrl.SetTitle(newOPC.actorInfo.TitleName);
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

        Model opc = null;
        PlayerRendererCtrl myRendererCtrl = null;
        bool faild = false;
        pendingDownload = Create(actorInfo, delegate(Model _opc, EResult _result)
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


    protected AssetMng.DownloadID Create(ActorInfo _info,
                                    System.Action<Model, EResult> _callback)
    {
        return exResources.GetRace(_info.Prof,
                                        delegate(GameObject _asset, EResult _result)
                                        {
                                            if (GameCenter.IsDummyDestroyed(ObjectType.Model, _info.ServerInstanceID))
                                            {
                                                _callback(null, EResult.Failed);
                                                return;
                                            }
                                            if (_result != EResult.Success)
                                            {
                                                _callback(null, _result);
                                                return;
                                            }
                                            this.gameObject.name = "Model [" + _info.ServerInstanceID + "]";

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
        SetInfoInCombat(true);
        SetInCombat(true);
        rendererCtrl.ResetLayer();
        rendererCtrl.OnCombineSuceess = OnCombineSucess;
        animationRoot.gameObject.transform.localScale *= scaleValue;
        height = actorInfo.NameHeight;
        nameHeight = height * scaleValue;
        ActiveBoxCollider(true, actorInfo.ColliderRadius * scaleValue);
        if (mouseCollider == null) mouseCollider = gameObject.GetComponent<BoxCollider>();

        if (!actorInfo.IsActor) //过场动画演员不展示名字
        {
            headTextCtrl.SetName(actorInfo.Name);
            headTextCtrl.SetTitle(actorInfo.TitleName);
        }


        inited_ = true;

        if (animFSM != null)
        {
            InitAnimation();
            SetInfoInCombat(true);
            SetInCombat(true);
            animFSM.StaticIdleImmediate();
        }
        stateMachine.Start();
        fxCtrl.DoShadowEffect(true);
    }


    protected void OnCombineSucess()
    {
        if (rendererCtrl != null)
        {
            rendererCtrl.Fossil(fossilColor);
        }
    }
    protected override void OnUpdateRenderQuality(SystemSettingMng.RendererQuality _quality)
    {
    }

    protected Color fossilColor = new Color(0.62f, 0.43f, 0.07f, 1);

    public override void Show(bool _show)
    {
        isShowing = _show;
        if (!_show)
        {
            if (GameCenter.curMainPlayer != null && GameCenter.curMainPlayer.CurTarget == this)
            {
                GameCenter.curMainPlayer.CurTarget = null;
            }
        }
        if (rendererCtrl != null)
        {
            rendererCtrl.Show(_show, true);
        }
        if (fxCtrl != null)
        {
            fxCtrl.DoShadowEffect(_show);
            fxCtrl.ShowBoneEffect(_show);
        }
        if (headTextCtrl != null) headTextCtrl.SetFlagsActive(_show);
        if (moveFSM != null)
        {
            moveFSM.IsDummy = !_show;
        }
        if (mouseCollider != null) mouseCollider.enabled = _show;
        if (animFSM != null)
        {
            animFSM.StaticIdleImmediate();
        }
    }


    protected new void Awake()
    {
        typeID = ObjectType.Model;
        base.Awake();
    }

    void OnEnable()
    {
        if (animFSM != null)
        {
            animFSM.StaticIdleImmediate();
        }
    }
    /// <summary>
    /// 勿删，Start（）实际在dummy状态被调用，要在启动时做的事情请去自定义的Init()接口 by吴江
    /// </summary>
    void Start()
    {
    }


}