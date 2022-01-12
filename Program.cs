using System;

namespace ObjToXml
{
	static class Program
	{
		static void Main(string[] args)
		{
			if (!Arguments.Parse(args))
			{
				Console.WriteLine(Arguments.Usage);
				return;
			}

			File.Read(Arguments.Source);

			ObjFile.Read();

			ObjXmlFile.Write(Arguments.Destination);
		}
	}
}
