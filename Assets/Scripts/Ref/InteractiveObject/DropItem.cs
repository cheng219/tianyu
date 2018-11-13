    //=======================================
//作者:吴江
//日期:2015/9/23
//描述:掉落物品的表现层对象
//=======================================



using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DropItem : InteractiveObject
{


    #region 数据
    protected DropItemInfo actorInfo = null;
    /// <summary>
    /// 是否资源(不占背包格子)
    /// </summary>
    public bool IsReource
    {
        get
        {
            return false;//actorInfo.ResourceId > 0;
        }
    }

    /// <summary>
    /// 本次取出以后的表现路径列表
    /// </summary>
    List<JumpPath> textJumpPathList = new List<JumpPath>();

    float deltTime = 0;
    float startTime = 0;
    float curDuration = 0;
    Vector3 curFrom = Vector3.zero;
//    Vector3 curScale = Vector3.zero;
    JumpPath curJumpPath = null;
    /// <summary>
    /// 总共能持续的时间
    /// </summary>
    public float totalTime = 0.75f;


    protected bool hasShowName = false;
    /// <summary>
    /// 这里为true,则表示掉落物的动画播放完毕且名字显示出来了
    /// </summary>
    public bool HasShowName
    {
        get
        {
            return hasShowName;
        }
    }
    protected bool hasJumpFinished = false;

    public List<uint> OwnerID
    {
        get
        {
            return actorInfo.OwnerID;
        }
    }

    public bool IsTimeOut
    {
        get
        {
            return actorInfo.IsTimeOut;
        }
    }
    #endregion

    #region 构造
    /// <summary>
    /// 创建净数据对象 by吴江
    /// </summary>
    /// <param name="_info"></param>
    /// <returns></returns>
    public static DropItem CreateDummy(DropItemInfo _info)
    {
        GameObject newGO = new GameObject("Dummy DropItem[" + _info.ServerInstanceID + "]");
        newGO.tag = "Monster";
        newGO.SetMaskLayer(LayerMask.NameToLayer("DropItem"));
        DropItem item = newGO.AddComponent<DropItem>();
        item.actorInfo = _info;
        item.isDummy_ = true;
        item.id = _info.ServerInstanceID;
        GameCenter.curGameStage.PlaceGameObjectFromServer(item, _info.StartPos.x, _info.StartPos.z, 90, _info.StartPos.y);
        GameCenter.curGameStage.AddObject(item);

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

        DropItem item = null;
        bool failed = false;
        pendingDownload = Create(actorInfo, delegate(DropItem _item, EResult _result)
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



    protected AssetMng.DownloadID Create(DropItemInfo _info, System.Action<DropItem, EResult> _callback)
    {
        return exResources.GetDropItem(_info.ConfigID, delegate(GameObject _asset, EResult _result)
        {
            if (GameCenter.IsDummyDestroyed(ObjectType.DropItem, _info.ServerInstanceID))
            {
                _callback(null, EResult.Failed);
                return;
            }
            if (_result != EResult.Success)
            {
                _callback(null, _result);
                return;
            }
            this.gameObject.name = "DropItem[" + _info.ServerInstanceID + "]";

            GameObject newGO = Instantiate(_asset) as GameObject;
            newGO.name = _asset.name;
            newGO.transform.parent = this.gameObject.transform;
            newGO.transform.localEulerAngles = Vector3.zero;
            newGO.transform.localPosition = Vector3.zero;
            newGO.transform.localScale = Vector3.one;
            newGO.AddComponent<FXCtrl>();
            RefreshShader(newGO);
            newGO.SetActive(false);
            newGO.SetActive(true);
            Animation anima = newGO.GetComponent<Animation>();
            if (anima != null && anima["drop"] != null)
            {
                anima["drop"].wrapMode = WrapMode.ClampForever;
                anima.Play("drop");
            }
            else
            {
                Debug.LogError("找不到 drop  动画!");
            }


            isDummy_ = false;

            Init();
            _callback(this, _result);
        });
    }
    #endregion


    #region Unity
    protected new void Awake()
    {
        typeID = ObjectType.DropItem;
        base.Awake();
    }

    void Update()
    {
        if (!inited_) return;
        if (!hasJumpFinished)
        {
            deltTime = Time.time - startTime;
            if (deltTime >= curDuration)
            {
                textJumpPathList.RemoveAt(0);
                if (textJumpPathList.Count > 0)
                {
                    NewStepInit();
                    deltTime = Time.time - startTime;
                }
                else
                {
                    hasJumpFinished = true;
                }
            }
            float curRate = deltTime / curDuration;
            transform.position = Vector3.Lerp(curFrom, curJumpPath.toPos, curRate);
           // transform.localScale = Vector3.Lerp(curScale, curJumpPath.toScale, curRate);
        }
        else
        {
            if (!hasShowName)
            {
                if (headTextCtrl == null)
                {
                    headTextCtrl = this.gameObject.AddComponent<HeadTextCtrl>();
                }
               // headTextCtrl.SetName(actorInfo.ItemName);
                UpdateName();
                if (fxCtrl != null && !string.IsNullOrEmpty(actorInfo.EffectName.Trim()))//策划填了空格,Trim排除掉
                {
                    fxCtrl.DoNormalEffect(actorInfo.EffectName);
                }
//				if (actorInfo.BlongToServerInstanceID.Contains((uint)GameCenter.curMainPlayer.id))
//                {
                    ActiveBoxCollider(true, 1.5f);
//                }
                hasShowName = true;
                InteractionSound();
            }
        }
    }

    public void UpdateName()
    {
        if (headTextCtrl == null)
        {
            headTextCtrl = this.gameObject.AddComponent<HeadTextCtrl>();
        }
        if (headTextCtrl == null) return;
        if (!GameCenter.systemSettingMng.ItemName)
        {
            headTextCtrl.SetName("");
        }
        else
        {
            headTextCtrl.SetName(actorInfo.ItemName);
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameCenter.curMainPlayer.gameObject)
        {
			//if (!actorInfo.OwnerID.Contains((uint)GameCenter.curMainPlayer.id)) return;
            //EquipmentRef eqRef = ConfigMng.Instance.GetEquipmentRef(actorInfo.ConfigID);
            //if (eqRef != null)
            //{
            //}
            if (GameCenter.inventoryMng.IsBagFull && !IsReource)
            {
                GameCenter.messageMng.AddClientMsg(18); //提示背包已经满 by吴江
                return;
            }
            GameCenter.sceneMng.C2S_PickUpDropItem(actorInfo.ServerInstanceID, false);
           
            //Debug.Log("物品的名字" + actorInfo.ItemName);
            if (actorInfo.Family==EquipmentFamily.SPECIALBOX)//宝箱位只有4个超过4个提示宝箱格已满
            {
                if (GameCenter.royalTreasureMng.royalTreasureDic.Count >= 4)
                    GameCenter.messageMng.AddClientMsg(486);
            }
        }
    }

    //void OnDisable()
    //{
    //    if (actorInfo.OwnerID == GameCenter.curMainPlayer.id && !actorInfo.IsTimeOut)
    //    {
    //         GameCenter.curMainPlayer.DoPickUpEffect(this.transform.position);
    //    }
    //}

    #endregion

    #region 辅助逻辑
    public override void InteractionSound()
    {
        base.InteractionSound();
        GameCenter.soundMng.PlaySound(actorInfo.DropSound, SoundMng.GetSceneSoundValue(transform, GameCenter.curMainPlayer.transform), false, true);
    }

    protected override void Init()
    {
        base.Init();
        nameHeight = 0.5f;
        height = 0.5f;

        if (actorInfo.DropFromServerInstanceID <= 0)
        {
            hasJumpFinished = true;
        }
        else
        {
            //Vector3 fromPos = ActorMoveFSM.LineCast(actorInfo.StartPos,isDummy);
            Vector3 toPos = ActorMoveFSM.LineCast(new Vector3(actorInfo.ServerPos.x,-300f,actorInfo.ServerPos.y), true);
            if (toPos.y < -200)
            {
                toPos = ActorMoveFSM.LineCast(actorInfo.StartPos, true);
            }
            textJumpPathList.Add(new JumpPath(1.0f, toPos, Vector3.one));
            NewStepInit();
        }
        inited_ = true;
    }

    /// <summary>
    /// 更新到下一步  by吴江
    /// </summary>
    void NewStepInit()
    {
        if (textJumpPathList.Count <= 0)
        {
            return;
        }
        startTime = Time.time;
        curJumpPath = textJumpPathList[0];
        curDuration = curJumpPath.durationRate * totalTime;
        curFrom = transform.position;
//        curScale = transform.localScale;
    }

    /// <summary>
    /// 品质颜色
    /// </summary>
    public Color QualityColor
    {
        get
        {
            return actorInfo.QualityColor;
        }
    }

    /// <summary>
    /// 品质
    /// </summary>
    public int Quality
    {
        get
        {
            return actorInfo.Quality;
        }
    }
    #endregion 




}


public class JumpPath
{
    /// <summary>
    /// 本阶段比率
    /// </summary>
    public float durationRate = 0;

    /// <summary>
    /// 目标坐标
    /// </summary>
    public Vector3 toPos = Vector3.zero;

    /// <summary>
    /// 目标大小
    /// </summary>
    public Vector3 toScale = Vector3.zero;


    public JumpPath(float _durationRate, Vector3 _toPos, Vector3 _toScale)
    {
        durationRate = _durationRate;
        toPos = _toPos;
        toScale = _toScale;
    }
}