Shader "Custom/Fire"{
    Properties{
        _MainTex ("MainTex", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _Emission ("Emission", Float) = 1
        [HideInInspector]
        _Stencil ("Stencil", Int) = 0
        _Edge ("Edge", Range(0,1)) = 0.7
        _Scale ("Scale", Vector) = (1,1,0,0)
        _Speed ("Speed", Vector) = (1,1,0,0)
    }

    SubShader{
        Tags{
            "Queue" = "Transparent"
            "RenderType" = "Sprite"
        }
        LOD 100

        ZWrite off
        Cull off
        Blend SrcAlpha OneMinusSrcAlpha

        Stencil{
            Ref [_StencilRef]
            Comp LEqual
        }

        Pass{
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma warning{disable}

            #include "Common/CommonShaderMethods.hlsl"

            struct appdata{
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f{
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Emission, _Edge;
            float4 _Color;
            float2 _Scale, _Speed;

            v2f vert(appdata v){
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                return o;
            }

            float4 frag(v2f i) : SV_Target{
                float h = (1 - pow(i.uv.y, 0.5)) - (1.5 - _Edge);
                float2 uv = (i.uv + _Time * _Speed * -1) * (1 / _Scale);
                float fire = GradientNoise(uv, 10) * SimpleNoise(uv, 10);

                float a = tex2D(_MainTex, i.uv).a * clamp(fire + h, 0, 1);
                float4 color = _Color * (_Emission + 1);
                return float4(color.rgb, a) * i.color;
            }
            ENDHLSL
        }
    }
}