Shader "Hidden/ShadowCasterBase"
{
	Properties
	{

	}
 Category 
{
	SubShader
	{
			// Pass to render object as a shadow caster
			Pass 
			{
				Name "MV_ShadowCaster"
				Tags { "LightMode" = "ShadowCaster" }
				
				ZWrite On ZTest LEqual Cull Off

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile_shadowcaster
				#include "UnityCG.cginc"

				struct v2f { 
					V2F_SHADOW_CASTER;
				};

				v2f vert( appdata_base v )
				{
					v2f o;
					TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
					return o;
				}

				float4 frag( v2f i ) : SV_Target
				{
					SHADOW_CASTER_FRAGMENT(i)
				}
				ENDCG
			}	


	// Pass to render object as a shadow caster
	Pass {
		Name "LV_ShadowCaster"
		Tags { "LightMode" = "ShadowCaster" }
		
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile_shadowcaster
				#include "UnityCG.cginc"

				struct v2f { 
					V2F_SHADOW_CASTER;
				};

				v2f vert( appdata_base v )
				{
					v2f o;
					TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
					return o;
				}

				float4 frag( v2f i ) : SV_Target
				{
					SHADOW_CASTER_FRAGMENT(i)
				}
				ENDCG

		}
	}
}
}
