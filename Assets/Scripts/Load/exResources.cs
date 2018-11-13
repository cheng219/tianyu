///////////////////////////////////////////////////////////////////////////////
// 作者：吴江SceneItem/
// 日期：2015/4/29
// 用途：资源管理的扩展类
///////////////////////////////////////////////////////////////////////////////


using UnityEngine;
using System.Collections;

public class exResources  {


    static public Object GetResource(ResourceType _type, string _name)
    {
        switch (_type)
        {
            case ResourceType.GUI:
                return Resources.Load("Prefab/NewUI/" + _name);
            case ResourceType.PLAYER:
                return Resources.Load("Prefab/Warrior/" + _name);
            case ResourceType.MONSTER:
                return Resources.Load("Prefab/Monster/" + _name);
            case ResourceType.NPC:
                return Resources.Load("Prefab/NPC/" + _name);
            case ResourceType.EFFECT:
                return Resources.Load("Prefab/Effect/" + _name);
            case ResourceType.SCENEITEM:
                return Resources.Load("Prefab/SceneItem/" + _name);
            case ResourceType.TEXT:
                return Resources.Load("Prefab/ObjectText/" + _name);
            case ResourceType.TEXTURE:
                return Resources.Load("Texture/" + _name);
            case ResourceType.OTHER:
                return Resources.Load("Prefab/other/" + _name);
            case ResourceType.FONT:
                return Resources.Load("NGUI/Font/" + _name);
            case ResourceType.SOUND:
                return Resources.Load("Sound/" + _name);
			case ResourceType.PICTURE:
				return Resources.Load("NGUI/NewAtlas/" + _name);
            default:
                return null;
        }
    }



    public static AssetMng.DownloadID GetRace(int _id, System.Action<GameObject, EResult> _onComplete)
    {
        PlayerConfig playerConfig = ConfigMng.Instance.GetPlayerConfig(_id);
        if (playerConfig != null)
        {
            AssetMng.DownloadID downloadID
                = AssetMng.instance.LoadAsset<GameObject>(AssetMng.GetPathWithExtension(playerConfig.res_name, AssetPathType.PersistentDataPath),
                                                            playerConfig.res_name,
                                                            _onComplete,
                                                            true);
            return downloadID;
        }
        else
        {
            GameSys.LogError("Unknown raceID " + _id);
            _onComplete(null, EResult.NotFound);
            return null;
        }
    }

	public static AssetMng.DownloadID GetRace(int _id, System.Action<GameObject, EResult> _onComplete,bool isCreatePlayer)
	{
		PlayerConfig playerConfig = ConfigMng.Instance.GetPlayerConfig(_id);
		if (playerConfig != null)
		{
			string resName = (isCreatePlayer?playerConfig.display_res:playerConfig.res_name);
			//Debug.Log("resName:"+resName);
			AssetMng.DownloadID downloadID
			= AssetMng.instance.LoadAsset<GameObject>(AssetMng.GetPathWithExtension(resName, AssetPathType.PersistentDataPath),
				resName,
				_onComplete,
				true);
			return downloadID;
		}
		else
		{
			GameSys.LogError("Unknown raceID " + _id);
			_onComplete(null, EResult.NotFound);
			return null;
		}
	}

    /// <summary>
    /// 获取随从模型
    /// </summary>
    /// <param name="_url"></param>
    /// <param name="_onComplete"></param>
    /// <returns></returns>
    public static AssetMng.DownloadID GetEntourage(string _url, System.Action<GameObject, EResult> _onComplete)
    {
            AssetMng.DownloadID downloadID
                = AssetMng.instance.LoadAsset<GameObject>(AssetMng.GetPathWithExtension(_url, AssetPathType.PersistentDataPath),
                                                            _url,
                                                            _onComplete,
                                                            true);
            return downloadID;
    }

    //public static void UnloadRace(int _id)
    //{
    //    PlayerConfig playerConfig = ConfigMng.Instance.GetPlayerConfig(_id);
    //    if (playerConfig != null)
    //    {
    //        AssetMng.instance.UnloadUrl(AssetMng.GetPathWithExtension("Player/" + playerConfig.res_name, (AssetPathType)playerConfig.path_type));
    //    }
    //}

