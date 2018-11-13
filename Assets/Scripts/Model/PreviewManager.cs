//=====================================
//作者：吴江
//日期：2015/6/9
//用途：预览模型的管理类
//===================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using st.net.NetBase;


public class PreviewManager
{
    public enum PreviewType
    {
        None,
        Player,
        Mob,
        Item,
        Scene,
        Mount,
    }

    protected Vector3 wholePos = new Vector3(0, 0, 4f);
    protected bool playerPosConfig = false;

    public Camera previewCamera;
    public PreviewPlayer previewCharacter;
	public PreviewMob previewMob;
    public PreviewEntourage previewEntourage;
    private RenderTexture previewRT;
    public void ReleasePreviewRT()
    {
        previewRT = null;
    }


    private UITexture curShowLabel = null;
    protected Dictionary<string, AssetMng.DownloadID> pendingDownloadList = new Dictionary<string, AssetMng.DownloadID>();

    private Vector3 currentPlayerGroupAngles = new Vector3(0, 0, 0);

    [System.NonSerialized] public Shader previewEffectShader = null;
    private float FieldOfView
    {
        set
        {
            if (value <= 0)
            {
                previewCamera.fieldOfView = 25.0f;
            }
            else
            {
                previewCamera.fieldOfView = value;
            }
        }
    }





    public static PreviewManager CreateNew()
    {
        if (GameCenter.previewManager == null)
        {
            PreviewManager previewManager = new PreviewManager();
            previewManager.Init();
            return previewManager;
        }
        else
        {
            GameCenter.previewManager.UnRegist();
            GameCenter.previewManager.Init();
            return GameCenter.previewManager;
        }
    }



    protected void Init()
    {
		MsgHander.Regist(0xD712, S2C_GotCharInfo);
		MsgHander.Regist(0xD713, S2C_OnGotPetInfo);
        MsgHander.Regist(0xD773, S2C_GotPlayerInfo);
    }


    protected void UnRegist()
    {
        MsgHander.UnRegist(0xD712, S2C_GotCharInfo);
		MsgHander.UnRegist(0xD713, S2C_OnGotPetInfo);
        MsgHander.UnRegist(0xD773, S2C_GotPlayerInfo);
    }



    #region 构造
    public PreviewManager()
    {
        previewCamera = GameCenter.cameraMng.previewCamera;
        if (null == previewCamera)
        {
            BuildCamera();
        }
        previewEffectShader = Shader.Find("ex2D/Alpha Blend Additive");
        GameCenter.cameraMng.PreviewCameraActive(false);
    }
    //
    //清理模型
    public void ClearModel()
    {
        currentPlayerGroupAngles = new Vector3(0, 0, 0);
        if (previewCamera != null && previewCamera.transform.childCount > 0)
        {
            Transform tsf = previewCamera.transform.FindChild("CtrlObj");
            if (tsf != null)
            {
                for (int i = 0; i < tsf.childCount; i++)
                {
                    UnityEngine.Object.Destroy(tsf.GetChild(i).gameObject);
                }
            }
            previewCharacter = null;
        }
    }

    public void ModelActive(bool _active)
    {
        if (previewCamera != null && previewCamera.transform.childCount > 0)
        {
            Transform tsf = previewCamera.transform.FindChild("CtrlObj");
            if (tsf != null)
            {
                tsf.gameObject.SetActive(_active);
            }
        }
    }


    public void BindRenderAndUI(UITexture _showLabel)
    {
        curShowLabel = _showLabel;
        if (null == _showLabel) return;
        Vector2 pos = new Vector2(_showLabel.width, _showLabel.height); 
        if (previewRT != null)
        {
            if (curShowLabel == _showLabel && previewRT.width == pos.x && previewRT.height == pos.y)
            {
                GameCenter.cameraMng.PreviewCameraActive(true);
                return;
            }
        }
        RenderTexture tex = previewCamera.targetTexture;
        previewCamera.targetTexture = null;
        if (tex != null) tex.Release();
        previewCamera.aspect = pos.x / pos.y;
        pos = CorrectRect(pos);
        previewRT = RenderTexture.GetTemporary((int)pos.x, (int)pos.y,1,RenderTextureFormat.ARGB32);
        previewRT.antiAliasing = 1;
        previewRT.wrapMode = TextureWrapMode.Clamp;
        previewRT.filterMode = FilterMode.Bilinear;
        previewRT.anisoLevel = 0;
        previewCamera.targetTexture = previewRT;
        _showLabel.mainTexture = (Texture)previewRT;
        _showLabel.enabled = true;
        _showLabel.gameObject.SetActive(true);
        GameCenter.cameraMng.PreviewCameraActive(true);
    }



    public Vector2 CorrectRect(Vector2 _oringin)
    {
        if (_oringin.x < 512 || _oringin.y < 512)
        {
            // 如果贴图太小，玩家放大分辨率时就会模糊，考虑改成换分辨率时重新创建RT
            _oringin.x = _oringin.x * 1.5f;
            _oringin.y = _oringin.y * 1.5f;
        }
        if (_oringin.x < 512 || _oringin.y < 512)
        {
            return CorrectRect(_oringin);
        }
        else
        {
            return _oringin;
        }
    }



