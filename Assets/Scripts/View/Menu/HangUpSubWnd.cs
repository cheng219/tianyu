//====================================
//作者：黄洪兴
//日期：2016/6/12
//用途：挂机设置窗口
//=====================================


using UnityEngine;
using System.Collections;

public class HangUpSubWnd : SubWnd
{
    public GameObject autoFight;
    public GameObject GoShopWnd;
    public UISlider lifeScrollBar;
    public UILabel lifeLabel;
    public GameObject lifeTouch;
    public GameObject lifeStartObj;
    public GameObject lifeEndObj;




    public UISlider magicScrollBar;
    public UILabel magicLabel;
    public GameObject magicTouch;
    public GameObject magicStartObj;
    public GameObject magicEndObj;



    public UISlider flyLifeScrollBar;
    public UILabel flyLifeLabel;
    public GameObject flyLifeTouch;
    public GameObject flyLifeStartObj;
    public GameObject flyLifeEndObj;



    public UISlider flyRandomLifeScrollBar;
    public UILabel flyRandomLifeLabel;
    public GameObject flyRandomLifeTouch;
    public GameObject flyRandomLifeStartObj;
    public GameObject flyRandomLifeEndObj;

    /// <summary>
    /// 是否开启自动吃药
    /// </summary>
    public UIToggle isSafeOpen;
    /// <summary>
    /// 自动吃药的模式  从大到小为TRUE
    /// </summary>
    public UIToggle[] safeModel=new UIToggle[2];
    /// <summary>
    /// 是否自动购买药品
    /// </summary>
    public UIToggle isAutoBuy;
    /// <summary>
    /// 是否开启自动回城
    /// </summary>
    public UIToggle isFlyOpen;
    /// <summary>
    /// 是否开启自动随机传送
    /// </summary>
    public UIToggle isRandomFlyOpen;
    /// <summary>
    /// 是否自动复活
    /// </summary>
    public UIToggle isAutoResurrection;
    /// <summary>
    /// 是否自动躲避BOSS
    /// </summary>
    public UIToggle isHideBoss;
    /// <summary>
    /// 拾取品质
    /// </summary>
    public UIToggle[] pickModel = new UIToggle[4];



    private float mousePoint;
    private bool pressLife = false;
    private bool pressMagic = false;
    private bool pressFlyLife = false;
    private bool pressFlyRandomLife = false;


