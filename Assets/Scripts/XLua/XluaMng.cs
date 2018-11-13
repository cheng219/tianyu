//==============================================
//作者：邓成
//日期：2017/03/31
//用途：XLUA热更管理类
//=============================================

using UnityEngine;
using System.Collections;
using XLua;

public class XluaMng {
    protected const string assetName = "scriptsConfig.txt";
    protected LuaEnv luaEnv = null;
    protected static XluaMng _instance = null;
    public static XluaMng instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new XluaMng();
            }
            return _instance;
        }
    }
    public XluaMng()
    {
        luaEnv = new LuaEnv();
    }
    protected string scripts = string.Empty;
    public void InitXlua()
    {
        if (luaEnv == null) luaEnv = new LuaEnv();
        
        if (!GameCenter.instance.isDevelopmentPattern)
        {
            AssetMng.instance.StartLoadScriptsConfig(assetName,(x) =>
                {
                    scripts = x;
                    ReadScriptsConfig();
                });
        }
        else
        {
            scripts = @"
xlua.private_accessible(CS.PlayGameStage)
xlua.hotfix(
CS.PlayGameStage,'DeleteInterObj',
function(self,_type,_instanceID)
        print('DeleteInterObj')
        local opc = self:GetOtherPlayer(_instanceID)
        local v = CS.UnityEngine.Vector3.zero; 
        v:SetZ(100);
end
        )";
            //scripts = string.Empty;
            ReadScriptsConfig(); 
        }
    }

    protected void ReadScriptsConfig()
    {
        try
        {
            if (string.IsNullOrEmpty(scripts))
            {
                Debug.LogWarning("the update config is null");
                return;
            }
            luaEnv.DoString(scripts);
        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e.Message);
        }
        finally
        {
            //luaEnv.Dispose();
            luaEnv = null;
        }
    }
}
