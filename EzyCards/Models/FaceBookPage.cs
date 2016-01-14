using System;
using System.ComponentModel.DataAnnotations;
namespace EzyCards.Models
{
	public class FaceBookPage
	{
		[ScaffoldColumn(false)]
		public Int64 Id { get; set; }

		[Required, StringLength(100), Display(Name = "FaceBookPageId")]
		public string FaceBookPageId { get; set; }

		[StringLength(100), Display(Name = "ProductName")]
		public string PgaeName { get; set; }
	}
}
