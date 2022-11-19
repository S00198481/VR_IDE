using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public sealed class CPixelReader : PixelReader
{
	public CPixelReader(BinaryReader reader, Framebuffer framebuffer) : base(reader, framebuffer)
	{
	}

	public override int ReadPixel()
	{
		byte[] b = reader.ReadBytes(3);
		return ToGdiPlusOrder(b[2], b[1], b[0]);
	}
}