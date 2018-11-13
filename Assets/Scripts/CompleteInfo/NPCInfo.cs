//====================================
//作者：吴江
//日期：2015/5/22
//用途: NPC数据层对象（Info结尾的类名都为数据层对象，包含 服务端数据  客户端静态数据   访问器 三部分）
//=====================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class NpcData : ActorData
{
    public NpcData()
    {
    }
}



public class NPCInfo : ActorInfo
{

    /// <summary>
    /// 服务端数据
    /// </summary>
    protected new NpcData serverData
    {
        get { return base.serverData as NpcData; }
        set
        {
            base.serverData = value;
        }
    }

    /// <summary>
    /// 实例数据
    /// </summary>
    protected SceneNPCRef instanceRef = null;
    protected SceneNPCRef InstanceRef
    {
        get
        {
            return instanceRef;
        }
    }
    /// <summary>
    /// AI数据
    /// </summary>
    protected NPCAIRef aiRef = null;
    protected NPCAIRef AIRef
    {
        get
        {
            return aiRef;
        }
    }


    /// <summary>
    /// 类型数据
    /// </summary>
    protected NPCTypeRef typeRef = null;
    protected NPCTypeRef TypeRef
    {
        get
        {
            if (typeRef == null || typeRef.id != serverData.prof)
            {
                typeRef = ConfigMng.Instance.GetNPCTypeRef((int)serverData.prof);
            }
            return typeRef;
        }
    }




    public NPCInfo(SceneNPCRef _refData,NPCAIRef _aiRef)
    {
        instanceRef = _refData;
        serverData = new NpcData();
        serverData.serverInstanceID = _refData.id;

        aiRef = _aiRef;
        serverData.camp = _aiRef.camp;
        serverData.startPosX = _aiRef.sceneX;
        serverData.startPosZ = _aiRef.sceneY;
        serverData.dir = _aiRef.scenePoint;
        serverData.prof = _aiRef.npcId;

        ProcessServerData(serverData);

    }


    public NPCInfo(NPCTypeRef _refData)
    {
        serverData = new NpcData();
        serverData.serverInstanceID = -1;
        serverData.prof = _refData.id;
        serverData.startPosX = -10000;
        serverData.startPosY = -10000;
        serverData.startPosZ = -10000;
        serverData.dir = 0;
        serverData.camp = 0;

        ProcessServerData(serverData);

    }

//    public NPCInfo(SceneAnimActionRef _refData)
//    {
//        serverData = new NpcData();
//		serverData.serverInstanceID = _refData.targetInstanceID;
//        serverData.prof = _refData.targetConfigID;
//		serverData.startPosX = _refData.values[0];
//		serverData.startPosZ = _refData.values[1];
//        serverData.dir = _refData.values[2];
//        serverData.camp = _refData.values[3];
//    }


    public void UpdateAI(NPCAIRef _aiRef)
    {
        aiRef = _aiRef;
        serverData.camp = _aiRef.camp;
        serverData.startPosX = _aiRef.sceneX;
        serverData.startPosZ = _aiRef.sceneY;
        serverData.dir = _aiRef.scenePoint;
        serverData.prof = _aiRef.npcId;

        if (OnAiUpdate != null)
        {
            OnAiUpdate();
        }
    }

    public System.Action OnAiUpdate;



    protected override void ProcessServerData(ActorData _data)
    {
        List<EquipmentInfo> curEquipList = new List<EquipmentInfo>();

        for (int i = 0; i < TypeRef.equipList.Count; i++)
        {
            EquipmentInfo eq = new EquipmentInfo(TypeRef.equipList[i], EquipmentBelongTo.PREVIEW);
            DefaultDictionary[eq.Slot] = eq;
        }

        UpadateEquipments(curEquipList);
    }

    #region 访问器
    /// <summary>
    /// 预览时的缩放比
    /// </summary>
    public float PreviewScale(PreviewConfigType _previewType)
    {
        if (TypeRef != null)
        {
            switch (_previewType)
            {
                case PreviewConfigType.Dialog:
                    return TypeRef.preview_scale;
                case PreviewConfigType.Task:
                    return TypeRef.taskPreviewScale;
                default:
                    return 1;
            }
        }
        return 1;
    }
    /// <summary>
    /// 预览时的相机距离
    /// </summary>
    public Vector3 PreviewPosition(PreviewConfigType _previewType)
    {
        if (TypeRef != null)
        {
            switch (_previewType)
            {
                case PreviewConfigType.Dialog:
                    return TypeRef.previewPscale;
                case PreviewConfigType.Task:
                    return TypeRef.taskpreviewPscale;
                default:
                    return Vector3.zero;
            }
        }
        return Vector3.zero;

    }
    /// <summary>
    /// 预览时的相机角度
    /// </summary>
    public Vector3 PreviewRotation(PreviewConfigType _previewType)
    {
        if (TypeRef != null)
        {
            switch (_previewType)
            {
                case PreviewConfigType.Dialog:
                    return TypeRef.previewRscale;
                case PreviewConfigType.Task:
                    return TypeRef.taskpreviewRscale;
                default:
                    return Vector3.zero;
            }
        }
        return Vector3.zero;
    }

