Shader "Custom/Fog"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _FogColor ("Fog Color", Color) = (0.5, 0.5, 0.5, 1)
        _FogStart ("Fog Start", Range(0, 50)) = 10
        _FogEnd ("Fog End", Range(0, 300)) = 50
        // _CameraDepthTexture ("Depth (Camera)", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
            
            sampler2D _MainTex;
            sampler2D _NoiseTex;
            float4 _FogColor;
            float _FogStart;
            float _FogEnd;
            sampler2D _CameraDepthTexture;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            half4 frag (v2f i) : SV_Target
            {
                half4 col = tex2D(_MainTex, i.uv);
                
                // Sample the depth and noise textures
                float depth = tex2D(_CameraDepthTexture, i.uv).r;
                half noise = tex2D(_NoiseTex, i.uv).r;

                // Linearize the depth
                float z = LinearEyeDepth(depth);
                float fogFactor = saturate((z - _FogStart) / (_FogEnd - _FogStart));

                // Add noise to the fog effect
                fogFactor += noise * 0.1;

                // Lerp between the original color and the fog color
                col = lerp(col, _FogColor, fogFactor);
                
                return col;
            }
            ENDCG
        }
    }
}
