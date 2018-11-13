///////////////////////////////////////////////////////////////////////////////
//作者：吴江
//日期：2015/7/20
//用途：对象渲染控制器
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SmartActorRendererCtrl : ActorRendererCtrl
{

    public enum LODLevel {
        Unknown = -1,
        Full = 0,       // 武器和防具
        ArmorOnly,      // 仅仅防具
        RawMesh,        // 普通模型
        SimpleCharacter // 角色 (顶点必须小于300才能batch)
    }



    // LOD
    protected LODLevel lodLevel = LODLevel.Unknown;
    /// <summary>
    /// 数据层对象引用
    /// </summary>
    protected ActorInfo actorInfo;
    /// <summary>
    /// 特效控制器
    /// </summary>
    protected FXCtrl fxCtrl;

    /// <summary>
    /// 角色骨骼
    /// </summary>
    protected Dictionary<string, Transform> cachedBones = new Dictionary<string, Transform>();


    protected new void Awake()
    {
        base.Awake();

        originalLayer = this.gameObject.layer;

        Renderer[] rendererList = GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rendererList)
        {
            if (r is ParticleRenderer == false)
                r.gameObject.layer = originalLayer;
        }

    }


    public void Init(ActorInfo _actorInfo,FXCtrl _fxCtrl)
    {
        actorInfo = _actorInfo;
        fxCtrl = _fxCtrl;
        if (actorInfo != null)
        {
            // cache bones
            Transform rootBone = transform.Find(actorInfo.Bone_Name);
            if (rootBone != null)
            {
                cachedBones = MeshHelper.CacheBones(rootBone);
            }
            else
            {
                Debug.LogError(actorInfo.Name + " , " + this.transform.root.name +  " 找不到指定的骨骼根目录！" + actorInfo.Bone_Name);
            }
        }
        EquipAll(); 
    }

	/// <summary>
	/// //重新绑定info,否则断线重连后,这个里面的actorInfo还是断线重连之前的actorInfo  by邓成
	/// </summary>
	public void ReBindInfo(ActorInfo _actorInfo)
	{
		actorInfo = _actorInfo;
	}


    public override bool Show(bool _show, bool _force = false)
    {
        if (base.Show(_show, _force))
        {
            List<Renderer> allRendererList = this.gameObject.GetComponentsInChildrenFast<Renderer>(true);
            for (int i = 0; i < allRendererList.Count; ++i)
            {
                allRendererList[i].enabled = _show;
            }
            return true;
        }
        return false;
    }

    public void ActivateCollider(bool _activate)
    {
        Collider collider = gameObject.GetComponentInChildrenFast<Collider>();
        if (collider != null)
        {
            collider.enabled = _activate;
        }
    }

    /// <summary>
    /// 初始化特效
    /// </summary>
    protected virtual void InitEffects()
    {
        if (fxCtrl == null) return;
        foreach (var item in actorInfo.CurShowDictionary.Values)
        {
            if (item != null && item.BoneEffectList.Count > 0)
            {
                for (int i = 0; i < item.BoneEffectList.Count; i++)
                {
                    fxCtrl.SetBoneEffect(item.BoneEffectList[i].boneName, item.BoneEffectList[i].effectName, actorInfo.ModelScale);
                }
            }
        }
        if (actorInfo != null && actorInfo.CurStarEffect.Length > 0 && actorInfo.CosmeticDictionary.Count == 0)
        {
            lastStarEffect = actorInfo.CurStarEffect;
            fxCtrl.SetBoneEffect("hitPoint", actorInfo.CurStarEffect, actorInfo.ModelScale);
        }
        //fxCtrl.CheckBoneEffectList();
		//fxCtrl.ShowBoneEffect(true);
    }

    protected string lastStarEffect = string.Empty;

    public void UpdateStarEffect()
    {
        if (actorInfo.CurStarEffect != lastStarEffect)
        {
            lastStarEffect = actorInfo.CurStarEffect;
            if (lastStarEffect.Length > 0)
            {
                fxCtrl.ClenBoneEffect("hitPoint");
                if(actorInfo != null && actorInfo.CosmeticDictionary.Count == 0)
                {
                fxCtrl.SetBoneEffect("hitPoint", lastStarEffect, actorInfo.ModelScale);
                }
            }
        }
    }




    protected new void LateUpdate()
    {
        base.LateUpdate();

        if (armorDirty && pendingArmors == 0)
        {
            armorDirty = false;
            fxCtrl.CleanBonesEffect();
            CombineArmor();
            InitEffects();
        }
    }


    protected bool isInComBat = true;
    /// <summary>
    /// 进入战斗状态切换 by吴江 
    /// </summary>
    /// <param name="_inCombat"></param>
    public void SetInCombat(bool _inCombat)
    {
        isInComBat = _inCombat;
        if (actorInfo != null)
        {
			EquipmentInfo mainWeapon = actorInfo.CurShowDictionary.ContainsKey(EquipSlot.weapon)?actorInfo.CurShowDictionary[EquipSlot.weapon]:null;
            if (mainWeapon != null)
            {
                GameObject mainWeaponObj = weaponDictionary[EquipSlot.weapon] as GameObject;
                SetWeaponPoint(mainWeapon, mainWeaponObj, _inCombat);
            }
			EquipmentInfo spacialWeapon = actorInfo.CurShowDictionary.ContainsKey(EquipSlot.special)?actorInfo.CurShowDictionary[EquipSlot.special]:null;
            if (spacialWeapon != null)
            {
                GameObject spacialWeaponObj = weaponDictionary[EquipSlot.special] as GameObject;
                SetWeaponPoint(spacialWeapon, spacialWeaponObj, _inCombat);
            }
        }
    }

    #region 设置实时阴影  by吴江
    public override void CastShadow(bool _cast)
    {
        base.CastShadow(_cast);
        foreach (GameObject item in weaponDictionary.Values)
        {
            if (item != null)
            {
                Renderer rd = item.GetComponent<Renderer>();
                if (rd != null)
                {
                    rd.shadowCastingMode = _cast ? UnityEngine.Rendering.ShadowCastingMode.On : UnityEngine.Rendering.ShadowCastingMode.Off;
                }
            }
        }
    }
    #endregion

    public void SetWeaponPoint(EquipmentInfo _info,GameObject _obj,bool _inCombat)
    {
        if (_obj != null)
        {
            Transform point = null;
            if (_inCombat)
            {
                if (cachedBones.ContainsKey(_info.FightWeaponPointName))
                {
                    point = cachedBones[_info.FightWeaponPointName];
                }
            }
            else
            {
                if (cachedBones.ContainsKey(_info.UnFightWeaponPointName))
                {
                    point = cachedBones[_info.UnFightWeaponPointName];
                }
            }
            if (point != null)
            {
                _obj.transform.parent = point;
                _obj.transform.localScale = Vector3.one;
                _obj.transform.localEulerAngles = Vector3.zero;
                _obj.transform.localPosition = Vector3.zero;
            }
        }
    }

    #region 换装部分
    protected bool armorDirty = false;
    /// <summary>
    /// 是否还未合并完毕
    /// </summary>
    public bool ArmorDirty
    {
        get
        {
            return armorDirty;
        }
    }
    protected int pendingArmors = 0;
    public int PendingArmors
    {
        get
        {
            return pendingArmors;
        }
    }
    protected GameObject[] armorList = new GameObject[(int)EquipSlot.count];
    public GameObject[] getArmorList    // OBSOLETE
    {
        get { return armorList; }
    }

    protected FDictionary weaponDictionary = new FDictionary();

    protected AssetMng.DownloadID[] armorDownloadingList = new AssetMng.DownloadID[(int)EquipSlot.count];


    public virtual void EquipAll()
    {
        if (isMorphing) return;
        foreach (var item in actorInfo.CurShowDictionary)
        {
            Equip(item.Key, item.Value);
        }
    }


    public virtual void Equip(EquipSlot _slot, EquipmentInfo _eq)
    {
        if (isMorphing || _eq == null) return;
        if (!_eq.HasModel() && !actorInfo.DefaultDictionary.ContainsKey(_eq.Slot)) return;
        if (!_eq.HasModel())
        {
            if (actorInfo.DefaultDictionary.ContainsKey(_eq.Slot))
            {
                if (_eq.ShowType == ShowType.SKIN)
                {
                    armorList[(int)_eq.Slot] = null;
                    armorDirty = true;
                }
                else if (_eq.ShowType == ShowType.HANGINGPOINT)
                {
                    if (weaponDictionary[_eq.Slot] != null)
                    {
                        Destroy(weaponDictionary[_eq.Slot] as GameObject);
                        weaponDictionary[_eq.Slot] = null;
                        weaponDictionary.Remove(_eq.Slot);
                    }
                }
            }
            return;
        }
        if (!_eq.WillChangeRender)
        {
            return;
        }
        switch (_eq.ShowType)
        {
            case ShowType.HANGINGPOINT:
                EquipWeapon(_eq,_slot, _eq.HasModel() ? _eq.ShortUrl : actorInfo.DefaultDictionary[_eq.Slot].ShortUrl, _eq.ModelName);
                break;
            case ShowType.SKIN:
                EquipArmor(_slot, _eq.HasModel() ? _eq.ShortUrl : actorInfo.DefaultDictionary[_eq.Slot].ShortUrl, _eq.ModelName);
                break;
            default:
                break;
        }
    }


    protected void Equip(EquipSlot _slot, string _url)
    {
        if (_url.Length == 0 || _url == "0") return;
        EquipArmor(_slot, _url,"");
    }


    public virtual void EquipUpdate(EquipSlot _slot)
    {
        if (actorInfo == null) return;
        Equip(_slot, actorInfo.CurShowDictionary[_slot]);
    }

    public virtual void EquipWeapon(EquipmentInfo _eq, EquipSlot _slot, string _shortURL, string _name)
    {
        int slot = (int)_slot;

        ++pendingArmors;
        armorDirty = true;

        AssetMng.DownloadID id = armorDownloadingList[slot];
        if (id != null)
        {
            --pendingArmors;
            AssetMng.instance.CancelDownload(id);
            armorDownloadingList[slot] = null;
        }

        bool done = false;
        id = AssetMng.instance.LoadAsset<GameObject>(_shortURL,
                                                       "",
                                                       delegate(GameObject _prefab, EResult _result)
                                                       {
                                                           --pendingArmors;
                                                           done = true;
                                                           if (_result == EResult.Success)
                                                           {
                                                               GameCenter.curGameStage.CacheEquipmentURL(_shortURL,_prefab);
                                                               armorDownloadingList[slot] = null;

                                                               Renderer smr = _prefab.GetComponentInChildrenFast<Renderer>(true);
                                                               if (smr != null)
                                                               {
                                                                   smr.sharedMaterial.shader = Shader.Find(smr.sharedMaterial.shader.name);
                                                               }
                                                               else
                                                               {
                                                               //    Debug.Log("smr 为空");
                                                               }
                                                               Transform point = null;
                                                               if (isInComBat)
                                                               {
                                                                   if (cachedBones.ContainsKey(_eq.FightWeaponPointName))
                                                                   {
                                                                       point = cachedBones[_eq.FightWeaponPointName];
                                                                   }
                                                                   else
                                                                   {
                                                                       point = this.transform.FindChild(_eq.FightWeaponPointName);
                                                                       if (point == null)
                                                                       {
                                                                           Debug.LogError("战斗挂点丢失 " + _eq.FightWeaponPointName);
                                                                       }
                                                                   } 
                                                               }
                                                               else
                                                               {
                                                                   if (cachedBones.ContainsKey(_eq.UnFightWeaponPointName))
                                                                   {
                                                                       point = cachedBones[_eq.UnFightWeaponPointName];
                                                                   }
                                                                   else
                                                                   {
                                                                       point = this.transform.FindChild(_eq.UnFightWeaponPointName);
                                                                       if (point == null)
                                                                       {
                                                                           Debug.LogError("非战斗挂点丢失 " + _eq.UnFightWeaponPointName);
                                                                       }		                                           
                                                                   }
                                                               }
                                                               if (point != null)
                                                               {
                                                                   GameObject weapon = Instantiate(_prefab) as GameObject;
                                                                   weapon.transform.parent = point;
                                                                   weapon.transform.localScale = Vector3.one;
                                                                   weapon.transform.localEulerAngles = Vector3.zero;
                                                                   weapon.transform.localPosition = Vector3.zero;
                                                                   weapon.SetMaskLayer(gameObject.layer);
                                                                   if (weaponDictionary.ContainsKey(_eq.Slot) && weaponDictionary[_eq.Slot] != null)
                                                                   {
                                                                       Destroy(weaponDictionary[_eq.Slot] as GameObject);
                                                                   }
                                                                   weaponDictionary[_eq.Slot] = weapon;
                                                                    bool hide = false;
                                                                    OtherPlayer actor = GameCenter.curGameStage.GetOtherPlayer(actorInfo.ServerInstanceID);
                                                                    if (actor != null)
                                                                    {
                                                                        if (!actor.IsShowing)
                                                                            hide = true;
                                                                    }
																	if(hide)//隐藏的玩家,进入视野时,可能导致武器未隐藏  by邓成
																	{
																		List<Renderer> allRendererList = weapon.GetComponentsInChildrenFast<Renderer>(true);
																		for (int i = 0; i < allRendererList.Count; ++i)
																		{
                                                                            //Debug.Log(allRendererList[i].gameObject.name);
																			allRendererList[i].enabled = false;
																		}
																	}

                                                               }
                                                           }
                                                       },
                                                       true
                                                     );
        if (done == false)
        {
            armorDownloadingList[slot] = id;
        }

    }

    public void EquipArmor(EquipSlot _slot, string _shortURL, string _name)
    {
        int slot = (int)_slot;

        ++pendingArmors;
        armorDirty = true;

        AssetMng.DownloadID id = armorDownloadingList[slot];
        if (id != null)
        {
            --pendingArmors;
            AssetMng.instance.CancelDownload(id);
            armorDownloadingList[slot] = null;
        }

        bool done = false;
        id = AssetMng.instance.LoadAsset<GameObject>(_shortURL,
                                                       "",
                                                       delegate(GameObject _prefab, EResult _result)
                                                       {
                                                           --pendingArmors;
                                                           done = true;
                                                           if (_result == EResult.Success)
                                                           {
                                                               GameCenter.curGameStage.CacheEquipmentURL(_shortURL, _prefab);
                                                               armorDownloadingList[slot] = null;
                                                               armorList[slot] = _prefab;

                                                               SkinnedMeshRenderer smr = _prefab.GetComponentInChildrenFast<SkinnedMeshRenderer>(true);
                                                               if (smr != null && smr.sharedMaterial != null)
                                                               {
                                                                   smr.sharedMaterial.shader = Shader.Find(smr.sharedMaterial.shader.name);
                                                               }
                                                               else
                                                               {
                                                                   Debug.Log("smr 为空");
                                                               }
                                                           }
                                                       },
                                                       true
                                                     );
        if (done == false)
        {
            armorDownloadingList[slot] = id;
        }
    }


    protected virtual void CombineArmor()
    {
        if (cachedBones == null)
        {
            Debug.LogError("Failed to combine armor, you didn't cache bones");
            return;
        }
        List<SkinnedMeshRenderer> smrList = new List<SkinnedMeshRenderer>();
        smrList.Add(gameObject.GetComponentInChildrenFast<SkinnedMeshRenderer>());
        for (int i = 0; i < (int)EquipSlot.count; ++i)
        {
            GameObject prefab = armorList[i];
            if (prefab != null)
            {
                SkinnedMeshRenderer smr = prefab.GetComponentInChildrenFast<SkinnedMeshRenderer>(true);
                if (smr != null)
                {
                    smrList.Add(smr);
                }
                else
                {
                    Debug.Log((EquipSlot)i + " SkinnedMeshRenderer为空 ！");
                }
            }
        }
        CombineSkinnedMesh(smrList, cachedBones);
    }


    public override void SetWeaponShader(Shader _shader,Color _color, string _propertyName = "")
    {
        foreach (GameObject item in weaponDictionary.Values)
        {
            if (item != null)
            {
                Renderer rd = item.GetComponent<Renderer>();
                if (rd != null)
                {
                    for (int i = 0; i < rd.materials.Length; i++)
                    {
                        if (rd.materials[i].name.Contains("_glow"))
                        {
                            continue;
                        }
                        rd.materials[i].shader = _shader;
                        if (_propertyName.Length > 0)
                        {
                            rd.materials[i].SetColor(_propertyName, _color);
                        }
                    }
                }
            }
        }
    }

    #endregion 

    #region 变身部分
    /// <summary>
    /// 是否正在变身状态中
    /// </summary>
    bool isMorphing = false;
    /// <summary>
    /// 开始变身
    /// </summary>
    /// <param name="_modelID"></param>
    public virtual void Morph(int _modelID)
    {
        //if (_modelID <= 0) return;
        //ChangeBuffRef refData = ConfigMng.Instance.GetChangeBuffRef(_modelID);
        //if (refData != null)
        //{
        //    isMorphing = true;
        //    if (refData.head != string.Empty && refData.head != "0")
        //    {
        //        Equip(ArmorSlot.head, AssetMng.GetPathWithExtension("Item/" + refData.head, AssetPathType.PersistentDataPath));
        //    }
        //    if (refData.body != string.Empty && refData.body != "0")
        //    {
        //        Equip(ArmorSlot.body, AssetMng.GetPathWithExtension("Item/" + refData.body, AssetPathType.PersistentDataPath));
        //    }
        //    if (refData.sarmor != string.Empty && refData.sarmor != "0")
        //    {
        //        Equip(ArmorSlot.sarmor, AssetMng.GetPathWithExtension("Item/" + refData.sarmor, AssetPathType.PersistentDataPath));
        //    }
        //    if (refData.special != string.Empty && refData.special != "0")
        //    {
        //        Equip(ArmorSlot.special, AssetMng.GetPathWithExtension("Item/" + refData.special, AssetPathType.PersistentDataPath));
        //    }
        //    if (refData.weapon != string.Empty && refData.weapon != "0")
        //    {
        //        Equip(ArmorSlot.weapon, AssetMng.GetPathWithExtension("Item/" + refData.weapon, AssetPathType.PersistentDataPath));
        //    }
        //}
    }


    /// <summary>
    /// 取消变身
    /// </summary>
    public void CancelMorph()
    {
        isMorphing = false;
        EquipAll();
    }

    #endregion




}
