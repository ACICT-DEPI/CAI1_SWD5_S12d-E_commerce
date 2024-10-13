using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace E_Commerce___DEPI.Models
{
	public class User
	{
		public int Id { get; set; }
		public bool IsAdmin { get; set; }

		[DisplayName("First name")]
		[MaxLength(55), Required]
		public string Fname { get; set; }

		[DisplayName("Last name")]
		[Required, MaxLength(55)]
		public string Lname { get; set; }

		[Display(Name = "Full Name")]
		public string FullName
		{
			get
			{
				return Lname + " " + Fname;
			}
		}

		[Required, MaxLength(55)]
		public string Email { get; set; }

		[MaxLength(20)]
		public string Phone { get; set; }

		[MaxLength(20)]
		public string Password { get; set; }

		[ForeignKey("AddressId")]
		public int? AddressId { get; set; }

		public virtual Address? Address { get; set; }

	}
}
