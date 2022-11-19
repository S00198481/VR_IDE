using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public sealed class PixelReader16 : PixelReader
{
	public PixelReader16(BinaryReader reader, Framebuffer framebuffer) : base(reader, framebuffer)
	{
	}

	public override int ReadPixel()
	{
		// NOTE: the 16 bit pixel value uses a 565 layout (i.e., 5 bits
		// for Red, 6 for Green, 5 for Blue).  As such, I'm doing an extra
		// shift below for each colour to get from 565 to 888 in order to 
		// return a full 32-bit pixel value.
		byte[] b = reader.ReadBytes(2);

		ushort pixel = (ushort)(((uint)b[0]) & 0xFF | ((uint)b[1]) << 8);

		byte red = (byte)(((pixel >> framebuffer.RedShift) & framebuffer.RedMax) << 3);  // 5 bits to 8
		byte green = (byte)(((pixel >> framebuffer.GreenShift) & framebuffer.GreenMax) << 2);  // 6 bits to 8
		byte blue = (byte)(((pixel >> framebuffer.BlueShift) & framebuffer.BlueMax) << 3);  // 5 bits to 8

		return ToGdiPlusOrder(red, green, blue);
	}
}