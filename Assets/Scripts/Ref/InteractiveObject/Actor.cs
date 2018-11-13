///////////////////////////////////////////////////////////////////////////////
//作者：吴江
//日期：2015/5/5
//用途：游戏中活动对象的基类
///////////////////////////////////////////////////////////////////////////////


using UnityEngine;
using System.Collections;
using System.Collections.Generic;




/// <summary>
/// 游戏中活动对象的基类 by吴江
/// </summary>
public class Actor : InteractiveObject
{

    #region 数据
    protected ActorInfo actorInfo = null;

    ///<summary>
    ///对象渲染控制器 by吴江
    ///</summary>
    [System.NonSerialized]
    public new ActorRendererCtrl rendererCtrl = null;

    /// <summary>
    ///对象动作控制器 by吴江
    ///</summary>
    [System.NonSerialized]
    protected ActorAnimFSM animFSM = null;
    public ActorAnimFSM GetAnimFSM() { return animFSM; }

    /// <summary>
    ///对象移动控制器 by吴江
    ///</summary>
    [System.NonSerialized]
    protected ActorMoveFSM moveFSM = null;
    public ActorMoveFSM GetMoveFSM() { return moveFSM; }

    /// <summary>
    /// 模型对象宽度（针对模版模型） by吴江
    /// </summary>
    public float radius = 1.0f;
    /// <summary>
    /// 被击点 by吴江
    /// </summary>
    protected Transform hitPoint = null;
    /// <summary>
    /// 被击点 by吴江
    /// </summary>
    public Transform HitPoint
    {
        get
        {
            if (hitPoint == null)
            {
                hitPoint = MeshHelper.GetBone(this.transform, "hitPoint");
                if (hitPoint == null)
                {
                    hitPoint = MeshHelper.GetBone(this.transform, "hitpoint");
                    if (hitPoint == null)
                    {
                        hitPoint = MeshHelper.GetBone(this.transform, "HitPoint");
                    }
                }
            }
            if (hitPoint == null)
            {
                GameObject temp = new GameObject("hitPoint");
                temp.transform.parent = this.transform;
                temp.transform.localEulerAngles = Vector3.zero;
                hitPoint = temp.transform;
				if(hitPoint != null)
				{
					hitPoint.transform.localPosition = new Vector3(0, 1.2f, 0);
				}
            }
            return hitPoint;
        }
    }


    public int Camp
    {
        get
        {
            return actorInfo.Camp;
        }
    }

    /// <summary>
    /// 当前正在执行的移动路径  by吴江 
    /// </summary>
    public Vector3[] CurPath
    {
        get
        {
            if (moveFSM != null)
            {
                return moveFSM.path;
            }
            else
            {
                return null;
            }
        }
    }



    [HideInInspector]
    public Transform animationRoot = null;

    protected bool isDead_ = false;
    /// <summary>
    /// 是否已经死亡
    /// </summary>
    public bool isDead { get { return isDead_; } }

    protected bool isOutOfControl_ = false;
    /// <summary>
    /// 是否已经不受玩家控制
    /// </summary>
    public bool isOutOfControl { get { return isOutOfControl_; } }

    /// <summary>
    /// 移动基础系数 by吴江
    /// </summary>
    public const float MOVE_SPEED_BASE = 1.0f;



    public float GetMoveSpeed() { return curMoveSpeed; }

    /// <summary>
    /// 当前无buff状态下的移动速度
    /// </summary>
    protected float curMoveSpeed = 6.0f;


    /// <summary>
    /// 当前真实使用中的移动速度 by吴江
    /// </summary>
    public float CurRealSpeed
    {
        get
        {
            if (moveFSM != null)
            {
                return moveFSM.curRealSpeed;
            }
            return 0;
        }
        set
        {
            if (moveFSM != null)
            {
                moveFSM.curRealSpeed = value;
            }
            else
            {
                Debug.LogError("找不到移动组件！无法设置速度！");
            }
            if (animFSM != null)
            {
                animFSM.SetMoveSpeed(value / (actorInfo.AnimationMoveSpeedBase * actorInfo.ModelScale));
            }
            OnRealSpeedUpdate();
        }
    }
    #endregion

    #region 构造
    /// <summary>
    /// 尽量避免使用Awake等unity控制流程的接口来初始化，而改用自己调用的接口 by吴江
    /// </summary>
    protected override void Init()
    {
        base.Init();
        if (moveFSM == null)
        {
            moveFSM = this.gameObject.GetComponent<ActorMoveFSM>();
        }
        if (moveFSM != null && isShowing)
        {
            moveFSM.IsDummy = false;
        }
        animFSM = this.gameObject.GetComponentInChildrenFast<ActorAnimFSM>();
        rendererCtrl = base.rendererCtrl as ActorRendererCtrl;
        headTextCtrl = this.gameObject.GetComponentInChildrenFast<HeadTextCtrl>();

        RegistMoveEvent(false);
        RegistMoveEvent(true);
        Regist();
    }

