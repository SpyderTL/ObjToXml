using System;
using System.Linq;
using System.Xml;

namespace ObjToXml
{
	internal class ObjXmlFile
	{
		internal static void Write(string destination)
		{
			var settings = new XmlWriterSettings
			{
				Indent = true,
				IndentChars = "\t"
			};

			var writer = XmlWriter.Create(destination, settings);

			writer.WriteStartDocument();
			writer.WriteStartElement("objectFile");

			if (Enumerable.SequenceEqual(ObjFile.PortableExecutableSignature, new char[] { 'P', 'E', (char)0, (char)0 }))
			{
				// Executable Header
				writer.WriteStartElement("executableHeader");

				writer.WriteElementString("magic", new string(ObjFile.ExecutableHeader.Magic));
				writer.WriteElementString("lastPageSize", ObjFile.ExecutableHeader.LastPageSize.ToString());
				writer.WriteElementString("pageCount", ObjFile.ExecutableHeader.PageCount.ToString());
				writer.WriteElementString("relocationCount", ObjFile.ExecutableHeader.RelocationCount.ToString());
				writer.WriteElementString("headerSize", ObjFile.ExecutableHeader.HeaderSize.ToString());
				writer.WriteElementString("minimumParagraphCount", ObjFile.ExecutableHeader.MinimumParagraphCount.ToString());
				writer.WriteElementString("maximumParagraphCount", ObjFile.ExecutableHeader.MaximumParagraphCount.ToString());
				writer.WriteElementString("stackSegment", ObjFile.ExecutableHeader.StackSegment.ToString("X4"));
				writer.WriteElementString("stackPointer", ObjFile.ExecutableHeader.StackPointer.ToString("X4"));
				writer.WriteElementString("checksum", ObjFile.ExecutableHeader.Checksum.ToString());
				writer.WriteElementString("instructionPointer", ObjFile.ExecutableHeader.InstructionPointer.ToString());
				writer.WriteElementString("codeSegment", ObjFile.ExecutableHeader.CodeSegment.ToString());
				writer.WriteElementString("relocationAddress", ObjFile.ExecutableHeader.RelocationAddress.ToString("X8"));
				writer.WriteElementString("overlayNumber", ObjFile.ExecutableHeader.OverlayNumber.ToString());
				writer.WriteElementString("reserved", Convert.ToHexString(ObjFile.ExecutableHeader.Reserved));
				writer.WriteElementString("oemID", ObjFile.ExecutableHeader.OemID.ToString());
				writer.WriteElementString("oemInformation", ObjFile.ExecutableHeader.OemInformation.ToString());
				writer.WriteElementString("reserved2", Convert.ToHexString(ObjFile.ExecutableHeader.Reserved2));
				writer.WriteElementString("nextHeaderAddress", ObjFile.ExecutableHeader.NextHeaderAddress.ToString("X8"));

				writer.WriteEndElement();

				// MS-DOS Stub
				writer.WriteElementString("msdosStub", Convert.ToHexString(ObjFile.MsDosStub));

				// PE Signature
				writer.WriteElementString("portableExecutableSignature", Convert.ToHexString(ObjFile.PortableExecutableSignature.Select(x => (byte)x).ToArray()));

				// Object Header
				writer.WriteStartElement("objectHeader");

				writer.WriteElementString("machine", ObjFile.ObjectHeader.Machine.ToString("X4"));
				writer.WriteElementString("sectionCount", ObjFile.ObjectHeader.SectionCount.ToString());
				var dateTimeStamp = new DateTime(1970, 1, 1).AddSeconds(ObjFile.ObjectHeader.DateTimeStamp);
				writer.WriteComment(dateTimeStamp.ToString());
				writer.WriteElementString("dateTimeStamp", ObjFile.ObjectHeader.DateTimeStamp.ToString());
				writer.WriteElementString("symbolTablePointer", ObjFile.ObjectHeader.SymbolTablePointer.ToString("X8"));
				writer.WriteElementString("symbolTableNumber", ObjFile.ObjectHeader.SymbolTableNumber.ToString("X8"));
				writer.WriteElementString("optionalHeaderSize", ObjFile.ObjectHeader.OptionalHeaderSize.ToString("X4"));
				writer.WriteElementString("characteristics", ObjFile.ObjectHeader.Characteristics.ToString("X4"));

				writer.WriteEndElement();

				if (ObjFile.ObjectHeader.OptionalHeaderSize > 0)
				{
					// Optional Header
					writer.WriteStartElement("optionalHeader");

					writer.WriteElementString("magic", ObjFile.ObjectOptionalHeader.Magic.ToString("X4"));
					writer.WriteElementString("linkerVersionMajor", ObjFile.ObjectOptionalHeader.LinkerVersionMajor.ToString());
					writer.WriteElementString("linkerVersionMinor", ObjFile.ObjectOptionalHeader.LinkerVersionMinor.ToString());
					writer.WriteElementString("codeSize", ObjFile.ObjectOptionalHeader.CodeSize.ToString("X8"));
					writer.WriteElementString("initializedDataSize", ObjFile.ObjectOptionalHeader.InitializedDataSize.ToString("X8"));
					writer.WriteElementString("uninitializedDataSize", ObjFile.ObjectOptionalHeader.UninitializedDataSize.ToString("X8"));
					writer.WriteElementString("entryPointAddress", ObjFile.ObjectOptionalHeader.EntryPointAddress.ToString("X8"));
					writer.WriteElementString("codeBase", ObjFile.ObjectOptionalHeader.CodeBase.ToString("X8"));

					if (ObjFile.ObjectOptionalHeader.Magic != 0x020B)
						writer.WriteElementString("dataBase", ObjFile.ObjectOptionalHeader.DataBase.ToString("X8"));

					writer.WriteEndElement();

					if (ObjFile.ObjectHeader.OptionalHeaderSize > 1) // TODO: Find actual length
					{
						writer.WriteStartElement("windowsHeader");

						if (ObjFile.ObjectOptionalHeader.Magic != 0x020B)
						{
							writer.WriteElementString("imageBase", ObjFile.WindowsHeader.ImageBase.ToString("X8"));
							writer.WriteElementString("sectionAlignment", ObjFile.WindowsHeader.SectionAlignment.ToString("X8"));
							writer.WriteElementString("fileAlignment", ObjFile.WindowsHeader.FileAlignment.ToString("X8"));
							writer.WriteElementString("operatingSystemMajorVersion", ObjFile.WindowsHeader.OperatingSystemMajorVersion.ToString("X4"));
							writer.WriteElementString("operatingSystemMinorVersion", ObjFile.WindowsHeader.OperatingSystemMinorVersion.ToString("X4"));
							writer.WriteElementString("imageMajorVersion", ObjFile.WindowsHeader.ImageMajorVersion.ToString("X4"));
							writer.WriteElementString("imageMinorVersion", ObjFile.WindowsHeader.ImageMinorVersion.ToString("X4"));
							writer.WriteElementString("subsystemMajorVersion", ObjFile.WindowsHeader.SubsystemMajorVersion.ToString("X4"));
							writer.WriteElementString("subsystemMinorVersion", ObjFile.WindowsHeader.SubsystemMinorVersion.ToString("X4"));
							writer.WriteElementString("windowsVersion", ObjFile.WindowsHeader.WindowsVersion.ToString("X8"));
							writer.WriteElementString("imageSize", ObjFile.WindowsHeader.ImageSize.ToString("X8"));
							writer.WriteElementString("headerSize", ObjFile.WindowsHeader.HeaderSize.ToString("X8"));
							writer.WriteElementString("checksum", ObjFile.WindowsHeader.Checksum.ToString("X8"));
							writer.WriteElementString("subsystem", ObjFile.WindowsHeader.Subsystem.ToString("X4"));
							writer.WriteElementString("dllCharacteristics", ObjFile.WindowsHeader.DllCharacteristics.ToString("X4"));
							writer.WriteElementString("stackReserveSize", ObjFile.WindowsHeader.StackReserveSize.ToString("X8"));
							writer.WriteElementString("stackCommitSize", ObjFile.WindowsHeader.StackCommitSize.ToString("X8"));
							writer.WriteElementString("heapReserveSize", ObjFile.WindowsHeader.HeapReserveSize.ToString("X8"));
							writer.WriteElementString("heapCommitSize", ObjFile.WindowsHeader.HeapCommitSize.ToString("X8"));
							writer.WriteElementString("loaderFlags", ObjFile.WindowsHeader.LoaderFlags.ToString("X8"));
							writer.WriteElementString("rvaCountSize", ObjFile.WindowsHeader.RvaCountSize.ToString("X8"));
						}
						else
						{
							writer.WriteElementString("imageBase", ObjFile.WindowsHeader.ImageBase.ToString("X16"));
							writer.WriteElementString("sectionAlignment", ObjFile.WindowsHeader.SectionAlignment.ToString("X8"));
							writer.WriteElementString("fileAlignment", ObjFile.WindowsHeader.FileAlignment.ToString("X8"));
							writer.WriteElementString("operatingSystemMajorVersion", ObjFile.WindowsHeader.OperatingSystemMajorVersion.ToString("X4"));
							writer.WriteElementString("operatingSystemMinorVersion", ObjFile.WindowsHeader.OperatingSystemMinorVersion.ToString("X4"));
							writer.WriteElementString("imageMajorVersion", ObjFile.WindowsHeader.ImageMajorVersion.ToString("X4"));
							writer.WriteElementString("imageMinorVersion", ObjFile.WindowsHeader.ImageMinorVersion.ToString("X4"));
							writer.WriteElementString("subsystemMajorVersion", ObjFile.WindowsHeader.SubsystemMajorVersion.ToString("X4"));
							writer.WriteElementString("subsystemMinorVersion", ObjFile.WindowsHeader.SubsystemMinorVersion.ToString("X4"));
							writer.WriteElementString("windowsVersion", ObjFile.WindowsHeader.WindowsVersion.ToString("X8"));
							writer.WriteElementString("imageSize", ObjFile.WindowsHeader.ImageSize.ToString("X8"));
							writer.WriteElementString("headerSize", ObjFile.WindowsHeader.HeaderSize.ToString("X8"));
							writer.WriteElementString("checksum", ObjFile.WindowsHeader.Checksum.ToString("X8"));
							writer.WriteElementString("subsystem", ObjFile.WindowsHeader.Subsystem.ToString("X4"));
							writer.WriteElementString("dllCharacteristics", ObjFile.WindowsHeader.DllCharacteristics.ToString("X4"));
							writer.WriteElementString("stackReserveSize", ObjFile.WindowsHeader.StackReserveSize.ToString("X16"));
							writer.WriteElementString("stackCommitSize", ObjFile.WindowsHeader.StackCommitSize.ToString("X16"));
							writer.WriteElementString("heapReserveSize", ObjFile.WindowsHeader.HeapReserveSize.ToString("X16"));
							writer.WriteElementString("heapCommitSize", ObjFile.WindowsHeader.HeapCommitSize.ToString("X16"));
							writer.WriteElementString("loaderFlags", ObjFile.WindowsHeader.LoaderFlags.ToString("X8"));
							writer.WriteElementString("rvaCountSize", ObjFile.WindowsHeader.RvaCountSize.ToString("X8"));
						}

						writer.WriteEndElement();

						writer.WriteStartElement("tableDirectory");

						writer.WriteStartElement("exportTable");
						writer.WriteElementString("address", ObjFile.TableDirectory.ExportTable.ToString("X8"));
						writer.WriteElementString("length", ObjFile.TableDirectory.ExportTableSize.ToString("X8"));
						writer.WriteEndElement();

						writer.WriteStartElement("importTable");
						writer.WriteElementString("address", ObjFile.TableDirectory.ImportTable.ToString("X8"));
						writer.WriteElementString("length", ObjFile.TableDirectory.ImportTableSize.ToString("X8"));
						writer.WriteEndElement();

						writer.WriteStartElement("resourceTable");
						writer.WriteElementString("address", ObjFile.TableDirectory.ResourceTable.ToString("X8"));
						writer.WriteElementString("length", ObjFile.TableDirectory.ResourceTableSize.ToString("X8"));
						writer.WriteEndElement();

						writer.WriteStartElement("exceptionTable");
						writer.WriteElementString("address", ObjFile.TableDirectory.ExceptionTable.ToString("X8"));
						writer.WriteElementString("length", ObjFile.TableDirectory.ExceptionTableSize.ToString("X8"));
						writer.WriteEndElement();

						writer.WriteStartElement("certificateTable");
						writer.WriteElementString("address", ObjFile.TableDirectory.CertificateTable.ToString("X8"));
						writer.WriteElementString("length", ObjFile.TableDirectory.CertificateTableSize.ToString("X8"));
						writer.WriteEndElement();

						writer.WriteStartElement("relocationTable");
						writer.WriteElementString("address", ObjFile.TableDirectory.RelocationTable.ToString("X8"));
						writer.WriteElementString("length", ObjFile.TableDirectory.RelocationTableSize.ToString("X8"));
						writer.WriteEndElement();

						writer.WriteStartElement("debugTable");
						writer.WriteElementString("address", ObjFile.TableDirectory.DebugTable.ToString("X8"));
						writer.WriteElementString("length", ObjFile.TableDirectory.DebugTableSize.ToString("X8"));
						writer.WriteEndElement();

						writer.WriteStartElement("architectureTable");
						writer.WriteElementString("address", ObjFile.TableDirectory.ArchitectureTable.ToString("X8"));
						writer.WriteElementString("length", ObjFile.TableDirectory.ArchitectureTableSize.ToString("X8"));
						writer.WriteEndElement();

						writer.WriteStartElement("globalPointer");
						writer.WriteElementString("address", ObjFile.TableDirectory.GlobalTable.ToString("X8"));
						writer.WriteElementString("length", ObjFile.TableDirectory.GlobalTableSize.ToString("X8"));
						writer.WriteEndElement();

						writer.WriteStartElement("threadLocalStorageTable");
						writer.WriteElementString("address", ObjFile.TableDirectory.ThreadLocalStorageTable.ToString("X8"));
						writer.WriteElementString("length", ObjFile.TableDirectory.ThreadLocalStorageTableSize.ToString("X8"));
						writer.WriteEndElement();

						writer.WriteStartElement("loadConfigurationTable");
						writer.WriteElementString("address", ObjFile.TableDirectory.LoadConfigurationTable.ToString("X8"));
						writer.WriteElementString("length", ObjFile.TableDirectory.LoadConfigurationTableSize.ToString("X8"));
						writer.WriteEndElement();

						writer.WriteStartElement("boundImportTable");
						writer.WriteElementString("address", ObjFile.TableDirectory.BoundImportTable.ToString("X8"));
						writer.WriteElementString("length", ObjFile.TableDirectory.BoundImportTableSize.ToString("X8"));
						writer.WriteEndElement();

						writer.WriteStartElement("importAddressTable");
						writer.WriteElementString("address", ObjFile.TableDirectory.ImportAddressTable.ToString("X8"));
						writer.WriteElementString("length", ObjFile.TableDirectory.ImportAddressTableSize.ToString("X8"));
						writer.WriteEndElement();

						writer.WriteStartElement("delayImportTable");
						writer.WriteElementString("address", ObjFile.TableDirectory.DelayImportTable.ToString("X8"));
						writer.WriteElementString("length", ObjFile.TableDirectory.DelayImportTableSize.ToString("X8"));
						writer.WriteEndElement();

						writer.WriteStartElement("clrTable");
						writer.WriteElementString("address", ObjFile.TableDirectory.ClrTable.ToString("X8"));
						writer.WriteElementString("length", ObjFile.TableDirectory.ClrTableSize.ToString("X8"));
						writer.WriteEndElement();

						writer.WriteElementString("reserved", Convert.ToHexString(ObjFile.TableDirectory.Reserved));

						writer.WriteEndElement();

						if (ObjFile.ExportTable != null)
						{
							writer.WriteStartElement("exportTable");

							writer.WriteElementString("flags", ObjFile.ExportTable.Flags.ToString("X8"));
							writer.WriteElementString("dateTimeStamp", ObjFile.ExportTable.Flags.ToString("X8"));
							writer.WriteElementString("versionMajor", ObjFile.ExportTable.VersionMajor.ToString("X4"));
							writer.WriteElementString("versionMinor", ObjFile.ExportTable.VersionMinor.ToString("X4"));
							writer.WriteElementString("nameAddress", ObjFile.ExportTable.NameAddress.ToString("X8"));
							writer.WriteElementString("ordinalBase", ObjFile.ExportTable.OrdinalBase.ToString("X8"));
							writer.WriteElementString("addressTableEntries", ObjFile.ExportTable.AddressTableEntries.ToString("X8"));
							writer.WriteElementString("namePointerCount", ObjFile.ExportTable.NamePointerCount.ToString("X8"));
							writer.WriteElementString("exportTableAddress", ObjFile.ExportTable.ExportTableAddress.ToString("X8"));
							writer.WriteElementString("namePointerTableAddress", ObjFile.ExportTable.NamePointerTableAddress.ToString("X8"));
							writer.WriteElementString("ordinalTableAddress", ObjFile.ExportTable.OrdinalTableAddress.ToString("X8"));
							writer.WriteElementString("name", ObjFile.ExportTable.Name);

							writer.WriteStartElement("exportAddressTable");

							for (var entry = 0; entry < ObjFile.ExportTable.ExportAddressTable.Length; entry++)
							{
								writer.WriteStartElement("export");
								writer.WriteAttributeString("type", ObjFile.ExportTable.ExportAddressTable[entry].Type.ToString());
								writer.WriteAttributeString("name", ObjFile.ExportTable.ExportAddressTable[entry].Name);
								writer.WriteString(ObjFile.ExportTable.ExportAddressTable[entry].Address.ToString("X8"));
								writer.WriteEndElement();
							}

							writer.WriteEndElement();

							writer.WriteStartElement("exportNameTable");

							for (var entry = 0; entry < ObjFile.ExportTable.ExportNamePointerTable.Length; entry++)
							{
								writer.WriteStartElement("name");
								writer.WriteAttributeString("ordinal", ObjFile.ExportTable.ExportOrdinalTable[entry].ToString());
								writer.WriteAttributeString("address", ObjFile.ExportTable.ExportNamePointerTable[entry].ToString("X8"));
								writer.WriteString(ObjFile.ExportTable.ExportNameTable[ObjFile.ExportTable.ExportNamePointerTable[entry]]);
								writer.WriteEndElement();
							}

							writer.WriteEndElement();

							writer.WriteEndElement();
						}

						writer.WriteStartElement("importTable");

						for(var import = 0; import < ObjFile.ImportTable.Length; import++)
						{
							writer.WriteStartElement("import");

							writer.WriteAttributeString("name", ObjFile.ImportTable[import].Name);

							writer.WriteElementString("lookupTableAddress", ObjFile.ImportTable[import].LookupTableAddress.ToString("X8"));
							writer.WriteElementString("dateTimeStamp", ObjFile.ImportTable[import].DateTimeStamp.ToString("X8"));
							writer.WriteElementString("forwardIndex", ObjFile.ImportTable[import].ForwardIndex.ToString());
							writer.WriteElementString("nameAddress", ObjFile.ImportTable[import].NameAddress.ToString("X8"));
							writer.WriteElementString("importTableAddress", ObjFile.ImportTable[import].ImportTableAddress.ToString("X8"));

							writer.WriteStartElement("importLookupTable");

							for (var lookup = 0; lookup < ObjFile.ImportTable[import].LookupTable.Length; lookup++)
							{
								writer.WriteStartElement("lookup");
								writer.WriteAttributeString("type", ObjFile.ImportTable[import].LookupTable[lookup].Type.ToString());

								if (ObjFile.ImportTable[import].LookupTable[lookup].Type == ObjFile.ImportLookupRecordType.Name)
								{
									writer.WriteAttributeString("value", ObjFile.ImportTable[import].LookupTable[lookup].Value.ToString("X8"));
									writer.WriteString(ObjFile.ImportTable[import].LookupTable[lookup].Name);
								}
								else
									writer.WriteAttributeString("value", ObjFile.ImportTable[import].LookupTable[lookup].Value.ToString());

								writer.WriteEndElement();
							}

							writer.WriteEndElement();

							writer.WriteEndElement();
						}

						writer.WriteEndElement();
					}
				}
			}

			writer.WriteEndElement();
			writer.WriteEndDocument();

			writer.Flush();
			writer.Close();
		}
	}
}