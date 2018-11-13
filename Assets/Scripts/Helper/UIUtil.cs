using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

public class UIUtil : MonoBehaviour {
	///   <summary>
	///   MD5加密 by 何明军
	///   </summary>
	public static string MD5Encrypt(string _text){
		MD5 md5 = new MD5CryptoServiceProvider();
		byte[] bytes = Encoding.Default.GetBytes(_text); 
		byte[] md5Bytes = md5.ComputeHash(bytes);
		string str = string.Empty;
		try{
			for (int i = 0; i < md5Bytes.Length; i++)
			{
				str += md5Bytes[i].ToString("x2");
			}
		}catch{
			str = _text;
		}
		return str;
	}
	/// <summary>
	/// base64解码 by 何明军
	/// </summary>
	public static string DecodeBase64(string _code){
		string str = string.Empty;
		byte[] bytes = System.Convert.FromBase64String(_code); 
		try 
		{ 
			str = Encoding.Default.GetString(bytes); 
		} 
		catch 
		{ 
			str = _code; 
		} 
		return str; 
	}
	/// <summary>
	/// base64编码 by 何明军
	/// </summary>
	public static string EncodeBase64(string _code){
		string str = string.Empty;
		byte[] bytes = Encoding.Default.GetBytes(_code); 
		try 
		{ 
			str = System.Convert.ToBase64String(bytes); 
		} 
		catch 
		{ 
			str = _code; 
		} 
		return str; 
	}


	public static GameObject CreateItemUIGame(GameObject parent){
		
		GameObject games = null;

        Object st = exResources.GetResource(ResourceType.GUI, "Item_icon/Item_icon");
		if(st == null)
			return null;
		
		games = Instantiate(st) as GameObject;
        st = null;

        games.name = "Item_icon";
		
		games.transform.parent = parent.transform;

		return games;
	}

    private static UILabel CreateLabel(GameObject parent,string str, UIFont mFont,string color)
    {
        int nextDepth = NGUITools.CalculateNextDepth(parent);
        UILabel lbl = NGUITools.AddWidget<UILabel>(parent);
        lbl.name = "Label_text";
        lbl.depth = nextDepth;
        lbl.bitmapFont = mFont;
        lbl.pivot = UILabel.Pivot.TopLeft;
        lbl.transform.localScale = new Vector3(mFont.defaultSize, mFont.defaultSize, 1f);
        lbl.MakePixelPerfect();
        lbl.supportEncoding = true;
        lbl.symbolStyle = NGUIText.SymbolStyle.Normal;
		
		if(color != null)
			lbl.color = ColorManage.GetColor(color);
        lbl.text = str;
        return lbl;
    }

	public static void AddFloatLabel(GameObject parent,string msg,UIFont mFont,string color)
	{
		int nextDepth = NGUITools.CalculateNextDepth(parent);
		UILabel lbl = NGUITools.AddWidget<UILabel>(parent);
		lbl.name = "FloatLabel";
		lbl.depth = nextDepth;
        lbl.bitmapFont = mFont;
		lbl.pivot = UILabel.Pivot.TopLeft;
        lbl.transform.localScale = new Vector3(mFont.defaultSize, mFont.defaultSize, 1f);
		lbl.MakePixelPerfect();
		lbl.supportEncoding = true;
        lbl.symbolStyle = NGUIText.SymbolStyle.Normal;
		
		if(color != null)
			lbl.color = ColorManage.GetColor(color);
		lbl.text = msg;
		TweenPosition pos = TweenPosition.Begin(lbl.gameObject,0.4f,lbl.transform.localPosition+new Vector3(0f,20f,0f));
        EventDelegate.Add(pos.onFinished, ()=>
		{
			Destroy(lbl.gameObject);
		}, true);

	}

    /// <summary>
    /// 返回物品转成的字符串
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    //public static string GetItemShowText(EquipmentInfo item)
    //{
    //    string Itemname = item.ItemName;

