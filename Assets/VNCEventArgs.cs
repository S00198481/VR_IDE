using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VncEventArgs : EventArgs
{
	IDesktopUpdater updater;

	public VncEventArgs(IDesktopUpdater updater) : base()
	{
		this.updater = updater;
	}

	/// <summary>
	/// Gets the IDesktopUpdater object that will handling re-drawing the desktop.
	/// </summary>
	public IDesktopUpdater DesktopUpdater
	{
		get
		{
			return updater;
		}
	}
}
