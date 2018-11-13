//==================================
//作者：朱素云
//日期：2016/4/14
//用途：鲜花类
//=================================
using UnityEngine;
using System.Collections;

public class Flower : MonoBehaviour  
{
    public ItemUI flowerItem;
    public UILabel countLab;
    public UIButton sendBtn;

    protected FlowerData flowerData = null;
    public FlowerData FlowerData 
    {
        get
        {
            return flowerData;
        }
        set
        {
            if (value != null) flowerData = value; 
            Show();
        }
    }

    void Awake()
    {
        if (sendBtn != null) UIEventListener.Get(sendBtn.gameObject).onClick = OnClickSend;
    }

    void Show()
    {
        if (flowerData != null)
        {
            flowerItem.FillInfo(new EquipmentInfo(flowerData.flowerId, EquipmentBelongTo.PREVIEW));
            countLab.text = GameCenter.inventoryMng.GetNumberByType(flowerData.flowerId).ToString(); 
        }
    }

    void OnClickSend(GameObject go)
    {
        if (flowerData == null) return;
        EquipmentRef flower = ConfigMng.Instance.GetEquipmentRef(flowerData.flowerId);
        if (flower == null) return;
        if (GameCenter.inventoryMng.GetNumberByType(flowerData.flowerId) > 0)
        {
            GameCenter.friendsMng.C2S_SendFlower(flowerData.flowerId, flowerData.someOneId, flowerData.flowerType);
        }
        else
        {
            if (!GameCenter.systemSettingMng.ShowBuyFlower)
            {
                if (GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount < flower.diamonPrice)
                {
                    MessageST mst1 = new MessageST();
                    mst1.messID = 137;
                    mst1.delYes = delegate
                    {
                        GameCenter.uIMng.SwitchToUI(GUIType.RECHARGE);
                    };
                    GameCenter.messageMng.AddClientMsg(mst1);
                }
                else
                {
                    GameCenter.friendsMng.C2S_SendFlower(flowerData.flowerId, flowerData.someOneId, flowerData.flowerType);
                }
            }
            else
            {
                MessageST mst = new MessageST();
                mst.messID = 489;
                if (flower != null) mst.words = new string[2] { (flower.diamonPrice).ToString(), flower.name };
                object[] pa = { 1 };
                mst.pars = pa;
                mst.delPars = delegate(object[] ob)
                {
                    if (ob.Length > 0)
                    {
                        bool b = (bool)ob[0];
                        if (b)
                            GameCenter.systemSettingMng.ShowBuyFlower = false;
                    } 
                };
                mst.delYes = delegate
                {
                    if (GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount < flower.diamonPrice)
                    {
                        MessageST mst1 = new MessageST();
                        mst1.messID = 137;
                        mst1.delYes = delegate
                        {
                            GameCenter.uIMng.SwitchToUI(GUIType.RECHARGE);
                        };
                        GameCenter.messageMng.AddClientMsg(mst1);
                    }
                    else
                    {
                        GameCenter.friendsMng.C2S_SendFlower(flowerData.flowerId, flowerData.someOneId, flowerData.flowerType);
                    }
                };
                GameCenter.messageMng.AddClientMsg(mst);
            }
        }
    }
}

public class FlowerData
{
    /// <summary>
    /// 送花的花id
    /// </summary>
    public int flowerId;
    /// <summary>
    /// 把花送给的人的id
    /// </summary>
    public int someOneId;
    /// <summary>
    /// 送花类型
    /// </summary>
    public int flowerType;
    public FlowerData()
    { 
        
    }
    /// <summary>
    /// 构造鲜花数据
    /// </summary>
    /// <param name="_flowerId">送花的花id</param>
    /// <param name="_someOneId">把花送给的人的id</param>
    public FlowerData(int _flowerId, int _someOneId, int _type)
    {
        this.flowerId = _flowerId;
        this.someOneId = _someOneId;
        this.flowerType = _type;
    }
}
