Shader "Custom/SelectiveGlow"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _EmissionColor ("Emission Color", Color) = (1,1,1)
        _EmissionPower ("Emission Power", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Overlay" } // Налаштування черги рендерингу
        LOD 200

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
            float4 _Color;
            float4 _EmissionColor;
            float _EmissionPower;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Основний колір текстури
                fixed4 texColor = tex2D(_MainTex, i.uv) * _Color;

                // Емісійний колір (додається до основного)
                fixed4 emission = _EmissionColor * _EmissionPower;

                // Повертаємо суму кольорів з HDR для Bloom
                return texColor + emission;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