    //    string text = "[{0}]【" + Itemname + (item.UpGradeLevel > 0 ? " +" + item.UpGradeLevel : "") + "】[-]";
 
    //    switch (item.Quality)
    //    {
    //        case 1:
    //             text = string.Format(text, "fbf1bb");
    //            break;
    //        case 2:
    //            text = string.Format(text, "a6d554");
    //            break;
    //        case 3:
    //            text = string.Format(text, "00eaff");
    //            break;
    //        case 4:
    //             text = string.Format(text, "8515ba");
    //            break;
    //        case 5:
    //            text = string.Format(text, "f66e0f");
    //            break;
    //        case 6:
    //             text = string.Format(text, "d30d0c");
    //            break;
    //        case 7:
    //            text = string.Format(text, "f75486");
    //            break;
    //        default:
    //             text = string.Format(text, "ffffff");
    //            break;
    //    }

    //    return text;

    //}

	public static string SetChatNameMilitaryLvIcon(string chatName){
		string str = "/rank";
		if(chatName.Contains(str)){
			return chatName.Remove(0,7);
		}
		return chatName;
	}

//    private static GameObject CreateOther(GameObject parent, string[] Params, UIFont mFont,int chatID = 0)
//    {
//        if (Params.Length < 1)
//            return null;
//        switch (Params[0])
//        {
//            case "usr_name":
//            {
//                    if (Params.Length < 3)
//                        return null;
//                    string name = Params[1];
////                    string id = Params[2];
//                    int depth = NGUITools.CalculateNextDepth(parent);
//                    UILabel lbl = NGUITools.AddWidget<UILabel>(parent);
//                    lbl.name = "usr_name";
//                    lbl.font = mFont;
//                    lbl.depth = depth;
//                    lbl.pivot = UIWidget.Pivot.TopLeft;
//                    lbl.transform.localScale = new Vector3(mFont.size, mFont.size, 1f);
//                    lbl.transform.localPosition = new Vector3(0, 0, 0);                  
//                    lbl.symbolStyle = UIFont.SymbolStyle.Colored;
//                    if (Params.Length > 3)
//                    {
//                         lbl.text = "[" + Params[3] + "] " + name + " [-]";
//                    }
//                    else
//                        lbl.text = " " + name + " ";

//                    //角色名字 加粗 描边
////                    lbl.effectStyle = UILabel.Effect.Outline;
////                    lbl.effectDistance = new Vector2(2, 2);
////					lbl.effectColor = Color.blue;
//                    lbl.MakePixelPerfect();
//            if(name == "公告"){return lbl.gameObject;}
//                   // BoxCollider box = 
//                    NGUITools.AddWidgetCollider(lbl.gameObject);
//                    lbl.gameObject.AddComponent<UIButton>().tweenTarget = lbl.gameObject;
//                    lbl.gameObject.AddComponent<UIButtonScale>();
//                    lbl.gameObject.AddComponent<UIButtonSound>();
//                    UIEventListener.Get(lbl.gameObject).onClick+=delegate(GameObject  go)
//                    {
//                        GameCenter.chatMng.NamePop(SetChatNameMilitaryLvIcon(name));
//                    };
//                    return lbl.gameObject;
//            }
//            case "battle":
//                return null;
//            case "VoiceIcon":
//            {
//                    if (Params.Length < 2)
//                        return null;
//                    string prefabname = Params[1];
//                    string length =Params[2].ToString();
//                    uint index =uint.Parse( Params[3]);
////                    int nextDepth = NGUITools.CalculateNextDepth(parent);
//                    string prefabPath ="";
//                    if (prefabname == "ChatVoice")
//                       prefabPath = "Chat/Button_ChatVoice";
//                    else if (prefabname == "ChatVoiceIn")
//                       prefabPath = "Chat/Button_ChatVoiceIn";
//                    else if (prefabname == "ChatVoiceOut")
//                       prefabPath = "Chat/Button_ChatVoiceOut";
//                    else
//                    {
//                        GameSys.LogError("VoiceIcon ");
//                    }
//                    Object prefab = exResources.GetResource(ResourceType.GUI, prefabPath);
//                    if (prefab == null)
//                    {
//                        GameSys.LogError("找不到预制："+prefabname);
//                        return null;
//                    }
//                    GameObject spirit = Instantiate(prefab) as GameObject;
//                    spirit.transform.parent = parent.transform;                    
//                    spirit.transform.localPosition = new Vector3(0, 0, 0);
//                    spirit.transform.localScale = new Vector3(1, 1, 1);
//                    UILabel UIS = spirit.GetComponentInChildren<UILabel>();
//                    UIS.text = length.ToString();
//                    UIEventListener.Get(spirit).onClick = delegate(GameObject  go)
//                    {
//                        GameSys.Log("播放语音"+spirit.ToString());
//                        GameCenter.chatMng.GetVoiceFromLists(index);
//                    };
//                    return spirit;
//            }
//            case "Item":
//                return null;
//            case "TextItem":
//            {
//                    if (Params.Length < 5)
//                        return null;
//                    int type = int.Parse(Params[1]);
//                    int instanceid = int.Parse(Params[2]);//强化等级
//                    int eid = int.Parse(Params[3]);
//                    int lev = int.Parse(Params[4]);
//            EquipmentInfo info = new EquipmentInfo(eid,0, EquipmentBelongTo.PREVIEW);
			       
//                    string Itemname =info.ItemName;
			       
//                    int depth = NGUITools.CalculateNextDepth(parent);
//                    UILabel lbl = NGUITools.AddWidget<UILabel>(parent);
//                    lbl.name = "TextItem";
//                    lbl.font = mFont;
//                    lbl.depth = depth + 1;
//                    lbl.pivot = UIWidget.Pivot.TopLeft;
//                    lbl.transform.localScale = new Vector3(mFont.size, mFont.size, 1f);
//                    lbl.transform.localPosition = new Vector3(0, 0, -5f);
//                    lbl.MakePixelPerfect();
//                    lbl.symbolStyle = UIFont.SymbolStyle.Colored;

//            string text = "[{0}]【" + Itemname + (lev>0?" +"+lev:"") + "】[-]";
//                #region 颜色
//            switch (info.Quality)
//            {
//                        case 1:
//                            lbl.text =string.Format(text,"fbf1bb");		
//                            break;
//                        case 2:
//                            lbl.text =string.Format(text,"a6d554");		
//                            break;
//                        case 3:
//                            lbl.text =string.Format(text,"00eaff");
//                            break;
//                        case 4:
//                            lbl.text =string.Format(text,"8515ba");	
//                            break;
//                        case 5:
//                            lbl.text =string.Format(text,"f66e0f");	
//                            break;
//                        case 6:
//                            lbl.text =string.Format(text,"d30d0c");
//                            break;
//                        case 7:
//                            lbl.text =string.Format(text,"f75486");
//                            break;
//                        default:
//                            lbl.text =string.Format(text,"ffffff");
//                            break;
//                    }
//                    #endregion
//                    //BoxCollider box = 
//                    NGUITools.AddWidgetCollider(lbl.gameObject);
//                    lbl.gameObject.AddComponent<UIButton>().tweenTarget = lbl.gameObject;
//                    lbl.gameObject.AddComponent<UIButtonScale>();
//                    lbl.gameObject.AddComponent<UIButtonOffset>();
//                    lbl.gameObject.AddComponent<UIButtonSound>();
//                    UIEventListener.Get(lbl.gameObject).onClick = delegate(GameObject  go)
//                    {
//                        if(instanceid == 0){
//                            ToolTipMng.ShowEquipmentTooltip(new EquipmentInfo(eid,0,EquipmentBelongTo.BACKPACK),
//                                                            ItemActionType.None, ItemActionType.None, ItemActionType.None,lbl.gameObject);
//                        }else{
//                            ChatObjectOnClickEty ety = GameCenter.chatMng.GetChatObjectOnClickEty(ChatObjectOnClickEty.ObjectType.ITEM,chatID);
////							if(ety != null){
////						ToolTipMng.ShowEquipmentTooltip((EquipmentInfo)ety.obj,
////								                                ItemActionType.None, ItemActionType.None, ItemActionType.None,lbl.gameObject);
////							}else{
//                                ToolTipMng.ShowEquipmentTooltip(new EquipmentInfo(GameCenter.chatMng.ItemType,instanceid),
//                                                                ItemActionType.None, ItemActionType.None, ItemActionType.None,lbl.gameObject);
//                                GameCenter.chatMng.C2S_CallOnClickItemData(ChatObjectOnClickEty.ObjectType.ITEM,instanceid,chatID);
////							}
//                        }
//                    };
//                    return lbl.gameObject;
//            }
//            case "FullScaleItem":
//            {
//                if (Params.Length < 2)
//                    return null;
//                int Eid = int.Parse(Params[1]);
//                EquipmentInfo info = new EquipmentInfo(Eid,0, EquipmentBelongTo.PREVIEW);
				
//                string Itemname = info.ItemName;
//                int depth = NGUITools.CalculateNextDepth(parent);
//                UILabel lbl = NGUITools.AddWidget<UILabel>(parent);
//                lbl.name = "FullScaleItem";
//                lbl.font = mFont;
//                lbl.depth = depth;
//                lbl.pivot = UIWidget.Pivot.TopLeft;
//                lbl.transform.localScale = new Vector3(mFont.size, mFont.size, 1f);
//                lbl.transform.localPosition = new Vector3(0, 0, 0);
//                lbl.MakePixelPerfect();
//                lbl.symbolStyle = UIFont.SymbolStyle.Colored;

//                string text = "[{0}]【" + Itemname + "】[-]";
//                #region 颜色
//                switch (info.Quality)
//                {
//                case 1:
//                    lbl.text =string.Format(text,"fbf1bb");		
//                    break;
//                case 2:
//                    lbl.text =string.Format(text,"a6d554");		
//                    break;
//                case 3:
//                    lbl.text =string.Format(text,"00eaff");
//                    break;
//                case 4:
//                    lbl.text =string.Format(text,"8515ba");	
//                    break;
//                case 5:
//                    lbl.text =string.Format(text,"f66e0f");	
//                    break;
//                case 6:
//                    lbl.text =string.Format(text,"d30d0c");
//                    break;
//                case 7:
//                    lbl.text =string.Format(text,"f75486");
//                    break;
//                default:
//                    lbl.text =string.Format(text,"ffffff");
//                    break;
//                }
//                #endregion
//                return lbl.gameObject;
//            }
//        }
//        return null;
//    }