    //创建相机
    private void BuildCamera()
    {
        //previewCamera = new Camera();
        //previewCamera.transform.localEulerAngles = new Vector3(0, 0, 0);
        //previewCamera.transform.localScale = new Vector3(1, 1, 1);
        //previewCamera.clearFlags = CameraClearFlags.SolidColor;
        //previewCamera.cullingMask = LayerMask.NameToLayer("Preview");
        //FieldOfView = 30f;
        //previewCamera.depth = 2;
        //previewCamera.enabled = false;
        //previewCamera.transform.parent = Camera.main.transform.root.gameObject.transform;
        //previewCamera.transform.localPosition = new Vector3(10000, 10000, 10000);
    }
    #endregion

    /// <summary>
    /// 获取展示全身的模型挂点 by吴江
    /// 
    /// 增加策划配置模型的localPosition与localRotation  09/04
    /// </summary>
    /// <returns></returns>
    public GameObject GetModelTransform(Vector3 position,Vector3 rotation)
    {
        Vector3 pos = wholePos;
        if (position != Vector3.zero)
        {
            pos = position;
        }
        previewCamera = GameCenter.cameraMng.previewCamera;
        if (previewCamera == null) return null;
        Transform tsf = previewCamera.transform.FindChild("CtrlObj");
        if (tsf == null)
        {
            GameObject ctrlObj = new GameObject();
            ctrlObj.transform.parent = previewCamera.transform;
            ctrlObj.name = "CtrlObj";
            ctrlObj.transform.localPosition = pos;
			if(rotation != Vector3.zero){
				currentPlayerGroupAngles = new Vector3((float)rotation.x,(float)rotation.y,(float)rotation.z);
			}
            ctrlObj.transform.localRotation = Quaternion.Euler(currentPlayerGroupAngles);
            ctrlObj.AddComponent<Actor>();
            return ctrlObj;
        }
        else
        {
            tsf.transform.localPosition = pos;
			if(rotation != Vector3.zero){
				currentPlayerGroupAngles = new Vector3((float)rotation.x,(float)rotation.y,(float)rotation.z);
			}
			tsf.transform.localRotation = Quaternion.Euler(currentPlayerGroupAngles);
            return tsf.gameObject;
        }
    }


    public void PushDownLoadTask(AssetMng.DownloadID _task)
    {
        if (_task == null || pendingDownloadList == null) return;
        if (pendingDownloadList.ContainsKey(_task.shortURL))
        {
            AssetMng.instance.CancelDownload(_task.shortURL);
            pendingDownloadList.Remove(_task.shortURL);
        }
        pendingDownloadList.Add(_task.shortURL, _task);
    }

    public void EndDownLoadTask(AssetMng.DownloadID _task)
    {
        if (_task == null || pendingDownloadList == null) return;
        if (pendingDownloadList.ContainsKey(_task.shortURL))
        {
            pendingDownloadList.Remove(_task.shortURL);
        }
    }
 
    public void CancelAllDownLoad()
    {
        if (pendingDownloadList.Count > 0)
        {
            foreach (var item in pendingDownloadList)
            {
                AssetMng.instance.CancelDownload(item.Key);
            }
            pendingDownloadList.Clear();
        }
    }


    #region 人物预览 by吴江
    /// <summary>
    /// 非经过我同意禁用本接口  by吴江
    /// </summary>
    /// <param name="_info"></param>
    /// <param name="_callBack"></param>
    /// <returns></returns>
    public bool TryPreviewSinglePlayer(PlayerBaseInfo _info, System.Action<PreviewPlayer> _callBack)
    {
        if (_info != null)
        {
            PreviewPlayer pp = PreviewPlayer.CreateDummy(_info);
            pp.mutualExclusion = false;
            pp.StartAsyncCreate(_callBack);
            return true;
        }
        return false;
    }
    /// <summary>
    /// 预览玩家 by吴江
    /// </summary>
    /// <param name="_info">玩家数据</param>
    /// <param name="_showLabel">展示ui</param>
    /// <param name="_tryShowEqList">试穿的装备列表</param>
    /// <param name="_randIdle">是否做随机动作</param>
    /// <returns></returns>
    public bool TryPreviewSinglePlayer(PlayerBaseInfo _info, UITexture _showLabel, Dictionary<EquipSlot, EquipmentInfo> _tryShowEqList = null, PlayerAnimFSM.EventType _type = ActorAnimFSM.EventType.Idle)
    {
        curShowLabel = _showLabel;
        playerPosConfig = true;
        CancelAllDownLoad();
        if (_info != null)
        {
            PreviewPlayer pp = PreviewPlayer.CreateDummy(_info, _tryShowEqList);
			pp.mutualExclusion = false;
            pp.needRandID = _type;
            pp.StartAsyncCreate(CreatePlayerCallBack);
            return true;
        }
        return false;
    }

