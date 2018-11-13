//=======================================
//作者：吴江
//日期:2015/1/22
//用途：资源更新的进度条界面
//=======================================



using UnityEngine;
using System.Collections;

public class UpdateAssetUI : GUIBase {

	/// <summary>
	/// 主文字显示
	/// </summary>
	public UILabel label_tip;
	/// <summary>
	/// 进度条
	/// </summary>
	public UISlider curProgress;
	/// <summary>
	/// 进度条上的文字预制列表
	/// </summary>
	public UILabel[] loadingTipArray = new UILabel[8];
	/// <summary>
	/// 单个读条的时间
	/// </summary>
	protected float singleProgressDuration = 1.5f;
	/// <summary>
	/// 本次读条的开始时间
	/// </summary>
	protected float startProgressTime = 0;
	/// <summary>
	/// 当前读条的文本序列号
	/// </summary>
	protected int curIndex = 0;

    public GameObject compressLabGo;

    public UILabel labInit;

    public UILabel secondComfirmLabel;
    public UILabel loadLabel;
    public UILabel netFaildLabel;
    public UILabel loadNetFaildLabel;
    

	void Awake()
	{
		mutualExclusion = true;
		Layer = GUIZLayer.NORMALWINDOW;
        if (secondComfirmLabel != null) GameHelper.secondComfirmText = secondComfirmLabel.text;
        if (loadLabel != null) GameHelper.loadText = loadLabel.text;
        if (netFaildLabel != null) GameHelper.netFaildText = netFaildLabel.text;
        if (loadNetFaildLabel != null)GameHelper.loadNetFaildText = loadNetFaildLabel.text;
	}



	protected override void OnOpen()
	{
		base.OnOpen();
		for (int i = 0; i < loadingTipArray.Length; i++)
		{
			loadingTipArray[i].enabled = (i == curIndex);
		}
		RefreshPregress();
	}

    protected override void HandEvent(bool _bind)
    {
        base.HandEvent(_bind);
        if (_bind)
        {
            UpdateAssetStage.OnLoadCountUpdate += UpdateTip;
            UpdateAssetStage.OnEventTypeUpdateEvent += RefreshWnd;
        }
        else
        {
            UpdateAssetStage.OnLoadCountUpdate -= UpdateTip;
            UpdateAssetStage.OnEventTypeUpdateEvent -= RefreshWnd;
        }
    }

    void RefreshWnd(UpdateAssetStage.EventType _eventType)
    {
        isCompressing = false;
        if (compressLabGo != null)
            compressLabGo.SetActive(false);
        if (loadLabel != null)
            loadLabel.enabled = false;
        if (labInit != null)
            labInit.enabled = false;
        switch (_eventType)
        {
            case UpdateAssetStage.EventType.AWAKE:
            case UpdateAssetStage.EventType.COMPAREMD5:
                if (labInit != null)labInit.enabled = true;
                break;
            case UpdateAssetStage.EventType.DECOMPRESSION:
                isCompressing = true;
                if (compressLabGo != null)compressLabGo.SetActive(true);
                break;
            case UpdateAssetStage.EventType.LOADASSET:
                if (loadLabel != null)loadLabel.enabled = true;
                break;
        }
    }

    bool isCompressing = false;
	protected void Update()
	{
        if (isCompressing)
		    RefreshPregress();
	}

	protected void RefreshPregress()
	{
		float rate = Mathf.Min(1, (Time.time - startProgressTime) / singleProgressDuration);
		curProgress.value = rate;
		if (rate >= 1)
		{
			startProgressTime = Time.time;
			curProgress.value = 0;
			curIndex++;
			if (curIndex >= loadingTipArray.Length)
			{
				curIndex = 0;
			}
			for (int i = 0; i < loadingTipArray.Length; i++)
			{
				loadingTipArray[i].enabled = (i == curIndex);
			}
		}
	}


    public void UpdateTip(string _tip)
    {
        if (loadLabel != null)
        {
            loadLabel.enabled = true;
            loadLabel.text = GameHelper.loadText.Replace("#0#", _tip);
        }
    }


}