    private static float FormatLine(List<GameObject> thisLine, float now_y)
    {
        float maxH = 0;
        foreach (GameObject go in thisLine)
        {
            Bounds b = NGUIMath.CalculateRelativeWidgetBounds(go.transform);
            float h = go.transform.localScale.y * b.size.y;
            maxH = (maxH > h) ? maxH : h;
        }

        foreach (GameObject go in thisLine)
        {
            Bounds b = NGUIMath.CalculateRelativeWidgetBounds(go.transform);
            float h = go.transform.localScale.y * b.size.y;
            go.transform.localPosition += new Vector3(0, now_y + -1 * (maxH - h), 0);
        }
        return now_y - maxH;
    }

    public static string Str2Str(string in_str, string[] parms = null)
    {
        if (in_str == "")
            return "";

        if (parms != null)
        {
            for (int i = 0; i < parms.Length; i++)
            {
                in_str = in_str.Replace("#" + i + "#", parms[i]);
            }
        }

        return in_str;
    }
	public static string Str2Str(string in_str, List<string> parms = null)
	{
		if (in_str == "")
			return "";
		
		if (parms != null)
		{
			for (int i = 0; i < parms.Count; i++)
			{
				in_str = in_str.Replace("#" + i + "#", parms[i]);
			}
		}
		
		return in_str;
	}
    public static UILabel Str2CentLab(GameObject parent, string in_str, UIFont mFont, Color color)
    {
        int nextDepth = NGUITools.CalculateNextDepth(parent);
        UILabel lbl = NGUITools.AddWidget<UILabel>(parent);
        lbl.name = "Label_text";
        lbl.depth = nextDepth;
        lbl.bitmapFont = mFont;
        lbl.pivot = UILabel.Pivot.Center;
        lbl.transform.localScale = new Vector3(mFont.defaultSize, mFont.defaultSize, 1f);
        lbl.MakePixelPerfect();
        lbl.supportEncoding = true;
        lbl.symbolStyle = NGUIText.SymbolStyle.Normal;//UIFont.SymbolStyle.Uncolored;  3.8.2更新导致的修改, by吴江 2015/7/2

        lbl.color = color;

        lbl.text = in_str;
        return lbl;
    }
    public static GameObject Str2Obj(GameObject parent,string in_str, UIFont mFont, float width, string[] parms = null,int chatID = 0)
    {
        in_str = Str2Str(in_str, parms);
        GameObject content = new GameObject("content");
        content.transform.parent = parent.transform;
        content.transform.localPosition = new Vector3(0, 0, 0);
        content.transform.localScale = new Vector3(1, 1, 1);
        content.layer = LayerMask.NameToLayer("NGUI");
        

        string color = null;
        string has = in_str;
        float DrawWidth = 0;

        float now_y = 0;
        List<GameObject> thisLine = new List<GameObject>();
        while (true)
        {
            if(has == null)
                break;
            if(has.Length == 0)
                break;

            int index_bef = has.IndexOf("[");
            int index_aft = has.IndexOf("]");
            if (index_bef == -1)
            {
                string strs = string.Empty;
                NGUIText.WrapText(has, out strs);
                if (strs.Length <= 0)
                {
                    if (DrawWidth == 0)
                    {
                        Debug.Log("can not set string");
                        return null;
                    }
                    now_y = FormatLine(thisLine, now_y);
                    DrawWidth = 0;
                    thisLine = new List<GameObject>();
                    continue;
                }

                UILabel lb = CreateLabel(content, strs, mFont, color);
                lb.transform.localPosition = new Vector3(DrawWidth, 0, 0);
                thisLine.Add(lb.gameObject);


                now_y = FormatLine(thisLine, now_y);
                DrawWidth = 0;
                thisLine = new List<GameObject>();

                if (has.Length > strs.Length)
                {
                    has = has.Substring(strs.Length, has.Length - strs.Length);
                    if (has[0] == '\n')
                    {
                        if (has.Length > 2)
                            has = has.Substring(1, has.Length - 1);
                        else
                            has = null;
                    }
                }
                else
                    has = null;
            }
            else
            {
                if(index_aft < index_bef)
                {
                    Debug.Log("error str = " + in_str);
                    return null;
                }
                if (index_aft == -1)
                {
                    Debug.Log("error str = " + in_str);
                    return null;
                }
                else
                {
                    string befor_has = has.Substring(0, index_bef);
                    while (true)
                    {
                        if (befor_has == null)
                            break;
                        if (befor_has.Length == 0)
                            break;

                        string befor_strs = string.Empty;
                        NGUIText.WrapText(befor_has, out befor_strs);
                        if (befor_strs.Length <= 0)
                        {
                            if (DrawWidth == 0)
                            {
                                Debug.Log("can not set string");
                                return null;
                            }
                            now_y = FormatLine(thisLine, now_y);
                            DrawWidth = 0;
                            thisLine = new List<GameObject>();
                            continue;
                        }
                        UILabel befor_lb = CreateLabel(content, befor_strs, mFont, color);
                        befor_lb.transform.localPosition = new Vector3(DrawWidth, 0, 0);
                        thisLine.Add(befor_lb.gameObject);
                        Bounds b = NGUIMath.CalculateRelativeWidgetBounds(befor_lb.gameObject.transform);
                        DrawWidth += b.size.x * mFont.defaultSize;


                        if (befor_has.Length > befor_strs.Length)
                        {
                            now_y = FormatLine(thisLine, now_y);
                            DrawWidth = 0;
                            thisLine = new List<GameObject>();

                            befor_has = befor_has.Substring(befor_strs.Length, befor_has.Length - befor_strs.Length);
                            if (befor_has[0] == '\n')
                            {
                                if (befor_has.Length > 2)
                                    befor_has = befor_has.Substring(1, befor_has.Length - 1);
                                else
                                    befor_has = null;
                            }
                        }
                        else
                        {
                            befor_has = null;
                        }
                    }
                    if(index_aft - index_bef > 1)
                    {
                        string trun_str = has.Substring(index_bef + 1, index_aft - index_bef - 1);
                        string[] trun_parms = trun_str.Split(',');
                        switch (trun_parms[0])
                        {
                            case "color":
                                color = trun_parms[1];
                                break;
                            case "-":
                                color = null;
                                break;
                            default:
                                //GameObject other = CreateOther(content, trun_parms, mFont,chatID);
                                //if (other != null)
                                //{
                                //    Bounds b = NGUIMath.CalculateRelativeWidgetBounds(other.transform);
                                //    if ((DrawWidth + b.size.x * other.transform.localScale.x) > width)
                                //    {
                                //        now_y = FormatLine(thisLine, now_y);
                                //        thisLine = new List<GameObject>();

                                //        other.transform.localPosition = new Vector3(0, 0, 0);
                                //        DrawWidth = b.size.x * other.transform.localScale.x;
                                //        thisLine.Add(other);
                                //    }
                                //    else
                                //    {
                                //        other.transform.localPosition = new Vector3(DrawWidth, 0, 0);
                                //        DrawWidth += b.size.x * other.transform.localScale.x;
                                //        thisLine.Add(other);
                                //    }
                                //}
                                break;
                        }
                    }

                    if (has.Length > index_aft + 1)
                        has = has.Substring(index_aft + 1, has.Length - (index_aft + 1));
                    else
                        has = null;
                }
            }
        }

        if (thisLine.Count > 0)
        {
            FormatLine(thisLine, now_y);
        }
        return content;
    }

}