    /// <summary>
    /// 相机关注的x方向
    /// </summary>
    public float FocusX
    {
        get
        {
            return instanceRef == null ? 0 : instanceRef.talkx;
        }
    }

    /// <summary>
    /// 相机关注的y方向
    /// </summary>
    public float FocusY
    {
        get
        {
            return instanceRef == null ? 0 : instanceRef.talky;
        }
    }

    /// <summary>
    /// 相机关注的距离
    /// </summary>
    public float FocusDistance
    {
        get
        {
            return instanceRef == null ? 0 : instanceRef.talkDistance;
        }
    }

    /// <summary>
    /// 行为循环类型
    /// </summary>
    public ActionType ActionModelType
    {
        get
        {
            return AIRef == null ? ActionType.NONE : AIRef.sort;
        }
    }
    /// <summary>
    /// 行为延迟参数
    /// </summary>
    public float ActionDelayTime
    {
        get
        {
            return AIRef == null ? 0 : AIRef.actionData / 1000f;
        }
    }
    /// <summary>
    /// 是否为智能NPC
    /// </summary>
    public bool IsSmart
    {
        get { return AIRef == null ? false : AIRef.isSmart; }
    }
    /// <summary>
    /// 当前NPC的行为类型
    /// </summary>
    public ActionType ActionType
    {
        get { return AIRef == null || !IsSmart ? ActionType.NONE : AIRef.sort; }
    }
    /// <summary>
    /// 当前NPC的行为列表
    /// </summary>
    public List<int> ActionList
    {
        get { return AIRef == null ? new List<int>() : AIRef.npcAction; }
    }
    /// <summary>
    /// 名字
    /// </summary>
    public new string Name
    {
        get
        {
            return TypeRef == null ? string.Empty : TypeRef.name;
        }
    }
	
	/// <summary>
	/// 静态表id
	/// </summary>
	public int RefID
	{
		get
		{
			return TypeRef == null ? 0 : TypeRef.id;
		}
	}
    /// <summary>
    /// 头像 by 贺丰
    /// </summary>
    public string IconName
    {
        get
        {
            return TypeRef == null ? string.Empty : TypeRef.res;
        }
    }
    /// <summary>
    /// 当前AI序号
    /// </summary>
    public int CurAiID
    {
        get
        {
            return AIRef == null ? -1 : AIRef.id;
        }
    }
    /// <summary>
    /// NPCAI 类型名字 by龙英杰
    /// </summary>
    public string AINpcDesName
    {
        get
        {
            return AIRef == null ? string.Empty : AIRef.desName;
        }
    }
    /// <summary>
    /// 类型
    /// </summary>
    public int Type
    {
        get {
            return serverData.prof;
        }
    }
    /// <summary>
    /// 互动音效 
    /// </summary>
    public new string ActionIngSoundRes
    {
        get
        {
            return TypeRef == null ? string.Empty : TypeRef.wav_name;
        }
    }
    /// <summary>
    /// 模型缩放
    /// </summary>
    public float ModulScale
    {
        get
        {
            return TypeRef == null ? 1.0f : TypeRef.npcSize;
        }
    }

    /// <summary>
    /// 高度
    /// </summary>
    public float Hight
    {
        get { return TypeRef.height; }
    }
	/// <summary>
	/// 功能 by 
	/// </value>
	public int Function
    {
        get { return TypeRef.function1; }
    }
	/// <summary>
	/// 功能 by 
	/// </value>
	public int FunctionNext
    {
        get { return TypeRef.function2; }
    }
	/// <summary>
	/// 对话 by 
	/// </value>
	public string Talk
    {
        get { return TypeRef.talk; }
    }
    /// <summary>
    /// 配置移动速度
    /// </summary>
    public float StaticMoveSpeed
    {
        get
        {
            return AIRef == null ? 0 : AIRef.moveSpeed;
        }
    }
    /// <summary>
    /// 动画资源原移动速率(unity单位/秒)
    /// </summary>
    public float ModelMoveScale
    {
        get
        {
            return AIRef == null ? 0 : AIRef.paceSpeed;
        }
    }
	/// <summary>
	/// 头像 by 
	/// </value>
	public string Tip
    {
        get { return TypeRef.res; }
    }
	/// <summary>
	/// 职称 by 
	/// </value>
	public string Title
    {
        get { return TypeRef.title; }
    }
	/// <summary>
    /// 声音
	/// </summary>
	public string SoundName
    {
        get { return TypeRef.wav_name; }
    }
    /// <summary>
    /// 资源名称
    /// </summary>
    public string AssetName
    {
        get { return TypeRef == null ? string.Empty : TypeRef.res_name; }
    }
    /// <summary>
    /// 资源类型
    /// </summary>
    public AssetPathType AssetType
    {
        get { return TypeRef == null ? AssetPathType.PersistentDataPath : (AssetPathType)TypeRef.path_type; }
    }
    protected BubbleRef clickBubble;
    /// <summary>
    /// 点击时的应该冒泡的内容 by吴江
    /// </summary>
    public BubbleRef ClickBubble
    {
        get
        {
            if(clickBubble == null)
            {
            //    clickBubble = InstanceRef == null ? null : ConfigMng.Instance.GetBubbleRef(InstanceRef.bubbleID);
            }
            return clickBubble;
        }
    }
    #endregion


}
