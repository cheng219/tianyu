//=============================================
//作者：吴江
//日期：2015/7/7
//用途：场景控制台的辅助类
//=====================================================



using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


/// <summary>
/// 场景控制台的辅助类 by 吴江
/// </summary>
public static class GameStageUtility
{
    public static Dictionary<AlertAreaType, Texture> textureDic = new Dictionary<AlertAreaType, Texture>();

    public static Texture GetTextureByType(AlertAreaType _type)
    {

        if (!textureDic.ContainsKey(_type) || textureDic[_type] == null)
        {
            string path = string.Empty;
            switch (_type)
            {
                case AlertAreaType.SECTOR30:
                    path = "AbilityProjector/30";
                    break;
                case AlertAreaType.SECTOR45:
                    path = "AbilityProjector/45";
                    break;
                case AlertAreaType.SECTOR60:
                    path = "AbilityProjector/60";
                    break;
                case AlertAreaType.SECTOR90:
                    path = "AbilityProjector/90";
                    break;
                case AlertAreaType.SECTOR135:
                    path = "AbilityProjector/135";
                    break;
                case AlertAreaType.SECTOR180:
                    path = "AbilityProjector/180";
                    break;
                case AlertAreaType.SECTOR270:
                    path = "AbilityProjector/270";
                    break;
                case AlertAreaType.SECTOR360:
                    path = "AbilityProjector/360";
                    break;
                case AlertAreaType.RECT:
                    path = "AbilityProjector/Arrow";
                    break;
                default:
                    return null;
            }
            textureDic[_type] = exResources.GetResource(ResourceType.TEXTURE, path) as Texture;
        }
        return textureDic[_type];
    }

    public static UIAtlas taskAtlas = null;
    public static UIAtlas TaskAtlas
    {
        get
        {
            if (taskAtlas == null)
            {
                taskAtlas = Resources.Load("NGUI/Atlas/ConstantUI/ConstantAtlas", typeof(UIAtlas)) as UIAtlas;
            }
            return taskAtlas;
        }
    }
    public static Shader custumColorShader = null;
    public static Shader fossilShader = null;

    public static Vector3[] StartPath(Vector3 _startPos, Vector3 _destinationPos, float _maxDistance = 24.0f)
    {
        NavMeshPath resultPath = new NavMeshPath();
		bool pathFound = NavMesh.CalculatePath(_startPos, _destinationPos, NavMesh.AllAreas, resultPath);
        if (pathFound)
        {
            return CheckPath(resultPath.corners);
        }
        else
        {
            NavMeshHit navHit = new NavMeshHit();
			if (NavMesh.SamplePosition(_startPos, out navHit, _maxDistance, NavMesh.AllAreas))
            {
				pathFound = NavMesh.CalculatePath(navHit.position, _destinationPos, NavMesh.AllAreas, resultPath);
                if (pathFound)
                {
                    return CheckPath(resultPath.corners);
                }
            }
        }
        return null;
    }


