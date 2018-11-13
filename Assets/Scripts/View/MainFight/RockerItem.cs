//==============================================
//作者：邓成(接手)
//日期：2015/8/16
//用途：摇杆功能
//=================================================


using UnityEngine;
using System.Collections;

public class RockerItem : MonoBehaviour
{
    #region UI控件对象
    /// <summary>
    /// 背景
    /// </summary>
    public UISprite BackGround;
    /// <summary>
    /// 控制点
    /// </summary>
    public UISprite Rocker;

    #endregion
    /// <summary>
    /// 灵敏度
    /// </summary>
    public float sensitive = 1.0f;
    /// <summary>
    /// 摇杆移动范围
    /// </summary>
    public int Range = 82;
    /// <summary>
    /// 移动目标相对位置
    /// </summary>
    private Vector2 GoalPos = new Vector2(0,0);
    private bool first = true;
    /// <summary>
    /// 摇杆是否移动
    /// </summary>
    private bool isRockMove = false;
    private bool isRockMoveLastFrame = false;
    public static System.Action<bool> OnDragStateChange;
    private Vector3 localPos = new Vector3(0,0,0);
    public delegate void MoveRocker(Vector2 detla);

    protected MainPlayer curMainPlayer;

    void Update()
    {
        if (isRockMove)
        {
            if (Input.touchCount == 0)
            {
                if (first)
                {
                    localPos = Input.mousePosition;
                    first = false;
                }
                OnDragging(sensitive * (Input.mousePosition - localPos));
                localPos = Input.mousePosition;
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                if (first)
                {
                    localPos = Input.GetTouch(0).position;
                    first = false;
                }
                Vector3 temp = Input.GetTouch(0).position;
                OnDragging(sensitive * (temp - localPos));
                localPos = Input.GetTouch(0).position;
            }
        }
        if (isRockMoveLastFrame != isRockMove)
        {
            PlayerInputListener.isDragingRockerItem = isRockMove;
            if (OnDragStateChange != null)
            {
                OnDragStateChange(isRockMove);
            }
            isRockMoveLastFrame = isRockMove;
            if (!isRockMove)
            {
                GameCenter.curMainPlayer.StopMovingTowards();
            }
        }
    }
    void OnPress(bool _bool)
    {
        isRockMove = _bool;
        GoalPos = new Vector2(0, 0);
        if (_bool)
        {
            first = true;
            PlayerInputListener.isDragingRockerItem = true;
            GameCenter.curMainPlayer.StopMovingTo();
            if (GameCenter.curMainPlayer.CurFSMState == MainPlayer.EventType.AI_FIGHT_CTRL) //应策划要求，自动战斗时的点击地面，不能直接跳转到普通，而只是临时执行。
            {
                GameCenter.curMainPlayer.BreakAutoFight();
            }
            else
            {
                GameCenter.curMainPlayer.GoNormal();
            }
            GameCenter.curMainPlayer.CancelCommands();
            if (Rocker != null)
            {
                Rocker.alpha = 1;
            }
        }
        else
        {
            ReturnPos();
			localPos.Set(0,0,0);
            if (Rocker != null)
            {
                Rocker.alpha = 0.5f;
            }
        }
    }



    protected Transform cameraTransform = null;

    void OnDragging(Vector2 delta)
    {
        if (curMainPlayer == null || curMainPlayer.isMoveLocked || curMainPlayer.isRigidity) return;
        if (curMainPlayer.isCasting || curMainPlayer.isCastingAttackEffect)
        {
            curMainPlayer.CancelAbility();
        }
        if (isRockMove && Rocker != null && BackGround != null)
        {
            GoalPos += delta;
            Vector2 nowPos = GoalPos;
            if (GoalPos.magnitude > Range)
            {
                nowPos = Range * GoalPos.normalized;
            }
            Rocker.gameObject.transform.localPosition = nowPos;
            if (cameraTransform == null)
            {
                cameraTransform = GameCenter.cameraMng.mainCamera.transform;
            }
            Vector3 dir = nowPos.y * cameraTransform.forward + nowPos.x * cameraTransform.right;
            dir.y = 0.0f;
            dir = dir.normalized;
            if (GameCenter.curMainPlayer.isPathMoving)
            {
                GameCenter.curMainPlayer.StopMovingTo();
            }
            if (dir == Vector3.zero)
            {
                GameCenter.curMainPlayer.StopMovingTowards();
            }
            else
            {
                GameCenter.curMainPlayer.MoveTowards(dir);
            }
            //if (GameCenter.curMainPlayer.IsMoving)
            //{
            //   GameCenter.curMainPlayer.MoveToBy8Dir(ActorMoveFSM.TranslateTo8Dir(nowPos));
            //}
            //else
            //{
            //    GameCenter.curMainPlayer.ForceMoveToBy8Dir(ActorMoveFSM.TranslateTo8Dir(nowPos));
            //}
        }
        PlayerInputListener.isDragingRockerItem = true;

    }
    void Awake()
    {
        curMainPlayer = GameCenter.curMainPlayer;
    }


    void OnDisable()
    {
        ReturnPos();
        isRockMove = false;
        PlayerInputListener.isDragingRockerItem = false;
    }

    void OnEnable()
    {
        Rocker.transform.localPosition = Vector3.zero;
    }
    //void OnDragBegin(GameObject obj)
    //{
    //    //Rocker.gameObject.transform.localPosition = BackGround.gameObject.transform.localPosition + new Vector3(deta.x,deta.y,0);
    //    isRockMove = true;
    //    GoalPos = new Vector2(0, 0);
    //    PlayerInputListener.isDragingRockerItem = true;
    //    GameCenter.curMainPlayer.AttakType = MainPlayer.AttackType.NONE;
    //    GameCenter.curMainPlayer.GoNormal();
    //    GameCenter.curMainPlayer.commandMng.CancelCommands();
    //    if (Rocker != null)
    //    {
    //        Rocker.alpha = 1;
    //    }
        
    //}
    //void OnDragging(GameObject obj, Vector2 delta)
    //{
    //    if (isRockMove && Rocker != null && BackGround != null)
    //    {
    //        GoalPos += delta * 1.5f;
    //        Vector2 nowPos = GoalPos;
    //        if (GoalPos.magnitude > Range)
    //        {
    //            nowPos = Range * GoalPos.normalized;
    //        }
    //        Rocker.gameObject.transform.localPosition = nowPos;
    //        GameCenter.curMainPlayer.MoveToBy8Dir(ActorMoveFSM.TranslateTo8Dir(nowPos));
    //    }
    //    PlayerInputListener.isDragingRockerItem = true;
       
    //}
    //void OnDraggEnd(GameObject obj)
    //{
    //    isRockMove = false;
    //    GoalPos = new Vector2(0, 0);
    //    ReturnPos();
    //    if (Rocker != null)
    //    {
    //        Rocker.alpha = 0.5f;
    //    }
    //    GameCenter.curMainPlayer.StopMovingTo();
    //}

    protected bool isFirstTime = true;
    void ReturnPos()
    {
        if (isFirstTime)
        {
            isFirstTime = false;
            return;
        }
        if (Rocker != null)
        {
			iTween.MoveTo(Rocker.gameObject,transform.position, 0.2f);
        }
    }

    //void OnDoubleClick(GameObject obj)
    //{

    //    Debug.Log("跳跃！！");
    //    Jump();
    //}
    //protected void Jump()
    //{
    //    GameCenter.curMainPlayer.Jump();
    //}
}