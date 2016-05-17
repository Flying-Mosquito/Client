Shader "Custom/Raindrop" {
	Properties{
		_Color("Main Color", Color) = (1,1,1,1)
		_MainTex("Base(RGB)", 2D) = "white" {}
		//_SubTex("2nd Texture", 2D) = "white"{}
		//_Glossiness ("Smoothness", Range(0,1)) = 0.5
		//_Metallic ("Metallic", Range(0,1)) = 0.0
		}
		SubShader{
			Tags{ "Queue" = "Transparent"}
			Pass{
				Blend SrcAlpha OneMinusSrcAlpha
				Material{
					Diffuse[_Color]
					Ambient[_Color]
				}
				//Lighting On
				SetTexture[_MainTex]{
					Combine texture * primary DOUBLE
				//	Combine texture

				}
			/*	SetTexture[_SubTex]{
					Combine texture * previous
					//ConstantColor[_Color]
					//Combine texture lerp(texture) previous//, constant
				
				}
				*/
			}
	}

}
