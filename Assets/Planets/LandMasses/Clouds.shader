Shader "UniPixelPlanet/Clouds"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
	    _Pixels("Pixels", range(10,512)) = 0.0
	    _Rotation("Rotation",range(0.0, 6.28)) = 0.0
	    _Cloud_cover("Cloud cover",range(0.0, 1.0)) = 0.0
    	_Light_origin("Light origin", Vector) = (0.39,0.39,0.39,0.39)
	    _Time_speed("Time Speed",range(-1.0, 1.0)) = 0.2
	    _Stretch("Stretch",range(1.0,3.0)) = 2.0
	    _Cloud_curve("Cloud Curve",range(1.0, 2.0)) = 1.3
	    _Light_border_1("Light border1",range(0.0, 1.0)) = 0.52
	    _Light_border_2("Light border2",range(0.0, 1.0)) = 0.62

	    _Base_color("Base Color", Color) = (1,1,1,1)
    	_Outline_color("Outline Color",Color) = (0,0,0,0)
	    _Shadow_Base_color("Shadow Base Color",Color) = (0,0,0,0)
	    _Shadow_Outline_color("Shadow outline Color", Color) = (0,0,0,0)

	    _Size("Size",float) = 50.0
	    _OCTAVES("OCTAVES", range(0,20)) = 0
	    _Seed("Seed",range(1, 10)) = 1

	    time("time",float) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline" "IgnoreProjector" = "True"}
        //LOD 100

        Pass
        {
			Tags { "LightMode"="UniversalForward"}

			CULL Off
			ZWrite Off // don't write to depth buffer 
         	Blend SrcAlpha OneMinusSrcAlpha // use alpha blending

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #include "../cginc/UniPixelPlanetUtil.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Rotation;
            float _Cloud_cover;
			float2 _Light_origin;    
            float _Stretch;
            float _Cloud_curve;
            float _Light_border_1;
			float _Light_border_2;
			fixed4 _Base_color;
            fixed4 _Outline_color;
			fixed4 _Shadow_Base_color;
			fixed4 _Shadow_Outline_color;
            
			//float _Size;
            //int _OCTAVES;
            //int _Seed;
			//float time;
            
			struct Input
	        {
	            float2 uv_MainTex;
	        };
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }
			
			fixed4 frag(v2f i) : COLOR {
				// pixelize uv

				//no pixelizy
				//float2 uv = i.uv;
				float2 uv = floor(i.uv*_Pixels)/_Pixels;				
				
				// distance to light source
				float d_light = distance(uv , _Light_origin);
				
				// cut out a circle
				float d_circle = distance(uv, float2(0.5,0.5));
				float a = 1.0f;//step(d_circle, 0.5);
				
				float d_to_center = distance(uv, float2(0.5,0.5));
				
				uv = rotate(uv, _Rotation);
				
				// map to sphere
				uv = spherify(uv);
				// slightly make uv go down on the right, and up in the left
				uv.y += smoothstep(0.0, _Cloud_curve, abs(uv.x-0.4));
				
				
				float c = cloud_alpha(uv*float2(1.0, _Stretch));
				
				// assign some colors based on cloud depth & distance from light
				float3 col = _Base_color.rgb;
				if (c < _Cloud_cover + 0.03) {
					col = _Outline_color.rgb;
				}
				if (d_light + c*0.2 > _Light_border_1) {
					col = _Shadow_Base_color.rgb;

				}
				if (d_light + c*0.2 > _Light_border_2) {
					col = _Shadow_Outline_color.rgb;
				}
				
				c *= step(d_to_center, 0.5);
            	return fixed4(col, step(_Cloud_cover, c) * a);
			}
            
            ENDCG
        }
    }
	FallBack "Diffuse"
}
