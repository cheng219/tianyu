//======================================
//作者:吴江
//日期:2016/2/3
//用途:创角选角场景控件
//======================================

using UnityEngine;
using System.Collections;

public class LoginSceneCtrl : MonoBehaviour {

    /// <summary>
    /// 当前的角色
    /// </summary>
    [System.NonSerialized]
    public CreatePlayer curCreatePlayer = null;

    public Transform positionTransform;

	public DragRotation dragRotation;

    public GameObject camera;

    protected Vector3 originalPosition = Vector3.zero;
    void Awake()
    {
        if (camera != null) originalPosition = camera.transform.localPosition;
    }
    public void SetCurPlayer(PlayerBaseInfo _info)
    {
        if (originalPosition != Vector3.zero && camera != null)
        {
            camera.transform.localPosition = originalPosition;
        }
        
        if (curCreatePlayer != null)
        {
            DestroyImmediate(curCreatePlayer.gameObject);
            curCreatePlayer = null;
        }
        if (_info == null) return;
        if (dragRotation != null)
        {
            dragRotation.SetObjRotY(180);
            dragRotation.SetEnable(false);//震动中不让拖动旋转,否则相机位置会偏差很大
        }
        curCreatePlayer = CreatePlayer.CreateDummy(_info);
        curCreatePlayer.transform.position = positionTransform.position;
        curCreatePlayer.StartAsyncCreate(() =>
            {
                
				AbilityInstance ability = new AbilityInstance(_info.CreateAbilityID,1,curCreatePlayer,null);
                curCreatePlayer.UseAbility(ability, () =>
                    {
                        ShakeCamera(ability.ProcessShakeV3, (ability.ProcessShakePowerList.Count > 0 ? ability.ProcessShakePowerList[0] : 0.2f));
                    }, FinishAbility);
            });
    }

    protected void ShakeCamera(Vector3 _shakeAmount, float _shakeTime)
    {
        iTween.ShakePosition(camera, _shakeAmount, _shakeTime);
    }

    protected void FinishAbility()
    {
        if (originalPosition != Vector3.zero && camera != null)
        {
            camera.transform.localPosition = originalPosition;
        }
        if (dragRotation != null) dragRotation.SetEnable(true);
    }

}
