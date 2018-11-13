//=======================================
//作者:吴江
//日期：2015/5/16
//用途：特效控制器
//=======================================



using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 特效控制器 by吴江
/// </summary>
public class FXCtrl : MonoBehaviour
{
    /// <summary>
    /// 是否不受上限限制 by吴江
    /// </summary>
    protected bool isUnLimit = false;
    /// <summary>
    /// 设置是否不受上限限制 by吴江
    /// </summary>
    public void SetUnLimit(bool _unLimit)
    {
        isUnLimit = _unLimit;
    }

    /// <summary>
    /// 是否屏蔽技能特效  by黄洪兴
    /// </summary>
    protected bool isHide = false;
    /// <summary>
    /// 设置是否屏蔽技能特效  by黄洪兴
    /// </summary>
    /// <param name="_unLimit"></param>
    public void SetHide(bool _hide)
    {
        isHide = _hide;
    }

    /// <summary>
    /// 是否还能显示 by吴江
    /// </summary>
    protected bool CanShow
    {
        get
        {
            if (isUnLimit) return true;
            return FXRoot.CurFxCount < GameCenter.systemSettingMng.EffectLimit;
        }
    }

    #region  骨骼特效，用于装备特效 by吴江
    public class BoneEffect
    {
        protected bool preview = false;
        protected GameObject target;
        protected string effectName = string.Empty;
        public string EffectName
        {
            get { return effectName; }
        }
        protected string boneName = string.Empty;
        public string BoneName
        {
            get { return boneName; }
        }

        protected bool isActive = true;
        public bool IsActive
        {
            get
            {
                return isActive;
            }
        }
        protected bool isDonwLoading = false;
        protected bool isShow = true;
        protected float scale = 1.0f;

        public GameObject boneObj = null;
        public GameObject effectObj = null;

        public BoneEffect(string _effectName, string _boneName, GameObject _obj,bool _isShow, float _scale = 1.0f)
        {
            isShow = _isShow;
            scale = _scale;
            effectName = _effectName;
            boneName = _boneName;
            target = _obj;
        }


        public void SetEffect(string _effectName, GameObject _actor,bool _isShow, float _scale = 1.0f)
        {
            isShow = _isShow;
            scale = _scale;
            effectName = _effectName;
            Enable(_actor, _isShow);
        }

        public void Enable(GameObject _actor, bool _isShow)
        {
            isShow = _isShow;
            if (_actor == null) return;
            isActive = true;
            if (boneObj == null) boneObj = GetBone(_actor);
            if (effectObj != null && effectObj.name != EffectName)
            {
                GameObject.DestroyImmediate(effectObj);
                effectObj = null;
            }
            if (!isDonwLoading)
            {
                if (effectObj == null)
                {
                    isDonwLoading = true;
                    AssetMng.GetEffectInstance(EffectName, (x) =>
                    {
                        isDonwLoading = false;
                        effectObj = x;
                        if (boneObj == null) boneObj = GetBone(_actor);
                        if (this == null || boneObj == null)
                        {
							if(boneObj == null)Debug.LogError("找不到指定挂点:"+boneName);
                            Destory();
                            return;
                        }
                        SetActive(isActive);
                        if (preview || boneObj.layer == LayerMask.NameToLayer("Preview"))
                        {
                            SetPreviewShader();
                        }
                    });
                }
                else
                {
                    SetActive(isActive);
                }
            }
        }


        public void Disable()
        {
            if (isActive)
            {
                isActive = false;
                SetActive(isActive);
            }
        }

        public void Destory()
        {
            if (effectObj != null)
            {
                effectObj.transform.parent = null;
                GameObject.DestroyImmediate(effectObj);
                effectObj = null;
            }
            boneObj = null;
        }



        protected void SetActive(bool _active)
        {
            isActive = _active && isShow;
            if (boneObj == null)
            {
                if (isActive)
                {
                    Debug.LogError("骨骼为空");
                }
                return;
            }
            if (effectObj == null)
            {
                if (isActive)
                {
					Debug.LogError("特效为空:"+EffectName);
                } 
                return;
            }
            if (isActive)
            {
                if (target != null)
                {
                    SetLayer(effectObj, target.layer);
                }
                effectObj.transform.localScale = new Vector3(scale, scale, scale);
                effectObj.transform.parent = boneObj.transform;
                effectObj.transform.localPosition = Vector3.zero;
                effectObj.transform.localEulerAngles = Vector3.zero;
                effectObj.SetActive(isActive);
            }
            else
            {
                effectObj.SetActive(isActive);
                effectObj.transform.parent = null;
                GameObject.Destroy(effectObj);
                effectObj = null;
            }
        }

        protected GameObject GetBone(GameObject _obj)
        {
            if (_obj == null) return null;
            if (_obj.name == boneName) return _obj;
            for (int i = 0; i < _obj.transform.childCount; i++)
            {
                GameObject bone = GetBone(_obj.transform.GetChild(i).gameObject);
                if (bone != null)
                {
                    return bone;
                }
            }
            return null;
        }

