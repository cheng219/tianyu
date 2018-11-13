using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class TreasureSuperItemUI : MonoBehaviour {
    #region UI控件
    //public UILabel needOpenTimes;
    public UILabel curOpenTimes;
    public ItemUIContainer rewardItems;
    public UIButton btnReturnPreviewWnd;
    public UIButton btnGetReward;
    public UISprite alreadyGet;
    private uint rewardKey;
    public UILabel des;
    #endregion
    #region unity函数
    void Awake()
    {
        if (btnGetReward != null)
            UIEventListener.Get(btnGetReward.gameObject).onClick = GetReward;
    }
    void Start () {
	
	}
    #endregion
    #region 界面的刷新与显示
    public static TreasureSuperItemUI CreateNew(Transform _parent,GameObject _item)
    {
        GameObject go = Instantiate(_item.gameObject);
        if (go != null)
        {
            go.transform.parent = _parent;
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            TreasureSuperItemUI itemUI = go.GetComponent<TreasureSuperItemUI>();
            return itemUI;
        }
        return null;
    }
    public void refreshAll(int _needTimes,int _curTimes,int _state,uint _id,List<ItemValue> _list,string _des)
    { 
        rewardKey = _id;
        //if (needOpenTimes != null)
        //    needOpenTimes.text = _needTimes.ToString();
        if (alreadyGet != null)
            alreadyGet.gameObject.SetActive(false);
        if (des != null)
            des.text = _des;
        if(rewardItems!=null)
            rewardItems.RefreshItems(_list,_list.Count, _list.Count);
        if (btnReturnPreviewWnd != null) btnReturnPreviewWnd.gameObject.SetActive(_needTimes > _curTimes ? true : false);
         
        if (_state == 0)
        {
            if (btnGetReward != null) btnGetReward.gameObject.SetActive(!btnReturnPreviewWnd.gameObject.activeSelf);
            if (curOpenTimes != null)
            {
                curOpenTimes.transform.parent.gameObject.SetActive(true);
                curOpenTimes.text = _curTimes.ToString() + "/" + _needTimes.ToString();
                if (_needTimes > _curTimes)
                {
                    curOpenTimes.text = "[ff0000]" + curOpenTimes.text;
                }
                else
                {
                    curOpenTimes.text = "[14E615FF]" + curOpenTimes.text;
                }
            }
        }
        if(_state==1)
        {
            if(alreadyGet!=null)alreadyGet.gameObject.SetActive(true);
            if(btnReturnPreviewWnd!=null)btnReturnPreviewWnd.gameObject.SetActive(false);
            if(btnGetReward!=null)btnGetReward.gameObject.SetActive(false);
            if (curOpenTimes != null)curOpenTimes.transform.parent.gameObject.SetActive(false);
        }
    }
    #endregion
    #region UI控件的响应
    void GetReward(GameObject _obj)
    {
        if(rewardKey!=0)
        GameCenter.treasureTroveMng.C2S_ReqTreasurebigPrizeReward(rewardKey);
    }
    #endregion
}
