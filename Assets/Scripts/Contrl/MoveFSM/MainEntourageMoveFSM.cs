//===================================
//作者：吴江
//日期：2015/6/19
//用途：主玩家随从的移动控制状态机
//=====================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 主玩家随从的移动控制状态机 by吴江
/// </summary>
public class MainEntourageMoveFSM : ActorMoveFSM
{

    protected MercenaryInfo refInfo = null;


    public void SetInfo(MercenaryInfo _refInfo)
    {
        refInfo = _refInfo;
    }


    public override void MoveTo(Vector3[] _path, float _rotY = 0, bool _needApplyStopDir = false)
    {
        if (!GameCenter.sceneMng.EnterSucceed) return;
        if (lockMoving)
        {
            return;
        }
        base.MoveTo(_path, _rotY, _needApplyStopDir);
        MainEntourageMoveTo(_path, _rotY, _needApplyStopDir);
    }



    /// <summary>
    /// 主玩家的移动 by吴江
    /// </summary>
    /// <param name="_actor"></param>
    /// <param name="_pos"></param>
    /// <param name="_maxDistance"></param>
    /// <param name="_noStop"></param>
    public void MainEntourageMoveTo(Vector3[] _path, float _rotY = 0, bool _needApplyStopDir = false)
    {
        GameCenter.mainPlayerMng.C2S_Move(ObjectType.Entourage, refInfo.ServerInstanceID, this.transform.position, _path,true, (int)transform.localEulerAngles.y);
    }




    /// <summary>
    ///
    /// </summary>
    public override void StopMovingTo()
    {
        if (!GameCenter.sceneMng.EnterSucceed) return;

        GameCenter.mainPlayerMng.C2S_Move(ObjectType.Entourage, refInfo.ServerInstanceID, transform.position, null, true,(int)transform.localEulerAngles.y);
        base.StopMovingTo();
        destPos = transform.position;
    }
}
