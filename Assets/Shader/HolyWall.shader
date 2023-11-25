//UNITY_SHADER_NO_UPGRADE
// Author: Cheng Chen

Shader "Unlit/HolyWall"
{
	Properties
	{
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Texture", 2D) = "white" {}
		_FlickerFrequency ("Flicker Frequency", Range(0, 1)) = 0.5
		_MinAlpha ("Min Alpha", float) = 0.2
		_MaxAlpha ("Max Alpha", float) = 0.8
		_RotateSpeed ("Rotate Speed", Range(0,1)) = 0.5

	}
	SubShader
	{
		Tags {"Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="Opaque" }
        LOD 100

		Pass
		{
			Tags{"LightMode" = "ForwardBase"}
			Cull Front

			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "Lighting.cginc"

			float4 _Color;
			uniform sampler2D _MainTex;	
			float4 _MainTex_ST;
			float _AlphaScale = 0.5;
			float _RotateSpeed;
			float _FlickerFrequency;
			float _MinAlpha;
			float _MaxAlpha;

			struct vertIn
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 texcoord : TEXCOORD0;
			};

			struct vertOut
			{
				float4 vertex : SV_POSITION;
				float3 normal : TEXCOORD0;
				float3 worldPos : TEXCOORD1;
				float2 uv : TEXCOORD2;
				float3 viewDir : TEXCOORD3;
			};

			vertOut vert(vertIn v)
			{
				vertOut o;
				o.viewDir = normalize(UnityWorldSpaceViewDir(v.vertex));

				float rotation = fmod((_RotateSpeed * _Time.y), 360);
                float angle = radians(rotation);
                float cosA = cos(angle);
                float sinA = sin(angle);
                float2x2 rotationMatrix = float2x2(cosA, -sinA, sinA, cosA);
				v.vertex.xz = mul(rotationMatrix, v.vertex.xz);

				o.vertex = UnityObjectToClipPos(v.vertex);
				o.normal = UnityObjectToWorldNormal(v.normal);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				return o;
			}
			
			// Implementation of the fragment shader
			fixed4 frag(vertOut v) : SV_Target
			{

				fixed3 worldNormal = normalize(v.normal);
				fixed3 worldLightDir = normalize(UnityWorldSpaceLightDir(v.worldPos));

				fixed4 texColor = tex2D(_MainTex, v.uv);

				fixed3 albedo = texColor.rgb * _Color.rgb;
				fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * albedo;
				fixed3 diffues = _LightColor0.rgb * albedo * max(0, dot(worldNormal, worldLightDir));
               
                return fixed4(ambient + diffues, texColor.a * _AlphaScale) * 3;
			}
			ENDCG
		}

		Pass
		{
			Tags{"LightMode" = "ForwardBase"}
			Cull Back

			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "Lighting.cginc"

			float4 _Color;
			uniform sampler2D _MainTex;	
			float4 _MainTex_ST;
			float _AlphaScale;
			float _RotateSpeed;
			float _FlickerFrequency;
			float _MinAlpha;
			float _MaxAlpha;

			struct vertIn
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 texcoord : TEXCOORD0;
			};

			struct vertOut
			{
				float4 vertex : SV_POSITION;
				float3 normal : TEXCOORD0;
				float3 worldPos : TEXCOORD1;
				float2 uv : TEXCOORD2;
				float3 viewDir : TEXCOORD3;
			};

			vertOut vert(vertIn v)
			{
				vertOut o;

				o.viewDir = normalize(UnityWorldSpaceViewDir(v.vertex));

				float rotation = fmod((_RotateSpeed * _Time.y), 360);
                float angle = radians(rotation);
                float cosA = cos(angle);
                float sinA = sin(angle);
                float2x2 rotationMatrix = float2x2(cosA, -sinA, sinA, cosA);
				v.vertex.xz = mul(rotationMatrix, v.vertex.xz);

				o.vertex = UnityObjectToClipPos(v.vertex);
				o.normal = UnityObjectToWorldNormal(v.normal);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				


				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				return o;
			}
			
			// Implementation of the fragment shader
			fixed4 frag(vertOut v) : SV_Target
			{
				fixed3 worldNormal = normalize(v.normal);
				fixed3 worldLightDir = normalize(UnityWorldSpaceLightDir(v.worldPos));

				fixed4 texColor = tex2D(_MainTex, v.uv);

				fixed3 albedo = texColor.rgb * _Color.rgb;
				fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * albedo;
				fixed3 diffues = _LightColor0.rgb * albedo * max(0, dot(worldNormal, worldLightDir));
               
				_AlphaScale += sin(_Time.y) * _FlickerFrequency;
				_AlphaScale = clamp(_AlphaScale, _MinAlpha, _MaxAlpha);
                return fixed4(ambient + diffues, texColor.a * _AlphaScale) * 3;
			}
			ENDCG
		}
	}
}