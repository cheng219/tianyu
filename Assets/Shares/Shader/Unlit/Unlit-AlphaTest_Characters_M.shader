///////////////////////////////////////////////////////////////////////////////////////////
//作者：沙新佳 qq：287490904
//最后修改时间：2015/9/2
//脚本描述：基本色，cutout，
//////////////////////////////////
// Unlit alpha-cutout shader.
// - no lighting
// - no lightmap support
// - no per-material color

Shader "Unlit/Transparent Cutout Characters_M" {
Properties {
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5

	[HideInInspector]
	[Space(50)]_MatCap ("Xray Dec  MatCap (RGB)", 2D) = "white" {}
	[HideInInspector]
	 _XrayColor("XrayColor", Color) = (1,1,1,0.5)
}
SubShader {
	//Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="TransparentCutout"} 
	LOD 100
	Lighting Off
	//UsePass "Hidden/XRayBase/XRAY_VF_R"	//UsePass "Hidden/XRayBase/XRAY_VF_R"
	Pass { 
		Tags {"Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout"} 
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
				half2 texcoord : TEXCOORD0;
				//float3  texcoord2 : TEXCOORD2;
				UNITY_FOG_COORDS(1)
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed _Cutoff;
			v2f vert (appdata_base v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);  
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			samplerCUBE _Cube;

			half4 frag (v2f i) : SV_Target
			{
				half4 col = tex2D(_MainTex, i.texcoord);
				clip(col.a - _Cutoff);
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
		ENDCG
	}
	UsePass "Hidden/ShadowCasterBase/MV_SHADOWCASTER"
}
	Fallback  Off
}