        public void SetLayer(GameObject _obj, int _layer)
        {
            if (_obj == null) return;
            _obj.layer = _layer;
            for (int i = 0; i < _obj.transform.childCount; i++)
            {
                SetLayer(_obj.transform.GetChild(i).gameObject, _layer);
            }
        }

        public void SetPreviewShader()
        {
            preview = true;
            SetPreviewShader(effectObj);
        }


        protected void SetPreviewShader(GameObject _obj)
        {
            if (_obj == null) return;
            if (_obj.GetComponent<Renderer>() != null && _obj.GetComponent<Renderer>().materials != null)
            {
                Material[] list = _obj.GetComponent<Renderer>().sharedMaterials;
                foreach (var item in list)
                {
                    if (item != null) item.shader = Shader.Find("Particles/Additive");
                }
                _obj.GetComponent<Renderer>().sharedMaterials = list;
            }
            for (int i = 0; i < _obj.transform.childCount; i++)
            {
                SetPreviewShader(_obj.transform.GetChild(i).gameObject);
            }
        }
        //Client_Preview

    }

    protected bool isShowBoneEffect = true;

    protected Dictionary<string, BoneEffect> boneEffectList = new Dictionary<string, BoneEffect>();

    /// <summary>
    /// 设置骨骼特效 by吴江
    /// </summary>
    /// <param name="_boneName"></param>
    /// <param name="_effectName"></param>
    public void SetBoneEffect(string _boneName, string _effectName,float _scale = 1.0f)
    {
        if (boneEffectList.ContainsKey(_boneName))
        {
			boneEffectList[_boneName].SetEffect(_effectName, this.gameObject, isShowBoneEffect && CanShow, _scale); 
        }
        else
        {
			boneEffectList[_boneName] = new BoneEffect(_effectName, _boneName, this.gameObject, isShowBoneEffect && CanShow, _scale);
            if (CanShow)
            {
            	boneEffectList[_boneName].Enable(this.gameObject, isShowBoneEffect);
            }
        }
    }

    public void CheckBoneEffectList()
    {
        List<string> needClear = new List<string>();
        foreach (string item in boneEffectList.Keys)
        {
            if (!boneEffectList[item].IsActive)
            {
                needClear.Add(item);
            }
        }
        for (int i = 0; i < needClear.Count; i++)
        {
            boneEffectList[needClear[i]].Destory();
            boneEffectList.Remove(needClear[i]);
        }
    }


    public void ShowBoneEffect(bool _show)
    {
		isShowBoneEffect = _show && CanShow;
        foreach (var item in boneEffectList.Values)
        {
            if (_show && CanShow)
            {
                item.Enable(this.gameObject, isShowBoneEffect);
            }
            else
            {
                item.Disable();
            }
        }
    }

    /// <summary>
    /// 设置预览着色器 by吴江
    /// </summary>
    public void SetPreviewShader()
    {
        foreach (var item in boneEffectList.Values)
        {
            item.SetPreviewShader();
        }
    }
	/// <summary>
	/// 重设置shader 仅用于预览时的特效显示 by邓成
	/// </summary>
	public void SetPreviewShader(GameObject _obj)
	{
		if (_obj == null) return;
		if (_obj.GetComponent<Renderer>() != null && _obj.GetComponent<Renderer>().materials != null)
		{
			Material[] list = _obj.GetComponent<Renderer>().sharedMaterials;
			foreach (var item in list)
			{
				if (item != null) item.shader = Shader.Find("Particles/Additive");
			}
			_obj.GetComponent<Renderer>().sharedMaterials = list;
		}
		for (int i = 0; i < _obj.transform.childCount; i++)
		{
			SetPreviewShader(_obj.transform.GetChild(i).gameObject);
		}
	}

    /// <summary>
    /// 清理骨骼特效 by吴江
    /// </summary>
    public void CleanBonesEffect()
    {
        foreach (var item in boneEffectList.Values)
        {
            item.Disable();
        }
    }


    public void ClenBoneEffect(string _boneName)
    {
        if (boneEffectList.ContainsKey(_boneName))
        {
            boneEffectList[_boneName].Disable();
        }
    }
    #endregion

    #region 升级特效 by吴江
    protected GameObject effect_Lev_last = null;

    public void DoLevelUPEffect(string _effectName)
    {
        if (!CanShow) return;
        if (effect_Lev_last == null)
        {
            AssetMng.GetEffectInstance(_effectName, (x) =>
            {
                if (x != null && this != null)
                {
                    effect_Lev_last = x;
                    x.SetMaskLayer(this.gameObject.layer);
                    x.transform.parent = this.gameObject.transform;
                    x.transform.localPosition = new Vector3(0, 0.1f, 0);
                    x.transform.localEulerAngles = Vector3.zero;
                    x.SetActive(true);
                }
            });
        }
        else
        {
            RePlay(effect_Lev_last.gameObject);
        }
    }

    public void ClearLevelUPEffect()
    {
        if (effect_Lev_last != null)
        {
            Destroy(effect_Lev_last);
            effect_Lev_last = null;
        }
    }

    #endregion

	#region 升级特效 by吴江
	protected GameObject effect_fly_last = null;

