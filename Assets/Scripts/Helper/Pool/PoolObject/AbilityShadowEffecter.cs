///////////////////////////////////////////////////////////////////////////////
//作者：吴江
//日期：2015/10/13
//用途：技能预警投射器对象池对象
///////////////////////////////////////////////////////////////////////////////


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 阴影投射器对象池对象 by 吴江
/// </summary>
public class AbilityShadowEffecter : PoolObject
{
    protected Color aimColor = Color.white;
    protected Color fromColor = Color.white;
    protected float shakeScale = 0.5f;
    protected float startShake = 0.2f;
    protected float endShake = 0.5f;
    protected Material mat = null;
    protected float startTime = 0;



    public void OnSpawned(AlertAreaType _type, Color _color, float _rotY, float _wid, float _length, float _duration)
    {
        base.OnSpawned();
        this.gameObject.transform.eulerAngles = new Vector3(90, _rotY, 0);
        Projector pro = this.gameObject.GetComponent<Projector>();
        pro.aspectRatio = _wid / _length;
        pro.orthographicSize = Mathf.Max(_length, _wid);
        mat = pro.material;
        if (mat != null)
        {
            mat.mainTexture = GameStageUtility.GetTextureByType(_type);
            fromColor = new Color(_color.r, _color.g, _color.b, 0);
            mat.color = fromColor;
            aimColor = _color;
            startTime = Time.time;
            shakeScale = Mathf.Abs(startShake);
        }
        Invoke("StartReturn", _duration);

    }


    void Update()
    {
        if (mat != null)
        {
            if (mat.color != aimColor)
            {
                float rate = Mathf.Min(shakeScale, (Time.time - startTime));
              //  mat.color = fromColor;
                mat.color = Color.Lerp(fromColor, aimColor, rate / shakeScale);
            }
        }
    }

    public void StartReturn()
    {
        shakeScale = Mathf.Abs(endShake);
        startTime = Time.time;
        fromColor = aimColor;
        aimColor = new Color(fromColor.a, fromColor.g, fromColor.b, 0);
        Invoke("ReturnBySelf", shakeScale);
    }

    /// <summary>
    /// 对象自主归还对象池
    /// </summary>
    public void ReturnBySelf()
    {
        if (GameCenter.spawner != null)
        {
            GameCenter.spawner.DespawnAbilityShadowEffecter(this);
        }
    }


}
