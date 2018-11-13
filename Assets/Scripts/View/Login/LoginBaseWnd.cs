//==============================
//作者：邓成
//日期：2016/7/12
//用途：登录界面类
//==============================

using UnityEngine;
using System.Collections;

public class LoginBaseWnd : GUIBase {
	
	void Awake()
	{
		//非互斥窗口
		mutualExclusion = true;
		//基础层窗口
		Layer = GUIZLayer.BASE;
	}
	protected override void OnOpen ()
	{
		base.OnOpen ();
		if(initSubGUIType == SubGUIType.NONE)
		{
			if(GameCenter.instance.isPlatform)
			{
				SwitchToSubWnd(SubGUIType.PLATFORMLOGIN);
			}else
			{
				SwitchToSubWnd(SubGUIType.NORMALLOGIN);
			}
		}
        PreloadModel();
	}
	protected override void OnClose ()
	{
		base.OnClose ();
	}

	protected override void HandEvent(bool _bind)
	{
		base.HandEvent(_bind);
		if (_bind)
		{
			GameCenter.OnConnectStateChange += OnConnectStateChange;
		}
		else
		{
			GameCenter.OnConnectStateChange -= OnConnectStateChange;
		}
	}

	protected void OnConnectStateChange(bool _isConnect)
	{
		if (GameCenter.instance.IsConnected && GameCenter.loginMng.CurConnectServerType == LoginMng.ConnectServerType.Queue)
		{
			GameCenter.loginMng.C2S_Login();
		}
	}

    protected void PreloadModel()
    {
        if (GameCenter.loginMng.createPlayerInfoDic.Count > 0) return;
        StartCoroutine(CouPreloadModel());
    }

    IEnumerator CouPreloadModel()
    {
        while (ConfigMng.Instance.Pendings > 0)
        {
            yield return new WaitForFixedUpdate();
        }
        if (GameCenter.loginMng.createPlayerInfoDic.Count == 0)
        {
            GameCenter.loginMng.InitCreatePlayerInfo();
            foreach (PlayerBaseInfo item in GameCenter.loginMng.createPlayerInfoDic.Values)
            {
                CreatePlayer preloadPlayer = CreatePlayer.CreateDummy(item);
                preloadPlayer.StartAsyncCreate(() =>
                {
                    preloadPlayer.transform.position = new Vector3(-1000, -1000, -1000);
                });

                AbilityInstance ability = new AbilityInstance(item.CreateAbilityID, 1, null, null);
                if (ability != null)
                {
                    string effectName = ability.ProcessEffectList.Count > 0 ? ability.ProcessEffectList[0] : string.Empty;
                    if (!string.IsNullOrEmpty(effectName))
                    {
                        AssetMng.GetEeffctAssetObject(effectName, (x) => { });
                        //AssetMng.GetEffectInstance(effectName, null);//预加载特效
                    }
                }
            }
        }
    }
}
