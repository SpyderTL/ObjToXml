using System;

namespace ObjToXml
{
	internal static class File
	{
		internal static byte[] Data;

		internal static void Read(string source)
		{
			Data = System.IO.File.ReadAllBytes(source);
		}
	}
}