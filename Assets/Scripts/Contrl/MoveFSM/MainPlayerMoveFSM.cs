//===================================
//作者：吴江
//日期：2015/6/19
//用途：主玩家的移动控制状态机
//=====================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 主玩家的移动控制状态机 by吴江
/// </summary>
public class MainPlayerMoveFSM : ActorMoveFSM
{
    public override void MoveTo(Vector3[] _path, float _rotY = 0, bool _needApplyStopDir = false)
    {

        if (!GameCenter.sceneMng.EnterSucceed) return;
        if (lockMoving)
        {
            return;
        }
        base.MoveTo(_path, _rotY, _needApplyStopDir);
        MainPlayerMoveTo(_path, _rotY, _needApplyStopDir);
    }



    /// <summary>
    /// 主玩家的移动 by吴江
    /// </summary>
    /// <param name="_actor"></param>
    /// <param name="_pos"></param>
    /// <param name="_maxDistance"></param>
    /// <param name="_noStop"></param>
    public void MainPlayerMoveTo(Vector3[] _path, float _rotY = 0, bool _needApplyStopDir = false)
    {
        GameCenter.mainPlayerMng.C2S_Move(ObjectType.Player,GameCenter.mainPlayerMng.MainPlayerInfo.ServerInstanceID, 
            this.transform.position, _path, true,(int)transform.localEulerAngles.y);
    }




    /// <summary>
    /// 主玩家停止移动的重载。因为需要与服务端同步坐标，但是移动申请又会在服务端打断技能的释放。因此在这里判断，如果坐标已经一致，就不向服务端发送移动申请了。 by吴江
    /// </summary>
    public override void StopMovingTo()
    {
        if (!GameCenter.sceneMng.EnterSucceed) return;

        GameCenter.mainPlayerMng.C2S_Move(ObjectType.Player,GameCenter.mainPlayerMng.MainPlayerInfo.ServerInstanceID,
            transform.position, null,true, (int)transform.localEulerAngles.y);
        base.StopMovingTo();
        destPos = transform.position;
    }

    protected float lastTickTime = 0.0f;
    protected Vector3 lasrtDir = Vector3.zero;
    protected float diffValue = 3.0f;

    protected bool CompareTowardsDir(Vector3 _old, Vector3 _new)
    {
        if (_old.z == 0) _old.z = 0.00001f;
        if (_new.z == 0) _new.z = 0.00001f;
        float diff = Mathf.Abs(Mathf.Tan(Mathf.Abs(_old.x / _old.z)) - Mathf.Tan(Mathf.Abs(_new.x / _new.z)));
        return diff > diffValue;
    }
    /// <summary>
    /// 直接朝某个方向行走（一般用于键盘的asdw操作，手游暂时用不到） by吴江
    /// </summary>
    /// <param name="_dir"></param>
    public override void MoveTowards(Vector3 _dir)
    {
        base.MoveTowards(_dir);
        if (Time.time - lastTickTime > tickTime)
        {
            lastTickTime = Time.time;
            lasrtDir = _dir;
            path = new Vector3[2] { this.transform.position, destPos };
			//检查前方是否不可走
			Vector3 smallDir = new Vector3(_dir.x/2f,_dir.y/2f,_dir.z/2f);
			Vector3 newPos = LineCast(transform.position.SetY(-500) + smallDir, !IsDummy);
			NavMeshPath resultPath = new NavMeshPath();
			bool pathFound = NavMesh.CalculatePath(transform.position, newPos, NavMesh.AllAreas, resultPath);
			if (pathFound)
			{
           	 	GameCenter.mainPlayerMng.C2S_Move(ObjectType.Player, GameCenter.mainPlayerMng.MainPlayerInfo.ServerInstanceID, this.transform.position,
                	path,false, (int)transform.localEulerAngles.y);
			}
        }
        else if (Time.time - lastTickTime > diffTickTime && CompareTowardsDir(lasrtDir, _dir))
        {
            lastTickTime = Time.time;
            lasrtDir = _dir;
            path = new Vector3[2] { this.transform.position, destPos };
			//检查前方是否不可走
			Vector3 smallDir = new Vector3(_dir.x/2f,_dir.y/2f,_dir.z/2f);
			Vector3 newPos = LineCast(transform.position.SetY(-500) + smallDir, !IsDummy);
			NavMeshPath resultPath = new NavMeshPath();
			bool pathFound = NavMesh.CalculatePath(transform.position, newPos, NavMesh.AllAreas, resultPath);
			if (pathFound)
			{
	            GameCenter.mainPlayerMng.C2S_Move(ObjectType.Player, GameCenter.mainPlayerMng.MainPlayerInfo.ServerInstanceID, this.transform.position,
	                path, false, (int)transform.localEulerAngles.y);
			}
            //string dddd = string.Empty;
            //for (int i = 0; i < path.Length; i++)
            //{
            //    dddd += ("(" + path[i].x + "," + path[i].y + "," + path[i].z + ")");
            //}
            //Debug.logger.Log("CompareTowardsDir MoveTowards " + dddd);
        }
    }

    /// <summary>
    /// 停止方向移动（一般用于键盘的asdw操作，手游暂时用不到） by吴江
    /// </summary>
    public override void StopMovingTowards()
    {
        if (curEventType != EventType.KEYBOARDMOVE) return;
        base.StopMovingTowards();
        GameCenter.mainPlayerMng.C2S_Move(ObjectType.Player, GameCenter.mainPlayerMng.MainPlayerInfo.ServerInstanceID, 
            transform.position, null,false, (int)transform.localEulerAngles.y);
        destPos = transform.position;
        //Debug.logger.Log("StopMovingTowards " + (int)transform.localEulerAngles.y);
    }

}