    #region UNITY
    void Awake()
    {
        type = SubGUIType.HANGUP;
    }
    protected override void HandEvent(bool _bind)
    {
        base.HandEvent(_bind);
        if (_bind)
        {
            if (lifeTouch != null) UIEventListener.Get(lifeTouch).onPress += OnPressLifeTouch;
            if (magicTouch != null) UIEventListener.Get(magicTouch).onPress += OnPressMagicTouch;
            if (flyLifeTouch != null) UIEventListener.Get(flyLifeTouch).onPress += OnPressFlyLifeTouch;
            if (flyRandomLifeTouch != null) UIEventListener.Get(flyRandomLifeTouch).onPress += OnPressFlyRandomLifeTouch;
            if (lifeScrollBar != null)
            {
                lifeScrollBar.value = (float)GameCenter.systemSettingMng.SafeLifeNum / 100;
                if(lifeLabel!=null)
                lifeLabel.text =GameCenter.systemSettingMng.SafeLifeNum.ToString();
            }
            if (magicScrollBar != null)
            {
                magicScrollBar.value = (float)GameCenter.systemSettingMng.SafeMagicNum / 100;
                if (magicLabel != null)
                    magicLabel.text = GameCenter.systemSettingMng.SafeMagicNum.ToString();
            }
            if (flyLifeScrollBar != null)
            {
                flyLifeScrollBar.value = (float)GameCenter.systemSettingMng.FlyLifeNum / 100;
                if (flyLifeLabel != null)
                    flyLifeLabel.text = GameCenter.systemSettingMng.FlyLifeNum.ToString();
            }
            if (flyRandomLifeScrollBar != null)
            {
                flyRandomLifeScrollBar.value = (float)GameCenter.systemSettingMng.RandomFlyLifeNum / 100;
                if (flyRandomLifeLabel != null)
                    flyRandomLifeLabel.text = GameCenter.systemSettingMng.RandomFlyLifeNum.ToString();
            }
            if (isSafeOpen != null)
            {
                EventDelegate.Add(isSafeOpen.onChange, OnClickIsSafeOpen);
                isSafeOpen.value = GameCenter.systemSettingMng.IsSafeOpen;
            }
            if (safeModel != null)
            {
                for (int i = 0; i < safeModel.Length; i++)
                {
                    EventDelegate.Add(safeModel[i].onChange, OnClickSafeModel);
                    safeModel[i].value = GameCenter.systemSettingMng.SafeModel ? i == 1 : i == 0;
                }
            }
            
            if (isAutoBuy != null)
            {
                EventDelegate.Add(isAutoBuy.onChange, OnClickIsAutoBuy);
                isAutoBuy.value = GameCenter.systemSettingMng.IsAutoBuy;
                if (GameCenter.vipMng.VipData.vLev < 1)
                {
                    isAutoBuy.value = false;
                    GameCenter.systemSettingMng.IsAutoBuy = false;
                }
            }
            if (isFlyOpen != null)
            {
                EventDelegate.Add(isFlyOpen.onChange, OnClickIsFlyOpen);
                isFlyOpen.value = GameCenter.systemSettingMng.IsFlyOpen;
            }
            if (isRandomFlyOpen != null)
            {
                EventDelegate.Add(isRandomFlyOpen.onChange, OnClickIsRadomFlyOpen);
                isRandomFlyOpen.value = GameCenter.systemSettingMng.IsRadomFlyOpen;
            }
            if (isAutoResurrection != null)
            {
                EventDelegate.Add(isAutoResurrection.onChange, OnClickIsAutoResurrection);
                isAutoResurrection.value = GameCenter.systemSettingMng.IsAutoResurrection;
            }
            if (isHideBoss != null)
            {
                EventDelegate.Add(isHideBoss.onChange, OnClickIsHideBoss);
                isHideBoss.value = GameCenter.systemSettingMng.IsHideBoss;
            }
            if (pickModel != null)
            {
                for (int i = 0; i < pickModel.Length; i++)
                {
                    EventDelegate.Add(pickModel[i].onChange, OnClickPickModel);
                    pickModel[i].value = i + 1 == GameCenter.systemSettingMng.PickModel;
                }
            }
            if (GoShopWnd != null)
                UIEventListener.Get(GoShopWnd).onClick += GoShop;
            if(autoFight!=null)
                UIEventListener.Get(autoFight).onClick += GoAutoFight;
        }
        else
        {
            if (lifeTouch != null) UIEventListener.Get(lifeTouch).onPress -= OnPressLifeTouch;
            if (magicTouch != null) UIEventListener.Get(magicTouch).onPress -= OnPressMagicTouch;
            if (flyLifeTouch != null) UIEventListener.Get(flyLifeTouch).onPress -= OnPressFlyLifeTouch;
            if (flyRandomLifeTouch != null) UIEventListener.Get(flyRandomLifeTouch).onPress -= OnPressFlyRandomLifeTouch;
            EventDelegate.Remove(isSafeOpen.onChange, OnClickIsSafeOpen);
            for (int i = 0; i < safeModel.Length; i++)
            {
                EventDelegate.Remove(safeModel[i].onChange, OnClickSafeModel);
            }
            EventDelegate.Remove(isAutoBuy.onChange, OnClickIsAutoBuy);
            EventDelegate.Remove(isFlyOpen.onChange, OnClickIsFlyOpen);
            EventDelegate.Remove(isRandomFlyOpen.onChange, OnClickIsRadomFlyOpen);
            EventDelegate.Remove(isAutoResurrection.onChange, OnClickIsAutoResurrection);
            EventDelegate.Remove(isHideBoss.onChange, OnClickIsHideBoss);
            for (int i = 0; i < pickModel.Length; i++)
            {
                EventDelegate.Remove(pickModel[i].onChange, OnClickPickModel);
            }
            if (GoShopWnd != null)
                UIEventListener.Get(GoShopWnd).onClick -= GoShop;
            if (autoFight != null)
                UIEventListener.Get(autoFight).onClick -= GoAutoFight;
        }
    }



