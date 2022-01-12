using System;
using System.Collections.Generic;

namespace ObjToXml
{
	internal static class ObjFile
	{
		internal static class ExecutableHeader
		{
			internal static char[] Magic;
			internal static ushort LastPageSize;
			internal static ushort PageCount;
			internal static ushort RelocationCount;
			internal static ushort HeaderSize;
			internal static ushort MinimumParagraphCount;
			internal static ushort MaximumParagraphCount;
			internal static ushort StackSegment;
			internal static ushort StackPointer;
			internal static ushort Checksum;
			internal static ushort InstructionPointer;
			internal static ushort CodeSegment;
			internal static ushort RelocationAddress;
			internal static ushort OverlayNumber;
			internal static byte[] Reserved;
			internal static ushort OemID;
			internal static ushort OemInformation;
			internal static byte[] Reserved2;
			internal static uint NextHeaderAddress;
		}

		internal static byte[] MsDosStub;

		internal static char[] PortableExecutableSignature;

		internal static class ObjectHeader
		{
			internal static ushort Machine;
			internal static ushort SectionCount;
			internal static uint DateTimeStamp;
			internal static uint SymbolTablePointer;
			internal static uint SymbolTableNumber;
			internal static ushort OptionalHeaderSize;
			internal static ushort Characteristics;
		}

		internal static class ObjectOptionalHeader
		{
			internal static ushort Magic;
			internal static byte LinkerVersionMajor;
			internal static byte LinkerVersionMinor;
			internal static uint CodeSize;
			internal static uint InitializedDataSize;
			internal static uint UninitializedDataSize;
			internal static uint EntryPointAddress;
			internal static uint CodeBase;
			internal static uint DataBase;
		}

		internal static class WindowsHeader
		{
			internal static ulong ImageBase;
			internal static uint SectionAlignment;
			internal static uint FileAlignment;
			internal static ushort OperatingSystemMajorVersion;
			internal static ushort OperatingSystemMinorVersion;
			internal static ushort ImageMajorVersion;
			internal static ushort ImageMinorVersion;
			internal static ushort SubsystemMajorVersion;
			internal static ushort SubsystemMinorVersion;
			internal static uint WindowsVersion;
			internal static uint ImageSize;
			internal static uint HeaderSize;
			internal static uint Checksum;
			internal static ushort Subsystem;
			internal static ushort DllCharacteristics;
			internal static ulong StackReserveSize;
			internal static ulong StackCommitSize;
			internal static ulong HeapReserveSize;
			internal static ulong HeapCommitSize;
			internal static uint LoaderFlags;
			internal static uint RvaCountSize;
		}

		internal static class TableDirectory
		{
			internal static uint ExportTable;
			internal static uint ExportTableSize;
			internal static uint ImportTable;
			internal static uint ImportTableSize;
			internal static uint ResourceTable;
			internal static uint ResourceTableSize;
			internal static uint ExceptionTable;
			internal static uint ExceptionTableSize;
			internal static uint CertificateTable;
			internal static uint CertificateTableSize;
			internal static uint RelocationTable;
			internal static uint RelocationTableSize;
			internal static uint DebugTable;
			internal static uint DebugTableSize;
			internal static uint ArchitectureTable;
			internal static uint ArchitectureTableSize;
			internal static uint GlobalTable;
			internal static uint GlobalTableSize;
			internal static uint ThreadLocalStorageTable;
			internal static uint ThreadLocalStorageTableSize;
			internal static uint LoadConfigurationTable;
			internal static uint LoadConfigurationTableSize;
			internal static uint BoundImportTable;
			internal static uint BoundImportTableSize;
			internal static uint ImportAddressTable;
			internal static uint ImportAddressTableSize;
			internal static uint DelayImportTable;
			internal static uint DelayImportTableSize;
			internal static uint ClrTable;
			internal static uint ClrTableSize;
			internal static byte[] Reserved;
		}

		internal static Section[] Sections;

		internal static ExportTableRecord ExportTable;
		internal static ImportTableRecord[] ImportTable;
		internal static ResourceDirectoryRecord ResourceDirectory;