    public static Vector3[] CheckPath(Vector3[] _originPath)
    {
        if (_originPath == null || _originPath.Length == 0) return null;
        List<Vector3> path = new List<Vector3>();
        path.Add(_originPath[0]);
        for (int i = 0; i < _originPath.Length; i++)
        {
            if (i < _originPath.Length - 1)
            {
                Vector3 temp = ActorMoveFSM.PathCast(_originPath[i], _originPath[i + 1]);
                if (temp == _originPath[i + 1])
                {
                    path.Add(_originPath[i + 1]);
                }
                else
                {
                    path.Add(temp);
                    break;
                }
            }
        }
        return path.ToArray();
    }
    /// <summary>
    /// 检查指定点是否可走 by吴江
    /// </summary>
    /// <param name="_from">一个合理的起点</param>
    /// <param name="_to">目标点</param>
    /// <returns></returns>
    public static bool CheckPosition(Vector3 _from, Vector3 _to)
    {
		Vector3[] path = StartPath(ActorMoveFSM.LineCast(_from,true), ActorMoveFSM.LineCast(_to,true));
        return path != null;
    }
    /// <summary>
    /// 在当前场景获取玩家对象 by吴江
    /// </summary>
    /// <param name="_gameStage"></param>
    /// <param name="_id"></param>
    /// <returns></returns>
    public static PlayerBase GetPlayerBase(this GameStage _gameStage, int _id) { return _gameStage.GetObject(ObjectType.Player, _id) as PlayerBase; }
    /// <summary>
    /// 在当前场景获取其他玩家对象 by吴江
    /// </summary>
    /// <param name="_gameStage"></param>
    /// <param name="_id"></param>
    /// <returns></returns>
    public static OtherPlayer GetOtherPlayer(this GameStage _gameStage, int _id) { return _gameStage.GetObject(ObjectType.Player, _id) as OtherPlayer; }
    /// <summary>
    /// 在当前场景获取其他随从对象 by吴江
    /// </summary>
    /// <param name="_gameStage"></param>
    /// <param name="_id"></param>
    /// <returns></returns>
    public static OtherEntourage GetOtherEntourage(this GameStage _gameStage, int _id) { return _gameStage.GetObject(ObjectType.Entourage, _id) as OtherEntourage; }
    /// <summary>
    /// 在当前场景获取npc对象 by吴江
    /// </summary>
    /// <param name="_gameStage"></param>
    /// <param name="_id"></param>
    /// <returns></returns>
    public static NPC GetNPC(this GameStage _gameStage, int _id) { return _gameStage.GetObject(ObjectType.NPC, _id) as NPC; }
    /// <summary>
    /// 在当前场景获取怪物对象 by吴江
    /// </summary>
    /// <param name="_gameStage"></param>
    /// <param name="_id"></param>
    /// <returns></returns>
    public static Monster GetMOB(this GameStage _gameStage, int _id) { return _gameStage.GetObject(ObjectType.MOB, _id) as Monster; }
    /// <summary>
    /// 在当前场景获取随从对象 by吴江
    /// </summary>
    /// <param name="_gameStage"></param>
    /// <param name="_id"></param>
    /// <returns></returns>
    public static EntourageBase GetEntourage(this GameStage _gameStage, int _id) { return _gameStage.GetObject(ObjectType.Entourage, _id) as EntourageBase; }
    /// <summary>
    /// 在当前场景获取掉落物品对象 by吴江
    /// </summary>
    /// <param name="_gameStage"></param>
    /// <param name="_id"></param>
    /// <returns></returns>
    public static DropItem GetDropItem(this GameStage _gameStage, int _id) { return _gameStage.GetObject(ObjectType.DropItem, _id) as DropItem; }
    /// <summary>
    /// 在当前场景获取陷阱对象 by吴江
    /// </summary>
    /// <param name="_gameStage"></param>
    /// <param name="_id"></param>
    /// <returns></returns>
    public static Trap GetTrap(this GameStage _gameStage, int _id) { return _gameStage.GetObject(ObjectType.Trap, _id) as Trap; }

    /// <summary>
    /// 在当前场景获取场景物品对象 by吴江
    /// </summary>
    /// <param name="_gameStage"></param>
    /// <param name="_id"></param>
    /// <returns></returns>
    public static SceneItem GetSceneItem(this GameStage _gameStage, int _id) { return _gameStage.GetObject(ObjectType.SceneItem, _id) as SceneItem; }
    /// <summary>
    /// 在当前场景获取对象 by吴江
    /// </summary>
    /// <param name="_gameStage"></param>
    /// <param name="_id"></param>
    /// <returns></returns>
    public static InteractiveObject GetInterActiveObj(this GameStage _gameStage, int _id) 
    {
        if (GameCenter.curMainPlayer != null && _id == GameCenter.curMainPlayer.id)
        {
            return GameCenter.curMainPlayer;
        }
        foreach (ObjectType item in Enum.GetValues(typeof(ObjectType)))
        {
            InteractiveObject obj = _gameStage.GetObject(item, _id);
            if (obj != null)
            {
                return obj;
            }
        }
        return null;
    }

