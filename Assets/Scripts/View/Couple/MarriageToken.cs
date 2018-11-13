//==================================
//作者：朱素云
//日期：2016/5/10
//用途：婚礼信物UI
//=================================
using UnityEngine;
using System.Collections;

public class MarriageToken : MonoBehaviour
{
    public UISprite weedingSp;
    public UILabel tokenNameLab;
    public UILabel costLab;
    public ItemUI token; 
    protected WeddingRef weddingData = null;
    public WeddingRef WeddingData
    {
        get
        {
            return weddingData;
        }
        set
        {
            if (value != null) weddingData = value;
            Show();
        }
    }

    void Show()
    {
        if (WeddingData != null)
        {
            weedingSp.spriteName = WeddingData.name;
            token.FillInfo(new EquipmentInfo(WeddingData.token_id, EquipmentBelongTo.PREVIEW)); 
            tokenNameLab.text = ConfigMng.Instance.GetEquipmentRef(WeddingData.token_id) != null ? 
                ConfigMng.Instance.GetEquipmentRef(WeddingData.token_id).name : "";
            costLab.text = WeddingData.consume_num + (ConfigMng.Instance.GetEquipmentRef(WeddingData.consume) != null ? 
                ConfigMng.Instance.GetEquipmentRef(WeddingData.consume).name : "");
        }
    }
} 