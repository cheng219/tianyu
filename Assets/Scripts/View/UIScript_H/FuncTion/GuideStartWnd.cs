//======================================================
//作者:何明军
//日期:2016/7/6
//用途: 指引界面
//======================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuideStartWnd : SubWnd {
    #region UI控件
    public GameObject allLock;
	public UISprite arrow;
	public BoxCollider box;
	public UILabel lab;
	public UISprite btnBg;
    /// <summary>
    /// 四个位置 左下 右下 左上右上的填充背景
    /// </summary>
    public UISprite leftDown;
	public UISprite rigthDown;
	public UISprite leftUp;
	public UISprite rigthUp;
	public GameObject effect;
    UIRoot uiroot;
    #endregion
    #region 事件
    //public List<EventDelegate> OnGuideOver = new List<EventDelegate>();
    //public System.Action<OpenNewFunctionGuideRef> OnSpecilGuild;
    #endregion
    /// <summary>
    /// 四个位置 左下 右下 左上右上(用于记录初始化位置)
    /// </summary>
    Vector3 leftDownPos;
	Vector3 rigthDownPos;
	Vector3 leftUpPos;
	Vector3 rigthUpPos;
	/// <summary>
    /// 缩放值
    /// </summary>
	float scale = 0;
    #region unity函数
    void Awake(){
		uiroot = GameObject.FindObjectOfType<UIRoot>();
        InitPos();
    }
    void Update()
    {
        if (isBox)
        {
            Test();
            isBox = false;
        }
        if (next && nextData != null)
        {
            next = false;
            RefData = nextData;
            Show();
        }
    }
    #endregion
    protected override void OnOpen()
    {
        base.OnOpen();
        Show();
        UIEventListener.Get(box.gameObject).onClick += OnClickBtn;
    }

    protected override void OnClose()
    {
        base.OnClose();
        UIEventListener.Get(box.gameObject).onClick -= OnClickBtn;
        CancelInvoke("TrusteeshipClickBtn");
        GameCenter.noviceGuideMng.OverGuide();
        //if (gameObject.activeSelf &&
        if (GameCenter.noviceGuideMng.OnGuideOver.Count > 0)
            EventDelegate.Execute(GameCenter.noviceGuideMng.OnGuideOver);
        if (GameCenter.noviceGuideMng.OnSpecilGuild != null && refData != null)
            GameCenter.noviceGuideMng.OnSpecilGuild(refData);
    }
    /// <summary>
    /// 计算缩放
    /// </summary>
    void SetScale(){
		float rootScale = (float)uiroot.manualWidth /(float)uiroot.activeHeight;//UI宽高比例
		float screenScale =   (float)Screen.width/(float)Screen.height;//屏幕分辨率
		if(screenScale > rootScale){
			scale = screenScale / rootScale;
		}else{
			scale = rootScale / screenScale;
		}
	}
	/// <summary>
    /// 计算缩放之后的位置
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
	Vector3 GetPos(Vector3 pos){
		float px = pos.x * scale + ((float)uiroot.manualWidth / 2 - pos.x)*(1-scale);
		float py = pos.y * scale + ((float)uiroot.activeHeight / 2 - pos.y)*(1-scale);
		return new Vector3(px,py,0);
	}
	Vector3 GetPos(Vector2 pos){
		float px = pos.x * scale + ((float)uiroot.manualWidth / 2 - pos.x)*(1-scale);
		float py = pos.y * scale + ((float)uiroot.activeHeight / 2 - pos.y)*(1-scale);
		return new Vector3(px,py,0);
	}
	OpenNewFunctionGuideRef nextData = null;
	bool next = false;
	
	OpenNewFunctionGuideRef refData;
	public OpenNewFunctionGuideRef RefData{
		set{
			refData = value;
		}
	}
	
	void Show(){
		nextData = null;
		next = false;
		if(refData == null){
			Debug.LogError("OpenNewFunctionGuideRef  refData  === null ！");
			return ;
		}
		SetScale();
		if(allLock != null)allLock.SetActive(refData.TypeOne);
		if(box != null){
			Vector3 boxPos = refData.TypeTwo ? GetPos(refData.rect.position) : Vector3.zero;
			box.transform.localPosition = boxPos;
			box.size = refData.TypeTwo ? GetPos(refData.rect.size) : new Vector3(uiroot.manualWidth,uiroot.activeHeight,0);
			SetLockSizePositon(refData.TypeTwo ? box.size : Vector3.zero);
			if(allLock != null)allLock.transform.localPosition = boxPos;
		}
		if(lab != null){
			lab.text = refData.Text;
			lab.width = (int)(refData.Textone.x);
			lab.height = (int)(refData.Textone.y);
			Vector3 btnBgPos = GetPos(refData.Texttwo);
			lab.transform.localPosition = btnBgPos;
			if(btnBg != null){
				btnBg.transform.localPosition = btnBgPos;
				btnBg.width = (int)(refData.Textone.x);
				btnBg.height = (int)(refData.Textone.y);
			}
		}
		if(arrow != null){
			arrow.gameObject.SetActive(refData.Arrow != 0);
			arrow.transform.localRotation = Quaternion.Euler(new Vector3(0f,0f,(float)refData.Arrow));
			arrow.transform.localPosition = GetPos(refData.ArrowPoint);
		}
		if(effect != null)effect.transform.localPosition = GetPos(refData.effectPoint);
		CancelInvoke("TrusteeshipClickBtn");
		Invoke("TrusteeshipClickBtn",10f);
	}
	
	void TrusteeshipClickBtn(){
		OnClickBtn(null);
	}
	void OnClickBtn(GameObject go){
		CancelInvoke("TrusteeshipClickBtn");
		if(refData == null){
			CloseUI();
			return ;
		}
		if(!string.IsNullOrEmpty(refData.Button)){
			GameObject games = GameObject.Find(refData.Button);
			if(games == null){
				Transform ta = uiroot.transform.Find(refData.Button);
				if(ta != null)games = ta.gameObject;
			}
			if(games != null)games.SendMessage("OnClick",SendMessageOptions.DontRequireReceiver);
		}
		nextData = ConfigMng.Instance.GetOpenNewFunctionGuideRef(refData.type,refData.step+1);
		if(nextData != null){
            //指引延时处理
            if (refData.time != 0)
            {
                SetDelayPos();
                CancelInvoke("RecoveryStepDelay");
                Invoke("RecoveryStepDelay", refData.time);
            }
            else
                next = true;
		}else{
			CloseUI();
		}
	}
    void SetDelayPos()
    {
        if (leftUp != null) leftUp.transform.localPosition = new Vector3(-900, -450, 0);
        if (rigthUp != null) rigthUp.transform.localPosition = new Vector3(900, -450, 0);
        if (leftDown != null) leftDown.transform.localPosition = new Vector3(-900, 450, 0);
        if (rigthDown != null) rigthDown.transform.localPosition = new Vector3(900, 450, 0);
        if (arrow != null) arrow.enabled = false;
        if (lab != null) lab.enabled = false;
        if (btnBg != null) btnBg.enabled = false;
        if (effect != null) effect.SetActive(false);
    }
    void RecoveryStepDelay()
    {
        if (arrow != null) arrow.enabled = true;
        if (lab != null) lab.enabled = true;
        if (btnBg != null) btnBg.enabled = true;
        if (effect != null) effect.SetActive(true);
        next = true;
    }
	/// <summary>
    /// 开启指引后设置四个填充区域的位置
    /// </summary>
    /// <param name="_pos"></param>
	void SetLockSizePositon(Vector3 _pos){
		if(leftUp == null || rigthUp == null || leftDown == null || rigthDown == null){
			if(leftUp != null)leftUp.transform.parent.transform.localPosition = _pos;
			return ;
		}
		float width = _pos.x/2;
		float hight = _pos.y/2;
		leftUp.transform.localPosition = new Vector3(leftUpPos.x + width,leftUpPos.y + hight,0);
		rigthUp.transform.localPosition = new Vector3(rigthUpPos.x + width,rigthUpPos.y - hight,0);
		leftDown.transform.localPosition = new Vector3(leftDownPos.x - width,leftDownPos.y + hight,0);
		rigthDown.transform.localPosition = new Vector3(rigthDownPos.x - width,rigthDownPos.y - hight,0);
	}
	
	void Test(){
		refData = new OpenNewFunctionGuideRef();
		if(box != null){
			Vector3 v2 = new Vector3(box.size.x,box.size.y,0);
			SetLockSizePositon(v2);
		}
	}
	public bool isBox = false;
//	public bool IsSetBox{ set{ if(value){ isBox = value; } } }
/// <summary>
/// 初始化位置坐标
/// </summary>
    void InitPos()
    {
        leftDownPos = new Vector3(-900, -450, 0);
        rigthDownPos = new Vector3(900, -450, 0);
        leftUpPos = new Vector3(-900, 450, 0);
        rigthUpPos = new Vector3(900, 450, 0);
    }

}
