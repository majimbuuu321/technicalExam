using System.ComponentModel.DataAnnotations;

namespace TechnicalExamAPI.Models
{
    public class Account
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        [Required]
        [StringLength(12)]
        public string AccountNumber { get; set; }

        [AccountType(ErrorMessage = "Account Type must be Savings or Checking only.")]
        public string AccountType { get; set; }

        [Required]
        [StringLength(50)]
        public string BranchAddress { get; set; }

        [Required]
        [Range(typeof(decimal), "0", "1000000", ErrorMessage = "Initial Deposit is up to 1000000")]
        public decimal InitialDeposit { get; set; }
    }

    public class AccountTypeAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var accountType = value.ToString();

            if (accountType == "Savings" || accountType == "Checking")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
