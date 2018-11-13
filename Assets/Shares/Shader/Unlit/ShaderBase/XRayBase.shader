///////////////////////////////////////////////////////////////////////////////////////////
//作者：沙新佳 qq：287490904
//最后修改时间：2015/9/2
//脚本描述：xray基础pass
//////////////////////////////////

Shader "Hidden/XRayBase"
{
Properties
{
   // _MainTex ("Base (RGB)", 2D) = "white" {}
    _MatCap ("MatCap (RGB)", 2D) = "white" {}
    _XrayColor("_Color", Color) = (1,1,1,0.2)
}
 Category 
 	{
	    Subshader
	    {
	        Pass
	        {
	        	Name "XRAY_VF"
	 			Tags { "Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="Transparent"}  
	            Blend SrcAlpha One  
	            Cull Off
	            Lighting Off
	            ZWrite Off
	            Ztest GEqual 
	 
	            CGPROGRAM
	                #pragma vertex vert
	                #pragma fragment frag
	                #pragma fragmentoption ARB_precision_hint_fastest
	                #include "UnityCG.cginc"
	 
	                struct v2f
	                {
	                    float4 pos    : SV_POSITION;
	                    float2 cap    : TEXCOORD0;
	                };
	 
	                v2f vert (appdata_base v)
	                {
	                    v2f o;
	                    o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
	 
	                    half2 capCoord;
	                    capCoord.x = dot(UNITY_MATRIX_IT_MV[0].xyz,v.normal);
	                    capCoord.y = dot(UNITY_MATRIX_IT_MV[1].xyz,v.normal);
	                    o.cap = capCoord * 0.5 + 0.5;
	 
	                    return o;
	                }
	 
	                uniform float4 _XrayColor;
	                uniform sampler2D _MatCap;
	 
	                float4 frag (v2f i) : COLOR
	                {
	                    float4 mc = tex2D(_MatCap, i.cap);
	 
	                    return _XrayColor * mc *_XrayColor.a;
	                }
	            ENDCG
	        }
	        
		  	Pass
			{
			Name "XRAY_VF_R"
			Tags { "Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="Transparent"}  
				Blend SrcAlpha One
				//Blend SrcAlpha OneMinusSrcColor
//				Blend oneminusdstcolor one
				ZWrite off
				Lighting off
				 Ztest GEqual   
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"

				float4 _XrayColor;
				
				struct appdata_t {
					float4 vertex : POSITION;
					float2 texcoord : TEXCOORD0;
					float4 color:COLOR;
					float4 normal:NORMAL;
				};

				struct v2f {
					float4  pos : SV_POSITION;
					float4	color:COLOR;
				} ;
				v2f vert (appdata_t v)
				{
					v2f o;
					o.pos = mul(UNITY_MATRIX_MVP,v.vertex);
					float3 viewDir = normalize(ObjSpaceViewDir(v.vertex));
	                		float rim = 1 - saturate(dot(viewDir,v.normal ));
	               			o.color = _XrayColor*pow(rim,_XrayColor.a*4);
					return o;
				}
				float4 frag (v2f i) : COLOR
				{
					return i.color; 
				}
				ENDCG
			}
			

    	}
    FallBack "Mobile/VertexLit"
	}
}