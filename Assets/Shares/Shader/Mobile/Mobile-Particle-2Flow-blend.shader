Shader "Mobile/Particles/2Flow-blend"
{
	Properties
	{
		_MainTex ("Main_Texture", 2D) = "white" {}
		_TintColor ("_TintColor", Color) = (0.5,0.5,0.5,0.5)
		
		_DetailTex1 ("Blend_Texture_01(RGB)", 2D) = "white" {}
		_TintColor1 ("_TintColor2", Color) = (0.5,0.5,0.5,0.5)
		
		_DetailTex2 ("Blend_Texture_02(RGB)", 2D) = "white" {}
		_TintColor2 ("_TintColor2", Color) = (0.5,0.5,0.5,0.5)
		
		_Scroll1 ("Blend_Texture_01 speed", Float) = 1.0
		_Scroll2 ("Blend_Texture_02 speed", Float) = 1.0
		
		_MMultiplier ("Brightness_Main", Float) = 1.0
		_BMultiplier ("Brightness_Blend", Float) = 1.0
	}
	SubShader
	{
		Tags {"Queue"="Transparent" "RenderType"="Transparent" }
	
		Blend One One
		Lighting Off Fog { Mode Off }
		cull off
		LOD 100
		CGINCLUDE
		#pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
		#include "UnityCG.cginc"
		sampler2D _MainTex;
		sampler2D _DetailTex1;
		sampler2D _DetailTex2;
		
		float4 _MainTex_ST;
		float4 _DetailTex1_ST;
		float4 _DetailTex2_ST;
		
		float _Scroll1;
		float _Scroll2;

		float _BMultiplier;
		float _MMultiplier;
		
		float4 _TintColor;
		float4 _TintColor1;
		float4 _TintColor2;
		struct v2f {
			float4 pos : SV_POSITION;
			float2 uv : TEXCOORD0;
			float2 uv2 : TEXCOORD1;
			float2 uv3 : TEXCOORD2;
			fixed4 color : TEXCOORD3;		
		};

		
		v2f vert (appdata_full v)
		{
			v2f o;
			o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
			o.uv = v.texcoord;
			o.uv2 = TRANSFORM_TEX(v.texcoord.xy,_DetailTex1) + frac(float2(_Scroll1, 0) * _Time);
			o.uv3 = TRANSFORM_TEX(v.texcoord.xy,_DetailTex2) + frac(float2(0, _Scroll2/5) * _Time);
			o.color = v.color * _TintColor * _TintColor.a;
			o.color.xyz *= _MMultiplier;

			return o;
		}
		ENDCG


		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest		
			fixed4 frag (v2f i) : COLOR
			{
				fixed4 o;
				fixed4 tex = tex2D (_MainTex, i.uv);
				fixed4 tex1 = tex2D (_DetailTex1, i.uv2);
				fixed4 tex2 = tex2D (_DetailTex2, i.uv3);
				
				o =tex* i.color + (tex1*_TintColor1*_TintColor1.a + tex2*_TintColor2*_TintColor2.a)*_BMultiplier/2;
				
				return o;
			}
			ENDCG 
		}
	}
}