	public void DoFlyEffect(string _effectName)
	{
		if(effect_fly_last != null)
		{
			Destroy(effect_fly_last);
			effect_fly_last = null;
		}
		if (!CanShow) return;
		if (effect_fly_last == null)
		{
			AssetMng.GetEffectInstance(_effectName, (x) =>
				{
					if (x != null)
					{
                        if (this == null)
                        {
                            Destroy(x);
                            return;
                        }
						if(GameCenter.mainPlayerMng.isStartingFlyEffect)//如果传送特效没在0.5f内出来,则不需要出来  解决:传送特效紫色的bug by邓成
						{
							effect_fly_last = x;
							x.SetMaskLayer(this.gameObject.layer);
							x.transform.parent = this.gameObject.transform;
							x.transform.localPosition = new Vector3(0, 0.1f, 0);
							x.transform.localEulerAngles = Vector3.zero;
							x.SetActive(true);
                            Invoke("ClearFlyEffect",0.5f);
						}else
						{
							Destroy(x);
						}
					}
				});
		}
	}
	public void ClearFlyEffect()
	{
		if (effect_fly_last != null)
		{
			Destroy(effect_fly_last);
			effect_fly_last = null;
		}
	}
	#endregion

    #region 路点指引特效 by吴江
    //protected DesPointCtrl desPointObj = null;
    //protected AbilityShadowEffecter desPointEffect = null;
    ///// <summary>
    ///// 路点指引特效
    ///// </summary>
    ///// <param name="_effectName"></param>
    //public void DoDesPointEffect(Vector3 _des)
    //{
    //    if (!CanShow) return;
    //    if (desPointObj == null)
    //    {
    //        desPointObj = new GameObject("DesPoint").AddComponent<DesPointCtrl>(); ;
    //        desPointObj.transform.parent = this.transform;
    //        desPointObj.transform.localPosition = Vector3.zero;
    //        desPointObj.transform.localScale = Vector3.one;
    //        desPointObj.desPoint = _des;
    //        desPointEffect = GameCenter.spawner.SpawnAbilityShadowEffecter(desPointObj.transform, 1, 2, 2, AlertAreaType.DES_POINT, Color.white, float.MaxValue);
    //    }
    //    else
    //    {
    //        desPointObj.desPoint = _des;
    //    }
    //}

    ///// <summary>
    ///// 路点指引特效
    ///// </summary>
    //public void ClearDesPointEffectt()
    //{
    //    if (desPointEffect != null)
    //    {
    //        GameCenter.spawner.DespawnAbilityShadowEffecter(desPointEffect);
    //        desPointEffect = null;
    //    }
    //    if (desPointObj != null)
    //    {
    //        Destroy(desPointObj.gameObject);
    //        Destroy(desPointObj);
    //        desPointObj = null;
    //    }
    //}

    #endregion

    #region 出生特效 by吴江
    protected GameObject spawnObj = null;
    /// <summary>
    /// 展示出生特效
    /// </summary>
    /// <param name="_effectName"></param>
    public void DoSpawnEffect(string _effectName)
    {
        if (!CanShow) return;
        if (spawnObj == null)
        {
            AssetMng.GetEffectInstance(_effectName, (x) =>
            {
                if (x != null && this != null)
                {
                    spawnObj = x;
                    x.SetMaskLayer(this.gameObject.layer);
                    x.transform.parent = this.gameObject.transform;
                    x.transform.localPosition = new Vector3(0, 0.1f, 0);
                    x.transform.localEulerAngles = Vector3.zero;
                    x.SetActive(true);
                    RePlay(spawnObj.gameObject);
                }
            });
        }
        else
        {
            RePlay(spawnObj.gameObject);
        }
    }

    /// <summary>
    /// 清理出生特效
    /// </summary>
    public void ClearSpawnEffect()
    {
        if (spawnObj != null)
        {
            Destroy(spawnObj);
            spawnObj = null;
        }
    }

    #endregion

    #region 喂养特效 by吴江
    protected GameObject feedingEffect = null;

    public void DoFeedingEffect(string _effectName)
    {
        if (feedingEffect == null)
        {
            AssetMng.GetEffectInstance(_effectName, (x) =>
            {
                if (x != null && this != null)
                {
                    feedingEffect = x;
                    x.SetMaskLayer(this.gameObject.layer);
                    x.transform.parent = this.gameObject.transform;
                    x.transform.localPosition = new Vector3(0, 0.1f, 0);
                    x.transform.localEulerAngles = Vector3.zero;
                    x.SetActive(true);
                }
            });
        }
        else
        {
            RePlay(feedingEffect.gameObject);
        }
    }

    public void ClearFeedingEffect()
    {
        if (feedingEffect != null)
        {
            Destroy(feedingEffect);
            feedingEffect = null;
        }
    }
    #endregion

    #region 洗练保存特效 by 易睿
    protected GameObject saveSuccinctEffect = null;

