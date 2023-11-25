//UNITY_SHADER_NO_UPGRADE
// Author: Cheng Chen

Shader "Unlit/WaveShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_WaveHeight ("WaveHeight", float) = 0.5
		_WaveSpeed ("WaveSpeed", float) = 1
	}
	SubShader
	{
		Pass
		{
			Cull Off

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
			float _WaveHeight;
			float _WaveSpeed;

			// Implementation of the vertex shader
			vertOut vert(vertIn v)
			{
				// Displace the original vertex in model space
				vertOut o;
				v.vertex = mul(UNITY_MATRIX_MV, v.vertex);
				float4 displacement = float4(0.0f, sin(v.vertex.x + _Time.y * _WaveSpeed) * _WaveHeight, 0.0f, 0.0f);
				v.vertex += displacement;
				o.vertex = mul(UNITY_MATRIX_P, v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			// Implementation of the fragment shader
			fixed4 frag(vertOut v) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, v.uv);
				return col;
			}
			ENDCG
		}
	}
}