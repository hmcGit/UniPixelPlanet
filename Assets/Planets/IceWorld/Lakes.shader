Shader "Unlit/Lakes"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
	    _Pixels("Pixels", range(10,100)) = 0.0
	    _Rotation("Rotation",range(0.0, 6.28)) = 0.0
    	_Light_origin("Light origin", Vector) = (0.39,0.39,0.39,0.39)
	    _Time_speed("Time Speed",range(-1.0, 1.0)) = 0.2
	    _Dither_Size("Dither Size",range(0.0, 10.0)) = 2.0
    	
	    _Light_border_1("Light border1",range(0.0, 1.0)) = 0.52
	    _Light_border_2("Light border2",range(0.0, 1.0)) = 0.62
		_Lake_cutoff("Lake Cutoff",range(0.0, 1.0)) = 0.0
    	    	
	    _Color1("Color1", Color) = (1,1,1,1)
    	_Color2("Color2", Color) = (1,1,1,1)
    	_Color3("Color3", Color) = (1,1,1,1)
    	
	    _Size("Size",float) = 50.0
	    _OCTAVES("OCTAVES", range(0,20)) = 0
	    _Seed("Seed",range(1, 10)) = 1
	    time("time",float) = 0.0
    	
    }
    SubShader
    {
        //Tags { "RenderType"="Opaque" }
        Tags { "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline" "IgnoreProjector" = "True"}
        LOD 100

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
            #include "../cginc/hlmod.cginc"
            
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
            float _Pixels;
            float _Rotation;
            float _Dither_Size;
			float2 _Light_origin;    	
			float _Time_speed;
            float _Light_border_1;
			float _Light_border_2;
            float _Lake_cutoff;
			float _Size;
            int _OCTAVES;
            int _Seed;
			float time;
    		fixed4 _Color1;
            fixed4 _Color2;
            fixed4 _Color3;
            
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
			float rand(float2 coord) {
				coord = mod(coord, float2(2.0,1.0)*round(_Size));
				return frac(sin(dot(coord.xy ,float2(12.9898,78.233))) * 43758.5453 * _Seed);
			}

			float noise(float2 coord){
				float2 i = floor(coord);
				float2 f = frac(coord);
				
				float a = rand(i);
				float b = rand(i + float2(1.0, 0.0));
				float c = rand(i + float2(0.0, 1.0));
				float d = rand(i + float2(1.0, 1.0));

				float2 cubic = f * f * (3.0 - 2.0 * f);

				return lerp(a, b, cubic.x) + (c - a) * cubic.y * (1.0 - cubic.x) + (d - b) * cubic.x * cubic.y;
			}
			bool dither(float2 uv1, float2 uv2) {
				return mod(uv1.x+uv2.y,2.0/_Pixels) <= 1.0 / _Pixels;
			}

			float fbm(float2 coord){
				float value = 0.0;
				float scale = 0.5;

				for(int i = 0; i < _OCTAVES ; i++){
					value += noise(coord) * scale;
					coord *= 2.0;
					scale *= 0.5;
				}
				return value;
			}

			float2 spherify(float2 uv) {
				float2 centered= uv *2.0-1.0;
				float z = sqrt(1.0 - dot(centered.xy, centered.xy));
				float2 sphere = centered/(z + 1.0);
				return sphere * 0.5+0.5;
			}

			float2 rotate(float2 coord, float angle){
				coord -= 0.5;
				//coord *= float2x2(float2(cos(angle),-sin(angle)),float2(sin(angle),cos(angle)));
            	coord = mul(coord,float2x2(float2(cos(angle),-sin(angle)),float2(sin(angle),cos(angle))));
				return coord + 0.5;
			}

			fixed4 frag(v2f i) : COLOR {
				// pixelize uv
            	
				float2 uv = floor(i.uv*_Pixels)/_Pixels;
				//uv.y = 1 - uv.y;

				float d_light = distance(uv , _Light_origin);
				
				// give planet a tilt
				uv = rotate(uv, _Rotation);

			//	// map to sphere
				uv = spherify(uv);
				
				// some scrolling noise for landmasses
				float fbm1 = fbm(uv*_Size+float2(time*_Time_speed,0.0));
				float lake = fbm(uv*_Size+float2(time*_Time_speed,0.0));
				
				// increase contrast on d_light
				d_light = pow(d_light, 2.0)*0.4;
				d_light -= d_light * lake;

				
				float3 col = _Color1.rgb;
				if (d_light > _Light_border_1) {
					col = _Color2.rgb;
				}
				if (d_light > _Light_border_2) {
					col = _Color3.rgb;
				}
				
				float a = step(_Lake_cutoff, lake);
				a*= step(distance(float2(0.5,0.5), uv), 0.5);
				return fixed4(col, a);
				}
            
            ENDCG
        }
    }
}
