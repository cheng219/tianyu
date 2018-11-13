Shader "TerrainTool/2TexturesDiffuseFast" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_Layer1 ("Layer1 (RGB)", 2D) = "white" {}
	_Layer2 ("Layer2 (RGB)", 2D) = "white" {}
	_MainTex ("Mask (RGB)", 2D) = "white" {}
}
SubShader {
	Tags { "RenderType"="Opaque" }
	LOD 200
CGPROGRAM
#pragma surface surf Lambert exclude_path:prepass  noforwardadd halfasview novertexlights

sampler2D _MainTex;
sampler2D _Layer1, _Layer2;
fixed4 _Color;

struct Input {
	float2 uv_Layer1: TEXCOORD0;
	float2 uv_Layer2 : TEXCOORD1;
	float2 uv_MainTex : TEXCOORD3;
};


void surf (Input IN, inout SurfaceOutput o) {
	
	half4 Mask = tex2D (_MainTex, IN.uv_MainTex);
	fixed4 lay;
	lay  = Mask.r * tex2D (_Layer1, IN.uv_Layer1);
	lay += Mask.g * tex2D (_Layer2, IN.uv_Layer2);
	o.Albedo = lay* _Color.rgb;
}
ENDCG
}
Fallback "Mobile/Diffuse"
}
