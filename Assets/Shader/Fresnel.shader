Shader "Custom/FresnelShader"
{
    Properties
    {
        _FresnelColor ("Fresnel Color", Color) = (1,1,1,1)
        _FresnelPower ("Fresnel Power", Range(1,10)) = 1
    }

    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
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
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float3 worldNormal : TEXCOORD0;
                float3 viewDir : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            float4 _FresnelColor;
            float _FresnelPower;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldNormal = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, v.normal));
                o.viewDir = normalize(UnityWorldSpaceViewDir(v.vertex.xyz));
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float fresnel = dot(i.worldNormal, i.viewDir);
                fresnel = pow(1.0 - fresnel, _FresnelPower);
                float4 col = _FresnelColor * fresnel;
                return col;
            }
            ENDCG
        }
    }
}