    /// <summary>
    /// 在当前场景获取所有npc对象 by吴江
    /// </summary>
    /// <param name="_gameStage"></param>
    /// <param name="_id"></param>
    /// <returns></returns>
    public static List<NPC> GetNPCs(this GameStage _gameStage) { return _gameStage.GetObjects<NPC>(); }
    /// <summary>
    /// 在当前场景获取所有传送点对象 by吴江
    /// </summary>
    /// <param name="_gameStage"></param>
    /// <param name="_id"></param>
    /// <returns></returns>
    public static List<FlyPoint> GetFlypoints(this GameStage _gameStage) { return _gameStage.GetObjects<FlyPoint>(); }
    /// <summary>
    /// 在当前场景获取所有其他玩家对象 by吴江
    /// </summary>
    /// <param name="_gameStage"></param>
    /// <param name="_id"></param>
    /// <returns></returns>
    public static List<OtherPlayer> GetOtherPlayers(this GameStage _gameStage) { return _gameStage.GetObjects<OtherPlayer>(); }
    /// <summary>
    /// 在当前场景获取所有其他玩家的随从对象 by吴江
    /// </summary>
    /// <param name="_gameStage"></param>
    /// <param name="_id"></param>
    /// <returns></returns>
    public static List<OtherEntourage> GetOtherEntourages(this GameStage _gameStage) { return _gameStage.GetObjects<OtherEntourage>(); }
    /// <summary>
    /// 在当前场景获取所有场景掉落物品对象 by吴江
    /// </summary>
    /// <param name="_gameStage"></param>
    /// <param name="_id"></param>
    /// <returns></returns>
    public static List<DropItem> GetDropItems(this GameStage _gameStage) { return _gameStage.GetObjects<DropItem>(); }
    /// <summary>
    /// 在当前场景获取所有场景触发器物品对象 by吴江
    /// </summary>
    /// <param name="_gameStage"></param>
    /// <param name="_id"></param>
    /// <returns></returns>
    public static List<SceneItem> GetSceneItems(this GameStage _gameStage) { return _gameStage.GetObjects<SceneItem>(); }
    /// <summary>
    /// 在当前场景获取所有场景雕像 by吴江
    /// </summary>
    /// <param name="_gameStage"></param>
    /// <param name="_id"></param>
    /// <returns></returns>
    public static List<Model> GetModels(this GameStage _gameStage) { return _gameStage.GetObjects<Model>(); }
    /// <summary>
    /// 在当前场景获取所有陷阱对象 by吴江
    /// </summary>
    /// <param name="_gameStage"></param>
    /// <param name="_id"></param>
    /// <returns></returns>
    public static List<Trap> GetTraps(this GameStage _gameStage) { return _gameStage.GetObjects<Trap>(); }
    /// <summary>
    /// 在当前场景获取所有怪物对象 by吴江
    /// </summary>
    /// <param name="_gameStage"></param>
    /// <param name="_id"></param>
    /// <returns></returns>
    public static List<Monster> GetMobs(this GameStage _gameStage) { return _gameStage.GetObjects<Monster>(); }
	
	public static List<SmartActor> GetSmartActor(this GameStage _gameStage) { return _gameStage.GetObjects<SmartActor>();}
    /// <summary>
    /// 在当前场景获取能被主玩家看到的所有npc对象 by吴江
    /// </summary>
    /// <param name="_gameStage"></param>
    /// <param name="_player"></param>
    /// <returns></returns>
    public static List<NPC> GetCullNPCs(this GameStage _gameStage, MainPlayer _player)
    {
        List<NPC> npcs = _gameStage.GetNPCs();
        List<NPC> cullNPCs = new List<NPC>();
        foreach (NPC npc in npcs)
        {
            if (npc.gameObject != null)
            {
                Vector3 vec = _player.transform.position - npc.gameObject.transform.position;
                if (vec.sqrMagnitude <= npc.cullDistance * npc.cullDistance)
                {
                    cullNPCs.Add(npc);
                }

            }
        }
        return cullNPCs;
    }


    /// <summary>
    /// 在当前场景获取能被主玩家看到的所有虚拟体npc对象 by吴江
    /// </summary>
    /// <param name="_gameStage"></param>
    /// <param name="_player"></param>
    /// <returns></returns>
    public static List<NPC> GetCullDummyNPCs(this GameStage _gameStage, MainPlayer _player)
    {
        List<NPC> npcs = _gameStage.GetNPCs();
        List<NPC> cullNPCs = new List<NPC>();
        foreach (NPC npc in npcs)
        {
            if (npc.gameObject != null && npc.isDummy)
            {
                Vector3 vec = _player.transform.position - npc.gameObject.transform.position;
                if (vec.sqrMagnitude <= npc.cullDistance * npc.cullDistance)
                {
                    cullNPCs.Add(npc);
                }

            }
        }
        return cullNPCs;
    }

    /// <summary>
    /// 在当前场景获取能被主玩家看到的所有虚拟体传送点对象 by吴江
    /// </summary>
    /// <param name="_gameStage"></param>
    /// <param name="_player"></param>
    /// <returns></returns>
    public static List<FlyPoint> GetCullDummyFlypoints(this GameStage _gameStage, MainPlayer _player)
    {
        List<FlyPoint> flypoints = _gameStage.GetFlypoints();
        List<FlyPoint> cullFly = new List<FlyPoint>();
        foreach (FlyPoint fly in flypoints)
        {
            if (fly.gameObject != null && fly.isDummy)
            {
                Vector3 vec = _player.transform.position - fly.gameObject.transform.position;
                if (vec.sqrMagnitude <= fly.cullDistance * fly.cullDistance)
                {
                    cullFly.Add(fly);
                }

            }
        }
        return cullFly;
    }

