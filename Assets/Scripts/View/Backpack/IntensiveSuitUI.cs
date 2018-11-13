//==========================
//作者：鲁家旗
//日期：2016/6/2
//用途：强化套装属性
//==========================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class IntensiveSuitUI : MonoBehaviour {
    public UILabel levDes;
    public UILabel[] nowAttribute;
    public UILabel nextDes;
    public UILabel[] nextAttribute;
    
    protected int num = 0;
    protected int nextNum = 0;
    protected int id = 0;
    protected int nextID = 0;

    protected Dictionary<int, EquipmentInfo> playerEqu = new Dictionary<int, EquipmentInfo>();
    protected List<StrengthenSuitRef> strengSuitList = new List<StrengthenSuitRef>();
    protected List<int> levList = new List<int>();
    protected int smallLev = 0;
    protected string str = null;
    protected string nextStr = null;
    public UIButton btnStar;
    public GameObject nextGo;
    protected bool isMax = false;
    void Awake()
    {
        if(btnStar != null) UIEventListener.Get(btnStar.gameObject).onClick = delegate
        {
            playerEqu = GameCenter.inventoryMng.GetPlayerEquipList();
            strengSuitList = ConfigMng.Instance.GetStrengSuitRefList(1);
            EquNum();
            ShowDes();
        };
    }
    StrengthenSuitRef strengSuitRef(int _id)
    {
        return ConfigMng.Instance.GetStrengSuitRef(_id);
    }
    void ShowDes()
    {
        //身上有几个强化等级达到的装备数量
        if (num >= 12)
             str = "[00ff00]" + num.ToString() + "[-]";
        else
             str = "[ff0000]" + num.ToString() + "[-]";
        levDes.text = ConfigMng.Instance.GetUItext(32, new string[3] { "[00ff00]" + strengSuitRef(id).str_Lev.ToString() + "[-]", str, "[00ff00]" + 12 + "[-]" });
       
        for (int i = 0; i < nowAttribute.Length; i++)
        {
            nowAttribute[i].text = strengSuitRef(id).des[i];
            nextAttribute[i].text = strengSuitRef(nextID).des[i];
        }
        if (isMax)
            nextGo.SetActive(false);
        else
            nextGo.SetActive(true);
        if(nextNum >= 12)
             nextStr = "[00ff00]" + nextNum.ToString() + "[-]";
        else
             nextStr = "[ff0000]" + nextNum.ToString() + "[-]";
        nextDes.text = ConfigMng.Instance.GetUItext(32, new string[3] { "[00ff00]" + strengSuitRef(nextID).str_Lev.ToString() + "[-]", nextStr, "[00ff00]" + 12 + "[-]" });
    }
    void EquNum()
    {
        levList.Clear();
        foreach (EquipmentInfo info in playerEqu.Values)
        {
            levList.Add(info.UpgradeLv);
        }
        //冒泡排序找出最小等级
        for (int j = 1; j <= levList.Count - 1; j++)
        {
            for (int i = 0; i < levList.Count - j; i++)
            {
                if (levList[i] > levList[i + 1])
                {
                    int t = levList[i];
                    levList[i] = levList[i + 1];
                    levList[i + 1] = t;
                }
            }
        }
        if(levList.Count != 0)
            smallLev = levList[0];
        //然后将最小强化等级和配表比较，显示合适的2条数据
        for (int i = 0; i < strengSuitList.Count; i++)
        {
            if (smallLev < strengSuitList[0].str_Lev)
            {
                id = strengSuitList[0].id;
                nextID = strengSuitList[1].id;
                isMax = false;
            }
            else if (i + 1 < strengSuitList.Count && smallLev >= strengSuitList[i].str_Lev && smallLev < strengSuitList[i + 1].str_Lev)
            {
                id = strengSuitList[i].id;
                nextID = strengSuitList[i + 1].id;
                isMax = false;
            }
            else if (i + 1 == strengSuitList.Count && smallLev >= strengSuitList[i].str_Lev)
            {
                id = strengSuitList[i].id;
                nextID = strengSuitList[i].id;
                isMax = true;
            }
        }
        num = 0;
        nextNum = 0;
        foreach (EquipmentInfo info in playerEqu.Values)
        {
            if (info.UpgradeLv >= strengSuitRef(id).str_Lev)
            {
                num++;
            }
            if (info.UpgradeLv >= strengSuitRef(nextID).str_Lev)
            {
                nextNum++;
            }
        }
    }
}
