using Transaction.Database.Entities;

namespace Transaction.Models
{
    public class Transaction
    {
        public string Id { get; set; }

        public string? BeneficiaryName { get; set; }

        public DateTime Date { get; set; }

        public DirectionsEnum Direction { get; set; }

        public double Amount { get; set; }

        public string? Description { get; set; }

        public string Currency { get; set; }

        public int? MCC { get; set; }

        public TransactionKindsEnum Kind { get; set; }

        public string? Catcode { get; set; }
    }
}
