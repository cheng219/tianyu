//====================================
//作者：黄洪兴
//日期：2016/7/7
//用途：系统设置窗口
//=====================================


using UnityEngine;
using System.Collections;

public class SettingRnderSubWnd : SubWnd
{


    //public UIToggle[] renderTogs;
	/// <summary>
	/// 音效
	/// </summary>
    public UIToggle soundTog;
	public UISlider soundSlider;
	public GameObject soundTouch;
    public GameObject soundStartObj;
    public GameObject soundEndObj;
    public GameObject soundShow;
    public GameObject soundUnshow;
    public UILabel soundDes;
    /// <summary>
    /// 音乐
    /// </summary>
	public UIToggle musicTog;
	public UISlider musicSlider;
	public GameObject musicTouch;
    public GameObject musicStartObj;
    public GameObject musicEndObj;
    public GameObject musicShow;
    public GameObject musicUnshow;
    public UILabel musicDes;
	/// <summary>
	/// 智能模式
	/// </summary>
	public UIToggle intelligentTog;
	/// <summary>
	/// 流畅模式
	/// </summary>
	public UIToggle fluencyTog;
	/// <summary>
	/// 流畅模式所包含的所有小选项
	/// </summary>
	public UIToggle[] fluencyTogs=new UIToggle[7];
	/// <summary>
	/// 同屏5个玩家
	/// </summary>
	public UIToggle Otherplayer5;
	/// <summary>
	/// 同屏10个玩家
	/// </summary>
	public UIToggle Otherplayer10;
	/// <summary>
	/// 同屏20个玩家
	/// </summary>
	public UIToggle Otherplayer20;
	/// <summary>
	/// 掉落物品名字
	/// </summary>
	public UIToggle itemName;
	/// <summary>
	/// 怪物名称
	/// </summary>
	public UIToggle monsterName;
	/// <summary>
	/// 自身特效
	/// </summary>
	public UIToggle selfEffect;
	/// <summary>
	/// 低纹理
	/// </summary>
	public UIToggle lowTexture;
	/// <summary>
	/// 仅显示可攻击
	/// </summary>
	public UIToggle onlyHostile;
	/// <summary>
	/// 其他玩家特效
	/// </summary>
	public UIToggle otherPlayerEffect;

    /// <summary>
    /// 实时阴影
    /// </summary>
    public UIToggle realShadow;



