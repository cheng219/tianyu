#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using XLua;
using System.Collections.Generic;


namespace XLua.CSObjectWrap
{
    using Utils = XLua.Utils;
    public class GameStageWrap
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			Utils.BeginObjectRegister(typeof(GameStage), L, translator, 0, 60, 12, 9);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CacheEquipmentURL", _m_CacheEquipmentURL);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PreGuiUpdate", _m_PreGuiUpdate);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GuiUpdate", _m_GuiUpdate);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Reset", _m_Reset);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Run", _m_Run);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Pause", _m_Pause);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Resume", _m_Resume);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Restart", _m_Restart);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ReadyToStart", _m_ReadyToStart);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Over", _m_Over);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PlaceGameObjectFromSceneAnima", _m_PlaceGameObjectFromSceneAnima);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PlaceGameObjectFromServer", _m_PlaceGameObjectFromServer);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PlaceGameObjectFromStaticRef", _m_PlaceGameObjectFromStaticRef);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InitSector", _m_InitSector);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsSectorInRange", _m_IsSectorInRange);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetSector", _m_GetSector);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetSectorByPosition", _m_GetSectorByPosition);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetCenterPosBySector", _m_GetCenterPosBySector);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetSectorRowCol", _m_GetSectorRowCol);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "DrawSector", _m_DrawSector);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "DrawSectorObjectCountAt", _m_DrawSectorObjectCountAt);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnPositionChanged", _m_OnPositionChanged);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "UnRegist", _m_UnRegist);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "UnRegistAll", _m_UnRegistAll);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddObject", _m_AddObject);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RemoveObject", _m_RemoveObject);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetObject", _m_GetObject);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetPlayerBase", _m_GetPlayerBase);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetOtherPlayer", _m_GetOtherPlayer);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetOtherEntourage", _m_GetOtherEntourage);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetNPC", _m_GetNPC);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetMOB", _m_GetMOB);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetEntourage", _m_GetEntourage);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetDropItem", _m_GetDropItem);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetTrap", _m_GetTrap);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetSceneItem", _m_GetSceneItem);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetInterActiveObj", _m_GetInterActiveObj);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetNPCs", _m_GetNPCs);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetFlypoints", _m_GetFlypoints);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetOtherPlayers", _m_GetOtherPlayers);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetOtherEntourages", _m_GetOtherEntourages);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetDropItems", _m_GetDropItems);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetSceneItems", _m_GetSceneItems);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetModels", _m_GetModels);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetTraps", _m_GetTraps);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetMobs", _m_GetMobs);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetSmartActor", _m_GetSmartActor);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetCullNPCs", _m_GetCullNPCs);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetCullDummyNPCs", _m_GetCullDummyNPCs);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetCullDummyFlypoints", _m_GetCullDummyFlypoints);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetAnotherSmartActor", _m_GetAnotherSmartActor);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetAnotherMob", _m_GetAnotherMob);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetClosestMob", _m_GetClosestMob);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetClosestPlayer", _m_GetClosestPlayer);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetClosestEntourage", _m_GetClosestEntourage);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetClosestSmartActorInFront", _m_GetClosestSmartActorInFront);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetClosestSmartActor", _m_GetClosestSmartActor);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetSmartActors", _m_GetSmartActors);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetClosestTypeNPC", _m_GetClosestTypeNPC);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetClosestSceneItem", _m_GetClosestSceneItem);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "SceneType", _g_get_SceneType);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SceneID", _g_get_SceneID);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "deinited", _g_get_deinited);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "sceneName", _g_get_sceneName);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "stageName", _g_get_stageName);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "sceneMng", _g_get_sceneMng);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "myLastIP", _g_get_myLastIP);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "startWidthPos", _g_get_startWidthPos);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "startLengthPos", _g_get_startLengthPos);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "width", _g_get_width);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "length", _g_get_length);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "scale", _g_get_scale);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "sceneName", _s_set_sceneName);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "stageName", _s_set_stageName);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "sceneMng", _s_set_sceneMng);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "myLastIP", _s_set_myLastIP);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "startWidthPos", _s_set_startWidthPos);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "startLengthPos", _s_set_startLengthPos);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "width", _s_set_width);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "length", _s_set_length);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "scale", _s_set_scale);
            
			Utils.EndObjectRegister(typeof(GameStage), L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(typeof(GameStage), L, __CreateInstance, 1, 0, 0);
			
			
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "UnderlyingSystemType", typeof(GameStage));
			
			
			Utils.EndClassRegister(typeof(GameStage), L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			try {
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					GameStage __cl_gen_ret = new GameStage();
					translator.Push(L, __cl_gen_ret);
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to GameStage constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CacheEquipmentURL(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    string _url = LuaAPI.lua_tostring(L, 2);
                    UnityEngine.Object _obj = (UnityEngine.Object)translator.GetObject(L, 3, typeof(UnityEngine.Object));
                    
                    __cl_gen_to_be_invoked.CacheEquipmentURL( _url, _obj );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PreGuiUpdate(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    
                    __cl_gen_to_be_invoked.PreGuiUpdate(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GuiUpdate(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    
                    __cl_gen_to_be_invoked.GuiUpdate(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Reset(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    
                    __cl_gen_to_be_invoked.Reset(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Run(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    
                    __cl_gen_to_be_invoked.Run(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Pause(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    
                    __cl_gen_to_be_invoked.Pause(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Resume(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    
                    __cl_gen_to_be_invoked.Resume(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Restart(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    
                    __cl_gen_to_be_invoked.Restart(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReadyToStart(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    
                    __cl_gen_to_be_invoked.ReadyToStart(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Over(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    
                    __cl_gen_to_be_invoked.Over(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PlaceGameObjectFromSceneAnima(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
            
            
			int __gen_param_count = LuaAPI.lua_gettop(L);
            
            try {
                if(__gen_param_count == 5&& translator.Assignable<UnityEngine.GameObject>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)) 
                {
                    UnityEngine.GameObject _go = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
                    int _x = LuaAPI.xlua_tointeger(L, 3);
                    int _y = LuaAPI.xlua_tointeger(L, 4);
                    int _rotation = LuaAPI.xlua_tointeger(L, 5);
                    
                    __cl_gen_to_be_invoked.PlaceGameObjectFromSceneAnima( _go, _x, _y, _rotation );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 4&& translator.Assignable<UnityEngine.GameObject>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    UnityEngine.GameObject _go = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
                    int _x = LuaAPI.xlua_tointeger(L, 3);
                    int _y = LuaAPI.xlua_tointeger(L, 4);
                    
                    __cl_gen_to_be_invoked.PlaceGameObjectFromSceneAnima( _go, _x, _y );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 3&& translator.Assignable<UnityEngine.GameObject>(L, 2)&& translator.Assignable<UnityEngine.GameObject>(L, 3)) 
                {
                    UnityEngine.GameObject _goA = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
                    UnityEngine.GameObject _goB = (UnityEngine.GameObject)translator.GetObject(L, 3, typeof(UnityEngine.GameObject));
                    
                    __cl_gen_to_be_invoked.PlaceGameObjectFromSceneAnima( _goA, _goB );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to GameStage.PlaceGameObjectFromSceneAnima!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PlaceGameObjectFromServer(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
            
            
			int __gen_param_count = LuaAPI.lua_gettop(L);
            
            try {
                if(__gen_param_count == 6&& translator.Assignable<InteractiveObject>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)) 
                {
                    InteractiveObject _go = (InteractiveObject)translator.GetObject(L, 2, typeof(InteractiveObject));
                    float _x = (float)LuaAPI.lua_tonumber(L, 3);
                    float _y = (float)LuaAPI.lua_tonumber(L, 4);
                    int _rotation = LuaAPI.xlua_tointeger(L, 5);
                    float _hight = (float)LuaAPI.lua_tonumber(L, 6);
                    
                    __cl_gen_to_be_invoked.PlaceGameObjectFromServer( _go, _x, _y, _rotation, _hight );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 5&& translator.Assignable<InteractiveObject>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)) 
                {
                    InteractiveObject _go = (InteractiveObject)translator.GetObject(L, 2, typeof(InteractiveObject));
                    float _x = (float)LuaAPI.lua_tonumber(L, 3);
                    float _y = (float)LuaAPI.lua_tonumber(L, 4);
                    int _rotation = LuaAPI.xlua_tointeger(L, 5);
                    
                    __cl_gen_to_be_invoked.PlaceGameObjectFromServer( _go, _x, _y, _rotation );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to GameStage.PlaceGameObjectFromServer!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PlaceGameObjectFromStaticRef(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
            
            
			int __gen_param_count = LuaAPI.lua_gettop(L);
            
            try {
                if(__gen_param_count == 6&& translator.Assignable<InteractiveObject>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)) 
                {
                    InteractiveObject _go = (InteractiveObject)translator.GetObject(L, 2, typeof(InteractiveObject));
                    int _x = LuaAPI.xlua_tointeger(L, 3);
                    int _y = LuaAPI.xlua_tointeger(L, 4);
                    int _rotation = LuaAPI.xlua_tointeger(L, 5);
                    float _hight = (float)LuaAPI.lua_tonumber(L, 6);
                    
                    __cl_gen_to_be_invoked.PlaceGameObjectFromStaticRef( _go, _x, _y, _rotation, _hight );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 5&& translator.Assignable<InteractiveObject>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)) 
                {
                    InteractiveObject _go = (InteractiveObject)translator.GetObject(L, 2, typeof(InteractiveObject));
                    int _x = LuaAPI.xlua_tointeger(L, 3);
                    int _y = LuaAPI.xlua_tointeger(L, 4);
                    int _rotation = LuaAPI.xlua_tointeger(L, 5);
                    
                    __cl_gen_to_be_invoked.PlaceGameObjectFromStaticRef( _go, _x, _y, _rotation );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to GameStage.PlaceGameObjectFromStaticRef!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InitSector(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
            
            
			int __gen_param_count = LuaAPI.lua_gettop(L);
            
            try {
                if(__gen_param_count == 7&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 7)) 
                {
                    int _width = LuaAPI.xlua_tointeger(L, 2);
                    int _length = LuaAPI.xlua_tointeger(L, 3);
                    int _scale = LuaAPI.xlua_tointeger(L, 4);
                    int _sectorSize = LuaAPI.xlua_tointeger(L, 5);
                    int _startWidthPos = LuaAPI.xlua_tointeger(L, 6);
                    int _startLengthPos = LuaAPI.xlua_tointeger(L, 7);
                    
                    __cl_gen_to_be_invoked.InitSector( _width, _length, _scale, _sectorSize, _startWidthPos, _startLengthPos );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 6&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)) 
                {
                    int _width = LuaAPI.xlua_tointeger(L, 2);
                    int _length = LuaAPI.xlua_tointeger(L, 3);
                    int _scale = LuaAPI.xlua_tointeger(L, 4);
                    int _sectorSize = LuaAPI.xlua_tointeger(L, 5);
                    int _startWidthPos = LuaAPI.xlua_tointeger(L, 6);
                    
                    __cl_gen_to_be_invoked.InitSector( _width, _length, _scale, _sectorSize, _startWidthPos );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 5&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)) 
                {
                    int _width = LuaAPI.xlua_tointeger(L, 2);
                    int _length = LuaAPI.xlua_tointeger(L, 3);
                    int _scale = LuaAPI.xlua_tointeger(L, 4);
                    int _sectorSize = LuaAPI.xlua_tointeger(L, 5);
                    
                    __cl_gen_to_be_invoked.InitSector( _width, _length, _scale, _sectorSize );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 4&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    int _width = LuaAPI.xlua_tointeger(L, 2);
                    int _length = LuaAPI.xlua_tointeger(L, 3);
                    int _scale = LuaAPI.xlua_tointeger(L, 4);
                    
                    __cl_gen_to_be_invoked.InitSector( _width, _length, _scale );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    int _width = LuaAPI.xlua_tointeger(L, 2);
                    int _length = LuaAPI.xlua_tointeger(L, 3);
                    
                    __cl_gen_to_be_invoked.InitSector( _width, _length );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to GameStage.InitSector!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsSectorInRange(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    GameStage.Sector _check = (GameStage.Sector)translator.GetObject(L, 2, typeof(GameStage.Sector));
                    GameStage.Sector _center = (GameStage.Sector)translator.GetObject(L, 3, typeof(GameStage.Sector));
                    int _range = LuaAPI.xlua_tointeger(L, 4);
                    
                        bool __cl_gen_ret = __cl_gen_to_be_invoked.IsSectorInRange( _check, _center, _range );
                        LuaAPI.lua_pushboolean(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetSector(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    int _r = LuaAPI.xlua_tointeger(L, 2);
                    int _c = LuaAPI.xlua_tointeger(L, 3);
                    
                        GameStage.Sector __cl_gen_ret = __cl_gen_to_be_invoked.GetSector( _r, _c );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetSectorByPosition(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    UnityEngine.Vector3 _pos;translator.Get(L, 2, out _pos);
                    
                        GameStage.Sector __cl_gen_ret = __cl_gen_to_be_invoked.GetSectorByPosition( _pos );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetCenterPosBySector(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    GameStage.Sector _sector = (GameStage.Sector)translator.GetObject(L, 2, typeof(GameStage.Sector));
                    
                        UnityEngine.Vector3 __cl_gen_ret = __cl_gen_to_be_invoked.GetCenterPosBySector( _sector );
                        translator.PushUnityEngineVector3(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetSectorRowCol(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    GameStage.Sector _sector = (GameStage.Sector)translator.GetObject(L, 2, typeof(GameStage.Sector));
                    int _row;
                    int _col;
                    
                    __cl_gen_to_be_invoked.GetSectorRowCol( _sector, out _row, out _col );
                    LuaAPI.xlua_pushinteger(L, _row);
                        
                    LuaAPI.xlua_pushinteger(L, _col);
                        
                    
                    
                    
                    return 2;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DrawSector(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    float _height = (float)LuaAPI.lua_tonumber(L, 2);
                    
                    __cl_gen_to_be_invoked.DrawSector( _height );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DrawSectorObjectCountAt(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
            
            
			int __gen_param_count = LuaAPI.lua_gettop(L);
            
            try {
                if(__gen_param_count == 5&& translator.Assignable<GameStage.Sector>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)) 
                {
                    GameStage.Sector _sector = (GameStage.Sector)translator.GetObject(L, 2, typeof(GameStage.Sector));
                    float _height = (float)LuaAPI.lua_tonumber(L, 3);
                    int _rowCount = LuaAPI.xlua_tointeger(L, 4);
                    int _colCount = LuaAPI.xlua_tointeger(L, 5);
                    
                    __cl_gen_to_be_invoked.DrawSectorObjectCountAt( _sector, _height, _rowCount, _colCount );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 4&& translator.Assignable<GameStage.Sector>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    GameStage.Sector _sector = (GameStage.Sector)translator.GetObject(L, 2, typeof(GameStage.Sector));
                    float _height = (float)LuaAPI.lua_tonumber(L, 3);
                    int _rowCount = LuaAPI.xlua_tointeger(L, 4);
                    
                    __cl_gen_to_be_invoked.DrawSectorObjectCountAt( _sector, _height, _rowCount );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 3&& translator.Assignable<GameStage.Sector>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    GameStage.Sector _sector = (GameStage.Sector)translator.GetObject(L, 2, typeof(GameStage.Sector));
                    float _height = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    __cl_gen_to_be_invoked.DrawSectorObjectCountAt( _sector, _height );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to GameStage.DrawSectorObjectCountAt!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnPositionChanged(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    InteractiveObject _object = (InteractiveObject)translator.GetObject(L, 2, typeof(InteractiveObject));
                    
                    __cl_gen_to_be_invoked.OnPositionChanged( _object );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UnRegist(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    
                    __cl_gen_to_be_invoked.UnRegist(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UnRegistAll(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    
                    __cl_gen_to_be_invoked.UnRegistAll(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddObject(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    InteractiveObject _object = (InteractiveObject)translator.GetObject(L, 2, typeof(InteractiveObject));
                    
                    __cl_gen_to_be_invoked.AddObject( _object );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemoveObject(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    InteractiveObject _object = (InteractiveObject)translator.GetObject(L, 2, typeof(InteractiveObject));
                    
                    __cl_gen_to_be_invoked.RemoveObject( _object );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetObject(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    ObjectType _type;translator.Get(L, 2, out _type);
                    int _id = LuaAPI.xlua_tointeger(L, 3);
                    
                        InteractiveObject __cl_gen_ret = __cl_gen_to_be_invoked.GetObject( _type, _id );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetPlayerBase(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    int _id = LuaAPI.xlua_tointeger(L, 2);
                    
                        PlayerBase __cl_gen_ret = __cl_gen_to_be_invoked.GetPlayerBase( _id );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetOtherPlayer(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    int _id = LuaAPI.xlua_tointeger(L, 2);
                    
                        OtherPlayer __cl_gen_ret = __cl_gen_to_be_invoked.GetOtherPlayer( _id );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetOtherEntourage(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    int _id = LuaAPI.xlua_tointeger(L, 2);
                    
                        OtherEntourage __cl_gen_ret = __cl_gen_to_be_invoked.GetOtherEntourage( _id );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetNPC(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    int _id = LuaAPI.xlua_tointeger(L, 2);
                    
                        NPC __cl_gen_ret = __cl_gen_to_be_invoked.GetNPC( _id );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetMOB(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    int _id = LuaAPI.xlua_tointeger(L, 2);
                    
                        Monster __cl_gen_ret = __cl_gen_to_be_invoked.GetMOB( _id );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetEntourage(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    int _id = LuaAPI.xlua_tointeger(L, 2);
                    
                        EntourageBase __cl_gen_ret = __cl_gen_to_be_invoked.GetEntourage( _id );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetDropItem(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    int _id = LuaAPI.xlua_tointeger(L, 2);
                    
                        DropItem __cl_gen_ret = __cl_gen_to_be_invoked.GetDropItem( _id );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetTrap(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    int _id = LuaAPI.xlua_tointeger(L, 2);
                    
                        Trap __cl_gen_ret = __cl_gen_to_be_invoked.GetTrap( _id );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetSceneItem(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    int _id = LuaAPI.xlua_tointeger(L, 2);
                    
                        SceneItem __cl_gen_ret = __cl_gen_to_be_invoked.GetSceneItem( _id );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetInterActiveObj(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    int _id = LuaAPI.xlua_tointeger(L, 2);
                    
                        InteractiveObject __cl_gen_ret = __cl_gen_to_be_invoked.GetInterActiveObj( _id );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetNPCs(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    
                        System.Collections.Generic.List<NPC> __cl_gen_ret = __cl_gen_to_be_invoked.GetNPCs(  );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetFlypoints(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    
                        System.Collections.Generic.List<FlyPoint> __cl_gen_ret = __cl_gen_to_be_invoked.GetFlypoints(  );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetOtherPlayers(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    
                        System.Collections.Generic.List<OtherPlayer> __cl_gen_ret = __cl_gen_to_be_invoked.GetOtherPlayers(  );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetOtherEntourages(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    
                        System.Collections.Generic.List<OtherEntourage> __cl_gen_ret = __cl_gen_to_be_invoked.GetOtherEntourages(  );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetDropItems(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    
                        System.Collections.Generic.List<DropItem> __cl_gen_ret = __cl_gen_to_be_invoked.GetDropItems(  );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetSceneItems(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    
                        System.Collections.Generic.List<SceneItem> __cl_gen_ret = __cl_gen_to_be_invoked.GetSceneItems(  );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetModels(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    
                        System.Collections.Generic.List<Model> __cl_gen_ret = __cl_gen_to_be_invoked.GetModels(  );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetTraps(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    
                        System.Collections.Generic.List<Trap> __cl_gen_ret = __cl_gen_to_be_invoked.GetTraps(  );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetMobs(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    
                        System.Collections.Generic.List<Monster> __cl_gen_ret = __cl_gen_to_be_invoked.GetMobs(  );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetSmartActor(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    
                        System.Collections.Generic.List<SmartActor> __cl_gen_ret = __cl_gen_to_be_invoked.GetSmartActor(  );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetCullNPCs(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    MainPlayer _player = (MainPlayer)translator.GetObject(L, 2, typeof(MainPlayer));
                    
                        System.Collections.Generic.List<NPC> __cl_gen_ret = __cl_gen_to_be_invoked.GetCullNPCs( _player );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetCullDummyNPCs(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    MainPlayer _player = (MainPlayer)translator.GetObject(L, 2, typeof(MainPlayer));
                    
                        System.Collections.Generic.List<NPC> __cl_gen_ret = __cl_gen_to_be_invoked.GetCullDummyNPCs( _player );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetCullDummyFlypoints(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    MainPlayer _player = (MainPlayer)translator.GetObject(L, 2, typeof(MainPlayer));
                    
                        System.Collections.Generic.List<FlyPoint> __cl_gen_ret = __cl_gen_to_be_invoked.GetCullDummyFlypoints( _player );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAnotherSmartActor(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    int _old = LuaAPI.xlua_tointeger(L, 2);
                    UnityEngine.Vector3 _comparePos;translator.Get(L, 3, out _comparePos);
                    
                        SmartActor __cl_gen_ret = __cl_gen_to_be_invoked.GetAnotherSmartActor( _old, _comparePos );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAnotherMob(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    int _old = LuaAPI.xlua_tointeger(L, 2);
                    
                        Monster __cl_gen_ret = __cl_gen_to_be_invoked.GetAnotherMob( _old );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetClosestMob(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
            
            
			int __gen_param_count = LuaAPI.lua_gettop(L);
            
            try {
                if(__gen_param_count == 2&& translator.Assignable<SmartActor>(L, 2)) 
                {
                    SmartActor _player = (SmartActor)translator.GetObject(L, 2, typeof(SmartActor));
                    
                        Monster __cl_gen_ret = __cl_gen_to_be_invoked.GetClosestMob( _player );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                if(__gen_param_count == 3&& translator.Assignable<SmartActor>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    SmartActor _player = (SmartActor)translator.GetObject(L, 2, typeof(SmartActor));
                    float _distance = (float)LuaAPI.lua_tonumber(L, 3);
                    
                        Monster __cl_gen_ret = __cl_gen_to_be_invoked.GetClosestMob( _player, ref _distance );
                        translator.Push(L, __cl_gen_ret);
                    LuaAPI.lua_pushnumber(L, _distance);
                        
                    
                    
                    
                    return 2;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to GameStage.GetClosestMob!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetClosestPlayer(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    SmartActor _player = (SmartActor)translator.GetObject(L, 2, typeof(SmartActor));
                    RelationType _relationType;translator.Get(L, 3, out _relationType);
                    float _distance = (float)LuaAPI.lua_tonumber(L, 4);
                    
                        OtherPlayer __cl_gen_ret = __cl_gen_to_be_invoked.GetClosestPlayer( _player, _relationType, ref _distance );
                        translator.Push(L, __cl_gen_ret);
                    LuaAPI.lua_pushnumber(L, _distance);
                        
                    
                    
                    
                    return 2;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetClosestEntourage(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    SmartActor _player = (SmartActor)translator.GetObject(L, 2, typeof(SmartActor));
                    RelationType _relationType;translator.Get(L, 3, out _relationType);
                    float _distance = (float)LuaAPI.lua_tonumber(L, 4);
                    
                        OtherEntourage __cl_gen_ret = __cl_gen_to_be_invoked.GetClosestEntourage( _player, _relationType, ref _distance );
                        translator.Push(L, __cl_gen_ret);
                    LuaAPI.lua_pushnumber(L, _distance);
                        
                    
                    
                    
                    return 2;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetClosestSmartActorInFront(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    PlayerBase _player = (PlayerBase)translator.GetObject(L, 2, typeof(PlayerBase));
                    RelationType _relationType;translator.Get(L, 3, out _relationType);
                    float _distance = (float)LuaAPI.lua_tonumber(L, 4);
                    
                        SmartActor __cl_gen_ret = __cl_gen_to_be_invoked.GetClosestSmartActorInFront( _player, _relationType, ref _distance );
                        translator.Push(L, __cl_gen_ret);
                    LuaAPI.lua_pushnumber(L, _distance);
                        
                    
                    
                    
                    return 2;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetClosestSmartActor(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    PlayerBase _player = (PlayerBase)translator.GetObject(L, 2, typeof(PlayerBase));
                    RelationType _relationType;translator.Get(L, 3, out _relationType);
                    float _distance = (float)LuaAPI.lua_tonumber(L, 4);
                    
                        SmartActor __cl_gen_ret = __cl_gen_to_be_invoked.GetClosestSmartActor( _player, _relationType, ref _distance );
                        translator.Push(L, __cl_gen_ret);
                    LuaAPI.lua_pushnumber(L, _distance);
                        
                    
                    
                    
                    return 2;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetSmartActors(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    
                        System.Collections.Generic.List<SmartActor> __cl_gen_ret = __cl_gen_to_be_invoked.GetSmartActors(  );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetClosestTypeNPC(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    PlayerBase _player = (PlayerBase)translator.GetObject(L, 2, typeof(PlayerBase));
                    NPCType _type;translator.Get(L, 3, out _type);
                    
                        NPC __cl_gen_ret = __cl_gen_to_be_invoked.GetClosestTypeNPC( _player, _type );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetClosestSceneItem(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    PlayerBase _player = (PlayerBase)translator.GetObject(L, 2, typeof(PlayerBase));
                    
                        SceneItem __cl_gen_ret = __cl_gen_to_be_invoked.GetClosestSceneItem( _player );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SceneType(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.SceneType);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SceneID(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, __cl_gen_to_be_invoked.SceneID);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_deinited(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, __cl_gen_to_be_invoked.deinited);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_sceneName(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, __cl_gen_to_be_invoked.sceneName);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_stageName(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, __cl_gen_to_be_invoked.stageName);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_sceneMng(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.sceneMng);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_myLastIP(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, __cl_gen_to_be_invoked.myLastIP);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_startWidthPos(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, __cl_gen_to_be_invoked.startWidthPos);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_startLengthPos(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, __cl_gen_to_be_invoked.startLengthPos);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_width(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, __cl_gen_to_be_invoked.width);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_length(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, __cl_gen_to_be_invoked.length);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_scale(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, __cl_gen_to_be_invoked.scale);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_sceneName(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.sceneName = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_stageName(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.stageName = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_sceneMng(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.sceneMng = (SceneMng)translator.GetObject(L, 2, typeof(SceneMng));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_myLastIP(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.myLastIP = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_startWidthPos(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.startWidthPos = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_startLengthPos(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.startLengthPos = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_width(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.width = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_length(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.length = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_scale(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                GameStage __cl_gen_to_be_invoked = (GameStage)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.scale = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
