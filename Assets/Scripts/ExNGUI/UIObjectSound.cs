//===============================
//作者:吴江
//日期:2015/3/3
//用途:UI控件的声音播放组件
//================================



using UnityEngine;
using System.Collections;

/// <summary>
/// UI控件的声音播放组件
/// </summary>
public class UIObjectSound : MonoBehaviour {


    public enum PlayType
    {
        None,
        OnStart,
        OnEnable,
        OnDisble,
        OnLoop,
    }

    public AudioClip audioClip;
    public string audioName;
    public PlayType playType = PlayType.OnEnable;
    public float volume = 1f;
    public float pitch = 1f;
    /// <summary>
    /// 声音长度 by吴江
    /// </summary>
    protected float length = 0.0f;
    protected float startTime = 0.0f;

    void Awake()
    {
        if (audioClip == null && audioName != string.Empty)
        {
            audioClip = Resources.Load("Sound/" + audioName) as AudioClip;
        }
    }

	void Start () {
        length = audioClip == null ? 0 : audioClip.length;
        playType = audioClip == null ? PlayType.None : playType;
        if (playType == PlayType.OnStart)
        {
            NGUITools.PlaySound(audioClip, volume, pitch);
        }
	}


    void OnEnable()
    {
        if (playType == PlayType.OnEnable || playType == PlayType.OnLoop)
        {
            startTime = Time.time;
            NGUITools.PlaySound(audioClip, volume, pitch);
        }
    }


    void OnDisable()
    {
        if (playType == PlayType.OnDisble)
        {
            NGUITools.PlaySound(audioClip, volume, pitch);
        }
    }
	
	void Update () {

        if (playType == PlayType.OnLoop)
        {
            if (Time.time - startTime > length)
            {
                NGUITools.PlaySound(audioClip, volume, pitch);
                startTime = Time.time;
            }
        }
	}
}