    protected virtual void RegistMoveEvent(bool _regist)
    {
        moveFSM = this.gameObject.GetComponent<ActorMoveFSM>();
        if (moveFSM == null)
        {
            return;
        }
        if (_regist)
        {
            moveFSM.OnPositionChange += PositionChange;
            moveFSM.OnMoveStart += MoveStart;
            moveFSM.OnMoveEnd += MoveEnd;
			moveFSM.OnAbilityMoveEnd += AbilityMoveEnd;
        }
        else
        {
            moveFSM.OnPositionChange -= PositionChange;
            moveFSM.OnMoveStart -= MoveStart;
            moveFSM.OnMoveEnd -= MoveEnd;
			moveFSM.OnAbilityMoveEnd += AbilityMoveEnd;
        }
    }
    #endregion

    #region 状态机
    /// <summary>
    /// 游戏状态枚举 大部分对象都永不上，不用关注这里
    /// 这里是预留给以后的扩展，比如对象的变形，完全的逻辑切换。by吴江
    /// </summary>
    public enum EventType
    {
        NORMAL = fsm.Event.USER_FIELD + 1,
    }

    protected fsm.State normal;

    protected EventType curEventType = EventType.NORMAL;

    /// <summary>
    /// 初始化状态机 by吴江
    /// </summary>
    protected override void InitStateMachine()
    {
        normal = new fsm.State("normal", stateMachine);

        stateMachine.initState = normal;
    }


    #endregion

    #region UNITY
    void Start()
    {
        //stateMachine.Start();
    }

    protected void Update()
    {
        // stateMachine.Update();
    }

    protected void LateUpdate()
    {
    }

    void OnDisable()
    {
        if (typeID != ObjectType.NPC && GameCenter.curMainPlayer != null && GameCenter.curMainPlayer.CurTarget == this)
        {
            GameCenter.curMainPlayer.CurTarget = null;
        }
    }
    #endregion

    #region 辅助逻辑
    protected virtual void OnRealSpeedUpdate()
    {
    }
    /// <summary>
    /// 注册事件
    /// </summary>
    protected override void Regist()
    {
        base.Regist();
        if (actorInfo != null)
        {
            actorInfo.OnHideUpdate += OnHideUpdate;
        }

    }
    /// <summary>
    /// 注销事件
    /// </summary>
    public override void UnRegist()
    {
        base.UnRegist();
        if (actorInfo != null)
        {
            actorInfo.OnHideUpdate -= OnHideUpdate;
        }

    }


    public void OnHideUpdate()
    {
        if (actorInfo != null)
        {
            if (IsHide != actorInfo.IsHide)
            {
//                if (typeID != ObjectType.MOB && headTextCtrl != null)
//                {
//                    headTextCtrl.HideBlood();
//                }
                IsHide = actorInfo.IsHide;
                if (!IsHide)
                {
                    GameCenter.curGameStage.PlaceGameObjectFromServer(this, actorInfo.ServerPos.x, actorInfo.ServerPos.z, actorInfo.RotationY);
                }
                if (IsHide && !actorInfo.IsAlive)
                {
                   // Destroy(this);  
                }
            }
        }
    }


    #region 朝向
    /// <summary>
    /// 朝向某个方向，直接赋Y值的重载 by吴江
    /// </summary>
    /// <param name="_eulerAngleY"></param>
    public void FaceTo(float _eulerAngleY){if (moveFSM != null){ moveFSM.FaceTo(_eulerAngleY);}}
    /// <summary>
    /// 看向某个点 by吴江
    /// </summary>
    /// <param name="_dir"></param>
    public void FaceTo(Vector3 _dir) {if (moveFSM != null){moveFSM.FaceTo(_dir); } }
    /// <summary>
    /// 直接看向某个方向，直接赋y值并且不经过时间插值的重载 by吴江
    /// </summary>
    /// <param name="_eulerAngleY"></param>
    public void FaceToNoLerp(float _eulerAngleY) {
        if (moveFSM != null)
        {
            moveFSM.FaceToNoLerp(_eulerAngleY);
        }
        else
        {
            this.gameObject.transform.localEulerAngles = new Vector3(0, _eulerAngleY, 0);
        }
    }
    /// <summary>
    /// 直接看向某个点，不经过时间插值的重载 by吴江
    /// </summary>
    /// <param name="_dir"></param>
    public void FaceToNoLerp(Vector3 _dir)
    {
        if (moveFSM != null)
        {
            moveFSM.FaceToNoLerp(_dir);
        }
    }
    #endregion

