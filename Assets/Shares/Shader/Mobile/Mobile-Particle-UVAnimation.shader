Shader "Mobile/Particles/UVAnimation"
{
	Properties
	{
		_MainTex ("Main_Texture", 2D) = "white" {}
		_TintColor ("_TintColor", Color) = (0.5,0.5,0.5,0.5)
	    _SizeX ("列数", Float) = 4
	    _SizeY ("行数", Float) = 4
	    _Speed ("播放速度", Float) = 200
	}
	SubShader
	{
		Tags {"Queue"="Transparent" "RenderType"="Transparent" }
	
		Blend One One
		Lighting Off ZWrite Off Fog { Mode Off }
		cull off
		LOD 100
		CGINCLUDE
		#pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
		#include "UnityCG.cginc"
		sampler2D _MainTex;		
		float4 _TintColor;

		uniform fixed _SizeX;
		uniform fixed _SizeY;
		uniform fixed _Speed;

		struct v2f {
			float4 pos : SV_POSITION;
			float2 uv : TEXCOORD0;
			fixed4 color : TEXCOORD1;		
		};

		
		v2f vert (appdata_full v)
		{
			v2f o;
			o.pos = mul(UNITY_MATRIX_MVP, v.vertex);

			 int index = floor(_Time .x * _Speed);
		    int indexY = index/_SizeX;
		    int indexX = index - indexY*_SizeX;
		    float2 testUV = float2(v.texcoord.x /_SizeX, v.texcoord.y /_SizeY);		    
		    testUV.x += indexX/_SizeX;
		    testUV.y -= indexY/_SizeY;

		    o.uv = testUV;

			o.color = v.color * _TintColor * _TintColor.a;


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
				o =tex* i.color;
				return o;
			}
			ENDCG 
		}
	}
}