	/// <summary>
	/// 切换目标怪物
	/// </summary>
	public static SmartActor GetAnotherSmartActor(this GameStage _gameStage, int _old, Vector3 _comparePos)
	{
		List<Monster> monsters = _gameStage.GetMobs();
		List<OtherPlayer> opcs = _gameStage.GetOtherPlayers();
		List<SmartActor> sActors = new List<SmartActor>();

		int selfCamp = GameCenter.curMainPlayer.Camp;
		SceneType sceneType = GameCenter.curGameStage.SceneType;
		SceneRef sceneRef = GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneRef;
		for (int i = 0; i < opcs.Count; i++)
		{
			if(sceneRef != null && sceneRef.pk_mode == 0)
				break;//不强制玩家切换PK模式的场景里,切换目标不锁定玩家
			OtherPlayer opc = opcs[i];
            if (opc.isDead || opc.actorInfo.IsHide) continue;
			if (PlayerAutoFightFSM.IsEnemy(opc) && !opc.IsActor && !opc.isDead)//修改玩家是否敌对判断  by邓成
			{
				if ((_comparePos - opc.transform.position).sqrMagnitude <= 15.0f * 15.0f) //2015/12/30 翟照要求15距离以内
				{
					sActors.Add(opc);
				}
			}
		}

		for (int i = 0; i < monsters.Count; i++)
		{
			Monster mob = monsters[i];
            if (mob.isDummy || !mob.IsShowing || mob.isDead) continue;
            if (GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneRef!= null)
            {
                if (GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneRef.sort == SceneType.SCUFFLEFIELD)
                {
                    if (GameCenter.systemSettingMng.IsHideBoss && mob.actorInfo.IsBoss)
                    {
                        continue;//自动寻怪避开BOSS  by黄洪兴
                    }
                }
            }
			if (ConfigMng.Instance.GetRelationType(selfCamp, mob.Camp, sceneType) == RelationType.AUTOMATEDATTACKS && !mob.IsActor && !mob.isDead)
			{
				if ((_comparePos - mob.transform.position).sqrMagnitude <= 15.0f * 15.0f) //2015/12/30 翟照要求15距离以内
				{
					sActors.Add(mob);
				}
			}
		}
		int count = sActors.Count;
		if (count == 0)
		{
			return null;
		}
		if (count == 1)
		{
			return sActors[0];
		}
		System.Random rm = new System.Random();
		int flag = 0;
		while (true)
		{
			flag++;
			if (flag > count) break;
			int index = rm.Next(0, count - 1);
			SmartActor monster = sActors[index];
			if (monster.id != _old)
				return monster;
		}
		return null;
	}

	/// <summary>
	/// 切换目标怪物
	/// </summary>
    public static Monster GetAnotherMob(this GameStage _gameStage, int _old)
    {
        List<Monster> monsters = _gameStage.GetMobs();
        List<Monster> mobs = new List<Monster>();

        int selfCamp = GameCenter.curMainPlayer.Camp;
        SceneType sceneType = GameCenter.curGameStage.SceneType;

        for (int i = 0; i < monsters.Count; i++)
        {
            Monster mob = monsters[i];
            if (mob.isDummy || !mob.IsShowing || mob.isDead) continue;
            if (ConfigMng.Instance.GetRelationType(selfCamp, mob.Camp, sceneType) == RelationType.AUTOMATEDATTACKS && !mob.IsActor && !mob.isDead)
            {
                if ((GameCenter.curMainPlayer.transform.position - mob.transform.position).sqrMagnitude <= 15.0f * 15.0f) //2015/12/30 翟照要求15距离以内
                {
                    mobs.Add(mob);
                }
            }
        }
        int count = mobs.Count;
        if (count == 0)
        {
            return null;
        }
        if (count == 1)
        {
            return mobs[0];
        }
        System.Random rm = new System.Random();
        int flag = 0;
        while (true)
        {
            flag++;
            if (flag > count) break;
            int index = rm.Next(0, count - 1);
            Monster monster = mobs[index];
            if (monster.id != _old)
                return monster;
        }
        return null;
    }


