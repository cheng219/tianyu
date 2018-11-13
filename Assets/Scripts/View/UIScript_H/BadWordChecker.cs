//=========================================================
//作者：gc
//日期：2014/7/19
//用途：敏感字的检测判断;脚本用txt文本，每个字一行
//

using UnityEngine;
using System.Collections;
using System;

public  class BadWordChecker 
{
    /// <summary>
    /// 从外部文件里读取txt内容
    /// </summary>
    /// <param name="file">文件路径+文件名</param>
    public static void load(string file)
    {
        if (badWords != null)
            return;
        if (!System.IO.File.Exists(file))
        {
            Debug.Log("---- load file error ---- file=" + file);
            return;
        }

            WWW w = new WWW("file:///" + file);
            while (!w.isDone)
            {
            }
            if (w.text != null)
           setBadWords(w.text.Split(new char[] { '\n', '\r' }));
    }

    /// <summary>
    /// 从u3d工程里通过预制读取txt内容
    /// </summary>
    /// <param name="file">txt的路径</param>
    public static void loadFromResources(string file)
    {
        if (badWords != null)
            return;
        TextAsset tex = (TextAsset)Resources.Load(file);
        if (tex==null)
        {
            Debug.Log("---- loadFromResources file error ---- file=" + file);
            return;
        }


        if (tex.text != null)
            setBadWords(tex.text.Split(new char[] { '\n', '\r' }));
    }

    private static string[] badWords;
    private static void setBadWords(string[] bw)
    {
        if (bw == null||bw.Length==0)
         return;
        badWords = bw;
        badWords[0] = null;//第一行有个非法字符 不算进去
    }

    /// <summary>
    /// 字符串检测是否有敏感字并且替换为*
    /// </summary>
    /// <param name="strSoure">源字符串</param>
    /// <returns>新的字符串</returns>
    public static string replaceBadWordToStar(string strSoure)
    {
        if (badWords == null || badWords.Length == 0)
            return strSoure;
        while (true)
        {

            string b = BadWordChecker.checkBadWord(strSoure);
            if (b == null)
            {
                break;
            }
            else
            {
                string star = "*";
                for (int i = 1; i < b.Length; i++)
                {
                    star += "*";
                }
                strSoure = strSoure.Replace(b, star);
            }
        }

        return strSoure;
    }

    /// <summary>
    /// 检测字符窜是否有敏感字
    /// </summary>
    /// <param name="strSoure">源字符串</param>
    /// <returns>返回第一个检测到的敏感字，以便提示是哪个字有问题</returns>
    public static string checkBadWord(string strSoure)
    {

        if (badWords == null || badWords.Length == 0)
            return null;
   
       foreach (string s in  badWords )
       {
          
           if (!string.IsNullOrEmpty(s))
           {

          //     Debug.Log("----------------=" + s + "ammmaa" + strSoure.IndexOf(s));
               if (strSoure.IndexOf(s) != -1)
                   return s;
           }

       }
       return null;
    }
}
