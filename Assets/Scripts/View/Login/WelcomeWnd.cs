//======================================================
//作者:朱素云
//日期:2016/9/8
//用途:欢迎界面
//======================================================
using UnityEngine;
using System.Collections;

public class WelcomeWnd : GUIBase
{
    public UIButton readyToPlay;//开始修仙

    public Vector3 cameraPos = Vector3.zero;
    public Vector3 cameraRot = Vector3.zero;
    //public float cameraFocusDistance = 0f;
    public float cameraMoveTime = 5f;

    void Awake()
    {
        mutualExclusion = true;
        if (readyToPlay != null) UIEventListener.Get(readyToPlay.gameObject).onClick = OnClickReadyToPlay;
    }
    void OnClickReadyToPlay(GameObject go)
    { 
        GameCenter.uIMng.SwitchToUI(GUIType.NONE);
        GameCenter.noviceGuideMng.OpenGuide(100024, 1);
    }
    protected override void OnOpen()
    {
        base.OnOpen();
        if (!cameraPos.Equals(Vector3.zero) && !cameraRot.Equals(Vector3.zero))
        {
            GameCenter.cameraMng.ActiveUICamera(false);
            if (GameCenter.curMainPlayer != null) GameCenter.curMainPlayer.inputListener.AddLockType(PlayerInputListener.LockType.SCENE_ANIM_PROCESS);
            GameCenter.cameraMng.FocusOn(cameraPos, cameraRot, 0.1f);
            Invoke("FocusOnMainPlayer", 0.1f);
        }
    }
    void FocusOnMainPlayer()
    {
        GameCenter.cameraMng.FocusOn(GameCenter.curMainPlayer, cameraMoveTime);
        Invoke("ActiveUICamera", cameraMoveTime);
    }
    void ActiveUICamera()
    {
        GameCenter.cameraMng.ActiveUICamera(true);
        if (GameCenter.curMainPlayer != null) GameCenter.curMainPlayer.inputListener.RemoveLockType(PlayerInputListener.LockType.SCENE_ANIM_PROCESS);
    }
}