public class UIChoose
{
    private Dictionary<int, GameObject> _set;
    private int now = -1;
    public UIChoose()
    {
        _set = new Dictionary<int, GameObject>();
    }
    public void add(int i, GameObject o)
    {
        o.SetActive(false);
        _set.Add(i, o);
    }

    public void Choose(int i)
    {
        if (_set.ContainsKey(i))
        {
            if (now >= 0)
            {
                if (_set.ContainsKey(now))
                    _set[now].SetActive(false);
            }
            _set[i].SetActive(true);
            now = i;
        }

    }
}

public class UIFormatScrollPanle
{
    public const int SORT_VERTICAL = 1;
    public const int SORT_HORIZONTAL = 2;

    public int sort;
    public float max_x;
    public float max_y;

    public UIDragScrollView bgUIDragPanelContents;//背景也能滑动

    public GameObject content = null;
   // private UIScrollBar usb = null;


    private void setData(int sort, UIPanel panel, UIScrollBar ScrBar)
    {
       // usb = ScrBar;
        this.sort = sort;
        if (sort == SORT_VERTICAL)
            init_v(panel, ScrBar);
        else
            init_h(panel, ScrBar);


        //以前是根据加的碰撞来算，现在按panel的clipRange来算
        max_x = panel.finalClipRegion.z;
        max_y = panel.finalClipRegion.w;
    }
    public UIFormatScrollPanle(int sort, UIPanel panel, UIScrollBar ScrBar)
    {
        setData(sort, panel, ScrBar);
    }
    public UIFormatScrollPanle(int sort, UIPanel panel, UIScrollBar ScrBar, UIDragScrollView bgUIDragPanelContents)
    {
        this.bgUIDragPanelContents = bgUIDragPanelContents;
        setData(sort, panel, ScrBar);
    }
    public Vector3 AddChild(GameObject o)
    {
        Vector3 p = o.transform.localPosition;
        Vector3 s = o.transform.localScale;
        o.transform.parent = content.transform;
        o.transform.localPosition = p;
        o.transform.localScale = s;
        
        Bounds b = NGUIMath.CalculateRelativeWidgetBounds(o.transform);
        //Vector3 vsp = new Vector3(o.transform.localPosition.x + b.center.x - b.size.x / 2, o.transform.localPosition.y + b.center.y - b.size.y / 2, o.transform.localPosition.z + b.center.z - b.size.z / 2);
        Vector3 vs = b.size;
    //    UpdateBox(vsp, vs);

        UIButton btn = o.GetComponent<UIButton>();
        if (btn != null)
        {
            UIDragScrollView dp = o.GetComponent<UIDragScrollView>();
			if(dp == null)
                dp = o.AddComponent<UIDragScrollView>();
            dp.scrollView = content.transform.parent.GetComponent<UIScrollView>();
        }
		
		UIPanel _uipanel = o.GetComponent<UIPanel>();
		if(_uipanel!=null)
		{
			 NGUITools.Destroy(_uipanel);
		}
        return vs;
    }

