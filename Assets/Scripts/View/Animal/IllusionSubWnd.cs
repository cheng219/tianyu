//==================================
//作者：朱素云
//日期：2016/3/28
//用途：坐骑幻化界面
//=================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IllusionSubWnd : SubWnd
{
    #region 数据
    public UITexture lookLike;//模型
    public UILabel nameLab;
    public UILabel levLab;
    public UISlider expSli;
    public UILabel expLab;
    public UIButton addPropertyBtn;
    public GameObject propertyObj;
    public UIButton leftBtn;//上一页
    public UIButton rightBtn;//下一页 
    public UIButton illusionBtn;//幻化按钮
    public UIButton relieveBtn;//解除幻化
    public UITimer timer;
    public UILabel timeLab; 
    /// <summary>
    /// 坐骑表中的幻兽链表
    /// </summary>
    protected List<MountRef> mountSkinList = new List<MountRef>(); 
    /// <summary>
    /// 显示在格子中的幻兽预制
    /// </summary> 
    protected FDictionary skinItemList = new FDictionary();
    /// <summary>
    /// 玩家拥有的皮肤
    /// </summary>
    protected FDictionary mountInfoDic
    {
        get
        {
            return GameCenter.newMountMng.mountSkinList; 
        }
    } 
    public GameObject parent;
    public UIButton closePropertyBtn; 
    protected MountRef curMount = null;
    protected MountInfo curSkin = null;
    public List<UILabel> curPropertyLab = new List<UILabel>();//属性加成
    public List<UILabel> nextLevAddLab = new List<UILabel>();
    private FDictionary skinDic
    {
        get
        {
            return GameCenter.newMountMng.AllSkinDic;
        }
    } 
    protected int dex = 0;
    #endregion

    #region 构造
    void Awake()
    { 
        if (addPropertyBtn != null) UIEventListener.Get(addPropertyBtn.gameObject).onClick = OnAddPropertyBtn;
        if (leftBtn != null) UIEventListener.Get(leftBtn.gameObject).onClick = OnClickLeftBtn;
        if (rightBtn != null) UIEventListener.Get(rightBtn.gameObject).onClick = OnClickRightBtn;
        if (closePropertyBtn != null) UIEventListener.Get(closePropertyBtn.gameObject).onClick = OnClickClosePropertyBtn;
        if (illusionBtn != null) UIEventListener.Get(illusionBtn.gameObject).onClick = OnClickIllusionBtn;
        if (relieveBtn != null) UIEventListener.Get(relieveBtn.gameObject).onClick = OnClickRelieveBtn;
        if (propertyObj != null) propertyObj.gameObject.SetActive(false);
    }
    protected override void OnOpen()
    {
        base.OnOpen();
        mountSkinList = GameCenter.newMountMng.MountList(2); 
        dex = 0;
        ShowSkin();
        RefreshSkinList();
        int len = dex / 9 == 0 ? (dex / 9 - 1) : dex / 9; 
        if (parent != null && parent.transform.localPosition.x > (100 - len * 428.6f))
        {
            parent.transform.localPosition = new Vector3(parent.transform.localPosition.x - 428.6f * len, parent.transform.localPosition.y);
        } 
        GameCenter.newMountMng.OnMountSkinListUpdate += ShowSkin;
        GameCenter.newMountMng.OnMountSkinUpdate += RefreshSkinList;
    }
    protected override void OnClose()
    {
        base.OnClose();
        lookLike.mainTexture = null; 
        GameCenter.newMountMng.OnMountSkinListUpdate -= ShowSkin;
        GameCenter.newMountMng.OnMountSkinUpdate -= RefreshSkinList; 
    } 
    #endregion

    #region 控件事件
    void OnChangeSkin(GameObject go)
    {   
        int id = (int)UIEventListener.Get(go).parameter;
        for (int i = 0, max = mountSkinList.Count; i < max; i++)
        {
            if (id== mountSkinList[i].mountId)
            {
                curMount = mountSkinList[i];
                break;
            }
        }
        RefreshSkinList();
    } 
    /// <summary>
    /// 关闭属性加成
    /// </summary>
    /// <param name="go"></param>
    void OnClickClosePropertyBtn(GameObject go)
    {
        if (propertyObj != null) propertyObj.gameObject.SetActive(false);
    }
    /// <summary>
    /// 属性加成
    /// </summary>
    /// <param name="go"></param>
    void OnAddPropertyBtn(GameObject go)
    {
        if (propertyObj != null) propertyObj.gameObject.SetActive(true);
        if (CheckIfHaveMount())
        { 
            return;  
        }
        //如果玩家没有幻兽
        for (int i = 0; i < curPropertyLab.Count; i++)
        {
            curPropertyLab[i].text = "0";
        }
        SkinPropertyRef nextInfo = ConfigMng.Instance.GetSkinPropertyRef(1);
        if (nextInfo != null)
        {
            for (int i = 0; i < nextLevAddLab.Count; i++)
            {
                nextLevAddLab[i].text = (nextInfo.attr[i]).ToString();
            }
        }
    } 
    /// <summary>
    /// 幻化
    /// </summary>
    /// <param name="go"></param>
    void OnClickIllusionBtn(GameObject go)
    {
        if (curMount != null)
        {
            if (mountInfoDic.ContainsKey(curMount.mountId))//幻化当前幻兽
            { 
                GameCenter.newMountMng.C2S_ReqRideMount(ChangeMount.PUTSKIN, curMount.mountId, MountReqRideType.SELT);
            }
            else
            {
                GameCenter.messageMng.AddClientMsg(305);
            }
        }
    }
    /// <summary>
    /// 解除幻化
    /// </summary>
    /// <param name="go"></param>
    void OnClickRelieveBtn(GameObject go)
    {
        if (GameCenter.newMountMng.skinId != 0)
            GameCenter.newMountMng.C2S_ReqRideMount(ChangeMount.DOWNSKIN, GameCenter.newMountMng.skinId, MountReqRideType.SELT);
    }
    /// <summary>
    /// 上一页
    /// </summary>
    /// <param name="go"></param>
    void OnClickLeftBtn(GameObject go)
    { 
        if (parent != null && parent.transform.localPosition.x < 100)
        {
            parent.transform.localPosition = new Vector3(parent.transform.localPosition.x + 428.6f, parent.transform.localPosition.y);
            ShowSkin();
        } 
    }
    /// <summary>
    /// 下一页
    /// </summary>
    /// <param name="go"></param>
    void OnClickRightBtn(GameObject go)
    { 
        int len = 0;
        if (skinItemList.Count / 9 == 0)
            len = skinItemList.Count / 9 - 1;
        else len = skinItemList.Count / 9; 
       
        if (parent != null && parent.transform.localPosition.x > (100 - len*428.6f))
        {
            parent.transform.localPosition = new Vector3(parent.transform.localPosition.x - 428.6f, parent.transform.localPosition.y);
            ShowSkin();
        } 
    } 

    void ShowPageBtn()
    { 
        if (parent != null && leftBtn != null && rightBtn != null)
        { 
            UISpriteEx leftsp = leftBtn.GetComponentInChildren<UISpriteEx>();
            if (leftsp != null)
            { 
                if (parent.transform.localPosition.x < 100)
                {

                    leftsp.IsGray = UISpriteEx.ColorGray.normal;
                }
                else
                { 
                    leftsp.IsGray = UISpriteEx.ColorGray.Gray;
                }
            }
             
            UISpriteEx rightsp = rightBtn.GetComponentInChildren<UISpriteEx>();
            if (rightsp != null)
            {
                int len = 0;
                if (skinItemList.Count / 9 == 0)
                    len = skinItemList.Count / 9 - 1;
                else len = skinItemList.Count / 9;
                if (parent.transform.localPosition.x > (100 - len * 403))
                {
                    rightsp.IsGray = UISpriteEx.ColorGray.normal;
                }
                else
                {
                    rightsp.IsGray = UISpriteEx.ColorGray.Gray;
                }
            }
        }
    }
    #endregion

    #region 刷新 
    void RefreshSkinList()
    { 
        if (GameCenter.newMountMng.curChooseSkin != null)
        {
            for (int i = 0, max = mountSkinList.Count; i < max; i++)
            {
                if (GameCenter.newMountMng.curChooseSkin.ConfigID == mountSkinList[i].mountId)
                {
                    curMount = mountSkinList[i]; 
                    GameCenter.newMountMng.curChooseSkin = null;
                    break;
                }
            } 
        } 
        if (curMount == null)
        {
            for (int i = 0, max = mountSkinList.Count; i < max; i++)
            {
                if (mountSkinList[i].mountId == GameCenter.newMountMng.skinId)
                {
                    curMount = mountSkinList[i];
                    break;
                }
            } 
        }
        if (curMount == null)
        { 
            foreach (SkinItem skin in skinItemList.Values)
            { 
                curMount = skin.MountRefDate;//默认显示第一个皮肤信息  
                break;
            } 
        } 
        if (curMount != null)
        { 
            foreach (SkinItem skin in skinItemList.Values)
            {
                UIToggle tog = skin.GetComponent<UIToggle>(); 
                if (tog != null && skin.MountRefDate.mountId == curMount.mountId)
                {
                    tog.value = true;
                    ++dex; 
                    break;
                } 
            }  
            nameLab.text = curMount.mountName;
            if (skinDic.ContainsKey(curMount.mountId))
            {
                GameCenter.previewManager.TryPreviewSingelMount(skinDic[curMount.mountId] as MountInfo, lookLike);
            }
            if (illusionBtn != null) illusionBtn.gameObject.SetActive(true);
            if (relieveBtn != null) relieveBtn.gameObject.SetActive(false);
            if (mountInfoDic.ContainsKey(curMount.mountId))
            {
                MountInfo info = mountInfoDic[curMount.mountId] as MountInfo;
                if (info.IsRiding)
                { 
                    if (illusionBtn != null) illusionBtn.gameObject.SetActive(false);
                    if (relieveBtn != null) relieveBtn.gameObject.SetActive(true);
                }
            } 
        }
        if (CheckIfHaveMount())
        {
            if (curMount != null)
            {
                int id = curMount.mountId; 
                if (mountInfoDic.ContainsKey(id))
                {
                    MountInfo info = mountInfoDic[id] as MountInfo; 
                    if (info.SkinRemainTime != 0 && timer != null)//限时
                    {
                        timer.StartIntervalTimer(info.SkinRemainTime);
                        timer.onTimeOut = (x) =>
                        {
                            if (timeLab != null) timeLab.text = ConfigMng.Instance.GetUItext(84);
                        };
                    }
                    else//这个幻兽是永久拥有的
                    {
                        if (timeLab != null) timeLab.gameObject.SetActive(false);
                    }
                } 
            } 
        }
        else
        {
            if (levLab != null) levLab.text = "Lv:0";
            if (expSli != null) expSli.value = 0;
            if (expLab != null) expLab.text = 0 + "/" + ((ConfigMng.Instance.GetSkinPropertyRef(1) != null) ? ConfigMng.Instance.GetSkinPropertyRef(1).exp : 0);
            if (timeLab != null) timeLab.text = ConfigMng.Instance.GetUItext(84);
            if (illusionBtn != null) illusionBtn.gameObject.SetActive(true);
            if (relieveBtn != null) relieveBtn.gameObject.SetActive(false); 
        }
    }
    //判断玩家是否有幻兽
    bool CheckIfHaveMount()
    { 
        SkinPropertyRef curPro = null;
        SkinPropertyRef nextInfo = null;
        int lev = GameCenter.newMountMng.curSkinLev;
        int exp = GameCenter.newMountMng.skinExp;
        if (lev >= ConfigMng.Instance.SkinPropertyRefTable.Count)
        {
            lev = ConfigMng.Instance.SkinPropertyRefTable.Count;
            curPro = ConfigMng.Instance.GetSkinPropertyRef(lev-1);
            nextInfo = curPro;
            exp = nextInfo.exp;
        }
        if (mountInfoDic.Count > 0)
        {
            if (levLab != null) levLab.text = "Lv:" + GameCenter.newMountMng.curSkinLev.ToString();
            if(curPro == null)curPro = ConfigMng.Instance.GetSkinPropertyRef(lev);
            if (curPro != null)
            { 
                if (curPropertyLab.Count > 0)
                {
                    for (int i = 0; i < curPropertyLab.Count; i++)
                    {
                        curPropertyLab[i].text = curPro.attr[i].ToString();
                    }
                }
            } 
            //下级等级属性
            if (nextLevAddLab.Count > 0)
            { 
                nextInfo = ConfigMng.Instance.GetSkinPropertyRef(lev + 1); 
                if (nextInfo != null)
                {
                    if (expSli != null) expSli.value = (float)exp / nextInfo.exp;
                    if (expLab != null) expLab.text = exp + "/" + nextInfo.exp;
                    for (int i = 0; i < nextLevAddLab.Count; i++)
                    {
                        if (curPro != null) nextLevAddLab[i].text = (nextInfo.attr[i] - curPro.attr[i]).ToString();
                        else
                            nextLevAddLab[i].text = (nextInfo.attr[i]).ToString();
                    }
                } 
            }
            //Debug.Log("判断玩家是否有幻兽 curSkinLev :   " + GameCenter.newMountMng.curSkinLev + " , skinExp : " + GameCenter.newMountMng.skinExp +
            //    "  , SkinPropertyRefTable : " + ConfigMng.Instance.SkinPropertyRefTable.Count + "  ,nextInfoid : " + nextInfo.level + "  , exp : " + nextInfo.exp);
            return true;
        }
        return false;
    } 
    //每页展示四个
    void ShowSkin()
    {
        foreach (SkinItem skin in skinItemList.Values)
        {
            skin.gameObject.SetActive(false);
        } 
        for (int i = 0, max = mountSkinList.Count; i < max; i++)
        {
            int id = mountSkinList[i].mountId;
            if (!skinItemList.ContainsKey(id))
            {
                if (parent != null)
                {
                    SkinItem item = SkinItem.CeateNew(i, id, parent);
                    if (item != null)
                    { 
                        item.gameObject.SetActive(true);
                        item.transform.localPosition = new Vector3((-236 + (i / 9) * 428.6f) + (i % 3) * 139, 117 - ((i - (i / 9) * 9) / 3) * 125); 
                        item.MountRefDate = ConfigMng.Instance.GetMountRef(id);
                        skinItemList[id] = item;
                        UIEventListener.Get(item.gameObject).onClick -= OnChangeSkin;
                        UIEventListener.Get(item.gameObject).onClick += OnChangeSkin;
                        UIEventListener.Get(item.gameObject).parameter = id;  
                    }
                }
            }
            else
            {
                SkinItem skin = skinItemList[id] as SkinItem;
                skin.gameObject.SetActive(true);
                skin.transform.localPosition = new Vector3((-236 + (i / 9) * 428.6f) + (i % 3) * 139, 117 - ((i - (i / 9) * 9) / 3) * 125); 
                skin.MountRefDate = ConfigMng.Instance.GetMountRef(id);
                skinItemList[id] = skin;
                UIEventListener.Get(skin.gameObject).onClick -= OnChangeSkin;
                UIEventListener.Get(skin.gameObject).onClick += OnChangeSkin;
                UIEventListener.Get(skin.gameObject).parameter = id;   
            }
        } 
        ShowPageBtn();
    } 
    #endregion
}
