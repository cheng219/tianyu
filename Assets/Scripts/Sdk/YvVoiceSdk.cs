//==============================================
//作者：黄洪兴
//日期：2016/8/22
//=================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System;
using YunvaIM;

/// <summary>
/// 云娃语音SDK
/// </summary>
public class YvVoiceSdk : MonoBehaviour
{

    public static bool isTest = true;

    /// <summary>
    /// 当前录音的语音音量
    /// </summary>
    public static float recordVolume = 0;


    /// <summary>
    /// 云娃语音初始化
    /// </summary>
    public static void YvVoiceInit()
    {
        if (GameCenter.instance.isPlatform && isTest)
        {
            int init = YunVaImSDK.instance.YunVa_Init(0, 1000481, Application.persistentDataPath, false);
            if (init == 0)
            {
                  //NGUIDebug.Log("云娃语音初始化成功");
                  //Debug.Log("云娃语音初始化成功");
                EventListenerManager.AddListener(ProtocolEnum.IM_RECORD_VOLUME_NOTIFY, ImRecordVolume);
            }
            else
            {
                //Debug.Log("云娃语音初始化失败");
                 //NGUIDebug.Log("云娃语音初始化失败");
            }
        }

    }

    /// <summary>
    /// 录音音量的回调
    /// </summary>
    /// <param name="data"></param>
    protected static void ImRecordVolume(object data)
    {
        ImRecordVolumeNotify RecordVolumeNotify = data as ImRecordVolumeNotify;
        // NGUIDebug.Log("ImRecordVolumeNotify:v_volume=" + RecordVolumeNotify.v_volume + ",v_ext=" + RecordVolumeNotify.v_ext);
        if (RecordVolumeNotify != null)
        {
            recordVolume = RecordVolumeNotify.v_volume;
        }
    }

    /// <summary>
    /// 打开录音音量
    /// </summary>
    protected static void OpenVoiceVolumeRecord()
    {
        YunVaImSDK.instance.RecordSetInfoReq(true);
    }

    /// <summary>
    /// 云娃语音登陆
    /// </summary>
    public static void YvVoiceLogin(string _name, int _useID)
    {
        if (GameCenter.instance.isPlatform && isTest)
        {
            string name = "name54321";
            string useID = "test";
            string loginID = "1";
            useID = string.IsNullOrEmpty(_useID.ToString()) ? useID : _useID.ToString();
            name = string.IsNullOrEmpty(_name) ? name : _name;
            loginID = string.IsNullOrEmpty(GameCenter.loginMng.Login_ID.ToString()) ? loginID : GameCenter.loginMng.Login_ID.ToString();
            string loginName = string.Format("{{\"nickname\":\"{0}\",\"uid\":\"{1}\"}}", name, useID);
            string[] wildcard = new string[2];
            wildcard[0] = "0x001";
            wildcard[1] = "0x002";

            // NGUIDebug.Log(loginID + ":登陆的服务器ID");                            
            // NGUIDebug.Log("name:" + name);
            // Debug.Log(name);
            YunVaImSDK.instance.YunVaOnLogin(loginName, loginID, wildcard, 0, (data) =>
            {
                // string labelText = string.Empty;
                if (data.result == 0)
                {

                    // labelText = string.Format("登录成功，昵称:{0},用户ID:{1}", data.nickName, data.userId);
                    // NGUIDebug.Log(labelText);
                    OpenVoiceVolumeRecord();
                }
                else
                {

                    //  labelText = data.result + string.Format("登录失败，错误消息：{0}", data.msg);
                    // NGUIDebug.Log(labelText);
                }
            }, (data1) =>
            {
                if (data1.result == 0) { }
                //  NGUIDebug.Log("频道登陆成功...");
                else { }
                //   NGUIDebug.Log("频道登陆失败...");
            });

        }
    }

    /// <summary>
    /// 云娃语音登出
    /// </summary>
    public static void YvVoiceLogOut()
    {
        if (GameCenter.instance.isPlatform && isTest)
        {
            YunVaImSDK.instance.YunVaLogOut();

        }
    }


