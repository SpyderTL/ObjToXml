namespace ObjToXml
{
	internal static class Arguments
	{
		internal static string Source;
		internal static string Destination;
		internal const string Usage = "Usage: ObjToXml.exe source [destination]";

		internal static bool Parse(string[] arguments)
		{
			if (arguments.Length == 0)
				return false;

			Source = arguments[0];

			if (arguments.Length > 1)
				Destination = arguments[1];
			else
				Destination = System.IO.Path.Join(System.IO.Path.GetDirectoryName(arguments[0]), System.IO.Path.GetFileNameWithoutExtension(arguments[0]) + ".xml");

			return true;
		}
	}
}