    private float mousePoint;
    private bool pressSound=false;
    private bool pressMusic=false;
    #region UNITY
    void Awake()
    {
        type = SubGUIType.SYSTEM_SETTING_RENDER;
    }
    protected override void HandEvent(bool _bind)
    {
        base.HandEvent(_bind);
        if (_bind)
        {
            if (realShadow != null)
            {
                EventDelegate.Add(realShadow.onChange, OnClickRealShadow);
                realShadow.value = SystemSettingMng.RealTimeShadow;
            }
            if (musicTouch != null) UIEventListener.Get(musicTouch).onPress += OnPressMusicTouch;
            if (soundTouch != null) UIEventListener.Get(soundTouch).onPress += OnPressSoundTouch;
            if (otherPlayerEffect != null) EventDelegate.Add(otherPlayerEffect.onChange, OnClickOtherPlayerEffectTog);
            if (onlyHostile != null) EventDelegate.Add(onlyHostile.onChange, OnClickOnlyHostileTog);
            if (lowTexture != null) EventDelegate.Add(lowTexture.onChange, OnClickLowTextureTog);
            if (selfEffect != null) EventDelegate.Add(selfEffect.onChange, OnClickSelfEffectTog);
            if (monsterName != null) EventDelegate.Add(monsterName.onChange, OnClickMonsterNameTog);
            if (itemName != null) EventDelegate.Add(itemName.onChange, OnClickItemNameTog);
            if (Otherplayer20 != null) EventDelegate.Add(Otherplayer20.onChange, OnClickOtherplayer20Tog);
            if (Otherplayer10 != null) EventDelegate.Add(Otherplayer10.onChange, OnClickOtherplayer10Tog);
            if (Otherplayer5 != null) EventDelegate.Add(Otherplayer5.onChange, OnClickOtherplayer5Tog);
            if (fluencyTogs[6] != null)
            {
                EventDelegate.Add(fluencyTogs[6].onChange, OnClickAllOtherPlayerTog);

            }
            if (fluencyTogs[5] != null)
            {
                EventDelegate.Add(fluencyTogs[5].onChange, OnClickEffectTog);
                fluencyTogs[5].value = !GameCenter.systemSettingMng.OtherPlayerEffect;
            }
            if (fluencyTogs[4] != null)
            {
                fluencyTogs[4].value = !GameCenter.systemSettingMng.OtherPlayerMagic;
                EventDelegate.Add(fluencyTogs[4].onChange, OnClickMagicTog);
            }
            if (fluencyTogs[3] != null)
            {
                EventDelegate.Add(fluencyTogs[3].onChange, OnClickTitleTog);
                fluencyTogs[3].value = !GameCenter.systemSettingMng.OtherPlayerTitle;
            }
            if (fluencyTogs[2] != null)
            {
                EventDelegate.Add(fluencyTogs[2].onChange, OnClickPetTog);
                fluencyTogs[2].value = !GameCenter.systemSettingMng.OtherPlayerPet;
            }
            if (fluencyTogs[1] != null)
            {
                EventDelegate.Add(fluencyTogs[1].onChange, OnClickSkillTog);
                fluencyTogs[1].value = !GameCenter.systemSettingMng.OtherPlayerSkill;
            }
            if (fluencyTogs[0] != null)
            {
                EventDelegate.Add(fluencyTogs[0].onChange, OnClickWingTog);
                fluencyTogs[0].value = !GameCenter.systemSettingMng.OtherPlayerWing;
            }
            if (fluencyTog != null)
            {
                EventDelegate.Add(fluencyTog.onChange, OnClickFluencyTog);
                fluencyTog.value = GameCenter.systemSettingMng.FluencyMode;
            }
            if (intelligentTog != null)
            {
                EventDelegate.Add(intelligentTog.onChange, OnClickIntelligentTog);
                intelligentTog.value = GameCenter.systemSettingMng.IntelligentMode;
            }
            if (soundTog != null)
            {
                EventDelegate.Add(soundTog.onChange, OnClickSoundTog);
                soundTog.value = GameCenter.systemSettingMng.OpenSoundEffect;
            }
            if (musicTog != null)
            {
                EventDelegate.Add(musicTog.onChange, OnClickMusicTog);
                musicTog.value = GameCenter.systemSettingMng.OpenMusic;
            }
			switch(GameCenter.systemSettingMng.MaxPlayer)
			{
                case 0: if (fluencyTogs[6]!=null) fluencyTogs[6].value = true; break;
                case 5: if (Otherplayer5!=null) Otherplayer5.value = true; break;
                case 10: if (Otherplayer10!=null) Otherplayer10.value = true; break;
                case 20: if (Otherplayer20!=null) Otherplayer20.value = true; break;
                default: if (fluencyTogs[6] && fluencyTogs[6]!=null) fluencyTogs[6].value = true; break;
			}
            if (musicSlider) musicSlider.value = GameCenter.systemSettingMng.BGMVolume;
            if (soundSlider!=null) soundSlider.value = GameCenter.systemSettingMng.SoundEffectVolume;
			switch(GameCenter.systemSettingMng.MaxPlayer)
			{
			case 0:fluencyTogs [6].value = true;break;
			case 5:Otherplayer5.value = true;break;
			case 10:Otherplayer10.value = true;break;
			case 20:Otherplayer20.value = true;break;
			default:fluencyTogs [6].value = true;break;
			}
			itemName.value=!GameCenter.systemSettingMng.ItemName;
			monsterName.value=!GameCenter.systemSettingMng.MonsterName;
			selfEffect.value=!GameCenter.systemSettingMng.SelfEffect;
			onlyHostile.value=GameCenter.systemSettingMng.OnlyHostile;
			otherPlayerEffect.value=!GameCenter.systemSettingMng.OtherPlayerEffect;
			lowTexture.value =GameCenter.systemSettingMng.LowTexture;
            if (soundShow != null) soundShow.SetActive(soundTog.value);
            if (soundUnshow != null) soundUnshow.SetActive(!soundTog.value);
            if (musicShow != null) musicShow.SetActive(musicTog.value);
            if (musicUnshow != null) musicUnshow.SetActive(!musicTog.value);
            RefreshDes();
           
        }
        else
        {
            if (realShadow != null) EventDelegate.Remove(realShadow.onChange, OnClickRealShadow);
            if (musicTouch != null) UIEventListener.Get(musicTouch).onPress -= OnPressMusicTouch;
            if (soundTouch != null) UIEventListener.Get(soundTouch).onPress -= OnPressSoundTouch;
            if (otherPlayerEffect != null) EventDelegate.Remove(otherPlayerEffect.onChange, OnClickOtherPlayerEffectTog);
            if (onlyHostile != null) EventDelegate.Remove(onlyHostile.onChange, OnClickOnlyHostileTog);
            if (lowTexture != null) EventDelegate.Remove(lowTexture.onChange, OnClickLowTextureTog);
            if (selfEffect != null) EventDelegate.Remove(selfEffect.onChange, OnClickSelfEffectTog);
            if (monsterName != null) EventDelegate.Remove(monsterName.onChange, OnClickMonsterNameTog);
            if (itemName != null) EventDelegate.Remove(itemName.onChange, OnClickItemNameTog);
            if (Otherplayer20 != null) EventDelegate.Remove(Otherplayer20.onChange, OnClickOtherplayer20Tog);
            if (Otherplayer10 != null) EventDelegate.Remove(Otherplayer10.onChange, OnClickOtherplayer10Tog);
            if (Otherplayer5 != null) EventDelegate.Remove(Otherplayer5.onChange, OnClickOtherplayer5Tog);
            if (fluencyTogs[6] != null) EventDelegate.Remove(fluencyTogs[6].onChange, OnClickAllOtherPlayerTog);
            if (fluencyTogs[5] != null) EventDelegate.Remove(fluencyTogs[5].onChange, OnClickEffectTog);
            if (fluencyTogs[4] != null) EventDelegate.Remove(fluencyTogs[4].onChange, OnClickMagicTog);
            if (fluencyTogs[3] != null) EventDelegate.Remove(fluencyTogs[3].onChange, OnClickTitleTog);
            if (fluencyTogs[2] != null) EventDelegate.Remove(fluencyTogs[2].onChange, OnClickPetTog);
            if (fluencyTogs[1] != null) EventDelegate.Remove(fluencyTogs[1].onChange, OnClickSkillTog);
            if (fluencyTogs[0] != null) EventDelegate.Remove(fluencyTogs[0].onChange, OnClickWingTog);
            if (fluencyTog != null) EventDelegate.Remove(fluencyTog.onChange, OnClickFluencyTog);
            if (intelligentTog != null) EventDelegate.Remove(intelligentTog.onChange, OnClickIntelligentTog);
            if (soundTog != null) EventDelegate.Remove(soundTog.onChange, OnClickSoundTog);
            if (musicTog != null) EventDelegate.Remove(musicTog.onChange, OnClickMusicTog);
        }
    }



