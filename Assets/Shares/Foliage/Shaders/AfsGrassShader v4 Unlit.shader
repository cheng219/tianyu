///////////////////////////////////////////////////////////////////////////////////////////
//作者：沙新佳 
//最后修改时间：#Date#
//着色器描述： 
///////////////////////////////////////////////////////////////////////////////////////////
Shader "AFS/Grass Shader v4 Unlit" {
Properties {
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
}
SubShader {
	Tags {"Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout" "AfsMode"="Grass"}
	LOD 100

	Lighting Off
	Cull Off
	Pass {  
		CGPROGRAM
			#pragma vertex AfsWavingGrassVert addshadow
			#pragma fragment frag
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"
			
			#include "TerrainEngine.cginc"
			#include "Includes/AfsWavingGrass.cginc"

			#pragma shader_feature _AFS_GRASS_APPROXTRANS
			#pragma multi_compile IN_EDITMODE IN_PLAYMODE
			
			
			#if defined (_AFS_GRASS_APPROXTRANS)
			float4 _AfsDirectSunDir;
			fixed3 _AfsDirectSunCol;
			//fixed3 _AfsTranslucencyColor;
			sampler2D _TerrianBumpTransSpecMap;
			#endif
			

			struct v2f {
				float4 vertex : SV_POSITION;
				half2 texcoord : TEXCOORD0;
				float4 color : COLOR;
				//float3 normal : NORMAL;
				float translucency:TEXCOORD2;
				UNITY_FOG_COORDS(1)
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed _Cutoff;


			v2f AfsWavingGrassVert (appdata_full v) 
			{
				v2f o;// = (v2f)v;
				 UNITY_INITIALIZE_OUTPUT(v2f,o);
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				#ifdef IN_EDITMODE
					v.normal = UnityObjectToWorldNormal (float3(0,1,0));
				#endif
				float waveAmount = v.color.a * _AfsWaveAndDistance.z;
				o.color = WaveGrassMesh (v.vertex, v.normal, waveAmount, v.color);

				#if !defined(UNITY_PASS_SHADOWCASTER)

					#ifdef IN_EDITMODE
						o.color.rgb = half3(1,1,1);
					#endif

					#if defined (_AFS_GRASS_APPROXTRANS)
						//	Calculate per vertex translucency according to _AfsDirectSunDir
						float4 worldPos = mul(_Object2World, v.vertex);
						half3 worldViewDir = normalize(UnityWorldSpaceViewDir(worldPos.xyz));
						o.translucency = saturate(dot(worldViewDir, _AfsDirectSunDir.xyz)) * _AfsDirectSunDir.w;
					#endif
				#endif
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}


//			v2f vert (appdata_t v)
//			{
//				v2f o;
//				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
//				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
//				UNITY_TRANSFER_FOG(o,o.vertex);
//				return o;
//			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.texcoord);
				clip(col.a - _Cutoff);
				
				#if !defined(UNITY_PASS_SHADOWCASTER)
					col.rgb = col.rgb * i.color.rgb*2;	
					#if defined (_AFS_GRASS_APPROXTRANS)
						col.rgb += tex2D(_TerrianBumpTransSpecMap, i.texcoord).rgb * i.translucency * i.color.rgb;
					#endif
				#endif
				
				
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
		ENDCG
	}
}

}
