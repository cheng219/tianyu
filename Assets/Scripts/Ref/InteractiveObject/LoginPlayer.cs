//==================================================
//作者：吴江
//日期:2015/5/6
//用途：登录界面人物的表现层对象
//=====================================================



using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// 登录界面人物的表现层对象 by吴江
/// </summary>
public class LoginPlayer : PlayerBase
{

    /// <summary>
    /// 创建净数据对象 by吴江
    /// </summary>
    /// <param name="_info"></param>
    /// <returns></returns>
    public static LoginPlayer CreateDummy(PlayerBaseInfo _info)
    {
        GameObject newGO = new GameObject("Dummy LoginPlayer[" + _info.ServerInstanceID + "]");
        newGO.tag = "Player";

       // newGO.AddComponent<ActorMoveFSM>();
        LoginPlayer loginPlayer = newGO.AddComponent<LoginPlayer>();
        loginPlayer.actorInfo = _info;
        loginPlayer.isDummy_ = true;
        GameCenter.curGameStage.AddObject(loginPlayer);
        return loginPlayer;
    }


    /// <summary>
    /// 异步开始创建渲染内容 by吴江
    /// </summary>
    /// <param name="_callback"></param>
    public virtual void StartAsyncCreate(Action<LoginPlayer> _callback)
    {
        StartCoroutine(CreateAsync(_callback));
    }

    protected IEnumerator CreateAsync(Action<LoginPlayer> _callback)
    {
        if (isDummy_ == false)
        {
            GameSys.LogError("You can only start create LoginPlayer in dummy: " + actorInfo.ServerInstanceID);
            yield break;
        }


        LoginPlayer loginPlayer = null;
        bool faild = false;
        pendingDownload = Create(actorInfo, delegate(LoginPlayer _pc, EResult _result)
        {
            if (_result != EResult.Success)
            {
                faild = true;
                return;
            }
            loginPlayer = _pc;
            pendingDownload = null;

        });
        while (loginPlayer == null || loginPlayer.inited == false)
        {
            if (faild) yield break;
            yield return null;
        }

        pendingDownload = null;
        while (GameCenter.curGameStage == null)
        {
            yield return null;
        }
        if (_callback != null)
        {
            _callback(loginPlayer);
        }
    }





    protected AssetMng.DownloadID Create(PlayerBaseInfo _info, System.Action<LoginPlayer, EResult> _callback)
    {
        return exResources.GetRace(_info.Prof, delegate(GameObject _asset,  EResult _result)
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
            this.gameObject.name = "LoginPlayer[" + _info.ServerInstanceID + "]";
            GameObject newGO = Instantiate(_asset) as GameObject;
            newGO.name = _asset.name;
            newGO.transform.parent = this.gameObject.transform;
            newGO.transform.localEulerAngles = Vector3.zero;
            newGO.transform.localPosition = Vector3.zero;
            newGO.AddComponent<PlayerRendererCtrl>();
            newGO.AddComponent<PlayerAnimFSM>();
            this.gameObject.AddComponent<FXCtrl>();

            CharacterController charCtrl = newGO.GetComponent<CharacterController>();
            if (charCtrl) GameObject.Destroy(charCtrl);

            animationRoot = newGO.transform;
            isDummy_ = false;

            Init();
            _callback(this, _result);
        });
    }






    protected override void Init()
    {
        base.Init();
        Invoke("StartRenderToCamera", 0.5f);
        rendererCtrl.ActivateCollider(false);
        ActiveBoxCollider(false,actorInfo.ColliderRadius);
        inited_ = true;

        if (animFSM != null)
        {
            InitAnimation();
        }
        else
        {
            GameSys.LogError(" 登录角色" + actorInfo.ServerInstanceID + " 启动时找不到动画控制器！");
        }
        if (fxCtrl != null)
        {
            fxCtrl.DoShadowEffect(true);
        }
    }




    protected void StartRenderToCamera()
    {
        //rendererCtrl.SetLayer(LayerMask.NameToLayer("NGUI3D"));
        this.gameObject.SetMaskLayer(LayerMask.NameToLayer("NGUI3D"));
    }

    /// <summary>
    /// 勿删，Start（）实际在dummy状态被调用，要在启动时做的事情请去自定义的Init()接口 by吴江
    /// </summary>
    protected void Start()
    {
    }


    /// <summary>
    /// 勿删，覆盖掉基类的每帧更新，登录界面的人物不需要进行每帧逻辑 by吴江
    /// </summary>
    protected new void Update()
    {
    }




    public int LastTime
    {
        get
        {
            return 0;
        }
    }


    public int RemoveTime
    {
        get
        {
            return 0;
        }
        set
        {
        }
    }
}
