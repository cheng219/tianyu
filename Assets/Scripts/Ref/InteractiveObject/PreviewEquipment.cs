//===============================================
//作者：吴江
//日期：2015/11/10
//用途：用作预览的装备对象
//===============================================




using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 用作预览的装备对象 by吴江
/// </summary>
public class PreviewEquipment : Actor
{
    /// <summary>
    /// 是否为互斥对象 by吴江
    /// </summary>
    public bool mutualExclusion = true;
    /// <summary>
    /// 完整数据层对象的引用 by吴江
    /// </summary>
    protected new EquipmentInfo actorInfo = null;

    protected Transform eqObjTransform;
    protected Vector3 originPos;

    public Color ItemColor
    {
        get
        {
            return actorInfo.ItemColor;
        }
    }


    /// <summary>
    /// 预览时的坐标
    /// </summary>
    public Vector3 PreviewPosition
    {
        get
        {
            return actorInfo.PreviewPosition;
        }
    }

    /// <summary>
    /// 预览时的角度
    /// </summary>
    public Vector3 PreviewRotation
    {
        get
        {
            return actorInfo.PreviewRotation;
        }
    }


    /// <summary>
    /// 创建净数据对象 by吴江
    /// </summary>
    /// <param name="_info"></param>
    /// <returns></returns>
    public static PreviewEquipment CreateDummy(EquipmentInfo _info)
    {
        if (_info == null) return null;
        GameObject newGO = new GameObject("Dummy Equipment[" + _info.InstanceID + "]");

        newGO.tag = "Player";
        PreviewEquipment item = newGO.AddComponent<PreviewEquipment>();
        item.isDummy_ = true;
        item.actorInfo = _info;

        return item;
    }





    public void StartAsyncCreate(System.Action<PreviewEquipment> _callback)
    {
        StartCoroutine(CreateAsync(_callback));
    }

    IEnumerator CreateAsync(System.Action<PreviewEquipment> _callback)
    {
        if (isDummy_ == false)
        {
            GameSys.LogError("You can only start create Equipment in dummy: " + actorInfo.InstanceID);
            yield break;
        }

        //
        PreviewEquipment item = null;
        MountRendererCtrl myRendererCtrl = null;
        bool failed = false;
        pendingDownload = Create(actorInfo, delegate(PreviewEquipment _eq, EResult _result)
        {
            if (_result != EResult.Success)
            {
                failed = true;
                return;
            }
            item = _eq;
            pendingDownload = null;
            myRendererCtrl = item.gameObject.GetComponentInChildrenFast<MountRendererCtrl>();
            if (myRendererCtrl != null)
            {
                myRendererCtrl.Show(true, true);
            }

        });
        if (mutualExclusion)
        {
            GameCenter.previewManager.PushDownLoadTask(pendingDownload);
        }
        while (item == null || item.inited == false)
        {
            if (failed) yield break;
            yield return null;
        }
        if (mutualExclusion)
        {
            GameCenter.previewManager.EndDownLoadTask(pendingDownload);
        }
        pendingDownload = null;

        item.isDownloading_ = false;
        if (_callback != null)
        {
            _callback(item);
        }
    }