		internal static void Read()
		{
			using (var stream = new System.IO.MemoryStream(File.Data))
			using (var reader = new System.IO.BinaryReader(stream))
			{
				if (File.Data[0] == (byte)'M' &&
					File.Data[1] == (byte)'Z')
				{
					ExecutableHeader.Magic = reader.ReadChars(2);
					ExecutableHeader.LastPageSize = reader.ReadUInt16();
					ExecutableHeader.PageCount = reader.ReadUInt16();
					ExecutableHeader.RelocationCount = reader.ReadUInt16();
					ExecutableHeader.HeaderSize = reader.ReadUInt16();
					ExecutableHeader.MinimumParagraphCount = reader.ReadUInt16();
					ExecutableHeader.MaximumParagraphCount = reader.ReadUInt16();
					ExecutableHeader.StackSegment = reader.ReadUInt16();
					ExecutableHeader.StackPointer = reader.ReadUInt16();
					ExecutableHeader.Checksum = reader.ReadUInt16();
					ExecutableHeader.InstructionPointer = reader.ReadUInt16();
					ExecutableHeader.CodeSegment = reader.ReadUInt16();
					ExecutableHeader.RelocationAddress = reader.ReadUInt16();
					ExecutableHeader.OverlayNumber = reader.ReadUInt16();
					ExecutableHeader.Reserved = reader.ReadBytes(8);
					ExecutableHeader.OemID = reader.ReadUInt16();
					ExecutableHeader.OemInformation = reader.ReadUInt16();
					ExecutableHeader.Reserved2 = reader.ReadBytes(20);
					ExecutableHeader.NextHeaderAddress = reader.ReadUInt32();

					MsDosStub = reader.ReadBytes((int)ExecutableHeader.NextHeaderAddress - (int)stream.Position);

					PortableExecutableSignature = reader.ReadChars(4);
				}

				ObjectHeader.Machine = reader.ReadUInt16();
				ObjectHeader.SectionCount = reader.ReadUInt16();
				ObjectHeader.DateTimeStamp = reader.ReadUInt32();
				ObjectHeader.SymbolTablePointer = reader.ReadUInt32();
				ObjectHeader.SymbolTableNumber = reader.ReadUInt32();
				ObjectHeader.OptionalHeaderSize = reader.ReadUInt16();
				ObjectHeader.Characteristics = reader.ReadUInt16();

				ObjectOptionalHeader.Magic = reader.ReadUInt16();
				ObjectOptionalHeader.LinkerVersionMajor = reader.ReadByte();
				ObjectOptionalHeader.LinkerVersionMinor = reader.ReadByte();
				ObjectOptionalHeader.CodeSize = reader.ReadUInt32();
				ObjectOptionalHeader.InitializedDataSize = reader.ReadUInt32();
				ObjectOptionalHeader.UninitializedDataSize = reader.ReadUInt32();
				ObjectOptionalHeader.EntryPointAddress = reader.ReadUInt32();
				ObjectOptionalHeader.CodeBase = reader.ReadUInt32();

				if (ObjectOptionalHeader.Magic != 0x020B)
				{
					// 32-Bit Header
					ObjectOptionalHeader.DataBase = reader.ReadUInt32();

					WindowsHeader.ImageBase = reader.ReadUInt32();
					WindowsHeader.SectionAlignment = reader.ReadUInt32();
					WindowsHeader.FileAlignment = reader.ReadUInt32();
					WindowsHeader.OperatingSystemMajorVersion = reader.ReadUInt16();
					WindowsHeader.OperatingSystemMinorVersion = reader.ReadUInt16();
					WindowsHeader.ImageMajorVersion = reader.ReadUInt16();
					WindowsHeader.ImageMinorVersion = reader.ReadUInt16();
					WindowsHeader.SubsystemMajorVersion = reader.ReadUInt16();
					WindowsHeader.SubsystemMinorVersion = reader.ReadUInt16();
					WindowsHeader.WindowsVersion = reader.ReadUInt32();
					WindowsHeader.ImageSize = reader.ReadUInt32();
					WindowsHeader.HeaderSize = reader.ReadUInt32();
					WindowsHeader.Checksum = reader.ReadUInt32();
					WindowsHeader.Subsystem = reader.ReadUInt16();
					WindowsHeader.DllCharacteristics = reader.ReadUInt16();
					WindowsHeader.StackReserveSize = reader.ReadUInt32();
					WindowsHeader.StackCommitSize = reader.ReadUInt32();
					WindowsHeader.HeapReserveSize = reader.ReadUInt32();
					WindowsHeader.HeapCommitSize = reader.ReadUInt32();
					WindowsHeader.LoaderFlags = reader.ReadUInt32();
					WindowsHeader.RvaCountSize = reader.ReadUInt32();
				}
				else
				{
					// 64-Bit Header
					ObjectOptionalHeader.DataBase = 0;

					WindowsHeader.ImageBase = reader.ReadUInt64();
					WindowsHeader.SectionAlignment = reader.ReadUInt32();
					WindowsHeader.FileAlignment = reader.ReadUInt32();
					WindowsHeader.OperatingSystemMajorVersion = reader.ReadUInt16();
					WindowsHeader.OperatingSystemMinorVersion = reader.ReadUInt16();
					WindowsHeader.ImageMajorVersion = reader.ReadUInt16();
					WindowsHeader.ImageMinorVersion = reader.ReadUInt16();
					WindowsHeader.SubsystemMajorVersion = reader.ReadUInt16();
					WindowsHeader.SubsystemMinorVersion = reader.ReadUInt16();
					WindowsHeader.WindowsVersion = reader.ReadUInt32();
					WindowsHeader.ImageSize = reader.ReadUInt32();
					WindowsHeader.HeaderSize = reader.ReadUInt32();
					WindowsHeader.Checksum = reader.ReadUInt32();
					WindowsHeader.Subsystem = reader.ReadUInt16();
					WindowsHeader.DllCharacteristics = reader.ReadUInt16();
					WindowsHeader.StackReserveSize = reader.ReadUInt64();
					WindowsHeader.StackCommitSize = reader.ReadUInt64();
					WindowsHeader.HeapReserveSize = reader.ReadUInt64();
					WindowsHeader.HeapCommitSize = reader.ReadUInt64();
					WindowsHeader.LoaderFlags = reader.ReadUInt32();
					WindowsHeader.RvaCountSize = reader.ReadUInt32();
				}

				TableDirectory.ExportTable = reader.ReadUInt32();
				TableDirectory.ExportTableSize = reader.ReadUInt32();

				TableDirectory.ImportTable = reader.ReadUInt32();
				TableDirectory.ImportTableSize = reader.ReadUInt32();

				TableDirectory.ResourceTable = reader.ReadUInt32();
				TableDirectory.ResourceTableSize = reader.ReadUInt32();

				TableDirectory.ExceptionTable = reader.ReadUInt32();
				TableDirectory.ExceptionTableSize = reader.ReadUInt32();

				TableDirectory.CertificateTable = reader.ReadUInt32();         // File Offset
				TableDirectory.CertificateTableSize = reader.ReadUInt32();

				TableDirectory.RelocationTable = reader.ReadUInt32();
				TableDirectory.RelocationTableSize = reader.ReadUInt32();

				TableDirectory.DebugTable = reader.ReadUInt32();
				TableDirectory.DebugTableSize = reader.ReadUInt32();

				TableDirectory.ArchitectureTable = reader.ReadUInt32();
				TableDirectory.ArchitectureTableSize = reader.ReadUInt32();

				TableDirectory.GlobalTable = reader.ReadUInt32();
				TableDirectory.GlobalTableSize = reader.ReadUInt32();

				TableDirectory.ThreadLocalStorageTable = reader.ReadUInt32();
				TableDirectory.ThreadLocalStorageTableSize = reader.ReadUInt32();

				TableDirectory.LoadConfigurationTable = reader.ReadUInt32();
				TableDirectory.LoadConfigurationTableSize = reader.ReadUInt32();

				TableDirectory.BoundImportTable = reader.ReadUInt32();
				TableDirectory.BoundImportTableSize = reader.ReadUInt32();

				TableDirectory.ImportAddressTable = reader.ReadUInt32();
				TableDirectory.ImportAddressTableSize = reader.ReadUInt32();

				TableDirectory.DelayImportTable = reader.ReadUInt32();
				TableDirectory.DelayImportTableSize = reader.ReadUInt32();

				TableDirectory.ClrTable = reader.ReadUInt32();
				TableDirectory.ClrTableSize = reader.ReadUInt32();

				TableDirectory.Reserved = reader.ReadBytes(8);

				Sections = new Section[ObjectHeader.SectionCount];

				for (var section = 0; section < ObjectHeader.SectionCount; section++)
				{
					Sections[section].SectionName = reader.ReadChars(8);
					Sections[section].VirtualSize = reader.ReadUInt32();
					Sections[section].VirtualAddress = reader.ReadUInt32();
					Sections[section].RawDataSize = reader.ReadUInt32();
					Sections[section].RawDataPointer = reader.ReadUInt32();
					Sections[section].RelocationPointer = reader.ReadUInt32();
					Sections[section].LineNumberPointer = reader.ReadUInt32();
					Sections[section].RelocationCount = reader.ReadUInt16();
					Sections[section].LineNumberCount = reader.ReadUInt16();
					Sections[section].SectionCharacteristics = reader.ReadUInt32();
				}

				for (var section = 0; section < Sections.Length; section++)
				{
					stream.Position = Sections[section].RawDataPointer;
					Sections[section].Data = reader.ReadBytes((int)Sections[section].RawDataSize);
				}
			}

			// Export Table
			if (TableDirectory.ExportTableSize == 0)
			{
				ExportTable = null;
			}
			else
			{
				ExportTable = new ExportTableRecord();

				for (var section = 0; section < Sections.Length; section++)
				{
					if (Sections[section].VirtualAddress <= TableDirectory.ExportTable &&
						Sections[section].VirtualAddress + Sections[section].VirtualSize > TableDirectory.ExportTable)
					{
						using (var stream = new System.IO.MemoryStream(Sections[section].Data))
						using (var reader = new System.IO.BinaryReader(stream))
						{
							stream.Position = TableDirectory.ExportTable - Sections[section].VirtualAddress;

							ExportTable.Flags = reader.ReadUInt32();
							ExportTable.DateTimeStamp = reader.ReadUInt32();
							ExportTable.VersionMajor = reader.ReadUInt16();
							ExportTable.VersionMinor = reader.ReadUInt16();
							ExportTable.NameAddress = reader.ReadUInt32();
							ExportTable.OrdinalBase = reader.ReadUInt32();
							ExportTable.AddressTableEntries = reader.ReadUInt32();
							ExportTable.NamePointerCount = reader.ReadUInt32();
							ExportTable.ExportTableAddress = reader.ReadUInt32();
							ExportTable.NamePointerTableAddress = reader.ReadUInt32();
							ExportTable.OrdinalTableAddress = reader.ReadUInt32();

							stream.Position = ExportTable.NameAddress - Sections[section].VirtualAddress;

							ExportTable.Name = reader.ReadNullTerminatedString();

							ExportTable.ExportAddressTable = new ExportAddressRecord[ExportTable.AddressTableEntries];

							stream.Position = ExportTable.ExportTableAddress - Sections[section].VirtualAddress;

							for (var entry = 0; entry < ExportTable.AddressTableEntries; entry++)
							{
								ExportTable.ExportAddressTable[entry].Address = reader.ReadUInt32();

								if (ExportTable.ExportAddressTable[entry].Address >= Sections[section].VirtualAddress &&
									ExportTable.ExportAddressTable[entry].Address < Sections[section].VirtualAddress + Sections[section].VirtualSize)
									ExportTable.ExportAddressTable[entry].Type = ExportAddressType.Forward;
								else
									ExportTable.ExportAddressTable[entry].Type = ExportAddressType.Symbol;
							}

							ExportTable.ExportNamePointerTable = new uint[ExportTable.NamePointerCount];

							stream.Position = ExportTable.NamePointerTableAddress - Sections[section].VirtualAddress;

							for (var entry = 0; entry < ExportTable.NamePointerCount; entry++)
								ExportTable.ExportNamePointerTable[entry] = reader.ReadUInt32();

							ExportTable.ExportOrdinalTable = new ushort[ExportTable.NamePointerCount];

							stream.Position = ExportTable.OrdinalTableAddress - Sections[section].VirtualAddress;

							for (var entry = 0; entry < ExportTable.ExportOrdinalTable.Length; entry++)
								ExportTable.ExportOrdinalTable[entry] = reader.ReadUInt16();

							ExportTable.ExportNameTable = new Dictionary<ulong, string>();

							for (var entry = 0; entry < ExportTable.NamePointerCount; entry++)
							{
								stream.Position = ExportTable.ExportNamePointerTable[entry] - Sections[section].VirtualAddress;

								ExportTable.ExportNameTable[ExportTable.ExportNamePointerTable[entry]] = reader.ReadNullTerminatedString();
							}

							for (var entry = 0; entry < ExportTable.ExportOrdinalTable.Length; entry++)
							{
								if (ExportTable.ExportAddressTable[ExportTable.ExportOrdinalTable[entry]].Type == ExportAddressType.Forward)
								{
									stream.Position = ExportTable.ExportNamePointerTable[ExportTable.ExportOrdinalTable[entry]] - Sections[section].VirtualAddress;

									ExportTable.ExportAddressTable[ExportTable.ExportOrdinalTable[entry]].Name = reader.ReadNullTerminatedString();
								}
								else
								{
									ExportTable.ExportAddressTable[ExportTable.ExportOrdinalTable[entry]].Name = ExportTable.ExportNameTable[ExportTable.ExportNamePointerTable[entry]];
								}
							}
						}
					}
				}
			}

			// Import Table
			if (TableDirectory.ImportTableSize == 0)
			{
				ImportTable = null;
			}
			else
			{
				var importTable = new List<ImportTableRecord>();

				for (var section = 0; section < Sections.Length; section++)
				{
					if (Sections[section].VirtualAddress <= TableDirectory.ImportTable &&
						Sections[section].VirtualAddress + Sections[section].VirtualSize > TableDirectory.ImportTable)
					{
						using (var stream = new System.IO.MemoryStream(Sections[section].Data))
						using (var reader = new System.IO.BinaryReader(stream))
						{
							stream.Position = TableDirectory.ImportTable - Sections[section].VirtualAddress;

							while (true)
							{
								var record = new ImportTableRecord();

								record.LookupTableAddress = reader.ReadUInt32();
								record.DateTimeStamp = reader.ReadUInt32();
								record.ForwardIndex = reader.ReadUInt32();
								record.NameAddress = reader.ReadUInt32();
								record.ImportTableAddress = reader.ReadUInt32();

								if ((record.LookupTableAddress |
									record.DateTimeStamp |
									record.ForwardIndex |
									record.NameAddress |
									record.ImportTableAddress) == 0)
									break;

								importTable.Add(record);
							}

							ImportTable = importTable.ToArray();

							for (var entry = 0; entry < ImportTable.Length; entry++)
							{
								var lookupTable = new List<ImportLookupRecord>();

								stream.Position = ImportTable[entry].LookupTableAddress - Sections[section].VirtualAddress;

								while (true)
								{
									var record = new ImportLookupRecord();

									if (ObjectOptionalHeader.Magic != 0x020B)
									{
										// 32-Bit Entry
										var value = reader.ReadUInt32();

										if (value == 0)
											break;

										if ((value & 0x80000000U) == 0)
											record.Type = ImportLookupRecordType.Name;
										else
											record.Type = ImportLookupRecordType.Ordinal;

										record.Value = value & 0x7FFFFFFFU;

										lookupTable.Add(record);
									}
									else
									{
										// 64-Bit Entry
										var value = reader.ReadUInt64();

										if (value == 0)
											break;

										if ((value & 0x8000000000000000UL) == 0UL)
											record.Type = ImportLookupRecordType.Name;
										else
											record.Type = ImportLookupRecordType.Ordinal;

										record.Value = (uint)(value & 0xFFFFFFFFUL);

										lookupTable.Add(record);
									}
								}

								ImportTable[entry].LookupTable = lookupTable.ToArray();
							}

							// DLL Names
							for (var entry = 0; entry < ImportTable.Length; entry++)
							{
								stream.Position = ImportTable[entry].NameAddress - Sections[section].VirtualAddress;

								ImportTable[entry].Name = reader.ReadNullTerminatedString();

								for (var lookup = 0; lookup < ImportTable[entry].LookupTable.Length; lookup++)
								{
									if (ImportTable[entry].LookupTable[lookup].Type == ImportLookupRecordType.Name)
									{
										stream.Position = (uint)ImportTable[entry].LookupTable[lookup].Value - Sections[section].VirtualAddress;

										var hint = reader.ReadUInt16();
										ImportTable[entry].LookupTable[lookup].Name = reader.ReadNullTerminatedString();
									}
								}
							}
						}
					}
				}
			}

			// Resource Table
			if (TableDirectory.ResourceTableSize == 0)
			{
				ResourceDirectory = null;
			}
			else
			{
				ResourceDirectory = new ResourceDirectoryRecord();

				for (var section = 0; section < Sections.Length; section++)
				{
					if (Sections[section].VirtualAddress <= TableDirectory.ResourceTable &&
						Sections[section].VirtualAddress + Sections[section].VirtualSize > TableDirectory.ResourceTable)
					{
						using (var stream = new System.IO.MemoryStream(Sections[section].Data))
						using (var reader = new System.IO.BinaryReader(stream))
						{
							stream.Position = TableDirectory.ResourceTable - Sections[section].VirtualAddress;

							ResourceDirectory.Characteristics = reader.ReadUInt32();
							ResourceDirectory.DateTimeStamp = reader.ReadUInt32();
							ResourceDirectory.VersionMajor = reader.ReadUInt16();
							ResourceDirectory.VersionMinor = reader.ReadUInt16();
							ResourceDirectory.NameCount = reader.ReadUInt16();
							ResourceDirectory.IdCount = reader.ReadUInt16();

							// Entries
							ResourceDirectory.Entries = new ResourceDirectoryEntry[ResourceDirectory.NameCount + ResourceDirectory.IdCount];

							var directories = new Queue<ResourceDirectoryEntry>();
							var resources = new Queue<ResourceDirectoryEntry>();

							for (var entry = 0; entry < ResourceDirectory.Entries.Length; entry++)
							{
								ResourceDirectory.Entries[entry] = new ResourceDirectoryEntry();

								var value = reader.ReadUInt32();
								var offset = reader.ReadUInt32();

								if ((value & 0x80000000U) == 0U)
								{
									ResourceDirectory.Entries[entry].IdentifierType = ResourceDirectoryIdentifierType.Id;
									ResourceDirectory.Entries[entry].Id = value;
								}
								else
								{
									ResourceDirectory.Entries[entry].IdentifierType = ResourceDirectoryIdentifierType.Name;
									ResourceDirectory.Entries[entry].NameOffset = value & 0x7FFFFFFFU;
								}

								if((offset & 0x80000000U) == 0U)
								{
									ResourceDirectory.Entries[entry].EntryType = ResourceDirectoryEntryType.Resource;
									ResourceDirectory.Entries[entry].ValueOffset = offset;

									resources.Enqueue(ResourceDirectory.Entries[entry]);
								}
								else
								{
									ResourceDirectory.Entries[entry].EntryType = ResourceDirectoryEntryType.Directory;
									ResourceDirectory.Entries[entry].ValueOffset = offset & 0x7FFFFFFFU;

									directories.Enqueue(ResourceDirectory.Entries[entry]);
								}
							}

							while (directories.Count != 0)
							{
								var directory = directories.Dequeue();

								stream.Position = directory.ValueOffset;

								directory.Directory = new ResourceDirectoryRecord();

								directory.Directory.Characteristics = reader.ReadUInt32();
								directory.Directory.DateTimeStamp = reader.ReadUInt32();
								directory.Directory.VersionMajor = reader.ReadUInt16();
								directory.Directory.VersionMinor = reader.ReadUInt16();
								directory.Directory.NameCount = reader.ReadUInt16();
								directory.Directory.IdCount = reader.ReadUInt16();

								// Entries
								directory.Directory.Entries = new ResourceDirectoryEntry[directory.Directory.NameCount + directory.Directory.IdCount];

								for (var entry = 0; entry < directory.Directory.Entries.Length; entry++)
								{
									directory.Directory.Entries[entry] = new ResourceDirectoryEntry();

									var value = reader.ReadUInt32();
									var offset = reader.ReadUInt32();

									if ((value & 0x80000000U) == 0U)
									{
										directory.Directory.Entries[entry].IdentifierType = ResourceDirectoryIdentifierType.Id;
										directory.Directory.Entries[entry].Id = value;
									}
									else
									{
										directory.Directory.Entries[entry].IdentifierType = ResourceDirectoryIdentifierType.Name;
										directory.Directory.Entries[entry].NameOffset = value & 0x7FFFFFFFU;
									}

									if ((offset & 0x80000000U) == 0U)
									{
										directory.Directory.Entries[entry].EntryType = ResourceDirectoryEntryType.Resource;
										directory.Directory.Entries[entry].ValueOffset = offset;
									
										resources.Enqueue(directory.Directory.Entries[entry]);
									}
									else
									{
										directory.Directory.Entries[entry].EntryType = ResourceDirectoryEntryType.Directory;
										directory.Directory.Entries[entry].ValueOffset = offset & 0x7FFFFFFFU;

										directories.Enqueue(directory.Directory.Entries[entry]);
									}
								}
							}

							while (resources.Count != 0)
							{
								var resource = resources.Dequeue();

								stream.Position = resource.ValueOffset;

								resource.Resource = new ResourceRecord
								{
									Address = reader.ReadUInt32(),
									Size = reader.ReadUInt32(),
									Codepage = reader.ReadUInt32()
								};
							}
						}
					}
				}
			}
		}