    /// <summary>
    /// 获取路径最近的怪物
    /// </summary>
    /// <param name="_gameStage"></param>
    /// <param name="_player"></param>
    /// <param name="_needAlive"></param>
    /// <returns></returns>
    public static Monster GetClosestMob(this GameStage _gameStage, SmartActor _player, ref float _distance)
    {
        List<Monster> mobs = _gameStage.GetMobs();
        FDictionary distanceDic = new FDictionary();
        FDictionary mobDic = new FDictionary();
        int selfCamp = _player.Camp;
        SceneType sceneType = GameCenter.curGameStage.SceneType;
        Vector3 selfPosition = _player.transform.position;


        for (int i = 0; i < mobs.Count; i++)
        {
            Monster mob = mobs[i];
            if (mob.isDummy || !mob.IsShowing || mob.isDead) continue;
            if (mob.gameObject != null && !mob.isDead && !mob.IsActor
                && ConfigMng.Instance.GetRelationType(selfCamp, mob.Camp, sceneType) == RelationType.AUTOMATEDATTACKS)
            {
                Vector3[] path = GameStageUtility.StartPath(selfPosition, mob.transform.position);
                if (path != null)
                {
                    if (path.Length != 2)
                    {
                        distanceDic.Add(mob.id, path.CountPathDistance());//距离计算方法改变
                    }
                    else
                    {
                        distanceDic.Add(mob.id, Vector3.Distance(selfPosition, mob.transform.position));
                    }
                    mobDic.Add(mob.id, mob);
                }
            }
        }
        int closestOne = -1;
        float distance = -1;
        foreach (int id in distanceDic.Keys)
        {
            if (distance < 0 || distance >= (float)distanceDic[id])
            {
                closestOne = id;
                distance = (float)distanceDic[id];
                _distance = distance;
            }
        }
        return mobDic.ContainsKey(closestOne) ? mobDic[closestOne] as Monster : null;
    }
    /// <summary>
    /// 获取路径最近的怪物
    /// </summary>
    /// <param name="_gameStage"></param>
    /// <param name="_player"></param>
    /// <param name="_needAlive"></param>
    /// <returns></returns>
    public static Monster GetClosestMob(this GameStage _gameStage, SmartActor _player)
    {
        float distance = 0;
        return _gameStage.GetClosestMob(_player, ref distance);
    }

    /// <summary>
    /// 获取路径最近指定关系玩家 by吴江
    /// </summary>
    /// <param name="_gameStage"></param>
    /// <param name="_player"></param>
    /// <param name="_needAlive"></param>
    /// <returns></returns>
    public static OtherPlayer GetClosestPlayer(this GameStage _gameStage, SmartActor _player, RelationType _relationType ,ref float _distance)
    {
        List<OtherPlayer> opcs = _gameStage.GetOtherPlayers();
        FDictionary distanceDic = new FDictionary();
        FDictionary opcDic = new FDictionary();
        int selfCamp = _player.Camp;
        SceneType sceneType = GameCenter.curGameStage.SceneType;
        Vector3 selfPosition = _player.transform.position;

        for (int i = 0; i < opcs.Count; i++)
        {
            OtherPlayer opc = opcs[i];
            if (opc.isDead || opc.actorInfo.IsHide) continue;
            if (opc.gameObject != null && !opc.isDead && !opc.IsActor
                && ConfigMng.Instance.GetRelationType(selfCamp, opc.Camp, sceneType) == _relationType)
            {
                Vector3[] path = GameStageUtility.StartPath(selfPosition, opc.transform.position);
                if (path != null)
                {
                    if (path.Length != 2)
                    {
                        distanceDic.Add(opc.id, path.CountPathDistance());//距离计算方法改变
                    }
                    else
                    {
                        distanceDic.Add(opc.id, Vector3.Distance(selfPosition, opc.transform.position));
                    }
                    opcDic.Add(opc.id, opc);
                }
            }
        }
        int closestOne = -1;
        float distance = -1;
        foreach (int id in distanceDic.Keys)
        {
            if (distance < 0 || distance >= (float)distanceDic[id])
            {
                closestOne = id;
                distance = (float)distanceDic[id];
                _distance = distance;
            }
        }
        return opcDic.ContainsKey(closestOne) ? opcDic[closestOne] as OtherPlayer : null;
    }

    /// <summary>
    /// 获取路径最近指定关系随从 by吴江
    /// </summary>
    /// <param name="_gameStage"></param>
    /// <param name="_player"></param>
    /// <param name="_needAlive"></param>
    /// <returns></returns>
    public static OtherEntourage GetClosestEntourage(this GameStage _gameStage, SmartActor _player, RelationType _relationType, ref float _distance)
    {
        List<OtherEntourage> opcs = _gameStage.GetOtherEntourages();
        FDictionary distanceDic = new FDictionary();
        FDictionary opcDic = new FDictionary();
        int selfCamp = _player.Camp;
        SceneType sceneType = GameCenter.curGameStage.SceneType;
        Vector3 selfPosition = _player.transform.position;

        for (int i = 0; i < opcs.Count; i++)
        {
            OtherEntourage opc = opcs[i];
            if (opc.isDummy || !opc.IsShowing) continue;
            if (opc.gameObject != null && !opc.isDead && !opc.IsActor
                && ConfigMng.Instance.GetRelationType(selfCamp, opc.Camp, sceneType) == _relationType)
            {
                Vector3[] path = GameStageUtility.StartPath(selfPosition, opc.transform.position);
                if (path != null)
                {
                    if (path.Length != 2)
                    {
                        distanceDic.Add(opc.id, path.CountPathDistance());//距离计算方法改变
                    }
                    else
                    {
                        distanceDic.Add(opc.id, Vector3.Distance(selfPosition, opc.transform.position));
                    }
                    opcDic.Add(opc.id, opc);
                }
            }
        }
        int closestOne = -1;
        float distance = -1;
        foreach (int id in distanceDic.Keys)
        {
            if (distance < 0 || distance >= (float)distanceDic[id])
            {
                closestOne = id;
                distance = (float)distanceDic[id];
                _distance = distance;
            }
        }
        return opcDic.ContainsKey(closestOne) ? opcDic[closestOne] as OtherEntourage : null;
    }




