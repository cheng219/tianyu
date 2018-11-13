//==================================
//作者：朱素云
//日期：2016/5/6
//用途：结义ui
//=================================
using UnityEngine;
using System.Collections;
using st.net.NetBase;

public class SwornInfoUi : MonoBehaviour {

    public UISpriteEx iconSp;

    public UILabel nameLab;
    public UILabel levLab; 
    /// <summary>
    /// 分道扬镳 结义
    /// </summary>
    public GameObject breakOutGo;
    public UIButton brokeOffBtn;
    public UIButton closeOutBtn;
    public UIButton sureOutBtn;
    public UIButton cancleBtn;
    /// <summary>
    /// 赠送美酒 结义
    /// </summary>
    public GameObject sendWineGo;
    public UIButton sendWine;
    public ItemUI wine;
    public UIButton sureBtn;
    public UIToggle noRemindTog;
    public UIButton closeWineBtn;
    private bool noRemind = false;
    /// <summary>
    /// 情义值 结义
    /// </summary>
    public UILabel friendShipLab;

    protected brothers_list brother = null;
    public brothers_list Brother
    {
        get
        {
            return brother;
        }
        set
        {
            if (value != null) brother = value; 
            Show();
        }
    }

    void OnDisable()
    {
        if (breakOutGo != null) breakOutGo.SetActive(false);
        if (sendWineGo != null) sendWineGo.SetActive(false);
    }

    void Show()
    { 
        if (brokeOffBtn != null) UIEventListener.Get(brokeOffBtn.gameObject).onClick = delegate
            {
                if (breakOutGo != null) breakOutGo.SetActive(true);
                if (closeOutBtn != null) UIEventListener.Get(closeOutBtn.gameObject).onClick = delegate { breakOutGo.SetActive(false); };
                if (cancleBtn != null) UIEventListener.Get(cancleBtn.gameObject).onClick = delegate { breakOutGo.SetActive(false); };
                if (sureOutBtn != null) UIEventListener.Get(sureOutBtn.gameObject).onClick = delegate 
                { GameCenter.swornMng.C2S_ReqBrokeUp();breakOutGo.SetActive(false); };
            };
        if (sendWine != null) UIEventListener.Get(sendWine.gameObject).onClick = delegate
        {
            if (noRemindTog != null)
            { 
                noRemind = noRemindTog.value;
            }
            if (noRemind)
            {
                GameCenter.swornMng.C2S_ReqSendWine(brother.uid);
            }
            else
            {
                if (sendWineGo != null)
                {
                    sendWineGo.SetActive(true);
                    wine.FillInfo(new EquipmentInfo(2600025, EquipmentBelongTo.PREVIEW));
                    if (closeWineBtn != null) UIEventListener.Get(closeWineBtn.gameObject).onClick = delegate { sendWineGo.SetActive(false); };
                    if (sureBtn != null) UIEventListener.Get(sureBtn.gameObject).onClick = delegate
                        {
                            GameCenter.swornMng.C2S_ReqSendWine(brother.uid);
                            sendWineGo.SetActive(false);
                        };
                }
            }
        };  
        if (friendShipLab != null) friendShipLab.text = brother.friend_ship.ToString();
        if (nameLab != null) nameLab.text = brother.name; 
        levLab.text = ConfigMng.Instance.GetLevelDes(brother.lev); 
        if (iconSp != null)
        {
            iconSp.spriteName = ConfigMng.Instance.GetPlayerConfig(brother.prof).res_head_Icon;
            if (brother.oline_state == 1)
                iconSp.IsGray = UISpriteEx.ColorGray.normal;
            else
                iconSp.IsGray = UISpriteEx.ColorGray.Gray; //头像置灰
        }
    }

}