    public void Clear()
    {
		if(content==null)return;
        while (content.transform.childCount > 0)
        {
            Transform t = content.transform.GetChild(0);
            NGUITools.Destroy(t.gameObject);
        }
    }

	public void ClearImmediate()
	{
		if(content==null)return;
		while (content.transform.childCount > 0)
		{
			Transform t = content.transform.GetChild(0);
			NGUITools.DestroyImmediate(t.gameObject);
		}
	}
	
	public void ToTop()
    {
        UIScrollView dp = content.transform.parent.GetComponent<UIScrollView>();
        dp.ResetPosition();
        dp.SetDragAmount(0f,0f, false);
    }


    public void ToBottom()
    {
        UIScrollView dp = content.transform.parent.GetComponent<UIScrollView>();
        dp.ResetPosition();
        dp.SetDragAmount((this.sort == SORT_VERTICAL) ? 0.01f : 1.0f, (this.sort == SORT_HORIZONTAL) ? 0.01f : 1.0f, false);
    }

    private void UpdateBox(Vector3 vsp,Vector3 vs)
    {
        if (sort == SORT_VERTICAL)
            UpdateBox_v(vsp, vs);
        else
            UpdateBox_h(vsp, vs); 
    }

    private void UpdateBox_v(Vector3 vsp, Vector3 vs)
    {
        BoxCollider bc = content.GetComponent<BoxCollider>();
        float min_y_old = (bc.center.y - bc.size.y / 2);
        float min_y_new = vsp.y;
        float min_y = (min_y_old < min_y_new) ? min_y_old : min_y_new;
        float max_y_old = (bc.center.y + bc.size.y / 2);
        float max_y_new = vsp.y + vs.y;
        float max_y = (max_y_old < max_y_new) ? max_y_new : max_y_old;
        bc.center = new Vector3(bc.center.x, (min_y + max_y) / 2, bc.center.z);
        bc.size = new Vector3(bc.size.x, max_y - min_y, bc.size.z);
    }