    public void DoSaveSuccinctEffect(string _effectName)
    {
        if (saveSuccinctEffect == null)
        {
            AssetMng.GetEffectInstance(_effectName, (x) =>
            {
                if (x != null && this != null)
                {
                    saveSuccinctEffect = x;
                    x.SetMaskLayer(this.gameObject.layer);
                    x.transform.parent = this.gameObject.transform;
                    x.transform.localPosition = new Vector3(0, 0.1f, 0);
                    x.transform.localEulerAngles = Vector3.zero;
                    x.SetActive(true);
                }
            });
        }
        else
        {
            RePlay(saveSuccinctEffect.gameObject);
        }
    }
    /// <summary>
    /// 清理洗练保存特效
    /// </summary>
    public void ClearsaveSuccinctEffect()
    {
        if (saveSuccinctEffect != null)
        {
            Destroy(saveSuccinctEffect);
            saveSuccinctEffect = null;
        }
    }

    #endregion


    #region 上下马特效 by吴江
    protected GameObject rideEffect = null;

    public void DoRideEffect(string _effectName)
    {
        if (!CanShow) return;
        if (rideEffect == null)
        {
            AssetMng.GetEffectInstance(_effectName, (x) =>
            {
                if (x != null && this != null)
                {
                    rideEffect = x;
                    x.SetMaskLayer(this.gameObject.layer);
                    x.transform.parent = this.gameObject.transform;
                    x.transform.localPosition = new Vector3(0, 0.1f, 0);
                    x.transform.localEulerAngles = Vector3.zero;
                    x.SetActive(true);
                }
            });
        }
        else
        {
            RePlay(rideEffect.gameObject);
        }
    }

    public void ClearRideEffect()
    {
        if (rideEffect != null)
        {
            Destroy(rideEffect);
            rideEffect = null;
        }
    }
    #endregion

    #region 死亡特效 by吴江
    protected GameObject effect_Dead_last = null;

    public void DoDeadEffect(string _effectName)
    {
        if (!CanShow) return;
        ClearDeadEffect();
        AssetMng.GetEffectInstance(_effectName, (x) =>
        {
			if(x != null)
			{
				effect_Dead_last = x;	
				x.SetMaskLayer(this.gameObject.layer);
				x.transform.parent = this.gameObject.transform;
				x.transform.position = this.gameObject.transform.position + new Vector3(0, 0.1f, 0);
				x.transform.rotation = this.gameObject.transform.rotation;
				x.SetActive(true);
			}
        });
    }

    public void ClearDeadEffect()
    {
        if (effect_Dead_last != null)
        {
            Destroy(effect_Dead_last);
            effect_Dead_last = null;
        }
    }
    #endregion

	#region BOSS特效  by邓成
	protected GameObject effect_Boss_last = null;
    public void DoBossEffect(float _radius,string _effectName)
	{
		if (!CanShow) return;
		ClearBossEffect();
        AssetMng.GetEffectInstance(_effectName, (x) =>
			{
				if(x != null)
				{
					effect_Boss_last = x;	
					x.SetMaskLayer(this.gameObject.layer);
					x.transform.parent = this.gameObject.transform;
					x.transform.position = this.gameObject.transform.position + new Vector3(0, 0.1f, 0);
					x.transform.rotation = this.gameObject.transform.rotation;
                    x.transform.localScale = new Vector3(_radius, 1, _radius);
					x.SetActive(true);
				}
			});
	}

	public void ClearBossEffect()
	{
		if (effect_Boss_last != null)
		{
			Destroy(effect_Boss_last);
			effect_Boss_last = null;
		}
	}
	#endregion

    #region 采集特效 by邓成
    protected GameObject effect_collect_last = null;
    /// <summary>
    /// 采集特效
    /// </summary>
    public void DoCollectEffect(string _effectName)
    {
        if (!CanShow) return;
        ClearCollectEffect();
        AssetMng.GetEffectInstance(_effectName, (x) =>
        {
            if (x != null)
            {
                effect_collect_last = x;
                x.SetMaskLayer(this.gameObject.layer);
                x.transform.parent = this.gameObject.transform;
                x.transform.position = this.gameObject.transform.position + new Vector3(0, 0.1f, 0);
                x.transform.rotation = this.gameObject.transform.rotation;
                x.transform.localScale = Vector3.one;
                x.SetActive(true);
            }
        });
    }

    public void ClearCollectEffect()
    {
        if (effect_collect_last != null)
        {
            Destroy(effect_collect_last);
            effect_collect_last = null;
        }
    }
    #endregion

	#region BOSS特效  by邓成
	protected List<GameObject> effect_Sedan_last = new List<GameObject>();
	public void DoSedanEffect()
	{
		if (!CanShow) return;
		ClearSedanEffect();
		for (int i = 1,max=5; i < max; i++) 
		{
			string pointName = "xl_hc_point"+i;
			AssetMng.GetEffectInstance("fx_hc_001", (x) =>
				{
					if(x != null)
					{
						Transform point = MeshHelper.GetBone(this.transform, pointName);//this.transform.Find(pointName);// GameObject.Find();
						effect_Sedan_last.Add(x);	
						x.SetMaskLayer(this.gameObject.layer);
						x.transform.parent = (point == null)?this.gameObject.transform:point;
						x.transform.localPosition = Vector3.zero;
						x.transform.localScale = Vector3.one;
						x.SetActive(true);
					}
				});
		}
	}

