// Upgrade NOTE: upgraded instancing buffer 'Smoke_Shader_Flipbook02' to new syntax.

// Made with Amplify Shader Editor v1.9.1.5
// Available at the Unity Asset Store - http://u3d.as/y3X
Shader "Smoke_Shader_Flipbook02"
{
	Properties
	{
		_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex ("Particle Texture", 2D) = "white" {}
		_InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
		_Flipbook("Flipbook", Vector) = (8,8,0,0)
		[HDR]_Light_Color("Light_Color", Color) = (1,1,1,0)
		_MainTexture("MainTexture", 2D) = "white" {}
		[HDR]_Main_Color("Main_Color", Color) = (1,1,1,0)
		_MotionVector("MotionVector", 2D) = "white" {}
		_DepthFade("Depth Fade", Float) = 1
		_Influense("Influense", Float) = 0.003

	}


	Category
	{
		SubShader
		{
		LOD 0

			Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMask RGB
			Cull Off
			Lighting Off
			ZWrite Off
			ZTest LEqual

			Pass {

				CGPROGRAM

				#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
				#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
				#endif

				#pragma vertex vert
				#pragma fragment frag
				#pragma target 2.0
				#pragma multi_compile_instancing
				#pragma multi_compile_particles
				#pragma multi_compile_fog
				#include "Lighting.cginc"
				#include "AutoLight.cginc"
				#define ASE_NEEDS_FRAG_COLOR


				#include "UnityCG.cginc"

				struct appdata_t
				{
					float4 vertex : POSITION;
					fixed4 color : COLOR;
					float4 texcoord : TEXCOORD0;
					UNITY_VERTEX_INPUT_INSTANCE_ID
					float4 ase_texcoord1 : TEXCOORD1;
				};

				struct v2f
				{
					float4 vertex : SV_POSITION;
					fixed4 color : COLOR;
					float4 texcoord : TEXCOORD0;
					UNITY_FOG_COORDS(1)
					#ifdef SOFTPARTICLES_ON
					float4 projPos : TEXCOORD2;
					#endif
					UNITY_VERTEX_INPUT_INSTANCE_ID
					UNITY_VERTEX_OUTPUT_STEREO
					float4 ase_texcoord3 : TEXCOORD3;
					float4 ase_texcoord4 : TEXCOORD4;
					float4 ase_texcoord5 : TEXCOORD5;
				};


				#if UNITY_VERSION >= 560
				UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
				#else
				uniform sampler2D_float _CameraDepthTexture;
				#endif

				//Don't delete this comment
				// uniform sampler2D_float _CameraDepthTexture;

				uniform sampler2D _MainTex;
				uniform fixed4 _TintColor;
				uniform float4 _MainTex_ST;
				uniform float _InvFade;
				//This is a late directive

				uniform float4 _Main_Color;
				uniform float4 _Light_Color;
				uniform sampler2D _MainTexture;
				uniform float2 _Flipbook;
				uniform sampler2D _MotionVector;
				uniform float _Influense;
				uniform float4 _CameraDepthTexture_TexelSize;
				uniform float _DepthFade;
				UNITY_INSTANCING_BUFFER_START(Smoke_Shader_Flipbook02)
				UNITY_INSTANCING_BUFFER_END(Smoke_Shader_Flipbook02)


				v2f vert ( appdata_t v  )
				{
					v2f o;
					UNITY_SETUP_INSTANCE_ID(v);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
					UNITY_TRANSFER_INSTANCE_ID(v, o);
					float3 ase_worldPos = mul(unity_ObjectToWorld, float4( (v.vertex).xyz, 1 )).xyz;
					o.ase_texcoord3.xyz = ase_worldPos;
					float4 ase_clipPos = UnityObjectToClipPos(v.vertex);
					float4 screenPos = ComputeScreenPos(ase_clipPos);
					o.ase_texcoord5 = screenPos;

					o.ase_texcoord4 = v.ase_texcoord1;

					//setting value to unused interpolator channels and avoid initialization warnings
					o.ase_texcoord3.w = 0;

					v.vertex.xyz +=  float3( 0, 0, 0 ) ;
					o.vertex = UnityObjectToClipPos(v.vertex);
					#ifdef SOFTPARTICLES_ON
						o.projPos = ComputeScreenPos (o.vertex);
						COMPUTE_EYEDEPTH(o.projPos.z);
					#endif
					o.color = v.color;
					o.texcoord = v.texcoord;
					UNITY_TRANSFER_FOG(o,o.vertex);
					return o;
				}

				fixed4 frag ( v2f i  ) : SV_Target
				{
					UNITY_SETUP_INSTANCE_ID( i );
					UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( i );

					#ifdef SOFTPARTICLES_ON
						float sceneZ = LinearEyeDepth (SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));
						float partZ = i.projPos.z;
						float fade = saturate (_InvFade * (sceneZ-partZ));
						i.color.a *= fade;
					#endif

					float2 texCoord57 = i.texcoord.xy * float2( 2,2 ) + float2( -1,-1 );
					float dotResult55 = dot( texCoord57 , texCoord57 );
					float3 appendResult56 = (float3(texCoord57 , sqrt( saturate( ( 1.0 - dotResult55 ) ) )));
					float3 ase_worldPos = i.ase_texcoord3.xyz;
					float3 worldSpaceLightDir = UnityWorldSpaceLightDir(ase_worldPos);
					float3 worldToViewDir67 = mul( UNITY_MATRIX_V, float4( worldSpaceLightDir, 0 ) ).xyz;
					float dotResult61 = dot( appendResult56 , worldToViewDir67 );
					float4 lerpResult69 = lerp( _Main_Color , _Light_Color , saturate( dotResult61 ));
					float temp_output_4_0_g2 = _Flipbook.x;
					float temp_output_5_0_g2 = _Flipbook.y;
					float2 appendResult7_g2 = (float2(temp_output_4_0_g2 , temp_output_5_0_g2));
					float totalFrames39_g2 = ( temp_output_4_0_g2 * temp_output_5_0_g2 );
					float2 appendResult8_g2 = (float2(totalFrames39_g2 , temp_output_5_0_g2));
					float4 texCoord84 = i.ase_texcoord4;
					texCoord84.xy = i.ase_texcoord4.xy * float2( 1,1 ) + float2( 0,0 );
					float4 texCoord83 = i.texcoord;
					texCoord83.xy = i.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
					float clampResult42_g2 = clamp( texCoord83.z , 0.0001 , ( totalFrames39_g2 - 1.0 ) );
					float temp_output_35_0_g2 = frac( ( ( texCoord84.z + clampResult42_g2 ) / totalFrames39_g2 ) );
					float2 appendResult29_g2 = (float2(temp_output_35_0_g2 , ( 1.0 - temp_output_35_0_g2 )));
					float2 temp_output_15_0_g2 = ( ( i.texcoord.xy / appendResult7_g2 ) + ( floor( ( appendResult8_g2 * appendResult29_g2 ) ) / appendResult7_g2 ) );
					float2 temp_output_7_0 = temp_output_15_0_g2;
					float2 temp_cast_0 = (1.0).xx;
					float4 tex2DNode9 = tex2D( _MainTexture, ( temp_output_7_0 - ( ( ( ( (tex2D( _MotionVector, temp_output_7_0 )).rg * 2.0 ) - temp_cast_0 ) * frac( texCoord84.z ) ) * _Influense ) ) );
					float4 screenPos = i.ase_texcoord5;
					float4 ase_screenPosNorm = screenPos / screenPos.w;
					ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
					float screenDepth73 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
					float distanceDepth73 = saturate( abs( ( screenDepth73 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _DepthFade ) ) );
					float4 appendResult53 = (float4(( ( lerpResult69 * i.color ) * tex2DNode9 ).rgb , ( ( i.color.a * tex2DNode9.a ) * distanceDepth73 )));


					fixed4 col = appendResult53;
					UNITY_APPLY_FOG(i.fogCoord, col);
					return col;
				}
				ENDCG
			}
		}
	}
	//CustomEditor "ASEMaterialInspector"

	Fallback Off
}
/*ASEBEGIN
Version=19105
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-1649.758,204.7667;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;24;-618.9548,175.8365;Inherit;False;Constant;_Float0;Float 0;5;0;Create;True;0;0;0;False;0;False;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;-83.53568,38.39453;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;160.655,37.70856;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;39;-10.96654,170.4187;Inherit;False;Property;_Influense;Influense;7;0;Create;True;0;0;0;False;0;False;0.003;0.002;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;25;-314.555,-12.03115;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;40;320.5705,110.4502;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;-528.8987,-42.52274;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;8;-1724.718,10.10409;Inherit;False;Property;_Flipbook;Flipbook;0;0;Create;True;0;0;0;False;0;False;8,8;8,8;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.FunctionNode;7;-1460.672,51.89872;Inherit;False;Flipbook;-1;;2;53c2488c220f6564ca6c90721ee16673;2,71,0,68,0;8;51;SAMPLER2D;0.0;False;13;FLOAT2;0,0;False;4;FLOAT;3;False;5;FLOAT;3;False;24;FLOAT;0;False;2;FLOAT;0;False;55;FLOAT;0;False;70;FLOAT;0;False;5;COLOR;53;FLOAT2;0;FLOAT;47;FLOAT;48;FLOAT;62
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;54;1844.302,145.3845;Float;False;True;-1;2;ASEMaterialInspector;0;11;Smoke_Shader_Flipbook02;0b6a9f8b4f707c74ca64c0be8e590de0;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;2;False;True;2;5;False;;10;False;;0;1;False;;0;False;;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;True;True;True;True;False;0;False;;False;False;False;False;False;False;False;False;False;True;2;False;;True;3;False;;False;True;4;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;False;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;0;;0;0;Standard;0;0;1;True;False;;False;0
Node;AmplifyShaderEditor.DynamicAppendNode;53;1629.89,149.3627;Inherit;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ComponentMaskNode;20;-775.8317,-26.96826;Inherit;False;True;True;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;17;-1148.573,-114.9323;Inherit;True;Property;_MotionVector;MotionVector;5;0;Create;True;0;0;0;False;0;False;-1;None;bbb3aae545376f042b7479bf672c38fb;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FractNode;35;-277.075,196.1377;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;55;-725.3322,-528.1332;Inherit;True;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;56;72.95422,-435.0472;Inherit;True;FLOAT3;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SqrtOpNode;58;-62.70583,-344.8077;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;59;-287.9531,-472.1968;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;60;-497.0713,-507.5183;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;69;1199.174,-339.3687;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;65;1353.074,-31.67062;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;70;1432.621,101.4276;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;71;1292.075,260.2178;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TransformDirectionNode;67;146.5876,-206.8597;Inherit;False;World;View;False;Fast;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DotProductOpNode;61;353.4367,-382.2905;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;68;-86.25835,-215.0576;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SaturateNode;64;627.9417,-384.3247;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;57;-1010.357,-551.968;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;2,2;False;1;FLOAT2;-1,-1;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;66;822.6894,-201.5625;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;9;504.4342,35.60888;Inherit;True;Property;_MainTexture;MainTexture;2;0;Create;True;0;0;0;False;0;False;-1;None;26ab0f98f6210124bab6dc8542362400;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;72;989.4667,456.9137;Inherit;False;Property;_DepthFade;Depth Fade;6;0;Create;True;0;0;0;False;0;False;1;3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;73;1219.867,405.0457;Inherit;False;True;True;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;3;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;74;1503.667,321.9797;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;75;-1703.27,327.0043;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;64;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;27;-453.0183,136.5604;Inherit;False;Constant;_Float1;Float 1;5;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;13;-1975.019,307.0361;Inherit;False;InstancedProperty;_Speed;Speed;4;0;Create;True;0;0;0;False;0;False;5;44;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;81;-2002.84,103.5616;Inherit;False;Property;_Start_Frame;Start_Frame;8;1;[IntRange];Create;True;0;0;0;False;0;False;0;1;1;64;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;80;-2394.164,29.1557;Inherit;False;Random Range;-1;;3;7b754edb8aebbfb4a9ace907af661cfc;0;3;1;FLOAT2;1,1;False;2;FLOAT;1;False;3;FLOAT;64;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;83;-2213.84,153.5616;Inherit;False;0;-1;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;12;-1916.758,212.7667;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;84;-2043.823,431.0609;Inherit;False;1;-1;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;79;-2213.979,-132.1061;Inherit;True;1;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;62;579.1134,-607.7019;Inherit;False;Property;_Main_Color;Main_Color;3;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,0;1.327179,1.327179,1.327179,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;63;363.8827,-607.6085;Inherit;False;Property;_Light_Color;Light_Color;1;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
WireConnection;10;0;12;0
WireConnection;10;1;84;3
WireConnection;33;0;25;0
WireConnection;33;1;35;0
WireConnection;37;0;33;0
WireConnection;37;1;39;0
WireConnection;25;0;22;0
WireConnection;25;1;27;0
WireConnection;40;0;7;0
WireConnection;40;1;37;0
WireConnection;22;0;20;0
WireConnection;22;1;24;0
WireConnection;7;4;8;1
WireConnection;7;5;8;2
WireConnection;7;24;83;3
WireConnection;7;2;84;3
WireConnection;54;0;53;0
WireConnection;53;0;70;0
WireConnection;53;3;74;0
WireConnection;20;0;17;0
WireConnection;17;1;7;0
WireConnection;35;0;84;3
WireConnection;55;0;57;0
WireConnection;55;1;57;0
WireConnection;56;0;57;0
WireConnection;56;2;58;0
WireConnection;58;0;59;0
WireConnection;59;0;60;0
WireConnection;60;0;55;0
WireConnection;69;0;62;0
WireConnection;69;1;63;0
WireConnection;69;2;64;0
WireConnection;65;0;69;0
WireConnection;65;1;66;0
WireConnection;70;0;65;0
WireConnection;70;1;9;0
WireConnection;71;0;66;4
WireConnection;71;1;9;4
WireConnection;67;0;68;0
WireConnection;61;0;56;0
WireConnection;61;1;67;0
WireConnection;64;0;61;0
WireConnection;9;1;40;0
WireConnection;73;0;72;0
WireConnection;74;0;71;0
WireConnection;74;1;73;0
WireConnection;79;1;80;0
ASEEND*/
//CHKSM=17E0A68F0D7566A659D8F088DB9C4CC1B575CEEF