		internal struct Section
		{
			internal char[] SectionName;
			internal uint VirtualSize;
			internal uint VirtualAddress;
			internal uint RawDataSize;
			internal uint RawDataPointer;
			internal uint RelocationPointer;
			internal uint LineNumberPointer;
			internal ushort RelocationCount;
			internal ushort LineNumberCount;
			internal uint SectionCharacteristics;
			internal byte[] Data;
		}

		internal class ExportTableRecord
		{
			internal uint Flags;
			internal uint DateTimeStamp;
			internal ushort VersionMajor;
			internal ushort VersionMinor;
			internal uint NameAddress;
			internal uint OrdinalBase;
			internal uint AddressTableEntries;
			internal uint NamePointerCount;
			internal uint ExportTableAddress;
			internal uint NamePointerTableAddress;
			internal uint OrdinalTableAddress;

			internal string Name;
			internal ExportAddressRecord[] ExportAddressTable;
			internal uint[] ExportNamePointerTable;
			internal ushort[] ExportOrdinalTable;
			internal Dictionary<ulong, string> ExportNameTable;
		}

		internal struct ExportAddressRecord
		{
			internal uint Address;
			internal ExportAddressType Type;
			internal string Name;
		}

		internal enum ExportAddressType
		{
			Symbol,
			Forward
		}

