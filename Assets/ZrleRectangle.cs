using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public sealed class ZrleRectangle : EncodedRectangle
{
	private const int TILE_WIDTH = 64;
	private const int TILE_HEIGHT = 64;

	static int[] palette = new int[128];
	static int[] tileBuffer = new int[TILE_WIDTH * TILE_HEIGHT];

	public ZrleRectangle(RfbProtocol rfb, Framebuffer framebuffer, Rectangle rectangle)
		: base(rfb, framebuffer, rectangle, RfbProtocol.ZRLE_ENCODING)
	{
	}

	public override void Decode()
	{
		rfb.ZrleReader.DecodeStream();

		for (int ty = 0; ty < rectangle.Height; ty += TILE_HEIGHT)
		{
			int th = Math.Min(rectangle.Height - ty, TILE_HEIGHT);

			for (int tx = 0; tx < rectangle.Width; tx += TILE_WIDTH)
			{
				int tw = Math.Min(rectangle.Width - tx, TILE_WIDTH);

				byte subencoding = rfb.ZrleReader.ReadByte();

				if ((subencoding >= 17 && subencoding <= 127) || subencoding == 129)
					throw new Exception("Invalid subencoding value");

				bool isRLE = (subencoding & 128) != 0;
				int paletteSize = subencoding & 127;

				// Fill palette
				for (int i = 0; i < paletteSize; i++)
					palette[i] = preader.ReadPixel();

				if (paletteSize == 1)
				{
					// Solid tile
					FillRectangle(new Rectangle(tx, ty, tw, th), palette[0]);
					continue;
				}

				if (!isRLE)
				{
					if (paletteSize == 0)
					{
						// Raw pixel data
						FillRectangle(new Rectangle(tx, ty, tw, th));
					}
					else
					{
						// Packed palette
						ReadZrlePackedPixels(tw, th, palette, paletteSize, tileBuffer);
						FillRectangle(new Rectangle(tx, ty, tw, th), tileBuffer);
					}
				}
				else
				{
					if (paletteSize == 0)
					{
						// Plain RLE
						ReadZrlePlainRLEPixels(tw, th, tileBuffer);
						FillRectangle(new Rectangle(tx, ty, tw, th), tileBuffer);
					}
					else
					{
						// Packed RLE palette
						ReadZrlePackedRLEPixels(tx, ty, tw, th, palette, tileBuffer);
						FillRectangle(new Rectangle(tx, ty, tw, th), tileBuffer);
					}
				}
			}
		}
	}

	private void ReadZrlePackedPixels(int tw, int th, int[] palette, int palSize, int[] tile)
	{
		int bppp = ((palSize > 16) ? 8 :
		  ((palSize > 4) ? 4 : ((palSize > 2) ? 2 : 1)));
		int ptr = 0;

		for (int i = 0; i < th; i++)
		{
			int eol = ptr + tw;
			int b = 0;
			int nbits = 0;

			while (ptr < eol)
			{
				if (nbits == 0)
				{
					b = rfb.ZrleReader.ReadByte();
					nbits = 8;
				}
				nbits -= bppp;
				int index = (b >> nbits) & ((1 << bppp) - 1) & 127;
				tile[ptr++] = palette[index];
			}
		}
	}

	private void ReadZrlePlainRLEPixels(int tw, int th, int[] tileBuffer)
	{
		int ptr = 0;
		int end = ptr + tw * th;
		while (ptr < end)
		{
			int pix = preader.ReadPixel();
			int len = 1;
			int b;
			do
			{
				b = rfb.ZrleReader.ReadByte();
				len += b;
			} while (b == byte.MaxValue);

			while (len-- > 0) tileBuffer[ptr++] = pix;
		}
	}

	private void ReadZrlePackedRLEPixels(int tx, int ty, int tw, int th, int[] palette, int[] tile)
	{
		int ptr = 0;
		int end = ptr + tw * th;
		while (ptr < end)
		{
			int index = rfb.ZrleReader.ReadByte();
			int len = 1;
			if ((index & 128) != 0)
			{
				int b;
				do
				{
					b = rfb.ZrleReader.ReadByte();
					len += b;
				} while (b == byte.MaxValue);
			}

			index &= 127;

			while (len-- > 0) tile[ptr++] = palette[index];
		}
	}
}