    private void UpdateBox_h(Vector3 vsp, Vector3 vs)
    {
        BoxCollider bc = content.GetComponent<BoxCollider>();
        float min_x_old = (bc.center.x - bc.size.x / 2);
        float min_x_new = vsp.x;
        float min_x = (min_x_old < min_x_new) ? min_x_old : min_x_new;
        float max_x_old = (bc.center.x + bc.size.x / 2);
        float max_x_new = vsp.x + vs.x;
        float max_x = (max_x_old < max_x_new) ? max_x_new : max_x_old;
        bc.center = new Vector3((min_x + max_x) / 2, bc.center.y, bc.center.z);
        bc.size = new Vector3(max_x - min_x, bc.size.y, bc.size.z);
    }

    private void init_v(UIPanel panel, UIScrollBar ScrBar)
    {
        panel.gameObject.AddComponent<UIScrollView>();
        UIScrollView pd = panel.gameObject.GetComponent<UIScrollView>();		
        if (pd)
        {
			pd.disableDragIfFits = true;
            pd.verticalScrollBar = ScrBar;
           // pd.scale = new Vector3(0, 1, 0);    //3.8.2 更新  by吴江 2015/7/2
            pd.scrollWheelFactor = -2;
            pd.momentumAmount = 35;
            pd.dragEffect = UIScrollView.DragEffect.MomentumAndSpring;
            pd.showScrollBars = UIScrollView.ShowCondition.Always;

            content = new GameObject("content");
            content.transform.parent = panel.gameObject.transform;
            content.transform.localPosition = new Vector3(0, 0, 0);
            content.transform.localScale = new Vector3(1, 1, 1);
            content.layer = LayerMask.NameToLayer("NGUI");
/*//好像没有用 有问题再去掉注释
            content.AddComponent<BoxCollider>();
            BoxCollider bc = content.gameObject.GetComponent<BoxCollider>();
            Vector4 v = panel.clipRange;
            max_x = panel.clipRange.z;
            bc.center = new Vector3(panel.clipRange.z / 2, 0, 0);
            bc.size = new Vector3(panel.clipRange.z, 0, 0);
            content.AddComponent<UIDragPanelContents>();
            UIDragPanelContents dp = content.GetComponent<UIDragPanelContents>();
            dp.draggablePanel = pd;
*/
            if (bgUIDragPanelContents != null)
                bgUIDragPanelContents.scrollView = pd;
        }
    }

