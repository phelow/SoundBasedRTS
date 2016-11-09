Shader "CoolGaussianBlurAdvanced" 
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

                v2f vert(appdata_base v)
                {
                    v2f o;
                   
                    o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
                    o.mainTexcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                   
                    return o;
                }
              
                float weight(in float x, in float sigma)
				{
				    return 0.39895 / sigma * exp( -0.5 * x * x / ( sigma * sigma ) );
				}
				
                float4 frag(v2f inFrag) : COLOR
                {
                    float4 ret = tex2D(_MainTex, inFrag.mainTexcoord.xy);
                    
                    // feel free to modify the value of RADIUS and ARRAY_SZIE
                    // but make sure : ARRAY_SIZE = RADIUS * 2 + 1
                    const int RADIUS = 7;
                    const int ARRAY_SIZE = 15;
                    
					float kernel[ARRAY_SIZE];

					for (int i = 0; i <= RADIUS; ++i)
					{
						kernel[RADIUS + i] = kernel[RADIUS - i] = weight(float(i), 10);
					}
					
					float sum = 0.0;
					for (int i = 0; i < ARRAY_SIZE; ++i)
					{
						sum += kernel[i];
					}
					
					float3 accColor = float3(0,0,0);
					for (int i = -RADIUS; i <= RADIUS; ++i)
					{
						for (int j = -RADIUS; j <= RADIUS; ++j)
						{
							accColor += kernel[RADIUS + j] * kernel[RADIUS + i] * tex2D(_MainTex, inFrag.mainTexcoord.xy + _BlurScale * float2(i, j) / _Resolution.xy).rgb;
						}
					}
					
					ret.rgb = lerp(ret.rgb, accColor / (sum * sum), _BlurFactor);
						
				    return ret;
                }
                
            ENDCG
        }
    }
    
	FallBack "Diffuse"
}