    #region 移动
    /// <summary>
    /// 位置有变化了，应当通知控制台坐标变化
    /// </summary>
    public virtual void PositionChange()
    {
        if (gameStage != null)
        {
            gameStage.OnPositionChanged(this);
        }
    }

	public virtual void AbilityMoveEnd(AbilityMoveData _abilityMoveData)
	{
		
	}

    public bool CaculateSetorRange(Actor _target, int _range)
    {
        if(_target == null) return false;
        if(Mathf.Abs(curSector.r -_target.curSector.r) <=  _range && Mathf.Abs(curSector.c -_target.curSector.c) <=  _range)
        {
            return true;
        }
        return false;
    }


    /// <summary>
    /// 展示/隐藏基本模型
    /// </summary>
    /// <param name="_show"></param>
    public virtual void Show(bool _show)
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
        if (moveFSM != null)
        {
            moveFSM.IsDummy = !_show;
        }
        if (headTextCtrl != null) headTextCtrl.SetFlagsActive(_show);
        if (mouseCollider != null) mouseCollider.enabled = _show;
    }

    /// <summary>
    /// 开始移动，执行移动动画
    /// </summary>
    public virtual void MoveStart()
    {
        if (animFSM)
        {
            animFSM.SetMoveSpeed(CurRealSpeed / (actorInfo.AnimationMoveSpeedBase * actorInfo.ModelScale));
            animFSM.Move();
        }
    }
    /// <summary>
    /// 移动结束，执行站立动画
    /// </summary>
    public virtual void MoveEnd()
    {
        if (animFSM)
        {
            animFSM.StopMoving();
        }
    }
    /// <summary>
    /// 是否正在移动坐标
    /// </summary>
    [HideInInspector]
    public bool IsMoving { get { if (moveFSM != null) { return moveFSM.isMoving; } return false; } }
    /// <summary>
    /// 是否正在路径移动
    /// </summary>
    public bool isPathMoving
    {
          get { if (moveFSM != null) { return moveFSM.isMovingTo; } return false; } 
    }
    public bool isMovingTowards
    {
        get { if (moveFSM != null) { return moveFSM.isMovingTowards; } return false; } 
    }
    /// <summary>
    /// 是否已经被禁止移动
    /// </summary>
    public bool isMoveLocked {get { if (moveFSM != null) {return moveFSM.isMoveLocked; } return true; } }
    /// <summary>
    /// 停止移动 by吴江
    /// </summary>
    public virtual void StopMovingTo() {if (moveFSM != null) {moveFSM.StopMovingTo(); } }
    /// <summary>
    /// 移动到指定终点  by吴江
    /// </summary>
    /// <param name="_pos"></param>
    /// <param name="_maxDistance"></param>
    /// <param name="_noStop"></param>
    public virtual void MoveTo(Vector3 _pos, float _maxDistance = 10.0f, bool _noStop = false) { 
        if (moveFSM != null){ 
        moveFSM.MoveTo(_pos, _maxDistance, _noStop); }
    }


    public virtual void ChangeTarget()
    {
    }
    /// <summary>
    /// 根据指定路径移动 by吴江
    /// </summary>
    /// <param name="_path"></param>
    public virtual void MoveTo(Vector3[] _path, float _rotY = 0, bool _faceWhenStop = false) {
        if (moveFSM != null)
        {
                moveFSM.MoveTo(_path, _rotY, _faceWhenStop);
        }
    }
    /// <summary>
    /// 停止方向移动（一般用于键盘的asdw操作，手游暂时用不到） by吴江
    /// </summary>
    public void StopMovingTowards(){if (moveFSM != null){ moveFSM.StopMovingTowards();} }
    /// <summary>
    /// 直接朝某个方向行走（一般用于键盘的asdw操作，手游暂时用不到） by吴江
    /// </summary>
    /// <param name="_dir"></param>
    public virtual void MoveTowards(Vector3 _dir){ if (moveFSM != null) moveFSM.MoveTowards(_dir); }
    #endregion


    /// <summary>
    /// 泡泡说话
    /// </summary>
    /// <param name="_text"></param>
    /// <param name="_time"></param>
    public virtual void BubbleTalk(string _text, float _time)
    {
        if (headTextCtrl == null)
        {
            headTextCtrl = this.gameObject.GetComponent<HeadTextCtrl>();
        }
        if (headTextCtrl == null)
        {
            headTextCtrl = this.gameObject.AddComponent<HeadTextCtrl>();
        }
        if (headTextCtrl != null)
        {
            headTextCtrl.SetBubble(_text, _time);
        }
    }

    //public bool CheckCapsule(float _radius)
    //{
    //    int layerMask = LayerMng.playerMoveLayerMask;
    //    return Physics.CheckCapsule(transform.position,
    //                                 transform.position + new Vector3(0.0f, height, 0.0f),
    //                                 _radius,
    //                                 layerMask);
    //}
    #endregion
}
