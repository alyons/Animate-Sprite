//Effect makes pixels in texture gray scale  by using the dot operation

sampler TextureSampler: register(s0);

float4 PixelShaderFunction(float2 Tex: TEXCOORD0) : COLOR0
{
	float4 Color = tex2D(TextureSampler, Tex);
	float r = Color.r;
	float g = Color.g;
	float b = Color.b;
	Color.rgb = dot(Color.rgb, float3(0.7, 0.59, 0.11));
	r = r - (r - Color.rgb);
	g = g - (g - Color.rgb);
	b = b - (b - Color.rgb);
	Color.r = r;
	Color.g = g;
	Color.b = b;

	return Color;
}

technique collide
{
	pass Pass1
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}