    /// <summary>
    /// 预览玩家 by吴江
    /// </summary>
    /// <param name="_info">玩家数据</param>
    /// <param name="_showLabel">展示ui</param>
    /// <param name="_showCosmetic">是否展示时装</param>
    /// <returns></returns>
    public bool TryPreviewSinglePlayer(PlayerBaseInfo _info, UITexture _showLabel, bool _showCosmetic)
    {
        curShowLabel = _showLabel;
        playerPosConfig = true;
        CancelAllDownLoad();
        if (_info != null)
        {
            PreviewPlayer pp = PreviewPlayer.CreateDummy(_info, null);
			pp.mutualExclusion = false;
            pp.showCosmetic = _showCosmetic;
            pp.StartAsyncCreate(CreatePlayerCallBack);
            return true;
        }
        return false;
    }

    /// <summary>
    /// 预览玩家 
    /// <returns></returns>
    public bool TryPreviewSinglePlayer(PlayerBaseInfo _info, UITexture _showLabel, bool _showCosmetic,string idleAnimName)
    {
        curShowLabel = _showLabel;
        playerPosConfig = true;
        CancelAllDownLoad();
        if (_info != null)
        {
            PreviewPlayer pp = PreviewPlayer.CreateDummy(_info, null);
            pp.mutualExclusion = false;
            pp.showCosmetic = _showCosmetic;
            pp.idleAnimName = idleAnimName;
            pp.StartAsyncCreate(CreatePlayerCallBack);
            return true;
        }
        return false;
    }

    /// <summary>
    /// 为对话框做玩家预览 by吴江
    /// </summary>
    /// <param name="_info">玩家数据</param>
    /// <param name="_showLabel">展示UI</param>
    /// <param name="_randIdle">是否做随机动作</param>
    /// <returns></returns>
    public bool TryPreviewSinglePlayerForDialog(PlayerBaseInfo _info, UITexture _showLabel, PlayerAnimFSM.EventType _type = ActorAnimFSM.EventType.Idle)
    {
        curShowLabel = _showLabel;
        playerPosConfig = true;
        CancelAllDownLoad();
        if (_info != null)
        {
            PreviewPlayer pp = PreviewPlayer.CreateDummy(_info, null);
            pp.needRandID = _type;
            pp.StartAsyncCreate(CreatePlayerCallBack);
            return true;
        }
        return false;
    }



    public void CreatePlayerCallBack(PreviewPlayer _player)
    {
        //防止有之前延迟的模型加载，再次清理
        ClearModel();
        previewCharacter = _player;
        GameObject parent = playerPosConfig ? GetModelTransform(_player.PreviewPos, _player.PreviewRotation) : GetModelTransform(Vector3.zero, Vector3.zero);
        if (null != parent)
        {
            previewCharacter.transform.parent = parent.transform;
            previewCharacter.transform.localPosition = new Vector3(0, -(previewCharacter.Height/2f - 0.25f), 0);
        }
        previewCharacter.FaceToNoLerp(180);
        UIDragObjectRotate3D _UIDragObjectRotate3D = curShowLabel.gameObject.GetComponent<UIDragObjectRotate3D>();
        if (_UIDragObjectRotate3D == null)
        {
            _UIDragObjectRotate3D = curShowLabel.gameObject.AddComponent<UIDragObjectRotate3D>();
        }
        _UIDragObjectRotate3D.target = _player;
		_UIDragObjectRotate3D.ResetPosition();
        BindRenderAndUI(curShowLabel);
    }

    /// <summary>
    /// 当前试图查看的玩家ID
    /// </summary>
    protected int curWantToPreview = -1;
    /// <summary>
    /// 当前试图查看的玩家ID
    /// </summary>
    public int CurWantToPreview
    {
        get { return curWantToPreview; }
    }
    /// <summary>
    /// 本次向服务器请求获得的玩家数据
    /// </summary>
    protected PlayerBaseInfo curAskPlayerInfo;
    /// <summary>
    /// 本次向服务器请求获得的玩家数据
    /// </summary>
    public PlayerBaseInfo CurAskPlayerInfo
    {
        get { return curAskPlayerInfo; }
    }
    /// <summary>
    /// 本次向服务器请求获得的玩家遗物数据
    /// </summary>
    protected List<lost_item_info> relicList = new List<lost_item_info>();
    public List<lost_item_info> RelicList
    {
        get { return relicList; }
    }
    /// <summary>
    /// 本次向服务器请求获得的玩家佣兵数据
    /// </summary>
    protected List<entourage_info_list> entourageList = new List<entourage_info_list>();
    public List<entourage_info_list> EntourageList
    {
        get { return entourageList; }
    }
    /// <summary>
    /// 服务端返回请求的玩家数据的事件
    /// </summary>
    public Action OnGotCurAskPlayerInfo;
    /// <summary>
    /// 服务端返回请求排行榜的玩家数据的事件
    /// </summary>
    public System.Action OnGotCurAskRankPlayerInfo;
    /// <summary>
    /// 排行榜的查看其他玩家界面
    /// </summary>
    public System.Action OnGotRankOtherInfo;
    /// <summary>
    /// 排行榜的查看其他宠物或坐骑界面
    /// </summary>
    public System.Action OnGotRankOtherPetInfo;
    #endregion

