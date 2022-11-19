using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public sealed class VncViewInputPolicy : IVncInputPolicy
{
	public VncViewInputPolicy(RfbProtocol rfb)
	{
		Debug.Assert(rfb != null);
	}

	public void WriteKeyboardEvent(uint keysym, bool pressed)
	{
	}

	public void WritePointerEvent(byte buttonMask, Point point)
	{
	}
}