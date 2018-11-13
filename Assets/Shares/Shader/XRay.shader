Shader "Custom/XRay_vert_frag"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _MatCap ("MatCap (RGB)", 2D) = "white" {}
        _Color("_Color", Color) = (0,1,0,1)
    }
    Subshader
    {
       // Tags { "Queue"="Transparent+10" "IgnoreProjector"="True" "RenderType"="Transparent" }
 
        Pass
        {
            Tags { "Queue"="Transparent+1" "LightMode" = "Vertex" }
 
            Blend One OneMinusSrcColor
            Cull Off
            Lighting Off
            ZWrite Off
            Ztest Greater
 
            CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma fragmentoption ARB_precision_hint_fastest
                #include "UnityCG.cginc"
 
                struct v2f
                {
                    float4 pos    : SV_POSITION;
                    float2 cap    : TEXCOORD0;
                };
 
                v2f vert (appdata_base v)
                {
                    v2f o;
                    o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
 
                    half2 capCoord;
                    capCoord.x = dot(UNITY_MATRIX_IT_MV[0].xyz,v.normal);
                    capCoord.y = dot(UNITY_MATRIX_IT_MV[1].xyz,v.normal);
                    o.cap = capCoord * 0.5 + 0.5;
 
                    return o;
                }
 
                uniform float4 _Color;
                uniform sampler2D _MatCap;
 
                float4 frag (v2f i) : COLOR
                {
                    float4 mc = tex2D(_MatCap, i.cap);
 
                    return _Color * mc * 2.0;
                }
            ENDCG
        }
        Pass {
            Tags {"Queue"="Overlay" "LightMode" = "Vertex" "RenderType"="Transparent"}
            ColorMaterial AmbientAndDiffuse
            Lighting On
            SetTexture [_MainTex] {
                combine texture
            } 
        }
    }
    FallBack "Mobile/VertexLit"
}