    #region NPC预览 by吴江
    /// <summary>
    /// 非经过我同意禁用本接口  by吴江
    /// </summary>
    /// <param name="_info"></param>
    /// <param name="_callBack"></param>
    /// <returns></returns>
    public bool TryPreviewSingelNPC(int _npcTypeID, System.Action<PreviewNPC> _callBack)
    {
        NPCTypeRef refData = ConfigMng.Instance.GetNPCTypeRef(_npcTypeID);
        if (refData != null)
        {
            PreviewNPC pp = PreviewNPC.CreateDummy(refData);
            pp.mutualExclusion = false;
            pp.StartAsyncCreate(_callBack);
            return true;
        }
        return false;
    }
    public bool TryPreviewSingelNPC(int _npcTypeID, UITexture _showLabel, PreviewConfigType _previewType)
    {
        curShowLabel = _showLabel;
        CancelAllDownLoad();
        NPCTypeRef refData = ConfigMng.Instance.GetNPCTypeRef(_npcTypeID);
        if (refData != null)
        {
            PreviewNPC pp = PreviewNPC.CreateDummy(refData);
            pp.previewConfigType = _previewType;
            pp.StartAsyncCreate(CreateNPCCallBack);
            return true;
        }
        return false;
    }

    public bool TryPreviewSingelNPC(int _npcTypeID, UITexture _showLabel, PreviewConfigType _previewType, string idleAnimName)
    {
        curShowLabel = _showLabel;
        CancelAllDownLoad();
        NPCTypeRef refData = ConfigMng.Instance.GetNPCTypeRef(_npcTypeID);
        if (refData != null)
        {
            PreviewNPC pp = PreviewNPC.CreateDummy(refData);
            pp.previewConfigType = _previewType;
            pp.idleAnimName = idleAnimName;
            pp.StartAsyncCreate(CreateNPCCallBack);
            return true;
        }
        return false;
    }

    public bool TryPreviewSingelNPC(NPCInfo _info, UITexture _showLabel, PreviewConfigType _previewType)
    {
        curShowLabel = _showLabel;
        CancelAllDownLoad();
        if (_info != null)
        {
            PreviewNPC pp = PreviewNPC.CreateDummy(_info);
            pp.previewConfigType = _previewType;
            pp.StartAsyncCreate(CreateNPCCallBack);
            return true;
        }
        return false;
    }

    public void CreateNPCCallBack(PreviewNPC _npc)
    {
        //防止有之前延迟的模型加载，再次清理
        ClearModel();
        GameObject parent = GetModelTransform(_npc.PreviewPosition,_npc.PreviewRotation);
        if (null != parent)
        {
            _npc.transform.parent = parent.transform;
            _npc.transform.localPosition = new Vector3(0, -_npc.Height / 2f, 0);
            _npc.transform.localEulerAngles = new Vector3(0, 180f, 0);
        }
        _npc.FaceToNoLerp(180f);
        BindRenderAndUI(curShowLabel);
    }
    #endregion

    #region 怪物预览 by吴江
    /// <summary>
    /// 非经过我同意禁用本接口  by吴江
    /// </summary>
    /// <param name="_info"></param>
    /// <param name="_callBack"></param>
    /// <returns></returns>
    public bool TryPreviewSingelMonster(MonsterInfo _info, System.Action<PreviewMob> _callBack)
    {
        if (_info != null)
        {
            PreviewMob pp = PreviewMob.CreateDummy(_info);
            pp.mutualExclusion = false;
            pp.StartAsyncCreate(_callBack);
            return true;
        }
        return false;
    }
    public bool TryPreviewSingelMonster(MonsterInfo _info, UITexture _showLabel, PreviewConfigType _previewType)
    {
        curShowLabel = _showLabel;
        CancelAllDownLoad();
        if (_info != null)
        {
            PreviewMob pp = PreviewMob.CreateDummy(_info);
            pp.previewConfigType = _previewType;
            pp.StartAsyncCreate(CreateMonsterCallBack);
            return true;
        }
        return false;
    }

    public bool TryPreviewSingelMonster(int _configID, UITexture _showLabel, PreviewConfigType _previewType)
    {
        curShowLabel = _showLabel;
        CancelAllDownLoad();
        MonsterRef refData = ConfigMng.Instance.GetMonsterRef(_configID);
        MonsterInfo info = new MonsterInfo(refData);
        if (info != null)
        {
            PreviewMob pp = PreviewMob.CreateDummy(info);
            pp.previewConfigType = _previewType;
            pp.StartAsyncCreate(CreateMonsterCallBack);
            return true;
        }
        return false;
    }

