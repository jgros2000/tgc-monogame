#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

// Custom Effects - https://docs.monogame.net/articles/content/custom_effects.html
// High-level shader language (HLSL) - https://docs.microsoft.com/en-us/windows/win32/direct3dhlsl/dx-graphics-hlsl
// Programming guide for HLSL - https://docs.microsoft.com/en-us/windows/win32/direct3dhlsl/dx-graphics-hlsl-pguide
// Reference for HLSL - https://docs.microsoft.com/en-us/windows/win32/direct3dhlsl/dx-graphics-hlsl-reference
// HLSL Semantics - https://docs.microsoft.com/en-us/windows/win32/direct3dhlsl/dx-graphics-hlsl-semantics

// Maximum number of bones
#define MAX_BONES 4

// Bone matrices passed from C#
float4x4 Bones[MAX_BONES];

// World/View/Projection
float4x4 World;
float4x4 View;
float4x4 Projection;

float3 DiffuseColor;

float Time = 0;

struct VertexInput
{
    float3 Position    : POSITION0;
    uint4  BoneIndices : BLENDINDICES0;
    float4 BoneWeights : BLENDWEIGHT0;
};

struct VertexOutput
{
    float4 Position : SV_POSITION;
    float3 Pos: COLOR;
};

VertexOutput MainVS(VertexInput input)
{
    VertexOutput output;

    float4 skinnedPosition = float4(0, 0, 0, 0);

    // Skin the vertex using 4 bone matrices
    [unroll]
    for (int i = 0; i < 4; i++)
    {
        uint boneIndex = input.BoneIndices[i];
        float weight = input.BoneWeights[i];

        float4x4 boneMatrix = Bones[boneIndex];

        skinnedPosition += mul(float4(input.Position, 1.0), boneMatrix) * weight;
    }

    float4 worldPos = mul(skinnedPosition, World);
    float4 viewPos = mul(worldPos, View);
    output.Position = mul(viewPos, Projection);
    output.Pos = float3(input.Position.x,input.Position.y,input.Position.z)+1.0;

    return output;
}

float4 MainPS(VertexOutput input) : COLOR
{
    return float4(input.Pos.x,input.Pos.y,input.Pos.z,DiffuseColor.r);
}

technique BasicColorDrawing
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};
