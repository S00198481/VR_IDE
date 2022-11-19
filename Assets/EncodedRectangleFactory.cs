using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class EncodedRectangleFactory
{
	RfbProtocol rfb;
	Framebuffer framebuffer;

	/// <summary>
	/// Creates an instance of the EncodedRectangleFactory using the connected RfbProtocol object and associated Framebuffer object.
	/// </summary>
	/// <param name="rfb">An RfbProtocol object that will be passed to any created EncodedRectangle objects.  Must be non-null, already initialized, and connected.</param>
	/// <param name="framebuffer">A Framebuffer object which will be used by any created EncodedRectangle objects in order to decode and draw rectangles locally.</param>
	public EncodedRectangleFactory(RfbProtocol rfb, Framebuffer framebuffer)
	{
		System.Diagnostics.Debug.Assert(rfb != null, "RfbProtocol object must be non-null");
		System.Diagnostics.Debug.Assert(framebuffer != null, "Framebuffer object must be non-null");

		this.rfb = rfb;
		this.framebuffer = framebuffer;
	}

	/// <summary>
	/// Creates an object type derived from EncodedRectangle, based on the value of encoding.
	/// </summary>
	/// <param name="rectangle">A Rectangle object defining the bounds of the rectangle to be created</param>
	/// <param name="encoding">An Integer indicating the encoding type to be used for this rectangle.  Used to determine the type of EncodedRectangle to create.</param>
	/// <returns></returns>
	public EncodedRectangle Build(Rectangle rectangle, int encoding)
	{
		EncodedRectangle e = null;

		switch (encoding)
		{
			case RfbProtocol.RAW_ENCODING:
				e = new RawRectangle(rfb, framebuffer, rectangle);
				break;
			case RfbProtocol.COPYRECT_ENCODING:
				e = new CopyRectRectangle(rfb, framebuffer, rectangle);
				break;
			case RfbProtocol.RRE_ENCODING:
				e = new RreRectangle(rfb, framebuffer, rectangle);
				break;
			case RfbProtocol.CORRE_ENCODING:
				e = new CoRreRectangle(rfb, framebuffer, rectangle);
				break;
			case RfbProtocol.HEXTILE_ENCODING:
				e = new HextileRectangle(rfb, framebuffer, rectangle);
				break;
			case RfbProtocol.ZRLE_ENCODING:
				e = new ZrleRectangle(rfb, framebuffer, rectangle);
				break;
			default:
				// Sanity check
				throw new VncProtocolException("Unsupported Encoding Format received: " + encoding.ToString() + ".");
		}
		return e;
	}
}