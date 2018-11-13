Shader "Projector/Additive" { 
   Properties { 
      _MainTex ("Texture", 2D) = "" { TexGen ObjectLinear } 
      //_FalloffTex ("FallOff", 2D) = "" { TexGen ObjectLinear } 
      _Color ("Main Color", Color) = (1,1,1,1)
   } 
   Subshader { 
      Pass { 
         ZWrite off 
         Fog { Color (0, 0, 0) } 
         ColorMask RGB 
        // Blend SrcAlpha Zero 
          Blend SrcAlpha OneMinusSrcAlpha 

         // SetTexture [_ShadowTex] {
         //    constantColor [_Color]
         //    combine texture * constant Double, ONE - texture 
         //    // combine texture * constant
         //    Matrix [_Projector] 
         // } 
         // SetTexture [_FalloffTex] { 
         //    constantColor (0,0,0,0) 
         //    combine previous lerp (texture) constant 
         //    Matrix [_ProjectorClip] 
         // } 

         SetTexture [_MainTex] { 
             constantColor [_Color]
             // combine texture * constant Double, ONE - texture 
             combine texture * constant Double
             Matrix [_Projector] 
         } 

		    // SetTexture [_FalloffTex] { 
            // constantColor (0,0,0,0) 
            // combine previous lerp (texture) constant 
           //  Matrix [_ProjectorClip] 
         // } 
      }
   }
}
