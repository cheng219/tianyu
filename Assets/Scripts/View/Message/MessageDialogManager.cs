//==================================
//作者：易睿
//日期：2015/12/2
//用途：错误提示界面类
//=================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MessageDialogManager :MonoBehaviour {

    public delegate void Finish(FloatTipObject tipObject);


    private const int Max = 5;
//    private const int startX=0,startY=80;
//    private static MessageDialogManager instance;
//    public static MessageDialogManager Instance
//    {
//        get
//        {
//            return instance;
//        }
//    }
    void Start()
    {
        otherTipParent = gameObject;
        if (temFloatObject == null)
        {
            Debug.LogWarning("MessageDialogManager not find temFloatObject !");
        }
    }

    void OnDestroy()
    {
//        instance = null;
        ClearAll();
    }



    private List<FloatTipObject> floatTipList = new List<FloatTipObject>();
    public FloatTipObject temFloatObject;
    public  GameObject floatTipParent;
    GameObject otherTipParent;
	//添加服务端浮动提示
	public void AddFloatTip(ErrorMsgStr mess)
    {
        AddFloatTip(mess.RefData);
    }
	//添加文本浮动提示
    public void AddFloatTip(string tip,bool showFrame=true)
    {
        if (temFloatObject == null)
        {
            Debug.LogWarning("MessageDialogManager  temFloatObject is null!");
            return;
        }
        FloatTipObject tem = null;


        tem = NGUITools.AddChild(otherTipParent, temFloatObject.gameObject).GetComponent<FloatTipObject>();
		tem.transform.localPosition = Vector3.zero;
		
        tem.gameObject.SetActive(true);
		
		ShowFloatTip(tem,tip,showFrame);
    }
	//上一个提示数据
//	MessageST messInfo = null;
	//间隔时间
	float time = 0;
	float values = 0;
	//添加客服端浮动提示
    public void AddFloatTip(MsgRefData mess,bool isEquInfo = false)
    {
        if (temFloatObject == null)
        {
            Debug.LogWarning("MessageDialogManager  temFloatObject is null!");
            return;
        }
		
		StartCoroutine(FloatTip(mess));
    }
	
	IEnumerator FloatTip(MsgRefData mess){
		bool isEquInfo = false;
//		float timeValue = 0;
		if(mess.sort == 12){
			//第一物品提示不用等待
			isEquInfo = true;
			if(values == 0){
				values ++ ;
			}else{
				time += mess.holdTime;
				values ++ ;
				yield return new WaitForSeconds(time);
			}
		}
		
		FloatTipObject tem = null;

        if (mess.sort == 13 || mess.sort == 14)
        {

            int a = floatTipParent.transform.childCount;
            if (a > 0)
            {
                for (int i = 0; i < a; i++)
                {
                    floatTipParent.transform.GetChild(i).DestroyChildren();
                }

            }
            if (floatTipParent != null && temFloatObject!=null) tem = NGUITools.AddChild(floatTipParent, temFloatObject.gameObject).GetComponent<FloatTipObject>();
        }
        else
        {
            if (otherTipParent != null && temFloatObject != null) tem = NGUITools.AddChild(otherTipParent, temFloatObject.gameObject).GetComponent<FloatTipObject>();
        }
        tem.transform.localPosition = mess.flowStartV3;
		if(mess.item != null){
			tem.equInfoItem = new EquipmentInfo(mess.item.eid,mess.item.count,EquipmentBelongTo.PREVIEW);
		}
		tem.Data = mess;
        tem.gameObject.SetActive(true);
		
		ShowFloatTip(tem,mess.messStr,mess.showBg,isEquInfo);
	}
	/// <summary>
	/// 显示该提示
	/// </summary>
	/// <param name='tem'>
	/// 显示体
	/// </param>
	/// <param name='tip'>
	/// 提示内容
	/// </param>
	/// <param name='showFrame'>
	/// 背景是否显示
	/// </param>
	/// <param name='isEquInfo'>
	/// 是否是物品提示，处理逻辑不同
	/// </param>
	void ShowFloatTip(FloatTipObject tem,string tip,bool showFrame=true,bool isEquInfo = false)
    { 
		tem.gameObject.SetActive(true);
		float height = NGUIMath.CalculateRelativeWidgetBounds(tem.transform, tem.transform).size.y;
		
		if (floatTipList.Count > 0 && !isEquInfo)
        {
			for (int i=0;i< floatTipList.Count;i++)
            {
				FloatTipObject t = floatTipList[i];
                StartFloatOne(t,  height + 5);
            }
        }
		
		tem.onFinish = Onfinish;
        if(tem.Data == null){
			StartCoroutine(tem.StartShow(tip, showFrame));
		}else{
			StartCoroutine(tem.StartShow());
		}
		
        if (floatTipList.Count >= Max)
        {
            Onfinish(floatTipList[0]);
        }
        if (tem.Data != null && (tem.Data.sort == 13 || tem.Data.sort == 14))
        {
        }
        else
        {
            floatTipList.Add(tem);
        }
	}
	

    void Onfinish(FloatTipObject tipObject)
    {
		if(tipObject.Data != null && tipObject.Data.sort == 12){
			time -= tipObject.Data.holdTime;
			values --;
			if(time < 0){
				time = 0;
			}
		}
       Destroy(tipObject.gameObject);
       floatTipList.Remove(tipObject);
		if(floatTipList.Count == 0){
			time = 0;
			values = 0;
		}
    }


    void StartFloatOne(FloatTipObject tipObject,float y)
    {
		if(tipObject.Data != null && tipObject.Data.showType.Contains(8)){
			return ;
		}
        Vector3 p = tipObject.transform.localPosition;
        tipObject.transform.localPosition = new Vector3(p.x, p.y + y, p.z);
//		Debug.Log(tipObject.Data.messID + " p = "+ p.y + "NEW p = "+ y +"   end   := "+tipObject.transform.localPosition.ToString());
    }


    public void ClearAll()
    {
		for (int i=0;i< floatTipList.Count;i++)
		{
			Destroy(floatTipList[i].gameObject);
        }
        floatTipList.Clear();
    }
    
}
