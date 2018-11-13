///////////////////////////////////////////////////////////////////////////////////////////
//作者：沙新佳 qq：287490904
//最后修改时间：2015/9/2
//脚本描述：反光shader 按高光区域 和 法线
//////////////////////////////////
// Unlit alpha-cutout shader.
// - no lighting
// - no lightmap support
// - no per-material color

Shader "Unlit/Transparent Cutout/cubeReflective" {
Properties {
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
	_BumpMap ("Normalmap", 2D) = "bump" {}
	_ReflectColor ("Reflection Color", Color) = (1,1,1,0.5)
	_Cube ("Reflection Cubemap", Cube) = "" { /* used to be TexGen CubeNormal */ }
	_Spec_Gloss_Reflec_Masks ("Spec(R) Gloss(G) Reflec(B)",2D) = "White" {}
	
		_FrezPow("Fresnel Reflection",Range(0,2)) = .25
	_FrezFalloff("Fresnal/EdgeAlpha Falloff",Range(0,10)) = 4
	_Metalics("Metalics",Range(0,1)) = .5
}
SubShader {
	Tags {"Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout"}
	LOD 100

	//Lighting Off
	//UsePass "Legacy Shaders/Reflective/Bumped Unlit Mask"
	UsePass "Legacy Shaders/Reflective/Bumped Unlit Mask/BASEMASK"
	Pass {  
			Blend One One ZWrite Off
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct v2f {
				float4 vertex : SV_POSITION;
				//float3 normal : NORMAL;
				
				half2 texcoord : TEXCOORD0;
				float3  texcoord2 : TEXCOORD2;
				UNITY_FOG_COORDS(1)
			};

			sampler2D _MainTex;
			//sampler2D _BumpMap;
			//sampler2D _Spec_Gloss_Reflec_Masks;
			float4 _MainTex_ST;
			fixed _Cutoff;
			uniform fixed4 _ReflectColor;
			//fixed _Reflection;
			v2f vert (appdata_base v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
//				float3 viewDir = normalize(ObjSpaceViewDir(v.vertex));
//           	 	o.texcoord2 = reflect(-viewDir, v.normal);
//           		o.texcoord2 = mul(UNITY_MATRIX_MV, float4(o.texcoord2,0));

				    float3 viewDir = WorldSpaceViewDir( v.vertex );
					float3 worldN = UnityObjectToWorldNormal( v.normal );
					o.texcoord2 = reflect( -viewDir, worldN );
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			samplerCUBE _Cube;

			float4 frag (v2f i) : SV_Target
			{
				float4 col = tex2D(_MainTex, i.texcoord);
				// half4 colcube =  texCUBE(_Cube, i.texcoord2)*_ReflectColor*_ReflectColor.a;
				// col = col+colcube;
//				   col = lerp(col, colcube, colcube.r*colcube.g*colcube.b*_Reflection);
				clip(col.a - _Cutoff);
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
		ENDCG
	}

}
FallBack  "Mobile/VertexLit" 
}
