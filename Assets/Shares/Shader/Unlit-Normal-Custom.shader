// Unlit shader. Simplest possible textured shader.
// - no lighting
// - no lightmap support
// - no per-material color

Shader "Unlit/TextureCustom" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_ColliderColor ("Collider Color", Color) = (0.01,0.7,1.0,0.3)
	_Color ("Main Color", Color) = (1,1,1,1)
}

SubShader {
	Tags { "RenderType"="Transparent" }
	LOD 100
	
	Pass {  

		ZWrite Off
		Ztest Greater
		Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata_t {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				half2 texcoord : TEXCOORD0;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _ColliderColor;
			
			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : COLOR
			{
				fixed4 col = tex2D(_MainTex, i.texcoord);
				col = _ColliderColor;
				return col;
			}
		ENDCG
	}


	Pass {  
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata_t {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				half2 texcoord : TEXCOORD0;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Color;
			
			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : COLOR
			{
				fixed4 col = tex2D(_MainTex, i.texcoord);
				col = col * _Color;
				return col;
			}
		ENDCG
	}
}

SubShader {
	Tags { "RenderType"="Opaque" "LightMode" = "ShadowCollector" "LightMode" = "ShadowCaster" }
	LOD 100
	
	Pass {
		Lighting Off
		SetTexture [_MainTex] { combine texture } 
	}
}
}
