//===============================================
//作者：吴江
//日期：2015/8/1
//用途：用作预览的怪物对象
//===============================================




using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 用作预览的怪物对象 by吴江
/// </summary>
public class PreviewMob : SmartActor
{
    /// <summary>
    /// 是否为互斥对象 by吴江
    /// </summary>
    public bool mutualExclusion = true;
    /// <summary>
    /// 完整数据层对象的引用 by吴江
    /// </summary>
    protected new MonsterInfo actorInfo
    {
        get { return base.actorInfo as MonsterInfo; }
        set
        {
            base.actorInfo = value;
        }
    }


    /// <summary>
    /// 对象动画控制器  by吴江 
    /// </summary>
    [System.NonSerialized]
    protected new MobAnimFSM animFSM = null;

    ///<summary>
    ///对象渲染控制器 by吴江
    ///</summary>
    [System.NonSerialized]
    public new MobRendererCtrl rendererCtrl = null;

	public string idleAnimName = string.Empty;
    /// <summary>
    /// 是否需要按配置摆放预览位置
    /// </summary>
    public PreviewConfigType previewConfigType = PreviewConfigType.Dialog;
    /// <summary>
    /// 预览时的坐标
    /// </summary>
    public Vector3 PreviewPosition
    {
        get
        {
            return actorInfo.PreviewPosition(previewConfigType);
        }
    }
	
	/// <summary>
    /// 预览时的角度
    /// </summary>
	public Vector3 PreviewRotation
    {
		get
        {
            return actorInfo.PreviewRotation(previewConfigType);
        }
	}

    /// <summary>
    /// 创建净数据对象 by吴江
    /// </summary>
    /// <param name="_info"></param>
    /// <returns></returns>
    public static PreviewMob CreateDummy(MonsterInfo _info)
    {

        GameObject newGO = null;
        if (GameCenter.instance.dummyMobPrefab != null)
        {
            newGO = Instantiate(GameCenter.instance.dummyMobPrefab) as GameObject;
            newGO.name = "Dummy PreviewMob [" + _info.ServerInstanceID + "]";
        }
        else
        {
            newGO = new GameObject("Dummy PreviewMob[" + _info.ServerInstanceID + "]");
        }
        newGO.tag = "Monster";
        PreviewMob mob = newGO.AddComponent<PreviewMob>();
        mob.actorInfo = _info;
        mob.isDummy_ = true;
        return mob;
    }





    public void StartAsyncCreate(System.Action<PreviewMob> _callback)
    {
        StartCoroutine(CreateAsync(_callback));
    }

    IEnumerator CreateAsync(System.Action<PreviewMob> _callback)
    {
        if (isDummy_ == false)
        {
            GameSys.LogError("You can only start create Mob in dummy: " + actorInfo.ServerInstanceID);
            yield break;
        }

        //
        PreviewMob mob = null;
        bool failed = false;
        pendingDownload = Create(actorInfo, delegate(PreviewMob _mob, EResult _result)
        {
            if (_result != EResult.Success)
            {
                failed = true;
                return;
            }
            mob = _mob;
            float previewScale = Mathf.Max(0, actorInfo.PreviewScale(previewConfigType));
            if (previewScale == 0) previewScale = 1;
            mob.transform.localScale *= previewScale;
            pendingDownload = null;

        });
        if (mutualExclusion)
        {
            GameCenter.previewManager.PushDownLoadTask(pendingDownload);
        }
        while (mob == null || mob.inited == false)
        {
            if (failed) yield break;
            yield return null;
        }
        if (mutualExclusion)
        {
            GameCenter.previewManager.EndDownLoadTask(pendingDownload);
        }
        pendingDownload = null;

        if (GameCenter.curGameStage == null)
        {
            yield break;
        }

        if (!actorInfo.IsAlive)
        {
            mob.Dead();
        }
        mob.isDownloading_ = false;

        if (_callback != null)
        {
            _callback(mob);
        }
    }



    protected AssetMng.DownloadID Create(MonsterInfo _info, System.Action<PreviewMob, EResult> _callback)
    {
        return exResources.GetMob(_info.Prof, delegate(GameObject _asset, EResult _result)
        {
            if (_result != EResult.Success)
            {
                _callback(null, _result);
                return;
            }
            this.gameObject.name = "Preview Monster[" + _info.ServerInstanceID + "]";

            GameObject newGO = Instantiate(_asset) as GameObject;
            newGO.name = _asset.name;
            newGO.transform.parent = this.gameObject.transform;
            newGO.transform.localEulerAngles = Vector3.zero;
            newGO.transform.localPosition = Vector3.zero;
            newGO.AddComponent<MobAnimFSM>();
            newGO.AddComponent<MobRendererCtrl>();
            FXCtrl fxCtrl = newGO.gameObject.AddComponent<FXCtrl>();
            fxCtrl.canShowShadow = false;
            newGO.SetActive(false);
            newGO.SetActive(true);


            isDummy_ = false;

            Init();
            _callback(this, _result);
        });
    }




    protected override void Init()
    {
        base.Init();
        animFSM = base.animFSM as MobAnimFSM;
        rendererCtrl = this.gameObject.GetComponentInChildrenFast<MobRendererCtrl>();
        rendererCtrl.SetLayer(LayerMask.NameToLayer("Preview"));
        rendererCtrl.Init(actorInfo, fxCtrl);

        inited_ = true;

        if (animFSM != null)
        {            
			animFSM.StartStateMachine();
            if (moveFSM != null)
            {
                if (moveFSM.isMoving)
                {
                    animFSM.Move();
                }
                else
                {
                    animFSM.Idle();
                }
            }
            animFSM.IdleImmediate();
			if(idleAnimName != string.Empty)
				InitAnimation();
        }
        else
        {
            GameSys.LogError("怪物" + gameObject.name + "没有动画组件!");
        }

        if (rendererCtrl)
        {
            rendererCtrl.Show(true);
        }

    }


    protected override void InitAnimation()
    {
		animFSM.SetupIdleAndMoveAnimationName(idleAnimName, "move2");
        animFSM.SetInCombat(false);
        animFSM.Movie();
    }
}
