using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class VncProtocolException : ApplicationException
{
	public VncProtocolException() : base()
	{
	}

	public VncProtocolException(string message) : base(message)
	{
	}

	public VncProtocolException(string message, Exception inner) : base(message, inner)
	{
	}

	public VncProtocolException(SerializationInfo info, StreamingContext cxt) : base(info, cxt)
	{
	}
}