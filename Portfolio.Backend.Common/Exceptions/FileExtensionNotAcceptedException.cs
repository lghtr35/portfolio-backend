using System;
namespace Portfolio.Backend.Common.Exceptions
{
	public class FileExtensionNotAcceptedException : Exception
	{
		public FileExtensionNotAcceptedException() : base()
		{
		}

		public FileExtensionNotAcceptedException(string message) : base(message) { }
	}
}

