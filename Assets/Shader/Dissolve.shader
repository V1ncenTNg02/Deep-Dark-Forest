Shader "Custom/DissolveShader"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _DissolveTex ("Dissolve Texture", 2D) = "white" {}
        _DissolveThreshold ("Dissolve Threshold", Range(0,1)) = 0
        _EdgeColor ("Edge Color", Color) = (1, 1, 1, 1) // Red for demonstration
        _EdgeWidth ("Edge Width", Range(0,0.1)) = 0.02
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
            sampler2D _DissolveTex;
            float _DissolveThreshold;
            float4 _EdgeColor;
            float _EdgeWidth;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                // Sample dissolve texture
                float dissolveValue = tex2D(_DissolveTex, i.uv).r;

                // Calculate edge
                float edge = smoothstep(_DissolveThreshold, _DissolveThreshold + _EdgeWidth, dissolveValue);

                // Dissolve effect
                clip(dissolveValue - _DissolveThreshold);

                // Output color
                float4 col = tex2D(_MainTex, i.uv); 
                col.rgb = lerp(_EdgeColor.rgb, col.rgb, edge);
                return col;
            }
            ENDCG
        }
    }
}