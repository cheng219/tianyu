//=====================================
//作者:吴江
//日期:2015/9/28
//用途:场景物品数据层对象
//=======================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;




public class SceneItemInfo : ActorInfo
{
    /// <summary>
    /// 实例类型配置
    /// </summary>
    protected SceneItemDisRef disRefData = null;
    /// <summary>
    /// 实例类型配置
    /// </summary>
    protected SceneItemDisRef DisRefData
    {
        get
        {
            if (disRefData == null)
            {
                disRefData = ConfigMng.Instance.GetSceneItemDisRef((int)serverData.prof);
            }
            return disRefData;
        }
    }

    /// <summary>
    /// 类型配置
    /// </summary>
    protected SceneItemRef refData = null;
    /// <summary>
    /// 类型配置
    /// </summary>
    protected SceneItemRef RefData
    {
        get
        {
            if (refData == null)
            {
                refData = ConfigMng.Instance.GetSceneItemRef(DisRefData.sceneItemId);
            }
            return refData;
        }
    }


    public SceneItemInfo(st.net.NetBase.scene_item _serverData)
    {
        serverData = new ActorData();
        serverData.serverInstanceID = (int)_serverData.iid;
        serverData.prof = (int)_serverData.type;
        serverData.dir = (int)_serverData.dir;
        serverData.camp = (int)_serverData.camp;
        serverData.startPosX = _serverData.x;
        serverData.startPosY = _serverData.y;
        serverData.startPosZ = _serverData.z;
    }

    public void Update(st.net.NetBase.scene_item _serverData)
    {
        serverData.serverInstanceID = (int)_serverData.iid;
        serverData.prof = (int)_serverData.type;
        serverData.dir = (int)_serverData.dir;
        serverData.camp = (int)_serverData.camp;
        serverData.startPosX = _serverData.x;
        serverData.startPosY = _serverData.y;
        serverData.startPosZ = _serverData.z;
    }



    #region 访问器
    /// <summary>
    /// 配置ID by吴江
    /// </summary>
    public int ConfigID
    {
        get
        {
            return DisRefData == null ? -1 : DisRefData.sceneItemId;
        }
    }
    /// <summary>
    /// 服务端朝向 by吴江
    /// </summary>
    public int Dir
    {
        get
        {
            return (int)serverData.dir;
        }
    }
    /// <summary>
    /// 名字 by吴江
    /// </summary>
    public new string Name
    {
        get
        {
            return RefData == null ? string.Empty : RefData.name;
        }
    }
    /// <summary>
    /// 名字高度 by吴江
    /// </summary>
    public new float NameHeight
    {
        get
        {
            return RefData == null ? 2.0f : RefData.nameHeight;
        }
    }
    /// <summary>
    /// 模型缩放 by吴江
    /// </summary>
    public override float ModelScale
    {
        get
        {
            return DisRefData.modelSize;
        }
    }
    /// <summary>
    /// 泡泡说话内容
    /// </summary>
    protected BubbleRef bubbleContent;
    /// <summary>
    /// 泡泡说话内容
    /// </summary>
    public BubbleRef BubbleContent
    {
        get
        {
            if (bubbleContent == null)
            {
            //    bubbleContent = DisRefData == null || DisRefData.bubbleID <= 0 ? null : ConfigMng.Instance.GetBubbleRef(DisRefData.bubbleID);
            }
            return bubbleContent;
        }
    }
    /// <summary>
    /// 关联类型  by吴江
    /// </summary>
    public ConnectType ItemConnectType
    {
        get
        {
            return DisRefData == null ? ConnectType.NONE : DisRefData.connectType;
        }
    }
    /// <summary>
    /// 触发方式  by吴江
    /// </summary>
    public SceneFunctionType FunctionType
    {
        get
        {
            return DisRefData == null ? SceneFunctionType.NONE : DisRefData.sceneFunctionType;
        }
    }
    /// <summary>
    /// 碰撞体积 by吴江
    /// </summary>
    public Vector3 colliderScale
    {
        get
        {
            return RefData == null ? Vector3.zero : RefData.modelVec;
        }
    }
    /// <summary>
    /// 交互时的玩家动作 by吴江 
    /// </summary>
    public string PlayerAnimName
    {
        get
        {
            return RefData == null ? string.Empty : RefData.playerAction;
        }
    }
    /// <summary>
    /// 开启时间(进度条的读条时间) by吴江 
    /// </summary>
    public float OpenTime
    {
        get
        {
            return RefData == null ? 0 : RefData.openTime;
        }
    }
    /// <summary>
    /// 开启行为描述（进度条上的字） by吴江 
    /// </summary>
    public string OpenDescription
    {
        get
        {
            return RefData == null ? string.Empty : RefData.actionIngDec;
        }
    }
    /// <summary>
    /// 触发操作方式
    /// </summary>
    public TouchType ItemTouchType
    {
        get
        {
            return RefData == null ? TouchType.NONE : RefData.touchType;
        }
    }
    /// <summary>
    /// 互动音效 
    /// </summary>
    public new string ActionIngSoundRes
    {
        get
        {
            return RefData == null ? string.Empty : RefData.actionIngSoundRes;
        }
    }
    /// <summary>
    /// 互动特效
    /// </summary>
    public string ActioningEffectName
    {
        get
        {
            return RefData == null?string.Empty:RefData.actionIngDisresName;
        }
    }

