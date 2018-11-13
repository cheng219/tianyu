//================================================================================
//作者：吴江
//日期：2015/12/16
//用途：声音加载及播放管理类
//================================================================================


using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;
using System.Collections;

public class SoundMng
{
    #region 数据
    /// <summary>
    /// 播放对象
    /// </summary>
    protected SoundPlayer mySoundPlayer = null;
    /// <summary>
    /// 加载对象
    /// </summary>
    protected SoundLoader mySoundLoader = null;
    #endregion


    #region 构造
    public static SoundMng CreateNew()
    {
        if (GameCenter.soundMng == null)
        {
            SoundMng soundMng = new SoundMng();
            soundMng.Init();
            return soundMng;
        }
        else
        {
            GameCenter.soundMng.UnRegist();
            GameCenter.soundMng.Init();
            return GameCenter.soundMng;
        }
    }


    /// <summary>
    /// 初始化
    /// </summary>
    protected virtual void Init()
    {
        //音频扬声器模式;
		//API修改(AudioSettings.driverCapabilities;在IOS上无声音) (修改前:AudioSettings.speakerMode = AudioSettings.driverCapabilities) by邓成
		AudioConfiguration confuguration = AudioSettings.GetConfiguration();
		confuguration.speakerMode = AudioSettings.driverCapabilities;
		AudioSettings.Reset(confuguration);
        //初始化播放源组件
        if (mySoundLoader != null)
        {
            GameObject.DestroyImmediate(mySoundLoader);
            mySoundLoader = null;
        }
        GameObject loaderObj = new GameObject("SoundLoader");
        loaderObj.transform.parent = GameCenter.instance.transform;
        loaderObj.transform.localPosition = Vector3.zero;
        loaderObj.transform.localEulerAngles = Vector3.zero;
        loaderObj.transform.localScale = Vector3.one;
        mySoundLoader = loaderObj.AddComponent<SoundLoader>();

        if (mySoundPlayer != null)
        {
            GameObject.DestroyImmediate(mySoundPlayer);
            mySoundPlayer = null;
        }
        GameObject playerObj = new GameObject("SoundPlayer");
        playerObj.transform.parent = GameCenter.instance.transform;
        playerObj.transform.localPosition = Vector3.zero;
        playerObj.transform.localEulerAngles = Vector3.zero;
        playerObj.transform.localScale = Vector3.one;
        mySoundPlayer = playerObj.AddComponent<SoundPlayer>();
        mySoundPlayer.Init(mySoundLoader);


        preLoadSound();
        GameCenter.systemSettingMng.OnUpdate += OnUpdateSystemSettings;
    }
    /// <summary>
    /// 注销
    /// </summary>
    protected virtual void UnRegist()
    {
        if (mySoundLoader != null)
        {
            mySoundLoader.ClearCache();
        }
        GameCenter.systemSettingMng.OnUpdate -= OnUpdateSystemSettings;
    }
    /// <summary>
    /// 预加载系统常用声音
    /// </summary>
    protected void preLoadSound()
    {
        //NGUIDebug.Log("预加载系统常用声音");
        if (GameCenter.systemSettingMng.OpenSoundEffect)
        {
            foreach (SystemSound wav in Enum.GetValues(typeof(SystemSound)))
            {
                mySoundLoader.AddCache(GetDescription(wav));
            }
        }
        //NGUIDebug.Log("预加载系统常用声音finish");
    }
    #endregion



    #region 辅助逻辑
    /// <summary>
    /// 响应系统设置的修改
    /// </summary>
    /// <param name="_type"></param>
    /// <param name="_open"></param>
    protected void OnUpdateSystemSettings(SystemSettingType _type, bool _open,float _num)
    {
        switch (_type)
        {
            case SystemSettingType.Music: 
                mySoundPlayer.SetCurBgmValue(_num);
                if (_open)
                { 
                    AutoPlayBGM();
                } 
                break;
            case SystemSettingType.SoundEffect:
                mySoundPlayer.SetCurSoundValue(_num);
                break;
            default:
                break;
        }
    }

