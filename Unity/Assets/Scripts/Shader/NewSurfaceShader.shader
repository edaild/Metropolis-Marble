Shader "Custom/KoreanBoardUI"
{
    Properties
    {
        _MainTex ("_MainTex (This is not used directly, just for Unity material system)", 2D) = "white" {} // 더미
        _BorderTex ("Border Pattern Texture (RGB)", 2D) = "white" {} // 전통 문양 테두리 텍스처
        _BackgroundTex ("Overall Background Texture (For Color Reference)", 2D) = "white" {} // 배경 그림 텍스처
        _InnerColor ("Inner UI Base Color (Pink)", Color) = (1.0, 0.75, 0.85, 1.0) // 밝은 핑크 기본색
        _BorderThicknessUV ("Border UV Thickness (0-0.5)", Range(0, 0.5)) = 0.12 // 테두리 두께 (UV 기준)
        _BackgroundInfluence ("Background Color Influence on Border", Range(0, 1)) = 0.7 // 배경색 영향도
        _NoiseAmount ("Noise Texture Amount", Range(0, 1)) = 0.05 // 노이즈 질감 정도 (선택 사항)
        _NoiseTex ("Noise Texture (R: Intensity)", 2D) = "white" {} // 노이즈 텍스처 (선택 사항)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // URP/HDRP 호환성을 위해 다음을 추가할 수 있습니다.
            // #pragma multi_compile _ ETC
            // #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl" // URP
            // #include "Packages/com.unity.render-pipelines.high-definition/ShaderLibrary/Core.hlsl" // HDRP

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

            sampler2D _BorderTex;
            sampler2D _BackgroundTex;
            sampler2D _NoiseTex; // 선택 사항
            float4 _InnerColor;
            float _BorderThicknessUV;
            float _BackgroundInfluence;
            float _NoiseAmount;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv; // 0~1 범위의 UV
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 finalColor;

                // 보드판 UV (0~1)
                float2 uv = i.uv;

                // 테두리 영역 계산 (중앙을 기준으로)
                bool isBorder = 
                    uv.x < _BorderThicknessUV || 
                    uv.x > (1.0 - _BorderThicknessUV) ||
                    uv.y < _BorderThicknessUV ||
                    uv.y > (1.0 - _BorderThicknessUV);

                if (isBorder)
                {
                    // **테두리 영역**: 전통 문양 텍스처와 배경 색감을 섞습니다.
                    fixed4 borderPatternColor = tex2D(_BorderTex, uv); // 문양 텍스처 샘플링
                    fixed4 backgroundColor = tex2D(_BackgroundTex, uv); // 배경 그림 텍스처 샘플링 (해당 UV 위치)
                    
                    // 배경 색상과 문양 색상을 혼합
                    finalColor.rgb = lerp(borderPatternColor.rgb, backgroundColor.rgb, _BackgroundInfluence);
                    finalColor.a = 1.0; // 불투명
                }
                else
                {
                    // **내부 영역**: 밝은 핑크색 기본색 사용
                    finalColor = _InnerColor;
                }

                // **선택 사항: 노이즈/질감 추가**
                if (_NoiseAmount > 0.0)
                {
                    fixed noiseValue = tex2D(_NoiseTex, uv * 5.0).r; // 노이즈 텍스처 샘플링 (타일링을 위해 uv에 스케일 적용)
                    finalColor.rgb = lerp(finalColor.rgb, finalColor.rgb * noiseValue, _NoiseAmount);
                }

                return finalColor;
            }
            ENDCG
        }
    }
}