    /// <summary>
    /// 云娃语音录音
    /// </summary>
    public static void YvVoiceRecordVoice()
    {
        if (GameCenter.instance.isPlatform && isTest)
        {
            //  YvVoiceStopPlayVoice(); 
            if (!GameCenter.chatMng.isStopMusic)
            {
                GameCenter.chatMng.StopSound();
            }
            string filePath = string.Format("{0}/{1}.amr", Application.persistentDataPath, DateTime.Now.ToFileTime());
            YunVaImSDK.instance.RecordStartRequest(filePath);
        }
    }

    /// <summary>
    /// 语音保存路径名字
    /// </summary>
    public static string recordPath = string.Empty;

    /// <summary>
    /// 云娃语音结束录音
    /// </summary>
    public static void YvVoiceStopRecordVoice(System.Action<string, string> _callBack)
    {
        if (GameCenter.instance.isPlatform && isTest)
        {
            YunVaImSDK.instance.RecordStopRequest((data) =>
            {

                if (data.result == 0)
                {
                    //   NGUIDebug.Log("录音成功:" + data.time + ":" + data.strfilepath);
                    if (data.time < 1.0f)
                    {
                        GameCenter.messageMng.AddClientMsg(452);
                        return;
                    }
                    else
                    {

                    }
                }
                else
                {
                    //   NGUIDebug.Log("录音失败描述:" + data.time + ":" + data.msg);

                    GameCenter.messageMng.AddClientMsg(476);
                }

                //GameCenter.soundMng.SetCurAllVoiceValueAlongSystemSetting(1);
                if (!string.IsNullOrEmpty(data.strfilepath))
                {
                    string voiceText = string.Empty;
                    string voiceUrlPath = string.Empty;
                    recordPath = data.strfilepath;
                    YvVoiceVoiceToText((data1) =>
                    {
                        voiceText = data1; 
                        //  NGUIDebug.Log("voiceText:" + voiceText);
                        YvVoiceUpLoadVoice((data2) =>
                        {
                            voiceUrlPath = data2;
                            //       NGUIDebug.Log("录音成功voiceText:" + voiceText);
                            if (_callBack != null) _callBack(voiceText, voiceUrlPath);
                        });
                    });
                      //string labelText = "停止录音返回:" + recordPath;
                      //NGUIDebug.Log(labelText);
                }
            });
        }
    }



    /// <summary>
    /// 云娃语音播放声音
    /// </summary>
    public static void YvVoicePlayVoice(string _recordPath)
    {

        if (GameCenter.instance.isPlatform && isTest)
        {

            YunVaImSDK.instance.RecordStartPlayRequest("", _recordPath, "", (data2) =>
            {

                if (data2.result == 0)
                {
                    YvVoiceStopPlayVoice();
                    //  NGUIDebug.Log("播放成功");

                }
                else
                {
                    GameCenter.messageMng.AddClientMsg(479);
                    //  NGUIDebug.Log("播放失败:" + data2.describe);

                }
                //GameCenter.soundMng.SetCurAllVoiceValueAlongSystemSetting(1);

            });
        }
    }

    /// <summary>
    /// 云娃语音播放声音回调
    /// </summary>
    public static void YvVoicePlayVoiceCallBack(string _recordPath,string _time, System.Action _callBack)
    {

        if (GameCenter.instance.isPlatform && isTest)
        {
            if (!GameCenter.chatMng.isStopMusic)
            {
                GameCenter.chatMng.StopSound();//播放语音暂停声音
            }
            // YvVoiceStopPlayVoice();
            YunVaImSDK.instance.RecordStartPlayRequest("", _recordPath, _time, (data2) =>
            {

                if (data2.result == 0)
                {
                    YvVoiceStopPlayVoice();
                    //NGUIDebug.Log("播放成功");

                }
                else
                {
                    //NGUIDebug.Log("播放失败:" + data2.describe);
                    GameCenter.messageMng.AddClientMsg(479);
                }
               // GameCenter.soundMng.SetCurAllVoiceValueAlongSystemSetting(1);
                if (_callBack != null) _callBack();
            });
        }
    }


    /// <summary>
    /// 云娃语音停止播放声音
    /// </summary>
    public static void YvVoiceStopPlayVoice()
    {
        if (GameCenter.instance.isPlatform && isTest)
        { 
            YunVaImSDK.instance.RecordStopPlayRequest();
        }
    }


