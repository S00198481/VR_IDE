using System.Collections;
using System.Collections.Generic;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using UnityEngine;

public sealed class RawRectangle : EncodedRectangle
{
	public RawRectangle(RfbProtocol rfb, Framebuffer framebuffer, Rectangle rectangle)
		: base(rfb, framebuffer, rectangle, RfbProtocol.RAW_ENCODING)
	{
	}

	public override void Decode()
	{
		// Each pixel from the remote server represents a pixel to be drawn
		for (int i = 0; i < rectangle.Width * rectangle.Height; ++i)
		{
			framebuffer[i] = preader.ReadPixel();
		}
	}
}