    void Update()
    {
        if (pressSound)
        {
            mousePoint = GameCenter.cameraMng.uiCamera.ScreenToWorldPoint(Input.mousePosition).x;
            if (mousePoint <= soundStartObj.transform.position.x)
            {
                soundSlider.value = 0;
                soundTog.value = false;
            }
            else if (mousePoint >= soundEndObj.transform.position.x)
            {
                soundSlider.value = 1f;
                soundTog.value = true;
            }
            else if (mousePoint > soundStartObj.transform.position.x && mousePoint < soundEndObj.transform.position.x)
            {
                soundSlider.value = (mousePoint - soundStartObj.transform.position.x) / (soundEndObj.transform.position.x - soundStartObj.transform.position.x);
                soundTog.value = true;
            }

            GameCenter.systemSettingMng.SoundEffectVolume = soundSlider.value;
            GameCenter.systemSettingMng.OpenSoundEffect = soundTog.value;
            if (soundShow != null) soundShow.SetActive(soundTog.value);
            if (soundUnshow != null) soundUnshow.SetActive(!soundTog.value);
            RefreshDes();
        }

        if (pressMusic)
        {
            mousePoint = GameCenter.cameraMng.uiCamera.ScreenToWorldPoint(Input.mousePosition).x;
            if (mousePoint <= musicStartObj.transform.position.x)
            {
                musicSlider.value = 0;
                musicTog.value = false;
            }
            else if (mousePoint >= musicEndObj.transform.position.x)
            {
                musicSlider.value = 1f;
                musicTog.value = true;
            }
            else if (mousePoint > musicStartObj.transform.position.x && mousePoint < musicEndObj.transform.position.x)
            {
                musicSlider.value = (mousePoint - musicStartObj.transform.position.x) / (soundEndObj.transform.position.x - musicStartObj.transform.position.x);
                musicTog.value = true;
            }
            GameCenter.systemSettingMng.BGMVolume = musicSlider.value;
            GameCenter.systemSettingMng.OpenMusic = musicTog.value;
            if (musicShow != null) musicShow.SetActive(musicTog.value);
            if (musicUnshow != null) musicUnshow.SetActive(!musicTog.value);
            RefreshDes();
        }



    }











