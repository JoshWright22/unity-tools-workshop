// Wobbles a sprite side to side, like a flag or underwater.
Shader "Workshop/Sprite Wave"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite", 2D) = "white" {}
        _Strength ("Strength", Range(0, 0.2)) = 0.05
        _Frequency ("Frequency", Range(0, 30)) = 10
        _Speed ("Speed", Range(0, 10)) = 3
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes { float4 positionOS : POSITION; float2 uv : TEXCOORD0; float4 color : COLOR; };
            struct Varyings { float4 positionCS : SV_POSITION; float2 uv : TEXCOORD0; float4 color : COLOR; };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float _Strength, _Frequency, _Speed;

            Varyings vert(Attributes v)
            {
                Varyings o;
                o.positionCS = TransformObjectToHClip(v.positionOS.xyz);
                o.uv = v.uv;
                o.color = v.color;
                return o;
            }

            float4 frag(Varyings i) : SV_Target
            {
                float2 uv = i.uv;
                uv.x += sin(uv.y * _Frequency + _Time.y * _Speed) * _Strength;
                return SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv) * i.color;
            }
            ENDHLSL
        }
    }
}
