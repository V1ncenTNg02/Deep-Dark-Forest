//UNITY_SHADER_NO_UPGRADE
// Author: Cheng Chen
// This Shader is use for Floating Gems, Allow them to self rotation and Glow a bit.

Shader "Unlit/FloatingGem"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
		_FloatSpeed ("FloatSpeed", float) = 1
		_RotationSpeed ("RotationSpeed", Range(0, 90)) = 30
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;	

			struct vertIn
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct vertOut
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};
			float _FloatSpeed;
			float _RotationSpeed;

			// Implementation of the vertex shader
			vertOut vert(vertIn v)
			{
				// Displace the original vertex in model space
				vertOut o;
				float4 displacement = float4(0.0f, sin(_Time.y) * _FloatSpeed, 0.0f, 0.0f);
				v.vertex += displacement;
				float rotation = fmod((_RotationSpeed * _Time.y), 360);
                float angle = radians(rotation);
                float cosA = cos(angle);
                float sinA = sin(angle);
                float2x2 rotationMatrix = float2x2(cosA, -sinA, sinA, cosA);
				v.vertex.xz = mul(rotationMatrix, v.vertex.xz);
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			float4 _Color;
			float _GlowColor;
			float _GlowIntensity;
			// Implementation of the fragment shader
			fixed4 frag(vertOut v) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, v.uv) * _Color;
				
				float4 glow = _GlowColor * _GlowIntensity;
                float4 finalColor = col + glow;
				
				return finalColor;
			}
			ENDCG
		}
	}
}