    void Update()
    {

        if (pressLife)
        {
            if (lifeScrollBar == null || lifeStartObj == null || lifeEndObj == null)
                return;
            mousePoint = GameCenter.cameraMng.uiCamera.ScreenToWorldPoint(Input.mousePosition).x;
            if (mousePoint <= lifeStartObj.transform.position.x)
            {
                lifeScrollBar.value = 0.01f;
            }
            else if (mousePoint >= lifeEndObj.transform.position.x)
            {
                lifeScrollBar.value = 0.99f;
            }
            else if (mousePoint > lifeStartObj.transform.position.x && mousePoint < lifeEndObj.transform.position.x)
            {
                lifeScrollBar.value = (mousePoint - lifeStartObj.transform.position.x) / (lifeEndObj.transform.position.x - lifeStartObj.transform.position.x);
            }

            RefreshLifeLabel();
        }

        if (pressMagic)
        {
            if (magicStartObj == null || magicEndObj == null || magicScrollBar == null)
                return;
            mousePoint = GameCenter.cameraMng.uiCamera.ScreenToWorldPoint(Input.mousePosition).x;
            if (mousePoint <= magicStartObj.transform.position.x)
            {
                magicScrollBar.value = 0.01f;
            }
            else if (mousePoint >= magicEndObj.transform.position.x)
            {
                magicScrollBar.value = 0.99f;
            }
            else if (mousePoint > magicStartObj.transform.position.x && mousePoint < magicEndObj.transform.position.x)
            {
                magicScrollBar.value = (mousePoint - magicStartObj.transform.position.x) / (magicEndObj.transform.position.x - magicStartObj.transform.position.x);
            }

            RefreshMagicLabel();
        }


        if (pressFlyLife)
        {
            if (flyLifeStartObj == null || flyLifeEndObj == null || flyLifeScrollBar == null)
                return;
            mousePoint = GameCenter.cameraMng.uiCamera.ScreenToWorldPoint(Input.mousePosition).x;
            if (mousePoint <= flyLifeStartObj.transform.position.x)
            {
                flyLifeScrollBar.value = 0.01f;
            }
            else if (mousePoint >= flyLifeEndObj.transform.position.x)
            {
                flyLifeScrollBar.value = 0.99f;
            }
            else if (mousePoint > flyLifeStartObj.transform.position.x && mousePoint < flyLifeEndObj.transform.position.x)
            {
                flyLifeScrollBar.value = (mousePoint - flyLifeStartObj.transform.position.x) / (flyLifeEndObj.transform.position.x - flyLifeStartObj.transform.position.x);
            }

            RefreshFlyLifeLabel();
        }


        if (pressFlyRandomLife)
        {
            if (flyRandomLifeStartObj == null || flyRandomLifeEndObj == null || flyRandomLifeScrollBar == null)
                return;
            mousePoint = GameCenter.cameraMng.uiCamera.ScreenToWorldPoint(Input.mousePosition).x;
            if (mousePoint <= flyRandomLifeStartObj.transform.position.x)
            {
                 flyRandomLifeScrollBar.value = 0.01f;
            }
            else if (mousePoint >= flyRandomLifeEndObj.transform.position.x)
            {
                flyRandomLifeScrollBar.value = 0.99f;
            }
            else if (mousePoint > flyRandomLifeStartObj.transform.position.x && mousePoint < flyRandomLifeEndObj.transform.position.x)
            {
                flyRandomLifeScrollBar.value = (mousePoint - flyRandomLifeStartObj.transform.position.x) / (flyRandomLifeEndObj.transform.position.x - flyRandomLifeStartObj.transform.position.x);
            }

            RefreshFlyRandomLifeLabel();
        }







    }












    #endregion



    void OnPressLifeTouch(GameObject _go,bool _b)
    {
        if (_b)
        {
            pressLife = true;
        }
        else
        {
            pressLife = false;
        }

    }


    void OnPressMagicTouch(GameObject _go, bool _b)
    {
        if (_b)
        {
            pressMagic = true;
        }
        else
        {
            pressMagic = false;
        }

    }
    void OnPressFlyLifeTouch(GameObject _go, bool _b)
    {

        if (_b)
        {
            pressFlyLife = true;
        }
        else
        {
            pressFlyLife = false;
        }
    }
    void OnPressFlyRandomLifeTouch(GameObject _go, bool _b)
    {
        if (_b)
        {
            pressFlyRandomLife = true;
        }
        else
        {
            pressFlyRandomLife = false;
        }

    }