	public void ClearSedanEffect()
	{
		if (effect_Boss_last != null)
		{
			for (int i = 0,max=effect_Sedan_last.Count; i < max; i++) 
			{
				Destroy(effect_Sedan_last[i]);
				effect_Sedan_last.RemoveAt(i);
			}
		}
	}
	#endregion

	#region 强化套装特效  by邓成
	protected GameObject effect_strength_last = null;
	public void DoStrengthEffect(string _effectName)
	{
		if (!CanShow) return;
		ClearStrengthEffect();
		AssetMng.GetEffectInstance(_effectName, (x) =>
			{
				if(x != null)
				{
					effect_strength_last = x;	
					x.SetMaskLayer(this.gameObject.layer);
					x.transform.parent = this.gameObject.transform;
					x.transform.localPosition = new Vector3(0, -0.06f, 0);
					x.transform.rotation = this.gameObject.transform.rotation;
					x.SetActive(true);
					if(this.gameObject.layer == LayerMask.NameToLayer("Preview"))
						SetPreviewShader(x);
				}
			});
	}

	public void ClearStrengthEffect()
	{
		if (effect_strength_last != null)
		{
			Destroy(effect_strength_last);
			effect_strength_last = null;
		}
	}
	#endregion

    #region 脚下光圈 by吴江
    protected RingEffecter ring = null;
    protected float ringDiff = 0.1f;
    protected float ringScale = 1f;

    /// <summary>
    /// 启动/关闭对象脚下光圈特效 by吴江
    /// </summary>
    /// <param name="_color"></param>
    /// <param name="_active"></param>
    public void DoRingEffect(Color _color, bool _active, float _radius)
    {
        if (_active)
        {
            if (ring != null)
            {
                GameCenter.spawner.DespawnRingEffecter(ring);
                ring = null;
            }
            ring = GameCenter.spawner.SpawnRingEffecter(new Vector3(0, ringDiff, 0), Quaternion.identity, this.gameObject.transform, _color);
			if(ring != null)
			{
	            ring.gameObject.SetMaskLayer(this.gameObject.layer);
	            ring.gameObject.transform.localScale = new Vector3(_radius * ringScale, 1, _radius * ringScale);
			}
        }
        else
        {
            if (ring != null)
            {
                ring.gameObject.transform.localScale = new Vector3(_radius * ringScale, 1, _radius * ringScale);
                GameCenter.spawner.DespawnRingEffecter(ring);
                ring = null;
            }
        }
    }
    public void UpdateRingSize(MonsterInfo info, float _scale)
    {
        ringScale = _scale;//改变光圈比例
        if (ring != null)
            ring.gameObject.transform.localScale = new Vector3(info.RingSize * ringScale, 1, info.RingSize * ringScale);//若此时光圈显示中,则改变大小
    }
    #endregion

    #region 阴影投射 by吴江
    public bool canShowShadow = true;
    protected GameObject shadowObj = null;
    protected bool shadowIsLoading = false;
    protected bool shadowActive = false;

    /// <summary>
    /// 开启/关闭 阴影投射器 by 吴江
    /// </summary>
    /// <param name="_active"></param>
    public void DoShadowEffect(bool _active)
    {
        if (!canShowShadow)
        {
            _active = false;
        }
        if (SystemSettingMng.RealTimeShadow)
        {
            _active = false;
        }
        shadowActive = _active;
        if (shadowObj == null)
        {
            if (_active && !shadowIsLoading)
            {
                shadowIsLoading = true;
                exResources.GetShadow("shadow", (x, y) =>
                    {
                        shadowIsLoading = false;
                        if (y == EResult.Success)
                        {
                            if (this == null || this.transform == null) Destroy(x);
                            shadowObj = Instantiate(x) as GameObject;
                            shadowObj.transform.parent = this.transform;
                            shadowObj.transform.localPosition = new Vector3(0, 0.1f, 0);
                            shadowObj.transform.localEulerAngles = Vector3.zero;
                            shadowObj.transform.localScale = Vector3.one;
                            shadowObj.gameObject.SetMaskLayer(this.gameObject.layer);
                            shadowObj.SetActive(shadowActive);
                            InteractiveObject.RefreshShader(shadowObj);
                        }
                    });
            }
        }
        else
        {
            shadowObj.SetActive(_active);
        }
    }

    #endregion


    #region 播放攻击特效 by吴江
    protected GameObject curAttackEffect = null;
    public bool IsCastingAttackEffect
    {
        get
        {
            return curAttackEffect != null || waitEffect.Count > 0;
        }
    }

    protected List<string> waitEffect = new List<string>();

