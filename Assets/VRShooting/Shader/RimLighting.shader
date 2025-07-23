Shader "Custom/RimEmissionShader"
{
    Properties
    {
        _RimColor ("Rim Color", Color) = (1, 0, 0, 1)
        _RimPower ("Rim Power", Range(0, 10)) = 4
    }

    SubShader
    {
        Tags 
        { 
            "RenderType" = "Transparent" 
            "Queue" = "Transparent"
            "RenderPipeline"="UniversalPipeline"
        }
        Blend SrcAlpha OneMinusSrcAlpha 
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            // #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID //追加
            };

            struct v2f
            {
                float2 texcoord : TEXCOORD0;
                float3 viewDir : TEXCOORD1;
                float3 normal : NORMAL;
                float4 vertex : SV_POSITION;
                UNITY_VERTEX_OUTPUT_STEREO//追加
            };

            float3 _RimColor;
            float _RimPower;

            float3 _CameraPos;
            float _CameraPosX;
            float _CameraPosY;
            float _CameraPosZ;

            v2f vert (appdata_t v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v); //追加
                UNITY_INITIALIZE_OUTPUT(v2f, o); //追加
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o); //追加

                _CameraPos.x = _CameraPosX;
                _CameraPos.y = _CameraPosY;
                _CameraPos.z = _CameraPosZ;
                
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                
                /* ここで
                 * o.normal = normalize(mul(unity_WorldToObject, v.normal));
                 * とすると意図した結果にならないのでnormalizeを次の行でかける
                 * 謎
                */
                o.normal = mul(unity_ObjectToWorld,v.normal);
                o.normal = normalize(o.normal);
                o.viewDir = normalize(_CameraPos-mul(unity_ObjectToWorld, v.vertex));
                
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                half rim = dot(i.normal, i.viewDir);
                rim = 1.0 - saturate(rim);
                rim = pow(rim, 10-_RimPower);
                
                // リムライトの強度に基づいてエミッションを計算
                half3 emission = _RimColor * rim;
                half3 minus = (255,255,255,1);
                
                half4 col = (255,255,255,1);
                
                // エミッションを加える
                col.rgb -= minus;
                col.rgb += emission;
                
                col.a = rim;
                
                return col;
                
            }
            ENDCG
        }
    }
}
