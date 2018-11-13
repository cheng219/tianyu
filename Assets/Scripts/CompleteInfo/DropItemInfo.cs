//=======================================
//作者:吴江
//日期:2015/9/23
//描述:掉落物品的数据层对象
//=======================================



using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 掉落物品的数据层对象 by吴江
/// </summary>
public class DropItemInfo : ActorInfo{

    /// <summary>
    /// 服务端数据 by吴江
    /// </summary>
    protected st.net.NetBase.drop_des dropServerData;
    /// <summary>
    /// 客户端静态配置数据 by吴江
    /// </summary>
    protected EquipmentRef refData = null;
    /// <summary>
    /// 客户端静态配置数据 by吴江
    /// </summary>
    protected EquipmentRef RefData
    {
        get
        {
            if (refData == null)
            {
				refData = ConfigMng.Instance.GetEquipmentRef((int)dropServerData.type);
            }
            return refData;
        }
    }

    /// <summary>
    /// 从属的角色ID
    /// </summary>
    protected int dropFromServerInstanceID = -1;


    public DropItemInfo(st.net.NetBase.drop_des _serverData,int _oid)
    {
		serverData = new ActorData();
		serverData.serverInstanceID = (int)_serverData.id;
		serverData.startPosX = _serverData.x;
		serverData.startPosY = _serverData.y;
        serverData.startPosZ = _serverData.z;
		serverData.prof = (int)_serverData.type;

		dropServerData = _serverData;
		dropFromServerInstanceID = (int)_serverData.from_id;
        Monster obj = GameCenter.curGameStage.GetMOB(dropFromServerInstanceID);
        if (obj != null)
        {
            //startPos.x = obj.transform.position.x;
            //startPos.y = obj.transform.position.z;
            startPos = obj.transform.position;
        }
        else
        {
			
			startPos.x = dropServerData.x;
			startPos.y = dropServerData.y;
            startPos.z = dropServerData.z;
        }
    }


    #region 访问器
    /// <summary>
    /// 掉落来源对象ID
    /// </summary>
    public int DropFromServerInstanceID
    {
        get
        {
            return dropFromServerInstanceID;
        }
    }

    /// <summary>
    /// 从属的角色ID
    /// </summary>
    public List<uint> BlongToServerInstanceID
    {
        get
        {
			return dropServerData.owner;
        }
    }
    /// <summary>
    /// 服务器唯一ID
    /// </summary>
    public new int ServerInstanceID
    {
        get
        {
			return (int)dropServerData.id;
        }
    }



    /// <summary>
    /// 配置ID
    /// </summary>
    public int ConfigID
    {
        get
        {
			return (int)dropServerData.type;
        }
    }

    protected new Vector2 serverPos = Vector2.zero;
    public new Vector2 ServerPos
    {
        get
        {
			serverPos.x = dropServerData.x;
			serverPos.y = dropServerData.z;
            return serverPos;
        }
    }

    protected Vector3 startPos = Vector3.zero;
    public Vector3 StartPos
    {
        get
        {
            return startPos;
        }
    }

    /// <summary>
    /// 物品名称 by吴江
    /// </summary>
    public string ItemName
    {
        get
        {
            return RefData == null ? "????" : RefData.name;
        }
    }
    /// <summary>
    /// 3d特效名称 by吴江
    /// </summary>
    public string EffectName
    {
        get
        {
            return RefData == null ? string.Empty : RefData.dropEffect;
        }
    }

    /// <summary>
    /// 品质颜色 by吴江
    /// </summary>
    public Color QualityColor
    {
        get
        {
            return RefData == null ? Color.white : EquipmentInfo.GetQualityColor(RefData.quality);
        }
    }
    /// <summary>
    /// 品质  by吴江
    /// </summary>
    public int Quality
    {
        get
        {
            return RefData == null ? 1 : RefData.quality;
        }
    }

    /// <summary>
    /// 从属对象ID by吴江
    /// </summary>
    public List<uint> OwnerID
    {
        get
        {
			return dropServerData.owner;
        }
    }

    /// <summary>
    /// 掉落声音 by吴江
    /// </summary>
    public string DropSound
    {
        get
        {
            return RefData == null ? string.Empty : RefData.soundDropRes;
        }
    }

    /// <summary>
    /// sort by黄洪兴
    /// </summary>
    public int DropSort
    {
        get
        {
            return RefData == null ? 0 : RefData.oldSort;
        }
    }
    /// <summary>
    /// 掉落物品的Family by 唐源
    /// </summary>
    public EquipmentFamily Family
    {
        get
        {
            return RefData == null ? 0 : RefData.family;
        }
    }

    /// <summary>
    /// 是否超时删除 by吴江
    /// </summary>
    public bool IsTimeOut = false;
    #endregion




}