    public static float CountPathDistance(this Vector3[] _path)
    {
        float distance = 0;
        int count = _path.Length - 1;
        if (count > 0)
        {
            for (int i = 0; i < count; i++)
            {
                distance += Vector3.Distance(_path[i].SetY(0), _path[i + 1].SetY(0));
            }
        }
        return distance;
    }
    /// <summary>
    /// 获取前方的最近的对象 by吴江
    /// </summary>
    /// <param name="_gameStage"></param>
    /// <param name="_player"></param>
    /// <param name="_distance"></param>
    /// <returns></returns>
    public static SmartActor GetClosestSmartActorInFront(this GameStage _gameStage, PlayerBase _player, RelationType _relationType, ref float _distance)
    {
        if (_player == null) return null;
        List<SmartActor> list = _gameStage.GetSmartActors();
        List<SmartActor> inFrontList = new List<SmartActor>();
        if (list.Count == 0) return null;
        for (int i = 0; i < list.Count; i++)
        {
            SmartActor sm = list[i];
             float angle = Mathf.Acos (Vector3.Dot ((sm.transform.position - _player.transform.position).normalized, _player.transform.forward)) * Mathf.Rad2Deg;
             if (Mathf.Abs(angle) < 90)
             {
                 inFrontList.Add(sm);
             }
        }
        return GetClosestSmartActor(_player, inFrontList, _relationType, ref _distance);
    }

    /// <summary>
    /// 最近的对象 by吴江
    /// </summary>
    public static SmartActor GetClosestSmartActor(this GameStage _gameStage, PlayerBase _player,RelationType _relationType, ref float _distance)
    {
        return GetClosestSmartActor(_player, _gameStage.GetSmartActors(), _relationType, ref _distance);
    }


    public static List<SmartActor> GetSmartActors(this GameStage _gameStage)
    {
        List<SmartActor> smartActors = new List<SmartActor>();
        List<Monster> mobs = _gameStage.GetMobs();
        List<OtherPlayer> opcs = _gameStage.GetOtherPlayers();
        List<OtherEntourage> opes = _gameStage.GetOtherEntourages();
        for (int i = 0; i < mobs.Count; i++)
        {
            smartActors.Add(mobs[i]);
        }
        for (int i = 0; i < opcs.Count; i++)
        {
            smartActors.Add(opcs[i]);
        }
        for (int i = 0; i < opes.Count; i++)
        {
            smartActors.Add(opes[i]);
        }
        return smartActors;
    }

    /// <summary>
    /// 获取路径最近的怪物
    /// </summary>
    /// <param name="_gameStage"></param>
    /// <param name="_player"></param>
    /// <param name="_needAlive"></param>
    /// <returns></returns>
    public static SmartActor GetClosestSmartActor(PlayerBase _player, List<SmartActor> smartActors, RelationType _relationType,ref float _instance)
    {
        List<SmartActor> smActors = smartActors;
        FDictionary distanceDic = new FDictionary();
        FDictionary mobDic = new FDictionary();


        int selfCamp = _player.Camp;
        SceneType sceneType = GameCenter.curGameStage.SceneType;
        Vector3 selfPosition = _player.transform.position;

        for (int i = 0; i < smActors.Count; i++)
        {
            SmartActor smActor = smActors[i];
            if (smActor.gameObject != null && !smActor.isDead && !smActor.IsActor
                && ConfigMng.Instance.GetRelationType(selfCamp, smActor.Camp, sceneType) == _relationType)
            {
                Vector3[] path = GameStageUtility.StartPath(selfPosition, smActor.transform.position);
                if (path != null)
                {
                    if (path.Length != 2)
                    {
                        distanceDic.Add(smActor.id, path.CountPathDistance());//距离计算方法改变
                    }
                    else
                    {
                        distanceDic.Add(smActor.id, Vector3.Distance(selfPosition, smActor.transform.position));
                    }
                    mobDic.Add(smActor.id, smActor);
                }
            }
        }
        int closestOne = -1;
        float distance = -1;
        foreach (int id in distanceDic.Keys)
        {
            if (distance < 0 || distance >= (float)distanceDic[id])
            {
                closestOne = id;
                distance = (float)distanceDic[id];
            }
        }
        _instance = distance;
        return mobDic.ContainsKey(closestOne) ? mobDic[closestOne] as SmartActor : null;
    }
    ///// <summary>
    ///// 获取路径最近的怪物 没有阵营区分
    ///// </summary>
    ///// <param name="_gameStage"></param>
    ///// <param name="_player"></param>
    ///// <param name="_needAlive"></param>
    ///// <returns></returns>
    //public static SmartActor GetClosestSmartActor(PlayerBase _player,bool _needAlive,List<SmartActor> smartActors)
    //{	
    //    List<SmartActor> mobs = smartActors;
    //    Dictionary<int, float> distanceDic = new Dictionary<int, float>();
    //    Dictionary<int, SmartActor> mobDic = new Dictionary<int, SmartActor>();
    //    if (_needAlive)
    //    {
    //        foreach (SmartActor mob in mobs)
    //        {
				