    public static AssetMng.DownloadID GetNPC(int _id, System.Action<GameObject, EResult> _onComplete)
    {
        NPCTypeRef npcRef = ConfigMng.Instance.GetNPCTypeRef(_id);
        if (npcRef != null)
        {
            string[] strs = npcRef.res_name.Split('/');
            string resName = strs[strs.Length - 1];
            AssetMng.DownloadID downloadID
                = AssetMng.instance.LoadAsset<GameObject>(AssetMng.GetPathWithExtension(npcRef.res_name, (AssetPathType)npcRef.path_type),
                                                            resName,
                                                            _onComplete,
                                                            true);
            return downloadID;
        }
        else
        {
            GameSys.LogError("Unknown type " + _id);
            _onComplete(null, EResult.NotFound);
            return null;
        }
    }


    public static AssetMng.DownloadID GetMob(int _id, System.Action<GameObject, EResult> _onComplete)
    {
        MonsterRef mobRef = ConfigMng.Instance.GetMonsterRef(_id);
        if (mobRef != null)
        {
            string[] strs = mobRef.boneName.Split('/');
            string resName = strs[strs.Length - 1];
            AssetMng.DownloadID downloadID
                = AssetMng.instance.LoadAsset<GameObject>(AssetMng.GetPathWithExtension(mobRef.resName, AssetPathType.PersistentDataPath),
                                                            resName,
                                                            _onComplete,
                                                            true);
            return downloadID;
        }
        else
        {
            GameSys.LogError("Unknown type " + _id);           
            _onComplete(null, EResult.NotFound);
            return null;
        }
    }

    /// <summary>
    /// 加载掉落物品模型 by吴江
    /// </summary>
    /// <param name="_id"></param>
    /// <param name="_onComplete"></param>
    /// <returns></returns>
    public static AssetMng.DownloadID GetDropItem(int _id, System.Action<GameObject, EResult> _onComplete)
    {
        EquipmentRef itemRef = ConfigMng.Instance.GetEquipmentRef(_id);
        if (itemRef != null)
        {
            string[] strs = itemRef.dropModel.Split('/');
            string resName = strs[strs.Length - 1];
            AssetMng.DownloadID downloadID
                = AssetMng.instance.LoadAsset<GameObject>(AssetMng.GetPathWithExtension(itemRef.dropModel, AssetPathType.PersistentDataPath),
                                                            resName,
                                                            _onComplete,
                                                            true);
            return downloadID;
        }
        else
        {
            GameSys.LogError("Unknown type " + _id);
            _onComplete(null, EResult.NotFound);
            return null;
        }
    }

    /// <summary>
    /// 场景物品 
    /// </summary>
    public static AssetMng.DownloadID GetSceneItem(int _id, System.Action<GameObject, EResult> _onComplete)
    {
        SceneItemRef itemRef = ConfigMng.Instance.GetSceneItemRef(_id);
        if (itemRef != null)
        {
            string[] strs = itemRef.assetName.Split('/');
            string resName = strs[strs.Length - 1];
            AssetMng.DownloadID downloadID
                = AssetMng.instance.LoadAsset<GameObject>(AssetMng.GetPathWithExtension(itemRef.assetName, AssetPathType.PersistentDataPath),
                                                          resName,
                                                          _onComplete,
                                                          true);
            return downloadID;
        }
        else
        {
            GameSys.LogError("Unknown type " + _id);
            _onComplete(null, EResult.NotFound);
            return null;
        }
    }

    public static AssetMng.DownloadID GetMount(string _modelName, System.Action<GameObject, EResult> _onComplete)
    {
        string[] strs = _modelName.Split('/');
        string resName = strs[strs.Length - 1];
        AssetMng.DownloadID downloadID
            = AssetMng.instance.LoadAsset<GameObject>(AssetMng.GetPathWithExtension(_modelName, AssetPathType.PersistentDataPath),
                                                        resName,
                                                        _onComplete,
                                                        true);
        return downloadID;
    }


    public static AssetMng.DownloadID GetShadow(string _name, System.Action<GameObject, EResult> _onComplete)
    {
        string url = AssetMng.GetPathWithoutExtension("effect/" + _name, AssetPathType.PersistentDataPath) + ".object";
        AssetMng.DownloadID downloadID  = AssetMng.instance.LoadAsset<GameObject>(url, _name, _onComplete, true);
        return downloadID;
    }
}
