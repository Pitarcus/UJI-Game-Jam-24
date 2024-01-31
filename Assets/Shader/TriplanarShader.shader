Shader "TriplanarTutorial/Triplanar_Final"
{
    Properties
    {
        _MainTex ("Main Texture ", 2D)  = "white" {}
        _TextureScale ("Texture Scale",float) = 1
        _TriplanarBlendSharpness ("Blend Sharpness",float) = 1
    }
    SubShader
    {
        Tags { 
            "RenderType"="Opaque" 
            "RenderPipeline"="UniversalRenderPipeline"
        }
        LOD 200
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
        
         
            struct Input
            {
                float3 worldPos;
                float3 worldNormal;
            }; 

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;

                //float2 uv_normal : TEXCOORD1;
                float3 normal_world : TEXCOORD2;
                //float4 tangent_world : TEXCOORD3;
                //float3 binormal_world : TEXCOORD4;
                float3 vertex_world : TEXCOORD5;
            };


            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _TextureScale;
            float _TriplanarBlendSharpness;

            v2f vert (appdata v)
            {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    //o.vertex = TransformObjectToHClip(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    UNITY_TRANSFER_FOG(o,o.vertex);
                
                    //o.uv_normal = TRANSFORM_TEX(v.uv, _NormalMap);
                    o.normal_world = normalize(mul(unity_ObjectToWorld, float4(v.normal, 0))).xyz;
                    o.vertex_world = mul(unity_ObjectToWorld, v.vertex);
                    return o;
            }

            half4 frag (v2f IN) : SV_Target
            {
                // Find our UVs for each axis based on world position of the fragment.
                half2 yUV = IN.vertex_world.xz / _TextureScale;
                half2 xUV = IN.vertex_world.zy / _TextureScale;
                half2 zUV = IN.vertex_world.xy / _TextureScale;
                // Now do texture samples from our diffuse map with each of the 3 UV set's we've just made.
                half3 yDiff = tex2D (_MainTex, yUV);
                half3 xDiff = tex2D (_MainTex, xUV);
                half3 zDiff = tex2D (_MainTex, zUV);
                // Get the absolute value of the world normal.
                // Put the blend weights to the power of BlendSharpness, the higher the value, 
                // the sharper the transition between the planar maps will be.
                half3 blendWeights = pow (abs(IN.normal_world), _TriplanarBlendSharpness);
                // Divide our blend mask by the sum of it's components, this will make x+y+z=1
                blendWeights = blendWeights / (blendWeights.x + blendWeights.y + blendWeights.z);
                // Finally, blend together all three samples based on the blend mask.
                return half4(xDiff * blendWeights.x + yDiff * blendWeights.y + zDiff * blendWeights.z, 1);
            }
            ENDCG
        }
    }
}