		internal struct ImportTableRecord
		{
			internal uint LookupTableAddress;
			internal uint DateTimeStamp;
			internal uint ForwardIndex;
			internal uint NameAddress;
			internal uint ImportTableAddress;

			internal string Name;
			internal ImportLookupRecord[] LookupTable;
		}

		internal struct ImportLookupRecord
		{
			internal ImportLookupRecordType Type;
			internal uint Value;
			internal string Name;
		}

		internal enum ImportLookupRecordType
		{
			Name,
			Ordinal
		}

		internal class ResourceDirectoryRecord
		{
			internal uint Characteristics;
			internal uint DateTimeStamp;
			internal ushort VersionMajor;
			internal ushort VersionMinor;
			internal ushort NameCount;
			internal ushort IdCount;
			internal ResourceDirectoryEntry[] Entries;
		}

		internal class ResourceDirectoryEntry
		{
			internal uint NameOffset;
			internal uint Id;
			internal uint ValueOffset;
			internal ResourceDirectoryIdentifierType IdentifierType;
			internal ResourceDirectoryEntryType EntryType;
			internal ResourceDirectoryRecord Directory;
			internal ResourceRecord Resource;
		}

		internal class ResourceRecord
		{
			internal uint Address;
			internal uint Size;
			internal uint Codepage;
		}

		internal enum ResourceDirectoryIdentifierType
		{
			Name,
			Id
		}

		internal enum ResourceDirectoryEntryType
		{
			Resource,
			Directory
		}
	}
}