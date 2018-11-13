//===============================
//作者：吴江
//日期：2015/7/24
//用途：聊天泡泡组件
//================================


using UnityEngine;
using System.Collections;

/// <summary>
/// 聊天泡泡组件 by吴江
/// </summary>
public class BubblePrefab : MonoBehaviour
{
    #region 数据
    public UILabel label;
    #endregion

    #region 辅助逻辑
    public void Content(string str)
    {
        label.text = str;
    }
    #endregion

}
