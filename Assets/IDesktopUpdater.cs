using System.Collections;
using System.Collections.Generic;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Formats.Bmp;
using UnityEngine;

public interface IDesktopUpdater
{
	/// <summary>
	/// Given a desktop Bitmap that is a local representation of the remote desktop, updates sent by the server are drawn into the area specifed by UpdateRectangle.
	/// </summary>
	/// <param name="desktop">The desktop Bitmap on which updates should be drawn.</param>
	void Draw(BmpFormat desktop);

	/// <summary>
	/// The region of the desktop Bitmap that needs to be re-drawn.
	/// </summary>
	Rectangle UpdateRectangle
	{
		get;
	}
}
