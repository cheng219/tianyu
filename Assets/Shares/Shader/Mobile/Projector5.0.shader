///////////////////////////////////////////////////////////////////////////////////////////
//作者：沙新佳 qq：287490904
//最后修改时间：2016/1/26
//脚本描述：简化版投射器，alpha当做透明，rgb为基本色
//////////////////////////////////

Shader "Mobile/Projector5.0" {
Properties {
 _TintColor ("_TintColor", Color) = (1,1,1,1)
 _MainTex ("Base", 2D) = "black" { }
}
SubShader {
	//Blend DstColor One
	Blend SrcAlpha OneMinusSrcAlpha 
	ColorMask RGB
	CGINCLUDE
	#include "UnityCG.cginc"
	float4x4 _Projector;
	sampler2D _MainTex;
    fixed4 _TintColor;
    
    struct v2f {
        float4 pos : SV_POSITION;
        float4 uv : TEXCOORD0;
    };

    v2f vert (appdata_base v)
    {
        v2f o;
        o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
        o.uv = mul (_Projector, v.vertex);
        return o;
    }
	ENDCG


 Pass { 	
        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag

        half4 frag (v2f IN) : COLOR
        {
       		 half4 MainTex = tex2Dproj(_MainTex, UNITY_PROJ_COORD(IN.uv));
             return MainTex*_TintColor;
        }
        ENDCG 
    } 
}

}