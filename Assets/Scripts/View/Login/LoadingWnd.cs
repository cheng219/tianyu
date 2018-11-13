//==============================================
//作者：吴江
//日期：2015/5/21
//用途：切换场景的加载窗口
//=================================================




using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// 切换场景的加载窗口 by吴江
/// </summary>
public class LoadingWnd : GUIBase
{
    #region UI控件对象
    public UISlider processUISlider;
    public UILabel processNum;
    public UITexture uiTexture;
    public UILabel tipsLabel;
	public UILabel[] labLoading;
    #endregion

    #region 数据
    bool isOnCreateChar = false;

    protected float changeTextTime = 0;

    protected float lastTipsTime = -10;

    protected float singleProgressDuration = 1.5f;
    protected float startProgressTime = 0;

    #endregion


    void Awake()
    {
        //非互斥窗口
        mutualExclusion = false;
        //基础层窗口
        Layer = GUIZLayer.BASE;
    }


    protected override void OnOpen()
    {
        base.OnOpen(); 
		startProgressTime = Time.time;
        Refresh();
        isOnCreateChar = GameCenter.loginMng.isOnCreateChar;
    }

    protected override void OnClose()
    {
        GameCenter.loginMng.isOnCreateChar = false;
        base.OnClose(); 
    }

    protected void Update()
    {
        RefreshPregress();
        RefreshTips();

        
    }



    protected void RefreshPregress()
    {
        float rate = Mathf.Min(1, (Time.time - startProgressTime) / singleProgressDuration);
        processUISlider.value = rate;
        if (rate >= 1)
        {
            processUISlider.value = 0f;
			startProgressTime = Time.time;
			RandomLoadingText();
        }
    }

	protected void RandomLoadingText()
	{
		if(labLoading != null && labLoading.Length > 0)
		{
			int count = labLoading.Length - 1;
			System.Random randomer = new System.Random();
			int index = randomer.Next(0, count);
			for (int i = 0,max=labLoading.Length; i < max; i++) 
			{
				if(i == index)labLoading[i].enabled = true;
				else labLoading[i].enabled = false;
			}
		}
	}

    protected override void HandEvent(bool _bind)
    {
        base.HandEvent(_bind);

    }


    #region 刷新
    protected void Refresh()
    {
        RefreshTexture();
        RefreshTips();
    }


    protected void RefreshTexture()
    {
        //if (uiTexture == null) return;
        //SceneRef refdata = ConfigMng.Instance.GetSceneRef(GameCenter.mainPlayerMng.MainPlayerInfo.SceneID);
        //if (refdata != null)
        //{
        //    Texture2D temp = exResources.GetResource(ResourceType.TEXTURE, refdata.loadingPic) as Texture2D;
        //    if (temp != null)
        //    {
        //        uiTexture.mainTexture = temp;
        //        temp = null;
        //    }
        //}
    }


    protected void RefreshTips()
    {
        if (tipsLabel == null) return;
        if (isOnCreateChar)
        {
            tipsLabel.text = ConfigMng.Instance.GetOnCreateTip();
        }
        else
        {
            if (Time.time - lastTipsTime >= 6.0f)
            {
                lastTipsTime = Time.time;
                tipsLabel.text = ConfigMng.Instance.GetRandomTipsRef();
            }
        }
    }
    #endregion


    #region 控件事件
    /// <summary>
    /// 点击登陆按钮的操作
    /// </summary>
    /// <param name="_btn"></param>
    protected void OnClickLoginBtn(GameObject _btn)
    {
    }
    #endregion




}
