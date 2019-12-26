using System;
using System.IO;

namespace Test.Support.InputOutput.Storage
{
	static internal class FileStreamHelper
	{
		static internal string FreeFilePath
		{
			get
			{
				DriveInfo[] drives = DriveInfo.GetDrives();
				for (int i = drives.Length - 1; i >= 0x0; i--)
				{
					DriveInfo drive = drives[i];
					if (!drive.IsReady)
						continue;
					for (uint j = 0x0; j < uint.MaxValue; j++)
					{
						string filePath = Path.Combine(drive.Name, j.ToString());
						if (!File.Exists(filePath))
							return filePath;
					}
				}
				throw new InvalidOperationException();
			}
		}

		static internal void Delete(string path)
		{
			if (File.Exists(path))
				File.Delete(path);
		}
	}
}