    void RefreshLifeLabel()
    {
        int num=1;
        if(lifeScrollBar.value*100<=1)
        {
            num=1;
        }
        else if(lifeScrollBar.value*100>=99)
        {
            num=99;
        }
        else{
            num=(int)(lifeScrollBar.value*100);
        }
        if (lifeLabel != null)
        {
            lifeLabel.text = num.ToString();
            GameCenter.systemSettingMng.SafeLifeNum = num;
        }
    }
    void RefreshMagicLabel()
    {
        int num = 1;
        if (magicScrollBar.value*100 <= 1)
        {
            num = 1;
        }
        else if (magicScrollBar.value*100 >= 99)
        {
            num = 99;
        }
        else
        {
            num = (int)(magicScrollBar.value * 100);
        }
        if (magicLabel != null)
        {
            magicLabel.text = num.ToString();
            GameCenter.systemSettingMng.SafeMagicNum = num;
        }
    }
    void RefreshFlyLifeLabel()
    {
        int num = 1;
        if (flyLifeScrollBar.value *100<= 1)
        {
            num = 1;
        }
        else if (flyLifeScrollBar.value*100 >= 99)
        {
            num = 99;
        }
        else
        {
            num = (int)(flyLifeScrollBar.value * 100);
        }
        if (flyLifeLabel != null)
        {
            flyLifeLabel.text = num.ToString();
            GameCenter.systemSettingMng.FlyLifeNum = num;
        }
    }
    void RefreshFlyRandomLifeLabel()
    {
        int num = 1;
        if (flyRandomLifeScrollBar.value * 100 <= 1)
        {
            num = 1;
        }
        else if (flyRandomLifeScrollBar.value * 100 >= 99)
        {
            num = 99;
        }
        else
        {
            num = (int)(flyRandomLifeScrollBar.value * 100);
        }
        if (flyRandomLifeLabel != null)
        {
            flyRandomLifeLabel.text = num.ToString();
            GameCenter.systemSettingMng.RandomFlyLifeNum = num;
        }
    }


    public void Refresh()
    {
        RefreshLifeLabel();
        RefreshMagicLabel();
        RefreshFlyLifeLabel();
        RefreshFlyRandomLifeLabel();
    }







    #region 控件事件
    void OnClickIsSafeOpen()
    {
        GameCenter.systemSettingMng.IsSafeOpen = isSafeOpen.value;
    }

    void OnClickSafeModel()
    {
        for (int i = 0; i < safeModel.Length; i++)
        {
            if (safeModel[i].value)
            {
                GameCenter.systemSettingMng.SafeModel = i == 1;
            }
        }
    }
    void OnClickIsAutoBuy()
    {
        if (GameCenter.vipMng.VipData.vLev < 1&&isAutoBuy.value)
        {
            isAutoBuy.value = false;
            GameCenter.messageMng.AddClientMsg(154);
            return;
        }
        
        GameCenter.systemSettingMng.IsAutoBuy = isAutoBuy.value;
    }

    void OnClickIsFlyOpen()
    {
        GameCenter.systemSettingMng.IsFlyOpen = isFlyOpen.value;
    }
    void OnClickIsRadomFlyOpen()
    {
        GameCenter.systemSettingMng.IsRadomFlyOpen = isRandomFlyOpen.value;
    }
    void OnClickIsAutoResurrection()
    {
        GameCenter.systemSettingMng.IsAutoResurrection = isAutoResurrection.value;
    }
    void OnClickIsHideBoss()
    {
        GameCenter.systemSettingMng.IsHideBoss = isHideBoss.value;
    }

    void OnClickPickModel()
    {
        for (int i = 0; i < pickModel.Length; i++)
        {
            if (pickModel[i].value)
            {
                GameCenter.systemSettingMng.PickModel = i + 1;
            }
        }
    }

    void GoShop(GameObject _obj)
    {
        GameCenter.shopMng.OpenWndByType(ShopItemType.NORMAL);

    }

    void GoAutoFight(GameObject _obj)
    {
        GameCenter.curMainPlayer.CancelCommands();
        if (GameCenter.curMainPlayer.AttakType != MainPlayer.AttackType.AUTOFIGHT)
        {
            GameCenter.curMainPlayer.GoAutoFight();
        }
        GameCenter.uIMng.SwitchToUI(GUIType.NONE);

    }

    #endregion
}
