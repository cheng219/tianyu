/// <summary>
/// Platform path mng.路径管理 by 沙新佳
/// </summary>


using UnityEngine;
using System.Collections;
using System.IO;

public class PlatformPathMng
{

    static string httpPath = "175.6.7.107:9998";
    public enum wwwProtocolType : int
    {
        http,
        https,
        file,
        ftp,
    }

    static public string GetWWWPath(string path, AssetPathType pathtype, string suffix = "")
    {
        string temppath = "";
        if (pathtype == AssetPathType.Http)
        {
            temppath = GetWWWTypePath(wwwProtocolType.http, pathtype, GetFilePath(path, pathtype, suffix));
        }
        else
        {
            temppath = GetWWWTypePath(wwwProtocolType.file, pathtype, GetFilePath(path, pathtype, suffix));
        }
        return temppath;

    }
    static public string GetWWWTypePath(wwwProtocolType type, AssetPathType pathtype, string completeFilePath)
    {
        string temppath = "";
        switch (type)
        {
            case wwwProtocolType.file:
                if (Application.platform == RuntimePlatform.Android && pathtype == AssetPathType.StreamingAssetsPath)
                {
                    temppath = completeFilePath;
                }
                else
                {
                    if (completeFilePath.Substring(0, 1) == "/")
                    {
                        temppath = "file://" + completeFilePath;
                    }
                    else
                        temppath = "file:///" + completeFilePath;
                }
                break;
            case wwwProtocolType.ftp:
                temppath = "ftp://" + completeFilePath;
                break;
            case wwwProtocolType.http:
                temppath = "http://" + completeFilePath;
                break;
            case wwwProtocolType.https:
                temppath = "https://" + completeFilePath;
                break;
        }
        return temppath;

    }

    static public string GetFilePath(string path, AssetPathType _pathtype, string suffix = "")
    {
        string temppath = "";
		if (path.Length > 0 && path.Substring(0, 1) == "/")
        {
            path = path.Remove(0, 1);
        }
        path += suffix;
        //拼接api固定路径
        switch (_pathtype)
        {
            case AssetPathType.StreamingAssetsPath:
                temppath = GetStreamingAssetsPath(path);
                break;
            case AssetPathType.PersistentDataPath:
                temppath = GetPersistentAssetsPath(path);
                break;
            case AssetPathType.DataPath:
                temppath = GetDataPath(path);
                break;
            case AssetPathType.TemporaryCachePath:
                temppath = GetTemporaryCachePath(path);
                break;
            case AssetPathType.Http:
                temppath = GetHttpAssetPath(path);
                break;
            default:
                break;
        }
        return temppath;
    }


    static string GetStreamingAssetsPath(string path)
    {
        return Application.streamingAssetsPath + Path.AltDirectorySeparatorChar + path;
        //	 return Path.Combine(Application.streamingAssetsPath,path);
    }
    static string GetPersistentAssetsPath(string path)
    {
        return Application.persistentDataPath + Path.AltDirectorySeparatorChar + path;
        //	 return Path.Combine(Application.persistentDataPath ,path);
    }
    static string GetDataPath(string path)
    {
        return Application.dataPath + Path.AltDirectorySeparatorChar + path;
        //	 return Path.Combine(Application.dataPath ,path);
    }
    static string GetTemporaryCachePath(string path)
    {
        return Application.temporaryCachePath + Path.AltDirectorySeparatorChar + path;
        //	 return Path.Combine(Application.temporaryCachePath ,path);
    }
    static string GetHttpAssetPath(string path)
    {
        return httpPath + Path.AltDirectorySeparatorChar + path; 
    }
    /// <summary>
    /// Subs the path by platform.添加平台分支路径
    /// </summary>
    /// <returns>
    /// The path by platform.
    /// </returns>
    /// <param name='path'>
    /// Path.
    /// </param>
    public static string SubPathByPlatform(string path)
    {
        if (path.Length == 0) return string.Empty;
        if (path.Substring(0, 1) == "/")
        {
            path = path.Remove(0, 1);
        }
        string subtemppath = "";
        //拼接平台差异路径

        subtemppath = GetPlatformString + Path.AltDirectorySeparatorChar + path;

        return subtemppath;
    }

    public static string GetPlatformString
    {
        get
        {
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.Android:
                    return "assetbundles_android";
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.IPhonePlayer:
                    return "assetbundles_ios";
                case RuntimePlatform.WindowsPlayer:
                    return "assetbundles_win32";
                default:
                    return "";
            }
        }
    }

    public static bool isFileExist(string path)
    {
        string splitstr = "://";
        return File.Exists(path.Substring(path.IndexOf(splitstr) + splitstr.Length)) || File.Exists(path.Substring(path.IndexOf(splitstr) + splitstr.Length + 1));
    }
}
