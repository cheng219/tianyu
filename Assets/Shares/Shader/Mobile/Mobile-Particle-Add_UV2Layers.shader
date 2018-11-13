///////////////////////////////////////////////////////////////////////////////////////////
//作者：沙新佳 
//最后修改时间：20150925
//着色器描述： 两层纹理叠加，可滚动变色
///////////////////////////////////////////////////////////////////////////////////////////

Shader "Mobile/Particles/Additive_UV2Layers" {
Properties {
	_TintColorA ("_TintColorA", Color) = (0.5,0.5,0.5,0.5)
	_MainTex ("_MainTex (RGB)", 2D) = "white" {}
	_TintColorB ("_TintColorB", Color) = (0.5,0.5,0.5,0.5)
	//_MainTex2 ("_MainTex2", 2D) = "white" {}
	_DetailTex ("_DetailTex (RGB) 2nd layer ", 2D) = "white" {}
	_MMultiplier ("Layer Multiplier", Range(0,2)) = 2
	
	// Will set "_INVERT_ON" shader keyword when set
   [Toggle] _INVERT (" Multiplierx4?", Float) = 0
   
	//[Toggle(ENABLE_MYTEST)] _MYTEST ("Mytest?", Float) = 0
// Will set "ENABLE_FANCY" shader keyword when set
//[Toggle(ENABLE_FANCY)] _Fancy ("Fancy?", Float) = 0
//
//[KeywordEnum(None, Add, Multiply)] _Overlay ("Overlay mode", Float) = 0

}


SubShader {

	//Tags { "RenderType"="Opaque" }
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
	//Blend One One,SrcAlpha DstAlpha 
	Blend SrcAlpha One
	//Blend DstColor SrcColor
	//	Blend One One
	ZWrite Off	
	Lighting Off  Fog { Color (0,0,0,0) }
	Cull Off
	LOD 100
	
	
	
	CGINCLUDE
	#pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
	//#pragma multi_compile ENABLE_MYTEST
	#pragma shader_feature   _INVERT_ON
	#include "UnityCG.cginc"
	sampler2D _MainTex;
	sampler2D _DetailTex;

	float4 _MainTex_ST;
	float4 _DetailTex_ST;
	fixed4 _TintColorA,	_TintColorB;
	float _MMultiplier;
	
	struct v2f {
		float4 pos : SV_POSITION;
		float4 uv : TEXCOORD0;
		float4 uv2 : TEXCOORD1;
		fixed4 color : TEXCOORD2;
	};

	
	v2f vert (appdata_full v)
	{
		v2f o;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		//o.uv =  v.texcoord;
		o.uv.xy = TRANSFORM_TEX(v.texcoord.xy,_MainTex);
		o.uv.zw = TRANSFORM_TEX(v.texcoord.zw,_MainTex);
		o.uv2.xy = TRANSFORM_TEX(v.texcoord.xy,_DetailTex);
		o.uv2.zw = TRANSFORM_TEX(v.texcoord.zw,_DetailTex);
		o.color = _MMultiplier.xxxx * v.color;
//		o.color = v.color;// * _TintColor * _TintColor.a;
//		o.color.xyz *= _MMultiplier;	
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
			fixed4 tex2 = tex2D (_DetailTex, i.uv2);
			
			tex.a = tex.a * _TintColorA.a;
			tex.rgb *=_TintColorA.rgb;
			tex.rgb *=tex.a;
			
			tex2.a = tex2.a* _TintColorB.a;
			tex2.rgb *= _TintColorB.rgb;
			tex2.rgb *=tex2.a;
			
			o= (tex+tex2)*i.color;
//			#if ENABLE_MYTEST
//			o= _TintColorA;
//			#endif	
			#if _INVERT_ON	
			o= (tex+tex2)*i.color*2;
			#endif	
			return o;
		}
		ENDCG 
	}	
}




}