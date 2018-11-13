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
    public class GameStageUtilityWrap
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			Utils.BeginObjectRegister(typeof(GameStageUtility), L, translator, 0, 0, 0, 0);
			
			
			
			
			
			Utils.EndObjectRegister(typeof(GameStageUtility), L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(typeof(GameStageUtility), L, __CreateInstance, 8, 5, 4);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "GetTextureByType", _m_GetTextureByType_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "StartPath", _m_StartPath_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "CheckPath", _m_CheckPath_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "CheckPosition", _m_CheckPosition_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetClosestSmartActor", _m_GetClosestSmartActor_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetClosestTypeNPCDistance", _m_GetClosestTypeNPCDistance_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetCloseNPCByIndex", _m_GetCloseNPCByIndex_xlua_st_);
            
			
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "UnderlyingSystemType", typeof(GameStageUtility));
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "TaskAtlas", _g_get_TaskAtlas);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "textureDic", _g_get_textureDic);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "taskAtlas", _g_get_taskAtlas);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "custumColorShader", _g_get_custumColorShader);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "fossilShader", _g_get_fossilShader);
            
			Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "textureDic", _s_set_textureDic);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "taskAtlas", _s_set_taskAtlas);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "custumColorShader", _s_set_custumColorShader);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "fossilShader", _s_set_fossilShader);
            
			Utils.EndClassRegister(typeof(GameStageUtility), L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "GameStageUtility does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetTextureByType_xlua_st_(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
            try {
                
                {
                    AlertAreaType _type;translator.Get(L, 1, out _type);
                    
                        UnityEngine.Texture __cl_gen_ret = GameStageUtility.GetTextureByType( _type );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_StartPath_xlua_st_(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			int __gen_param_count = LuaAPI.lua_gettop(L);
            
            try {
                if(__gen_param_count == 3&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.Vector3 _startPos;translator.Get(L, 1, out _startPos);
                    UnityEngine.Vector3 _destinationPos;translator.Get(L, 2, out _destinationPos);
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 3);
                    
                        UnityEngine.Vector3[] __cl_gen_ret = GameStageUtility.StartPath( _startPos, _destinationPos, _maxDistance );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                if(__gen_param_count == 2&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)) 
                {
                    UnityEngine.Vector3 _startPos;translator.Get(L, 1, out _startPos);
                    UnityEngine.Vector3 _destinationPos;translator.Get(L, 2, out _destinationPos);
                    
                        UnityEngine.Vector3[] __cl_gen_ret = GameStageUtility.StartPath( _startPos, _destinationPos );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to GameStageUtility.StartPath!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CheckPath_xlua_st_(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
            try {
                
                {
                    UnityEngine.Vector3[] _originPath = (UnityEngine.Vector3[])translator.GetObject(L, 1, typeof(UnityEngine.Vector3[]));
                    
                        UnityEngine.Vector3[] __cl_gen_ret = GameStageUtility.CheckPath( _originPath );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CheckPosition_xlua_st_(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
            try {
                
                {
                    UnityEngine.Vector3 _from;translator.Get(L, 1, out _from);
                    UnityEngine.Vector3 _to;translator.Get(L, 2, out _to);
                    
                        bool __cl_gen_ret = GameStageUtility.CheckPosition( _from, _to );
                        LuaAPI.lua_pushboolean(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetClosestSmartActor_xlua_st_(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
            try {
                
                {
                    PlayerBase _player = (PlayerBase)translator.GetObject(L, 1, typeof(PlayerBase));
                    System.Collections.Generic.List<SmartActor> smartActors = (System.Collections.Generic.List<SmartActor>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<SmartActor>));
                    RelationType _relationType;translator.Get(L, 3, out _relationType);
                    float _instance = (float)LuaAPI.lua_tonumber(L, 4);
                    
                        SmartActor __cl_gen_ret = GameStageUtility.GetClosestSmartActor( _player, smartActors, _relationType, ref _instance );
                        translator.Push(L, __cl_gen_ret);
                    LuaAPI.lua_pushnumber(L, _instance);
                        
                    
                    
                    
                    return 2;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetClosestTypeNPCDistance_xlua_st_(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
            try {
                
                {
                    GameStage _gameStage = (GameStage)translator.GetObject(L, 1, typeof(GameStage));
                    PlayerBase _player = (PlayerBase)translator.GetObject(L, 2, typeof(PlayerBase));
                    NPCType _type;translator.Get(L, 3, out _type);
                    
                        float __cl_gen_ret = GameStageUtility.GetClosestTypeNPCDistance( _gameStage, _player, _type );
                        LuaAPI.lua_pushnumber(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetCloseNPCByIndex_xlua_st_(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
            try {
                
                {
                    GameStage _gameStage = (GameStage)translator.GetObject(L, 1, typeof(GameStage));
                    PlayerBase _player = (PlayerBase)translator.GetObject(L, 2, typeof(PlayerBase));
                    int _index = LuaAPI.xlua_tointeger(L, 3);
                    
                        NPC __cl_gen_ret = GameStageUtility.GetCloseNPCByIndex( _gameStage, _player, _index );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TaskAtlas(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			    translator.Push(L, GameStageUtility.TaskAtlas);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_textureDic(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			    translator.Push(L, GameStageUtility.textureDic);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_taskAtlas(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			    translator.Push(L, GameStageUtility.taskAtlas);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_custumColorShader(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			    translator.Push(L, GameStageUtility.custumColorShader);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_fossilShader(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			    translator.Push(L, GameStageUtility.fossilShader);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_textureDic(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			    GameStageUtility.textureDic = (System.Collections.Generic.Dictionary<AlertAreaType, UnityEngine.Texture>)translator.GetObject(L, 1, typeof(System.Collections.Generic.Dictionary<AlertAreaType, UnityEngine.Texture>));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_taskAtlas(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			    GameStageUtility.taskAtlas = (UIAtlas)translator.GetObject(L, 1, typeof(UIAtlas));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_custumColorShader(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			    GameStageUtility.custumColorShader = (UnityEngine.Shader)translator.GetObject(L, 1, typeof(UnityEngine.Shader));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_fossilShader(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			    GameStageUtility.fossilShader = (UnityEngine.Shader)translator.GetObject(L, 1, typeof(UnityEngine.Shader));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
