
///////////////////////////////////////////////////////////////////////////////////////////
//作者：沙新佳 qq：287490904
//最后修改时间：2015/9/2
//脚本描述：反光shader 按高光区域 添加Rim
//////////////////////////////////
// Unlit alpha-cutout shader.
// - no lighting
// - no lightmap support
// - no per-material color

Shader "Unlit/Transparent Cutout/cubeRim" {
Properties {
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
	_ReflectColor ("Reflection Color", Color) = (1,1,1,0.5)
	_Cube ("Reflection Cubemap", Cube) = "" { /* used to be TexGen CubeNormal */ }
	_Spec_Gloss_Reflec_Masks ("Spec(R) Gloss(G) Reflec(B)",2D) = "White" {}
	_RimColor ("Rim Color", Color) = (0.238, 0.414, 0.652, 1)
	 _RimWidth ("Rim Width", Float) = 0.1
}
SubShader {
	Tags {"Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout"}
	LOD 100

	Lighting Off

	Pass {  
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata_t {
				float4 vertex : POSITION;
				float4 normal : NORMAL;
				float2 texcoord : TEXCOORD0;
				float3  texcoord1 : TEXCOORD1;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				//float4 normal : NORMAL;
				fixed3 color : COLOR;
				fixed2 texcoord : TEXCOORD0;
				float3  texcoord2 : TEXCOORD2;
				UNITY_FOG_COORDS(1)
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed _Cutoff;
			fixed _Reflection;
			uniform fixed4 _ReflectColor;
			sampler2D _Spec_Gloss_Reflec_Masks;
			uniform fixed4 _RimColor;
			float _RimWidth;
			v2f vert (appdata_base v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				//o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.texcoord =v.texcoord.xy;
//					float3 viewDir = normalize(ObjSpaceViewDir(v.vertex));
//           	 	o.texcoord2 = reflect(-viewDir, v.normal);
//           		o.texcoord2 = mul(UNITY_MATRIX_MV, float4(o.texcoord2,0));
           		
           			float3 viewDir = normalize(WorldSpaceViewDir( v.vertex ));
					float3 worldN = UnityObjectToWorldNormal( v.normal );
					//o.texcoord2 = reflect( -viewDir, worldN );
           		
           			float3 viewDir2 = normalize(ObjSpaceViewDir(v.vertex));
           			o.texcoord2 = reflect( -viewDir2, worldN );
           			float dotProduct = 1 - dot(v.normal, viewDir2);                  
                    o.color = smoothstep(1 - _RimWidth, 1.0, dotProduct);
                    o.color *= _RimColor;
           		
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			samplerCUBE _Cube;

			half4 frag (v2f i) : SV_Target
			{
				half4 col = tex2D(_MainTex, i.texcoord);
				 half4 colcube =  texCUBE(_Cube, i.texcoord2)*_ReflectColor*_ReflectColor.a;
				 half4 sgr = tex2D(_Spec_Gloss_Reflec_Masks,i.texcoord);
				 col.rgb =col.rgb+ i.color*sgr.r;
				 col = col+colcube*sgr.b;
				// col = lerp(col, colcube, colcube.r*colcube.g*colcube.b*_Reflection);
				clip(col.a - _Cutoff);
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
		ENDCG
	}
	
	// Pass to render object as a shadow caster
	Pass 
	{
		Name "ShadowCaster"
		Tags { "LightMode" = "ShadowCaster" }
		
		ZWrite On ZTest LEqual Cull Off

		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma multi_compile_shadowcaster
		#include "UnityCG.cginc"

		struct v2f { 
			V2F_SHADOW_CASTER;
			float2 uv : TEXCOORD1;
		};

		v2f vert( appdata_base v )
		{
			v2f o;
			TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
			o.uv = v.texcoord;
			return o;
		}
		sampler2D _MainTex;
		fixed _Cutoff;
		float4 frag( v2f i ) : SV_Target
		{
			fixed4 texcol = tex2D( _MainTex, i.uv );
			clip( texcol.a - _Cutoff );
			SHADOW_CASTER_FRAGMENT(i)
		}
		ENDCG
	}	
}
	Fallback Off
	//FallBack  "Mobile/VertexLit"
}
