//======================================================
//作者:唐源
//日期:2017/2/28
//用途:新手引导界面
//======================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class NoviceGuideWnd : GUIBase {
    #region UI控件
    public GameObject allLock;
    public UISprite arrow;
    public BoxCollider box;
    public UILabel lab;
    public UISprite btnBg;
    /// <summary>
    /// 四个位置 左下 右下 左上右上的填充背景(屏幕适配)
    /// </summary>
    public UISprite leftDown;
    public UISprite rigthDown;
    public UISprite leftUp;
    public UISprite rigthUp;
    public GameObject effect;
    UIRoot uiroot; 
    #endregion
    #region 字段
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
    /// <summary>
    /// 当前引导的静态配置数据
    /// </summary>
    OpenNewFunctionGuideRef refData = null;
    /// <summary>
    /// 下一级引导的静态配置数据
    /// </summary>
    OpenNewFunctionGuideRef nextData = null;
    /// <summary>
    /// 是否开启下一级引导
    /// </summary>
    bool next = false;

    public bool isBox = false;
    private float realY = 0f;
    #endregion
    #region unity函数
    // Use this for initialization
    void Awake()
    {
        mutualExclusion = false;
        this.transform.localPosition = Vector3.zero;
        uiroot = GameObject.FindObjectOfType<UIRoot>();
        InitPos();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isBox)
        {
            CallLockSizePosition();
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
    #region OnOpen
    protected override void OnOpen()
    {
        //Debug.Log("打开引导窗口");
        base.OnOpen();
        if (GameCenter.noviceGuideMng.RefData!=null)
        refData = GameCenter.noviceGuideMng.RefData;
        //在界面开启的时候显示展示引导
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
        if (GameCenter.noviceGuideMng.OnSpecilGuild != null && GameCenter.noviceGuideMng.RefData!= null)
            GameCenter.noviceGuideMng.OnSpecilGuild(GameCenter.noviceGuideMng.RefData);
    }
    #endregion
    #region 位置坐标的计算与适配的实现
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
    /// <summary>
    /// 计算缩放值
    /// </summary>
    void SetScale()
    {
        float rootScale = (float)uiroot.manualWidth / (float)uiroot.activeHeight;//UI宽高比例
        float screenScale = (float)Screen.width / (float)Screen.height;//屏幕分辨率
        if (screenScale > rootScale)
        {
            scale = screenScale / rootScale;
        }
        else
        {
            scale = rootScale / screenScale;
        }
    }
    /// <summary>
    /// 得出缩放值之后计算位置
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    Vector3 GetPos(Vector3 pos)
    { 
        float px = pos.x * scale + ((float)uiroot.manualWidth / 2 - pos.x) * (1 - scale);
        float py = pos.y * scale + ((float)uiroot.activeHeight / 2 - pos.y) * (1 - scale);
        return new Vector3(px, py, 0);
    }  
    Vector3 GetPos(Vector2 pos)
    {
        float px = pos.x * scale + ((float)uiroot.manualWidth / 2 - pos.x) * (1 - scale);
        float py = pos.y * scale + ((float)uiroot.activeHeight / 2 - pos.y) * (1 - scale);
        return new Vector3(px, py, 0);
    }
    /// <summary>
    /// 展示指引
    /// </summary>
    void Show()
    {
        nextData = null;
        next = false;
        if (refData == null)
        {
            Debug.LogError("OpenNewFunctionGuideRef  refData  === null ！");
            return;
        }
        SetScale();
        if (allLock != null) allLock.SetActive(refData.TypeOne);
        if (box != null)
        {
            Vector3 boxPos = refData.TypeTwo ? SetData(refData.rect.position, refData.anchor) : Vector3.zero;
            box.transform.localPosition = boxPos;
            box.size = refData.TypeTwo ? GetPos(refData.rect.size) : new Vector3(uiroot.manualWidth, uiroot.activeHeight, 0);
            SetLockSizePositon(refData.TypeTwo ? box.size : Vector3.zero);
            if (allLock != null) allLock.transform.localPosition = boxPos;
        }
        if (lab != null)
        {
            lab.text = refData.Text;
            lab.width = (int)(refData.Textone.x);
            lab.height = (int)(refData.Textone.y);
            Vector3 btnBgPos = SetData(refData.Texttwo, refData.anchor);
            lab.transform.localPosition = btnBgPos;
            if (btnBg != null)
            {
                btnBg.transform.localPosition = btnBgPos;
                btnBg.width = (int)(refData.Textone.x);
                btnBg.height = (int)(refData.Textone.y);
            }
        }
        if (arrow != null)
        {
            arrow.gameObject.SetActive(refData.Arrow != 0);
            arrow.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, (float)refData.Arrow));
            arrow.transform.localPosition = SetData(refData.ArrowPoint, refData.anchor); 
        } 
        if (effect != null) effect.transform.localPosition = SetData(refData.effectPoint, refData.anchor); 

        CancelInvoke("TrusteeshipClickBtn");
        Invoke("TrusteeshipClickBtn", 10.0f);
    }
    /// <summary>
    /// 显示指引UI
    /// </summary>
    void DisPlay(bool _t)
    {
        if (arrow != null) arrow.enabled = _t;
        if (lab != null) lab.gameObject.SetActive(_t);
        if (btnBg != null) btnBg.gameObject.SetActive(_t);
        if (effect != null) effect.SetActive(_t);
    }
    void SetDelayPos()
    {
        if (leftUp != null) leftUp.transform.localPosition = new Vector3(-900, -450, 0);
        if (rigthUp != null) rigthUp.transform.localPosition = new Vector3(900, -450, 0);
        if (leftDown != null) leftDown.transform.localPosition = new Vector3(-900, 450, 0);
        if (rigthDown != null) rigthDown.transform.localPosition = new Vector3(900, 450, 0); 
        DisPlay(false);
    }
    void RecoveryStepDelay()
    {
        DisPlay(true);
        next = true;
    }
    /// <summary>
    /// 开启指引后设置四个填充区域的位置
    /// </summary>
    /// <param name="_pos"></param>
    void SetLockSizePositon(Vector3 _pos)
    {
        if (leftUp == null || rigthUp == null || leftDown == null || rigthDown == null)
        {
            if (leftUp != null) leftUp.transform.parent.transform.localPosition = _pos;
            return;
        }
        float width = _pos.x / 2;
        float hight = _pos.y / 2;
        leftUp.transform.localPosition = new Vector3(leftUpPos.x + width, leftUpPos.y + hight, 0);
        rigthUp.transform.localPosition = new Vector3(rigthUpPos.x + width, rigthUpPos.y - hight, 0);
        leftDown.transform.localPosition = new Vector3(leftDownPos.x - width, leftDownPos.y + hight, 0);
        rigthDown.transform.localPosition = new Vector3(rigthDownPos.x - width, rigthDownPos.y - hight, 0);
    }
    /// <summary>
    /// 调用设置位置填充区域
    /// </summary>
    void CallLockSizePosition()
    {
        refData = new OpenNewFunctionGuideRef();
        if (box != null)
        {
            Vector3 v2 = new Vector3(box.size.x, box.size.y, 0);
            SetLockSizePositon(v2);
        }
    }
    #endregion
    #region 属性                        
    public OpenNewFunctionGuideRef RefData
    {
        set
        {
            refData = value;
        }
    }
    #endregion
    #region 控件响应事件
    void TrusteeshipClickBtn()
    {
        OnClickBtn(null);
    }
    void OnClickBtn(GameObject go)
    {
        //Debug.Log("当前引导的ID：" + refData.id);
        CancelInvoke("TrusteeshipClickBtn");
        if (refData == null)
        {
            CloseUI();
            return;
        }
        if (!string.IsNullOrEmpty(refData.Button))
        {
            GameObject games = GameObject.Find(refData.Button);
            if (games == null)
            {
                Transform ta = uiroot.transform.Find(refData.Button);
                if (ta != null) games = ta.gameObject;
            }
            if (games != null) games.SendMessage("OnClick", SendMessageOptions.DontRequireReceiver);
        }
        nextData = ConfigMng.Instance.GetOpenNewFunctionGuideRef(refData.type, refData.step + 1);
        if (nextData != null)
        {
            //GameCenter.uIMng.ReleaseGUI(GUIType.NEWGUID);
            //指引延时处理
            if (refData.time != 0)
            {
                SetDelayPos();
                CancelInvoke("RecoveryStepDelay");
                Invoke("RecoveryStepDelay", refData.time);
            }
            else
                next = true;
        }
        else
        {
            CloseUI();
        }
        //新手引导关闭副本同时关闭提示
        if(refData.id==78||refData.id==94|| refData.id == 96)
        {
            //GameCenter.uIMng.ReleaseGUI(GUIType.MESSAGE);
            GameCenter.messageMng.ClearAllMsg();
        }
    }
    #endregion  

    public Vector3 SetData(Vector2 _pos, int _needChange)
    { 
        //若change != 0,则需要换算坐标,锚点上的需要换坐标,1为向下的锚点,2为向上的锚点
        //后面减去的或者加上的是缩放引起的误差
        realY = _needChange== 0 ? 1f : 1136.0f * (float)Screen.height / (float)Screen.width / 640.0f;
        float posY = 0;
        if (_needChange == 2)
        {
            posY = _pos.y * realY + (320f - _pos.y) * Mathf.Abs((1 - realY));
        }
        else if (_needChange == 1)
        {
            posY = _pos.y * realY - (320f + _pos.y) * Mathf.Abs((1 - realY));
        }
        else
        {
            posY = _pos.y;
        }
        return new Vector3(_pos.x, posY, 0);
    }
}