    public static string GetDescription(SystemSound value)
    {
        string path = "";
        switch (value)
        {
            default:
                break;
        }
        return path;
        //	    FieldInfo fi= value.GetType().GetField(value.ToString());
        //	    DescriptionAttribute[] attributes =(DescriptionAttribute[])fi.GetCustomAttributes(
        //	    typeof(DescriptionAttribute), false);
        //	    return (attributes.Length>0)?attributes[0].Description:value.ToString();
    }

    #endregion

    #region 外部调用接口
    /// <summary>
    /// 播放已经加载的剪辑
    /// </summary>
    public void PlayLoadedSound(AudioClip kClip, float volumn_mostTimeNoUse, bool bIgnoreDuplicate = true, bool bIgnoreVolumeSetting = false, int playCount = 1)
    {
        mySoundPlayer.PlayLoadedSound(kClip, volumn_mostTimeNoUse, bIgnoreDuplicate, bIgnoreVolumeSetting, playCount);
    }

        /// <summary>
    /// 添加到缓存
    /// </summary>
    public void AddCache(string _soundName)
    {
        mySoundLoader.AddCache(_soundName);
    }

    /// <summary>
    /// 添加到缓存
    /// </summary>
    public void AddCache(string kName, AudioClip kClip)
    {
        mySoundLoader.AddCache(kName, kClip);
        mySoundPlayer.SoundPlayTime[kName] = Time.time;
    }
    /// <summary>
    /// 播放系统音效
    /// </summary>
    public void PlaySystemSound(SystemSound wav)
    {
        mySoundPlayer.PlaySound(GetDescription(wav));
    }

    /// <summary>
    /// 取消当前重播
    /// </summary>
    void CancelRePlay()
    {
        mySoundPlayer.mySoundSource.Stop();
    }
    /// <summary>
    /// 播放声音
    /// </summary>
    /// <param name="_soundName"></param>
    /// <param name="volumn_mostTimeNoUse"></param>
    /// <param name="bIgnoreDuplicate"></param>
    /// <param name="bIgnoreVolumeSetting"></param>
    public void PlaySound(string _soundName, float volumn_mostTimeNoUse = 0.5f, bool bIgnoreDuplicate = false, bool bIgnoreVolumeSetting = false)
    {
        mySoundPlayer.PlaySound(_soundName, volumn_mostTimeNoUse, bIgnoreDuplicate, bIgnoreVolumeSetting);
    }


    public static float GetSceneSoundValue(Transform _point, Transform _center,float _limit = 20.0f)
    {
        if(_point == null || _center == null) return 0;
        float value = (_point.position - _center.position).sqrMagnitude > _limit * _limit ? 0 : 0.5f;
        return value;
    }


    /// <summary>
    /// 播放技能声音队列
    /// </summary>
    /// <param name="_list"></param>
    public void PlaySkillSoundList(List<SkillSoundPair> _list)
    {
        mySoundPlayer.PlaySkillSoundList(_list);;
    }
    /// <summary>
    /// 设置当前的所有音量的大小 by吴江
    /// </summary>
    /// <param name="_value"></param>
    /// <param name="_record"></param>
    public void SetCurAllVoiceValue(float _value, bool _record = false)
    {
        mySoundPlayer.SetCurAllVoiceValue(_value, _record);
    }
    /// <summary>
    /// 设置当前的音效大小  by吴江
    /// </summary>
    /// <param name="_value"></param>
    /// <param name="_record"></param>
    public void SetCurSoundValue(float _value, bool _record = false)
    {
        mySoundPlayer.SetCurSoundValue(_value, _record);
    }
    /// <summary>
    /// 设置当前BGM的音量大小  by吴江
    /// </summary>
    /// <param name="_value"></param>
    /// <param name="_record"></param>
    public void SetCurBgmValue(float _value, bool _record = false)
    {
        mySoundPlayer.SetCurBgmValue(_value, _record);
    }


    /// <summary>
    /// 插入非固定背景音乐
    /// </summary>
    public void InsertPlayBGM(string name)
    {
        mySoundPlayer.InsertPlayBGM(name);

    }
    /// <summary>
    /// 插入（估计次）非固定背景音乐（0,循环；1一次；x,多次）
    /// </summary>
    public void InsertPlayBGM(string name, int count)
    {
        mySoundPlayer.InsertPlayBGM(name, count);
    }



