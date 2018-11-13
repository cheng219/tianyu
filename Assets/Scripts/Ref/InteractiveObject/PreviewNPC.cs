//=======================================
//作者:吴江
//日期:2015/11/2
//用途：用作预览的NPC对象
//=======================================



using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PreviewNPC : SmartActor
{
    /// <summary>
    /// 是否为互斥对象 by吴江
    /// </summary>
    public bool mutualExclusion = true;
    /// <summary>
    /// 基本数据
    /// </summary>
    protected new NPCInfo actorInfo
    {
        get { return base.actorInfo as NPCInfo; }
        set
        {
            base.actorInfo = value;
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



    /// <summary>
    /// 创建NPC的净数据对象 by吴江
    /// </summary>
    /// <param name="_info"></param>
    /// <returns></returns>
    public static PreviewNPC CreateDummy(NPCTypeRef _refData)
    {
        GameObject newGO = null;
        NPCInfo info = new NPCInfo(_refData);
        if (GameCenter.instance.dummyMobPrefab != null)
        {
            newGO = Instantiate(GameCenter.instance.dummyNpcPrefab) as GameObject;
            newGO.name = "Dummy Preview NPC [" + info.ServerInstanceID + "]";
        }
        else
        {
            newGO = new GameObject("Dummy Preview NPC[" + info.ServerInstanceID + "]");
        }
        newGO.tag = "NPC";
        PreviewNPC npc = newGO.AddComponent<PreviewNPC>();
        npc.actorInfo = info;
        npc.isDummy_ = true;
        return npc;
    }


    /// <summary>
    /// 创建NPC的净数据对象 by吴江
    /// </summary>
    /// <param name="_info"></param>
    /// <returns></returns>
    public static PreviewNPC CreateDummy(NPCInfo _info)
    {
        GameObject newGO = null;
        if (GameCenter.instance.dummyMobPrefab != null)
        {
            newGO = Instantiate(GameCenter.instance.dummyNpcPrefab) as GameObject;
            newGO.name = "Dummy Preview NPC [" + _info.ServerInstanceID + "]";
        }
        else
        {
            newGO = new GameObject("Dummy Preview NPC[" + _info.ServerInstanceID + "]");
        }
        newGO.tag = "NPC";
        PreviewNPC npc = newGO.AddComponent<PreviewNPC>();
        npc.actorInfo = _info;
        npc.isDummy_ = true;
        return npc;
    }

    /// <summary>
    /// 是否需要按配置摆放预览位置
    /// </summary>
    public PreviewConfigType previewConfigType = PreviewConfigType.Dialog;

    public string idleAnimName = string.Empty;
	/// <summary>
    /// 预览时的相机距离 
    /// </summary>
	public Vector3 PreviewPosition
    {
        get
        {
            return actorInfo.PreviewPosition(previewConfigType);
        }
    }
	/// <summary>
    /// 预览时的相机角度
    /// </summary>
	public Vector3 PreviewRotation
    {
        get
        {
            return actorInfo.PreviewRotation(previewConfigType);
        }
    }


    public void StartAsyncCreate(Action<PreviewNPC> _callback)
    {
        StartCoroutine(CreateAsync(_callback));
    }

    protected IEnumerator CreateAsync(Action<PreviewNPC> _callback)
    {
        if (isDummy_ == false)
        {
            GameSys.LogError("You can only start create NPC in dummy: " + actorInfo.ServerInstanceID);
            yield break;
        }

        isDownloading_ = true;
        PreviewNPC npc = null;
        //NPCRendererCtrl myRendererCtrl = null;
        bool faild = false;
        pendingDownload = Create(actorInfo, delegate(PreviewNPC _npc, EResult _result)
        {
            if (_result != EResult.Success)
            {
                faild = true;
                return;
            }
            npc = _npc;
            float previewScale = Mathf.Max(0, actorInfo.PreviewScale(previewConfigType));
            if (previewScale == 0) previewScale = 1;
            npc.transform.localScale *= previewScale;
            pendingDownload = null;

            //myRendererCtrl = npc.GetComponent<NPCRendererCtrl>();   //TO TO:初始化出来应该先隐藏，由任务等其他信息决定是否要显示 by吴江
            //myRendererCtrl.Show(false, true);
        });
        if (mutualExclusion)
        {
            GameCenter.previewManager.PushDownLoadTask(pendingDownload);
        }
        while (npc == null || npc.inited == false)
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
            _callback(npc);
        }
    }

    protected AssetMng.DownloadID Create(NPCInfo _info, System.Action<PreviewNPC, EResult> _callback)
    {
        return exResources.GetNPC(_info.Type, delegate(GameObject _asset, EResult _result)
        {
            if (_result != EResult.Success)
            {
                _callback(null, _result);
                return;
            }

            this.gameObject.name = "Preview NPC[" + _info.ServerInstanceID + "]";

            GameObject newGO = Instantiate(_asset) as GameObject;
            newGO.name = _asset.name;
            newGO.transform.parent = this.gameObject.transform;
            newGO.transform.localEulerAngles = Vector3.zero;
            newGO.transform.localPosition = Vector3.zero;
            newGO.AddComponent<NPCRendererCtrl>();
            FXCtrl fxCtrl = newGO.gameObject.AddComponent<FXCtrl>();
            fxCtrl.canShowShadow = false;
            newGO.SetActive(false);
            newGO.SetActive(true);

            animationRoot = newGO.transform;
            isDummy_ = false;

            Init();
            _callback(this, _result);
        });
    }


    protected override void Init()
    {
        rendererCtrl = this.gameObject.GetComponentInChildrenFast<NPCRendererCtrl>();
        height = actorInfo.Hight / 100f;
        nameHeight = height;
        inited_ = true;


        if (isDummy) return;
        var ani = cachedAnimation;
        if (ani && ani["stand"])
        {
            ani["stand"].wrapMode = WrapMode.Loop;
            ani.Play("stand");
        }
        this.gameObject.tag = "NPC";
        inited_ = true;
        rendererCtrl.SetLayer(LayerMask.NameToLayer("Preview"));
        if (idleAnimName != string.Empty)
            InitAnimation();
    }
    protected override void InitAnimation()
    {
        animFSM.SetupIdleAndMoveAnimationName(idleAnimName, "move2");
        animFSM.SetInCombat(false);
        animFSM.Idle();
    }
}
