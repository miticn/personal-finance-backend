using Transaction.Models;

namespace Transaction.Database.Entities
{
    public class TransactionSplitEntity
    {
        public string TransactionId { get; set; }
        public string catcode { get; set; }
        public double amount { get; set; }
    }
}
