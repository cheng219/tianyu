//=====================================
//作者:吴江
//日期:2016/1/28
//用途:坐骑表现层对象类
//======================================



using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 坐骑表现层对象类 by吴江
/// </summary>
public class Mount : Actor {

    /// <summary>
    /// 特效控制器
    /// </summary>
    //[System.NonSerialized]
    //public FXCtrl fxCtrl = null;
    /// <summary>
    /// 渲染控制器
    /// </summary>
    [System.NonSerialized]
    public new MountRendererCtrl rendererCtrl = null;
    /// <summary>
    /// 完整数据层对象的引用 by吴江
    /// </summary>
    protected new MountInfo actorInfo
    {
        get { return base.actorInfo as MountInfo; }
        set
        {
            base.actorInfo = value;
        }
    }


    public int ConfigID
    {
        get
        {
            return actorInfo.ConfigID;
        }
    }

    protected PlayerBase owner = null;
    public PlayerBase Owner
    {
        get
        {
            if (owner == null)
            {
                if (GameCenter.curMainPlayer != null && actorInfo.OwnerID == GameCenter.curMainPlayer.id)
                {
                    owner = GameCenter.curMainPlayer;
                }
                else
                {
                    owner = GameCenter.curGameStage.GetOtherPlayer(actorInfo.OwnerID);
                }
            }
            return owner;
        }
    }

    protected Transform animRoot;
    protected Transform tireFront;
    protected Transform tireBack;


    /// <summary>
    /// 创建净数据对象 by吴江
    /// </summary>
    /// <param name="_info"></param>
    /// <returns></returns>
    public static Mount CreateDummy(MountInfo _info)
    {
        if (_info == null) return null;
        GameObject newGO = null;
        if (GameCenter.instance.dummyMobPrefab != null)
        {
            newGO = Instantiate(GameCenter.instance.dummyMobPrefab) as GameObject;
            newGO.name = "Dummy Mount [" + _info.ConfigID + "]";
        }
        else
        {
            newGO = new GameObject("Dummy Mount[" + _info.ConfigID + "]");
        }
        newGO.tag = "Player";
        newGO.SetMaskLayer(_info.OwnerID == GameCenter.mainPlayerMng.MainPlayerInfo.ServerInstanceID ? LayerMask.NameToLayer("Player") : LayerMask.NameToLayer("OtherPlayer"));
        Mount mount = newGO.AddComponent<Mount>();
        mount.isDummy_ = true;
        mount.actorInfo = _info;

        return mount;
    }


    //bool startCheck = false;

    //void OnGUI()
    //{
    //    if (GUI.Button(new Rect(100, 100, 100, 50), "Test"))
    //    {
    //        startCheck = !startCheck;
    //        Time.timeScale = 1;
    //    }
    //}

    protected new void LateUpdate()
    {
        base.LateUpdate();

        if (Owner == null || actorInfo.MoveRecastType != RecastType.DOUBLE) return;
        if (tireFront && tireBack)
        {
            Vector3 dir = transform.forward;
            dir.y = 0.0f;
            transform.forward = dir.normalized;

            Vector3 front = ActorMoveFSM.LineCast(tireFront.position,!Owner.isDummy);
            Vector3 back = ActorMoveFSM.LineCast(tireBack.position, !Owner.isDummy);
            Vector3 forward = (front - back).normalized;
            Vector3 forward2 = forward;
            forward2.y = 0.0f;
            forward2.Normalize();

           // Vector3 old = animRoot.localEulerAngles;
            float angle = Vector3.Angle(forward, forward2);
            animRoot.localEulerAngles = new Vector3(-1.0f * angle * Mathf.Sign(forward.y), 0.0f, 0.0f);
            //if (animRoot.localEulerAngles.x > 300 && startCheck)
            //{
            //    Debug.logger.Log("front = " + front + " , back = " + back + " , forward = " + forward);
            //    Debug.logger.Log(" animRoot.localEulerAngles = " + animRoot.localEulerAngles + " , Mathf.Abs(angle) = " + Mathf.Abs(angle));
            //    Time.timeScale = 0;
            //}

            //if (Mathf.Abs(angle) > 30.0f)
            //{
            //    animRoot.localEulerAngles = old;
            //}
            //if (owner)
            //{
            //    Vector3 myForward = animRoot.transform.forward;
            //    Vector3 destDir = owner.GetMoveFSM().destDir;
            //    destDir.y = forward.y;
            //    destDir.Normalize();
            //    owner.FaceToNoLerp(destDir);
            //    animRoot.localEulerAngles = Vector3.zero;

            //    if (doMount)
            //    {
            //        if (anchor)
            //        {
            //            owner.transform.parent = anchor.transform;
            //        }
            //        else
            //        {
            //            owner.transform.parent = animRoot.transform;
            //        }
            //        owner.transform.localPosition = Vector3.zero;
            //        owner.transform.localRotation = Quaternion.identity;
            //    }
            //}
        }
    }