    public void CreateMonsterCallBack(PreviewMob _mob)
    {
        // previewCamera.fieldOfView = 60.0f;
        //防止有之前延迟的模型加载，再次清理
        ClearModel();
		previewMob = _mob;
        GameObject parent = GetModelTransform(_mob.PreviewPosition, _mob.PreviewRotation);
        if (null != parent)
        {
            _mob.transform.parent = parent.transform;
            _mob.transform.localPosition = new Vector3(0, -_mob.Height / 2f, 0);
            _mob.transform.localEulerAngles = new Vector3(0, 180f, 0);
        }
        _mob.FaceToNoLerp(180f);
        UIDragObjectRotate3D _UIDragObjectRotate3D = curShowLabel.gameObject.GetComponent<UIDragObjectRotate3D>();
        if (_UIDragObjectRotate3D == null)
        {
            _UIDragObjectRotate3D = curShowLabel.gameObject.AddComponent<UIDragObjectRotate3D>();
        }
        _UIDragObjectRotate3D.target = _mob;
        BindRenderAndUI(curShowLabel);
    }

	public bool TryPreviewSingelMonster(int _configID, UITexture _showLabel, PreviewConfigType _previewType,string animName)
	{
		curShowLabel = _showLabel;
		CancelAllDownLoad();
		MonsterRef refData = ConfigMng.Instance.GetMonsterRef(_configID);
		MonsterInfo info = new MonsterInfo(refData);
		if (info != null)
		{
			PreviewMob pp = PreviewMob.CreateDummy(info);
			pp.idleAnimName = animName;
			pp.previewConfigType = _previewType;
			pp.StartAsyncCreate(CreateMonsterCallBack);
			return true;
		}
		return false;
	}

    #endregion

    #region 随从预览 by吴江
    /// <summary>
    /// 非经过我同意禁用本接口  by吴江
    /// </summary>
    /// <param name="_info"></param>
    /// <param name="_callBack"></param>
    /// <returns></returns>
    public bool TryPreviewSingelEntourage(MercenaryInfo _info, System.Action<PreviewEntourage> _callBack)
    {
        if (_info != null)
        {
            PreviewEntourage pp = PreviewEntourage.CreateDummy(_info);
            pp.mutualExclusion = false;
            pp.StartAsyncCreate(_callBack);
            return true;
        }
        return false;
    }
    public bool TryPreviewSingelEntourage(MercenaryInfo _info, UITexture _showLabel)
    {
        curShowLabel = _showLabel;
        CancelAllDownLoad();
        if (_info != null)
        {
            PreviewEntourage pp = PreviewEntourage.CreateDummy(_info);
            pp.StartAsyncCreate(CreateEntourageCallBack);
            return true;
        }
        return false;
    }

    public bool TryPreviewSingelEntourage(int _configID, UITexture _showLabel)
    {
        curShowLabel = _showLabel;
        CancelAllDownLoad();
        NewPetRef refData = ConfigMng.Instance.GetNewPetRef(_configID);
        MercenaryInfo info = new MercenaryInfo(refData);
        if (info != null)
        {
            PreviewEntourage pp = PreviewEntourage.CreateDummy(info);
            pp.StartAsyncCreate(CreateEntourageCallBack);
            return true;
        }
        return false;
    }

    public bool TryPreviewSingelEntourage(int _configID, UITexture _showLabel,string idleAnimName)
    {
        curShowLabel = _showLabel;
        CancelAllDownLoad();
        NewPetRef refData = ConfigMng.Instance.GetNewPetRef(_configID);
        MercenaryInfo info = new MercenaryInfo(refData);
        if (info != null)
        {
            PreviewEntourage pp = PreviewEntourage.CreateDummy(info);
            pp.idleAnimName = idleAnimName;
            pp.StartAsyncCreate(CreateEntourageCallBack);
            return true;
        }
        return false;
    }

    public void CreateEntourageCallBack(PreviewEntourage _ent)
    {
        // previewCamera.fieldOfView = 60.0f;
        //防止有之前延迟的模型加载，再次清理
        ClearModel();
        previewEntourage = _ent;
        GameObject parent = GetModelTransform(_ent.PreviewPosition, _ent.PreviewRotation);
        if (null != parent)
        {
            _ent.transform.parent = parent.transform;
            _ent.transform.localPosition = new Vector3(0, -_ent.Height / 2f, 0);
            _ent.transform.localEulerAngles = new Vector3(0, 180f, 0);
        }
        _ent.FaceToNoLerp(180f);
        UIDragObjectRotate3D _UIDragObjectRotate3D = curShowLabel.gameObject.GetComponent<UIDragObjectRotate3D>();
        if (_UIDragObjectRotate3D == null)
        {
            _UIDragObjectRotate3D = curShowLabel.gameObject.AddComponent<UIDragObjectRotate3D>();
        }
        _UIDragObjectRotate3D.target = _ent;
        BindRenderAndUI(curShowLabel);
    }


    #endregion

