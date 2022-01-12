using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjToXml
{
	internal static class BinaryReaderExtensions
	{
		internal static string ReadNullTerminatedString(this BinaryReader reader)
		{
			var characters = new List<char>();

			while (true)
			{
				var character = reader.ReadChar();

				if (character == (char)0)
					break;

				characters.Add(character);
			}

			return new string(characters.ToArray());
		}
	}
}
