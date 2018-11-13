///////////////////////////////////////////////////////////////////////////////////////////
//作者：沙新佳 qq：287490904
//最后修改时间：2015/9/2
//脚本描述:  基本法线高光pass

Shader "Legacy Shaders/Reflective/Bumped Unlit Mask" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_ReflectColor ("Reflection Color", Color) = (1,1,1,0.5)
	_MainTex ("Base (RGB), RefStrength (A)", 2D) = "white" {}
	_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
	_Cube ("Reflection Cubemap", Cube) = "" {}
	_BumpMap ("Normalmap", 2D) = "bump" {}
	_Spec_Gloss_Reflec_Masks ("Spec(R) Gloss(G) Reflec(B)",2D) = "White" {}
}

Category {
	Tags { "RenderType"="Opaque" }
	LOD 250
	
	SubShader {
		// Always drawn reflective pass
		Pass {
			Name "BASEMASK"
			Tags {"LightMode" = "Always"}
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			struct v2f {
				float4 pos : SV_POSITION;
				float2	uv		: TEXCOORD0;
				float2	uv2		: TEXCOORD1;
				float3	I		: TEXCOORD2;
				float3	TtoW0 	: TEXCOORD3;
				float3	TtoW1	: TEXCOORD4;
				float3	TtoW2	: TEXCOORD5;
				UNITY_FOG_COORDS(6)
			};

			uniform float4 _MainTex_ST, _BumpMap_ST;

			v2f vert(appdata_tan v)
			{
				v2f o;
				o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord,_MainTex);
				o.uv2 = TRANSFORM_TEX(v.texcoord,_BumpMap);
				
				o.I = -WorldSpaceViewDir( v.vertex );
				
				float3 worldNormal = UnityObjectToWorldNormal(v.normal);
				float3 worldTangent = UnityObjectToWorldDir(v.tangent.xyz);
				float3 worldBinormal = cross(worldNormal, worldTangent) * v.tangent.w;
				o.TtoW0 = float3(worldTangent.x, worldBinormal.x, worldNormal.x);
				o.TtoW1 = float3(worldTangent.y, worldBinormal.y, worldNormal.y);
				o.TtoW2 = float3(worldTangent.z, worldBinormal.z, worldNormal.z);
				
				UNITY_TRANSFER_FOG(o,o.pos);
				return o; 
			}

			uniform sampler2D _BumpMap;
			uniform sampler2D _MainTex;
			uniform fixed _Cutoff;
			uniform sampler2D _Spec_Gloss_Reflec_Masks;
			uniform samplerCUBE _Cube;
			uniform fixed4 _ReflectColor;
			uniform fixed4 _Color;

			fixed4 frag (v2f i) : SV_Target
			{
				// Sample and expand the normal map texture	
				fixed3 normal = UnpackNormal(tex2D(_BumpMap, i.uv));
				
				fixed4 texcol = tex2D(_MainTex,i.uv);
				clip(texcol.a - _Cutoff);
				fixed4 sgr = tex2D(_Spec_Gloss_Reflec_Masks,i.uv2);
				// transform normal to world space
				half3 wn;
				wn.x = dot(i.TtoW0, normal);
				wn.y = dot(i.TtoW1, normal);
				wn.z = dot(i.TtoW2, normal);
				
				// calculate reflection vector in world space
				half3 r = reflect(i.I, wn);
				
				fixed4 c = UNITY_LIGHTMODEL_AMBIENT * texcol;
				fixed4 reflcolor = texCUBE(_Cube, r) * _ReflectColor*_ReflectColor.a * texcol.r;
				fixed4 reflcolor2 = texCUBE(_Cube, r) * _ReflectColor;
				reflcolor = lerp(fixed4(0,0,0,0),reflcolor, sgr.b);
				c = c + reflcolor;

				UNITY_APPLY_FOG(i.fogCoord, c);
				UNITY_OPAQUE_ALPHA(c.a);
				return c;
			}
			ENDCG  
		} 
	}	
}
	
FallBack "Legacy Shaders/VertexLit"
}