    #region 坐骑预览 by吴江
    protected MountInfo curPreviewInfo = null;
    protected PreviewMount curPreviewMount = null;
    /// <summary>
    /// 非经过我同意禁用本接口  by吴江
    /// </summary>
    /// <param name="_info"></param>
    /// <param name="_callBack"></param>
    /// <returns></returns>
    public bool TryPreviewSingelMount(MountInfo _info, System.Action<PreviewMount> _callBack)
    {
        if (_info != null)
        {
            PreviewMount pp = PreviewMount.CreateDummy(_info);
            pp.mutualExclusion = false;
			pp.StartAsyncCreate(_callBack);
            return true;
        }
        return false;
    }

    public bool TryPreviewSingelMount(MountInfo _info, UITexture _showLabel)
    {
        curShowLabel = _showLabel;
        if (curPreviewInfo != null && curPreviewInfo.ConfigID == _info.ConfigID && curPreviewMount != null)
        {
            curPreviewInfo = _info;
            BindRenderAndUI(curShowLabel);
            return true;
        }
        else
        {
            curPreviewInfo = _info;
            CancelAllDownLoad();
            if (_info != null)
            {
                PreviewMount pp = PreviewMount.CreateDummy(_info);
                pp.StartAsyncCreate(CreateMountCallBack);
                return true;
            }
            return false;
        }
    }

    public bool TryPreviewSingelMount(int _configID, UITexture _showLabel,string idleAnimName)
    {
        curShowLabel = _showLabel;
        CancelAllDownLoad();
        MountRef refData = ConfigMng.Instance.GetMountRef(_configID);
        MountInfo info = new MountInfo(refData);
        if (info != null)
        {
            PreviewMount pp = PreviewMount.CreateDummy(info);
            pp.idleAnimName = idleAnimName;
            pp.StartAsyncCreate(CreateMountCallBack);
            return true;
        }
        return false;
    }

    public bool TryPreviewMount(MountInfo _info, UITexture _showLabel, MountAnimFSM.EventType _type = ActorAnimFSM.EventType.Idle)
    {
        curShowLabel = _showLabel;
        CancelAllDownLoad();
        if (_info != null)
        {
            PreviewMount pp = PreviewMount.CreateDummy(_info);
            pp.needRandID = _type;
            pp.StartAsyncCreate(CreateMountCallBack);
            return true;
        }
        return false;
    }

    public void CreateMountCallBack(PreviewMount _mob)
    {

        //防止有之前延迟的模型加载，再次清理
        ClearModel();
        curPreviewMount = _mob;
        GameObject parent = GetModelTransform(_mob.PreviewPosition, _mob.PreviewRotation);
        if (null != parent)
        {
            _mob.transform.parent = parent.transform;
            _mob.transform.localPosition = new Vector3(0, -_mob.Height / 2f, 0);
            _mob.transform.localEulerAngles = new Vector3(0, 180f, 0);
        }
        _mob.FaceToNoLerp(180f);
        UIDragObjectRotate3D _UIDragObjectRotate3D = curShowLabel.gameObject.GetComponent<UIDragObjectRotate3D>();
        if (_UIDragObjectRotate3D == null)
        {
            _UIDragObjectRotate3D = curShowLabel.gameObject.AddComponent<UIDragObjectRotate3D>();
        }
        _UIDragObjectRotate3D.target = _mob;
        BindRenderAndUI(curShowLabel);
    }

    public void SetPreviewMountEnpty()
    {
        curPreviewInfo = null;
    }
    #endregion

    #region 装备预览 by吴江
    protected EquipmentInfo curPreviewEqInfo = null;
    protected PreviewEquipment curPreviewEqObj = null;
    /// <summary>
    /// 非经过我同意禁用本接口  by吴江
    /// </summary>
    /// <param name="_info"></param>
    /// <param name="_callBack"></param>
    /// <returns></returns>
    public bool TryPreviewSingleEquipment(EquipmentInfo _eq, System.Action<PreviewEquipment> _callBack)
    {
        if (_eq == null) return false;
        PreviewEquipment pe = PreviewEquipment.CreateDummy(_eq);
        pe.mutualExclusion = false;
        pe.StartAsyncCreate(_callBack);
        return true;
    }
    /// <summary>
    /// 预览单件装备
    /// </summary>
    /// <param name="_eq"></param>
    /// <param name="_showLabel"></param>
    /// <returns></returns>
    public bool TryPreviewSingleEquipment(EquipmentInfo _eq, UITexture _showLabel)
    {
        if (_eq == null) return false;
        curShowLabel = _showLabel;
        if (curPreviewEqInfo != null && curPreviewEqInfo.EID == _eq.EID && curPreviewEqObj != null)
        {
            curPreviewEqInfo = _eq;
            BindRenderAndUI(curShowLabel);
            return true;
        }
        else
        {
            ClearModel();
            curPreviewEqInfo = _eq;
            CancelAllDownLoad();
            PreviewEquipment pe = PreviewEquipment.CreateDummy(_eq);
            pe.StartAsyncCreate(CreateEquipmentCallBack);
            return true;
        }
    }

