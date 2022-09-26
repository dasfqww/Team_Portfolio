Shader "N3K/AlwaysVisible"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Color("Always visible color", Color) = (0,0,0,0)
        _Outline("Outline Width",Range(0.0,0.1)) = 0.009
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" }
        LOD 100

        Pass
        {
            Cull Off
            ZWrite Off

            ZTest Always
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            uniform float _Outline;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 mNormal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 oColor : COLOR;
            };

            float4 _Color;

            v2f vert(appdata v)
            {
                v2f o;
                v2f origin;
                origin.vertex.xyz = o.vertex.xyz;
                o.vertex.xyz = v.vertex.xyz + v.mNormal * _Outline;
                /*if (origin.vertex.x != o.vertex.x)
                {
                    o.oColor.a = 0;
                }*/
                o.vertex = UnityObjectToClipPos(o.vertex);
                
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                if (i.oColor.r != _Color.r)
                {
                    _Color.a = 0;
                }
                return _Color;
            }

            ENDCG
        }

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
                //float4 vColor : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 oColor : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _Color;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                /*if (o.oColor.r != _Color.r)
                {
                    o.oColor.a = 0;
                }*/
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                //sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                /*if (i.oColor.r != _Color.r)
                {
                    i.oColor.a = 0;
                }*/
                return col;
            }

            ENDCG
        }
    }
}