using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public sealed class PixelReader32 : PixelReader
{
	public PixelReader32(BinaryReader reader, Framebuffer framebuffer) : base(reader, framebuffer)
	{
	}

	public override int ReadPixel()
	{
		// Read the pixel value
		byte[] b = reader.ReadBytes(4);

		uint pixel = (uint)(((uint)b[0]) & 0xFF |
							((uint)b[1]) << 8 |
							((uint)b[2]) << 16 |
							((uint)b[3]) << 24);

		// Extract RGB intensities from pixel
		byte red = (byte)((pixel >> framebuffer.RedShift) & framebuffer.RedMax);
		byte green = (byte)((pixel >> framebuffer.GreenShift) & framebuffer.GreenMax);
		byte blue = (byte)((pixel >> framebuffer.BlueShift) & framebuffer.BlueMax);

		return ToGdiPlusOrder(red, green, blue);
	}
}