    #endregion

    #region 控件事件
	protected void   OnClickOtherPlayerEffectTog()
	{
		GameCenter.systemSettingMng.OtherPlayerEffect =otherPlayerEffect.value;
	}
	protected void   OnClickOnlyHostileTog()
	{
		GameCenter.systemSettingMng.OnlyHostile =onlyHostile.value;
	}

	protected void   OnClickLowTextureTog()
	{
        GameCenter.systemSettingMng.LowTexture = lowTexture.value;
	}

	protected void   OnClickSelfEffectTog()
	{
      //  Debug.Log("点结果为" + selfEffect.value);
        if (!selfEffect.value)
        {
            fluencyTog.value = false;
            GameCenter.systemSettingMng.FluencyMode = fluencyTog.value;
        }

        GameCenter.systemSettingMng.SelfEffect = !selfEffect.value;


	}

	protected void   OnClickMonsterNameTog()
	{
		GameCenter.systemSettingMng.MonsterName =!monsterName.value;
	}

	protected void   OnClickItemNameTog()
	{
		GameCenter.systemSettingMng.ItemName = !itemName.value;
	}

	protected void   OnClickOtherplayer20Tog()
	{
		if (Otherplayer20.value) {
			GameCenter.systemSettingMng.MaxPlayer = 20;
		}
	}

	protected void   OnClickOtherplayer10Tog()
	{
		if (Otherplayer10.value) {
			GameCenter.systemSettingMng.MaxPlayer = 10;
		}

	}

	protected void   OnClickOtherplayer5Tog()
	{
		if (Otherplayer5.value) {
			GameCenter.systemSettingMng.MaxPlayer = 5;
		}
	}

	protected void  OnClickAllOtherPlayerTog()
	{
		if (fluencyTogs [6].value) {
			GameCenter.systemSettingMng.MaxPlayer = 0;
		} else {
			fluencyTog.value = false;
			GameCenter.systemSettingMng.FluencyMode = fluencyTog.value;
		}
	}

	protected void  OnClickEffectTog()
	{
		if (!fluencyTogs [5].value) {
			fluencyTog.value = false;
			GameCenter.systemSettingMng.FluencyMode = fluencyTog.value;
		}
		GameCenter.systemSettingMng.OtherPlayerEffect = !fluencyTogs [5].value;
	}

	protected void  OnClickMagicTog()
	{
		if (!fluencyTogs [4].value) {
			fluencyTog.value = false;
			GameCenter.systemSettingMng.FluencyMode = fluencyTog.value;
		}
		GameCenter.systemSettingMng.OtherPlayerMagic = !fluencyTogs [4].value;
	}

	protected void  OnClickTitleTog()
	{
		if (!fluencyTogs [3].value) {
			fluencyTog.value = false;
			GameCenter.systemSettingMng.FluencyMode = fluencyTog.value;
		}
		GameCenter.systemSettingMng.OtherPlayerTitle = !fluencyTogs [3].value;
	}

	protected void  OnClickPetTog()
	{
		if (!fluencyTogs [2].value) {
			fluencyTog.value = false;
			GameCenter.systemSettingMng.FluencyMode = fluencyTog.value;
		}
		GameCenter.systemSettingMng.OtherPlayerPet = !fluencyTogs [2].value;
	}