    /// <summary>
    /// 结束死亡音效 
    /// </summary>
    public new string EndSoundRes
    {
        get
        {
            return RefData == null ? string.Empty : RefData.actionSoundRes;
        }
    }
    /// <summary>
    /// 死亡特效 by吴江
    /// </summary>
    public string DeadEffectName
    {
        get
        {
            return RefData == null ? string.Empty : RefData.actionDisresName;
        }
    }
    /// <summary>
    /// 死亡音效 by吴江
    /// </summary>
    public string DeadSoundName
    {
        get
        {
            return RefData == null ? string.Empty : RefData.actionSoundRes;
        }
    }
    /// <summary>
    /// 死亡动画 by吴江
    /// </summary>
    public string DeadAnimName
    {
        get
        {
            return RefData == null ? string.Empty : RefData.action;
        }
    }
    /// <summary>
    /// 死亡表现时长 by吴江
    /// </summary>
    public float DeadTime
    {
        get
        {
            return RefData == null ? 0 : RefData.actionTimes;
        }
    }
    /// <summary>
    /// 可用任务线
    /// </summary>
    public int TaskID
    {
        get
        {
            return DisRefData == null ? 0 : DisRefData.task;
        }
    }
    /// <summary>
    /// 开始步骤
    /// </summary>
    public int StartStep
    {
        get
        {
            return DisRefData == null ? 0 : (int)DisRefData.startStep;
        }
    }
    /// <summary>
    /// 开始状态要求
    /// </summary>
    public TaskStateType StartTaskStateType
    {
        get
        {
            return DisRefData == null ? TaskStateType.UnTake : DisRefData.startStepTime;
        }
    }
    /// <summary>
    /// 结束步骤
    /// </summary>
    public int EndStep
    {
        get
        {
            return DisRefData == null ? 0 : (int)DisRefData.overStep;
        }
    }
    /// <summary>
    /// 结束状态要求
    /// </summary>
    public TaskStateType EndTaskStateType
    {
        get
        {
            return DisRefData == null ? TaskStateType.UnTake : DisRefData.overStepTime;
        }
    }

    /// <summary>
    /// 骨骼特效列表
    /// </summary>
    public List<SceneItemEffect> BoneEffectList
    {
        get
        {
            return RefData == null ? new List<SceneItemEffect>() : RefData.sceneItemEffect;
        }
    }
    #endregion

}
