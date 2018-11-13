//=======================================
//作者:吴江
//日期:2015/6/9
//用途：用作预览的玩家对象
//=======================================



using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PreviewPlayer : PlayerBase
{
    /// <summary>
    /// 是否为互斥对象 by吴江
    /// </summary>
    public bool mutualExclusion = true;
    [System.NonSerialized]
    public int positionTag = -1;
    [System.NonSerialized]
    public int inventoryTag = 0;
    [System.NonSerialized]
    public int vipFlags = 0;
    /// <summary>
    /// 创建完毕后是否做稍息动作
    /// </summary>
    public PlayerAnimFSM.EventType needRandID = ActorAnimFSM.EventType.Idle;
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

    private int copyFromID_ = -1;
    public int copyFromID { get { return copyFromID_; } }
    public bool showCosmetic = true;
    public string idleAnimName = string.Empty;

    /// <summary>
    /// 渲染控制器
    /// </summary>
    [System.NonSerialized]
    public new PreviewPlayerRendererCtrl rendererCtrl = null;
    /// <summary>
    /// 当前展示的全套装备数据
    /// </summary>
    protected Dictionary<EquipSlot, EquipmentInfo> curShowEquipments = new Dictionary<EquipSlot, EquipmentInfo>();
    /// <summary>
    /// 试穿的装备数据 （这份数据和人物原本的装备信息共同构成了 curShowEquipments）
    /// </summary>
    protected Dictionary<EquipSlot, EquipmentInfo> tryShowEquipments = null;

    //Dictionary<EquipSlot, EquipmentInfo> nowShowEqList = new Dictionary<EquipSlot, EquipmentInfo>();

    public static PreviewPlayer CreateDummy(PlayerBaseInfo _info,Dictionary<EquipSlot,EquipmentInfo> _tryShowList = null)
    {
        GameObject newGO = new GameObject("Dummy Preview[" + _info.ServerInstanceID + "]");
        newGO.AddComponent<ActorMoveFSM>();
        PreviewPlayer pP = newGO.AddComponent<PreviewPlayer>();
        pP.actorInfo = _info;
        pP.id = _info.ServerInstanceID;
        pP.isDummy_ = true;
        pP.tryShowEquipments = _tryShowList;
        pP.typeID = ObjectType.PreviewPlayer;
        return pP;
    }

    /// <summary>
    /// 预览时的坐标
    /// </summary>
    public Vector3 PreviewPos
    {
        get
        {
            return actorInfo.PreviewPos;
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


    #region 创建游戏内预览人物


    public void StartAsyncCreate(Action<PreviewPlayer> _callBack)
    {
        StartCoroutine(CreateAsync(_callBack));
    }

    IEnumerator CreateAsync(Action<PreviewPlayer> _callBack)
    {
        if (isDummy_ == false)
        {
            GameSys.LogError("You can only start create preview player in dummy: " + actorInfo.ServerInstanceID);
            yield break;
        }
        //
        isDownloading_ = true;				//判断是否正在下载，防止重复创建

        PreviewPlayer pp = null;
        //PlayerRendererCtrl myRendererCtrl = null;
        //bool faild = false;
        pendingDownload = Create(actorInfo, delegate(PreviewPlayer _pp, EResult _result)
        {
            if (_result != EResult.Success)
            {
                //faild = true;
                return;
            }
            if (mutualExclusion)
            {
                GameCenter.previewManager.EndDownLoadTask(pendingDownload);
            }
            pp = _pp;
            pp.transform.localScale *= actorInfo.PreviewScale;
            pendingDownload = null;
            if (pp.inited)
            {
                pendingDownload = null;
                isDownloading_ = false;
                if (_callBack != null)
                {
                    _callBack(pp);
                }
            }


        });
        if (mutualExclusion)
        {
            GameCenter.previewManager.PushDownLoadTask(pendingDownload);
        }
        GameCenter.previewManager.PushDownLoadTask(pendingDownload);
        //while (pp == null || pp.inited == false)
        //{
        //    if (faild) yield break;
        //    Debug.Log("1111");
        //    yield return new WaitForFixedUpdate();
        //}

        //pendingDownload = null;
        //isDownloading_ = false;
        //Debug.Log("3333333");
        //if (_callBack != null)
        //{
        //    Debug.Log("44444444");
        //    _callBack(pp);
        //}

    }


    protected AssetMng.DownloadID Create(PlayerBaseInfo _info,
                                    System.Action<PreviewPlayer, EResult> _callback)
    {
        return exResources.GetRace(_info.Prof,
                                        delegate(GameObject _asset, EResult _result)
                                        {
                                            if (_result != EResult.Success)
                                            {
                                                _callback(null, _result);
                                                return;
                                            }

                                            this.gameObject.name = "Preview Player [" + _info.ServerInstanceID + "]";

                                            GameObject newGo = Instantiate(_asset) as GameObject;
                                            newGo.name = _asset.name;
                                            newGo.transform.parent = this.gameObject.transform;
                                            newGo.transform.localEulerAngles = Vector3.zero;
                                            newGo.transform.localPosition = Vector3.zero;
                                            newGo.AddComponent<PlayerAnimFSM>();
                                            newGo.AddComponent<PreviewPlayerRendererCtrl>();
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
        typeID = ObjectType.PreviewPlayer;
        base.Init();
        rendererCtrl = base.rendererCtrl as PreviewPlayerRendererCtrl;
        if (!showCosmetic || (tryShowEquipments != null && tryShowEquipments.Count > 0))
        {
            UpdateAllModel();
        }
        gameObject.SetMaskLayer(LayerMask.NameToLayer("Preview"));
        inited_ = true;

        if (animFSM != null)
        {
            InitAnimation();
        }
		SetInCombat(true);
        PlayStrenEffect();
        stateMachine.Start();
        if (headTextCtrl != null) Destroy(this.headTextCtrl);//预览的时候删除头顶上的东西 addby zsy
    }
    protected override void Regist()
    {
        base.Regist();
        GameCenter.inventoryMng.OnEquipUpdate += PlayStrenEffect;
    }
    public override void UnRegist()
    {
        base.UnRegist();
        GameCenter.inventoryMng.OnEquipUpdate -= PlayStrenEffect;
    }
    protected void PlayStrenEffect()
    {
        //预览玩家是主玩家
        if (actorInfo.ServerInstanceID == GameCenter.mainPlayerMng.MainPlayerInfo.ServerInstanceID)
        {
            if (fxCtrl != null)
                GameCenter.inventoryMng.PlayEquStrengEffectName(GameCenter.inventoryMng.GetPlayerEquList(), fxCtrl);
        }
        else//预览玩家是其他玩家
        {
            if (fxCtrl != null)
                GameCenter.inventoryMng.PlayEquStrengEffectName(actorInfo.RealEquipmentInfoList, fxCtrl);
        }
    }

    protected void EquipAll()
    {
        curShowEquipments.Clear();
        if (showCosmetic)
        {
            foreach (var item in actorInfo.CurShowDictionary)
            {
                curShowEquipments[item.Key] = item.Value;
            }
        }
        else
        {
            foreach (var item in actorInfo.DefaultDictionary)
            {
                curShowEquipments[item.Key] = item.Value;
            }
            foreach (var item in actorInfo.EquipmentDictionary)
            {
                if (item.Value != null)
                {
                    curShowEquipments[item.Key] = item.Value;
                }
            }
        }
        if (tryShowEquipments != null)
        {
            foreach (var item in tryShowEquipments)
            {
                if (item.Value != null)
                {
                    curShowEquipments[item.Key] = item.Value;
					if(rendererCtrl != null)rendererCtrl.SetCurTryShowEquip(item.Value.Slot,item.Value);
                }
            }
        }
        foreach (var item in curShowEquipments)
        {
            if (item.Value == null) continue;
            rendererCtrl.Equip(item.Key, item.Value);
        }
    }



    //根据数据更新所有外形模型
    private void UpdateAllModel()
    {
        if (isDummy_) return;
        EquipAll();
        gameObject.SetMaskLayer(LayerMask.NameToLayer("Preview"));
    }



    /// <summary>
    /// 勿删，Start（）实际在dummy状态被调用，要在启动时做的事情请去自定义的Init()接口 by吴江
    /// </summary>
    void Start()
    {

    }



    protected override void InitAnimation()
    {
        animFSM.SetupIdleAndMoveAnimationName(idleAnimName == string.Empty?"idle2":idleAnimName, "move2");
        animFSM.SetupCombatAnimationName(idleAnimName == string.Empty ? "idle2" : idleAnimName, null);
       // animFSM.SetRandIdleAnimationName("idle2", null);
        if (needRandID == ActorAnimFSM.EventType.Idle)
        {
            animFSM.IdleImmediate();
            animFSM.StartStateMachine();
        }
        else
        {
            animFSM.StartStateMachine();
            animFSM.GoState(needRandID);
        }
    }



    #endregion

    #region 更新游戏内预览人物

    /// <summary>
    /// 试穿某一件装备
    /// </summary>
    /// <param name="_eq"></param>
    public void PreviewPcTryShowEq(EquipmentInfo _eq)
    {
        if (_eq == null || !_eq.WillChangeRender) return;
        if (tryShowEquipments == null) tryShowEquipments = new Dictionary<EquipSlot, EquipmentInfo>();
        tryShowEquipments[_eq.Slot] = _eq;
        UpdateAllModel();
    }
    /// <summary>
    /// 清理所有的试穿装备，还原本身面貌
    /// </summary>
    public void CleanTryShowEq()
    {
        if (tryShowEquipments == null || tryShowEquipments.Count == 0) return;
        tryShowEquipments.Clear();
        UpdateAllModel();
    }


    /// <summary>
    /// 重建模型，一般是因为转职
    /// </summary>
    protected override void ReStartAsyncCreate(int _prof)
    {
        StartAsyncCreate(null);
    }
    #endregion

    protected new void OnDestroy()
    {
        base.OnDestroy();
        if (isDummy_)
        {
            AssetMng.instance.CancelDownload(pendingDownload);
        }
    }





}
