using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public interface IVncInputPolicy
{
	void WriteKeyboardEvent(uint keysym, bool pressed);

	void WritePointerEvent(byte buttonMask, Point point);
}
