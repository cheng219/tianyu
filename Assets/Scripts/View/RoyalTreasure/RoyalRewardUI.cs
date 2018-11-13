//======================================================
//作者:鲁家旗
//日期:2017/1/20
//用途:宝箱领奖界面
//======================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class RoyalRewardUI : MonoBehaviour {
    public UISprite spIcon;
    public UILabel nameLabel;
    public GameObject oldItem;
    public UIButton okBtn;
    //public UIGrid itemGird;
    public GameObject[] openEffects;//打开宝箱特效
    protected List<EquipmentInfo> infoList = new List<EquipmentInfo>();
    public List<GameObject> items = new List<GameObject>();
    public GameObject fxAfterOpen;

    public UIButton sureBtn;
    public UIButton closeBtn;

    void Start()
    {
        if (closeBtn != null) UIEventListener.Get(closeBtn.gameObject).onClick = delegate
            {
                this.gameObject.SetActive(false);
            };
        if (sureBtn != null) UIEventListener.Get(sureBtn.gameObject).onClick = delegate { 
            this.gameObject.SetActive(false);
            if (GameCenter.openServerRewardMng.wdfTaroatData != null)
            {
                if (GameCenter.openServerRewardMng.wdfTaroatData.activeCount <= 0)
                {
                    GameCenter.openServerRewardMng.CloseTaroatActive();
                }
            }
        };
        if (okBtn != null) UIEventListener.Get(okBtn.gameObject).onClick = delegate
        {
            if (GameCenter.royalTreasureMng.OnGetRewardOver != null)
            {
                if (GameCenter.royalTreasureMng.curGetRewardBoxData != null)
                {
                    GameCenter.royalTreasureMng.curGetRewardBoxData = null;
                }
                GameCenter.royalTreasureMng.OnGetRewardOver();
                //ReleaseGrid();
            }
        };
    }

    void OnDisable()
    {
        ReleaseGrid();
    }

    void IniteUi()
    {
        ReleaseGrid();
        if (spIcon != null) spIcon.gameObject.SetActive(false);
        if (fxAfterOpen != null) fxAfterOpen.SetActive(false);
        //if (itemGird != null) itemGird.gameObject.SetActive(false);
        if (okBtn != null) okBtn.gameObject.SetActive(false);
        if (nameLabel != null) nameLabel.gameObject.SetActive(false);
        for (int i = 0, max = openEffects.Length; i < max; i++)
        {
            openEffects[i].SetActive(false);
        }
        //if (oldItem != null) oldItem.SetActive(false); 
    }

    public void CreateRewardItem(List<EquipmentInfo> _list, RoyalTreaureData _data = null)
    {  
        infoList = _list; 
        if (_data == null)
        {
            IniteUi(); 
            ShowItem(); 
        }
        else
        {
            CancelInvoke("ShowItem");
            IniteUi(); 
            RoyalBoxRef refData = ConfigMng.Instance.GetRoyalBoxRef(_data.ItemID);
            if (refData != null)
            {
                //if (spIcon != null) spIcon.spriteName = refData.haveOpenIcon;
                if (nameLabel != null)
                {
                    EquipmentInfo info = _data.RoyalTreasueInfo;
                    if (info != null)
                    {
                        nameLabel.text = info.ItemStrColor + info.ItemName;
                    }
                }
                for (int i = 0, max = openEffects.Length; i < max; i++)
                {
                    if (refData.effect == openEffects[i].name)
                    {
                        //if (spIcon != null) spIcon.gameObject.SetActive(false);
                        NsEffectManager.RunReplayEffect(openEffects[i], true);
                        Invoke("ShowItem", 1.2f);
                        break;
                    }
                }
            }
        }
    }


    void ShowItem()
    {
        if (fxAfterOpen != null)
        {
            NsEffectManager.RunReplayEffect(fxAfterOpen, true);
            switch (infoList.Count)
            {
                case 1:
                    fxAfterOpen.transform.localPosition = new Vector3(156,30,0);
                    break;
                case 2:
                    fxAfterOpen.transform.localPosition = new Vector3(124,30,0);
                    break;
                case 3:
                    fxAfterOpen.transform.localPosition = new Vector3(75,30,0);
                    break;
                case 4:
                    fxAfterOpen.transform.localPosition = new Vector3(55,30,0);
                    break;
                case 5:
                    fxAfterOpen.transform.localPosition = new Vector3(0,30,0);
                    break;
                default:
                    fxAfterOpen.transform.localPosition = new Vector3(0,56,0);
                    break;
            }
        }
        if (okBtn != null) okBtn.gameObject.SetActive(true);
        if (nameLabel != null) nameLabel.gameObject.SetActive(true); 
        for (int i = 0 , max = items.Count; i < max; i++)
        {
            if (infoList.Count > i)
            {
                items[i].SetActive(true);
                ItemUI itemUI = ItemUI.CreatNew(items[i].transform, Vector3.zero, Vector3.one * 0.5f);  
                if (itemUI != null)
                    itemUI.FillInfo(infoList[i]); 
            }
            else
            {
                items[i].SetActive(false);
            }
        } 
    }

    void ShowFxAfterOpen()
    {
        if (fxAfterOpen != null) NsEffectManager.RunReplayEffect(fxAfterOpen, true);
    }

    public void ReleaseGrid()
    { 
        for (int i = 0, max = items.Count; i < max; i++)
        {
            items[i].SetActive(false);
            if (items[i].GetComponentInChildren<ItemUI>() != null)
                Destroy(items[i].GetComponentInChildren<ItemUI>().gameObject);
        }
    }
}
