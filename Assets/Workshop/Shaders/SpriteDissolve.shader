// Burns a sprite away with a glowing edge. Drag _Amount from 0 to 1.
Shader "Workshop/Sprite Dissolve"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite", 2D) = "white" {}
        _Amount ("Amount", Range(0, 1)) = 0.3
        _EdgeColor ("Edge Color", Color) = (1, 0.5, 0.1, 1)
        _EdgeWidth ("Edge Width", Range(0, 0.2)) = 0.06
        _NoiseScale ("Noise Scale", Range(1, 60)) = 18
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
            float _Amount, _EdgeWidth, _NoiseScale;
            float4 _EdgeColor;

            // cheap value noise, same idea as Shader Graph's Simple Noise node
            float hash(float2 p) { return frac(sin(dot(p, float2(127.1, 311.7))) * 43758.5453); }
            float noise(float2 p)
            {
                float2 i = floor(p), f = frac(p);
                f = f * f * (3 - 2 * f);
                return lerp(lerp(hash(i), hash(i + float2(1, 0)), f.x), lerp(hash(i + float2(0, 1)), hash(i + 1), f.x), f.y);
            }

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
                float4 c = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv) * i.color;
                float n = noise(i.uv * _NoiseScale);
                clip(n - _Amount);
                if (_Amount > 0 && n - _Amount < _EdgeWidth) c.rgb = _EdgeColor.rgb;
                return c;
            }
            ENDHLSL
        }
    }
}
