using System;
namespace Portfolio.Backend.Common.Exceptions
{
	public class OperationNotCompleteOnBlobServiceException : Exception
	{
		public OperationNotCompleteOnBlobServiceException() : base()
		{

		}

		public OperationNotCompleteOnBlobServiceException(string msg) : base(msg)
		{

		}
    }
}

