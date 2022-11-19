using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public sealed class RreRectangle : EncodedRectangle
{
	public RreRectangle(RfbProtocol rfb, Framebuffer framebuffer, Rectangle rectangle)
		: base(rfb, framebuffer, rectangle, RfbProtocol.RRE_ENCODING)
	{
	}

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
			x = (int)rfb.ReadUInt16();
			y = (int)rfb.ReadUInt16();
			w = (int)rfb.ReadUInt16();
			h = (int)rfb.ReadUInt16();

			// Colour in this sub-rectangle
			FillRectangle(new Rectangle(x, y, w, h), subRectVal);
		}
	}
}