    //            if (mob.gameObject != null && !mob.isDead && !mob.IsActor
    //                )
    //            {
    //                MOVEDES move = map.Find_Path(GameCenter.curGameStage.mapConfig, MYPOint.local2worldV3(_player.transform.localPosition), 
    //                                             MYPOint.local2worldV3(mob.gameObject.transform.position));
    //                if(move != null)
    //                {
    //                    if(move.way.Count != 1)
    //                        distanceDic.Add(mob.id, move.CountDistance);//距离计算方法改变	by 					
    //                    else	
    //                        distanceDic.Add(mob.id, Mathf.Sqrt((_player.transform.localPosition-mob.gameObject.transform.position).sqrMagnitude));						
    //                    mobDic.Add(mob.id, mob);						
    //                }
    //            }
    //        }
    //    }
    //    else
    //    {
    //        foreach (SmartActor mob in mobs)
    //        {
    //            if (mob.gameObject != null && !mob.IsActor
    //                && ConfigMng.Instance.GetRelationType(GameCenter.curMainPlayer.Camp,mob.Camp) == RelationType.AUTOMATEDATTACKS)
    //            {
    //                Vector3 vec = _player.transform.position - mob.gameObject.transform.position;
    //                distanceDic.Add(mob.id, vec.sqrMagnitude);
    //                mobDic.Add(mob.id, mob);
    //            }
    //        }
    //    }
    //    int closestOne = -1;
    //    float distance = -1;
    //    foreach (var id in distanceDic.Keys)
    //    {
    //        if (distance <0 || distance >= distanceDic[id])
    //        {
    //            closestOne = id;
    //            distance = distanceDic[id];
    //        }
    //    }
    //    return mobDic.ContainsKey(closestOne) ? mobDic[closestOne] : null;
    //}


    /// <summary>
    /// 获得距离指定玩家最近的指定类型的NPC by吴江
    /// </summary>
    /// <param name="_gameStage"></param>
    /// <param name="_player">指定的玩家</param>
    /// <param name="_type">指定的NPC类型</param>
    /// <returns></returns>
    public static NPC GetClosestTypeNPC(this GameStage _gameStage, PlayerBase _player, NPCType _type)
    {
        List<NPC> npcs = _gameStage.GetNPCs();
        Dictionary<int, float> distanceDic = new Dictionary<int, float>();
        Dictionary<int, NPC> npcDic = new Dictionary<int, NPC>();
        for (int i = 0; i < npcs.Count; i++)
        {
            NPC npc = npcs[i];
            if (npc.gameObject != null && npc.npcType == _type)
            {
                Vector3 vec = _player.transform.position - npc.gameObject.transform.position;
                if (vec.sqrMagnitude <= npc.cullDistance * npc.cullDistance)
                {
                    distanceDic.Add(npc.id, vec.sqrMagnitude);
                    npcDic.Add(npc.id, npc);
                }
            }
        }
        int closestOne = -1;
        float distance = -1;
        foreach (var id in distanceDic.Keys)
        {
            if (distance < 0 || distance >= distanceDic[id])
            {
                closestOne = id;
                distance = distanceDic[id];
            }
        }
        return npcDic.ContainsKey(closestOne) ? npcDic[closestOne] : null;
    }

