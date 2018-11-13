//=======================================
//作者:吴江
//日期:2015/9/23
//描述:掉落物品的表现层对象
//=======================================



using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneItem : Actor
{


    #region 数据
    protected new SceneItemInfo actorInfo = null;

	protected bool isActiveForMainplayer = true;
    protected bool IsActiveForMainplayer
    {
        get
        {
            return isActiveForMainplayer;
        }
        set
        {
            if (isActiveForMainplayer != value)
            {
                isActiveForMainplayer = value;
                if (isActiveForMainplayer)
                {
                    ActiveBoxCollider(true, actorInfo.colliderScale);
                    if (headTextCtrl != null)
                    {
                        headTextCtrl.TextParent.SetActive(true);
						UpdateName ();
                    }
                }
                else
                {
                    ActiveBoxCollider(false, actorInfo.colliderScale);
                    if (headTextCtrl != null)
                    {
                        headTextCtrl.TextParent.SetActive(false);
                    }
                }
            }
        }
    }


    /// <summary>
    /// 动画控制器
    /// </summary>
    [System.NonSerialized]
    protected new SceneItemAnimFSM animFSM = null;

    public TouchType IsTouchType
    {
        get
        {
            if (actorInfo == null) return TouchType.NONE;
            return actorInfo.ItemTouchType;
        }
    }

    public SceneFunctionType MySceneFunctionType
    {
        get
        {
            return actorInfo.FunctionType;
        }
    }

	public int ConfigID
	{
		get
		{
			return actorInfo.ConfigID;
		}
	}
    #endregion

    #region 构造
    /// <summary>
    /// 创建净数据对象 by吴江
    /// </summary>
    /// <param name="_info"></param>
    /// <returns></returns>
    public static SceneItem CreateDummy(SceneItemInfo _info)
    {
        GameObject newGO = new GameObject("Dummy SceneItem[" + _info.ServerInstanceID + "]");
        newGO.tag = "Monster";
        if (_info.FunctionType == SceneFunctionType.BLOCK)
        {
            newGO.SetMaskLayer(LayerMask.NameToLayer("Block"));
        }
        else
        {
            newGO.SetMaskLayer(LayerMask.NameToLayer("SceneItem"));
        }
        SceneItem item = newGO.AddComponent<SceneItem>();
        item.actorInfo = _info;
        item.isDummy_ = true;
        item.id = _info.ServerInstanceID;
        GameCenter.curGameStage.PlaceGameObjectFromServer(item, _info.ServerPos.x, _info.ServerPos.z, _info.Dir, _info.ServerPos.y);//人参果宴采集了人参果，在斜坡或地图边缘,其时间到达自动掉落，会掉落在地底
        GameCenter.curGameStage.AddObject(item);
		item.IsActiveForMainplayer = true;//item.CheckIsActiveForMainplayer();
        return item;
    }

    public void StartAsyncCreate(bool _isSpawn = false)
    {
        StartCoroutine(CreateAsync(_isSpawn));
    }


    /// <summary>
    /// 异步创建协程  by吴江 
    /// </summary>
    /// <param name="_isSpawn"></param>
    /// <returns></returns>
    IEnumerator CreateAsync(bool _isSpawn)
    {
        if (isDummy_ == false)
        {
            GameSys.LogError("You can only start create DropItem in dummy: " + actorInfo.ServerInstanceID);
            yield break;
        }

        SceneItem item = null;
        bool failed = false;
        pendingDownload = Create(actorInfo, delegate(SceneItem _item, EResult _result)
        {
            if (_result != EResult.Success)
            {
                failed = true;
                return;
            }
            item = _item;
            pendingDownload = null;

        });
        while (item == null || item.inited == false)
        {
            if (failed) yield break;
            yield return null;
        }
        pendingDownload = null;

        if (GameCenter.curGameStage == null)
        {
            yield break;
        }


        item.isDownloading_ = false;
    }



    protected AssetMng.DownloadID Create(SceneItemInfo _info, System.Action<SceneItem, EResult> _callback)
    {
        return exResources.GetSceneItem(_info.ConfigID, delegate(GameObject _asset, EResult _result)
        {
            if (GameCenter.IsDummyDestroyed(ObjectType.SceneItem, _info.ServerInstanceID))
            {
                _callback(null, EResult.Failed);
                return;
            }
            if (_result != EResult.Success)
            {
                _callback(null, _result);
                return;
            }
            this.gameObject.name = "SceneItem[" + _info.ServerInstanceID + "]";

            GameObject newGO = Instantiate(_asset) as GameObject;
            newGO.name = _asset.name;
            newGO.transform.parent = this.gameObject.transform;
            newGO.transform.localEulerAngles = Vector3.zero;
            newGO.transform.localPosition = Vector3.zero;
			if(_info.FunctionType == SceneFunctionType.BLOCK)//光墙放到地下去一点 谢凯要求
			{
				newGO.transform.localPosition = new Vector3(0,-1,0);
			}
            newGO.transform.localScale = Vector3.one * _info.ModelScale;
            newGO.AddComponent<FXCtrl>();
            newGO.AddComponent<SceneItemAnimFSM>();
            newGO.SetActive(false);
            newGO.SetActive(true);
            RefreshShader(newGO);

            isDummy_ = false;

            Init();
            _callback(this, _result);
        });
    }

    #endregion

    #region Unity
    protected new void Awake()
    {
        typeID = ObjectType.SceneItem;
        base.Awake();
    }


	void OnTriggerExit(Collider other)
	{
		Actor actor = other.GetComponent<Actor>();
		if (actor != null)
		{
            if (actorInfo.FunctionType == SceneFunctionType.SAFEROOM)
            {
				GameCenter.messageMng.AddClientMsg(310);
            }
		}
	}

    void OnTriggerEnter(Collider other)
    {
        Actor actor = other.GetComponent<Actor>();
        if (actor != null)
        {
            if (actorInfo.FunctionType == SceneFunctionType.BLOCK)
            {
                InteractionSound();
            }
            if (actorInfo.FunctionType == SceneFunctionType.SAFEROOM)
            {
                GameCenter.messageMng.AddClientMsg(309);
            }
            DoAction(actor);
        }
    }


    void OnTriggerStay(Collider other)
    {
        Actor actor = other.GetComponent<Actor>();
        if (actor != null)
        {
            DoAction(actor);
        }
    }



    #endregion

    #region 辅助逻辑
    protected override void Regist()
    {
        base.Regist();
        GameCenter.taskMng.OnTaskListUpdate += OnTaskListUpdate;
        GameCenter.taskMng.updateSingleTask += OnTaskListUpdate;
        GameCenter.taskMng.AddNewTask += OnTaskListUpdate;
    }


    public override void UnRegist()
    {
        base.UnRegist();
        GameCenter.taskMng.OnTaskListUpdate -= OnTaskListUpdate;
        GameCenter.taskMng.updateSingleTask -= OnTaskListUpdate;
        GameCenter.taskMng.AddNewTask -= OnTaskListUpdate;
    }


    protected void OnTaskListUpdate(TaskDataType _type,TaskType _taskType)
    {
		IsActiveForMainplayer = true;//CheckIsActiveForMainplayer();
    }

    protected void OnTaskListUpdate(TaskInfo _info)
    {
		IsActiveForMainplayer = true;//CheckIsActiveForMainplayer();

    }


    public bool CheckIsActiveForMainplayer()
    {
       TaskInfo info =  GameCenter.taskMng.GetTaskInfo(actorInfo.TaskID);
       if (info == null) return false;
       if (info.Step > actorInfo.StartStep && info.Step < actorInfo.EndStep)
       {
           return true;
       }
       if (info.Step == actorInfo.StartStep && info.TaskState >= actorInfo.StartTaskStateType)
       {
           return true;
       }
       if (info.Step == actorInfo.EndStep && info.TaskState <= actorInfo.EndTaskStateType)
       {
           return true;
       }
       return false;
    }

    protected override void Init()
    {
        base.Init();
        animFSM = base.animFSM as SceneItemAnimFSM;
		if(animFSM != null)
		{
	        animFSM.SetDeadAnim(actorInfo.DeadAnimName, actorInfo.DeadTime);
			animFSM.SetupIdleAndMoveAnimationName("idle1", "move2");
			animFSM.SetupCombatAnimationName("idle1", null);
			animFSM.Idle();
		}
		nameHeight = actorInfo.NameHeight;
		height = 0f;
        beAttackRadius = actorInfo.ColliderRadius;
        //IsActiveForMainplayer = CheckIsActiveForMainplayer();
        if (headTextCtrl == null) headTextCtrl = this.gameObject.AddComponent<HeadTextCtrl>();
		if (!actorInfo.IsActor)// && IsActiveForMainplayer) //过场动画演员不展示名字
        {
			UpdateName ();
        }
//        if (actorInfo.BubbleContent != null)
//        {
//            headTextCtrl.SetBubble(actorInfo.BubbleContent.content, actorInfo.BubbleContent.time);
//        }
        AssetMng.GetEeffctAssetObject(actorInfo.DeadEffectName, null);//预加载死亡特效(镜像文件)
        inited_ = true;



        ActiveBoxCollider(IsActiveForMainplayer, actorInfo.colliderScale);
        ShowPointEffect();
    }

	/// <summary>
	/// 更新掉落物品名字
	/// </summary>
	public  void UpdateName()
	{
		headTextCtrl.SetName(actorInfo.Name.Replace("\\n","\n"));
	}


    public void Dead()
    {
        if (this == null) return;
        isDead_ = true;
        if (isDummy)//还处在isDummy_状态,直接删除
        {
            Destroy(this);
            return;
        }
        if (animFSM != null)
        {
            animFSM.Dead();
        }
        if (fxCtrl != null)
        {
            fxCtrl.DoDeadEffect(actorInfo.DeadEffectName);
        }
        GameCenter.soundMng.PlaySound(actorInfo.DeadSoundName, SoundMng.GetSceneSoundValue(transform, GameCenter.curMainPlayer.transform), false, true);
        Invoke("DestorySelf", actorInfo.DeadTime);
    }


    public void DoAction(Actor _actor)
    {
        switch (actorInfo.FunctionType)
        {
            case SceneFunctionType.BLOCK:
                break;
            case SceneFunctionType.TRIGGER:
                break;
            case SceneFunctionType.OTHER:
                break;
            default:
                break;
        }
    }

    public void BeClicked()
    {
        if (fxCtrl == null)
        {
            fxCtrl = this.gameObject.AddComponent<FXCtrl>();
        }
        if (!string.IsNullOrEmpty(actorInfo.ActioningEffectName) && !actorInfo.ActioningEffectName.Equals("0"))
        {
            fxCtrl.DoCollectEffect(actorInfo.ActioningEffectName);
        }
    }

    public void ClearCollectEffect()
    {
        if (isDead || isDummy) return;
        if (fxCtrl != null) fxCtrl.ClearCollectEffect();
    }

    public override void InteractionSound()
    {
        base.InteractionSound();
        GameCenter.soundMng.PlaySound(actorInfo.ActionIngSoundRes, SoundMng.GetSceneSoundValue(transform, GameCenter.curMainPlayer.transform), false, true);
    }

    void ShowPointEffect()
    {
        if (fxCtrl == null)
        {
            fxCtrl = this.gameObject.AddComponent<FXCtrl>();
        }
        fxCtrl.CleanBonesEffect();
        if (isDummy) return;//sceneItem没有调用show方法,不判断IsShowing
        if (fxCtrl != null)
        {
            if (actorInfo != null && actorInfo.BoneEffectList.Count > 0)
            {
                for (int i = 0, max = actorInfo.BoneEffectList.Count; i < max; i++)
                {
                    SceneItemEffect effect = actorInfo.BoneEffectList[i];
                    if (!string.IsNullOrEmpty(effect.pointName) && !string.IsNullOrEmpty(effect.effectName) && !effect.pointName.Equals("0"))
                        fxCtrl.SetBoneEffect(effect.pointName, effect.effectName, actorInfo.ModelScale);
                }
                fxCtrl.ShowBoneEffect(true);
            }
        }
    }

    protected void DestorySelf()
    {
        Destroy(this);
    }
    #endregion




}

