//==================================
//作者：邓成
//日期：2017/4/6
//用途：宝藏活动至尊豪礼界面类
//=================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TreasureSuperWnd : SubWnd
{
    public TreasureSuperItemUI superItem;
    public UITimer timer;
    public UIScrollView scrollView;
    public UIGrid grid;
    //protected List<TreasureSuperItemUI> superItems = new List<TreasureSuperItemUI>();
    private List<ItemValue> itemList = new List<ItemValue>();
    protected Dictionary<int, TreasureSuperItemUI> superItems = new Dictionary<int, TreasureSuperItemUI>();
    private List<string> desList = new List<string>();
    protected override void OnOpen()
    {
        base.OnOpen();
        GameCenter.treasureTroveMng.C2S_ReqTreasurebigPrize();
    }
    protected override void OnClose()
    {
        base.OnClose();
    }
    protected override void HandEvent(bool _bind)
    {
        if (_bind)
        {
            GameCenter.treasureTroveMng.updateTreasurebigPrize += RefreshSuperWnd;

        }
        else
        {
            GameCenter.treasureTroveMng.updateTreasurebigPrize -= RefreshSuperWnd;
        }
    }
    void RefreshSuperWnd()
    {
        //Debug.Log("RefreshSuperWnd()");
        if (timer != null)
        {
            timer.StartIntervalTimer(GameCenter.treasureTroveMng.Time);
        }
        List<st.net.NetBase.treasure_times_reward> list = GameCenter.treasureTroveMng.GetRewardInfo();
        if (list != null && grid != null && superItem != null&& desList!=null)
        {
            desList.Clear();
           
            for (int i = 0, len = list.Count; i < len; i++)
            {
                if (!superItems.ContainsKey((int)list[i].id))
                {
                    TreasureSuperItemUI item = TreasureSuperItemUI.CreateNew(grid.transform, superItem.gameObject);
                    if (item != null)
                    {
                        superItems[(int)list[i].id] = item;
                    }
                }
                itemList.Clear(); 

                for (int j = 0, length = list[i].reward_info.Count; j < length; j++)
                {
                    itemList.Add(new ItemValue((int)list[i].reward_info[j].item_id, (int)list[i].reward_info[j].item_num));
                }
                desList.Add(list[i].desc);  

                superItems[(int)list[i].id].refreshAll((int)list[i].need_times, GameCenter.treasureTroveMng.Times, list[i].status, list[i].id, itemList, desList[i]);
                 
            }
        }
        //for (int k = 0,num = list.Count; k < num; k++)
        //{
        //    //Debug.Log("list.Count:"+ list.Count);
        //    //Debug.Log("desList:"+ desList.Count);
            

        //}
        if (superItem != null)
            superItem.gameObject.SetActive(false);
        if (grid != null)
            grid.Reposition();
    }
}