    /// <summary>
    /// 播放攻击特效 by吴江
    /// </summary>
    /// <param name="_effectName"></param>
    /// <param name="_time"></param>
    public void DoAttackEffect(string _effectName, float _time,float _speedRate, Vector3 _scale,Transform _parent)
    {
        if (isHide) return;
        if (!CanShow) return;
        if (_time == 0) return;
        waitEffect.Add(_effectName);
        if (curAttackEffect != null)
        {
            DestroyImmediate(curAttackEffect);
        }
        AssetMng.GetEffectInstance(_effectName, (x) =>
            {
                if (x == null || this == null || this.gameObject == null || !waitEffect.Contains(_effectName))
                {
                    Destroy(x);
                    return;
                }
                waitEffect.Remove(_effectName);
                curAttackEffect = x;
                AdjustSpeedRuntime(x, _speedRate);
                x.SetMaskLayer(this.gameObject.layer);
                if (_parent == null)
                {
                    x.transform.parent = this.transform;
                }
                else
                {
                    x.transform.parent = _parent;
                }
                x.transform.localPosition = Vector3.zero;
                x.transform.localEulerAngles = Vector3.zero;
                x.transform.localScale = _scale;
                if (_time >= 0)
                {
                    Destroy(x, _time);
                }
            });
    }



    public void CancelAttackEffect()
    {
        waitEffect.Clear();
        if (curAttackEffect != null)
        {
            Destroy(curAttackEffect);
        }
    }
    #endregion

    #region 播放被击特效 by吴江
    protected Transform hitPoint = null;
    /// <summary>
    /// 播放被击特效 by吴江
    /// </summary>
    /// <param name="_effectName"></param>
    /// <param name="_time"></param>
    public void DoDefEffect(string _effectName, float _time, Vector3 _scale, Quaternion _rotation)
    {
        if (!CanShow) return;
        if (_effectName == string.Empty || _time <= 0) return;
        GameCenter.spawner.SpawnEffecter(_effectName, _time, (x) =>
            {
                if (x == null) return;
                if (hitPoint == null)
                {
                    hitPoint = MeshHelper.GetBone(this.transform, "hitPoint");
                    if (hitPoint == null)
                    {
                        hitPoint = this.transform;
                    }
                }
                x.gameObject.SetMaskLayer(this.gameObject.layer);
                x.transform.parent = null;
                x.transform.rotation = _rotation;
                x.transform.parent = hitPoint;
                x.transform.localPosition = Vector3.zero;
                x.transform.localScale = _scale;
            },true);
    }

    /// <summary>
    /// 播放被击特效 by吴江
    /// </summary>
    /// <param name="_effectName"></param>
    /// <param name="_time"></param>
    public void DoDefEffectFixedPosition(string _effectName, float _time, Vector3 _scale, Quaternion _rotation)
    {
        if (!CanShow) return;
        if (_effectName == string.Empty || _time <= 0) return;
        GameCenter.spawner.SpawnEffecter(_effectName, _time, (x) =>
        {
            if (x == null) return;
            x.gameObject.SetMaskLayer(this.gameObject.layer);
            if (hitPoint == null)
            {
                hitPoint = MeshHelper.GetBone(this.transform, "hitPoint");
                if (hitPoint == null)
                {
                    hitPoint = this.transform;
                }
            }
            x.transform.parent = null;
            x.transform.position = hitPoint.transform.position;
            x.transform.rotation = _rotation;
            x.transform.localScale = _scale;
        },true);
    }

	/// <summary>
	/// 播放被击特效 by邓成
	/// </summary>
	/// <param name="_effectName"></param>
	/// <param name="_time"></param>
	public void DoDefEffectFixedPosition(string _effectName, float _time, Vector3 _scale, Quaternion _rotation,Transform _hitPoint)
	{
		if (!CanShow) return;
		if (_effectName == string.Empty || _time <= 0) return;
		GameCenter.spawner.SpawnEffecter(_effectName, _time, (x) =>
			{
				if (x == null) return;
				x.gameObject.SetMaskLayer(this.gameObject.layer);

				x.transform.parent = null;
				x.transform.rotation = _rotation;
				x.transform.localScale = _scale;

				x.transform.parent = _hitPoint;
				x.transform.position = _hitPoint.position;
			},true);
	}
	#endregion

    #region 播放拾取特效 by吴江
    protected float holdTime = 1.0f;
    /// <summary>
    /// 播放拾取特效 by吴江
    /// </summary>
    /// <param name="_effectName"></param>
    /// <param name="_time"></param>
    public void DoPickUpEffect(string _effectName, float _time, Vector3 _from, SmartActor _to)
    {
        if (!CanShow) return;
        if (_effectName == string.Empty || _time <= 0) return;
        GameCenter.spawner.SpawnEffecter(_effectName, _time + holdTime, (x) =>
        {
            if (x == null) return;
            x.gameObject.SetMaskLayer(this.gameObject.layer);
            x.transform.parent = null;
            x.transform.rotation = Quaternion.identity;// Quaternion.Euler(_from - transform.position);
            x.transform.parent = null;
            x.transform.localScale = Vector3.one;
            PickUpEffect p = x.gameObject.GetComponent<PickUpEffect>();
            if (p == null) p = x.gameObject.AddComponent<PickUpEffect>();
            p.Init(_from, _to, _time,holdTime, x);
        },false);
    }
    #endregion


    #region 技能全程特效 by吴江
    protected GameObject attckWholeTimeEffect = null;

