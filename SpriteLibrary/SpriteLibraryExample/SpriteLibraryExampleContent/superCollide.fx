//Effect makes pixels in texture gray scale  by using the dot operation

sampler TextureSampler: register(s0);

float4 PixelShaderFunction(float2 Tex: TEXCOORD0) : COLOR0
{
	float4 Color = tex2D(TextureSampler, Tex);

	float r = Color.r;
	float g = Color.g;
	float b = Color.b;

	if (b > r && b > g)
	{
		Color.r = 1.0 * b;
		Color.g = 0.9 * b;
		Color.b = 0.0 * b;
	}

	return Color;
}

technique superCollide
{
	pass Pass1
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}