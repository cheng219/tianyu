//=====================================
//作者：吴江
//日期：2015/7/28
//用途: 漂浮数字的朝向控制组件
//=====================================


using UnityEngine;
using System.Collections;

public class NumberLookAt : MonoBehaviour
{

    public Camera target = null;

    void Update()
    {
        transform.eulerAngles = target.transform.eulerAngles;
    }
}
