//========================
//作者:鲁家旗
//日期:2016.8.18
//用途:排行榜其它玩家界面
//========================
using UnityEngine;
using System.Collections;

public class RankCheckOpcWnd : MonoBehaviour {
    #region 外部控件
    /// <summary>
    /// 切换toggle
    /// </summary>
    public UIToggle[] toggles = new UIToggle[3];
    /// <summary>
    /// 关闭按钮
    /// </summary>
    public UIButton CloseButton;
    public GameObject[] go;
    protected UITexture modelTex;
    protected int rankType = 0;
    #endregion
    void OnEnable()
    {
        GameCenter.previewManager.OnGotRankOtherInfo -= RefreshPetInfo;
        GameCenter.previewManager.OnGotRankOtherInfo += RefreshPetInfo;
    }
    void OnDisable()
    {
        GameCenter.previewManager.OnGotRankOtherInfo -= RefreshPetInfo;
    }
    void RefreshPetInfo()
    {
        if (GameCenter.newRankingMng.CurOtherId != GameCenter.previewManager.CurAskPlayerInfo.ServerInstanceID) return;
        GameCenter.previewManager.C2S_AskOpcPetPreview(GameCenter.previewManager.CurAskPlayerInfo.ServerInstanceID);
    }
    void Start()
    {
        if (CloseButton != null) UIEventListener.Get(CloseButton.gameObject).onClick = OnClickClose;
        for (int i = 0; i < toggles.Length; i++)
        {
            if (toggles[i] != null) UIEventListener.Get(toggles[i].gameObject).onClick -= SetCurOpenWnd;
            if (toggles[i] != null) UIEventListener.Get(toggles[i].gameObject).onClick += SetCurOpenWnd;
        }
    }
    #region 控件事件
    /// <summary>
    /// 点击关闭按钮事件
    /// </summary>
    /// <param name="obj"></param>
    void OnClickClose(GameObject obj)
    {
        GameCenter.previewManager.ClearModel();
        RefreshModel();
        CancelInvoke("ActiveModel");
        Invoke("ActiveModel", 0.2f);
        this.gameObject.SetActive(false);
        //DestroyImmediate(this.gameObject);
    }
    void RefreshModel()
    {
        PlayerBaseInfo curRankPlayerInfo = GameCenter.newRankingMng.CurRankPlayerInfo;
        if (curRankPlayerInfo != null)
        {
            if (rankType == 2 && modelTex != null && curRankPlayerInfo.CurPetInfo != null)
            {
                GameCenter.previewManager.TryPreviewSingelEntourage(curRankPlayerInfo.CurPetInfo, modelTex);
            }
            else if (rankType == 3 && modelTex != null && curRankPlayerInfo.CurMountInfo != null)
            {
                GameCenter.previewManager.TryPreviewMount(curRankPlayerInfo.CurMountInfo, modelTex);
            }
            else if (modelTex != null)
            {
                GameCenter.previewManager.TryPreviewSinglePlayer(curRankPlayerInfo, modelTex);
            }
        }

    }
    void ActiveModel()
    {
        if (modelTex != null)
            modelTex.gameObject.SetActive(true);
    }
    /// <summary>
    /// 打开界面信息_type = 2(宠物) _type = 3(坐骑) _type = 1(人物)
    /// </summary>
    public void SetOtherInfo(int  _type,UITexture _tex)
    {
        modelTex = _tex;
        rankType = _type;
        //优化，去掉硬编码
        if (_type <= toggles.Length && toggles[_type - 1] != null)
            toggles[_type - 1].value = true;

        for (int i = 0; i < go.Length; i++)
        {
            if (go[i] != null)
                go[i].SetActive(i + 1 == _type);
        }
        //switch (_type)
        //{
        //    case 1:
        //        toggles[0].value = true;
        //        go[0].SetActive(true);
        //        go[1].SetActive(false);
        //        go[2].SetActive(false);
        //        break;
        //    case 2:
        //        toggles[1].value = true;
        //        go[0].SetActive(false);
        //        go[1].SetActive(true);
        //        go[2].SetActive(false);
        //        break;
        //    case 3:
        //        toggles[2].value = true;
        //        go[0].SetActive(false);
        //        go[1].SetActive(false);
        //        go[2].SetActive(true);
        //        break;
        //}
    }
    /// <summary>
    /// 设置当前打开的界面
    /// </summary>
    /// <param name="obj"></param>
    void SetCurOpenWnd(GameObject obj)
    {
        //优化，去掉硬编码
        for (int i = 0; i < go.Length; i++)
        {
            if (go[i] != null)
                go[i].SetActive(toggles[i].value);
        }
        //if (toggles[0].value)
        //{
        //    go[0].SetActive(true);
        //    go[1].SetActive(false);
        //    go[2].SetActive(false);
        //}
        //else if (toggles[1].value)
        //{
        //    go[0].SetActive(false);
        //    go[1].SetActive(true);
        //    go[2].SetActive(false);
        //}
        //else if (toggles[2].value)
        //{
        //    go[0].SetActive(false);
        //    go[1].SetActive(false);
        //    go[2].SetActive(true);
        //}
    }
    #endregion
}
