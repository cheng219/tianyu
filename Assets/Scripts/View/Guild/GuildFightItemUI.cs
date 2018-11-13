//=====================================
//作者:黄洪兴
//日期:2016/6/30
//用途:仙盟战组件
//========================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuildFightItemUI : MonoBehaviour
{
    #region 控件数据
    /// <summary>
    /// 公会名字
    /// </summary>
    public UILabel GuildName;
    /// <summary>
    /// 旗子图片
    /// </summary>
    public UISpriteEx flag;
    /// <summary>
    /// 底座图片
    /// </summary>
    public UISpriteEx baseSprite;
    #endregion
    #region 数据
    /// <summary>
    /// 当前填充的数据
    /// </summary>
    public GuildFightItemInfo Info;
    //	protected SkillInfo oldSkillinfo;  //for upgrade effect -by ms
    #endregion
    // Use this for initialization
    void Start()
    {



    }





    /// <summary>
    /// 填充数据
    /// </summary>
    /// <param name="_info"></param>
    public void FillInfo(GuildFightItemInfo _info)
    {

            Info = _info;
        Refresh();
    }
    /// <summary>
    /// 刷新表现
    /// </summary>
    public void Refresh()
    {
        if (Info != null)
        {
            if (Info.state == 0 || Info.state == 1 || Info.state==3)
            {
                if (GuildName != null)
                    GuildName.text = "[00FF00]" + Info.name;
                if (flag != null)
                    flag.IsGray = UISpriteEx.ColorGray.normal;
                if (baseSprite != null)
                    baseSprite.IsGray = UISpriteEx.ColorGray.normal;
            }
            else
            {
                if (GuildName != null)
                    GuildName.text = "[A8A8A8]" + Info.name;
                if (flag != null)
                    flag.IsGray = UISpriteEx.ColorGray.Gray;
                if (baseSprite != null)
                    baseSprite.IsGray = UISpriteEx.ColorGray.Gray;

            }

        }
        else
        {
            if (GuildName != null)
                GuildName.text = "";
            if (flag != null)
                flag.IsGray = UISpriteEx.ColorGray.Gray;
            if (baseSprite != null)
                baseSprite.IsGray = UISpriteEx.ColorGray.Gray;

        }


    }






}