    /// <summary>
    /// 获得距离指定玩家距离最近的指定类型的NPC的距离 by吴江
    /// </summary>
    /// <param name="_gameStage"></param>
    /// <param name="_player"></param>
    /// <param name="_type"></param>
    /// <returns></returns>
    public static float GetClosestTypeNPCDistance(GameStage _gameStage, PlayerBase _player, NPCType _type)
    {
        if (_player == null || _player.gameObject == null) return -1;
        List<NPC> npcs = _gameStage.GetNPCs();
        Dictionary<int, float> distanceDic = new Dictionary<int, float>();
        Dictionary<int, NPC> npcDic = new Dictionary<int, NPC>();
        for (int i = 0; i < npcs.Count; i++)
        {
            NPC npc = npcs[i];
            if (npc.gameObject != null && !npc.isDummy && npc.npcType == _type)
            {
                Vector3 vec = _player.transform.position - npc.gameObject.transform.position;
                if (vec.sqrMagnitude <= npc.cullDistance * npc.cullDistance)
                {
                    distanceDic.Add(npc.id, vec.sqrMagnitude);
                    npcDic.Add(npc.id, npc);
                }
            }
        }
        //int closestOne = -1;
        float distance = -1;
        foreach (var id in distanceDic.Keys)
        {
            if (distance < 0 || distance >= distanceDic[id])
            {
                //closestOne = id;
                distance = distanceDic[id];
            }
        }
        return distance;
    }


    /// <summary>
    /// 获得距离排名的NPC by吴江
    /// </summary>
    /// <param name="_gameStage"></param>
    /// <param name="_player"></param>
    /// <param name="_index">距离远近的排名，0为最近</param>
    /// <returns></returns>
    public static NPC GetCloseNPCByIndex(GameStage _gameStage, PlayerBase _player, int _index)
    {
        if (_player == null || _player.gameObject == null) return null;
        List<NPC> npcs = _gameStage.GetNPCs();
        Dictionary<float, int> distanceDic = new Dictionary<float, int>();
        List<float> distanceArray = new List<float>();
        Dictionary<int, NPC> npcDic = new Dictionary<int, NPC>();

        for (int i = 0; i < npcs.Count; i++)
        {
            NPC npc = npcs[i];
            if (npc.gameObject != null && !npc.isDummy)
            {
                Vector3 vec = _player.transform.position - npc.gameObject.transform.position;
                if (vec.sqrMagnitude <= npc.cullDistance * npc.cullDistance)
                {
                    distanceDic[vec.sqrMagnitude] = npc.id;
                    distanceArray.Add(vec.sqrMagnitude);
                    npcDic.Add(npc.id, npc);
                }
            }
        }

        for (int i = 0; i < distanceArray.Count; i++)
        {
            for (int j = distanceArray.Count - 1; j > i; j--)
            {
                if (distanceArray[i] > distanceArray[j])
                {
                    float temp = distanceArray[i];
                    distanceArray[i] = distanceArray[j];
                    distanceArray[j] = temp;
                }
            }
        }
        if (distanceArray.Count > _index && distanceDic.ContainsKey(distanceArray[_index]) && npcDic.ContainsKey(distanceDic[distanceArray[_index]]))
        {
            return npcDic[distanceDic[distanceArray[_index]]];
        }
        return null;
    }


    /// <summary>
    /// 获取路径最近的场景物品
    /// </summary>
    /// <param name="_gameStage"></param>
    /// <param name="_player"></param>
    /// <param name="_needAlive"></param>
    /// <returns></returns>
    public static SceneItem GetClosestSceneItem(this GameStage _gameStage, PlayerBase _player)
    {
        List<SceneItem> sceneItems = _gameStage.GetSceneItems();
        FDictionary distanceDic = new FDictionary();
        FDictionary sceneDic = new FDictionary();
        //int selfCamp = _player.Camp;
        //SceneType sceneType = GameCenter.curGameStage.SceneType;
        Vector3 selfPosition = _player.transform.position;

        Debug.Log(sceneItems.Count);
        for (int i = 0; i < sceneItems.Count; i++)
        {
            SceneItem sceneItem = sceneItems[i];
            Debug.Log(sceneItem.isDummy + ":" + sceneItem.IsShowing);
        //    if (sceneItem.isDummy || !sceneItem.IsShowing) continue;
            if (sceneItem.gameObject != null)
            {
                Vector3[] path = GameStageUtility.StartPath(selfPosition, sceneItem.transform.position);
                if (path != null)
                {
                    if (path.Length != 2)
                    {
                        distanceDic.Add(sceneItem.id, path.CountPathDistance());//距离计算方法改变
                    }
                    else
                    {
                        distanceDic.Add(sceneItem.id, Vector3.Distance(selfPosition, sceneItem.transform.position));
                    }
                    sceneDic.Add(sceneItem.id, sceneItem);
                }
            }
        }
        int closestOne = -1;
        float distance = -1;
        foreach (int id in distanceDic.Keys)
        {
            if (distance < 0 || distance >= (float)distanceDic[id])
            {
                closestOne = id;
                Debug.Log(closestOne);
                distance = (float)distanceDic[id];
                
            }
        }
        return sceneDic.ContainsKey(closestOne) ? sceneDic[closestOne] as SceneItem : null;
    }


}