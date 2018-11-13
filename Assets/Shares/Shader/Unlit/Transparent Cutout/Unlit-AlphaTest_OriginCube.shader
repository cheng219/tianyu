///////////////////////////////////////////////////////////////////////////////////////////
//作者：沙新佳 qq：287490904
//最后修改时间：2015/9/2
//脚本描述：反光shader 
//////////////////////////////////
// Unlit alpha-cutout shader.
// - no lighting
// - no lightmap support
// - no per-material color

Shader "Unlit/Transparent Cutout/Origincube" {
Properties {
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
	_Reflection("Reflection", Range (0.00, 1)) = 0.5
	_Cube ("Reflection Cubemap", Cube) = "" { /* used to be TexGen CubeNormal */ }
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
				half2 texcoord : TEXCOORD0;
				float3  texcoord1 : TEXCOORD1;
				UNITY_FOG_COORDS(1)
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed _Cutoff;
			fixed _Reflection;
			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				float3 viewDir = normalize(ObjSpaceViewDir(v.vertex));
           	 	o.texcoord1 = reflect(-viewDir, v.normal);
           		 o.texcoord1 = mul(UNITY_MATRIX_MV, float4(o.texcoord1,0));
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			samplerCUBE _Cube;

			half4 frag (v2f i) : SV_Target
			{
				half4 col = tex2D(_MainTex, i.texcoord);
				 half4 colcube =  texCUBE(_Cube, i.texcoord1);
				 //col = col+colcube*0.4;
				   col = lerp(col, colcube, colcube.r*colcube.g*colcube.b*_Reflection);
				clip(col.a - _Cutoff);
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
		ENDCG
	}
}

}