    void CancelInserBGNDelay(float delayTime)
    {
        mySoundPlayer.CancelInserBGNDelay(delayTime);
    }
    /// <summary>
    /// 取消插入的音乐，并播放固定背景音乐
    /// </summary>
    public void CancelInserBGN()
    {
        mySoundPlayer.CancelInserBGN();
    }
    /// <summary>
    /// 停止播放背景音乐
    /// </summary>
    public void StopPlayBGM()
    {
        mySoundPlayer.StopPlayBGM();

    }

    /// <summary>
    /// 自动播放背景音效
    /// </summary>
    public void AutoPlayBGM()
    { 
        if (!GameCenter.systemSettingMng.OpenMusic) return; 
        //因为场景配表中没有对应的场景，所以登录子状态的背景音乐只能按状态硬编码
		if (GameCenter.curGameStage is LoginStage || GameCenter.curGameStage is CharacterSelectStage || GameCenter.curGameStage is CharacterCreateStage)
        {
            mySoundPlayer.PlayBGM("music/denglu.mp3");
        }else
        {
            int sceneID = GameCenter.mainPlayerMng.MainPlayerInfo.SceneID;
            SceneRef sceneRef = ConfigMng.Instance.GetSceneRef(sceneID);
            if (sceneRef != null)
            {
                mySoundPlayer.PlayBGM(sceneRef.music.ToString());
            }
            else
            {
                mySoundPlayer.PlayBGM("music/zhucheng.mp3");
            }
        }

    }
    #endregion


	
	
}



public class AudioLoader : LoaderCallback
{
    /// <summary>
    /// 声音名称
    /// </summary>
    public string kName;

    /// <summary>
    /// 声音剪辑
    /// </summary>
    public AudioClip kClip;

    /// <summary>
    /// 加载完成 执行的方法
    /// </summary>
    public override void OnLoaded(Object kObj, bool bError)
    {
        this.bResult = bError;
        if (kObj == null)
        {
            return;
        }
        this.kClip = (kObj as AudioClip);
        if (this.kClip != null)
        {
            this.kClip.name = kName;
            GameCenter.soundMng.AddCache(this.kName, this.kClip);
        }
    }
}

public class AudioAutoPlayLoader : LoaderCallback
{
    /// <summary>
    /// 忽略重复
    /// </summary>
    public bool bIgnoreDuplicate;

    /// <summary>
    /// 忽略音量设置
    /// </summary>
    public bool bIgnoreVolumeSetting;

    /// <summary>
    ///音量
    /// </summary>
    public float fVol;

    /// <summary>
    /// 声音剪辑
    /// </summary>
    public AudioClip kClip;

    /// <summary>
    /// 声音名称
    /// </summary>
    public string kName;

    public int playCount = 1;
    /// <summary>
    /// 加载完成 执行的方法
    /// </summary>
    public override void OnLoaded(Object kObj, bool bError)
    {
        this.bResult = bError;
        if (kObj == null)
        {
            return;
        }
        this.kClip = (kObj as AudioClip);
        if (this.kClip != null)
        {
            this.kClip.name = kName;

            GameCenter.soundMng.AddCache(this.kName, this.kClip);
            GameCenter.soundMng.PlayLoadedSound(this.kClip, this.fVol, this.bIgnoreDuplicate, this.bIgnoreVolumeSetting, playCount);
        }
    }
}

public class BGMLoader : LoaderCallback
{
    public AudioClip kClip;
    public AudioSource kBGM;
    public float fVol;
    public string kName;
    public int playCount = 0;
    public Action<float> OnPlayTime;
    public override void OnLoaded(Object kObj, bool bError)
    {
        this.bResult = bError;
        if (kObj == null)
        {
            return;
        }
        this.kClip = (kObj as AudioClip);
        if (this.kClip != null && this.kBGM != null && this.kBGM.enabled)
        {
            this.kClip.name = kName;
            this.kBGM.clip = this.kClip;
            this.kBGM.volume = this.fVol;
            if (playCount == 0)
                this.kBGM.Play();
            else
            {
                float durationTime = this.kBGM.clip.length * playCount;
                this.kBGM.Play();
                if (OnPlayTime != null) OnPlayTime(durationTime);
            }
        }
    }
}