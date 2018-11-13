using UnityEngine;
using System.Collections;

public class RewardRecord : MonoBehaviour {

    #region UI 控件
    public UILabel playerName;
    public UILabel label;
    public UILabel inventoryName;
    #endregion

    #region 填充
    public void FillInfo(string _name, string _inventoryName,int[] _array)
    {
        //Debug.Log("_name:= " + _name);
        if (playerName != null)
        {
            //UIButton nameBtn = playerName.GetComponent<UIButton>();
            //UIEventListener.Get(nameBtn.gameObject).onClick -= OnClickName;
            //UIEventListener.Get(nameBtn.gameObject).onClick += OnClickName;
            //UIEventListener.Get(nameBtn.gameObject).parameter = _array[0];
            playerName.text = _name;
            //Debug.Log(" playerName.text: =" + playerName.text);
            label.text = ConfigMng.Instance.GetUItext(22);
            inventoryName.text = _inventoryName;
        }
        else
        {
            Debug.LogError("名为 playerName 的组件为空");
            playerName.text = "";
            label.text = "";
            inventoryName.text = "";
        }
       if(inventoryName!=null)
        {
            UIEventListener.Get(inventoryName.gameObject).onClick -= OnClickGoods;
            UIEventListener.Get(inventoryName.gameObject).onClick += OnClickGoods;
            UIEventListener.Get(inventoryName.gameObject).parameter = _array[1];
        }
        else
        {
            Debug.LogError("名为 inventoryName 的组件为空");
            playerName.text = "";
            label.text = "";
            inventoryName.text = "";
        }
        //Debug.Log(" playerName.text: =" + playerName.text);
    }
    #endregion
    #region 刷新
    public void RefreshUI(string _name,string _inventoryName)
    {
        playerName.text = "[u]" + name;
        label.text = ConfigMng.Instance.GetUItext(22);
        inventoryName.text = _inventoryName;
    }
    #endregion
    #region 控件事件
    /// <summary>
    /// 点击玩家名后
    /// </summary>
    void OnClickName(GameObject obj)
    {
        //Debug.Log("点击玩家名字");
        //int id = (int)UIEventListener.Get(obj).parameter;
        //GameCenter.previewManager.C2S_AskOPCPreview(GameCenter.treasureHouseMng.playerId[id]);
    }
    /// <summary>
    /// 点击抽到道具后
    /// </summary>
    void OnClickGoods(GameObject obj)
    {
        Debug.Log("点击抽取道具");
        int id = (int)UIEventListener.Get(obj).parameter;
        ToolTipMng.ShowEquipmentTooltip(new EquipmentInfo(id, EquipmentBelongTo.PREVIEW), ItemActionType.None, ItemActionType.None, ItemActionType.None, ItemActionType.None);
    }
    #endregion
}
