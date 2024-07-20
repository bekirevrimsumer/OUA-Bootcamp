/******************************************************************************
 *                                                                            *
 *                                                                            *
 *                          Fantasy Mirror                                    *
 *                                                                            *
 *                           Version: 1.1.0                                   *
 *                                                                            *
 *                                                                            *
 *                                                                            *
 *  Author: Yi Zhen                                                           *
 *  Date  : 2021-2                                                            *
 *                                                                            *
 ******************************************************************************/




Shader "Customer/SHDBaseMirrorBlock"
{
	Properties
	{
		_MirrorMap("Texture", 2D) = "white" {}
 	}

    //Just mirror 
	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+1"   }

		Pass
		{
		  
			Stencil
			{
			  Ref 0
			  Comp NotEqual
			}
		


			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
						
			#include "UnityCG.cginc"
            #include "UnityShaderVariables.cginc"


			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 vpp : TEXCOORD1;
			};

			sampler2D _MirrorMap;
			float fAspect;
					
			v2f vert (appdata v)
			{
				v2f o;

				o.vertex = UnityObjectToClipPos(v.vertex);
				o.vpp = o.vertex;
				

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float2 uv = i.vpp.xy;
								
				uv.x = uv.x / i.vpp.w;
				uv.y = uv.y / (i.vpp.w * fAspect);
				
				uv.x = 1.0 - (uv.x + 1.0) * 0.5;				
				uv.y = 1.0 - (uv.y + 1.0) * 0.5;

				fixed4 col = tex2D(_MirrorMap, uv);

				return col;
			}
			ENDCG
		}
	}

}
