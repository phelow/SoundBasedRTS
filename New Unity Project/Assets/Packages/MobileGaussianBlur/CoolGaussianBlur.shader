Shader "CoolGaussianBlur" 
{
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Resolution ("Resolution", Vector) = (0,0,0,0)
		_BlurFactor ("Blur factor", Range(0.0, 1.0)) = 1.0
		_BlurScale ("Blur scale", Range(1.0, 10.0)) = 1.0
	}
	
	SubShader
    {
        Pass
        {
            ZTest Always Cull Off ZWrite Off
           
            CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
               
                #include "UnityCG.cginc"
              
                uniform sampler2D _MainTex;
                uniform float4 _MainTex_ST;
                float4 _Resolution;
                float _BlurFactor;
                float _BlurScale;
               
                struct v2f
                {
                    float4 pos : SV_POSITION;
                    float2 mainTexcoord : TEXCOORD0;
                };
				
				// sigma = 10
				//0.039895 * exp(- x*x / 200) 
//				const float PreWeights[15] = 
//				{
//				   0.06282,// 0
//				   0.06447,// 1
//				   0.06587,// 2
//				   0.06695,// 3
//				   0.0679, // 4
//				   0.06854,// 5
//				   0.06892,// 6
//				   
//				   0.06905,//0.10845 // 7
//				   
//                   0.06892,//0.10825 // 8
//                   0.06854,//0.107648 // 9
//                   0.0679, //0.10665 // 10
//                   0.06695,//0.105154 // 11
//                   0.06587,//0.10346 // 12
//                   0.06447,//0.101265 // 13
//                   0.06282,//0.09867 //14
//                   
//                   // sum = 1.570644
//				};
				
				

                v2f vert(appdata_base v)
                {
                    v2f o;
                   
                    o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
                    o.mainTexcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                   
                    return o;
                }
                
                float4 frag(v2f inFrag) : COLOR
                {
                    float4 ret = tex2D(_MainTex, inFrag.mainTexcoord.xy);
                    
                    const int RADIUS = 3;
                    // sigma = 10
					//0.039895 * exp(- x*x / 200) 
					const float PreWeights[7] = 
					{
					   0.14157,// 0
					   0.1429,// 1
					   0.14369,// 2
					   
					   0.14396,//0.10845 // 3
					   
	                   0.14369,//0.10825 // 4
	                   0.1429,//0.10765 // 5
	                   0.14157, //0.10665 // 6
	                   
	                   // sum = 0.75335
					};
					
					float3 accColor = float3(0,0,0);
					for (int i = -RADIUS; i <= RADIUS; ++i)
					{
						for (int j = -RADIUS; j <= RADIUS; ++j)
						{
							accColor += PreWeights[RADIUS + i] * PreWeights[RADIUS + j] * tex2D(_MainTex, inFrag.mainTexcoord.xy + _BlurScale * float2(float(i),float(j)) / _Resolution.xy).rgb;
						}
					}
					
					ret.rgb = lerp(ret.rgb, accColor, _BlurFactor);
						
				    return ret;
                }
                
            ENDCG
        }
    }
    
	FallBack "Diffuse"
}