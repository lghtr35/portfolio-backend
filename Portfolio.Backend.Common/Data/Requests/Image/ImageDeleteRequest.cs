using System;
using System.ComponentModel.DataAnnotations;

namespace Portfolio.Backend.Common.Data.Requests.Image
{
	public class ImageDeleteRequest
	{
		[Required, MinLength(1)]
		public int[] ImageIds { get; set; }
	}
}