    private void init_h(UIPanel panel, UIScrollBar ScrBar)
    {
        panel.gameObject.AddComponent<UIScrollView>();
        UIScrollView pd = panel.gameObject.GetComponent<UIScrollView>();
        if (pd)
        {
            pd.horizontalScrollBar = ScrBar;
           // pd.scale = new Vector3(0, 1, 0);  //3.8.2 更新  by吴江 2015/7/2
            pd.scrollWheelFactor = -2;
            pd.momentumAmount = 35;
            pd.dragEffect = UIScrollView.DragEffect.MomentumAndSpring;

            content = new GameObject("content");
            content.transform.parent = panel.gameObject.transform;
            content.transform.localPosition = new Vector3(0, 0, 0);
            content.transform.localScale = new Vector3(1, 1, 1);
            content.layer = LayerMask.NameToLayer("NGUI");
/*
            content.AddComponent<BoxCollider>();
            BoxCollider bc = content.gameObject.GetComponent<BoxCollider>();
            Vector4 v = panel.clipRange;
            max_y = panel.clipRange.w;
            bc.center = new Vector3(0, -1 * panel.clipRange.w / 2, 0);
            bc.size = new Vector3(0, panel.clipRange.w, 0);
            content.AddComponent<UIDragPanelContents>();
            UIDragPanelContents dp = content.GetComponent<UIDragPanelContents>();
            dp.draggablePanel = pd;
*/
            if (bgUIDragPanelContents != null)
                bgUIDragPanelContents.scrollView = pd;

        }
    }
}
