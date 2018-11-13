//================================================================================
//作者：吴江
//日期：2015/12/16
//用途：声音播放组件
//================================================================================



using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundPlayer : MonoBehaviour
{
    #region 数据  by吴江
    public SoundLoader mySoundLoader = null;
    /// <summary>
    /// 声音收听引用  by吴江
    /// </summary>
    public AudioListener myListener;
    /// <summary>
    /// 音效源  by吴江
    /// </summary>
    public AudioSource mySoundSource;
    /// <summary>
    /// 背景音乐源  by吴江
    /// </summary>
    public AudioSource myBgmSource;
    /// <summary>
    /// 声音的时间
    /// </summary>
    public Dictionary<string, object> SoundPlayTime = new Dictionary<string, object>();

    protected List<List<SoundPlayTask>> soundTaskList = new List<List<SoundPlayTask>>();
    protected List<List<SoundPlayTask>> completeList = new List<List<SoundPlayTask>>();
    #endregion


    #region 初始化
    /// <summary>
    /// 初始化  by吴江
    /// </summary>
    public void Init(SoundLoader _loader)
    {
        mySoundLoader = _loader;
        //音效源 
        if (mySoundSource == null)
        {
            mySoundSource = this.gameObject.AddComponent<AudioSource>();
        }
        //背景音乐源
        if (myBgmSource == null)
        {
            myBgmSource = this.gameObject.AddComponent<AudioSource>();
        }
        //收听组件 
        if (myListener == null)
        {
            myListener = GameObject.FindObjectOfType(typeof(AudioListener)) as AudioListener;
            if (myListener == null)
            {
                myListener = this.gameObject.AddComponent<AudioListener>();
            }
        }

        //根据数据设置源组件
        SetCurBgmValue(GameCenter.systemSettingMng.OpenMusic ? 1 : 0);
        SetCurSoundValue(GameCenter.systemSettingMng.OpenSoundEffect ? 1 : 0);
    }
    #endregion

    #region UNITY
    void Update()
    {
        #region 声音播放队列
        if (soundTaskList.Count > 0)
        {
            completeList.Clear();
            foreach (var list in soundTaskList)
            {
                if (list.Count == 0)
                {
                    completeList.Add(list);
                    continue;
                }
                SoundPlayTask task = list[0];
                if (task.start)
                {
                    if (Time.time - task.startTime >= task.holdTime)
                    {
                        list.RemoveAt(0);
                        if (list.Count == 0)
                        {
                            completeList.Add(list);
                            continue;
                        }
                        task = list[0];
                        PlaySound(task.res);
                        task.startTime = Time.time;
                        task.start = true;
                    }
                }
                else
                {
                    PlaySound(task.res);
                    task.startTime = Time.time;
                    task.start = true;
                }
            }
            foreach (var item in completeList)
            {
                soundTaskList.Remove(item);
            }
        }
        #endregion
    }
    #endregion

    #region 外部调用接口  by吴江

    /// <summary>
    /// 播放指定名称的声音，并返回剪辑
    /// </summary>
    public AudioClip PlaySound(string kClipName, float volumn_mostTimeNoUse = 0.5f, bool bIgnoreDuplicate = false, bool bIgnoreVolumeSetting = false)
    {
        if (kClipName == null || kClipName == string.Empty || kClipName == "0")
        {
            return null;
        }
        mySoundLoader.CheackAbove();
        AudioClip audioClip = null;

        string fileNameWithExtension = kClipName;
        //string fileNameWithoutExtension = Path.GetFileNameWithoutExtension (kClipName);
        if (mySoundLoader.mySoundClipCache.ContainsKey(fileNameWithExtension))
        {
            audioClip = mySoundLoader.mySoundClipCache[fileNameWithExtension];
            PlayLoadedSound(audioClip, volumn_mostTimeNoUse, bIgnoreDuplicate, bIgnoreVolumeSetting);
        }
        else
        {
            mySoundLoader.LoadSoundAsyncAutoPlay(kClipName, volumn_mostTimeNoUse, bIgnoreDuplicate, bIgnoreVolumeSetting);
        }
        return audioClip;
    }
    /// <summary>
    /// 播放已经加载的剪辑
    /// </summary>
    public void PlayLoadedSound(AudioClip kClip, float volumn_mostTimeNoUse, bool bIgnoreDuplicate = true, bool bIgnoreVolumeSetting = false, int playCount = 1)
    {
        if (kClip == null)
        {
            return;
        }
        if (bIgnoreDuplicate)
        {
            if (this.SoundPlayTime[kClip.name] == null)
            {
                this.SoundPlayTime[kClip.name] = Time.time;
            }
            else
            {
                float num = (float)this.SoundPlayTime[kClip.name];
                if (Time.time < num + 2f)
                {
                    return;
                }
                this.SoundPlayTime[kClip.name] = Time.time;
            }
        }
        this.PlaySound(kClip, volumn_mostTimeNoUse, bIgnoreVolumeSetting, playCount);
    }



    /// <summary>
    /// 播放指定剪辑
    /// </summary>
    public void PlaySound(AudioClip kClip, float volumn_mostTimeNoUse, bool bIgnoreVolumeSetting = false, int playCount = 1)
    {

        if (kClip == null)
        {
            return;
        }
        float num = GameCenter.systemSettingMng.SoundEffectVolume;
        if (!bIgnoreVolumeSetting)
        {
            num = Mathf.Clamp01(num);
        }
        else
        {
            num = volumn_mostTimeNoUse;
        }
        if (num == 0f)
        {
            return;
        }
        if (playCount <= 1)
        {
            if (this.mySoundSource.enabled)
            {
                this.mySoundSource.PlayOneShot(kClip, num);
            }
        }
        else
        {
            float durationTime = kClip.length * playCount;
            this.mySoundSource.clip = kClip;
            this.mySoundSource.loop = true;
            this.mySoundSource.Play();
            Invoke("CancelRePlay", durationTime);
        }
    }
    public void CancelRePlay()
    {
        this.mySoundSource.Stop();
    }
    /// <summary>
    /// 播放技能声音队列
    /// </summary>
    /// <param name="_list"></param>
    public void PlaySkillSoundList(List<SkillSoundPair> _list)
    {
        if (_list == null || _list.Count == 0) return;
        List<SoundPlayTask> list = new List<SoundPlayTask>();
        foreach (var item in _list)
        {
            list.Add(new SoundPlayTask(item));
        }
        soundTaskList.Add(list);
    }
    /// <summary>
    /// 设置当前的所有音量的大小 by吴江
    /// </summary>
    /// <param name="_value"></param>
    /// <param name="_record"></param>
    public void SetCurAllVoiceValue(float _value, bool _record = false)
    {
        SetCurSoundValue(_value, _record);
        SetCurBgmValue(_value, _record);
    }
    /// <summary>
    /// 设置当前的音效大小  by吴江
    /// </summary>
    /// <param name="_value"></param>
    /// <param name="_record"></param>
    public void SetCurSoundValue(float _value, bool _record = false)
    {
        _value = Mathf.Clamp01(_value);
      //  GameCenter.systemSettingMng.SoundEffectVolume = _value;
        if (_value > 0)
        {
            mySoundSource.enabled = true;
			mySoundSource.volume = _value;
        }
        else
        {
            mySoundSource.Stop();
            mySoundSource.enabled = false;
			mySoundSource.volume = 0;
        }
    }
    /// <summary>
    /// 设置当前BGM的音量大小  by吴江
    /// </summary>
    /// <param name="_value"></param>
    /// <param name="_record"></param>
    public void SetCurBgmValue(float _value, bool _record = false)
    { 
        _value = Mathf.Clamp01(_value);
        //GameCenter.systemSettingMng.BGMVolume = _value;
        if (_value > 0)
        {
            //myBgmSource.enabled = true;录音播放语音时会重新播放声音，所以注释
            //myBgmSource.Play();
			myBgmSource.volume = _value;
        }
        else
        {
            //myBgmSource.Stop();
            //myBgmSource.enabled = false;
			myBgmSource.volume = 0;
        }
    }


    /// <summary>
    /// 插入非固定背景音乐
    /// </summary>
    public void InsertPlayBGM(string name)
    {
        StopPlayBGM();
        PlayBGM(name, 1);

    }
    /// <summary>
    /// 插入（估计次）非固定背景音乐（0,循环；1一次；x,多次）
    /// </summary>
    public void InsertPlayBGM(string name, int count)
    {
        StopPlayBGM();
        PlayBGM(name, count);

    }



    public void CancelInserBGNDelay(float delayTime)
    {
        Invoke("CancelInserBGN", delayTime);
    }
    /// <summary>
    /// 取消插入的音乐，并播放固定背景音乐
    /// </summary>
    public void CancelInserBGN()
    {
        AutoPlayBGM();
        CancelInvoke("CancelInserBGN");
    }
    /// <summary>
    /// 停止播放背景音乐
    /// </summary>
    public void StopPlayBGM()
    {
        myBgmSource.Stop();
        myBgmSource.clip = null;

    }
    /// <summary>
    /// 播放背景音效 （按名称,次数,音量）
    /// </summary>
    public void PlayBGM(string kBGM, int playCount = 0)
    {
        if (string.IsNullOrEmpty(kBGM))
        {
            return;
        }
        AudioSource component = myBgmSource;
        component.loop = true;

        string audioBundlePath = SoundLoader.GetAudioAssetPath(kBGM);

        BGMLoader bGMLoader = new BGMLoader();
        bGMLoader.kBGM = component;
        bGMLoader.kName = kBGM;
        bGMLoader.fVol = Mathf.Clamp01(GameCenter.systemSettingMng.BGMVolume);
        if (playCount == 0)
        {
            bGMLoader.playCount = 0;
            bGMLoader.OnPlayTime = null;
        }
        else
        {
            bGMLoader.playCount = playCount;
            bGMLoader.OnPlayTime = CancelInserBGNDelay;
        }
        mySoundLoader.RequestAsyncLoad(audioBundlePath, kBGM, bGMLoader);
    }
    /// <summary>
    /// 自动播放背景音效
    /// </summary>
    public void AutoPlayBGM()
    {
        //因为场景配表中没有对应的场景，所以登录子状态的背景音乐只能按状态硬编码
        if (!GameCenter.systemSettingMng.OpenMusic) return; 
		if (GameCenter.curGameStage is LoginStage || GameCenter.curGameStage is CharacterSelectStage || GameCenter.curGameStage is CharacterCreateStage)
        {
            PlayBGM("music/denglu.mp3");
        }else
        {
            int sceneID = GameCenter.mainPlayerMng.MainPlayerInfo.SceneID;
            SceneRef sceneRef = ConfigMng.Instance.GetSceneRef(sceneID);
            if (sceneRef != null && sceneRef.sort != SceneType.CITY)
                PlayBGM(sceneRef.music.ToString());
            else
            {
                //string[] musics = new string[3]{"Music/zhucheng.mp3","Music/newzhucheng.mp3","Music/juqingtianjingpingdan.mp3"};
                //System.Random random = new System.Random();
                //int index = random.Next(0,3);
                PlayBGM("music/zhucheng.mp3");
            }
        }

    }
    #endregion
}



public class SoundPlayTask
{
    public bool start = false;
    public string res;
    public float holdTime;
    public float startTime;


    public SoundPlayTask()
    {
        start = false;
    }
    public SoundPlayTask(SkillSoundPair _pair)
    {
        res = _pair.res;
        holdTime = _pair.time;
        startTime = Time.time;
        start = false;
    }
}