    /// <summary>
    /// 云娃语音上传声音
    /// </summary>
    public static void YvVoiceUpLoadVoice(System.Action<string> _callback)
    {
        if (GameCenter.instance.isPlatform && isTest)
        {
            YunVaImSDK.instance.UploadFileRequest(recordPath, "", (data1) =>
            {
                if (data1.result == 0)
                {
                    string recordUrlPath = data1.fileurl;
                    //  string labelText = "上传成功:" + recordUrlPath;
                    //  NGUIDebug.Log(labelText);
                    if (_callback != null)
                    {
                        _callback(recordUrlPath);
                    }


                }
                else
                {
                    //string labelText = data1.msg +":"+data1.percent;
                    //GameCenter.messageMng.AddClientMsg(360);
                     //NGUIDebug.Log("上传失败:" + labelText);
                    GameCenter.messageMng.AddClientMsg(476);
                }
            });
        }
    }

    /// <summary>
    /// 云娃语音下载声音
    /// </summary>
    public static void YvVoiceLoadVoice(string _recordUrlPath, System.Action<string> _callBack)
    {
        if (GameCenter.instance.isPlatform && isTest)
        {
            // string recordUrlPath = string.Empty;
            string DownLoadfilePath = string.Empty;
            //   NGUIDebug.Log(_recordUrlPath);
            YunVaImSDK.instance.DownLoadFileRequest(_recordUrlPath, DownLoadfilePath, "88888", (data4) =>
            {
                if (data4.result == 0)
                {
                    //  string labelText = "下载成功:" + data4.filename;

                    if (_callBack != null)
                    {
                        _callBack(data4.filename);
                    }
                    //    NGUIDebug.Log(labelText);

                }
                else
                {
                    GameCenter.messageMng.AddClientMsg(476);
                      //string labelText = "下载失败:" + data4.msg + ":" + data4.percent;
                       //NGUIDebug.Log(labelText);
                }
            });
        }
    }



    /// <summary>
    /// 云娃语音语音识别
    /// </summary>
    private static void YvVoiceVoiceToText(System.Action<string> _callback)
    {
        YunVaImSDK.instance.SpeechSetLanguage(Yvimspeech_language.im_speech_zn, yvimspeech_outlanguage.im_speechout_simplified);
        bool voiceText = true;
        YunVaImSDK.instance.SpeechStartRequest(recordPath, "", (data3) =>
        {
             string labelText = string.Empty;

            if (data3.result == 0)
            {
                //  labelText = "识别成功，识别内容:" + data3.text;
                voiceText = GameCenter.loginMng.CheckBadWord(data3.text); 
                //if (!voiceText)
                //{
                    
                //    GameCenter.messageMng.AddClientMsg(299);
                //}
                //else
                if (voiceText)//add
                {
                    if (_callback != null)
                    {
                        _callback(data3.text);
                    }
                }
                //  NGUIDebug.Log(labelText);

            }
            else
            {
                //labelText = "识别失败，原因:" + data3.msg;
                //GameCenter.messageMng.AddClientMsg(351);
                //NGUIDebug.Log(labelText);
                GameCenter.messageMng.AddClientMsg(476);
            }
        });

    }



    #region 辅助逻辑

    private static bool StringIsLegal(string _string)
    {
        bool legal = false;
        if (BadWordChecker.checkBadWord(_string) == null) legal = true;
        return legal;
    }


    ///// <summary>
    ///// 判断手势是否滑动
    ///// </summary>
    ///// <returns></returns>
    //public static bool MoveSingle()
    //{
    //    bool flag = false;
    //    if (GameCenter.instance.isPlatform && isTest)
    //    {

    //        if (Input.touchCount > 0 && Input.GetTouch(GameCenter.chatMng.TouchFingerNum).phase == TouchPhase.Moved)
    //        {
    //            Vector2 deleta = Input.GetTouch(GameCenter.chatMng.TouchFingerNum).deltaPosition;
    //            if (Mathf.Abs(deleta.x) > 2 || Mathf.Abs(deleta.y) > 2) flag = true;
    //            //  NGUIDebug.Log(Input.GetTouch(GameCenter.chatMng.TouchFingerNum).deltaPosition + ":手指开始移动了");

    //        }

    //    }

    //    if (flag) GameCenter.chatMng.SetFingerMove(flag);
    //    return flag;
    //}

    /// <summary>
    /// 是否在WIFI下联网
    /// </summary>
    /// <returns></returns>
    public static bool IsWifiNetWork()
    {
        bool flag = false;

        flag = Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork;
        return flag;
    }


    #endregion



}
