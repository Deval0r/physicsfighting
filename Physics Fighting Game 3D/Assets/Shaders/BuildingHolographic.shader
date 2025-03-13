// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/BuildingHolographic"
{
    Properties
    {
        _Color ("Main Color", Color) = (0, 1, 1, 0.5) // Light blue with transparency
        _GlowColor ("Glow Color", Color) = (0, 1, 1, 1)
        _GlowIntensity ("Glow Intensity", Float) = 1.0
        _MainTex ("Base (RGB)", 2D) = "white" { }
    }
    SubShader
    {
        Tags { "Queue"="Overlay" "RenderType"="Opaque" }
        Pass
        {
            Name "Hologram"
            Tags { "LightMode"="Always" }
            Cull Front
            ZWrite Off           // ZWrite Off to support transparency
            ZTest LEqual
            Blend SrcAlpha OneMinusSrcAlpha

            // Shader code
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            // Properties declared in Unity’s way
            sampler2D _MainTex;
            float4 _Color;
            float4 _GlowColor;
            float _GlowIntensity;

            // Structure to hold vertex data
            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            // Structure to pass data from vertex to fragment shader
            struct v2f
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
            };

            // Vertex shader
            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            // Fragment shader
            half4 frag(v2f i) : SV_Target
            {
                // Sample texture if available
                half4 texColor = tex2D(_MainTex, i.uv);
                
                // Combine texture color with main color
                half4 finalColor = _Color * texColor;

                // Add glow effect
                finalColor.rgb += _GlowColor.rgb * _GlowIntensity;

                return finalColor;
            }

            ENDCG
        }
    }

    Fallback "Unlit/Color"
}