    public void DoAttckWholeTimeEffect(string _effectName,Transform _animaRoot)
    {
        if (isHide) return;
        if (!CanShow || _effectName == string.Empty) return;
        ClearAttckWholeTimeEffect();
        AssetMng.GetEffectInstance(_effectName, (x) =>
        {
            if (this != null && this.gameObject != null && x != null)
            {
                attckWholeTimeEffect = x;
                x.SetMaskLayer(this.gameObject.layer);
                x.transform.parent = _animaRoot;
                x.transform.localPosition = new Vector3(0, 0.1f, 0);
                x.transform.rotation = Quaternion.identity;
            }
        }, true);
    }

    public void ClearAttckWholeTimeEffect()
    {
        if (attckWholeTimeEffect != null)
        {
            Destroy(attckWholeTimeEffect);
            attckWholeTimeEffect = null;
        }
    }
    #endregion

    #region 常驻特效 by吴江
    protected GameObject normalEffect = null;

    public void DoNormalEffect(string _effectName)
    {
        if (!CanShow || _effectName == string.Empty) return;
        ClearNormalEffect();
        AssetMng.GetEffectInstance(_effectName, (x) =>
        {
            if (this != null && this.gameObject != null && x != null)
            { 
                normalEffect = x;
                x.SetMaskLayer(this.gameObject.layer);
                x.transform.parent = this.gameObject.transform;
                x.transform.position = this.gameObject.transform.position + new Vector3(0, 0.1f, 0);
                x.transform.rotation = this.gameObject.transform.rotation;
            }
        },true);
    }

    public void DoNormalEffect(string _effectName,System.Action _callBack)
    {
        if (!CanShow || _effectName == string.Empty) return;
        ClearNormalEffect();
        AssetMng.GetEffectInstance(_effectName, (x) =>
        {
            normalEffect = x;
            x.SetMaskLayer(this.gameObject.layer);
            x.transform.parent = this.gameObject.transform;
            x.transform.position = this.gameObject.transform.position + new Vector3(0, 0.1f, 0);
            x.transform.rotation = this.gameObject.transform.rotation;
            if (_callBack != null) _callBack();
        }, true);
    }

	public void DoNormalEffect(string _effectName,System.Action<GameObject> _callBack)
	{
		if (!CanShow || _effectName == string.Empty) return;
		ClearNormalEffect();
		AssetMng.GetEffectInstance(_effectName, (x) =>
			{
				normalEffect = x;
				x.SetMaskLayer(this.gameObject.layer);
				x.transform.parent = this.gameObject.transform;
				x.transform.position = this.gameObject.transform.position + new Vector3(0, 0.1f, 0);
				x.transform.rotation = this.gameObject.transform.rotation;
				if (_callBack != null) _callBack(x);
			}, true);
	}

    public void ClearNormalEffect()
    {
        if (normalEffect != null)
        {
            Destroy(normalEffect);
            normalEffect = null;
        }
    }
    #endregion

    #region buff特效 by吴江
    protected Dictionary<string, GameObject> buffDic = new Dictionary<string, GameObject>();
    protected Dictionary<string, bool> waitList = new Dictionary<string, bool>();

    public void DoBuffEffect(string _assetName,Transform _parent)
    {
        if (!CanShow) return;
        if (_assetName.Length == 0 || _assetName == "0") return;
        if (buffDic.ContainsKey(_assetName) && buffDic[_assetName] != null)
        {
            buffDic[_assetName].SetActive(true);
            RePlay(buffDic[_assetName]);
        }
        else
        {
            if (!waitList.ContainsKey(_assetName))
            {
                waitList[_assetName] = true;
                AssetMng.GetEffectInstance(_assetName, (x) =>
                {
                    if (x == null)
                    {
                        return;
                    }
                    if (this.gameObject == null)
                    {
                        Destroy(x);
                        return;
                    }
                    buffDic[_assetName] = x;
                    x.SetMaskLayer(this.gameObject.layer);
                    if (_parent == null)
                    {
                        x.transform.parent = this.transform;
                    }
                    else
                    {
                        x.transform.parent = _parent;
                    }
                    x.transform.localScale = Vector3.one;
                    x.transform.localEulerAngles = Vector3.zero;
                    x.transform.localPosition = Vector3.zero;
                    x.SetActive(waitList[_assetName]);
                    FXRoot fr = x.GetComponent<FXRoot>();
                    if (fr != null)
                    {
                        fr.isWorldMatrix = true;
                    }
                    waitList.Remove(_assetName);
                }, true);
            }
            else if (!waitList[_assetName])
            {
                waitList[_assetName] = true;
            }
        }
    }

    public void HideBuffEffect(string _assetName)
    {
        if (waitList.ContainsKey(_assetName))
        {
            waitList[_assetName] = false;
        }
        if (buffDic.ContainsKey(_assetName) && buffDic[_assetName] != null)
        {
            buffDic[_assetName].SetActive(false);
        }
    }

    public void DeleteBuffEffect(string _assetName)
    {
        if (waitList.ContainsKey(_assetName))
        {
            waitList[_assetName] = false;
        }
        if (buffDic.ContainsKey(_assetName) && buffDic[_assetName] != null)
        {
            Destroy(buffDic[_assetName]);
            buffDic[_assetName] = null;
            buffDic.Remove(_assetName);
        }
    }

