//==================================
//作者：黄洪兴
//日期：2016/5/12
//用途：复活界面类
//=================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResurrectionWnd : GUIBase
{  
    public GameObject Model1;
    public UILabel Model1Time;
    public ItemUI Model1Item;
    public GameObject Model1StandingBtn;
    public UILabel Model1BtnLabel;


    public GameObject Model2;
    public ItemUI Mode2Item;
    public UILabel Model2Name;
    public UILabel Model2ItemName;
    public UILabel Model2ItemNun;
    public GameObject Model2standingBtn;
    public GameObject Model2SafeBtn;
    public UILabel Model2Time;
    public UILabel Model2BtnLabel;


    public GameObject Model3;
    public UILabel Model3Time;
    public UILabel Model3Name;
    public ItemUI Model3Item;
    public GameObject Model3StandingBtn;
    public UILabel Model3BtnLabel;
   


    public GameObject Model4;
    public UILabel Model4Time;
    public GameObject Model4StandingBtn;
    public UILabel Model4TipLab;//当rebon_type = 7去掉括号里面的提示

	public GameObject Model5;
	public UILabel Model5Time;
	public ItemUI Model5Item;
	public GameObject Model5StandingBtn;
	public UILabel Model5BtnLabel;

    public GameObject strengthenBtn;

    bool canFree = false;

    //MainPlayerInfo mainPlayerInfo = null;

    void Awake()
    {
        mutualExclusion = true;
    }
    protected override void OnOpen()
    {
        base.OnOpen();
        if (Model1!=null) Model1.SetActive(false);
        if (Model2 != null) Model2.SetActive(false);
        if (Model3 != null) Model3.SetActive(false);
        if (Model4 != null) Model4.SetActive(false);
		if (Model5 != null) Model4.SetActive(false);
        OpenWndByType(GameCenter.resurrectionMng.ResurrectionType);
        if (Model1StandingBtn != null) UIEventListener.Get(Model1StandingBtn).onClick += AskResurrection2;
        if (Model2standingBtn != null) UIEventListener.Get(Model2standingBtn).onClick += AskResurrection2;
        if (Model3StandingBtn != null) UIEventListener.Get(Model3StandingBtn).onClick += AskResurrection2;
        if (Model4StandingBtn != null) UIEventListener.Get(Model4StandingBtn).onClick += AskResurrection3;
		if (Model5StandingBtn != null) UIEventListener.Get(Model5StandingBtn).onClick += AskResurrection2;
        if (Model2SafeBtn != null) UIEventListener.Get(Model2SafeBtn).onClick += AskResurrection1;
        if (strengthenBtn != null) UIEventListener.Get(strengthenBtn).onClick += AskstrengthenBtn;
        GameCenter.rebornMng.OnRebornEvent += CloseThisUI;
    }
    protected override void OnClose()
    {
        base.OnClose();
        if (Model1StandingBtn != null) UIEventListener.Get(Model1StandingBtn).onClick -= AskResurrection2;
        if (Model2standingBtn != null) UIEventListener.Get(Model2standingBtn).onClick -= AskResurrection2;
        if (Model3StandingBtn != null) UIEventListener.Get(Model3StandingBtn).onClick -= AskResurrection2;
        if (Model4StandingBtn != null) UIEventListener.Get(Model4StandingBtn).onClick -= AskResurrection3;
		if (Model5StandingBtn != null) UIEventListener.Get(Model5StandingBtn).onClick -= AskResurrection2;
        if (Model2SafeBtn != null) UIEventListener.Get(Model2SafeBtn).onClick -= AskResurrection1;
        if (strengthenBtn != null) UIEventListener.Get(strengthenBtn).onClick -= AskstrengthenBtn;
        GameCenter.rebornMng.OnRebornEvent -= CloseThisUI;
		GameCenter.resurrectionMng.HeavenDead = false;
    }
    protected override void HandEvent(bool _bind)
    {
        base.HandEvent(_bind);
        if (_bind)
        {
          
        }
        else
        {
       
        }
    }
    void OpenWndByType(int type)
    {
        canFree=false;
        int reliveTime = GameCenter.resurrectionMng.DieNum;
        canFree = reliveTime > 0;
        //VIPRef Ref = ConfigMng.Instance.GetVIPRef(GameCenter.mainPlayerMng.VipData.vLev);
        //if (Ref != null)
        //{
        //    reliveTime = Ref.reliveTime - GameCenter.resurrectionMng.DieNum;
        //    canFree = reliveTime > 0;
        //}
        string a = ConfigMng.Instance.GetUITextRef(66).text;
        string[] w= { reliveTime.ToString() };
        string b = ConfigMng.Instance.GetUItext(67, w);
        InvokeRepeating("RefreshTime", 0.0f, 1.0f);
		if(GameCenter.resurrectionMng.HeavenDead == false)
		{
	        if(type==1)
	        {
	            if (Model1!=null) Model1.SetActive(true);
	            if (Model1Item!=null) Model1Item.FillInfo(new EquipmentInfo(2600020, EquipmentBelongTo.PREVIEW));
	            if (Model1BtnLabel != null)
	            {
	                Model1BtnLabel.text = canFree ? b : a;
	            }
	            Model1Item.gameObject.SetActive(!canFree);


	        }
	       else if (type == 2)
	        {
	            if (Model2!=null) Model2.SetActive(true);
	            if (Mode2Item!=null) Mode2Item.FillInfo(new EquipmentInfo(2600020, EquipmentBelongTo.PREVIEW));
	            if (Model2Name!=null) Model2Name.text = GameCenter.resurrectionMng.ResurrectionInfo.kill_name;
	            if (Model2ItemName!=null) Model2ItemName.text = new EquipmentInfo(GameCenter.resurrectionMng.ResurrectionInfo.drop_item, EquipmentBelongTo.PREVIEW).ItemName;
	            if (Model2ItemNun!=null) Model2ItemNun.text = "1";
	            if (Model2BtnLabel != null)
	            {
	                Model2BtnLabel.text = canFree ? b : a;
	            }
	            Mode2Item.gameObject.SetActive(!canFree);
	        }
	        else if (type == 3)
	        {
	            if (Model3!=null) Model3.SetActive(true);
	            if (Model3Item!=null) Model3Item.FillInfo(new EquipmentInfo(2600020, EquipmentBelongTo.PREVIEW));
	            if (Model3Name != null)
	            {
	                string[] word={GameCenter.resurrectionMng.ResurrectionInfo.kill_name};
	                string st = ConfigMng.Instance.GetUItext(49,word);
	                if(st!=null)
	                Model3Name.text = st;
	            }
	            if (Model3BtnLabel != null)
	            {
	                Model3BtnLabel.text = canFree ? b : a;
	            }
	            Model3Item.gameObject.SetActive(!canFree);

	        }
	        else if (type == 4)
	        {
	            if (Model4!=null) Model4.SetActive(true);
                if (Model4TipLab != null) Model4TipLab.gameObject.SetActive(true);

	        }
            else if (type == 7)
            {
                if (Model4 != null) Model4.SetActive(true);
                if (Model4TipLab != null) Model4TipLab.gameObject.SetActive(false);
            }
            else
            {
                return;
            }
		}else
		{
			if (Model5!=null) Model5.SetActive(true);
			if (Model5Item!=null) Model5Item.FillInfo(new EquipmentInfo(2600020, EquipmentBelongTo.PREVIEW));
			if (Model5BtnLabel != null)
			{
				Model5BtnLabel.text = canFree ? b : a;
			}
			Model5Item.gameObject.SetActive(!canFree);
		}
    }



    void RefreshTime()
    {
		if(GameCenter.resurrectionMng.HeavenDead == false)
		{
	        if (GameCenter.resurrectionMng.ResurrectionType == 1)
	        {
	            int i = (int)(GameCenter.resurrectionMng.ReviveTime - (Time.time - GameCenter.resurrectionMng.GotTime));
	            if (i < 0)
	            {
	                i = 0;
	            }
	            if (Model1Time!=null) Model1Time.text = i.ToString();
	        }
	        if (GameCenter.resurrectionMng.ResurrectionType == 2)
	        {
	            int i = (int)(GameCenter.resurrectionMng.ReviveTime - (Time.time - GameCenter.resurrectionMng.GotTime));
	            if (i < 0)
	            {
	                i = 0;
	            }
	            if (Model2Time!=null) Model2Time.text = i.ToString();
	        }
	        if (GameCenter.resurrectionMng.ResurrectionType == 3)
	        {
	            int i = (int)(GameCenter.resurrectionMng.ReviveTime - (Time.time - GameCenter.resurrectionMng.GotTime));
	            if (i < 0)
	            {
	                i = 0;
	            }
	            if (Model3Time!=null) Model3Time.text = i.ToString();
	        }
	        if (GameCenter.resurrectionMng.ResurrectionType == 4)
	        {
	            int i = (int)(GameCenter.resurrectionMng.ReviveTime - (Time.time - GameCenter.resurrectionMng.GotTime));
	            if (i < 0)
	            {
	                i = 0;
	            }
	            if (Model4Time!=null) Model4Time.text = i.ToString();
	        }
		}else
		{
			int i = (int)(GameCenter.resurrectionMng.ReviveTime - (Time.time - GameCenter.resurrectionMng.GotTime));
			if (i < 0)
			{
				i = 0;
			}
			if (Model5Time!=null) Model5Time.text = i.ToString();
		}
    }

    void AskstrengthenBtn(GameObject go)
    {
        GameCenter.littleHelperMng.OpenWndByType(LittleHelpType.STRONG);
    }




    void AskResurrection1(GameObject go)
    {
        GameCenter.resurrectionMng.C2S_AskResurrection(1,0);

    }
    void AskResurrection2(GameObject go)
    {
        if (GameCenter.inventoryMng.GetNumberByType(2600020) > 0||canFree)
        {
            GameCenter.resurrectionMng.C2S_AskResurrection(2, 0);
        }
        else
        {

            MessageST mst = new MessageST();
            mst.messID = 217;
            mst.words = new string[3] { ConfigMng.Instance.GetEquipmentRef(2600020).diamonPrice.ToString(), "1", ConfigMng.Instance.GetEquipmentRef(2600020).name };
            mst.delYes = delegate
            {
                if (GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount < 5)
                {
                    MessageST message = new MessageST();
                    message.messID = 210;
                    message.delYes = delegate
                    {
                        //TODO 充值
                        GameCenter.uIMng.SwitchToUI(GUIType.NONE);
                        GameCenter.uIMng.SwitchToUI(GUIType.VIP);
                    };
                    GameCenter.messageMng.AddClientMsg(message);
                }
                else
                {
                    GameCenter.resurrectionMng.C2S_AskResurrection(2, 1);
                }
            };
            GameCenter.messageMng.AddClientMsg(mst);

        }


        
    }
    void AskResurrection3(GameObject go)
    {
        GameCenter.resurrectionMng.C2S_AskResurrection(3,0);

    }

    void CloseThisUI(int _i)
    {
        GameCenter.uIMng.SwitchToUI(GUIType.NONE);
    }
}
