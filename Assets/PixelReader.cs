using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public abstract class PixelReader
{
	protected BinaryReader reader;
	protected Framebuffer framebuffer;

	protected PixelReader(BinaryReader reader, Framebuffer framebuffer)
	{
		this.reader = reader;
		this.framebuffer = framebuffer;
	}

	public abstract int ReadPixel();

	protected int ToGdiPlusOrder(byte red, byte green, byte blue)
	{
		// Put colour values into proper order for GDI+ (i.e., BGRA, where Alpha is always 0xFF)
		return (int)(blue & 0xFF | green << 8 | red << 16 | 0xFF << 24);
	}
}