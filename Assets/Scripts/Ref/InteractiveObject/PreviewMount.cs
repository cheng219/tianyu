//===============================================
//作者：吴江
//日期：2015/11/2
//用途：用作预览的坐骑对象
//===============================================




using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 用作预览的坐骑对象 by吴江
/// </summary>
public class PreviewMount : Actor
{
    /// <summary>
    /// 是否为互斥对象 by吴江
    /// </summary>
    public bool mutualExclusion = true;
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
    ///对象动作控制器 by吴江
    ///</summary>
    [System.NonSerialized]
    protected new MountAnimFSM animFSM = null;
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
    public string idleAnimName = string.Empty;

    public ActorAnimFSM.EventType needRandID = ActorAnimFSM.EventType.Idle;
    /// <summary>
    /// 预览时的坐标
    /// </summary>
    public Vector3 PreviewPosition
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
            return actorInfo.PreviewRot;
        }
    }


    /// <summary>
    /// 创建净数据对象 by吴江
    /// </summary>
    /// <param name="_info"></param>
    /// <returns></returns>
    public static PreviewMount CreateDummy(MountInfo _info)
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
        PreviewMount mount = newGO.AddComponent<PreviewMount>();
        mount.isDummy_ = true;
        mount.actorInfo = _info;

        return mount;
    }





    public void StartAsyncCreate(System.Action<PreviewMount> _callback)
    {
        StartCoroutine(CreateAsync(_callback));
    }

    IEnumerator CreateAsync(System.Action<PreviewMount> _callback)
    {
        if (isDummy_ == false)
        {
            GameSys.LogError("You can only start create Mob in dummy: " + actorInfo.ConfigID);
            yield break;
        }

        //
        PreviewMount mount = null;
        MountRendererCtrl myRendererCtrl = null;
        bool failed = false;
        pendingDownload = Create(actorInfo, delegate(PreviewMount _mount, EResult _result)
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
                myRendererCtrl.Show(true, true);
            }

        }); 
        if (mutualExclusion)
        {
            GameCenter.previewManager.PushDownLoadTask(pendingDownload);
        }
        while (mount == null || mount.inited == false)
        {
            if (failed) yield break;
            yield return null;
        }
        if (mutualExclusion)
        {
            GameCenter.previewManager.EndDownLoadTask(pendingDownload);
        }
        pendingDownload = null;

        mount.isDownloading_ = false;
        if (_callback != null)
        {
            _callback(mount);
        }
    }



    protected AssetMng.DownloadID Create(MountInfo _info, System.Action<PreviewMount, EResult> _callback)
    {
        return exResources.GetMount(_info.AssetName, delegate(GameObject _asset, EResult _result)
        {
            if (_result != EResult.Success)
            {
                _callback(null, _result);
                return;
            }

            this.gameObject.name = "Preview Mount[" + _info.ConfigID + "]";

            GameObject newGO = Instantiate(_asset) as GameObject;
            newGO.name = _asset.name;
            animationRoot = newGO.transform;
            newGO.transform.parent = this.gameObject.transform;
            newGO.transform.localEulerAngles = Vector3.zero;
            newGO.transform.localPosition = Vector3.zero;
            newGO.transform.localScale = Vector3.one;
            animFSM = newGO.AddComponent<MountAnimFSM>();
            newGO.AddComponent<MountRendererCtrl>();
            newGO.AddComponent<FXCtrl>();
            FXCtrl fx = this.gameObject.AddComponent<FXCtrl>();
            fx.SetUnLimit(true);
            fx.canShowShadow = false;


            isDummy_ = false;

            Init();
            _callback(this, _result);
        });
    }


    protected override void Init()
    {
        base.Init();
        OnEqChangeEvent(0);
        this.gameObject.SetMaskLayer(LayerMask.NameToLayer("Preview"));
        if (animFSM != null)
        {
            InitAnimation();
        }
        rendererCtrl = base.rendererCtrl as MountRendererCtrl;
        if (rendererCtrl != null)
        {
            rendererCtrl.Show(true, true);
            rendererCtrl.Init(actorInfo);
        }
        inited_ = true;
        if (actorInfo != null)
        {
            actorInfo.OnEffectChangeEvent += OnEqChangeEvent;
        }
		OnEffectChangeEvent(0);
    }


    public void DelayToPlayLevelUp(float _time)
    {
        StartCoroutine(PlayLevelUp(_time));
    }


    IEnumerator PlayLevelUp(float _time)
    {
        yield return new WaitForSeconds(_time);
        if (this != null && !isDummy_)
        {
            //if (animFSM != null)
            //{
            //    animFSM.PlayLevelUp();
            //}
            //if (fxCtrl != null && GameCenter.mountMng.IsLvUpState)
            //{
            //    fxCtrl.DoLevelUPEffect("TrainingEffect_B02");
            //}
        }
    }

    private Animation cachedAnimation_;
    private bool cacheAnimationInited = false;
    public Animation cachedAnimation
    {
        get
        {
            if (cachedAnimation_ == null && cacheAnimationInited == false)
            {
                cachedAnimation_ = animationRoot.gameObject.GetComponent<Animation>();
                cacheAnimationInited = true;
            }
            return cachedAnimation_;
        }
    }

    void InitAnimation()
    {
        var ani = cachedAnimation;
        string idleName = idleAnimName == string.Empty ? "idle1" : idleAnimName;
        if (ani && ani[idleName])
        {
            ani[idleName].wrapMode = WrapMode.Loop;
            ani.Play(idleName);
        }
        animFSM.SetupIdleAndMoveAnimationName(idleName, "move2");
        animFSM.StartStateMachine();
        animFSM.Idle();

    }




    protected new void OnDestroy()
    {
        base.OnDestroy();
        UnRegist();
        if (actorInfo != null)
        {
            actorInfo.OnEffectChangeEvent -= OnEqChangeEvent;
        }
    }

    protected void OnEqChangeEvent(int _index)
    {
        if (fxCtrl == null)
        {
            fxCtrl = this.gameObject.AddComponent<FXCtrl>();
        }
        fxCtrl.CleanBonesEffect();
        //if (isDummy || !IsShowing) return;
    }

	protected void OnEffectChangeEvent(int _index)
	{
		if (fxCtrl == null)
		{
			fxCtrl = this.gameObject.AddComponent<FXCtrl>();
		}
		fxCtrl.CleanBonesEffect();
		if (isDummy) return;

		if (fxCtrl != null)
		{
			if (actorInfo != null && actorInfo.MountEffectList.Count > 0)
			{
				//                foreach (var effect in actorInfo.BoneEffectList)
				//                {
				//					if(!string.IsNullOrEmpty(effect.boneName) && !string.IsNullOrEmpty(effect.effectName))
				//                    	fxCtrl.SetBoneEffect(effect.boneName, effect.effectName, actorInfo.ModelScale);
				//                }
				for (int i = 0,max=actorInfo.MountEffectList.Count; i < max; i++) 
				{ 
					MountEffect effect = actorInfo.MountEffectList[i];
                    if (actorInfo.MountKind == MountType.SKINLIST)
                    {
                        if (!string.IsNullOrEmpty(effect.boneName) && !string.IsNullOrEmpty(effect.effectName))
                            fxCtrl.SetBoneEffect(effect.boneName, effect.effectName, actorInfo.ModelScale);
                    }
                    else
                    {
                        if ((actorInfo.ConfigID - 1) * 9 + effect.effectLev <= GameCenter.newMountMng.CurLev)
                        {
                            if (!string.IsNullOrEmpty(effect.boneName) && !string.IsNullOrEmpty(effect.effectName))
                                fxCtrl.SetBoneEffect(effect.boneName, effect.effectName, actorInfo.ModelScale);
                        }
                    }
				}
			}
			fxCtrl.ShowBoneEffect(true);
		}
	}
}
