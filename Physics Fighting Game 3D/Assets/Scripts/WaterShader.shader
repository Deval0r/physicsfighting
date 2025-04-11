Shader "Custom/WaterShader"
{
    Properties
    {
        _Color ("Fill Color", Color) = (0.0, 0.5, 1.0, 1.0) // Default color for transparent regions
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags {"Queue"="Transparent"}
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
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
                float4 pos : POSITION;
            };

            sampler2D _MainTex;
            float4 _Color;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 tex = tex2D(_MainTex, i.uv);
                if (tex.a < 0.5) // If transparency is below a threshold
                {
                    return _Color; // Render the fill color
                }
                return tex; // Render the original texture
            }
            ENDCG
        }
    }
}