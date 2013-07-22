float4x4 World;
float4x4 View;
float4x4 Projection;
float DistortionScale;
float Time;

struct PositionPosition
{
	float4 Position : POSITION;

	float4 PositionAsTexCoord : TEXCOORD;
};

PositionPosition TransformAndCopyPosition_VertexShader(float4 position : POSITION)
{
	PositionPosition output;
	
	output.Position = mul(position, Projection);
	output.PositionAsTexCoord = output.Position;

	return output;
}

float4 HeatHaze_PixelShader(float4 position : TEXCOORD) : COLOR
{
	float2 displacement;

	displacement.x = sin(position.x / 60 + Time * 1.5) * sin(position.x / 10) * cos(position.x / 50);
	displacement.y = sin(position.y / 50 - Time * 2.75);
	displacement *= 3.0f;
	displacement = (displacement + float2(1, 1)) / 2;

	return float4(displacement, 0, 1);
}

technique HeatHaze
{
	pass
	{
		VertexShader = compile vs_1_1 TransformAndCopyPosition_VertexShader();
		PixelShader = compile ps_2_0 HeatHaze_PixelShader();
	}
}