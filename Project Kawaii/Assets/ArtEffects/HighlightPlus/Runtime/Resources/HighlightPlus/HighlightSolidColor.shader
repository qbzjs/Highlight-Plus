Shader "HighlightPlus/Geometry/SolidColor" {
Properties {
    _MainTex ("Texture", 2D) = "white" {}
    _Color ("Color", Color) = (1,1,1) // not used; dummy property to avoid inspector warning "material has no _Color property"
    _CutOff("CutOff", Float ) = 0.5
    _Cull ("Cull Mode", Int) = 2
	_ZTest("ZTest", Int) = 4
}
    SubShader
    {
        Tags { "Queue"="Transparent+100" "RenderType"="Transparent" "DisableBatching" = "True" }

        // Compose effect on camera target
        Pass
        {
            ZWrite Off
            Cull [_Cull]
			ZTest Always
            Stencil {
                Ref 2
                Comp NotEqual
                Pass replace 
            }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_local _ HP_ALPHACLIP
            #pragma multi_compile_local _ HP_DEPTHCLIP

            #include "UnityCG.cginc"
            #include "CustomVertexTransform.cginc"

            sampler _MainTex;
            #if HP_DEPTHCLIP
                UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);
            #endif
      		float4 _MainTex_ST;
            fixed _CutOff;
            fixed4 _Color;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
				float4 pos    : SV_POSITION;
                float2 uv     : TEXCOORD0;
                #if HP_DEPTHCLIP
                    float depth   : TEXCOORD1;
                    float4 scrPos : TEXCOORD2;
                #endif
                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };

            v2f vert (appdata v)
            {
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_OUTPUT(v2f, o);
                UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				o.pos = ComputeVertexPosition(v.vertex);
				o.uv = TRANSFORM_TEX (v.uv, _MainTex);
                #if HP_DEPTHCLIP
                    COMPUTE_EYEDEPTH(o.depth);
                    o.scrPos = ComputeScreenPos(o.pos);
                #endif
				return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

            	#if HP_ALPHACLIP
            	    fixed4 col = tex2D(_MainTex, i.uv);
            	    clip(col.a - _CutOff);
            	#endif
                #if HP_DEPTHCLIP
                    float depthRaw = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, UnityStereoTransformScreenSpaceTex(i.scrPos.xy / i.scrPos.w ));
                    float depthPersp = LinearEyeDepth(depthRaw);
                    #if defined(UNITY_REVERSED_Z)
                        depthRaw = 1.0 - depthRaw;
                    #endif
                    float depthOrtho = lerp(_ProjectionParams.y, _ProjectionParams.z, depthRaw);
                    float vz = unity_OrthoParams.w ? depthOrtho : depthPersp;
                    clip( vz - i.depth * 0.999);
                #endif

            	return fixed4(1.0, 1.0, 1.0, 1.0);
            }
            ENDCG
        }

    }
}