	protected void  OnClickSkillTog()
	{
		if (!fluencyTogs [1].value) {
			fluencyTog.value = false;
			GameCenter.systemSettingMng.FluencyMode = fluencyTog.value;
		}
		GameCenter.systemSettingMng.OtherPlayerSkill = !fluencyTogs [1].value;
	}

	protected void OnClickWingTog()
	{
		if (!fluencyTogs [0].value) {
			fluencyTog.value = false;
			GameCenter.systemSettingMng.FluencyMode = fluencyTog.value;
		}
		GameCenter.systemSettingMng.OtherPlayerWing = !fluencyTogs [0].value;
	}



	protected void OnClickFluencyTog()
	{
		if (fluencyTog.value) {
			for (int i = 0; i < fluencyTogs.Length; i++) {
				fluencyTogs [i].value = true;
			}
            selfEffect.value = true;
//			GameCenter.systemSettingMng.OtherPlayerWing = !fluencyTogs [0].value;
//			GameCenter.systemSettingMng.OtherPlayerSkill = !fluencyTogs [1].value;
//			GameCenter.systemSettingMng.OtherPlayerPet = !fluencyTogs [2].value;
//			GameCenter.systemSettingMng.OtherPlayerTitle = !fluencyTogs [3].value;
//			GameCenter.systemSettingMng.OtherPlayerMagic = !fluencyTogs [4].value;
//			GameCenter.systemSettingMng.OtherPlayerEffect = !fluencyTogs [5].value;
//			GameCenter.systemSettingMng.MaxPlayer = 0;
			OnClickWingTog ();
			OnClickSkillTog ();
			OnClickPetTog ();
			OnClickTitleTog ();
			OnClickMagicTog ();
			OnClickEffectTog ();
			OnClickAllOtherPlayerTog ();
            OnClickSelfEffectTog();


		}
        if (intelligentTog.value == fluencyTog.value && fluencyTog.value==true)
        {
            intelligentTog.value = !fluencyTog.value;
            OnClickIntelligentTog();
        }
		GameCenter.systemSettingMng.FluencyMode = fluencyTog.value;

	}


	protected void OnClickIntelligentTog()
	{
		GameCenter.systemSettingMng.IntelligentMode = intelligentTog.value;
        if (fluencyTog.value == intelligentTog.value && fluencyTog.value==true)
        {
            fluencyTog.value = !intelligentTog.value;
            OnClickFluencyTog();
        }

	}


    protected void OnClickSoundTog()
    {
		GameCenter.systemSettingMng.OpenSoundEffect = soundTog.value;
        if (soundShow != null) soundShow.SetActive(soundTog.value);
        if (soundUnshow != null) soundUnshow.SetActive(!soundTog.value);
        RefreshDes();
    }
    protected void OnClickMusicTog()
    {
		GameCenter.systemSettingMng.OpenMusic = musicTog.value;
        if (musicShow != null) musicShow.SetActive(musicTog.value);
        if (musicUnshow != null) musicUnshow.SetActive(!musicTog.value);
        RefreshDes();
    }

    protected void OnClickRealShadow()
    {
        SystemSettingMng.RealTimeShadow= realShadow.value;
        SceneRoot instance = SceneRoot.GetInstance();
        if (instance != null)
        {
            instance.DirectionalLightActive(SystemSettingMng.RealTimeShadow);
        }
    }





	void OnPressSoundTouch(GameObject _obj,bool _b)
	{
        if (_b)
        {
            pressSound = true;
        }
        else
        {
            pressSound = false;
        }


		
	}
	void OnPressMusicTouch(GameObject obj,bool _b)
	{

        if (_b)
        {
            pressMusic = true;
        }
        else
        {
            pressMusic = false;
        }

	}


    void RefreshDes()
    {
        if (musicDes != null)
        {
            if (musicShow != null)
            {
                if (!musicShow.activeSelf)
                {
                    musicDes.text = ConfigMng.Instance.GetUItext(325);
                }
                else
                {
                    musicDes.text =ConfigMng.Instance.GetUItext(326);
                }
            }
        }
        if (soundDes != null)
        {
            if (soundShow != null)
            {
                if (!soundShow.activeSelf)
                {
                    soundDes.text = ConfigMng.Instance.GetUItext(327);
                }
                else
                {
                    soundDes.text = ConfigMng.Instance.GetUItext(328);
                }
            }
        }

    }


    #endregion
}