    public void CreateEquipmentCallBack(PreviewEquipment _eq)
    {
        //防止有之前延迟的模型加载，再次清理
        ClearModel();
        curPreviewEqObj = _eq;
        GameObject parent = GetModelTransform(_eq.PreviewPosition, _eq.PreviewRotation);
        if (null != parent)
        {
            _eq.transform.parent = parent.transform;
            _eq.transform.localPosition = new Vector3(0, -_eq.Height / 2f, 0);
            _eq.transform.localEulerAngles = new Vector3(0, 180f, 0);
        }
        _eq.FaceToNoLerp(180f);
        UIDragObjectRotate3D _UIDragObjectRotate3D = curShowLabel.gameObject.GetComponent<UIDragObjectRotate3D>();
        if (_UIDragObjectRotate3D == null)
        {
            _UIDragObjectRotate3D = curShowLabel.gameObject.AddComponent<UIDragObjectRotate3D>();
        }
        _UIDragObjectRotate3D.target = _eq;
        BoxCollider bc = curShowLabel.gameObject.GetComponent<BoxCollider>();
        if (bc == null) curShowLabel.gameObject.AddComponent<BoxCollider>();
        BindRenderAndUI(curShowLabel);
    }
    #endregion






    #region S2C
	/// <summary>
	/// 获取装备及人物信息
	/// </summary>
	/// <param name="_info">Info.</param>
	protected void S2C_GotCharInfo(Pt _info)
    {
		//Debug.Log("S2C_GotCharInfo");
		pt_look_usr_list_d712 pt = _info as pt_look_usr_list_d712;
		curAskPlayerInfo = new PlayerBaseInfo(new PlayerBaseData(pt));
        if (OnGotCurAskPlayerInfo != null)
        {
            OnGotCurAskPlayerInfo();
        }
        NewRankingWnd wnd = GameCenter.uIMng.GetGui<NewRankingWnd>();
        if (wnd == null)
        {
            GameCenter.uIMng.GenGUI(GUIType.PREVIEWOTHERS, true);
        }
        else
        {
            if (OnGotRankOtherInfo != null)
            {
                OnGotRankOtherInfo();
            }
        }
    }


    /// <summary>
    /// 获取装备及人物信息
    /// </summary>
    /// <param name="_pt"></param>
    protected void S2C_OnGotEquipment(Pt _pt)
    {
        pt_usr_info_equi_d123 msg = _pt as pt_usr_info_equi_d123;
        //Debug.logger.Log("pt_usr_info_equi_d123");
        if (msg != null)
        {
            curAskPlayerInfo = null;

            curAskPlayerInfo = new PlayerBaseInfo(new PlayerBaseData(msg));
            curAskPlayerInfo.UpdateStarEffect();
            if (OnGotCurAskPlayerInfo != null)
                OnGotCurAskPlayerInfo();
        }
    }
    /// <summary>
    /// 获取宠物属性信息
    /// </summary>
    /// <param name="_pt"></param>
	protected void S2C_OnGotPetInfo(Pt _pt)
    {
		pt_look_pet_ride_info_d713 msg = _pt as pt_look_pet_ride_info_d713;
		//Debug.Log("S2C_OnGotPetInfo");
        if (msg != null)
        {
            if ( curAskPlayerInfo!=null )
                curAskPlayerInfo.UpdateInfo(msg);
            if (OnGotCurAskPlayerInfo != null)
                OnGotCurAskPlayerInfo();
            NewRankingWnd wnd = GameCenter.uIMng.GetGui<NewRankingWnd>();
            if (wnd != null)
            {
                if (OnGotRankOtherPetInfo != null && GameCenter.newRankingMng.CurOtherId == curAskPlayerInfo.ServerInstanceID)
                {
                    OnGotRankOtherPetInfo();
                }
            }
        }
    }
    /// <summary>
    /// 获取排行榜其他玩家信息
    /// </summary>
    /// <param name="_msg"></param>
    protected void S2C_GotPlayerInfo(Pt _msg)
    {
        pt_req_look_rank_usrinfo_d773 msg = _msg as pt_req_look_rank_usrinfo_d773;
        if (msg != null)
        {
            curAskPlayerInfo = new PlayerBaseInfo(new PlayerBaseData(msg));
            GameCenter.newRankingMng.CurRankPlayerInfo = curAskPlayerInfo;
        }
        if (OnGotCurAskRankPlayerInfo != null)
        {
            OnGotCurAskRankPlayerInfo();
        }
    }