    protected AssetMng.DownloadID Create(EquipmentInfo _info, System.Action<PreviewEquipment, EResult> _callback)
    {
        if (_info == null) return null;
        return AssetMng.instance.LoadAsset<GameObject>(_info.ShortUrl,
                                                      "",
                                                      delegate(GameObject _asset, EResult _result)
                                                      {
                                                          if (_result != EResult.Success)
                                                          {
                                                              _callback(null, _result);
                                                              return;
                                                          }
                                                          this.gameObject.name = "Preview Equipment[" + _info.InstanceID + "]";
                                                          GameObject newGO = Instantiate(_asset) as GameObject;
                                                          newGO.name = _asset.name;
                                                          eqObjTransform = newGO.transform;
                                                          newGO.transform.parent = this.gameObject.transform;
                                                          //newGO.transform.localEulerAngles = PreviewRotation;
                                                          RefreshShader(newGO);
                                                          float scale = _info.PreviewScale;
                                                          newGO.transform.localScale = new Vector3(scale, scale, scale);
                                                          //AssetMng.GetEffectInstance("ShowEffect_A01", (x) =>
                                                          //    {
                                                          //        if (newGO == null)
                                                          //        {
                                                          //            GameObject.DestroyImmediate(x);
                                                          //            return;
                                                          //        }
                                                          //        x.transform.parent = this.gameObject.transform;
                                                          //        x.transform.localPosition = new Vector3(0, -1.41f, 0);
                                                          //        x.transform.localEulerAngles = Vector3.zero;
                                                          //        x.SetMaskLayer(LayerMask.NameToLayer("Preview"));
                                                          //        x.SetColor(_info.ItemColor);
                                                          //    });
                                                          //Renderer rd = newGO.GetComponent<SkinnedMeshRenderer>();
                                                          //if (rd != null)
                                                          //{
                                                          //    Vector3 ct = rd.bounds.center;
                                                          //    newGO.transform.localPosition = new Vector3(-ct.x, -ct.y + minValue, -ct.z);
                                                          //}
                                                          //else
                                                          //{
                                                          //    newGO.transform.localPosition = new Vector3(0, minValue, 0);
                                                          //}
                                                          minValue = actorInfo.PreviewPosition.y;
                                                          newGO.transform.localPosition = Vector3.zero;//actorInfo.PreviewPosition;
                                                          originPos = newGO.transform.localPosition;
                                                          isDummy_ = false;

                                                          Init();
                                                          _callback(this, _result);
                                                      },
                                                      true
                                                    );
    }


    protected override void Init()
    {
        this.gameObject.SetMaskLayer(LayerMask.NameToLayer("Preview"));

        Regist();
        inited_ = true;
        if (actorInfo.BoneEffectList.Count > 0)
        {
            FXCtrl fx = this.gameObject.AddComponent<FXCtrl>();
            foreach (var item in actorInfo.BoneEffectList)
            {
                fx.SetBoneEffect(item.boneName, item.effectName);
            }
        }
    }


    void InitAnimation()
    {
    }

    protected float curCountTime;
    protected float curStartTime;
    public bool curOutside = false;
    public float outsideTime;
    protected float maxValue = 0.2f;
    protected float minValue = 0.0f;
    protected float autoDiff = 0.005f;
    protected float refVol = 0.0f;
    public void OnOutSide(bool _outSide)
    {
        curOutside = _outSide;
        curStartTime = Time.time;
        float y = Mathf.SmoothDamp(eqObjTransform.localPosition.y, originPos.y, ref refVol, 0.15f);
        eqObjTransform.localPosition = new Vector3(0, y, 0);
    }

    //new void Update()
    //{
    //    if (eqObjTransform != null)
    //    {
    //        if (curOutside)
    //        {
    //            curCountTime = Time.time - curStartTime;
    //            float y = Mathf.SmoothDamp(eqObjTransform.localPosition.y, maxValue - (maxValue - originPos.y) * Mathf.Abs(outsideTime / 2.0f - curCountTime) / outsideTime,
    //                ref refVol, 0.15f);

    //            eqObjTransform.localPosition = eqObjTransform.localPosition.SetY(y);
    //        }
    //        else
    //        {
    //            if (eqObjTransform.localPosition.y - originPos.y > 0.15f)
    //            {
    //                if (autoDiff > 0) autoDiff = -autoDiff;
    //            }
    //            else if (eqObjTransform.localPosition.y - originPos.y < -0.15f)
    //            {
    //                if (autoDiff < 0) autoDiff = -autoDiff;
    //            }
    //            eqObjTransform.localPosition = eqObjTransform.localPosition.SetY(eqObjTransform.localPosition.y + autoDiff);
    //        }

    //    }
    //}



    protected new void OnDestroy()
    {
        base.OnDestroy();
        UnRegist();
    }

}