    public void StartAsyncCreate(System.Action<Mount,EResult> _callback)
    {
        StartCoroutine(CreateAsync(_callback));
    }

    IEnumerator CreateAsync(System.Action<Mount, EResult> _callback)
    {
        if (isDummy_ == false)
        {
            GameSys.LogError("You can only start create Mob in dummy: " + actorInfo.ConfigID);
            yield break;
        }

        //
        Mount mount = null;
        MountRendererCtrl myRendererCtrl = null;
        bool failed = false;
        pendingDownload = Create(actorInfo, delegate(Mount _mount, EResult _result)
        {
            if (_result != EResult.Success)
            {
                failed = true;
                return;
            }
            mount = _mount;
            pendingDownload = null;
            myRendererCtrl = mount.gameObject.GetComponentInChildrenFast<MountRendererCtrl>();
            if (myRendererCtrl != null)
            {
                myRendererCtrl.Show(actorInfo.IsRiding,true);
            }

        });
        while (mount == null || mount.inited == false)
        {
            if (failed) yield break;
            yield return null;
        }
        pendingDownload = null;


        mount.isDownloading_ = false;
        if (_callback != null)
        {
            _callback(mount, failed ? EResult.Failed : EResult.Success);
        }
    }

    /// <summary>
    /// 获取坐骑起程挂点
    /// </summary>
    /// <param name="_obj"></param>
    /// <param name="_objName"></param>
    /// <returns></returns>
    public static GameObject AssPositionObj(GameObject _obj, string _objName)
    {
        if (_obj == null) return null;
        if (_obj.name == _objName) return _obj;
        int count = _obj.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            GameObject temp = AssPositionObj(_obj.transform.GetChild(i).gameObject,_objName);
            if (temp != null)
            {
                return temp;
            }
        }
        return null;
    }



    protected AssetMng.DownloadID Create(MountInfo _info, System.Action<Mount, EResult> _callback)
    {
        return exResources.GetMount(_info.AssetName, delegate(GameObject _asset, EResult _result)
        {
            if (_result != EResult.Success)
            {
                _callback(null, _result);
                return;
            }

            GameCenter.curGameStage.CacheEquipmentURL(_info.AssetURL, _asset);
            if (this == null ||this.gameObject == null) return;
            this.gameObject.name = "Mount[" + _info.ConfigID + "]";

            GameObject newGO = Instantiate(_asset) as GameObject;
            newGO.name = _asset.name;
            newGO.transform.parent = this.gameObject.transform;
            newGO.transform.localEulerAngles = Vector3.zero;
            newGO.transform.localPosition = Vector3.zero;
            newGO.transform.localScale = Vector3.one;

            if (actorInfo.MoveRecastType == RecastType.DOUBLE)
            {
                GameObject frontPoint = new GameObject("FrontPoint");
                frontPoint.transform.parent = newGO.transform;
                frontPoint.transform.localPosition = actorInfo.frontPoint;
                frontPoint.transform.localScale = Vector3.one;
                tireFront = frontPoint.transform;

                GameObject behindPoint = new GameObject("BehindPoint");
                behindPoint.transform.parent = newGO.transform;
                behindPoint.transform.localPosition = actorInfo.BehindPoint;
                behindPoint.transform.localScale = Vector3.one;
                tireBack = behindPoint.transform;
            }



            newGO.AddComponent<MountAnimFSM>();
            newGO.AddComponent<MountRendererCtrl>();
            animRoot = newGO.transform;

            FXCtrl fx = newGO.AddComponent<FXCtrl>();
            fx.SetUnLimit(Owner == GameCenter.curMainPlayer);
            fx.canShowShadow = false;



            isDummy_ = false;

            Init();
            _callback(this, _result);
        });
    }


    protected override void Init()
    {
        typeID = ObjectType.Mount;
        base.Init();
        this.id = actorInfo.ConfigID;
        if (animFSM != null)
        {
            InitAnimation();
            if (actorInfo.IsMoving)
            {
                animFSM.Move();
            }
            else
            {
				animFSM.StopMoving();
            }
        }
        rendererCtrl = base.rendererCtrl as MountRendererCtrl;
        if (rendererCtrl != null)
        {
            rendererCtrl.Show(actorInfo.IsRiding,true);
            rendererCtrl.Init(actorInfo);
        }
        OnEffectChangeEvent(0);
        inited_ = true;
    }

    protected new void OnDestroy()
    {
        base.OnDestroy();
        UnRegist();
    }


    void InitAnimation()
    {
        animFSM.SetupIdleAndMoveAnimationName("idle1", "move2");
        animFSM.StartStateMachine();
        if (actorInfo != null)
        {
           // animFSM.SetMoveSpeed(1.0f);//actorInfo.MoveRate);
        }
    }

    public virtual void OnMountStateUpate(bool _ride,bool _isMoving,bool _ownerShowing)
    {
        if (_ride)
        {
            if (_isMoving)
            {
				MoveStart();
            }
            else
            {
                MoveEnd();
            }
            Show(_ownerShowing);
        }
        else
        {
            MoveEnd();
            Show(false);
        }
    }



    /// <summary>
    /// 展示/隐藏基本模型
    /// </summary>
    /// <param name="_show"></param>
    public override void Show(bool _show)
    {
        base.Show(_show);
        if (rendererCtrl != null)
        {
            rendererCtrl.Show(_show, true);
        }
        if (_show)
        {
            OnEffectChangeEvent(0);
        }
    }


    /// <summary>
    /// 开始移动，执行移动动画
    /// </summary>
    public override void MoveStart()
    {
        actorInfo.IsMoving = true;
        if (animFSM)
        {
            UpdateAnimSpeed();
        }
    }

    public void UpdateAnimSpeed()
    {
        if (animFSM)
        {
             if (Owner != null)
             {
                 animFSM.SetMoveSpeed(Owner.CurRealSpeed / (actorInfo.AnimationMoveSpeedBase * actorInfo.ModelScale));
			}else
			{
				animFSM.SetMoveSpeed(1.0f);
			}
			if(actorInfo.IsMoving)
				animFSM.Move();
         }
     }
    /// <summary>
    /// 移动结束，执行站立动画
    /// </summary>
    public override void MoveEnd()
    {
        actorInfo.IsMoving = false;
        base.MoveEnd();
    }

    protected void OnBaseUpdate(ActorBaseTag _tag,int _value)
    {
        switch (_tag)
        {
            case ActorBaseTag.Level:
                if (fxCtrl != null)
                {
                    fxCtrl.DoLevelUPEffect("TrainingEffect_B02");
                }
                break;
            case ActorBaseTag.Exp:
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 注册事件
    /// </summary>
    protected override void Regist()
    {
        if (actorInfo != null)
        {
           // actorInfo.OnBaseUpdate += OnBaseUpdate;
            actorInfo.OnEffectChangeEvent += OnEffectChangeEvent;
        }
    }
    /// <summary>
    /// 注销事件
    /// </summary>
    public override void UnRegist()
    {
        if (actorInfo != null)
        {
            //actorInfo.OnBaseUpdate -= OnBaseUpdate;
            actorInfo.OnEffectChangeEvent -= OnEffectChangeEvent;
        }
    }



    public void CancelDownLoad()
    {
        StopAllCoroutines();
        isDownloading_ = false;
    }



    protected void OnEffectChangeEvent(int _index)
    { 
        if (fxCtrl == null)
        {
            fxCtrl = this.gameObject.AddComponent<FXCtrl>();
        }
        fxCtrl.CleanBonesEffect();
        if (isDummy || !IsShowing) return;

        if (fxCtrl != null)
        { 
            if (actorInfo != null && actorInfo.BoneEffectList.Count > 0)
            {
//                foreach (var effect in actorInfo.BoneEffectList)
//                {
//					if(!string.IsNullOrEmpty(effect.boneName) && !string.IsNullOrEmpty(effect.effectName))
//                    	fxCtrl.SetBoneEffect(effect.boneName, effect.effectName, actorInfo.ModelScale);
//                }
                for (int i = 0, max = actorInfo.BoneEffectList.Count; i < max; i++) 
				{
                    BoneEffectRef effect = actorInfo.BoneEffectList[i];
					if(!string.IsNullOrEmpty(effect.boneName) && !string.IsNullOrEmpty(effect.effectName))
						fxCtrl.SetBoneEffect(effect.boneName, effect.effectName, actorInfo.ModelScale);
				}
                fxCtrl.ShowBoneEffect(true);
            } 
        }
    }

}