    /// <summary>
    /// 获取佣兵信息
    /// </summary>
    /// <param name="_pt"></param>
    protected void S2C_OnGotEntourage(Pt _pt)
    {
        pt_usr_info_entourage_d125 msg = _pt as pt_usr_info_entourage_d125;
        //Debug.logger.Log("pt_usr_info_entourage_d125");
        if (msg != null)
        {
            entourageList.Clear();
            for (int i = 0; i < msg.entourage_info_list.Count; i++)
            {
                entourage_info_list info = new entourage_info_list();
                info = msg.entourage_info_list[i];
                entourageList.Add(info);
            }
            if (OnGotCurAskPlayerInfo != null)
                OnGotCurAskPlayerInfo();
        }
    }
    /// <summary>
    /// 获取遗物信息
    /// </summary>
    /// <param name="_pt"></param>
    protected void S2C_OnGotReclic(Pt _pt)
    {
        pt_usr_info_lost_item_d126 msg = _pt as pt_usr_info_lost_item_d126;
        //Debug.logger.Log("pt_usr_info_lost_item_d126");
        if (msg != null)
        {
            relicList.Clear();
            for (int i = 0; i < msg.lost_item_info.Count; i++)
            {
                lost_item_info info = new lost_item_info();
                info = msg.lost_item_info[i];
                relicList.Add(info);
            }
            if (OnGotCurAskPlayerInfo != null)
                OnGotCurAskPlayerInfo();
        }
    }
    #endregion

    #region C2S
    //public static void C2S_ReqCharInf(int pid)
    //{
    //    curWantToPreview = pid;
    //    //如果是查看自己 
    //    if (GameCenter.mainPlayerMng.MainPlayerInfo.ID == pid)
    //    {
    //        curAskPlayerInfo = GameCenter.mainPlayerMng.MainPlayerInfo;
    //        if (OnGotCurAskPlayerInfo != null)
    //        {
    //            OnGotCurAskPlayerInfo();
    //        }
    //        return;
    //    }
    //    //如果客户端已经有这个玩家的数据了（场景内附近的玩家），则直接返回；   PS这部分数据不全，还是得像服务端重新要 by吴江
    //    //OtherPlayerInfo opcInfo = GameCenter.sceneMng.GetOPCInfo(pid);
    //    //if (opcInfo != null)
    //    //{
    //    //    curAskPlayerInfo = opcInfo;
    //    //    if (OnGotCurAskPlayerInfo != null)
    //    //    {
    //    //        OnGotCurAskPlayerInfo();
    //    //    }
    //    //    return;
    //    //}
    //    curAskPlayerInfo = null;
    //    //如果没有，则向服务器申请
    //    Cmd cmd = new Cmd(MsgHander.PT_ACCEPT_CHARINF);
    //    cmd.write_int(pid);
    //    NetMsgMng.SendCMD(cmd);
    //}


    /// <summary>
    /// 请求其他玩家数据
    /// </summary>
    public void C2S_AskOPCPreview(int id)
    {
		//Debug.Log("C2S_AskOPCPreview:"+id);
        curWantToPreview = id;
		pt_req_look_usr_info_d711 msg = new pt_req_look_usr_info_d711();
		msg.uid = id;
		msg.state = 1;
        NetMsgMng.SendMsg(msg);
    }
	/// <summary>
	/// 请求其他玩家宠物数据
	/// </summary>
	public void C2S_AskOpcPetPreview(int id)
	{
        //Debug.Log("C2S_AskOpcPetPreview:" + id);
		pt_req_look_usr_info_d711 msg = new pt_req_look_usr_info_d711();
		//msg.uid = curAskPlayerInfo != null?curAskPlayerInfo.ServerInstanceID:0;
        msg.uid = id;
		msg.state = 2;
		NetMsgMng.SendMsg(msg);
	}

    /// <summary>
    /// 请求排行榜其他玩家信息
    /// </summary>
    public void C2S_ReqGetInfo(int _id,int _type)
    {
        pt_req_look_rank_usr_d774 msg = new pt_req_look_rank_usr_d774();
        msg.uid = _id;
        msg.type = _type;
        NetMsgMng.SendMsg(msg);
    }

    /// <summary>
    /// 请求装备数据
    /// </summary>
    public void C2S_AskEquitMent()
    {
        //Debug.logger.Log("msg.action = 1076 " + curWantToPreview);
        pt_action_int_d003 msg = new pt_action_int_d003();
        msg.action = 1076;
        msg.data = curWantToPreview;
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 请求信息数据
    /// </summary>
    public void C2S_AskInformation()
    {
        //Debug.logger.Log("msg.action = 1077 " + curWantToPreview);
        pt_action_int_d003 msg = new pt_action_int_d003();
        msg.action = 1077;
        msg.data = curWantToPreview;
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 请求佣兵数据
    /// </summary>
    public void C2S_AskEntourage()
    {
        //Debug.logger.Log("msg.action = 1078 " + curWantToPreview);
        pt_action_int_d003 msg = new pt_action_int_d003();
        msg.action = 1078;
        msg.data = curWantToPreview;
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 请求遗物数据
    /// </summary>
    public void C2S_AskRelic()
    {
        //Debug.logger.Log("msg.action = 1079 " + curWantToPreview);
        pt_action_int_d003 msg = new pt_action_int_d003();
        msg.action = 1079;
        msg.data = curWantToPreview;
        NetMsgMng.SendMsg(msg);
    }
    #endregion



}
