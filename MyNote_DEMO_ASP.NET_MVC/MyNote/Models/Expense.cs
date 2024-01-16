using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyNote.Models
{
	public class Expense
	{
		[Key]
		public int Id { get; set; }
		[DisplayName("Expenses Name")]
		[Required]
        public string ExpensesName { get; set; }
		[Required]
		[Range(1, int.MaxValue, ErrorMessage = "Amount must be greater than 0 !")]
		public int Amount { get; set; }

    }
}
