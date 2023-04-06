Shader "Custom/RainbowShaderSubstrate"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _Distance ("Grating distance", Range(0,10000)) = 1600 //nm
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Diffraction fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0


        float _Distance;

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        float3 worldTangent;
        fixed4 _Color;


        #include "UnityPBSLighting.cginc"

        inline fixed3 bump3y(fixed3 x, fixed3 yoffset)
        {
            float3 y = 1 - x * x;
            y = saturate(y - yoffset);
            return y;
        }

        fixed3 spectral_zucconi6(float w)
        {
            // w: [400, 700]
            // x: [0,   1]
            fixed x = saturate((w - 400.0) / 300.0);
            const float3 c1 = float3(3.54585104, 2.93225262, 2.41593945);
            const float3 x1 = float3(0.69549072, 0.49228336, 0.27699880);
            const float3 y1 = float3(0.02312639, 0.15225084, 0.52607955);
            const float3 c2 = float3(3.90307140, 3.21182957, 3.96587128);
            const float3 x2 = float3(0.11748627, 0.86755042, 0.66077860);
            const float3 y2 = float3(0.84897130, 0.88445281, 0.73949448);
            return
                bump3y(c1 * (x - x1), y1) +
                bump3y(c2 * (x - x2), y2);
        }



        inline fixed4 LightingDiffraction(SurfaceOutputStandard s, fixed3 viewDir, UnityGI gi) {
            //original color
            fixed4 pbr = LightingStandard(s, viewDir, gi);

            // Diffraction grating effect
            float3 L = gi.light.dir;
            float3 V = viewDir;
            float3 T = worldTangent;

            float d = _Distance;
            float cos_ThetaL = dot(L, T);
            float cos_ThetaV = dot(V, T);
            float u = abs(cos_ThetaL - cos_ThetaV);

            if (u == 0)
                return pbr;


            //Calculates the reflection color
            fixed3 color = 0;
            for (int n = 1; n <= 8; n++) {
                float wavelength = u * d / n;
                color += spectral_zucconi6(wavelength);
            }
            color = saturate(color);

            //adds the reflection to the material color
            pbr.rgb += color*0.05;
            return pbr;
        }

        void LightingDiffraction_GI(SurfaceOutputStandard s, UnityGIInput data, inout UnityGI gi) {
            LightingStandard_GI(s, data, gi);
        }


        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;


            fixed2 uv = IN.uv_MainTex * 2 - 1;
            fixed2 uv_orthogonal = normalize(uv);
            fixed3 uv_tangent = fixed3(uv_orthogonal.x, 0, -uv_orthogonal.x);

            worldTangent = normalize(mul(unity_ObjectToWorld, float4(uv_tangent, 0)));
        }
        ENDCG
    }
    FallBack "Diffuse"
}
