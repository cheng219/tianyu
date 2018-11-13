///////////////////////////////////////////////////////////////////////////////
//作者：吴江
//日期：2015/5/15
//用途：渲染控制器基类
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class RendererCtrl : MonoBehaviour {

    public class BlinkParams {
        public BlinkParams ( Shader _shader, Color _color, float _duration ) {
            shader = _shader;
            color = _color;
            duration = _duration;
        }

        public Shader shader = null;
        public Color color = Color.white;
        public float duration = 1.0f;
    }

    protected bool isShow_ = false;
    public bool isShow { get { return isShow_; } }

    protected Material[] originalMaterials = null;
    protected int originalLayer = -1;


    protected void Awake () {
    }


    public virtual void Init () {
    }


    public virtual bool Show ( bool _show, bool _force = false ) {
        if ( _force || isShow_ != _show ) {
            isShow_ = _show;
            return true;
        }
        return false;
    }


    public virtual void SetLayer ( int _layer ) { }


    public virtual void ResetLayer () { }


    public virtual void SetSkinnedMeshShader ( Shader _shader ) { }

    public virtual void SetWeaponShader(Shader _shader, Color _color, string _propertyName = "") { }

    public virtual void SetMaterial ( Material _material ) { }


    public virtual void ResetMaterials () { }




    public virtual void SetOutlineParameters ( Color _color, float _width ) {
    }


    public virtual void SetBlinkColor ( Color _color ) {
    }


    public void Blink ( Shader _shader, Color _color, float _duration ) {
        StopCoroutine("Blink_CO");
        BlinkParams blinkParams = new BlinkParams( _shader, _color, _duration );
        StartCoroutine("Blink_CO", blinkParams);
    }


    public void StopBlink () {
        StopCoroutine("Blink_CO");
        ResetMaterials();
    }

    protected virtual IEnumerator Blink_CO ( BlinkParams _params ) {
        SetSkinnedMeshShader ( _params.shader );

        float timer = 0.0f;
        while ( timer <= _params.duration ) {
            float ratio = timer / _params.duration;
            ratio = exEase.ExpoOut(ratio);
            ratio = Mathf.PingPong(ratio * 2.0f, 1.0f);
            Color clr = Color.Lerp(Color.black, _params.color, ratio);

            SetBlinkColor ( clr );

            //
            timer += Time.deltaTime;
            yield return null;
        }
        
        ResetMaterials();
    }
}
