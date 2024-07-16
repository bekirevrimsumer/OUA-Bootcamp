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


Shader "Customer/SHDBaseMirrorMask"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}

	SubShader
	{
		
		Tags{ "RenderType"="Transparent"  "Queue"="Transparent"  }
				
		
		Pass
		{
		    Blend Zero One
		  		

		   Stencil
	       {
		      Ref 1
			  Comp Always
		      Pass Replace
	       }
		
		
		}
		
	}
}