    public void CleanBuffEffects()
    {
        foreach (var item in buffDic.Values)
        {
            Destroy(item);
        }
    }
    #endregion

    #region 闪电链 by吴江
    public void CastLightingShake(AbilityInstance _instance)
    {
        if (_instance == null || _instance.CurClientShowType != ClientShowType.Lightingskill) return;
        List<Transform> list = _instance.TargetTransforms;
        string effectName = _instance.ProcessEffectList.Count > 0 ? _instance.ProcessEffectList[0] : "e_c_lightning_emitter";
        for (int i = 0; i < list.Count; i++)
        {
            System.Action preLoad = Utils.Functor<int>(i, (z) =>
            {
                GameCenter.spawner.SpawnEffecter(effectName, 1.0f, (x) =>
                {
                    LightningEmitter le = x.GetComponent<LightningEmitter>();
                    if (le != null)
                    {
                        if (z <= 0)
                        {
                            le.Target1 = _instance.UserActor.HitPoint;
                        }
                        else
                        {
                            le.Target1 = list[z - 1];
                        }
                        le.Target2 = list[z];
                        le.animSpeed = 3.0f;
                    }
                    else
                    {
                        Debug.LogError("闪电链特效组件缺失,播放失败");
                    }
                }, false);
            });
            preLoad();

           
        }
        
    }
    #endregion

    #region 链式特效 by吴江
    protected FDictionary lineEffectDic = new FDictionary();
    protected GameObject lineEffect = null;
    /// <summary>
    /// 创建链式特效(唯一) by吴江
    /// </summary>
    /// <param name="_tarA"></param>
    /// <param name="_tarB"></param>
    /// <param name="_effectName"></param>
    public void CastLineEffect(Actor _tarA, Actor _tarB, string _effectName,float _speed = 1.0f)
    {
        if (lineEffectDic.ContainsKey(_effectName))
        {
            Destroy(lineEffectDic[_effectName] as GameObject);
            lineEffectDic.Remove(_effectName);
        }
        if (_tarA == null || _tarB == null) return;
        AssetMng.GetEffectInstance(_effectName, (x) =>
            {
                lineEffectDic[_effectName] = x;
                LightningEmitter le = x.GetComponent<LightningEmitter>();
                if (le != null)
                {
                    le.Target1 = _tarA.HitPoint;
                    le.Target2 = _tarB.HitPoint;
                    le.animSpeed = _speed;
                }
            });


    }
    /// <summary>
    /// 删除链式特效 by吴江
    /// </summary>
    public void DeleteLineEffect()
    {
        foreach (GameObject item in lineEffectDic.Values)
        {
            Destroy(item);
        }
        lineEffectDic.Clear();
    }

    /// <summary>
    /// 删除链式特效 by吴江
    /// </summary>
    public void DeleteLineEffect(string _effectName)
    {
        if (lineEffectDic.ContainsKey(_effectName))
        {
            Destroy(lineEffectDic[_effectName] as GameObject);
            lineEffectDic.Remove(_effectName);
        }
    }
    #endregion


    #region 调整速度和重新播放
    public static void AdjustSpeedRuntime(GameObject _target, float _fSpeedRate)	
    {
        NsEffectManager.AdjustSpeedRuntime(_target, _fSpeedRate);
        FXRoot fr = _target.GetComponent<FXRoot>();
        if (fr != null)
        {
            fr.Speed =  _fSpeedRate;
        }
    }


    public static void RePlayMatchingSpeed(GameObject _target,float _duraiton, float _delayTime = 0)
    {
        if (_duraiton == 0) return;
        NsEffectManager.RunReplayEffect(_target, true);
        FXRoot fr = _target.GetComponent<FXRoot>();
        if (fr != null)
        {
            if (_delayTime > 0 || _duraiton != fr.length)
            {
                fr.Play(_delayTime, fr.length / _duraiton);
            }
            else
            {
                fr.Play();
            }
        }
        else
        {
            Debug.Log(_target.name + "找不到FXRoot组件!");
        }
    }

    public static void RePlay(GameObject _target, float _delayTime = 0)
    {
        NsEffectManager.RunReplayEffect(_target, true);
        FXRoot fr = _target.GetComponent<FXRoot>();
        if (fr != null)
        {
            if (_delayTime > 0)
            {
                fr.Play(_delayTime, 1.0f);
            }
            else
            {
                fr.Play();
            }
        }
        else
        {
            Debug.Log(_target.name + "找不到FXRoot组件!");
        }
    }


    public static void Stop(GameObject _target)
    {
        NsEffectManager.SetReplayEffect(_target);
        FXRoot fr = _target.GetComponent<FXRoot>();
        if (fr != null)
        {
            fr.Stop();
        }
        else
        {
            Debug.Log(_target.name + "找不到FXRoot组件!");
        }

    }

    #endregion

    void OnDisable()
    {
        if (lineEffect != null)
        {
            Destroy(lineEffect);
        }
    }
}
