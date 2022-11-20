using System.Collections;
using System.Collections.Generic;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using UnityEngine;

public sealed class CoRreRectangle : EncodedRectangle
{
	public CoRreRectangle(RfbProtocol rfb, Framebuffer framebuffer, Rectangle rectangle) : base(rfb, framebuffer, rectangle, RfbProtocol.CORRE_ENCODING)
	{
	}

	/// <summary>
	/// Decodes a CoRRE Encoded Rectangle.
	/// </summary>
	public override void Decode()
	{
		int numSubRect = (int)rfb.ReadUint32(); // Number of sub-rectangles within this rectangle
		int bgPixelVal = preader.ReadPixel();       // Background colour
		int subRectVal = 0;                         // Colour to be used for each sub-rectangle

		// Dimensions of each sub-rectangle will be read into these
		int x, y, w, h;

		// Initialize the full pixel array to the background colour
		FillRectangle(rectangle, bgPixelVal);

		// Colour in all the subrectangles, reading the properties of each one after another.
		for (int i = 0; i < numSubRect; i++)
		{
			subRectVal = preader.ReadPixel();
			x = (int)rfb.ReadByte();
			y = (int)rfb.ReadByte();
			w = (int)rfb.ReadByte();
			h = (int)rfb.ReadByte();

			// Colour in this sub-rectangle with the colour provided.
			FillRectangle(new Rectangle(x, y, w, h), subRectVal);
		}
	}
}