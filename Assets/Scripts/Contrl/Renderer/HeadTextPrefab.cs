//=========================================
//作者：吴江
//日期：2015/5/24
//用途：头顶文字预制
//========================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class HeadTextPrefab : MonoBehaviour {

    public class TextStruct
    {
        public GameObject spriteBg;
        public UILabel lab;
		public UILabel guildLab;//公会名字label by 
        public DateTime start = DateTime.Now;
        public float time = 0f;
        public Vector3 satrt_pos;
    }
	/// <summary>
	/// 名字的字体
	/// </summary>
	public UIFont name_font = null;
	/// <summary>
	/// 伤害敌方造成爆击的字体(UIFont_crit_dmg)
	/// </summary>
	public UIFont bj_font = null;
	/// <summary>
	/// 伤害敌方字体(UIFont_dmg)
	/// </summary>
	public UIFont font_dmg = null;
	/// <summary>
	/// 幸运一击字体
	/// </summary>
	public UIFont xy_font = null;
	/// <summary>
	/// 宠物造成伤害字体
	/// </summary>
	public UIFont font_pet_dmg = null;
	/// <summary>
	/// 友方被伤害的字体,包括暴击、幸运一击、闪避等(UIFont_get_dmg)
	/// </summary>
	public UIFont font_get_dmg = null;
	/// <summary>
	/// 治疗字体(UIFont_heal)
	/// </summary>
	public UIFont zl_font = null;

    public float off = 5f;
    public float vx = 1f;
    public float vy = 2f;
    public float g = -10f;
    public float time = 3.0f;
    public Color colorName;
    public TextStruct setName = null;
    [System.NonSerialized]
    public float fontSize = 0.0224f;//字体大小
    public UIAtlas spriteBgAtlas;//背景底框的材质
    public float bgRate = 0.13f;
    public string spriteBgName;




    public void SetText(string _name)
    {

        if (setName == null)
        {

            UILabel namelab = UIUtil.Str2CentLab(this.gameObject, _name, name_font, colorName);
            namelab.gameObject.SetActive(true);
            namelab.gameObject.transform.localPosition = Vector3.zero;
            namelab.effectStyle = UILabel.Effect.Outline;
            namelab.gameObject.transform.localScale = new Vector3(fontSize, fontSize, 1f);
            namelab.depth = 2;
            namelab.overflowMethod = UILabel.Overflow.ResizeFreely;
			namelab.pivot = UIWidget.Pivot.Bottom;
				
            TextStruct str = new TextStruct();
            str.lab = namelab;
            setName = str;

            //if (spriteBgAtlas != null)
            //{
            //    UISprite sprite = NGUITools.AddSprite(this.gameObject, spriteBgAtlas, spriteBgName);
            //    sprite.depth = namelab.depth - 1;
            //    sprite.type = UISprite.Type.Simple;
            //    string[] nameArray = _name.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            //    int maxLength = 6;
            //    foreach (var item in nameArray)
            //    {
            //        maxLength = Mathf.Max(maxLength, item.Length);
            //    }
            //    sprite.color = new Color(0, 0, 0, 0.5f);
            //    sprite.transform.localScale = new Vector3(maxLength * fontSize * bgRate, nameArray.Length * fontSize * bgRate, 1);//宽度是6和最长单行字符串长度中比较大的值,高度时字符串行数 by吴江
            //    //==========================
            //    sprite.transform.localPosition = new Vector3(namelab.transform.localPosition.x,
            //        namelab.transform.localPosition.y, namelab.transform.localPosition.z + 0.1f);
            //    str.spriteBg = sprite.gameObject;
            //}

        }
        else
        {
            setName.lab.text = _name;
        }
    }

	
	/// <summary>
	/// 公会名字改变，通过0x3339协议改变
	/// </summary>
//    public void SetGuildText(string _guildName)
//    {
//		
//		
//		
//		
//		
//		if(setName == null || setName.guildLab == null)
//			return;
//		if(_guildName.Equals(""))
//			setName.guildLab.text = _guildName;
//		else
//			setName.guildLab.text = new System.Text.StringBuilder().Append("<").Append(_guildName).Append(">").ToString();
//    }
	


    public void SetColor(Color _color)
    {
        colorName = _color;
        setName.lab.color = colorName;
    }


    public void SetOutLineColor(Color _color)
    {
        setName.lab.effectColor = _color;
    }

}
