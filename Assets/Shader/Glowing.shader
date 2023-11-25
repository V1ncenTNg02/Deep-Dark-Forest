Shader "Custom/Glowing" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _GlowColor ("Glow Color", Color) = (1, 1, 1, 1)
        _PulseSpeed ("Pulse Speed", Range(0, 5)) = 1.5
        _PulseRange ("Pulse Range", Range(0, 100)) = 0.5
    }

    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _GlowColor;
            float _PulseSpeed;
            float _PulseRange;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }
            
            half4 frag (v2f i) : SV_Target {
                half4 col = tex2D(_MainTex, i.uv);
                half mappedPulseValue = (sin(_Time.y * _PulseSpeed) + 1) * 0.5f;
                half pulse = mappedPulseValue * _PulseRange;
                half3 glow = _GlowColor.rgb * pulse;
                return half4(col.rgb + glow, col.a);
            }
            ENDCG
        }
    }
}
